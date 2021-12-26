using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAAAUThrowing;

namespace SGAmod.HavocGear.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	public class MangroveHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mangrove Helmet");
			Tooltip.SetDefault("15% increased throwing velocity\n10% increased throwing damage and crit chance\n1% increased Throwing Apocalyptical Chance");
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
			player.Throwing().thrownVelocity += 0.15f;
			player.Throwing().thrownDamage += 0.1f;
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
			Tooltip.SetDefault("33% to not consume thrown items\n8% increased throwing damage\n1% increased Throwing Apocalyptical Chance");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 50000;
			item.rare = 4;
			item.defense = 16;
		}

		public override void UpdateEquip(Player player)
		{
			player.SGAPly().apocalypticalChance[3] += 1.0;
			player.Throwing().thrownCost33 = true;
			player.Throwing().thrownDamage += 0.08f;
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
			Tooltip.SetDefault("20% faster throwing item use speed and movement speed\n7% increased throwing damage\n1% increased Throwing Apocalyptical Chance");
		}

		public override void UpdateEquip(Player player)
		{
			player.SGAPly().apocalypticalChance[3] += 1.0;
			player.SGAPly().ThrowingSpeed += 0.20f;
			player.SGAPly().ThrowingSpeed += 0.20f;
			player.Throwing().thrownDamage += 0.07f;
			player.maxRunSpeed += 0.6f;
			player.accRunSpeed += 0.20f;
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 50000;
			item.rare = 4;
			item.defense = 8;
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