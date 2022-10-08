using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;
using System;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumables;
using Terraria.DataStructures;

namespace SGAmod.Items.Placeable.TechPlaceable
{
	public class NumismaticCrucibleItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Numismatic Crucible");
			Tooltip.SetDefault("Simulates npc deaths and attempts to output their drops below\nPlace a filled Soul Jar into the machine to designate that type of enemy\nRequires Raw money inputed via coins to function\nMoney cost is relative to the money ammount dropped by the designated enemy\nProcess rate and max capacity increases in hardmode, post mech, post Plantera, and post Moonlord\n'basically a mob farm block'");
		}

        public override string Texture => "SGAmod/Tiles/TechTiles/NumismaticCrucibleTile";

        public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Cyan;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.TechTiles.NumismaticCrucibleTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 12);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 12);
			recipe.AddIngredient(ModContent.ItemType<EnergizerBattery>(), 2);
			recipe.AddIngredient(ModContent.ItemType<Weapons.Technical.LaserMarker>(), 10);
			recipe.AddIngredient(ItemID.CookingPot, 1);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}