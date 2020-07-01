#if TrueHellionUpdate
using System.Linq;
using System;
using System.IO;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.NPCs.TrueDraken
{
	[AutoloadHead]
	public class TrueDraken : ModNPC
	{

		private float[] oldRot = new float[8];
		private Vector2[] oldPos = new Vector2[8];
		float appear = 0.25f;

		int introstate = 0;
		int intro = 800;
		int StopMoving = 0;
		int NoFriction = 0;
		int Ramming = 0;
		float empowered = 0f;

		float stealth = 1f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("TRUE Draken");
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.650f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override void SetDefaults()
		{
			npc.boss = true;
			npc.friendly = false;
			npc.width = 72;
			npc.height = 72;
			npc.aiStyle = -1;
			npc.damage = 250;
			npc.noTileCollide = true;
			npc.defense = 0;
			npc.netAlways = true;
			npc.noGravity = true;
			npc.lifeMax = 3000000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
		}

		public void GetHit(Player player, ref int damage, ref float knockback, ref bool crit)
		{
			int ogdamage = damage;
			if (npc.HasBuff(BuffID.Ichor))
				damage += (int)(ogdamage * 0.15);
			if (npc.HasBuff(BuffID.CursedInferno))
				damage += (int)(ogdamage * 0.15);
			if (npc.HasBuff(BuffID.BetsysCurse))
				damage += (int)(ogdamage * 0.20);
			for (int i = 0; i < npc.buffTime.Length; i += 1)
			{
				if (npc.buffType[i] > 0)
				{
					damage += (int)(ogdamage * 0.05);
				}
			}
		}

		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Main.player[projectile.owner].active)
				GetHit(Main.player[projectile.owner], ref damage, ref knockback, ref crit);
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			GetHit(player, ref damage, ref knockback, ref crit);
		}

		public override string Texture
		{
			get
			{
				return "SGAmod/NPCs/TownNPCs/DrakenFly";
			}
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return introstate > 1 && Ramming > 0;
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			intro = reader.ReadInt32();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(intro);
		}

		public virtual void trailingeffect()
		{
			if (appear < 0.25f)
				appear += 0.005f;
			Rectangle hitbox = new Rectangle((int)npc.position.X - 8, (int)npc.position.Y - 8, npc.height + 16, npc.width + 16);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

				if (Main.rand.Next(0, 10) == 1)
				{
					int typr = mod.DustType("TornadoDust");

					int dust = Dust.NewDust(new Vector2(oldPos[k].X, oldPos[k].Y), hitbox.Width, hitbox.Height, typr);
					Main.dust[dust].scale = (0.75f * appear) + (npc.velocity.Length() / 50f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 normvel = npc.velocity;
					normvel.Normalize(); normvel *= 2f;

					Main.dust[dust].velocity = ((randomcircle / 1f) + (-normvel)) - npc.velocity;
					Main.dust[dust].noGravity = true;

				}

			}

			oldRot[0] = npc.rotation;
			oldPos[0] = npc.Center;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			Texture2D tex = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/DrakenFly");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((npc.Center - Main.screenPosition));
			Vector2 adder = Vector2.Zero;
			Color color = drawColor;
			int timing = (int)(Main.GlobalTime * (8f));
			if (introstate < 1)
				timing = (int)(Main.GlobalTime * 8f);

			timing %= 4;

			int mydirection = npc.rotation.ToRotationVector2().X > 0 ? 1 : -1;

			if (timing == 0)
			{
				adder = ((npc.rotation + (float)Math.PI / 2f).ToRotationVector2() * (8f * mydirection));
			}


			timing *= ((tex.Height) / 4);
			if (introstate < 1)
			{
				spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), color, 0, drawOrigin, npc.scale, (npc.Center - Main.LocalPlayer.Center).X < 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}
			else
			{
				if (empowered > 0f)
				{
					for (int i = -1; i < 2; i += 2)
					{
						Texture2D texture7 = ModContent.GetTexture("Terraria/Extra_" + 34);
						spriteBatch.Draw(texture7, npc.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime) % 1f, 1f, 0.75f) * 0.50f* empowered*stealth, -Main.GlobalTime * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTime / 1.1694794f)), 1f)* empowered, SpriteEffects.None, 0f);
						texture7 = ModContent.GetTexture("Terraria/Projectile_490");
						spriteBatch.Draw(texture7, npc.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime) % 1f, 1f, 0.75f) * 0.50f* empowered * stealth, Main.GlobalTime * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTime / 1.1694794f)), 1f)* empowered, SpriteEffects.None, 0f);
					}
				}

				for (int k = oldRot.Length - 1; k >= 0; k -= 1)
				{


					//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
					float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
					float alphaz2 = Math.Max((0.75f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f, 0f);
					for (float xx = 0; xx < 1f; xx += 0.05f)
					{
						float scaleffect = 2f - ((k + xx) / oldRot.Length);
						drawPos = ((oldPos[k] - Main.screenPosition)) + (npc.velocity * xx);
						spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), ((Color.Lerp(drawColor, Main.hslToRgb(((k / oldRot.Length) + Main.GlobalTime) % 1f, 1f, 0.75f), alphaz2) * alphaz) * (appear)) * 0.25f, npc.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, scaleffect, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
					}
				}

				drawPos = ((npc.Center - Main.screenPosition));
				spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), Color.White * stealth, npc.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, npc.scale, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				Texture2D texture6 = ModContent.GetTexture("Terraria/Projectile_" + 540);
				spriteBatch.Draw(texture6, npc.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime) % 1f, 1f, 0.75f)*0.50f* empowered * stealth, Main.GlobalTime*37.134f, new Vector2(texture6.Width / 2f, texture6.Height / 2f), new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTime*1.694794f)), 3f), SpriteEffects.None, 0f);



			}


			return false;
		}

		public override bool CheckActive()
		{
			return (!Main.player[npc.target].active || Main.player[npc.target].dead);
		}

		public override void AI()
		{
			intro += 1;
			npc.dontTakeDamage = false;
			npc.chaseable = true;
			stealth = Math.Min(1f, stealth+0.01f);
			if (introstate > 0)
				trailingeffect();
			if (DoingIntro())
			{
				npc.dontTakeDamage = true;
				return;

			}

			if (!LivePlayer())
			{
				npc.velocity.X *= 0.75f;
				npc.velocity.Y -= 0.10f;
				return;
			}

			npc.ai[0] += 1;

			if (npc.ai[1] < 1)
				npc.ai[0] = npc.ai[0] % 1000;
			if (npc.ai[1]>1 && (intro>1020) && empowered<1f)
				empowered+=0.01f;

				Player P = Main.player[npc.target];
			Vector2 PWhere = P.Center;
			Vector2 DrakenWhere = npc.Center;
			Vector2 FlyTo = PWhere - new Vector2(0, 200);
			Vector2 FlySpeed = new Vector2(0.10f, 0.10f);
			Vector2 FlyFriction = new Vector2(0.95f, 0.95f);
			StopMoving -= 1; NoFriction -= 1; Ramming -= 1;

			if (npc.life < npc.lifeMax * 0.90 && npc.ai[1] == 0)
			{
				npc.ai[1] = 1;
				intro = 960;
				npc.netUpdate = true;
			}
			if (npc.life < npc.lifeMax * 0.50 && npc.ai[1] == 1)
			{
				npc.ai[1] = 2;
				npc.life = npc.lifeMax;
				intro = 960;
				npc.netUpdate = true;
			}
			Vector2 Turntof = PWhere - DrakenWhere;
			Turntof.Normalize();


			if (npc.ai[1] == 2 && npc.ai[0] < 1)
			{
				Phase2Touhou(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction,true);
				goto Movementstuff;

			}

			if (npc.ai[0] % 2800 > 2200)
			{
				LaserBarrage(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
			}
			else
			{
				if ((npc.ai[0] % 400 > 350 || npc.ai[0] % 710 > 650) && npc.ai[1] > 0)
				{
					SpinCycle(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
				}
				else
				{
					if (npc.ai[0] % 1000 > 400)
					{
						Phase1Hover(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
					}
					else
					{
						Phase1Dash(P, ref PWhere, ref DrakenWhere, ref FlyTo, ref FlySpeed, ref FlyFriction);
					}
				}

			}



			Movementstuff:
			if (StopMoving < 1)
			{
				Vector2 Thereis = (FlyTo - DrakenWhere);
				if (FlySpeed.X < 0)
				{
					Thereis.Normalize();
					npc.velocity += Thereis * -FlySpeed;
				}
				else
				{
					npc.velocity += (Thereis) * FlySpeed;
				}

			}
			if (NoFriction < 1)
				npc.velocity *= FlyFriction;


		}

		public void Phase2Touhou(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction, bool introattack = false)
		{

			//Touhou Attack
			npc.chaseable = false;
			Vector2 Turnto = PWhere - DrakenWhere;
			appear /= 1.15f;
			stealth = stealth/1.15f;
			if (introattack)
				npc.dontTakeDamage = true;
			if ((npc.ai[0]) % 100 == 0)
			{
				for (int i = 0; i < 360; i += 360 / 3)
				{
					int prog = Projectile.NewProjectile(introattack ? PWhere : npc.Center, ((MathHelper.ToRadians(i+ npc.ai[0]*1.3734f)).ToRotationVector2() * 12f), mod.ProjectileType("TrueDrakenCopyNoDeath"), 0, 1f, 255, 0, npc.whoAmI);
					Main.projectile[prog].ai[1] = npc.whoAmI;

					Func<Vector2, TrueDrakenCopy, int, bool> ProjectileAct = delegate (Vector2 playerpos, TrueDrakenCopy DrakenCopy, int timer)
							{
								Vector2 rotspeed = new Vector2(npc.ai[0] % 200 >= 100 ? 0.25f : -0.25f, 0.75f);
								DrakenCopy.projectile.ai[0] += 1;
								if (DrakenCopy.projectile.timeLeft > 300)
									DrakenCopy.projectile.timeLeft = 300;

								float arot = DrakenCopy.projectile.velocity.ToRotation();
								float ogvel = DrakenCopy.projectile.velocity.Length();
								if (DrakenCopy.projectile.ai[0] < 2)
								{
									DrakenCopy.alocation = DrakenCopy.projectile.Center;
									DrakenCopy.projectile.rotation = arot;
								}

								Vector2 ShootThere = DrakenCopy.alocation - DrakenCopy.projectile.Center;
								ShootThere.Normalize();

								float len = DrakenCopy.projectile.velocity.Length();

								DrakenCopy.projectile.rotation += MathHelper.ToRadians(rotspeed.X* rotspeed.Y);
								Vector2 Gowhere = DrakenCopy.projectile.rotation.ToRotationVector2();
								if (ShootThere.Length() > 0)
									DrakenCopy.projectile.Center -= ShootThere * 6f;
								DrakenCopy.projectile.velocity = Gowhere * ogvel;

								if (DrakenCopy.projectile.ai[0] % 30 == 0 && (DrakenCopy.projectile.ai[0] > 100))
								{
									int prog2 = Projectile.NewProjectile(DrakenCopy.projectile.Center, ShootThere * 15f, mod.ProjectileType("TrueDrakenCopyNoDeath"), 80, 1f, 255, 150, npc.whoAmI);
									Main.projectile[prog2].ai[0] = 200;
									Main.projectile[prog2].ai[1] = npc.whoAmI;
									Main.projectile[prog2].rotation = -ShootThere.ToRotation();
									Main.projectile[prog2].timeLeft = 900;
									Main.projectile[prog2].netUpdate = true;
									Main.PlaySound(SoundID.NPCHit, (int)npc.Center.X, (int)npc.Center.Y, 37, 0.75f, 0.5f);
								}


								return true;
							};


					(Main.projectile[prog].modProjectile as TrueDrakenCopy).ProjectileAct = ProjectileAct;
					Main.projectile[prog].netUpdate = true;

				}

				Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 84, 0.75f, 0.5f);
			}

			//if (npc.ai[0] % 90 > 20)
				npc.rotation = npc.rotation.AngleLerp((Turnto).ToRotation(), 0.04f);

			if (!introattack)
			{
				FlyTo = PWhere - new Vector2(256, 0).RotatedBy(MathHelper.ToRadians(npc.ai[0] * 7f));
				FlySpeed /= 2f;
			}
			else
			{
				FlySpeed /= 3.5f;
			}
		}

		public void Phase1Dash(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{

			//Charging
			Vector2 Turnto = PWhere - DrakenWhere;
			if (npc.ai[0] % 80 == 30)
			{
				Turnto = PredictiveAim(100f, npc.Center, false) - DrakenWhere;
				Turnto.Normalize();
				npc.velocity = Turnto * 100f;
				//npc.velocity += Turnto * 70f;
				npc.rotation = Turnto.ToRotation();
				npc.spriteDirection = (npc.velocity.X) > 0 ? 1 : -1;
				Ramming = 30;
			}
			if (npc.ai[0] % 80 == 0)
			{
				Turnto = Turnto.RotatedBy(MathHelper.ToRadians((Main.rand.Next(30, 60) * (Main.rand.NextBool() ? 1 : 1))));
				Turnto.Normalize();
				npc.velocity = Turnto * 80f;
				npc.velocity += Turnto * 30f;
				npc.rotation = Turnto.ToRotation();
				npc.spriteDirection = (npc.velocity.X) > 0 ? 1 : -1;
				Ramming = 32;
			}
			if (npc.ai[0] % 90 > 20)
				npc.rotation = npc.rotation.AngleLerp((Turnto).ToRotation(), 0.15f);
			StopMoving = 80;

		}

		public void SpinCycle(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{
			//Draken Clone Spin Cycle
			Vector2 Turnto = PWhere - DrakenWhere;
			npc.rotation = npc.rotation.AngleLerp((Turnto).ToRotation(), 0.025f);
			Vector2 shootthere = PredictiveAim(30f, npc.Center, false);
			npc.Center = npc.Center.RotatedBy(MathHelper.ToRadians(npc.ai[0] % 800 > 400 ? 5f : -5f), PWhere);
			shootthere -= DrakenWhere;
			shootthere.Normalize();
			if (StopMoving < 3)
				StopMoving = 3;
			if (npc.ai[0] % 3 == 0 && (npc.ai[0] % 400 > 375 || npc.ai[0] % 710 > 675))
			{
				int prog = Projectile.NewProjectile(npc.Center, shootthere * 15f, mod.ProjectileType("TrueDrakenCopy"), 80, 1f, 255, 150, npc.whoAmI);
				Main.projectile[prog].ai[0] = 150;
				Main.projectile[prog].ai[1] = npc.whoAmI;
				Main.projectile[prog].netUpdate = true;
				Main.PlaySound(SoundID.NPCHit, (int)npc.Center.X, (int)npc.Center.Y, 37, 0.75f, 0.5f);
			}

		}

		public void Phase1Hover(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{

			//Hovering

			FlyFriction = new Vector2(0.95f, 0.95f);
			FlyTo = PWhere - new Vector2((float)Math.Sin(npc.ai[0] / 73f) * 400f, 250);
			Vector2 Turnto = PWhere - DrakenWhere;
			Turnto.X *= 3f;
			npc.rotation = npc.rotation.AngleLerp((Turnto).ToRotation(), 0.025f);
			npc.spriteDirection = (FlyTo.X - DrakenWhere.X) > 0 ? 1 : -1;
			FlySpeed = new Vector2(-8.20f, -8.20f) * Math.Min((FlyTo - DrakenWhere).Length() / 400f, 1f);

			Vector2 shootthere = PWhere - DrakenWhere;
			if (npc.ai[0] % 10 == 0 && npc.ai[0] % 100 > 60)
			{
				shootthere.Normalize();
				int prog = Projectile.NewProjectile(npc.Center, shootthere * 30f, ProjectileID.CursedFlameHostile, 80, 1f);
			}
			if (npc.ai[0] % 5 == 0 && npc.ai[0] % 100 > 60 && npc.ai[1] == 0)
				Idglib.Shattershots(npc.Center, PWhere, new Vector2(0, 0), ProjectileID.CursedFlameHostile, 80, 50, 100, 3, false, 0, false, 100);

		}

		public void LaserBarrage(Player P, ref Vector2 PWhere, ref Vector2 DrakenWhere, ref Vector2 FlyTo, ref Vector2 FlySpeed, ref Vector2 FlyFriction)
		{
			//Laser Barrage
			StopMoving = 160;
			if (npc.ai[0] % 2800 < 2300)
			{
				Vector2 Turnto = PWhere - DrakenWhere;
				Turnto.Normalize();
				npc.velocity -= Turnto * 1f;
				npc.rotation = npc.rotation.AngleLerp(Turnto.ToRotation(), 0.15f);
			}
			else
			{
				Vector2 Turnto = PWhere - DrakenWhere;
				for (int i = -12; i < 5; i += 4)
				{
					if (npc.ai[0] % 3 == 0 && npc.ai[0] % 150 <= 25)
					{
						Vector2 Turnto2 = (PWhere - DrakenWhere);
						Turnto2 = Turnto2.RotatedBy(MathHelper.ToRadians(i));
						Turnto2.Normalize();

						int prog = Projectile.NewProjectile(npc.Center, Turnto2 * 8f, SGAmod.Instance.ProjectileType("HellionBeam"), 100, 15f);


					}
				}
				npc.Center = npc.Center.RotatedBy(MathHelper.ToRadians(4), PWhere);
				Turnto.Normalize();
				npc.velocity /= 1.15f;
				npc.rotation = Turnto.ToRotation();
			}
		}
			public bool DoingIntro()
		{
			introstate = 0;
			if (intro < 1200)
			{
				if (intro>=950)
					introstate = 1;
				if (intro < 200)
				{
					npc.velocity.Y -= 0.5f;
				}
				if (intro == 100)
					Idglib.Chat("Draken: ...", 0, 200, 0);
				if (intro == 200)
					Idglib.Chat("Draken: I...", 0, 200, 0);
				if (intro == 400)
					Idglib.Chat("Draken: Trusted you...", 0, 200, 0);
				if (intro == 600)
					Idglib.Chat("Draken: How could...", 0, 200, 0);
				if (intro == 700)
					Idglib.Chat("Draken: You do this?", 0, 200, 0);
				if (intro == 800)
					Idglib.Chat("Draken: how Could...!", 0, 200, 0);
				if (intro == 950)
				{
					Idglib.Chat("True Draken: YOU?!!", 200, 0, 0);
					RippleBoom.MakeShockwave(npc.Center, 8f, 2f, 20f, 100, 3f, true);
					Main.PlaySound(15, (int)npc.Center.X, (int)npc.Center.Y, 2, 1f, -0.5f);
					npc.velocity.Y = 0;
				}
				if (npc.ai[1] < 1)
				{
					if (intro == 1100)
						Idglib.Chat("True Draken: Justice shall fall down on you!", 200, 0, 0);
					if (intro == 1199)
						Idglib.Chat("True Draken: Repent!", 200, 0, 0);
				}

				if (npc.ai[1] == 1)
				{
					npc.ai[0] = 2200;
					if (intro == 1000)
						Idglib.Chat("True Draken: What do you hope to gain?", 200, 0, 0);
					if (intro == 1150)
						Idglib.Chat("True Draken: Tell me what?!", 200, 0, 0);
				}
				if (npc.ai[1] == 2)
				{
					npc.ai[0] = -999;
					if (intro == 1000)
						Idglib.Chat("True Draken: I won't let you...", 200, 0, 0);
					if (intro == 1150)
						Idglib.Chat("True Draken: I WON'T LET YOU!!", 200, 0, 0);
				}

				if (intro > 700 && intro < 950)
				{
					npc.velocity.Y -= (intro - 700f) / 1000f;
				}
				npc.velocity /= 1.5f;
				return true;
			}
			introstate = 2;
			return false;


		}

		//From Joost, get perms yeah
		private Vector2 PredictiveAim(float speed, Vector2 origin, bool ignoreY)
		{
			Player P = Main.player[npc.target];
			Vector2 vel = (ignoreY ? new Vector2(P.velocity.X, 0) : P.velocity);
			Vector2 predictedPos = P.MountedCenter + P.velocity + (vel * (Vector2.Distance(P.MountedCenter, origin) / speed));
			predictedPos = P.MountedCenter + P.velocity + (vel * (Vector2.Distance(predictedPos, origin) / speed));
			predictedPos = P.MountedCenter + P.velocity + (vel * (Vector2.Distance(predictedPos, origin) / speed));
			return predictedPos;
		}

		public bool LivePlayer()
		{

			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
					return false;
				}
			}
			return true;

		}


	}

		public class TrueDrakenCopy : ModProjectile
	{

		public Vector2 alocation;

		public Func<Vector2, TrueDrakenCopy, int, bool> ProjectileAct = delegate (Vector2 playerpos, TrueDrakenCopy DrakenCopy, int timer)
		{

			float len = DrakenCopy.projectile.velocity.Length();
			Vector2 Gowhere = playerpos - DrakenCopy.projectile.Center;
			if (timer == DrakenCopy.projectile.ai[0])
			{
				Gowhere.Normalize();
				DrakenCopy.projectile.velocity = DrakenCopy.projectile.rotation.ToRotationVector2() * len;

			}
			if (timer < DrakenCopy.projectile.ai[0])
			{
				DrakenCopy.projectile.rotation = DrakenCopy.projectile.rotation.AngleLerp(Gowhere.ToRotation(), 0.10f);
				DrakenCopy.projectile.position -= DrakenCopy.projectile.velocity;
			}

			return true;
		};

		public Func<TrueDrakenCopy, int, bool> ProjectileDie = delegate (TrueDrakenCopy DrakenCopy, int timer)
		{
			Main.PlaySound(SoundID.Item45, DrakenCopy.projectile.Center);
			for (int i = 0; i < 360; i+=360/6)
			{
				Vector2 perturbedSpeed = new Vector2(DrakenCopy.projectile.velocity.X, DrakenCopy.projectile.velocity.Y).RotatedBy(MathHelper.ToRadians(i));
				int hoste=Projectile.NewProjectile(DrakenCopy.projectile.Center.X, DrakenCopy.projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.SwordBeam, 40, DrakenCopy.projectile.knockBack, DrakenCopy.projectile.owner, 0f, 0f);
				Main.projectile[hoste].hostile = true;
				Main.projectile[hoste].friendly = false;
				Main.projectile[hoste].netUpdate = true;

			}
			return true;
		};

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.friendly = reader.ReadBoolean();
			projectile.hostile = reader.ReadBoolean();
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.friendly);
			writer.Write(projectile.hostile);
		}

		int timer = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken's Pain");
		}

		private float[] oldRot = new float[12];
		private Vector2[] oldPos = new Vector2[12];
		float appear = 0.5f;

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.timeLeft = 400;
			projectile.width = 64;
			projectile.height = 64;
			projectile.aiStyle = -99;
			projectile.extraUpdates = 2;
			projectile.tileCollide = false;
		}
		public override bool PreKill(int timeLeft)
		{
			ProjectileDie(this, timeLeft);
			return true;
		}

		public override void AI()
		{
			appear = Math.Max(Math.Min(0.75f, (float)timer / 200f), (float)projectile.timeLeft / 400f);
			trailingeffect();
			NPC master = Main.npc[(int)projectile.ai[1]];

				Player player = Main.player[master.target];

			timer += 1;

			if (ProjectileAct(player.Center, this, timer))
			{

				int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("TornadoDust"), projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 20, Color.Lime * 0.25f, 0.15f);
				Main.dust[DustID2].noGravity = true;

			}


		}
		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.DD2PetDragon); }
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			Texture2D tex = ModContent.GetTexture("SGAmod/NPCs/TownNPCs/DrakenFly");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition));
			Vector2 adder = Vector2.Zero;
			Color color = drawColor;
			int timing = (int)(Main.GlobalTime * (8f));

			timing %= 4;

			int mydirection = projectile.rotation.ToRotationVector2().X > 0 ? 1 : -1;

			if (timing == 0)
			{
				adder = ((projectile.rotation + (float)Math.PI / 2f).ToRotationVector2() * (8f * mydirection));
			}


			timing *= ((tex.Height) / 4);

				for (int k = oldRot.Length - 1; k >= 0; k -= 1)
				{


					//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
					float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
					float alphaz2 = Math.Max((0.75f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f, 0f);
				float scaleffect = 1f;
						drawPos = ((oldPos[k] - Main.screenPosition));
						spriteBatch.Draw(tex, drawPos - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4), ((Color.Lerp(drawColor, Main.hslToRgb(((k / oldRot.Length) + Main.GlobalTime) % 1f, 1f, 0.75f), alphaz2) * alphaz) * (appear)) * 0.35f, projectile.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, scaleffect, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
				}


			return false;
		}

		public virtual void trailingeffect()
		{

			Rectangle hitbox = new Rectangle((int)projectile.position.X - 5, (int)projectile.position.Y - 5, projectile.height + 10, projectile.width + 10);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

			}

			oldRot[0] = projectile.rotation;
			oldPos[0] = projectile.Center;
		}
	}

	public class TrueDrakenCopyNoDeath : TrueDrakenCopy
	{

		public override bool PreKill(int timeLeft)
		{
			//null;
			return true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			ProjectileAct = delegate (Vector2 playerpos, TrueDrakenCopy DrakenCopy, int timer)
			{

				float len = DrakenCopy.projectile.velocity.Length();
				Vector2 Gowhere = DrakenCopy.projectile.velocity - DrakenCopy.projectile.rotation.ToRotationVector2();
				if (timer == DrakenCopy.projectile.ai[0])
				{
					Gowhere.Normalize();
					DrakenCopy.projectile.velocity = DrakenCopy.projectile.rotation.ToRotationVector2() * len;

				}
				if (timer < DrakenCopy.projectile.ai[0])
				{
					DrakenCopy.projectile.rotation = DrakenCopy.projectile.rotation.AngleLerp(Gowhere.ToRotation(), 0.10f);
					DrakenCopy.projectile.position -= DrakenCopy.projectile.velocity;
				}

				return true;
			};
		}

	}

}
 
#endif