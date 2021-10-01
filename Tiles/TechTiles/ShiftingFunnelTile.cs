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
				Item coins = new Item();

				SGAmod.ExtractedItem.Item3 = true;
				typeof(Player).GetMethod("ExtractinatorUse", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(null, new object[] { item.type });
				extractedStuff = (SGAmod.ExtractedItem.Item1, SGAmod.ExtractedItem.Item2);
				SGAmod.ExtractedItem.Item3 = false;
				SGAmod.ExtractedItem.Item2 = -1;
				SGAmod.ExtractedItem.Item1 = -1;

				coins.SetDefaults(extractedStuff.Item1);
				coins.stack = extractedStuff.Item2;
				coins.newAndShiny = true;

				bool dontTestCoins = false;
				//int stacksize = 1;

				remainingStack -= 1;

				MoveItem(coins, tilePos, movementCount + 1, ref coins.stack, ref dontTestCoins);

				//item.stack -= 1;

			}
			return false;
		}
	}

}