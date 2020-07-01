using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;

namespace SGAmod.HavocGear.Projectiles
{
    public class SwampWave : ModProjectile
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("SwampWave");
	    }
		
		public override void SetDefaults()
        { 
            projectile.width = 40;      
            projectile.height = 40; 
            projectile.friendly = true;
            projectile.magic = true;        
            projectile.tileCollide = false;   
            projectile.penetrate = 1000;  
            projectile.timeLeft = 60;  
            projectile.light = 0.75f;   
            projectile.extraUpdates = 1;
   		    projectile.ignoreWater = true;   
        }
        public override bool? CanHitNPC(NPC target)
        {
            if (projectile.penetrate < 996)
                return false;
            return base.CanHitNPC(target);
        }
        public override void AI()         
        {
            projectile.velocity *= 0.98f;
            projectile.alpha = (255- (int)(100 * (projectile.timeLeft / 60f)));
            projectile.scale += 0.02f;
            projectile.width = (int)(40 * projectile.scale);
            projectile.height = (int)(40 * projectile.scale);
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;  
       	
		    if (Main.rand.Next(3) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("MangroveDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 200, default(Color), 0.7f);
				Main.dust[dustIndex].velocity += projectile.velocity * 0.3f;
				Main.dust[dustIndex].velocity *= 0.2f;
			}
		}
    }
}