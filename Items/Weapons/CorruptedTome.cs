using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class CorruptedTome : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupted Tome");
			Tooltip.SetDefault("Spews a stream of Cursed Flames\nInflicts Cursed Inferno for far longer than usual and inflicts Everlasting Suffering\nEverlasting Suffering increases damage over time by 250% and makes other debuffs last until it ends");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 5;
			item.mana = 8;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0.20f;
			item.value = 100000;
			item.rare = 8;
			item.autoReuse = true;
			item.UseSound = SoundID.Item34;
			item.shootSpeed = 8f;
			item.shoot = ProjectileID.EyeFire;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CursedFlames, 1);
			recipe.AddIngredient(ItemID.Flamethrower, 1);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
			recipe.AddIngredient(ItemID.SoulofSight, 10);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 0f), position.Y + (int)(speedY * 0f), speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].ranged = false;
			Main.projectile[probg].magic = true;
			Main.projectile[probg].melee = false;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			IdgProjectile.AddOnHitBuff(probg, BuffID.CursedInferno, 60 * 20);
			IdgProjectile.AddOnHitBuff(probg, mod.BuffType("EverlastingSuffering"), 60 * 7);
			IdgProjectile.Sync(probg);
			return false;
		}
	}

}
