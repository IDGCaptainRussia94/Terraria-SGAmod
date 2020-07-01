using Terraria.ModLoader;

namespace SGAmod.Items.Placeable
{
	public class PrismalStation: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Extractor");
			Tooltip.SetDefault("It would seem the Lihzahrds knew a way to awaken Novus to its full potiental...\nAllows transmuting of Novus ore into Prismal ore");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 10;
			item.rare = 10;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("PrismalStation");
		}
	}
}