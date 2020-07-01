using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles
{
	public class SunbringerFlare : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunbringer Flare");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
            projectile.CloneDefaults(163);
			//projectile.width = 30;
			//projectile.height = 30;
			//projectile.aiStyle = 1;
			//projectile.friendly = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			projectile.timeLeft = 400;
		}
       // public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
       // {
       //     target.AddBuff(189, 5 * 60);
        //}
			public override void OnHitNPC(NPC n, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			n.AddBuff(189, 5*60);
		}


	}
}

