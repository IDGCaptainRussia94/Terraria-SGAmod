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
using System.Linq;

namespace SGAmod.NPCs.Cratrosity
{
	public class CratrosityInstancedCrate
    {
		public int crateType;
		public int crateRing;
		public float crateAngle=0;
		public float crateDist;
		public Vector2 crateVector;
		public int crateState = 0;
		public int crateTimer = 0;
		public Cratrosity boss;

		public CratrosityInstancedCrate(Cratrosity boss,int crateType,float crateDist,int crateRing)
        {
			this.crateType = crateType;
			this.crateDist = crateDist;
			this.crateRing = crateRing;
			this.boss = boss;
		}

		public bool Update()
        {
			crateTimer += 1;
			return true;
		}

		public void Release(float cratehp)
        {
			NPC npc = boss.npc;
			Mod mod = boss.mod;
			int crateToSpawn = mod.NPCType("CratrosityCrate" + crateType.ToString());
			if (crateType == ModContent.ItemType<HavocGear.Items.DankCrate>())
				crateToSpawn = mod.NPCType("CratrosityCrateDankCrate");

			int spawnedint = NPC.NewNPC((int)crateVector.X, (int)crateVector.Y, crateToSpawn);
			NPC spawned = Main.npc[spawnedint];
			Item cratetypeitem = new Item();
			cratetypeitem.SetDefaults(crateType);

			spawned.value = cratetypeitem.value;
			spawned.damage = (int)spawned.damage * (npc.damage / boss.defaultdamage);
			if (crateType == ItemID.WoodenCrate)
			{
				spawned.aiStyle = 10;
				spawned.knockBackResist = 0.98f;
				spawned.lifeMax = (int)cratehp / 5;
				spawned.life = (int)cratehp / 5;
				spawned.damage = (int)50 * (npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ItemID.IronCrate)
			{
				spawned.aiStyle = 23;
				spawned.knockBackResist = 0.99f;
				spawned.lifeMax = (int)(cratehp * 0.30);
				spawned.life = (int)(cratehp * 0.30);
				spawned.damage = (int)60 * (npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ItemID.DungeonFishingCrate || crateType == ItemID.JungleFishingCrate)
			{
				spawned.aiStyle = crateType == ItemID.DungeonFishingCrate ? 56 : 86;
				spawned.knockBackResist = crateType == ItemID.DungeonFishingCrate ? 0.92f : 0.96f;
				spawned.lifeMax = crateType == ItemID.DungeonFishingCrate ? (int)(cratehp * 0.4) : (int)(cratehp * 0.30);
				spawned.life = crateType == ItemID.DungeonFishingCrate ? (int)(cratehp * 0.4) : (int)(cratehp * 0.30);
				spawned.damage = (crateType == ItemID.DungeonFishingCrate ? (int)(80) : (int)(80)) * ((int)npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ItemID.HallowedFishingCrate || crateType == Cratrosity.EvilCrateType)
			{
				spawned.aiStyle = crateType == ItemID.HallowedFishingCrate ? 63 : 62;
				spawned.knockBackResist = crateType == ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
				spawned.lifeMax = crateType == ItemID.HallowedFishingCrate ? (int)(cratehp * 0.60) : (int)(cratehp * 0.45);
				spawned.life = crateType == ItemID.HallowedFishingCrate ? (int)(cratehp * 0.60) : (int)(cratehp * 0.45);
				spawned.damage = (crateType == ItemID.HallowedFishingCrate ? (int)(100) : (int)(40)) * ((int)npc.damage / boss.defaultdamage);
				return;
			}
			if (crateType == ModContent.ItemType<HavocGear.Items.DankCrate>())
			{
				spawned.aiStyle = 49;
				spawned.knockBackResist = 0.85f;
				spawned.lifeMax = (int)(cratehp * 0.65);
				spawned.life = (int)(cratehp * 0.65);
				spawned.damage = (int)(45 * (npc.damage / boss.defaultdamage));
				return;
			}
			//if (layer == 0)
			//{
				spawned.knockBackResist = crateType == ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
				spawned.lifeMax = (int)(cratehp * 0.65);
				spawned.life = (int)(cratehp * 0.65);
			//}
		}

		public void Draw(SpriteBatch spriteBatch,Color drawColor)
        {
			Texture2D texture = Main.itemTexture[crateType];
			Vector2 drawPos = crateVector - Main.screenPosition;
			spriteBatch.Draw(texture, drawPos, null, drawColor, crateAngle, texture.Size() / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
		}

	}



	[AutoloadBossHead]
	public class Cratrosity : ModNPC, ISGABoss
	{
		public string Trophy() => GetType() == typeof(Cratrosity) ? "CratrosityTrophy" : "CratrogeddonTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0;

		public string RelicName() => GetType() == typeof(Cratrosity) ? "Cratrosity" : "Cratrogeddon";
		public void NoHitDrops()
		{
			Item.NewItem(npc.position,npc.Hitbox.Size(),ModContent.ItemType<Items.Accessories.AvariceRing>());
		}

		public Vector2 offsetype = new Vector2(0, 0);
		public int phase = 5;
		public int defaultdamage = 60;
		public int themode = 0;
		public float compressvar = 1;
		public float compressvargoal = 1;
		public int doCharge = 0;
		public bool init = false;
		public static int EvilCrateType => WorldGen.crimson ? ItemID.CrimsonFishingCrate : ItemID.CorruptFishingCrate;
		Vector2 theclostestcrate = new Vector2(0, 0);
		public int[] Cratesperlayer = new int[5] { 0, 0, 0, 0, 0};
		public List<List<CratrosityInstancedCrate>> cratesPerRing = new List<List<CratrosityInstancedCrate>>();
		public int postmoonlord = 0;
		public float nonaispin = 0f;
		public bool firstTimeSetup = true;

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
			npc.lifeMax = 10000;
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
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
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
				if (phase < 4 && GetType() == typeof(Cratrosity))
                {
					npc.localAI[3] = 1;
					npc.frameCounter = 1;
                }
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

        public override bool? CanBeHitByItem(Player player, Item item)
        {
			return false;
        }
        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
			return false;
		}

        public override bool PreAI()
        {
			bool newoneneeded = true;
			if (NPC.CountNPCS(ModContent.NPCType<CratrosityHitBox>()) > 0)
			{
				foreach (NPC npc2 in Main.npc.Where(testby => testby.active && testby.type == ModContent.NPCType<CratrosityHitBox>()))
				{
					if (npc2.realLife == npc.whoAmI && npc2.type == ModContent.NPCType<CratrosityHitBox>())
					{
						newoneneeded = false;
						break;
					}
				}
			}

			if (newoneneeded)
			{
				//npc.dontTakeDamage = true;
				int npc2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<CratrosityHitBox>());
				Main.npc[npc2].realLife = npc.whoAmI;
				Main.npc[npc2].netUpdate = true;
				//Main.NewText("new one");
				init = true;
			}

			return true;
        }

		public Player Target => Main.player[npc.target];

        public override void AI()
		{

			doCharge -= 1;
			Player P = Target;
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

				if (npc.localAI[3] > 0)
				{
					if (npc.localAI[3] < 100)
					{
						npc.dontTakeDamage = true;
						npc.velocity *= 0.85f;
						npc.localAI[3] += 1;
						if (phase > 2 && npc.localAI[3]>=0)
						{
							npc.frameCounter = 16f * (1f - ((npc.localAI[3]) / 100f));
                        }
                        else
                        {
							npc.frameCounter = 19 + (npc.localAI[3]/3f) % 3;
						}

						if (npc.localAI[3] == 50)
						{
							npc.extraValue += Item.buyPrice(0, 1, 0, 0);
							npc.moneyPing(npc.Center);

						}

						goto gohere;
					}
					else
					{
						
						if (npc.localAI[3] < 200)
						{
							//if (npc.frameCounter < 5)
							//{
							npc.frameCounter += 0.25f;
							npc.frameCounter %= 8;
							if (npc.frameCounter >= 5 && npc.frameCounter < 8)
								npc.frameCounter = 5 - npc.frameCounter;
							//}
						}
						
						if (npc.localAI[3] >= 200 && npc.localAI[3] < 400)
						{
							npc.localAI[3]++;
							//if (npc.frameCounter < 5)
							//{
							if (npc.frameCounter < 18)
								npc.frameCounter = 18;

							if (npc.frameCounter < 21)
								npc.frameCounter += 0.25f;

							if (npc.frameCounter > 18 && npc.localAI[3]>215)
								npc.frameCounter -= 0.25f;

							if (npc.localAI[3] > 230)
								npc.localAI[3] = 100;
							//}
						}
						if (npc.localAI[3] >= 400)
						{
							npc.localAI[3]++;
							npc.frameCounter = 21f * (((npc.localAI[3]-400) / 100f));
							if (npc.localAI[3] > 460)
								npc.localAI[3] = 201;
						}
						
						

					}
					npc.dontTakeDamage = false;
				}


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
							if (phase < 1)
								theclostestcrate = Vector2.Zero;

							npc.velocity = new Vector2(-theammount * ((GetType() == typeof(Cratrogeddon)) ? 15 : 10), 0);
							if (npc.ai[0] % 15 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(npc.Center, P.Center + new Vector2(0, P.Center.Y > npc.Center.Y ? 600 : -600), new Vector2(0, 0), ModContent.ProjectileType<GlowingCopperCoin>(), (int)(npc.damage * (10.00 / defaultdamage)), 10, 0, 1, true, 0, true, 100);
							}
							if (npc.ai[0] % 40 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(theclostestcrate, P.Center, new Vector2(0, 0), ModContent.ProjectileType<GlowingGoldCoin>(), (int)(npc.damage * (30.00 / defaultdamage)), 10, 0, 1, true, 0, true, 200);
							}
							if (((npc.ai[0] + 20) % 40 == 0) && Main.expertMode)
							{
								List<Projectile> itz = Idglib.Shattershots(theclostestcrate, P.Center + new Vector2(0, P.Center.Y > theclostestcrate.Y ? 600 : -600), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (20.00 / defaultdamage)), 10, 0, 1, true, 0, true, 200);
								SGAprojectile modeproj = itz[0].GetGlobalProjectile<SGAprojectile>();
								//modeproj.splittingcoins = true;
								//modeproj.splithere = P.Center;
							}
							if (phase < 4)
							{
								if (npc.ai[0] % 8 == 0)
								{
									Idglib.Shattershots(npc.Center, npc.Center + new Vector2(-npc.velocity.X, 0), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (20.00 / defaultdamage)), 25, 0, 1, true, 0, false, 40);
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
										Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingCopperCoin>(), (int)(npc.damage * (10f / (float)defaultdamage)), 10, 0, 1, true, 0, true, 150);
									}
									break;
								}
							case 4:
								{
									if (npc.ai[0] % 10 == 0)
									{
										Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (20f / (float)defaultdamage)), 14, 0, 1, true, 0, true, 100);
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
										List<Projectile> itz = Idglib.Shattershots(npc.Center, npc.Center + new Vector2(0, -5), new Vector2(0, 0), ModContent.ProjectileType<GlowingSilverCoin>(), (int)(npc.damage * (20f / (float)defaultdamage)), 7, 360, 2, true, npc.ai[0] / 20, true, 300);
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
								if (npc.localAI[3]>0)
								npc.localAI[3] = 400;


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

