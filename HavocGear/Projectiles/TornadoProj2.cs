using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class TornadoProj2 : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tornado");
		}
        
		public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 5;
            projectile.timeLeft = 500;
			projectile.melee = true;
			projectile.aiStyle = 36;
			projectile.tileCollide = true;
			Main.projFrames[projectile.type] = 6;  
        }

	public override bool? CanHitNPC(NPC target){
		if (projectile.timeLeft<20)
		return false;
		else
		return base.CanHitNPC(target);
	}

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
    {
    projectile.timeLeft=(int)MathHelper.Clamp(projectile.timeLeft,-1,19);
			target.immune[projectile.owner] = 1;

	}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.timeLeft=(int)MathHelper.Clamp(projectile.timeLeft,-1,19);
			return false;
		}

        public override void AI()
        {

		    int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y + 2f), projectile.width + 2, projectile.height + 2, mod.DustType("TornadoDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), projectile.Opacity);
            Main.dust[DustID2].noGravity = true;
            projectile.Opacity=MathHelper.Clamp(((float)projectile.timeLeft/20),0f,1f);

        	if (projectile.timeLeft>19){
			float num472 = projectile.Center.X;
			float num473 = projectile.Center.Y;
			float num474 = 400f;
			bool flag17 = false;
			for (int num475 = 0; num475 < 200; num475++)
			{
				if (Main.npc[num475].CanBeChasedBy(projectile, false) && Collision.CanHit(projectile.Center, 1, 1, Main.npc[num475].Center, 1, 1))
				{
					float num476 = Main.npc[num475].position.X + (float)(Main.npc[num475].width / 2);
					float num477 = Main.npc[num475].position.Y + (float)(Main.npc[num475].height / 2);
					float num478 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num476) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num477);
					if (num478 < num474)
					{
						num474 = num478;
						num472 = num476;
						num473 = num477;
						flag17 = true;
					}
				}
			}
			if (flag17)
			{
				float num483 = 6f;
				Vector2 vector35 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num484 = num472 - vector35.X;
				float num485 = num473 - vector35.Y;
				float num486 = (float)Math.Sqrt((double)(num484 * num484 + num485 * num485));
				num486 = num483 / num486;
				num484 *= num486;
				num485 *= num486;
				projectile.velocity.X = (projectile.velocity.X * 20f + num484) / 21f;
				projectile.velocity.Y = (projectile.velocity.Y * 20f + num485) / 21f;
				return;
			}

			}
        
		}


    }
}