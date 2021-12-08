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
using Idglibrary;

namespace SGAmod.Tiles.TechTiles
{
	public class HopperTile : ModTile, IHopperInterface
	{
		const int maxXSize = 18 * 6;
		const int maxYSize = 18 * 4;
		protected virtual int DropItem => ModContent.ItemType<HopperItem>();
		public override void SetDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
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

			TileObjectData.newTile.AnchorWall = true;
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;
			if (GetType() == typeof(ChestHopperTile))
				TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<ChestHopperTE>().Hook_AfterPlacement, -1, 0, true);
			TileObjectData.addTile(Type);

			dustType = 7;
			disableSmartCursor = true;
			disableSmartInteract = true;
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
		public static Point GetRealHopperCorner(Point here, Tile tile)
		{
			Point coords = new Point(here.X - ((tile.frameX) % 36) / 18, here.Y - ((tile.frameY) % 36) / 18);
			return coords;

		}

		// This progression matches vanilla tiles, you don't have to follow it if you don't want. Some vanilla traps don't have 6 states, only 4. This can be implemented with different logic in Slope. Making 8 directions is also easily done in a similar manner.
		// We can use the Slope method to override what happens when this tile is hammered.

		public static Point[] tileDirection = { new Point(0, 2), new Point(-2, 0), new Point(2, 0) };

		public static void HandleItemHoppers(Item item)
		{
			if (item.active && item.velocity.Y == 0 && SGAWorld.modtimer % 30 == 0)
			{
				//Main.NewText("Debug Message!");
				Point tilePosition = new Point((int)(item.Center.X / 16), ((int)(item.Center.Y) / 16) + 2);
				if (WorldGen.InWorld(tilePosition.X, tilePosition.Y))
				{
					Tile tile = Framing.GetTileSafely(tilePosition.X, tilePosition.Y);
					//Main.NewText(tile.type + " this type "+item.position);
					if (tile.type == ModContent.TileType<HopperTile>() || tile.type == ModContent.TileType<ChestHopperTile>() || tile.type == ModContent.TileType<ShiftingFunnelTile>() || tile.type == ModContent.TileType<LiquidationHopperTile>())
					{

						if (ModContent.GetModTile(tile.type) is ModTile modTile)
						{
							if (modTile != null && modTile is HopperTile)
							{
								bool teststatus = false;
								(modTile as HopperTile).HopperMoveItem(item, tilePosition, 0, ref item.stack, ref teststatus);
							}
						}


						//MoveItem(item, tilePosition, 0,ref item.stack,ref teststatus);
					}
				}
			}
		}

		public virtual bool HopperMoveItem(Item item, Point tilePos, int movementCount, ref int remainingStack, ref bool testOnly)
        {
			return MoveItem(item, tilePos, movementCount + 1, ref remainingStack, ref testOnly);
		}

		public bool HopperInputItem(Item item, Point tilePos, int movementCount, ref bool testOnly )
		{
			return HopperMoveItem(item, tilePos, movementCount, ref item.stack,ref testOnly);
		}

		public bool HopperExportItem(ref Item item, Point tilePos, int movementCount, ref bool testOnly)
		{
			return false;
		}

		public static void CleanUpGlitchedItems()
        {
			foreach (Item itemxx in Main.item)
			{
				if (itemxx.IsAir)
				{
					itemxx.active = false;

				}
			}
		}

