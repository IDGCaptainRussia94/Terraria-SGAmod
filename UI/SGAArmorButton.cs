using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using SGAmod.UI;
using SGAmod.UI.UIClasses;
using Terraria.GameInput;

namespace SGAmod.UI
{
	public class SGAArmorButton : UIState
	{

		internal bool visible;

		internal UIArmorButtonPanel ItemPanel;


		public SGAArmorButton()
		{
			/*SetPadding(10f);
			Width.Set(128f, 0f);
			Height.Set(128f, 0f);
			_UIView = new UIElement();
			_UIView.CopyStyle(this);
			_UIView.Left.Set((float)Main.screenWidth / 2f - _UIView.Width.Pixels / 2f, 0f);
			_UIView.Top.Set((float)Main.screenHeight / 2f - _UIView.Height.Pixels / 2f, 0f);
			Append(_UIView);*/
		}

		public override void OnInitialize()
		{

			ItemPanel = new UIArmorButtonPanel();
			ItemPanel.Left.Set(1782, 0);// 0.9435f);
			ItemPanel.Top.Set((Main.mapStyle == 1 ? 398 : 142), 0);
			ItemPanel.HintText = "Click here to toggle Armor effects";
			Append(ItemPanel);

		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//Main.NewText("tests");
			Recalculate();

			bool hovering = ItemPanel.Left.Pixels <= Main.mouseX && Main.mouseX <= ItemPanel.Left.Pixels + ItemPanel.Width.Pixels &&
	ItemPanel.Top.Pixels <= Main.mouseY && Main.mouseY <= ItemPanel.Top.Pixels + ItemPanel.Height.Pixels && !PlayerInput.IgnoreMouseInterface;
			if (hovering)
			{
				Main.LocalPlayer.mouseInterface = true;
			}
			//ItemPanel.Draw(spriteBatch);

			base.DrawSelf(spriteBatch);
		}

		public override void Update(GameTime gameTime)
		{

			int placement = 0;
			int mH = (int)(typeof(Main).GetField("mH", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null));
			int num45 = (int)((float)(174 + mH) + (float)(placement * 56) * Main.inventoryScale)-32;


			ItemPanel.Left.Set(Main.screenWidth-107, 0);// 0.9435f);
			ItemPanel.Top.Set((Main.mapStyle == 1 ? num45 : 142), 0);
			//ItemPanel.Recalculate();
			base.Update(gameTime);

		}
	}

	internal class UIArmorButtonPanel : UIPanel
	{
		internal const float panelwidth = 24f;

		internal const float panelheight = 10f;

		internal const float panelpadding = 0f;

		public string HintText
		{
			get;
			set;
		}


		public UIArmorButtonPanel(float width = 28f, float height = 32f)
		{
			this.Width.Set(width, 0f);
			this.Height.Set(height, 0f);
			base.SetPadding(0f);
		}

        public override void Click(UIMouseEvent evt)
        {
			//if (!Main.dedServ)
			//{
				Main.PlaySound(SoundID.Mech, new Vector2(-1, -1),0);
			Main.LocalPlayer.SGAPly().armorToggleMode = !Main.LocalPlayer.SGAPly().armorToggleMode;
			//}
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			//base.DrawSelf(spriteBatch);
			CalculatedStyle innerDimensions = base.GetInnerDimensions();

			spriteBatch.Draw(Main.EquipPageTexture[0], innerDimensions.Position(), null, (Main.LocalPlayer.SGAPly().armorToggleMode ? Color.White : Color.Gray), 0f, Vector2.Zero, Vector2.One / 1f, SpriteEffects.None, 0f);
			//spriteBatch.Draw(Main.cursorTextures[15], innerDimensions.Position(), null, (Main.LocalPlayer.SGAPly().armorToggleMode ? Color.Lime : Color.Red)*0.75f, 0f, new Vector2(-8, -8), Vector2.One / 1f, SpriteEffects.None, 0f);

			if (base.IsMouseHovering)
            {
				Main.hoverItemName = (this.HintText ?? string.Empty);
			}


		}
	}

}
