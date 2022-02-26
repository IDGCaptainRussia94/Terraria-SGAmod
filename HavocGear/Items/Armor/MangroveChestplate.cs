using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MangroveHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mangrove Helmet");
			Tooltip.SetDefault("5% increased throwing damage and 10% crit chance\n15% increased throwing velocity\n1% increased Throwing Apocalyptical Chance");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 50000;
			item.rare = 4;
			item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.SGAPly().apocalypticalChance[3] += 1.0;
			player.Throwing().thrownVelocity += 0.15f;
			player.Throwing().thrownDamage += 0.05f;
			player.Throwing().thrownCrit += 10;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 7);
			recipe.AddIngredient(null, "DankWoodHelm", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MangroveChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mangrove Chestplate");
			Tooltip.SetDefault("10% increased throwing damage\n33% to not consume thrown items\n1% increased Throwing Apocalyptical Chance");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 50000;
			item.rare = 4;
			item.defense = 12;
		}

		public override void UpdateEquip(Player player)
		{
			player.SGAPly().apocalypticalChance[3] += 1.0;
			player.Throwing().thrownCost33 = true;
			player.Throwing().thrownDamage += 0.07f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 11);
			recipe.AddIngredient(null, "DankWoodChest", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MangroveGreaves : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mangrove Greaves");
			Tooltip.SetDefault("6% increased throwing damage and 10% faster throwing item use speed\n20% faster movement speed\n1% increased Throwing Apocalyptical Chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.SGAPly().apocalypticalChance[3] += 1.0;
			player.SGAPly().ThrowingSpeed += 0.10f;
			player.Throwing().thrownDamage += 0.06f;
			player.maxRunSpeed += 0.20f;
			player.accRunSpeed += 0.25f;
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 50000;
			item.rare = 4;
			item.defense = 6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 9);
			recipe.AddIngredient(null, "DankLegs", 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

}