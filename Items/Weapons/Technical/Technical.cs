using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;

namespace SGAmod.Items.Weapons.Technical
{
	public class AssaultRifle : SeriousSamWeapon
	{
		int firemode = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tactical SMG Rifle");
			Tooltip.SetDefault("Adjustable Machinegun!\nVery Fast! But no ammo saving chance and causes very bad recoil if held down for too long.\nRight click to toggle firemodes");
		}

		public override void SetDefaults()
		{
			item.damage = 28;
			item.ranged = true;
			item.width = 42;
			item.height = 16;
			item.useTime = 3;
			item.useAnimation = 3;
			item.useStyle = 5;
			item.reuseDelay = 0;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = 750000;
			item.rare = 8;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 40f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(firemode);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			firemode = reader.ReadInt32();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.itemTime > 0 && player.altFunctionUse == 2)
				return false;
			if (player.altFunctionUse == 2)
			{
				string[] things = { "Fully Automatic", "Burst"};
				item.reuseDelay = 15;
				player.itemTime = 20;
				firemode += 1;
				firemode %= 2;
				Main.PlaySound(40, player.Center);
				if (Main.myPlayer == player.whoAmI)
				Main.NewText("Toggled: " + things[firemode] + " mode");
			}
			else
			{
				item.reuseDelay = 0;
				item.useTime = 3;
				item.useAnimation = 3;
				if (firemode == 1)
				{
					item.useTime = 2;
					item.useAnimation = 12;
					item.reuseDelay = 25;
				}

			}
			return (player.altFunctionUse!=2);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(0, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (player.GetModPlayer<SGAPlayer>().recoil<75f)
			player.GetModPlayer<SGAPlayer>().recoil += firemode==1 ? 0.4f : 0.75f;

			Main.PlaySound(SoundID.Item41, player.Center);

			float speed = 1.5f;
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(1+ player.GetModPlayer<SGAPlayer>().recoil);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 20f;

			Vector2 perturbedSpeed2 = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = perturbedSpeed2.RotatedBy(MathHelper.ToRadians(MathHelper.Lerp(-0.1f, 0.1f, i/ (numberProjectiles-1)))); // Watch out for dividing by 0 if there is only 1 projectile.

				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].knockBack = item.knockBack;
				player.itemRotation = Main.projectile[proj].velocity.ToRotation();
				if (player.direction < 0)
					player.itemRotation += (float)Math.PI;
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddIngredient(ItemID.PlatinumBar, 10);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 15);
			recipe.AddIngredient(mod.ItemType("PlasmaCell"), 2);
			recipe.AddIngredient(ItemID.Nanites, 100);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class OnyxTacticalShotgun : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Onyx Tactical Shotgun");
			Tooltip.SetDefault("Fires a Spread of Bullets and Onyx Rockets");
		}

		public override void SetDefaults()
		{
			item.damage = 27;
			item.ranged = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 500000;
			item.rare = 7;
			item.UseSound = SoundID.Item38;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "HeatBeater", 1);
			recipe.AddIngredient(null, "SharkTooth", 50);
			recipe.AddIngredient(ItemID.OnyxBlaster, 1);
			recipe.AddIngredient(ItemID.TacticalShotgun, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 normz = new Vector2(speedX, speedY);normz.Normalize();
			position += normz * 24f;

			int numberProjectiles = 9 + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(30));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)(damage*0.75), knockBack, player.whoAmI);
				IdgProjectile.Sync(prog);
			}
			numberProjectiles = 3 + Main.rand.Next(1);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				float scale = 1f - (Main.rand.NextFloat() * .6f);
				perturbedSpeed = perturbedSpeed * scale;
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.BlackBolt, (int)(damage*2.0), knockBack, player.whoAmI);
				IdgProjectile.Sync(prog);

			}
			return false;
		}
	}

	public class CircuitBreakerBlade : SeriousSamWeapon
	{
		public static NPC FindClosestTarget(Player ply,Vector2 loc, Vector2 size, bool block = true, bool friendlycheck = true, bool chasecheck = false)
		{
			int num;
			float num170 = 1000000;
			NPC num171 = null;

			for (int num172 = 0; num172 < Main.maxNPCs; num172 = num + 1)
			{
				float num173 = Main.npc[num172].position.X + (float)(Main.npc[num172].width / 2);
				float num174 = Main.npc[num172].position.Y + (float)(Main.npc[num172].height / 2);
				float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
				if (Main.npc[num172].active)
				{

					if (num175 < num170 && !Main.npc[num172].dontTakeDamage && ((Collision.CanHit(new Vector2(loc.X, loc.Y), 1, 1, Main.npc[num172].position, Main.npc[num172].width, Main.npc[num172].height) && block) || block == false) && (Main.npc[num172].townNPC == false && (Main.npc[num172].CanBeChasedBy(new Projectile(), false) || !chasecheck)))
					{
						if (Main.npc[num172].immune[ply.whoAmI]<1)
						{
							num170 = num175;
							num171 = Main.npc[num172];
						}
					}
				}
				num = num172;
			}
			if (num170 > 400)
				return null;

			return num171;

		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Circuit Breaker Blade");
			Tooltip.SetDefault("Melee hits against enemies discharge bolts of energy at nearby enemies that chain to other enemies on hit\nChains up to a max of 3 times, and each bolt may hit 2 targets max\nRequires 500 Electric Charge to discharge bolts\nCounts as a True Melee sword");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			item.damage = 65;
			item.width = 48;
			item.height = 48;
			item.melee = true;
			item.useTurn = true;
			item.rare = 7;
			item.value = 400000;
			item.useStyle = 1;
			item.useAnimation = 35;
			item.useTime = 35;
			item.knockBack = 8;
			item.autoReuse = true;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CircuitBreakerBlade_Glow");
			}
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			Vector2 position = player.Center;
			Vector2 eree = player.itemRotation.ToRotationVector2();
			eree *= player.direction;

			position += (eree * Main.rand.NextFloat(58f,160f));

				target.immune[player.whoAmI] = 15;
				NPC target2 = CircuitBreakerBlade.FindClosestTarget(player, position, new Vector2(0, 0));
				if (target2 != null)
				{
					if (player.SGAPly().ConsumeElectricCharge(500, 100))
					{
					Vector2 Speed = (target2.Center - target.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(target.Center.X, target.Center.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(), (int)(damage * 0.80), knockBack / 2f, player.whoAmI, 3);
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, position);
					}

				}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TeslaStaff"), 1);
			recipe.AddIngredient(ItemID.BreakerBlade, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.Cog, 50);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 2);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class CBreakerBolt : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 2;
			projectile.melee = true;
			projectile.timeLeft = 120;
			projectile.light = 0.1f;
			projectile.extraUpdates = 120;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Breaker Bolt");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.ignoreWater = true;
			projectile.Kill();
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.ai[0] > 0)
			{
				projectile.ai[0] -= 1;
				NPC target2 = CircuitBreakerBlade.FindClosestTarget(Main.player[projectile.owner], projectile.Center, new Vector2(0, 0));
				if (target2 != null)
				{
					Vector2 Speed = (target2.Center - target.Center);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(target.Center.X, target.Center.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(),projectile.damage, projectile.knockBack / 2f, projectile.owner, projectile.ai[0]);
					Main.projectile[prog].melee = projectile.melee;
					Main.projectile[prog].magic = projectile.magic;
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, projectile.Center);
				}
			}
			if (projectile.penetrate < 1)
			{
				projectile.ignoreWater = true;
				projectile.Kill();
			}
		}

		public override bool PreKill(int timeLeft)
		{
			if (projectile.ignoreWater) {
				for (int k = 0; k < 10; k++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 1f;
					int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1.5f*(1f+(projectile.ai[0]/3f)));
					Main.dust[num655].noGravity = true;
					Main.dust[num655].velocity *= 0.5f;
				}
			}


			return true;
		}

		Vector2 basepoint=Vector2.Zero;

		public override void AI()
		{
			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1f * (1f + (projectile.ai[0] / 3f)));
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;

			Vector2 gothere = projectile.velocity;
			gothere=gothere.RotatedBy(MathHelper.ToRadians(90));
			gothere.Normalize();
			Player player = Main.player[projectile.owner];

			if (basepoint == Vector2.Zero)
			{
				basepoint = projectile.Center;
				projectile.localAI[1] = (float)player.SGAPly().timer;
			}
			else
			{
				basepoint += projectile.velocity;
			}

			float theammount = ((float)projectile.timeLeft + (float)(projectile.whoAmI*6454f)+(projectile.localAI[1]*3.137f));

			projectile.Center += ((gothere * ((float)Math.Sin((double)theammount / 7.10) * 1.97f))+ (gothere * ((float)Math.Cos((double)theammount / -13.00) * 2.95f))+ (gothere * ((float)Math.Sin((double)theammount / 4.34566334) * 2.1221f))
				*(1f-projectile.ai[1]));

			if (projectile.localAI[1] == 0f)
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}
		}
	}

	public class TeslaStaff : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tesla Staff");
			Tooltip.SetDefault("Zaps nearby enemies with a shock of electricity that is able to pierce twice\nRequires 50 Electric Charge to discharge bolts");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 2;
			item.width = 40;
			item.height = 40;
			item.useTime = 4;
			item.useAnimation = 4;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0;
			item.value = 75000;
			item.rare = 3;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("UnmanedBolt");
			item.shootSpeed = 4f;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/TeslaStaff_Glow");
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 normz = new Vector2(speedX, speedY); normz.Normalize();
			position += normz * 58f;


			NPC target2 = CircuitBreakerBlade.FindClosestTarget(player, position, new Vector2(0, 0));
			if (target2 != null)
			{
				if (player.SGAPly().ConsumeElectricCharge(50, 60))
				{
					Vector2 Speed = (target2.Center - position);
					Speed.Normalize(); Speed *= 2f;
					int prog = Projectile.NewProjectile(position.X, position.Y, Speed.X, Speed.Y, ModContent.ProjectileType<CBreakerBolt>(), (int)(damage * 1), knockBack, player.whoAmI);
					Main.projectile[prog].melee = false;
					Main.projectile[prog].magic = true;
					IdgProjectile.Sync(prog);
					Main.PlaySound(SoundID.Item93, position);
				}
			}

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Wire, 50);
			recipe.AddIngredient(mod.ItemType("UnmanedStaff"), 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 6);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

		public class Massacre : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Massacre Prototype");
			Tooltip.SetDefault("Fires a chain of Stardust Explosions\nFiring this weapon throws you back\n'Ansaksie would not approve'");
		}

		public override void SetDefaults()
		{
			item.damage = 250;
			item.magic = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 90;
			item.useAnimation = 90;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = Item.sellPrice(platinum: 2);
			item.rare = 11;
			item.UseSound = SoundID.Item122;
			item.autoReuse = true;
			item.shoot = 14;
			item.mana = 200;
			item.shootSpeed = 200f;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, -4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "PrismalLauncher", 1);
			recipe.AddIngredient(ItemID.ProximityMineLauncher,1);
			recipe.AddIngredient(ItemID.Stynger, 1);
			recipe.AddIngredient(ItemID.FragmentStardust, 10);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 5);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 15);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.velocity += new Vector2(Math.Sign(-player.direction) * 20, (-10f-(speedY / 15f)));
			int numberProjectiles = 4;// + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				float scale = 1f - (Main.rand.NextFloat() * .3f);
				perturbedSpeed = perturbedSpeed * scale;
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<MassacreShot>(), damage, knockBack, player.whoAmI);
				IdgProjectile.Sync(prog);
			}
			return false;
		}
	}

	public class MassacreShot : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.timeLeft = 10;
			projectile.light = 0.1f;
			projectile.extraUpdates = 0;
			projectile.tileCollide = false;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Massa proj");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}


		public override void AI()
		{
			projectile.velocity = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(25));
			Vector2 vex = Main.rand.NextVector2Circular(160, 160);
			int prog = Projectile.NewProjectile(projectile.Center.X+ vex.X, projectile.Center.Y+ vex.Y, 0,0, ProjectileID.StardustGuardianExplosion, projectile.damage, projectile.knockBack, projectile.owner,0f,8f);
			Main.projectile[prog].scale = 3f;
			Main.projectile[prog].usesLocalNPCImmunity = true;
			Main.projectile[prog].localNPCHitCooldown = -1;
			Main.projectile[prog].magic = true;
			Main.projectile[prog].minion = false;
			Main.projectile[prog].netUpdate = true;
			IdgProjectile.Sync(prog);
		}
	}


	public class BigDakka : SeriousSamWeapon
	{
		int ammotype;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Dakka");
			Tooltip.SetDefault("Rapidly fires 2 pairs of bullets from the smaller chambers\ncontinue holding to charge up the central barrel to fire a homing shot that unleashes fiery death!\n75% to not consume ammo per bullet fired");
		}

		public override void SetDefaults()
		{
			item.damage = 80;
			item.ranged = true;
			item.width = 32;
			item.height = 62;
			item.crit = 5;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 75, 0, 0);
			item.rare = 10;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.channel = true;
			item.reuseDelay = 5;
			item.useAmmo = AmmoID.Bullet;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/BigDakka_Glow");
				item.GetGlobalItem<ItemUseGlow>().glowOffsetX = -26;
			}
		}

		public override bool ConsumeAmmo(Player player)
		{
			return false;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-26, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(ItemID.SnowmanCannon, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 12);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 8);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 8);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 50);
			recipe.AddIngredient(mod.ItemType("CalamityRune"), 3);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			//Main.NewText(ammotype);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[mod.ProjectileType("BigDakkaCharging")] < 1)
			{
				int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("BigDakkaCharging"), damage, knockBack, player.whoAmI);
				Main.projectile[proj].ai[1] = type;
				Main.projectile[proj].netUpdate = true;
			}
			return false;
		}

	}

	public class BigDakkaCharging : ModProjectile
	{
		int thestart;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Dakka Charging");
		}

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
			projectile.ranged = true;
			projectile.timeLeft = 600;
			aiType = 0;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(projectile.localAI[0]);
			writer.Write(projectile.localAI[1]);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			projectile.localAI[0] = reader.ReadInt32();
			projectile.localAI[1] = reader.ReadInt32();
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			int DustID2;
			if (projectile.ai[0] < 150)
			{
				DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X * 1.2f, projectile.velocity.Y * 1.2f, 20, default(Color), 1f);
				Main.dust[DustID2].noGravity = true;
			}
			else
			{
				if (projectile.ai[0] == 150)
				{
					for (float new1 = 0.5f; new1 < 2f; new1 = new1 + 0.05f)
					{
						float angle2 = MathHelper.ToRadians(Main.rand.Next(150, 210));
						Vector2 angg2 = projectile.velocity.RotatedBy(angle2);
						DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 20, default(Color), 2f);
						Main.dust[DustID2].velocity = new Vector2(angg2.X, angg2.Y) * new1;
						Main.dust[DustID2].noGravity = true;
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 117, 0.75f, 0.5f);
					}

				}
				for (float new1 = -1f; new1 < 2f; new1 = new1 + 2f)
				{
					float angle = 90;
					Vector2 angg = projectile.velocity.RotatedBy(angle * new1);
					DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 20, default(Color), 1f);
					Main.dust[DustID2].velocity = new Vector2(angg.X * 2f, angg.Y * 2f);
					Main.dust[DustID2].noGravity = true;
				}

				for (float new1 = 0.2f; new1 < 0.8f; new1 = new1 + 0.2f)
				{
					if (Main.rand.Next(0, 10) < 2)
					{
						float angle2 = MathHelper.ToRadians(Main.rand.Next(150, 210));
						Vector2 angg2 = projectile.velocity.RotatedBy(angle2);
						Vector2 posz = new Vector2(projectile.position.X, projectile.position.Y);
						Vector2 norm = projectile.velocity; norm.Normalize();
						posz += norm * -76f;
						posz += norm.RotatedBy(MathHelper.ToRadians(-90 * player.direction)) * 16f;
						DustID2 = Dust.NewDust(posz, projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 20, default(Color), 1.5f);
						Main.dust[DustID2].velocity = (new Vector2(angg2.X, angg2.Y - Main.rand.NextFloat(0f, 1f)) * new1);
						Main.dust[DustID2].noGravity = true;
					}
				}

			}

			if (thestart == 0) { thestart = player.itemTime; }
			if (!player.channel || player.dead || projectile.timeLeft < 300)
			{
				projectile.tileCollide = true;
				if (projectile.timeLeft > 6)
				{
					if (projectile.ai[0] > 150)
					{

						int proj = Projectile.NewProjectile(projectile.Center, projectile.velocity, mod.ProjectileType("DakkaShot"), projectile.damage * 6, projectile.knockBack, player.whoAmI);
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 116, 0.75f, 0.5f);

					}
					projectile.timeLeft = 6;

				}
			}
			else
			{
				projectile.ai[0] += 1;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					diff.Normalize();
					projectile.velocity = diff;
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);
				player.itemTime = 40;
				player.itemAnimation = 40;
				player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);

				projectile.Center = player.Center + (projectile.velocity * 56f);
				projectile.velocity *= 8f;



				if (projectile.timeLeft < 600)
				{
					projectile.timeLeft = 600 + thestart;
					player.itemTime = thestart;
					Vector2 perturbedSpeed = projectile.velocity; // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= 1.25f;
					int ammotype = (int)player.GetModPlayer<SGAPlayer>().myammo;
					if (ammotype > 0) {
						Item ammo2 = new Item();
						ammo2.SetDefaults(ammotype);
						int ammo = ammo2.shoot;
						int damageproj = projectile.damage;
						float knockbackproj = projectile.knockBack;
						float sppez = perturbedSpeed.Length();
						if (ammo2.modItem != null)
						ammo2.modItem.PickAmmo(player.HeldItem,player,ref ammo,ref sppez, ref projectile.damage,ref projectile.knockBack);
						int type = ammo;


						Vector2 normal = perturbedSpeed; normal.Normalize();
						Vector2 newspeed = normal; newspeed *= sppez;
						for (int num315 = -1; num315 < 2; num315 = num315 + 2)
						{
							if (player.HasItem(ammotype))
							{
								for (int new1 = 0; new1 < 5; new1 = new1 + 3)
								{
									Vector2 newloc = projectile.Center;
									newloc -= normal * 8f;
									newloc += (normal.RotatedBy(MathHelper.ToRadians(90)) * num315) * (10 + new1);
									int proj = Projectile.NewProjectile(newloc.X, newloc.Y, newspeed.X * 1.5f, newspeed.Y * 1.5f, type, damageproj, knockbackproj, player.whoAmI);
									if (Main.rand.Next(100) < 25 && (ammo2.modItem!=null && ammo2.modItem.ConsumeAmmo(player)) && ammo2.maxStack>1)
										player.ConsumeItem(ammotype);
								}

							}
						}
						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 38, 0.5f, 0.5f);
					}
					
				}
			}
		}
	}

	public class DakkaShot : UnmanedArrow
	{
		protected override float homing => 0.1f;
		protected override float gravity => 0f;
		protected override float maxhoming => 10000f;
		protected override float homingdist => 2000f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dakka Shot");
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/WaveProjectile"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.arrow = true;
			projectile.penetrate = 2;
			projectile.timeLeft = 800;
			projectile.extraUpdates = 5;
			projectile.tileCollide = false;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override bool PreKill(int timeLeft)
		{
			return false;
		}

		public override void effects(int type)
		{
			if (type == 0)
			{
				int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 235, projectile.velocity.X * 0.25f, projectile.velocity.Y * 0.25f, 20, default(Color), 1.5f);
				Main.dust[DustID2].noGravity = true;

				if (projectile.ai[0] % 30 == 0)
				{
					Main.PlaySound(SoundID.Item45, projectile.Center);
					int numProj = 2;
					//float rotation = MathHelper.ToRadians(1);
					for (int i = 0; i < numProj; i++)
					{
						//Vector2 perturbedSpeed = new Vector2(projectile.velocity.X, projectile.velocity.Y).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numProj - 1)));
						Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BoulderBlast"), projectile.damage, projectile.knockBack, projectile.owner, 0f, 0f);
					}

				}

			}

		}


	}

	public class RodofEnforcement : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Enforcement");
			Tooltip.SetDefault("Conjures an impacting force of energy at the mouse cursor\nRelease to send the force flying towards your cursor; pierces many times");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 18;
			item.value = 75000;
			item.rare = 4;
			item.UseSound = SoundID.Item100;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ROEproj");
			item.shootSpeed = 16f;
			item.mana = 15;
			item.channel = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/RodofEnforcement_Glow");
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddIngredient(ItemID.Actuator, 10);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Vector2 normz = new Vector2(speedX, speedY); normz.Normalize();
			//position += normz * 24f;

			
			return true;
		}
	}



	public class ROEproj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Enforcing");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.magic = true;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.ai[0]<1)
			return false;
			return null;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item45, projectile.Center);

			return true;
		}

		public override void AI()
		{

			//int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"), projectile.velocity.X * 1f, projectile.velocity.Y * 1f, 20, default(Color), 1f);

			bool cond = projectile.ai[1] < 1 || projectile.ai[0] == 2 || projectile.timeLeft == 2;
			for (int num621 = 0; num621 < (cond ? 30 : 1); num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, projectile.velocity.X * (cond ? 1.5f : 0.5f), projectile.velocity.Y * (cond ? 1.5f : 0.5f), 20, default(Color), 1f);
				Main.dust[num622].velocity *= 1f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;
			}


			Player player = Main.player[projectile.owner];
			projectile.ai[1] += 1;
			if (player.dead)
			{
				projectile.Kill();
			}
			else
			{
				if (((projectile.ai[0] > 0 || player.statMana < 1) || !player.channel) && projectile.ai[1]>1)
				{
					projectile.ai[0] += 1;
					if (projectile.ai[0] == 1)
					{
						Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 113, 0.5f, -0.25f);

					}


					if (projectile.ai[0] == 4)
					{

					}
				}
				else
				{
					if (projectile.ai[0] < 1)
					{
						Vector2 mousePos = Main.MouseWorld;
						if (projectile.owner == Main.myPlayer && projectile.ai[1] < 2)
						{
							projectile.Center = mousePos;
							projectile.netUpdate = true;
						}
						if (projectile.owner == Main.myPlayer && mousePos!= projectile.Center)
						{
							Vector2 diff2 = mousePos - projectile.Center;
							diff2.Normalize();
							projectile.velocity = diff2 * 20f;
							projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
							projectile.netUpdate = true;
							//projectile.Center = mousePos;

						}

						projectile.timeLeft = 40;
						projectile.position -= projectile.velocity;

						//projectile.position -= projectile.Center;
						int dir = projectile.direction;
						player.ChangeDir(dir);
						player.itemTime = 40;
						player.itemAnimation = 38;


						Vector2 distz = projectile.Center - player.Center;
						player.itemRotation = (float)Math.Atan2(distz.Y * dir, distz.X*dir);
						//projectile.Center = (player.Center + projectile.velocity * 26f) + new Vector2(0, -24);
					}
				}
			}
		}
	}

	public class BeamCannon : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Cannon");
			Tooltip.SetDefault("Fires discharged bolts of piercing plasma\nThe less mana you have, the more your bolts fork out from where you aim\nAlt fire drains Plasma Cells to fire a wide spread of bolts");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 4));
			SGAmod.UsesPlasma.Add(SGAmod.Instance.ItemType("BeamCannon"), 1000);
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.magic = true;	
			item.crit = 15;
			item.width = 56;
			item.height = 28;
			item.useTime = 7;
			item.useAnimation = 7;
			item.useStyle = 5;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.knockBack = 1;
			item.value = 1000000;
			item.rare = 9;
			item.UseSound = SoundID.Item115;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("BeamCannonHolding");
			item.shootSpeed = 16f;
			item.mana = 8;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("BeamCannonHolding")] > 0)
				return false;
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
				return (modply.RefilPlasma());
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "StarMetalBar", 15);
			recipe.AddIngredient(null, "PlasmaCell", 3);
			recipe.AddIngredient(null, "ManaBattery", 5);
			recipe.AddIngredient(null, "AdvancedPlating", 8);
			recipe.AddIngredient(ItemID.ChargedBlasterCannon, 1);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;

			if (player.altFunctionUse == 2)
			{
				modply.plasmaLeftInClip -= 50;
				player.statMana -= (int)(40f * player.manaCost);
				player.itemTime *= 5;
				player.itemAnimation *= 5;
				Main.PlaySound(SoundID.Item, player.Center, 122);
			}
			else
			{
				modply.plasmaLeftInClip -= 5;
			}
				if (modply.plasmaLeftInClip < 1)
				{
					player.itemTime = 90;
				}

			for (float i = -40; i < 41; i += 20)
			{
				if (player.altFunctionUse == 2 || i == 0f)
				{
					Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
					Vector2 perturbedSpeed2 = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(Math.Max(1f,-20f+(60f-(float)((float)player.statMana/ (float)player.statManaMax2)*60f))));
					float scale = 1f;// - (Main.rand.NextFloat() * .2f);
					perturbedSpeed = perturbedSpeed * scale;

					float rotation = MathHelper.ToRadians(3);
					Vector2 speed = new Vector2(0f, 72f);
					Vector2 starting = new Vector2(position.X + offset.X, position.Y + offset.Y);
					float aithis = (perturbedSpeed2).ToRotation() + MathHelper.ToRadians(i);
					int proj = Projectile.NewProjectile(starting.X+ Math.Sign(perturbedSpeed.X) * 6, starting.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CultistBossLightningOrbArc, damage, 15f, player.whoAmI, aithis, modply.timer);
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					//Main.projectile[proj].usesLocalNPCImmunity = true;
					Main.projectile[proj].localNPCHitCooldown = 5;
					Main.projectile[proj].penetrate = -1;
					Main.projectile[proj].timeLeft = 300;
					Main.projectile[proj].magic = true;
					if (player.altFunctionUse != 2)
					{
						Main.projectile[proj].GetGlobalProjectile<SGAprojectile>().shortlightning = 14;
						Main.projectile[proj].extraUpdates = 2;
					}
					Main.projectile[proj].ai[0] = aithis;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);
				}


			}
			offset /= 8f;
			int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, offset.X, offset.Y, mod.ProjectileType("BeamCannonHolding"), damage, knockBack, player.whoAmI);

			return false;
		}
	}

	public class BeamCannonHolding : ModProjectile
	{
		public virtual bool bounce => true;
		//public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Gun");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.timeLeft = 3;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void AI()
		{
			projectile.localAI[0] += 1f;

			Player player = Main.player[projectile.owner];

			if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				if (player.dead)
				{
					projectile.Kill();
				}
				else
				{


					player.heldProj = projectile.whoAmI;
					player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * projectile.direction, projectile.velocity.X * projectile.direction);
					projectile.rotation = player.itemRotation - MathHelper.ToRadians(90);
					projectile.Center = (player.Center + new Vector2(player.direction * 6, 0)) + (projectile.velocity * 10f);


					projectile.position -= projectile.velocity;
					if (player.itemTime > 0)
					projectile.timeLeft = 2;
					Vector2 position = projectile.Center;
					Vector2 offset = new Vector2(projectile.velocity.X, projectile.velocity.Y);
					offset.Normalize();
					offset *= 16f;

				}
			}
			else
			{
				projectile.Kill();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/Technical/BeamCannon");
			int frames = 4;
			//Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProjGlow");
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / frames) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
			Color color = projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(Main.GlobalTime*8f);
			timing %= frames;
			timing *= ((tex.Height) / frames);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / frames), color, projectile.rotation - MathHelper.ToRadians(90*projectile.direction), drawOrigin, projectile.scale, projectile.direction < 1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);
			//spriteBatch.Draw(texGlow, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / frames), Color.White, projectile.rotation - MathHelper.ToRadians(90 * projectile.direction), drawOrigin, projectile.scale, projectile.direction < 1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);

			return false;
		}


	}



}
