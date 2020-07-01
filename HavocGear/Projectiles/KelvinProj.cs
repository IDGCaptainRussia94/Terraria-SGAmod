using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class KelvinProj : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kelvin");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 2.2f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 200f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 14f;
		}
       
	public override void SetDefaults()
        {
        	Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
        }
	
	public override void AI()
	{
		if (Main.rand.Next(3) == 0)
		{
			Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"));
		}
        int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = projectile.velocity*(float)(Main.rand.Next(20,100)*0.005f);

		Lighting.AddLight(projectile.position, 0.6f, 0.5f, 0f);
	}
	
	public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        	Player player = Main.player[projectile.owner];
		target.immune[projectile.owner] = 2;
		target.AddBuff(mod.BuffType("ThermalBlaze"), 120);
    	}
    }
}