using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SGAmod.Tiles
{
    public class ReverseEngineeringStation : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileTable[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 20 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.DrawYOffset = -2;
            //TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            adjTiles = new int[] { TileID.TinkerersWorkbench };
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Reverse Engineering Station");
            //name.AddTranslation(GameCulture.Chinese, "烤炉");
            AddMapEntry(new Color(227, 216, 195), name);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if(Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
            {
                Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType("ReverseEngineeringStation"), 1, false, 0, false, false);
            }
        }
    }
}