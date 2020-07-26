using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Accessories
{
	public class TF2Emblem : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("TF2 Emblem");
            Tooltip.SetDefault("5% increased crit chance and damage, 25% increased throwing velocity");
        }

		public override bool CanEquipAccessory(Player player, int slot)
		{
			bool canequip = true;
			for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
			{
				if (player.armor[x].modItem != null)
				{
					int myType = (player.armor[x]).type;
					Type myclass = player.armor[x].modItem.GetType();
					if (myclass.BaseType == typeof(TF2Emblem) || myclass == typeof(TF2Emblem) || myclass.IsSubclassOf(typeof(TF2Emblem)))
					{

						canequip = false;
						break;
					}
				}
			}
			return canequip;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
		tooltips.Add(new TooltipLine(mod, "Tf2Elem", "You may wear only one TF2 Emblem at a time"));
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0);;
			item.rare = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//player.GetModPlayer<SGAPlayer>().Havoc = 1;
			player.Throwing().thrownCrit += 5;
			player.rangedCrit += 5;
			player.meleeCrit += 5;
			player.magicCrit += 5;
			player.magicDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.meleeDamage += 0.05f;
			player.minionDamage += 0.05f;
			player.Throwing().thrownDamage += 0.05f;
			player.Throwing().thrownVelocity += 0.25f;
			SGAmod.BoostModdedDamage(player, 0.05f, 5);
			player.SGAPly().tf2emblemLevel = 1;
		}
	}

	public class TF2EmblemRed : TF2Emblem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("RED Emblem");
			Tooltip.SetDefault("5% increased crit chance and damage, 25% increased throwing velocity\n50 Increased Max HP");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0); ;
			item.rare = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statLifeMax2 += 50;
			base.UpdateAccessory(player, hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TF2Emblem"), 1);
			recipe.AddIngredient(ItemID.GreaterHealingPotion, 30);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class TF2EmblemBlu : TF2Emblem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BLU Emblem");
			Tooltip.SetDefault("5% increased crit chance and damage, 25% increased throwing velocity\n100 Increased Max Mana and 5% mana cost reduction");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0); ;
			item.rare = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statManaMax2 += 100;
			player.manaCost -=0.05f;
			base.UpdateAccessory(player, hideVisual);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TF2Emblem"), 1);
			recipe.AddIngredient(ItemID.GreaterManaPotion, 30);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class TF2EmblemCommando : TF2Emblem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Commando Emblem");
			Tooltip.SetDefault("5% increased crit chance and damage, 25% increased throwing velocity\n50 increased Max HP\n100 Increased Max Mana and 5% mana cost reduction\n5% extra throwing damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0); ;
			item.rare = 7;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.statManaMax2 += 100;
			player.statLifeMax2 += 50;
			player.Throwing().thrownDamage += 0.05f;
			base.UpdateAccessory(player, hideVisual);
			player.SGAPly().tf2emblemLevel = 2;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TF2EmblemRed"), 1);
			recipe.AddIngredient(mod.ItemType("TF2EmblemBlu"), 1);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class TF2EmblemAssassin : TF2EmblemCommando
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assassin Emblem");
			Tooltip.SetDefault("10% increased crit chance, 15% increased damage, 35% increased throwing velocity\n75 increased Max HP\n100 Increased Max Mana and 7.5% mana cost reduction\n10% extra throwing damage");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(1, 0, 0, 0); ;
			item.rare = 9;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.Throwing().thrownCrit += 5;
			player.rangedCrit += 5;
			player.meleeCrit += 5;
			player.magicCrit += 5;
			player.magicDamage += 0.10f;
			player.rangedDamage += 0.10f;
			player.meleeDamage += 0.10f;
			player.minionDamage += 0.10f;
			SGAmod.BoostModdedDamage(player, 0.10f, 5);
			player.Throwing().thrownDamage += 0.15f;
			player.Throwing().thrownVelocity += 0.1f;
			player.manaCost -= 0.025f;
			player.statLifeMax2 += 25;
			base.UpdateAccessory(player, hideVisual);
			player.SGAPly().tf2emblemLevel = 3;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TF2EmblemCommando"), 1);
			recipe.AddIngredient(ItemID.DestroyerEmblem, 1);
			recipe.AddIngredient(ItemID.CrystalBall, 1);
			recipe.AddIngredient(ItemID.LifeCrystal, 5);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	public class TF2EmblemElite : TF2EmblemAssassin
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elite Emblem");
			Tooltip.SetDefault("10% increased crit chance, 20% increased damage, 50% increased throwing velocity\n100 increased Max HP\n120 Increased Max Mana and 10% mana cost reduction\n15% extra throwing damage\nEffects of MVM Upgrade");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(2, 0, 0, 0); ;
			item.rare = 12;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.magicDamage += 0.05f;
			player.rangedDamage += 0.05f;
			player.meleeDamage += 0.05f;
			player.minionDamage += 0.05f;
			player.Throwing().thrownDamage += 0.10f;
			player.Throwing().thrownVelocity += 0.15f;
			SGAmod.BoostModdedDamage(player, 0.05f, 0);
			player.statManaMax2 += 20;
			player.statLifeMax2 += 25;
			player.manaCost -= 0.025f;
			//mod.GetItem("LifeforceQuintessence").UpdateAccessory(player, hideVisual);
			base.UpdateAccessory(player, hideVisual);
			ModContent.GetInstance<MVMUpgrade>().UpdateAccessory(player, hideVisual);
			player.SGAPly().tf2emblemLevel = 4;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TF2EmblemAssassin"), 1);
			recipe.AddIngredient(mod.ItemType("MVMUpgrade"), 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddIngredient(ItemID.LifeFruit, 5);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 15);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 10);
			recipe.AddTile(TileID.TinkerersWorkbench);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

}