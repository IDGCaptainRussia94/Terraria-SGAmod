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
using SGAmod.Projectiles;
using System.Linq;
using SGAmod.Effects;

namespace SGAmod.Items.Weapons
{
		public class CrystalComet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Comet");
			Tooltip.SetDefault("Manifests a purple Comet that pierces infinitely, releasing shards as it flies");
		}

        public override void SetDefaults()
		{
			item.damage = 120;
			item.magic = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 40;
			item.mana = 20;
			item.crit = 10;
			item.useAnimation = 40;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0.15f;
			item.value = 100000;
			item.rare = ItemRarityID.Yellow;
			item.autoReuse = true;
			item.UseSound = SoundID.Item105;
			item.shootSpeed = 8f;
			item.shoot = ModContent.ProjectileType<PrismicShowerProj>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalStorm, 1); 
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 15);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 0f), position.Y + (int)(speedY * 0f), speedX, speedY, type, damage, knockBack, player.whoAmI,ai0: Main.rand.Next(600));
			Main.projectile[probg].ranged = false;
			Main.projectile[probg].magic = true;
			Main.projectile[probg].melee = false;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			//IdgProjectile.AddOnHitBuff(probg, BuffID.CursedInferno, 60 * 20);
			IdgProjectile.Sync(probg);
			return false;
		}
	}

	public class PrismicShowerProj : ModProjectile
	{
		float strength => Math.Min(projectile.timeLeft / 120f, 1f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Storm");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;
			projectile.width = 4;
			projectile.height = 4;
			aiType = ProjectileID.Bullet;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.light = 0.1f;
			projectile.timeLeft = 300;
			projectile.magic = true;
			projectile.extraUpdates = 3;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

        public override bool CanDamage()
        {
			return true;
        }

		public override void AI()
		{
			projectile.ai[0] += 1;

			Vector2 offset = Vector2.Normalize(projectile.velocity).RotatedBy(MathHelper.Pi/2);
			float speed = Main.rand.NextFloat(0f, 8f) * (Main.rand.NextBool() ? 1f : -1f);

			if ((int)projectile.ai[0] % 3 == 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(projectile.Center + offset * Main.rand.NextFloat(-64, 64), offset * speed, ProjectileID.CrystalStorm, (int)(projectile.damage), projectile.knockBack, projectile.owner);
				proj.timeLeft = (int)(projectile.timeLeft / 5f);

			}
			Dust num126;
				num126 = Dust.NewDustPerfect(projectile.Center+(offset* Main.rand.NextFloat(-12, 12f)), 112, (offset* Main.rand.NextFloat(-9f, 3f)*(projectile.velocity.X>0 ? -1f : 1f)) + projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-8, 0)), 255-(int)(100f*strength), Color.White, 1f);
				num126.noGravity = true;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("noise"));
			trail.color = delegate (float percent)
			{
				return Color.Magenta;
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 13;
			trail.capsize = new Vector2(8f, 0f);
			trail.strength = strength;
			trail.trailThicknessIncrease = 15;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			return false;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedBolt";

	}

}
