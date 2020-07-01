using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace SGAmod.Items.Weapons
{
	public class Sunbringer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sun Bringer");
			//Tooltip.SetDefault("Gotta start somewhere you know");
		}
		public override void SetDefaults()
		{
            item.CloneDefaults(164);
			item.damage = 12;
			item.useTime = 62;
			item.useAnimation = 62;
			item.knockBack = 6;
			item.value = 75000;
			item.rare = 5;
			item.shoot = mod.ProjectileType("SunbringerFlare");
			item.useAmmo = AmmoID.Flare;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Terraria.Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("SunbringerFlare"), damage, knockBack, player.whoAmI);
			return false;
		} 
	}
}
