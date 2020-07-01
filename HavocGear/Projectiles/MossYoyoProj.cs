using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class MossYoyoProj : ModProjectile
    {
    	public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quagmire");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 3f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 160f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 14f;
		}
       
	    public override void SetDefaults()
        {
        	Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Amarok);
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 100) < 40 && !target.boss && !target.buffImmune[BuffID.Poisoned])
				target.AddBuff(mod.BuffType("DankSlow"), 60 * 40);
		}
	}
}