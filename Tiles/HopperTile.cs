using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Items.Placeable;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;

namespace SGAmod.Tiles
{
	public class HopperTile : ModTile, IHopperInterface
	{
		const int maxXSize = 18 * 6;
		const int maxYSize = 18 * 4;
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

			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			TileObjectData.addTile(Type);

			dustType = 7;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hopper");
			AddMapEntry(new Color(150, 105, 85), name);
		}

		// PlaceInWorld is needed to facilitate styles and alternates since this tile doesn't use a TileObjectData. Placing left and right based on player direction is usually done in the TileObjectData, but the specifics of that didn't work for how we want this tile to work. 
		/*public override void PlaceInWorld(int i, int j, Item item)
		{
			Tile tile = Main.tile[i, j];
			tile.frameX = 0;
			tile.frameY = 0;
			if (Main.netMode == 1)
			{
				NetMessage.SendTileSquare(-1, Player.tileTargetX, Player.tileTargetY, 1, TileChangeType.None);
			}
		}*/


		// - ((tile.frameX / 36) % 2)
		public static Point GetRealHopperCorner(Point here,Tile tile)
        {
			Point coords = new Point(here.X - ((tile.frameX)%36) / 18, here.Y - (tile.frameY / 18));
			return coords;

		}

		// This progression matches vanilla tiles, you don't have to follow it if you don't want. Some vanilla traps don't have 6 states, only 4. This can be implemented with different logic in Slope. Making 8 directions is also easily done in a similar manner.
		// We can use the Slope method to override what happens when this tile is hammered.

		public static Point[] tileDirection = { new Point(0, 2), new Point(-2, 0), new Point(2, 0) };

		public static void HandleItemHoppers(Item item)
		{
			if (item.velocity.Y == 0 && SGAWorld.modtimer % 30 == 0)
			{
				Main.NewText("Debug Message!");
				Point tilePosition = new Point((int)(item.Center.X/16), ((int)(item.Center.Y)/16)+2);
				Tile tile = Framing.GetTileSafely(tilePosition.X, tilePosition.Y);
				Main.NewText(tile.type + " this type "+item.position);
				if (tile.type == ModContent.TileType<HopperTile>())
				{
					MoveItem(item, tilePosition,0);
				}
			}
		}

		public bool HopperInputItem(Item item, Point tilePos, int movementCount)
        {
			return MoveItem(item, tilePos, movementCount + 1);
		}

		public static bool MoveItem(Item item, Point tilePos,int movementCount)
		{
			if (movementCount >= 100)
				return false;

			Main.NewText("Debug Message 2!");
			Tile tile = Framing.GetTileSafely(tilePos.X, tilePos.Y);
			Point coords = GetRealHopperCorner(tilePos, tile);
			Point offset = tileDirection[tile.frameX / 36];
			Point checkCoords = new Point(coords.X + offset.X, coords.Y + offset.Y);

			Tile modtile = Framing.GetTileSafely(checkCoords.X, checkCoords.Y);

			if (ModContent.GetModTile(modtile.type) is ModTile modTile)
            {
				if (modTile!=null && modTile is IHopperInterface)
				{
					return (modTile as IHopperInterface).HopperInputItem(item, checkCoords, movementCount + 1);
				}
            }

			int chester = Chest.FindChest(checkCoords.X, checkCoords.Y);
			if (chester >= 0)
			{

				int emptyslot = -1;
				bool matchingType = false;
				int ammountleft = item.stack;
				for (int i = 0; i < 40; i++)
				{
					Item itemInChest = Main.chest[chester].item[i];
					if (itemInChest.IsAir)
					{
						emptyslot = i;
						break;
					}
					if (itemInChest.type == item.type && item.maxStack>1 && item.stack < item.maxStack)
                    {
						ammountleft -= item.maxStack;
						matchingType = true;
						emptyslot = i;
						break;

					}
				}

				if (emptyslot < 0)
					return false;


				if (!matchingType)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Item clonedItem = item.DeepClone();

						Main.chest[chester].item[emptyslot] = clonedItem;
						item.TurnToAir();
					}
                }
                else
                {
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Main.chest[chester].item[emptyslot].stack += item.stack;
						Main.chest[chester].item[emptyslot].stack = System.Math.Min(Main.chest[chester].item[emptyslot].stack, Main.chest[chester].item[emptyslot].maxStack);
					}
					if (ammountleft > 0)
					{
						item.stack = ammountleft;
						if (Main.netMode != NetmodeID.SinglePlayer)
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
					}
                    else
                    {
						item.TurnToAir();
					}
				}
				SGAUtils.ForceUpdateChestsForPlayers();
				return true;
			}

			return false;
		}
		public override bool Slope(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealHopperCorner(new Point(i, j), tile);

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
			Point coords = GetRealHopperCorner(new Point(i, j), tile);

			for (int x = 0; x < 2; x += 1)
			{
				for (int y = 0; y < 2; y += 1)
				{
					Tile corner = Framing.GetTileSafely(coords.X + x, coords.Y + y);
					corner.frameY = (short)((corner.frameX + 36) % maxYSize);
				}
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(-1, coords.X, coords.Y, 2, TileChangeType.None);
			}

		}

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealHopperCorner(new Point(i, j), tile);

			for (int x = 0; x < 2; x += 1)
			{
				if (Chest.FindChest(coords.X+i, coords.Y - 2) >= 0)
				{
					return false;
				}
			}

			return true;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			int item = ModContent.ItemType<HopperItem>();
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealHopperCorner(new Point(i, j), tile);

			if (item > 0)
			{
				if (((tile.frameX) % 36) / 18 == 0 && ((tile.frameY) % 36) / 18 == 0)
				Item.NewItem(coords.X * 16, coords.Y * 16, 32, 32, item);
			}
			
		}
	}
}