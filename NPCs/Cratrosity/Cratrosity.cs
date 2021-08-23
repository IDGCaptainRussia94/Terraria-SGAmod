using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items.Weapons;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod.NPCs.Cratrosity
{
	[AutoloadBossHead]
	public class Cratrosity : ModNPC, ISGABoss
	{
		public string Trophy() => GetType() == typeof(Cratrosity) ? "CratrosityTrophy" : "CratrogeddonTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;

		public Vector2 offsetype = new Vector2(0, 0);
		public int phase = 5;
		public int defaultdamage = 60;
		public int themode = 0;
		public float compressvar = 1;
		public float compressvargoal = 1;
		public int doCharge = 0;
		public int evilcratetype = WorldGen.crimson ? ItemID.CrimsonFishingCrate : ItemID.CorruptFishingCrate;
		Vector2 theclostestcrate = new Vector2(0, 0);
		public int[,] Cratestype = new int[,] {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
		public int[] Cratesperlayer = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
		public float[,] Cratesdist = new float[,] {{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f}};
		public Vector2[,] Cratesvector = new Vector2[,] {{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)}};
		public double[,] Cratesangle = new double[,] {{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0}};
		public int postmoonlord = 0;
		public float nonaispin = 0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}

		public override bool Autoload(ref string name)
		{
			return base.Autoload(ref name);
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 50;
			npc.defense = 50;
			npc.lifeMax = 5000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath37;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.boss = true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			theclostestcrate = npc.Center;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Cratrosity");
			npc.value = Item.buyPrice(0, 10, 0, 0);
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.GoldenCrate; }
		}

		public override string BossHeadTexture => "Terraria/Item_" + ItemID.GoldenCrate;


		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override bool CheckActive()
		{
			return false;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(doCharge);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			doCharge = reader.ReadInt32();
        }

        public virtual void FalseDeath(int phase)
		{
			//nothing here

		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return npc.localAI[0] > 0 && doCharge<1;
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion;
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrariacoCrateKey"));
			Achivements.SGAAchivements.UnlockAchivement("Cratrosity", Main.LocalPlayer);
			if (SGAWorld.downedCratrosity == false)
			{
				Idglib.Chat("The hungry video game industry has been tamed! New items are available for buying", 244, 179, 66);
			}
			SGAWorld.downedCratrosity = true;
		}


		public override bool CheckDead()
		{
			if (npc.life < 1 && phase > 0)
			{
				npc.life = npc.lifeMax;
				phase -= 1;
				Cratrosity origin = npc.modNPC as Cratrosity;
				CrateRelease(phase);
				FalseDeath(phase);
				if (origin.postmoonlord > 0)
				{
					//do stuff here
				}
				npc.active = true;
				return false;
			}
			else { return true; }

		}

		public override void AI()
		{
			doCharge -= 1;
			Player P = Main.player[npc.target];
			npc.localAI[0] -= 1;
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


				npc.ai[0] += 1f;
				Vector2 gohere = new Vector2(P.Center.X, P.Center.Y - 220) + offsetype;
				float thespeed = 0.01f;
				float friction = 0.98f - phase * 0.04f;
				float friction2 = 0.99f - phase * 0.0075f;
				themode -= 1;
				npc.ai[1] += Main.rand.Next(0, 5);
				if (npc.ai[1] % 2000 > 850)
				{
					if (System.Math.Abs(npc.ai[2]) < 300)
					{
						compressvargoal = 1;
						int theammount = (npc.ai[2] > 0 ? 1 : -1) * (offsetype.X > 0 ? 1 : -1);
						if (npc.ai[1] % 2000 < 1100)
						{
							gohere = new Vector2(P.Center.X + (theammount * 800), P.Center.Y - 220);
							npc.velocity = (npc.velocity + ((gohere - npc.Center) * thespeed)) * 0.98f;
						}
						else
						{

							npc.velocity = new Vector2(-theammount * ((GetType() == typeof(Cratrogeddon)) ? 15 : 10), 0);
							if (npc.ai[0] % 15 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(npc.Center, P.Center + new Vector2(0, P.Center.Y > npc.Center.Y ? 600 : -600), new Vector2(0, 0), ModContent.ProjectileType<GlowingCopperCoin>(), (int)(npc.damage * (20.00 / defaultdamage)), 10, 0, 1, true, 0, true, 100);
							}
							if (npc.ai[0] % 40 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(theclostestcrate, P.Center, new Vector2(0, 0), ModContent.ProjectileType<GlowingGoldCoin>(), (int)(npc.damage * (30.00 / defaultdamage)), 10, 0, 1, true, 0, true, 200);
							}
							if (((npc.ai[0] + 20) % 40 == 0) && Main.expertMode)
							{
								List<Projectile> itz = Idglib.Shattershots(theclostestcrate, P.Center + new Vector2(0, P.Center.Y > theclostestcrate.Y ? 600 : -600), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (25.00 / defaultdamage)), 10, 0, 1, true, 0, true, 200);
								SGAprojectile modeproj = itz[0].GetGlobalProjectile<SGAprojectile>();
								modeproj.splittingcoins = true;
								modeproj.splithere = P.Center;
							}
							if (phase < 4)
							{
								if (npc.ai[0] % 8 == 0)
								{
									Idglib.Shattershots(npc.Center, npc.Center + new Vector2(-npc.velocity.X, 0), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (25.00 / defaultdamage)), 25, 0, 1, true, 0, false, 40);
								}
							}
							themode = 300;
							if (offsetype.X >= 0)
							{
								npc.localAI[0] = 5;
								if (npc.Center.X < P.Center.X - 700)
								{
									npc.ai[2] = Math.Abs(npc.ai[2]);
								}
								if (npc.Center.X > P.Center.X + 700)
								{
									npc.ai[2] = -Math.Abs(npc.ai[2]);
								}
							}
							else
							{
								if (npc.Center.X < P.Center.X - 700)
								{
									npc.ai[2] = -Math.Abs(npc.ai[2]);
								}
								if (npc.Center.X > P.Center.X + 700)
								{
									npc.ai[2] = Math.Abs(npc.ai[2]);
								}
							}
							//npc.ai[1]=1600+(2000-1600);
							//}
						}

					}
					else
					{
						Vector2 gogo = P.Center - npc.Center; gogo.Normalize(); gogo = gogo * (8 - phase * 1);
						if (GetType() == typeof(Cratrogeddon))
							gogo *= 1.25f;
						npc.velocity = gogo;
						compressvargoal = 2;
						npc.localAI[0] = 5;
					}
				}
				else
				{
					npc.ai[2] = Main.rand.Next(-600, 600);
					if (npc.ai[0] % 600 < 350)
					{
						npc.velocity = (npc.velocity + (((gohere) - npc.Center) * thespeed)) * friction;
						compressvargoal = 1;

						switch (phase)
						{
							case 5:
								{
									if (npc.ai[0] % 30 == 0)
									{
										Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingCopperCoin>(), (int)(npc.damage * (20f / (float)defaultdamage)), 10, 0, 1, true, 0, true, 150);
									}
									break;
								}
							case 4:
								{
									if (npc.ai[0] % 10 == 0)
									{
										Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (25f / (float)defaultdamage)), 14, 0, 1, true, 0, true, 100);
									}
									break;
								}
							case 3:
								{
									if (npc.ai[0] % 3 == 0 && npc.ai[0] % 50 > 38)
									{
										Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingGoldCoin>(), (int)(npc.damage * (30f / (float)defaultdamage)), 16, 0, 1, true, 0, true, 90);
									}
									if (npc.ai[0] % 8 == 0 && Main.expertMode)
									{
										List<Projectile> itz = Idglib.Shattershots(npc.Center, npc.Center + new Vector2(0, -5), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (25f / (float)defaultdamage)), 7, 360, 2, true, npc.ai[0] / 20, true, 300);
									}
									break;
								}
							case 2:
								{

									if (npc.ai[0] % 5 == 0)
									{

										Idglib.Shattershots(npc.Center, npc.Center + new Vector2((P.velocity.X * 4f) + Main.rand.NextFloat(-24f, 24f), -96f), Vector2.Zero, ModContent.ProjectileType<GlowingGoldCoinHoming>(), (int)(npc.damage * (30f / (float)defaultdamage)), Main.rand.NextFloat(6f, 10f), 0, 1, true, 0, true, 600);
									}
								}
								break;

						}

						doCharge = -100000;
					}
					else
					{
						npc.localAI[0] = 5;
						compressvargoal = 0.4f;
						Vector2 gogo = P.Center - npc.Center; gogo.Normalize(); gogo = gogo * (30 - phase * 2);

						if (doCharge < -1000)
						{
							doCharge = 120;
						}

						if (doCharge > 0)
						{
							npc.ai[0] -= 1;
							if (doCharge == 60)
							{
								SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageAttack, npc.Center);
								if (sound != null)
								{
									sound.Pitch = -0.25f;
								}

								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
								{
									Vector2 offset = f.ToRotationVector2();
									int dust = Dust.NewDust(npc.Center + (offset * 32f), 0, 0, DustID.GoldFlame);
									Main.dust[dust].scale = 1.5f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
								}
							}

							npc.velocity /= 1.5f;
						}
						else
						{

							if (GetType() == typeof(Cratrogeddon))
								gogo *= 0.4f;

							float tiev = (GetType() == typeof(Cratrogeddon)) ? npc.ai[0] % (50 + (phase * 3)) : npc.ai[0] % (25 + (phase * 10));

							if (tiev < (GetType() == typeof(Cratrogeddon) ? 10 : 1))
							{
								npc.velocity = (npc.velocity + gogo);
								if (npc.velocity.Length() > 30)
								{
									npc.velocity.Normalize();
									npc.velocity *= 30f;
								}

							}
							npc.velocity = npc.velocity * friction2;
						}
					}
				}
			}

			npc.defense = (int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.WoodenCrate.ToString()))) * 5 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.IronCrate.ToString()))) * 6 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.GoldenCrate.ToString()))) * 6 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.DungeonFishingCrate.ToString()))) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.JungleFishingCrate.ToString()))) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + evilcratetype.ToString()))) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.HallowedFishingCrate.ToString()))) * 10 +
			NPC.CountNPCS(mod.NPCType("CratrosityCrateDankCrate")) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.FloatingIslandFishingCrate.ToString()))) * (30);
			npc.defense *= Main.expertMode ? 4 : 2;
			npc.defense += Main.expertMode ? 20 : 0;
			OrderOfTheCrates(P);



		}

		public virtual void OrderOfTheCrates(Player P)
		{
			nonaispin = nonaispin + 1f;
			compressvar += (compressvargoal - compressvar) / 20;
			float themaxdist = 99999f;
			for (int a = 0; a < phase; a = a + 1)
			{
				Cratesperlayer[a] = 4 + (a * 4);
				for (int i = 0; i < Cratesperlayer[a]; i = i + 1)
				{
					Cratesangle[a, i] = (a % 2 == 0 ? 1 : -1) * ((nonaispin * 0.01f) * (1f + a / 3f)) + 2.0 * Math.PI * ((i / (double)Cratesperlayer[a]));
					Cratesdist[a, i] = compressvar * 20f + ((float)a * 20f) * compressvar;

					float theexpand = 0f;

					if (themode > 0)
					{
						theexpand = (((i / 1f) * (a + 1f))) * (themode / 30f);
					}
					Cratesvector[a, i] += ((Cratesdist[a, i] * (new Vector2((float)Math.Cos(Cratesangle[a, i]), (float)Math.Sin(Cratesangle[a, i]))) + npc.Center) - Cratesvector[a, i]) / (theexpand + (Math.Max((((compressvar) - 1) * (2 + (a * 1))), 1)));
					float sinner = npc.ai[0] + ((float)(i * 5) + (a * 14));
					float sinner2 = (float)(10f + (Math.Sin(sinner / 30f) * 7f));
					if (compressvar > 1.01)
					{
						int[] projtype = { ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingGoldCoin>(), ModContent.ProjectileType<GlowingSilverCoin>(), ModContent.ProjectileType<GlowingCopperCoin>() };
						int[] projdamage = { 25, 30, 30, 50, 60, 60 };
						float[] projspeed = { 1f, 1f, 1f, 9f, 8f, 7f };
						if (a == phase - 1)
						{
							if (sinner2 < 4 && (npc.ai[0] + (i * 4)) % 30 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(Cratesvector[a, i], P.position, new Vector2(P.width, P.height), projtype[a + 1], (int)((double)npc.damage * ((double)projdamage[a + 1] / (double)defaultdamage)), projspeed[a + 1], 0, 1, true, 0, false, 110);
								if (projtype[a + 1] == ModContent.ProjectileType<GlowingPlatinumCoin>()) { itz[0].aiStyle = 18; IdgProjectile.AddOnHitBuff(itz[0].whoAmI, BuffID.ShadowFlame, 60 * 10); }
							}
						}

						Cratesvector[a, i] += ((P.Center - Cratesvector[a, i]) / (sinner2)) * ((Math.Max(compressvar - 1, 0) * 1));
					}
					float dist = (P.Center - Cratesvector[a, i]).Length();
					if (dist < themaxdist)
					{
						themaxdist = dist;
						theclostestcrate = Cratesvector[a, i];
					}
					//}
					Cratestype[a, i] = ItemID.WoodenCrate;
					if (a == 3)
					{
						Cratestype[a, i] = ItemID.IronCrate;
						if (npc.ai[3] > 300000 && i % 2 == 0)
						{
							Cratestype[a, i] = ModContent.ItemType<HavocGear.Items.DankCrate>();
						}
						else if (npc.ai[3] > 100000 && i % 2 == 0)
						{
							Cratestype[a, i] = ItemID.HallowedFishingCrate;
						}
						else
						{
							if (npc.ai[3] < -100000 && i % 2 == 0)
							{
								Cratestype[a, i] = evilcratetype;
							}
						}
					}
					if (a == 2)
					{
						Cratestype[a, i] = (i % 2 == 0) ? ItemID.JungleFishingCrate : ItemID.DungeonFishingCrate;
					}
					if (a == 1)
					{
						Cratestype[a, i] = (i % 2 == 0) ? ItemID.HallowedFishingCrate : (evilcratetype);
					}
					if (a == 0)
					{
						Cratestype[a, i] = ItemID.FloatingIslandFishingCrate;
					}

					if (!(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime))
					{
						//if (npc.ai[0]%600<350){
						//if ((npc.ai[0]/100)%Cratesperlayer[a]==i){
						//List<Projectile> projs=SgaLib.Shattershots(Cratesvector[a,i],P.position,new Vector2(P.width,P.height),ProjectileID.CopperCoin,35,12,0,1,true,0,true,150);
					}

				}
			}

		}

		/*public override bool CanHitPlayer(Player ply, ref int cooldownSlot){
			return true;
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return CanBeHitByPlayer(player);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return CanBeHitByPlayer(Main.player[projectile.owner]);
		}
		private bool? CanBeHitByPlayer(Player P){
		return true;
		}*/
		public virtual void CrateRelease(int layer)
		{
			double cratehp = npc.lifeMax * 0.50;
			for (int i = 0; i < Cratesperlayer[layer]; i = i + 1)
			{
				int crateToSpawn = mod.NPCType("CratrosityCrate" + Cratestype[layer, i].ToString());
				if (Cratestype[layer, i] == ModContent.ItemType<HavocGear.Items.DankCrate>())
					 crateToSpawn = mod.NPCType("CratrosityCrateDankCrate");

					int spawnedint = NPC.NewNPC((int)Cratesvector[layer, i].X, (int)Cratesvector[layer, i].Y, crateToSpawn);
				NPC spawned = Main.npc[spawnedint];
				spawned.value = Cratestype[layer, i];
				spawned.damage = (int)spawned.damage * (npc.damage / defaultdamage);
				if (Cratestype[layer, i] == ItemID.WoodenCrate)
				{
					spawned.aiStyle = 10;
					spawned.knockBackResist = 0.98f;
					spawned.lifeMax = (int)cratehp / 5;
					spawned.life = (int)cratehp / 5;
					spawned.damage = (int)50 * (npc.damage / defaultdamage);
				}
				if (Cratestype[layer, i] == ItemID.IronCrate)
				{
					spawned.aiStyle = 23;
					spawned.knockBackResist = 0.99f;
					spawned.lifeMax = (int)(cratehp * 0.30);
					spawned.life = (int)(cratehp * 0.30);
					spawned.damage = (int)60 * (npc.damage / defaultdamage);
				}
				if (Cratestype[layer, i] == ItemID.DungeonFishingCrate || Cratestype[layer, i] == ItemID.JungleFishingCrate)
				{
					spawned.aiStyle = Cratestype[layer, i] == ItemID.DungeonFishingCrate ? 56 : 86;
					spawned.knockBackResist = Cratestype[layer, i] == ItemID.DungeonFishingCrate ? 0.92f : 0.96f;
					spawned.lifeMax = Cratestype[layer, i] == ItemID.DungeonFishingCrate ? (int)(cratehp * 0.4) : (int)(cratehp * 0.30);
					spawned.life = Cratestype[layer, i] == ItemID.DungeonFishingCrate ? (int)(cratehp * 0.4) : (int)(cratehp * 0.30);
					spawned.damage = (Cratestype[layer, i] == ItemID.DungeonFishingCrate ? (int)(80) : (int)(80)) * ((int)npc.damage / defaultdamage);
				}
				if (Cratestype[layer, i] == ItemID.HallowedFishingCrate || Cratestype[layer, i] == evilcratetype)
				{
					spawned.aiStyle = Cratestype[layer, i] == ItemID.HallowedFishingCrate ? 63 : 62;
					spawned.knockBackResist = Cratestype[layer, i] == ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
					spawned.lifeMax = Cratestype[layer, i] == ItemID.HallowedFishingCrate ? (int)(cratehp * 0.60) : (int)(cratehp * 0.45);
					spawned.life = Cratestype[layer, i] == ItemID.HallowedFishingCrate ? (int)(cratehp * 0.60) : (int)(cratehp * 0.45);
					spawned.damage = (Cratestype[layer, i] == ItemID.HallowedFishingCrate ? (int)(100) : (int)(40)) * ((int)npc.damage / defaultdamage); ;
				}
				if (Cratestype[layer, i] == ModContent.ItemType<HavocGear.Items.DankCrate>())
				{
					spawned.aiStyle = 49;
					spawned.knockBackResist = 0.85f;
					spawned.lifeMax = (int)(cratehp * 0.65);
					spawned.life = (int)(cratehp * 0.65);
					spawned.damage = (int)(45 * (npc.damage / defaultdamage));
				}
				if (layer == 0)
				{
					spawned.knockBackResist = Cratestype[layer, i] == ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
					spawned.lifeMax = (int)(cratehp * 0.65);
					spawned.life = (int)(cratehp * 0.65);
				}
				//CratrosityCrate crate=npc.modNPC();
				//CratrosityCrate crate=(CratrosityCrate)spawned.modNPC;
				//crate.cratetype=Cratestype[layer,i];
			}
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("MoneyMismanagement"), 250, true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int a = 0; a < phase; a = a + 1)
			{
				for (int i = 0; i < Cratesperlayer[a]; i = i + 1)
				{
					Color glowingcolors1 = Main.hslToRgb((float)(npc.ai[0] / 30) % 1, 1f, 0.9f);
					//Texture2D texture = mod.GetTexture("Items/IronSafe");
					Texture2D texture = Main.itemTexture[Cratestype[a, i]];
					//Main.getTexture("Terraria/Item_" + ItemID.IronCrate);
					//Vector2 drawPos = npc.Center-Main.screenPosition;
					Vector2 drawPos = Cratesvector[a, i] - Main.screenPosition;
					spriteBatch.Draw(texture, drawPos, null, lightColor, (float)Cratesangle[a, i], new Vector2(16, 16), new Vector2(1, 1), SpriteEffects.None, 0f);
				}
			}

			Texture2D mainTex = GetType() == typeof(Cratrogeddon) ? Main.itemTexture[ItemID.GoldenCrate] : Main.itemTexture[ItemID.GoldenCrate];
			Main.spriteBatch.Draw(mainTex, npc.Center - Main.screenPosition, null, lightColor, npc.rotation, mainTex.Size() / 2f, npc.scale, default, 0);

			return false;
		}


	}
	public class GlowingCopperCoin : ModProjectile,IDrawAdditive
	{
		protected virtual int FakeID2 => ProjectileID.CopperCoin;
		protected virtual Color GlowColor => new Color(184, 115, 51);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Copper Coin");
		}

		public override string Texture
		{
			get { return "Terraria/Coin_" + 0; }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(FakeID2);
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.aiStyle = -1;
			projectile.timeLeft = 600;
		}

        public override bool PreKill(int timeLeft)
		{
			projectile.type = FakeID2;
			return true;
		}

		public override void AI()
		{
			projectile.localAI[0] += 1;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			int texHeight = tex.Height / 8;
			Vector2 offset = new Vector2(tex.Width, texHeight / 8) / 2f;
			int index = (int)(projectile.localAI[0] / 6f) % 8;

			spriteBatch.Draw(tex, drawPos, new Rectangle(0, texHeight * index, tex.Width, texHeight), Color.Lerp(GlowColor, Color.White, 0.50f)*0.50f, projectile.rotation, offset, projectile.scale+0.25f, SpriteEffects.None, 0f);
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawPos = projectile.Center-Main.screenPosition;
			int texHeight = tex.Height / 8;
			Vector2 offset = new Vector2(tex.Width, texHeight / 8) / 2f;
			int index = (int)(projectile.localAI[0]/6f)%8;

			spriteBatch.Draw(tex, drawPos, new Rectangle(0, texHeight*index, tex.Width, texHeight), Color.Lerp(lightColor,Color.White,0.50f), projectile.rotation, offset, projectile.scale, SpriteEffects.None, 0f);
			Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
			spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, GlowColor * 0.75f, projectile.rotation+MathHelper.Pi, tex2.Size() / 2f, projectile.scale / 1.5f, default, 0);
			return false;
        }
    }
	public class GlowingSilverCoin : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.SilverCoin;
		protected override Color GlowColor => Color.Silver;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Silver Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 1; }
		}
	}
	public class GlowingGoldCoin : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.GoldCoin;
		protected override Color GlowColor => Color.Gold;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Gold Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 2; }
		}
	}
	public class GlowingGoldCoinHoming : GlowingGoldCoin, IDrawAdditive
	{
		public override void AI()
        {
			base.AI();
			if (projectile.localAI[0] >= 0f)
				projectile.localAI[0] += 1;
			if (projectile.ai[1] < 1000)
            {
				projectile.ai[0] = Main.rand.Next(60, 200);
				projectile.ai[1] = 1000+(int)Player.FindClosest(projectile.Center, 0, 0);
				projectile.netUpdate = true;

			}
            else
            {
				Player player = Main.player[(int)projectile.ai[1]-1000];
				if (projectile.localAI[0] > projectile.ai[0] && projectile.localAI[0] >= 0)
                {
					Vector2 dotter = player.Center - projectile.Center;
					float speed = projectile.velocity.Length();
					projectile.velocity = (projectile.velocity.ToRotation().AngleLerp(dotter.ToRotation(), 0.05f)).ToRotationVector2()*speed;
					if (Vector2.Dot(Vector2.Normalize(dotter), Vector2.Normalize(projectile.velocity)) > 0.650f)
                    {
						projectile.localAI[0] = -10000;
					}

                }

            }

        }
	}
	public class GlowingPlatinumCoin : GlowingCopperCoin, IDrawAdditive
	{
		protected override int FakeID2 => ProjectileID.PlatinumCoin;
		protected override Color GlowColor => new Color(229, 228, 226);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Platinum Coin");
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 3; }
		}
	}
}