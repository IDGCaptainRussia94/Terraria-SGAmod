using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SGAmod.HavocGear.Items.Tools;

using SGAmod.Dusts;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class MangroveBow : MangrovePickaxe, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Bow");
			Tooltip.SetDefault("Shoots 2 arrows offsetted at angles at the cost of 1");
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.ranged = true;
			item.width = 20;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 25000;
			item.rare = 5;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.useAmmo = AmmoID.Arrow;
		}

        	public override void AddRecipes()
        	{
            		ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DankWoodBow", 1);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
            		recipe.AddRecipe();
        	}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 2 + Main.rand.Next(1);
			float rotation = MathHelper.ToRadians(10);
			speedX *= player.ArrowSpeed(); speedY *= player.ArrowSpeed();

			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}

	public class MangroveStriker : MangroveBow, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Striker");
			Tooltip.SetDefault("Attacks may inflict Dryad's Bane, or bless yourself");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.melee = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 19;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 7;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(0, 100) < 25)
			target.AddBuff(BuffID.DryadsWardDebuff, 60 * 4);
			if (Main.rand.Next(0, 100) < 25)
				player.AddBuff(BuffID.DryadsWard, 60 * 5);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DankWoodSword", 1);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class MangroveStaff : MangroveBow, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Staff");
			Tooltip.SetDefault("Shoots 3 fast moving homing mangrove orbs");
		}

		public override void SetDefaults()
		{
			item.damage = 12;
			item.magic = true;
			item.mana = 5;
			item.width = 38;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 30;
			Item.staff[item.type] = true;
			item.useStyle = 5;
			item.knockBack = 0.5f;
			item.value = 25000;
			item.rare = 3;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("MangroveStaffOrb");
			item.shootSpeed = 12f;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}

	}

	public class MangroveShiv : MangroveStaff, IMangroveSet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Throwing Shiv");
			Tooltip.SetDefault("Shivs pierce and chain between enemies on hit\nEnemies hit who have Dryad's Bane allow an additional chain");
		}

		public override void SetDefaults()
		{
			item.damage = 16;
			item.Throwing().thrown = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 1;
			item.knockBack = 1;
			item.value = 25;
			item.consumable = true;
			item.maxStack = 999;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("MangroveShivProj");
			item.shootSpeed = 15f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;

			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) * 0.04f);
			Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBar", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this,50);
			recipe.AddRecipe();
		}

	}

	class MangroveShivProj : ModProjectile
	{

		bool hitonce = false;
		List<Point> listOfPoints = new List<Point>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Mangrove Shiv");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Grenade);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 600;
			projectile.aiStyle = -1;
			projectile.width = 24;
			projectile.height = 24;
			projectile.penetrate = 3;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Items/Weapons/MangroveShiv"); }
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			listOfPoints.Add(new Point(target.whoAmI,1000000));
			List<NPC> closestnpcs = SGAUtils.ClosestEnemies(projectile.Center, 200,AddedWeight: listOfPoints,checkCanChase: false);

			if (target.HasBuff(BuffID.DryadsWardDebuff))
				projectile.penetrate += 1;

			if (closestnpcs != null && closestnpcs.Count>0)
            {
				NPC target2 = closestnpcs[0];
				Vector2 dist = target2.Center - projectile.Center;

				while (dist.Length() > 24f)
                {
					projectile.Center += Vector2.Normalize(dist)*6f;
					dist = target2.Center - projectile.Center;


					int dust = Dust.NewDust(new Vector2(projectile.Center.X - 8, projectile.Center.Y - 8), 16, 16, ModContent.DustType<MangroveDust>());
					Main.dust[dust].scale = 1.75f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = Vector2.Zero;
				}
				projectile.velocity = Vector2.Normalize(dist) * 6f;
				projectile.rotation = projectile.velocity.ToRotation()+(MathHelper.Pi/4f);

			}
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			return true;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 3) == 0)
			{
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 12, projectile.Center.Y - 12), 24, 24, 96);
				Main.dust[dust].scale = 0.50f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = -projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			projectile.timeLeft -= 1;
			projectile.velocity.Y += 0.25f;
			projectile.velocity.X *= 0.98f;
			projectile.rotation += projectile.velocity.Length() * (float)(Math.Sign(projectile.velocity.X * 1000f) / 1000f) * 10f;
		}

	}

}
