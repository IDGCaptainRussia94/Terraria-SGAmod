using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons;
using Idglibrary;
using SGAmod.Projectiles;
using System.Linq;
using SGAmod.Effects;
using System.IO;
using Terraria.DataStructures;
using Terraria.Utilities;
using SGAmod.NPCs.Hellion;

namespace SGAmod.Items.Weapons
{

	public class BoyfriendsMic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boyfriend's Mic");
			Tooltip.SetDefault("'So, I heard you like funking on a friday night...'\nChallenge everyone around you to a beatdown of beats\nDeals damage per note hit to random enemies, per Max Sentries, halved for each enemy\nScoring SICK beats spawns hearts and awards more points\nSICK beats also become crits\nDeals your score to ALL enemies near you on completion\nRequires atleast 4 minion slots, spawns an arrow for each slot\n" + Idglib.ColorText(Color.Red, "You will be unable to use other items while rapping"));
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 500;
			item.knockBack = 50;
			item.width = 32;
			item.height = 32;
			item.useTime = 16;
			item.useAnimation = 16;
			item.autoReuse = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.value = Item.buyPrice(1, 0, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			//item.buffType = ModContent.BuffType<FlyMinionBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<FlySwarmMinion>();
		}
		public override bool CanUseItem(Player player)
		{
			return (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) > 4f) && player.ownedProjectileCounts[ModContent.ProjectileType<BFMicMasterProjectile>()] < 1;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies

			Projectile proj = Projectile.NewProjectileDirect(player.MountedCenter, Vector2.UnitY * 2f, ModContent.ProjectileType<BFMicMasterProjectile>(), damage, knockBack, player.whoAmI);
			proj.minionSlots = player.maxMinions - player.SGAPly().GetMinionSlots;
			proj.netUpdate = true;

