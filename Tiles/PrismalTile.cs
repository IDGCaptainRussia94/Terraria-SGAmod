using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles
{
    public class VibraniumCrystalTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileShine[Type] = 800;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 200;
            TileID.Sets.Ore[Type] = true;
            minPick = 250;
            soundType = SoundID.Tink;
            soundStyle = 1;
            drop = mod.ItemType("VibraniumCrystal");
            mineResist = 2.50f;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Vibranium Crystal");
            AddMapEntry(new Color(255, 60, 60), name);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            r = 0.75f;
            g = 0.25f;
            b = 0.25f;
        }
    }

    public class PrismalTile : ModTile
    {
        public override void SetDefaults()
        {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
            Main.tileShine[Type] = 800;
            Main.tileShine2[Type] = true;
            Main.tileSpelunker[Type] = true;
            Main.tileValue[Type] = 200;
            TileID.Sets.Ore[Type] = true;
            minPick = 120;
            soundType = 21;
            soundStyle = 1;
            drop = mod.ItemType("PrismalOre");
            mineResist = 1.25f;
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Prismal Ore");
            AddMapEntry(new Color(70, 0, 40), name);
        }
      
        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)  
        {
            r = 0.5f;
            g = 0.5f;
            b = 0.5f;
        }
    }
}