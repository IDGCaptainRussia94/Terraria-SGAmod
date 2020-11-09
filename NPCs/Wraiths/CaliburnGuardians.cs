using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.World.Generation;
using Idglibrary;


namespace SGAmod.NPCs.Wraiths
{
	[AutoloadBossHead]
	public class CaliburnGuardianHardmode : CaliburnGuardian, ISGABoss
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath of Caliburn");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void AI()
		{
			if (npc.ai[3] < 10)
			{
				npc.ai[2] = Main.rand.Next(0, 3);
				npc.ai[3] = 10;
				npc.netUpdate = true;
			}
			base.AI();
		}
		public override void NPCLoot()
		{
			SGAWorld.downedCaliburnGuardianHardmode = true;
			int[] types = { mod.ItemType("CaliburnTypeA"), mod.ItemType("CaliburnTypeB"), mod.ItemType("CaliburnTypeC") };
				npc.DropItemInstanced(npc.Center, new Vector2(npc.width,npc.height), types[(int)npc.ai[2]]);
		}
		protected override int caliburnlevel => 3;

	}




	[AutoloadBossHead]
	public class CaliburnGuardian : ModNPC, ISGABoss
	{
		public string Trophy() => "Caliburn"+ names[(int)npc.ai[2]] +"Trophy";
		public bool Chance() => true;

		protected string[] names = {"A","B","C"};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of Caliburn");
			Main.npcFrameCount[npc.type] = 1;
		}

		private float[] oldRot = new float[12];
		private Vector2[] oldPos = new Vector2[12];
		public float appear = 0f;
		public int nohit;
		protected virtual int caliburnlevel => SGAWorld.downedCaliburnGuardians;

		public virtual void trailingeffect()
		{

			Rectangle hitbox = new Rectangle((int)npc.position.X - 10, (int)npc.position.Y - 10, npc.height + 10, npc.width + 10);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

				if (Main.rand.Next(0, 10) == 1)
				{
					int typr = mod.DustType("TornadoDust");

					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, typr);
					Main.dust[dust].scale = (0.75f * appear)+(npc.velocity.Length()/50f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 normvel = npc.velocity;
					normvel.Normalize(); normvel *= 2f;

					Main.dust[dust].velocity = ((randomcircle / 1f) + (-normvel))-npc.velocity;
					Main.dust[dust].noGravity = true;

				}

			}

			oldRot[0] = npc.rotation;
			oldPos[0] = npc.Center;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.npcTexture[npc.type];
			if (npc.ai[2] == 1)
				tex = ModContent.GetTexture("SGAmod/Items/Weapons/Caliburn/CaliburnTypeB");
			if (npc.ai[2] == 2)
				tex = ModContent.GetTexture("SGAmod/Items/Weapons/Caliburn/CaliburnTypeC");

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{


				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				float alphaz2 = Math.Max((0.5f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f,0f);
				float fadein = MathHelper.Clamp(nohit+10f/60, 0f, 0.2f);
				for (float xx = 0; xx < 1f; xx += 0.20f)
				{
					Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + (npc.velocity * xx);
					spriteBatch.Draw(tex, drawPos, null, ((Color.Lerp(lightColor,Color.White, alphaz2) * alphaz) * (appear)) * 0.2f, oldRot[k], drawOrigin, npc.scale, npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}
			}
			return false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeA"; }
		}

		public override string BossHeadTexture => "Terraria/UI/PVP_2";
		public CaliburnGuardian()
		{
			nohit = 60;
		}
		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 15;
			npc.defDamage = 15;
			npc.defense = 5;
			npc.boss = true;
			npc.lifeMax = 1000;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath7;
			npc.value = 15000f;
			npc.knockBackResist = 0.85f;
			npc.aiStyle = -1;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Swamp_Remix");
			aiType = -1;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			if (caliburnlevel == 1)
			{
				npc.ai[3] = 1;
				npc.lifeMax = 2000;
				npc.value = 40000f;
				npc.damage = 20;
				npc.defDamage = 20;
				npc.defense = 7;
			}
			if (caliburnlevel == 2)
			{
				npc.ai[3] = 2;
				npc.lifeMax = 3500;
				npc.value = 100000f;
				npc.damage = 50;
				npc.defDamage = 50;
				npc.defense = 10;
			}
			if (caliburnlevel > 2)
			{
				npc.ai[3] = 2;
				npc.lifeMax = 25000;
				npc.value = 100000f;
				npc.damage = 75;
				npc.defDamage = 75;
				npc.defense = 30;
			}
		}
		public override void NPCLoot()
		{
			SGAWorld.downedCaliburnGuardians = Math.Min(3, SGAWorld.downedCaliburnGuardians+1);

			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendData(MessageID.WorldData);
				SGAWorld.downedCaliburnGuardiansPoints += 1;

				ModPacket packet = SGAmod.Instance.GetPacket();
				packet.Write((int)996);
				packet.Write(SGAWorld.downedCaliburnGuardians);
				packet.Write(SGAWorld.downedCaliburnGuardiansPoints);
				packet.Send();
			}

			if (Main.netMode==NetmodeID.SinglePlayer)
			SGAWorld.downedCaliburnGuardiansPoints += 1;


			Achivements.SGAAchivements.UnlockAchivement("Caliburn", Main.LocalPlayer);
		}
		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public void DropBolders()
		{
			Player P = Main.player[npc.target];
			Vector2 theside = new Vector2((P.Center.X - npc.Center.X > 0 ? -300f : 300f)*(0.5f+(float)Math.Sin((double)npc.ai[0]/120f)*0.7f), -200);

			Vector2 itt = ((P.Center + theside) - npc.Center);
			float locspeed = 0.25f;

			npc.velocity = npc.velocity * 0.90f;

			itt.Normalize();
			npc.velocity = npc.velocity + (itt * locspeed);

			npc.rotation = ((float)Math.Cos((double)npc.ai[0] / 6f)*1f)+Math.Max(0f,MathHelper.ToRadians(npc.localAI[0]*8f));

			if (npc.localAI[0] > 0)
			{
				npc.ai[0] -= 1;

				int typr = mod.DustType("TornadoDust");
				double angle = npc.rotation+MathHelper.ToRadians(-45);
				Vector2 here = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle))*new Vector2(npc.spriteDirection < 0 ? 1f : 1f, 1f);

				int dust = Dust.NewDust((npc.Center-new Vector2(6,6)) + here * 8f, 12, 12, typr);
				Main.dust[dust].scale = 0.75f;
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = npc.velocity+ here*3f;

			}

			if (npc.ai[0] % 90 == 0 && npc.localAI[0]<1)
			{
				Idglib.Shattershots(npc.Center, P.Center + new Vector2((P.Center.X - npc.Center.X) > 0 ? 600 : -600, 0), new Vector2(P.width, P.height), ProjectileID.Boulder, 50, 3, caliburnlevel * 50, 1+ caliburnlevel, true, 0, true, 300);

			}



		}

		public void DeployTraps()
		{
			Player P = Main.player[npc.target];
			Vector2 theside = new Vector2((P.Center.X - npc.Center.X > 0 ? -300f : 300f) * (0.5f + (float)Math.Sin((double)npc.ai[0] / 120f) * 0.7f), -200);
			npc.ai[1] += 1;
			for (int f = 0; f < (npc.ai[1]*(360f/50f))%360; f = f + 1)
			{
				int typr = mod.DustType("TornadoDust");
				double angle = MathHelper.ToRadians(f);
				Vector2 here = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

				int dust = Dust.NewDust(npc.Center + here*24f, 0,0, typr);
				Main.dust[dust].scale = (0.5f * appear) + (npc.velocity.Length() / 50f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Vector2 normvel = npc.velocity;
				normvel.Normalize(); normvel *= 2f;

				npc.rotation = npc.rotation.AngleLerp((P.Center-npc.Center).ToRotation()-MathHelper.ToRadians(-45f*npc.spriteDirection), 0.1f);

				Main.dust[dust].velocity = (here*3f);
				Main.dust[dust].noGravity = true;
			}


			if (npc.ai[1] > 300)
			{
				npc.ai[0] = 0;
				npc.ai[1] = 0;
			}

			if (npc.ai[1] % 50 == 49)
			{
				List<Vector2> heremaybe = new List<Vector2>();

				for (int zz = -1; zz < 2+ caliburnlevel; zz = zz + 1)
				{
					for (int f = -10; f < 10; f = f + 1)
					{
						if (Main.tile[(int)(P.Center.X / 16) + f, (int)(P.Center.Y / 16) + zz].active())
						{
							heremaybe.Add(new Vector2((int)(P.Center.X / 16f) + f, (int)(P.Center.Y / 16f) + 1));
						}
					}
				}
					if (heremaybe.Count > 0)
					{
					for (int f = 0; f < 1+ caliburnlevel; f = f + 1)
					{
						Vector2 there = heremaybe[Main.rand.Next(heremaybe.Count)];
						WorldGen.placeTrap((int)there.X, (int)there.Y, (caliburnlevel > 0 && f>0) ? 3 : 0);
					}

					if (Main.netMode == 2)
					{
						NetMessage.SendData(MessageID.WorldData);
					}
					//WorldGen.placeTrap((int)(P.Center.X / 16f), (int)(P.Center.Y / 16f) + 1, 0);
				}
			}

			npc.velocity *= 0.85f;




		}


		public override void AI()
		{
			npc.localAI[0] -= 1;
			appear = Math.Max(appear - 0.03f, Math.Min(appear+0.025f,0.50f));
			trailingeffect();
			npc.knockBackResist = 0.85f;
			nohit += 1;

			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
					npc.active = false;
				}
			}
			else
			{
				npc.dontTakeDamage = false;
				if (!P.GetModPlayer<SGAPlayer>().DankShrineZone)
					npc.dontTakeDamage = true;

					if (npc.ai[1] < 1)
					{
						npc.spriteDirection = npc.velocity.X > 0 ? -1 : 1;
						npc.ai[0] += 1;
					}

				float floattime = (90 - (caliburnlevel * 10));

				Vector2 theside = new Vector2(P.Center.X - npc.Center.X > 0 ? -200 : 200, 0);
				if (npc.ai[0] % 600 < 280 && !(npc.ai[0] % 1150 > 700))
				{
					theside = new Vector2(P.Center.X - npc.Center.X > 0 ? -120 : 120, 0);
					theside *= 0.5f + (float)Math.Cos(-npc.ai[0] / (floattime / 4f)) * 0.20f;
				}

				Vector2 itt = ((P.Center + theside) - npc.Center);
				float locspeed = 0.25f;

				if (npc.ai[1] > 0)
				{
					DeployTraps();
					return;
				}

				if (npc.ai[0] % 1150 > 800)
				{
					DropBolders();
				}
				else
				{
					npc.localAI[0] = 100;

					if (npc.ai[0] % 600 > 350)
					{
						nohit = -10;
						if (Main.expertMode)
						npc.knockBackResist = 0f;
						npc.damage = (int)npc.defDamage * 3;
						itt = itt = (P.Center - npc.Center + new Vector2(0, -8));
						npc.rotation = npc.rotation + (0.65f * npc.spriteDirection);

						if (npc.ai[0] % (70) == 0)
						{
							if (caliburnlevel > 0)
							Idglib.Shattershots(npc.Center, npc.Center-npc.velocity*26f, new Vector2(P.width, P.height), ProjectileID.CrystalShard, 10, caliburnlevel * 8f, caliburnlevel * 60, 8 + caliburnlevel * 8, true, 0, false, 200);
							if (caliburnlevel > 0)
							Idglib.Shattershots(npc.Center, npc.Center + npc.velocity * 26f, new Vector2(P.width, P.height), ProjectileID.EnchantedBeam, 20, 6f, 30, caliburnlevel, true, 0, false, 200);

							npc.velocity = npc.velocity * 0.96f;
							npc.velocity = npc.velocity + (itt * locspeed);
							itt.Normalize();
							npc.velocity = itt * 24f;
							appear = 0.9f;
						}
						locspeed = 0.15f;

					}
					else
					{
						npc.damage = (int)npc.defDamage;
						if (npc.ai[0] % 300 < 60)
						{
							locspeed = 2.5f;
							npc.velocity = npc.velocity * 0.92f;
						}
						else
						{
							if (npc.ai[0] > 1600 && Main.expertMode)
							npc.ai[1] = 1;
							npc.velocity = npc.velocity * 0.96f;

							if (npc.ai[0] % 90- (caliburnlevel * 10) == 0)
							Idglib.Shattershots(npc.Center, P.Center + new Vector2((P.Center.X - npc.Center.X) > 0 ? 200 : -200, 0), new Vector2(P.width, P.height), ProjectileID.DiamondBolt, 15, 5, caliburnlevel * 5, 1+ caliburnlevel, true, 0, false, 200);


						}

						npc.rotation = (float)npc.velocity.X * 0.09f;
						locspeed = 0.5f;
					}

					if (npc.ai[0] % 600 > 350 && npc.ai[0] % 300 > 60)
					{
						npc.velocity = npc.velocity * 0.96f;
					}

					itt.Normalize();
					npc.velocity = npc.velocity + (itt * locspeed);
				}

			}

		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return nohit<1;
		}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.expertMode || Main.rand.Next(2) == 0)
			{
				player.AddBuff(BuffID.Bleeding, 60*10, true);
			}
		}


	}

}