			return false;
		}

	}

	public class BFMicMasterProjectile : HellionFNFArrowMinigameMasterProjectile
	{

		public override float AlphaPeriod => 64f;
		public override float TimeoutPeriod => 80f;
		public float ScoreScale => Owner.minionDamage * 5f;
		public override int ArrowType => ModContent.ProjectileType<BoyfriendFNFArrow>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BF FNF Tracklist");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.aiStyle = -1;
			projectile.timeLeft = 80;
			projectile.minionSlots = 4;
			projectile.scale = 1 / 3f;
			projectile.minion = true;
		}

		public override void GenerateNotes()
		{
			int maxslots = (int)projectile.minionSlots;
			int arrowindex = 0;// Main.rand.Next(maxslots);
			float rotter = arrowindex * MathHelper.PiOver2;

			for (int i = 0; i < maxslots; i += 1)
			{
				float half = (0.5f / (float)maxslots);
				float percent = (i / (float)maxslots) + half;
				float mid = percent - 0.5f;
				float max = maxslots * 42f;
				rotter = (arrowindex * MathHelper.PiOver2);

				Func<Vector2> arrowpos = delegate ()
				{
					Vector2 vector = new Vector2(i, 0);
					return new Vector2(mid * max, (float)(Math.Sin((vector.X/ 8f)+(Main.GlobalTime*2f)))*0f +(Math.Abs(mid) * 0f)+12);
				};
				HUDArrow upArrow = new HUDArrow(arrowpos);
				upArrow.rotation = rotter;
				upArrow.scaleBy = 1f / 3f;
				hudNotes.Add(upArrow);

				AddNote(-1, 20);
				for (int iii = 0; iii < 8; iii += 1)
				{
					AddNote(arrowindex, Math.Max(30 - (maxslots * Main.rand.Next(1, 4)), 5));
				}
				AddNote(-1, 30);

				arrowindex++;
				arrowindex %= maxslots;
			}

			notesToSpawn = notesToSpawn.OrderBy(testby => Main.rand.Next()).ToList();
			AddNote(-1, 60);
			notesToSpawn.Reverse();
			AddNote(-1, 30);

		}
		public override void MoveIntoPosition()
		{
			if (!IsDone)
			{
				Owner.itemTime = 80;
				Owner.itemAnimation += 1;
			}

			projectile.position.X = Owner.position.X;
			projectile.position.Y = Owner.position.Y - MathHelper.SmoothStep(420f, 160f, MathHelper.Clamp(projectile.localAI[0] / 80f, 0f, 1f));
		}

		public override void OnEnded()
		{
			if (missed > -10)
			{
				NPC[] enemies = Main.npc.Where(testby => testby.IsValidEnemy() && (testby.Center - Owner.MountedCenter).LengthSquared() < 4000000).OrderBy(testby => Main.rand.Next()).ToArray();

				if (enemies.Length > 0)
				{
					foreach (NPC enemy in enemies)
					{
						enemy.StrikeNPC(score, projectile.knockBack, 0, false);
						Owner.addDPS(score);
					}
				}
				var explode = SGAmod.AddScreenExplosion(projectile.Center, 60, 2f, 2000);
				explode.warmupTime = 60;
				explode.decayTime = 24;
				missed = -10;
			}
		}

		public override void AddNote(int type, int delay)
		{
			if (type >= 0)
				scoreMax += (int)(50 * ScoreScale);

			notesToSpawn.Add((type, delay));
		}

		public override void HitNote(HellionFNFArrow fnfarrow)
		{
			int timeWindow = fnfarrow.projectile.timeLeft;

			float[] directions = new float[] { -MathHelper.PiOver2 / 2f, -MathHelper.PiOver2 / 8f, MathHelper.PiOver2 / 3f, MathHelper.PiOver2 / 8f };

			Owner.itemRotation = directions[(int)(fnfarrow.projectile.ai[1]%4)]*Owner.direction;

			fnfarrow.GotNote(false, timeWindow);
			hudNotes[(int)fnfarrow.projectile.ai[1]].HitBeat(timeWindow, fnfarrow);

			if (timeWindow < 8)
				score += ((int)(MathHelper.Clamp(8 - fnfarrow.projectile.timeLeft, 0, 5))) * ((int)(25 * ScoreScale));
			score += (int)(50 * ScoreScale);
		}

		public override void SpawnNote(int type, int delay)
		{
			base.SpawnNote(type, delay);
		}

		public override void FailedNote(HellionFNFArrow fnfarrow)
		{
			//base.FailedNote(fnfarrow);
		}
	}

	public class BoyfriendFNFArrow : HellionFNFArrow
	{
		public override float MovementRate => 320f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Friday Night Funked, BF edition");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 100;
			projectile.aiStyle = -1;
			projectile.scale = 1 / 3f;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void DoUpdate()
		{
			Projectile owner = Main.projectile[(int)projectile.ai[0]];
			projectile.position += owner.velocity;
		}


		public override void GotNote(bool failed, int timeleft = 999)
		{
			Player Owner = Main.player[projectile.owner];
			if (!failed)
			{
				HellionFNFArrowMinigameMasterProjectile master = Main.projectile[(int)projectile.ai[0]].modProjectile as HellionFNFArrowMinigameMasterProjectile;

				NPC[] enemies = Main.npc.Where(testby => testby.IsValidEnemy() && (testby.Center - Owner.MountedCenter).LengthSquared() < 4000000).OrderBy(testby => testby.boss ? 0 : Main.rand.Next()).ToArray();

				if (enemies.Length > 0)
				{
					int damage = projectile.damage;
					for (int i = 0; i < Owner.maxTurrets; i += 1)
					{
						NPC enemy = enemies[i % enemies.Length];

						Vector2 where = master.projectile.Center+master.hudNotes[(int)projectile.ai[1]].Position;

						Projectile proj = Projectile.NewProjectileDirect(where, Vector2.Zero, ModContent.ProjectileType<NoteLaserProj>(), damage, projectile.knockBack, projectile.owner, Main.rgbToHsl(color).X, enemy.whoAmI);
						proj.penetrate = timeleft < 8 ? 100 : -1;
						proj.netUpdate = true;

						//enemy.StrikeNPC(damage, projectile.knockBack, 0, timeleft < 8);
						//Owner.addDPS(damage);
						/*if (timeleft < 8)
						{
							var explode = SGAmod.AddScreenExplosion(projectile.Center, 8, 1.5f + (5f - timeleft) / 5f, 320);
							explode.warmupTime = 16;
							explode.decayTime = 8;
						}*/
						damage /= 2;
					}
				}
			}

			base.GotNote(failed, timeleft);
		}

	}

	public class NoteLaserProj : MegidolaLaserProj
	{
		Vector2 startingloc = default;
		Vector2 hitboxchoose = default;
		protected override float AppearTime => 4f;
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.timeLeft = 60;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Funky Laser Proj");
		}

		public override void AI()
		{
			NPC enemy = Main.npc[(int)projectile.ai[1]];

			if (startingloc == default)
			{
				color1 = Main.hslToRgb(projectile.ai[0], 1f, 0.70f);
				color2 = Color.Lerp(color1, Color.White, 0.25f);
				startingloc = projectile.Center;
			}

			projectile.localAI[0] += 1f;

			if (enemy != null && enemy.active && projectile.localAI[1] < 1)
			{
				if (hitboxchoose == default)
				{
					hitboxchoose = new Vector2(Main.rand.Next(enemy.width), Main.rand.Next(enemy.height));
				}
				projectile.velocity = (enemy.position + hitboxchoose) - projectile.Center;

				if (projectile.localAI[0] == 5)
				{
					int damage = Main.DamageVar(projectile.damage);
					enemy.StrikeNPC(damage, 0, 1, projectile.penetrate >99);
					SGAmod.AddScreenShake(4f, 420, projectile.Center);
					Main.player[projectile.owner].addDPS(damage);
				}
			}
			else
			{
				projectile.localAI[1]++;
			}

			projectile.position -= projectile.velocity;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			float alpha = MathHelper.Clamp(projectile.localAI[0] / AppearTime, 0f, 1f) * MathHelper.Clamp((projectile.timeLeft-50) / AppearTime, 0f, 1f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			if (alpha > 0)
			{

				Vector2[] points = new Vector2[] { startingloc, startingloc + projectile.velocity };

				TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", mod.GetTexture("SmallLaser"));
				//UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);
				Color colorz = Color.Lerp(color1,Color.White,0.50f);
				Color colorz2 = color2;
				trail.color = delegate (float percent)
				{
					return Color.Lerp(colorz, colorz2, percent);
				};
				trail.projsize = Vector2.Zero;
				trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
				trail.coordMultiplier = new Vector2(1f, 1f);
				trail.doFade = false;
				trail.trailThickness = 16;
				trail.trailThicknessIncrease = 0;
				//trail.capsize = new Vector2(6f, 0f);
				trail.strength = alpha * 1f;
				trail.DrawTrail(points.ToList(), projectile.Center);

			}

			Texture2D mainTex = SGAmod.ExtraTextures[96];
			Texture2D glowTex = ModContent.GetTexture("SGAmod/Glow");

			float alpha2 = MathHelper.Clamp(projectile.localAI[0] / 3f, 0f, 1f) * MathHelper.Clamp(projectile.timeLeft / 25f, 0f, 1f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect effect = SGAmod.TextureBlendEffect;

			effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
			effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
			effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

			effect.Parameters["Texture"].SetValue(SGAmod.Instance.GetTexture("Extra_49c"));
			effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.GetTexture("Extra_49c"));
			effect.Parameters["noiseProgress"].SetValue(projectile.localAI[0]/30f);
			effect.Parameters["textureProgress"].SetValue(0f);
			effect.Parameters["noiseBlendPercent"].SetValue(1f);
			effect.Parameters["strength"].SetValue(alpha2);

			effect.Parameters["colorTo"].SetValue(color1.ToVector4()*new Vector4(0.5f,0.5f,0.5f,1f));
			effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

			effect.CurrentTechnique.Passes["TextureBlend"].Apply();

			Main.spriteBatch.Draw(mainTex, startingloc + projectile.velocity - Main.screenPosition, null, Color.White, projectile.rotation, mainTex.Size() / 2f, (alpha2+ (projectile.localAI[0]/60f)) * 0.75f, default, 0);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Main.spriteBatch.Draw(glowTex, startingloc + projectile.velocity - Main.screenPosition, null, Color.White* alpha2, projectile.rotation, glowTex.Size() / 2f, 0.2f+((alpha2 + (projectile.localAI[0] / 80f)) * 0.42f), default, 0);

			return false;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			base.PostDraw(spriteBatch, lightColor);
		}
	}

}
