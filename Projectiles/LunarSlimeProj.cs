using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;


namespace SGAmod.Projectiles
{
    public class LunarSlimeProjectile : ModProjectile
    {
        bool remove = false;
        private Vector2[] oldPos = new Vector2[8];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Lunar Slime");
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1000;
            projectile.light = 0.5f;
            projectile.width = 24;
            projectile.height = 24;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override string Texture
        {
            get { return ("SGAmod/Items/LunarRoyalGel"); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            if (projectile.ai[1] > 0)
                return false;

            Texture2D tex = mod.GetTexture("Items/LunarRoyalGel");
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 10) / 2f;

            //oldPos.Length - 1
            for (int k = oldPos.Length - 1; k >= 0; k -= 2)
            {

                Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 4f);
                Color color = (Main.hslToRgb((projectile.ai[0] / 8f) % 1f, 1f, 0.9f)) * (1f - (float)(k + 1) / (float)(oldPos.Length + 2));
                int timing = (int)(projectile.localAI[0] / 8f);
                timing %= 10;
                timing *= ((tex.Height) / 10);
                spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 10), color, projectile.velocity.X * 0.04f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return false;
        }

        private void explode()
        {
            Main.PlaySound(SoundID.Item45, projectile.Center);
            int numProj = 2;
            float rotation = MathHelper.ToRadians(1);
            for (int i = 0; i < numProj; i++)
            {
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("SlimeBlast"), projectile.damage*2, projectile.knockBack, projectile.owner, 0f, 0f);
            }
            List<int> types = new List<int>();
                types.Add(BuffID.Regeneration); types.Add(BuffID.RapidHealing); types.Add(BuffID.DryadsWard); types.Add(BuffID.ParryDamageBuff); types.Add(BuffID.Clairvoyance); types.Add(BuffID.Sharpened); types.Add(BuffID.AmmoBox);
            types.Add(BuffID.Honey); types.Add(BuffID.Invisibility); types.Add(BuffID.Ironskin);
            Player player = Main.player[projectile.owner];
            if (!player.dead)
            player.AddBuff(types[Main.rand.Next(0,types.Count)],60*8);
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.ai[1] > 0 || projectile.localAI[0] < 130)
                return false;

            return base.CanHitNPC(target);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (projectile.penetrate < 995)
            {
                projectile.penetrate = 1000;
                projectile.ai[1] = 60 * 6;
                explode();
            }
            projectile.localAI[0] = 100;
            target.immune[projectile.owner] = 1;
        }

        public override void AI()
        {

            if (projectile.ai[1] < 1)
            {
                for (int i = 0; i < Main.maxProjectiles; i += 1)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj != null && proj.active)
                    {
                        if (proj.hostile && projectile.damage>0)
                        {
                            Rectangle mecol = projectile.Hitbox;
                            Rectangle themcol = proj.Hitbox;
                            if (themcol.Intersects(mecol) && proj.damage>1)
                            {
                                proj.damage = 1;
                                projectile.penetrate -= proj.CanReflect() ? 0 : 2;
                                Main.PlaySound(29, (int)proj.position.X, (int)proj.position.Y, Main.rand.Next(66, 69), 1f, -0.6f);

                                if (projectile.penetrate < 995)
                                {
                                    projectile.penetrate = 1000;
                                    projectile.ai[1] = 60 * 10;
                                    explode();
                                }
                            }

                        }
                    }
                }
            }

            for (int k = oldPos.Length - 1; k > 0; k--)
            {
                oldPos[k] = oldPos[k - 1];
            }
            oldPos[0] = projectile.Center;

            if (projectile.ai[1] > 0)
            {
                int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 72, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, Main.hslToRgb((float)(projectile.ai[0] / 4f) % 1, 1f, 0.9f) * 0.2f, 1f);
                Main.dust[DustID2].noGravity = true;
            }

            Player player = Main.player[projectile.owner];
            if (player == null)
            {
                remove = true;
                projectile.Kill();
            }
            else
            {
                if (player.dead || (!player.GetModPlayer<SGAPlayer>().lunarSlimeHeart && projectile.ai[1]<1))
                    return;
                projectile.localAI[1] += 1;
                projectile.timeLeft = 2;
                projectile.ai[1] -= 1;
                projectile.localAI[0] += 1;
                projectile.ai[0] += 0.1f;

                    projectile.damage = player.GetModPlayer<SGAPlayer>().lunarSlimeHeartdamage;

                double angle = ((1f + projectile.ai[0] / 8f)) + 2.0 * Math.PI * (projectile.ai[0] / ((double)8f));
                float dist = Math.Min(projectile.localAI[0] * 2, 100f);
                Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));


                projectile.Center = player.Center + (thisloc);
                projectile.velocity = thisloc;
                projectile.velocity.Normalize();

            }



        }
    }

    public class SlimeBlast : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blast");
        }

        public override void SetDefaults()
        {
            projectile.width = 160;
            projectile.height = 160;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 30;
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
        crit = false;
        }

        public override string Texture
        {
            get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
        }

        public override void AI()
        {
            Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.01f) / 255f, ((255 - projectile.alpha) * 0.025f) / 255f, ((255 - projectile.alpha) * 0.25f) / 255f);
            bool flag15 = false;
            bool flag16 = false;
            if (projectile.velocity.X < 0f && projectile.position.X < projectile.ai[0])
            {
                flag15 = true;
            }
            if (projectile.velocity.X > 0f && projectile.position.X > projectile.ai[0])
            {
                flag15 = true;
            }
            if (projectile.velocity.Y < 0f && projectile.position.Y < projectile.ai[1])
            {
                flag16 = true;
            }
            if (projectile.velocity.Y > 0f && projectile.position.Y > projectile.ai[1])
            {
                flag16 = true;
            }
            if (flag15 && flag16)
            {
                projectile.Kill();
            }
            float num461 = 25f;
            if (projectile.ai[0] > 180f)
            {
                num461 -= (projectile.ai[0] - 180f) / 2f;
            }
            if (num461 <= 0f)
            {
                num461 = 0f;
                projectile.Kill();
            }
            num461 *= 0.7f;
            projectile.ai[0] += 4f;
            int num462 = 0;
            while ((float)num462 < num461)
            {
                float num463 = (float)Main.rand.Next(-30, 31);
                float num464 = (float)Main.rand.Next(-30, 31);
                Vector2 stuff2 = new Vector2(num463, num464);
                stuff2.Normalize();
                stuff2*=(5f+Main.rand.NextFloat(0f,6f))*((float)projectile.width/ 160f);
                int dustx = (Main.rand.NextBool()) ? mod.DustType("AcidDust") : 184;
                if (Main.rand.NextBool())
                    dustx = (Main.rand.NextBool()) ? mod.DustType("HotDust") : 43;
                int num467 = Dust.NewDust(new Vector2(projectile.Center.X,projectile.Center.Y), 0,0, dustx);
                Main.dust[num467].noGravity = true;
                Main.dust[num467].scale = 1f;
                Main.dust[num467].position.X = projectile.Center.X;
                Main.dust[num467].position.Y = projectile.Center.Y;
                Main.dust[num467].position.X += (float)Main.rand.Next(-10, 11);
                Main.dust[num467].position.Y += (float)Main.rand.Next(-10, 11);
                Main.dust[num467].velocity.X = stuff2.X;
                Main.dust[num467].velocity.Y = stuff2.Y;
                //Main.dust[num467].fadeIn = 0f;
                Main.dust[num467].noGravity = true;
                Main.dust[num467].color = Main.hslToRgb(Main.rand.NextFloat(0f, 1f), 0.7f, 1f);
                num462++;
            }
            return;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(mod.BuffType("ThermalBlaze"), 300);
            target.AddBuff(BuffID.Daybreak, 300);
            target.AddBuff(mod.BuffType("AcidBurn"), 180);
            target.AddBuff(BuffID.Frostburn, 300);
        }
    }

}