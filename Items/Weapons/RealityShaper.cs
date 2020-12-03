using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class RealityShaper : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reality Shaper");
			Tooltip.SetDefault("The elements are yours to shape\nRequires a small amount of mana to swing, Double tab to launch 2 heat waves in succession\nFunctions as both a sword and a staff\nHitting with the blade opens several rifts that launch Sky Fracture Blades into the target\nAfter the swing animation hold left mouse to open a rift\nThis will fire fast moving cirno bolts\nFurthermore, portals appear behind you that summon Hot Rounds!\nThe damage of these are less than the melee damage but are improved by your magic damage multiplier\nIf done after a double tap, you'll summon 2 portals to shoot twice as many projectiles, at double the mana costs");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 340;
			item.crit = 15;
			item.melee = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 10;
			item.useAnimation = 11;
			item.useStyle = 5;
			item.knockBack = 15;
			item.value = 1500000;
			item.shootSpeed = 28f;
			item.shoot = mod.ProjectileType("ProjectilePortalRealityShaper");
			item.rare = 11;
			item.UseSound = SoundID.Item71;
			item.autoReuse = false;
			item.useTurn = false;
			item.channel = true;
			item.mana = 20;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/RealityShaper_Glow");
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana<20 || player.ownedProjectileCounts[mod.ProjectileType("ProjectilePortalRealityShaper")]>1)
			return false;
			else
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			item.noMelee = false;
			item.useStyle = 1;

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(8);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			Main.PlaySound(SoundID.Item, player.Center, 45);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("HeatWave"), (int)((float)damage * 0.15f), knockBack / 3f, player.whoAmI);
				Main.projectile[proj].melee = true;
				Main.projectile[proj].magic = false;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);

				//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
			}
			//SGAPlayer.LimitProjectiles(player, 0, new ushort[] {(ushort)mod.ProjectileType("ProjectilePortalRealityShaper") });
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{

			for (int i = 0; i < 360; i += 36)
			{
				float angle = MathHelper.ToRadians(i);
				Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 256;
				hereas += target.Center;
				Vector2 gohere = (target.Center - hereas); gohere.Normalize(); gohere *= 16f;
				int proj = Projectile.NewProjectile(hereas, gohere, mod.ProjectileType("ProjectilePortalRealityShaperFracturePortal"), (int)(damage*0.2f), knockBack, player.whoAmI, ProjectileID.SkyFracture);
				Main.projectile[proj].magic = false;
				Main.projectile[proj].melee = true;
				Main.projectile[proj].timeLeft = 40;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);

			}
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			/*for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}*/

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
			recipe.AddIngredient(mod.ItemType("BigBang"), 1);
			recipe.AddIngredient(mod.ItemType("HeatWave"), 1);
			recipe.AddIngredient(ItemID.SkyFracture, 1);
			recipe.AddIngredient(ItemID.ChristmasTreeSword, 1);
			recipe.AddIngredient(ItemID.InfluxWaver, 1);
			recipe.AddIngredient(mod.ItemType("CircuitBreakerBlade"), 1);
			recipe.AddIngredient(ItemID.DD2SquireBetsySword, 1);
			recipe.AddIngredient(mod.ItemType("OmegaSigil"), 1);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 8);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 10);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 100);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}


	}

	public class ProjectilePortalRealityShaperFracturePortal : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 16;

	}

	public class ProjectilePortalRealityShaperHit : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 16;
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
			projectile.timeLeft = 40;
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

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(6)) * projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)projectile.ai[0], projectile.damage, projectile.knockBack, owner.whoAmI);
					Main.projectile[proj].melee = true;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}


	public class ProjectilePortalRealityShaper : ProjectilePortalDSupernova
	{
		public override int openclosetime => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
		}

		public override int projectilerate => 25;
		public override int manacost => 6;
		public override int portalprojectile => mod.ProjectileType("CirnoBoltPlayer");
		public override int takeeffectdelay =>  Main.player[projectile.owner].HeldItem.useTime;
		public override float damagescale => 0.50f * Main.player[projectile.owner].magicDamage;
		public override int penetrate => 1;
		public override int startrate => 60;
		public override int drainrate => 5;
		public override int timeleftfirerate => 20;

		public int everyother = 0;

		public override void Explode()
		{

			if (projectile.timeLeft == timeleftfirerate && projectile.ai[0] > 0)
			{
				Player owner = Main.player[projectile.owner];

					if (owner != null && !owner.dead && owner.channel)
				{
					everyother += 1;
					everyother %= 2;

					Vector2 gotohere = new Vector2();
					gotohere = projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(15)) * projectile.velocity.Length();
					if (everyother == 1)
					{
						int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y)*2f, (int)projectile.ai[0], (int)(projectile.damage*1f* damagescale), projectile.knockBack / 10f, owner.whoAmI);
						Main.projectile[proj].magic = true;
						Main.projectile[proj].timeLeft = 300;
						Main.projectile[proj].penetrate = penetrate;
						IdgProjectile.Sync(proj);
					}


					Vector2 backthere = (owner.Center- gotohere*32)-(new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(135))*96);
					Vector2 gohere = Main.MouseWorld - backthere;
					gohere.Normalize();
					gohere *= 28f;
					gohere = gohere.RotatedByRandom(MathHelper.ToRadians(20));

					int proj2 = Projectile.NewProjectile(backthere, gohere, mod.ProjectileType("ProjectilePortalRealityShaperHit"), (int)(projectile.damage*1.25 * damagescale), projectile.knockBack / 6f, owner.whoAmI, mod.ProjectileType("HotRound"));
					IdgProjectile.Sync(proj2);

				}

			}

		}

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