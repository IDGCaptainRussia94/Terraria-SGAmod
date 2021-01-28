using System.IO;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using System.Linq;
using Idglibrary;
using SGAmod;
using SubworldLibrary;
using ReLogic.Graphics;
using Terraria.Utilities;
using SGAmod.Effects;
using SGAmod.NPCs.Hellion;
using SGAmod.Items.Consumable;
using SGAmod.NPCs.Murk;
using SGAmod.Items;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Dimensions.NPCs;
using SGAmod.NPCs;

namespace SGAmod.Dimensions
{

        public class SGADimPlayer : ModPlayer
    {

        public int enterlimbo = 0;
        public bool noLight = false;
        public int noLightGrow = 0;
        public int lightSize = 3000;
        public int lightGrowRate = 5;
        public int heartBeat = 0;
        public static int staticHeartBeat = 0;
        public static float staticHeartRate = 0;
        public override void UpdateBiomeVisuals()
        {
            //TheProgrammer
            player.ManageSpecialBiomeVisuals("SGAmod:LimboSky", (SGAPocketDim.WhereAmI == typeof(LimboDim) || SGAPocketDim.WhereAmI == typeof(Limborinth)) ? true : false, player.Center);
        }

        public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (SGAmod.anysubworld)
            {
                if (SLWorld.currentSubworld is SGAPocketDim)
                    DimDingeonsWorld.deathtimer = Math.Max(1, DimDingeonsWorld.deathtimer);

            }
        }

        public override void ResetEffects()
        {
            int count = 0;
            if (SGAPocketDim.WhereAmI == typeof(LimboDim))
                count = (int)(player.CountItem(ModContent.ItemType<Entrophite>(), 1000) / 15f);
            heartBeat += count;

            if (heartBeat > 10000 && !player.dead)
            {
                SoundEffectInstance sound = Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Heartbeat").WithVolume(Math.Min(0.25f + (count / 250f), 1f)).WithPitchVariance(.05f), player.Center);
                if (sound != null)
                {
                    sound.Pitch = -0.75f + (float)Math.Min(Math.Atan(count / 25f), 1.7f);
                }
                NullWatcher.DoAwarenessChecks(600 + count*5, false, true, player.Center);
                heartBeat = 0;
                staticHeartBeat = 30;
                staticHeartRate = Math.Min(count/500f,0.04f);
            }
            if (!Main.dedServ && Main.LocalPlayer == player)
            {
                staticHeartBeat -= 1;
            }

            noLight = false;
            noLightGrow = Math.Max(noLightGrow - 1, 0);
            if (!(player.HasBuff(BuffID.Darkness) || player.HasBuff(BuffID.Blackout)))
            lightSize = Math.Min(lightSize + (noLightGrow > 0 ? 0 : lightGrowRate), 3000);// 2000+(int)(Math.Sin(Main.GlobalTime/2f)*1000);
            lightGrowRate = 5;
        }

