using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class CutestIceFairy : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cutest Ice Fairy");
			Main.projFrames[projectile.type] = 8;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.DD2PetGato); //Has 8 frames. But doesn't seem to use the last 4 frames, even in vanilla. Vanilla: Projectile_703
															   //Modified AI to use the last 4 frames if the projectile is moving fast
			projectile.aiStyle = -1;
			projectile.width = 24;
			projectile.height = 24;
			//aiType = ProjectileID.DD2PetGato;
			drawOffsetX = -12;
			drawOriginOffsetY -= 14;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.petFlagDD2Gato = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.CutestIceFairyBuff>()))
			{
				projectile.timeLeft = 2;
			}
			Lighting.AddLight(projectile.Center, 0.0f, 0.18f, 0.25f);

			//Taken from AI_144_DD2Pets() and modified
			float numIs3f = 3f;
			int numOfFrames = Main.projFrames[projectile.type];
			Vector2 playerDirection = new Vector2(player.direction * 30, -20f);
			playerDirection.Y += (float)Math.Cos(projectile.localAI[0] * ((float)Math.PI / 30f)) * 2f;
			projectile.direction = (projectile.spriteDirection = player.direction);
			Vector2 playerCenterAndDirection = player.MountedCenter + playerDirection;
			float distToProjCenter = Vector2.Distance(projectile.Center, playerCenterAndDirection);
			if (distToProjCenter > 1000f)
			{
				projectile.Center = player.Center + playerDirection;
			}
			Vector2 distPlayerToProj = playerCenterAndDirection - projectile.Center;
			if (distToProjCenter < numIs3f)
			{
				projectile.velocity *= 0.25f;
			}
			if (distPlayerToProj != Vector2.Zero)
			{
				if (distPlayerToProj.Length() < numIs3f * 0.5f)
				{
					projectile.velocity = distPlayerToProj;
				}
				else
				{
					projectile.velocity = distPlayerToProj * 0.1f;
				}
			}
			if (projectile.velocity.Length() > 6f)
			{
				float num7 = projectile.velocity.X * 0.08f + projectile.velocity.Y * projectile.spriteDirection * 0.02f;
				if (Math.Abs(projectile.rotation - num7) >= (float)Math.PI)
				{
					if (num7 < projectile.rotation)
					{
						projectile.rotation -= (float)Math.PI * 2f;
					}
					else
					{
						projectile.rotation += (float)Math.PI * 2f;
					}
				}
				float num8 = 12f;
				projectile.rotation = (projectile.rotation * (num8 - 1f) + num7) / num8;
				if (projectile.frameCounter++ >= 2)
				{
					projectile.frameCounter = 0;
					projectile.frame++;
				}
			}
			else
			{
				if (projectile.rotation > (float)Math.PI)
				{
					projectile.rotation -= (float)Math.PI * 2f;
				}
				if (projectile.rotation > -0.005f && projectile.rotation < 0.005f)
				{
					projectile.rotation = 0f;
				}
				else
				{
					projectile.rotation *= 0.96f;
				}
				if (projectile.frameCounter++ >= 4)
				{
					projectile.frameCounter = 0;
					if (projectile.frame++ >= numOfFrames)
					{
						projectile.frame = 0;
					}
				}
			}
			if (Math.Abs(projectile.velocity.X) > 6f)
			{
				if (projectile.frame < 4)
				{
					projectile.frame += 4;
				}
				if (projectile.frame >= 8)
				{
					projectile.frame = 4;
				}
			}
			else
			{
				if (projectile.frame >= 4)
				{
					projectile.frame = 0;
				}
			}
			projectile.localAI[0] += 1f;
			if (projectile.localAI[0] > 120f)
			{
				projectile.localAI[0] = 0f;
			}
		}
	}
}