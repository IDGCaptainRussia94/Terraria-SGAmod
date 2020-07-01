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

namespace SGAmod.UI
{
	public class SGACustomUIMenu : UIState
	{

		internal Point16? currentTEPosition;

		internal bool visible;

		internal bool dragging;

		private Vector2 offset;

		internal const float vpadding = 10f;

		internal const float vwidth = 555f;

		internal const float vheight = 400f;

		internal UIElement _UIView;

		internal UIPanel basePanel;

		internal UIText baseTitle;

		internal UIImageButton closeButton;

		internal UIEnchantingCatalystPanel EnchantingCataylstPanel;

		internal EnchantingCataylst2Panel EnchantingCataylstPanel2;

		internal List<UIEnchantingCatalystPanel> EnchantingStationsPanels;

		internal UIEItemPanel EItemPanel;

		internal UIImageButton[] EnchantButton;

		internal UIPanel bufferpanel;

		internal static List<Recipe> currentRecipes;

		internal static List<short> failureTypes;

		internal static string hoverString;

		public int powerlevel = 0;

		public SGACustomUIMenu()
		{
			SetPadding(10f);
			Width.Set(555f, 0f);
			Height.Set(400f, 0f);
			_UIView = new UIElement();
			_UIView.CopyStyle(this);
			_UIView.Left.Set((float)Main.screenWidth / 2f - _UIView.Width.Pixels / 2f, 0f);
			_UIView.Top.Set((float)Main.screenHeight / 2f - _UIView.Height.Pixels / 2f, 0f);
			Append(_UIView);
		}

		public override void OnInitialize()
		{

			this.basePanel = new UIPanel();
			this.basePanel.CopyStyle(this);
			this.basePanel.SetPadding(10f);
			this._UIView.Append(this.basePanel);

			bufferpanel = new UIPanel();
			bufferpanel.SetPadding(0);
			// We need to place this UIElement in relation to its Parent. Later we will be calling `base.Append(coinCounterPanel);`. 
			// This means that this class, ExampleUI, will be our Parent. Since ExampleUI is a UIState, the Left and Top are relative to the top left of the screen.
			bufferpanel.Left.Set(0f, 0f);
			bufferpanel.Top.Set(0f, 0f);
			bufferpanel.Width.Set((float)Width.Pixels, 0f);
			bufferpanel.Height.Set(30f, 0f);
			bufferpanel.BackgroundColor = new Color(200, 94, 96);
			basePanel.Append(bufferpanel);

			UIText baseTitle2 = new UIText("Enchanting", 0.75f, true);
			baseTitle2.Left.Set(0, 0f);
			bufferpanel.Append(baseTitle2);

			UIImageButton uIImageButton = new UIImageButton(ModContent.GetTexture("Terraria/UI/ButtonDelete"));
			uIImageButton.OnClick += new UIElement.MouseEvent(this.CloseButton_OnClick);
			uIImageButton.Width.Set(20f, 0f);
			uIImageButton.Height.Set(20f, 0f);
			uIImageButton.Left.Set(this.basePanel.Width.Pixels - uIImageButton.Width.Pixels * 2f - 47.5f, 0f);
			uIImageButton.Top.Set((uIImageButton.Height.Pixels / 2f) - 8f, 0f);
			bufferpanel.Append(uIImageButton);

			EnchantingStationsPanels = new List<UIEnchantingCatalystPanel>();

			float nextinline = 0;
			UIEnchantingBookShelfPanel mypanel = new UIEnchantingBookShelfPanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.Bookcase), "Place Bookcase Here");
			mypanel.Left.Set(nextinline, 0f);
			mypanel.Top.Set(40f, 0f);
			basePanel.Append(mypanel);
			EnchantingStationsPanels.Add(mypanel);

