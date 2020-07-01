using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class HeatWave : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heat Wave");
			Tooltip.SetDefault("Summons heated crystals that lodge into foes, setting them ablaze" +
				"\nAfter a while the crystal explodes, inflicting Thermal Blaze on the enemy they were attached to" +
	"\nOnly one crystal may be lodged into a foe at a time");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.magic = true;
			item.mana = 8;
			item.width = 34;      
            item.height = 24;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 6;
	        item.shootSpeed = 14f;
            item.noMelee = true; 
			item.shoot = mod.ProjectileType("HotRound");
			item.UseSound = SoundID.Item9;		
			item.autoReuse = true;
		    item.useTurn = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position += new Vector2(speedX, speedY) * 1;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
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