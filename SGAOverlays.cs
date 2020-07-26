using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.GameContent.UI;
using Terraria.UI;
using Terraria.Graphics;
using SGAmod.SkillTree;

namespace SGAmod
{

	public abstract class SGAInterface
	{
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

			for (int k = 0; k < layers.Count; k++)
			{
				if (layers[k].Name == "Vanilla: Resource Bars")
				{
					layers.Insert(k + 1, new LegacyGameInterfaceLayer("SGAmod: HUD", DrawHUD, InterfaceScaleType.UI));
				}
			}

			int mouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (mouseTextIndex != -1)
			{
				layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
					"SGAmod: CustomUI", DrawUI,
					InterfaceScaleType.UI)
				);
			}
			layers.Insert(0, new LegacyGameInterfaceLayer("SGAmod: UnderHUD", DrawUnderHUD, InterfaceScaleType.Game));
		}

		public static bool DrawUI()
		{
			if (SGAmod.CustomUIMenu.visible) 
			{
				SGAmod.CustomUIMenu.Draw(Main.spriteBatch);
			}
			return true;
		}

		public static bool DrawUnderHUD()
		{

			if (Main.gameMenu || SGAmod.Instance == null && !Main.dedServ)
				return true;

			SGAPlayer sga = Main.LocalPlayer.SGAPly();
			if (sga.gunslingerLegendtarget > -1)
			{

				NPC target = Main.npc[sga.gunslingerLegendtarget];

				bool sizeup = false;
				for (int i = 0; i < 360; i += 360 / 8)
				{
					sizeup = !sizeup;
					float angle = MathHelper.ToRadians(i+((sizeup ? 400f : -400f)*MathHelper.Clamp(1f- ((float)sga.lockoneffect/70f), 0f,1f)));
					Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (180f+MathHelper.Clamp(300- sga.lockoneffect*4,0,300));

					Vector2 drawPos = ((hereas * (sizeup ? 1f : 1f)*Main.essScale) + target.Center) - Main.screenPosition;
					int sizer = (sizeup ? 8 : 4);
					Main.spriteBatch.Draw(Main.blackTileTexture, drawPos, new Rectangle(0, 0, 50, sizer * 2), Color.Red*MathHelper.Clamp((float)sga.lockoneffect/70f,0f,1f), MathHelper.ToRadians(i), new Vector2(50, sizer), new Vector2(1, 1), SpriteEffects.None, 0f);

				}
			}

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
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1, 1, 0f));
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
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

				}
				if (SGAmod.UsesPlasma.ContainsKey(locply.HeldItem.type))
				{
					if (!locply.dead)
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
				}

				if (SGAmod.UsesClips.ContainsKey(locply.HeldItem.type))
				{
					if (!locply.dead)
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

					int offsetY = -texture.Height;

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

					if (modply.CooldownStacks.Count > 0)
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
								if (q - 1 >= 0)
									percprev = Math.Min(1f, (float)modply.CooldownStacks[q - 1].timeleft / 30f);
								float percent = (float)modply.CooldownStacks[q].timeleft / (float)modply.CooldownStacks[q].maxtime;
								spriteBatch.Draw(texture, new Vector2(drawX - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawY), null, Color.Lerp(Color.Black, Color.DarkGray, 0.25f) * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
								spriteBatch.Draw(texture, new Vector2(drawX - ((((maxx - 1) * (int)(texture.Width * 0.5)) / 2) / 2f) + (xoffset * percprev), drawY), new Rectangle(0, 0, texture.Width, (int)((float)texture.Height * percent)), Color.White * MathHelper.Clamp((float)modply.CooldownStacks[q].timerup / 30f, 0f, perc), 0f, new Vector2(texture.Width / 2, texture.Height / 2), 0.5f, SpriteEffects.None, 0);
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
			





		}

		public override void Activate(Vector2 position, params object[] args) { }

		public override void Deactivate(params object[] args) { }

		public override bool IsVisible()
		{
			bool draw=false;
			if (!Main.gameMenu && Main.LocalPlayer != null && SGAmod.Instance != null)
			{
				//if (Main.LocalPlayer.HeldItem.type == SGAmod.Instance.ItemType("Expertise"))
				draw = true;
			}


			return draw;
		}
	}
}
