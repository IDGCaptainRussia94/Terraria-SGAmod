using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{	
	public class SiftingFunnelItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sifting Funnel");
			Tooltip.SetDefault("'Contains several layers of sieves meshes'\nExtracts items that pass through the funnel\nCan only process 5 items from a stack at a time\nRetains all previous functionality of a Hopper");
		}
        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemRarityID.LightRed;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 10);
			item.rare = ItemRarityID.LightRed;
			item.createTile = mod.TileType("ShiftingFunnelTile");
			item.placeStyle = 0;
		}
        public override string Texture => "SGAmod/Items/Placeable/TechPlaceable/SiftingFunnelItem";

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Extractinator, 1);
			recipe.AddIngredient(ModContent.ItemType<HopperItem>(), 1);
			recipe.AddIngredient(ItemID.MetalShelf, 10);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}