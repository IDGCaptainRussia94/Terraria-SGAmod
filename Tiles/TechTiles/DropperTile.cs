using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Items.Placeable;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader.IO;
using System.IO;
using SGAmod.Items.Placeable.TechPlaceable;

namespace SGAmod.Tiles.TechTiles
{
	public class DropperTile : ModTile, IHopperInterface
	{
		const int maxXSize = 18 * 8;
		const int maxYSize = 18 * 4;
		protected int DropItem => ModContent.ItemType<DropperItem>();
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			Main.tileLavaDeath[Type] = false;
			Main.tileSolid[Type] = true;
			//Main.tileSolidTop[Type] = true;
			Main.tileFrameImportant[Type] = true;
			//Main.tileSolid[Type] = true; // TODO: tModLoader hook for allowing non solid tiles to be hammer-able.

			//TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.DrawYOffset = 0;

			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.addTile(Type);

			dustType = 7;
			disableSmartCursor = true;
			disableSmartInteract = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Dropper");
			AddMapEntry(new Color(150, 105, 85), name);
		}

		// This progression matches vanilla tiles, you don't have to follow it if you don't want. Some vanilla traps don't have 6 states, only 4. This can be implemented with different logic in Slope. Making 8 directions is also easily done in a similar manner.
		// We can use the Slope method to override what happens when this tile is hammered.

		public static Point[] tileDirection = { new Point(2, 0), new Point(0, -2), new Point(-2, 0), new Point(0, 2) };

		public static Point GetRealDropperCorner(Point here, Tile tile)
		{
			Point coords = new Point(here.X - ((tile.frameX) % 36) / 18, here.Y - ((tile.frameY) % 36) / 18);
			return coords;
		}

		public bool HopperInputItem(Item item, Point tilePos, int movementCount)
		{
			return DropperDropItem(item, tilePos);
		}

		public bool HopperExportItem(ref Item item, Point tilePos, int movementCount)
		{

			return false;
		}

		public static bool DropperDropItem(Item item, Point tilePos)
		{

			Tile tile = Framing.GetTileSafely(tilePos.X, tilePos.Y);

			if ((tile.frameY / 36) % 2 > 0)
				return false;


			Point coords = GetRealDropperCorner(tilePos, tile);
			Vector2 offset = tileDirection[tile.frameX / 36].ToVector2();

			Vector2 SpitFrom = ((coords).ToVector2() * 16) + (offset * 16) + (Vector2.One * 0);


			int item2 = Item.NewItem(Vector2.Zero, Vector2.Zero, item.type, item.stack);
			Main.item[item2].TurnToAir();
			Main.item[item2] = item.DeepClone();
			Main.item[item2].favorited = false;
			Main.item[item2].newAndShiny = false;
			Main.item[item2].position = SpitFrom;
			Main.item[item2].velocity = offset * 2;

			item.TurnToAir();

			HopperTile.CleanUpGlitchedItems();

			NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
			NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item2);

			return true;
		}
		

		public override bool Slope(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealDropperCorner(new Point(i, j), tile);

			for (int x = 0; x < 2; x += 1)
			{
				for (int y = 0; y < 2; y += 1)
				{
					Tile corner = Framing.GetTileSafely(coords.X + x, coords.Y + y);
					corner.frameX = (short)((corner.frameX + 36) % maxXSize);
				}
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(-1, coords.X, coords.Y, 2, TileChangeType.None);
			}
			return false;
		}

		public override void HitWire(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealDropperCorner(new Point(i, j), tile);

			for (int x = 0; x < 2; x += 1)
			{
				for (int y = 0; y < 2; y += 1)
				{
					Tile corner = Framing.GetTileSafely(coords.X + x, coords.Y + y);
					corner.frameY = (short)((corner.frameY + 36) % maxYSize);
				}
			}
			if (Main.netMode == NetmodeID.Server)
			{
				NetMessage.SendTileSquare(-1, coords.X, coords.Y, 2, TileChangeType.None);
			}
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealDropperCorner(new Point(i, j), tile);

			SGAmod.Instance.Logger.Warn("Tile xy: " + (i) + " - " + (j));
			for (int x = 0; x < 3; x += 1)
			{
				for (int y = 1; y < 4; y += 1)
				{
					SGAmod.Instance.Logger.Warn("Tile here: " + (coords.X + x) + " - " + (coords.Y - y));
					Tile tilehere = Framing.GetTileSafely(coords.X + x, coords.Y - y);
					ModTile modtile = TileLoader.GetTile(tilehere.type);
					if (Chest.FindChestByGuessing(coords.X + x, coords.Y - y) >= 0 || tilehere.type == TileID.Containers || (modtile != null && modtile.chest != ""))
					{
						return false;
					}
				}
			}

			return true;
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail)
				return;

			int item = DropItem;
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealDropperCorner(new Point(i, j), tile);

			if (item > 0)
			{
				if (((tile.frameX) % 36) / 18 == 0 && ((tile.frameY) % 36) / 18 == 0)
					Item.NewItem(coords.X * 16, coords.Y * 16, 32, 32, item);
			}

		}


		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Tile tile = Framing.GetTileSafely(i, j);
			if (Main.tile[i, j].type == base.Type)
			{
				Point coords = GetRealDropperCorner(new Point(i, j), tile);
				if (coords.X == i && coords.Y == j)
				{

					Texture2D inner = Main.tileTexture[Type];
					Rectangle machineRect = new Rectangle(0,0, 32, 32);
					Rectangle arrowRect = new Rectangle(32, 0, 32, 32);
					Vector2 offset = zerooroffset + (new Vector2(i, j) * 16);

					Vector2 offsetRotation = tileDirection[tile.frameX / 36].ToVector2();
					Color onOff = (tile.frameY % 72) == 0 ? Color.Lime : Color.Red;

					spriteBatch.Draw(inner, offset - Main.screenPosition, machineRect, Lighting.GetColor(i,j,Color.White), 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
					spriteBatch.Draw(inner, offset+ Vector2.One * 16 - Main.screenPosition, arrowRect, onOff, offsetRotation.ToRotation(), Vector2.One*16, Vector2.One, SpriteEffects.None, 0f);

				}
			}


			return false;
		}

	}
}