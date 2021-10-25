using Microsoft.Xna.Framework;
using System.Linq;
using On.Terraria.DataStructures;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Buffs;

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
            TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
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

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.showItemIconText = "";
            Main.LocalPlayer.showItemIcon = true;
            Main.LocalPlayer.showItemIcon2 = ModContent.ItemType<Items.Consumables.Debug1>();

            if (!Main.LocalPlayer.HeldItem.IsAir && !Main.LocalPlayer.HeldItem.favorited)
            {
                Item item = Main.LocalPlayer.HeldItem;

                UncraftClass craft = new UncraftClass(new Terraria.DataStructures.Point16(i, j - (Main.tile[i, j].frameY / 16) - 3), item, SGAmod.RecipeIndex, i+j*3);
                SGAInterface.Uncrafts = craft;
                if (!craft.uncraftable)
                {
                    Main.LocalPlayer.showItemIcon2 = item.type;
                    Main.LocalPlayer.showItemIconText = "\nNeed " + craft.stackSize;
                }
            }

        }

        public override bool NewRightClick(int i, int j)
        {
            if (!Main.LocalPlayer.HeldItem.IsAir && !Main.LocalPlayer.HeldItem.favorited)
            {
                Item item = Main.LocalPlayer.HeldItem;

                UncraftClass craft = new UncraftClass(new Terraria.DataStructures.Point16(i, j - (Main.tile[i, j].frameY / 16) - 3), item, SGAmod.RecipeIndex, i + j * 3);
                SGAInterface.Uncrafts = craft;
                    craft.Uncraft(Main.LocalPlayer,75+(Main.LocalPlayer.SGAPly().uncraftBoost>0 ? 15 : 0));
            }

            return true;
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