		public static bool UpgradeCoins(Item item,Chest chest)
        {
			if (item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin || item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin)
			{
				if (SGAUtils.UpdateCoins(chest, item.value))
				{
					item.TurnToAir();
					return true;
				}
			}
			return false;
			/*
			
			int typetoboost = -1;
			if (item.stack >= item.maxStack)
            {
				switch (item.type)
                {
					case ItemID.CopperCoin:
						typetoboost = ItemID.SilverCoin;
						goto upgradeLabel;

					case ItemID.SilverCoin:
						typetoboost = ItemID.GoldCoin;
						goto upgradeLabel;

					case ItemID.GoldCoin:
						typetoboost = ItemID.PlatinumCoin;
						goto upgradeLabel;

					default:
						break;
				}
            }
			return false;

		upgradeLabel:

			bool foundACoin = false;
			item.TurnToAir();

			for (int i = 0; i < Chest.maxItems; i += 1)
            {
				Item chestItem = chest.item[i];
				if (chestItem.type == typetoboost)
                {
					chestItem.stack += 1;
					chestItem.newAndShiny = true;
					UpgradeCoins(chestItem,chest);
					return true;
                }
            }

			//we make one if we can't find one
			for (int i = 0; i < Chest.maxItems; i += 1)
			{
				Item chestItem = chest.item[i];
				if (chestItem.IsAir)
                {
					Item coin = new Item();
					coin.SetDefaults(typetoboost);
					coin.stack = 1;
					coin.newAndShiny = true;

					chest.item[i] = coin;

					UpgradeCoins(chestItem, chest);
					return true;
				}

			}

				return false;
			*/
        }

		public static bool ExportFromChest(out Item item,out Point chestdata, Point checkCoords)
		{
			item = null;
			chestdata = new Point(-1, -1);
			int chester = Chest.FindChest(checkCoords.X, checkCoords.Y);
			int i=0;
			if (chester >= 0 && !Chest.isLocked(checkCoords.X, checkCoords.Y))
			{
				for (i = 0; i < Chest.maxItems; i++)
				{
					Item itemInChest = Main.chest[chester].item[i];
					if (itemInChest.IsAir)
					{
						continue;
					}
                    else
                    {
						item = itemInChest;
						goto foundItem;
					}
				}

				return false;

			foundItem:

				chestdata = new Point(chester, i);
				return true;
			}
			return false;
		}

		public static bool InputToChest(Item item, Point checkCoords, ref int remainingStack,bool testOnly=false)
		{
			Tile tile = Framing.GetTileSafely(checkCoords);

			if (item.IsConsumablePickup())
            {
				int item2 = Item.NewItem((checkCoords.ToVector2())*16, Vector2.Zero, item.type, item.stack);
				//Main.item[item2].TurnToAir();
				Main.item[item2] = item.Clone();
				Main.item[item2].favorited = false;
				Main.item[item2].newAndShiny = true;

				item.TurnToAir();

				HopperTile.CleanUpGlitchedItems();

				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
				NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item2);
				return true;
			}

			if (tile == null || !tile.active())
				return false;

