using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class HopperItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hopper");
			Tooltip.SetDefault("Picks up items that fall onto its top, can be placed mid-air\nHoppers place items into chests and other chained Hoppers\nHammer the hopper to change its output position\nHoppers can be disabled by sending a wire signal to themm");
		}
        public override string Texture => "Terraria/Item_"+ItemID.JungleKeyMold;
        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 10);
			item.rare = 1;
			item.createTile = mod.TileType("HopperTile");
			item.placeStyle = 0;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 4);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 4);
			recipe.AddIngredient(ItemID.Chest, 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}