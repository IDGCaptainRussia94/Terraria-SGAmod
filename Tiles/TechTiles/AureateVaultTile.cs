using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using SGAmod.Items.Placeable;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ModLoader.IO;
using System.IO;
using SGAmod.Items.Placeable.TechPlaceable;
using System;
using Terraria.Utilities;
using System.Collections.Generic;
using System.Linq;
using Idglibrary;

namespace SGAmod.Tiles.TechTiles
{
	public class AureateVault : ModTile, IHopperInterface
	{
		const int maxXSize = 18 * 6;
		const int maxYSize = 18 * 4;
		protected virtual int DropItem => ModContent.ItemType<AureateVaultItem>();
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = "SGAmod/Invisible";
			return true;
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
				Main.LocalPlayer.AddBuff(ModContent.BuffType<Buffs.GildingAuraBuff>(),3);
			}
        }
        public override void SetDefaults()
		{
			TileID.Sets.DrawsWalls[Type] = true;
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			Main.tileLavaDeath[Type] = false;
			Main.tileSolid[Type] = false;
			Main.tileTable[Type] = false;
			Main.tileSolidTop[Type] = false;
			//Main.tileShine[Type] = 400;
			//Main.tileShine2[Type] = true;
			Main.tileLighted[Type] = true;

			//Main.tileSolid[Type] = true; // TODO: tModLoader hook for allowing non solid tiles to be hammer-able.

			//TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorLeft = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorRight = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			//TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.newTile.StyleHorizontal = false;
			TileObjectData.newTile.DrawYOffset = 0;

			TileObjectData.newTile.AnchorTop = AnchorData.Empty;
			TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);

			TileObjectData.addTile(Type);

			dustType = 57;
			//disableSmartCursor = true;
			//disableSmartInteract = true;

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Aureate Vault");
			AddMapEntry(Color.Goldenrod, name);
		}

		public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
		{
			r = 1f;
			g = 0.80f;
			b = 0;
		}

		public override bool NewRightClick(int i, int j)
		{
			/*Main.playerInventory = true;
			Main.mouseRightRelease = false;

			Main.LocalPlayer.chest = -3;
			Main.LocalPlayer.lastChest = -3;
			Main.LocalPlayer.chestX = i;
			Main.LocalPlayer.chestY = j;
			Main.LocalPlayer.talkNPC = -1;
			Main.npcShop = 0;
			Main.playerInventory = true;*/
			Main.PlaySound(SoundID.DoorOpen);

			Recipe.FindRecipes();


			if (Main.LocalPlayer.SGAPly().midasMoneyConsumed > 0)
            {
				SGAUtils.SpawnCoins(Main.LocalPlayer.Center, Main.LocalPlayer.SGAPly().midasMoneyConsumed, 0);
				Main.LocalPlayer.SGAPly().midasMoneyConsumed = 0;
			}

			return true;
		}

		// - ((tile.frameX / 36) % 2)
		public static Point GetRealAureateVaultCorner(Point here, Tile tile)
		{
			Point coords = new Point(here.X - ((tile.frameX) % 36) / 18, here.Y - ((tile.frameY) % 36) / 18);
			return coords;

		}
		public bool HopperInputItem(Item item, Point tilePos, int movementCount, ref bool testOnly)
		{
			return false;
		}

		public bool HopperExportItem(ref Item item, Point tilePos, int movementCount, ref bool testOnly)
		{

			Item coins = new Item();
			coins.SetDefaults(ItemID.CopperCoin);
			coins.stack = Main.rand.Next(10, 40);
			coins.newAndShiny = true;

			item = coins;

			return true;
		}

		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (fail)
				return;

			int item = DropItem;
			Tile tile = Framing.GetTileSafely(i, j);
			Point coords = GetRealAureateVaultCorner(new Point(i, j), tile);

			if (tile.type == ModContent.TileType<ChestHopperTile>())
				ModContent.GetInstance<ChestHopperTE>().Kill(coords.X, coords.Y);

			if (item > 0)
			{
				if (((tile.frameX) % 36) / 18 == 0 && ((tile.frameY) % 36) / 18 == 0)
					Item.NewItem(coords.X * 16, coords.Y * 16, 32, 32, item);
			}

		}

        public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
			Point there = new Point(x, y);
			Tile tile = Framing.GetTileSafely(there.X, there.Y);

			if (Main.tile[x, y].type == base.Type && there == GetRealAureateVaultCorner(there, tile))
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
			Point there = new Point(i, j);
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
			Tile tile = Framing.GetTileSafely(i, j);
			if (Main.tile[i, j].type == base.Type)
			{
				if (tile.frameX == 0 && tile.frameY == 0)
				{

					//Main.spriteBatch.End();
					//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

					UnifiedRandom rando = new UnifiedRandom((i*(j+1))%5648584);
					Texture2D coinTexture = Main.coinTexture[2];
					int texHeight = coinTexture.Height / 8;
					Vector2 offset = new Vector2(coinTexture.Width, texHeight / 8) / 2f;

					Vector2 baseoffset = zerooroffset+(new Point(i + 1, j + 1).ToVector2() * 16) + new Vector2(0, (float)Math.Sin(Main.GlobalTime / 2f) * 3f);

					List<(Vector3,int)> orderedValues = new List<(Vector3, int)>();

					for (int index = 0; index < 10; index += 1)
					{
						float time = Main.GlobalTime;

						float percent = rando.NextFloat(1f);
						float rate = rando.NextFloat(0.20f)+0.90f;

						float percent2 = rando.NextFloat(1f);
						float rate2 = rando.NextFloat(0.20f) + 0.40f;

						Vector2 scaled = new Vector2(24f,16f);

						Vector3 transform = Vector3.Transform(new Vector3(1f, 0, 0),Matrix.CreateFromAxisAngle(Vector3.Up, rando.NextFloat(MathHelper.TwoPi)+(time * rate)));

						Vector2 offset2 = new Vector2(transform.X, (float)Math.Sin((percent2 * MathHelper.TwoPi) + (time * rate2)))* scaled;

						//Vector2 drawPos = baseoffset+ (offset2 * scaled);
						int spriteindex = (int)((time* rando.NextFloat(5f,9f)*(rando.NextBool() ? 1f : -1f)) +rando.Next()) % 8;

						orderedValues.Add((new Vector3(offset2.X, offset2.Y-8, transform.Z), spriteindex));
					}

					orderedValues = orderedValues.OrderBy(testby => testby.Item1.Z).ToList();

					foreach((Vector3, int) tuple in orderedValues)
                    {
						if (tuple.Item1.Z >= 0)
							continue;
						Rectangle rect = new Rectangle(0, texHeight * tuple.Item2, coinTexture.Width, texHeight);
						spriteBatch.Draw(coinTexture, baseoffset + new Vector2(tuple.Item1.X, tuple.Item1.Y) - Main.screenPosition, rect, Color.White, 0, offset, 0.75f + tuple.Item1.Z * 0.25f, SpriteEffects.None, 0f);
					}

					Texture2D tex = ModContent.GetTexture("SGAmod/Items/Placeable/TechPlaceable/AureateVaultItem");
					spriteBatch.Draw(tex, baseoffset - Main.screenPosition, null, Color.White, 0, tex.Size() / 2f, 1, SpriteEffects.None, 0f);

					for (int ii = 0; ii < 20; ii += 1)
					{
						Texture2D texstar = Main.starTexture[rando.Next(Main.starTexture.Length)];
						spriteBatch.Draw(texstar, baseoffset + new Vector2(rando.NextFloat(-12, 12), rando.NextFloat(-16, 16)) - Main.screenPosition, null, Color.PaleGoldenrod*0.50f, Main.star[rando.Next(Main.star.Length)].rotation, texstar.Size() / 2f, rando.NextFloat(0.40f, 0.40f) * MathHelper.Clamp(((float)Math.Sin(Main.GlobalTime * rando.NextFloat(3.8f, 5.5f)) * rando.NextFloat(2f, 3f)),0f,1f), SpriteEffects.None, 0f);
					}

					foreach ((Vector3, int) tuple in orderedValues)
					{
						if (tuple.Item1.Z < 0)
							continue;

						Rectangle rect = new Rectangle(0, texHeight * tuple.Item2, coinTexture.Width, texHeight);
						spriteBatch.Draw(coinTexture, baseoffset+new Vector2(tuple.Item1.X, tuple.Item1.Y) - Main.screenPosition, rect, Color.White, 0, offset, 0.85f+tuple.Item1.Z*0.15f, SpriteEffects.None, 0f);
					}

					//Main.spriteBatch.End();
					//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

				}
			}
		}

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
			Point there = new Point(i, j);
			Tile tile = Framing.GetTileSafely(there.X, there.Y);

			return true;// there == GetRealAureateVaultCorner(there, tile);
        }
    }
}