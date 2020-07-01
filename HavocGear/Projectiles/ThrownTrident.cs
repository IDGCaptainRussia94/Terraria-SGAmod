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
    public class ThrownTrident : ModProjectile
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Trident");
	    }
		
		public override void SetDefaults()
        { 
		 projectile.damage = 15;      
            projectile.width = 16;      
            projectile.height = 16; 
            projectile.friendly = false;     
            projectile.melee = false;           
            projectile.tileCollide = true;   
            projectile.penetrate = 4;     
            projectile.timeLeft = 500;  
            projectile.light = 0.25f;   
            projectile.extraUpdates = 1;
   		    projectile.ignoreWater = true;   
        }
        public override void AI()           //this make that the projectile will face the corect way
        {                                                           // |
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;  
       	}
		public override void Kill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item10, projectile.position);
            for (int num315 = 0; num315 < 15; num315 = num315 + 1)
            {
                int dustType = 15;//Main.rand.Next(139, 143);
                int dustIndex = Dust.NewDust(projectile.Center+new Vector2(-16,-16), 32,32, dustType);//,0,5,0,new Color=Main.hslToRgb((float)(npc.ai[0]/300)%1, 1f, 0.9f),1f);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = projectile.velocity.X*0.4f;
                dust.velocity.Y = projectile.velocity.Y*0.4f;
                dust.scale *= 1f + Main.rand.Next(-30, 31) * 0.01f;
                dust.fadeIn = 0.25f;
                dust.noGravity = true;
                Color mycolor = Color.Aqua;//new Color(25,22,18);
                dust.color=mycolor;
                dust.alpha=20;
            }
		}
    }
}