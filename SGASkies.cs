using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.ModLoader;
using SGAmod.NPCs.Hellion;


namespace SGAmod
{
	public class HellionSky : CustomSky
	{
		private Random _random = new Random();
		private bool _isActive;
		private float[] xoffset = new float[200];
		private Color acolor = Color.White;
		private static bool spinornah=false;

		private static void drawit(Matrix zoomitz, float rotation = 0f, float scale = 1f)
		{
			if (Hellion.GetHellion() != null)
			{
				if (Hellion.GetHellion().GetType() == typeof(HellionFinal))
					HellionSky.spinornah = true;
				else
					HellionSky.spinornah = false;


			}
				
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, zoomitz);

			int width = (int)(200f); int height = (int)(200f);

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];


			///


			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					float dist = (new Vector2(x, y) - new Vector2(width / 2, height / 2)).Length();
						float alg = ((-Main.GlobalTime + ((float)(dist) / 0.5f)) / 1f);
						dataColors[x + y * width] = (Main.hslToRgb(alg % 1f, 0.75f, 0.5f));
				}
			}

			///


			beam.SetData(0, null, dataColors, 0, width * height);
			Color color = Color.White;
			float tempcolor = SGAmod.HellionSkyalpha;
			if (SGAmod.HellionSkyalpha > 0.30)
			{
				color = Color.Lerp(Color.White, Color.Black, Math.Min(0.9f, (SGAmod.HellionSkyalpha - 0.30f) * 3.50f));
				tempcolor *= 1.5f;
			}
			if (HellionSky.spinornah)
			{
				Main.spriteBatch.Draw(beam, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f), null, color * Math.Min(1f, tempcolor)*0.5f, Main.GlobalTime/4f, new Vector2(beam.Width / 2, beam.Height / 2), new Vector2(Main.screenWidth, Main.screenHeight) / new Vector2(1920, 1080) * 12f, SpriteEffects.None, 0f);
				Main.spriteBatch.Draw(beam, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f), null, color * Math.Min(1f, tempcolor)*0.5f, -Main.GlobalTime / 4f, new Vector2(beam.Width / 2, beam.Height / 2), new Vector2(Main.screenWidth, Main.screenHeight) / new Vector2(1920, 1080) * 12f, SpriteEffects.None, 0f);

			}
			else
			{
				Main.spriteBatch.Draw(beam, new Vector2(Main.screenWidth / 2f, Main.screenHeight / 2f), null, color * Math.Min(1f, tempcolor), rotation, new Vector2(beam.Width / 2, beam.Height / 2), new Vector2(Main.screenWidth, Main.screenHeight) / new Vector2(1920, 1080) * 10f, SpriteEffects.None, 0f);
			}
			//}

		}

		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			Filters.Scene["SGAmod:HellionSky"].GetShader().UseColor(0.5f, 0.5f, 0.5f).UseOpacity(SGAmod.HellionSkyalpha);
			float amax = 0f;
			Hellion nullg = Hellion.GetHellion();
		if (nullg != null)
		{
				if (nullg.introtimer > 199 && nullg.subphase>1)
				{
					amax = 0.05f;
				}

				if (nullg.tyrant>0)
				{
					amax = 0.25f;
				}

				if (nullg.npc.ai[1]< 99700 && nullg.npc.ai[1]>30000)
				{
					amax = 0.60f;
				}
			}
			else
			{
				amax = 1f;
			}

			int hellcount = NPC.CountNPCS((SGAmod.Instance).NPCType("Hellion")) + NPC.CountNPCS((SGAmod.Instance).NPCType("HellionFinal"));
			acolor = Main.hslToRgb((Main.GlobalTime / 10f) % 1, 0.81f, 0.5f);
			SGAmod.HellionSkyalpha = MathHelper.Clamp(hellcount > 0 ? (SGAmod.HellionSkyalpha + 0.015f / 5f) : SGAmod.HellionSkyalpha - (0.015f/5f), 0f, amax);
		}

		public override Color OnTileColor(Color inColor)
		{
			return acolor;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			double thevalue = Main.GlobalTime * 2.0;
			double movespeed = Main.GlobalTime * 0.2;

			//NPC theboss=Main.npc[NPC.FindFirstNPC((SGAmod.Instance).NPCType("Asterism"))];
			//float valie=((float)theboss.life/(float)theboss.lifeMax);
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			//var deathShader = GameShaders.Misc["WaterProcessor"];
			//GameShaders.Misc["WaterProcessor"].Apply(new DrawData?(new DrawData(this._distortionTarget, Vector2.Zero, Color.White)));
			//deathShader.UseOpacity(0.5f);
			//deathShader.Apply(null);
			if (maxDepth >= 0 && minDepth < 0)
			{
				Texture2D texa = ModContent.GetTexture("SGAmod/noise");

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

				HellionSky.drawit(Main.GameViewMatrix.ZoomMatrix);

				//Main.spriteBatch.End();
				//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

			}
		}

		public override float GetCloudAlpha()
		{
			return 1f - (0.75f * SGAmod.HellionSkyalpha);
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		public override void Reset()
		{
			this._isActive = false;
		}

		public override bool IsActive()
		{
			return this._isActive;
		}
	}




	public class ProgramSky : CustomSky
	{
		private Random _random = new Random();
		private bool _isActive;
		private float[] xoffset = new float[200];
		private Color acolor = Color.White;


		public override void OnLoad()
		{
		}

		public override void Update(GameTime gameTime)
		{
			acolor = Main.hslToRgb((Main.GlobalTime / 10f) % 1, 0.81f, 0.5f);
			SGAmod.ProgramSkyAlpha = MathHelper.Clamp(NPC.CountNPCS((SGAmod.Instance).NPCType("SPinky")) > 0 ? SGAmod.ProgramSkyAlpha + 0.005f : SGAmod.ProgramSkyAlpha - 0.005f, 0f, 1f);
		}

		private float GetIntensity()
		{
			return SGAmod.ProgramSkyAlpha;
		}

		public override Color OnTileColor(Color inColor)
		{
			return acolor;
		}

		public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
		{
			double thevalue = Main.GlobalTime * 2.0;
			double movespeed = Main.GlobalTime * 0.2;

			//NPC theboss=Main.npc[NPC.FindFirstNPC((SGAmod.Instance).NPCType("Asterism"))];
			//float valie=((float)theboss.life/(float)theboss.lifeMax);
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			//var deathShader = GameShaders.Misc["WaterProcessor"];
			//GameShaders.Misc["WaterProcessor"].Apply(new DrawData?(new DrawData(this._distortionTarget, Vector2.Zero, Color.White)));
			//deathShader.UseOpacity(0.5f);
			//deathShader.Apply(null);
			//if (maxDepth >= 0 && minDepth < 0)
			Single[,] singles = { { 3.40282347E+38f, 3.40282347E+38f },{4f,4f } };
			float[] alphaz = {1f,0.5f };
			float[] movedir = { 1f, 1f };
			for (int i = 0; i < 2; i += 1)
			{
				if (maxDepth >= singles[i,0] && minDepth < singles[i,1])
				{
					Texture2D texa = ModContent.GetTexture("SGAmod/noise");

					int sizechunk = texa.Width;
					for (int y = 0; y < Main.screenHeight + sizechunk; y += sizechunk)
					{
						for (int x = -sizechunk * 2; x < Main.screenWidth + sizechunk * 4; x += sizechunk)
						{
							//thevalue += (y * 0.15) + ((x) / 610);
							float thecoloralpha = 0.5f + (float)Math.Sin(thevalue) * 0.05f;


							Main.spriteBatch.End();
							Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
							ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowDye); shader.Apply(null);
							shader = GameShaders.Armor.GetShaderFromItemId(ItemID.MidnightRainbowDye);
							shader.Apply(null);
							spriteBatch.Draw(texa, new Rectangle(x - ((int)(Main.GlobalTime* movedir[i] * 30) % sizechunk * 3), y, sizechunk, sizechunk), (acolor) * (thecoloralpha * SGAmod.ProgramSkyAlpha* alphaz[i]));
						}
					}
				}
			}


			if (maxDepth >= 3.40282347E+38f && minDepth < 3.40282347E+38f)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				ArmorShaderData shader2 = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye); shader2.Apply(null);
				Texture2D sun = ModContent.GetTexture("Terraria/Sun");
				spriteBatch.Draw(sun, new Vector2(Main.screenWidth / 2, Main.screenHeight / 8), null, Color.DeepPink * SGAmod.ProgramSkyAlpha, 0, new Vector2(sun.Width / 2f, sun.Height / 2f), new Vector2(5f, 5f) * SGAmod.ProgramSkyAlpha, SpriteEffects.None, 0f);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.Transform);

		}

		public override float GetCloudAlpha()
		{
			return 1f-(0.75f*SGAmod.ProgramSkyAlpha);
		}

		public override void Activate(Vector2 position, params object[] args)
		{
			this._isActive = true;
		}

		public override void Deactivate(params object[] args)
		{
			this._isActive = false;
		}

		public override void Reset()
		{
			this._isActive = false;
		}

		public override bool IsActive()
		{
			return this._isActive;
		}
	}

}
