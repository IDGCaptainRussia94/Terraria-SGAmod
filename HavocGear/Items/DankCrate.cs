using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

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
      Tooltip.SetDefault("Right click to open");
    }


        public override bool CanRightClick()
        {
            return true;
        }

        public override void RightClick(Player player)
        {

			List<int> types=new List<int>();
			types.Insert(types.Count,0);
			types.Insert(types.Count,1);
			types.Insert(types.Count,2);
			types.Insert(types.Count,3);
			types.Insert(types.Count,4);
			types.Insert(types.Count,5);
			types.Insert(types.Count,7);
			types.Insert(types.Count,8);
			types.Insert(types.Count,9);
			types.Insert(types.Count,10);
			types.Insert(types.Count,11);
			types.Insert(types.Count,12);

			int pick=types[Main.rand.Next(0,types.Count)];

            int rand = Main.rand.Next(12);
		if (!Main.hardMode)
            player.QuickSpawnItem(ItemID.SilverCoin, Main.rand.Next(15, 80));
		else
            player.QuickSpawnItem(ItemID.GoldCoin, Main.rand.Next(1, 5));
            if (pick == 0)
                player.QuickSpawnItem(Main.hardMode ? ItemID.CobaltBar : ItemID.IronBar, Main.rand.Next(4, 9));
            else if (pick == 1)
                player.QuickSpawnItem(Main.hardMode ? ItemID.PalladiumBar : ItemID.LeadBar, Main.rand.Next(4, 9));
            else if (pick == 2)
                player.QuickSpawnItem(Main.hardMode ? ItemID.MythrilBar : ItemID.SilverBar, Main.rand.Next(4, 9));
            else if (pick == 3)
                player.QuickSpawnItem(Main.hardMode ? ItemID.OrichalcumBar : ItemID.TungstenBar, Main.rand.Next(4, 9));
            else if (pick == 4)
                player.QuickSpawnItem(Main.hardMode ? ItemID.AdamantiteBar : ItemID.PlatinumBar, Main.rand.Next(4, 9));
            else if (pick == 5)
                player.QuickSpawnItem(Main.hardMode ? ItemID.TitaniumBar : ItemID.GoldBar, Main.rand.Next(4, 9));
            else if (pick == 7)
                player.QuickSpawnItem(mod.ItemType("DankWood"), Main.rand.Next(8, 25)); 
            else if (pick == 8)                
                player.QuickSpawnItem(mod.ItemType("NoviteOre"), Main.rand.Next(4, 16)); 
            else if (pick == 9)
                player.QuickSpawnItem(mod.ItemType("DankCore"), Main.rand.Next(1, 2)); 
            else if (pick == 10)
                player.QuickSpawnItem(mod.ItemType("DecayedMoss"), Main.rand.Next(2, 5));    
            else if (pick == 11)
                player.QuickSpawnItem(mod.ItemType("Biomass"), Main.rand.Next(4, 16));    
            else if (pick == 12)
                player.QuickSpawnItem(mod.ItemType("UnmanedOre"), Main.rand.Next(4, 16));    

        }
	}
}
