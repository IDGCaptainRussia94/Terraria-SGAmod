using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SGAmod.Tiles
{
    public class PrismalStation : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileTable[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 18 };
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleHorizontal = false;
            //TileObjectData.newTile.StyleWrapLimit = 36;
			animationFrameHeight = 54;
			TileObjectData.addTile(Type);
			adjTiles = new int[]{96};
			ModTranslation name = CreateMapEntryName();
            name.SetDefault("Prismal Extractor");
            //name.AddTranslation(GameCulture.Chinese, "烤炉");
            AddMapEntry(new Color(227, 216, 195), name);
        }

        public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frameCounter++;
			if(frameCounter > ((frame>2 && frame<4) ? 24 : 7))
			{
				frameCounter = 0;
				frame++;
				frame %= 5;
			}
		}

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if(Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
            {
                Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("PrismalStation"), 1, false, 0, false, false);
            }
        }
    }
}