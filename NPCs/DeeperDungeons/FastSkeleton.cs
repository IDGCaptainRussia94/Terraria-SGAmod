using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class FastSkeleton : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fast Skeleton");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Fritz]; //9
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 48;
            npc.damage = 21;
            npc.defense = 5;
            npc.lifeMax = 55;
            npc.value = 140f;
            npc.aiStyle = -1;
            npc.knockBackResist = 0.4f;
            aiType = NPCID.Fritz;
            animationType = NPCID.Fritz;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.buffImmune[BuffID.Confused] = false;
            banner = npc.type;
            bannerItem = mod.ItemType("FastSkeletonBanner");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, 42, 1f); //Skeleton head gore
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * -16, 0), npc.velocity, 43, 1f); //Skeleton arm gore
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, 43, 1f); //Skeleton arm gore
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, 44, 1f); //Skeleton leg gore

            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(100) < 98) //98% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone, Main.rand.Next(1, 3));
            }
            if (Main.rand.Next(65) == 0) //1.53% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
            }
            if (Main.rand.Next(250) == 0) //0.4% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);
            }
            if (Main.rand.Next(300) == 0) //0.33% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ClothierVoodooDoll);
            }
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
            }
        }
		//Copied from aiStyle 3. Fritz clone
		public override void AI()
		{
			bool flag3 = false;
			if (npc.velocity.X == 0f)
			{
				flag3 = true;
			}
			if (npc.justHit)
			{
				flag3 = false;
			}
			int num41 = 60;
			bool flag4 = false;
			bool flag5 = true;
			bool flag6 = false;
			bool flag7 = true;
			if (!flag6 && flag7)
			{
				if (npc.velocity.Y == 0f && ((npc.velocity.X > 0f && npc.direction < 0) || (npc.velocity.X < 0f && npc.direction > 0)))
				{
					flag4 = true;
				}
				if (npc.position.X == npc.oldPosition.X || npc.ai[3] >= (float)num41 || flag4)
				{
					npc.ai[3] += 1f;
				}
				else if ((double)Math.Abs(npc.velocity.X) > 0.9 && npc.ai[3] > 0f)
				{
					npc.ai[3] -= 1f;
				}
				if (npc.ai[3] > (float)(num41 * 10))
				{
					npc.ai[3] = 0f;
				}
				if (npc.justHit)
				{
					npc.ai[3] = 0f;
				}
				if (npc.ai[3] == (float)num41)
				{
					npc.netUpdate = true;
				}
			}
			if (npc.velocity.Y == 0f && (Main.player[npc.target].Center - npc.Center).Length() < 150f && Math.Abs(npc.velocity.X) > 3f && ((npc.velocity.X < 0f && npc.Center.X > Main.player[npc.target].Center.X) || (npc.velocity.X > 0f && npc.Center.X < Main.player[npc.target].Center.X)))
			{
				npc.velocity.X *= 1.75f;
				npc.velocity.Y -= 4.5f;
				if (npc.Center.Y - Main.player[npc.target].Center.Y > 20f)
				{
					npc.velocity.Y -= 0.5f;
				}
				if (npc.Center.Y - Main.player[npc.target].Center.Y > 40f)
				{
					npc.velocity.Y -= 1f;
				}
				if (npc.Center.Y - Main.player[npc.target].Center.Y > 80f)
				{
					npc.velocity.Y -= 1.5f;
				}
				if (npc.Center.Y - Main.player[npc.target].Center.Y > 100f)
				{
					npc.velocity.Y -= 1.5f;
				}
				if (Math.Abs(npc.velocity.X) > 7f)
				{
					if (npc.velocity.X < 0f)
					{
						npc.velocity.X = -7f;
					}
					else
					{
						npc.velocity.X = 7f;
					}
				}
			}
			if (npc.ai[3] < (float)num41)
			{
				npc.TargetClosest();
			}
			else if (!(npc.ai[2] > 0f) || true)
			{
				if (Main.dayTime && (double)(npc.position.Y / 16f) < Main.worldSurface && npc.timeLeft > 10)
				{
					npc.timeLeft = 10;
				}
				if (npc.velocity.X == 0f)
				{
					if (npc.velocity.Y == 0f)
					{
						npc.ai[0] += 1f;
						if (npc.ai[0] >= 2f)
						{
							npc.direction *= -1;
							npc.spriteDirection = npc.direction;
							npc.ai[0] = 0f;
						}
					}
				}
				else
				{
					npc.ai[0] = 0f;
				}
				if (npc.direction == 0)
				{
					npc.direction = 1;
				}
			}
			if (true)
			{
				float num63 = 4f;
				if (npc.velocity.X < 0f - num63 || npc.velocity.X > num63)
				{
					if (npc.velocity.Y == 0f)
					{
						npc.velocity *= 0.8f;
					}
				}
				else if (npc.velocity.X < num63 && npc.direction == 1)
				{
					npc.velocity.X += 0.07f;
					if (npc.velocity.X > num63)
					{
						npc.velocity.X = num63;
					}
				}
				else if (npc.velocity.X > 0f - num63 && npc.direction == -1)
				{
					npc.velocity.X -= 0.07f;
					if (npc.velocity.X < 0f - num63)
					{
						npc.velocity.X = 0f - num63;
					}
				}
				if (npc.velocity.Y == 0f && ((npc.direction > 0 && npc.velocity.X < 0f) || (npc.direction < 0 && npc.velocity.X > 0f)))
				{
					npc.velocity.X *= 0.9f;
				}
			}
			bool flag22 = false;
			if (npc.velocity.Y == 0f)
			{
				int num171 = (int)(npc.position.Y + (float)npc.height + 7f) / 16;
				int num172 = (int)npc.position.X / 16;
				int num173 = (int)(npc.position.X + (float)npc.width) / 16;
				for (int num174 = num172; num174 <= num173; num174++)
				{
					if (Main.tile[num174, num171] == null)
					{
						return;
					}
					if (Main.tile[num174, num171].nactive() && Main.tileSolid[Main.tile[num174, num171].type])
					{
						flag22 = true;
						break;
					}
				}
			}
			if (npc.velocity.Y >= 0f)
			{
				int num175 = 0;
				if (npc.velocity.X < 0f)
				{
					num175 = -1;
				}
				if (npc.velocity.X > 0f)
				{
					num175 = 1;
				}
				Vector2 vector34 = npc.position;
				vector34.X += npc.velocity.X;
				int num176 = (int)((vector34.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 1) * num175)) / 16f);
				int num177 = (int)((vector34.Y + (float)npc.height - 1f) / 16f);
				if (Main.tile[num176, num177] == null)
				{
					Main.tile[num176, num177] = new Tile();
				}
				if (Main.tile[num176, num177 - 1] == null)
				{
					Main.tile[num176, num177 - 1] = new Tile();
				}
				if (Main.tile[num176, num177 - 2] == null)
				{
					Main.tile[num176, num177 - 2] = new Tile();
				}
				if (Main.tile[num176, num177 - 3] == null)
				{
					Main.tile[num176, num177 - 3] = new Tile();
				}
				if (Main.tile[num176, num177 + 1] == null)
				{
					Main.tile[num176, num177 + 1] = new Tile();
				}
				if (Main.tile[num176 - num175, num177 - 3] == null)
				{
					Main.tile[num176 - num175, num177 - 3] = new Tile();
				}
				if ((float)(num176 * 16) < vector34.X + (float)npc.width && (float)(num176 * 16 + 16) > vector34.X && ((Main.tile[num176, num177].nactive() && !Main.tile[num176, num177].topSlope() && !Main.tile[num176, num177 - 1].topSlope() && Main.tileSolid[Main.tile[num176, num177].type] && !Main.tileSolidTop[Main.tile[num176, num177].type]) || (Main.tile[num176, num177 - 1].halfBrick() && Main.tile[num176, num177 - 1].nactive())) && (!Main.tile[num176, num177 - 1].nactive() || !Main.tileSolid[Main.tile[num176, num177 - 1].type] || Main.tileSolidTop[Main.tile[num176, num177 - 1].type] || (Main.tile[num176, num177 - 1].halfBrick() && (!Main.tile[num176, num177 - 4].nactive() || !Main.tileSolid[Main.tile[num176, num177 - 4].type] || Main.tileSolidTop[Main.tile[num176, num177 - 4].type]))) && (!Main.tile[num176, num177 - 2].nactive() || !Main.tileSolid[Main.tile[num176, num177 - 2].type] || Main.tileSolidTop[Main.tile[num176, num177 - 2].type]) && (!Main.tile[num176, num177 - 3].nactive() || !Main.tileSolid[Main.tile[num176, num177 - 3].type] || Main.tileSolidTop[Main.tile[num176, num177 - 3].type]) && (!Main.tile[num176 - num175, num177 - 3].nactive() || !Main.tileSolid[Main.tile[num176 - num175, num177 - 3].type]))
				{
					float num178 = num177 * 16;
					if (Main.tile[num176, num177].halfBrick())
					{
						num178 += 8f;
					}
					if (Main.tile[num176, num177 - 1].halfBrick())
					{
						num178 -= 8f;
					}
					if (num178 < vector34.Y + (float)npc.height)
					{
						float num179 = vector34.Y + (float)npc.height - num178;
						float num180 = 16.1f;
						if (num179 <= num180)
						{
							npc.gfxOffY += npc.position.Y + (float)npc.height - num178;
							npc.position.Y = num178 - (float)npc.height;
							if (num179 < 9f)
							{
								npc.stepSpeed = 1f;
							}
							else
							{
								npc.stepSpeed = 2f;
							}
						}
					}
				}
			}
			if (flag22)
			{
				int num181 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
				int num182 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
				if (Main.tile[num181, num182] == null)
				{
					Main.tile[num181, num182] = new Tile();
				}
				if (Main.tile[num181, num182 - 1] == null)
				{
					Main.tile[num181, num182 - 1] = new Tile();
				}
				if (Main.tile[num181, num182 - 2] == null)
				{
					Main.tile[num181, num182 - 2] = new Tile();
				}
				if (Main.tile[num181, num182 - 3] == null)
				{
					Main.tile[num181, num182 - 3] = new Tile();
				}
				if (Main.tile[num181, num182 + 1] == null)
				{
					Main.tile[num181, num182 + 1] = new Tile();
				}
				if (Main.tile[num181 + npc.direction, num182 - 1] == null)
				{
					Main.tile[num181 + npc.direction, num182 - 1] = new Tile();
				}
				if (Main.tile[num181 + npc.direction, num182 + 1] == null)
				{
					Main.tile[num181 + npc.direction, num182 + 1] = new Tile();
				}
				if (Main.tile[num181 - npc.direction, num182 + 1] == null)
				{
					Main.tile[num181 - npc.direction, num182 + 1] = new Tile();
				}
				Main.tile[num181, num182 + 1].halfBrick();
				if (Main.tile[num181, num182 - 1].nactive() && (TileLoader.IsClosedDoor(Main.tile[num181, num182 - 1]) || Main.tile[num181, num182 - 1].type == 388) && flag5)
				{
					npc.ai[2] += 1f;
					npc.ai[3] = 0f;
					if (npc.ai[2] >= 60f)
					{
						npc.velocity.X = 0.5f * (float)(-npc.direction);
						int num183 = 5;
						if (Main.tile[num181, num182 - 1].type == 388)
						{
							num183 = 2;
						}
						npc.ai[1] += num183;
						npc.ai[2] = 0f;
						bool flag23 = false;
						if (npc.ai[1] >= 10f)
						{
							flag23 = true;
							npc.ai[1] = 10f;
						}
						WorldGen.KillTile(num181, num182 - 1, fail: true);
						if ((Main.netMode != NetmodeID.MultiplayerClient || !flag23) && flag23 && Main.netMode != NetmodeID.MultiplayerClient)
						{
							if (TileLoader.OpenDoorID(Main.tile[num181, num182 - 1]) >= 0)
							{
								bool flag24 = WorldGen.OpenDoor(num181, num182 - 1, npc.direction);
								if (!flag24)
								{
									npc.ai[3] = num41;
									npc.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag24)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 0, num181, num182 - 1, npc.direction);
								}
							}
							if (Main.tile[num181, num182 - 1].type == 388)
							{
								bool flag25 = WorldGen.ShiftTallGate(num181, num182 - 1, closing: false);
								if (!flag25)
								{
									npc.ai[3] = num41;
									npc.netUpdate = true;
								}
								if (Main.netMode == NetmodeID.Server && flag25)
								{
									NetMessage.SendData(MessageID.ChangeDoor, -1, -1, null, 4, num181, num182 - 1);
								}
							}
						}
					}
				}
				else
				{
					int num184 = npc.spriteDirection;
					if ((npc.velocity.X < 0f && num184 == -1) || (npc.velocity.X > 0f && num184 == 1))
					{
						if (npc.height >= 32 && Main.tile[num181, num182 - 2].nactive() && Main.tileSolid[Main.tile[num181, num182 - 2].type])
						{
							if (Main.tile[num181, num182 - 3].nactive() && Main.tileSolid[Main.tile[num181, num182 - 3].type])
							{
								npc.velocity.Y = -8f;
								npc.netUpdate = true;
							}
							else
							{
								npc.velocity.Y = -7f;
								npc.netUpdate = true;
							}
						}
						else if (Main.tile[num181, num182 - 1].nactive() && Main.tileSolid[Main.tile[num181, num182 - 1].type])
						{
							npc.velocity.Y = -6f;
							npc.netUpdate = true;
						}
						else if (npc.position.Y + (float)npc.height - (float)(num182 * 16) > 20f && Main.tile[num181, num182].nactive() && !Main.tile[num181, num182].topSlope() && Main.tileSolid[Main.tile[num181, num182].type])
						{
							npc.velocity.Y = -5f;
							npc.netUpdate = true;
						}
						else if (npc.directionY < 0 && (!Main.tile[num181, num182 + 1].nactive() || !Main.tileSolid[Main.tile[num181, num182 + 1].type]) && (!Main.tile[num181 + npc.direction, num182 + 1].nactive() || !Main.tileSolid[Main.tile[num181 + npc.direction, num182 + 1].type]))
						{
							npc.velocity.Y = -8f;
							npc.velocity.X *= 1.5f;
							npc.netUpdate = true;
						}
						else if (flag5)
						{
							npc.ai[1] = 0f;
							npc.ai[2] = 0f;
						}
						if (npc.velocity.Y == 0f && flag3 && npc.ai[3] == 1f)
						{
							npc.velocity.Y = -5f;
						}
					}
				}
			}
			else if (flag5)
			{
				npc.ai[1] = 0f;
				npc.ai[2] = 0f;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient || true || !(npc.ai[3] >= (float)num41))
			{
				return;
			}
			int num185 = (int)Main.player[npc.target].position.X / 16;
			int num186 = (int)Main.player[npc.target].position.Y / 16;
			int num187 = (int)npc.position.X / 16;
			int num188 = (int)npc.position.Y / 16;
			int num189 = 20;
			int num190 = 0;
			bool flag26 = false;
			if (Math.Abs(npc.position.X - Main.player[npc.target].position.X) + Math.Abs(npc.position.Y - Main.player[npc.target].position.Y) > 2000f)
			{
				num190 = 100;
				flag26 = true;
			}
			while (!flag26 && num190 < 100)
			{
				num190++;
				int num191 = Main.rand.Next(num185 - num189, num185 + num189);
				for (int num192 = Main.rand.Next(num186 - num189, num186 + num189); num192 < num186 + num189; num192++)
				{
					if ((num192 < num186 - 4 || num192 > num186 + 4 || num191 < num185 - 4 || num191 > num185 + 4) && (num192 < num188 - 1 || num192 > num188 + 1 || num191 < num187 - 1 || num191 > num187 + 1) && Main.tile[num191, num192].nactive())
					{
						bool flag27 = true;
						if (Main.tile[num191, num192 - 1].lava())
						{
							flag27 = false;
						}
						if (flag27 && Main.tileSolid[Main.tile[num191, num192].type] && !Collision.SolidTiles(num191 - 1, num191 + 1, num192 - 4, num192 - 1))
						{
							npc.position.X = num191 * 16 - npc.width / 2;
							npc.position.Y = num192 * 16 - npc.height;
							npc.netUpdate = true;
							npc.ai[3] = -120f;
						}
					}
				}
			}
		}
	}
}