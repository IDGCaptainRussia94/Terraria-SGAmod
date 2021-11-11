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
using SGAmod.Dimensions;
using SGAmod.Dimensions.NPCs;
using Terraria.Cinematics;
using Terraria.Graphics;
using System.Linq;
using SGAmod.Effects;
using Terraria.Utilities;

namespace SGAmod.NPCs.Wraiths
{
	public enum StateIds
    {
		Nothing = 0,
		Transform = 100,
		PhaseAdvance = 99,
		Circle = 1,
		DashAtAnimedAngle = 2,
		LookAtPlayer = 3,
		CircleLeader = 4,
		Hover = 5,
	}

	public class SwordAttackSpawnProjectilesNovaBurst : SwordAttackSpawnProjectilesAtLooking, ICloneable
	{
		public int ammount = 10;



		public SwordAttackSpawnProjectilesNovaBurst(int type, int time, float timerVariable = 120, int projHowOften = 10) : base(type, time, timerVariable)
		{
			this.projHowOften = projHowOften;
		}
		public override void Update()
		{
			if (boss.SpecialStateTimer % projHowOften == 0)
			{
				for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / (float)ammount)
				{
					Projectile proj = Main.projectile[Projectile.NewProjectile(boss.npc.Center, (boss.npc.rotation+f+(boss.specialState == (int)StateIds.DashAtAnimedAngle ? MathHelper.PiOver2 : 0) - MathHelper.PiOver4).ToRotationVector2() * projSpeed, projType, damage, 5f)];
					proj.hostile = true;
					proj.friendly = false;
				}
			}
		}
	}

	public class SwordAttackSpawnProjectilesAtLooking : SwordAttack, ICloneable
	{
		public int projHowOften = 10;
		public float projSpeed = 5f;
		public int projType = ModContent.ProjectileType<CalburnSwordAttack>();
		public int damage = 30;


		public SwordAttackSpawnProjectilesAtLooking(int type, int time, float timerVariable = 120,int projHowOften=10) : base(type, time, timerVariable)
		{
			this.projHowOften = projHowOften;
		}
		public override void Update()
		{
			if (boss.SpecialStateTimer % projHowOften == 0)
			{
				Projectile proj = Main.projectile[Projectile.NewProjectile(boss.npc.Center, (boss.npc.rotation - MathHelper.PiOver4).ToRotationVector2() * projSpeed, projType, damage, 5f)];
				proj.hostile = true;
				proj.friendly = false;
			}
		}
	}

	public class SwordAttack : ICloneable
    {
		public int type = 0;
		public int time = 0;
		public float timerVariable = 120f;
		public float spinSpeed = 1f;
		public float lookAt = 0f;
		public Vector2 hoverArea = Vector2.One;
		public Vector2 timerVariable2 = Vector2.One;
		public CaliburnGuardianHardmode boss;

		public SwordAttack(int type,int time, float timerVariable = 120)
        {
			this.type = type;
			this.time = time;
			this.timerVariable = timerVariable;
		}

		public virtual void Update()
        {

        }

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}


	[AutoloadBossHead]
	public class CaliburnGuardianHardmode : CaliburnGuardian, ISGABoss
	{
		public int specialState = 0;
		public int SpecialStateTimer
        {
            get
            {
				return Math.Max(_specialStateTimer+alteredStateTimer,0);
			}
            set
            {
				_specialStateTimer = value;
			}
        }
		public int _specialStateTimer = 0;
		public int alteredStateTimer = 0;
		public int specialStateMaxTime = 0;
		public Vector2 specialStateVar;
		public bool Leader => leaderId == null;
		float swordFadeOut = 0f;
		public int leftOrRight = 0;
		public List<CaliburnGuardianHardmode> brothers = new List<CaliburnGuardianHardmode>();
		public CaliburnGuardianHardmode leaderId = null;
		public static Film film = new Film();
		int camlength;
		int camlength2;
		public float trailAlpha = 0f;
		public int globalStateTimer = 0;

		public static Vector3 cutscenestartpos;
		public static Vector3 scenecamend;
		public static Vector3 scenecam;
		public float spinSpeed = 1f;
		public static int cutsceneposition = 0;
		List<SwordAttack> OrderedAttacks = new List<SwordAttack>();
		SwordAttack currentAttack = null;
		public int phase = 0;

		public static bool CutSceneActive(ref bool cam) => cam || CaliburnGuardianHardmode.film.IsActive;

		public override bool Autoload(ref string name)
        {
			SGAmod.ModifyTransformMatrixEvent += MoveCamera;
			SGAWorld.CutsceneActiveEvent += CutSceneActive;
			return true;
        }

        public override bool CheckDead()
        {
			if (specialState == 0)
			{
				ChangeState(100);
				npc.lifeMax *= 3;
				npc.life = npc.lifeMax;


				camlength = 75;
				camlength2 = 300;
				cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 1f);

				scenecam = cutscenestartpos;
				CaliburnGuardianHardmode.film = new Film();
				//scenecamend = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				CaliburnGuardianHardmode.film.AppendSequence(camlength, FilmSetCamera);
				CaliburnGuardianHardmode.film.AppendSequence(camlength2 - camlength, FilmSetCamera);
				CaliburnGuardianHardmode.film.AppendSequence(75, FilmSetCamera);

				CaliburnGuardianHardmode.film.AddSequence(0, CaliburnGuardianHardmode.film.AppendPoint, PerFrameSettings);

				CinematicManager.Instance.PlayFilm(CaliburnGuardianHardmode.film);
				return false;
			}

			//if (npc.realLife >= 0)
			//{
				//brothers.Add(this);
				foreach (CaliburnGuardianHardmode sworder in brothers)
				{
					sworder.npc.realLife = -1;
					sworder.npc.Center = npc.Center;
					sworder.NPCLoot();
					sworder.npc.boss = false;
					sworder.npc.active = false;
				}
			//}
			//npc.StrikeNPC(npc.lifeMax, 5, 1);

			return true;
        }

		private void FilmSetCamera(FrameEventData evt)
		{
			if (evt.Frame < 2)
				cutscenestartpos = new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 1f);

			if (evt.AbsoluteFrame < camlength)
			{
				Vector2 diff = npc.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				scenecamend = Vector3.Lerp(cutscenestartpos, new Vector3(diff.X, diff.Y, 1f), evt.Frame / (float)evt.Duration);
			}

			if (evt.AbsoluteFrame > camlength2)
			{
				Vector2 diff = Main.LocalPlayer.Center - (new Vector2(Main.screenWidth, Main.screenHeight) / 2f);
				scenecamend = Vector3.Lerp(cutscenestartpos, new Vector3(diff.X, diff.Y, 1f - MathHelper.Clamp(((evt.Frame / (float)evt.Duration) * 3f) - 2f, 0f, 1f)), (evt.Frame / (float)evt.Duration) * (1.25f));
			}

			scenecam += (scenecamend - scenecam) * 0.05f;
			//Main.screenPosition = scenecam;
		}

		private void PerFrameSettings(FrameEventData evt)
			{
				cutsceneposition = 2;
			}

			private void MoveCamera(ref SpriteViewMatrix transform)
			{
				if (CaliburnGuardianHardmode.cutsceneposition > 0 && CaliburnGuardianHardmode.film.IsActive)
				{
					Vector2 oldpos = Main.screenPosition;
					Main.screenPosition = Vector2.Lerp(new Vector2(CaliburnGuardianHardmode.scenecam.X, CaliburnGuardianHardmode.scenecam.Y), Main.screenPosition, MathHelper.Clamp(1f - CaliburnGuardianHardmode.scenecam.Z, 0f, 1f));
				}
			}

			public static CaliburnGuardianHardmode SpawnBrother(CaliburnGuardianHardmode leader, Vector2 location,int state,int stateTime,int LeftOrRight, int npcai2)
        {
			int npcid = NPC.NewNPC((int)location.X, (int)location.Y, ModContent.NPCType<CaliburnGuardianHardmode>());
			if (npcid >= 0)
            {
				NPC npc = Main.npc[npcid];
				CaliburnGuardianHardmode brother = npc.modNPC as CaliburnGuardianHardmode;
				brother.specialState = state;
				brother.SpecialStateTimer = stateTime;
				brother.leaderId = leader;
				npc.realLife = leader.npc.whoAmI;
				npc.ai[2] = npcai2;
				leader.brothers.Add(brother);
				brother.brothers.Add(leader);
				brother.leftOrRight = LeftOrRight;
				npc.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
				npc.netUpdate = true;
				brother.appear = 1f;
				return brother;
			}
			return null;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath of Caliburn");
			Main.npcFrameCount[npc.type] = 1;
		}

		public void AddAttackOrder()
		{

		tryagain:

			OrderedAttacks.Clear();

			WeightedRandom<int> wrand = new WeightedRandom<int>();
			//if (phase < 1)

			wrand.Add(4, 1.0);

			wrand.Add(0, phase > 1 ? 1.25 : 1);

			wrand.Add(1, 1.0);
			if (phase > 0)
			{
				wrand.Add(5, 1.0);
				wrand.Add(2, 1.0);
				wrand.Add(3, 1.0);
			}
			if (phase > 0)
			{
				wrand.Add(41, 1.5);
			}

			int finalchoice = wrand.Get();
			//if (specialState == 100)//always to start off
			//	finalchoice = 0;

			alteredStateTimer = 0;
			spinSpeed = 1f;

			/*if (finalchoice == 0 && !Leader)
			{
				wrand = new WeightedRandom<int>();
                if (phase > 1)
                {
                    wrand.Add(40, 1.0);
					wrand.Add(3, 1.0);
					wrand.Add(1, 1.0);

                    finalchoice = wrand.Get();
                }
			}*/

			bool bouldermode = false;

			if (finalchoice == 5)//Boulders!
			{
				if (!Leader)
				{
					int gyiser = phase > 1 ? (Main.rand.NextBool() ? -1 : 1) : 1;


					SwordAttack sword1 = new SwordAttack((int)StateIds.Hover, 180, 700);
					sword1.hoverArea = new Vector2(P.Center.X - 960 * leftOrRight, P.Center.Y - 160 * gyiser);
					sword1.lookAt = MathHelper.PiOver2 * gyiser;
					OrderedAttacks.Add(sword1);

					SwordAttackSpawnProjectilesAtLooking sword = new SwordAttackSpawnProjectilesAtLooking((int)StateIds.Hover, 260, 2000);
					sword.hoverArea = new Vector2(P.Center.X + 1280 * leftOrRight, P.Center.Y - 160* gyiser);
					sword.lookAt = MathHelper.PiOver2* gyiser;
					sword.projType = gyiser> 0 ? ProjectileID.Boulder : ProjectileID.GeyserTrap;
					sword.damage = 75;
					sword.projHowOften = 20;
					sword.projSpeed = gyiser < 0 ? 8f : 1f;
					OrderedAttacks.Add(sword);

					SwordAttack sword4 = new SwordAttack((int)StateIds.Circle, 800, 2400);
					OrderedAttacks.Add(sword4);

                }
                else
                {
					wrand = new WeightedRandom<int>();
					if (phase > 1)
					{
						wrand.Add(40, 1.0);
					}
					else
					{
						wrand.Add(0, 1.0);
						wrand.Add(1, 1.0);
					}
					bouldermode = true;
					finalchoice = wrand.Get();
				}
			}

			if (finalchoice == 4 || finalchoice == 40 || finalchoice == 41)//Chasing Nova Burst
			{
				if (finalchoice == 41 && !Leader)
				{
					finalchoice = 3;

				}
				else
				{


					//if (Leader)
					//{
					int forty = finalchoice == 40 ? 6 : 3;
					SwordAttack swordsd = new SwordAttack((int)StateIds.LookAtPlayer, 30 + (leftOrRight * 30), 3200);
					OrderedAttacks.Add(swordsd);

					for (int i = 0; i < forty; i++)
					{

						SwordAttack sword = new SwordAttack((int)StateIds.Hover, i < 1 ? (finalchoice > 3 ? 160 : 60) : 30, i < 1 ? (finalchoice > 3 ? 160 : 420) : 30);
						sword.hoverArea = P.Center + new Vector2(Main.rand.Next(-640, 640), Main.rand.Next(-420, -240));
						sword.lookAt = Main.rand.NextFloat(MathHelper.TwoPi);
						OrderedAttacks.Add(sword);

						SwordAttackSpawnProjectilesNovaBurst sword2 = new SwordAttackSpawnProjectilesNovaBurst((int)StateIds.LookAtPlayer, 4, 3200);
						sword2.projSpeed = 8f;
						sword2.ammount = (bouldermode && phase>1 ? 16 : (finalchoice == 41 ? 12 : 10))+((phase * 2)-2);
						//sword2.projType = ProjectileID.DiamondBolt;
						sword2.projHowOften = 3;
						OrderedAttacks.Add(sword2);

						SwordAttack sword3 = new SwordAttack((int)StateIds.LookAtPlayer, 20, 3200);
						OrderedAttacks.Add(sword3);
					}
					SwordAttack sword4 = new SwordAttack((int)StateIds.LookAtPlayer, 60, 3200);
					OrderedAttacks.Add(sword4);
				}
			}


			if (finalchoice == 3)//Circle Swords
			{
				UnifiedRandom rando = new UnifiedRandom(SGAWorld.modtimer);
				float spinspeedz = phase > 1 ? -0.60f : rando.NextFloat(0.9f, 1.2f);
				SwordAttack attacc = new SwordAttack((int)StateIds.Circle, 100, 100);
				attacc.spinSpeed = spinspeedz;

				OrderedAttacks.Add(attacc);
				for (int i = 0; i < 3; i++)
				{
					SwordAttack spin = new SwordAttackSpawnProjectilesAtLooking((int)StateIds.Circle, 60, 1);
					spin.spinSpeed = spinspeedz;
					OrderedAttacks.Add(spin);

					SwordAttack attac = new SwordAttack((int)StateIds.Circle, 100, 1);
					attac.spinSpeed = spinspeedz;

					OrderedAttacks.Add(attac);
				}
			}

			if (finalchoice == 2)//Combined Sword dashing
			{
				if (Leader)
				{
					OrderedAttacks.Add(new SwordAttack((int)StateIds.Circle, 80, 120));
					for (int i = 0; i < 7; i++)
					{
						OrderedAttacks.Add(new SwordAttack((int)StateIds.LookAtPlayer, 15, 0.25f));
						SwordAttack swrod = new SwordAttack((int)StateIds.DashAtAnimedAngle, 25, 5);
						swrod.timerVariable2.X = 1.75f;
						OrderedAttacks.Add(swrod);
					}
					OrderedAttacks.Add(new SwordAttack((int)StateIds.Circle, 200, 240));
				}
				else
				{
					for (int i = 0; i < 1; i++)
					{
						OrderedAttacks.Add(new SwordAttack((int)StateIds.CircleLeader, 360, 25));
					}
					/*OrderedAttacks.Add(new SwordAttack((int)StateIds.LookAtPlayer, 15, 2));
					SwordAttack swrod = new SwordAttack((int)StateIds.DashAtAnimedAngle, 60, 5);
					swrod.timerVariable2.X = 0.75f;
					OrderedAttacks.Add(swrod);*/
					OrderedAttacks.Add(new SwordAttack((int)StateIds.Circle, 1200, 160));
				}
			}

			if (finalchoice == 1)//Dash indivually at the enemy
			{

				OrderedAttacks.Add(new SwordAttack((int)StateIds.Circle, 150+(phase > 0 ? leftOrRight*45 : 0), 320));

				for (int i = 0; i < 6+(Leader ? 0 : -1); i++)
				{
					OrderedAttacks.Add(new SwordAttack((int)StateIds.LookAtPlayer, 30, 20));
					if (phase > 1)
					{
						SwordAttackSpawnProjectilesNovaBurst novaBurst = new SwordAttackSpawnProjectilesNovaBurst((int)StateIds.DashAtAnimedAngle, 30, 15);
						novaBurst.ammount = 2;
						novaBurst.projHowOften = 16;
						novaBurst.projSpeed = 7;
						OrderedAttacks.Add(novaBurst);
					}
					else
					{
						OrderedAttacks.Add(new SwordAttack((int)StateIds.DashAtAnimedAngle, 30, 15));
					}
				}
				if (!Leader)
					OrderedAttacks.Add(new SwordAttack((int)StateIds.LookAtPlayer, 300, 150));
			}

			if (finalchoice == 0)//Circle and dash in
			{
				//if (phase > 1)
				//alteredStateTimer = 30 + (leftOrRight * 30);

				int delay = 0;
				if (phase > 1)
					delay = 3 + (leftOrRight * 3);

				OrderedAttacks.Add(new SwordAttack((int)StateIds.Circle, delay+(specialState == 100 ? 300 : 240), specialState == 100 ? 240 : 640));

				SwordAttack dasher = new SwordAttack((int)StateIds.DashAtAnimedAngle, 60, 25);
				if (phase > 0)
				{
					dasher.timerVariable2.X = 1.50f;
					dasher.time = (40);
					dasher.timerVariable = 20f;
				}
				if (phase > 1)
				{
					dasher.timerVariable2.X = 1.75f;
				}

				OrderedAttacks.Add(dasher);

				for (int i = 0; i < 2; i++)
				{
					SwordAttack attack = new SwordAttack((int)StateIds.Circle, (phase > 0 ? 120 : 150) + delay, 100);
						attack.timerVariable2.Y = 0.50f;
					if (phase > 1)
					{
						attack.timerVariable2.Y = 0.40f;
						attack.spinSpeed = 0.75f;
					}
					OrderedAttacks.Add(attack);
					OrderedAttacks.Add(dasher);
				}
				OrderedAttacks.Add(new SwordAttack((int)StateIds.LookAtPlayer, 50,50));
			}


			if (debugMode)
				Main.NewText("added new attacks: " + OrderedAttacks.Count);

		}

		public void PhaseAdvanceState()
		{
			npc.dontTakeDamage = true;

			if (SpecialStateTimer < 140)
			{
				appear *= 0.75f;
				appear -= 0.02f;
				if (appear < 0)
					appear = 0;
			}

			float timeMax = (float)200f;

			float easeIn = MathHelper.Clamp(SpecialStateTimer / timeMax, 0f, 1f);

			Vector2 playerPos = P.Center + new Vector2(leftOrRight * 240f, -240f);

			npc.Center += (playerPos - npc.Center) * MathHelper.SmoothStep(0f, 1f, -5f+(easeIn * 10f));
			npc.velocity *= 0.950f;

			float percentSin = ((float)SpecialStateTimer) / timeMax;
			npc.rotation += (float)Math.Sin(MathHelper.Pi * percentSin) * 0.45f;

			npc.rotation = npc.rotation.AngleLerp((Vector2.UnitY).ToRotation() + MathHelper.PiOver4, MathHelper.Clamp(easeIn * 1.25f, 0f, 1f));

			if (SpecialStateTimer > timeMax)
			{
				ChangeState();
			}

		}

		public void TransformState()
		{
			float timeMax = 300f;
			npc.dontTakeDamage = true;
			npc.knockBackResist = 0f;

			float percentSin = ((float)SpecialStateTimer) / timeMax;

			if (Leader)
			{
				for (int i = 0; i < Main.musicFade.Length; i++)
					Main.musicFade[i] *= MathHelper.Clamp(1.15f - percentSin, 0f, 1f);


				if (SpecialStateTimer == 140)
				{
					specialStateVar = (P.Center - npc.Center);
					var bro = SpawnBrother(this, npc.Center, specialState, SpecialStateTimer, -1, (int)((npc.ai[2] + 1)) % 3);
					bro.npc.velocity = Vector2.Normalize(specialStateVar).RotatedBy(MathHelper.PiOver2) * 12f;
					bro.globalStateTimer = globalStateTimer;
				}

				if (SpecialStateTimer == 180)
				{
					var bro = SpawnBrother(this, npc.Center, specialState, SpecialStateTimer, 1, (int)((npc.ai[2] + 2)) % 3);
					bro.npc.velocity = Vector2.Normalize(specialStateVar).RotatedBy(-MathHelper.PiOver2) * 12f;
					bro.globalStateTimer = globalStateTimer;
				}
			}

			if (SpecialStateTimer > 150)
			{
				npc.spriteDirection = -1;
			}

			npc.velocity *= 0.90f;

			npc.rotation += (float)Math.Sin(MathHelper.Pi * percentSin) * 0.45f;

			swordFadeOut = MathHelper.Clamp((float)Math.Sin((MathHelper.Pi * percentSin)-(MathHelper.Pi * 0.0025f))*1.50f, 0f, 5f);

			afterGlow = 1f-MathHelper.Clamp((float)Math.Sin((MathHelper.Pi * percentSin)-(MathHelper.Pi * 0.0025f))*1.50f, 0f, 1f);

			if (SpecialStateTimer > 150)
				npc.rotation.AngleLerp((P.Center - npc.Center).ToRotation() + MathHelper.PiOver4, (SpecialStateTimer - (timeMax - 200f)) / timeMax);

			swordFadeOut *= 1f-MathHelper.Clamp((swordFadeOut/(float)(timeMax-30))/30f, 0f, 1f);

			if (SpecialStateTimer > timeMax)
			{
				ChangeState();
			}
		}

		public void LookAtPlayerState()
		{
			float timeMax = (float)currentAttack.time;

			float easeIn = MathHelper.Clamp(SpecialStateTimer / currentAttack.timerVariable, 0f, 1f);

			specialStateVar.X += easeIn;

			npc.rotation = npc.rotation.AngleLerp((P.Center - npc.Center).ToRotation() + MathHelper.PiOver4, MathHelper.Clamp(easeIn * 2f, 0f, 1f));

			specialStateVar.Y = npc.rotation - MathHelper.PiOver4;

			npc.velocity *= 0.925f;

			if (SpecialStateTimer > timeMax)
			{
				ChangeState(-1);
			}
		}

		public void CircleLeaderState()
		{
			float timeMax = (float)currentAttack.time;

			float easeIn = MathHelper.Clamp(SpecialStateTimer / currentAttack.timerVariable, 0f, 1f);
			float easeIn2 = MathHelper.Clamp((SpecialStateTimer / currentAttack.timerVariable) / 3f, 0f, 1f);

			specialStateVar.X = currentAttack.timerVariable2.X * 0.075f;

			if (easeIn2>=1f)
			nohit = -10;

			specialStateVar.Y = npc.rotation - MathHelper.PiOver4;

			Vector2 differ = npc.Center - leaderId.npc.Center;

			Vector2 toPosition = leaderId.npc.Center + (Vector2.UnitX * (240f * currentAttack.timerVariable2.Y)).RotatedBy(differ.ToRotation());
			Vector2 toPosition2 = leaderId.npc.Center + (Vector2.UnitX * (32f * currentAttack.timerVariable2.Y)).RotatedBy((leftOrRight * (MathHelper.Pi / 2f)) + (globalStateTimer* currentAttack.spinSpeed) / 2f);

			Vector2 toPos = Vector2.Lerp(toPosition, toPosition2, easeIn2);

			npc.Center += (toPos - npc.Center) * MathHelper.SmoothStep(0f, 1f, easeIn);
			npc.velocity *= 0.975f;

			npc.rotation = npc.rotation.AngleLerp((npc.Center- leaderId.npc.Center).ToRotation() + MathHelper.PiOver4, MathHelper.Clamp(easeIn2 * 1.25f, 0f, 1f));

			if (leaderId.specialState == (int)StateIds.LookAtPlayer)
			leaderId.npc.velocity *= 0.85f;

			if (SpecialStateTimer > timeMax)
			{
				ChangeState();
			}

		}

		public void CircleState()
		{
			float timeMax = (float)currentAttack.time;

			float easeIn = MathHelper.Clamp(SpecialStateTimer / currentAttack.timerVariable, 0f,1f);
			float easeIn2 = MathHelper.Clamp((SpecialStateTimer / currentAttack.timerVariable)/3f, 0f, 1f);

			specialStateVar.X = currentAttack.timerVariable2.X*0.025f;

			specialStateVar.Y = npc.rotation -MathHelper.PiOver4;

			Vector2 differ = npc.Center- P.Center;

			Vector2 toPosition = P.Center + (Vector2.UnitX * (640f * currentAttack.timerVariable2.Y)).RotatedBy(differ.ToRotation());
			Vector2 toPosition2 = P.Center + (Vector2.UnitX * (640f * currentAttack.timerVariable2.Y)).RotatedBy((leftOrRight*(MathHelper.Pi/1.414213562373f))+((globalStateTimer*currentAttack.spinSpeed)/ 30f));

			Vector2 toPos = Vector2.Lerp(toPosition,toPosition2, easeIn2);

			npc.Center += (toPos-npc.Center) * MathHelper.SmoothStep(0f,1f, easeIn);
			npc.velocity *= 0.975f;

			npc.rotation = npc.rotation.AngleLerp((P.Center - npc.Center).ToRotation() + MathHelper.PiOver4, MathHelper.Clamp(easeIn2 * 1.25f, 0f, 1f));

			if (SpecialStateTimer > timeMax)
			{
				(P.Center - npc.Center).ToRotation();
				specialStateVar.Y = npc.rotation - MathHelper.PiOver4;
				ChangeState();
			}
		}

		public void HoverState()
		{
			float timeMax = (float)currentAttack.time;

			float easeIn = MathHelper.Clamp(SpecialStateTimer / currentAttack.timerVariable, 0f, 1f);
			float easeIn2 = MathHelper.Clamp((SpecialStateTimer / currentAttack.timerVariable) / 3f, 0f, 1f);

			specialStateVar.X = currentAttack.timerVariable2.X * 0.025f;

			specialStateVar.Y = npc.rotation - MathHelper.PiOver4;

			//Vector2 toPosition = P.Center + (Vector2.UnitX.RotatedBy(((MathHelper.TwoPi * (2f / 3f)) * (float)leftOrRight) * (640f* currentAttack.timerVariable2.Y))).RotatedBy(specialStateVar.X* easeIn);

			float addrotter = (specialStateVar.X * easeIn);

			Vector2 toPos = currentAttack.hoverArea;

			npc.Center += (toPos - npc.Center) * MathHelper.SmoothStep(0f, 1f, easeIn);
			npc.velocity *= 0.975f;

			npc.rotation = npc.rotation.AngleLerp(currentAttack.lookAt + MathHelper.PiOver4, MathHelper.Clamp(easeIn2 * 1.25f, 0f, 1f));

			if (SpecialStateTimer > timeMax)
			{
				(P.Center - npc.Center).ToRotation();
				specialStateVar.Y = npc.rotation - MathHelper.PiOver4;
				ChangeState();
			}

		}

		public void DashAtAnimedAngleState()
		{
			float timeMax = (float)currentAttack.time;

			float easeIn = MathHelper.Clamp(SpecialStateTimer / currentAttack.timerVariable, 0f, 1f);

			nohit = -6;

			if (SpecialStateTimer == 8)
            {
				var sound = Main.PlaySound(SoundID.DD2_JavelinThrowersAttack, (int)npc.Center.X, (int)npc.Center.Y);
				if (sound != null)
				{
					sound.Pitch = -0.5f;
				}
			}

			if (SpecialStateTimer > 10 && SpecialStateTimer < timeMax-6)
            {
				trailAlpha = MathHelper.Clamp(trailAlpha + 0.22f, 0f, 1.25f);
			}

			npc.velocity *= 0.96f;

			npc.velocity += specialStateVar.Y.ToRotationVector2() * easeIn * (currentAttack.timerVariable2.X*2f);

			if (SpecialStateTimer > timeMax)
			{
				ChangeState();
			}
		}

		public void ChangeState(int idealState = -1)
        {
			if (specialStateVar == default)
			specialStateVar = Vector2.UnitX;

			SpecialStateTimer = 0;
			if (idealState == -1)
            {
				if (OrderedAttacks.Count < 1)
                {
					AddAttackOrder();

					if (Leader)
					{
						List<CaliburnGuardianHardmode> us = new List<CaliburnGuardianHardmode>(brothers);
						us.Add(this);

						foreach (CaliburnGuardianHardmode sworder in us)
						{
							//sworder.ChangeState((int)StateIds.PhaseAdvance);
							sworder.AddAttackOrder();
							sworder.ChangeState();
						}
						return;
					}
				}

				SwordAttack attack = OrderedAttacks[0];
				specialState = attack.type;
				specialStateMaxTime = attack.time;
				currentAttack = (SwordAttack)attack.Clone();
				OrderedAttacks.RemoveAt(0);

				return;
            }
			specialState = idealState;

		}

		public void StateMachine()
        {
			if (currentAttack != null)
            {
				currentAttack.boss = this;
				currentAttack.Update();
            }

			if (npc.life < (int)(npc.lifeMax * 0.40f) && phase == 1 && Leader)//Advance to Phase 3
			{
				currentAttack = null;
				List<CaliburnGuardianHardmode> us = new List<CaliburnGuardianHardmode>(brothers);
				us.Add(this);

				foreach (CaliburnGuardianHardmode sworder in us)
				{
					sworder.phase = 2;
					sworder.ChangeState((int)StateIds.PhaseAdvance);
					sworder.AddAttackOrder();
				}
			}

			if (npc.life < (int)(npc.lifeMax * 0.80f) && phase==0 && Leader)//Advance to Phase 2
            {
				currentAttack = null;
				List<CaliburnGuardianHardmode> us = new List<CaliburnGuardianHardmode>(brothers);
				us.Add(this);

				foreach (CaliburnGuardianHardmode sworder in us)
				{
					sworder.phase = 1;
					sworder.ChangeState((int)StateIds.PhaseAdvance);
					sworder.AddAttackOrder();
				}
			}

			if (specialState == (int)StateIds.Transform)
				TransformState();
			if (specialState == (int)StateIds.PhaseAdvance)
				PhaseAdvanceState();

			if (specialState == (int)StateIds.Circle)
				CircleState();
			if (specialState == (int)StateIds.DashAtAnimedAngle)
				DashAtAnimedAngleState();
			if (specialState == (int)StateIds.LookAtPlayer)
				LookAtPlayerState();
			if (specialState == (int)StateIds.CircleLeader)
				CircleLeaderState();
			if (specialState == (int)StateIds.Hover)
				HoverState();

		}

		public override void AI()
		{
			trailAlpha = MathHelper.Clamp(trailAlpha - 0.10f, 0f, 2f);

			if (npc.ai[3] < 10 && Leader)
			{
				npc.ai[2] = Main.rand.Next(0, 3);
				npc.ai[3] = 10;
				npc.netUpdate = true;
			}
			if (specialState != 0)
            {
				globalStateTimer += 1;
				_specialStateTimer += 1;
				npc.dontTakeDamage = (!P.GetModPlayer<SGAPlayer>().DankShrineZone && !debugMode);

				StateMachine();

				return;
            }

			base.AI();
		}


		protected override void ExtraDraw(SpriteBatch spriteBatch, Texture2D swordtex, int layer)
		{
			Vector2 drawPos = npc.Center;

			if (layer == -1)
            {
				if (swordFadeOut > 0)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					Texture2D glow = ModContent.GetTexture("SGAmod/Glow");

					for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 8f)
					{
						Vector2 dist2 = Vector2.UnitX.RotatedBy(f + npc.rotation / 8f) * MathHelper.Clamp(64f - (globalStateTimer / 2f), 6f, 640f);
						spriteBatch.Draw(glow, (drawPos + dist2) - Main.screenPosition, null, Color.White* MathHelper.Clamp(swordFadeOut*1.25f, 0f, 1f)*1f, npc.rotation+MathHelper.PiOver4, glow.Size() / 2f, new Vector2(0.5f,1f)*(1.5f+(npc.scale*(globalStateTimer/128f)*0.75f)), npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				}

			}

			if (layer == 0)
			{
				Color swordColor = Main.hslToRgb(npc.ai[2] / 3f, 0.35f, 0.75f);
				if (trailAlpha >= 0)
				{
					TrailHelper trail = new TrailHelper("FadedBasicEffectPass", SGAmod.Instance.GetTexture("SmallLaser"));
					trail.coordMultiplier = new Vector2(1f, 1f);// projectile.velocity.Length());
					trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
					trail.trailThickness = 12;
					trail.strengthPow = 2f;
					trail.strength = trailAlpha * 2.5f;
					trail.doFade = true;
					trail.trailThicknessIncrease = 4;

					trail.color = delegate (float percent)
					{
						Color traillengthcol = Color.Lerp(Color.White, Color.CadetBlue, percent);
						return Color.Lerp(swordColor, Color.White, percent);
					};

					/*trail.trailThicknessFunction = delegate (float percent)
					{
						return 4f + (float)Math.Sin(percent + MathHelper.PiOver4) * 3f;
					};*/

					trail.DrawTrail(oldPos.Select(testby => new Vector3(testby.X, testby.Y, 0)).ToList(), npc.Center);

				}

					float alpha = MathHelper.Clamp((globalStateTimer - 260f) / 60f, 0f, 1f);
					if (alpha > 0)
					{
						float dist = (1f + (float)Math.Sin((Main.GlobalTime) * 0.25f))*4f;
						for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
						{
							Vector2 dist2 = Vector2.UnitX.RotatedBy(f + (npc.rotation / 8f)+ (Main.GlobalTime*4f)) * dist;
							//float ftta = (0.05f + (float)Math.Sin((Main.GlobalTime + f) * MathHelper.Pi) * 0.025f);
							spriteBatch.Draw(swordtex, (drawPos + dist2) - Main.screenPosition, null, swordColor * appear * alpha * 0.045f, npc.rotation, swordtex.Size() / 2f, npc.scale, npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
						}
					}
				
			}


			if (layer == 1)
			{

				if (swordFadeOut > 0)
				{
					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

					Effect fadeIn = SGAmod.FadeInEffect;

					fadeIn.Parameters["alpha"].SetValue(MathHelper.Clamp(swordFadeOut/3f,0f,1f));
					fadeIn.Parameters["strength"].SetValue(MathHelper.Clamp(swordFadeOut/1.5f, 0f, 1f));
					fadeIn.Parameters["fadeColor"].SetValue(Color.White.ToVector3());
					fadeIn.Parameters["blendColor"].SetValue(Color.White.ToVector3());

					fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

					for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
					{
						Vector2 dist2 = Vector2.UnitX.RotatedBy(f + npc.rotation / 8f) * MathHelper.Clamp(64f - (globalStateTimer / 2f), 6f, 640f);
						spriteBatch.Draw(swordtex, (drawPos+ dist2) - Main.screenPosition, null, Color.White, npc.rotation, swordtex.Size() / 2f, npc.scale, npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				}
			}
		}

		public override void NPCLoot()
		{
			int[] types = { mod.ItemType("CaliburnTypeA"), mod.ItemType("CaliburnTypeB"), mod.ItemType("CaliburnTypeC") };
			npc.DropItemInstanced(npc.Center, new Vector2(npc.width, npc.height), types[(int)npc.ai[2]]);

			if (npc.SGANPCs().OnlyOnce() && Main.projectile.Where(testby => testby.type == ModContent.ProjectileType<SpookyDarkSectorEye>()).Count()<1)
			{
				SGAWorld.downedCaliburnGuardianHardmode = true;

				if (!SGAWorld.darknessVision && npc.life<1)
				SpookyDarkSectorEye.Release(npc.Center, true, new Vector2(20, 20));
			}

		}
		protected override int caliburnlevel => 3;

	}




	[AutoloadBossHead]
	public class CaliburnGuardian : ModNPC, ISGABoss
	{
		public string Trophy() => "Caliburn"+ names[(int)npc.ai[2]] +"Trophy";
		public bool Chance() => true;
		public string RelicName() => "Caliburn";
		public void NoHitDrops() { }

		protected string[] names = {"A","B","C"};
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spirit of Caliburn");
			Main.npcFrameCount[npc.type] = 1;
		}

		protected float[] oldRot = new float[12];
		protected Vector2[] oldPos = new Vector2[12];
		public float appear = 0f;
		public float afterGlow = 1f;
		public int nohit;
		public static bool debugMode => true;
		protected virtual int caliburnlevel => SGAWorld.downedCaliburnGuardians;

		protected virtual void ExtraDraw(SpriteBatch spriteBatch, Texture2D swordtex,int layer)
        {

		}

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

			ExtraDraw(spriteBatch,tex,-1);

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{


				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				float alphaz2 = Math.Max((0.5f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f,0f);
				float fadein = MathHelper.Clamp(nohit+10f/60, 0f, 0.2f);
				for (float xx = 0; xx < 1f; xx += 0.20f)
				{
					if (xx >= 0.80f)
					{
						ExtraDraw(spriteBatch, tex, 0);
					}
					Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + (npc.velocity * xx);
					spriteBatch.Draw(tex, drawPos, null, ((Color.Lerp(lightColor,Color.White, alphaz2) * alphaz) * (appear)) * 0.2f* afterGlow, oldRot[k], drawOrigin, npc.scale, npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}
			}

			ExtraDraw(spriteBatch, tex, 1);

			return false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeA"; }
		}

		public override string BossHeadTexture
		{
			get { return ("SGAmod/Items/Weapons/Caliburn/CaliburnType"+(new string[3] { "A", "B", "C"})[(int)npc.ai[2]]); }
		}

		//public override string BossHeadTexture => "Terraria/UI/PVP_2";
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
			npc.netAlways = true;

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
				npc.damage = 35;
				npc.defDamage = 35;
				npc.defense = 10;
			}
			if (caliburnlevel > 2)
			{
				npc.ai[3] = 2;
				npc.lifeMax = 7500;
				npc.value = 100000f;
				npc.damage = 75;
				npc.defDamage = 75;
				npc.defense = 30;
			}
		}
		public override void NPCLoot()
		{
			if (npc.SGANPCs().OnlyOnce())
			{
				if (SGAWorld.downedMurk < 2 && SGAWorld.downedCaliburnGuardians == 2)
					Idglib.Chat("The Moist Stone around Dank Shrines has weakened and can be broken.", 75, 225, 75);

				SGAWorld.downedCaliburnGuardians = Math.Min(3, SGAWorld.downedCaliburnGuardians + 1);

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

				if (Main.netMode == NetmodeID.SinglePlayer)
					SGAWorld.downedCaliburnGuardiansPoints += 1;

				Achivements.SGAAchivements.UnlockAchivement("Caliburn", Main.LocalPlayer);
			}
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

        public override bool PreAI()
        {
			appear = Math.Max(appear - 0.03f, Math.Min(appear + 0.025f, 0.50f));

			P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
					npc.active = false;
				}
			}


			nohit += 1;
			trailingeffect();
			return true;
        }

		public Player P;

		public override void AI()
		{
			npc.localAI[0] -= 1;

			npc.knockBackResist = 0.85f;
			if (caliburnlevel>2)
			npc.knockBackResist = 0f;

			P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{

			}
			else
			{
				npc.dontTakeDamage = false;
				if (!P.GetModPlayer<SGAPlayer>().DankShrineZone && !debugMode)
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
								Idglib.Shattershots(npc.Center, npc.Center - npc.velocity * 26f, new Vector2(P.width, P.height), ProjectileID.CrystalShard, 10, 5f + caliburnlevel * 1.5f, caliburnlevel * 60, 8 + caliburnlevel * 8, true, 0, false, 200);
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

							if (npc.ai[0] % 90 - (caliburnlevel * 10) == 0)
								Idglib.Shattershots(npc.Center, P.Center + new Vector2((P.Center.X - npc.Center.X) > 0 ? 200 : -200, 0), new Vector2(P.width, P.height), ProjectileID.DiamondBolt, 15, 5, caliburnlevel * 5, 1 + caliburnlevel, true, 0, false, 200);


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

	public class CalburnSwordAttack : Hellion.HellionXemnasAttack
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Holy Swords");
		}
	}


}

