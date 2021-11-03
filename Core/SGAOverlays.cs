using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.GameContent.UI;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.Graphics;
using ReLogic.Graphics;
using SGAmod.SkillTree;
using SGAmod.Dimensions;
using SGAmod.Items;
using System.Reflection;

namespace SGAmod
{

	public abstract class SGAInterface
	{

		public static UncraftClass Uncrafts = default;

		public static void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{

			if (SGAmod.SkillUIActive)
			{
				layers.Clear();
				layers.Add(new LegacyGameInterfaceLayer(
					"SGAmod: SkillUI", SKillUI.DrawSkillUI,
					InterfaceScaleType.UI)
				);
				return;
			}

			int foundIndex = 0;
			int foundLastIndex = layers.Count-1;

			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k].Name == "Vanilla: Resource Bars")
				{
					foundIndex = k+1;
					foundLastIndex = foundIndex;
					break;
				}
			}

			layers.Insert(foundIndex, new LegacyGameInterfaceLayer("SGAmod: HUD", DrawHUD, InterfaceScaleType.UI));
			//layers.Insert(foundLastIndex, new LegacyGameInterfaceLayer("SGAmod: Over HUD", DrawOverHUD, InterfaceScaleType.UI));

			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"SGAmod: CustomUI", DrawUI,
					InterfaceScaleType.UI)
				);
				/*layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
				"SGAmod: CustomUI", DrawUI,
				InterfaceScaleType.UI)
);*/
			}
			layers.Insert(0, new LegacyGameInterfaceLayer("SGAmod: UnderHUD", DrawUnderHUD, InterfaceScaleType.Game));
			//layers.Insert(0, new LegacyGameInterfaceLayer("SGAmod: Effects", DimDingeonsWorld.DrawSectors, InterfaceScaleType.Game));

		}

		internal static void HUDCode(int type, (int, int, int, int) itta = default)
		{
			if (type == 0)
			{
				SGAmod.VanillaHearts.Item1 = Main.heartTexture;
				SGAmod.VanillaHearts.Item2 = Main.heart2Texture;

				if (Main.heartTexture == SGAmod.Instance.GetTexture("Invisible"))
				{
					Main.heartTexture = SGAmod.OGVanillaHearts.Item1;
					Main.heart2Texture = SGAmod.OGVanillaHearts.Item2;
				}
			}
			if (type == 2)
			{
				Main.heartTexture = SGAmod.VanillaHearts.Item1;
				Main.heart2Texture = SGAmod.VanillaHearts.Item2;

				if (Main.heartTexture == SGAmod.Instance.GetTexture("Invisible"))
				{
					Main.heartTexture = SGAmod.OGVanillaHearts.Item1;
					Main.heart2Texture = SGAmod.OGVanillaHearts.Item2;
				}
			}
			if (type == 1)
			{

				int i = itta.Item1;

				SGAPlayer sgaply = Main.LocalPlayer.SGAPly();

				if (sgaply.energyShieldReservation > 0f)
				{
					int UI_ScreenAnchorX = (int)(typeof(Main).GetField("UI_ScreenAnchorX", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null));

					Player player = sgaply.player;

					int totalhearts = Math.Min((int)Math.Ceiling((decimal)player.statLifeMax / 20), 20);

					float UIDisplay_LifePerHeart = totalhearts;


					//int num2 = (player.statLifeMax - 400) / 5;

					int num = player.statLifeMax2;

					//Main.NewText(totalhearts);

					float startingheartindex = ((num * (1f-sgaply.energyShieldReservation)) / num) * UIDisplay_LifePerHeart;

					Main.heartTexture = SGAmod.VanillaHearts.Item1;
					Main.heart2Texture = SGAmod.VanillaHearts.Item2;

					//SGAmod.VanillaHearts.Item1 = Main.heartTexture;
					//SGAmod.VanillaHearts.Item2 = Main.heart2Texture;

					if (i > startingheartindex)
					{

						float start = (i - startingheartindex);

						float energy = MathHelper.Clamp((((sgaply.GetEnergyShieldAmmountAndRecharge.Item1 / (float)sgaply.GetEnergyShieldAmmountAndRecharge.Item2)) * (UIDisplay_LifePerHeart - (startingheartindex - 1))) - start, 0f, 1f);

						int num8 = itta.Item2;
						float scale = 1f;
						int num6 = itta.Item3;
						int num9 = itta.Item4;

						Main.heartTexture = SGAmod.Instance.GetTexture("Invisible");
						Main.heart2Texture = SGAmod.Instance.GetTexture("Invisible");

						Texture2D heartTexture = SGAmod.Instance.GetTexture("GreyHeart");
						Texture2D heartTexture2 = SGAmod.Instance.GetTexture("ShieldHealth");

						//Main.spriteBatch.Draw(heartTexture, new Vector2(0,0), null, Color.White, 0f, new Vector2(heartTexture.Width / 2, heartTexture.Height / 2), num6, SpriteEffects.None, 0f);

						Color colorDye = Main.LocalPlayer.SGAPly().jellybruSet ? Color.HotPink : Color.White;

						//Zap!
						int counter = (Main.LocalPlayer.miscCounter + i * 73) / 4;
						Rectangle glowrect = new Rectangle(0, (counter % 7) * (Main.glowMaskTexture[25].Height / 7), Main.glowMaskTexture[25].Width, Main.glowMaskTexture[25].Height / 7);

						Main.spriteBatch.Draw(Main.glowMaskTexture[25], new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + heartTexture.Width / 2, 32f + ((float)heartTexture.Height - (float)heartTexture.Height * 1f) / 2f + (float)num9 + (float)(heartTexture.Height / 2)), glowrect, Color.Lerp(colorDye,Color.Black,0f) * energy, MathHelper.Pi, glowrect.Size() / 2f, 0.75f + (energy * 0.25f),
							((counter % 14) > 6 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | ((counter % 28) > 13 ? SpriteEffects.FlipVertically : SpriteEffects.None), 0f);

						//Hearts
						Main.spriteBatch.Draw(heartTexture, new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + heartTexture.Width / 2, 32f + ((float)heartTexture.Height - (float)heartTexture.Height * 1f) / 2f + (float)num9 + (float)(heartTexture.Height / 2)), null, Color.White * 0.50f, 0, new Vector2(heartTexture.Width / 2, heartTexture.Height / 2), 0.75f, SpriteEffects.None, 0f);

						Main.spriteBatch.Draw(heartTexture2, new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + heartTexture.Width / 2, 32f + ((float)heartTexture.Height - (float)heartTexture.Height * 1f) / 2f + (float)num9 + (float)(heartTexture.Height / 2)), null, colorDye * energy, 0, new Vector2(heartTexture.Width / 2, heartTexture.Height / 2), 0.75f + (energy * 0.25f), SpriteEffects.None, 0f);

						//Zap again!
						Main.spriteBatch.Draw(Main.glowMaskTexture[25], new Vector2(500 + 26 * (i - 1) + num8 + UI_ScreenAnchorX + heartTexture.Width / 2, 32f + ((float)heartTexture.Height - (float)heartTexture.Height * 1f) / 2f + (float)num9 + (float)(heartTexture.Height / 2)), glowrect, colorDye * energy, 0, glowrect.Size() / 2f, 0.75f + (energy * 0.25f), 
							((counter % 14)>6 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | ((counter % 28) > 13 ? SpriteEffects.FlipVertically : SpriteEffects.None), 0f);

					}

				}

			}
		}

		public static bool DrawUI()
		{
			if (SGAmod.CustomUIMenu.visible) 
			{
				SGAmod.CustomUIMenu.Draw(Main.spriteBatch);
			}
			if (Main.playerInventory)
			{
				if (SGAmod.ArmorButtonUpdate && Main.EquipPage == 0)
				SGAmod.armorButton.Draw(Main.spriteBatch);

				if (Main.LocalPlayer.SGAPly().benchGodFavor)
				SGAmod.craftBlockPanel.Draw(Main.spriteBatch);
			}
			return true;
		}

		public static bool DrawUnderHUD()
		{

			if (Main.gameMenu || SGAmod.Instance == null && !Main.dedServ)
				return true;

			SGAPlayer sga = Main.LocalPlayer.SGAPly();

			if (Uncrafts != default)
			{
				Uncrafts.Draw();
				Uncrafts = default;
			}



			if (sga.gunslingerLegendtarget > -1)
			{

				NPC target = Main.npc[sga.gunslingerLegendtarget];

				bool sizeup = false;
				for (int i = 0; i < 360; i += 360 / 8)
				{
					sizeup = !sizeup;
					float angle = MathHelper.ToRadians(i+((sizeup ? 400f : -400f)*MathHelper.Clamp(1f- ((float)sga.lockoneffect/70f), 0f,1f)));
					Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (180f+MathHelper.Clamp(300- sga.lockoneffect*4,0,300));

					Texture2D arrow = ModContent.GetTexture("SGAmod/MatrixArrow");

					Vector2 drawPos = ((hereas * (sizeup ? 1f : 1f)*Main.essScale) + target.Center) - Main.screenPosition;
					float sizer = (sizeup ? 0.5f : 1f);
					Main.spriteBatch.Draw(arrow, drawPos, null, Main.hslToRgb(((i/360f)-Main.GlobalTime*1f)%1f,1f,0.75f)*MathHelper.Clamp((float)sga.lockoneffect/70f,0f,1f), MathHelper.ToRadians(i)+MathHelper.Pi, new Vector2(arrow.Width* sizer, arrow.Height/2), new Vector2(1, 1), SpriteEffects.None, 0f);

				}
			}

			return true;
		}

		public static bool DrawOverHUD()
		{

			if (Main.gameMenu || SGAmod.Instance == null && !Main.dedServ)
				return false;

			SpriteBatch spriteBatch = Main.spriteBatch;

			SGAPlayer sga = Main.LocalPlayer.SGAPly();

			if (sga.GetEnergyShieldAmmountAndRecharge.Item1 > 0 && sga.GetEnergyShieldAmmountAndRecharge.Item2 > 0)
			{

				Vector2 drawPos = new Vector2(Main.screenWidth-44,42);

				Texture2D stain = SGAmod.Instance.GetTexture("TiledPerlin");
				float percentit = (sga.GetEnergyShieldAmmountAndRecharge.Item1 / (float)sga.GetEnergyShieldAmmountAndRecharge.Item2);
				int barlength = (int)((255f * (percentit)) *Main.UIScale);
				int height = (int)(24 * Main.UIScale);

				spriteBatch.End();
				//Matrix Custommatrix = Matrix.CreateScale(Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

				//DrawData value28 = new DrawData(stain, new Vector2(240, 240), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 120, 120)), Microsoft.Xna.Framework.Color.White, 0, stain.Size() / 2f, 1f, SpriteEffects.None, 0);

				/*DrawData value28 = new DrawData(stain, new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 320, 320)), Microsoft.Xna.Framework.Color.White, 0, stain.Size() / 2f, 1f, SpriteEffects.None, 0);

				ArmorShaderData stardustsshader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.NebulaDye);
				stardustsshader3.UseColor(Color.Blue.ToVector3());
				stardustsshader3.UseOpacity(0.5f);
				stardustsshader3.Apply(null, new DrawData?(value28));

				spriteBatch.Draw(stain, drawPos, new Rectangle(0, 0, size, 48), Color.Blue * 0.50f, MathHelper.Pi, Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, 0f);
				*/

				VertexBuffer vertexBuffer;

				Vector3 d3pos = new Vector3(drawPos.X, drawPos.Y, 0)*Main.UIScale;

				Effect effect = SGAmod.TrailEffect;

				effect.Parameters["WorldViewProjection"].SetValue(Effects.WVP.View(Main.GameViewMatrix.Zoom) * Effects.WVP.Projection());
				effect.Parameters["imageTexture"].SetValue(stain);
				effect.Parameters["coordOffset"].SetValue(new Vector2(0, Main.GlobalTime*-0.1f));
				effect.Parameters["coordMultiplier"].SetValue(new Vector2(0.3f,0.1f));
				effect.Parameters["strength"].SetValue(MathHelper.Clamp(1.5f,0,3));

				VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

				Vector3 screenPos = new Vector3(-16, 0, 0);

				vertices[0] = new VertexPositionColorTexture(d3pos + new Vector3(0, 0, 0), Color.Blue, new Vector2(0, 0));
				vertices[1] = new VertexPositionColorTexture(d3pos + new Vector3(0, height, 0), Color.Blue, new Vector2(0, 1));
				vertices[2] = new VertexPositionColorTexture(d3pos + new Vector3(-barlength, 0, 0), Color.Blue, new Vector2(percentit, 0));

				vertices[3] = new VertexPositionColorTexture(d3pos + new Vector3(-barlength, height, 0), Color.Blue, new Vector2(percentit, 1));
				vertices[4] = new VertexPositionColorTexture(d3pos + new Vector3(0, height, 0), Color.Blue, new Vector2(0, 1));
				vertices[5] = new VertexPositionColorTexture(d3pos + new Vector3(-barlength, 0, 0), Color.Blue, new Vector2(percentit, 0));;

				vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
				vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

				Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

				RasterizerState rasterizerState = new RasterizerState();
				rasterizerState.CullMode = CullMode.None;
				Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

				effect.CurrentTechnique.Passes["FadedBasicEffectPassY"].Apply();
				Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

			}

			spriteBatch.End();
			//Matrix Custommatrix = Matrix.CreateScale(Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);
			spriteBatch.Begin(SpriteSortMode.Deferred, default, default, default, default, null, Matrix.Identity);


			return true;
		}

			public static bool DrawHUD()
		{

			if (Main.gameMenu || SGAmod.Instance == null && !Main.dedServ)
				return false;
			Player locply = Main.LocalPlayer;
			if (locply == null)
				return false;
			if (locply != null && locply.whoAmI == Main.myPlayer)
			{
				SpriteBatch spriteBatch = Main.spriteBatch;

				if (locply.HeldItem.type == SGAmod.Instance.ItemType("CaliburnCompess"))
				{
					spriteBatch.End();
					//Matrix Custommatrix = Matrix.CreateScale(Main.screenWidth / 1920f, Main.screenHeight / 1024f, 0f);
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
					for (int i = 0; i < SGAWorld.CaliburnAlterCoordsX.Length; i += 1)
					{
						string[] texs = { "SGAmod/Items/Weapons/Caliburn/CaliburnTypeA", "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB", "SGAmod/Items/Weapons/Caliburn/CaliburnTypeC" };
						Texture2D tex = ModContent.GetTexture(texs[i]);

						Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

						Vector2 drawPos = (new Vector2(Main.screenWidth, Main.screenHeight) / 2f) * Main.UIScale;

						Vector2 Vecd = (new Vector2(SGAWorld.CaliburnAlterCoordsX[i], SGAWorld.CaliburnAlterCoordsY[i] + 96) - (drawPos + Main.screenPosition));
						float pointthere = Vecd.ToRotation();
						bool flip = Vecd.X > 0;

						spriteBatch.Draw(tex, drawPos + (pointthere.ToRotationVector2() * 64f) + (pointthere.ToRotationVector2() * (float)Math.Pow(Vecd.Length(), 0.9) / 50), null, Color.White, pointthere + MathHelper.ToRadians(45) + (flip ? MathHelper.ToRadians(-90) * 3f : 0), drawOrigin, Main.UIScale, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
						//spriteBatch.Draw(tex, new Vector2(150, 150), null, Color.White, Main.GlobalTime, drawOrigin, 1, flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);

					}

					if (SGAWorld.darknessVision && DimDingeonsWorld.darkSectors.Count > 0)
					{
                        for (int i = 0; i < DimDingeonsWorld.darkSectors.Count; i += 1)
                        {

							Texture2D tex = ModContent.GetTexture("SGAmod/Items/WatchersOfNull");
							Texture2D tex2 = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];
							Rectangle rect = new Rectangle(0, 0, tex.Width, tex.Height / 13);

							Vector2 drawOrigin = new Vector2(tex.Width, tex.Height/13) / 2f;

							Vector2 drawPos = (new Vector2(Main.screenWidth, Main.screenHeight) / 2f) * Main.UIScale;

							DarkSector sector = DimDingeonsWorld.darkSectors[i];

							Vector2 Vecd = (sector.position.ToVector2()*16) - (drawPos + Main.screenPosition);
							float pointthere = Vecd.ToRotation();

							for(int k=-1;k<3;k+=2)
							spriteBatch.Draw(tex2, drawPos + (pointthere.ToRotationVector2() * 64f) + (pointthere.ToRotationVector2() * (float)Math.Pow(Vecd.Length(), 0.9) / 50), null, Color.Black*0.4f, Main.GlobalTime * 2f*k, tex2.Size() / 2f, Main.UIScale*1.25f, SpriteEffects.FlipHorizontally, 0f);
							spriteBatch.Draw(tex, drawPos + (pointthere.ToRotationVector2() * 64f) + (pointthere.ToRotationVector2() * (float)Math.Pow(Vecd.Length(), 0.9) / 50), rect, Color.White*Main.essScale, 0, drawOrigin, Main.UIScale,SpriteEffects.FlipHorizontally, 0f);


						}
					}

					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				}

				if (!locply.dead)
				{

					//Plamsa Clip
					if (SGAmod.UsesPlasma.ContainsKey(locply.HeldItem.type))
					{
						SGAmod mod = SGAmod.Instance;
						SGAPlayer modply = locply.GetModPlayer<SGAPlayer>();
						int maxclip;
						bool check = SGAmod.UsesPlasma.TryGetValue(locply.HeldItem.type, out maxclip);

						if (check)
						{
							Color color = Lighting.GetColor((int)locply.Center.X / 16, (int)locply.Center.Y / 16);
							Texture2D texture = mod.GetTexture("Items/PlasmaCell");
							int drawX = (int)(((0)));
							int drawY = (int)(((-36)));//gravDir 

							spriteBatch.End();
							spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(locply.Center.X - Main.screenPosition.X, locply.Center.Y - Main.screenPosition.Y, 0));


							float percent = ((float)modply.plasmaLeftInClip / (float)modply.plasmaLeftInClipMax);

							//DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f), (float)Math.PI, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
							//DrawData data2 = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), Color.White, (float)Math.PI, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);

							spriteBatch.Draw(texture, new Vector2(drawX, drawY), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f), (float)Math.PI, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
							spriteBatch.Draw(texture, new Vector2(drawX, drawY), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), Color.White, (float)Math.PI, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);

						}
					}

					//Ammo Clips
					if (SGAmod.UsesClips.ContainsKey(locply.HeldItem.type))
					{
						SGAmod mod = SGAmod.Instance;
						SGAPlayer modply = locply.GetModPlayer<SGAPlayer>();
						int maxclip;
						bool check = SGAmod.UsesClips.TryGetValue(locply.HeldItem.type, out maxclip);
						maxclip += modply.ammoLeftInClipMax - 6;

						if (check)
						{
							Texture2D texture = mod.GetTexture("AmmoHud");
							int drawX = (int)(((-texture.Width / 2f)));
							int drawY = (int)(((-32)));//gravDir 

							spriteBatch.End();
							spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(locply.Center.X - Main.screenPosition.X, locply.Center.Y - Main.screenPosition.Y, 0));


							for (int q = 0; q < modply.ammoLeftInClip; q++)
							{
								//DrawData data = new DrawData(texture, new Vector2((drawX - (q * texture.Width)) + (int)((maxclip * texture.Width) / 2), drawY), null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);
								spriteBatch.Draw(texture, new Vector2((drawX - (q * texture.Width)) + (int)((maxclip * texture.Width) / 2), drawY), null, Color.White, 0f, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0);

							}
						}
					}

				}

				//if (SGAmod.UsesClips.ContainsKey(locply.HeldItem.type))
				//{
				if (!locply.dead)
				{
					SGAmod mod = SGAmod.Instance;
					SGAPlayer modply = locply.GetModPlayer<SGAPlayer>();

					float perc = (float)modply.boosterPowerLeft / (float)modply.boosterPowerLeftMax;

					Texture2D texture = mod.GetTexture("BoostBar");

					int offsetY = -texture.Height+SGAConfigClient.Instance.HUDDisplacement;

					if (perc > 0)
					{

						float drawcolortrans = MathHelper.Clamp((modply.boosterdelay + 100) / 100f, 0f, 1f);

							spriteBatch.End();

							Vector2 scaler = new Vector2(modply.boosterPowerLeftMax / 300f, 1);
							int drawX = (int)(((locply.position.X + (locply.width / 2))) - Main.screenPosition.X);
							int drawY = (int)((locply.position.Y + (locply.gravDir == 1 ? locply.height + 10 : -10)) - Main.screenPosition.Y);//gravDir 

							spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(drawX, drawY, 0));

						if (drawcolortrans > 0f)
						{
							spriteBatch.Draw(texture, new Vector2(-scaler.X - 2, offsetY), new Rectangle(0, 0, 2, texture.Height), Color.White * drawcolortrans, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
							spriteBatch.Draw(texture, new Vector2(-scaler.X, offsetY), new Rectangle(2, 0, 2, texture.Height), Color.DarkGray * drawcolortrans, 0f, new Vector2(0, 0), scaler, SpriteEffects.None, 0);
							spriteBatch.Draw(texture, new Vector2(-scaler.X, offsetY), new Rectangle(2, 0, 2, texture.Height), Color.Orange * drawcolortrans, 0f, new Vector2(0, 0), new Vector2(scaler.X * perc, scaler.Y), SpriteEffects.None, 0);
							spriteBatch.Draw(texture, new Vector2(+scaler.X, offsetY), new Rectangle(4, 0, 2, texture.Height), Color.White * drawcolortrans, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
							offsetY += texture.Height;
						}
					}

					if (modply.electricChargeMax > 0)
					{
						perc = (float)modply.electricCharge / (float)modply.electricChargeMax;
						if (perc > 0)
						{

							spriteBatch.End();

							Vector2 scaler = new Vector2(modply.electricChargeMax / 200f, 1);
							int drawX = (int)(((locply.position.X + (locply.width / 2))) - Main.screenPosition.X);
							int drawY = (int)((locply.position.Y + (locply.gravDir == 1 ? locply.height + 10 : -10)) - Main.screenPosition.Y);//gravDir 

							spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(drawX, drawY, 0));

							float drawcolortrans = MathHelper.Clamp((modply.electricdelay + 100) / 100f, 0.15f+(float)Math.Sin(Main.GlobalTime*5f)/10f, 1f)* (MathHelper.Clamp((1f-perc) * 250f, 0f, 1f));

							if (drawcolortrans > 0f)
							{
								spriteBatch.Draw(texture, new Vector2(-scaler.X - 2, offsetY), new Rectangle(0, 0, 2, texture.Height), Color.White * drawcolortrans, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
								spriteBatch.Draw(texture, new Vector2(-scaler.X, offsetY), new Rectangle(2, 0, 2, texture.Height), Color.DarkGray * drawcolortrans, 0f, new Vector2(0, 0), scaler, SpriteEffects.None, 0);

								spriteBatch.End();
								spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(drawX, drawY, 0));
								GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye).Apply(null);

								spriteBatch.Draw(texture, new Vector2(-scaler.X, offsetY), new Rectangle(2, 2, 2, texture.Height-2), Color.Aqua * drawcolortrans, 0f, new Vector2(0, 0), new Vector2(scaler.X * perc, scaler.Y), SpriteEffects.None, 0);
								
								spriteBatch.End();
								spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(drawX, drawY, 0));

								spriteBatch.Draw(texture, new Vector2(+scaler.X, offsetY), new Rectangle(4, 0, 2, texture.Height), Color.White * drawcolortrans, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
								offsetY += texture.Height;
							}
						}
					}

					perc = (float)SGAWorld.SnapCooldown / (60f * 300f);
					if (perc > 0)
					{

						spriteBatch.End();

						Vector2 scaler = new Vector2(80f, 1);
						int drawX = (int)(((locply.position.X + (locply.width / 2))) - Main.screenPosition.X);
						int drawY = (int)((locply.position.Y + (locply.gravDir == 1 ? locply.height + 10 : -10)) - Main.screenPosition.Y);//gravDir 

						spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(drawX, drawY, 0));

						float drawcolortrans = MathHelper.Clamp(perc*50f, 0f, 1f);
						spriteBatch.Draw(texture, new Vector2(-scaler.X - 2, offsetY), new Rectangle(0, 0, 2, texture.Height), Color.White * drawcolortrans, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
						spriteBatch.Draw(texture, new Vector2(-scaler.X, offsetY), new Rectangle(2, 0, 2, texture.Height), Color.DarkGray * drawcolortrans, 0f, new Vector2(0, 0), scaler, SpriteEffects.None, 0);
						spriteBatch.Draw(texture, new Vector2(-scaler.X, offsetY), new Rectangle(2, 0, 2, texture.Height), Main.hslToRgb((Main.GlobalTime/3f)%1f,1f,0.75f) * drawcolortrans, 0f, new Vector2(0, 0), new Vector2(scaler.X * perc, scaler.Y), SpriteEffects.None, 0);
						spriteBatch.Draw(texture, new Vector2(+scaler.X, offsetY), new Rectangle(4, 0, 2, texture.Height), Color.White * drawcolortrans, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0);
						offsetY += texture.Height;
					}


					if (modply.CooldownStacks != null && modply.CooldownStacks.Count > 0)
					{


						texture = mod.GetTexture("ActionCooldown");
						int drawX = (int)(((-texture.Width / 4f)));
						int drawY = (int)(((48+offsetY)));//gravDir 

						spriteBatch.End();
						spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(Main.UIScale) * Matrix.CreateTranslation(locply.Center.X - Main.screenPosition.X, locply.Center.Y - Main.screenPosition.Y, 0));

						int maxx = Math.Max(modply.MaxCooldownStacks, modply.CooldownStacks.Count);

						for (int q = 0; q < maxx; q++)
						{
							if (q < modply.CooldownStacks.Count)
							{
								float xoffset = ((float)q * ((float)texture.Width * 0.5f));

								perc = Math.Min(1f, (float)modply.CooldownStacks[q].timeleft / 30f);
								float percprev = 0f;
								Color colormode = Color.White;
								if (modply.MaxCooldownStacks <= q)
									colormode = Color.Lerp(Color.White,Color.Red,0.50f);

								if (q - 1 >= 0)
									percprev = Math.Min(1f, (float)modply.CooldownStacks[q - 1].timeleft / 30f);
								float percent = (float)modply.CooldownStacks[q].timeleft / (float)modply.CooldownStacks[q].maxtime;
								spriteBatch.Draw(texture, new Vector2(drawX - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawY), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f) * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
								spriteBatch.Draw(texture, new Vector2(drawX - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawY), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), colormode * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
							}
						}
					}
				}
			}

			return true;
		}

	}




	public class SGAHUD : Overlay
	{
		public SGAHUD() : base(EffectPriority.Medium, RenderLayers.All) { }

		public override void Update(GameTime gameTime)
		{
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			if (IsVisible())
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

				Effect TrippyRainbowEffect = SGAmod.TrippyRainbowEffect;

				TrippyRainbowEffect.Parameters["uColor"].SetValue(new Vector3(0.05f, 0.05f, 0f));
				TrippyRainbowEffect.Parameters["uScreenResolution"].SetValue(new Vector2(Main.screenWidth, Main.screenHeight) / 6f);
				TrippyRainbowEffect.Parameters["uOpacity"].SetValue(0.15f * Filters.Scene["SGAmod:ScreenWave"].GetShader().CombinedOpacity);
				TrippyRainbowEffect.Parameters["uDirection"].SetValue(new Vector2(1f, Main.GlobalTime * 0.1f));
				TrippyRainbowEffect.Parameters["uIntensity"].SetValue(1f);
				TrippyRainbowEffect.Parameters["uScreenPosition"].SetValue(Main.screenPosition / 500f);
				TrippyRainbowEffect.Parameters["uTargetPosition"].SetValue(Main.screenPosition / 500f);
				TrippyRainbowEffect.Parameters["uProgress"].SetValue(Main.GlobalTime * 0.05f);
				TrippyRainbowEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));
				TrippyRainbowEffect.CurrentTechnique.Passes["ScreenTrippy"].Apply();

				spriteBatch.Draw(Main.blackTileTexture, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, new Rectangle(0, 0, 128, 128), Color.White * 0.25f, -MathHelper.PiOver2, Vector2.One * 56, Vector2.One * 128, SpriteEffects.None, 0f);
				TrippyRainbowEffect.Parameters["uDirection"].SetValue(new Vector2(1f, Main.GlobalTime * 0.1f));
				TrippyRainbowEffect.Parameters["uProgress"].SetValue(Main.GlobalTime * 0.0075f);
				TrippyRainbowEffect.CurrentTechnique.Passes["ScreenTrippy"].Apply();

				spriteBatch.Draw(Main.blackTileTexture, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, new Rectangle(0, 0, 128, 128), Color.White * 0.25f, 0, Vector2.One * 73, Vector2.One * 128, SpriteEffects.None, 0f);

				TrippyRainbowEffect.Parameters["uDirection"].SetValue(new Vector2(1f, Main.GlobalTime * 0.06f));
				TrippyRainbowEffect.Parameters["uProgress"].SetValue(Main.GlobalTime * 0.0075f);
				TrippyRainbowEffect.CurrentTechnique.Passes["ScreenTrippy"].Apply();

				spriteBatch.Draw(Main.blackTileTexture, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, new Rectangle(0, 0, 128, 128), Color.White * 0.25f, -MathHelper.PiOver4, Vector2.One * 100, Vector2.One * 128, SpriteEffects.None, 0f);


				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Matrix.Identity);
			}
		}

		public override void Activate(Vector2 position, params object[] args) { }

		public override void Deactivate(params object[] args) { }

		public override bool IsVisible()
		{
			bool draw=false;
			if (!Main.gameMenu && Main.LocalPlayer != null && SGAmod.Instance != null && Filters.Scene["SGAmod:ScreenWave"].GetShader().CombinedOpacity>0f)
			{
				//if (Main.LocalPlayer.HeldItem.type == SGAmod.Instance.ItemType("Expertise"))
				draw = true;
			}


			return draw;
		}
	}
}
