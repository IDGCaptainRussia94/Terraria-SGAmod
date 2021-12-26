using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class SwampWall : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Debug: Swamp Wall");
			Tooltip.SetDefault("Use this incase your Dank Shrines get messed up to fix them");
		}
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 7;
			item.useStyle = 1;
			item.consumable = true;
			item.createWall = mod.WallType("SwampWall");
		}
	}

		public class SwampWoodWall : ModItem
		{
			public override void SetDefaults()
			{

				item.width = 12;
				item.height = 12;
				item.maxStack = 999;
				item.useTurn = true;
				item.autoReuse = true;
				item.useAnimation = 15;
				item.useTime = 7;
				item.useStyle = 1;
				item.consumable = true;
				item.createWall = mod.WallType("SwampWoodWall");
			}

			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Dankwood Wall");
				Tooltip.SetDefault("");
			}

			public override void AddRecipes()
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(null, "DankWood");
				recipe.SetResult(this, 4);
				recipe.AddRecipe();
			}
		}
	
}
