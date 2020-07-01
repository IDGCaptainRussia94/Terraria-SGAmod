using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.UI;
using Terraria.ID;
using SGAmod.UI.UIClasses;

namespace SGAmod.UI
{
	internal class UIInteractableItemPanel : UIItemPanel
	{
		public UIInteractableItemPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
			base.OnClick += new UIElement.MouseEvent(this.UIInteractableItemPanel_OnClick);
		}

		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (base.IsMouseHovering && Main.mouseRight)
			{
				if (!this.item.IsAir)
				{
					Main.playerInventory = true;
					if (Main.stackSplit <= 1 && (Main.mouseItem.type == this.item.type || Main.mouseItem.IsAir))
					{
						int num = Main.superFastStack + 1;
						for (int i = 0; i < num; i++)
						{
							if (Main.mouseItem.IsAir || (Main.mouseItem.stack < Main.mouseItem.maxStack && this.item.stack > 0))
							{
								if (i == 0)
								{
									Main.PlaySound(SoundID.Coins, -1, -1, 1, 1f, 0f);
								}
								if (Main.mouseItem.IsAir)
								{
									Main.mouseItem = this.item.Clone();
									if (this.item.prefix != 0)
									{
										Main.mouseItem.Prefix((int)this.item.prefix);
									}
									Main.mouseItem.stack = 0;
								}
								Main.mouseItem.stack++;
								this.item.stack--;
								Main.stackSplit = ((Main.stackSplit == 0) ? 15 : Main.stackDelay);
								if (this.item.stack <= 0)
								{
									this.item.TurnToAir();
								}
							}
						}
					}
				}
				this.PostOnRightClick();
			}
		}

		public virtual bool CanTakeItem(Item item)
		{
			return true;
		}

		public virtual void PostOnRightClick()
		{
			if (!item.IsAir)
			Main.PlaySound(SoundID.MenuTick, -1, -1, 0, 1f, 0f);
		}

		public virtual void PostOnClick(UIMouseEvent evt, UIElement e)
		{
			if (!item.IsAir)
				Main.PlaySound(SoundID.Grab, -1, -1, 0, 1f, 0f);
		}

		private void UIInteractableItemPanel_OnClick(UIMouseEvent evt, UIElement e)
		{
			if (!this.item.IsAir)
			{
				if (Main.mouseItem.IsAir)
				{
					Main.playerInventory = true;
					Main.mouseItem = this.item.Clone();
					this.item.TurnToAir();
				}
				else if (this.CanTakeItem(Main.mouseItem))
				{
					Main.playerInventory = true;
					if (this.item.type == Main.mouseItem.type)
					{
						int num = this.item.stack + Main.mouseItem.stack;
						if (this.item.maxStack >= num)
						{
							this.item.stack = num;
							Main.mouseItem.TurnToAir();
						}
						else
						{
							int stack = num - this.item.maxStack;
							this.item.stack = this.item.maxStack;
							Main.mouseItem.stack = stack;
						}
					}
					else
					{
						Item arg_F6_0 = this.item.Clone();
						Item item = Main.mouseItem.Clone();
						Main.mouseItem = arg_F6_0;
						this.item = item;
					}
				}
			}
			else if (this.CanTakeItem(Main.mouseItem))
			{
				Main.playerInventory = true;
				this.item = Main.mouseItem.Clone();
				Main.mouseItem.TurnToAir();
			}
			this.PostOnClick(evt, e);
		}
	}
}
