using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class AcidicSpiderling : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acidic Spiderling");
			Main.projFrames[projectile.type] = 11;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.VenomSpider); //a little buggy because its a minion and not a pet
			projectile.width = 18;
			projectile.height = 18;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			projectile.friendly = true;
			projectile.ignoreWater = false;
			projectile.tileCollide = true;
			//aiType = ProjectileID.VenomSpider;
			aiType = 0;
			//drawOriginOffsetY -= 6;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			//player.petFlagDD2Gato = false; // Relic from aiType
			//player.spiderMinion = false;
			//player.numMinions -= 1;
			return true;
		}

		/*
		 * Frames
		 * 0-3 walking on floor
		 * 4-7 walking on wall
	     * 8-10 flying
		 */

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.AcidicSpiderlingBuff>()))
			{
				projectile.timeLeft = 2;
			}



			//Taken from AI style 26
			//stills needs a lot of cleaning up. Originally was >1200 lines, now <850
			//Known bugs:
			//	doesn't teleport to the player when they are too far away.
			//	Doesn't start flying to stay near the play if the player is far away vertically (seems to happen with the spider summons too)
			//	Faces right when then player is idle and facing the left (only on floor)
			if (!Main.player[projectile.owner].active)
			{
				projectile.active = false;
				return;
			}
			bool flag1 = false;
			bool flag2 = false;
			bool flag3PlayerAboveProjectile = false;
			bool flag4ProjectileInSolidTile = false;
			int num;
			num = 10;
			int num2 = 40 * (projectile.minionPos + 1) * Main.player[projectile.owner].direction;
			if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) < projectile.position.X + (float)(projectile.width / 2) - (float)num + (float)num2)
			{
				flag1 = true;
			}
			else if (Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) > projectile.position.X + (float)(projectile.width / 2) + (float)num + (float)num2)
			{
				flag2 = true;
			}
			if (projectile.ai[0] != 0f)
			{
				float num40 = 0.2f;
				int num41 = 200;
				projectile.tileCollide = false;
				Vector2 vector7 = new Vector2(projectile.position.X + (float)projectile.width * 0.5f, projectile.position.Y + (float)projectile.height * 0.5f);
				float num42 = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - vector7.X;
				num42 -= (float)(40 * Main.player[projectile.owner].direction);
				bool flag6ChaseNPC = false;
				int num44 = -1;
				//For chasing other NPCs to attack
				/*for (int j = 0; j < 200; j++)
				{
					if (!Main.npc[j].CanBeChasedBy(projectile))
					{
						continue;
					}
					float num45 = Main.npc[j].position.X + (float)(Main.npc[j].width / 2);
					float num46 = Main.npc[j].position.Y + (float)(Main.npc[j].height / 2);
					if (Math.Abs(Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - num45) + Math.Abs(Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - num46) < num43)
					{
						if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[j].position, Main.npc[j].width, Main.npc[j].height))
						{
							num44 = j;
						}
						flag6ChaseNPC = true;
						break;
					}
				}*/
				if (!flag6ChaseNPC)
				{
					num42 -= (float)(40 * projectile.minionPos * Main.player[projectile.owner].direction);
				}
				if (flag6ChaseNPC && num44 >= 0)
				{
					projectile.ai[0] = 0f;
				}
				float num47 = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - vector7.Y;
				float num48 = (float)Math.Sqrt(num42 * num42 + num47 * num47);
				float num49 = 10f;
				if (num48 < (float)num41 && Main.player[projectile.owner].velocity.Y == 0f && projectile.position.Y + (float)projectile.height <= Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height && !Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.ai[0] = 0f;
					if (projectile.velocity.Y < -6f)
					{
						projectile.velocity.Y = -6f;
					}
				}
				if (num48 < 60f)
				{
					num42 = projectile.velocity.X;
					num47 = projectile.velocity.Y;
				}
				else
				{
					num48 = num49 / num48;
					num42 *= num48;
					num47 *= num48;
				}
				if (projectile.velocity.X < num42)
				{
					projectile.velocity.X += num40;
					if (projectile.velocity.X < 0f)
					{
						projectile.velocity.X += num40 * 1.5f;
					}
				}
				if (projectile.velocity.X > num42)
				{
					projectile.velocity.X -= num40;
					if (projectile.velocity.X > 0f)
					{
						projectile.velocity.X -= num40 * 1.5f;
					}
				}
				if (projectile.velocity.Y < num47)
				{
					projectile.velocity.Y += num40;
					if (projectile.velocity.Y < 0f)
					{
						projectile.velocity.Y += num40 * 1.5f;
					}
				}
				if (projectile.velocity.Y > num47)
				{
					projectile.velocity.Y -= num40;
					if (projectile.velocity.Y > 0f)
					{
						projectile.velocity.Y -= num40 * 1.5f;
					}
				}
				// set sprite direction
				if ((double)projectile.velocity.X > 0.5)
				{
					projectile.spriteDirection = -1;
				}
				else if ((double)projectile.velocity.X < -0.5)
				{
					projectile.spriteDirection = 1;
				}
				int num51 = (int)(projectile.Center.X / 16f);
				int num52 = (int)(projectile.Center.Y / 16f);
				if (Main.tile[num51, num52] != null && Main.tile[num51, num52].wall > 0)
				{
					projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
					projectile.frameCounter += (int)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y));
					if (projectile.frameCounter > 5)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 7)
					{
						projectile.frame = 4;
					}
					if (projectile.frame < 4)
					{
						projectile.frame = 7;
					}
				}
				else
				{
					projectile.frameCounter++;
					if (projectile.frameCounter > 2)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame < 8 || projectile.frame > 10)
					{
						projectile.frame = 8;
					}
					projectile.rotation = projectile.velocity.X * 0.1f;
				}
				//The Spiders don't usually have this dust, but I think its cool.

				int num55 = Dust.NewDust(new Vector2(projectile.position.X + (float)(projectile.width / 2) - 4f, projectile.position.Y + (float)(projectile.height / 2) - 4f) - projectile.velocity, 8, 8, 163, (0f - projectile.velocity.X) * 0.5f, projectile.velocity.Y * 0.5f, 50, default(Color), 1.7f); //DustID.PoionStaff
				Main.dust[num55].velocity.X = Main.dust[num55].velocity.X * 0.2f;
				Main.dust[num55].velocity.Y = Main.dust[num55].velocity.Y * 0.2f;
				Main.dust[num55].noGravity = true;

				return;
			}
			bool flag7 = false;
			Vector2 vector9 = Vector2.Zero;
			bool flag8IsOnWall = false;

			//The position behind the player. "40 * projectile.minionPos;" in vanilla
			float num80 = 60;
			int num81 = 60;
			projectile.localAI[0] -= 1f;
			if (projectile.localAI[0] < 0f)
			{
				projectile.localAI[0] = 0f;
			}
			if (projectile.ai[1] > 0f)
			{
				projectile.ai[1] -= 1f;
			}
			else
			{
				float num82 = projectile.position.X;
				float num83 = projectile.position.Y;
				float num84 = 100000f;
				float num85 = num84;
				int num86 = -1;
				//For chasing other NPCs to attack
				/*NPC ownerMinionAttackTargetNPC2 = projectile.OwnerMinionAttackTargetNPC;
				if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(projectile))
				{
					float x = ownerMinionAttackTargetNPC2.Center.X;
					float y = ownerMinionAttackTargetNPC2.Center.Y;
					float num87 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - x) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - y);
					if (num87 < num84)
					{
						if (num86 == -1 && num87 <= num85)
						{
							num85 = num87;
							num82 = x;
							num83 = y;
						}
						if (Collision.CanHit(projectile.position, projectile.width, projectile.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
						{
							num84 = num87;
							num82 = x;
							num83 = y;
							num86 = ownerMinionAttackTargetNPC2.whoAmI;
						}
					}
				}
				if (num86 == -1)
				{
					for (int m = 0; m < 200; m++)
					{
						if (!Main.npc[m].CanBeChasedBy(projectile))
						{
							continue;
						}
						float num88 = Main.npc[m].position.X + (float)(Main.npc[m].width / 2);
						float num89 = Main.npc[m].position.Y + (float)(Main.npc[m].height / 2);
						float num90 = Math.Abs(projectile.position.X + (float)(projectile.width / 2) - num88) + Math.Abs(projectile.position.Y + (float)(projectile.height / 2) - num89);
						if (num90 < num84)
						{
							if (num86 == -1 && num90 <= num85)
							{
								num85 = num90;
								num82 = num88;
								num83 = num89;
							}
							if (Collision.CanHit(projectile.position, projectile.width, projectile.height, Main.npc[m].position, Main.npc[m].width, Main.npc[m].height))
							{
								num84 = num90;
								num82 = num88;
								num83 = num89;
								num86 = m;
							}
						}
					}
				}*/
				if (!Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.tileCollide = true;
				}
				if (num86 == -1 && num85 < num84)
				{
					num84 = num85;
				}
				else if (num86 >= 0)
				{
					flag7 = true;
					vector9 = new Vector2(num82, num83) - projectile.Center;
					if (Main.npc[num86].position.Y > projectile.position.Y + (float)projectile.height)
					{
						int num91 = (int)(projectile.Center.X / 16f);
						int num92 = (int)((projectile.position.Y + (float)projectile.height + 1f) / 16f);
						if (Main.tile[num91, num92] != null && Main.tile[num91, num92].active() && TileID.Sets.Platforms[Main.tile[num91, num92].type])
						{
							projectile.tileCollide = false;
						}
					}
					Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					Rectangle value = new Rectangle((int)Main.npc[num86].position.X, (int)Main.npc[num86].position.Y, Main.npc[num86].width, Main.npc[num86].height);
					int num93 = 10;
					value.X -= num93;
					value.Y -= num93;
					value.Width += num93 * 2;
					value.Height += num93 * 2;
					if (rectangle.Intersects(value))
					{
						flag8IsOnWall = true;
						Vector2 vector10 = Main.npc[num86].Center - projectile.Center;
						if (projectile.velocity.Y > 0f && vector10.Y < 0f)
						{
							projectile.velocity.Y *= 0.5f;
						}
						if (projectile.velocity.Y < 0f && vector10.Y > 0f)
						{
							projectile.velocity.Y *= 0.5f;
						}
						if (projectile.velocity.X > 0f && vector10.X < 0f)
						{
							projectile.velocity.X *= 0.5f;
						}
						if (projectile.velocity.X < 0f && vector10.X > 0f)
						{
							projectile.velocity.X *= 0.5f;
						}
						if (vector10.Length() > 14f)
						{
							vector10.Normalize();
							vector10 *= 14f;
						}
						projectile.rotation = (projectile.rotation * 5f + vector10.ToRotation() + (float)Math.PI / 2f) / 6f;
						projectile.velocity = (projectile.velocity * 9f + vector10) / 10f;
						for (int n = 0; n < 1000; n++)
						{
							if (projectile.whoAmI != n && projectile.owner == Main.projectile[n].owner && (Main.projectile[n].type >= 390 && Main.projectile[n].type <= 392 || true) && (Main.projectile[n].Center - projectile.Center).Length() < 15f)
							{
								float num94 = 0.5f;
								if (projectile.Center.Y > Main.projectile[n].Center.Y)
								{
									Main.projectile[n].velocity.Y -= num94;
									projectile.velocity.Y += num94;
								}
								else
								{
									Main.projectile[n].velocity.Y += num94;
									projectile.velocity.Y -= num94;
								}
								if (projectile.Center.X > Main.projectile[n].Center.X)
								{
									projectile.velocity.X += num94;
									Main.projectile[n].velocity.X -= num94;
								}
								else
								{
									projectile.velocity.X -= num94;
									Main.projectile[n].velocity.Y += num94;
								}
							}
						}
					}
				}
				float num95;
				num95 = 500f;
				if ((double)projectile.position.Y > Main.worldSurface * 16.0)
				{
					num95 = 250f;
				}
				if (num84 < num95 + num80 && num86 == -1)
				{
					float num96 = num82 - (projectile.position.X + (float)(projectile.width / 2));
					if (num96 < -5f)
					{
						flag1 = true;
						flag2 = false;
					}
					else if (num96 > 5f)
					{
						flag2 = true;
						flag1 = false;
					}
				}
				bool flag9 = false;
				if (projectile.localAI[1] > 0f)
				{
					flag9 = true;
					projectile.localAI[1] -= 1f;
				}
				if (num86 >= 0 && num84 < 800f + num80)
				{
					projectile.friendly = true;
					projectile.localAI[0] = num81;
					float num97 = num82 - (projectile.position.X + (float)(projectile.width / 2));
					if (num97 < -10f)
					{
						flag1 = true;
						flag2 = false;
					}
					else if (num97 > 10f)
					{
						flag2 = true;
						flag1 = false;
					}
					if (num83 < projectile.Center.Y - 100f && num97 > -50f && num97 < 50f && projectile.velocity.Y == 0f)
					{
						float num98 = Math.Abs(num83 - projectile.Center.Y);
						if (num98 < 120f)
						{
							projectile.velocity.Y = -10f;
						}
						else if (num98 < 210f)
						{
							projectile.velocity.Y = -13f;
						}
						else if (num98 < 270f)
						{
							projectile.velocity.Y = -15f;
						}
						else if (num98 < 310f)
						{
							projectile.velocity.Y = -17f;
						}
						else if (num98 < 380f)
						{
							projectile.velocity.Y = -18f;
						}
					}
					if (flag9)
					{
						projectile.friendly = false;
						if (projectile.velocity.X < 0f)
						{
							flag1 = true;
						}
						else if (projectile.velocity.X > 0f)
						{
							flag2 = true;
						}
					}
				}
				else
				{
					projectile.friendly = false;
				}
			}
			if (projectile.ai[1] != 0f)
			{
				flag1 = false;
				flag2 = false;
			}
			else if (true)
			{
				int num99 = (int)(projectile.Center.X / 16f);
				int num100 = (int)(projectile.Center.Y / 16f);
				if (Main.tile[num99, num100] != null && Main.tile[num99, num100].wall > 0)
				{
					flag1 = (flag2 = false);
				}
			}
			if (!flag8IsOnWall)
			{
				projectile.rotation = 0f;
			}
			float num101;
			float num102;
			num102 = 6f;
			num101 = 0.2f;
			if (num102 < Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y))
			{
				num102 = Math.Abs(Main.player[projectile.owner].velocity.X) + Math.Abs(Main.player[projectile.owner].velocity.Y);
				num101 = 0.3f;
			}
			num101 *= 2f;
			if (flag1)
			{
				if ((double)projectile.velocity.X > -3.5)
				{
					projectile.velocity.X -= num101;
				}
				else
				{
					projectile.velocity.X -= num101 * 0.25f;
				}
			}
			else if (flag2)
			{
				if ((double)projectile.velocity.X < 3.5)
				{
					projectile.velocity.X += num101;
				}
				else
				{
					projectile.velocity.X += num101 * 0.25f;
				}
			}
			else
			{
				projectile.velocity.X *= 0.9f;
				if (projectile.velocity.X >= 0f - num101 && projectile.velocity.X <= num101)
				{
					projectile.velocity.X = 0f;
				}
			}
			if (flag1 || flag2)
			{
				int num103 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
				int j2 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
				if (flag1)
				{
					num103--;
				}
				if (flag2)
				{
					num103++;
				}
				num103 += (int)projectile.velocity.X;
				if (WorldGen.SolidTile(num103, j2))
				{
					flag4ProjectileInSolidTile = true;
				}
			}
			if (Main.player[projectile.owner].position.Y + (float)Main.player[projectile.owner].height - 8f > projectile.position.Y + (float)projectile.height)
			{
				flag3PlayerAboveProjectile = true;
			}
			Collision.StepUp(ref projectile.position, ref projectile.velocity, projectile.width, projectile.height, ref projectile.stepSpeed, ref projectile.gfxOffY);
			if (projectile.velocity.Y == 0f)
			{
				if (!flag3PlayerAboveProjectile && (projectile.velocity.X < 0f || projectile.velocity.X > 0f))
				{
					int num104 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
					int j3 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16 + 1;
					if (flag1)
					{
						num104--;
					}
					if (flag2)
					{
						num104++;
					}
					WorldGen.SolidTile(num104, j3);
				}
				if (flag4ProjectileInSolidTile)
				{
					int num105 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
					int num106 = (int)(projectile.position.Y + (float)projectile.height) / 16 + 1;
					if (WorldGen.SolidTile(num105, num106) || Main.tile[num105, num106].halfBrick() || Main.tile[num105, num106].slope() > 0)
					{
						try
						{
							num105 = (int)(projectile.position.X + (float)(projectile.width / 2)) / 16;
							num106 = (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16;
							if (flag1)
							{
								num105--;
							}
							if (flag2)
							{
								num105++;
							}
							num105 += (int)projectile.velocity.X;
							if (!WorldGen.SolidTile(num105, num106 - 1) && !WorldGen.SolidTile(num105, num106 - 2))
							{
								projectile.velocity.Y = -5.1f;
							}
							else if (!WorldGen.SolidTile(num105, num106 - 2))
							{
								projectile.velocity.Y = -7.1f;
							}
							else if (WorldGen.SolidTile(num105, num106 - 5))
							{
								projectile.velocity.Y = -11.1f;
							}
							else if (WorldGen.SolidTile(num105, num106 - 4))
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
			if (projectile.velocity.X > num102)
			{
				projectile.velocity.X = num102;
			}
			if (projectile.velocity.X < 0f - num102)
			{
				projectile.velocity.X = 0f - num102;
			}
			if (projectile.velocity.X < 0f)
			{
				projectile.direction = -1;
			}
			if (projectile.velocity.X > 0f)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X > num101 && flag2)
			{
				projectile.direction = 1;
			}
			if (projectile.velocity.X < 0f - num101 && flag1)
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
			int num118 = (int)(projectile.Center.X / 16f);
			int num119 = (int)(projectile.Center.Y / 16f);
			if (Main.tile[num118, num119] != null && Main.tile[num118, num119].wall > 0)
			{
				projectile.position.Y += projectile.height;
				projectile.height = 34;
				projectile.position.Y -= projectile.height;
				projectile.position.X += projectile.width / 2;
				projectile.width = 34;
				projectile.position.X -= projectile.width / 2;
				float num120 = 9f;
				float num121 = 40 * (projectile.minionPos + 1);
				Vector2 vector12 = Main.player[projectile.owner].Center - projectile.Center;
				if (flag7)
				{
					vector12 = vector9;
					num121 = 10f;
				}
				else if (!Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
				{
					projectile.ai[0] = 1f;
				}
				if (vector12.Length() < num121)
				{
					projectile.velocity *= 0.9f;
					if ((double)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y)) < 0.1)
					{
						projectile.velocity *= 0f;
					}
				}
				else if (vector12.Length() < 800f || !flag7)
				{
					projectile.velocity = (projectile.velocity * 9f + Vector2.Normalize(vector12) * num120) / 10f;
				}
				if (vector12.Length() >= num121)
				{
					projectile.spriteDirection = projectile.direction;
					projectile.rotation = projectile.velocity.ToRotation() + (float)Math.PI / 2f;
				}
				else
				{
					projectile.rotation = vector12.ToRotation() + (float)Math.PI / 2f;
				}
				projectile.frameCounter += (int)(Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y));
				if (projectile.frameCounter > 5)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
				}
				if (projectile.frame > 7)
				{
					projectile.frame = 4;
				}
				if (projectile.frame < 4)
				{
					projectile.frame = 7;
				}
				return;
			}
			if (!flag8IsOnWall)
			{
				projectile.rotation = 0f;
			}
			if (projectile.direction == -1)
			{
				projectile.spriteDirection = 1;
			}
			if (projectile.direction == 1)
			{
				projectile.spriteDirection = -1;
			}
			projectile.position.Y += projectile.height;
			projectile.height = 30;
			projectile.position.Y -= projectile.height;
			projectile.position.X += projectile.width / 2;
			projectile.width = 30;
			projectile.position.X -= projectile.width / 2;
			if (!flag7 && !Collision.CanHitLine(projectile.Center, 1, 1, Main.player[projectile.owner].Center, 1, 1))
			{
				projectile.ai[0] = 1f;
			}
			if (!flag8IsOnWall && projectile.frame >= 4 && projectile.frame <= 7)
			{
				Vector2 vector13 = Main.player[projectile.owner].Center - projectile.Center;
				if (flag7)
				{
					vector13 = vector9;
				}
				float num122 = 0f - vector13.Y;
				if (!(vector13.Y > 0f))
				{
					if (num122 < 120f)
					{
						projectile.velocity.Y = -10f;
					}
					else if (num122 < 210f)
					{
						projectile.velocity.Y = -13f;
					}
					else if (num122 < 270f)
					{
						projectile.velocity.Y = -15f;
					}
					else if (num122 < 310f)
					{
						projectile.velocity.Y = -17f;
					}
					else if (num122 < 380f)
					{
						projectile.velocity.Y = -18f;
					}
				}
			}
			if (flag8IsOnWall)
			{
				projectile.frameCounter++;
				if (projectile.frameCounter > 3)
				{
					projectile.frame++;
					projectile.frameCounter = 0;
				}
				if (projectile.frame >= 8)
				{
					projectile.frame = 4;
				}
				if (projectile.frame <= 3)
				{
					projectile.frame = 7;
				}
			}
			else if (projectile.velocity.Y >= 0f && (double)projectile.velocity.Y <= 0.8)
			{
				if (projectile.velocity.X == 0f)
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
				else if ((double)projectile.velocity.X < -0.8 || (double)projectile.velocity.X > 0.8)
				{
					projectile.frameCounter += (int)Math.Abs(projectile.velocity.X);
					projectile.frameCounter++;
					if (projectile.frameCounter > 5)
					{
						projectile.frame++;
						projectile.frameCounter = 0;
					}
					if (projectile.frame > 2)
					{
						projectile.frame = 0;
					}
				}
				else
				{
					projectile.frame = 0;
					projectile.frameCounter = 0;
				}
			}
			else
			{
				projectile.frameCounter = 0;
				projectile.frame = 3;
			}
			projectile.velocity.Y += 0.4f;
			if (projectile.velocity.Y > 10f)
			{
				projectile.velocity.Y = 10f;
			}
		}
	}
}