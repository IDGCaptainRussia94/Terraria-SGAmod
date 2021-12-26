using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Tiles
{

	public class SwampWoodWall : SwampWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = true;
			AddMapEntry(new Color(100, 40, 40));
		}

        public override bool Drop(int i, int j, ref int type)
        {
			type = ModContent.ItemType<Items.Placeable.SwampWoodWall>();
			return true;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}
	}
	public class SwampWall : ModWall
	{
		public override void SetDefaults()
		{
			Main.wallHouse[Type] = false;
			AddMapEntry(new Color(150, 250, 150));
		}

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = fail ? 1 : 3;
		}

		public override bool CanExplode(int i, int j)
		{
			return SGAWorld.downedMurk > 1;
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			//r = 0.4f;
			//g = 0.4f;
			//b = 0.4f;
		}
	}
}