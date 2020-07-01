using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.World.Generation;
using Terraria.GameContent.Generation;
using SubworldLibrary;
using SGAmod;
using SGAmod.Items;

namespace SGAmod.Dimensions.Tiles
{

	public class EntrophicOre : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("Fabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("AnicentFabric")] = true;
			minPick = 200;
			soundType = 7;
			mineResist = 3f;
			dustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			drop = ModContent.ItemType<Entrophite>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Entrophite");
			AddMapEntry(new Color(20, 0, 15), name);
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (nextSpecialDrawIndex < Main.specX.Length && Main.rand.Next(0, 10) == 1)
			{
				Main.specX[nextSpecialDrawIndex] = x;
				Main.specY[nextSpecialDrawIndex] = y;
				nextSpecialDrawIndex += 1;
			}
		}
		public override void SpecialDraw(int x, int y, SpriteBatch spriteBatch)
		{
			if (Main.tile[x, y].type == base.Type)
			{
				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 drawOffset = new Vector2((float)(x * 16) - Main.screenPosition.X, (float)(y * 16) - Main.screenPosition.Y) + zerooroffset;

				spriteBatch.Draw(LimboDim.staticeffects[Main.rand.Next(0, LimboDim.staticeffects.Length)], drawOffset, Color.Lerp(Lighting.GetColor((int)drawOffset.X, (int)drawOffset.Y), Color.Purple, 0.5f));
			}
		}
	}

	public class AncientFabric : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("Fabric")] = true;
			minPick = 0;
			soundType = 7;
			mineResist = 5f;
			dustType = DustID.Smoke;
			drop = ModContent.ItemType<AncientFabricItem>();
			TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
			//drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(80, 0, 0));
		}

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void FloorVisuals(Player player)
		{
			SGAPocketDim.ExitSubworld();
		}
		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (nextSpecialDrawIndex < Main.specX.Length && Main.rand.Next(0, 5) == 1)
			{
				Main.specX[nextSpecialDrawIndex] = x;
				Main.specY[nextSpecialDrawIndex] = y;
				nextSpecialDrawIndex += 1;
			}
		}
		public override void SpecialDraw(int x, int y, SpriteBatch spriteBatch)
		{
			if (Main.tile[x, y].type == base.Type)
			{
				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 drawOffset = new Vector2((float)(x * 16) - Main.screenPosition.X, (float)(y * 16) - Main.screenPosition.Y) + zerooroffset;

				spriteBatch.Draw(LimboDim.staticeffects[Main.rand.Next(0, LimboDim.staticeffects.Length)], drawOffset, Color.Lerp(Lighting.GetColor((int)drawOffset.X, (int)drawOffset.Y), Color.Red, 0.5f));
			}
		}
	}

		public class Fabric : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("AncientFabric")] = true;
			minPick = 0;
			soundType = 7;
			mineResist = 0.5f;
			dustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			//drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(50, 50, 50));
		}
		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (nextSpecialDrawIndex < Main.specX.Length && Main.rand.Next(0, 100) == 1)
			{
				Main.specX[nextSpecialDrawIndex] = x;
				Main.specY[nextSpecialDrawIndex] = y;
				nextSpecialDrawIndex += 1;
			}
		}
		public override void SpecialDraw(int x, int y, SpriteBatch spriteBatch)
		{
			if (Main.tile[x,y].type==base.Type)
			{
				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 drawOffset = new Vector2((float)(x * 16) - Main.screenPosition.X, (float)(y * 16) - Main.screenPosition.Y)+ zerooroffset;
				//spriteBatch.Draw(Main.blackTileTexture, drawOffset);
				//spriteBatch.Draw(Main.blackTileTexture, drawOffset, new Rectangle(0, 0, 16, 16), Color.Purple, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);

				spriteBatch.Draw(LimboDim.staticeffects[Main.rand.Next(0, LimboDim.staticeffects.Length)],drawOffset, Color.Lerp(Lighting.GetColor((int)drawOffset.X, (int)drawOffset.Y), Color.White, 0.25f));
			}
		}
	}
}