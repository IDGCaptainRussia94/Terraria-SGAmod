using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.HavocGear.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class CosmicGrasp : ModItem, IHitScanItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Grasp");
			Tooltip.SetDefault("'With more harmonious convergence, its power is greatly increased'\n" +
				"Launches a piercing spear of cosmic light that creates Quasar Beam Explosions on hit" +
				"\nShadowflame tendrils explode from enemies killed with the spear of light and on crit"+
				"\nExplosion doesn't crit, damage falls off over distance\nFirst hit does 3X damage, No immunity frames are caused by this weapon");
		}
		public override void SetDefaults()
		{
			item.damage = 100;
			item.noMelee = true;
			item.magic = true;
			item.mana = 20;
			item.crit = 25;
			item.width = 22;
			item.height = 22;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 10;
			item.value = 2000000;
			item.rare = 12;
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("CGraspSpear");
			item.shootSpeed = 10;
			Item.staff[item.type] = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CosmicGrasp_Glow");
			}
		}
		public override bool Shoot (Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = mod.ProjectileType("CGraspSpear");
			float numberProjectiles = 1; // 3, 4, or 5 shots
			float rotation = MathHelper.ToRadians(Main.rand.Next(33));
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, (int)damage*3, knockBack, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 4);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Cosmillash"), 1);
			recipe.AddIngredient(ItemID.ShadowbeamStaff, 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("EldritchTentacle"), 15);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 12);
			recipe.AddIngredient(ItemID.Amethyst, 1);
			recipe.AddTile(TileID.LunarCraftingStation);			
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class QuasarOrbLessParticles : QuasarOrb
	{
		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}
	}

		public class CGraspSpear : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 6;
			projectile.magic = true;
			projectile.timeLeft = 300;
			projectile.light = 0.1f;
			projectile.extraUpdates = 300;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Grasp");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			explode(projectile.Center, projectile.damage, projectile.knockBack, false);
			projectile.Kill();
			return false;
		}

		public void explode(Vector2 target, int damage, float knockback, bool crit)
		{
			for (int i = 0; i < 1; i++)
			{
				int proj = Projectile.NewProjectile(target.X, target.Y, Vector2.Zero.X, Vector2.Zero.Y, mod.ProjectileType("QuasarOrbLessParticles"), projectile.damage, projectile.knockBack, projectile.owner);
				Main.projectile[proj].timeLeft = 2;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);
				Main.projectile[proj].Kill();
			}

		}

		public static void BeamBurst(Vector2 where, int damage, int owner,int count)
		{
			float[] speeds = { 1.5f, 5f };
			if (count > 5)
				speeds = new float[] {5f,9f };
			for (int i = 0; i < count; i++)
			{
				Vector2 velorand = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000));
				velorand.Normalize();
				int a = Projectile.NewProjectile(where.X, where.Y, velorand.X * Main.rand.NextFloat(speeds[0], speeds[1]), velorand.Y * Main.rand.NextFloat(speeds[0], speeds[1]), ProjectileID.ShadowFlame, (int)(damage * 1.25f), 0, owner);
				Main.projectile[a].tileCollide = true;
				Main.projectile[a].timeLeft = (int)((120 + Main.rand.Next(80)));
				Main.projectile[a].netUpdate = true;
				Main.projectile[a].usesLocalNPCImmunity = true;
				Main.projectile[a].localNPCHitCooldown = -1;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			explode(target.Center, projectile.damage, knockback, crit);

			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;
				projectile.damage /= 3;

			}

			if (crit)
				CGraspSpear.BeamBurst(target.Center, projectile.damage, projectile.owner, 1);
			if (target.life - damage < 1)
				CGraspSpear.BeamBurst(target.Center, projectile.damage, projectile.owner, 8);
		}

		public override void AI()
		{
			int num126 = Dust.NewDust(projectile.Center, 0, 0, 173, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 3.0f);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = projectile.velocity * 0.5f;
		}
	}

}
