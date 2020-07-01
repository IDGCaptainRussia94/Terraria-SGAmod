using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class SnappyShark : ModItem
	{
	public override void SetStaticDefaults()
		{	
			DisplayName.SetDefault("Snappy Shark");
			Tooltip.SetDefault("Shoots gouging teeth which halves enemy defense\n30% chance to not consume teeth");
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (Main.rand.Next(0, 100) < 30)
				return false;
			return base.ConsumeAmmo(player);
		}

		public override void SetDefaults()
		{
	item.CloneDefaults(ItemID.Megashark);
			item.damage = 35;
			item.noMelee = true;
			item.ranged = true;
			item.width = 52;
			item.height = 28;
			item.useTime = 9;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 5;
			item.UseSound = SoundID.Item41;
			item.autoReuse = true;
            item.useAmmo = mod.ItemType("SharkTooth");
			item.shoot = mod.ProjectileType("SnappyTooth");
		}		

		public override bool Shoot (Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
	        Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;
			return true;
		}		
		
		public override Vector2? HoldoutOffset()
		{
	        return new Vector2(0, 3);
		}
	}
}
