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
using Microsoft.Xna.Framework.Graphics;
using Terraria.ObjectData;
using Terraria.Enums;

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
			return SGAWorld.downedMurk > 1 || SGAWorld.downedCaliburnGuardians > 2;
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return SGAWorld.downedMurk > 1 || SGAWorld.downedCaliburnGuardians > 2;
		}

		public override void RandomUpdate(int i, int j)
		{

			for (int xi = 1; xi <= 1; xi += 1)
			{
				if (!Main.tile[i, j - xi].active())
				{
					//if (Main.rand.Next(6) == 1)
					//{
						string[] onts = new string[] { "SwampGrassGrow", "SwampGrassGrow2", "SwampGrassGrow3" };
					if (xi<0)
					onts = new string[] { "SwampGrassGrowTop", "SwampGrassGrowTop2", "SwampGrassGrowTop3" };

					WorldGen.PlaceObject(i, j - xi, mod.TileType(onts[Main.rand.Next(onts.Length)]), true);
					//}
				}
			}
		}

	}
	public class SwampGrassGrow : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			dustType = 40;
			soundType = 6;
			//TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);

			//TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			//TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorTop = AnchorData.Empty;

			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				mod.TileType("MoistStone")
			};
			TileObjectData.addTile(Type);
		}

		public override void SetSpriteEffects(int i, int j, ref SpriteEffects spriteEffects)
		{
			if (i % 2 == 1)
			{
				spriteEffects = SpriteEffects.FlipHorizontally;
			}
			/*if (Main.tile[i, j - 2].type == ModContent.TileType<MoistStone>())
			{
				spriteEffects = SpriteEffects.FlipVertically;
			}*/
		}

		public override bool Drop(int i, int j)
		{
			int stage = Main.tile[i, j].frameX / 18;
			if (stage == 2 && Main.rand.Next(5)<1)
			{
				Item.NewItem(i * 16, j * 16, 0, 0, mod.ItemType("SwampSeeds"),Main.rand.Next(1,4));
			}
			return false;
		}

		public override void RandomUpdate(int i, int j)
		{
			if (Main.tile[i, j].frameX == 0)
			{
				Main.tile[i, j].frameX += 18;
			}
			else if (Main.tile[i, j].frameX == 18)
			{
				Main.tile[i, j].frameX += 18;
			}
		}
	}

	public class SwampGrassGrowTop : SwampGrassGrow
	{
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = texture.Replace("Top","");

			return true;
        }
        public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileCut[Type] = true;
			Main.tileNoFail[Type] = true;
			dustType = 40;
			soundType = 6;
			//TileObjectData.newTile.CopyFrom(TileObjectData.StyleAlch);
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);

			//TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			//TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, 1, 1);
			TileObjectData.newTile.AnchorBottom = AnchorData.Empty;

			TileObjectData.newTile.AnchorValidTiles = new int[]
			{
				mod.TileType("MoistStone")
			};
			TileObjectData.addTile(Type);
		}
	}

	public class SwampGrassGrowTop2 : SwampGrassGrowTop
	{

	}
	public class SwampGrassGrowTop3 : SwampGrassGrowTop
	{

	}

	public class SwampGrassGrow2 : SwampGrassGrow
	{

    }
	public class SwampGrassGrow3 : SwampGrassGrow
	{

	}


	public class MoistSand : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSand[Type] = true;
			Main.tileMerge[Type][59] = true;
			Main.tileMerge[59][Type] = true;
			Main.tileMerge[Type][53] = true;
			Main.tileMerge[53][Type] = true;
			Main.tileMerge[Type][397] = true;
			Main.tileMerge[397][Type] = true;
			Main.tileLighted[Type] = true;
			minPick = 0;
			//soundType = 21;
			//mineResist = 4f;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			TileID.Sets.Conversion.HardenedSand[Type] = true;
			TileID.Sets.Mud[Type] = true;
			drop = mod.ItemType("MoistSand");
			AddMapEntry(new Color(140, 160, 100));
			SetModCactus(new MudCactus());
		}
	}

	public class MudCactus : ModCactus
	{
		private Mod mod
		{
			get
			{
				return SGAmod.Instance;
			}
		}

		public override Texture2D GetTexture()
		{
			return mod.GetTexture("Tiles/MudCactus");
		}
	}
}