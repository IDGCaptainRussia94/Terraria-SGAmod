using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Consumable
{
	public class ConsumeHell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Consumable Hell");
			Tooltip.SetDefault("A Red Devil's favorite, eating it will turn your mouth into a literal flame thrower\nThis lasts for 1 minute, cannot be cancelled\nSeveral peppers may be consumed in succession to increase the power of the fire breath\nDeals non-class damage, does not crit\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 90 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool UseItem(Player player)
		{
			player.SGAPly().AddCooldownStack(60 * 90, 1);
			player.SGAPly().FireBreath += 1;
			player.AddBuff(mod.BuffType("ConsumeHellBuff"), 60 * 60);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 5;
			item.value = 15000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
			//item.buffType = mod.BuffType("ConsumeHellBuff");
			//item.buffTime = 60*60;
		}
	}

	public class EnergizerBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energizer Battery");
			Tooltip.SetDefault("'Keeping going and going...'\nInstantly restores 25% of your Max Electric Charge on use\nIncreases your Max Electric Charge by 200 per use (up to 2000)\nAfter Sharkvern, this is raised 3000\nAfter Luminite Wraith, this is raised again to 5000\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 40 seconds each"));
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool UseItem(Player player)
		{
			int max = 2000 + (SGAWorld.downedSharkvern ? 1000 : 0) + (SGAWorld.downedWraiths>3 ? 2000 : 0);
			player.SGAPly().Electicpermboost += 200;
			player.SGAPly().AddCooldownStack(60 * 40, 1);
			player.SGAPly().electricCharge += (int)((float)player.SGAPly().electricChargeMax*0.25f);
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item2;
			item.consumable = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VialofAcid"), 9);
			recipe.AddIngredient(mod.ItemType("BiomassBar"), 6);
			recipe.AddIngredient(ItemID.Bunny, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}

}