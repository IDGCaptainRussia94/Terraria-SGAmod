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

    public class TwineOfFateClothier : TwineOfFate
    {
        bool remove = false;
        public override float theangle => (float)Math.PI;
        public override int myNPC => NPCID.Clothier;

        public override string Texture
        {
            get { return ("Terraria/Item_" + ItemID.ClothierVoodooDoll); }
        }

    }
    public class TwineOfFate : ModProjectile
    {
        bool remove = false;
        public virtual float theangle => 0f;
        public virtual int myNPC => NPCID.Guide;
        private Vector2[] oldPos = new Vector2[8];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Doll");
        }

        public override bool CanDamage()
        {
            return NPC.CountNPCS(myNPC) > 0;
        }
        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 1000;
            projectile.width = 24;
            projectile.height = 24;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
        }

        public override string Texture
        {
            get { return ("Terraria/Item_" + ItemID.GuideVoodooDoll); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            if (NPC.CountNPCS(myNPC) > 0)
            {
                double angle = theangle + MathHelper.ToRadians(projectile.ai[0]);
                float dist = Math.Min(projectile.localAI[0] * 2, 96f);
                Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));

                //spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 10), color, projectile.velocity.X * 0.04f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
                SGAUtils.DrawFishingLine(projectile.position, Main.player[projectile.owner].Center, new Vector2(20*Math.Sign(thisloc.X), 0), new Vector2(0, 0), 0f);
                return true;
            }
            else
            {
                return false;
            }
        }

        private void GetHit(int damage)
        {
            for (int i = 0; i < Main.maxNPCs; i += 1)
            {
                NPC npc = Main.npc[i];
                if (npc.type == myNPC)
                {
                    Vector2 previously = npc.Center;
                    npc.Center = projectile.Center;
                    npc.StrikeNPC(damage, 0f, 1);
                    if (npc.life > 0 && NPC.CountNPCS(myNPC) > 0)
                    {
                        npc.Center = previously;
                    }
                    else
                    {
                        npc.netUpdate = true;
                    }
                }

            }

        }

        public override void AI()
        {
            projectile.velocity *= 0.95f;
            if (projectile.ai[1] < 1 && NPC.CountNPCS(myNPC) > 0)
            {
                for (int i = 0; i < Main.maxProjectiles; i += 1)
                {
                    Projectile proj = Main.projectile[i];
                    if (proj != null && proj.active)
                    {
                        if (proj.hostile && proj.damage > 0)
                        {
                            Rectangle mecol = projectile.Hitbox;
                            Rectangle themcol = proj.Hitbox;
                            if (themcol.Intersects(mecol) && proj.damage > 1)
                            {
                                projectile.velocity = proj.velocity * 3;
                                proj.velocity *= -1f;
                                Main.PlaySound(SoundID.NPCHit, (int)proj.position.X, (int)proj.position.Y, 1, 1f, 0.25f);

                                projectile.ai[1] = 30;
                                projectile.damage = 1;
                                GetHit(proj.damage);
                            }

                        }
                    }
                }
            }


            Player player = Main.player[projectile.owner];
            if (player == null)
            {
                remove = true;
                projectile.Kill();
            }
            else
            {
                if (player.dead || (!player.GetModPlayer<SGAPlayer>().twinesoffate))
                    return;
                projectile.localAI[1] += 1;
                projectile.timeLeft = 2;
                projectile.ai[1] -= 1;
                projectile.localAI[0] += 1;
                projectile.ai[0] -= 3f;

                double angle = theangle + MathHelper.ToRadians(projectile.ai[0]);
                float dist = Math.Min(projectile.localAI[0] * 2, 96f);
                Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));


                projectile.Center = player.Center + (thisloc);

            }



        }

    }

}