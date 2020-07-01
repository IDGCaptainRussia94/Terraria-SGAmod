using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Tiles
{
	public class Biomass: ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine2[Type] = false;
			Main.tileSpelunker[Type] = false;
			Main.tileValue[Type] = 20;
			TileID.Sets.Ore[Type] = true;
			soundType = 21;
			soundStyle = 1;
			dustType = 128;
			drop = mod.ItemType("Biomass");
			minPick = 10;
			mineResist = 1f;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Biomass");
			AddMapEntry(new Color(40, 160, 40), name);
		}
	}
}