			nextinline = (mypanel.Left.Pixels + mypanel.Width.Pixels) + 15f;
			UIEnchantingCrystalBallPanel mypanel2 = new UIEnchantingCrystalBallPanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.CrystalBall), "Place Crystal Ball Here");
			mypanel2.Left.Set(nextinline, 0f);
			mypanel2.Top.Set(40f, 0f);
			basePanel.Append(mypanel2);
			EnchantingStationsPanels.Add(mypanel2);

			nextinline = (mypanel2.Left.Pixels + mypanel2.Width.Pixels) + 15f;
			UIEnchantingShinyStonePanel mypanel3 = new UIEnchantingShinyStonePanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.ShinyStone), "Place Focusing Crystal Here");
			mypanel3.Left.Set(nextinline, 0f);
			mypanel3.Top.Set(40f, 0f);
			basePanel.Append(mypanel3);
			EnchantingStationsPanels.Add(mypanel3);

			nextinline = (mypanel3.Left.Pixels + mypanel3.Width.Pixels) + 15f;
			mypanel2 = new UIEnchantingCrystalBallPanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.CrystalBall), "Place Crystal Ball Here");
			mypanel2.Left.Set(nextinline, 0f);
			mypanel2.Top.Set(40f, 0f);
			basePanel.Append(mypanel2);
			EnchantingStationsPanels.Add(mypanel2);

			nextinline = (mypanel2.Left.Pixels + mypanel2.Width.Pixels) + 15f;
			mypanel = new UIEnchantingBookShelfPanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.Bookcase), "Place Bookcase Here");
			mypanel.Left.Set(nextinline, 0f);
			mypanel.Top.Set(40f, 0f);
			basePanel.Append(mypanel);
			EnchantingStationsPanels.Add(mypanel);



			this.EnchantingCataylstPanel = new UIEnchantingCatalystPanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.Diamond), "Place 5 Catalyst Agents here");
			this.EnchantingCataylstPanel.Top.Set(100, 0f);
			this.basePanel.Append(this.EnchantingCataylstPanel);

			this.EnchantingCataylstPanel2 = new EnchantingCataylst2Panel(0, 0, ModContent.GetTexture("SGAmod/Items/UnmanedBar"), "Place 5 Novus bars here");
			this.EnchantingCataylstPanel2.Left.Set((EnchantingCataylstPanel.Left.Pixels + EnchantingCataylstPanel.Width.Pixels) + 15f, 0f);
			this.EnchantingCataylstPanel2.Top.Set(100, 0f);
			this.basePanel.Append(this.EnchantingCataylstPanel2);

			this.EItemPanel = new UIEItemPanel(0, 0, ModContent.GetTexture("Terraria/Item_" + ItemID.CopperShortsword), "Place Weapon Here");
			this.EItemPanel.Top.Set(260, 0f);
			this.basePanel.Append(this.EItemPanel);
			this.EItemPanel.DoUpdate();

			baseTitle = new UIText("Power: ", 0.5f, true);
			baseTitle.Left.Set(0, 0f);
			baseTitle.Top.Set(-30, 0f);
			EItemPanel.Append(baseTitle);


			EnchantButton = new UIImageButton[3];

			this.EnchantButton[0] = new UIImageButton(ModContent.GetTexture("Terraria/Item_" + ItemID.MagicMirror));
			this.EnchantButton[0].OnClick += new UIElement.MouseEvent(this.EnchantButtonOnClick);
			this.EnchantButton[0].Width.Set(30f, 0f);
			this.EnchantButton[0].Height.Set(30f, 0f);
			this.EnchantButton[0].Left.Set(300f, 0f);
			this.EnchantButton[0].Top.Set((260), 0f);
			this.basePanel.Append(this.EnchantButton[0]);

			UIText baseTitle3 = new UIText("Enchant", 0.5f, true);
			baseTitle3.Left.Set(-20, 0f);
			baseTitle3.Top.Set(40, 0f);
			this.EnchantButton[0].Append(baseTitle3);

		}

		private void CloseButton_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			SGAmod.TryToggleUI(new bool?(false));
		}

		public void _Recalculate(Vector2 mousePos, float precent = 0f)
		{
			this._UIView.Left.Set(Math.Max(-20f, Math.Min(mousePos.X - this.offset.X, (float)Main.screenWidth - this.basePanel.Width.Pixels + 20f)), precent);
			this._UIView.Top.Set(Math.Max(-20f, Math.Min(mousePos.Y - this.offset.Y, (float)Main.screenHeight - this.basePanel.Height.Pixels + 20f)), precent);
			this.Recalculate();
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			if (this.visible)
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
			}
		}

		public void TryGetSource(bool force = false)
		{
			if (!this.EItemPanel.item.IsAir && (!this.visible | force))
			{
				Main.LocalPlayer.QuickSpawnClonedItem(this.EItemPanel.item, this.EItemPanel.item.stack);
				this.EItemPanel.item.TurnToAir();
			}
		}

		public void TryGetCube(bool force = false)
		{
			if (!this.EnchantingCataylstPanel.item.IsAir && (!this.visible | force))
			{
				Main.LocalPlayer.QuickSpawnClonedItem(this.EnchantingCataylstPanel.item, this.EnchantingCataylstPanel.item.stack);
				this.EnchantingCataylstPanel.item.TurnToAir();
			}

			if (!this.EnchantingCataylstPanel2.item.IsAir && (!this.visible | force))
			{
				Main.LocalPlayer.QuickSpawnClonedItem(this.EnchantingCataylstPanel2.item, this.EnchantingCataylstPanel2.item.stack);
				this.EnchantingCataylstPanel2.item.TurnToAir();
			}

			for (int i = 0; i < EnchantingStationsPanels.Count; i += 1)
			{
				if (!EnchantingStationsPanels[i].item.IsAir && (!this.visible | force))
				{
					Main.LocalPlayer.QuickSpawnClonedItem(EnchantingStationsPanels[i].item, EnchantingStationsPanels[i].item.stack);
					EnchantingStationsPanels[i].item.TurnToAir();
				}
			}
		}

		public void ToggleUI(bool on = true)
		{
			if (!on)
			{
				this.TryGetCube(false);
				this.TryGetSource(false);
				return;
			}
			this.EItemPanel.DoUpdate();
		}

		private void EnchantButtonOnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!EnchantingCataylstPanel.item.IsAir && !EnchantingCataylstPanel2.item.IsAir && EnchantingCataylstPanel.item.stack >= 5 && EnchantingCataylstPanel2.item.stack >= 5)
			{

				EnchantingCataylstPanel.item.stack -= 5;
				if (EnchantingCataylstPanel.item.stack < 1)
					EnchantingCataylstPanel.item.TurnToAir();

				EnchantingCataylstPanel2.item.stack -= 5;
				if (EnchantingCataylstPanel2.item.stack < 1)
					EnchantingCataylstPanel2.item.TurnToAir();

				this.EItemPanel.item.damage += 10;


				Main.PlaySound(SoundID.Item, -1, -1, 46, 1f, 0.25f);
			}
		}

		public override void Update(GameTime gameTime)
		{
			powerlevel = 0;
			powerlevel += GetPowerLevel();

			baseTitle.SetText("Power: " + powerlevel);
			base.Update(gameTime);
		}

		public int GetPowerLevel()
		{
			int power = 0;

			for (int i = 0; i < EnchantingStationsPanels.Count; i += 1)
			{
				if (!EnchantingStationsPanels[i].item.IsAir)
				{
					EnchantmentCraftingMaterial valuz;
					bool find = SGAmod.EnchantmentFocusCrystal.TryGetValue(EnchantingStationsPanels[i].item.type, out valuz);
					if (find)
						power += valuz.value;
					else
					power += EnchantingStationsPanels[i].enchantmentboost;
				}
			}
			float powersofar = 1f+(power/40f);
			float powa2 = 0;
			if (!EnchantingCataylstPanel.item.IsAir)
			{
				EnchantmentCraftingMaterial valuz;
				bool find = SGAmod.EnchantmentCatalyst.TryGetValue(EnchantingCataylstPanel.item.type, out valuz);
				if (find)
				powa2 += valuz.value;
			}
			power += (int)(powa2 * powersofar);
			return power;
		}
	}
}