								if (npc.localAI[3] > 0)
									npc.localAI[3] = 201;

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

			gohere:

			npc.defense = (int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.WoodenCrate.ToString()))) * 5 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.IronCrate.ToString()))) * 6 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.GoldenCrate.ToString()))) * 6 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.DungeonFishingCrate.ToString()))) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.JungleFishingCrate.ToString()))) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + EvilCrateType.ToString()))) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.HallowedFishingCrate.ToString()))) * 10 +
			NPC.CountNPCS(mod.NPCType("CratrosityCrateDankCrate")) * 10 +
			(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.FloatingIslandFishingCrate.ToString()))) * (30);
			npc.defense *= Main.expertMode ? 4 : 2;
			npc.defense += Main.expertMode ? 20 : 0;
			OrderOfTheCrates(P);



		}

		public virtual void OrderOfTheCrates(Player P)
		{
			if (firstTimeSetup)
            {
				for (int layer = 0; layer < phase; layer++)
				{
					Cratesperlayer[layer] = 4 + (layer * 4);
					List<CratrosityInstancedCrate> cratesInThisLayer = new List<CratrosityInstancedCrate>();

					for (int i = 0; i < Cratesperlayer[layer]; i = i + 1)
					{
						int crateType = ItemID.WoodenCrate;
						if (layer == 3)
						{
							crateType = ItemID.IronCrate;
							if (npc.ai[3] > 300000 && i % 2 == 0)
							{
								crateType = ModContent.ItemType<HavocGear.Items.DankCrate>();
							}
							else if (npc.ai[3] > 100000 && i % 2 == 0)
							{
								crateType = ItemID.HallowedFishingCrate;
							}
							else
							{
								if (npc.ai[3] < -100000 && i % 2 == 0)
								{
									crateType = EvilCrateType;
								}
							}
						}
						if (layer == 2)
						{
							crateType = (i % 2 == 0) ? ItemID.JungleFishingCrate : ItemID.DungeonFishingCrate;
						}
						if (layer == 1)
						{
							crateType = (i % 2 == 0) ? ItemID.HallowedFishingCrate : (EvilCrateType);
						}
						if (layer == 0)
						{
							crateType = ItemID.FloatingIslandFishingCrate;
						}
						cratesInThisLayer.Add(new CratrosityInstancedCrate(this, crateType, 8, layer));
					}
					cratesPerRing.Add(cratesInThisLayer);

				}
				firstTimeSetup = false;
			}


			nonaispin = nonaispin + 1f;
			compressvar += (compressvargoal - compressvar) / 20;
			float themaxdist = 99999f;
			for (int layer = 0; layer < phase; layer++)
			{
				for (int index = 0; index < cratesPerRing[layer].Count; index++)
				{
					CratrosityInstancedCrate crate = cratesPerRing[layer][index];

					if (!crate.Update())
						continue;

					crate.crateAngle = (layer % 2 == 0 ? 1 : -1) * ((nonaispin * 0.01f) * (1f + layer / 3f)) + MathHelper.TwoPi * ((index / (float)cratesPerRing[layer].Count));
					crate.crateDist = compressvar * 20f + ((float)layer * 20f) * compressvar;
					//Cratesangle[a, i] = (a % 2 == 0 ? 1 : -1) * ((nonaispin * 0.01f) * (1f + a / 3f)) + 2.0 * Math.PI * ((i / (double)Cratesperlayer[a]));
					//Cratesdist[a, i] = compressvar * 20f + ((float)a * 20f) * compressvar;

					float theexpand = 0f;

					if (themode > 0)
					{
						theexpand = (((index / 1f) * (layer + 1f))) * (themode / 30f);
					}

					crate.crateVector += ((crate.crateDist * (new Vector2((float)Math.Cos(crate.crateAngle), (float)Math.Sin(crate.crateAngle))) + npc.Center) - crate.crateVector) / (theexpand + (Math.Max((((compressvar) - 1) * (2 + (layer * 1))), 1)));
					//if (index)
					//crate.crateVector += Main.rand.NextVector2Circular(32, 32);

					float sinner = npc.ai[0] + ((float)(index * 5) + (layer * 14));
					float sinner2 = (float)(10f + (Math.Sin(sinner / 30f) * 7f));

					if (compressvar > 1.01)
					{
						int[] projtype = { ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingPlatinumCoin>(), ModContent.ProjectileType<GlowingGoldCoin>(), ModContent.ProjectileType<GlowingSilverCoin>(), ModContent.ProjectileType<GlowingCopperCoin>() };
						int[] projdamage = { 60, 50, 40, 30, 20, 10 };
						float[] projspeed = { 1f, 1f, 1f, 9f, 8f, 7f };
						if (layer == phase - 1)
						{
							if (sinner2 < 4 && (npc.ai[0] + (index * 4)) % 30 == 0)
							{
								List<Projectile> itz = Idglib.Shattershots(crate.crateVector, P.position, new Vector2(P.width, P.height), projtype[layer + 1], (int)((double)npc.damage * ((double)projdamage[layer + 1] / (double)defaultdamage)), projspeed[layer + 1], 0, 1, true, 0, false, 110);
								if (projtype[layer + 1] == ModContent.ProjectileType<GlowingPlatinumCoin>()) { itz[0].aiStyle = 18; IdgProjectile.AddOnHitBuff(itz[0].whoAmI, BuffID.ShadowFlame, 60 * 10); }
							}
						}

						crate.crateVector += ((P.Center - crate.crateVector) / (sinner2)) * ((Math.Max(compressvar - 1, 0) * 1));
					}
					float dist = (P.Center - crate.crateVector).Length();
					if (dist < themaxdist)
					{
						themaxdist = dist;
						theclostestcrate = crate.crateVector;
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
			float cratehp = npc.lifeMax * 0.50f;
			List<CratrosityInstancedCrate> cratesInThisLayer = cratesPerRing[layer];

			for (int i = 0; i < cratesInThisLayer.Count; i = i + 1)
			{
				cratesInThisLayer[i].Release(cratehp);
			}
			npc.lifeMax = (int)(npc.lifeMax * 0.75f);
			npc.life = npc.lifeMax;
			cratesPerRing.RemoveAt(layer);
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("MoneyMismanagement"), 250, true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (firstTimeSetup)
				return false;

			for (int layer = 0; layer < phase; layer++)
			{
				List<CratrosityInstancedCrate> cratesInThisLayer = cratesPerRing[layer];
				for (int i = 0; i < cratesInThisLayer.Count; i++)
				{
					Point there = (cratesInThisLayer[i].crateVector / 16).ToPoint();
					cratesInThisLayer[i].Draw(spriteBatch,Lighting.GetColor(there.X, there.Y,lightColor));
					//Color glowingcolors1 = Main.hslToRgb((float)(npc.ai[0] / 30) % 1, 1f, 0.9f);
					//Texture2D texture = mod.GetTexture("Items/IronSafe");
					//Texture2D texture = Main.itemTexture[Cratestype[a, i]];
					//Main.getTexture("Terraria/Item_" + ItemID.IronCrate);
					//Vector2 drawPos = npc.Center-Main.screenPosition;
					//Vector2 drawPos = Cratesvector[a, i] - Main.screenPosition;
					//spriteBatch.Draw(texture, drawPos, null, lightColor, (float)Cratesangle[a, i], new Vector2(16, 16), new Vector2(1, 1), SpriteEffects.None, 0f);
				}
			}

			if (GetType() != typeof(Cratrogeddon) && (npc.localAI[3] > 0 || phase<3))
			{
				Texture2D trueFormTexture = mod.GetTexture("NPCs/Cratrosity/Cratosity");
				int width = trueFormTexture.Width;
				int height = trueFormTexture.Height;
				int frames = 22;
				int framesHeight = height / frames;
				int frame = (int)npc.frameCounter;

				float direction = Math.Sign(npc.velocity.X);
				npc.frame = new Rectangle(0, frame * framesHeight, width, framesHeight);
				Main.spriteBatch.Draw(trueFormTexture, npc.Center - Main.screenPosition, npc.frame, lightColor, (float)Math.Pow(Math.Abs(npc.velocity.X/60f),0.75f)* direction, new Vector2(width, framesHeight)/2f, npc.scale, default, 0);

				return false;
			}

			Texture2D mainTex = GetType() == typeof(Cratrogeddon) ? mod.GetTexture("NPCs/Cratrosity/TitanCrate") : Main.itemTexture[ItemID.GoldenCrate];
			Main.spriteBatch.Draw(mainTex, npc.Center - Main.screenPosition, null, lightColor, npc.rotation, mainTex.Size() / 2f, npc.scale, default, 0);

			return false;
		}


	}

	public class CratrosityHitBox : ModNPC
	{

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
			npc.width = 160;
			npc.height = 160;
			npc.damage = 0;
			npc.defense = 50;
			npc.lifeMax = 5000000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath37;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.hide = true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
		}
		public override string Texture
		{
			get { return "Terraria/Coin_" + 0; }
		}

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
			return false;
        }

        public override void AI()
        {
			if (npc.realLife >= 0)
			{
				if (npc.realLife < 1)
                {
					//npc.StrikeNPCNoInteraction(100000, 0, 0, noEffect: true);
					return;
				}
				npc.dontTakeDamage = Main.npc[npc.realLife].dontTakeDamage;
				npc.Center = Main.npc[npc.realLife].Center;
				npc.defense = Main.npc[npc.realLife].defense;
				ModNPC modnpc = Main.npc[npc.realLife].modNPC;
				if (modnpc != null)
				{
					if (!Main.npc[npc.realLife].modNPC.GetType().IsSubclassOf(typeof(Cratrosity)))
					{
						//npc.StrikeNPCNoInteraction(100000, 0, 0, noEffect: true);
						return;
					}
					else
					{
						Cratrosity crate = Main.npc[npc.realLife].modNPC as Cratrosity;
						npc.width = 80 + (crate.phase * 16);
						npc.height = 80 + (crate.phase * 16);
					}
				}
			}
			//Main.NewText("exists "+ npc.Center);
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