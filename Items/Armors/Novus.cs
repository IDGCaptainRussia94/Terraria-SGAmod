using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors
{

	[AutoloadEquip(EquipType.Head)]
	public class UnmanedHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Hood");
			Tooltip.SetDefault("5% faster item use times");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 2;
			item.defense=2;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.UseTimeMul+=0.05f;
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

	[AutoloadEquip(EquipType.Body)]
	public class UnmanedBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Breastplate");
			Tooltip.SetDefault("5% increased crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 2;
			item.defense=4;
		}
		public override void UpdateEquip(Player player)
		{
			player.meleeCrit += 5;
			player.rangedCrit += 5;
			player.magicCrit += 5;
			player.thrownCrit += 5;
		}		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class UnmanedLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Leggings");
			Tooltip.SetDefault("5% increased movement speed\n10% increased acceleration and max running speed");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 2;
			item.defense=2;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 1.05f;
			player.accRunSpeed *= 1.1f;
			player.maxRunSpeed *= 1.1f;
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


}