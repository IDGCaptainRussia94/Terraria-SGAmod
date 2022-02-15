using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using SGAmod.Items.Placeable.DankWoodFurniture;

namespace SGAmod.Tiles.DankWoodFurniture
{
	public class DankWoodWall : ModWall
	{
		public override void SetDefaults() {
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<Items.Placeable.DankWoodFurniture.DankWoodWall>();
			AddMapEntry(new Color(41, 31, 23));
		}
	}
	public class DankWoodFence : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<Items.Placeable.DankWoodFurniture.DankWoodFence>();
			AddMapEntry(new Color(49, 41, 26));
		}
	}
	public class BrokenDankWoodFence : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			drop = ModContent.ItemType<Items.Placeable.DankWoodFurniture.BrokenDankWoodFence>();
			AddMapEntry(new Color(31, 33, 13));
		}
	}
}