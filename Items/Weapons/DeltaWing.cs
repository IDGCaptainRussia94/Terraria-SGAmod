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
	public class DeltaWing : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Delta Wing");
			Tooltip.SetDefault("Converts arrows into Windfall arrows" +
				"\nWindfall arrows grant back a small amount of WingTime to the player on hit, and are nearly lighter than air");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.ranged = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 100000;
			item.rare = 4;
			item.autoReuse = true;
			item.UseSound = SoundID.Item5;
			item.shoot = ModContent.ProjectileType<Projectiles.WindfallArrow>();
			item.shootSpeed = 9f;
			item.useAmmo = AmmoID.Arrow;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBow"), 1);
			recipe.AddIngredient(ItemID.Feather, 10);
			recipe.AddIngredient(ItemID.SoulofFlight, 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();

			type = ModContent.ProjectileType<Projectiles.WindfallArrow>();
			return true;
		}

	}

}