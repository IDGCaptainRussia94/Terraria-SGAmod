using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;

namespace SGAmod.Items.Weapons
{

	public class SoldierRocketLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soldier's Rocket Launcher");
			Tooltip.SetDefault("Becomes stronger with higher tier TF2 emblems you equip\nBlasts from the rockets will push players away with sizable force");
			SGAmod.UsesClips.Add(SGAmod.Instance.ItemType("SoldierRocketLauncher"), 6);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			SGAPlayer sgaply = Main.LocalPlayer.SGAPly();
			if (sgaply.tf2emblemLevel > 0)
			tooltips.Add(new TooltipLine(mod, "SoldierLine", "Tier 1: Damage Increased by 30%, Reload and firing speed are faster per level"));
			if (sgaply.tf2emblemLevel > 1)
				tooltips.Add(new TooltipLine(mod, "SoldierLine", "Tier 2: Damage Increased by 50%; rockets explode larger and move faster per level"));
			if (sgaply.tf2emblemLevel > 2)
				tooltips.Add(new TooltipLine(mod, "SoldierLine", "Tier 3: Damage Increased by 75%; Rockets slow targets, gain a firingspeed boost when you rocket jump"));
			if (sgaply.tf2emblemLevel > 3)
				tooltips.Add(new TooltipLine(mod, "SoldierLine", "Tier 4: Damage Increased by 300%; Rocket jumping restores WingTime and slow is stronger"));
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 4) % 1f, 0.4f, 0.6f);
			tooltips.Add(new TooltipLine(mod, "IDG Debug Item", Idglib.ColorText(c, "'He didn't fly into heaven, he rocket jumped into heaven'")));
			c = Main.hslToRgb((float)((Main.GlobalTime+5.77163f) / 4) % 1f, 0.35f, 0.65f);
			tooltips.Add(new TooltipLine(mod, "IDG Debug Item", Idglib.ColorText(c, "RIP Rick May: 1940-2020")));
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			SGAPlayer sgaply = player.SGAPly();
			if (sgaply.tf2emblemLevel > 0)
				mult += 0.30f;
			if (sgaply.tf2emblemLevel > 1)
				mult += 0.50f;
			if (sgaply.tf2emblemLevel > 2)
				mult += 0.75f;
			if (sgaply.tf2emblemLevel > 3)
				mult += 3.00f;
		}
		public override void SetDefaults()
		{
			var itemsnd=item.UseSound;
			item.CloneDefaults(ItemID.RocketLauncher);
			item.UseSound = itemsnd;
			item.damage = 35;
			item.width = 48;
			item.height = 48;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 6;
			item.crit = 10;
			item.value = 250000;
			item.ranged = true;
			item.rare = 8;
			item.shootSpeed = 7f;
			item.shoot= mod.ProjectileType("SoldierRocketLauncherProj");
			item.noMelee = true;
			item.useAmmo = AmmoID.Rocket;
			item.expert = true;
		}
		public override float UseTimeMultiplier(Player player)
		{
			SGAPlayer sgaply = player.SGAPly();
			return (sgaply.soldierboost > 0 ? 2.5f : 1f) + ((float)sgaply.tf2emblemLevel * 0.25f);
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -6);
		}

		public override void HoldItem(Player player)
		{
			SGAPlayer sply = player.SGAPly();
			if (sply.timer % (50- (int)(((float)sply.previoustf2emblemLevel*6)*sply.RevolverSpeed)) == 0 && sply.timer > (sply.previoustf2emblemLevel > 3 ? 80 : 90) - (sply.previoustf2emblemLevel * 12) && sply.ammoLeftInClip < sply.ammoLeftInClipMax)
			{
				sply.ammoLeftInClip += 1;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/rocket_reload").WithVolume(.9f).WithPitchVariance(.15f), player.Center);
			}
		}

		public override bool CanUseItem(Player player)
		{
			SGAPlayer sply = player.SGAPly();
			return sply.ammoLeftInClip>0;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().timer = 1;
			type = mod.ProjectileType("SoldierRocketLauncherProj");

			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/rocket_shoot").WithVolume(.4f).WithPitchVariance(.15f), player.Center);

			position = player.Center;

			player.SGAPly().ammoLeftInClip -= 1;

			knockBack = player.SGAPly().tf2emblemLevel;
			//position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			int theproj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, knockBack);
			return false;

		}


	}

	public class SoldierRocketLauncherProj : ModProjectile
	{

		double keepspeed = 0.0;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soldier's Rocket");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
			projectile.localNPCHitCooldown = -1;
			projectile.usesLocalNPCImmunity = true;
			aiType = -1;
			projectile.aiStyle = -1;
			projectile.extraUpdates = 2;
		}

		public override string Texture
		{
			get { return "SGAmod/Projectiles/SoldierRocketLauncherProj"; }
		}

		bool hitonce = false;

		public override bool PreKill(int timeLeft)
		{
			float size = projectile.ai[1];

			for (int i = 0; i < Main.maxPlayers; i += 1)
			{
				Player pp = Main.player[i];
				float dist = (pp.Center - projectile.Center).Length();
				float dist2 = 120 + (int)Math.Max(size - 1, 0) * 30;
				if (pp.active && dist < dist2)
				{
					Vector2 norm = (pp.Center - projectile.Center); norm.Normalize();
					pp.velocity += (norm) * (1f - (dist / dist2)) * (24f + (projectile.ai[1] * 2f));

					if (projectile.ai[1] > 3)
					pp.wingTime = MathHelper.Clamp(pp.wingTime+((1f - (dist / dist2)) * (25f)),0, pp.wingTimeMax);
					if (projectile.ai[1] > 2)
					{
						pp.SGAPly().soldierboost = Math.Max(pp.SGAPly().soldierboost, (int)((projectile.ai[1] > 3 ? 150 : 80)* (1f - (dist / dist2))));

					}
				}
			}

			for (int i = 0; i < 125; i += 1)
			{
				float randomfloat = Main.rand.NextFloat(1f, 4f+ projectile.ai[1]);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();

				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 32, projectile.Center.Y - 32), 64, 64, DustID.Fire);
				Main.dust[dust].scale = 2.5f;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].velocity = (randomcircle * randomfloat);
			}

			float perc = 0.25f;
			if (projectile.ai[1]>3)
				perc = 0.50f;

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((float)projectile.damage * perc), projectile.knockBack, projectile.owner, 0f, projectile.ai[1]);
			Main.projectile[theproj].ranged = true;
			Main.projectile[theproj].usesLocalNPCImmunity = true;
			Main.projectile[theproj].localNPCHitCooldown = -1;
			Main.projectile[theproj].width = 120 + (int)Math.Max(size - 1, 0) * 30;
			Main.projectile[theproj].height = 120 + (int)Math.Max(size - 1, 0) * 30;
			Main.projectile[theproj].Center = projectile.Center;

			projectile.velocity = default(Vector2);
			projectile.type = size>2 ? ProjectileID.GrenadeIII : size > 0 ? ProjectileID.GrenadeI : ProjectileID.Grenade;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!hitonce)
			{
				hitonce = true;

				if (projectile.ai[1] > 2)
				{
					Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("RocketShockBoom"), 0, projectile.knockBack, projectile.owner, 0f, 0f);
					target.AddBuff(mod.BuffType("DankSlow"), 60 * (projectile.ai[1] > 3 ? 7 : 4));
					target.velocity /= (projectile.ai[1] > 3 ? 20f : 5f);
				}

				projectile.timeLeft = 1;

			}
			//projectile.Center -= new Vector2(48,48);
		}

		public override void AI()
		{
			projectile.rotation = ((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f)-MathHelper.ToRadians(-90f);

			if (projectile.ai[0] == 0)
			{
				Vector2 velocs = projectile.velocity;
				velocs.Normalize();
				if (projectile.ai[1] > 1)
					projectile.velocity += velocs * ((projectile.ai[1]-1f)/2f);
			}

			projectile.ai[0] = projectile.ai[0] + 1;

			if (Main.rand.Next(0, 3) == 0)
			{
				for (float i = 0; i < 2.5; i += 0.75f)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, Main.rand.Next(0, 100) < 15 ? DustID.Fire : DustID.Smoke);
					Main.dust[dust].scale = 1.25f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = -projectile.velocity * (float)(Main.rand.Next(20, 50 + (int)(i * 40f)) * 0.01f) / 2f;
				}
			}
		}


	}

	public class RocketShockBoom : ModProjectile
	{
		float ranspin = 0;
		float ranspin2 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soldier Blast");
		}

		public void getstuff()
		{

			if (ranspin2 == 0)
			{
				ranspin2 = Main.rand.NextFloat(-0.2f, 0.2f);
			}
			else
			{
				ranspin += ranspin2;

			}
		}

		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 20;
			projectile.penetrate = -1;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			getstuff();
			Texture2D tex = SGAmod.ExtraTextures[96];
			float timeleft = ((float)projectile.timeLeft / 20f);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition));
			Color color = Color.White * timeleft;
			spriteBatch.Draw(tex, drawPos, null, color, ranspin, drawOrigin, (1f - timeleft) * (7f+(projectile.ai[1]*3f)), SpriteEffects.None, 0f);
			return false;
		}


		public override void AI()
		{
			projectile.localAI[0] += 1f;

			if (projectile.ai[0] < 1)
			{
				projectile.ai[0] = 1;
			}

			return;
		}
	}

	public class PrismalLauncher : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Launcher");
			Tooltip.SetDefault("Launches a trio of rockets that may inflict a myriad of debuffs\n'Something something rocket launcher upgrade'");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.RocketLauncher);
			item.damage = 80;
			item.width = 48;
			item.height = 48;
			item.useTime = 30;
			item.useAnimation = 30;
			item.knockBack = 6;
			item.value = 500000;
			item.ranged = true;
			item.rare = 9;
			item.shootSpeed = 14f;
			item.noMelee = true;
			item.useAmmo = AmmoID.Rocket;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -6);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 4f;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < 7; i += 3)
			{

				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * (speed + ((float)i))).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				speedX = perturbedSpeed.X;
				speedY = perturbedSpeed.Y;

				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 600;
				Main.projectile[proj].knockBack = item.knockBack;

				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, mod.BuffType("ThermalBlaze"), 60 * 10);
				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, BuffID.DryadsWardDebuff, 60 * 10);
				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, BuffID.ShadowFlame, 60 * 10);
				if (Main.rand.Next(0, 100) < 20)
					IdgProjectile.AddOnHitBuff(proj, BuffID.Venom, 60 * 10);

			}

			return false;

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new StarMetalRecipes(mod);
			recipe.AddIngredient(ItemID.RocketLauncher, 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 15);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}

}