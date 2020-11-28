using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ThermalPike : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Pike");
			Tooltip.SetDefault("Pierces half of the enemy's defense");
		}

		public override void SetDefaults()
		{
			item.width = 36;
			item.damage = 24;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 16;
			item.useStyle = 5;
			item.useTime = 12;
			item.knockBack = 5.5f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.height = 44;
			item.maxStack = 1;
			item.value = 75000;
			item.rare = 6;
			item.shoot = mod.ProjectileType("ThermalPikeProj");
			item.shootSpeed = 10f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FieryShard", 12);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}