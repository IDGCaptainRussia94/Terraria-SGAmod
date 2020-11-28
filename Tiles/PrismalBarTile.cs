using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace SGAmod.Tiles
{
    public class BarTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileShine[Type] = 2100;
            Main.tileSolid[Type] = true;
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileLighted[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.StyleHorizontal = true;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault(barname);
            AddMapEntry(new Color(210, 0,100), name);
            drop = itemID;
        }

        int itemID => SGAmod.Instance.ItemType(internalname);
        string internalname;
        string barname;
        Color color;

        public BarTile(string internalname2,string barname,Color color)
        {
            internalname = internalname2;
            this.barname = barname;
            this.color = color;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            return false;
        }

    }
}