			int chester = Chest.FindChest(checkCoords.X, checkCoords.Y);
			if (chester >= 0 && !Chest.isLocked(checkCoords.X, checkCoords.Y))
			{
				int emptyslot = -1;
				bool matchingType = false;
				int ammountleft = item.stack;
				int originalStacksize = item.stack;

				for (int i = 0; i < Chest.maxItems; i++)
				{
					Item itemInChest = Main.chest[chester].item[i];
					if (itemInChest != null && itemInChest.IsAir)
					{
						emptyslot = i;
						break;
					}
					if (itemInChest != null && itemInChest.type == item.type && item.maxStack > 1 && itemInChest.stack < itemInChest.maxStack)
					{
						ammountleft -= itemInChest.maxStack - itemInChest.stack;
						matchingType = true;
						emptyslot = i;
						break;

					}
				}

				if (emptyslot < 0)
					return false;

				if (testOnly)
					return true;


				if (!matchingType)
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						Item clonedItem = item.Clone();

						Main.chest[chester].item[emptyslot] = clonedItem;

						HopperTile.UpgradeCoins(Main.chest[chester].item[emptyslot], Main.chest[chester]);

						remainingStack = 0;
						item.TurnToAir();
						NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
					}
				}
				else
				{
					if (Main.netMode != NetmodeID.MultiplayerClient)
					{
						int beforeAdd = Main.chest[chester].item[emptyslot].stack;
						Main.chest[chester].item[emptyslot].stack += item.stack;
						Main.chest[chester].item[emptyslot].stack = System.Math.Min(Main.chest[chester].item[emptyslot].stack, Main.chest[chester].item[emptyslot].maxStack);
						int difference = (beforeAdd + originalStacksize) - 100;

						int itembefore = item.type;

						HopperTile.UpgradeCoins(Main.chest[chester].item[emptyslot],Main.chest[chester]);

						/*if (difference > 0)
						{
							Item leftOvers = new Item();
							leftOvers.SetDefaults(item.type);
							leftOvers.stack = difference;

							InputToChest(leftOvers, checkCoords,ref leftOvers.stack);
							ammountleft = difference;
						}*/

						//NetMessage.SendData(MessageID.SyncItem, -1, -1, null, Main.chest[chester].item[emptyslot].whoAmI);

					}
					if (ammountleft > 0)
					{
						item.stack = ammountleft;
						remainingStack = ammountleft;
						if (Main.netMode != NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
					}
					else
					{
						remainingStack = 0;
						item.TurnToAir();
						if (Main.netMode != NetmodeID.MultiplayerClient)
							NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
					}
				}

				HopperTile.CleanUpGlitchedItems();

				SGAUtils.ForceUpdateChestsForPlayers();
				return true;
			}
			return false;
		}

		public static bool MoveItem(Item item, Point tilePos, int movementCount,ref int remainingStack,ref bool testOnly)
		{
			if (movementCount >= 100)
				return false;

			//Main.NewText("Debug Message 2!");
			Tile tile = Framing.GetTileSafely(tilePos.X, tilePos.Y);

			if ((tile.frameY / 36) % 2 > 0)
				return false;

			Point coords = GetRealHopperCorner(tilePos, tile);
			Point offset = tileDirection[tile.frameX / 36];

			Point checkCoords = new Point(coords.X + offset.X, coords.Y + offset.Y);

			Tile modtile = Framing.GetTileSafely(checkCoords.X, checkCoords.Y);

			if (ModContent.GetModTile(modtile.type) is ModTile modTile)
			{
				if (modTile != null && modTile is IHopperInterface)
				{
					return (modTile as IHopperInterface).HopperInputItem(item, checkCoords, movementCount + 1, ref testOnly);
				}
			}

			return InputToChest(item, checkCoords,ref remainingStack, testOnly);
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
			Point coords = GetRealHopperCorner(new Point(i, j), tile);

			SGAmod.Instance.Logger.Warn("Tile xy: " + (i) + " - " + (j));
			for (int x = 0; x < 3; x += 1)
			{
				for (int y = 1; y < 4; y += 1)
				{
					SGAmod.Instance.Logger.Warn("Tile here: "+(coords.X + x) +" - "+(coords.Y - y));
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
			Point coords = GetRealHopperCorner(new Point(i, j), tile);

			if (tile.type == ModContent.TileType<ChestHopperTile>())
				ModContent.GetInstance<ChestHopperTE>().Kill(coords.X, coords.Y);

			if (item > 0)
			{
				if (((tile.frameX) % 36) / 18 == 0 && ((tile.frameY) % 36) / 18 == 0)
					Item.NewItem(coords.X * 16, coords.Y * 16, 32, 32, item);
			}

		}

	}

	public class ChestHopperTile : HopperTile, IHopperInterface
	{
		const int maxXSize = 18 * 6;
		const int maxYSize = 18 * 4;
		protected override int DropItem => ModContent.ItemType<HopperChestItem>();
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Tiles/TechTiles/HopperTile";
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Chest Hopper");
			AddMapEntry(new Color(150, 105, 85), name);
		}

		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (Main.tile[x, y].type == base.Type)
			{
				if (nextSpecialDrawIndex < Main.specX.Length)
				{
					Main.specX[nextSpecialDrawIndex] = x;
					Main.specY[nextSpecialDrawIndex] = y;
					nextSpecialDrawIndex += 1;
				}
			}
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Tile tile = Framing.GetTileSafely(i, j);
			Texture2D curser = Main.cursorTextures[7];
			if (Main.tile[i, j].type == base.Type)
			{
				if (tile.frameX%36 == 0 && tile.frameY%36 == 0)
				{
					Vector2 offset = zerooroffset + (new Vector2(i, j) * 16) + new Vector2(16, 16);
					spriteBatch.Draw(curser, offset - Main.screenPosition, null, (tile.frameY > 0 ? Color.Gray : Color.White).MultiplyRGBA(Lighting.GetColor(i, j)), 0, curser.Size() / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0f);
				}
			}
		}
	}

	public class ChestHopperTE : ModTileEntity
	{
		public Item heldItem;
		public int updateTimer = 0;

		public ChestHopperTE()
		{
			updateTimer = 0;
		}

		public override void OnKill()
		{
			//stuff
		}

		public bool TileCheck(int i, int j)
		{
			Tile tile = Framing.GetTileSafely(i, j);

			bool valid = tile.active() && tile.type == ModContent.TileType<ChestHopperTile>();
			if (!valid)
			{
				LuminousAlterTE.DebugText("Deleted");
			}
			return valid;
		}

		public override void Update()
		{
			updateTimer += 1;
			if (updateTimer % 60 == 0)
            {
				Point coords = new Point(Position.X, Position.Y);
				Tile mymodtile = Framing.GetTileSafely(coords.X, coords.Y);
				Point ChestData = new Point(-1,-1);

				if (mymodtile.frameY < 36)
				{
					Point testcoords = new Point(coords.X, coords.Y - 2);
					Tile modtile = Framing.GetTileSafely(testcoords.X, testcoords.Y);
					Item exporteditem = null;
					Item clonedItem = null;
					bool checkIfOnlyWeAreTesting = false;

					if (ModContent.GetModTile(modtile.type) is ModTile modTile)
					{
						if (modTile != null && modTile is IHopperInterface)
						{
							if ((modTile as IHopperInterface).HopperExportItem(ref exporteditem, testcoords, 0,ref checkIfOnlyWeAreTesting))
							{
								if (!checkIfOnlyWeAreTesting)
								{
									clonedItem = exporteditem.Clone();
									goto DoExport;
								}
							}
						}
					}

					if (HopperTile.ExportFromChest(out exporteditem, out ChestData, testcoords))
					{
						clonedItem = exporteditem.Clone();
						goto DoExport;
					}

					return;

				DoExport:

					int remainingStack = clonedItem != null ? clonedItem.stack : 0;
					bool testOnly = false;
					if (clonedItem != null && HopperTile.MoveItem(exporteditem, coords, 0, ref remainingStack, ref testOnly))
					{
						LuminousAlterTE.DebugText("Yet More Test: ChestData is " + ChestData);

						if (ChestData.X >= 0 && !testOnly)
						{
							LuminousAlterTE.DebugText("Remaining stacks " + remainingStack);

							if (Main.netMode != NetmodeID.MultiplayerClient)
							{
								if ((Main.chest[ChestData.X].item[ChestData.Y].stack - remainingStack) > 0)
									Main.chest[ChestData.X].item[ChestData.Y].stack -= (Main.chest[ChestData.X].item[ChestData.Y].stack-remainingStack);// remainingStack>2 ? remainingStack+1 : remainingStack;
								else
									Main.chest[ChestData.X].item[ChestData.Y].TurnToAir();
							}

							if (remainingStack<1)
							exporteditem.TurnToAir();

							if (Main.netMode != NetmodeID.SinglePlayer)
							{
								NetMessage.SendData(MessageID.SyncItem, -1, -1, null, exporteditem.whoAmI);
								NetMessage.SendData(MessageID.SyncItem, -1, -1, null, clonedItem.whoAmI);
							}

							SGAUtils.ForceUpdateChestsForPlayers();
						}

						return;
					}

				}

			}

		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{

			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				NetMessage.SendTileSquare(Main.myPlayer, i + 1, j + 1, 4);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
				return -1;
			}

			int num = Place(i, j);
			LuminousAlterTE.DebugText("Placed");
			return num;
		}

		public override bool ValidTile(int i, int j)
		{
			return TileCheck(i, j);
		}

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			writer.Write(updateTimer);
		}

		/*public override void NetReceive(BinaryReader reader, bool lightReceive)
		{
			updateTimer = reader.Read();
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound
			{
				{"updateTimer",updateTimer }
			};
			return tag;

		}
		public TagCompound DoLoad(TagCompound tag)
		{
			updateTimer = tag.GetInt("updateTimer");
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			DoLoad(tag);
		}*/

	}

}