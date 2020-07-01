using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class MossYoyo : MangroveBow
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quagmire");
            Tooltip.SetDefault("Hits apply Dank Slow against your foes\nContinous hits make the slow stronger\nEnemies who are immune to Poison are also immune to Dank Slow");
        }
        
		public override void SetDefaults()
        {
            Item refItem = new Item();
			refItem.SetDefaults(ItemID.Amarok);                                 
            item.damage = 25;
            item.useTime = 24;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.channel = true;
            item.noMelee = true;
            item.melee = true;
			item.crit = 4;
            item.knockBack = 4.5f;
            item.value = 47000*5;
            item.rare = 3;
            item.noUseGraphic = true;
            item.UseSound = SoundID.Item1;
			item.autoReuse = true;
            item.shoot = mod.ProjectileType("MossYoyoProj");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.WoodYoyo);
            recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
            recipe.AddRecipe();
        }
      
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
		    return false;
	    }
    }
}