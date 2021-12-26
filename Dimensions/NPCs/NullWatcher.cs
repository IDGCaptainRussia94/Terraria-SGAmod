using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Buffs;
using SubworldLibrary;

namespace SGAmod.Dimensions.NPCs
{

    public class NullWatcher : ModNPC,IPostEffectsDraw
    {
        Player P;
        Vector2 wantToGoTo=default;
        public static List<NullWatcher> watchers = new List<NullWatcher>();
        float visableEffect = 0f;
        int timer = 0;
        float eyeAngle = 0;
        public static int SeeDistance { get; set; }
        public static int HearDistance { get; set; }

        static NullWatcher()
        {
        SeeDistance = 600;
        HearDistance = 600;
        }

         public float Awareness
        {
            get
            {
                return (int)npc.ai[0];
            }
            set
            {
                npc.ai[0] = value;
            }
        }
        public float AwarenessDelay
        {
            get
            {
                return (int)npc.ai[1];
            }
            set
            {
                npc.ai[1] = value;
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("???");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }
        public override void SetDefaults()
        {
            npc.width = 38;
            npc.height = 32;
            npc.damage = 14;
            npc.defense = 6;
            npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 1.1f;
            npc.dontTakeDamage = true;
            npc.noGravity = true;
            npc.noTileCollide = true;
            npc.aiStyle = -1;
            npc.alpha = 0;
            npc.SGANPCs().dotImmune = true;
            npc.SGANPCs().TimeSlowImmune = true;
        }

        public static void SoundChecks(Vector2 where)
        {
            DoAwarenessChecks(HearDistance, false, true, where);
        }

        public static void DoAwarenessChecks(int distance, bool checkwalls, bool sound = false,Vector2 soundLoc=default)
        {
            if (watchers.Count < 1)
                return;


            if (!Main.gameMenu && sound == false && SGAPocketDim.WhereAmI != null)
            {
                if (SLWorld.currentSubworld is SGAPocketDim sub && sub.GetType() == typeof(LimboDim))
                {
                    distance *= 2;
                }

            }

            foreach (NullWatcher watcher in watchers)
            {
                if (!sound)
                {
                    List<Player> players = Main.player.Where(playercheck => playercheck != null && playercheck.active && !playercheck.dead && (((!playercheck.invis || playercheck.itemTime > 0) && !playercheck.SGAPly().magatsuSet) || playercheck.SGAPly().watcherDebuff>=500) && playercheck.Distance(watcher.npc.Center) < (distance+playercheck.SGAPly().watcherDebuff)
                   && (!checkwalls || Collision.CanHitLine(playercheck.Center, 1, 1, watcher.npc.Center, 1, 1))).ToList();
                    players = players.OrderBy(playercheck2 => playercheck2.Distance(watcher.npc.Center)).ToList();

                    if (players != null && players.Count>0)
                    {
                        Player them = players[0];
                        int add = them.SGAPly().watcherDebuff > 0 ? 2 : 1;
                        watcher.GrowAwareness(soundLoc, add, distance,true,them);
                    }

                }

                if (sound && (soundLoc - watcher.npc.Center).Length() < distance)
                {
                    watcher.GrowAwareness(soundLoc, 30, distance, false);
                }

            }

        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            if (SGAPocketDim.WhereAmI != null && SGAPocketDim.WhereAmI == typeof(LimboDim))
            {
                SGADimPlayer dimply = target.GetModPlayer<SGADimPlayer>();
                dimply.enterlimbo = -7;
                return;
            }
            if (Main.netMode < 1)
            SGAPocketDim.EnterSubworld(mod.GetType().Name + "_LimboDim");
            return;
        }

        public void GrowAwareness(Vector2 location,int ammount,int maxdistance,bool sight=false,Player player=null)
        {
            bool limbo = SGAPocketDim.WhereAmI == typeof(LimboDim);
            if (sight)
            {
                if ((Awareness > 0 || (SGAmod.anysubworld && limbo)) && timer%2==0)
                {
                    P = player;
                    Awareness += (int)(ammount);
                    AwarenessDelay = Math.Max(AwarenessDelay, 300);

                    npc.ai[3] = 200;

                    npc.localAI[3] = (player.Center - npc.Center).ToRotation();

                    if (Awareness > 60)
                    {
                        player.AddBuff(Idglib.Instance.BuffType("LimboFading"), (int)(Awareness-100)/2);
                        //player.GetModPlayer<IdgPlayer>().radationlevel += (int)(Awareness/10);
                    }

                    if (Awareness > 200)
                    {
                        wantToGoTo = P.Center;
                    }
                }
            }
            else
            {
                if (limbo)
                    ammount = (int)(ammount/1.50f);

                int boost = (int)((ammount) * ((1f-(npc.Distance(location)/(float)maxdistance))));
                if (Awareness > 30)
                    boost /= 3+(int)(1+Awareness/300);

                    Awareness += boost;

                if (Awareness > 40)
                {
                    wantToGoTo = location;
                    AwarenessDelay = 60*8;
                    npc.localAI[3] = (location - npc.Center).ToRotation();
                }
            }

        }

        public override bool PreAI()
        {
            NullWatcher.watchers.Add(this);
            return true;
        }

        public override void AI()
        {
            timer += 1;
            AwarenessDelay -= 1f;
            if (AwarenessDelay < 1)
            {
                Awareness -= 1;
                if (Awareness < 1)
                {
                    Awareness = 0;
                    P = null;
                }
            }

            if (npc.ai[3] < 100 && Main.netMode != 1)
            {
                npc.ai[3] = 100;

                for(int i = 0; i < Main.rand.Next(48,300); i += 16)
                {
                    if (Collision.CanHitLine(npc.Center,1,1,npc.Center - new Vector2(0, 16), 1, 1))
                    {
                        npc.Center -= new Vector2(0,16);
                    }
                    else
                    {
                        break;
                    }

                }

                npc.netUpdate = true;

            }

            npc.damage = 0;
            if (Awareness > 200)
                npc.damage = 1;

            visableEffect = MathHelper.Clamp(visableEffect + (AwarenessDelay > 0 || Awareness > 0 ? 0.01f : -0.02f), 0f, 1f);
            eyeAngle = eyeAngle.AngleTowards(npc.localAI[3],(1f/60f)+ (npc.localAI[0]/600f));

            if (visableEffect<=0 && npc.ai[3] > 100 && Awareness == 0)
            {
                npc.active = false;
            }

            if (Main.rand.Next(0,200)==0)
            npc.localAI[3] = Main.rand.NextFloat(0f, MathHelper.TwoPi);

            npc.localAI[2] = MathHelper.Clamp((Awareness-60f) / 260f, 0f,1f)*4f;

            if (wantToGoTo == default)
            {
                wantToGoTo = npc.Center;
            }
            else
            {
                Vector2 gothere = wantToGoTo - npc.Center;
                if (gothere.Length() > 18)
                {
                    npc.velocity += Vector2.Normalize(gothere) * (0.01f + Math.Max((Awareness - 100f) / 5000f, 0f));
                }
                npc.velocity *= 0.98f;
            }

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            return false;
        }

        public void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale = 2)
        {
            float alpha = visableEffect;
            if (alpha <= 0)
                return;

            Texture2D tex = Main.npcTexture[npc.type];
            Rectangle rect = new Rectangle(0, (tex.Height / 7) * Math.Min(2+(int)(Awareness / 60f),6), tex.Width, tex.Height / 7);
            Rectangle recteye = new Rectangle(0, 0, tex.Width, tex.Height / 7);

            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;


            Vector2 drawPos = (npc.Center - Main.screenPosition)/ drawScale;


            float scale = (2f / drawScale);


            spriteBatch.Draw(tex, drawPos, rect, Color.White * alpha, 0, drawOrigin, npc.scale * scale, SpriteEffects.None, 0f);

            spriteBatch.Draw(tex, drawPos + eyeAngle.ToRotationVector2() * Math.Max(0,npc.localAI[2]) * scale, recteye, Color.White * alpha * MathHelper.Clamp(npc.localAI[2],0f,1f), 0, drawOrigin, npc.scale * scale, SpriteEffects.None, 0f);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];

