using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Accessories
{
	public class SerratedTooth : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Serrated Tooth");
            Tooltip.SetDefault("Dealing more damage than 5 times an enemy's defense inflicts Massive Bleeding\nbase duration is 1 second and increases further over an enemy's defense your attack is\nHowever this is hard capped at 5 seconds\n+10 Armor Penetration");
        }

        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 15, 0, 0);;
			item.rare = 5;
			item.accessory = true;
			item.expert=true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		player.armorPenetration+=10;
		player.GetModPlayer<SGAPlayer>().SerratedTooth = true;
		}
	}
}