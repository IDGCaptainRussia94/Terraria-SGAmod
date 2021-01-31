using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;
using System;

namespace SGAmod.Items.Placeable
{
	public class LuminousAlter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Alter");
			Tooltip.SetDefault("When exposed to a clear night sky: transmutes items with Starlight\nConverts items into other items over a period of time\nCan corrupt specific items instead when the moon turns red");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 1;
			item.rare = ItemRarityID.Cyan;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("LuminousAlter");
		}
		public static Color AuroraLineColor
        {
            get
            {
				Color rainbowColor = Main.hslToRgb((Main.GlobalTime * 0.085f) % 1f, 1f, 0.75f);
				return Color.Lerp(rainbowColor, Color.Lerp(Color.Pink, Color.Aqua, 0.5f + (float)Math.Sin(Main.GlobalTime * 0.735f)), 0.5f + (float)Math.Sin(Main.GlobalTime * 0.375f));
			}
        }
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					Color rainbowColor = Main.hslToRgb((Main.GlobalTime * 0.085f) % 1f, 1f, 0.75f);
					line.overrideColor = AuroraLineColor;
				}
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(mod.ItemType("AuroraTear"), 1);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 12);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}