using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
	public class MudBlob : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 26;
			projectile.height = 26;
			projectile.aiStyle = 1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 3;
			projectile.melee = true;
			projectile.damage = 34;
			projectile.timeLeft = 1000;
			projectile.light = 0.1f;
			aiType = -1;
            Main.projFrames[projectile.type] = 3;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mud Blob");
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			for (int num654 = 0; num654 < 10; num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(projectile.position + Vector2.UnitX * -20f, projectile.width + 40, projectile.height, 184, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y+ randomcircle.Y*8f, 100, new Color(30, 30, 30, 20), 2f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].velocity *= 0.5f;
			}

			for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
			{
				NPC target = Main.npc[num172];
				float damagefalloff = 1f - ((target.Center - projectile.Center).Length() / 120f);
				if ((target.Center - projectile.Center).Length() < 120f && !target.friendly && !target.dontTakeDamage)
				{
					target.AddBuff(BuffID.Oiled, 60 + (int)(60f * damagefalloff * 6f));
				}
			}

			return true;
		}

		public override void AI()
		{
			if (projectile.localAI[1] == 0f)
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}
		}
	}
}