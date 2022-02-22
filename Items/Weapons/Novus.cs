using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Tools;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	public class UnmanedStaff : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Staff");
			Tooltip.SetDefault("Casts homing Novus bolts");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 5;
			item.width = 40;
			item.height = 40;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("UnmanedBolt");
			item.shootSpeed = 4f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class UnmanedBow : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Bow");
			Tooltip.SetDefault("Converts wooden arrows into Novus arrows");
		}

		public override void SetDefaults()
		{
			item.damage = 16;
			item.ranged = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.UseSound = SoundID.Item5;
			item.shoot = ProjectileID.WoodenArrowFriendly;
			item.shootSpeed = 6f;
			item.useAmmo = AmmoID.Arrow;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();

			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = mod.ProjectileType("UnmanedArrow");
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class UnmanedSword : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Sword");
		}
		public override void SetDefaults()
		{
			item.damage = 18;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Blue;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class UnmanedShuriken : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Shuriken");
			Tooltip.SetDefault("Returns to the player like a boomerang on enemy hit");
		}

		public override void SetDefaults()
		{
			item.damage = 14;
			item.Throwing().thrown = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.knockBack = 1;
			item.value = 10;
			item.consumable = true;
			item.maxStack = 999;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.UseSound = SoundID.Item1;
			item.shoot = ModContent.ProjectileType<UnmanedShurikenProj>();
			item.shootSpeed = 10f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 16f;
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 1);
			recipe.AddIngredient(ItemID.Shuriken, 100);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}


	public class UnmanedShurikenProj : ModProjectile
	{

		double keepspeed = 0.0;
		float homing = 0.05f;
		public float beginhoming = 20f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Shuriken");
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedShuriken";

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 16;
			projectile.height = 16;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.thrown = false;
			projectile.penetrate = 3;
			projectile.Throwing().thrown = true;
			aiType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			Player owner = Main.player[projectile.owner];
			if (projectile.aiStyle == 3 && (owner.MountedCenter - projectile.Center).Length() < 64)
			{
				owner.QuickSpawnItem(ModContent.ItemType<UnmanedShuriken>(), 1);
				return true;
			}


			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("NovusSparkle"), projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Main.hslToRgb(0.4f, 0f, 0.15f), 2.4f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			projectile.type = ProjectileID.Shuriken;

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.friendly && projectile.aiStyle != 3)
			{
				projectile.aiStyle = 3;
				projectile.extraUpdates = 1;
				projectile.netUpdate = true;
			}
		}

		public override void AI()
		{
			for (int num315 = 0; num315 < (projectile.aiStyle == 3 ? 1 : 2); num315++)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("NovusSparkle"), 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}
		}
	}
}