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
using System;

namespace SGAmod.Tiles.TechTiles
{

	public class LiquidationHopperTile : HopperTile, IHopperInterface
	{
		protected override int DropItem => ModContent.ItemType<LiquidationHopperItem>();
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Tiles/TechTiles/HopperTile";
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			dustType = 57;
			disableSmartCursor = true;
			disableSmartInteract = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Liquidation Hopper");
			AddMapEntry(Color.Goldenrod, name);
		}

		public override bool HopperMoveItem(Item item, Point tilePos, int movementCount, ref int remainingStack, ref bool testOnly)
		{

			if (testOnly)
				return MoveItem(item, tilePos, movementCount + 1, ref remainingStack, ref testOnly);

			bool testIfWeCanDoIt = true;
			bool finalTransfer = true;

			for (int i = 0; i < 1; i += 1)
			{
				Item coins = new Item();
				coins.SetDefaults(ItemID.GoldCoin);
				coins.stack = Math.Min(item.stack, 5);

				finalTransfer &= MoveItem(coins, tilePos, movementCount + 1, ref remainingStack, ref testIfWeCanDoIt);
			}

			if (!finalTransfer)
				return false;

			int stackMax = Math.Min(item.stack, 10);

			for (int i = 0; i < stackMax; i += 1)
			{
				Item coins = new Item();
				int[] worth = SGAUtils.GetCoins((item.value / 10)* stackMax);

				int[] typeofcoin = {ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin };
				bool dontTestCoins = false;

				for (int cointype = 0; cointype < 4; cointype += 1)
				{
					if (worth[cointype] < 1)
						continue;

					coins.SetDefaults(typeofcoin[cointype]);
					coins.stack = worth[cointype];
					coins.newAndShiny = true;

					MoveItem(coins, tilePos, movementCount + 1, ref coins.stack, ref dontTestCoins);
				}

				//int stacksize = 1;

				remainingStack -= 1;

				//item.stack -= 1;

			}
			return false;
		}

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Tile tile = Framing.GetTileSafely(i, j);
			Texture2D tex = Main.tileTexture[tile.type];

			Vector2 offset = zerooroffset + (new Vector2(i, j) * 16);
			spriteBatch.Draw(tex, offset - Main.screenPosition, new Rectangle(tile.frameX, tile.frameY,16,16), (Color.Yellow.MultiplyRGBA(Lighting.GetColor(i,j)))*1f, 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
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
			Texture2D coinTexture = Main.coinTexture[2];
			int texHeight = coinTexture.Height / 8;
			Rectangle rect = new Rectangle(0, (int)((Main.GlobalTime*8) % 8) * texHeight, coinTexture.Width, texHeight);

			if (Main.tile[i, j].type == base.Type)
			{
				if (tile.frameX % 36 == 0 && tile.frameY == 0)
				{
					Vector2 offset = zerooroffset + (new Vector2(i, j) * 16) + new Vector2(16, 16);
					spriteBatch.Draw(coinTexture, offset - Main.screenPosition, rect, Color.White.MultiplyRGBA(Lighting.GetColor(i, j)), 0, new Vector2(coinTexture.Width,texHeight)/2, new Vector2(1f, 1f), SpriteEffects.None, 0f);
				}
			}
		}
	}

	public class ShiftingFunnelTile : HopperTile, IHopperInterface
	{
		protected override int DropItem => ModContent.ItemType<SiftingFunnelItem>();
		public override void SetDefaults()
		{
			base.SetDefaults();
			dustType = 7;
			disableSmartCursor = true;
			disableSmartInteract = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Sifting Funnel");
			AddMapEntry(new Color(150, 105, 85), name);
		}

		public override bool HopperMoveItem(Item item, Point tilePos, int movementCount,ref int remainingStack,ref bool testOnly)
		{

			if (ItemID.Sets.ExtractinatorMode[item.type] < 0)
				return false;

			if (testOnly)
				return MoveItem(item, tilePos, movementCount + 1, ref remainingStack, ref testOnly);

			(int,int) extractedStuff = (-12,-12);
			//ItemLoader.ExtractinatorUse(ref extractedStuff.Item1, ref extractedStuff.Item2, ItemID.Sets.ExtractinatorMode[item.type]);

			SGAmod.ExtractedItem.Item3 = true;

			typeof(Player).GetMethod("ExtractinatorUse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] {item.type});

			if (SGAmod.ExtractedItem.Item1<0)
				return false;

			extractedStuff = (SGAmod.ExtractedItem.Item1, SGAmod.ExtractedItem.Item2);

			SGAmod.ExtractedItem.Item3 = false;
			SGAmod.ExtractedItem.Item2 = -1;
			SGAmod.ExtractedItem.Item1 = -1;

			//LuminousAlterTE.DebugText("Extract test: " + extractedStuff.Item1);

				if (extractedStuff.Item1 < 0)
				return false;

			bool testIfWeCanDoIt = true;
			bool finalTransfer = true;

			for (int i = 0; i < 1; i += 1)
			{
				Item coins = new Item();
				coins.SetDefaults(ItemID.GoldCoin);
				coins.stack = Math.Min(item.stack, 5);

				finalTransfer &= MoveItem(coins, tilePos, movementCount + 1, ref remainingStack, ref testIfWeCanDoIt);
			}

			if (!finalTransfer)
				return false;

			for (int i = 0; i < Math.Min(item.stack, 5); i += 1)
			{
				Item extractedItemStuff = new Item();

				SGAmod.ExtractedItem.Item3 = true;
				typeof(Player).GetMethod("ExtractinatorUse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { item.type });
				extractedStuff = (SGAmod.ExtractedItem.Item1, SGAmod.ExtractedItem.Item2);
				SGAmod.ExtractedItem.Item3 = false;
				SGAmod.ExtractedItem.Item2 = -1;
				SGAmod.ExtractedItem.Item1 = -1;

				extractedItemStuff.SetDefaults(extractedStuff.Item1);
				extractedItemStuff.stack = extractedStuff.Item2;
				extractedItemStuff.newAndShiny = true;

				bool dontTestCoins = false;
				//int stacksize = 1;

				remainingStack -= 1;

				MoveItem(extractedItemStuff, tilePos, movementCount + 1, ref extractedItemStuff.stack, ref dontTestCoins);

				//item.stack -= 1;

			}
			return false;
		}
	}

}