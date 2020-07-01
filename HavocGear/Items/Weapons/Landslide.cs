using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Landslide : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 30;
			item.magic = true;
			item.mana = 20;
			item.width = 28;
			item.height = 30;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 4;
			item.noMelee = true;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 1;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
			item.shoot = 1;
            item.shootSpeed = 10f;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Landslide");
            Tooltip.SetDefault("");
        }


        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            for (int i = 0; i < Main.rand.Next(6, 13); i++)
            {
                string[] projectileArray = { "Landslide1", "Landslide2", "Landslide3" };
                Projectile.NewProjectile(Main.MouseWorld.X + (Main.rand.Next(-20, 21)), player.position.Y - 600, Main.rand.Next(-2, 3), Main.rand.Next(12, 16), mod.ProjectileType(projectileArray[Main.rand.Next(projectileArray.Length)]), item.damage, 0f, Main.myPlayer, 0f, 0f);
            }
            return false;
        }
	}
}
