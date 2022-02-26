using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;


namespace SGAmod.Items.Armors
{

	[AutoloadEquip(EquipType.Head)]
	public class AncientHallowedVisor : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hallowed Visor");
			Tooltip.SetDefault("12% increased throwing damage\n10% increased throwing crit\n20% increased throwing velocity");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,5);
			item.rare = ItemRarityID.Pink;
			item.defense = 16;
		}
		public override void UpdateEquip(Player player)
		{
			player.Throwing().thrownDamage += 0.12f;
			player.Throwing().thrownCrit += 10;
			player.Throwing().thrownVelocity += 0.20f;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 12);
			recipe.AddTile(GetType() == typeof(HallowedVisor) ? TileID.DemonAltar : TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Head)]
	public class HallowedVisor : AncientHallowedVisor
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Future Hallowed Visor");
			Tooltip.SetDefault("12% increased throwing damage\n10% increased throwing crit\n20% increased throwing velocity\n'Reverse 1.4 lol'");
		}
	}

}