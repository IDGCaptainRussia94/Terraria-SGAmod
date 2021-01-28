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
			Tooltip.SetDefault("MC Hopper PH Text");
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
			item.value = Item.buyPrice(gold: 1);
			item.rare = 1;
			item.createTile = mod.TileType("HopperTile");
			item.placeStyle = 0;
		}
	}
}