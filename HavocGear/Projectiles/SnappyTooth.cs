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
    public class SnappyTooth : ModProjectile
    {
        public override void SetStaticDefaults()
	{
		DisplayName.SetDefault("Snappy Tooth");
	}
		
	public override void SetDefaults()
        { 
	    projectile.CloneDefaults(ProjectileID.Bullet);
	    aiType = ProjectileID.Bullet;
            projectile.width = 9;      
            projectile.height = 12;
            projectile.friendly = true;     
            projectile.ranged = true;        
            projectile.tileCollide = true;   
            projectile.penetrate = -1;     
            projectile.timeLeft = 2000; 
            projectile.ignoreWater = true;   
        }

        public override bool PreKill(int timeLeft)
        {
            Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
            for (int num315 = 0; num315 < 15; num315 = num315 + 1)
            {
                int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 51, projectile.velocity.X+(float)(Main.rand.Next(-20,20)/15f), projectile.velocity.Y+(float)(Main.rand.Next(-20,20)/15f), 50, Main.hslToRgb(0f, 0.15f, 0.8f)*0.75f, 0.75f);
                Dust dust3 = Main.dust[num316];
                dust3.velocity *= 0.75f;
            }
            return true;
        }

	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)

	{
        if (this.GetType().Name=="SnappyTooth")
		target.AddBuff(mod.BuffType("Gourged"), 240);
	}			
    }
}