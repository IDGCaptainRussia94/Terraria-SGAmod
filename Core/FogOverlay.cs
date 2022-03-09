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
using SGAmod.Items.Consumables;
using SGAmod.NPCs.Murk;
using SGAmod.Items;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Dimensions.NPCs;
using SGAmod.NPCs;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.Wraiths;

namespace SGAmod.Dimensions
{

    public class DrawOverride : ModProjectile
    {
        static int fogeffect = 0;
        static int swaptargets = 0;
        public bool customSky = false;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Timer and Fog Drawer");
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
                    int fogDetail = Math.Max((SGAConfigClient.Instance.FogDetail)/5,1);
                    float fogAlpha = 0.04f * (6f / (float)fogDetail);

                    //Draw Texture Parts

                    //Main.spriteBatch.End();

                    Main.graphics.GraphicsDevice.SetRenderTarget(SGAmod.drawnscreen);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                    Texture2D pern = ModContent.GetTexture("SGAmod/Perlin");
                    Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);
                    Main.spriteBatch.Draw(Main.blackTileTexture, new Vector2(0, 0), new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);

                    if (SGAPocketDim.WhereAmI != null && SGAPocketDim.WhereAmI == typeof(SpaceDim))
                    {
                        //pern = ModContent.GetTexture("SGAmod/Space");
                        //Main.spriteBatch.Draw(pern, Vector2.Zero, null, Color.White * 1f, 0, Vector2.Zero, new Vector2(Main.screenWidth / (float)pern.Width, Main.screenHeight / (float)pern.Height), SpriteEffects.None, 0f);

                        SpaceSky.StarryNebulaSky();

                        goto skipdraw;
                    }

                    if (isMurk)
                    {
                        Murk.MurkFog();
                    }
                    else
                    {
                        Main.spriteBatch.Draw(pern, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, (Main.hslToRgb((Main.GlobalTime / 3) % 1f, 1f, 0.75f)).MultiplyRGB(Color.Lerp(Color.Black,Color.White,0.50f)) * 0.5f, Main.GlobalTime * 0.24f, new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(5f, 5f), SpriteEffects.None, 0f);
                        Main.spriteBatch.Draw(pern, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, Main.hslToRgb(((Main.GlobalTime + 1.5f) / 3) % 1f, 1f, 0.75f).MultiplyRGB(Color.Lerp(Color.Black, Color.White, 0.50f)) * 0.5f, Main.GlobalTime * -0.24f, new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(5f, 5f), SpriteEffects.None, 0f);
                    }

                    skipdraw:
                    Main.spriteBatch.End();

                    //Draw Additive Parts
                    Main.graphics.GraphicsDevice.SetRenderTarget(SGAmod.drawnscreenAdditiveTextures);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                    pern = ModContent.GetTexture("SGAmod/Extra_49c");
                    //Yay, finally we have Additive Blending! No more nega-blending!

                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
                    Vector2 half = new Vector2(pern.Width / 2, pern.Height / 2);

