using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Kelvin : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Kelvin");
			Tooltip.SetDefault("'Flaming!'");
		}
        
		public override void SetDefaults()
        {
            Item refItem = new Item();
	    refItem.SetDefaults(ItemID.TheEyeOfCthulhu);                                 
            item.damage = 40;
            item.useTime = 22;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.channel = true;
            item.melee = true;
            item.knockBack = 2.5f;
            item.value = 10000;
            item.noMelee = true;
            item.rare = 6;
            item.noUseGraphic = true;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("KelvinProj");
            item.UseSound = SoundID.Item19;
        }
      
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
		    return false;
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