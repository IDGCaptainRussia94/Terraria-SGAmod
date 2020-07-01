using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Dimensions.Walls
{
	public class NullWall : ModWall
	{
		public override void SetDefaults() {
			Main.wallHouse[Type] = false;
			dustType = DustID.Smoke;
			AddMapEntry(new Color(0, 0, 0));
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void KillWall(int i, int j, ref bool fail)
		{
			fail = true;
		}
	}
}