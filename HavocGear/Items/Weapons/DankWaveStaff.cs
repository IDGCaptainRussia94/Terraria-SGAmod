using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class DankWaveStaff : ModItem
	{
		public override void SetDefaults()
		{

			item.damage = 12;
			item.magic = true;
			item.mana = 8;
			item.width = 50;
			item.height = 52;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3;
			item.value = 10000;
			item.rare = 2;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
			item.shoot = mod.ProjectileType("SwampWave");
			item.shootSpeed = 10f;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wave Staff");
      Tooltip.SetDefault("Shoots a short piercing wave");
    }

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DankWood", 15);
			recipe.AddIngredient(null, "DankCore", 1);
			recipe.AddIngredient(null, "VialofAcid", 5);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
}
