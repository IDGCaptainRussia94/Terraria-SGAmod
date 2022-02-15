using SGAmod.HavocGear.Items;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Items.Placeable.DankWoodFurniture
{
	public class DankWoodPlatform : ModItem
	{
		public override void SetStaticDefaults() 
		{
			Tooltip.SetDefault("It still smells funny...");
		}

		public override void SetDefaults()
		{
			item.width = 8;
			item.height = 10;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = TileType<Tiles.DankWoodFurniture.DankWoodPlatformTile>();
		}

		public override void AddRecipes() 
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemType<DankWood>());
			recipe.SetResult(this, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.AddRecipe();
		}
	}
}