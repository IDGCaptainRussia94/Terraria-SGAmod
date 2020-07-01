using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.HavocGear.Items.Accessories;
using Idglibrary;

namespace SGAmod.Items.Accessories.Charms
{
	public class MiningCharmlv1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Charm Tier 1");
			Tooltip.SetDefault("25% increased mining/hammering/chopping speed\n"+Idglib.ColorText(Color.Red,"Damage you take is increased by 20%"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GetType().Name.Length - GetType().Name.Replace("lv3","").Length>0)
			tooltips.Add(new TooltipLine(mod, "Lv3Charm", "Includes the functionality of Mechanical Minecart"));
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			bool canequip = true;
			for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
			{
				if (player.armor[x].modItem != null) {
					Type myclass = player.armor[x].modItem.GetType();
					if (myclass.BaseType == typeof(MiningCharmlv1) || myclass == typeof(MiningCharmlv1)) {

						//if (myType==mod.ItemType("MiningCharmlv1") || myType==mod.ItemType("MiningCharmlv2") || myType == mod.ItemType("MiningCharmlv3")){
						canequip = false;
						break;
					} } }
			return canequip;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMulPickaxe += 0.25f;
			sgaplayer.damagetaken += 0.20f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddIngredient(ItemID.CopperPickaxe, 1);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 15);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class EnhancingCharmlv1 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enhancing Charm Tier 1");
			Tooltip.SetDefault("buffs are 25% longer and debuffs are 25% shorter\n" + Idglib.ColorText(Color.Red, "Potion Sickness is not affected, and is also 10% longer"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0); ;
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.EnhancingCharm = 4;
			sgaplayer.potionsicknessincreaser = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddIngredient(ItemID.LesserHealingPotion, 5);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 15);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	public class AnticipationCharmlv1 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Charm Tier 1");
			Tooltip.SetDefault("When a boss fight starts, you are healed by 100 HP, but only every 2 minutes and while " + Idglib.ColorText(Color.Green, "Anticipation") + " is low" +
				"\nDuring a boss fight, you build up " + Idglib.ColorText(Color.Green, "Anticipation") + ", which causes your held weapon to do more damage, this caps at a 25% increase\n" +
	"You lose half your " + Idglib.ColorText(Color.Green, "Anticipation") + " when hurt, and passively drains while no boss is alive\n" + Idglib.ColorText(Color.Red, "There is a very rare chance to consume an extra item whenever items are consumed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0); ;
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.anticipationLevel = 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddIngredient(ItemID.CopperBroadsword, 1);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 15);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AnticipationCharmlv2 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Charm Tier 2");
			Tooltip.SetDefault("When a boss fight starts, you are healed by 200 HP, but only every 2 minutes and while " + Idglib.ColorText(Color.Green, "Anticipation") + " is low" +
				"\nDuring a boss fight, you build up " + Idglib.ColorText(Color.Green, "Anticipation") + ", which causes your held weapon to do more damage, this caps at a 50% increase\n" +
	"You lose half your " + Idglib.ColorText(Color.Green, "Anticipation") + " when hurt, and passively drains while no boss is alive\n" + Idglib.ColorText(Color.Red, "There is a rare chance to consume an extra item whenever items are consumed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.anticipationLevel = 2;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AnticipationCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AnticipationCharmlv3 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Charm Tier 3");
			Tooltip.SetDefault("When a boss fight starts, you are healed by 300 HP, but only every 2 minutes and while " + Idglib.ColorText(Color.Green, "Anticipation") + " is low" +
				"\nDuring a boss fight, you build up " + Idglib.ColorText(Color.Green, "Anticipation") + ", which causes your held weapon to do more damage, this caps at a near 100% increase\n" +
	"You lose half your " + Idglib.ColorText(Color.Green, "Anticipation") + " when hurt, and passively drains while no boss is alive\n" + Idglib.ColorText(Color.Red, "There is a semi-rare chance to consume an extra item whenever items are consumed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.anticipationLevel = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AnticipationCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 10);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class EnhancingCharmlv2 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enhancing Charm Tier 2");
			Tooltip.SetDefault("buffs are 50% longer and debuffs are 33% shorter\n" + Idglib.ColorText(Color.Red, "Potion Sickness is not affected, and is also 15% longer"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.EnhancingCharm = 3;
			sgaplayer.potionsicknessincreaser = 9;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EnhancingCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class EnhancingCharmlv3 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enhancing Charm Tier 3");
			Tooltip.SetDefault("buffs are 100% longer and debuffs are 50% shorter\n" + Idglib.ColorText(Color.Red, "Potion Sickness is not affected, and is also 20% longer"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.EnhancingCharm = 2;
			sgaplayer.potionsicknessincreaser = 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EnhancingCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 10);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class MiningCharmlv2 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Charm Tier 2");
			Tooltip.SetDefault("50% increased mining/hammering/chopping speed\n" + Idglib.ColorText(Color.Red, "Damage you take is increased by 30%"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMulPickaxe += 0.50f;
			sgaplayer.damagetaken += 0.30f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("MiningCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class MiningCharmlv3 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Charm Tier 3");
			Tooltip.SetDefault("100% increased mining/hammering/chopping speed\n" + Idglib.ColorText(Color.Red, "Damage you take is increased by 40%"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMulPickaxe += 1f;
			sgaplayer.damagetaken += 0.40f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("MiningCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 10);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

}