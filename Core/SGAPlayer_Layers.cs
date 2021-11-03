using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.Default;
using Idglibrary;
using Terraria.ModLoader.IO;
using Terraria.Graphics.Shaders;
using SGAmod.NPCs;
using SGAmod.NPCs.Wraiths;
using SGAmod.NPCs.Cratrosity;
using SGAmod.NPCs.Murk;
using SGAmod.NPCs.Sharkvern;
using SGAmod.NPCs.SpiderQueen;
using SGAmod.NPCs.Hellion;
using CalamityMod;
using AAAAUThrowing;
using Terraria.Utilities;
using SGAmod.SkillTree;

namespace SGAmod
{
		public partial class SGAPlayer : ModPlayer
	{

		public static readonly PlayerLayer WaveBeamArm = new PlayerLayer("SGAmod", "WaveBeamArm", PlayerLayer.Arms, delegate (PlayerDrawInfo drawInfo)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				SGAmod mod = SGAmod.Instance;
				SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

				//better version, from Qwerty's Mod
				Color color = drawInfo.bodyColor;
				Texture2D texture = mod.GetTexture("Items/Armors/BeamArms");
					int drawX = (int)((drawInfo.position.X+drawPlayer.bodyPosition.X+10) - Main.screenPosition.X);
					int drawY = (int)(((drawPlayer.bodyPosition.Y-4)+drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0,drawPlayer.bodyFrame.Y,drawPlayer.bodyFrame.Width,drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width/2,drawPlayer.bodyFrame.Height/2), 1f, (drawPlayer.direction==-1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir>0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
					Main.playerDrawData.Add(data);
			});

		public static readonly PlayerLayer SpaceDiverTank = new PlayerLayer("SGAmod", "SpaceDiverTank", PlayerLayer.BackAcc, delegate (PlayerDrawInfo drawInfo)
			{
				Player drawPlayer = drawInfo.drawPlayer;
				SGAmod mod = SGAmod.Instance;
				SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

				//better version, from Qwerty's Mod
				Color color = drawInfo.bodyColor;

				Texture2D texture = mod.GetTexture("Items/Accessories/PrismalAirTank_Back");
					int drawX = (int)((drawInfo.position.X+drawPlayer.bodyPosition.X+10) - Main.screenPosition.X);
					int drawY = (int)(((drawPlayer.bodyPosition.Y-4)+drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
					DrawData data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0,drawPlayer.bodyFrame.Y,drawPlayer.bodyFrame.Width,drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width/2,drawPlayer.bodyFrame.Height/2), 1f, (drawPlayer.direction==-1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir>0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
					data.shader = (int)drawPlayer.cWings;
					Main.playerDrawData.Add(data);
			});


		public static readonly PlayerLayer BreathingReed = new PlayerLayer("SGAmod", "BreathingReed", PlayerLayer.MountBack, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			if (!drawPlayer.wet)
				return;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			//better version, from Qwerty's Mod
			Color color = drawInfo.bodyColor;

			Texture2D texture = Main.itemTexture[ItemID.BreathingReed];
			int drawX = (int)((drawPlayer.MountedCenter.X + drawPlayer.direction*6) - Main.screenPosition.X);
			int drawY = 0;
			drawY = (int)((drawPlayer.MountedCenter.Y-12*drawPlayer.gravDir) - Main.screenPosition.Y);
			float facing = drawPlayer.velocity.X > 0 ? 1f : -1f;

			float velosway = 60 / MathHelper.Pi * (float)Math.Atan(Math.Abs(drawPlayer.velocity.X/5f));
			velosway *= -facing*0.025f;

			DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, color, (float)(drawPlayer.fullRotation + velosway - (MathHelper.Pi/4f)), new Vector2(4,texture.Height-4), 1f, (SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
			data.shader = (int)drawPlayer.cWings;
			Main.playerDrawData.Add(data);
		});

		public static void drawdigistuff(PlayerDrawInfo drawInfo,bool front)
		{

			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();


			int bonusattacks = (int)(((float)modply.digiStacks / (float)modply.digiStacksMax) * (float)modply.digiStacksCount);

			if (bonusattacks > 0)
			{
				List<Vector2> whichone = new List<Vector2>();
				for (int i = 0; i < bonusattacks; i += 1)
				{
					float angle = MathHelper.ToRadians(modply.timer + ((((float)i - 1) / (float)bonusattacks) * 360f));
					float scaler = 1f + ((float)Math.Sin(angle) * 0.25f);
					whichone.Add(new Vector2(scaler, angle));
					//Vector2 apos = new Vector2((float)Math.Cos(angle) * 64, (float)Math.Sin(angle) * 24);
				}
				whichone=whichone.OrderBy((x) => x.X).ToList();

				if (whichone.Count > 0)
				{
					for (int a = 0; a < whichone.Count; a += 1)
					{
						Vector2 theplace = whichone[a];
						float scaler = theplace.X;

						if ((scaler >= 1f && front) || (scaler < 1f && !front))
						{
							float angle = theplace.Y;

							Vector2 apos = new Vector2((float)Math.Cos(angle) * 64, (float)Math.Sin(angle) * 12);

							Texture2D texture = Main.itemTexture[ItemID.ManaCrystal];

							int drawX = (int)((drawPlayer.Center.X + apos.X) - Main.screenPosition.X);
							int drawY = (int)((drawPlayer.MountedCenter.Y + apos.Y) - Main.screenPosition.Y);//gravDir 
							DrawData data = new DrawData(texture, new Vector2(drawX, drawY), null, Color.White, (float)0, new Vector2(texture.Width / 2, texture.Height / 2), scaler, (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
							//data.shader = (int)drawPlayer.dye[2].dye;
							Main.playerDrawData.Add(data);
						}
					}
				}

			}

		}

		public static readonly PlayerLayer DigiEffect = new PlayerLayer("SGAmod", "DigiEffect", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
		{
			SGAPlayer.drawdigistuff(drawInfo,true);
		});

		public static readonly PlayerLayer DigiEffectBack = new PlayerLayer("SGAmod", "DigiEffect", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
		{
			SGAPlayer.drawdigistuff(drawInfo, false);
		});

		public static void DrawilluminantLayer(PlayerDrawInfo drawInfo, bool front)
		{

			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();


			int activestacks = modply.activestacks;

			if (activestacks > 0)
			{
				List<Vector4> whichone = new List<Vector4>();
				UnifiedRandom rando = new UnifiedRandom(drawPlayer.whoAmI);

				for (int i = 0; i < activestacks; i += 1)
				{
					float percent = rando.NextFloat(1f) * MathHelper.TwoPi;
					Matrix mxatrix = Matrix.CreateRotationY((Main.GlobalTime*2f)+ percent) * Matrix.CreateRotationZ(((i / (float)activestacks) * MathHelper.TwoPi)+(Main.GlobalTime * (rando.NextFloat(0.4f,0.6f))));
					Vector3 vec3 = Vector3.Transform(Vector3.UnitX, mxatrix);
					float alpha = 1f;
					if (modply.CooldownStacks.Count >= i)
						alpha = MathHelper.Clamp((modply.CooldownStacks[i].timeleft / (float)modply.CooldownStacks[i].maxtime)*3f,0f,1f);

					whichone.Add(new Vector4(vec3, alpha));
				}
				whichone = whichone.OrderBy((x) => x.Z).ToList();

				if (whichone.Count > 0)
				{
					for (int a = 0; a < whichone.Count; a += 1)
					{
						Vector4 theplace = whichone[a];
						float scaler = 1+theplace.Z;

						if ((scaler >= 1f && front) || (scaler < 1f && !front))
						{

							Texture2D texture = SGAmod.Instance.GetTexture("Extra_57b");

							Vector2 drawhere = new Vector2(theplace.X, theplace.Y)* 64f;
							DrawData data = new DrawData(texture, drawPlayer.MountedCenter+drawhere - Main.screenPosition, null, Color.Magenta*(theplace.W*0.75f), (float)Math.Sin(theplace.X), texture.Size()/2f, 0.5f+(scaler-1f)*0.25f, (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
							//data.shader = (int)drawPlayer.dye[2].dye;
							Main.playerDrawData.Add(data);
						}
					}
				}

			}

		}

		public static readonly PlayerLayer illuminantEffect = new PlayerLayer("SGAmod", "illuminantEffect", PlayerLayer.MiscEffectsFront, delegate (PlayerDrawInfo drawInfo)
		{
			SGAPlayer.DrawilluminantLayer(drawInfo, true);
		});

		public static readonly PlayerLayer illuminantEffectBack = new PlayerLayer("SGAmod", "illuminantEffect", PlayerLayer.MiscEffectsBack, delegate (PlayerDrawInfo drawInfo)
		{
			SGAPlayer.DrawilluminantLayer(drawInfo, false);
		});

		public static readonly PlayerLayer JoyriderWings = new PlayerLayer("SGAmod", "AltWings", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			//better version, from Qwerty's Mod
			Color color = drawInfo.bodyColor;

			//if (modply.CustomWings == 1)
			//{

				float angle = MathHelper.ToRadians(90f + (drawPlayer.velocity.X * 2f));

				int joy = Math.Max(0, modply.JoyrideShake);

				float nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
				float nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);

				Texture2D texture;
				int drawX;
				int drawY;
				Vector2 org;

				float stealth = (0.2f + drawPlayer.stealth * 0.8f) *Math.Max(0.10f,((float)drawInfo.bodyColor.A / 255f));

			int num = drawPlayer.bodyFrame.Y / 56;
			if (num >= Main.OffsetsPlayerHeadgear.Length)
				num = 0;

			for (int i = -10; i < 11; i += 20)
				{
					nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
					nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);
					drawX = (int)((drawPlayer.MountedCenter.X + (drawPlayer.direction * (-8 + i)) + nalzs));
					drawY = (int)((drawPlayer.MountedCenter.Y + nalzs2 - 8f));//gravDir 
				Vector2 extraY = Vector2.UnitY* (drawPlayer.gfxOffY + Main.OffsetsPlayerHeadgear[num].Y);
					Vector2 whereat2 = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));
					color = Lighting.GetColor((int)(whereat2.X / 16f), (int)(whereat2.Y / 16f)) * stealth;
					texture = Main.itemTexture[ItemID.Megashark];
					org = new Vector2(texture.Width * (drawPlayer.direction > 0 ? 0.25f : 0.25f), texture.Height / 2f);
					DrawData data2 = new DrawData(texture, whereat2 + extraY - Main.screenPosition, null, color, (float)drawPlayer.fullRotation + angle, org, 0.75f, (drawPlayer.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
					data2.shader = (int)drawPlayer.cWings;
					Main.playerDrawData.Add(data2);
				}

				nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
				nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);

				texture = Main.itemTexture[ItemID.ChainGun];
				drawX = (int)((drawPlayer.MountedCenter.X + (drawPlayer.direction * -8) + nalzs));
				drawY = (int)((drawPlayer.MountedCenter.Y + nalzs2 - 6f));//gravDir 
				Vector2 whereat = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));

				color = Lighting.GetColor((int)(whereat.X / 16f), (int)(whereat.Y/16f), drawInfo.bodyColor) * stealth;
				org = new Vector2(texture.Width * (drawPlayer.direction > 0 ? 0.25f : 0.25f), texture.Height / 2f);
				DrawData data = new DrawData(texture, whereat - Main.screenPosition, null, color, (float)drawPlayer.fullRotation + angle, org, 0.75f, (drawPlayer.direction == -1 ? SpriteEffects.FlipVertically : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.cWings;
				Main.playerDrawData.Add(data);

			//}

		});

		public static PlayerLayer DergWings => new PlayerLayer("SGAmod", "AltWings", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			//better version, from Qwerty's Mod
			Color color = drawInfo.bodyColor;

			float angle = 0;

			int joy = Math.Max(0, modply.JoyrideShake);

			float nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
			float nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);

			Texture2D texture;
			int drawX;
			int drawY;
			Vector2 org;

			float stealth = (0.2f + drawPlayer.stealth * 0.8f) * Math.Max(0.10f, ((float)drawInfo.bodyColor.A / 255f));

			int num = drawPlayer.bodyFrame.Y / 56;
			if (num >= Main.OffsetsPlayerHeadgear.Length)
				num = 0;
			Vector2 adderPos = new Vector2(Main.OffsetsPlayerHeadgear[num].X, drawPlayer.gfxOffY + Main.OffsetsPlayerHeadgear[num].Y);

			#region RenderTarget2D
			if (SGAConfigClient.Instance.AvariceLordWings)
			{
				drawX = (int)(drawPlayer.MountedCenter.X);
				drawY = (int)(drawPlayer.MountedCenter.Y);

				texture = Items.Accessories.TrueDragonWings.wingsSurface;

				org = texture.Size() / 2f;

				Vector2 whereat2 = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));
				color = Lighting.GetColor((int)(whereat2.X / 16f), (int)(whereat2.Y / 16f)) * stealth;

				DrawData data2 = new DrawData(texture, whereat2 + adderPos - Main.screenPosition, null, color, (float)drawPlayer.fullRotation + angle, org, 2f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);

				data2.shader = (int)drawPlayer.cWings;

				Main.playerDrawData.Add(data2);
				return;
			}
			#endregion


			#region ScaledDraw

			for (int i = 1; i >= 0; i -= 1)
			{

				drawX = (int)((drawPlayer.MountedCenter.X + (-8 + i*16f)*drawPlayer.direction));
				drawY = (int)((drawPlayer.MountedCenter.Y + nalzs2 - 2f));
				if (i < 1)
					drawY += 8;

				texture = ModContent.GetTexture("SGAmod/Items/Accessories/BetsyWings/BetsyWings"+(i<1 ? "Front" : "Back"));

				int wingIndex = drawPlayer.GetModPlayer<Items.Accessories.DergWingsPlayer>().wingFrames/4;

				Point textureframe = new Point(texture.Width / 2, texture.Height / 5);

				int wingindexmod = wingIndex % 9;

				int wingX = (wingindexmod / 5)%2;
				int wingY = wingindexmod % 5;

				float flyingAngle = drawPlayer.GetModPlayer<Items.Accessories.DergWingsPlayer>().flyingAngle.Item1;

				Vector2 scale = new Vector2(0.5f, 0.5f) * new Vector2(0.25f + ((flyingAngle) * 0.25f), 0.5f + ((flyingAngle) * 0.25f));

				org = (i > 0 ? new Vector2(drawPlayer.direction < 0 ? textureframe.X-38 : 38, 164) : new Vector2(drawPlayer.direction< 0 ? textureframe.X - 222 : 222, 188))+new Vector2(wingindexmod*2*drawPlayer.direction, wingindexmod*2);

				if (drawPlayer.direction < 0)
				{
					drawX -= (int)((i < 1 ? 16 : -16) * scale.X);
				}
                else
                {
					drawX -= (int)((i > 0 ? 16 : -16)* scale.X);
				}

				Rectangle erect = new Rectangle((wingX) * textureframe.X, (wingY) * textureframe.Y, textureframe.X, textureframe.Y);

				angle = 0;// (((-0.5f + i)*(MathHelper.Pi*0.5f))* ((1f- flyingAngle) *0.2f)+(float)(0f - Math.Pow(Math.Abs(drawPlayer.velocity.X)*0.015f,0.75f))* (1f-flyingAngle)) *drawPlayer.direction;

				Vector2 whereat3 = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));
				color = Lighting.GetColor((int)(whereat3.X / 16f), (int)(whereat3.Y / 16f)) * stealth;

