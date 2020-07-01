using Microsoft.Xna.Framework;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Localization;
using Terraria.GameContent.UI;
using Idglibrary;

namespace SGAmod
{
	public class ScrapMetalCurrency: CustomCurrencySingleCoin
	{
		public Color SGACustomCurrencyTextColor = Color.BlueViolet;

		public ScrapMetalCurrency(int coinItemID, long currencyCap) : base(coinItemID, currencyCap)
		{
		}

		public override bool Accepts(Item item)
		{
			return true;
		}

		public override long CountCurrency(out bool overFlowing, Item[] inv, params int[] ignoreSlots)
		{
			overFlowing = false;
			return (long)99999;
		}

		public override bool TryPurchasing(int price, List<Item[]> inv, List<Point> slotCoins, List<Point> slotsEmpty, List<Point> slotEmptyBank, List<Point> slotEmptyBank2, List<Point> slotEmptyBank3)
		{
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			if (modplayer.ExpertiseCollected >= price*10)
			{
				modplayer.ExpertiseCollected -= price*10;
				return true;
			}
			return false;
		}

		public override void DrawSavingsMoney(SpriteBatch sb, string text, float shopx, float shopy, long totalCoins, bool horizontal = false)
		{
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			base.DrawSavingsMoney(sb, text, shopx, shopy, totalCoins, horizontal);
		}

		public override void GetPriceText(string[] lines, ref int currentLine, int price)
		{
			Color color = SGACustomCurrencyTextColor * ((float)Main.mouseTextColor / 255f);
			SGAPlayer modplayer = Main.LocalPlayer.GetModPlayer<SGAPlayer>();
			string text = " (Current: " + modplayer.ExpertiseCollected + "/Total: " + modplayer.ExpertiseCollectedTotal + ")";
			lines[currentLine++] = string.Format("[c/{0:X2}{1:X2}{2:X2}:{3} {4} {5}]", new object[]
				{
					color.R,
					color.G,
					color.B,
					Language.GetTextValue("LegacyTooltip.50"),
					price*10,
					"Expertise"+text
				});
		}
	}
}
