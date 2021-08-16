using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;
using Terraria.Utilities;
using System.Linq;
using SGAmod.Effects;
using SGAmod.Items;

namespace SGAmod.NPCs
{

	[AutoloadBossHead]
	public class SPinkyTrue : SPinky, ISGABoss
	{
		int realcounter;
		List<NPC> SupremeArmy = new List<NPC>();
		float slimecalleffect = 0;
		float dashaim = 0;
		float roter = 0;
		float effectScale = 0f;
		int attackPhaseTime = 1200;//1200;
		SoundEffectInstance soundz;
		SoundEffectInstance soundfinish;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pink 'Singularit'Y");
			Main.npcFrameCount[npc.type] = 5;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}
		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 64;
			npc.damage = 200;
			npc.defense = 25;
			npc.lifeMax = 200000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.aiStyle = 1;
			npc.netAlways = true;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.aiStyle = -1;
			npc.boss = true;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			music = MusicID.Boss2;
			bossBag = mod.ItemType("SPinkyBag");
			npc.value = Item.buyPrice(0, 1, 0, 0);
			phase = 0;
			attackPhaseTime = 1200;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			if (npc.ai[0] < 400)
				return false;

			return base.CanHitPlayer(target, ref cooldownSlot);
		}

		public void UpdateSlime(NPC npc)
        {
			if (Main.netMode > 0)
			{
				ModPacket packet = mod.GetPacket();
				packet.Write((ushort)MessageType.UpdateLocalVars);
				packet.Write(npc.whoAmI);
				packet.WriteVector2(new Vector2(npc.localAI[0], npc.localAI[1]));
				packet.WriteVector2(new Vector2(npc.localAI[2], npc.localAI[3]));
				packet.Send();
			}
		}

        public override bool CheckDead()
        {
			if (npc.ai[0] < 1000300)
			{
				npc.life = 1337;
				npc.active = true;
				npc.ai[1] = 20;
				stopmoving = 1000000;
				npc.netUpdate = true;
				return false;
			}
			return true;
        }

        private bool IntroDeal()
		{
			realcounter = (int)(npc.ai[0]) - 400;
			getHitEffect -= 1f;
			npc.ai[0] += 1;
			npc.dontTakeDamage = false;
			stopmoving -= 1;
			npc.localAI[0] += 1f;


			if (npc.ai[0]>295)
				generalcounter += 1;

			if (soundz != null)
				soundz.Pitch = Main.rand.NextFloat(-0.5f, 0.5f);

			if (npc.ai[0] < 400)
			{
				if (npc.ai[0] < 300)
					slimecalleffect += 1f;

				if (npc.ai[0] == 2)
				{
					//npc.GivenName = "Not Goomza";
					SoundEffectInstance sound2 = Main.PlaySound(SoundID.DD2_EtherianPortalOpen, npc.Center);
					if (sound2 != null)
					{
						sound2.Pitch = 0.75f;
					}

				}

				if (npc.ai[0] < 300 && Main.rand.Next(80)<npc.ai[0])
                {
					Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
					int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y)+ offset*Main.rand.NextFloat(32f, npc.ai[0]>100 ? 32f : 96f), 0, 0, DustID.PurpleCrystalShard);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)(Main.rand.NextFloat(0.50f, 2.50f));
					Main.dust[dust].noGravity = true;
				}

				if (npc.ai[0] == 100)
				{
					Main.NewText("<???> PINKY...", 255, 100, 255);
				}

				if (npc.ai[0] == 50)
                {
					SoundEffectInstance sound2 = Main.PlaySound(SoundID.DD2_DefeatScene, npc.Center);
					if (sound2 != null)
					{
						sound2.Pitch = 0.75f;
					}

				}
				if (npc.ai[0] == 100)
                {
					RippleBoom.MakeShockwave(npc.Center, 8f, 10f, 40f, 60, 1f,true);

					for (int i = 0; i < 100; i += 1)
					{
						Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
						int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(0f, 32f), 0, 0, DustID.PurpleCrystalShard);
						Main.dust[dust].scale = 2f;
						Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(Main.rand.NextFloat(8.00f, 32f));
						Main.dust[dust].noGravity = true;
					}

					foreach (Player p in Main.player.Where(testby => testby.active && testby.Distance(npc.Center) < 320))
					{
						p.velocity += Vector2.Normalize(p.Center - npc.Center) * 24f;
					}
				}

				if (npc.ai[0] == 200)
				{
					Main.NewText("<???> WILL NOT...", 255, 100, 255);
				}
				if (npc.ai[0] >= 0 && npc.ai[0] < 300 && getHitEffect<-5)
					getHitEffect = 15f;

				if (npc.ai[0] == 300)
				{
					SGAmod.ProgramSkyAlpha = Math.Max(SGAmod.ProgramSkyAlpha, 0.005f);
					Main.NewText("<???> DIE!!!", 255, 100, 255);
					RippleBoom.MakeShockwave(npc.Center, 12f, 10f, 60f, 60, 1.5f, true);
					soundz = Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0);

					for (int i = 0; i < 200; i += 1)
					{
							Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
							int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(0f, 96f), 0, 0, DustID.PurpleCrystalShard);
							Main.dust[dust].scale = 2f;
							Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(Main.rand.NextFloat(2.00f, 8f));
							Main.dust[dust].noGravity = true;
					}

					Main.StartSlimeRain();
				}

				npc.dontTakeDamage = true;
				return false;
			}

			drawdist += (1f - drawdist) * 0.01f;

			P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || Main.dayTime)
				{
					float speed = ((-0.25f));
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
					npc.timeLeft = Math.Min(npc.timeLeft, 1);
				}
				return false;
			}
			return true;
		}

		public void SlimeCall(int callCount = 10)
        {
			if (P.Distance(npc.Center)<1200)
			stopmoving = 30;
			if (slimecalleffect<60)
			slimecalleffect += 1;

			if (npc.ai[0] == 1700)
			{
				if (aicounter<2)
				aicounter = 2;
			}
			if (npc.ai[0] % 20 == 0)
            {
				SoundEffectInstance sound2 = Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
				if (sound2 != null)
				{
					sound2.Pitch = 0.75f;
				}
			}

			if (npc.ai[0]%(10-phase) == 0)
			{

				int num7 = NPC.NewNPC((int)npc.Center.X+Main.rand.Next(-800,800), (int)Main.screenPosition.Y-100,1);
				Main.npc[num7].ai[0] = 1;
				if (Main.rand.Next(200) == 0)
				{
					Main.npc[num7].SetDefaults(-4);
				}
				else if (Main.expertMode)
				{
					if (Main.rand.Next(7) == 0)
					{
						Main.npc[num7].SetDefaults(-7);
					}
					else if (Main.rand.Next(3) == 0)
					{
						Main.npc[num7].SetDefaults(-3);
					}
				}
				else if (Main.rand.Next(10) == 0)
				{
					Main.npc[num7].SetDefaults(-7);
				}
				else if (Main.rand.Next(5) < 2)
				{
					Main.npc[num7].SetDefaults(-3);
				}
			}



			//for (int i = 0; i < callCount; i += 1)
			//NPC.SlimeRainSpawns(P.whoAmI);

			if (npc.ai[0] > 2000)
            {
				if (phase < 3)
				{
					npc.ai[0] = Main.rand.Next(905, 1200);
					npc.ai[1] = 0;
				}
                else
                {
					Main.NewText("<SUPREME PINKY> WARNING, EVENT SINGULARITY FORMING!", 255, 100, 255);
					npc.ai[0] = 5000;
					npc.ai[1] = 10;
					SoundEffectInstance sound2 = Main.PlaySound(SoundID.DD2_KoboldExplosion, npc.Center);
					if (sound2 != null)
					{
						sound2.Pitch = -0.25f;
					}

					soundz = Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 2);

				}
				npc.netUpdate = true;
			}

		}

		public void SlimeCommand(int type = 0)
		{
			int maxxer = 2900;
			goThere = P.MountedCenter + Vector2.Normalize(P.velocity + new Vector2(0, -0.05f)) * 640f;
			speed = new Vector2(0.25f, 0.45f);

			if (npc.ai[0] < 2800)
			{
				stopmoving = 30;
			}
			if (stopmoving > 0)
				friction /= 2f;

				if (npc.ai[0] == 2700)
			{

				RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 3f, true);
				soundz = Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0);
			}

			if (npc.ai[0] > 2700 && npc.ai[0]%8==0 && npc.ai[0] < maxxer-40)
            {
				foreach (NPC npc2 in SupremeArmy.OrderBy(testby => Main.rand.Next(100)).Where(testby => testby.ai[0] == 0 && testby.localAI[3]<-100))
                {
					Vector2 bex = new Vector2(P.Center.X, P.Center.Y);
					bex += Vector2.Normalize(bex - npc2.Center)*640f;
					npc2.ai[0] = 1;
					npc2.localAI[1] = bex.X;
					npc2.localAI[2] = bex.Y;
					npc2.localAI[3] = 30;
					UpdateSlime(npc2);
					npc2.netUpdate = true;

					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsysWrathShot, npc2.Center);
					if (sound != null)
					{
						sound.Pitch = -0.25f;
					}

					for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
					{
						Vector2 offset = f.ToRotationVector2();
						int dust = Dust.NewDust(npc2.Center + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
						Main.dust[dust].scale = 1.5f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
					}

					break;
                }

            }

			//for (int i = 0; i < callCount; i += 1)
			//NPC.SlimeRainSpawns(P.whoAmI);

			if (npc.ai[0] > maxxer)
			{
				/*if (SupremeArmy.Count > 8)
                {
					npc.ai[0] -= 300f;
					npc.netUpdate = true;
					return;
				}*/

				stopmoving = 60;
				if (SupremeArmy.Count > 8)
				{
					npc.ai[0] = Main.rand.Next(905, 1100);
					npc.ai[1] = 0;
				}
				else
				{
					npc.ai[0] = Main.rand.Next(405, 600);
					npc.ai[1] = 0;
				}

				npc.netUpdate = true;
			}

		}

		public void SlimeKamakaze(int type = 0)
		{
			goThere = circleLoc;
			speed = new Vector2(0.25f, 0.25f);

			if (npc.Distance(circleLoc) < 32 && npc.ai[0]<6050)
				npc.ai[0] = 6050;

			if (npc.ai[0] > 6050)
			{
				stopmoving = 120;
			}
			if (stopmoving > 0)
				friction /= 2f;

			if (npc.ai[0] == 6060 || npc.ai[0] == 6310)
			{

				RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 3f, true);
				soundz = Main.PlaySound(SoundID.Roar, (int)npc.Center.X, (int)npc.Center.Y, 0);

				if (npc.ai[0] == 6060)
				{
					List<NPC> armycommand = SupremeArmy.Where(testby => testby.ai[0] == 0).ToList();

					float index = 0;
					foreach (NPC npc2 in armycommand)
					{
						npc2.ai[0] = 4;
						npc2.localAI[1] = 0;
						npc2.localAI[2] = (index / armycommand.Count) * MathHelper.TwoPi;
						npc2.localAI[3] = 30;
						UpdateSlime(npc2);
						npc2.netUpdate = true;
						index += 1f;
					}
				}
			}

			//Teleport slimes
			if (npc.ai[0] > 6300)
			{
				List<NPC> armycommand = SupremeArmy.OrderBy(testby => Main.rand.Next(100)).Where(testby => testby.ai[0] == 4).ToList();

				if (armycommand.Count>0)
				{
					foreach (NPC npc2 in armycommand)
					{
						float sizer = (32f * armycommand.Count);
						Vector2 bex = P.Center + ((Vector2.UnitX * Main.rand.NextFloat(128f+sizer, 640f+sizer)).RotatedByRandom(MathHelper.TwoPi));

						npc2.ai[0] = 5;
						npc2.localAI[1] = bex.X;
						npc2.localAI[2] = bex.Y;
						npc2.localAI[3] = 30;

						UpdateSlime(npc2);
						npc2.netUpdate = true;

						Vector2 there = (bex - npc2.Center);

						Projectile.NewProjectile(npc2.Center, Vector2.Normalize(there)*1f, ProjectileID.DemonScythe, 50, 0f);
						Projectile.NewProjectile(npc2.Center + Vector2.Normalize(there)*64f, Vector2.Normalize(there), ModContent.ProjectileType<PinkyWarning>(), 5, 0f);

						SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsysWrathShot, bex);
						if (sound != null)
						{
							sound.Pitch = -0.25f;
						}

						for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
						{
							Vector2 offset = f.ToRotationVector2();
							int dust = Dust.NewDust(bex + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
							Main.dust[dust].scale = 1.5f;
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
						}

						npc.ai[0] = 6296;
						break;
					}

				}
			}

			//Slimes go BOOM! Laser light show!
			if (npc.ai[0] > 6360)
			{
				List<NPC> armycommand = SupremeArmy.OrderBy(testby => Main.rand.Next(10000)).Where(testby => testby.ai[0] == 5).ToList();

				if (armycommand.Count > 0)
				{
					foreach (NPC npc2 in armycommand)
					{
						Vector2 bex = new Vector2(npc2.localAI[1],npc2.localAI[2]);
						Vector2 wasHere = npc.Center;
						npc2.Center = bex;

						SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BallistaTowerShot, npc2.Center);
						if (sound != null)
						{
							sound.Pitch = -0.25f;
						}

						for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
						{
							Vector2 offset = f.ToRotationVector2();
							int dust = Dust.NewDust(bex + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
							Main.dust[dust].scale = 1.5f;
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity = f.ToRotationVector2() * 16f;

							float angle = (generalcounter / 60f) + f;
							Vector2 rotter = angle.ToRotationVector2();
						}

						npc.Center = npc2.Center;

						int proj=Projectile.NewProjectile(npc.Center, Vector2.Normalize(wasHere-npc.Center) * ((wasHere - npc.Center).Length()/2200f), SGAmod.Instance.ProjectileType("HellionBeam"), 100, 15f);

						Main.projectile[proj].timeLeft = 250;

						npc2.StrikeNPC(100000, 0, 0, noEffect: true);

						npc.ai[0] = 6355;
						break;
					}

				}
			}

			//for (int i = 0; i < callCount; i += 1)
			//NPC.SlimeRainSpawns(P.whoAmI);

			if (npc.ai[0] == 6370)
            {
				soundz = Main.PlaySound(SoundID.Zombie, (int)npc.Center.X, (int)npc.Center.Y, 105);
			}

			if (npc.ai[0] > 6400)
			{

				stopmoving = 60;
				if (SupremeArmy.Count > 8)
				{
					npc.ai[0] = Main.rand.Next(905, 1100);
					npc.ai[1] = 0;
				}
				else
				{
					npc.ai[0] = Main.rand.Next(405, 600);
					npc.ai[1] = 0;
				}

				npc.netUpdate = true;
			}

		}

		public void PhaseShift()
		{
			int maxxer = 4000;
			goThere = P.MountedCenter + Vector2.Normalize(P.velocity + new Vector2(0, -0.05f)) * 640f;
			speed = new Vector2(0.25f, 0.45f);

			npc.dontTakeDamage = true;

			stopmoving = 30;
			friction /= 2f;

			if (npc.ai[0] == 3900)
            {
				phase += 1;
				if (phase == 3)
                {
					aicounter = 5;

				}
				//if (phase>1)
				//roter = MathHelper.TwoPi / 400f;

				slimecalleffect = 100;
				getHitEffect = 200;
				npc.ai[2] += 1;
				RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 3f, true);
				soundz = Main.PlaySound(SoundID.DD2_BetsyScream, (int)npc.Center.X, (int)npc.Center.Y);
			}

			if (npc.ai[0] > maxxer)
			{

				npc.ai[0] = Main.rand.Next(1500, 1800);
				npc.ai[1] = 0;

				npc.netUpdate = true;
			}

		}

		public void Dying()
		{
			npc.dontTakeDamage = true;
			if (npc.ai[0] < 1000000)
            {
				npc.ai[0] = 1000000;
				if (soundfinish != null)
				soundfinish.Stop();

				Projectile.NewProjectile(npc.Center,Vector2.Zero, ModContent.ProjectileType<PinkyExplode>(), 50, 0f);

				soundfinish = Main.PlaySound(SoundID.DD2_WinScene, npc.Center);
				npc.netUpdate = true;
			}
			if (soundfinish != null)
			{
				soundfinish.Pitch = (npc.ai[0]-1000200)/300f;
			}
			if (npc.ai[0] % 10 == 0)
			{
				SoundEffectInstance sound3 = Main.PlaySound(SoundID.DD2_EtherianPortalSpawnEnemy, npc.Center);
				if (sound3 != null)
				{
					sound3.Pitch = (npc.ai[0] - 1000200) / 300f;
					sound3.Volume = 0.15f;
				}
			}

			effectScale /= 1.005f;

			for (int i = 0; i < 10; i += 1)
			{
				if (Main.rand.Next(200) < npc.ai[0] - 1000000 && npc.ai[0]<1000420)
				{
					Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
					int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f, 64f+(npc.ai[0]-1000000)*2f), 0, 0, DustID.PurpleCrystalShard);
					Main.dust[dust].scale = 1.5f;
					Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)(Main.rand.NextFloat(0.50f, 2.50f));
					Main.dust[dust].noGravity = true;
				}
			}

			if (npc.ai[0] == 1000500)
			{
				RippleBoom.MakeShockwave(npc.Center, 12f, 3f, 20f, 200, 2f, true);
				npc.StrikeNPCNoInteraction(100000, 0, 0, true, true);
			}

		}

			public void FinalAttack()
		{
			stopmoving = 10000;
			effectScale = Math.Min(effectScale+1,3000);
			if (npc.ai[0] % 90 == 0)
            {
				RippleBoom.MakeShockwave(npc.Center, 8f, 1f, 10f, 60, 3f, true);
			}
			circlesize += (1600f - circlesize) / 1000f;
			circleLoc += (npc.Center - circleLoc) / 400f;

			if (soundfinish == null || soundfinish.State == SoundState.Stopped)
            {
				soundfinish = Main.PlaySound(SoundID.DD2_BookStaffTwisterLoop, npc.Center);
				if (soundfinish != null)
				{
					soundfinish.Volume = 0.99f;
				}
            }

				if (soundfinish != null)
				soundfinish.Pitch = Math.Min((effectScale/3000f)-0.75f, 0.75f);

			for (int i = 0; i < 1+Math.Min((npc.ai[0] - 5000)/2500f,3); i += 1)
			{
				if (npc.ai[0] % Math.Max((int)(10 - (npc.ai[0] - 5000) / 400f), 5) == 0 && npc.ai[0] > 5100)
				{
					Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
					Vector2 offsetpoint = npc.Center + offset * 2500;
					if (offsetpoint.Y < 12)
						offsetpoint.Y = 12;

					Projectile.NewProjectile(offsetpoint, -offset * 12f, ModContent.ProjectileType<SlimeProjectile>(), 50, 0f);
				}
			}

				if (Main.rand.Next(200) < npc.ai[0]-5000)
			{
				Vector2 offset = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
				int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + offset * Main.rand.NextFloat(32f, 128f), 0, 0, DustID.PurpleCrystalShard);
				Main.dust[dust].scale = 1.5f;
				Main.dust[dust].velocity = Vector2.Normalize(-offset) * (float)(Main.rand.NextFloat(0.50f, 2.50f));
				Main.dust[dust].noGravity = true;
			}

				foreach(Player player in Main.player.Where(testby => testby.active))
            {
				Vector2 pull = npc.Center - player.MountedCenter;
				player.position += Collision.TileCollision(player.position, Vector2.Normalize(pull)*Math.Min((effectScale / 600f), 5f), player.width, player.height);
				if (player.Distance(npc.Center) < Math.Min(npc.ai[0]/10f,256f))
                {
					player.Hurt(PlayerDeathReason.ByCustomReason("Became one with the Pink"), 100, 0,cooldownCounter: 1);
                }
			}
			if (npc.ai[0] % 300 == 0 && npc.ai[0] > 5200)
			{

				Projectile proj = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<PinkyRingAttack>(), 200, 0f);
				if (proj != null)
                {
					proj.ai[0] = Main.rand.Next(0, 120);
					proj.netUpdate = true;
				}

				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsySummon,npc.Center);
				if (sound != null)
					sound.Pitch += 0.50f;
			}

		}

			public void FlyNDash()
        {

			if (generalcounter % 700 > 660)
			{
				//Dash Timer
				npc.ai[3] = Main.rand.Next(300,500);
				stopmoving = 60;
			}

			if (npc.ai[3] > 0 && npc.ai[3] < 500)
			{
				Vector2 norm = Vector2.Normalize(P.MountedCenter - npc.Center);

				//Dashing!
				if (stopmoving > 50 && Vector2.Dot(Vector2.Normalize(P.MountedCenter - npc.Center), Vector2.Normalize(npc.velocity)) > -0.75f)
				{
					friction = Vector2.One;

					List<NPC> avilAttackers = SupremeArmy.OrderBy(testby => Main.rand.Next(100)).Where(testby => testby.ai[0] == 0 && testby.localAI[3] < -90).ToList();
					//Command attacks
					if (generalcounter % 8 == 0 && phase>0)
					{
						foreach (NPC npc2 in avilAttackers)
						{

							Vector2 bex = P.Center + ((Vector2.UnitX * Main.rand.NextFloat(240f, 640f)).RotatedByRandom(MathHelper.TwoPi));
							npc2.ai[0] = 3;
							npc2.localAI[1] = -20;
							UpdateSlime(npc2);
							npc2.netUpdate = true;

							SoundEffectInstance sound3 = Main.PlaySound(SoundID.DD2_BookStaffCast, npc2.Center);
							if (sound3 != null)
							{
								sound3.Pitch = 0.25f;
							}

							for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
							{
								Vector2 offset = f.ToRotationVector2();
								int dust = Dust.NewDust(npc2.Center + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
								Main.dust[dust].scale = 1.5f;
								Main.dust[dust].noGravity = true;
								Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
							}
							break;
						}
					}

				}
				else
				{
					//Slowing Down from dash!
					friction = new Vector2(0.92f, 0.92f);
				}

				float speed = npc.velocity.Length();
				npc.velocity = npc.velocity.ToRotation().AngleTowards(norm.ToRotation(), roter).ToRotationVector2()* speed;

				if (Main.rand.Next(0, 2) == 1)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.PurpleCrystalShard);
					Main.dust[dust].scale = 3f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Vector2.Normalize(P.Center - npc.Center) * (float)(Main.rand.NextFloat(1f, 4f));
				}
				
				//Warning effect
				if (stopmoving == 20)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsyWindAttack, (int)npc.Center.X, (int)npc.Center.Y);
					if (sound != null)
						sound.Pitch -= 0.50f;

					dashaim = norm.ToRotation();
					float tempaim = dashaim;
					for (float f = 0; f < 3000; f += 16f)
					{
						int dust = Dust.NewDust(new Vector2(npc.Center.X, npc.Center.Y) + tempaim.ToRotationVector2() * f, 0, 0, DustID.PurpleCrystalShard);
						tempaim = tempaim.AngleTowards(norm.ToRotation(), roter);
						Main.dust[dust].scale = 1.5f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = norm * (float)(Main.rand.NextFloat(1f, 4f));
					}
				}
				if (stopmoving < 1)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsyFlyingCircleAttack, (int)npc.Center.X, (int)npc.Center.Y);
					if (sound != null)
						sound.Pitch -= 0.50f;

					stopmoving = 80;
					npc.velocity = dashaim.ToRotationVector2() * 64f;
				}
			}
			else
			{
				//Not Dashing, Flying around the player
				circleLoc += (P.Center - circleLoc)/1000f;

				List<NPC> avilAttackers = SupremeArmy.OrderBy(testby => Main.rand.Next(100)).Where(testby => testby.ai[0] == 0 && testby.localAI[3] < -300).ToList();
				//Command attacks
				if (generalcounter % (330 + (avilAttackers.Count * 10)) > 300)
				{
					if (generalcounter % (phase < 2 ? 16 : 8) == 0)
					{
						foreach (NPC npc2 in avilAttackers)
						{
							stopmoving = 40;

							Vector2 bex = P.Center + ((Vector2.UnitX * Main.rand.NextFloat(240f, 640f)).RotatedByRandom(MathHelper.TwoPi));
							npc2.ai[0] = 2;
							npc2.localAI[1] = bex.X;
							npc2.localAI[2] = bex.Y;
							UpdateSlime(npc2);
							npc2.netUpdate = true;

							SoundEffectInstance sound3 = Main.PlaySound(SoundID.DD2_BetsysWrathShot, npc2.Center);
							if (sound3 != null)
							{
								sound3.Pitch = 0.25f;
							}

							for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
							{
								Vector2 offset = f.ToRotationVector2();
								int dust = Dust.NewDust(npc2.Center + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
								Main.dust[dust].scale = 1.5f;
								Main.dust[dust].noGravity = true;
								Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
							}
							break;
						}
					}
					return;
				}

				if (realcounter % 100 <= 60 && realcounter % 10 == 0 && realcounter > 60 && npc.ai[3]<-150)
				{

					List<Projectile> itz = Idglib.Shattershots(npc.Center, P.Center, new Vector2(0, 0), ProjectileID.NebulaBolt, 30, 15f, 40, 2, true, 0, true, 200);
					SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 110);
					if (sound != null)
						sound.Pitch += 0.50f;
				}
			}

		}

		public void PickAttack()
		{
			bool critical = false;
			if (aicounter != 0 && aicounter != 5 && SupremeArmy.Count > 6+phase*2)
			{
				int HPtest = 0;
				foreach (NPC npc in SupremeArmy)
				{
					HPtest += npc.life;
				}

				if (Main.rand.NextFloat((npc.lifeMax * 0.04f) * (15f+(phase*5f))) > HPtest)
				{
					critical = true;
					if (Main.rand.NextBool())
					{
						npc.ai[0] = 5600;
						npc.ai[1] = 4;
						npc.netUpdate = true;
						return;
					}
				}
			}

			if (SupremeArmy.Count < 10 || aicounter == 0 || aicounter == 5)
			{
				if (aicounter==0)
				Main.NewText("<SUPREME PINKY> COME MY CHILDREN, LET US FINISH THIS!", 255, 100, 255);

				if (aicounter < 10 && phase>2)
				{
					aicounter = 10;
					Main.NewText("<SUPREME PINKY> CODE PINK, CODE PINK!!!!", 255, 100, 255);
				}

				npc.ai[0] = 1600;
				npc.ai[1] = 1;
				npc.netUpdate = true;
			}
            else
            {
				if (SupremeArmy.Where(testby => testby.ai[0] == 0 && testby.localAI[3] < 0).ToList().Count > 10 || critical)
				{
					npc.ai[0] = 2660;
					npc.ai[1] = 2;
					npc.netUpdate = true;
				}
			}
			npc.netUpdate = true;
		}

		public void SupremeArmyCommand()
		{

			//Main.NewText("<SUPREME PINKY> COME MY CHILDREN, LET US FINISH THEM!", 255, 100, 255);

			for (int i = 0; i < SupremeArmy.Count; i += 1)
			{
				NPC npc2 = SupremeArmy[i];
				if (npc2 != null && npc2.active && npc2.life > 0)
				{
					SGAnpcs sganpc = npc2.SGANPCs();
					//if (sganpc.PinkyMinion<6)
					sganpc.PinkyMinion += 1;
					npc2.timeLeft = 900;
					if (npc2.ai[0] < 0)
						npc2.ai[0] += 1;
					if (sganpc.PinkyMinion > 5)
					{
						UnifiedRandom rando = new UnifiedRandom(npc2.whoAmI - 7500);
						//Fly out attack, come back, boomerang style
						if (npc2.ai[0] == 1)
						{
							//Main.NewText(npc2.Center);
							Vector2 vecta = new Vector2(npc2.localAI[1], npc2.localAI[2]);

							npc2.velocity += Vector2.Normalize(vecta - npc2.Center) * 4f;
							npc2.velocity *= 0.90f;

							if (npc2.Distance(vecta) < 64 || Vector2.Dot(Vector2.Normalize(vecta - npc2.Center), Vector2.Normalize(npc2.velocity)) < -0.75f)
							{
								npc2.ai[0] = 0;
								npc2.localAI[1] = 0;
								npc2.localAI[2] = 120;
								UpdateSlime(npc2);
								npc2.netUpdate = true;
							}
							continue;

						}
						//Fly out, shoot shot
						if (npc2.ai[0] == 2)
						{
							//Main.NewText(npc2.Center);
							if (npc2.localAI[1] > 0)
							{
								Vector2 vecta = new Vector2(npc2.localAI[1], npc2.localAI[2]);

								Vector2 vexa = vecta - npc2.Center;
								npc2.velocity += Vector2.Normalize(vexa) * (0.75f + (vexa.Length() / 600f));
								npc2.velocity *= 0.90f;

								if (npc2.Distance(vecta) < 64 || Vector2.Dot(Vector2.Normalize(vecta - npc2.Center), Vector2.Normalize(npc2.velocity)) < -0.75f)
								{
									npc2.localAI[1] = -200;
									npc2.localAI[2] = 0;
									UpdateSlime(npc2);
									npc2.netUpdate = true;
								}
							}
							else
							{
								npc2.localAI[1] = (int)npc2.localAI[1] + 1;
								npc2.velocity /= 2f;

								if (npc2.localAI[1] < -100 && Main.rand.Next(0, 2) == 1)
								{
									int dust = Dust.NewDust(new Vector2(npc2.position.X, npc2.position.Y), npc2.width, npc2.height, DustID.PurpleCrystalShard);
									Main.dust[dust].scale = 1.5f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = Vector2.Normalize(P.Center - npc2.Center) * (float)(Main.rand.NextFloat(1f, 4f));
								}
								if (npc2.localAI[1] == -100)
								{
									List<Projectile> itz = Idglib.Shattershots(npc2.Center, P.Center, new Vector2(0, 0), ProjectileID.NebulaBolt, 30, 20f, 0, 1, true, 0, true, 200);
									SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc2.Center.X, (int)npc2.Center.Y, 110);
									if (sound != null)
										sound.Pitch += 0.50f;
								}

								if (npc2.localAI[1] > -20)
								{
									npc2.ai[0] = 0;
									npc2.localAI[1] = 0;
									npc2.localAI[2] = 0;
									npc2.localAI[3] = 200;
									UpdateSlime(npc2);
									npc2.netUpdate = true;
								}
							}
							continue;
						}
						//Stop in place, shoot projectiles based on velocity
						if (npc2.ai[0] == 3)
						{

							npc2.localAI[1] = (int)npc2.localAI[1] + 1;
							if (npc2.localAI[1] == -19)
							{
								npc2.localAI[2] = npc.velocity.ToRotation() + MathHelper.PiOver2;
							}
							if ((npc.ai[3] > 1 && stopmoving > -5) && npc2.localAI[1] > -15 && npc2.localAI[1] < -13)
							{
								npc2.localAI[1] = -15;
							}
							npc2.velocity /= 1.5f;

							float anglesToShoot = phase + 1;

							for (int ii = 0; ii < anglesToShoot; ii += 1)
							{
								if (npc2.localAI[1] < -10 && Main.rand.Next(0, 2) == 1)
								{
									int dust = Dust.NewDust(new Vector2(npc2.position.X, npc2.position.Y), npc2.width, npc2.height, DustID.PurpleCrystalShard);
									Main.dust[dust].scale = 2f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = npc2.localAI[2].ToRotationVector2().RotatedBy(((MathHelper.TwoPi / anglesToShoot) * ii)) * (float)(Main.rand.NextFloat(0f, 20f));
								}
								if (npc2.localAI[1] == -10)
								{
									Idglib.Shattershots(npc2.Center, npc2.Center + (npc2.localAI[2] + ((MathHelper.TwoPi / anglesToShoot) * ii)).ToRotationVector2(), new Vector2(0, 0), ProjectileID.DemonScythe, 50, 1f, 0, 1, true, 0, true, 200);
									Idglib.Shattershots(npc2.Center, npc2.Center + (npc2.localAI[2] + ((MathHelper.TwoPi / anglesToShoot) * ii)).ToRotationVector2(), new Vector2(0, 0), ModContent.ProjectileType<PinkyWarning>(), 50, 2f, 0, 1, true, 0, true, 150);
									SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc2.Center.X, (int)npc2.Center.Y, 71);
									if (sound != null)
										sound.Pitch += 0.75f;
								}
							}

							if (npc2.localAI[1] > 30)
							{
								npc2.ai[0] = 0;
								npc2.localAI[1] = 0;
								npc2.localAI[2] = 0;
								npc2.localAI[3] = 120;
								UpdateSlime(npc2);
								npc2.netUpdate = true;


								SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsysWrathShot, npc2.Center);
								if (sound != null)
								{
									sound.Pitch = -0.25f;
								}

								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
								{
									Vector2 offset = f.ToRotationVector2();
									int dust = Dust.NewDust(npc2.Center + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
									Main.dust[dust].scale = 1.5f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
								}

								npc2.Center = npc.Center;
								npc2.netUpdate = true;

							}
							continue;
						}

						//Teleport to Boss, fly out, than do attack
						if (npc2.ai[0] == 4)
						{
							npc2.localAI[1] = (int)npc2.localAI[1] + 1;

							if (npc2.localAI[1] < 30)
								npc2.velocity *= 0.85f;

							if (npc2.localAI[1] == 30)
							{
								SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsysWrathShot, npc2.Center);
								if (sound != null)
								{
									sound.Pitch = -0.25f;
								}

								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
								{
									Vector2 offset = f.ToRotationVector2();
									int dust = Dust.NewDust(npc2.Center + (offset * 32f), 0, 0, DustID.PinkCrystalShard);
									Main.dust[dust].scale = 1.5f;
									Main.dust[dust].noGravity = true;
									Main.dust[dust].velocity = f.ToRotationVector2() * 4f;
								}

								npc2.Center = npc.Center;
								npc2.velocity = Vector2.Zero;
								npc2.netUpdate = true;
							}
							Vector2 angletoshootat = npc2.localAI[2].ToRotationVector2();

							if (npc2.localAI[1] == 80)
							{
								Idglib.Shattershots(npc2.Center, npc2.Center + angletoshootat, new Vector2(0, 0), ModContent.ProjectileType<PinkyWarning>(), 1, 2f, 0, 1, true, 0, true, 150);
								SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc2.Center.X, (int)npc2.Center.Y, 71);
								if (sound != null)
									sound.Pitch += 0.75f;
							}
							if (npc2.localAI[1] < 80 || npc2.Distance(circleLoc) > circlesize)
							{
								npc2.velocity = npc2.velocity * 0.75f;
							}
							else
							{
								npc2.velocity += angletoshootat * MathHelper.Clamp((npc2.localAI[1] - 80) / 80f, 0f, 2f);
								npc2.velocity *= 0.95f;

							}

							//npc2.ai[0] = 0;
							//npc2.localAI[1] = 0;
							//npc2.localAI[2] = 0;
							//npc2.localAI[3] = 120;
							//UpdateSlime(npc2);
							//npc2.netUpdate = true;

							continue;
						}

						//Teleport to the player, and explode (handled on the boss's end)
						if (npc2.ai[0] == 5)
						{
							Vector2 vecta = new Vector2(npc2.localAI[1], npc2.localAI[2]);

							int dust = Dust.NewDust(vecta-new Vector2(npc2.width,npc2.height)/2f, npc2.width, npc2.height, DustID.PurpleCrystalShard);
							Main.dust[dust].scale = 3.5f;
							Main.dust[dust].noGravity = true;
							Main.dust[dust].velocity = Main.rand.NextVector2Circular(2f,6f);
							continue;

						}


							npc2.localAI[3] = MathHelper.Clamp(npc2.localAI[3] - 1, -1000, 10000);
						float dister = rando.NextFloat(180f, 320f) * (npc.ai[3] > 0 && npc.ai[1] == 0 ? 0.1f : 1f);
						Vector2 gothere = Vector2.UnitX.RotatedBy(sganpc.PinkyMinion * rando.NextFloat(0.025f, 0.12f) * (rando.NextBool() ? 1f : -1f)) * dister;
						gothere += npc.Center + npc.velocity;

						if ((gothere - npc2.Center).Length() > 200)
						{
							Vector2 differance = (gothere - npc2.Center);
							npc2.velocity += (Vector2.Normalize(differance) * 0.75f) + ((differance) / 1000f);
							npc2.velocity *= 0.96f;
						}
						npc2.velocity *= 0.99f;
						npc2.position += (npc.velocity) * (1f - (Math.Max(npc2.localAI[3],0) / 120f));

					}
				}
				else
				{
					SupremeArmy.RemoveAt(i);
				}
			}
		}
		//Main.NewText(SupremeArmy.Count);

		Vector2 goThere;
		Vector2 friction;
		Vector2 speed;
		Vector2 center;

		public override void AI()
		{
			if (Main.dayTime)
            {
				Main.dayTime = false;
            }

			slimecalleffect = Math.Max(slimecalleffect-0.75f,0f);
			if (IntroDeal())
			{
				if (circleLoc == default)
					circleLoc = npc.Center;

				if (phase == 3)
                {
					npc.defense = 25 + SupremeArmy.Count*5;
				}

				npc.ai[3] -= 1;
				//foreach(Player player in Main.player.Where(testby => testby.active))
				Main.slimeRainKillCount = 0;

				//Recruiter!
				if (npc.ai[1] != 4)
				{
					foreach (NPC npc2 in Main.npc.Where(testby => testby.active && testby.life > 0 && testby.SGANPCs().PinkyMinion > 0 && testby.SGANPCs().PinkyMinion < 2))
					{
						//if (SupremeArmy.FirstOrDefault(testby2 => testby2.type == npc2.type) == default)
						//{
						npc2.life = (int)(npc.lifeMax * 0.04f);
						npc2.lifeMax = (int)(npc.lifeMax * 0.04f);
						npc2.knockBackResist = 0f;
						if (npc2.ai[0] != 1)
                        {
							npc2.Center = npc.Center;
                        }
						npc2.ai[0] = 0;
						//npc2.realLife = npc.whoAmI;
						npc2.noGravity = true;
						npc2.noTileCollide = true;
						npc2.timeLeft = 900;
						npc2.defense = 50;
						npc2.damage = 200;
						npc2.defDamage = 200;

						for (int i = 0; i < npc2.buffImmune.Length; i += 1)
						{
							npc2.buffImmune[i] = true;
						}

						npc2.netUpdate = true;
						SupremeArmy.Add(npc2);
						//}
					}
				}
				if (aicounter == 0 || aicounter == 5)
				{
					PickAttack();
					aicounter = 1;
				}

				if (SupremeArmy.Count > 0)
				{
					SupremeArmyCommand();
				}

				NoEscape(circlesize, circleLoc-P.MountedCenter);
				goThere = P.MountedCenter + new Vector2(0, -300);
				friction = new Vector2(0.985f, 0.985f);
				speed = new Vector2(0.45f, 0.45f);
				center = npc.Center;

				if (npc.ai[1] == 20)
				{
					Dying();
					goto labeljump;
				}

				if (npc.ai[1] == 10)
				{
					FinalAttack();
					goto labeljump;
				}

				if (npc.ai[1] == 4)
				{
					SlimeKamakaze();
					goto labeljump;
				}
				if (npc.ai[1] == 3)
				{
					PhaseShift();
					goto labeljump;
				}
				if (npc.ai[1] == 2)
				{
					SlimeCommand();
					goto labeljump;
				}

				if (npc.ai[1] == 1)
				{
					SlimeCall();
					goto labeljump;
				}

				if (npc.ai[1] == 0)
				{

					bool phase1 = npc.life < (int)(npc.lifeMax * 0.75f) && phase == 0;
					bool phase2 = npc.life < (int)(npc.lifeMax * 0.5f) && phase == 1;
					bool phase3 = npc.life < (int)(npc.lifeMax * 0.25f) && phase == 2;
					if (phase1 || phase2 || phase3)
					{
						npc.ai[1] = 3;
						npc.ai[0] = 3860;
						goto labeljump;
					}

					if (npc.Center.X < P.MountedCenter.X - 500)
						npc.spriteDirection = 1;
					if (npc.Center.X > P.MountedCenter.X + 500)
						npc.spriteDirection = -1;

					goThere = P.MountedCenter + new Vector2(npc.spriteDirection * 500, P.MountedCenter.Y-npc.Center.Y>0 ? -300 : 300);
					speed *= MathHelper.Clamp(realcounter / 40f, 0f, 1f);

					if (npc.ai[3]<1)//Not dashing
                    {
						if (realcounter > attackPhaseTime && npc.ai[3]<500) 
						{
							PickAttack();
						}
                    }
					FlyNDash();
				}

				labeljump:

				if (stopmoving < 1)
				{
					Vector2 flyhere = goThere- center;
					npc.velocity += Vector2.Normalize(flyhere) * speed;
				}

				npc.velocity *= friction;
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D texture = SGAmod.ExtraTextures[96];
			Texture2D texture2 = ModContent.GetTexture("SGAmod/Items/LunarRoyalGel");
			Texture2D texture3 = Main.npcTexture[npc.type];
			Texture2D inner = SGAmod.ExtraTextures[111];
			float floater = MathHelper.Clamp(getHitEffect / 15f, 0f, 1f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			hallowed.Parameters["alpha"].SetValue(1);
			hallowed.Parameters["prismAlpha"].SetValue(1f);
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime/1f, Main.GlobalTime / 2f));
			hallowed.Parameters["overlayAlpha"].SetValue(0f);
			hallowed.Parameters["overlayStrength"].SetValue(0f);
			hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(1,1));

			Vector2 drawOrigin2 = new Vector2(inner.Width, inner.Height / 2) / 2f;

			foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<SlimeProjectile>()))
            {
				UnifiedRandom rando = new UnifiedRandom(proj.whoAmI * 753);
				float alpha2 = Math.Min(proj.ai[0]/30f,1f);
				hallowed.Parameters["prismColor"].SetValue((proj.modProjectile as SlimeProjectile).color.ToVector3());
				spriteBatch.Draw(inner, proj.Center - Main.screenPosition,
					new Rectangle(0, 0, inner.Width, inner.Height / 2),
					Color.White,
					proj.ai[0]*(proj.ai[1]), drawOrigin2, npc.scale, SpriteEffects.None, 0f);

				hallowed.Parameters["alpha"].SetValue(alpha2);
				hallowed.CurrentTechnique.Passes["Prism"].Apply();
			}


			if (slimecalleffect > 0)
			{
				UnifiedRandom rando2 = new UnifiedRandom(npc.whoAmI * 753);
				float localtimer = npc.localAI[0]/90f;
				for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 8f)
				{
					for (float i1 = 0; i1 < 1f; i1 += 0.05f)
					{
						float i = i1 + (f / MathHelper.TwoPi) * 0.05f;
						float alpha = MathHelper.Clamp(((0f + ((i + ((float)localtimer)) % 1f)) * 1f) * MathHelper.Clamp(slimecalleffect / 60f, 0f, 1f), 0f, 1f);
						if (alpha > 0)
						{
							Color color = Main.hslToRgb(rando2.NextFloat() % 1f, 1f, 0.65f);
							hallowed.Parameters["prismColor"].SetValue(color.ToVector3());
							Vector2 offset = (Vector2.One * (1f - (1f * ((((float)localtimer) + i) % 1f)))).RotatedBy((i + f) * (MathHelper.TwoPi));
							spriteBatch.Draw(inner, npc.Center + (offset * 960f) - Main.screenPosition,
								new Rectangle(0, 0, inner.Width, inner.Height / 2),
								color * alpha,
								rando2.NextFloat(MathHelper.TwoPi)+(i* MathHelper.TwoPi), drawOrigin2, npc.scale, SpriteEffects.None, 0f);

							hallowed.Parameters["alpha"].SetValue(alpha);
							hallowed.CurrentTechnique.Passes["Prism"].Apply();
						}
					}
				}
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			
			if (npc.ai[0] > 295)
			{
				float sizer = 32f * MathHelper.Clamp((npc.ai[0] - 295) / 20f, 0f, 1f);
				UnifiedRandom rando = new UnifiedRandom(npc.whoAmI * 753);

				for (float ix = 0; ix < 24; ix += 1f)
				{

					Vector2 vex = Vector2.One.RotatedBy(rando.NextFloat(MathHelper.TwoPi)) * sizer;
					float[] rando2 = { rando.NextFloat(MathHelper.TwoPi), rando.NextFloat(MathHelper.TwoPi), rando.NextFloat(MathHelper.TwoPi) };
					float[] rando3 = { rando.NextFloat(0.1f, 0.25f), rando.NextFloat(0.1f, 0.25f), rando.NextFloat(0.1f, 0.25f) };

					float i = (generalcounter / 2f);
					Matrix matrix = Matrix.CreateRotationZ((i * rando3[0]) + rando2[0]) * Matrix.CreateRotationY((i * rando3[1]) + rando2[1]) * Matrix.CreateRotationX((i * rando3[2]) + rando2[2]);

					spriteBatch.Draw(texture3, Vector2.Transform(vex, matrix) + npc.Center - Main.screenPosition, npc.frame, drawColor*MathHelper.Clamp((npc.ai[0] - 295) / 20f, 0f, 1f), rando.NextFloat(MathHelper.TwoPi) + (npc.velocity.X / 30f), new Vector2(texture3.Width, texture3.Height / 5f) / 2f, npc.scale * 1f, SpriteEffects.None, 0f);
				}
			}

			if (generalcounter > 0)
			{
				float sizer = 64f * MathHelper.Clamp((npc.ai[0] - 295) / 20f, 0f, 1f);
				float dister = 1f;
				//for (float dister = 0f; dister < 0f; dister += 0.05f)
				//{
					for (float z = -1; z < 2; z += 2)
					{
						for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 5f)
						{
							float dist = MathHelper.Clamp(generalcounter / 60f, 0f, 2 + z);
							float timez = generalcounter;
							float angle = i + (generalcounter * (npc.direction > 0 ? 0.1f : -0.1f)) * z;
							Vector2 drawOrigin = new Vector2(texture2.Width, texture2.Height / 6) / 2f;
							spriteBatch.Draw(texture2, npc.Center+(npc.velocity* dister) + (Vector2.UnitX.RotatedBy(angle)) * sizer - Main.screenPosition, new Rectangle(0, ((int)((timez / 5f) % 6)) * ((texture2.Height) / 6), texture2.Width, (texture2.Height - 1) / 6), drawColor*dister, npc.velocity.X / 30f, drawOrigin, npc.scale, SpriteEffects.None, 0f);
						}
					}
				//}
			}

			if (npc.ai[0] > 5000)
			{
				Texture2D tex2 = Main.itemTexture[ModContent.ItemType<StygianCore>()];
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				for (int i = -1; i < 2; i += 2)
				{

					hallowed.Parameters["alpha"].SetValue(0.750f);
					hallowed.Parameters["prismColor"].SetValue(Color.White.ToVector3());
					hallowed.Parameters["prismAlpha"].SetValue(0f);
					hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("TiledPerlin"));
					hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime / 1f, Main.GlobalTime / 2f));
					hallowed.Parameters["overlayAlpha"].SetValue(Math.Min((npc.ai[0] - 5000) / 60f, 20f));
					hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0.50f, 1f));
					hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
					hallowed.Parameters["rainbowScale"].SetValue(1f);
					hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));
					hallowed.CurrentTechnique.Passes["Prism"].Apply();

					spriteBatch.Draw(tex2, npc.Center - Main.screenPosition, null, Color.White, (i*MathHelper.PiOver2), tex2.Size() / 2f, new Vector2(1f, 1f) * Math.Min((effectScale) / 300f, 10f), SpriteEffects.None, 0f);
				}

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}

			spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.Magenta * 0.8f * floater, npc.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(2f, 2f) * floater, SpriteEffects.None, 0f);


			return false;
		}

		public override void NPCLoot()
		{
			base.NPCLoot();
		}

		public override string Texture
		{
			get { return ("SGAmod/NPCs/SPinky"); }
		}


	}
	[AutoloadBossHead]
	public class SPinky : ModNPC, ISGABoss
	{
		public string Trophy() => "SupremePinkyTrophy";
		public bool Chance() => Main.rand.Next(0, 10) == 0 && ((GetType() == typeof(SPinky) || !Main.expertMode) || (GetType() == typeof(SPinkyTrue)));

		protected float getHitEffect = 0f;
		protected int aicounter = 0;
		protected int pushtimes = 0;
		protected int trytorun = 0;
		protected int phase = 0;
		protected int father = 0;
		protected int fatherphase = 0;
		protected int fathercharge = -150;
		protected int fatherhp = 0;
		protected float circlesize = 2400;
		protected float dpscap = 0;
		protected int generalcounter = 0;
		protected float drawdist = 0;
		protected int stopmoving = 0;
		protected int radpoison = 0;
		protected Vector2 circleLoc;
		protected Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supreme Pinky");
			Main.npcFrameCount[npc.type] = 5;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}
		public override void SetDefaults()
		{
			npc.width = 16;
			npc.height = 16;
			npc.damage = 100;
			npc.defense = 50;
			npc.lifeMax = 50000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.aiStyle = 1;
			npc.netAlways = true;
			npc.boss = true;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			npc.noTileCollide = false;
			npc.noGravity = false;
			music = MusicID.Boss2;
			//bossBag = mod.ItemType("SPinkyBag");
			npc.value = Item.buyPrice(0, 1, 0, 0);
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

        /*public override bool PreNPCLoot()
		{
			if (npc.GetType()==typeof(SPinky))
			npc.GivenName = "Surpreme Pinky...?";
			return true;
		}*/

        public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.expertMode)
			{
				if (npc.boss)
					target.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationThree").Type, 60 * 5);
				else
					target.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationTwo").Type, 60 * 10);

			}
		}

		public Matrix TransformationMatrix(Texture2D tex, Vector2 DrawPosition, float Rotation, Vector2 Scale, bool FlipHorizontally = false, Vector2 Origin = default(Vector2))
		{
			if (Origin == default(Vector2))
				Origin = new Vector2(tex.Width, tex.Height) / 2;

			return Matrix.CreateTranslation(-new Vector3(tex.Width * Scale.X / 2, 0, 0))
				* Matrix.CreateScale(new Vector3(FlipHorizontally ? -1 : 1, 1, 1))
				* Matrix.CreateTranslation(new Vector3(tex.Width * Scale.X / 2, 0, 0))
				* Matrix.CreateTranslation(-new Vector3(Origin, 0))
				* Matrix.CreateScale(new Vector3(Scale, 0))
				* Matrix.CreateRotationZ(Rotation)
				* Matrix.CreateTranslation(new Vector3(DrawPosition, 0));

		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
			Texture2D texture = SGAmod.ExtraTextures[96];
			Texture2D texture2 = ModContent.GetTexture("SGAmod/Items/LunarRoyalGel");
			Texture2D texture3 = Main.npcTexture[npc.type];
			float floater = MathHelper.Clamp(getHitEffect / 15f, 0f, 1f);

			spriteBatch.Draw(texture3, npc.Center - Main.screenPosition, npc.frame, drawColor, npc.velocity.X / 30f, new Vector2(texture3.Width, texture3.Height/5f)/2f, npc.scale, SpriteEffects.None, 0f);


			if (GetType() == typeof(SPinkyClone) || generalcounter > 0)
			{
				for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 5f)
				{
					float dist = MathHelper.Clamp(generalcounter / 60f, 0f, 1f);
					float timez = generalcounter;
					float angle = i + (generalcounter * (npc.direction > 0 ? 0.1f : -0.1f));
					Vector2 drawOrigin = new Vector2(texture2.Width, texture2.Height / 6) / 2f;
					spriteBatch.Draw(texture2, npc.Center + (Vector2.UnitX.RotatedBy(angle)) * 32f - Main.screenPosition, new Rectangle(0, ((int)((timez / 5f) % 6)) * ((texture2.Height) / 6), texture2.Width, (texture2.Height - 1) / 6), drawColor, npc.velocity.X / 30f, drawOrigin, npc.scale, SpriteEffects.None, 0f);
				}
			}

			spriteBatch.Draw(texture, npc.Center - Main.screenPosition, null, Color.Magenta * 0.8f * floater, npc.rotation, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(1f, 1f) * floater, SpriteEffects.None, 0f);


			return false;
        }

		public void CircleAura(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = circleLoc;
			Texture2D texture = SGAmod.ExtraTextures[96];
			if (GetType() == typeof(SPinkyClone) || !(drawdist > 0f))
				return;
			float inrc = Main.GlobalTime / 30f;

			List<Vector2> vects = new List<Vector2>();
			int maxDetail = 180;
			for (int i = 0; i < maxDetail; i += 1)
			{
				float angle = ((i/ (float)maxDetail)*MathHelper.TwoPi)+inrc;
				float dist = circlesize;
				Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));
				vects.Add(circleLoc+thisloc);
			}

			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
			trail.color = delegate (float percent)
			{
				return Color.Magenta;
			};

			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 64;
			trail.trailThicknessIncrease = 0;
			trail.doFade = false;
			trail.connectEnds = true;
			trail.strength = drawdist;
			trail.DrawTrail(vects, npc.Center);

		}

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			CircleAura(spriteBatch,drawColor);
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			if (GetType() == typeof(SPinkyTrue))
				npc.lifeMax = (int)(npc.lifeMax * 0.750f);
			else
				npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}
		public override void NPCLoot()
		{
			if (GetType() == typeof(SPinkyTrue))
            {
				npc.DropBossBags();
				goto Ded;

			}
			if (npc.boss)
			{
				if (Main.expertMode)
				{
					if (!SGAWorld.downedSPinky || TheWholeExperience.Check())
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SPinkyBagFake"));
                    }
                    else
                    {
						NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, ModContent.NPCType<SPinkyTrue>());
                    }
					return;
				}
				else
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LunarRoyalGel"), 30);
                    Items.Armors.Illuminant.IlluminantHelmet.IlluminantArmorDrop(1, npc.Center);
				}
			}

			//float targetX = npc.Center.X;
			//float targetY = npc.Center.Y;
			//NPC.NewNPC((int)npc.Center.X + 13, (int)npc.Center.Y - 2, mod.NPCType("GraySlime6"));
			//NPC.NewNPC((int)npc.Center.X - 13, (int)npc.Center.Y - 2, mod.NPCType("GraySlime6"));

			Ded:
			Achivements.SGAAchivements.UnlockAchivement("SPinky", Main.LocalPlayer);
			if (!SGAWorld.downedSPinky)
				SGAWorld.AdvanceHellionStory();
			SGAWorld.downedSPinky = true;
			
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.SuperHealingPotion;
		}

        public override bool PreAI()
        {
			if (GetType() == typeof(SPinkyClone))
			return true;

			if (phase > 1 && generalcounter > 0)
			{
				AI();
				return false;
			}
			return true;

		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return new Rectangle((int)npc.Center.X-8, (int)npc.Center.Y - 8,16,16).Intersects(target.Hitbox);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			getHitEffect = MathHelper.Clamp((float)damage/50f,15f,90f);
		}

		public void NoEscape(float dashdist,Vector2 dist)
		{

			if (dist.Length() > (dashdist) || (aicounter == 0 && dist.Length() > 600))
			{
				if (aicounter != 0 && GetType() != typeof(SPinkyClone) && dist.Length() > circlesize)
				{
					P.AddBuff(21, 120);
					P.AddBuff(160, 150);
					//P.velocity=P.velocity*10;
					//P.velocity=P.velocity/11;
					if (P.mount.Type != 4)
						P.mount.Dismount(P);
					P.mount.SetMount(4, P, false);
					P.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationThree").Type, 30);

					if (trytorun == 0)
					{
						trytorun = 1;
						Main.NewText("<Supreme Pinky> YOU AIN`T GOING ANYWHERE", 255, 100, 255);
					}
				}

			}
		}

		public override void AI()
		{
			if (npc.aiStyle != 15)
			{
				npc.width = 64;
				npc.height = 64;
				npc.scale = 1;
			}
			getHitEffect -= 1f;
			P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || Main.dayTime)
				{
					float speed = ((-10f));
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
					npc.active = false;
					if (father > 0)
					{
						Main.npc[father].active = false;
						Main.npc[father].velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);

					}

				}

			}
			else
			{
				if (float.IsNaN(npc.Center.X))
                {
					npc.Center = P.Center + new Vector2(0, -200);
                }
				if (Main.expertMode)
					P.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type, 1);

				if (GetType() == typeof(SPinky))
					npc.GivenName = "Supreme Pinky";
				else
					npc.GivenName = "Doppelgangers";


				npc.netUpdate = true;
				npc.timeLeft = 99999;
				if (npc.life > 0)
					npc.active = true;

				if (npc.aiStyle < 0)
				{
					//Vector2 ownerloc=npc.ai[0].Center;
				}

				Vector2 ploc = P.Center;
				circleLoc = npc.Center;
				Vector2 meloc = npc.Center;
				float moveup = 0;
				int isboss = 0;
				if (npc.boss == true)
				{
					isboss = 1;
				}
				pushtimes = pushtimes + 1;
				if (pushtimes > 200 + (isboss * 30))
				{
					pushtimes = -100 + (Main.rand.Next(120));
					moveup = Main.rand.Next(-50, 200);
				}
				Vector2 dist = ploc - meloc;
				int adder = 0;
				if (npc.aiStyle == 19)
				{
					adder = 90;
				}
				if (npc.aiStyle == 52)
				{
					adder = 60;
				}
				if (stopmoving < 1)
				{
					if (phase == 1)
					{
						npc.velocity.Y -= 0.01f;
						npc.velocity.Normalize();
						npc.velocity = npc.velocity * (dist.Length() / 300);
					}
					if (phase == 3)
					{
						npc.velocity.Y += 0.01f;
						npc.velocity.Normalize();
						npc.velocity = npc.velocity * (dist.Length() / 40);
					}
				}
				if (isboss > 0)
				{
					if (father > 0)
					{
						fatherhp = Main.npc[father].life;
						Vector2 fatherloc = Main.npc[father].Center;
						fathercharge = fathercharge + 1;
						if (fathercharge > 170)
						{
							fathercharge = 0;
						}
						Main.npc[father].velocity *= 0.998f;
						if (fathercharge > 140 || fathercharge < 0)
						{
							Main.npc[father].noTileCollide = true;
							Main.npc[father].noGravity = true;
							if (ploc.X > fatherloc.X)
							{
								Main.npc[father].velocity = new Vector2(((ploc.X - 450) - fatherloc.X)/32f, (((ploc.Y + 320) - fatherloc.Y) / 13) + moveup);
							}
							else
							{
								Main.npc[father].velocity = new Vector2(((ploc.X + 450) - fatherloc.X)/32f, (((ploc.Y + 320) - fatherloc.Y) / 13) + moveup);
							}
						}
						else
						{
							Main.npc[father].noTileCollide = true;
							Main.npc[father].noGravity = true;
						}

					}
				}

				/*if ((fatherhp>0 && (Main.npc[father].ai[0]!=npc.whoAmI || Main.npc[father].active==false))){
								father=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y - 32, 50);
							Main.npc[father].life=fatherhp;
							Main.npc[father].lifeMax=(int)(npc.lifeMax*2.75);
							Main.npc[father].boss=true;
							Main.npc[father].aiStyle=87 ;
							Main.npc[father].ai[0]=npc.whoAmI;
							Main.npc[father].defense=550;
							Main.npc[father].damage=60;
				Main.NewText("<Supreme Pinky> FATHER, COME BACK!",255, 100, 255);
				}*/



				if (GetType() == typeof(SPinky))
				{
					stopmoving -= 1;
					if (stopmoving > 0)
						npc.velocity = Vector2.Zero;

					if (NPC.CountNPCS(mod.NPCType("SPinkyClone")) + NPC.CountNPCS(NPCID.KingSlime) < 1 && npc.aiStyle != 69)
						generalcounter += 1;
					else
						generalcounter = 0;

					if (generalcounter % 300 > 200)
					{
						if (phase < 2)
						{
							stopmoving = 75;
							/*for (int i = 0; i < itz.Count; i += 1) {
								itz[i].friendly = false;
								itz[i].hostile = true;
								itz[i].netUpdate = true;
								}*/
							if (generalcounter % 10 == 0)
							{
								Idglib.Shattershots(npc.Center, P.Center, new Vector2(0, 0), ProjectileID.DemonScythe, 50, 1f, (100 - (float)((generalcounter % 300) - 200) * 2) * 2, 2, false, 0, true, 220);
								Idglib.Shattershots(npc.Center, P.Center, new Vector2(0, 0), ModContent.ProjectileType<PinkyWarning>(), 1, 1f, (100 - (float)((generalcounter % 300) - 200) * 2) * 2, 2, false, 0, true, 220);
							}

						}
					}

					if (phase > 1)
					{
						if (generalcounter % 400 > 150 && generalcounter % 5 == 0)
						{
							stopmoving = 15;
							Vector2 here = (P.Center - npc.Center);
							here.Normalize();
							List<Projectile> itz = Idglib.Shattershots(npc.Center + (here * circlesize), npc.Center, new Vector2(0, 0), ProjectileID.DemonScythe, 50, 1f, 70, 1, true, 0, true, 180);
							itz = Idglib.Shattershots(npc.Center, npc.Center + (here * circlesize), new Vector2(0, 0), ProjectileID.DemonScythe, 50, 1f, 70, 1, true, 0, true, 180);

						}

						if (generalcounter % 400 > 100)// && generalcounter % 150 == 0)
						{
							/*Vector2 here = (P.Center - npc.Center);
							here.Normalize();
							List<Projectile> itz = Idglib.Shattershots(npc.Center + new Vector2(0, 0), P.Center, new Vector2(0, 0), ProjectileID.SaucerMissile, 50, 18f, 200, 2, false, 0, false, 400);
							itz[0].localAI[1] = -20;
							itz[1].localAI[1] = -20;
							itz = Idglib.Shattershots(npc.Center, P.Center.RotatedBy(MathHelper.ToRadians(180), npc.Center), new Vector2(0, 0), ProjectileID.SaucerMissile, 50, 15f, 180, 1, true, 0, false, 400);
							itz[0].localAI[1] = -10;*/

							if (generalcounter % 400 == 30)
							{
								Projectile proj = Projectile.NewProjectileDirect(npc.Center, Vector2.Zero, ModContent.ProjectileType<PinkyRingAttack>(), 150, 0f);
								if (proj != null)
								{
									proj.ai[0] = Main.rand.Next(-300, 0);
									(proj.modProjectile as PinkyRingAttack).maxTime = 250;
									proj.timeLeft = 250;
									(proj.modProjectile as PinkyRingAttack).ringSize = 36;
									proj.netUpdate = true;
								}
							}
							if (generalcounter % 300 == 100)
							{
								for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 16f)
								{
									float ff = f + generalcounter / 400f;
									Projectile.NewProjectile(npc.Center, ff.ToRotationVector2() * 1f, ProjectileID.DemonScythe, 50, 0f);
									Projectile.NewProjectile(npc.Center + (ff.ToRotationVector2() * 32f), ff.ToRotationVector2() * 1f, ModContent.ProjectileType<PinkyWarning>(), 1, 0f);
								}

								SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsySummon, npc.Center);
								if (sound != null)
									sound.Pitch += 0.50f;
							}


						}

					}

				}
				else
				{

					generalcounter += 1;

					if (npc.aiStyle == 87 || npc.aiStyle == 63 || npc.aiStyle == 91)
					{
						stopmoving -= 1;
						int modez = (Main.expertMode ? 1 : 2);

						int delay = npc.aiStyle == 63 ? 150 : 87;
						int offsetter = 0;
						if (npc.aiStyle == 91)
						{
							delay = 300;
							offsetter = npc.whoAmI * 173;
						}

						if ((generalcounter + offsetter + 60) % ((50 + delay) * modez) < 80)
						{
							if (Main.rand.Next(0, 2) == 1)
							{
								int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, DustID.PurpleCrystalShard);
								Main.dust[dust].scale = 1.5f;
								Main.dust[dust].noGravity = true;
								Main.dust[dust].velocity = Vector2.Normalize(P.Center - npc.Center) * (float)(Main.rand.NextFloat(1f, 4f));
							}
							stopmoving = 3;
							npc.position -= npc.velocity;
						}

						if ((generalcounter + offsetter + 60) % ((50 + delay) * modez) == 60)
						{
							List<Projectile> itz = Idglib.Shattershots(npc.Center, P.Center, new Vector2(0, 0), ProjectileID.NebulaBolt, 30, 12f, 40, 2, true, 0, true, 200);
							SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 110);
							if (sound != null)
								sound.Pitch += 0.50f;

						}
					}
				}

				dpscap /= 1.05f;

				if (aicounter != 0 || drawdist > 0)
					drawdist += (1f - drawdist) * 0.01f;

				float dashdist = (2200 - (isboss * 800));
				if (npc.aiStyle == 41 && generalcounter%300>150)
				{
					dashdist /= 2f;
					npc.noGravity = true;
					npc.noTileCollide = true;
					npc.velocity = Vector2.Normalize(P.Center - npc.Center).RotatedBy(MathHelper.Pi / 3f) * 16f;
					npc.velocity.X /= 1.5f;
				}

				NoEscape(circlesize, circleLoc-P.MountedCenter);

				if (dist.Length() > (dashdist) || (aicounter == 0 && dist.Length() > 600) || pushtimes > 190 - adder)
				{

					if (stopmoving < 1)
					{
                        if (ploc.X > meloc.X)
                        {
                            npc.velocity = new Vector2(4f, (((ploc.Y + 8) - meloc.Y) / 8) + moveup);
                        }
                        else
                        {
                            npc.velocity = new Vector2(-4f, (((ploc.Y + 8) - meloc.Y) / 8) + moveup);
                        }
						float speed = 12;
						if (npc.aiStyle == 87)
							speed = npc.aiStyle == 63 ? 16 : 24;
						if (npc.velocity.LengthSquared() > 2 * 2)
						{
							npc.velocity.Normalize();
							npc.velocity = npc.velocity * speed;
						}
						npc.noTileCollide = true;
						npc.noGravity = true;
					}

				}
				else
				{
					if (npc.aiStyle != 63 && npc.aiStyle != 91 && npc.aiStyle != 23 && npc.aiStyle != 4 && npc.aiStyle != 69 && npc.aiStyle != 52 && npc.aiStyle != 19)
					{
						npc.noTileCollide = false;
						npc.noGravity = false;
					}
				}
				if (npc.aiStyle == 63 || npc.aiStyle == 87 || npc.aiStyle == 91 || npc.aiStyle == 23 || npc.aiStyle == 4 || npc.aiStyle == 69 || npc.aiStyle == 52 || npc.aiStyle == 19)
				{
					npc.noTileCollide = true;
					npc.noGravity = true;
				}
				if (npc.aiStyle == 19)
				{
					if (Main.rand.Next(300) < 4)
					{
						npc.velocity = new Vector2((float)(-6f + Main.rand.Next(12)), (float)(-6f + Main.rand.Next(12)));
					}
				}


				if (npc.boss == true)
				{
					if (phase>1 && generalcounter>0)
						npc.noGravity = true;
					npc.defense = ((NPC.CountNPCS(mod.NPCType("SPinkyClone"))) * 100) + fatherphase;
					if (((NPC.CountNPCS(mod.NPCType("SPinkyClone")) > 0) && Main.rand.Next(30) < 290) || fatherphase == 2)
					{
						npc.dontTakeDamage = true;
						if (fatherphase == 2)
						{
							//npc.life=npc.life+20;
						}
					}
					else
					{
						npc.dontTakeDamage = false;
					}

					if (Main.expertMode)
					{
						if (radpoison < 5 && (P.statLifeMax2 < P.statLifeMax * 0.15) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 5;
							Main.NewText("<Supreme Pinky> ALMOST FINISHED, SOON YOU SHALL BE NOTHING BUT SLIME FOR THE PRINCESS!", 255, 100, 255);
						}
						if (radpoison < 4 && (P.statLifeMax2 < P.statLifeMax * 0.3) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 4;
							Main.NewText("<Supreme Pinky> CONVERSION AT 70%!", 255, 100, 255);
						}
						if (radpoison < 3 && (P.statLifeMax2 < P.statLifeMax * 0.5) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 3;
							Main.NewText("<Supreme Pinky> YOUR BODY MELTS, CONVERSION AT 50%!", 255, 100, 255);
						}
						if (radpoison < 2 && (P.statLifeMax2 < P.statLifeMax * 0.75) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 2;
							Main.NewText("<Supreme Pinky> THE CONVERSION PROCESS IS PROCEEDING NICELY", 255, 100, 255);
						}

						if ((Main.npc[father] == null) || father < 1)
						{
							/*if (fatherphase==1){
								Main.NewText("<Supreme Pinky> NNNOOOOOOOO!!!! I'M DONE PLAYING GAMES!",255, 100, 255);
							npc.defDamage=200;
							npc.damage=200;
							fatherphase=90;
							}*/
							if (fatherphase == 0)
							{
								fatherphase = 1;
								Main.NewText("<Supreme Pinky> EXPERT HUH? YOU WILL SOON BE CONVERTED!", 255, 100, 255);
								/*
								father=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y - 32, 50);
							Main.npc[father].life=(int)(npc.lifeMax*0.9);
							Main.npc[father].lifeMax=(int)(npc.lifeMax*0.9);
							Main.npc[father].boss=true;
							Main.npc[father].aiStyle=87 ;
							Main.npc[father].ai[0]=npc.whoAmI;
							Main.npc[father].defense=550;
							Main.npc[father].damage=60;*/
							}
							if (fatherphase == 2)
							{
								Main.NewText("<Supreme Pinky> FATHER NOOOOOO... waaahhhhh....", 255, 100, 255);
								fatherphase = 3;
							}
						}
						else
						{
							if (Main.npc[father].life < 1)
							{
								Main.npc[father].StrikeNPCNoInteraction(999999, 0, 0);
								father = 0;
							}
						}

						if (npc.life < npc.lifeMax * 0.24 && fatherphase == -1)
						{
							fatherphase = 2;

							father = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y - 32, NPCID.KingSlime);
							Main.npc[father].life = (int)(npc.lifeMax * 0.9);
							Main.npc[father].lifeMax = (int)(npc.lifeMax * 0.9);
							Main.npc[father].boss = true;
							Main.npc[father].ai[0] = npc.whoAmI;
							Main.npc[father].damage = 60;
							Main.npc[father].defense = 50;
							Main.npc[father].aiStyle = 30;
							Main.npc[father].netUpdate = true;
							Main.NewText("<Supreme Pinky> OHHH, HE'S HURTING ME, HELP ME FATHER!", 255, 100, 255);
						}



					}
					if (npc.life < npc.lifeMax * 0.95)
					{
						aicounter = aicounter + 1;
						if (aicounter == 30)
						{
							int newguy1 = NPC.NewNPC((int)npc.Center.X - 13, (int)npc.Center.Y - 2, mod.NPCType("SPinkyClone"));
							int newguy2 = NPC.NewNPC((int)npc.Center.X - 13, (int)npc.Center.Y - 2, mod.NPCType("SPinkyClone"));
							Main.npc[newguy1].life = npc.lifeMax / 8;
							Main.npc[newguy1].lifeMax = npc.lifeMax / 8;
							Main.npc[newguy2].life = npc.lifeMax / 8;
							Main.npc[newguy2].lifeMax = npc.lifeMax / 8;
							Main.npc[newguy1].boss = false;
							Main.npc[newguy2].boss = false;
							Main.npc[newguy1].aiStyle = 87;
							Main.npc[newguy2].aiStyle = 87;
							Main.npc[newguy1].ai[0] = npc.whoAmI;
							Main.npc[newguy2].ai[0] = npc.whoAmI;
							Main.npc[newguy2].netUpdate = true;
							Main.npc[newguy1].netUpdate = true;
							npc.aiStyle = 15;
							Main.NewText("<Supreme Pinky> REAL MEN WEAR PINK! ", 255, 100, 255);
						}
					}

					if (npc.life < npc.lifeMax * 0.75)
					{
						if (aicounter < 90000)
						{
							aicounter = 90000;
							int newguy3 = NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
							int newguy4 = NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy3].life = npc.lifeMax / 4;
							Main.npc[newguy3].lifeMax = npc.lifeMax / 4;
							Main.npc[newguy4].life = npc.lifeMax / 4;
							Main.npc[newguy4].lifeMax = npc.lifeMax / 4;
							Main.npc[newguy3].boss = false;
							Main.npc[newguy4].boss = false;
							Main.npc[newguy3].defense = 0;
							Main.npc[newguy4].defense = 0;
							Main.npc[newguy3].aiStyle = 41;
							Main.npc[newguy4].aiStyle = 41;
							Main.npc[newguy3].ai[0] = npc.whoAmI;
							Main.npc[newguy4].ai[0] = npc.whoAmI;
							Main.npc[newguy3].netUpdate = true;
							Main.npc[newguy4].netUpdate = true;
							npc.aiStyle = 96;
							phase = 1;
							Main.NewText("<Supreme Pinky> YOU DARE RESIST THE PINK!?! ", 255, 100, 255);
						}
					}

					if (npc.life < npc.lifeMax * 0.50 || aicounter > 94050)
					{
						if (aicounter < 100000)
						{
							aicounter = 100000;
							int newguy666 = NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy666].life = npc.lifeMax / 6;
							Main.npc[newguy666].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy666].life = npc.lifeMax / 6;
							Main.npc[newguy666].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy666].boss = false;
							Main.npc[newguy666].defense = 0;
							Main.npc[newguy666].aiStyle = 91;
							Main.npc[newguy666].noGravity = true;
							Main.npc[newguy666].ai[0] = npc.whoAmI;
							Main.npc[newguy666].netUpdate = true;


							int newguy667 = NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy667].life = npc.lifeMax / 6;
							Main.npc[newguy667].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy667].life = npc.lifeMax / 6;
							Main.npc[newguy667].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy667].boss = false;
							Main.npc[newguy667].defense = 0;
							Main.npc[newguy667].aiStyle = 91;
							Main.npc[newguy667].noGravity = true;
							Main.npc[newguy667].ai[0] = npc.whoAmI;
							Main.npc[newguy667].netUpdate = true;


							int newguy668 = NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y - 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy668].life = npc.lifeMax / 6;
							Main.npc[newguy668].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy668].life = npc.lifeMax / 6;
							Main.npc[newguy668].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy668].boss = false;
							Main.npc[newguy668].defense = 0;
							Main.npc[newguy668].aiStyle = 91;
							Main.npc[newguy668].noGravity = true;
							Main.npc[newguy668].ai[0] = npc.whoAmI;
							Main.npc[newguy668].netUpdate = true;


							int newguy669 = NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y - 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy669].life = npc.lifeMax / 6;
							Main.npc[newguy669].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy669].life = npc.lifeMax / 6;
							Main.npc[newguy669].lifeMax = npc.lifeMax / 6;
							Main.npc[newguy669].boss = false;
							Main.npc[newguy669].defense = 0;
							Main.npc[newguy669].aiStyle = 91;
							Main.npc[newguy669].noGravity = true;
							Main.npc[newguy669].ai[0] = npc.whoAmI;
							Main.npc[newguy669].netUpdate = true;
							phase = 2;
							npc.aiStyle = 15;
							Main.NewText("<Supreme Pinky> YOU WILL BECOME PART OF THE PINK!!", 255, 100, 255);
						}
					}

					if (npc.life < npc.lifeMax * 0.30 && aicounter > 100150)
					{
						if (aicounter < 150000)
						{
							aicounter = 150000;
							npc.aiStyle = 96;
							npc.damage = 50;
							int newguy670 = NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"), npc.whoAmI, 0, npc.whoAmI);
							Main.npc[newguy670].life = npc.lifeMax / 5;
							Main.npc[newguy670].lifeMax = npc.lifeMax / 5;
							Main.npc[newguy670].boss = false;
							Main.npc[newguy670].defense = 0;
							Main.npc[newguy670].aiStyle = 48;
							Main.npc[newguy670].ai[0] = npc.whoAmI;
							Main.npc[newguy670].netUpdate = true;

							int newguy671 = NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"), npc.whoAmI, 0, npc.whoAmI);
							Main.npc[newguy671].life = npc.lifeMax / 5;
							Main.npc[newguy671].lifeMax = npc.lifeMax / 5;
							Main.npc[newguy671].boss = false;
							Main.npc[newguy671].defense = 0;
							Main.npc[newguy671].aiStyle = 48;
							Main.npc[newguy671].ai[0] = npc.whoAmI;
							Main.npc[newguy671].netUpdate = true;
							phase = 3;

							Main.NewText("<Supreme Pinky> THE PINK WILL NOT BE DENIED!", 255, 100, 255);
						}
					}


					if (npc.life < npc.lifeMax * 0.23 && aicounter > 100150 && fatherphase>1)
					{
						if (aicounter < 200000)
						{
							aicounter = 200000;
							npc.aiStyle = 63;
							npc.damage = 50;
							int newguy670 = NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy670].life = npc.lifeMax / 4;
							Main.npc[newguy670].lifeMax = npc.lifeMax; Main.npc[newguy670].boss = false;
							Main.npc[newguy670].defense = 10;
							Main.npc[newguy670].aiStyle = 4;
							Main.npc[newguy670].ai[0] = npc.whoAmI;
							Main.npc[newguy670].netUpdate = true;

							int newguy671 = NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
							Main.npc[newguy671].life = npc.lifeMax / 4;
							Main.npc[newguy671].lifeMax = npc.lifeMax;
							Main.npc[newguy671].boss = false;
							Main.npc[newguy671].defense = 10;
							Main.npc[newguy671].aiStyle = 4;
							Main.npc[newguy671].ai[0] = npc.whoAmI;
							Main.npc[newguy671].netUpdate = true;

							phase = 4;
							Main.NewText("<Supreme Pinky> WE WILL NOT STOP!!", 255, 100, 255);
						}
					}

					if (npc.life < npc.lifeMax * 0.20 && npc.aiStyle != 69 && !npc.dontTakeDamage && fatherphase > 1)
					{
						npc.aiStyle = 69;
						npc.damage = 20;
						npc.ai[0] = 0;
						npc.defense = 300;
						Main.NewText("<Supreme Pinky> PINKY WILL NOT DIE AGAIN!", 255, 100, 255);
					}






					if (aicounter == 90500)
					{
						Main.NewText("<Supreme Pinky> MY CHIDREN! ASSIST YOUR PRINCESS!", 255, 100, 255);
					}
					if (aicounter == 100550)
					{
						Main.NewText("<Supreme Pinky> HE'S STRONG, SLIMES TO ME!", 255, 100, 255);
					}
					if (aicounter == 200550)
					{
						Main.NewText("<Supreme Pinky> MY CHILDREN! I NEED YOU ALL!", 255, 100, 255);
					}

					if (aicounter > 90560 && aicounter < 91220)
					{
						if (aicounter % 50 == 0)
						{
							for (int i = 0; i <= 2; i++)
							{
								int newguy5 = NPC.NewNPC((int)npc.Center.X + 150 - Main.rand.Next(300), (int)npc.Center.Y + 30, 1);
								Main.npc[newguy5].life = npc.lifeMax / 30;
								Main.npc[newguy5].lifeMax = npc.lifeMax / 30;
								Main.npc[newguy5].life = npc.lifeMax / 30;
								Main.npc[newguy5].lifeMax = npc.lifeMax / 30;
								Main.npc[newguy5].boss = false;
								Main.npc[newguy5].defense = 90;
								if (aicounter > 90650)
								{
									Main.npc[newguy5].aiStyle = 23;
								}
								else
								{
									Main.npc[newguy5].aiStyle = 44;
									Main.npc[newguy5].knockBackResist = 0.9f;
								}
								Main.npc[newguy5].damage = 50;
								Main.npc[newguy5].netUpdate = true;
							}
						}


					}

					if (aicounter > 100550 && aicounter < 101750)
					{
						if (aicounter % 64 == 0)
						{
							int newguy55 = NPC.NewNPC((int)P.Center.X + Main.rand.Next(-300, 300), (int)P.Center.Y - 320, 16, npc.whoAmI);
							Main.npc[newguy55].life = npc.lifeMax / 12;
							Main.npc[newguy55].lifeMax = npc.lifeMax / 12;
							Main.npc[newguy55].boss = false;
							Main.npc[newguy55].defense = 40;
							Main.npc[newguy55].noTileCollide = true;
							Main.npc[newguy55].noGravity = true;
							Main.npc[newguy55].aiStyle = 49;
							Main.npc[newguy55].damage = 55;
							Main.npc[newguy55].netUpdate = true;

							int newguy16 = NPC.NewNPC((int)P.Center.X - 800, (int)P.Center.Y - 30, 1);
							Main.npc[newguy16].life = npc.lifeMax / 15;
							Main.npc[newguy16].lifeMax = npc.lifeMax / 15;
							Main.npc[newguy16].boss = false;
							Main.npc[newguy16].defense = 50;
							Main.npc[newguy16].noTileCollide = true;
							Main.npc[newguy16].noGravity = true;
							Main.npc[newguy16].aiStyle = 10;
							Main.npc[newguy16].damage = 63;
							Main.npc[newguy16].netUpdate = true;

							int newguy116 = NPC.NewNPC((int)P.Center.X + 800, (int)P.Center.Y - 30, 1);
							Main.npc[newguy116].life = npc.lifeMax / 15;
							Main.npc[newguy116].lifeMax = npc.lifeMax / 15;
							Main.npc[newguy116].boss = false;
							Main.npc[newguy116].defense = 60;
							Main.npc[newguy116].noTileCollide = true;
							Main.npc[newguy116].noGravity = true;
							Main.npc[newguy116].aiStyle = 10;
							Main.npc[newguy116].damage = 63;
							Main.npc[newguy16].netUpdate = true;

						}
					}

					if (aicounter > 200550 && NPC.CountNPCS(NPCID.DungeonSlime) < 5)
					{
						if (aicounter % 200 == 0)
						{
							int newguy5 = NPC.NewNPC((int)npc.Center.X, (int)P.Center.Y, NPCID.DungeonSlime);
							Main.npc[newguy5].life = npc.lifeMax / 3;
							Main.npc[newguy5].lifeMax = npc.lifeMax / 3;
							Main.npc[newguy5].life = npc.lifeMax / 3;
							Main.npc[newguy5].lifeMax = npc.lifeMax / 3;
							Main.npc[newguy5].boss = false;
							Main.npc[newguy5].defense = 70;
							Main.npc[newguy5].noTileCollide = true;
							Main.npc[newguy5].noGravity = true;
							Main.npc[newguy5].aiStyle = 43;
							Main.npc[newguy5].damage = 87;
							Main.npc[newguy5].netUpdate = true;
						}
					}
					if (aicounter > 200550)
					{

						//nope
					}




				}

				if (npc.boss == false)
				{
					npc.dontTakeDamage = false;
					if (npc.ai[0] == null || npc.life < 1)
					{
						npc.StrikeNPCNoInteraction(999999, 0, 0);
						npc.active = false;
					}
					float lifepercent = npc.aiStyle == 11 ? 0.25f : 0.75f;
					float lifepercent2 = npc.aiStyle == 11 ? 0.15f : 0.50f;
					if (npc.aiStyle == 87 || npc.aiStyle == 11)
					{
						if (aicounter < 990999 && npc.life < npc.lifeMax * 0.6)
						{
							aicounter = 1000000;
							int newguy1 = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));
							Main.npc[newguy1].life = (int)(npc.lifeMax * lifepercent);
							Main.npc[newguy1].lifeMax = (int)(npc.lifeMax * lifepercent);
							Main.npc[newguy1].boss = false;
							Main.npc[newguy1].aiStyle = 63;
							Main.npc[newguy1].ai[0] = npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.npc[newguy1].noGravity = true;
							Main.NewText("<Supreme Pinky> JOIN THE PINK SIDE OF THE FORCE!", 255, 100, 255);
						}

					}

					if (npc.aiStyle == 48)
					{
						if (aicounter < 99999 && npc.life < npc.lifeMax * 0.4)
						{
							aicounter = 100000;
							int newguy1 = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));
							Main.npc[newguy1].life = (int)(npc.lifeMax * lifepercent2);
							Main.npc[newguy1].lifeMax = (int)(npc.lifeMax * lifepercent2);
							Main.npc[newguy1].boss = false;
							Main.npc[newguy1].aiStyle = 41;
							Main.npc[newguy1].noGravity = true;
							//Main.npc[newguy1].ai[0] = npc.ai[0];
							Main.npc[newguy1].netUpdate = true;

							int newguy2 = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));
							Main.npc[newguy2].life = (int)(npc.lifeMax * lifepercent);
							Main.npc[newguy2].lifeMax = (int)(npc.lifeMax * lifepercent);
							Main.npc[newguy2].boss = false;
							Main.npc[newguy2].aiStyle = 87;
							//Main.npc[newguy2].ai[0] = npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> PPPIIINNNKKKK!!!", 255, 100, 255);
						}

					}

					if (npc.aiStyle == 4)
					{
						if (aicounter < 99999 && npc.life < npc.lifeMax * 0.15)
						{
							aicounter = 100000;
							int newguy1 = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));
							Main.npc[newguy1].life = (int)(npc.lifeMax * 0.40);
							Main.npc[newguy1].lifeMax = (int)(npc.lifeMax * 0.40);
							Main.npc[newguy1].boss = false;
							Main.npc[newguy1].aiStyle = 11;
							Main.npc[newguy1].ai[0] = npc.ai[0];
							Main.npc[newguy1].netUpdate = true;

							int newguy2 = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));
							Main.npc[newguy2].life = (int)(npc.lifeMax * 0.40);
							Main.npc[newguy2].lifeMax = (int)(npc.lifeMax * 0.40);
							Main.npc[newguy2].boss = false;
							Main.npc[newguy2].aiStyle = 11;
							Main.npc[newguy2].ai[0] = npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> PINK PINK PINK PINK!", 255, 100, 255);
						}

					}


					if (npc.aiStyle == 41)
					{
						if (aicounter < 99999 && npc.life < npc.lifeMax * 0.4)
						{
							aicounter = 100000;
							int newguy1 = NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));
							Main.npc[newguy1].life = (int)(npc.lifeMax * 0.75);
							Main.npc[newguy1].lifeMax = (int)(npc.lifeMax * 0.75);
							Main.npc[newguy1].boss = false;
							Main.npc[newguy1].aiStyle = 38;
							Main.npc[newguy1].ai[0] = npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> THE PINK WILL CONSUME YOU!", 255, 100, 255);
						}

					}




				}


			}
		}




		public override bool CheckActive()
		{
			return npc.life < 1 || (npc.timeLeft < 2 && GetType() == typeof(SPinkyTrue));
		}
		public override bool CheckDead()
		{
			return true;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			if (GetType() == typeof(SPinkyTrue))
				return;

			damage = (int)Math.Pow(damage, 0.85);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (GetType() == typeof(SPinkyTrue))
				return;

			if (GetType() == typeof(SPinkyClone))
			{
				damage = (int)Math.Pow(damage, 0.96);
				if (npc.aiStyle != 69)
					damage = (int)(damage / (1 + (dpscap / 1000f)));
				dpscap += damage;

			}
			else
			{

					if (GetType() == typeof(SPinky))
					damage = (int)Math.Pow(damage, 0.85);
				if (npc.aiStyle != 69)
					damage = (int)(damage / (1 + (dpscap / 750f)));
				dpscap += damage;
			}
		}




	}

	[AutoloadBossHead]
	public class SPinkyClone : SPinky
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Not Supreme Pinky");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.netAlways = false;
			npc.boss = false;
		}

		public override void NPCLoot()
		{
			//nothing
		}

		public override string Texture
		{
			get { return ("SGAmod/NPCs/SPinky"); }
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			//sss
		}

		/*public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			//sss
		}*/


	}

	public class SlimeProjectile : ModProjectile
	{
		public Color color;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("A Rouge Slime");
		}
		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 600;
			aiType = -1;
			projectile.aiStyle = -1;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/AcidRocket"; }
		}

		public override void AI()
        {
			if (projectile.ai[0] == 0)
            {
				color = Main.hslToRgb(Main.rand.NextFloat() % 1f, 1f, 0.65f);
				projectile.ai[1] = Main.rand.NextFloat(-0.5f,0.5f);
            }
			int index = NPC.FindFirstNPC(ModContent.NPCType<SPinkyTrue>());
			if (index >= 0)
			{
				NPC pinky = Main.npc[index];
				projectile.ai[0] += 1;

				if (pinky != null && pinky.active)
				{
					float length = projectile.velocity.Length();
					Vector2 vector = pinky.Center - projectile.Center;
					projectile.velocity = Vector2.Normalize(vector) * length;

					if (vector.Length() < 96 || pinky.ai[0] > 1000000)
					{
						projectile.Kill();
					}
					return;
				}
			}
			projectile.Kill();
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override bool CanDamage()
		{
			return true;
		}
	}

	public class PinkyRingAttack : ModProjectile
	{
		Effect effect => SGAmod.TrailEffect;
		public int maxTime = 520;
		public int flashTime = 120;
		public int ringSize = 48;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Ring");
		}

		public override string Texture => "SGAmod/HopefulHeart";

		public override void SetDefaults()
		{
			projectile.width = 132;
			projectile.height = 132;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.tileCollide = false;
			projectile.alpha = 40;
			projectile.timeLeft = maxTime;
			projectile.extraUpdates = 0;
			projectile.ignoreWater = true;
			projectile.damage = 20;
		}

		float FlashTimer => (float)Math.Sin((projectile.ai[0] / (float)flashTime) * MathHelper.TwoPi);

		public override void AI()
		{
			float realsize = ((projectile.width * 32f) * 0.5f) * 1.1f;
			projectile.ai[0] += 1;
			projectile.localAI[0] += 1;
			if (FlashTimer > 0 && projectile.timeLeft > 60)
			{
				foreach (Player player in Main.player.Where(testby => ringSize - Math.Abs(((testby.Center) - projectile.Center).Length() - (realsize * ((float)projectile.localAI[0] / (float)maxTime))) > 0))
				{
					player.Hurt(PlayerDeathReason.ByProjectile(255, projectile.whoAmI), projectile.damage, 0, Crit: true);
				}
			}
		}

		public override bool CanDamage()
		{
			return FlashTimer > 0 && projectile.timeLeft > 60 && projectile.localAI[0]>60;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D mainTex = Main.projectileTexture[projectile.type];
			Effect RadialEffect = SGAmod.RadialEffect;
			float alpha = MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 0.50f+MathHelper.Clamp(FlashTimer,-0.25f,0.50f))*Math.Min((projectile.localAI[0]-30f)/30f,1f);
			Vector2 half = mainTex.Size() / 2f;

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));//SGAmod.PearlIceBackground
			RadialEffect.Parameters["alpha"].SetValue(0.80f*alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f, -Main.GlobalTime * 0.175f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 2f));
			RadialEffect.Parameters["ringScale"].SetValue(0.075f*((80f/ (float)projectile.width) * ((float)ringSize/64f)));
			RadialEffect.Parameters["ringOffset"].SetValue((1f-(projectile.timeLeft/(float)maxTime))*0.9f);
			RadialEffect.Parameters["ringColor"].SetValue(Color.Pink.ToVector3());
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.LightGray, 0, half, projectile.width*2f, default, 0);

			RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Stain"));//SGAmod.PearlIceBackground
			RadialEffect.Parameters["alpha"].SetValue(1.25f * alpha);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * -0.125f, -Main.GlobalTime * 0.175f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 2f));
			RadialEffect.Parameters["ringScale"].SetValue(0.05f * ((80f / (float)projectile.width) * ((float)ringSize / 64f)));
			RadialEffect.Parameters["ringColor"].SetValue(Color.Magenta.ToVector3());

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.LightGray, 0, half, projectile.width * 2f, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

	}

	public class PinkyWarning : Hellion.HellionBolt
	{
		protected float timeleft = 150f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Warning Forever!");
		}
		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = true;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 150;
			projectile.extraUpdates = 1;
			aiType = -1;
			projectile.aiStyle = -1;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float there = projectile.velocity.ToRotation() - MathHelper.ToRadians(-90);
			//if (projectile.ai[0] < 120)
			//{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			spriteBatch.Draw(SGAmod.ExtraTextures[60], startpos - Main.screenPosition, null, Color.Purple * MathHelper.Clamp(projectile.timeLeft / (float)timeleft, 0f, 0.75f), there, (SGAmod.ExtraTextures[60].Size() / 2f) + new Vector2(0, 12), new Vector2(0.75f, projectile.ai[0]* projectile.damage), SpriteEffects.None, 0f);
			//}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}
	public class PinkyExplode : ModProjectile
	{
		protected float timeleft = 150f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pinky is dying animation");
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/AcidRocket"; }
		}
		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = false;
			projectile.tileCollide = false;
			projectile.timeLeft = 560;
			projectile.extraUpdates = 0;
			aiType = -1;
			projectile.aiStyle = -1;
		}

        public override void AI()
        {
			projectile.ai[0] += 1;
			if (projectile.timeLeft < 60)
				projectile.ai[0] += 50;

		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Texture2D texture2 = Main.itemTexture[ModContent.ItemType<StygianCore>()];
			Texture2D texture = SGAmod.ExtraTextures[96];
			//if (projectile.ai[0] < 120)
			//{

			/*for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi/16f)
			{
				float there = f-Main.GlobalTime/10f;
				spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, (Color.Pink* 0.25f) * MathHelper.Clamp((projectile.timeLeft-60) / 60f, 0f, projectile.ai[0] / 800f), there, texture2.Size() / 2f, new Vector2(4f, 4f)+ (new Vector2(32f, 32f)/(1+(projectile.ai[0] / 300f))), SpriteEffects.None, 0f);
			}*/

			VertexBuffer vertexBuffer;
			Effect effect = SGAmod.TrailEffect;

			effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Main.GameViewMatrix.Zoom) * WVP.Projection());
			effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("Space"));
			effect.Parameters["coordOffset"].SetValue(0);
			effect.Parameters["coordMultiplier"].SetValue(4f);
			effect.Parameters["strength"].SetValue(MathHelper.Clamp((projectile.timeLeft - 60) / 120f, 0f, projectile.ai[0] / 800f));

			VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

			Vector3 screenPos = (projectile.Center-Main.screenPosition).ToVector3();
			float skymove = ((Math.Max(Main.screenPosition.Y - 8000, 0)) / (Main.maxTilesY * 16f));
			float size = 640f + (640f/(1 + (projectile.ai[0] / 300f)));

			vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-size, -size, 0), Color.DeepPink, new Vector2(0, 0));
			vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-size, size, 0), Color.DeepPink, new Vector2(0, 1));
			vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(size, -size, 0), Color.DeepPink, new Vector2(1, 0));

			vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(size, size, 0), Color.DeepPink, new Vector2(1, 1));
			vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-size, size, 0), Color.DeepPink, new Vector2(0, 1));
			vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(size, -size, 0), Color.DeepPink, new Vector2(1, 0));

			vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
			vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

			Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

			RasterizerState rasterizerState = new RasterizerState();
			rasterizerState.CullMode = CullMode.None;
			Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

			effect.CurrentTechnique.Passes["DefaultPassSinShade"].Apply();

			Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 5f)
			{
				float there = f+Main.GlobalTime/10f;
				spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, (Color.White* 0.20f) * MathHelper.Clamp(projectile.timeLeft / 30f, 0f, 1f), there, texture.Size() / 2f, new Vector2(0.95f, 1f) * (projectile.ai[0] / 150f), SpriteEffects.None, 0f);
			}

			//}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
        }

        public override bool PreKill(int timeLeft)
		{
			return true;
		}

        public override bool CanDamage()
        {
            return false;
        }
	}



}

