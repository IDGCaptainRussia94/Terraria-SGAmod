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
            TileID.Sets.Ore[Type] = true;
            if (!drakenite)
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
            AddMapEntry(color, name);
            drop = itemID;
        }

        bool drakenite = false;
        int itemID => SGAmod.Instance.ItemType(internalname);
        string internalname;
        string barname;
        Color color;

        public BarTile(string internalname2,string barname,Color color,bool drakenite=false)
        {
            internalname = internalname2;
            this.barname = barname;
            this.color = color;
            this.drakenite = drakenite;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            return false;
        }

        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            if (Main.tile[i,j].type == mod.TileType("DrakeniteBarTile"))
            Dimensions.Tiles.Fabric.DrawStatic(this, i, j, spriteBatch,drakenite: true);
        }

    }
}