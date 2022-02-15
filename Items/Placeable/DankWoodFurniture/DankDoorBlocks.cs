using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.HavocGear.Items;
using SGAmod.Items.Placeable;
using System.Linq;

namespace SGAmod.HavocGear.Items
{
	public class DankDoorItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Confuses hostile mobs when hit due to the released stench");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 28;
			item.maxStack = 99;
			item.rare = 1;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 150;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankDoorClosed>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenDoor);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 15);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

}