            if (!Main.gameMenu && SGAPocketDim.WhereAmI != null)
            {
                if (SGAPocketDim.WhereAmI != null && SGAPocketDim.WhereAmI == typeof(LimboDim))
                {
                    return 1f;
                }

            }

            if (DimDingeonsWorld.darkSectors.Count < 1)
                return 0f;

            float chance = spawnInfo.player.SGAPly().ShadowSectorZone > 0 ? 0.5f : 0;

            if (Main.rand.Next(6) == 0)
            {
                foreach (DarkSector sector in DimDingeonsWorld.darkSectors)
                {
                    if (sector.PointInside(new Vector2(spawnInfo.spawnTileX * 16, spawnInfo.spawnTileY * 16), 8))
                        chance += 12f;
                }
            }
            chance += spawnInfo.player.SGAPly().watcherDebuff > 0 ? 0.5f : 0;
            return chance;

        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }
    }

    public class SpookyDarkSectorEye : ModProjectile, IPostEffectsDraw
    {
        public Player P;
        public Vector2 lookat = default;
        public float eyeDist = 4;

        public override string Texture
        {
            get { return ("SGAmod/HavocGear/Projectiles/MangroveOrb"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Null Seeker");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public static void Release(Vector2 center, bool caliburn = false, Vector2 speed = default)
        {
            if (speed == default)
                speed = new Vector2(8f, 8f);
            if ((!SGAWorld.darknessVision) || !caliburn)
            {
                if (caliburn)
                    Idglib.Chat("The Darkness grows restless...", 180, 25, 25);

                int proj = Projectile.NewProjectile(center, Vector2.One.RotatedByRandom(Math.PI) * speed, ModContent.ProjectileType<SpookyDarkSectorEye>(), 1, 0,ai1: caliburn ? 1 : 0);
                Main.projectile[proj].ai[1] = caliburn ? 1 : 0;
                Main.projectile[proj].netUpdate = true;
            }
        }

        public override void SetDefaults()
        {
            projectile.width = 16;
            projectile.height = 16;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.timeLeft = 99999;
            projectile.alpha = 100;
            projectile.extraUpdates = 6;
            projectile.light = 0f;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.extraUpdates = 1;
            aiType = ProjectileID.AmethystBolt;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public virtual void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale = 2f)
        {

            float alpha = 1f;
            if (projectile.ai[0] < 1000 && projectile.ai[0] >= 0)
                alpha = Math.Max((projectile.ai[0] - 600) / 400f, 0);
            if (projectile.ai[0] < 0)
                alpha = 1f + projectile.ai[0] / 120f;

            if (alpha <= 0)
                return;

            Texture2D tex = ModContent.GetTexture("SGAmod/Dimensions/NPCs/NullWatcher");
            Rectangle rect = new Rectangle(0, (tex.Height / 7) * 6, tex.Width, tex.Height / 7);
            Rectangle recteye = new Rectangle(0, 0, tex.Width, tex.Height / 7);

            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 7) / 2f;

            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = (projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY))/ drawScale;
                float coloralpha = ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);

                float scale = (1f + (projectile.ai[0] < 0 ? -projectile.ai[0] / drawScale : 0)) * (2f / drawScale);

                spriteBatch.Draw(tex, drawPos, rect, Color.GreenYellow * coloralpha * 0.75f * alpha, projectile.rotation, drawOrigin, projectile.scale * 1f * scale, SpriteEffects.None, 0f);
                spriteBatch.Draw(tex, drawPos + (lookat == default ? Vector2.Zero : (Vector2.Normalize(lookat - projectile.Center) * eyeDist)), recteye, Color.White * coloralpha * alpha, projectile.rotation, drawOrigin, projectile.scale * scale, SpriteEffects.None, 0f);
            }
        }

        public override void AI()
        {
            projectile.velocity /= 1.05f;

            if (projectile.ai[0] < 0)
            {



                if (projectile.ai[0] == -2)
                {

                    if (SGAWorld.darknessVision == true)
                    {
                        if (P != null && P.active && !P.dead)
                        {
                            P.AddBuff(ModContent.BuffType<Watched>(), 60 * 45);
                        }
                    }

                    SoundEffectInstance sound = Main.PlaySound(SoundID.Roar, (int)projectile.Center.X, (int)projectile.Center.Y, 2);
                    if (sound != null)
                        sound.Pitch -= 0.75f;
                }
                projectile.ai[0] -= 1;
                if (P != null && P.active && !P.dead)
                {
                    projectile.Center = P.Center;
                }
                if (projectile.ai[0] < -120)
                {
                    projectile.Kill();
                }

                if ((int)projectile.ai[1] == 1)
                {
                    if (SGAWorld.darknessVision != true)
                    {
                        SGAWorld.darknessVision = true;

                        if (Main.netMode == 2)
                        {
                            NetMessage.SendData(MessageID.WorldData);
                        }
                        Idglib.Chat("Your Compass has has gained a new function...", 180, 25, 25);
                    }
                }

                return;
            }

            projectile.ai[0] += 1;

            if (projectile.ai[0] > 600)
            {
                P = Main.player[Player.FindClosest(projectile.Center, 1, 1)];
                lookat = P.Center;
                if (P != null && P.active && !P.dead)
                {
                    projectile.velocity += ((P.Center - projectile.Center) * ((1f + projectile.ai[0] / 50f)) / 8000f) * Math.Min((projectile.ai[0] - 600) / 1000f, 1f);
                    if (P.Distance(projectile.Center) < 24 + (projectile.ai[0] / 300))
                    {
                        projectile.ai[0] = -1;

                    }
                }


            }

        }

    }


}
