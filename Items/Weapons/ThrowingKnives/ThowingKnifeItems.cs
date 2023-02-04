using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.ThrowingKnives
{
	public class FrostburnKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/FrostburnKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Frostburn for 4 seconds");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 11f;
			item.damage = 13;
			item.value = Item.sellPrice(copper: 12);
			item.shoot = ModContent.ProjectileType<FrostburnKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 50);
			recipe.AddIngredient(ModContent.ItemType<FrigidShard>(), 1);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
	public class AcidKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/AcidKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Acid Burn for 5 seconds");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 12f;
			item.damage = 20;
			item.crit = 4;
			item.knockBack = 2.5f;
			item.rare = ItemRarityID.Green;
			item.value = Item.sellPrice(copper: 20);
			item.shoot = ModContent.ProjectileType<AcidKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 75);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 75);
			recipe.AddRecipe();
		}
	}
	public class CursedKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/CursedKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Cursed Inferno for 6 seconds");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 13f;
			item.damage = 32;
			item.crit = 4;
			item.knockBack = 3f;
			item.autoReuse = true;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(copper: 35);
			item.shoot = ModContent.ProjectileType<CursedKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 75);
			recipe.AddIngredient(ItemID.CursedFlame, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 75);
			recipe.AddRecipe();
		}
	}
	public class IchorKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/IchorKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Ichor for 6 seconds");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 13f;
			item.damage = 32;
			item.crit = 4;
			item.knockBack = 3f;
			item.autoReuse = true;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(copper: 35);
			item.shoot = ModContent.ProjectileType<IchorKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 75);
			recipe.AddIngredient(ItemID.Ichor, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 75);
			recipe.AddRecipe();
		}
	}
	public class BlazingKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/BlazingKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Thermal Blaze for 6 seconds");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 13f;
			item.damage = 32;
			item.crit = 4;
			item.knockBack = 3f;
			item.autoReuse = true;
			item.rare = ItemRarityID.LightRed;
			item.value = Item.sellPrice(copper: 35);
			item.shoot = ModContent.ProjectileType<BlazingKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 75);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.FieryShard>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 75);
			recipe.AddRecipe();
		}
	}
	public class DankKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/DankKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Dank Slow for 5 seconds\nKnives bounce off tiles");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 13f;
			item.damage = 38;
			item.crit = 4;
			item.knockBack = 3f;
			item.useAnimation = 13;
			item.useTime = 13;
			item.autoReuse = true;
			item.rare = ItemRarityID.Pink;
			item.value = Item.sellPrice(copper: 45);
			item.shoot = ModContent.ProjectileType<DankKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 50);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.VirulentOre>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 200);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.VirulentBar>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
	}
	public class VenomKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/VenomKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Venom for 7 seconds\nKnives bounce off tiles");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 14f;
			item.damage = 48;
			item.crit = 4;
			item.knockBack = 3.5f;
			item.useAnimation = 11;
			item.useTime = 11;
			item.height = 26;
			item.autoReuse = true;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(copper: 55);
			item.shoot = ModContent.ProjectileType<VenomKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 100);
			recipe.AddIngredient(ItemID.VialofVenom, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class NanoKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/NanoKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Confusion for 4 seconds\nKnives smartly bounce to nearby enemies");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 16f;
			item.damage = 54;
			item.crit = 4;
			item.knockBack = 3.5f;
			item.useAnimation = 12;
			item.useTime = 12;
			item.height = 26;
			item.autoReuse = true;
			item.rare = ItemRarityID.Lime;
			item.value = Item.sellPrice(copper: 55);
			item.shoot = ModContent.ProjectileType<NanoKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 100);
			recipe.AddIngredient(ItemID.Nanites, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
	public class SolarKnife : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/SolarKnife";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Daybroken for 8 seconds\nKnives bounce and explode on their last hit");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 16f;
			item.damage = 100;
			item.crit = 8;
			item.knockBack = 5f;
			item.useAnimation = 8;
			item.useTime = 8;
			item.width = 18;
			item.height = 30;
			item.autoReuse = true;
			item.rare = ItemRarityID.Red;
			item.value = Item.sellPrice(silver: 1);
			item.shoot = ModContent.ProjectileType<SolarKnifeProjectile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 250);
			recipe.AddIngredient(ItemID.FragmentSolar, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 250);
			recipe.AddRecipe();
		}
	}
	public class StarMetalKnives : ThrowingKnifeBaseItem
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/StarMetalKnives";
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Moon Light Curse for 2 seconds\nThrows two knives\nKnives smartly bounce to nearby enemies");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			item.shootSpeed = 16f;
			item.damage = 130;
			item.crit = 8;
			item.knockBack = 5.5f;
			item.useAnimation = 8;
			item.useTime = 8;
			item.width = 34;
			item.height = 24;
			item.autoReuse = true;
			item.rare = ItemRarityID.Purple;
			item.value = Item.sellPrice(silver: 1, copper: 50);
			item.shoot = ModContent.ProjectileType<StarMetalKnivesProjectile>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 2;// 2 shots
			float rotation = MathHelper.ToRadians(5);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 500);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 500);
			recipe.AddRecipe();
		}
	}
}