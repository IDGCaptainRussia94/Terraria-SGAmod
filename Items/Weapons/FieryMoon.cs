using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class FieryMoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Moon");
			Tooltip.SetDefault("Launches molten rock that can pierce several times before exploding into a thermal nova\nHold left click to keep the ball attached to the end of the staff");
			Item.staff[item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			item.damage = 70;
			item.magic = true;
			item.width = 34;    
			item.mana = 20;
            item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 1000000;
			item.rare = 5;
	        item.shootSpeed = 8f;
            item.noMelee = true; 
			item.shoot = mod.ProjectileType("FieryRock");
            item.shootSpeed = 7f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "FieryShard", 10);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

}