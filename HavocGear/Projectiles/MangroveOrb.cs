using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using SGAmod.Dusts;

namespace SGAmod.HavocGear.Projectiles
{
	public class MangroveOrb : ModProjectile
	{
		double keepspeed = 0.0;
		float homing = 0.15f;
		public float beginhoming = 20f;
		public Player P;
		public NPC target2;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Orb");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 320;
			projectile.alpha = 100;
			projectile.light = 0.4f;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.extraUpdates = 1;
			aiType = ProjectileID.AmethystBolt;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				Main.PlaySound(SoundID.Item10, projectile.position);
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void AI()
		{
			if (projectile.timeLeft < 200)
				projectile.aiStyle = 1;
			Lighting.AddLight(projectile.position, 0.0f, 0.3f, 0.1f);
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			if (Main.rand.Next(3) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.MangroveDust>(), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 200, default(Color), 0.7f);
				Main.dust[dustIndex].velocity += projectile.velocity * 0.3f;
				Main.dust[dustIndex].velocity *= 0.2f;
			}

			projectile.ai[0] = projectile.ai[0] + 1;
			if (projectile.ai[0] < 2)
			{
				keepspeed = (projectile.velocity).Length();
			}
			if (target2 == null || !target2.active)
				target2 = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
			if (target2 != null)
			{
				if ((target2.Center - projectile.Center).Length() < 800f)
				{
					if (projectile.ai[0] > (beginhoming))
					{
						projectile.velocity = projectile.velocity + (projectile.DirectionTo(target2.Center) * ((float)keepspeed * homing));
						projectile.velocity.Normalize();
						projectile.velocity = projectile.velocity * (float)keepspeed;
					}
				}

			}
		}

	}
}
