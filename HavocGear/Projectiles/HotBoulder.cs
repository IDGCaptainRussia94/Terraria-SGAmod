using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Projectiles
{
    public class HotBoulder : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hot Boulder");
		}
		
		public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;      
            projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 3;
            projectile.melee = true;
            projectile.light = 0.5f;
            projectile.width = 16;
            projectile.height = 16;
        }
       
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
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
            }
            return false;
        }

	    public override bool PreKill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item45,projectile.Center);
        	int numProj = 2;
            float rotation = MathHelper.ToRadians(1);
            for (int i = 0; i < numProj; i++)
            {
                Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
                Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("BoulderBlast"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
            }
            return true;
        }
		
		public override void AI()
        {
			
           int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, mod.DustType("HotDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            Main.dust[DustID2].noGravity = true;
		
		
		}
    }
}