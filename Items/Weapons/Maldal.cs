using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using SGAmod.Projectiles;
using SGAmod.NPCs.Hellion;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod.Items.Weapons
{
	public class Maldal : ElementalCascade
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Maldal");
			Tooltip.SetDefault("Floods your screen with files that explode when they touch an enemy!\nNot useable while your files are on the system");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 200;
			item.crit = 15;
			item.magic = true;
			item.mana = 250;
			item.width = 40;
			item.height = 40;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 4;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 1000000;
			item.rare = 10;
			item.UseSound = SoundID.Item78;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("MagoldFiles");
			item.shootSpeed = 8f;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor = Main.hslToRgb((Main.GlobalTime / 6f) % 1f, 0.85f, 0.45f);
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[mod.ProjectileType("MagoldFiles")] < 1;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/LostNotes"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 100);
			//recipe.AddIngredient(mod.ItemType("LostNotes"), 5);
			recipe.AddRecipeGroup("Fragment", 15);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.Worm, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int a = -100; a < 101; a += 10)
			{
				for (int i = -100; i < 101; i += 10)
				{
					int probg = Projectile.NewProjectile(player.Center.X + a * 10f, player.Center.Y + i * 10f, 0, 0, type, damage, knockBack, player.whoAmI);
					Main.projectile[probg].friendly = true;
					Main.projectile[probg].hostile = false;
					Main.projectile[probg].netUpdate = true;
					IdgProjectile.Sync(probg);

				}
			}
			return false;
		}

		public class MagoldFiles : ModProjectile
		{
			private Vector2[] oldPos = new Vector2[6];
			private float[] oldRot = new float[6];
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("RaVe");
			}

			public override string Texture
			{
				get { return ("SGAmod/Items/Accessories/LostNotes"); }
			}

			public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
			{
				Vector2 size = Main.fontDeathText.MeasureString("RaVe" + projectile.whoAmI);
				spriteBatch.DrawString(Main.fontMouseText, "RaVe" + projectile.whoAmI, (projectile.Center + new Vector2(-size.X / 6f, 24)) - Main.screenPosition, Color.White);
				return base.PreDraw(spriteBatch, Color.White);
			}

			public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
			{
				for (int i = 0; i < 359; i += 359)
				{
					double angles = MathHelper.ToRadians(i);
					float randomx = 48f;//Main.rand.NextFloat(54f, 96f);
					Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

					SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y,61);
					if (sound != null)
						sound.Pitch = 0.925f;

					int thisone = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0,0, ModContent.ProjectileType<MaldalBlast>(), projectile.damage, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
					IdgProjectile.Sync(thisone);


				}
			}

			public override void SetDefaults()
			{
				//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
				projectile.width = 16;
				projectile.height = 20;
				projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
				projectile.hostile = false;
				projectile.friendly = true;
				projectile.tileCollide = true;
				projectile.magic = true;
				projectile.timeLeft = 450;
				aiType = ProjectileID.Bullet;
			}

		}

	}

	public class MaldalBlast : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Viral Blast");
		}

		public override void SetDefaults()
		{
			projectile.width = 96;
			projectile.height = 96;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

        public override bool CanDamage()
        {
            return projectile.ai[0]<30;
        }

        public override void AI()
		{
			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.01f) / 255f, ((255 - projectile.alpha) * 0.025f) / 255f, ((255 - projectile.alpha) * 0.25f) / 255f);
			projectile.ai[0] += 1;

			float size = projectile.ai[0] < 2 ? 0f : 1f;

			for (int i = 0; i < 1 + (projectile.ai[0] < 2 ? 32 : 0); i++)
			{
				Vector2 randomcircle = Main.rand.NextVector2CircularEdge(1f, 1f);
				int num655 = Dust.NewDust(projectile.Center + Main.rand.NextVector2Circular(projectile.width, projectile.width)* size, 0, 0, ModContent.DustType<Dusts.ViralDust>(), projectile.velocity.X + randomcircle.X * (4f), projectile.velocity.Y + randomcircle.Y * (4f), 150, Main.hslToRgb(Main.rand.NextFloat(), 1f, (projectile.timeLeft / 60f)), 0.5f);
				Main.dust[num655].noGravity = true;
			}
		}
	}


}