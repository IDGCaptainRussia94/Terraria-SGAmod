using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles
{
    public class Explosion : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosion");
		}

		public override void SetDefaults()
        {
            projectile.width = 96;
            projectile.height = 96;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.timeLeft = 2;
        }

   		public override string Texture
		{
			get { return("SGAmod/HavocGear/Projectiles/BoulderBlast");}
		}     

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        target.immune[projectile.owner] = 2;
    	}

    }

}