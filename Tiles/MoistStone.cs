using System;
using System.Linq;
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
            mineResist = 5f;
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

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			/*if ((effectOnly || fail) && SGAWorld.downedMurk < 2)
            {
				foreach(Player player in Main.player.Where(playertest => playertest.active && playertest.DistanceSQ(new Vector2(i, j) * 16) < 256 * 256))
                {
					if (player.HeldItem.IsAir || player.HeldItem.pick < 1)
						continue;

					player.AddBuff(ModContent.BuffType<Buffs.MiningFatigue>(), 300);
					player.AddBuff(BuffID.OgreSpit, 300);
					player.AddBuff(BuffID.Poisoned, 180);
				}

			}*/
		}
    }
}