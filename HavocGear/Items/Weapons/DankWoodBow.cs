using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class DankWoodBow : ModItem
	{
		public override void SetDefaults()
		{

			item.damage = 8;
			item.ranged = true;
			item.width = 18;
			item.height = 32;
			item.useTime = 45;
			item.useAnimation = 45;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 1;
			item.value = 3000;
			item.rare = 1;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 10f;
			item.useAmmo = AmmoID.Arrow;
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wood Bow");
      Tooltip.SetDefault("Wooden Arrows shot from this bow become Dank Arrows");
    }
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (type == ProjectileID.WoodenArrowFriendly)
				type = mod.ProjectileType("DankArrow");
			return true;
		}


		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DankWood", 25);
            recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
