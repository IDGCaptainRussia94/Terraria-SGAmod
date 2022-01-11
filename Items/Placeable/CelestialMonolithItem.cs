using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Tiles.Monolith;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{



	public class CelestialMonolithManager
    {

		public static RenderTarget2D CarveOutTarget2D;
		public static RenderTarget2D SkyRenderTarget2D;
		public static int queueRenderTargetUpdate = 0;
		public static bool onlyDrawSky = false;

		public static double GetInvertedTime(double timeOfDay) => timeOfDay / (Main.dayTime ? Main.dayLength : Main.nightLength);

		public static void Unload()
        {
            if (CarveOutTarget2D != null && !CarveOutTarget2D.IsDisposed)
                CarveOutTarget2D.Dispose();
            if (SkyRenderTarget2D != null && !SkyRenderTarget2D.IsDisposed)
                SkyRenderTarget2D.Dispose();
        }

		public static void SGAmod_RenderTargetsCheckEvent(ref bool yay)
		{
			yay &= !((CarveOutTarget2D == null || CarveOutTarget2D.IsDisposed) || (SkyRenderTarget2D == null || SkyRenderTarget2D.IsDisposed));
		}

		public static void SGAmod_RenderTargetsEvent()
		{
			if (CelestialMonolithManager.SkyRenderTarget2D == null || CelestialMonolithManager.SkyRenderTarget2D.IsDisposed)
			{
				int width = Main.screenWidth;
				int height = Main.screenHeight;
				CelestialMonolithManager.SkyRenderTarget2D = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
			}

			if (CelestialMonolithManager.CarveOutTarget2D == null || CelestialMonolithManager.CarveOutTarget2D.IsDisposed)
			{
				int width = Main.screenWidth;
				int height = Main.screenHeight;
				CelestialMonolithManager.CarveOutTarget2D = new RenderTarget2D(Main.graphics.GraphicsDevice, width, height, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
			}
		}

		public static void DrawMonolithAura()
		{
			if (queueRenderTargetUpdate > 0)
			{
				if (CarveOutTarget2D != null && !CarveOutTarget2D.IsDisposed)
				{

					Main.spriteBatch.End();
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, default, default, default, Matrix.Identity);

					Effect ShadowEffect = NPCs.Hellion.ShadowParticle.ShadowEffect;

					Color colorIn = Main.dayTime ? Color.Lerp(Color.Blue, Color.Cyan, 0.50f) : Color.Lerp(Color.Orange, Color.Yellow, 0.50f);
					Color colorOut = Color.White;

					ShadowEffect.Parameters["overlayTexture"].SetValue(CarveOutTarget2D);
					ShadowEffect.Parameters["noiseTexture"].SetValue(SkyRenderTarget2D);

					ShadowEffect.Parameters["colorAmmount"].SetValue(64);
					ShadowEffect.Parameters["screenSize"].SetValue(SkyRenderTarget2D.Size());
					//Color.DarkMagenta
					//Color.Black
					ShadowEffect.Parameters["colorFrom"].SetValue((colorIn).ToVector4());
					ShadowEffect.Parameters["colorTo"].SetValue((colorOut).ToVector4());
					ShadowEffect.Parameters["colorOutline"].SetValue((Main.dayTime ? Color.Aqua : Color.Yellow).ToVector4() * 1f);

					ShadowEffect.Parameters["edgeSmooth"].SetValue(0.05f);
					ShadowEffect.Parameters["invertLuma"].SetValue(false);
					ShadowEffect.Parameters["alpha"].SetValue(1f);

					//Show through
					float percent = 1f;

					ShadowEffect.Parameters["noisePercent"].SetValue(percent);
					ShadowEffect.Parameters["noiseScalar"].SetValue(new Vector4(0, 0, 1f, 1f));

					ShadowEffect.CurrentTechnique.Passes["ColorFilter"].Apply();

					Main.spriteBatch.Draw(SkyRenderTarget2D, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);

					//Main.spriteBatch.End();
					//Main.spriteBatch.Begin(default, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, default, Main.GameViewMatrix.TransformationMatrix);
				}
			}
		}

		internal static void SGAmod_PostUpdateEverythingEvent()
		{

			if (CelestialMonolithManager.queueRenderTargetUpdate > 0)
			{
				CelestialMonolithManager.queueRenderTargetUpdate -= 1;
				if (Main.dedServ)
					return;

				if (CelestialMonolithManager.queueRenderTargetUpdate > 1)
				{

					SGAmod_RenderTargetsEvent();

					RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

					Main.graphics.GraphicsDevice.SetRenderTarget(CelestialMonolithManager.SkyRenderTarget2D);
					Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                    //Main.spriteBatch.Begin();

                    double timeOfDay = Main.time;
					double perc = GetInvertedTime(timeOfDay);
					bool day = Main.dayTime;

					Main.dayTime = !Main.dayTime;
					Main.time = (Main.dayTime ? Main.dayLength : Main.nightLength)* perc;

					GameTime gm = new GameTime();

					//Main.spriteBatch.Begin();


					Effect effect = SGAmod.TrailEffect;
					effect.Parameters["WorldViewProjection"].SetValue(Effects.WVP.View(Vector2.One) * Effects.WVP.Projection());
					effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));
					effect.Parameters["coordOffset"].SetValue(Main.screenPosition/120000f);
					effect.Parameters["coordMultiplier"].SetValue(new Vector2(0.5f, 0.35f));
					effect.Parameters["strength"].SetValue(!day ? 1f : 0.5f);

					VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];
					VertexBuffer vertexBuffer;

					Vector3 screenPos = new Vector3(-16, 0, 0);

					Color colorsa2 = day ? Color.Lerp(Color.Black, Color.Gray, 0.65f) : Color.Lerp(Color.Black,Color.Gray,0.25f);
					Color colorsa = Color.Lerp(!day ? Color.Yellow : Color.Blue, Color.Gray, 0.50f);

					typeof(Main).GetField("bgColor", SGAmod.UniversalBindingFlags).SetValue(Main.instance, !day ? Color.Lerp(Color.Yellow, Color.White, 0.50f) : Color.Lerp(Color.Turquoise, Color.White, 0.20f));
					typeof(Main).GetField("trueBackColor", SGAmod.UniversalBindingFlags).SetValue(Main.instance, !day ? Color.Lerp(Color.Orange, Color.White, 0.50f) : Color.Lerp(Color.CornflowerBlue, Color.White, 0.20f));

					vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-16, -16, 0), colorsa, new Vector2(0, 0));
					vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight + 16, 0), colorsa2, new Vector2(0, 1));
					vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, -16, 0), colorsa, new Vector2(1, 0));

					vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, Main.screenHeight + 16, 0), colorsa2, new Vector2(1, 1));
					vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight + 16, 0), colorsa2, new Vector2(0, 1));
					vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, -16, 0), colorsa, new Vector2(1, 0));

					vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
					vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

					Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

					RasterizerState rasterizerState = new RasterizerState();
					rasterizerState.CullMode = CullMode.None;
					Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

					effect.CurrentTechnique.Passes["BasicEffectPass"].Apply();
					Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);






					onlyDrawSky = true;
					typeof(Main).GetMethod("DoDraw", SGAmod.UniversalBindingFlags).Invoke(Main.instance, new object[1] { gm });

					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Matrix.Identity);

					//Main.spriteBatch.Draw(Main.backgroundTexture[0], Vector2.Zero, null, Color.White, 0f, Vector2.Zero, new Vector2(32, 16), SpriteEffects.None, 0f);
					Main.spriteBatch.End();


					Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.instance.Rasterizer, null, Main.BackgroundViewMatrix.TransformationMatrix);

					if (Main.dayTime)
					Main.spriteBatch.Draw(Main.sunTexture, SGAUtils.SunPosition(), null, Color.White, 0f, Main.sunTexture.Size() / 2f, 1.25f, SpriteEffects.None, 0f);
					else
					Main.spriteBatch.Draw(Main.moonTexture[Main.moonType], SGAUtils.SunPosition(), new Rectangle(0,Main.moonPhase*(Main.moonTexture[Main.moonType].Width),(Main.moonTexture[Main.moonType].Width),(Main.moonTexture[Main.moonType].Width)), Color.White, 0f,new Vector2(Main.moonPhase * (Main.moonTexture[Main.moonType].Width), Main.moonPhase * (Main.moonTexture[Main.moonType].Width)) / 2f, 1.25f, SpriteEffects.None, 0f);



					typeof(Main).GetMethod("DrawSurfaceBG", SGAmod.UniversalBindingFlags).Invoke(Main.instance, new object[0] { });
					onlyDrawSky = false;

					Main.spriteBatch.End();

                    #region OldSurfaceDrawCode
                    /*
					Texture2D tex = Main.backgroundTexture[2];
					if (tex != null)
					{
						for (int i = 1; i >= 0; i -= 1)
						{
							effect.Parameters["WorldViewProjection"].SetValue(Effects.WVP.View(Vector2.One) * Effects.WVP.Projection());

							Vector2 sizer = (new Vector2(Main.screenWidth, Main.screenHeight) / new Vector2(tex.Width / 3f, tex.Height)) / 4f;

							effect.Parameters["imageTexture"].SetValue(Main.backgroundTexture[2]);
							effect.Parameters["coordOffset"].SetValue(new Vector2(Main.screenPosition.X / (Main.screenWidth / sizer.X), 0f));
							effect.Parameters["coordMultiplier"].SetValue(sizer);
							effect.Parameters["strength"].SetValue(1f);

							vertices = new VertexPositionColorTexture[6];

							screenPos = new Vector3(0, 0, 0);

							colorsa2 = Color.Red * 0f;
							colorsa = Color.White;

							if (i > 0)
							{
								colorsa2 = Color.Black;
								colorsa = Color.Black;
							}

							float yyy = (float)(((Main.screenPosition.Y / 16.0) * 16.0) - Main.worldSurface * 16.0);
							yyy = (Main.screenHeight - yyy) - Main.screenHeight;
							Vector2 top = new Vector2(-16, yyy);
							Vector2 bottom = new Vector2(Main.screenWidth + 16, i > 0 ? Main.screenHeight : (yyy + 256f));

							vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(top.X, top.Y, 0), colorsa, new Vector2(0, 0));
							vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(top.X, bottom.Y, 0), colorsa2, new Vector2(0, 1));
							vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(bottom.X, top.Y, 0), colorsa, new Vector2(1, 0));

							vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(bottom.X, bottom.Y, 0), colorsa2, new Vector2(1, 1));
							vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(top.X, bottom.Y, 0), colorsa2, new Vector2(0, 1));
							vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(bottom.X, top.Y, 0), colorsa, new Vector2(1, 0));

							vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
							vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

							Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

							rasterizerState = new RasterizerState();
							rasterizerState.CullMode = CullMode.None;
							Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

							effect.CurrentTechnique.Passes["BasicEffectPass"].Apply();
							Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
						}
					}

					*/
                    #endregion

                    Main.dayTime = day;
					Main.time = timeOfDay;

					Main.graphics.GraphicsDevice.SetRenderTarget(CelestialMonolithManager.CarveOutTarget2D);
					Main.graphics.GraphicsDevice.Clear(Color.Transparent);

					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicWrap, default, default, default, Main.GameViewMatrix.ZoomMatrix);

					Texture2D glowOrb = SGAmod.Instance.GetTexture("Extra_49c");

					int index = 0;
					foreach (CelestialMonolithTE cte in CelestialMonolithTE.CelestialMonolithTileEntities)
					{
						index++;
						float rangeWidth = (cte.MaxRange / glowOrb.Width)*(2.30f);
						Vector2 pos = (cte.RealPosition);
						Main.spriteBatch.Draw(glowOrb, pos + new Vector2(index * 0, 0) - Main.screenPosition, null, Color.White, 0f, glowOrb.Size() / 2f, rangeWidth * cte.ActiveState, SpriteEffects.None, 0f);
					}
					Main.spriteBatch.End();



					Main.graphics.GraphicsDevice.SetRenderTargets(binds);


				}
				if (CelestialMonolithManager.queueRenderTargetUpdate == 1)
				{
					Unload();
					/*
					if (CelestialMonolithManager.SkyRenderTarget2D != null && !CelestialMonolithManager.SkyRenderTarget2D.IsDisposed)
					{
						CelestialMonolithManager.SkyRenderTarget2D.Dispose();
					}
					if (CelestialMonolithManager.CarveOutTarget2D != null && !CelestialMonolithManager.CarveOutTarget2D.IsDisposed)
					{
						CelestialMonolithManager.CarveOutTarget2D.Dispose();
					}
					*/
				}
			}

		}





	}

	public class CelestialMonolithItem : ModItem
	{
		public override bool Autoload(ref string name)
		{
			SGAmod.PostUpdateEverythingEvent += CelestialMonolithManager.SGAmod_PostUpdateEverythingEvent;
			SGAmod.RenderTargetsCheckEvent += CelestialMonolithManager.SGAmod_RenderTargetsCheckEvent;
			SGAmod.RenderTargetsEvent += CelestialMonolithManager.SGAmod_RenderTargetsEvent;

			return true;
		}
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Monolith");
			Tooltip.SetDefault("'A device able to project the world in a different 'light' '\nWhen activated, creates a small time shift where sun and moon are swapped");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 32;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.rare = 10;
			item.value = Item.buyPrice(0, 10, 0, 0);
			item.createTile = ModContent.TileType<CelestialMonolith>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SunStone, 1);
			recipe.AddIngredient(ItemID.MoonStone, 1);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 12);
			recipe.AddIngredient(ModContent.ItemType<HeliosFocusCrystal>(), 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}