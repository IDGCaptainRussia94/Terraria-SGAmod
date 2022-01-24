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
using IL.Terraria.DataStructures;
using Idglibrary;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using System.Linq;
using SGAmod.Items.Tools;

namespace SGAmod.Dimensions.Tiles
{
	class WatcherOre : ModTile
	{
		public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileBrick[Type] = false;
			Main.tileFrameImportant[Type] = true;
			minPick = 200;
			soundType = 7;
			mineResist = 3f;
			dustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			drop = ModContent.ItemType<Entrophite>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("???");
			AddMapEntry(new Color(0, 255, 255), name);
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{

			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Vector2 location = (new Vector2(i, j) * 16) + zerooroffset;

			spriteBatch.Draw(Main.tileTexture[Main.tile[i, j].type], location - Main.screenPosition, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY, 16, 16), Lighting.GetColor(i, j), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);


			return true;
		}

		public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
		{
			if (Main.tile[x, y].type == base.Type)
			{
				if (nextSpecialDrawIndex < Main.specX.Length)
				{
					Main.specX[nextSpecialDrawIndex] = x;
					Main.specY[nextSpecialDrawIndex] = y;
					nextSpecialDrawIndex += 1;
				}
			}
		}

		public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
		{

			if (Main.tile[i, j].type == base.Type)
			{

				NoiseGenerator Noisegen = new NoiseGenerator(WorldGen._lastSeed);
				Noisegen.Amplitude = 1.5f;
				Noisegen.Frequency *= 3.50;

				//spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale * 2f, SpriteEffects.None, 0f);
				Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
				Vector2 location = (new Vector2(i, j) * 16) + new Vector2(8, 8) + zerooroffset;
				Texture2D tex = ModContent.GetTexture("SGAmod/Items/WatchersOfNull");
				int framesize = (int)(tex.Height / 13f);
				Rectangle Frame = new Rectangle(0, framesize * 4, tex.Width, framesize);
				Vector2 offset = new Vector2(tex.Width / 2f, (tex.Height / 13f) / 2f);

				Vector2 drawOffset2 = new Vector2((float)(i * 16) - Main.screenPosition.X, (float)(j * 16) - Main.screenPosition.Y) + zerooroffset;

				Vector2 playerdist = (Main.LocalPlayer.Center + zerooroffset) - location;
				Vector2 offset2 = new Vector2(0,0)+Vector2.Normalize((Main.LocalPlayer.Center + zerooroffset) - location)*16f;

				spriteBatch.Draw(LimboDim.staticeffects[Main.rand.Next(0, LimboDim.staticeffects.Length)], drawOffset2, Color.Black);

				Color color = Color.White * MathHelper.Clamp(0.50f + (float)Noisegen.Noise(i + (int)(Main.GlobalTime * 10.00), j+(int)(Main.GlobalTime * -5.00)) / 2f, 0f, 1f);

				spriteBatch.Draw(tex, location + offset2 - Main.screenPosition, Frame, color, 0f, offset, 0.5f, SpriteEffects.None, 0f);

			}
		}
	}

	public class Spacerock : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;

			Main.tileMerge[(ushort)mod.TileType("Spacerock")][(ushort)mod.TileType("Spacerock2")] = true;
			Main.tileMerge[(ushort)mod.TileType("Spacerock2")][(ushort)mod.TileType("Spacerock")] = true;

			Main.tileMerge[TileID.Meteorite][(ushort)mod.TileType("Spacerock")] = true;
			Main.tileMerge[(ushort)mod.TileType("Spacerock")][TileID.Meteorite] = true;

			Main.tileMerge[(ushort)mod.TileType("Spacerock2")][TileID.Meteorite] = true;
			Main.tileMerge[TileID.Meteorite][(ushort)mod.TileType("Spacerock2")] = true;

			Main.tileMerge[(ushort)mod.TileType("AstrialLuminite")][(ushort)mod.TileType("Spacerock2")] = true;
			Main.tileMerge[(ushort)mod.TileType("Spacerock2")][(ushort)mod.TileType("AstrialLuminite")] = true;

			Main.tileMerge[(ushort)mod.TileType("AstrialLuminite")][(ushort)mod.TileType("Spacerock")] = true;
			Main.tileMerge[(ushort)mod.TileType("Spacerock")][(ushort)mod.TileType("AstrialLuminite")] = true;

			Main.tileMerge[TileID.Meteorite][(ushort)mod.TileType("AstrialLuminite")] = true;
			Main.tileMerge[(ushort)mod.TileType("AstrialLuminite")][TileID.Meteorite] = true;

			Main.tileMerge[(ushort)mod.TileType("VibraniumCrystalTile")][(ushort)mod.TileType("Spacerock")] = true;
			Main.tileMerge[(ushort)mod.TileType("Spacerock")][(ushort)mod.TileType("VibraniumCrystalTile")] = true;

			Main.tileMerge[(ushort)mod.TileType("VibraniumCrystalTile")][(ushort)mod.TileType("Spacerock2")] = true;
			Main.tileMerge[(ushort)mod.TileType("Spacerock2")][(ushort)mod.TileType("VibraniumCrystalTile")] = true;

			Main.tileMerge[(ushort)mod.TileType("VibraniumCrystalTile")][TileID.Meteorite] = true;
			Main.tileMerge[TileID.Meteorite][(ushort)mod.TileType("VibraniumCrystalTile")] = true;

			int[] oreType = { TileID.LunarBlockNebula, TileID.LunarBlockSolar, TileID.LunarBlockStardust, TileID.LunarBlockVortex };
			foreach (int typeofore in oreType)
            {
				Main.tileMerge[typeofore][Type] = true;
				Main.tileMerge[Type][typeofore] = true;
			}

			TileID.Sets.ChecksForMerge[Type] = true;

			minPick = 100;
			soundType = SoundID.Tink;
			soundStyle = 0;
			mineResist = 5f;
			dustType = 36;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("");
			AddMapEntry(Color.DarkGray, name);
		}
	}

	public class Spacerock2 : Spacerock
	{
		public override void SetDefaults()
		{
			base.SetDefaults();
			minPick = 100;
			soundType = SoundID.Tink;
			soundStyle = 0;
			mineResist = 5f;
			dustType = 36;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("");
			AddMapEntry(Color.DarkRed, name);
			TileID.Sets.ChecksForMerge[Type] = true;
		}
	}

	public class AstrialLuminite : ModTile
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Tiles_" + TileID.LunarOre;
			return true;
		}
		public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			minPick = 200;// !SGAConfig.Instance.EarlyLuminite ? 225 : 200;
			soundType = SoundID.Tink;
			soundStyle = 0;
			mineResist = 5f;
			dustType = DustID.LunarOre;
			drop = ModContent.ItemType<CelestineChunk>();
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Astrial Luminite");
			AddMapEntry(Color.White, name);
		}

        public override bool CanExplode(int i, int j)
        {
			return false;
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
			//if (!SpaceDim.postMoonLord)
			//	fail = true;

			if (!fail)
			{ 
				noItem = true;

				for (int zz = 0; zz < Main.rand.Next(1, 4); zz += 1)
				{
					Item.NewItem(new Vector2(i, j) * 16, 16, 16, ItemID.LunarOre, 1);
				}
			}
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].active())
			{
				r = Color.PaleTurquoise.R * 0.02f;
				g = Color.PaleTurquoise.G * 0.02f;
				b = Color.PaleTurquoise.B * 0.02f;
			}
		}


	}

	public class HopeOre : Fabric
	{
		public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileLighted[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("Fabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("AnicentFabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("EntrophicOre")] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			minPick = 100;
			soundType = SoundID.NPCHit;
			soundStyle = 5;
			mineResist = 2f;
			dustType = DustID.GoldCoin;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			drop = ModContent.ItemType<HopeHeart>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Hope...");
			AddMapEntry(Color.LightGoldenrodYellow, name);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			if (Main.tile[i, j].active())
			{
				r = Color.Goldenrod.R * 0.02f;
				g = Color.Goldenrod.G * 0.02f;
				b = Color.Goldenrod.B * 0.02f;
			}
		}

		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}

	public class EntrophicOre : Fabric
	{
		public override void SetDefaults()
		{
			TileID.Sets.Ore[Type] = true;
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("Fabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("AnicentFabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HopeOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HardenedFabric")] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			minPick = 200;
			soundType = 7;
			mineResist = 3f;
			dustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			drop = ModContent.ItemType<Entrophite>();
			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Entrophite");
			AddMapEntry(new Color(30, 0, 25), name);
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
	public class HardenedFabric : Fabric
	{
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = "SGAmod/Dimensions/Tiles/Fabric";
			return true;
        }
        public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("Fabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("EntrophicOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HopeOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("AnicentFabric")] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			minPick = 240;
			soundType = 7;
			mineResist = 5f;
			dustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			//drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(5, 5, 5));
		}

		/*public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Vector2 location = (new Vector2(i, j) * 16) + zerooroffset;
			spriteBatch.Draw(Main.tileTexture[Main.tile[i, j].type], location - Main.screenPosition, new Rectangle(Main.tile[i, j].frameX, Main.tile[i, j].frameY, 16, 16), Color.Lerp(Lighting.GetColor(i, j),Color.Black,0.75f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
			return true;
		}*/

		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			return false;
		}
		public override bool CanExplode(int i, int j)
		{
			return false;
		}
	}
	public class AncientFabric : Fabric
	{
		public override void SetDefaults()
		{
			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("Fabric")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HopeOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("EntrophicOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HardenedFabric")] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			minPick = 240;
			soundType = 7;
			mineResist = 25f;
			dustType = DustID.Smoke;
			drop = ModContent.ItemType<AncientFabricItem>();
			TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
			//drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(80, 0, 0));
		}
		public override bool CanKillTile(int i, int j, ref bool blockDamaged)
		{
			bool canbreak = Main.player.Where(testby => testby.DistanceSQ(new Vector2(i * 16, j*16))<160*160 && !testby.HeldItem.IsAir && testby.HeldItem.type == ModContent.ItemType<Braxsaw>()).ToList().Count>0;
			return canbreak;
		}
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail && !effectOnly)
            {
				DimDingeonsWorld.ancientLockedFabric = false;
			}
        }
        public override bool CanExplode(int i, int j)
		{
			return false;
		}
		public override void FloorVisuals(Player player)
		{
			if (player.HeldItem.type != ModContent.ItemType<Braxsaw>())
			SGAPocketDim.ExitSubworld();
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
			Main.tileMerge[Type][(ushort)mod.TileType("EntrophicOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HopeOre")] = true;
			Main.tileMerge[Type][(ushort)mod.TileType("HardenedFabric")] = true;
			TileID.Sets.ChecksForMerge[Type] = true;
			minPick = 0;
			soundType = 7;
			mineResist = 0.5f;
			dustType = DustID.Smoke;
			TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
			//drop = mod.ItemType("Moist Stone");
			AddMapEntry(new Color(50, 50, 50));
		}
		public static void DrawStatic(object me, int x, int y, SpriteBatch spriteBatch,bool drakenite=false)
		{
			//if (Main.tile[x, y].type == base.Type)
			//{
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Vector2 drawOffset = new Vector2((float)(x * 16), (float)(y * 16)) + zerooroffset;
			//spriteBatch.Draw(Main.blackTileTexture, drawOffset);
			//spriteBatch.Draw(Main.blackTileTexture, drawOffset, new Rectangle(0, 0, 16, 16), Color.Purple, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);

			int chance = 20;

			Color light = Lighting.GetColor(x, y);

			Color basecolor = Color.White;
			float basealpha = 1f;
			float alphamin = 0.15f;
			float alphamax = 0.3f;
			int glowchance = 50;
			if (me.GetType() == typeof(HardenedFabric))
			{
				basecolor = new Color(55, 55, 55);
				basealpha = 1f;
				chance = 15;
				alphamin = 0.25f;
				alphamax = 0.5f;
			}
			if (me.GetType() == typeof(AncientFabric))
			{
				basecolor = Color.Red;
				basealpha = 1f;
				chance = 40;
				alphamin = 0.75f;
				alphamax = 1f;
			}
			if (me.GetType() == typeof(EntrophicOre))
			{
				basecolor = Color.Purple;
				basealpha = 1f;
				chance = 1;
				alphamin = 0.9f;
				alphamax = 1f;
				glowchance = 1;
			}
			if (me.GetType() == typeof(HopeOre))
			{
				basecolor = Color.Goldenrod;
				basealpha = 1f;
				chance = 2;
				alphamin = 0.5f;
				alphamax = 1f;
				glowchance = 2;
			}
			if (drakenite)
			{
				basecolor = Main.hslToRgb(((x/22f)+(y/16f)+Main.GlobalTime/2f) %1f,1f,0.50f);
				basealpha = 2f;
				chance = 0;
				alphamin = 1f;
				alphamax = 1f;
				glowchance = 1;
			}
			if (Main.rand.Next(0, 100) < chance)
				return;

			//stem.Drawing.Color basiccolor = System.Drawing.Color.FromArgb(lightblack.R, lightblack.G, lightblack.B);

			float brightness = Math.Max((float)light.R, Math.Max((float)light.G, (float)light.B)) / 255f;

			float alpha = Main.rand.Next(0, glowchance) == 0 ? Main.rand.NextFloat(alphamin, alphamax) : brightness;

			if (alpha > 0f)
				spriteBatch.Draw(LimboDim.staticeffects[Main.rand.Next(0, LimboDim.staticeffects.Length)], drawOffset - Main.screenPosition, Color.Lerp(light, basecolor, basealpha) * alpha);
		}
		//}
		public override void PostDraw(int x, int y, SpriteBatch spriteBatch)
		{
			DrawStatic(this, x, y, spriteBatch);
		}
	}
}