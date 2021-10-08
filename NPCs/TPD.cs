using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Idglibrary;

namespace SGAmod.NPCs
{
	[AutoloadBossHead]
	public class TPD : ModNPC, ISGABoss
	{
		public string Trophy() => "TwinPrimeDestroyersTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;
		int aistate = 0;
		int facing = 0;
		int[] bosses = { -1, -1, -1, -1, -1, -1 };
		int[] bossattach = { -1, -1, -1, -1, -1, -1, -1 };
		int[] bossactive = { 1, 1, 1, 1, 1, 1, -1 };
		int rotatationsensation = 0;
		int shooting = 0;
		int phase = 0;
		int normhp = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Twin Prime Destroyers");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.width = 96;
			npc.height = 96;
			npc.damage = 0;
			npc.defense = 10;
			npc.lifeMax = 30000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 0;
			npc.boss = true;
			//aiType = NPCID.BlueSlime;
			//animationType = NPCID.BlueSlime;
			npc.noTileCollide = false;
			npc.noGravity = false;
			music = MusicID.Boss3;
			npc.noTileCollide = true;
			npc.noGravity = true;
			bossBag = mod.ItemType("SPinkyBag");
			npc.value = Item.buyPrice(0, 60, 0, 0);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override void NPCLoot()
		{
			if (Main.netMode != NetmodeID.SinglePlayer)
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarMetalMold"));

			if (Main.expertMode)
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TPDCPU"));

			List<int> types = new List<int>();
			types.Insert(types.Count, ItemID.Ectoplasm);
			types.Insert(types.Count, ItemID.ShroomiteBar); types.Insert(types.Count, ItemID.ChlorophyteBar); types.Insert(types.Count, ItemID.SpectreBar);

			SGAUtils.DropFixedItemQuanity(types.ToArray(), Main.expertMode ? 100 : 50, npc.Center);

			/*
			for (int f = 0; f < (Main.expertMode ? 100 : 50); f = f + 1)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0, types.Count)]);
			}*/

			Achivements.SGAAchivements.UnlockAchivement("TPD", Main.LocalPlayer);
			if (!SGAWorld.downedTPD)
			{
				SGAWorld.downedTPD = true;
				//SgaLib.Chat("You have regained the knowledge to craft a furnace!",150, 150, 70);
			}
		}
		//end of function

