using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using System.Collections.Generic;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;

namespace SGAmod.Tiles
{
	public class MoistStone : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			//Main.tileLighted[Type] = true;
            minPick = 50;
            soundType = 21;
            mineResist = 2f;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
            //drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(100, 160, 100));
		}

		public override bool CanExplode(int i, int j)
		{
			return SGAWorld.downedMurk > 1;
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return SGAWorld.downedMurk > 1;
		}
	}
}