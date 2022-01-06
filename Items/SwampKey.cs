using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SGAmod.Items
{
	public class SwampKey : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 32;
			item.maxStack = 99;
			item.rare = ItemRarityID.Yellow;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (NPC.downedPlantBoss == false)
			{
				tooltips.Add(new TooltipLine(mod, "PrePlantera", "It has been cursed by a powerful Jungle creature"));
			}
			if (NPC.downedPlantBoss == true)
			{
				tooltips.Add(new TooltipLine(mod, "PostPlantera", "Unlocks a Swamp Chest in the dungeon"));
			}
		}
	}
}
