using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class StarfishProjectile : ModProjectile
    {
        public override void SetDefaults()
        {
            projectile.width = 7;
            projectile.height = 7;
            projectile.aiStyle = 1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ranged = true;
            projectile.alpha = 255;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.penetrate = 3;
            projectile.localNPCHitCooldown = 15;
            projectile.usesLocalNPCImmunity = true;
            aiType = ProjectileID.Bullet;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 5; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 190);
                Main.dust[dust].scale = 0.7f;
                Main.dust[dust].noGravity = false;
            }
        }

        public override void AI()
        {
            int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 190, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 0.3f);
            Main.dust[dust].noGravity = true;
            {
                Lighting.AddLight(projectile.position, 0.2f, 0.1f, 0.05f);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 3; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 190);
                Main.dust[dust].scale = 0.4f;
                Main.dust[dust].noGravity = false;
            }

            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
                Main.PlaySound(SoundID.Item10, projectile.position);
            }
            else
            {
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
                Main.PlaySound(SoundID.Item10, projectile.position);
            }
            return false;
        }
    }
}