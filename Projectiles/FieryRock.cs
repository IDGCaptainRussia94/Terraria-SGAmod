using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.Projectiles
{
    public class FieryRock : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Rock");
		}
		
		public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;      
            projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
            projectile.width = 24;
            projectile.height = 24;
            projectile.magic = true;
            projectile.tileCollide=false;
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
                Main.PlaySound(SoundID.Item10,projectile.Center);
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
			
           int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            Main.dust[DustID2].noGravity = true;
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0]>0 || !player.channel || player.dead){
            projectile.tileCollide=true;
            projectile.ai[0]+=1;
            }else{
            projectile.timeLeft=600;
            Vector2 mousePos = Main.MouseWorld;

            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = mousePos - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
                projectile.Center = mousePos;
            }
            int dir = projectile.direction;
            player.ChangeDir(dir);
            player.itemTime = 40;
            player.itemAnimation = 40;
            player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);

            projectile.Center=player.Center+(projectile.velocity*76f);
            projectile.velocity*=8f;

        }
		
		
		}
    }
}