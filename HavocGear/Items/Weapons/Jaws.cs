using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Jaws : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jaws");
            Tooltip.SetDefault("Throws a yoyo with spinning teeth that halve enemy defense\nthe teeth break on tiles or after they hit enemies enough times");
		}
        
		public override void SetDefaults()
        {
            Item refItem = new Item();
			refItem.SetDefaults(ItemID.Amarok);                                 
            item.damage = 45;
            item.useTime = 24;
            item.useAnimation = 22;
            item.useStyle = 5;
            item.channel = true;
            item.melee = true;
            item.noMelee = true;
            item.crit = 4;
            item.knockBack = 2.2f;
            item.value = 10000;
            item.rare = 5;
            item.noUseGraphic = true;
			item.autoReuse = true;
            item.UseSound = SoundID.Item19;
            item.shoot = mod.ProjectileType("JawsProj");
        }
      
        public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
		    return false;
	    }
    }
}