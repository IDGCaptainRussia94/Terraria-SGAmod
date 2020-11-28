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
	public class SGACraftBlockPanel : UIState
	{

		internal bool visible;

		internal UIWorkbenchPanel ItemPanel;


		public SGACraftBlockPanel()
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

			ItemPanel = new UIWorkbenchPanel(0, 0, Main.itemTexture[ItemID.WorkBench], "");
			ItemPanel.Left.Set(560, 0);
			ItemPanel.Top.Set(32, 0);

			Append(ItemPanel);

		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			Recalculate();

			bool hovering = ItemPanel.Left.Pixels <= Main.mouseX && Main.mouseX <= ItemPanel.Left.Pixels + ItemPanel.Width.Pixels &&
	ItemPanel.Top.Pixels <= Main.mouseY && Main.mouseY <= ItemPanel.Top.Pixels + ItemPanel.Height.Pixels && !PlayerInput.IgnoreMouseInterface;
			if (hovering)
			{
				Main.LocalPlayer.mouseInterface = true;

			}

				base.DrawSelf(spriteBatch);
			/*if (this.visible)
			{
				Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY);
				if (this._UIView.ContainsPoint(vector))
				{
					Main.LocalPlayer.mouseInterface = true;
				}
				if (this.dragging)
				{
					this._Recalculate(vector, 0f);
				}
			}*/
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
		}
	}
}
