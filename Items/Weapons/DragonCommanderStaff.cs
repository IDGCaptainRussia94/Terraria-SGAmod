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

namespace SGAmod.Items.Weapons
{

	public class DragonCommanderStaff : ModItem, IHitScanItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon Commander");
			Tooltip.SetDefault("Controls a very powerful familiar of Draken that scales with Total Expertise\nLeft click to use minion slots to summon orders\nClicking near an enemy commands your oldest order to follow that enemy\nHold CTRL and click to cancel all current orders\nDraken will rapid fly between these orders\nWhen not given orders, Draken does the following:\nWhen deselecting this weapon, give Draken a large aura before despawning\nWhen following you, Draken increases your life regen and damage resist\n'Jetpacks not included'");
		}

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			SGAPlayer sga = player.SGAPly();
			mult = 0.25f+((sga.ExpertiseCollectedTotal/20000f)*0.75f);
		}

        public override void SetDefaults()
		{
			item.damage = 750;
			item.summon = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 10;
			item.mana = 25;
			item.useAnimation = 10;
			item.autoReuse = false;
			item.useTurn = false;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0f;
			item.value = 100000;
			item.rare = ItemRarityID.Purple;
			item.UseSound = SoundID.Item100;
			item.shootSpeed = 12f;
			item.shoot = ModContent.ProjectileType<DrakenSummonTargetProj>();
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Invisible");
			}
		}
        public override bool CanUseItem(Player player)
        {
			if (player.statMana < 30)
				return false;

			item.shoot = ModContent.ProjectileType<DrakenSummonTargetProj>();

			List<NPC> enemies = SGAUtils.ClosestEnemies(Main.MouseWorld, 64);

			List<Projectile> myproj = Main.projectile.Where(testproj => testproj.active && testproj.owner == player.whoAmI && testproj.type == ModContent.ProjectileType<DrakenSummonTargetProj>()).ToList();
			myproj = myproj.OrderBy(order => (100000 - order.localAI[1])+ (order.ai[1] >= 100000 ? 999999 : 0)).ToList();

			if (player.controlSmart)
            {
				if (myproj != null && myproj.Count > 0)
				{
					foreach(Projectile proj in myproj)
                    {
						proj.timeLeft = Math.Min(proj.timeLeft, 30);


					}
				}
					item.shoot = 16;
				return true;
            }

			if (enemies != null && enemies.Count > 0 && player.ownedProjectileCounts[ModContent.ProjectileType<DrakenSummonTargetProj>()] > 0)
			{
				NPC enemy = enemies[0];

				if (myproj != null && myproj.Count > 0 && myproj[0].ai[1]<100000)
				{
					myproj[0].ai[1] = 100000 + enemy.whoAmI;
					myproj[0].netUpdate = true;
					item.shoot = 16;
					return true;
				}


			}

			return true;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			if (type == ModContent.ProjectileType<DrakenSummonTargetProj>())
			Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y, 0,0, type, damage, knockBack, player.whoAmI);
			return false;
        }
        public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 4) % 1f, 0.4f, 0.45f);
			tooltips.Add(new TooltipLine(mod, "IDG Dev Item", Idglib.ColorText(c, "IDGCaptainRussia94's other dev weapon")));
		}
	}

	public class DrakenSummonProj : ModProjectile
	{
		float alphaStrength => Math.Min(projectile.timeLeft / 30f, 1f);
		float cosmeticTrailFade = 0f;
		Vector2 there = default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken!");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 60;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = false;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = false;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override bool CanDamage()
		{
			return projectile.ai[0]>5;
		}

		public override void SetDefaults()
		{
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;
			projectile.width = 32;
			projectile.height = 32;
			aiType = ProjectileID.Bullet;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.light = 0.1f;
			projectile.timeLeft = 300;
			projectile.minion = true;
			projectile.extraUpdates = 2;
			//projectile.usesLocalNPCImmunity = false;
			//projectile.localNPCHitCooldown = 2;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.immune[projectile.owner] = 2;
		}

        public override void AI()
		{
			projectile.minion = false;
			projectile.localAI[0] += 1;
			projectile.ai[0] -= 1;
			float friction = 0.98f;
			float velosway = 60 / MathHelper.PiOver2 * (float)Math.Atan(Math.Abs(projectile.velocity.X / 5f));
			Player owner = Main.player[projectile.owner];
			if (there == default)
				there = projectile.Center;

			int followPlayer = owner.ownedProjectileCounts[ModContent.ProjectileType<DrakenSummonTargetProj>()];
			if (followPlayer > 0)
			{
				if (projectile.ai[0] < 900)//Move Order
				{
					List<Projectile> myproj = Main.projectile.Where(testproj => testproj.active && testproj.owner == projectile.owner && testproj.type == ModContent.ProjectileType<DrakenSummonTargetProj>()).ToList();
					myproj = myproj.OrderBy(order => order.localAI[1]).ToList();
					if (myproj != null && myproj.Count > 0)
					{
						Projectile target = myproj[0];
						projectile.damage = target.damage;

						if (target.DistanceSQ(projectile.Center) > 200 * 200)
							there = target.Center + Vector2.Normalize(target.Center - projectile.Center) * 640;

						Vector2 normal = Vector2.Normalize(target.Center - projectile.Center);

						friction = 0.98f;

						projectile.velocity += (there - projectile.Center) / 150f;
						projectile.velocity = Vector2.Normalize(projectile.velocity) * Math.Min(projectile.velocity.Length()/1f, 26f);

						if (Vector2.Dot(normal, Vector2.Normalize(projectile.velocity)) < -0.9f && projectile.DistanceSQ(target.Center) > 32 * 32 && projectile.DistanceSQ(target.Center) < 96 * 96)
						{
							if (target.active)
							{
								target.localAI[1] += 1000;
								target.netUpdate = true;
								return;
							}
						}

						projectile.rotation = projectile.rotation.AngleLerp(projectile.velocity.ToRotation(),0.1f);
						int dir = projectile.velocity.X > 0 ? 1 : -1;
						if (dir != projectile.spriteDirection)
						{
							projectile.spriteDirection = dir;
							projectile.rotation += MathHelper.Pi;
						}

						projectile.ai[0] = 30;
						cosmeticTrailFade = 1.2f;

					}
				}


			}
			else
			{

				if (owner.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
				{
					if (projectile.ai[0] < 1) 
					{
						there = owner.Center + new Vector2(-projectile.spriteDirection * 48, -72);
						if (projectile.velocity.Length() > 3f || projectile.velocity.Length() < 0.25f)
						{
							int dir = projectile.velocity.Length() > 3f ? (projectile.rotation.ToRotationVector2().X > 0 ? 1 : -1) : owner.direction;
							if (dir != projectile.spriteDirection)
							{
								projectile.spriteDirection = dir;
								projectile.rotation += MathHelper.Pi;
							}
						}
						projectile.rotation = projectile.rotation.AngleLerp(projectile.velocity.Length() > 3f ? projectile.velocity.ToRotation() : projectile.spriteDirection < 0 ? MathHelper.Pi : 0, 0.01f);
						projectile.velocity = (there - projectile.Center) / 40f;

					}

					if (projectile.ai[0] < 1 && owner.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
					{
						projectile.ai[0] = 0;
						projectile.damage = owner.HeldItem.damage;
						owner.AddBuff(ModContent.BuffType<DrakenDefenseBuff>(), 3);
					}

				}
			}

			if (projectile.ai[0] >=0)
			projectile.timeLeft = 300;

			if (projectile.ai[0] < -5 && projectile.ai[0] > -240)
            {
				if (projectile.ai[0] % 15 == 0)
                {
					List<NPC> enemies = SGAUtils.ClosestEnemies(projectile.Center, 200,checkCanChase: false);
					if (enemies!=null && enemies.Count > 0) 
					{
						foreach (NPC enemy in enemies)
						{
							float damazz = (Main.DamageVar((float)projectile.damage*2) * (1f-(enemy.DistanceSQ(projectile.Center) / (200 * 200))));
							enemy.StrikeNPC((int)damazz, 0f, 0, false, true, true);
							owner.addDPS((int)damazz);
							if (Main.netMode != 0)
							{
								NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, enemy.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
							}
						}
					}

				}


            }


			projectile.velocity *= friction;
			cosmeticTrailFade -= 0.04f;

			projectile.Opacity = alphaStrength;


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition));
			Vector2 adder = Vector2.Zero;
			Player owner = Main.player[projectile.owner];
			int timing = (int)(Main.GlobalTime * (8f));

			timing %= 4;

			int mydirection = projectile.spriteDirection;

			if (timing == 0)
			{
				adder = ((projectile.rotation + (float)Math.PI / 2f).ToRotationVector2() * (8f * mydirection));
			}

			if (projectile.ai[0] > 1)
			{
				TrailHelper trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("noise"));
				trail.projsize = projectile.Hitbox.Size() / 2f;
				trail.coordOffset = new Vector2(0, Main.GlobalTime * -0.5f);
				trail.trailThickness = 6;
				trail.trailThicknessIncrease = 36;
				trail.strength = alphaStrength * MathHelper.Clamp(cosmeticTrailFade, 0f, 1f);
				trail.capsize = new Vector2(8, 0);
				trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);
			}


			if (projectile.ai[0] < 28 && projectile.ai[0] >= 0)
			{
				List<Vector2> Swirl = new List<Vector2>();

				UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI*753);

				Vector2 vex = Vector2.One.RotatedBy(rando.NextFloat(MathHelper.TwoPi)) * 48f;
				float[] rando2 = {rando.NextFloat(MathHelper.TwoPi), rando.NextFloat(MathHelper.TwoPi) , rando.NextFloat(MathHelper.TwoPi) };
				float[] rando3 = { rando.NextFloat(0.1f,0.25f), rando.NextFloat(0.1f, 0.25f) , rando.NextFloat(0.1f, 0.25f) };
				for (float ix = 0; ix < 24; ix += 0.10f)
                {
					float i = (-projectile.localAI[0] / 2f) + ix;
					Matrix matrix = Matrix.CreateRotationZ((i * rando3[0]) + rando2[0])* Matrix.CreateRotationY((i * rando3[1]) + rando2[1]) *Matrix.CreateRotationX((i * rando3[2]) + rando2[2]);
					Swirl.Add(Vector2.Transform(vex, matrix)+owner.MountedCenter);

                }


				TrailHelper trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("Perlin"));
				trail.coordOffset = new Vector2(Main.GlobalTime * -0.5f,0f);
				trail.trailThickness = 6;
				trail.trailThicknessIncrease = 12;
				trail.strength = alphaStrength * MathHelper.Clamp(1f-(projectile.ai[0] / 50f), 0f, 0.75f);
				trail.capsize = new Vector2(8, 0);
				trail.color = delegate (float percent)
				{
					return Main.hslToRgb(((1f-percent)+ Main.GlobalTime/3f) %1f,0.9f,0.75f);
				};
				trail.DrawTrail(Swirl, projectile.Center);
			}

			if (projectile.ai[0]<0)
			{
				for (float i = -1; i < 2; i += 0.4f)
				{
					Vector2 scaleup = new Vector2((float)Math.Abs(Math.Sin(Main.GlobalTime / 1.1694794f)), 1f) * MathHelper.Clamp(-projectile.ai[0]/30, 0f, 1f) * alphaStrength;
					Texture2D texture7 = SGAmod.ExtraTextures[34];
					spriteBatch.Draw(texture7, projectile.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime) % 1f, 1f, 0.75f) * 0.50f, -Main.GlobalTime * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), scaleup, SpriteEffects.None, 0f);
					texture7 = SGAmod.HellionTextures[6];
					spriteBatch.Draw(texture7, projectile.Center - Main.screenPosition, null, Main.hslToRgb((Main.GlobalTime) % 1f, 1f, 0.75f) * 0.50f, Main.GlobalTime * 17.134f * i, new Vector2(texture7.Width / 2f, texture7.Height / 2f), scaleup, SpriteEffects.None, 0f);
				}
			}


			timing *= ((tex.Height) / 4);

			for (int k = (projectile.oldRot.Length/2) - 1; k >= 0; k -= 1)
			{


				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaxx = MathHelper.Clamp((projectile.velocity.Length()-3f)/20f,0f,0.1f);
				float alphaz = (1f - (float)(k + 1) / (float)(projectile.oldRot.Length + 2)) * (k>0 ? alphaxx : 1f);
				float scaleffect = 1f;
				//Color fancyColor = Main.hslToRgb(((k / projectile.oldRot.Length) + projectile.localAI[0] / 30f) % 1f, 1f, 0.75f);
				drawPos = ((projectile.oldPos[k] - Main.screenPosition));
				spriteBatch.Draw(tex, drawPos + (drawOrigin / 4f) - adder, new Rectangle(0, timing + 2, tex.Width, (tex.Height - 1) / 4),
					((drawColor * alphaz) * (projectile.Opacity))
					, projectile.rotation - (float)(mydirection < 0 ? Math.PI : 0), drawOrigin, scaleffect, mydirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
			}


			return false;
		}

		public override string Texture => "SGAmod/NPCs/TownNPCs/DrakenFly";

	}

	public class DrakenSummonTargetProj : ModProjectile
	{
		float strength => Math.Min(projectile.timeLeft / 30f, 1f);
		public Vector2 bezspot1=default;
		public Vector2 bezspot2=default;
		public Vector2 bezspot3 = default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Draken Aim");
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

        public override bool CanDamage()
        {
            return false;
        }

        public override void SetDefaults()
		{
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;
			projectile.width = 4;
			projectile.height = 4;
			aiType = ProjectileID.Bullet;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.light = 0.1f;
			projectile.timeLeft = 900;
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 1f;
			projectile.extraUpdates = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 2;
		}

        public override void AI()
		{
			projectile.localAI[0] += 1;
			Player owner = Main.player[projectile.owner];
			if (projectile.localAI[0] == 1)
            {
				bezspot1 = owner.MountedCenter + Main.rand.NextVector2CircularEdge(200, 200);
				bezspot2 = projectile.Center + Main.rand.NextVector2CircularEdge(500, 500);
				bezspot3 = owner.MountedCenter+new Vector2(0,-32f);
				projectile.spriteDirection = (owner.MountedCenter - projectile.Center).X < 0 ? 1 : -1;

			}


			if (owner != null && owner.active && projectile.timeLeft>40 && owner.HeldItem.type == ModContent.ItemType<DragonCommanderStaff>())
			{
				projectile.timeLeft += 1;
			}

			projectile.velocity /= 1.25f;


			if (projectile.localAI[0] > 30)
			{
				/*if (projectile.localAI[0] % 15 == 0) 
				{
				List<Projectile> myprojs = Main.projectile.Where(testproj => testproj.active && testproj.owner == projectile.owner && testproj.type == ModContent.ProjectileType<DrakenSummonTargetProj>() && testproj.whoAmI != projectile.whoAmI).ToList();
				myprojs = myprojs.OrderBy(order => order.localAI[0]).ToList();

					if (myprojs != null && myprojs.Count > 0 && myprojs[0] != projectile)
					{
						List<Projectile> draken = Main.projectile.Where(testproj => testproj.active && testproj.owner == projectile.owner && testproj.type == ModContent.ProjectileType<DrakenSummonProj>()).ToList();
						if (draken != null && draken.Count > 0)
						{

							//myprojs = myprojs.OrderBy(order => order.DistanceSQ(projectile.Center)).ToList();

							//foreach (Projectile zapperFriendly in myprojs)
							//{

							List<NPC> enemies = SGAUtils.ClosestEnemies(projectile.Center, 800);
							if (enemies != null && enemies.Count > 0)
							{

								NPC zapperFriendly = enemies[0];

								if (zapperFriendly.DistanceSQ(projectile.Center) < 1600 * 1600 && zapperFriendly.DistanceSQ(projectile.Center) > 120 * 120)
								{
									Vector2 center = projectile.Center + (zapperFriendly.Center - projectile.Center) + Main.rand.NextVector2Circular(16f, 16f);
									Projectile proj = Projectile.NewProjectileDirect(projectile.Center, Vector2.Normalize(zapperFriendly.Center - projectile.Center) * 8f, ProjectileID.GreenLaser, (int)(projectile.damage / 5f), 0f, projectile.owner);
									proj.usesIDStaticNPCImmunity = true;
									proj.idStaticNPCHitCooldown = 10;
									proj.netUpdate = true;
								}
							}
						}
					}




				}*/

					if (projectile.ai[1] >= 100000)
				{
					NPC enemy = Main.npc[(int)projectile.ai[1] - 100000];
					if (enemy != null && enemy.active)
					{
						Vector2 vectx = enemy.Center - projectile.Center;
						if (vectx.LengthSquared()>64)
						projectile.velocity += Vector2.Normalize(vectx) * ((vectx.Length() / 5f));


					}
					else
					{
						projectile.ai[1] = 0;
						projectile.netUpdate = true;

					}

                }
			}


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D texture = SGAmod.ExtraTextures[110];
			Vector2 origin = texture.Size() / 2f;
			float timeAdvance = Main.GlobalTime * 2;

			Vector2 drawHere = IdgExtensions.BezierCurve(bezspot3, bezspot3, bezspot1,bezspot2, projectile.Center, Math.Min(projectile.localAI[0]/30f, 1f));
			drawHere -= Main.screenPosition;

			for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.PiOver4)
			{
				spriteBatch.Draw(texture, drawHere + (Vector2.One.RotatedBy(i - timeAdvance)) * 8f, null, Color.Lime * 0.75f * MathHelper.Clamp(projectile.timeLeft / 20f, 0f, 1f), -MathHelper.PiOver4 + (i - timeAdvance), origin, 0.25f, SpriteEffects.None, 0);
			}
			texture = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texture, drawHere, null, Color.White * 0.75f * strength, 0, texture.Size() / 2f, 1f, projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

			return false;
		}

		public override string Texture => "SGAmod/NPCs/TownNPCs/DrakenFly_Head";

	}

	public class DrakenDefenseBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Draken Defense");
			Description.SetDefault("damage reduced to damage^0.90\nand massively boosted life regen");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/NPCs/TownNPCs/DrakenFly_Head";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.lifeRegen += 10;

		}
	}

}
