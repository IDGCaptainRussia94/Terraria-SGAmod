using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{
	public class AureateVaultItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aureate Vault");
			Tooltip.SetDefault("'Entry point to the Planes of Wealth, but you can't reach inside'\nGrants nearby players Aureation Aura, which gilds nearby NPCs\nCan be right clicked to collect any money sacrificed to the Midas Insignia\nInfinite Gold can be exctracted over time, but not by hand");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "sacrificedMoney", Main.LocalPlayer.SGAPly().midasMoneyConsumed / (float)Item.buyPrice(1) + " platinum collected"));
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = Item.sellPrice(gold: 10);
			item.rare = ItemRarityID.Yellow;
			item.createTile = mod.TileType("AureateVault");
			item.placeStyle = 0;
		}
	}
}