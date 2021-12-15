using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;

namespace SGAmod.Items.Weapons.Technical
{
	public class NoviteKnife : SeriousSamWeapon, ITechItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Knife");
			Tooltip.SetDefault("Instantly hits against targets where you swing\nHitting some types of targets in the rear will backstab, automatically becoming a crit\nHolding this weapon increases your movement speed and jump height");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			item.damage = 36;
			item.crit = 25;
			item.width = 48;
			item.height = 48;
			item.melee = true;
			item.useTurn = true;
			item.rare = ItemRarityID.Green;
			item.value = 2500;
			item.useStyle = 1;
			item.useAnimation = 50;
			item.useTime = 50;
			item.knockBack = 8;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.consumable = false;
			item.noMelee = true;
			item.shootSpeed = 2f;
			item.shoot = ModContent.ProjectileType<NoviteStab>();
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/Technical/NoviteKnife");
				item.GetGlobalItem<ItemUseGlow>().angleAdd = MathHelper.ToRadians(-20);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Knife").WithVolume(.7f).WithPitchVariance(.15f), player.Center);
			return true;
		}

	}

	public class NoviteStab : ModProjectile,ITrueMeleeProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.melee = true;
			projectile.timeLeft = 40;
			projectile.extraUpdates = 40;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stab");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			if (target.spriteDirection== player.direction)
			{
				if ((target.aiStyle > 1 && target.aiStyle < 10) || target.aiStyle == 14 || target.aiStyle == 16 || target.aiStyle == 26 || target.aiStyle == 39 || target.aiStyle == 41 || target.aiStyle == 44)
					crit = true;

			}
		}

		public override void AI()
		{
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
		}
	}

	public class NoviteBlaster : SeriousSamWeapon, ITechItem
	{
		private bool altfired = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Blaster");
			Tooltip.SetDefault("Fires a piercing bolt of electricity\nConsumes Electric Charge, 50 up front, 200 over time to charge up\nHold the fire button to charge a stronger, more accurate shot\nCan deal up to 3X damage and chain once at max charge");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.magic = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 0;
			item.value = Item.buyPrice(0, 0, 25, 0);
			item.rare = 2;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.noUseGraphic = false;
			item.channel = true;
			item.reuseDelay = 5;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().ConsumeElectricCharge(50, 0, consume: false);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[ModContent.ProjectileType<NovaBlasterCharging>()] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<NovaBlasterCharging>(), damage, knockBack, player.whoAmI);
				player.SGAPly().ConsumeElectricCharge(50, 100);
			}
			return false;
		}

	}

	public class NovaBlasterCharging : ModProjectile
	{

		public virtual int chargeuptime => 100;
		public virtual float velocity => 32f;
		public virtual float spacing => 24f;
		public virtual int fireRate => 5;
		public virtual int FireCount => 1;
		//public virtual float ForcedLock => 1f;
		public virtual (float,float) AimSpeed => (1f,0f);
		public int firedCount = 0;
		protected Player player;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova Blaster Charging");
		}

		public override bool? CanHitNPC(NPC target) => false;

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/WaveProjectile"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public virtual void ChargeUpEffects()
		{

			if (projectile.ai[0] < chargeuptime)
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y) + randomcircle * 20, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = -randomcircle.X;
						Main.dust[num622].velocity.Y = -randomcircle.Y;
						Main.dust[num622].alpha = 150;
					}
				}
			}
			else
			{
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1.5f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.X * 2;
						Main.dust[num622].velocity.Y = randomcircle.Y * 2;
						Main.dust[num622].alpha = 100;
					}
				}
			}


			if (projectile.ai[0] == chargeuptime)
			{
				for (int num315 = 0; num315 < 15; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y), 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity = true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X * 4f;
					Main.dust[num622].velocity.Y = randomcircle.Y * 4f;
					Main.dust[num622].alpha = 150;
				}
			}

		}

		public virtual void FireWeapon(Vector2 direction)
        {
			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			float speed = 3f + perc * 3f;

			Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y) * speed); // Watch out for dividing by 0 if there is only 1 projectile.

			projectile.Center += projectile.velocity;

			int prog = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<CBreakerBolt>(), (int)((float)projectile.damage * (1f + (perc * 2f))), projectile.knockBack, player.whoAmI, perc >= 0.99f ? 1 : 0, 0.50f + (perc * 0.20f));
			Main.projectile[prog].localAI[0] = (perc * 0.90f);
			Main.projectile[prog].magic = true;
			Main.projectile[prog].melee = false;
			Main.projectile[prog].netUpdate = true;

			IdgProjectile.Sync(prog);
			Main.PlaySound(SoundID.Item91, player.Center);

			if (firedCount>=FireCount)
			projectile.Kill();
		}

		public virtual bool DoChargeUp()
        {
			return player.SGAPly().ConsumeElectricCharge(2, 60);
        }

		public override void AI()
		{
			player = Main.player[projectile.owner];

			if (player == null)
				projectile.Kill();
			if (player.dead)
				projectile.Kill();
			projectile.timeLeft = 2;

			/*if (firedCount < FireCount && channeling)
			{
				player.itemTime = 6;
				player.itemAnimation = 6;
			}*/

			Vector2 direction = (Main.MouseWorld - player.MountedCenter);
			Vector2 directionmeasure = direction;
			direction.Normalize();

			bool cantchargeup = false;

			if (projectile.ai[0] < chargeuptime + 1 && firedCount<1)
			{
				if (DoChargeUp())
					projectile.ai[0] += 1;
				else
					cantchargeup = true;
			}

			bool channeling = ((player.channel || (projectile.ai[0] < 5 && !cantchargeup)) && !player.noItems && !player.CCed);
			bool aiming = true;// firedCount < FireCount;

			if (aiming || channeling)
			{
				Vector2 mousePos = Main.MouseWorld;
				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.MountedCenter;
					diff.Normalize();
					projectile.velocity = Vector2.Lerp(Vector2.Normalize(projectile.velocity),diff, channeling ? AimSpeed.Item1 : AimSpeed.Item2);
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);

				player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
				projectile.Center = player.MountedCenter + projectile.velocity * velocity;

				if (channeling)
				{
					player.itemTime = fireRate;
					player.itemAnimation = fireRate;
				}
			}

			projectile.Center = player.MountedCenter + Vector2.Normalize(projectile.velocity) * spacing;

			if (projectile.ai[0] > 10)
			{

				ChargeUpEffects();



				if (!channeling && player.itemTime<fireRate && firedCount < FireCount)
				{
					firedCount += 1;
					player.itemTime = fireRate*(firedCount< FireCount ? 2 : 1);
					player.itemAnimation = fireRate * (firedCount < FireCount ? 2 : 1);
					FireWeapon(Vector2.Normalize(projectile.velocity));
				}

			}
		}

	}

	public class NoviteTowerSummon : SeriousSamWeapon, ITechItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Tesla Tower");
			Tooltip.SetDefault("Summons a Tesla Tower to zap enemies\nConsumes 25 Electric Charge per zap");
		}

		public override void SetDefaults()
		{
			item.damage = 13;
			item.summon = true;
			item.sentry = true;
			item.width = 24;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 2f;
			item.value = Item.buyPrice(0, 0, 25, 0);
			item.rare = 1;
			item.autoReuse = false;
			item.shootSpeed = 0f;
			item.UseSound = SoundID.Item78;
			item.shoot = ModContent.ProjectileType<NoviteTower>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				position = Main.MouseWorld;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
				player.UpdateMaxTurrets();
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class NoviteTower : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Novite Tower");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 52;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = -1;
		}

		public override void AI()
		{

			if (projectile.ai[0] == 0)
			{
				for (int i = 0; i < 4000; i += 1)
				{
					if (!Collision.CanHitLine(new Vector2(projectile.Center.X, projectile.position.Y + projectile.height), 1, 1, new Vector2(projectile.Center.X, projectile.position.Y + projectile.height + 2), 1, 1))
					{
						break;
					}
					projectile.position.Y += 1;
				}
			}

			Player player = Main.player[base.projectile.owner];
			projectile.ai[0] += 1;
			if (projectile.ai[0] > 30) {
				if (projectile.ai[0] % 20 == 0)
				{
					NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center - new Vector2(0f, 20f), new Vector2(0f, 0f), true, true, true, projectile)];
					if (target != null && target.active && target.life>0 && Vector2.Distance(target.Center, projectile.Center) < 300)
					{
						if (player.SGAPly().ConsumeElectricCharge(25, 60))
						{
							Vector2 there = projectile.Center - new Vector2(3f, 20f);
							Vector2 Speed = (target.Center - there);
							Speed.Normalize(); Speed *= 2f;
							int prog = Projectile.NewProjectile(there.X, there.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(), projectile.damage, 1f, player.whoAmI, 0);
							Main.projectile[prog].minion = true;
							Main.projectile[prog].melee = false;
							Main.projectile[prog].usesLocalNPCImmunity = true;
							Main.projectile[prog].localNPCHitCooldown = -1;
							IdgProjectile.Sync(prog);
							Main.PlaySound(SoundID.Item93, player.Center);
						}

					}

				}

				for (int num315 = 0; num315 < 3; num315 = num315 + 1)
				{
					if (Main.rand.Next(0, 15) == 0)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						int num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) - new Vector2(3f, 20f) + randomcircle * 8, 0, 0, DustID.Electric, 0f, 0f, 100, default(Color), 0.75f);

						Main.dust[num622].scale = 1f;
						Main.dust[num622].noGravity = true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Main.dust[num622].velocity.X = randomcircle.RotatedBy(MathHelper.ToRadians(-90)).X;
						Main.dust[num622].velocity.Y = randomcircle.RotatedBy(MathHelper.ToRadians(-90)).Y;
						Main.dust[num622].alpha = 150;
					}
				}

			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(texa, projectile.Center-Main.screenPosition, null, lightColor*MathHelper.Clamp(projectile.ai[0]/30f,0f,1f), 0f, new Vector2(texa.Width, texa.Height)/2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}

}
