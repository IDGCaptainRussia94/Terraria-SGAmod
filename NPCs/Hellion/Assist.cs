using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using SGAmod.NPCs.TownNPCs;
using Terraria.Localization;
using Idglibrary;

namespace SGAmod.NPCs.Hellion
{
    class Assist
    {

		private static int spawnRangeX = (int)((double)(NPC.sWidth / 16) * 0.7);

		private static int spawnRangeY = (int)((double)(NPC.sHeight / 16) * 0.7);

		private static int spawnSpaceX = 3;

		private static int spawnSpaceY = 3;


		public static void SpawnOnPlayerButNoTextAndReturnValue(int plr, int Type,out int npc)
		{
			npc = -1;
			if (Main.netMode == 1)
			{
				return;
			}
			if (Type == 262 && NPC.AnyNPCs(262))
			{
				return;
			}
			if (Type == 245)
			{
				if (NPC.AnyNPCs(245))
				{
					return;
				}
				try
				{
					int num = (int)Main.player[plr].Center.X / 16;
					int num2 = (int)Main.player[plr].Center.Y / 16;
					int num3 = 0;
					int num4 = 0;
					for (int i = num - 20; i < num + 20; i++)
					{
						for (int j = num2 - 20; j < num2 + 20; j++)
						{
							if (Main.tile[i, j].active() && Main.tile[i, j].type == 237 && Main.tile[i, j].frameX == 18 && Main.tile[i, j].frameY == 0)
							{
								num3 = i;
								num4 = j;
							}
						}
					}
					if (num3 > 0 && num4 > 0)
					{
						int num5 = num4 - 15;
						int num6 = num4 - 15;
						for (int k = num4; k > num4 - 100; k--)
						{
							if (WorldGen.SolidTile(num3, k))
							{
								num5 = k;
								break;
							}
						}
						for (int l = num4; l < num4 + 100; l++)
						{
							if (WorldGen.SolidTile(num3, l))
							{
								num6 = l;
								break;
							}
						}
						num4 = (num5 + num5 + num6) / 3;
						int num7 = NPC.NewNPC(num3 * 16 + 8, num4 * 16, 245, 100, 0f, 0f, 0f, 0f, 255);
						npc = num7;
						Main.npc[num7].target = plr;
						string typeName = Main.npc[num7].TypeName;
					}
				}
				catch
				{
				}
				return;
			}
			else if (Type == 370)
			{
				Player player = Main.player[plr];
				if (!player.active || player.dead)
				{
					return;
				}
				int m = 0;
				while (m < 1000)
				{
					Projectile projectile = Main.projectile[m];
					if (projectile.active && projectile.bobber && projectile.owner == plr)
					{
						int num8 = NPC.NewNPC((int)projectile.Center.X, (int)projectile.Center.Y + 100, 370, 0, 0f, 0f, 0f, 0f, 255);
						string typeName2 = Main.npc[num8].TypeName;
						if (Main.netMode == 0)
						{
							Main.NewText(Language.GetTextValue("Announcement.HasAwoken", typeName2), 175, 75, 255, false);
							return;
						}
						if (Main.netMode == 2)
						{
							NetMessage.BroadcastChatMessage(NetworkText.FromKey("Announcement.HasAwoken", new object[]
							{
								Main.npc[num8].GetTypeNetName()
							}), new Color(175, 75, 255), -1);
							return;
						}
						break;
					}
					else
					{
						m++;
					}
				}
				return;
			}
			else
			{
				if (Type != 398)
				{
					bool flag = false;
					int num9 = 0;
					int num10 = 0;
					int num11 = (int)(Main.player[plr].position.X / 16f) - Assist.spawnRangeX * 2;
					int num12 = (int)(Main.player[plr].position.X / 16f) + Assist.spawnRangeX * 2;
					int num13 = (int)(Main.player[plr].position.Y / 16f) - Assist.spawnRangeY * 2;
					int num14 = (int)(Main.player[plr].position.Y / 16f) + Assist.spawnRangeY * 2;
					int num15 = (int)(Main.player[plr].position.X / 16f) - NPC.safeRangeX;
					int num16 = (int)(Main.player[plr].position.X / 16f) + NPC.safeRangeX;
					int num17 = (int)(Main.player[plr].position.Y / 16f) - NPC.safeRangeY;
					int num18 = (int)(Main.player[plr].position.Y / 16f) + NPC.safeRangeY;
					if (num11 < 0)
					{
						num11 = 0;
					}
					if (num12 > Main.maxTilesX)
					{
						num12 = Main.maxTilesX;
					}
					if (num13 < 0)
					{
						num13 = 0;
					}
					if (num14 > Main.maxTilesY)
					{
						num14 = Main.maxTilesY;
					}
					for (int n = 0; n < 1000; n++)
					{
						int num19 = 0;
						while (num19 < 100)
						{
							int num20 = Main.rand.Next(num11, num12);
							int num21 = Main.rand.Next(num13, num14);
							if (Main.tile[num20, num21].nactive() && Main.tileSolid[(int)Main.tile[num20, num21].type])
							{
								goto IL_730;
							}
							if ((!Main.wallHouse[(int)Main.tile[num20, num21].wall] || n >= 999) && (Type != 50 || n >= 500 || Main.tile[num21, num21].wall <= 0))
							{
								int num22 = num21;
								while (num22 < Main.maxTilesY)
								{
									if (Main.tile[num20, num22].nactive() && Main.tileSolid[(int)Main.tile[num20, num22].type])
									{
										if (num20 < num15 || num20 > num16 || num22 < num17 || num22 > num18 || n == 999)
										{
											ushort num23 = Main.tile[num20, num22].type;
											num9 = num20;
											num10 = num22;
											flag = true;
											break;
										}
										break;
									}
									else
									{
										num22++;
									}
								}
								if (flag && Type == 50 && n < 900)
								{
									int num24 = 20;
									if (!Collision.CanHit(new Vector2((float)num9, (float)(num10 - 1)) * 16f, 16, 16, new Vector2((float)num9, (float)(num10 - 1 - num24)) * 16f, 16, 16) || !Collision.CanHit(new Vector2((float)num9, (float)(num10 - 1 - num24)) * 16f, 16, 16, Main.player[plr].Center, 0, 0))
									{
										num9 = 0;
										num10 = 0;
										flag = false;
									}
								}
								if (!flag || n >= 999)
								{
									goto IL_730;
								}
								int num25 = num9 - Assist.spawnSpaceX / 2;
								int num26 = num9 + Assist.spawnSpaceX / 2;
								int num27 = num10 - Assist.spawnSpaceY;
								int num28 = num10;
								if (num25 < 0)
								{
									flag = false;
								}
								if (num26 > Main.maxTilesX)
								{
									flag = false;
								}
								if (num27 < 0)
								{
									flag = false;
								}
								if (num28 > Main.maxTilesY)
								{
									flag = false;
								}
								if (flag)
								{
									for (int num29 = num25; num29 < num26; num29++)
									{
										for (int num30 = num27; num30 < num28; num30++)
										{
											if (Main.tile[num29, num30].nactive() && Main.tileSolid[(int)Main.tile[num29, num30].type])
											{
												flag = false;
												break;
											}
										}
									}
									goto IL_730;
								}
								goto IL_730;
							}
						IL_728:
							num19++;
							continue;
						IL_730:
							if (!flag && !flag)
							{
								goto IL_728;
							}
							break;
						}
						if (flag && n < 999)
						{
							Rectangle rectangle = new Rectangle(num9 * 16, num10 * 16, 16, 16);
							for (int num31 = 0; num31 < 255; num31++)
							{
								if (Main.player[num31].active)
								{
									Rectangle rectangle2 = new Rectangle((int)(Main.player[num31].position.X + (float)(Main.player[num31].width / 2) - (float)(NPC.sWidth / 2) - (float)NPC.safeRangeX), (int)(Main.player[num31].position.Y + (float)(Main.player[num31].height / 2) - (float)(NPC.sHeight / 2) - (float)NPC.safeRangeY), NPC.sWidth + NPC.safeRangeX * 2, NPC.sHeight + NPC.safeRangeY * 2);
									if (rectangle.Intersects(rectangle2))
									{
										flag = false;
									}
								}
							}
						}
						if (flag)
						{
							break;
						}
					}
					if (flag)
					{
						int num32 = NPC.NewNPC(num9 * 16 + 8, num10 * 16, Type, 1, 0f, 0f, 0f, 0f, 255);
						npc = num32;
						if (num32 == 200)
						{
							return;
						}
						Main.npc[num32].target = plr;
						Main.npc[num32].timeLeft *= 20;
						string typeName3 = Main.npc[num32].TypeName;
						if (Main.netMode == 2 && num32 < 200)
						{
							NetMessage.SendData(23, -1, -1, null, num32, 0f, 0f, 0f, 0, 0, 0);
						}
						if (Type == 125)
						{
							if (Main.netMode == 0)
							{
								Main.NewText(Lang.misc[48].Value, 175, 75, 255, false);
								return;
							}
							if (Main.netMode == 2)
							{
								NetMessage.BroadcastChatMessage(Lang.misc[48].ToNetworkText(), new Color(175, 75, 255), -1);
								return;
							}
						}
						else if (Type != 82 && Type != 126 && Type != 50 && Type != 398 && Type != 551)
						{

						}
					}
					return;
				}
				Player player2 = Main.player[plr];
				npc = NPC.NewNPC((int)player2.Center.X, (int)player2.Center.Y - 150, Type, 0, 0f, 0f, 0f, 0f, 255);
				return;
			}
		}



	}
}
