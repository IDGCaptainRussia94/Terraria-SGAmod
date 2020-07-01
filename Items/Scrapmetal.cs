using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items
{
	public class Scrapmetal : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Scrap Metal");
			Tooltip.SetDefault("A peice of the never ending gravel wars");

		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.rare = 0;
			item.value = 0;//Item.sellPrice(0, 0, 20, 0);		
		}
	}
}