using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Dyes
{
	public class BloomDye : ModItem
	{
		//PlayerHooks.GetDyeTraderReward(this, list);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloom Dye");
		}

		public override void SetDefaults() {
			byte dye = item.dye;
			item.CloneDefaults(ItemID.GelDye);
			item.dye = dye;
		}
	}
}
