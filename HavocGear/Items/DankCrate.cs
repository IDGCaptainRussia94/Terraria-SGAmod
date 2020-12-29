using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using Terraria.Localization;

namespace SGAmod.HavocGear.Items
{
    public class DankCrate : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 18;
            item.height = 26;
            item.rare = 2;
            item.maxStack = 99;
            item.melee = false;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Crate");
      Tooltip.SetDefault(Language.GetTextValue("CommonItemTooltip.RightClickToOpen"));
    }


        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {
            int pick = Main.rand.Next(8);
		if (!Main.hardMode)
            player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(15, 80));
		else
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 3));

        if (Main.rand.Next(15) == 0)
                player.QuickSpawnItem(mod.ItemType("MurkyCharm"));

            if (pick == 0)
                player.QuickSpawnItem(Main.hardMode ? ItemID.CobaltBar : ItemID.IronBar, Main.rand.Next(3, 9));
            else if (pick == 1)
                player.QuickSpawnItem(Main.hardMode ? ItemID.PalladiumBar : ItemID.LeadBar, Main.rand.Next(3, 9));
            else if (pick == 2)
                player.QuickSpawnItem(Main.hardMode ? ItemID.MythrilBar : ItemID.SilverBar, Main.rand.Next(3, 9));
            else if (pick == 3)
                player.QuickSpawnItem(Main.hardMode ? ItemID.OrichalcumBar : ItemID.TungstenBar, Main.rand.Next(3, 9));
            else if (pick == 4)
                player.QuickSpawnItem(Main.hardMode ? ItemID.AdamantiteBar : ItemID.PlatinumBar, Main.rand.Next(3, 9));
            else if (pick == 5)
                player.QuickSpawnItem(Main.hardMode ? ItemID.TitaniumBar : ItemID.GoldBar, Main.rand.Next(3, 9));
            else if (pick == 6)
                player.QuickSpawnItem(SGAWorld.WorldIsNovus ? mod.ItemType("UnmanedBar") : mod.ItemType("NoviteBar"), Main.rand.Next(3, 9));
            else if (pick == 7)
                player.QuickSpawnItem(mod.ItemType("TransmutationPowder"), Main.rand.Next(4, 11));

            pick = Main.rand.Next(5);
            if (pick == 0)
                player.QuickSpawnItem(mod.ItemType("DankWood"), Main.rand.Next(12, 36)); 
            else if (pick == 1)
                player.QuickSpawnItem(mod.ItemType("DankCore"), Main.rand.Next(1, 2)); 
            else if (pick == 2)
                player.QuickSpawnItem(mod.ItemType("DecayedMoss"), Main.rand.Next(2, 5));    
            else if (pick == 3)
                player.QuickSpawnItem(mod.ItemType("Biomass"), Main.rand.Next(4, 16));    
            else if (pick == 4)
                player.QuickSpawnItem(SGAWorld.WorldIsNovus ? mod.ItemType("UnmanedOre") : mod.ItemType("NoviteOre"), Main.rand.Next(4, 16));    

        }
	}
}
