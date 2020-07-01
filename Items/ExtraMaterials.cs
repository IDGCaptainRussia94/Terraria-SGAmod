using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items
{
	public class BrokenAlter : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Ancient Alter");
			Tooltip.SetDefault("A strange device, it seems to still be functional\nMaybe I could repair it to full power...");
		}
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 14;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.sellPrice(0, 12, 0, 0);
			item.rare = 10;
			item.createTile = mod.TileType("BrokenAlter");
		}
		public override string Texture
        {
            get { return "SGAmod/Tiles/BrokenAlter"; }
        }
	}

}