                    foreach (PostDrawCollection postdraw in SGAmod.PostDraw)
                    {
                        Vector3 vecx = postdraw.light;
                        float size = vecx.Z / pern.Width;

                        Main.spriteBatch.Draw(pern, new Vector2(vecx.X, vecx.Y) - Main.screenPosition, null, Color.White, 0, half, new Vector2(1.5f, 1.5f) * size, SpriteEffects.None, 0f);

                        for (float scale = 1f; scale < 2f; scale += 0.25f)
                        {
                            for (int i = 0; i < 360; i += 360 / fogDetail)
                            {
                                Main.spriteBatch.Draw(pern, new Vector2(vecx.X, vecx.Y) - Main.screenPosition, null, Color.White * fogAlpha, MathHelper.ToRadians(i), half, new Vector2(1.25f, 1.5f) * size* scale, SpriteEffects.None, 0f);
                            }

                        }
                        //for (int i = 0; i < Main.maxNPCs; i += 1)
                        //{
                        //    if (Main.npc[i].active && Main.npc[i].townNPC)
                        //    {
                        //        for (int x = 0; x < 360; x += 360 / 30)
                        //            Main.spriteBatch.Draw(pern, (Main.npc[i].Center) - Main.screenPosition, null, Color.White * 0.25f, MathHelper.ToRadians(x), new Vector2(pern.Width / 2, pern.Height / 2), new Vector2(7f, 7f), SpriteEffects.None, 0f);
                        //    }
                        //}

                        //pern = ModContent.GetTexture("SGAmod/Extra_49");

                        /*
                         * 
                        for (int i = 0; i < 360; i += 360 / 30)
                        {
                            float sizer = 1f - (i / 1000f);
                            Main.spriteBatch.Draw(pern, new Vector2(vecx.X, vecx.Y) - Main.screenPosition, null, Color.White * 0.1f, MathHelper.ToRadians(i) + (Main.GlobalTime * ((i % (360 / 15)) == 0 ? 0.25f : -0.25f)), new Vector2(pern.Width / 2, pern.Height / 2), (new Vector2(1f, 1f) * size) * sizer, SpriteEffects.None, 0f);
                        }
                        */


                    }
                    Main.spriteBatch.End();
                }

                SGAmod.postRenderEffectsTargetDoUpdates--;

                if (SGAmod.postRenderEffectsTargetDoUpdates < -4)
                {
                    SGAmod.postRenderEffectsTargetDoUpdates = 1;
                }

                if (SGAmod.postRenderEffectsTargetDoUpdates > 0)
                {

                    swaptargets = (swaptargets + 1) % 2;
                    RenderTarget2D target = swaptargets == 0 ? SGAmod.postRenderEffectsTarget : SGAmod.postRenderEffectsTargetCopy;
                    RenderTarget2D targetOther = swaptargets == 1 ? SGAmod.postRenderEffectsTarget : SGAmod.postRenderEffectsTargetCopy;

                    Main.graphics.GraphicsDevice.SetRenderTarget(target);
                    Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                    Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

                    //Main.spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
                    Main.spriteBatch.Draw(targetOther, new Vector2(0, 0), null, Color.Black * 0.96f, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

                    foreach (Projectile proj in Main.projectile.Where(proj => proj.active && proj.modProjectile != null && proj.modProjectile is IPostEffectsDraw))
                    {
                        SGAmod.postRenderEffectsTargetDoUpdates = 450;
                        (proj.modProjectile as IPostEffectsDraw).PostEffectsDraw(Main.spriteBatch, 2f);
                    }
                    foreach (NPC npc in Main.npc.Where(npc => npc.active && npc.modNPC != null && npc.modNPC is IPostEffectsDraw))
                    {
                        SGAmod.postRenderEffectsTargetDoUpdates = 450;
                        (npc.modNPC as IPostEffectsDraw).PostEffectsDraw(Main.spriteBatch, 2f);
                    }

                    Main.spriteBatch.End();

                }


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

                foreach (Player player in Main.player)
                {
                    if (player == null || !player.active)
                        break;
                    if (player.dead)
                        continue;



                    SGAPlayer sgaply = player.SGAPly();

                    if (sgaply.valkyrieSet.Item2 > 0)
                    {
                        UnifiedRandom rando = new UnifiedRandom(player.whoAmI + Main.worldName.GetHashCode());


                        Texture2D extra = Main.extraTexture[89];
                        Vector2 sizehalf = extra.Size() / 2f;

                        float counter = sgaply.valkyrieSet.Item2 * 1f;

                        for (int i = 0; i < counter; i += 1)
                        {

                            float alphaxxx = MathHelper.Clamp((counter - i), 0f, 1f) * 0.75f;

                            Matrix aurashards = Matrix.CreateFromYawPitchRoll(rando.NextFloat(MathHelper.TwoPi) + (Main.GlobalTime * (rando.NextFloat(0.25f, 1f) * (rando.NextBool() ? 1f : -1f))) + (player.Center.X / 120f), 0, rando.NextFloat(-MathHelper.Pi / 1f, MathHelper.Pi / 1f));

                            Vector3 loc = Vector3.Transform(Vector3.UnitX, aurashards);

                            spriteBatch.Draw(extra, (player.MountedCenter + new Vector2(loc.X, loc.Y) * (48f * sgaply.valkyrieSet.Item4)) - Main.screenPosition, null, Main.hslToRgb(rando.NextFloat(1f), 0.45f, 0.75f) * MathHelper.Clamp(loc.Z, 0f, 1f) * alphaxxx, 0, sizehalf, 1f, SpriteEffects.None, 0f);
                        }


                    }


                    if (sgaply.GetEnergyShieldAmmountAndRecharge.Item1 > 0)
                    {
                        //Main.NewText("test");
                        Texture2D noise = SGAmod.Instance.GetTexture(sgaply.jellybruSet ? "NPCs/Prismicbansheerealtex" : "Perlin");
                        Vector2 noisesize = noise.Size();
                        float alphapercent = (sgaply.GetEnergyShieldAmmountAndRecharge.Item1 / (float)sgaply.GetEnergyShieldAmmountAndRecharge.Item2);


                        if (false)//sgaply.jellybruSet)
                        {
                            Effect hallowed = SGAmod.HallowedEffect;


                            hallowed.Parameters["alpha"].SetValue(alphapercent);
                            hallowed.Parameters["prismColor"].SetValue(Color.Magenta.ToVector3());
                            hallowed.Parameters["prismAlpha"].SetValue(0f);
                            hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Stain"));
                            hallowed.Parameters["overlayProgress"].SetValue(new Vector3(Main.GlobalTime / 5f, Main.GlobalTime / 3f, 0f));
                            hallowed.Parameters["overlayAlpha"].SetValue(0.25f);
                            hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, Main.GlobalTime / 4f));
                            hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
                            hallowed.Parameters["rainbowScale"].SetValue(0.8f);
                            hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));

                            hallowed.CurrentTechnique.Passes["Prism"].Apply();


                            Texture2D bubbles = ModContent.GetTexture("Terraria/NPC_" + NPCID.DetonatingBubble);
                            Vector2 half = new Vector2(bubbles.Width, bubbles.Height / 2f) / 2f;

                            spriteBatch.Draw(bubbles, (player.MountedCenter) - Main.screenPosition, new Rectangle(0, bubbles.Height / 2, bubbles.Width, bubbles.Height / 2), Color.LightPink * alphapercent, -(float)Math.Sin(Main.GlobalTime) / 4f, half, (Vector2.One * 1.5f) + (new Vector2((float)Math.Sin(Main.GlobalTime), (float)Math.Cos(Main.GlobalTime))) * 0.25f, SpriteEffects.None, 0f);

                            spriteBatch.Draw(bubbles, (player.MountedCenter) - Main.screenPosition, new Rectangle(0, bubbles.Height / 2, bubbles.Width, bubbles.Height / 2), Color.LightPink * alphapercent, (float)Math.Sin(Main.GlobalTime) / 4f, half, (Vector2.One * 1.5f) + (new Vector2((float)Math.Cos(Main.GlobalTime + MathHelper.PiOver2), (float)Math.Sin(Main.GlobalTime - MathHelper.PiOver2))) * 0.25f, SpriteEffects.None, 0f);

                            Main.spriteBatch.End();
                            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                        }
                        else
                        {

                            DrawData value9 = new DrawData(noise, new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, (int)noisesize.X, (int)noisesize.Y)), Microsoft.Xna.Framework.Color.White, 0, noise.Size() / 2f, 1f, SpriteEffects.None, 0);
                            var deathShader = GameShaders.Misc["ForceField"];
                            deathShader.UseColor(new Vector3(1f, 1f, 1f));
                            GameShaders.Misc["ForceField"].Apply(new DrawData?(value9));
                            deathShader.UseOpacity(1f);

                            float angle = MathHelper.Pi;
                            Vector2 loc = new Vector2((float)((Math.Cos(angle) * 0f)), (float)((Math.Sin(angle) * 0f)));

                            Color basecolor = Color.Lerp(Color.LightBlue, player.GetImmuneAlpha(Color.White, 0.5f), 0.5f) * 0.75f;

                            if (sgaply.jellybruSet)
                            {
                                basecolor = Color.Lerp(Color.Magenta, player.GetImmuneAlpha(Color.LightPink, 0.5f), 0.75f) * 1f;
                            }

                            Color lighting = Lighting.GetColor((int)(player.MountedCenter.X) >> 4, (int)(player.MountedCenter.Y) >> 4, Color.White);
                            spriteBatch.Draw(noise, (player.MountedCenter + loc) - Main.screenPosition, null, basecolor * alphapercent*(((lighting.R+ lighting.G+ lighting.B)/255f)/3f), angle, noise.Size() / 2f, (new Vector2(200f, 150f) / noisesize) * 0.6f, SpriteEffects.None, 0f);
                        }

                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                DimDingeonsWorld.DrawSectors();

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                float alpha = 1f - ((Main.LocalPlayer.GetModPlayer<SGADimPlayer>().lightSize - 1800f) / 600f);

                Matrix Custommatrix = Matrix.CreateScale(1f, 1f, 0f);// Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);

                if (Lighting.lightMode < 2)
                {

                    if (alpha > 0f)
                    {
                        bool isMurk = NPC.CountNPCS(ModContent.NPCType<Murk>()) > 0;
                        Main.spriteBatch.End();

                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, default, default, default, Custommatrix);
                        //Draw Shadow RenderTarget2D
                        if (!isMurk || (isMurk && !SGAConfigClient.Instance.Murklite))
                        {
                            if (SGAmod.drawnscreenAdditiveTextures != null && !SGAmod.drawnscreenAdditiveTextures.IsDisposed)
                            {
                                Effect ShadowEffect = ShadowParticle.ShadowEffect;

                                ShadowEffect.Parameters["overlayTexture"].SetValue(SGAmod.drawnscreenAdditiveTextures);
                                ShadowEffect.Parameters["noiseTexture"].SetValue(SGAmod.drawnscreen);


                                ShadowEffect.Parameters["colorAmmount"].SetValue(256);
                                ShadowEffect.Parameters["screenSize"].SetValue(SGAmod.drawnscreen.Size());
                                //Color.DarkMagenta
                                //Color.Black

                                //Show through
                                float percent = alpha * SGAmod.fogAlpha;


                                ShadowEffect.Parameters["colorFrom"].SetValue(Color.White.ToVector4() * 0f);
                                ShadowEffect.Parameters["colorTo"].SetValue(Color.White.ToVector4() * 1.00f);
                                ShadowEffect.Parameters["colorOutline"].SetValue((Color.Transparent).ToVector4() * 1f);

                                ShadowEffect.Parameters["edgeSmooth"].SetValue(0.0f);
                                ShadowEffect.Parameters["invertLuma"].SetValue(true);
                                ShadowEffect.Parameters["alpha"].SetValue(percent);

                                ShadowEffect.Parameters["noisePercent"].SetValue(1f);
                                ShadowEffect.Parameters["noiseScalar"].SetValue(new Vector4(0, 0, 1f, 1f));

                                ShadowEffect.CurrentTechnique.Passes["ColorFilterInverted"].Apply();



                                Main.spriteBatch.Draw(SGAmod.drawnscreen, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), new Vector2(1f, 1f), SpriteEffects.None, 0f);
                            }
                        }
                    }
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);

                    if (SGAmod.fogDrawNPCsCounter > 0)
                    {

                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.TransformationMatrix);

                        foreach (NPC npc in Main.npc.Where(testby => testby.active && testby.modNPC != null && testby.modNPC is IDrawThroughFog))
                        {
                            IDrawThroughFog ifog = npc.modNPC as IDrawThroughFog;
                            ifog.DrawThroughFog(spriteBatch);
                        }
                        foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.modProjectile != null && testby.modProjectile is IDrawThroughFog))
                        {
                            IDrawThroughFog ifog = proj.modProjectile as IDrawThroughFog;
                            ifog.DrawThroughFog(spriteBatch);
                        }
                    }

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);


                    ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.TwilightDye);
                    shader.UseOpacity(1f);
                    shader.UseSaturation(1f);
                    shader.UseColor(0.4f,0f,0.1f);
                    DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), new Vector2(Main.GlobalTime * 6, 0), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle((int)(Main.GlobalTime * 24f) * (DrawOverride.swaptargets == 0 ? 1 : -1), 0, 64, 64)), Microsoft.Xna.Framework.Color.Purple, Main.GlobalTime * 30f, new Vector2(256f, 256f), 1f, SpriteEffects.None, 0);
                    shader.Apply(null, new DrawData?(value9));

                    for (float f = 0; f < 1f; f += 0.25f)
                    {
                        float perce = (f + Main.GlobalTime/3f) % 1f;
                        Main.spriteBatch.Draw(SGAmod.postRenderEffectsTargetCopy, SGAmod.postRenderEffectsTargetCopy.Size(), null, Color.White * 0.50f*(1f- perce), 0, SGAmod.postRenderEffectsTargetCopy.Size()/2f, new Vector2(2f, 2f)* (1f+MathHelper.SmoothStep(0f,1f, perce) /16f), SpriteEffects.None, 0f);
                    }

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Custommatrix);

                    Main.spriteBatch.Draw(SGAmod.postRenderEffectsTarget, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
                else
                {
                    DrawPostEffectNPCs(1f);
                }
                //}




                List<HellionInsanity> madness = DimDungeonsProxy.madness;

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

}
