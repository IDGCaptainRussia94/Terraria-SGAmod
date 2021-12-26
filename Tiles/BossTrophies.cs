using IL.Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SGAmod.Tiles
{
	public class BossTrophies : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileLavaDeath[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3Wall);
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.StyleWrapLimit = 6;
			TileObjectData.addTile(Type);
			dustType = 7;
			disableSmartCursor = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Trophy");
			AddMapEntry(new Color(120, 85, 60), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			int item = 0;
			if (frameY / 54 == 0) //PreHardmode-1st row
			{
				switch (frameX / 54)
				{
					case 0:
						item = mod.ItemType("CopperWraithTrophy");
						break;
					case 1:
						item = mod.ItemType("CaliburnATrophy");
						break;
					case 2:
						item = mod.ItemType("CaliburnBTrophy");
						break;
					case 3:
						item = mod.ItemType("CaliburnCTrophy");
						break;
					case 4:
						item = mod.ItemType("SpiderQueenTrophy");
						break;
					case 5:
						item = mod.ItemType("MurkTrophy");
						break;
				}
			}
			if (frameY / 54 == 1) //Hardmode-2nd row
			{
				switch (frameX / 54)
				{
					case 0:
						item = mod.ItemType("CirnoTrophy");
						break;
					case 1:
						item = mod.ItemType("CobaltWraithTrophy");
						break;
					case 2:
						item = mod.ItemType("SharkvernTrophy");
						break;
					case 3:
						item = mod.ItemType("CratrosityTrophy");
						break;
					case 4:
						item = mod.ItemType("TwinPrimeDestroyersTrophy");
						break;
					case 5:
						item = mod.ItemType("DoomHarbingerTrophy");
						break;
				}

			}
			if (frameY / 54 == 2) //PML-3nd row
			{
				switch (frameX / 54)
				{
					case 0:
						item = mod.ItemType("LuminiteWraithTrophy");
						break;
					case 1:
						item = mod.ItemType("CratrogeddonTrophy");
						break;
					case 2:
						item = mod.ItemType("SupremePinkyTrophy");
						break;
					case 3:
						item = mod.ItemType("HellionTrophy");
						break;
					case 4:
						item = mod.ItemType("PhaethonTrophy");
						break;
				}

			}
			if (item > 0)
			{
				Item.NewItem(i * 16, j * 16, 48, 48, item);
			}
		}
	}
}