				DrawData data3 = new DrawData(texture, whereat3 + adderPos - Main.screenPosition, erect, color, (float)drawPlayer.fullRotation + angle, org, scale, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);

				data3.shader = (int)drawPlayer.cWings;

				Main.playerDrawData.Add(data3);
			}

			#endregion

		});

		public override void ModifyDrawInfo(ref PlayerDrawInfo drawInfo)
		{
			//drawInfo.
		}


		public static void DrawGlowmasks(PlayerDrawInfo drawInfo, int index)
		{

			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();
			Color GlowColor = modply.armorglowcolor[index](drawPlayer, index);

			Color color = (Color.Lerp(drawInfo.bodyColor, GlowColor, drawPlayer.stealth * ((float)drawInfo.bodyColor.A / 255f)));

			if (drawPlayer.immune && !drawPlayer.immuneNoBlink && drawPlayer.immuneTime > 0)
				color = drawInfo.bodyColor * drawInfo.bodyColor.A;

			if (modply.armorglowmasks[index] != null && !drawPlayer.mount.Active)
			{
				Texture2D texture = ModContent.GetTexture(modply.armorglowmasks[index]);

				int drawX = (int)((drawInfo.position.X + drawPlayer.bodyPosition.X + 10) - Main.screenPosition.X);
				int drawY = (int)(((drawPlayer.bodyPosition.Y - 3) + drawPlayer.MountedCenter.Y) - Main.screenPosition.Y);//gravDir 
				DrawData data;
				if (index == 3)
					data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.legFrame.Y, drawPlayer.legFrame.Width, drawPlayer.legFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.legFrame.Width / 2, drawPlayer.legFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				else
					data = new DrawData(texture, new Vector2(drawX, drawY), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), color, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
				data.shader = (int)drawPlayer.dye[index > 1 ? index - 1 : index].dye;

				Main.playerDrawData.Add(data);

				if (modply.valkyrieSet.Item3)
				{
					int indexer = drawPlayer.FindBuffIndex(ModContent.BuffType<Items.Armors.Valkyrie.RagnarokBuff>());
					if (indexer >= 0)
					{
						for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.Pi / 8f)
						{
							float distance = (2f + (float)Math.Sin(Main.GlobalTime * 3f) * 2f)+(20f * (modply.valkyrieSet.Item4-0.25f));
							float drawX2 = (float)(drawX + Math.Cos(Main.GlobalTime + f) * distance);
							float drawY2 = (float)(drawY + Math.Sin(Main.GlobalTime + f) * distance);

							Color colorz = Color.White * MathHelper.Clamp(drawPlayer.buffTime[indexer] / 200f, 0f, 1f);

							if (index == 3)
								data = new DrawData(texture, new Vector2(drawX2, drawY2), new Rectangle(0, drawPlayer.legFrame.Y, drawPlayer.legFrame.Width, drawPlayer.legFrame.Height), colorz * 0.05f, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
							else
								data = new DrawData(texture, new Vector2(drawX2, drawY2), new Rectangle(0, drawPlayer.bodyFrame.Y, drawPlayer.bodyFrame.Width, drawPlayer.bodyFrame.Height), colorz * 0.05f, (float)drawPlayer.fullRotation, new Vector2(drawPlayer.bodyFrame.Width / 2, drawPlayer.bodyFrame.Height / 2), 1f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);
							data.shader = (int)drawPlayer.dye[index > 1 ? index - 1 : index].dye;

							Main.playerDrawData.Add(data);
						}
					}
				}
			}

		}

		public override void ModifyDrawLayers(List<PlayerLayer> layers)
		{
			//plasmaLeftInClip
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();

			#region MiscVisuals

			if (sgaplayer.SpaceDiverset && CustomWings<1)
			{
			/*int wingsLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
			int backacclayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("BackAcc"));
			if (SpaceDiverWings < 0.6f)
			layers.RemoveAt(wingsLayer);
			SpaceDiverTank.visible = true;
			layers.Insert(backacclayer, SpaceDiverTank);*/
			}

			//Main.NewText(sgaplayer.CustomWings);
			if (sgaplayer.CustomWings>0)
			{
				int wingsLayer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("Wings"));
				if (wingsLayer >= 0)
				{
					if (sgaplayer.CustomWings == 1)
					{
						JoyriderWings.visible = true;
						//layers.RemoveAt(wingsLayer);
						layers.Insert(wingsLayer, JoyriderWings);
					}
					if (sgaplayer.CustomWings == 2)
					{
						DergWings.visible = true;
						//layers.RemoveAt(wingsLayer);
						layers.Insert(wingsLayer, DergWings);
					}
				}
			}

			if (sgaplayer.terraDivingGear)
			{
				int armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MountBack"));
				if (armLayer2 >= 0)
				{
					BreathingReed.visible = true;
					layers.Insert(armLayer2, BreathingReed);
				}
			}

			if (player.HeldItem.type == mod.ItemType("WaveBeam"))
			{
				int armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("HandOnAcc"));
				if (armLayer2 >= 0)
				{
					WaveBeamArm.visible = true;
					layers.Insert(armLayer2, WaveBeamArm);
				}
			}

			if (IDGset || illuminantSet.Item1 > 4)
			{
				int armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsFront"));
				if (armLayer2 >= 0)
				{
					if (IDGset)
					{
						DigiEffect.visible = true;
						layers.Insert(armLayer2, DigiEffect);
					}
					if (illuminantSet.Item1 > 4)
                    {
						illuminantEffect.visible = true;
						layers.Insert(armLayer2, illuminantEffect);
					}

					armLayer2 = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals("MiscEffectsBack"));
					if (armLayer2 >= 0)
					{
						if (IDGset)
						{
							DigiEffectBack.visible = true;
							layers.Insert(armLayer2, DigiEffectBack);
						}
						if (illuminantSet.Item1 > 4)
						{
							illuminantEffectBack.visible = true;
							layers.Insert(armLayer2, illuminantEffectBack);
						}
					}
				}
			}
            #endregion

            #region armor glowmasks

            string[] stringsz = { "Head", "Body", "Arms", "Legs"};
			PlayerLayer[] thelayer = { PlayerLayer.Head, PlayerLayer.Body, PlayerLayer.Arms, PlayerLayer.Legs };

			for (int intc = 0; intc < 4; intc += 1)
			{

				if (sgaplayer.armorglowmasks[intc] != null)
				{
					Action<PlayerDrawInfo> glowtarget;
					switch (intc)//donno why but passing the value here from the for loop causes a crash, boo
					{
						case 1:
							glowtarget = s => DrawGlowmasks(s, 1);
							break;
						case 2:
							glowtarget = s => DrawGlowmasks(s, 2);
							break;
						case 3:
							glowtarget = s => DrawGlowmasks(s, 3);
							break;
						default:
							glowtarget = s => DrawGlowmasks(s, 0);
							break;
					}
					PlayerLayer glowlayer = new PlayerLayer("SGAmod", "Armor Glowmask", thelayer[intc], glowtarget);
					int layer = layers.FindIndex(PlayerLayer => PlayerLayer.Name.Equals(stringsz[intc])) + 1;
					glowlayer.visible = true;
					layers.Insert(layer, glowlayer);
				}

			}
            #endregion

        }

    }

}