using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons
{
	public class LanceMight : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lance a-lot");
			Tooltip.SetDefault("What's better than a lance? How about ALOT of them");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.damage = 32;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.useTime = 15;
			item.knockBack = 5f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 1;
			item.value = 100000;
			item.rare = 6;
			item.shoot = mod.ProjectileType("LanceMightProj");
			item.shootSpeed = 7f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Marble, 50);
			recipe.AddIngredient(null,"DankWood", 20);
			recipe.AddIngredient(ItemID.DarkLance, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
					for (int i = -120; i < 121; i += 40)
					{
				Vector2 speed = new Vector2(speedX, speedY);
				speed *= 1f - (float)(Math.Abs(i) / 200f);
				int thisoned = Projectile.NewProjectile(position.X, position.Y, speed.RotatedBy(MathHelper.ToRadians(i)).X, speed.RotatedBy(MathHelper.ToRadians(i)).Y, type, damage, knockBack, Main.myPlayer);
					}

			return false;
		}
	}
	public class LanceMightProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lance");
		}

		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 42;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.ownerHitCheck = true;
			projectile.aiStyle = 19;
			projectile.melee = true;
			projectile.timeLeft = 90;
			projectile.hide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override void AI()
		{
			Main.player[projectile.owner].direction = projectile.direction;
			//Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Main.player[projectile.owner].itemTime = Main.player[projectile.owner].itemAnimation;
			projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
			projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
			projectile.position += projectile.velocity * projectile.ai[0];
			if (Main.rand.Next(4) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.TornadoDust>(), 0f, 0f, 254, default(Color), 0.3f);
				Main.dust[dustIndex].velocity += projectile.velocity * 0.5f;
				Main.dust[dustIndex].velocity *= 0.5f;
				return;
			}
			if (projectile.ai[0] == 0f)
			{
				projectile.ai[0] = 3f;
				projectile.netUpdate = true;
			}
			if (Main.player[projectile.owner].itemAnimation < Main.player[projectile.owner].itemAnimationMax / 3)
			{
				projectile.ai[0] -= 2.4f;
				if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
				{
					projectile.localAI[0] = 1f;
					//Projectile.NewProjectile(projectile.Center.X + projectile.velocity.X, projectile.Center.Y + projectile.velocity.Y, projectile.velocity.X * 1f, projectile.velocity.Y * 1f, mod.ProjectileType("AcidSpear"), (int)((double)projectile.damage * 0.5), projectile.knockBack * 0.85f, projectile.owner, 0f, 0f);
				}
			}
			else
			{
				projectile.ai[0] += 0.95f;
			}

			if (Main.player[projectile.owner].itemAnimation == 0)
			{
				projectile.Kill();
			}

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 2.355f;
			if (projectile.spriteDirection == -1)
			{
				projectile.rotation -= 1.57f;
			}
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{

			bool facingleft = projectile.direction < 0f;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			if (facingleft)
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, ((projectile.rotation - (float)Math.PI / 2) - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, effect, 0);
			if (!facingleft)
				Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, (projectile.rotation - (float)Math.PI / 2) + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, SpriteEffects.None, 0);

			return false;
		}
	}
}