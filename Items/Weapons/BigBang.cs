using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class BigBang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Big Bang");
			Tooltip.SetDefault("Honed by the elements, requires a small amount of mana to swing\nFunctions as both a sword and a staff\nHitting with the blade opens rifts that launch Enchanted Swords\nAfter the swing animation hold left mouse to open a rift to fire Cirno bolts!\nThis is 50% of the weapon's base damage multiplied by your magic damage multiplier");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.melee = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 25;
			item.crit = 15;
			item.useAnimation = 26;
			item.useStyle = 5;
			item.autoReuse = true;
			item.knockBack = 15;
			item.value = 500000;
			item.shootSpeed = 8f;
			item.shoot = mod.ProjectileType("ProjectilePortalBigBang");
			item.rare = 5;
			item.UseSound = SoundID.Item71;
			item.autoReuse = false;
			item.useTurn = false;
			item.channel = true;
			item.mana = 10;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/BigBang_Glow");
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana<20 || player.ownedProjectileCounts[mod.ProjectileType("ProjectilePortalBigBang")]>0)
			return false;
			else
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			item.noMelee = false;
			item.useStyle = 1;
			if (player.ownedProjectileCounts[mod.ProjectileType("ProjectilePortalBigBang")] > 0)
				return false;
			return true;
			//return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			Vector2 hereas = new Vector2(Main.rand.Next(-1000, 1000), Main.rand.Next(-1000, 1000)); hereas.Normalize();
			hereas *= Main.rand.NextFloat(100f, 200f);
			hereas += target.Center;
			 Vector2 gohere=(target.Center-hereas); gohere.Normalize(); gohere *= 10f;
			int proj = Projectile.NewProjectile(hereas, gohere, mod.ProjectileType("ProjectilePortalBBHit"), damage, knockBack, player.whoAmI,ProjectileID.EnchantedBeam);
			Main.projectile[proj].penetrate = 2;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("TornadoDust"));// 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}

			for (int num475 = 3; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 15f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 3f) + ((player.direction) * player.itemRotation.ToRotationVector2() * (float)num475);
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("DormantSupernova"), 1);
			recipe.AddIngredient(mod.ItemType("ForagersBlade"), 1);
			recipe.AddIngredient(mod.ItemType("IceScepter"), 1);
			recipe.AddIngredient(mod.ItemType("RubiedBlade"), 1);
			recipe.AddIngredient(mod.ItemType("MangroveStriker"), 1);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 12);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 10);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 16);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 6);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("DormantSupernova"), 1);
			recipe.AddIngredient(mod.ItemType("RustworkBlade"), 1);
			recipe.AddIngredient(mod.ItemType("IceScepter"), 1);
			recipe.AddIngredient(mod.ItemType("RubiedBlade"), 1);
			recipe.AddIngredient(mod.ItemType("MangroveStriker"), 1);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 12);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 10);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 16);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 6);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}


	}


	public class ProjectilePortalBBHit : ProjectilePortal
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawner");
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			//projectile.aiStyle = 1;
			projectile.friendly = true;
			//projectile.magic = true;
			//projectile.penetrate = 1;
			projectile.timeLeft = 100;
			projectile.tileCollide = false;
			aiType = -1;
		}

		public override void Explode()
		{

			if (projectile.timeLeft == 30 && projectile.ai[0] > 0)
			{
				Player owner = Main.player[projectile.owner];
				if (owner != null && !owner.dead)
				{

					Vector2 gotohere = new Vector2();
					gotohere = projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(50)) * projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)projectile.ai[0], projectile.damage, projectile.knockBack, owner.whoAmI);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}


	public class ProjectilePortalBigBang : ProjectilePortalDSupernova
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
		}

		public override int projectilerate => 40;
		public override int manacost => 15;
		public override int portalprojectile => mod.ProjectileType("CirnoBoltPlayer");
		public override int takeeffectdelay =>  Main.player[projectile.owner].HeldItem.useTime;
		public override float damagescale => 0.50f * Main.player[projectile.owner].magicDamage;
		public override int penetrate => 1;

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.tileCollide = false;
			projectile.timeLeft = 38;
		}

	}

}