        public override void PostUpdateRunSpeeds()
        {

            if (!noLight)
            SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(player.Center.X, player.Center.Y, lightSize)));
        }

        public override void PreUpdate()
        {

            SGAmod.anysubworld = SGAPocketDim.WhereAmI != null;
            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub)
                {
                    int limit = sub.LimitPlayers;
                    if (limit%16==0 && limit>0)
                    {
                        player.AddBuff(BuffID.NoBuilding,2);
                    }
                    player.GetModPlayer<SGAPlayer>().noModTeleport = true;
                }
                SLWorld.noReturn = false;
                if (SGAPocketDim.WhereAmI == typeof(LimboDim))
                    SLWorld.noReturn = true;
            }

            if (Main.netMode != NetmodeID.Server && !Main.dedServ && Main.LocalPlayer==player)
            {
                Projectile.NewProjectile(Main.screenPosition+new Vector2(Main.screenWidth,Main.screenHeight)/2, Vector2.Zero, mod.ProjectileType("DrawOverride"), 0, 0f);
            }

            enterlimbo += 1;
            if (SGAPocketDim.WhereAmI != null)
            {
                //Main.NewText(DimDingeonsWorld.texttimer);
                //if (DimDingeonsWorld.Instance!=null)
                //DimDingeonsWorld.Instance.PostUpdate();
                //Projectile.NewProjectile(new Vector2(player.Center.X, player.Center.Y), Vector2.Zero, mod.ProjectileType("TimerOverride"), 0, 0);
            }
            if (SGAPocketDim.WhereAmI == typeof(LimboDim))
            {
                player.AddBuff(Idglib.Instance.BuffType("LimboFading"), 1);
                if (enterlimbo > 0)
                {
                    enterlimbo = -6;
                }
                if (enterlimbo == -5)
                {
                    Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/crack"), new Vector2(-1, -1));
                    player.Center = new Vector2(Main.rand.Next(200,Main.maxTilesX-400)*16, 64);
                }
                if (enterlimbo > -2)
                {
                    enterlimbo = -2;
                    if (Framing.GetTileSafely((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f)).wall == mod.WallType("NullWall") && DimDingeonsWorld.ancientLockedFabric)
                    {
                        enterlimbo = -6;

                    }
                }


            }
        }

    }

    public class SGADimEnemies : GlobalNPC
    {

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub)
                {
                    spawnRate = (int)((float)spawnRate * sub.spawnRate);
                    maxSpawns = (int)((float)maxSpawns * sub.maxSpawns);
                }
            }
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {

            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub)
                {
                    sub.EnemySpawnsOverride(pool, spawnInfo, sub);
                }
            }
        }

        public override bool PreNPCLoot(NPC npc)
        {

            if (npc.type == NPCID.SkeletonArcher && SGAPocketDim.WhereAmI == typeof(DeeperDungeon))
            {
                NPCLoader.blockLoot.Add(ItemID.MagicQuiver);
                if (Main.rand.Next(50) == 0)
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("NormalQuiver"));
            }


            return true;
        }


    }

    public class Dimitems : GlobalItem
    {
        public override bool CanUseItem(Item item, Player player)
        {
            if (SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub)
                {
                    int limit = sub.LimitPlayers;
                    bool powertool = false;
                    if (!player.HeldItem.IsAir)
                    {
                        Projectile them = new Projectile();
                        them.SetDefaults(player.HeldItem.shoot);
                        if (them.aiStyle == 20)
                            powertool = true;
                    }

                        if (limit % 16 == 0 && limit > 0 && ((item.pick > 0 && !powertool) || item.hammer > 0 || item.axe > 0 || item.createTile > -1 || item.createWall > -1))
                    {
                        return false;
                    }
                }
            }
            return base.CanUseItem(item, player);
        }


    }

    public class DimDungeonTiles : GlobalTile
    {
        public override bool CanExplode(int i, int j, int type)
        {
            if (SLWorld.currentSubworld is SGAPocketDim sub && SGAPocketDim.WhereAmI!=null)
            {
                int dimmusic = sub.LimitPlayers;
                if (dimmusic > 0 && dimmusic % 16 == 0)
                {
                    //Main.NewText("test");
                    if (DeeperDungeon.instance.IsSpike(type,1))
                        return true;

                    return false;
                }
            }
            return base.CanExplode(i, j, type);
        }
        public override void KillTile(int i, int j, int type, ref bool fail, ref bool effectOnly, ref bool noItem)
        {

            if (SLWorld.currentSubworld is SGAPocketDim sub && SGAPocketDim.WhereAmI != null)
            {
                int dimmusic = sub.LimitPlayers;
                if (dimmusic > 0 && dimmusic % 16 == 0)
                {
                    if (DeeperDungeon.instance.IsSpike(type,1))
                    {
                        fail = false;
                        effectOnly = false;
                    }
                    else
                    {
                        fail = true;
                        effectOnly = true;
                    }

                }
            }

        }
    }

    public class DimDungeonWalls : GlobalWall
    {
        public override bool CanExplode(int i, int j, int type)
        {
            if (SLWorld.currentSubworld is SGAPocketDim sub)
            {
                int limiter = sub.LimitPlayers;
                if (limiter > 0 && limiter % 16 == 0)
                {
                    return false;

                }
            }
            return base.CanExplode(i, j, type);
        }

    }

    public class SGAPocketDim : Subworld
    {
        public override int width => 2400;
        public override int height => 3400;
        public override ModWorld modWorld => ModContent.GetInstance<DimDingeonsWorld>();
        public override bool saveSubworld => false;

        public int enemyseed = 0;

        public bool chooseenemies = false;

        public virtual int DimType => -1;

        public virtual float maxSpawns => 1f;
        public virtual float spawnRate => 1f;

        public Func<IDictionary<int, float>, NPCSpawnInfo, SGAPocketDim, int> EnemySpawnsOverride = delegate (IDictionary<int,float> pool, NPCSpawnInfo spawnInfo, SGAPocketDim pocket)
        {
            return 1;
        };

        public static Type WhereAmI
        {

            get
            {
                if (Main.netMode > 0)
                    return null;//This is null for clients on the overworld (normal), but not on the server, wtf? Whatever SP only content!
                if (!SLWorld.subworld || !Subworld.AnyActive(SGAmod.Instance))
                    return null;
                return SLWorld.currentSubworld.GetType();
            }

        }

        public int LimitPlayers = 0;

        public virtual string DimName => "NoName";
        public virtual int? Music
        {

            get
            {
                return null;
            }

        }
        public virtual bool IsSpike(int it,int type = 0)
        {
            bool match = (it == SGAmod.Instance.TileType("UnmanedBarTile") || it == SGAmod.Instance.TileType("NoviteBarTile") || it == SGAmod.Instance.TileType("BiomassBarTile"));
            return ((it == TileID.Spikes || it == TileID.WoodenSpikes) && type == 1) || it == TileID.Cobweb || it==TileID.BreakableIce || it == TileID.MagicalIceBlock || it == TileID.MetalBars || match;
        }

        public virtual bool IsDirt(int it)
        {
            return it == TileID.Dirt;
        }

        public override void Unload()
        {
            SGAmod.cachedata = true;
            //SubworldCache.AddCache("SGAmod", "SGAWorld", "downedSharkvern", SGAWorld.downedSharkvern,null);
            //SubworldCache.AddCache("SGAmod", "SGAWorld", "downedWraiths", null, SGAWorld.downedWraiths);
        }

        public static void EnterSubworld(string whereto,bool vote = false)
        {
            DimDungeonsProxy.DungeonSeeds = (int)(System.DateTime.Now.Millisecond * 1370.3943162338);
            //DimDungeons.DungeonSeeds = (int)((Main.time+SGAWorld.dungeonlevel) * (double)(Math.PI*17));
            SGAmod.cachedata = true;
            if (SGAPocketDim.WhereAmI != null)
            {
                SGAWorld.dungeonlevel += 1;
                if (SGAPocketDim.WhereAmI == typeof(LimboDim))
                {
                    SGAWorld.dungeonlevel = 0;
                    SLWorld.noReturn = true;
                }

            }
            Subworld.Enter(whereto, !vote);
        }

        public static void ExitSubworld(bool novote = false)
        {
            SLWorld.noReturn = false;
            Subworld.Exit(novote);
        }

        public static void UpdateMusic(ref int music, ref MusicPriority priority)
        {
            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub)
                {
                    int? dimmusic = sub.Music;
                    if (dimmusic != null)
                    {
                        //GetSoundSlot(SoundType.Music, dimmusic)
                        music = (int)dimmusic;
                        priority = MusicPriority.Environment;
                        return;
                    }
                }
            }
        }


        /*public virtual void genpass(GenerationProgress prog)
        {
            Main.spawnTileX = (Main.maxTilesX / 2) / 16;
            Main.spawnTileY = 0;
            prog.Message = "ello there!";
        }*/
        public override List<GenPass> tasks => new List<GenPass>()
        {
        new SubworldGenPass(2f, progress =>
        {
            progress.Message = "Loading Pre"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY +54; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY+64; //Hides the cavern layer way out of bounds
        }),
        };

        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 27000;
        }
    }

    public class DrawOverride : ModProjectile
    {
        static int fogeffect = 0;
        static int swaptargets = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Timer");
        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            drawCacheProjsOverWiresUI.Add(index);
        }

        public override string Texture
        {
            get { return "Terraria/Projectile_" + ProjectileID.ShadowBeamFriendly; }
        }

        public static void DrawFog()
        {

            DrawOverride.fogeffect += 1;
            bool isMurk = NPC.CountNPCS(ModContent.NPCType<Murk>()) > 0;

            if (DrawOverride.fogeffect < 120 || Main.dedServ)
                return;

            BlendState blind = new BlendState
            {

                ColorSourceBlend = Blend.Zero,
                ColorDestinationBlend = Blend.InverseSourceColor,

                AlphaSourceBlend = Blend.Zero,
                AlphaDestinationBlend = Blend.InverseSourceColor

            };


            if (!Main.dedServ)
            {
                int lightingtotal = Main.LocalPlayer.GetModPlayer<SGADimPlayer>().lightSize;

                Matrix Custommatrix = Matrix.CreateScale(Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);
                RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

                if (lightingtotal < 2600 && (!isMurk || (!SGAConfigClient.Instance.Murklite && isMurk)))
                {
                    int fogDetail = SGAConfigClient.Instance.FogDetail;
                    float fogAlpha = 0.05f * (30f / (float)fogDetail);

                    //Main.spriteBatch.End();
                    Main.graphics.GraphicsDevice.SetRenderTarget(SGAmod.drawnscreen);
                    Main.graphics.GraphicsDevice.Clear(Color.Black);

                    Texture2D pern = ModContent.GetTexture("SGAmod/Perlin");
                    Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);
                    Main.spriteBatch.Draw(Main.blackTileTexture, new Vector2(0, 0), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);

                    if (isMurk)
                    {
                        Main.spriteBatch.Draw(pern, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, Color.Green * 1f, Main.GlobalTime * 0.24f, new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(5f, 5f), SpriteEffects.None, 0f);
                        Main.spriteBatch.Draw(pern, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, Color.DarkOliveGreen * 1f, Main.GlobalTime * 0.14f, new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(5f, 5f), SpriteEffects.None, 0f);
                    }
                    else
                    {
                        Main.spriteBatch.Draw(pern, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, Main.hslToRgb((Main.GlobalTime / 3) % 1f, 1f, 0.75f) * 0.5f, Main.GlobalTime * 0.24f, new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(5f, 5f), SpriteEffects.None, 0f);
                        Main.spriteBatch.Draw(pern, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, Main.hslToRgb(((Main.GlobalTime + 1.5f) / 3) % 1f, 1f, 0.75f) * 0.5f, Main.GlobalTime * -0.24f, new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(5f, 5f), SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.End();


                    pern = ModContent.GetTexture("SGAmod/Extra_49");
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

                    foreach (PostDrawCollection postdraw in SGAmod.PostDraw)
                    {
                        Vector3 vecx = postdraw.light;
                        float size = vecx.Z / pern.Width;

                        for (int i = 0; i < 360; i += 360 / fogDetail)
                            Main.spriteBatch.Draw(pern, new Vector2(vecx.X, vecx.Y) - Main.screenPosition, null, Color.White * fogAlpha, MathHelper.ToRadians(i), new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(1.5f, 1.5f) * size, SpriteEffects.None, 0f);

                        /*for (int i = 0; i < Main.maxNPCs; i += 1)
                        {
                            if (Main.npc[i].active && Main.npc[i].townNPC)
                            {
                                for (int x = 0; x < 360; x += 360 / 30)
                                    Main.spriteBatch.Draw(pern, (Main.npc[i].Center) - Main.screenPosition, null, Color.White * 0.25f, MathHelper.ToRadians(x), new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(7f, 7f), SpriteEffects.None, 0f);
                            }
                        }*/

                        pern = ModContent.GetTexture("SGAmod/Extra_49");
                        for (int i = 0; i < 360; i += 360 / 30)
                        {
                            float sizer = 1f - (i / 1000f);
                            Main.spriteBatch.Draw(pern, new Vector2(vecx.X, vecx.Y) - Main.screenPosition, null, Color.White * 0.1f, MathHelper.ToRadians(i) + (Main.GlobalTime * ((i % (360 / 15)) == 0 ? 0.25f : -0.25f)), new Vector2(pern.Width / 2, pern.Height / 2), (new Vector2(1f, 1f) * size) * sizer, SpriteEffects.None, 0f);
                        }


                    }
                    Main.spriteBatch.End();
                }

                swaptargets = (swaptargets + 1) % 2;
                RenderTarget2D target = swaptargets == 0 ? SGAmod.postRenderEffectsTarget : SGAmod.postRenderEffectsTargetCopy;
                RenderTarget2D targetOther = swaptargets == 1 ? SGAmod.postRenderEffectsTarget : SGAmod.postRenderEffectsTargetCopy;

                Main.graphics.GraphicsDevice.SetRenderTarget(target);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1f, 1f, 0f));

                //Main.spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
                Main.spriteBatch.Draw(targetOther, new Vector2(0, 0), null, Color.Black * 0.96f, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                foreach (Projectile proj in Main.projectile.Where(proj => proj.active && proj.modProjectile != null && proj.modProjectile is IPostEffectsDraw))
                {
                    (proj.modProjectile as IPostEffectsDraw).PostEffectsDraw(Main.spriteBatch, 2f);
                }
                foreach (NPC npc in Main.npc.Where(npc => npc.active && npc.modNPC != null && npc.modNPC is IPostEffectsDraw))
                {
                    (npc.modNPC as IPostEffectsDraw).PostEffectsDraw(Main.spriteBatch, 2f);
                }

                Main.spriteBatch.End();


                Main.graphics.GraphicsDevice.SetRenderTargets(binds);






            }
        }

            public static void DrawPostEffectNPCs(float scale = 2f)
            {
                foreach (Projectile proj in Main.projectile.Where(proj => proj.active && proj.modProjectile != null && proj.modProjectile is IPostEffectsDraw))
                {
                    (proj.modProjectile as IPostEffectsDraw).PostEffectsDraw(Main.spriteBatch, scale);
                }
                foreach (NPC npc in Main.npc.Where(npc => npc.active && npc.modNPC != null && npc.modNPC is IPostEffectsDraw))
                {
                    (npc.modNPC as IPostEffectsDraw).PostEffectsDraw(Main.spriteBatch, scale);
                }
            }

        public static VertexBuffer vertexBuffer;

        public static void InitTestThings()
        {
            if (!Main.dedServ)
            {
                DrawOverride.basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);

                VertexPositionColor[] vertices = new VertexPositionColor[3];
                vertices[0] = new VertexPositionColor(new Vector3(0, 1f, 0), Color.Red);
                vertices[1] = new VertexPositionColor(new Vector3(+1f, -1f, 0), Color.Green);
                vertices[2] = new VertexPositionColor(new Vector3(-1f, -1f, 0), Color.Blue);

                DrawOverride.vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
                DrawOverride.vertexBuffer.SetData<VertexPositionColor>(vertices);
            }
        }

        public static BasicEffect basicEffect;
        public static Matrix world = WVP.World();
        public static Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 3), new Vector3(0, 0, 0), new Vector3(0, 1, 0));
        public static Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f/600f, 0.01f, 500f);

        public Vector3 ConvertToPolyspace(Vector2 input)
        {
            Vector2 scale = new Vector2(Main.screenWidth / 1920f,Main.screenHeight / 1024f);
            //input *= scale;
            return new Vector3(((-Main.screenWidth / 2) + input.X) * 0.920f, ((Main.screenHeight / 2) - input.Y) * 1.21f, 0) / 16f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (!Main.dedServ)
            {

                /*
                Vector3 loc = new Vector3(Main.MouseScreen.X, Main.MouseScreen.Y, 0);

                VertexPositionColor[] vertices = new VertexPositionColor[3];
                vertices[0] = new VertexPositionColor(loc+new Vector3(0, 100f, 0), Color.Red);
                vertices[1] = new VertexPositionColor(loc+new Vector3(+100f, -100f, 0), Color.Green);
                vertices[2] = new VertexPositionColor(loc+new Vector3(-100f, -100f, 0), Color.Blue);

                DrawOverride.vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColor), vertices.Length, BufferUsage.WriteOnly);
                DrawOverride.vertexBuffer.SetData<VertexPositionColor>(vertices);

                //GraphicsDevice device = Main.graphics.GraphicsDevice;

                Vector2 zoom = Main.GameViewMatrix.Zoom;
                view = WVP.View(zoom);
                projection = WVP.Projection();

                basicEffect.World = world;
                basicEffect.View = view;
                basicEffect.Projection = projection;
                basicEffect.VertexColorEnabled = true;

                Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 1);
                }*/

                PrismShardHinted.Draw(spriteBatch, lightColor);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                DimDingeonsWorld.DrawSectors();

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                float alpha = 1f-((Main.LocalPlayer.GetModPlayer<SGADimPlayer>().lightSize - 1800f) / 600f);

                Matrix Custommatrix = Matrix.CreateScale(1f, 1f, 0f);// Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);

                if (Lighting.lightMode < 2)
                {

                    if (alpha > 0f)
                    {
                        bool isMurk = NPC.CountNPCS(ModContent.NPCType<Murk>()) > 0;
                        Main.spriteBatch.End();

                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);
                        //Draw Shadow RenderTarget2D
                        if (!isMurk || (isMurk && !SGAConfigClient.Instance.Murklite))
                        Main.spriteBatch.Draw(SGAmod.drawnscreen, new Vector2(0, 0), null, new Color(50, 50, 50) * alpha * SGAmod.fogAlpha, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);

                    ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye);
                    shader.UseOpacity(1f);
                    shader.UseSaturation(1f);
                    DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), new Vector2(Main.GlobalTime * 6, 0), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int)(Main.GlobalTime * 64f) * (DrawOverride.swaptargets == 0 ? 1 : -1), 0, 64, 64)), Microsoft.Xna.Framework.Color.White, Main.GlobalTime * 30f, new Vector2(256f, 256f), 1f, SpriteEffects.None, 0);
                    shader.Apply(null, new DrawData?(value9));

                    Main.spriteBatch.Draw(SGAmod.postRenderEffectsTargetCopy, Main.rand.NextVector2Circular(8, 24).RotatedByRandom((double)MathHelper.TwoPi), null, Color.White * 0.50f, 0, new Vector2(0, 0), new Vector2(2f, 2f), SpriteEffects.None, 0f);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);

                    Main.spriteBatch.Draw(SGAmod.postRenderEffectsTarget, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
                else
                {
                    DrawPostEffectNPCs(1f);
                }
                //}




                List<HellionInsanity> madness= DimDungeonsProxy.madness;

                if (madness.Count > 0)
                {
                    for (int i = 0; i < madness.Count; i += 1)
                    {
                        HellionInsanity pleasemakeitstop = madness[i];
                        pleasemakeitstop.Draw();
                    }
                }
            }

            return false;

        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.timeLeft = 2;
            projectile.hide = true;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
        {
            //DimDingeonsWorld.Instance.PostUpdate();
        }


    }
    public class DimDingeonsWorld : ModWorld
    {

        private Subworld lastworld;
        public static int texttimer=0;
        public static int deathtimer = 0;
        public static ModWorld Instance;
        public static List<DarkSector> darkSectors = new List<DarkSector>();
        public static bool ancientLockedFabric = true;

        public override void Load(TagCompound tag)
        {
            DimDingeonsWorld.Instance = this;
        }
        public override void Initialize()
        {
            ancientLockedFabric = true;
            darkSectors.Clear();
        }

        public static bool DrawSectors()
        {

            BlendState blind = new BlendState
            {

                ColorSourceBlend = Blend.SourceColor,
                ColorDestinationBlend = Blend.InverseSourceColor,

                ColorBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.SourceColor,

                AlphaDestinationBlend = Blend.SourceColor,
                AlphaBlendFunction = BlendFunction.Subtract

            };

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointWrap, DepthStencilState.DepthRead, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
            Vector2 size = new Vector2(60, 60);
            foreach (DarkSector sector in DimDingeonsWorld.darkSectors)
            {
                sector.Draw(size, ModContent.GetTexture("SGAmod/Extra_49c"));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.ZoomMatrix);
            return true;

        }

        public override void PostUpdate()
        {

            if (NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && !SGAmod.anysubworld && darkSectors.Count<1 && SGAWorld.modtimer>150)
            {
                UnifiedRandom rando = new UnifiedRandom(Main.worldName.GetHashCode());

                Point randomspot = new Point(rando.Next(300, Main.maxTilesX - 600), rando.Next((int)(Main.maxTilesY * 0.25), Main.maxTilesY - 300));

                new DarkSector(randomspot.X, randomspot.Y, seed: Main.worldName.GetHashCode());

            }

            foreach (DarkSector sector in darkSectors)
            {
                sector.PostUpdate();
            }

            if (SGAPocketDim.WhereAmI != null)
                SGAmod.cachedata = true;

            DimDingeonsWorld.Instance = this;
            if (SGAPocketDim.WhereAmI==null)
            SGAWorld.dungeonlevel = 0;
            if (DimDingeonsWorld.Instance != null)
            {
                //Main.NewText(SGAWorld.dungeonlevel);
                DimDingeonsWorld.texttimer += 1;
                if (lastworld != SLWorld.currentSubworld || SGAPocketDim.WhereAmI==null)
                    DimDingeonsWorld.texttimer = 0;
                lastworld = SLWorld.currentSubworld;

                if (DimDingeonsWorld.deathtimer > 0)
                {
                    DimDingeonsWorld.deathtimer +=1;
                    if (deathtimer > 200)
                    {
                        DimDingeonsWorld.deathtimer = 0;
                        Subworld.Exit(true);
                    }
                }

            }
        }



    }

    public abstract class DimDungeonsInterface
    {

        public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {

            for (int k = 0; k < layers.Count; k++)
            {
                if (layers[k].Name == "Vanilla: Resource Bars")
                {
                    layers.Insert(k + 1, new LegacyGameInterfaceLayer("SGAmod: DIMDUNGHUD", DrawHUD, InterfaceScaleType.Game));
                }
            }
        }

        public static bool DrawHUD()
        {

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            if (Main.gameMenu || SGAmod.Instance == null && Main.netMode != 1)
                return true;

            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub)
                {
                    string dimstring = sub.DimName;
                    float alpha = Math.Min((DimDingeonsWorld.texttimer - 150f) / 80f, 1f);
                    if (DimDingeonsWorld.texttimer>400)
                        alpha = 1-Math.Min((DimDingeonsWorld.texttimer - 500f) / 80f, 1f);

                    SpriteBatch spriteBatch = Main.spriteBatch;

                    //All this crap just to draw a Text effect on the screen that fades in and out

                    if (dimstring != null)
                    {

                        Player locply = Main.LocalPlayer;
                        spriteBatch.End();

                        Vector2 offset = new Vector2(Main.screenWidth / 2, Main.screenHeight / 3);
                        Matrix Custommatrix2 = Matrix.CreateScale(2f, 2f, 1) * Matrix.CreateTranslation(offset.X, offset.Y, 0f) *
                        Matrix.CreateScale(Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);
                        spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Custommatrix2);

                        Vector2 size = Main.fontDeathText.MeasureString(dimstring);
                        if (alpha > 0f)
                            spriteBatch.DrawString(Main.fontDeathText, dimstring, new Vector2(-size.X / 2f, 0), Color.White * alpha);

                    }

                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1f,1f,1f));

                    float alpha2 = SGADimPlayer.staticHeartBeat>0 ? (float)Math.Sin((SGADimPlayer.staticHeartBeat/30f)*MathHelper.Pi) * 150f : 0;
                    //A Fade out black screen effect
                    spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * MathHelper.Clamp(((float)(DimDingeonsWorld.deathtimer+alpha2) / 200f),0f,1f), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);



                }
            }

            return true;
        }



    } 

}
