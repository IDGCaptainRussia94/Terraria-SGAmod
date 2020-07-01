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
    public class SkyToothProj : SnappyTooth
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Sky Tooth");
	    }
		
		public override void SetDefaults()
        { 
            projectile.width = 14;      
            projectile.height = 20; 
            projectile.friendly = true;     
            projectile.magic = true;     
            projectile.tileCollide = true;
            projectile.penetrate = 4;     
            projectile.timeLeft = 2000;  
            projectile.light = 0.75f;
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
		}
    }
}