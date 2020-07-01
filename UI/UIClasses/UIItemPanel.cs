using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace SGAmod.UI
{
	internal class UIItemPanel : UIPanel
	{
		internal const float panelwidth = 50f;

		internal const float panelheight = 50f;

		internal const float panelpadding = 0f;

		public Item item;

		public string HintText
		{
			get;
			set;
		}

		public Texture2D HintTexture
		{
			get;
			set;
		}

		public UIItemPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null, float width = 50f, float height = 50f)
		{
			this.Width.Set(width, 0f);
			this.Height.Set(height, 0f);
			base.SetPadding(0f);
			this.item = new Item();
			this.item.netDefaults(netID);
			this.item.stack = stack;
			this.HintTexture = hintTexture;
			this.HintText = hintText;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();
			Texture2D texture2D;
			Color color;
			if (this.HintTexture != null && this.item.IsAir)
			{
				texture2D = this.HintTexture;
				color = Color.LightGray * 0.5f;
				if (base.IsMouseHovering)
				{
					Main.hoverItemName = (this.HintText ?? string.Empty);
				}
			}
			else
			{
				if (this.item.IsAir)
				{
					return;
				}
				texture2D = Main.itemTexture[this.item.type];
				color = this.item.GetAlpha(Color.White);
				if (base.IsMouseHovering)
				{
					Main.hoverItemName = this.item.Name;
					Main.HoverItem = this.item.Clone();
					Item arg_135_0 = Main.HoverItem;
					string arg_130_0 = Main.HoverItem.Name;
					ModItem expr_D3 = Main.HoverItem.modItem;
					string arg_130_1;
					if (expr_D3 == null)
					{
						arg_130_1 = null;
					}
					else
					{
						string arg_120_0 = expr_D3.mod.Name;
						ModItem expr_EE = Main.HoverItem.modItem;
						arg_130_1 = arg_120_0.Insert(((expr_EE != null) ? new int?(expr_EE.mod.Name.Length) : null).Value, "]").Insert(0, " [");
					}
					arg_135_0.SetNameOverride(arg_130_0 + arg_130_1);
				}
			}
			Rectangle rectangle = (!this.item.IsAir && Main.itemAnimations[this.item.type] != null) ? Main.itemAnimations[this.item.type].GetFrame(texture2D) : texture2D.Frame(1, 1, 0, 0);
			float num = 1f;
			if ((float)rectangle.Width > innerDimensions.Width || (float)rectangle.Height > innerDimensions.Width)
			{
				if (rectangle.Width > rectangle.Height)
				{
					num = innerDimensions.Width / (float)rectangle.Width;
				}
				else
				{
					num = innerDimensions.Width / (float)rectangle.Height;
				}
			}
			float num2 = num;
			Color white = Color.White;
			ItemSlot.GetItemLight(ref white, ref num, this.item.type, false);
			Vector2 position = new Vector2(innerDimensions.X, innerDimensions.Y);
			position.X += innerDimensions.Width * 1f / 2f - (float)rectangle.Width * num / 2f;
			position.Y += innerDimensions.Height * 1f / 2f - (float)rectangle.Height * num / 2f;
			if (this.item.modItem == null || this.item.modItem.PreDrawInInventory(spriteBatch, position, rectangle, color, color, Vector2.Zero, num))
			{
				spriteBatch.Draw(texture2D, position, new Rectangle?(rectangle), color, 0f, Vector2.Zero, num, SpriteEffects.None, 0f);
				Item expr_2B9 = this.item;
				if (expr_2B9 == null || expr_2B9.color != default(Color))
				{
					spriteBatch.Draw(texture2D, position, new Rectangle?(rectangle), color, 0f, Vector2.Zero, num, SpriteEffects.None, 0f);
				}
			}
			ModItem expr_303 = this.item.modItem;
			if (expr_303 != null)
			{
				expr_303.PostDrawInInventory(spriteBatch, position, rectangle, color, color, Vector2.Zero, num);
			}
			Item expr_321 = this.item;
			if (expr_321 != null && expr_321.stack > 1)
			{
				Utils.DrawBorderStringFourWay(spriteBatch, Main.fontItemStack, Math.Min(9999, this.item.stack).ToString(), innerDimensions.Position().X + 10f, innerDimensions.Position().Y + 26f, Color.White, Color.Black, Vector2.Zero, num2 * 0.8f);
			}
		}
	}
}
