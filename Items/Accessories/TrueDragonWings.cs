using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Accessories
{
	public class DergWingsPlayer : ModPlayer
	{
		public bool draconicBoost = false;
		public int wingFrames = 0;
		public (float, int) flyingAngle = (0,0);

        public override void ResetEffects()
        {
			draconicBoost = false;
			if (flyingAngle.Item2 > 0)
			{
				flyingAngle.Item1 += (1f- flyingAngle.Item1)*0.60f;
				flyingAngle.Item2 -= 1;
            }
            else
            {
				flyingAngle.Item1 *= 0.60f;
			}
        }
    }
		[AutoloadEquip(EquipType.Wings)]
		public class TrueDragonWings : ModItem
	{

		public static RenderTarget2D wingsSurfacePre;
		public static RenderTarget2D wingsSurface;
		public static int shadowParticlesDrawTime = 0;

		public static void Load()
		{
			wingsSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, 160,240, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
			wingsSurfacePre = new RenderTarget2D(Main.graphics.GraphicsDevice, 160, 240, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
		}

		public static PlayerLayer DergWings => new PlayerLayer("SGAmod", "AltWings", PlayerLayer.Wings, delegate (PlayerDrawInfo drawInfo)
		{
			Player drawPlayer = drawInfo.drawPlayer;
			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			//better version, from Qwerty's Mod
			Color color = drawInfo.bodyColor;

			float angle = drawPlayer.legRotation;

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
			Vector2 adderPos = new Vector2(Main.OffsetsPlayerHeadgear[num].X, drawPlayer.gfxOffY + Main.OffsetsPlayerHeadgear[num].Y)* (int)drawPlayer.gravDir+ drawPlayer.bodyPosition;

			#region RenderTarget2D
			if (SGAConfigClient.Instance.AvariceLordWings)
			{
				drawX = (int)(drawPlayer.MountedCenter.X);
				drawY = (int)(drawPlayer.MountedCenter.Y);

				texture = Items.Accessories.TrueDragonWings.wingsSurface;

				org = texture.Size() / 2f;

				Vector2 whereat2 = (new Vector2(drawX, drawY).RotatedBy(drawPlayer.fullRotation, drawPlayer.MountedCenter));
				color = Lighting.GetColor((int)(whereat2.X / 16f), (int)(whereat2.Y / 16f)) * stealth;

				DrawData data2 = new DrawData(texture, whereat2 + adderPos - Main.screenPosition, null, color, (float)drawPlayer.fullRotation + angle, org, 2f, (drawPlayer.direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None), 0);

				data2.shader = (int)drawPlayer.cWings;

				Main.playerDrawData.Add(data2);
				return;
			}
			#endregion


			#region ScaledDraw

			for (int i = 1; i >= 0; i -= 1)
			{

				drawX = (int)((drawPlayer.MountedCenter.X + (-8 + i * 16f) * drawPlayer.direction));
				drawY = (int)((drawPlayer.MountedCenter.Y + (nalzs2 - 2f)*drawPlayer.gravDir));
				if (i < 1)
					drawY += 8 * (int)drawPlayer.gravDir;

				texture = ModContent.GetTexture("SGAmod/Items/Accessories/BetsyWings/BetsyWings" + (i < 1 ? "Front" : "Back"));

				int wingIndex = drawPlayer.GetModPlayer<Items.Accessories.DergWingsPlayer>().wingFrames / 4;

				Point textureframe = new Point(texture.Width / 2, texture.Height / 5);

				int wingindexmod = wingIndex % 9;

				int wingX = (wingindexmod / 5) % 2;
				int wingY = wingindexmod % 5;

				float flyingAngle = drawPlayer.GetModPlayer<Items.Accessories.DergWingsPlayer>().flyingAngle.Item1;

				Vector2 scale = new Vector2(0.5f, 0.5f) * new Vector2(0.25f + ((flyingAngle) * 0.25f), 0.5f + ((flyingAngle) * 0.25f));

				org = (i > 0 ? new Vector2(drawPlayer.direction < 0 ? textureframe.X - 38 : 38, 164) : new Vector2(drawPlayer.direction < 0 ? textureframe.X - 222 : 222, 188)) + new Vector2(wingindexmod * 2 * drawPlayer.direction, wingindexmod * 2);

				if (drawPlayer.direction < 0)
				{
					drawX -= (int)((i < 1 ? 16 : -16) * scale.X);
				}
				else
				{
					drawX -= (int)((i > 0 ? 16 : -16) * scale.X);
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

		public static void DrawWings(Player drawPlayer)
		{

			SGAmod mod = SGAmod.Instance;
			SGAPlayer modply = drawPlayer.GetModPlayer<SGAPlayer>();

			if (Main.dedServ || modply.CustomWings != 2 || SGAWorld.modtimer<30 || !SGAConfigClient.Instance.AvariceLordWings)
				return;

			RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

			Main.graphics.GraphicsDevice.SetRenderTarget(wingsSurfacePre);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, null, Matrix.CreateScale(0.5f,0.5f,1f)*Matrix.CreateTranslation(wingsSurface.Width/2, wingsSurface.Height/2,0));

			//SGAmod.FadeInEffect.Parameters["alpha"].SetValue(1f);
			//SGAmod.FadeInEffect.CurrentTechnique.Passes["NoAlphaPass"].Apply();

			float angle = 0;

			int joy = Math.Max(0, modply.JoyrideShake);

			float nalzs = Main.rand.NextFloat(-joy, joy) / 2f;
			float nalzs2 = Main.rand.NextFloat(-joy / 1f, 0);

			int direction = 1;//drawPlayer.direction

			Texture2D texture;
			int drawX;
			int drawY;
			Vector2 org;
			for (int i = 1; i >= 0; i -= 1)
			{


				int num = drawPlayer.bodyFrame.Y / 56;
				if (num >= Main.OffsetsPlayerHeadgear.Length)
					num = 0;

				drawX = (int)(((-8 + i * 16f) * direction));
				drawY = (int)((nalzs2 - 2f));
				if (i < 1)
				{
					drawY += 8;
					drawX += 4;
				}

				texture = ModContent.GetTexture("SGAmod/Items/Accessories/BetsyWings/BetsyWings" + (i < 1 ? "Front" : "Back"));

				int wingIndex = drawPlayer.GetModPlayer<Items.Accessories.DergWingsPlayer>().wingFrames / 4;

				Point textureframe = new Point(texture.Width / 2, texture.Height / 5);

				int wingindexmod = wingIndex % 9;

				int wingX = (wingindexmod / 5) % 2;
				int wingY = wingindexmod % 5;

				float flyingAngle = drawPlayer.GetModPlayer<Items.Accessories.DergWingsPlayer>().flyingAngle.Item1;

				Vector2 scale = new Vector2(0.5f, 0.5f) * new Vector2(0.25f + ((flyingAngle) * 0.25f), 0.5f + ((flyingAngle) * 0.25f));

				org = (i > 0 ? new Vector2(direction < 0 ? textureframe.X - 38 : 38, 164) : new Vector2(direction < 0 ? textureframe.X - 222 : 222, 188)) + new Vector2(wingindexmod * 2 * direction, wingindexmod * 2);

				if (direction < 0)
				{
					drawX -= (int)((i < 1 ? 16 : -16) * scale.X);
				}
				else
				{
					drawX -= (int)((i > 0 ? 16 : -16) * scale.X);
				}

				Rectangle erect = new Rectangle((wingX) * textureframe.X, (wingY) * textureframe.Y, textureframe.X, textureframe.Y);

				angle = 0;// (((-0.5f + i)*(MathHelper.Pi*0.5f))* ((1f- flyingAngle) *0.2f)+(float)(0f - Math.Pow(Math.Abs(drawPlayer.velocity.X)*0.015f,0.75f))* (1f-flyingAngle)) *drawPlayer.direction;

				Vector2 whereat2 = (new Vector2(drawX, drawY));//.RotatedBy(drawPlayer.fullRotation, new Vector2(wingsSurface.Width, wingsSurface.Height) / 2));
				Color color = Color.White;

				Main.spriteBatch.Draw(texture, whereat2, erect, color, (float)drawPlayer.fullRotation + angle, org, scale, (direction == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None) | (drawPlayer.gravDir > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically), 0);


			}
			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.SetRenderTarget(wingsSurface);
			Main.graphics.GraphicsDevice.Clear(Color.Transparent);

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, null, Matrix.Identity);

			SGAmod.FadeInEffect.Parameters["alpha"].SetValue(1f);
			SGAmod.FadeInEffect.CurrentTechnique.Passes["NoAlphaPass"].Apply();

			Main.spriteBatch.Draw(wingsSurfacePre, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);

			Main.spriteBatch.End();

			Main.graphics.GraphicsDevice.SetRenderTargets(binds);

		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Lord's Wings");
			Tooltip.SetDefault("'The Finest draconic wings;\nJust trimmed the hands off 1st...'\nHold DOWN to reduce vertical but increase horizontal movement\nHold UP to gain boosts into the air whenever you flap");
			Main.itemTexture[ModContent.ItemType<TrueDragonWings>()] = Main.itemTexture[ItemID.BetsyWings];
		}

		public override void SetDefaults()
		{
			sbyte wingslo = item.wingSlot;
			item.CloneDefaults(ItemID.FrozenWings);
			item.width = 26;
			item.height = 38;
			item.value = 750000;
			item.accessory = true;
			item.rare = ItemRarityID.Red;
			item.wingSlot = wingslo;
		}
        public override bool Autoload(ref string name)
        {
			Load();
            SGAPlayer.PostCharmsUpdateEquipsEvent += SGAPlayer_PostCharmsUpdateEquipsEvent;
			return true;
        }

        private void SGAPlayer_PostCharmsUpdateEquipsEvent(SGAPlayer sgaplayer)
        {
			Player player = sgaplayer.player;
			DergWingsPlayer drakeplayer = player.GetModPlayer<DergWingsPlayer>();

			if (player.wingsLogic == item.wingSlot)
			{
				bool canfly = true;

				if (drakeplayer.draconicBoost)
				{
					player.wingTimeMax += 180;
					player.armorPenetration += 50;
					if (player.controlUp && ((player.mount != null && !player.mount.Active) || player.mount == null))
					{
						if (player.statLife <= 50)
						{
							canfly = false;
						}
						else
						{
							if (player.wingTime < 2)
							{
								player.statLife -= 1;
								player.netLife = true;
								player.wingTime = 2;
							}
						}
					}
				}

				if (player.wingTime > 0 && player.controlJump)
				{
					drakeplayer.wingFrames += 1;
					drakeplayer.flyingAngle.Item2 = 4;
				}

				if ((drakeplayer.wingFrames) % (9 * 4) == 4 * 3)
				{

					if ((player.wingTime > 0 || drakeplayer.draconicBoost) && player.controlUp)//Not Vanity
					{

						player.velocity.Y -= player.gravDir * 8f;

						for (float f = -24; f <= 16; f += 2)
						{
							Dust dust = Dust.NewDustPerfect(new Vector2(player.Center.X + (f * player.direction), player.Center.Y), ModContent.DustType<Armors.Engineer.AdaptedEngieSmokeEffect>(), new Vector2((Main.rand.NextFloat(-4, 4)) - player.velocity.X, Main.rand.NextFloat(24, 96)), 120, Color.Gray, 1f);
							dust.color = new Color(196, 179, 143);
						}
						SGAmod.AddScreenShake(8, 250, player.Center);
					}
				}
			}
		}

        public override void UpdateVanity(Player player, EquipType type)
		{
			if (player.wingTime < 1 && player.velocity.Y != 0 && player.controlJump && player.wings == item.wingSlot)
			{
				player.GetModPlayer<DergWingsPlayer>().wingFrames = 4 * 10;
				player.GetModPlayer<DergWingsPlayer>().flyingAngle.Item2 = 4;
			}
			player.GetModPlayer<SGAPlayer>().CustomWings = 2;
			DrawWings(player);
		}

		public override bool WingUpdate(Player player, bool inUse)
		{
			if (inUse)
			{
				if (player.wingsLogic != item.wingSlot && player.wingTime > 0 && player.controlJump)
				{
					player.GetModPlayer<DergWingsPlayer>().wingFrames += 1;
					player.GetModPlayer<DergWingsPlayer>().flyingAngle.Item2 = 2;
				}

				if ((player.GetModPlayer<DergWingsPlayer>().wingFrames) % (9*4) == 4*3)
                {
					var snd = Main.PlaySound(SoundID.Item,(int)player.MountedCenter.X, (int)player.MountedCenter.Y, 32);
					snd.Pitch = 0.80f;
				}
			}
			else
			{
				player.GetModPlayer<DergWingsPlayer>().wingFrames = 7 * 10;
			}
			return true;
		}

        public void UpdateAccessoryLocal(Player player, bool hideVisual, bool doBiomeThings = true)
		{
			player.wingTimeMax = 240;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			UpdateAccessoryLocal(player, hideVisual);
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.3f;
			ascentWhenRising = 0.45f;
			maxCanAscendMultiplier = 1.65f;
			maxAscentMultiplier = 4f;
			constantAscend = 0.045f;

			if (player.controlDown && player.controlJump && player.wingTime > 0)
			{
				maxCanAscendMultiplier /= 8f;
				maxAscentMultiplier /= 8f;
				constantAscend /= 8f;
			}

		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			float speed2 = 12f;
			float acceleration2 = 4.5f;
			if (player.controlDown && player.controlJump && player.wingTime>0)
            {
				speed2 += 4f;
				acceleration2 += 24f;
			}
			speed = speed2;
			acceleration *= acceleration2;
		}
		public override void AddRecipes()
		{
			DragonClawsRecipe recipe = new DragonClawsRecipe(mod);
			recipe.AddIngredient(ItemID.BetsyWings, 1);
			recipe.AddIngredient(ItemID.WireCutter, 1);
			recipe.AddIngredient(ModContent.ItemType<MoneySign>(), 15);
			recipe.AddIngredient(ItemID.GoldCoin, 50);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(ModContent.ItemType<OmegaSigil>(), 1);
			recipe2.AddIngredient(ModContent.ItemType<HavocGear.Items.FieryShard>(), 20);
			recipe2.AddIngredient(ModContent.ItemType<MoneySign>(), 25);
			recipe2.AddIngredient(ItemID.DefenderMedal, 25);
			recipe2.AddIngredient(ItemID.GoldCoin, 75);
			recipe2.AddTile(TileID.WorkBenches);
			recipe2.SetResult(this, 1);
			recipe2.AddRecipe();
		}
	}
}