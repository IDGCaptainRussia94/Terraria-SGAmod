using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.ObjectData;

namespace SGAmod.Tiles.Monolith
{
	//Most of this came from Elemental Unleash by Blushie Magic
	//I don't like doing tiles, lol
	public class CelestialMonolith : ModTile
	{
		public override void SetDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2xX);
			TileObjectData.newTile.Height = 3;
			TileObjectData.newTile.Origin = new Point16(1, 2);
			TileObjectData.newTile.CoordinateHeights = new int[]{ 16, 16, 18 };
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<CelestialMonolithTE>().Hook_AfterPlacement, -1, 0, false);
			TileObjectData.addTile(Type);
			AddMapEntry(new Color(75, 139, 166));
			dustType = 1;
			animationFrameHeight = 56;
			disableSmartCursor = true;
			adjTiles = new int[]{ TileID.LunarMonolith };
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(i * 16, j * 16, 32, 48, ModContent.ItemType<CelestialMonolithItem>());
			ModContent.GetInstance<CelestialMonolithTE>().Kill(i, j);
			CelestialMonolithTE.ResetTEs();
		}

		public override void AnimateTile(ref int frame, ref int frameCounter)
		{
			frame = 0;// Main.tileFrame[TileID.LunarMonolith];
			frameCounter = Main.tileFrameCounter[TileID.LunarMonolith];
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
			Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);

			Tile tile = Framing.GetTileSafely(i, j);
			if (Main.tile[i, j].type == base.Type)
			{
				if (tile.frameX == 0 && tile.frameY % 56 == 0)//top left
				{
					int yFrame = j - (tile.frameY / 18) - (tile.frameY == 56 ? -3 : 0);
					//Texture2D inner = Main.tileTexture[Type];
					Texture2D inner = ModContent.GetTexture("SGAmod/Tiles/Monolith/CelestialMonolithTex");
					Texture2D star = ModContent.GetTexture("SGAmod/Tiles/TechTiles/LuminousAlterStar");
					Rectangle rect = new Rectangle(0, (int)((Main.GlobalTime * 1) % 2) * (star.Height / 2), star.Width, star.Height / 2);
					Rectangle rect2 = new Rectangle(0, (int)(((Main.GlobalTime * 1) + 1) % 2) * (star.Height / 2), star.Width, star.Height / 2);

					int height = tile.frameY == 36 ? 18 : 16;
					int animate = 0;

					if (tile.frameY >= 56)
					{
						animate = Main.tileFrame[Type] * animationFrameHeight;
					}

					int teid = ModContent.GetInstance<CelestialMonolithTE>().Find(i - (tile.frameX / 18), yFrame);

					if (teid != -1)
					{
						CelestialMonolithTE te = (CelestialMonolithTE)TileEntity.ByID[teid];
						if (te != null)
						{

							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

							Texture2D sunTex = Main.sunTexture;

							ArmorShaderData stardustsshader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

							DrawData value28 = new DrawData(inner, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, inner.Width, inner.Height)), Microsoft.Xna.Framework.Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
							stardustsshader3.UseColor(Color.Lerp(Color.Transparent, (Main.dayTime ? Color.Blue * 1.0f : Color.Yellow * 0.50f), te.ActiveState).ToVector3());
							stardustsshader3.UseOpacity(te.ActiveState/1f);
							stardustsshader3.Apply(null, new DrawData?(value28));

							//Main.spriteBatch.Draw(mod.GetTexture("Tiles/Monolith/CelestialMonolith"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

							float floater = (float)Math.Sin(Main.GlobalTime);
							Vector2 offset = zerooroffset + (new Vector2(i, j) * 16) + new Vector2(16, (38*(1f-te.ActiveState)) - (floater * 8) * te.ActiveState);

							if (te.ActiveState > 0)
							{
								//spriteBatch.Draw(inner, zerooroffset + (new Vector2(i, j) * 16) - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

								int yyy = Main.dayTime ? 1 : 4;

								for (int xx = 0; xx < 2; xx++)
								{
									for (int yy = 0; yy < 3; yy++)
									{
										int yyyy = (yy * 18) + (int)(yyy * (56));

										Rectangle rectz = new Rectangle(xx * 18, yyyy, 16, 16);
										Main.spriteBatch.Draw(inner, new Vector2(((i + xx) * 16), (j + yy) * 16) + zerooroffset-Main.screenPosition, rectz, Color.White* te.ActiveState, 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
									}
								}
							}

							if (Main.dayTime)
							{
								int maxFrame = 8;
								int frame = (int)((Main.GlobalTime*8) % maxFrame);
								int frameHeight = (Main.moonTexture[Main.moonType].Height / maxFrame);
								Rectangle rectx = new Rectangle(0, frame * frameHeight, (Main.moonTexture[Main.moonType].Width), frameHeight);

								Main.spriteBatch.End();
								Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

								if (te.ActiveState > 0)
								{
									Texture2D glowTex = mod.GetTexture("Glow");
									for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
									{
										Main.spriteBatch.Draw(glowTex, offset - Main.screenPosition, null, Color.Blue * te.ActiveState, f, glowTex.Size() / 2f, new Vector2(3.2f + floater, 0.80f) * te.ActiveState, SpriteEffects.None, 0f);
									}
								}

								Main.spriteBatch.End();
								Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

								Texture2D glowOrb = ModContent.GetTexture("SGAmod/Glow");
								for(int xx=0;xx<4;xx+=1)
								Main.spriteBatch.Draw(glowOrb, offset - Main.screenPosition, null, Color.Black * Math.Min(te.ActiveState*2f,1f), 0f, glowOrb.Size() / 2f, 0.60f, SpriteEffects.None, 0f);
								Main.spriteBatch.Draw(Main.moonTexture[Main.moonType], offset - Main.screenPosition, rectx, Color.White, 0f, rectx.Size() / 2f, 0.25f + (0.75f * te.ActiveState), SpriteEffects.None, 0f);


							}
							else
							{

								Main.spriteBatch.End();
								Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

								if (te.ActiveState > 0)
								{
									Texture2D glowTex = mod.GetTexture("Glow");
									for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 10f)
									{
										Main.spriteBatch.Draw(glowTex, offset - Main.screenPosition, new Rectangle(0, 0, glowTex.Width / 2, glowTex.Height), Color.White * te.ActiveState, f, glowTex.Size() / 2f, new Vector2(3.2f + floater, 0.80f) * te.ActiveState, SpriteEffects.None, 0f);
									}
								}

								Main.spriteBatch.Draw(sunTex, offset - Main.screenPosition, null, Color.White, 0f, sunTex.Size() / 2f, 0.25f + (0.75f * te.ActiveState), SpriteEffects.None, 0f);
							}

							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);


						}

					}

				}
			}
		}

		public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
		{

			Tile tile = Main.tile[i, j];


			Texture2D texture;
			Texture2D texture2 = ModContent.GetTexture("SGAmod/Tiles/Monolith/CelestialMonolithTex");
			if (Main.canDrawColorTile(i, j))
			{
				texture = Main.tileAltTexture[Type, (int)tile.color()];
			}
			else
			{
				texture = Main.tileTexture[Type];
			}
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}

			int height = tile.frameY == 36 ? 18 : 16;
			int animate = 0;

			if (tile.frameY >= 56)
			{
				animate = Main.tileFrame[Type] * animationFrameHeight;
			}

			//int yFrame = j - (tile.frameY / 18) - (tile.frameY == 56 ? -3 : 0);
			//int teid = ModContent.GetInstance<CelestialMonolithTE>().Find(i - (tile.frameX / 18), yFrame);

			int yyy = Main.dayTime ? 2 : 3;

			Rectangle recta = new Rectangle(tile.frameX, (tile.frameY >= 56 ? tile.frameY - 56 : tile.frameY) + (int)(yyy * (56)), 16, height);

			if (tile.frameY < 56)
            {
				//recta = new Rectangle(tile.frameX, (tile.frameY) + (int)(yyy * (0)), 16, height);
			}

			Main.spriteBatch.Draw(texture2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, recta, Lighting.GetColor(i, j), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);

			//Main.spriteBatch.Draw(texture2, new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY + animate, 16, height), Lighting.GetColor(i, j), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
			return true;

		}

        public override bool NewRightClick(int i, int j)
        {
			Main.PlaySound(SoundID.Mech, i * 16, j * 16, 0);
			HitWire(i, j);
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.player[Main.myPlayer];
			player.noThrow = 2;
			player.showItemIcon = true;
			player.showItemIcon2 = ModContent.ItemType<CelestialMonolithItem>();
		}

		public override void HitWire(int i, int j)
		{
			int x = i - (Main.tile[i, j].frameX / 18) % 2;
			int y = j - (Main.tile[i, j].frameY / 18) % 3;
			for (int l = x; l < x + 2; l++)
			{
				for (int m = y; m < y + 3; m++)
				{
					if (Main.tile[l, m] == null)
					{
						Main.tile[l, m] = new Tile();
					}
					if (Main.tile[l, m].active() && Main.tile[l, m].type == Type)
					{
						if (Main.tile[l, m].frameY < 56)
						{
							Main.tile[l, m].frameY += 56;
						}
						else
						{
							Main.tile[l, m].frameY -= 56;
						}
					}
				}
			}
			if (Wiring.running)
			{
				Wiring.SkipWire(x, y);
				Wiring.SkipWire(x, y + 1);
				Wiring.SkipWire(x, y + 2);
				Wiring.SkipWire(x + 1, y);
				Wiring.SkipWire(x + 1, y + 1);
				Wiring.SkipWire(x + 1, y + 2);
			}
			NetMessage.SendTileSquare(-1, x, y + 1, 3);
			CelestialMonolithTE.ResetTEs();
		}
	}

	public class CelestialMonolithTE : ModTileEntity
	{

		public static List<CelestialMonolithTE> CelestialMonolithTileEntities = new List<CelestialMonolithTE>();
		private float _activeState = 0;
		public Vector2 RealPosition => (Position.ToVector2() * 16f) + new Vector2(16f, 32f);
		public float MaxRange => 2600f;
		public int timer = 0;
		public float ActiveState
        {
            get
            {
				return MathHelper.SmoothStep(0f, 1f, (_activeState*1f));
            }
        }
		public bool active = false;

		public override void NetSend(BinaryWriter writer, bool lightSend)
		{
			writer.Write(timer);
			writer.Write((double)_activeState);
			writer.Write(active);
		}

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
			timer = reader.ReadInt32();
			_activeState = (float)reader.ReadDouble();
			active = reader.ReadBoolean();
		}
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["timer"] = timer;
			tag["_activeState"] = _activeState;
			tag["active"] = active;

			return tag;
		}

		public override void Load(TagCompound tag)
        {
			timer = tag.GetInt("timer");
			_activeState = tag.GetFloat("_activeState");
			active = tag.GetBool("active");
		}

        public static void ResetTEs()
		{
			CelestialMonolithTileEntities.Clear();
			for (int i = 0; i < TileEntity.ByID.Count; i += 1)
			{
				TileEntity te;
				if (TileEntity.ByID.TryGetValue(i, out te))
				{
					if (te != null)
					{
						//Main.NewText("tester " + te.GetType().Name);
						if (te is CelestialMonolithTE cele)
						{
							CelestialMonolithTileEntities.Add(cele);
						}
					}
				}
			}
		}

		public override void Update()
		{
			Tile tile = Framing.GetTileSafely(Position.X, Position.Y);
			if (tile != null)
			{
				active = false;
				if (tile.frameY != 0)
				{
					active = true;
				}

				_activeState = MathHelper.Clamp(_activeState + (active ? 0.005f : -0.005f), 0f, 1f);
				if (_activeState > 0)
				{

					CelestialMonolithManager.queueRenderTargetUpdate = Math.Max(30, CelestialMonolithManager.queueRenderTargetUpdate);
					timer += 1;

					if (timer % 45 == 0)
					{
						Vector2 pos = RealPosition;
						float range = MaxRange * ActiveState;

						foreach (NPC npc in Main.npc.Where(testby => (testby.Center - pos).Length() < range))
						{
							npc.SGANPCs().invertedTime = 60;
						}
						foreach (Player player in Main.player.Where(testby => (testby.Center - pos).Length() < range))
						{
							player.SGAPly().invertedTime = 60;
						}
					}
				}
			}

		}

		public override bool ValidTile(int i, int j)
		{
			Tile tile = Main.tile[i, j];
			bool valid = tile.active() && tile.type == ModContent.TileType<CelestialMonolith>() && tile.frameX == 0;

			//if (_activeState<0.01f)
			//Tiles.Monolith.CelestialMonolithTE.ResetTEs();

			return valid;
		}

		public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
		{
			if (Main.netMode == 1)
			{
				NetMessage.SendTileRange(Main.myPlayer, i - 1, j - 2, 2, 3);
				NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i - 1, j - 2, Type, 0f, 0, 0, 0);
				return -1;
			}
			return Place(i - 1, j - 2);
		}
	}

}