		public int owner
		{
			get
			{
				return (int)npc.ai[0];
			}
			set
			{
				npc.ai[0] = value;
			}
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}
		public override void AI()
		{
			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || Main.dayTime)
				{
					float speed = ((-10f));
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
					npc.active = false;
				}

			}
			else
			{
				npc.netUpdate = true;
				npc.timeLeft = 99999;
				if (npc.ai[0] == 0)
					npc.position = P.position + new Vector2(0, 500f);
				npc.ai[0] = npc.ai[0] + 1;
				if (npc.ai[0] == 2)
				{
					normhp = npc.life;
					bosses[0] = NPC.NewNPC((int)npc.Center.X + 900, (int)npc.Center.Y - 32, NPCID.Spazmatism);
					Main.npc[bosses[0]].boss = false;
					Main.npc[bosses[0]].timeLeft = 99999;
					bossattach[0] = -1;
					bosses[1] = NPC.NewNPC((int)npc.Center.X + 900, (int)npc.Center.Y - 32, NPCID.SkeletronPrime);
					Main.npc[bosses[1]].boss = false;
					Main.npc[bosses[1]].timeLeft = 99999;
					Main.npc[bosses[1]].defDefense = 5000;
					Main.npc[bosses[1]].defense = 5000;
					bossattach[1] = bosses[0];
					bosses[2] = NPC.NewNPC((int)npc.Center.X - 900, (int)npc.Center.Y - 32, NPCID.Retinazer);
					Main.npc[bosses[2]].boss = false;
					Main.npc[bosses[2]].timeLeft = 99999;
					bossattach[2] = -1;
					bosses[3] = NPC.NewNPC((int)npc.Center.X - 900, (int)npc.Center.Y - 32, NPCID.SkeletronPrime);
					Main.npc[bosses[3]].boss = false;
					Main.npc[bosses[3]].timeLeft = 99999;
					Main.npc[bosses[3]].defDefense = 5000;
					Main.npc[bosses[3]].defense = 5000;
					bossattach[3] = bosses[2];
					bosses[4] = NPC.NewNPC((int)npc.Center.X + 900, (int)npc.Center.Y - 32, NPCID.TheDestroyer);
					Main.npc[bosses[4]].boss = false;
					Main.npc[bosses[4]].timeLeft = 99999;
					//Main.npc[bosses[4]].active=false;
					bossattach[4] = bosses[1];
					bosses[5] = NPC.NewNPC((int)npc.Center.X - 900, (int)npc.Center.Y - 32, NPCID.TheDestroyer);
					Main.npc[bosses[5]].boss = false;
					Main.npc[bosses[5]].timeLeft = 99999;
					bossattach[5] = bosses[3];
					//Main.npc[bosses[5]].active=false;



					npc.life = npc.life + Main.npc[bosses[0]].life;
					npc.life = npc.life + Main.npc[bosses[1]].life;
					npc.life = npc.life + Main.npc[bosses[2]].life;
					npc.life = npc.life + Main.npc[bosses[3]].life;
					npc.life = npc.life + Main.npc[bosses[4]].life;
					npc.life = npc.life + Main.npc[bosses[5]].life;

					npc.lifeMax = npc.life;
				}
				if (npc.ai[0] > 2)
				{

					if (npc.ai[0] == 3)
					{

						for (int i = 0; i <= 5; i++)
						{
							NPC thisboss = Main.npc[bosses[i]];
							if (thisboss == null || thisboss.active == false)
							{
								npc.Center = P.Center - new Vector2(0, 128);
								npc.active = false;
								Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Mechacluskerf"));

								for (int z = 0; z <= Main.maxNPCs; z++)
								{
									NPC zz = Main.npc[z];
									if (zz.type == NPCID.SkeletronHand ||
										zz.type == NPCID.SkeletronHead ||
										zz.type == NPCID.SkeletronPrime ||
										zz.type == NPCID.Retinazer ||
										zz.type == NPCID.Spazmatism ||
										zz.type == NPCID.TheDestroyer ||
										zz.type == NPCID.TheDestroyerBody ||
										zz.type == NPCID.TheDestroyerTail)
									{
										zz.active = false;
									}
								}
								Idglib.Chat("Boss failed to properly spawn, boss item has been given back", 255, 255, 255);
								break;

							}
						}
					}

					if (phase < 60)
					{
						npc.life = normhp;
					}
					Vector2 averagevec = npc.Center;
					int divider = 1;

					for (int i = 0; i <= 5; i++)
					{
						NPC thisboss = Main.npc[bosses[i]];
						if (bosses[i] > -1 || npc.ai[0] < 30)
						{
							thisboss.boss = false;
							if (thisboss.life < 1 && npc.ai[0] > 30)
							{
								bossactive[i] = 0;
								if (i == 0)
								{
									Main.npc[bosses[1]].defDefense = 20;
									Main.npc[bosses[1]].defense = 20;
									bossattach[1] = -1;
								}
								if (i == 2)
								{
									Main.npc[bosses[3]].defDefense = 20;
									Main.npc[bosses[3]].defense = 20;
									bossattach[3] = -1;
								}


								thisboss.active = false;
								bossattach[i] = -1;
								bosses[i] = 0;
							}
							else
							{
								if (phase < 1)
									npc.life = npc.life + thisboss.life;
							}
						}

						if (thisboss.active == true)
						{
							if (thisboss.life > 0 && bossactive[i] > 0)
							{
								divider = divider + 1;
								averagevec = averagevec + thisboss.Center;
								if (bossattach[i] > -1)
								{
									NPC myfellaboss = Main.npc[bossattach[i]];
									if (myfellaboss.active == true)
									{
										Main.npc[bosses[i]].position = myfellaboss.Center;
									}
								}
							}
						}


					}
					//averagevec=averagevec-npc.Center;
					if (divider > 1)
					{
						shooting = shooting + 1;
						float shoottype = 0;
						if (divider < 4)
						{
							if (shooting % 1200 < 700)
							{
								if (shooting % 1200 > 100)
								{
									shoottype = 1;
								if (shooting % 200 == 0 && shooting % 8 == 0)
									{
										for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.Pi / 10f)
										{
											float angle = (shooting / 60f) + f;
											int proj = Projectile.NewProjectile(npc.Center, angle.ToRotationVector2() * 24f, mod.ProjectileType("HellionBolt"), 30, 15f);
											Main.projectile[proj].timeLeft = 200;
											Main.projectile[proj].netUpdate = true;
											//Idglib.Shattershots(npc.position, P.position + P.velocity * (1f + (shooting - 120 % 160) / 150f), new Vector2(P.width, P.height), 88, 25, 8, 0, 1, true, 0, false, 450);
										}
									}
								}
							}
							else
							{
								if (shooting % 1200 > 850)
								{
									shoottype = 2;
									if (shooting % 10 == 0)
									{
										for (float f = -1; f < 2; f += 2f)
										{
											float angle = (shooting / 20f) * f;
											int proj = Projectile.NewProjectile(npc.Center, angle.ToRotationVector2() * 18f, mod.ProjectileType("HellionBolt"), 30, 15f);
											Main.projectile[proj].timeLeft = 200;
											Main.projectile[proj].netUpdate = true;
										}
									}
								}
							}

							if (shoottype == 1)
							{
								if ((shooting + 90) % 180 == 0 && Main.expertMode)
								{
									if (Main.npc[bosses[1]].active && Main.npc[bosses[1]].type == NPCID.SkeletronPrime)
									{
										Vector2 Centerhere = (P.position - Main.npc[bosses[1]].Center);
										List<Projectile> itz = Idglib.Shattershots(Main.npc[bosses[1]].Center, P.position, new Vector2(P.width, P.height), ProjectileID.SaucerMissile, 25, 10, 80, 2, false, 0, false, 450);
										itz[0].localAI[1] = -30;
										itz[1].localAI[1] = -30;
									}
								}
								if (shooting % 180 == 0 && Main.expertMode)
								{
									if (Main.npc[bosses[3]].active && Main.npc[bosses[3]].type == NPCID.SkeletronPrime)
									{
										Vector2 Centerhere = (P.position - Main.npc[bosses[3]].Center);
										List<Projectile> itz = Idglib.Shattershots(Main.npc[bosses[3]].Center, P.position, new Vector2(P.width, P.height), ProjectileID.SaucerMissile, 25, 10, 80, 2, false, 0, false, 450);
										itz[0].localAI[1] = -30;
										itz[1].localAI[1] = -30;
									}
								}
							}

							if (shoottype == 2)
							{
								if (true)
								{
									if (Main.npc[bosses[1]].active && Main.npc[bosses[1]].type == NPCID.SkeletronPrime)
									{
										Main.npc[bosses[1]].position.X -= 12.15f;
									}
								}
								if (true)
								{
									if (Main.npc[bosses[3]].active && Main.npc[bosses[3]].type == NPCID.SkeletronPrime)
									{
										Main.npc[bosses[3]].position.X += 12.15f;
									}
								}
							}

						}
						averagevec = averagevec / divider;
						Vector2 lastpos = npc.position;
						npc.position = npc.position + ((averagevec - npc.position) / 15);
						rotatationsensation = (int)(rotatationsensation + (5));
					}
					else
					{
						if (phase < 60)
						{
							phase = phase + 1;
							rotatationsensation = (int)(rotatationsensation + (5 + (phase / 33)));
						}
						else
						{
							shooting = shooting + 1;
							rotatationsensation = (int)(rotatationsensation + 12);
							phase = phase + 1;
							int expert = 0;
							if (Main.expertMode)
							{
								expert = 1;
							}
							if (shooting % ((int)(30 + (npc.life / 5000))) == 0)
							{
								Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), 100, 25, 12, 60, 2 + expert, true, 0, true, 300);
							}

							if (shooting % (int)(60) < 30 && shooting % 3 == 0)
							{
								Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), 83, 20, 3 + expert, 0, 1, true, (float)shooting / 22, false, 300);
							}
							if ((shooting - 30) % (int)(60) < 30 && shooting % 3 == 0)
							{
								Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), 83, 20, 3 + expert, 0, 1, true, -(float)shooting / 22, false, 300);
							}
							if (phase < 240)
							{
								npc.velocity = new Vector2(((P.position.X - 500) - npc.position.X) / 32, ((P.position.Y - 150) - npc.position.Y) / 16);
							}
							else
							{
								npc.velocity = new Vector2(((P.position.X + 500) - npc.position.X) / 32, ((P.position.Y - 150) - npc.position.Y) / 16);
								if (phase > 420)
								{
									phase = 60;
								}
							}
						}




					}

				}




				//npc.spriteDirection = (int)npc.ai[1]*14;
			}



			//int dust = Dust.NewDust(npc.Center, 0, 0, 178, 0, 5, 0, new Color=Main.hslToRgb((float)(npc.ai[0]/300)%1, 1f, 0.9f), 1f);
			int dustType = 43;//Main.rand.Next(139, 143);
			int dustIndex = Dust.NewDust(npc.Center + new Vector2(-16, -16), 32, 32, dustType);//,0,5,0,new Color=Main.hslToRgb((float)(npc.ai[0]/300)%1, 1f, 0.9f),1f);
			Dust dust = Main.dust[dustIndex];
			dust.velocity.X = dust.velocity.X + Main.rand.Next(-50, 51) * 0.01f;
			dust.velocity.Y = dust.velocity.Y + Main.rand.Next(-50, 51) * 0.01f;
			dust.scale *= 3f + Main.rand.Next(-30, 31) * 0.01f;
			dust.fadeIn = 0f;
			dust.noGravity = true;
			dust.color = Main.hslToRgb((float)(npc.ai[0] / 300) % 1, 1f, 0.9f);
			npc.spriteDirection = rotatationsensation;

		}







		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return CanBeHitByPlayer(player);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			if (projectile.hostile)
				return false;
			return CanBeHitByPlayer(Main.player[projectile.owner]);
		}
		private bool? CanBeHitByPlayer(Player P)
		{
			if (phase < 60)
			{
				return false;
			}
			else
			{
				return true;
			}
		}


		public override void FindFrame(int frameHeight)
		{
			//npc.spriteDirection = (int)npc.ai[1]*14;
			npc.frame.Y = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float Scale = 0f;
			Scale = (float)phase;
			if (Scale > 60)
			{

				Scale = 60;
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			var shader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

			DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 600, 600)), Microsoft.Xna.Framework.Color.White * 1f, npc.rotation, new Vector2(600f, 600f), npc.scale * 1f, SpriteEffects.None, 0);
			shader.Apply(null);

			Color glowingcolors1 = Main.hslToRgb((float)(npc.ai[0] / 300) % 1, 1f, 0.9f);
			Color glowingcolors2 = Main.hslToRgb((float)(-npc.ai[0] / 300) % 1, 1f, 0.9f - (float)Scale / 30);
			Color glowingcolors3 = Main.hslToRgb((float)(npc.ai[0] / 80) % 1, 1f, 0.9f - (float)Scale / 90);
			Texture2D texture = mod.GetTexture("NPCs/TPD");
			Texture2D texture2 = mod.GetTexture("NPCs/TPD1");
			Texture2D texture3 = mod.GetTexture("NPCs/TPD2");
			//Vector2 drawPos = npc.Center-Main.screenPosition;
			Vector2 drawPos = npc.Center - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, null, lightColor, npc.spriteDirection, new Vector2(16, 16), new Vector2(3, 3), SpriteEffects.None, 0f);
			spriteBatch.Draw(texture2, drawPos, null, glowingcolors1, npc.spriteDirection, new Vector2(16, 16), new Vector2(3, 3), SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			spriteBatch.Draw(texture3, drawPos, null, glowingcolors2, npc.spriteDirection, new Vector2(16, 16), new Vector2(3, 3) * (1), SpriteEffects.None, 0f);
			if (phase > 0)
			{

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				shader = GameShaders.Armor.GetShaderFromItemId(ItemID.MidnightRainbowDye);
				shader.Apply(null);
				spriteBatch.Draw(texture2, drawPos, null, glowingcolors3, (float)shooting / 40, new Vector2(16, 16), new Vector2(3, 3) * (1.00f + (Scale / 30.00f)), SpriteEffects.None, 0f);
				spriteBatch.Draw(texture3, drawPos, null, glowingcolors3, (float)shooting / 70, new Vector2(16, 16), new Vector2(3, 3) * (1.00f + (Scale / 20.00f)), SpriteEffects.None, 0f);
			}


			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			return false;
		}



	}

}

