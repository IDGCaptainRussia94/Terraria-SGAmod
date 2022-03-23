using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class SlimeBaron : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Slime Baron");
			Main.projFrames[projectile.type] = 12;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.KingSlimePet); in 1.4 //Vanilla Projectile_881
			projectile.netImportant = true;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.timeLeft *= 5;
			projectile.width = 20;
			projectile.height = 20;
			//aiType = ProjectileID.KingSlimePet;
			//aiType = -1;
			drawOffsetX = -8;
			drawOriginOffsetY -= 7;
		}

		/*public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			//player.petFlagKingSlimePet = false; // Relic from aiType
			return true;
		}*/

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.SlimeBaronBuff>()))
			{
				projectile.timeLeft = 2;
			}

			//Taken from 1.4's AI_026

			#region AI
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 85;
			if (player.position.X + (player.width / 2) < projectile.position.X + (projectile.width / 2) - num)
			{
				flag1 = true;
			}
			else if (player.position.X + (player.width / 2) > projectile.position.X + (projectile.width / 2) + num)
			{
				flag2 = true;
			}
			if (projectile.ai[1] == 0f)
			{
				int num69 = 500;
				if (player.rocketDelay2 > 0)
				{
					projectile.ai[0] = 1f;
				}
				Vector2 val8 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
				float num70 = player.position.X + (player.width / 2) - val8.X;
				float num71 = player.position.Y + (player.height / 2) - val8.Y;
				float num72 = (float)Math.Sqrt(num70 * num70 + num71 * num71);
				if (num72 > 2000f)
				{
					projectile.position.X = player.position.X + (player.width / 2) - (projectile.width / 2);
					projectile.position.Y = player.position.Y + (player.height / 2) - (projectile.height / 2);
				}
				else if (num72 > num69 || (Math.Abs(num71) > 300f && ((true && true && (false || true)) || !(projectile.localAI[0] > 0f))))
				{
					if (num71 > 0f && projectile.velocity.Y < 0f)
					{
						projectile.velocity.Y = 0f;
					}
					if (num71 < 0f && projectile.velocity.Y > 0f)
					{
						projectile.velocity.Y = 0f;
					}
					projectile.ai[0] = 1f;
				}
			}
			if (projectile.ai[0] != 0f)
			{
				float num73 = 0.2f;
				int num74 = 200;
				projectile.tileCollide = false;
				Vector2 val9 = new Vector2(projectile.position.X + projectile.width * 0.5f, projectile.position.Y + projectile.height * 0.5f);
				float num75 = player.position.X + (player.width / 2) - val9.X;
				float num81 = player.position.Y + (player.height / 2) - val9.Y;
				float num82 = (float)Math.Sqrt(num75 * num75 + num81 * num81);
				float num84 = 10f;
				if (num82 < num74 && player.velocity.Y == 0f && projectile.position.Y + projectile.height <= player.position.Y + player.height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					if (projectile.velocity.Y < -6f)
					{
						projectile.velocity.Y = -6f;
					}
				}
				if (num82 < 60f)
				{
					num75 = projectile.velocity.X;
					num81 = projectile.velocity.Y;
				}
				else
				{
					num82 = num84 / num82;
					num75 *= num82;
					num81 *= num82;
				}
				if (projectile.velocity.X < num75)
				{
					projectile.velocity.X += num73;
					if (projectile.velocity.X < 0f)
					{
						projectile.velocity.X += num73 * 1.5f;
					}
				}
				if (projectile.velocity.X > num75)
				{
					projectile.velocity.X -= num73;
					if (projectile.velocity.X > 0f)
					{
						projectile.velocity.X -= num73 * 1.5f;
					}
				}
				if (projectile.velocity.Y < num81)
				{
					projectile.velocity.Y += num73;
					if (projectile.velocity.Y < 0f)
					{
						projectile.velocity.Y += num73 * 1.5f;
					}
				}
				if (projectile.velocity.Y > num81)
				{
					projectile.velocity.Y -= num73;
					if (projectile.velocity.Y > 0f)
					{
						projectile.velocity.Y -= num73 * 1.5f;
					}
				}
				if (projectile.velocity.X > 0.5)
				{
					projectile.spriteDirection = -1;
				}
				else if (projectile.velocity.X < -0.5)
				{
					projectile.spriteDirection = 1;
				}
				//Gore for when it transforms from walking to flying
				/*int num86 = 1226; //1226 The Crown
				if (projectile.type == 934)//Slime Princess
				{
					num86 = 1261;
				}
				if (projectile.frame < 6 || projectile.frame > 11)
				{
					Gore.NewGore(new Vector2(projectile.Center.X, projectile.position.Y), projectile.velocity * 0.5f, num86);
				}*/
				projectile.frameCounter++;
				if (projectile.frameCounter > 4)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
				}
				if (projectile.frame < 6 || projectile.frame > 11)
				{
					projectile.frame = 6;
				}
				Vector2 v5 = projectile.velocity;
				v5.Normalize();
				projectile.rotation = v5.ToRotation() + (float)Math.PI / 2f;
			}
			else
			{
				if (projectile.ai[1] != 0f)
				{
					flag1 = false;
					flag2 = false;
				}
				projectile.rotation = 0f;
				projectile.tileCollide = true;
				float num152 = 6f;
				float num151 = 0.2f;
				if (num152 < Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y))
				{
					num152 = Math.Abs(player.velocity.X) + Math.Abs(player.velocity.Y);
					num151 = 0.3f;
				}
				if (flag1)
				{
					if (projectile.velocity.X > -3.5)
					{
						projectile.velocity.X -= num151;
					}
					else
					{
						projectile.velocity.X -= num151 * 0.25f;
					}
				}
				else if (flag2)
				{
					if (projectile.velocity.X < 3.5)
					{
						projectile.velocity.X += num151;
					}
					else
					{
						projectile.velocity.X += num151 * 0.25f;
					}
				}
				else
				{
					projectile.velocity.X *= 0.9f;
					if (projectile.velocity.X >= 0f - num151 && projectile.velocity.X <= num151)
					{
						projectile.velocity.X = 0f;
					}
				}
				if (flag1 || flag2)
				{
					int num153 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
					int j2 = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
					if (projectile.type == 236)
					{
						num153 += projectile.direction;
					}
					if (flag1)
					{
						num153--;
					}
					if (flag2)
					{
						num153++;
					}
					num153 += (int)projectile.velocity.X;
					if (WorldGen.SolidTile(num153, j2))
					{
						flag4 = true;
					}
				}
				if (player.position.Y + player.height - 8f > projectile.position.Y + projectile.height)
				{
					flag3 = true;
				}
				if (projectile.type == 268 && projectile.frameCounter < 10)
				{
					flag4 = false;
				}
				if (projectile.type == 860 && projectile.velocity.X != 0f)
				{
					flag4 = true;
				}
				if (projectile.velocity.X != 0f)
				{
					flag4 = true;
				}
				Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
				if (projectile.velocity.Y == 0f)
				{
					if (!flag3 && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
					{
						int i3 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
						int j3 = (int)(projectile.position.Y + (projectile.height / 2)) / 16 + 1;
						if (flag1)
						{
							i3--;
						}
						if (flag2)
						{
							i3++;
						}
						WorldGen.SolidTile(i3, j3);
					}
					if (flag4)
					{
						int num154 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
						int num155 = (int)(projectile.position.Y + projectile.height) / 16;
						if (WorldGen.SolidTileAllowBottomSlope(num154, num155) || Main.tile[num154, num155].halfBrick() || Main.tile[num154, num155].slope() > 0)
						{
							try
							{
								num154 = (int)(projectile.position.X + (projectile.width / 2)) / 16;
								num155 = (int)(projectile.position.Y + (projectile.height / 2)) / 16;
								if (flag1)
								{
									num154--;
								}
								if (flag2)
								{
									num154++;
								}
								num154 += (int)projectile.velocity.X;
								if (!WorldGen.SolidTile(num154, num155 - 1) && !WorldGen.SolidTile(num154, num155 - 2))
								{
									projectile.velocity.Y = -5.1f;
								}
								else if (!WorldGen.SolidTile(num154, num155 - 2))
								{
									projectile.velocity.Y = -7.1f;
								}
								else if (WorldGen.SolidTile(num154, num155 - 5))
								{
									projectile.velocity.Y = -11.1f;
								}
								else if (WorldGen.SolidTile(num154, num155 - 4))
								{
									projectile.velocity.Y = -10.1f;
								}
								else
								{
									projectile.velocity.Y = -9.1f;
								}
							}
							catch
							{
								projectile.velocity.Y = -9.1f;
							}
						}
					}
				}
				if (projectile.velocity.X > num152)
				{
					projectile.velocity.X = num152;
				}
				if (projectile.velocity.X < 0f - num152)
				{
					projectile.velocity.X = 0f - num152;
				}
				if (projectile.velocity.X < 0f)
				{
					projectile.direction = -1;
				}
				if (projectile.velocity.X > 0f)
				{
					projectile.direction = 1;
				}
				if (projectile.velocity.X > num151 && flag2)
				{
					projectile.direction = 1;
				}
				if (projectile.velocity.X < 0f - num151 && flag1)
				{
					projectile.direction = -1;
				}
				if (projectile.direction == -1)
				{
					projectile.spriteDirection = 1;
				}
				if (projectile.direction == 1)
				{
					projectile.spriteDirection = -1;
				}
				projectile.spriteDirection = 1;
				if (player.Center.X < projectile.Center.X)
				{
					projectile.spriteDirection = -1;
				}
				if (projectile.velocity.Y > 0f)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 2)
					{
						projectile.frame++;
						if (projectile.frame >= 2)
						{
							projectile.frame = 2;
						}
						projectile.frameCounter = 0;
					}
				}
				else if (projectile.velocity.Y < 0f)
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 2)
					{
						projectile.frame++;
						if (projectile.frame >= 5)
						{
							projectile.frame = 0;
						}
						projectile.frameCounter = 0;
					}
				}
				else if (projectile.frame == 0)
				{
					projectile.frame = 0;
				}
				else if (++projectile.frameCounter > 3)
				{
					projectile.frame++;
					if (projectile.frame >= 6)
					{
						projectile.frame = 0;
					}
					projectile.frameCounter = 0;
				}
				if (projectile.wet && player.position.Y + player.height < projectile.position.Y + projectile.height && projectile.localAI[0] == 0f)
				{
					if (projectile.velocity.Y > -4f)
					{
						projectile.velocity.Y -= 0.2f;
					}
					if (projectile.velocity.Y > 0f)
					{
						projectile.velocity.Y *= 0.95f;
					}
				}
				else
				{
					projectile.velocity.Y += 0.4f;
				}
				if (projectile.velocity.Y > 10f)
				{
					projectile.velocity.Y = 10f;
				}
			}
			#endregion
		}
    }
}