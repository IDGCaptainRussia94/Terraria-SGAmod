using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Idglibrary;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Events;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Effects;
using System.Linq;

namespace SGAmod.NPCs
{
	[AutoloadBossHead]
	public class Cirno : ModNPC, ISGABoss
	{
		public string Trophy() => "CirnoTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;

		int aicounter = 0;
		int frameid = 0;
		int framevar = 0;
		int stateaction = 0;
		int bobbing = 0;
		int aistate = 0;
		int card = 0;
		int attacktype = 0;
		int nightmareprog = 0;
		float damagetospellcard = 0.9f;
		bool nightmaremode => SGAWorld.NightmareHardcore > 0;
		int mixup = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno");
			Main.npcFrameCount[npc.type] = 20;
		}
		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 60;
			npc.damage = 90;
			npc.defense = 10;
			npc.lifeMax = 15000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 20, 0, 0);
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.boss = true;
			aiType = NPCID.Wraith;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Cirno_v2");
			bossBag = mod.ItemType("CirnoBag");
			npc.coldDamage = true;
			npc.value = Item.buyPrice(0, 15, 0, 0);
		}

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
			npc.lifeMax = (int)(npc.lifeMax * 0.750f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.5f);
		}
		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.HealingPotion;
		}
		public override void NPCLoot()
		{
			if (npc.boss)
			{


				List<Projectile> itz = Idglib.Shattershots(npc.Center - new Vector2(npc.spriteDirection * 20, 100), npc.Center - new Vector2(npc.spriteDirection * 20, 100), new Vector2(0, 0), mod.ProjectileType("SnowCloud"), 40, 4f, 0, 1, true, 0, false, 1000);

				if (Main.expertMode)
				{
					npc.DropBossBags();
				}
				else
				{
					string[] dropitems = { "Starburster", "Snowfall", "IceScepter", "RubiedBlade", "IcicleFall", "Magishield" };
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType(dropitems[Main.rand.Next(0, dropitems.Length)]));
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("CryostalBar"), Main.rand.Next(15, 25));
					if (Main.rand.Next(3)==0)
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("GlacialStone"));
				}
			}
			Achivements.SGAAchivements.UnlockAchivement("Cirno", Main.LocalPlayer);
			if (SGAWorld.downedCirno == false)
			{
				SGAWorld.downedCirno = true;
				Idglib.Chat("The snowflakes have thawed from your wings", 50, 200, 255);
			}
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write((int)npc.localAI[3]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			npc.localAI[3] = reader.ReadInt32();
        }

        public override bool CheckDead()
		{
			if (npc.localAI[3] < 360)
			{
				npc.active = true;
				npc.life = 5;
				npc.localAI[3] = Math.Min(npc.localAI[3] + 1, 5);
				npc.netUpdate = true;
			}
			return npc.localAI[3]>320;
		}

		public override void AI()
		{
			if (npc.localAI[3]>0)
            {

				if (npc.localAI[3] == 360)
				{
					Gore.NewGore(npc.Center + new Vector2(0, -24), new Vector2(0, -18), mod.GetGoreSlot("Gores/CirnoHeadGore"));
					Gore.NewGore(npc.Center + new Vector2(0, -24), new Vector2(0, -18), mod.GetGoreSlot("Gores/Cirno_bow_gib"), 1f);
					Gore.NewGore(npc.Center + new Vector2(0, 8), new Vector2(-1, -0), mod.GetGoreSlot("Gores/Cirno_leg_gib"), 1f);
					Gore.NewGore(npc.Center + new Vector2(0, 8), new Vector2(1, -0), mod.GetGoreSlot("Gores/Cirno_leg_gib"), 1f);
					Gore.NewGore(npc.Center, new Vector2(-2, -1), mod.GetGoreSlot("Gores/Cirno_arm_gib"), 1f);
					Gore.NewGore(npc.Center, new Vector2(2, -1), mod.GetGoreSlot("Gores/Cirno_arm_gib"), 1f);
					npc.localAI[3] = 2000;
					npc.StrikeNPCNoInteraction(100000, 0, 0);
					Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 117, 1f,-0.75f);
				}

				if (npc.localAI[3] % (int)(40 / (1 + ((npc.localAI[3] / 120f))))==0 || npc.localAI[3] == 360)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 100,0.75f);
					npc.velocity += Main.rand.NextVector2Circular(4f, 4f);
					if (sound != null && npc.localAI[3]<400)
						sound.Pitch = -0.80f + (npc.localAI[3] / 200f);

					for (int num654 = 0; num654 < 1 + npc.localAI[3] / 8f; num654++)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						Dust num655 = Dust.NewDustPerfect(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), 59, randomcircle * (12f + (npc.localAI[3] > 350 ? Main.rand.NextFloat(8f,15f) : 0f)), 150, Color.Aqua, 2f+(npc.localAI[3]>350 ? 2f : 0f));
						num655.noGravity = true;
						num655.noLight = true;
					}
				}

				npc.localAI[3] += 1;
					npc.velocity /= 1.15f;
				npc.dontTakeDamage = true;
				return;
            }
			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || (!Main.dayTime && GetType() == typeof(Cirno)))
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || !Main.dayTime)
				{
					float speed = ((-10f));
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
					npc.active = false;
				}

			}
			else
			{
				if (Main.expertMode)
				{
					if (npc.life < npc.lifeMax * 0.20 && npc.ai[3] == 0)
					{
						npc.ai[3] = 1;
						NPC.SpawnOnPlayer(P.whoAmI, mod.NPCType("CirnoIceFairy"));
						NPC.SpawnOnPlayer(P.whoAmI, mod.NPCType("CirnoIceFairy2"));
					}
					npc.defense = 20 + (NPC.CountNPCS(mod.NPCType("CirnoIceFairy")) * 50) + (NPC.CountNPCS(mod.NPCType("CirnoIceFairy2")) * 50);
				}

				bool snow = P.ZoneSnow;
				//npc.dontTakeDamage = (!snow);
				npc.netUpdate = true;
				npc.timeLeft = 99999;
				bobbing = bobbing + 1;
				npc.spriteDirection = -npc.direction;
				aicounter = aicounter + 1;

				Vector2 playerloc = P.Center;
				if (GetType() == typeof(CirnoHellion) && Hellion.Hellion.GetHellion() != null)
				{
					playerloc = Hellion.Hellion.GetHellion().npc.Center;
					npc.dontTakeDamage = true;
				}

				Vector2 dist = playerloc - npc.Center;

				if (card > 0)
				{
					if (nightmaremode)
					{
						if (nightmareprog < card * 400)
							nightmareprog += 1;
						/*if (nightmareprog == 1)
						{
							

						}*/
						if (SGAWorld.CirnoBlizzard < card * 100 && nightmareprog % 3 == 0)
							SGAWorld.CirnoBlizzard += 1;
						ScreenShaderData shad = Filters.Scene["SGAmod:CirnoBlizzard"].GetShader();
						shad.UseColor(Color.Lerp(Color.Blue, Color.Turquoise, 0.5f + (float)Math.Sin(Main.GlobalTime)));
						Main.raining = true;
						Main.windSpeed = MathHelper.Clamp(Main.windSpeed + Math.Sign((P.Center.X - npc.Center.X)) * (-0.002f / 3f), -0.4f, 0.4f);
						Main.maxRaining = Math.Min(Main.maxRaining + 0.001f, 0.10f);
						Main.rainTime = 5;
						Main.UseStormEffects = true;


						nightmareprog = Math.Min(nightmareprog, 2000);


						if (nightmareprog > 100)
						{
							for (int i = 0; i < Main.maxPlayers; i += 1)
							{
								if (Main.player[i].active && !Main.player[i].dead)
								{
									Main.player[i].AddBuff(BuffID.WindPushed, 2);
								}
							}
						}
						if (nightmareprog > 500)
						{
							ScreenObstruction.screenObstruction = Math.Min((nightmareprog - 500) / 600f, 0.5f);
							for (int i = 0; i < Main.maxPlayers; i += 1)
							{
								if (Main.player[i].active && !Main.player[i].dead)
								{
									//if (card>3)
									//Main.player[i].AddBuff(BuffID.Obstructed, (int)((Main.maxRaining-0.5f)*120f));
									Main.player[i].AddBuff(BuffID.Darkness, (int)((Main.maxRaining - 0.5f) * 60f));
								}
							}
						}

					}



				}

				if (aistate == 2)
				{
					spellcard(card, aicounter, P);
					float floater = (float)(Math.Sin(bobbing / 14f) * 4f);
					npc.direction = -1;
					if (dist.X > 0)
					{
						npc.direction = 1;
					}
					npc.velocity = new Vector2(((playerloc.X - ((npc.Center.X))) / 150), ((playerloc.Y - ((npc.Center.Y + 120))) / 110) + floater);
					if (npc.life < npc.lifeMax * damagetospellcard)
					{
						aistate = 0;
						aicounter = 0;
						framevar = 0;
						stateaction = 0;
						frameid = 0;
						attacktype = 0;
						damagetospellcard = damagetospellcard - 0.15f;
					}
				}



				if (aistate == 3)
				{

					if (aicounter > 15)
					{
						int dustType = 113;
						int dustIndex = Dust.NewDust(npc.Center + new Vector2(-16, -16), 32, 32, dustType);
						Dust dust = Main.dust[dustIndex];
						dust.velocity.X = dust.velocity.X - npc.velocity.X;
						dust.velocity.Y = dust.velocity.Y - npc.velocity.Y;
						dust.scale *= 3f + Main.rand.Next(-30, 31) * 0.01f;
						dust.fadeIn = 0f;
						dust.noGravity = true;

						npc.velocity = npc.velocity + (npc.DirectionTo(playerloc+(P.velocity*3f)) * (0.75f+((float)bobbing/120f)));
						npc.velocity *= 0.96f;
						float extraspeed = (bobbing / 180f);

						if (npc.velocity.Length() > 6f+extraspeed)
						{
							npc.velocity.Normalize();
							npc.velocity = npc.velocity * (6f+extraspeed);
						}
						if (npc.velocity.X > 0)
						{
							npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X);
							npc.direction = 1;
						}
						else
						{
							npc.rotation = (float)Math.Atan2(-npc.velocity.Y, -npc.velocity.X);
							npc.direction = -1;
						}
						if (bobbing > 220)
						{
							float aimer = Main.rand.Next(-1000, 1000);
							if (bobbing % 25 == 0)
							{
								List<Projectile> bolts = Idglib.Shattershots(new Vector2(npc.Center.X, npc.Center.Y), P.position, new Vector2(P.width, P.height), ProjectileID.IceBolt, 20, 32f, 25, 3, true, (float)(aimer / 8000), false, 60);
							}
						}
						if (snow)
						{
							aistate = 0;
							aicounter = 40;
							framevar = 0;
							stateaction = 0;
							frameid = 0;
							attacktype = 0;
						}


					}
					else
					{
						bobbing = 0;
						npc.velocity = npc.velocity * 0.9f;
					}
				}
				else
				{
					npc.rotation = (float)0f;
				}


				if (aistate == 0)
				{
					if (npc.life < npc.lifeMax * damagetospellcard)
					{
						aistate = 2;
						aicounter = 0;
						stateaction = 0;
						attacktype = 0;
						card = card + 1;
						damagetospellcard = damagetospellcard - 0.20f;
					}

					if (aicounter > 90 && !snow && Main.rand.Next(0,60) == 0)
					{
						aicounter = 0;
						aistate = 3;
						framevar = 0;
						stateaction = 2;
						frameid = 14;
					}
					if (aicounter > 140 || (aicounter > 100 && card == 3))
					{


						aicounter = 0;
						aistate = 1;
						framevar = 0;
						stateaction = 1;
						frameid = 6;
						attacktype = (int)Main.rand.Next(0, 2);

					}

					//}
					float floater = (float)(Math.Sin(bobbing / 17f) * 9f);
					if (npc.Center.X < playerloc.X)
					{
						npc.direction = 1;
						npc.velocity = new Vector2(2, ((playerloc.Y - npc.Center.Y) / 12) + floater);
					}
					else
					{
						npc.velocity = new Vector2(-2, ((playerloc.Y - npc.Center.Y) / 12) + floater);
						npc.direction = -1;
					}

					npc.velocity.Normalize();
					npc.velocity = npc.velocity * 2;
					Vector2 offset = (new Vector2(playerloc.X, playerloc.Y) - npc.Center);
					if (offset.Length() > 640)
					{
						npc.velocity += (Vector2.Normalize(offset)*((offset.Length()-640f) / 96f));
					}
				}
				if (aistate == 1 || aistate == 10)
				{
					npc.velocity = new Vector2(0, 0);

					if (aistate == 10)
					{

						if (aicounter < 5)
							mixup = Main.rand.Next(0, 3);
						if (aicounter > 19 && aicounter < 76 && aicounter % 3 == 0 && mixup == 0)
						{
							//float aimer = Main.rand.Next(0, 0);
							//Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), 348, 10, (float)26, 0, 1, true, (float)(aimer / 8000), false, Main.rand.Next(100, 120));

							float circle = aicounter/30f;
									Vector2 offset = Vector2.Lerp(npc.Center,P.MountedCenter-new Vector2(0,200), circle);
									int proj2 = Projectile.NewProjectile(offset,Vector2.UnitY*(aicounter % 6 == 0 ? 16f : 10f), mod.ProjectileType("CirnoIceShardHinted"), 40, 4, 0);
									(Main.projectile[proj2].modProjectile as CirnoIceShardHinted).CirnoStart = npc.Center;
									Main.projectile[proj2].netUpdate = true;

						}

						if (mixup == 2)
						{

							if (aicounter == 19)
							{

								for (int num315 = 0; num315 < 60; num315 = num315 + 1)
								{
									int dust = Dust.NewDust(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), 0, 0, 113, Main.rand.Next(-5, 5), Main.rand.Next(-5, 5), 25, Main.hslToRgb(0.6f, 0.9f, 1f), 3f);
									Main.dust[dust].noGravity = true;
								}
							}
							if (aicounter > 9 && aicounter < 66 && aicounter % 2 == 0)
							{
								float aimer = Main.rand.Next(0, 0);
								Idglib.Shattershots(P.position + new Vector2(Main.rand.Next(-20, 20), P.Center.Y + 200), P.position + new Vector2(Main.rand.Next(-20, 20), P.Center.Y - 500), Vector2.Zero, ProjectileID.IcewaterSpit, 20, (float)23, 0, 1, true, 0, false, Main.rand.Next(270, 370));
							}
						}

						if (aicounter == 19 && mixup == 1)
						{
							List<Projectile> itz = Idglib.Shattershots(npc.Center - new Vector2(0, 30), npc.Center - new Vector2(-npc.direction * 20, 30), new Vector2(0, 0), mod.ProjectileType("SnowCloud"), 40, 5f, 0, 1, true, 0, false, 500);
							itz[0].velocity = itz[0].velocity + new Vector2(0, -6);
						}

					}
					if (aistate == 1)
					{

						if (card == 0 || card > 1)
						{
							if (attacktype == 0)
							{
								if (aicounter == 20)
								{
									float rotter = Main.rand.NextFloat(MathHelper.TwoPi);
									for (float circle = 0; circle < MathHelper.TwoPi; circle += MathHelper.Pi / 6f)
									{
										//if (circle % MathHelper.Pi > 1f)
										//{
											Vector2 offset = Vector2.One.RotatedBy(circle + rotter);
											int proj2 = Projectile.NewProjectile(P.Center.X + (offset.X * 300f), P.Center.Y + (offset.Y * 300f), -offset.X * 5f, -offset.Y * 5f, mod.ProjectileType("CirnoIceShardHinted"), 40, 4, 0);
											(Main.projectile[proj2].modProjectile as CirnoIceShardHinted).CirnoStart = npc.Center;
											Main.projectile[proj2].netUpdate = true;
										//}
									}
								}
							}
							if (attacktype == 1)
							{
								if (aicounter > 19 && aicounter < 46 && aicounter % 3 == 0)
								{
									float aimer = Main.rand.Next(-1000, 1000);
									List<Projectile> bolts = Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), mod.ProjectileType("CirnoBolt"), 50, (float)Main.rand.Next(60, 80) / 10f, 0, 1, true, (float)(aimer / 8000), false, 200);
									CirnoBolt Cbolt = bolts[0].modProjectile as CirnoBolt;
									Cbolt.homing = 0.04f;
									bolts[0].netUpdate = true;

									//Shattershots(npc.position,P.position,new Vector2(P.width,P.height),83,20,12,40,2,true,0);
								}
							}

						}

						if (card == 1 || card > 1)
						{
							if (attacktype == 0)
							{
								if (aicounter == 20)
								{
									Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), mod.ProjectileType("CirnoBolt"), 50, 8f, 85, 4, false, 0, false, 450);
								}
							}

							if (attacktype == 1)
							{
								for (int i = 0; i < 2; i += 1)
								{
									if (aicounter > 19 && aicounter < 79 && aicounter % 2 == 0)
									{
										float rotter = Main.rand.NextFloat(MathHelper.TwoPi);
										Vector2 offset = Main.rand.NextVector2Circular(Main.rand.NextFloat(-320, 320), Main.rand.NextFloat(-480, 240));
										Vector2 playerAim = Vector2.Normalize(P.MountedCenter - npc.Center);

										float speed = aicounter / 2f;
										int proj2 = Projectile.NewProjectile(npc.Center.X + (offset.X), npc.Center.Y + (offset.Y), playerAim.X * speed, playerAim.Y * speed, mod.ProjectileType("CirnoIceShardHinted"), 40, 4, 0);
										(Main.projectile[proj2].modProjectile as CirnoIceShardHinted).CirnoStart = npc.Center;
										Main.projectile[proj2].netUpdate = true;

									}
								}
							}
						}
					}


					if (aicounter > 80)
					{
						frameid = 12;
					}
					if (aicounter == 90 || (aicounter == 40 && card == 3 && aistate != 10))
					{
						if (aistate == 10)
						{
							aistate = 2;
						}
						else
						{
							aistate = 0;
						}
						framevar = 0;
						stateaction = 0;
						frameid = 0;
						aicounter = 0;
					}
				}




			}
		}

		//		public override bool? CanBeHitByItem(Player player, Item item)
		//{
		//	return CanBeHitByPlayer(player);
		//}
		//public override bool? CanBeHitByProjectile(Projectile projectile)
		//{
		//	return CanBeHitByPlayer(Main.player[projectile.owner]);
		//}
		//private bool? CanBeHitByPlayer(Player P){
		//if (phase<60){
		//return false;
		//}else{
		//return true;
		//}
		//}

		/*		public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
		{
			OnHit(player, damage);
		}

		public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
		{
			OnHit(Main.player[projectile.owner], damage);
		}

		private void OnHit(Player player, int damage)
		{
			if (player.active)
			{
			Vector2 dist=player.Center-npc.Center;
			if (dist.Length()>600 || (aistate>1 && aicounter<50)){
				npc.life=npc.life+damage;
				damage=0;
			}else{
			damagetospellcard=damagetospellcard-damage;
			}
			}else{
			npc.life=npc.life+damage;
			}

		}
		*/

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			//if (Main.expertMode || Main.rand.Next(2) == 0)
			//{
			player.AddBuff(BuffID.Chilled, 600, true);
			if (Main.expertMode)
				player.AddBuff(BuffID.Frostburn, 60 * 4, true);
			//}
		}




		public override void FindFrame(int frameHeight)
		{
			framevar = framevar + 1;
			if (stateaction == 0)
			{
				if (frameid > 6)
				{
					frameid = 0;
				}
				if (framevar > 4)
				{
					frameid = frameid + 1;
					framevar = 0;
					if (frameid > 5)
					{
						frameid = 0;
					}
				}
			}

			if (stateaction == 2)
			{
				if (aistate == 0)
				{
					stateaction = 0;
					frameid = 0;
				}
				if (framevar > 3)
				{
					framevar = 0;
					frameid = frameid + 1;
				}
				if (frameid > 19)
				{
					frameid = 16;
				}
			}

			if (stateaction == 1)
			{
				if (aicounter < 81)
				{
					if (framevar > 5)
					{
						framevar = 0;
						frameid = frameid + 1;
					}
					if (frameid > 12)
					{
						frameid = 11;
					}
				}
				else
				{
					frameid = 12;
				}
			}
			//npc.spriteDirection = (int)npc.ai[1]*14;
			if (frameid == 2) { frameid = 3; }
			npc.frame.Y = frameid * 80;
		}


		private void spellcard(int mycardid, int counter, Player P)
		{

			if (counter > 50)
			{
				if (mycardid == 1)
				{

					if (counter % 6 == 0)
					{

						Vector2 vis = new Vector2(P.Center.X + Main.rand.Next(-800, 800), P.Center.Y);
						float aimer = Main.rand.Next(-1000, 1000);
						if ((counter % 600) > 540 && Main.expertMode)
						{
							Idglib.Shattershots(new Vector2(P.Center.X - 800, P.Center.Y), new Vector2(P.Center.X + 800, P.Center.Y), new Vector2(0, 0), mod.ProjectileType("CirnoBolt"), 25, (float)Main.rand.Next(60, 80) / 8, 0, 1, true, (float)(aimer / 9000), false, 200);
							Idglib.Shattershots(new Vector2(P.Center.X + 800, P.Center.Y), new Vector2(P.Center.X - 800, P.Center.Y), new Vector2(0, 0), mod.ProjectileType("CirnoBolt"), 25, (float)Main.rand.Next(60, 80) / 8, 0, 1, true, (float)(aimer / 9000), false, 200);
						}
						Idglib.Shattershots(new Vector2(vis.X, P.Center.Y - 600f), new Vector2(vis.X, vis.Y + 600f), new Vector2(0, 0), mod.ProjectileType("CirnoIceShard"), 20, (float)Main.rand.Next(60, 80) / 10, 0, 1, true, (float)(aimer / 5000), false, 200);

					}

				}
				if (card == 2)
				{
					if (Main.expertMode)
					{

						if (mixup < 100)
						{
							mixup = 100;
							for (int i = -400; i <= 401; i = i + 800)
							{
								List<Projectile> itz = Idglib.Shattershots(npc.Center - new Vector2(i * 3, 50), npc.Center, new Vector2(0, -50), mod.ProjectileType("SnowCloud"), 40, 2f, 0, 1, true, 0, false, 1000);
								itz[0].velocity = itz[0].velocity + new Vector2(0, -4);
							}
						}


						if (counter % 350 == 0)
						{
							Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 30, 1f, -0.5f);
							for (int i = -200; i <= 201; i = i + 50)
							{
								Idglib.Shattershots(new Vector2(P.Center.X - 600, P.Center.Y + i), P.Center + new Vector2(0, i), new Vector2(0, 0), mod.ProjectileType("CirnoIceShard"), 30, (float)2, 0, 1, true, 0, false, 190);
								Idglib.Shattershots(new Vector2(P.Center.X + 600, P.Center.Y + i), P.Center + new Vector2(0, i), new Vector2(0, 0), mod.ProjectileType("CirnoIceShard"), 30, (float)2, 0, 1, true, 0, false, 190);
							}
						}
					}
					if ((counter) % 180 == 0) { Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), ProjectileID.EnchantedBeam, 50, (float)13, 0, 1, true, 0, false, 120); }
					if ((counter - 40) % 180 == 0) { Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), mod.ProjectileType("CirnoIceShard"), 20, (float)7, 135, 8, true, 0, false, 160); Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 30, 1f, 0f); }
					if ((counter - 80) % 180 == 0) { Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), mod.ProjectileType("CirnoIceShard"), 20, (float)9, 85, 4, false, 0, false, 140); Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 30, 1f, 0.30f); }
				}

				if (card == 3)
				{
					if (counter % 8 == 0)
					{
						Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), 263, 35, (float)(counter / 8), 0, 1, true, (float)(counter - 140) / 70, true, 120);
						Idglib.Shattershots(new Vector2(npc.Center.X + (npc.direction * 48), npc.Center.Y), P.position, new Vector2(P.width, P.height), 263, 35, (float)(counter / 8), 0, 1, true, -(float)(counter - 140) / 70, true, 120);
					}
					if (counter % 200 == 0)
					{
						aicounter = 0;
						aistate = 10;
						framevar = 0;
						stateaction = 1;
						frameid = 6;
						attacktype = 0;//(int)Main.rand.Next(0,2);
					}
				}


			}

		}


	}


	public class SnowCloud : ModProjectile
	{

		public virtual int projectileid => ProjectileID.SnowBallFriendly;
		public virtual Color colorcloud => Main.hslToRgb(0.6f, 0.8f, 0.8f);
		public virtual int rate => 5;


		public override void SetDefaults()
		{
			projectile.width = 30;
			projectile.height = 30;
			//projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.hostile = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			projectile.timeLeft = 600;
			projectile.tileCollide=false;
		}

				public override string Texture
		{
			get { return "Terraria/Item_" + 5; }
		}

	public override bool? CanHitNPC(NPC target){
		return false;
	}
	public override bool CanHitPlayer(Player target){
		return false;
	}
	public override void AI()
	{
		projectile.velocity=new Vector2(projectile.velocity.X,projectile.velocity.Y*0.95f);
		int q=0;
			for (q = 0; q < 4; q++)
				{

					int dust = Dust.NewDust(projectile.position-new Vector2(100,0), 200, 12, DustID.Smoke, 0f, projectile.velocity.Y * 0.4f, 100, colorcloud, 3f);
					Main.dust[dust].noGravity = true;
					//Main.dust[dust].velocity *= 1.8f;
					//Main.dust[dust].velocity.Y -= 0.5f;
					//Main.playerDrawDust.Add(dust);
				}
				projectile.ai[0]++;
		int target2=Idglib.FindClosestTarget(projectile.friendly ? 0 : 1,projectile.position,new Vector2(0,0));
		Entity target;
		target=Main.player[target2] as Player;
		if (projectile.friendly){
		target=Main.npc[target2] as NPC;
		//target=Main.player[target2];
		}

		if (target is Player){
		Player targetasplayer=target as Player;
		if (targetasplayer.ownedProjectileCounts[mod.ProjectileType("SnowfallCloud")]>0){
		projectile.Kill();
		}}

		if (target!=null){



		Vector2 dist=target.Center-projectile.position;
		if (System.Math.Abs(dist.X)<250){
		if (projectile.ai[0]%rate==0){
		List<Projectile> itz=Idglib.Shattershots(projectile.Center+new Vector2(Main.rand.Next(-100,100),0),projectile.Center+new Vector2(Main.rand.Next(-200,200),500),new Vector2(0,0), projectileid, (int)projectile.damage,8f,0,1,true,0,true,220);
		itz[0].friendly=projectile.friendly;
		itz[0].hostile=projectile.hostile;
		itz[0].coldDamage = true;
		itz[0].netUpdate=true;
		itz[0].ranged = false;
		itz[0].minion = true;
		}
		}


		}



	}

public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
return false;
}

}

	public class CirnoBolt : ModProjectile
	{

		double keepspeed=0.0;
		public float homing=0.02f;
		Vector2 gothere;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno's Grace");
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.IceBolt; }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.IceBolt);
			projectile.width = 30;
			projectile.height = 30;
			projectile.magic = true;
			projectile.coldDamage = true;
			projectile.npcProj = true;
			aiType = 0;//ProjectileID.IceBolt;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Frostburn, 60 * 10);
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.rand.Next(1,3)==1)
			target.AddBuff(BuffID.Frostburn, 60 * 5);
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write((double)gothere.X);
			writer.Write((double)gothere.Y);
			writer.Write((double)homing);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			gothere.X = (float)reader.ReadDouble();
			gothere.Y = (float)reader.ReadDouble();
			homing = (float)reader.ReadDouble();
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type=ProjectileID.IceBolt;
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 92, projectile.velocity.X+(float)(Main.rand.Next(-20,20)/15f), projectile.velocity.Y+(float)(Main.rand.Next(-20,20)/15f), 50, Main.hslToRgb(0.6f,0.9f, 1f), 2.4f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			return true;
		}

		public override void AI()
		{
		for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 92, projectile.velocity.X, projectile.velocity.Y, 50, Main.hslToRgb(0.6f,0.9f, 1f), 1.7f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
				//dust3.shader = GameShaders.Armor.GetShaderFromItemId(ItemID.MidnightRainbowDye);
			}

			if (projectile.ai[0] < 1)
			{
				if (projectile.hostile)
					homing *= 1f;
				projectile.ai[1] = -1;
			}

			projectile.ai[0]=projectile.ai[0]+1;
		if (projectile.ai[0]<2){
		keepspeed=(projectile.velocity).Length();
		}
			if (gothere==null || projectile.ai[0] % 40 == 0 || projectile.ai[0] == 1)
			{
				int target3 = Idglib.FindClosestTarget(projectile.friendly ? 0 : 1, projectile.position, new Vector2(0, 0), true, true, true, projectile);

				//if (target2 > 0) {
				Entity target;
				target = Main.player[target3] as Player;
				if (projectile.friendly)
				{
					target = Main.npc[target3] as NPC;
					//target=Main.player[target2];
				}
				gothere = target.Center;
				projectile.netUpdate = true;
			}
				if (gothere != null && (gothere - projectile.Center).Length() < 1000f)
				{
					if (projectile.ai[0] < (Main.expertMode == true ? 150f : 50f) || projectile.friendly)
					{
						projectile.velocity = projectile.velocity + (projectile.DirectionTo(gothere) * ((float)keepspeed * homing));
						projectile.velocity.Normalize();
						projectile.velocity = projectile.velocity * (float)keepspeed;
					}
				}

		}



		}

	public class CirnoIceFairy : IceFairy
	{
		int shooting2=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("2nd Strongest Ice Fairy");
			Main.npcFrameCount[npc.type] = 4;
		}
		public override string Texture
		{
			get { return("SGAmod/NPCs/IceFairy");}
		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 0;
			npc.defense = 4;
			npc.lifeMax = 1800;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0.0f;
			npc.aiStyle = 22;
			aiType = 0;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = 40000f;
		}

				public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			return 0f;
		}

		public override void AI()
		{
		overAI(npc);
		}

		public void overAI(NPC npc2)
		{
		base.AI();
		shooting2=shooting2+1;
		Player P = Main.player[npc.target];
		npc2.ai[3]+=Main.rand.Next(-2,4);
		int npctype=mod.NPCType("Cirno");
		if (NPC.CountNPCS(npctype)>0){
		NPC myowner=Main.npc[NPC.FindFirstNPC(npctype)];
			Vector2 here=myowner.Center;
		if (npc2.ai[3]%400>150){
		float leftorright = (npc.type==mod.NPCType("CirnoIceFairy2")) ? -128f : 128f;
		if (npc2.ai[3]%300>200){
		here=P.Center;
		leftorright = (npc.type==mod.NPCType("CirnoIceFairy2")) ? 220f : -220f;
		}
		float bobbing=-30f+(float)Math.Sin(npc.ai[3]/31)*20f;
		npc2.velocity=(npc2.velocity+((here+new Vector2(leftorright,bobbing)-(npc2.position))*0.02f)*0.01f)*0.99f;
		npc.aiStyle = -1;
		}else{
		npc.aiStyle = 22;
		}
		if (shooting2%400>250){
		if (shooting2%15==0){
		Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 30, 1f, -0.25f+Main.rand.NextFloat()/2f);
		Idglib.Shattershots(npc.Center,myowner.Center, new Vector2(0,0), mod.ProjectileType("CirnoIceShard"), 30,15,0,1,true,0,false,180);
		}}
		npc2.timeLeft=99;
		}else{
		npc2.active=false;
		}
		}

		public override void NPCLoot()
		{
		//stuff	
        }


	}

	public class CirnoIceFairy2 : CirnoIceFairy
	{
		int shooting=100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("3rd Strongest Ice Fairy");
			Main.npcFrameCount[npc.type] = 4;
		}
		public override string Texture
		{
			get { return("SGAmod/NPCs/IceFairy");}
		}
		public override void SetDefaults()
		{
			npc.width = 40;
			npc.height = 40;
			npc.damage = 0;
			npc.defense = 20;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = 0f;
			npc.knockBackResist = 0f;
			npc.aiStyle = 22;
			aiType = 0;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = 40000f;
		}

	}

	public class CirnoIceShard : ModProjectile
	{

		int fakeid = ProjectileID.FrostShard;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Baka Ice Shard");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.CloneDefaults(fakeid);
			projectile.width = 8;
			projectile.height = 8;
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.penetrate = 1;
			projectile.extraUpdates = 0;
			projectile.aiStyle = -1;
			projectile.timeLeft = 1000;
			projectile.coldDamage = true;
			projectile.npcProj = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = fakeid;
			return true;
		}

		public virtual void Moving()
		{
			if (Main.rand.Next(0, 2) == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Ice);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.NextFloat(0.1f, 0.25f));
			}
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public override void AI()
		{
			Moving();
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(BuffID.Chilled, 60 * 5);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Chilled, 60 * 5);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.localAI[0] < 100)
				projectile.localAI[0] = 100 + Main.rand.Next(0, 3);
			Texture2D tex = SGAmod.HellionTextures[5];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			int timing = (int)(projectile.localAI[0] - 100);
			timing %= 5;
			timing *= ((tex.Height) / 5);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), lightColor*projectile.Opacity, MathHelper.ToRadians(0) + projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

	}

	public class CirnoIceShardHinted : CirnoIceShard
	{
		float strength => Math.Min(1f-(projectile.localAI[1] / 110f), 1f);
		public Vector2 bezspot1 = default;
		public Vector2 bezspot2 = default;
		public Vector2 CirnoStart = default;
		public Vector2[] oldPos = new Vector2[10];
		public float speedIncrease = 60f;
		public float homeInTime = 100;
		protected Vector2 VectorEffect = default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stay Frosty!");
		}

		public override bool CanDamage()
		{
			return projectile.localAI[1] >= 100;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.timeLeft = 1000;
		}

		public override void AI()
		{
			projectile.localAI[1] += 1;
			if (projectile.localAI[1] >= homeInTime+10)
			{
				base.AI();
            }
            else
            {
				projectile.rotation = MathHelper.Lerp((projectile.Center-VectorEffect).ToRotation()+MathHelper.PiOver2,projectile.velocity.ToRotation()+MathHelper.PiOver2, projectile.localAI[1]/ homeInTime);
			}
			projectile.Opacity = ((projectile.localAI[1]-30)/70f);
			projectile.position -= projectile.velocity * MathHelper.Clamp(1f-((projectile.localAI[1] - homeInTime) / 60f),0f, 1f);


			if (projectile.localAI[1] == 1)
			{
				bezspot1 = CirnoStart + Main.rand.NextVector2CircularEdge(200, 200);
				bezspot2 = projectile.Center + Main.rand.NextVector2CircularEdge(500, 500);
				for (int k = oldPos.Length - 1; k > 0; k--)
				{
					oldPos[k] = CirnoStart;
				}
			}

			VectorEffect = IdgExtensions.BezierCurve(CirnoStart, CirnoStart, bezspot1, bezspot2, projectile.Center, Math.Min(projectile.localAI[1] / homeInTime, 1f));
			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = VectorEffect;

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				if (oldPos[k] == default)
				oldPos[k] = VectorEffect;
			}

			if (strength > 0)
			{
				TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
				trail.color = delegate (float percent)
				{
					return Color.Aqua;
				};
				trail.projsize = projectile.Hitbox.Size() / 2f;
				trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
				trail.trailThickness = 2;
				trail.trailThicknessIncrease = 6;
				trail.capsize = new Vector2(4f, 0f);
				trail.strength = strength;
				trail.DrawTrail(oldPos.ToList(), projectile.Center);
			}

			if (projectile.Opacity > 0)
			{
				if (projectile.localAI[0] < 100)
					projectile.localAI[0] = 100 + Main.rand.Next(0, 3);
				Texture2D tex = SGAmod.HellionTextures[5];
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
				Vector2 drawPos = ((VectorEffect - Main.screenPosition)) + new Vector2(0f, 4f);
				int timing = (int)(projectile.localAI[0] - homeInTime);
				timing %= 5;
				timing *= ((tex.Height) / 5);
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), lightColor * projectile.Opacity, MathHelper.ToRadians(0) + projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}

			return false;
		}

	}

		public class CirnoHellion : Cirno
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno, Hellion Annointed");
			Main.npcFrameCount[npc.type] = 20;
		}

		public override string Texture
		{
			get { return "SGAmod/NPCs/Cirno"; }
		}
		public override void AI()
		{
			base.AI();
			Hellion.Hellion hell = Hellion.Hellion.GetHellion();
			if (hell == null)
			{
				npc.active = false;
				return;
			}
			npc.life = 10 + (int)(((float)hell.army.Count / (float)hell.armysize) * npc.lifeMax);
		}
		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 60;
			npc.damage = 90;
			npc.defense = 30;
			npc.lifeMax = 25000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.value = Item.buyPrice(0, 20, 0, 0);
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.boss = true;
			aiType = NPCID.Wraith;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			music = MusicID.Boss2;
			bossBag = mod.ItemType("CirnoBag");
			npc.value = Item.buyPrice(0, 15, 0, 0);
		}
	}

}

