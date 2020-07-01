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
	public class DormantSupernova : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dormant Supernova");
			Tooltip.SetDefault("Mana charged into this modifed Novus staff can open a rift in the cosmos at its tip, from which countless novus bolts will appear\nHold left click to keep the portal open, increasing its rate of fire, but also consuming more mana");
			Item.staff[item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.width = 34;    
			item.mana = 10;
            item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 2;
			item.value = 100000;
			item.rare = 2;
	        item.shootSpeed = 8f;
            item.noMelee = true; 
			item.shoot = mod.ProjectileType("ProjectilePortalDSupernova");
            item.shootSpeed = 5f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedStaff"), 1);
			recipe.AddIngredient(mod.ItemType("RedManaStar"), 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 5);
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}



	}

}