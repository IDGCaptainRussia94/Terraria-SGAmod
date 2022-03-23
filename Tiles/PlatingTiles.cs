using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles
{
	public class AdvancedPlatingTile : ModTile
	{
		public override void SetDefaults() {
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			soundType = SoundID.Tink;
			soundStyle = 1;
			dustType = DustID.Iron;
			drop = ModContent.ItemType<Items.AdvancedPlating>();
			AddMapEntry(new Color(181, 165, 107));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}
	}
	public class VibraniumPlatingTile : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			soundType = SoundID.Tink;
			soundStyle = 1;
			dustType = DustID.Iron;
			drop = ModContent.ItemType<Items.VibraniumPlating>();
			AddMapEntry(new Color(88, 44, 86));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
}