using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Quests
{
	public class PremiumUpgrade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contracker");
			Tooltip.SetDefault("Activating this will grant it's owner the TF2 Emblem and allow Crate Drops\nThe crates will drop per world on activation, however only new characters will receive the TF2 Emblem");

		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.consumable = false;
			item.width = 40;
			item.height = 40;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.value = Item.buyPrice(0, 2, 0, 0);
		}
		//player.CountItem(mod.ItemType("ModItem"))

		public override bool UseItem(Player ply)
		{
			SGAWorld.QuestCheck(0,ply);
			return true;
		}
	}
}