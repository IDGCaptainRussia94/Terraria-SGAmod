using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	class Plythe : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plythe");
			Tooltip.SetDefault("Rapidly throw out seeking Scythes that fly back to the player on hit, can hit up to 3 times\nStacks up to 5 allowing several Plythe blades out at once\nThey reap that which they sow");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Plythe"); }
		}
		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown=true;
			item.damage = 110;
			item.crit = 20;
			item.shootSpeed = 45f;
			item.shoot = ModContent.ProjectileType<Scythe>();
			item.useTurn = true;
			item.width = 54;
			item.height = 32;
			item.maxStack = 5;
			item.knockBack = 0;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 8;
			item.useTime = 8;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(1, 0, 0, 0);
			item.rare = ItemRarityID.Cyan;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < item.stack;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//speedX *= player.thrownVelocity;
			//speedY *= player.thrownVelocity;
			return true;
		}

		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LightDisc, 1);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 6);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 4);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 2);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this,1);
            recipe.AddRecipe();
		}
	}

	public class Scythe : ModProjectile
	{

		float hittime = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scythe");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 64;
			projectile.height = 64;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 120;
			projectile.penetrate = 12;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public Scythe()
		{
			projectile.localAI[0] = 0.5f;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.penetrate < 10)
				return false;
			else
				return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.velocity *= -1f;
			target.immune[projectile.owner] = 5;
			hittime = 150f;
		}

		public override void AI()
		{

			Lighting.AddLight(projectile.Center, Color.Aquamarine.ToVector3() * 0.5f);

			hittime = Math.Max(1f, hittime / 1.5f);
			;
			float dist2 = 54f;

			//Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			for (float num315 = 0; num315 < MathHelper.Pi + 0.04; num315 = num315 + MathHelper.Pi)
			{
				float angle = (projectile.rotation + MathHelper.Pi / 5f) + num315;
				Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist2), (float)(Math.Sin(angle) * dist2));
				Vector2 offset = (thisloc * projectile.localAI[0])+projectile.velocity;
				int num316 = Dust.NewDust(new Vector2(projectile.Center.X - 1, projectile.Center.Y) + offset, 0, 0, mod.DustType("NovusSparkleBlue"), 0f, 0f, 50, Color.White, 1.5f);
				Main.dust[num316].noGravity = true;
				Main.dust[num316].velocity = thisloc / 30f;
			}

			projectile.ai[0] = projectile.ai[0] + 1;
			projectile.velocity.Y += 0.1f;
			if (projectile.ai[0] > 14f && !Main.player[projectile.owner].dead)
			{
				Vector2 dist = (Main.player[projectile.owner].Center - projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();
				projectile.velocity += distnorm * 5f;
				projectile.velocity /= 1.05f;
				//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));
				if (dist.Length() < 80)
					projectile.Kill();
			}

			NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
			if (target != null && projectile.penetrate > 9)
			{
				if ((target.Center - projectile.Center).Length() < 500f)
				{

					projectile.Center += (projectile.DirectionTo(target.Center) * (projectile.ai[0] > 14f ? (50f * Main.player[projectile.owner].thrownVelocity) / hittime : 12f));

				}
			}

			projectile.localAI[0] += ((hittime > 10 ? 3.0f : 0.25f) - projectile.localAI[0])/ 10f;
			projectile.localAI[0] = MathHelper.Clamp(projectile.localAI[0], 0.5f, 1f);
			projectile.rotation += 0.38f + (hittime/50f);
		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/ScytheGlow");
			Texture2D texture2 = mod.GetTexture("Items/Weapons/Plythe");
			float scale = projectile.localAI[0];

			spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), scale, SpriteEffects.None, 0f);

			Texture2D tex = Main.projectileTexture[projectile.type];

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.Aqua* scale, projectile.rotation + MathHelper.Pi / 4f, tex.Size() / 2f, new Vector2(0.5f, 1.75f*scale), default, 0);

			return false;
		}


	}

}
