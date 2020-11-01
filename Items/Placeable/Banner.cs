using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Tiles;
using Idglibrary;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Items.Placeable
{
	public class SGABanner : ModItem
	{
		/*public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (" + Name2[0] + ")");
			Tooltip.SetDefault(Idglib.ColorText(Color.PaleTurquoise, "'" + Name2[1] + "'") + Idglib.ColorText(Color.PaleGoldenrod, " : Composed by " + Name2[2]));
		}*/

		/// </summary>

		public override bool CloneNewInstances => true;

		private int placeStyle;

		public SGABanner(string internalname)
		{
			placeStyle = Banners.idToItem.Count;
			Banners.idToItem.Add(Banners.idToItem.Count, internalname + "Banner");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 24;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 1;
			item.value = Item.buyPrice(0, 0, 10, 0);
			item.createTile = mod.TileType("Banners");
			item.placeStyle = placeStyle;
		}
		public override bool Autoload(ref string name)
		{
			return false;
		}
	}
}