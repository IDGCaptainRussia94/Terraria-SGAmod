using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;

namespace SGAmod.UI
{
	internal sealed class UIEItemPanel : UIInteractableItemPanel
	{
		public UIEItemPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
		}

		public override bool CanTakeItem(Item item)
		{
			return item.damage > 1;
		}

		public override void PostOnClick(UIMouseEvent evt, UIElement e)
		{
			this.DoUpdate();
		}

		public override void PostOnRightClick()
		{
			this.DoUpdate();
		}

		internal void DoUpdate()
		{
		}
	}
}
