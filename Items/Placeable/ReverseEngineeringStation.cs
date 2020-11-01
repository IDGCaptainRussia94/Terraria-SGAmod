using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;

namespace SGAmod.Items.Placeable
{
	public class ReverseEngineeringStation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reverse Engineering Station");
			Tooltip.SetDefault("Allows weaponization of unusual tidbits and crafting of advanced machinery\nSome formerly uncraftable items may be crafted here\nDoubles as a Tinkerer's Workbench");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 1;
			item.rare = ItemRarityID.Blue;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("ReverseEngineeringStation");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string s = "Not Binded!";
			foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
			{
				s = key;
			}

			tooltips.Add(new TooltipLine(mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Allows you to uncraft non-favorited held items on right click")));
			tooltips.Add(new TooltipLine(mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "Press the 'Toggle Recipe' (" + s + ") Hotkey to swap between recipes")));
			tooltips.Add(new TooltipLine(mod, "uncraft", Idglib.ColorText(Color.CornflowerBlue, "There is a net loss in materials on uncraft, this can be reduced")));
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.TinkerersWorkshop, 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 25);
			recipe.AddRecipeGroup("SGAmod:PressurePlates", 2);
			//recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddRecipeGroup("SGAmod:TechStuff", 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}