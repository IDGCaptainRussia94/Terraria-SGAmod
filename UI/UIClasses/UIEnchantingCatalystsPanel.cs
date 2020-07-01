using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using SGAmod.Items;
using Terraria.ModLoader;

namespace SGAmod.UI.UIClasses
{
	internal class UIEnchantingCatalystPanel : UIInteractableItemPanel
	{
		public UIEnchantingCatalystPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
		}

		public virtual int enchantmentboost => 0;

		public override bool CanTakeItem(Item item)
		{
			EnchantmentCraftingMaterial valuz;
			bool find = SGAmod.EnchantmentCatalyst.TryGetValue(item.type, out valuz);
			return find;
		}
	}

	internal class EnchantingCataylst2Panel : UIEnchantingCatalystPanel
	{
		public EnchantingCataylst2Panel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
		}

		public override int enchantmentboost => 5;

		public override bool CanTakeItem(Item item)
		{
			return item.type == ModContent.ItemType<UnmanedBar>();
		}
	}

	internal class UIEnchantingBookShelfPanel : UIEnchantingCatalystPanel
	{
		public UIEnchantingBookShelfPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
		}

		public override int enchantmentboost => 5;

		public override bool CanTakeItem(Item item)
		{
			return item.HoverName.Contains("Bookcase");
		}
	}
	internal class UIEnchantingCrystalBallPanel : UIEnchantingCatalystPanel
	{
		public UIEnchantingCrystalBallPanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
		}

		public override int enchantmentboost => 8;

		public override bool CanTakeItem(Item item)
		{
			return item.type==ItemID.CrystalBall;
		}
	}
	internal class UIEnchantingShinyStonePanel : UIEnchantingCatalystPanel
	{
		public UIEnchantingShinyStonePanel(int netID = 0, int stack = 0, Texture2D hintTexture = null, string hintText = null) : base(netID, stack, hintTexture, hintText)
		{
		}

		public override int enchantmentboost => 20;

		public override bool CanTakeItem(Item item)
		{
			EnchantmentCraftingMaterial valuz;
			bool find = SGAmod.EnchantmentFocusCrystal.TryGetValue(item.type, out valuz);
			return find;
		}
	}
}
