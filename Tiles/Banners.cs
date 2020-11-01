using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SGAmod.Tiles
{
	public class Banners : ModTile
	{
		public static Dictionary<int,string> idToItem = new Dictionary<int, string>();
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 16 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 111;
			TileObjectData.addTile(Type);
			dustType = -1;
			disableSmartCursor = true;
            AddMapEntry(new Color(13, 88, 130));
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int style = frameX / 18;
			string item;
			if (idToItem.TryGetValue(style, out item))
			Item.NewItem(i * 16, j * 16, 16, 48, mod.ItemType(item+"Banner"));
		}

		public override void NearbyEffects(int i, int j, bool closer)
		{
			if (closer)
			{
				Player player = Main.player[Main.myPlayer];
				int style = Main.tile[i, j].frameX / 18;
				string type;
				if (idToItem.TryGetValue(style, out type))
				{
					player.NPCBannerBuff[mod.NPCType(type)] = true;
					player.hasBanner = true;
				}
			}
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
		}
	}
}