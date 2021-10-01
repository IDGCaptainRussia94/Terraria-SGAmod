using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{	
	public class DropperItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dropper");
			Tooltip.SetDefault("Drops items sent into it via hoppers\nHammer the dropper to change its output direction\nDroppers can be disabled by sending a wire signal to them");
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
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 10);
			item.rare = 1;
			item.createTile = mod.TileType("DropperTile");
			item.placeStyle = 0;
			item.mech = true;
		}
        public override string Texture => "SGAmod/Items/Placeable/TechPlaceable/DropperItem";

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 3);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 3);
			recipe.AddIngredient(ItemID.IronCrate, 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
}