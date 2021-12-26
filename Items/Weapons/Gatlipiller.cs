using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class Gatlipiller : ModItem
	{
		public int delay = 45;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gatlipiller");
			Tooltip.SetDefault("Fires a 5-round burst that gets faster the longer you hold down the fire button\nBullets poison targets\nThe shots do not travel far\n50% to not consume ammo");
		}

		public override void SetDefaults()
		{
			item.damage = 7;
			item.ranged = true;
			item.width = 42;
			item.height = 16;
			item.useTime = 4;
			item.useAnimation = 20;
			item.reuseDelay = delay;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = 50000;
			item.rare = 3;
			item.UseSound = SoundID.Item111;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 26f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override bool ConsumeAmmo(Player player)
		{
			return Main.rand.Next(100) < 50;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-16, 0);
		}
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(delay);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			delay=reader.ReadInt32();
		}

		public override void UpdateInventory(Player player)
		{
			if (player.itemAnimation < 1)
				delay = 45;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (delay>8 && !(player.itemAnimation < item.useAnimation - 2))
				delay -= 5;
			item.reuseDelay = delay;

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 48f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 100;
				Main.projectile[proj].knockBack = item.knockBack;
				IdgProjectile.AddOnHitBuff(proj, BuffID.Poisoned, 60 * 6);
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddIngredient(mod.ItemType("BiomassBar"), 10);
			recipe.AddIngredient(mod.ItemType("DankCore"), 2);
			recipe.AddIngredient(ItemID.Minishark, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

}
