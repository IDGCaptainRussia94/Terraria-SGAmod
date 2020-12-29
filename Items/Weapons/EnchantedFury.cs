using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod.Items.Weapons
{
	public class EnchantedFury : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Fury"); // By default, capitalization in classnames will add spaces to the display name. You can customize the display name here by uncommenting this line.
			Tooltip.SetDefault("Primary fire shoots a sword that explodes erupts stars on kill\nAlt-fire rains stars down from the heavens, 1.1 Starfury style\n'The combination of magic!'");
		}

		public override void SetDefaults()
		{
			item.damage = 40;
			item.mana = 15;
			item.magic = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 20;
			item.useAnimation = 12;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = 150000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<EnchantedFuryProjectile>();
			item.shootSpeed = 16f;
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			/*if (target.life - damage < 1)
				EnchantedFuryProjectile.StarBurst(target.Center, damage, player.whoAmI);*/
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.Item28, position);
				if (sound != null)
				{
					sound.Pitch += 0.50f;
				}
				return true;
			}

			int numberProjectiles = 2 + Main.rand.Next(3);
			for (int index = 0; index < numberProjectiles; ++index)
			{
				Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
				vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
				vector2_1.Y -= (float)(100 * index);
				float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
				float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
				if ((double)num13 < 0.0) num13 *= -1f;
				if ((double)num13 < 20.0) num13 = 20f;
				float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
				float num15 = (item.shootSpeed * 1f) / num14;
				float num16 = num12 * num15;
				float num17 = num13 * num15;
				float SpeedX = num16 + (float)Main.rand.Next(-40, 41) * 0.05f;
				float SpeedY = num17 + (float)Main.rand.Next(-40, 41) * 0.05f;
				Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, ProjectileID.FallingStar, (int)(damage * 1f), knockBack, Main.myPlayer, 0.0f, (float)Main.rand.Next(5));
				SoundEffectInstance sound = Main.PlaySound(SoundID.Item9, new Vector2(vector2_1.X, vector2_1.Y));
				if (sound != null)
				{
					sound.Pitch += 0.75f;
				}
			}

			return false;
		}
		/*public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EnchantedSword);
			recipe.AddIngredient(ItemID.);
			recipe.AddIngredient(ItemID.HellstoneBar, 40);
			recipe.AddIngredient(ItemID.BrokenHeroSword);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}*/
	}

	public class EnchantedFuryProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enchanted Fury");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Starfury);
			aiType = ProjectileID.Starfury;
		}

		/*public override string Texture
		{
			get { return ("Terraria/Projectile_"+ProjectileID.Starfury); }
		}*/

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life - damage < 1)
				EnchantedFuryProjectile.StarBurst(target.Center, projectile.damage, projectile.owner);
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.Starfury;
			return true;
		}

		public override void PostAI()
		{
			projectile.localAI[1] += 1;
			projectile.position -= projectile.velocity * MathHelper.Clamp(1f - (projectile.localAI[1] / 30f), 0f, 1f);
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}

		public static void StarBurst(Vector2 where, int damage, int owner)
		{
			Main.PlaySound(SoundID.Item29, where);
			for (int i = 0; i < 5; i++)
			{
				Vector2 velorand = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				velorand.Normalize();
				int a = Projectile.NewProjectile(where.X, where.Y, velorand.X * Main.rand.NextFloat(0.5f, 3f), velorand.Y * Main.rand.NextFloat(0.5f, 3f), ProjectileID.Starfury, (int)(damage * 1f), 0, owner);
				Main.projectile[a].tileCollide = true;
				Main.projectile[a].timeLeft = 60 + Main.rand.Next(40);
				Main.projectile[a].scale = 0.5f;
				Main.projectile[a].Opacity = 0.25f;
				Main.projectile[a].netUpdate = true;
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return base.PreDraw(spriteBatch, lightColor);
		}
	}
}
