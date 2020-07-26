using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons;
using SGAmod.Projectiles;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	


	public class TrapWeapon : ModItem
	{

		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(TrapWeapon) && GetType() != typeof(DefenseTrapWeapon);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{

			if (GetType() != typeof(SuperDartTrapGun) && item.accessory != true && item.damage > 0)
			{
				tooltips.RemoveAt(2);
			}

		}

	}

		public class DartTrapGun : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dart Trap 'gun'");
			Tooltip.SetDefault("Atleast those traps might be of some use in a fight now" +
				"\nUses Darts as ammo, launches dart trap darts\nTrap Darts Pierce infinitely, but don't crit or count as player damage (they won't activate on damage buffs, for example)");
		}

		public override void SetDefaults()
		{
			item.damage = 28;
			item.ranged = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 100000;
			item.rare = 4;
			item.autoReuse = true;
			item.UseSound = SoundID.Item11;
			item.shootSpeed = 9f;
			item.shoot = ProjectileID.PoisonDart;
			item.useAmmo = AmmoID.Dart;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddIngredient(ItemID.DartTrap, 1);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//if (type == ProjectileID.WoodenArrowFriendly)
			//{
			type = ProjectileID.PoisonDartTrap;
			//}
			int probg = Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].ranged = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Main.projectile[probg].owner= player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			return false;
		}

	}

	public class PortableMakeshiftSpearTrap : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portable 'Makeshift' Spear Trap");
			Tooltip.SetDefault("It's not the same as found in the temple, but it'll do" +
				"\nLaunches piercing spears at close range" +
	"\nCounts as trap damage, pierces infinitely, but doesn't crit");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DartTrapGun"); }
		}


		public override void SetDefaults()
		{
			item.damage = 35;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 25;
			item.useAnimation = 25;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 100000;
			item.rare = 4;
			item.autoReuse = true;
			item.UseSound = SoundID.Item11;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("TrapSpearGun");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddIngredient(ItemID.DartTrap, 1);
			recipe.AddIngredient(ItemID.Spear, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].melee = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			return false;
		}

	}

	public class PortableSpearTrapGun : PortableMakeshiftSpearTrap
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Portable Spear Trap");
			Tooltip.SetDefault("Now we're stabbing" +
				"\nVery quickly launches piercing spears at close range" +
	"\nCounts as trap damage, pierces infinitely, but doesn't crit");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/PortableSpearTrap"); }
		}


		public override void SetDefaults()
		{
			item.damage = 120;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 9;
			item.value = 100000;
			item.rare = 9;
			item.autoReuse = true;
			item.UseSound = SoundID.Item11;
			item.shootSpeed = 10f;
			item.shoot = mod.ProjectileType("TrapSpearGun2");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PortableMakeshiftSpearTrap"), 1);
			recipe.AddIngredient(ItemID.SpearTrap, 5);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 2);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 25);
			recipe.AddIngredient(ItemID.LihzahrdPressurePlate, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)speedX * 4, position.Y + (int)speedY * 4, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].melee = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			return false;
		}

	}


	public class SuperDartTrapGun : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Dart Trap 'gun'");
			Tooltip.SetDefault("With this, you can carry their own tech against them" +
				"\nLaunches Darts at fast speeds!\nConverts Poison Darts into Dart Trap Darts\nTrap Darts Pierce infinitely, but don't crit");
		}

		public override void SetDefaults()
		{
			item.damage = 85;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 100000;
			item.rare = 9;
			item.autoReuse = true;
			item.UseSound = SoundID.Item99;
			item.shootSpeed = 15f;
			item.shoot = ProjectileID.PoisonDart;
			item.useAmmo = AmmoID.Dart;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 10);
			recipe.AddIngredient(ItemID.SuperDartTrap, 1);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
			recipe.AddIngredient(ItemID.LihzahrdPressurePlate, 1);
			recipe.AddIngredient(ItemID.Nanites, 50);
			recipe.AddIngredient(mod.ItemType("DartTrapGun"), 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10,4);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (type == ProjectileID.PoisonDartBlowgun || type == ProjectileID.PoisonDart)
			{
				type = ProjectileID.PoisonDartTrap;
			}
			//}
			int probg = Projectile.NewProjectile(position.X + (int)speedX * (type == ProjectileID.PoisonDartTrap ? 2 : 0), position.Y + (int)speedY * (type==ProjectileID.PoisonDartTrap ? 2 : 0), speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].ranged = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			IdgProjectile.Sync(probg);
			return false;
		}

	}

	public class FlameTrapThrower : DartTrapGun
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("FlameTrap 'Thrower'");
			Tooltip.SetDefault("'Of course the hottest flames are found within the temple dedicated to the sun'\nSprays fire that remains in place for a couple of seconds" +
				"\nUses Gel as ammo, 50% chance to not consume gel\nPress Alt Fire to spray the flames in a wide arc instead\nCounts as trap damage, pierces infinitely, but dooesn't crit");
		}

		public override bool ConsumeAmmo(Player player)
		{
			if (Main.rand.Next(0, 100) <= 50)
				return false;

			return base.ConsumeAmmo(player);
		}

		public override void SetDefaults()
		{
			item.damage = 60;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 5;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0.25f;
			item.value = 100000;
			item.rare = 9;
			item.autoReuse = true;
			item.UseSound = SoundID.Item34;
			item.shootSpeed = 10f;
			item.shoot = ProjectileID.FlamethrowerTrap;
			item.useAmmo = AmmoID.Gel;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 5);
			recipe.AddIngredient(ItemID.Nanites, 50);
			recipe.AddIngredient(ItemID.FlameTrap, 1);
			recipe.AddIngredient(ItemID.Flamethrower, 1);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
			recipe.AddIngredient(ItemID.LihzahrdPressurePlate, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 4);
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 2f), position.Y + (int)(speedY * 2f), speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].ranged = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(player.altFunctionUse==2 ? 60 : 5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			IdgProjectile.Sync(probg);
			return false;
		}

	}

	class ThrowableBoulderTrap : TrapWeapon
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("'Throwable' Boulder Trap");
			Tooltip.SetDefault("'Rolling Stones from the palm of your hand!'\nCounts as trap damage, Pierce infinitely, but don't crit");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.BoulderStaffOfEarth); }
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 160;
			item.shootSpeed = 1f;
			item.shoot = ProjectileID.Boulder;
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 28;
			item.knockBack = 5;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 150;
			item.useTime = 150;
			item.maxStack = 999;
			item.consumable = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 0, 0, 50);
			item.rare = 3;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].Throwing().thrown = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(25));
			//Main.projectile[probg].velocity.X = perturbedSpeed.X * player.thrownVelocity;
			//Main.projectile[probg].velocity.Y = perturbedSpeed.Y * player.thrownVelocity;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			IdgProjectile.Sync(probg);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Boulder, 25);
			recipe.AddIngredient(ItemID.Detonator, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 25);
			recipe.AddRecipe();
		}


		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}

	class ThrowableTrapSpikyball : TrapWeapon
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("'Throwable' Trap Spiky Ball");
			Tooltip.SetDefault("Dunno how, but hey, it's pretty neat!\nCounts as trap damage, Pierce infinitely, but don't crit");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+ProjectileID.SpikyBallTrap); }
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 90;
			item.shootSpeed = 8f;
			item.shoot = ProjectileID.SpikyBallTrap;
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 28;
			item.knockBack = 1;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 20;
			item.useTime = 20;
			item.maxStack = 999;
			item.consumable = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 0, 1, 0);
			item.rare = 8;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].Throwing().thrown = true;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			Main.projectile[probg].velocity.X = perturbedSpeed.X*player.thrownVelocity;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y*player.thrownVelocity;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			Main.projectile[probg].netUpdate = true;
			IdgProjectile.Sync(probg);
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LihzahrdBrick, 10);
			recipe.AddIngredient(ItemID.SpikyBall, 100);
			recipe.AddIngredient(ItemID.SpikyBallTrap, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this,100);
			recipe.AddRecipe();
		}


		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}


	public class TrapSpearGun2 : TrapSpearGun
	{

		public override int stuntime => 4;
		public override float traveldist => 500;
		int fakeid = ProjectileID.SpearTrap;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spear Trap");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.extraUpdates = 2;
		}
	}

		public class TrapSpearGun : ModProjectile
	{

		public virtual int stuntime => 5;
		public virtual float traveldist => 300;
		int fakeid = ProjectileID.SpearTrap;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spear Trap");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.CloneDefaults(ProjectileID.SpearTrap);
			projectile.aiStyle=-1;
			//projectile.type = ProjectileID.SpearTrap;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreAI()
		{
			projectile.type = ProjectileID.SpearTrap;
			return base.PreAI();
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = fakeid;
			return true;
		}

		public override void AI()
		{
			projectile.type = fakeid;
			Player basep = Main.player[projectile.owner];
			basep.itemAnimation = stuntime;
			basep.itemTime = stuntime;
			if (basep == null || basep.dead)
			{
				projectile.Kill();
				return;
			}

			if (projectile.ai[1] == 0f)
			{
				projectile.ai[1] = 1f;

			}
			Vector2 anglez = basep.Center - projectile.Center;
			anglez.Normalize(); anglez *= 5f;
				projectile.localAI[0] = basep.Center.X - anglez.X * (-1.5f);
				projectile.localAI[1] = basep.Center.Y - anglez.Y * (-1.5f);

			Vector2 value8 = new Vector2(projectile.localAI[0], projectile.localAI[1]);
			projectile.rotation = (basep.Center - value8).ToRotation() - 1.57079637f;
			basep.direction = ((projectile.Center- basep.Center).X > 0).ToDirectionInt();
			basep.itemRotation = (projectile.rotation+(float)(Math.PI/2))+(basep.direction<0 ? (float)Math.PI : 0f);
			if (projectile.ai[0] == 0f)
			{
				if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height))
				{
					projectile.velocity *= -1f;
					projectile.ai[0] += 1f;
					return;
				}
				float num384 = Vector2.Distance(projectile.Center, value8);
				if (num384 > traveldist)
				{
					projectile.velocity *= -1f;
					projectile.ai[0] += 1f;
					return;
				}
			}
			else if (Collision.SolidCollision(projectile.position, projectile.width, projectile.height) || Vector2.Distance(projectile.Center, value8) < projectile.velocity.Length()+5f)
			{
				projectile.Kill();
				return;
			}

			if (projectile.ai[0]>0)
			{
				float speezx = projectile.velocity.Length();
				projectile.velocity = basep.Center - projectile.Center;
				projectile.velocity.Normalize();
				projectile.velocity *= (speezx+0.15f);

			}

		}

		/*public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}*/

	}

	public class SpikeballFlail : TrapWeapon
	{
		public override void SetDefaults()
		{

			item.width = 30;
			item.height = 10;
			item.value = Item.sellPrice(0, 3, 0, 0);
			item.rare = 3;
			item.noMelee = true;
			item.useStyle = 5;
			item.useAnimation = 20;
			item.useTime = 44;
			item.knockBack = 6f;
			item.damage = 30;
			item.scale = 1f;
			item.noUseGraphic = true;
			item.shoot = mod.ProjectileType("SpikeballFlailProj");
			item.shootSpeed = 14f;
			item.UseSound = SoundID.Item1;
			item.melee = true;
			item.channel = true;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spike Ball Flail");
			Tooltip.SetDefault("Atleast this... I can buy being made into a weapon" +
				"\nCounts as trap damage, doesn't crit\nEnemies hit by the flail at high speeds may become Gourged; cutting their defense in half");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Chain, 25);
			recipe.AddIngredient(ItemID.Spike, 25);
			recipe.AddIngredient(ItemID.Hook, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}



	}

	public class SpikeballFlailProj : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 40;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.aiStyle = 15;
			projectile.trap = true;
			projectile.scale = 1f;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Spikeball");
		}
		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.SpikeBall); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (projectile.velocity.Length() > 13)
			target.AddBuff(mod.BuffType("Gourged"), 60 * 5);
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = Main.chainTexture;

			Vector2 position = projectile.Center;
			Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
			Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
			Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
			float num1 = (float)texture.Height;
			Vector2 vector2_4 = mountedCenter - position;
			float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
			bool flag = true;
			if (float.IsNaN(position.X) && float.IsNaN(position.Y))
				flag = false;
			if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
				flag = false;
			while (flag)
			{
				if ((double)vector2_4.Length() < (double)num1 + 5.0)
				{
					flag = false;
				}
				else
				{
					Vector2 vector2_1 = vector2_4;
					vector2_1.Normalize();
					position += vector2_1 * num1;
					vector2_4 = mountedCenter - position;
					Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
					color2 = projectile.GetAlpha(color2);
					Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
				}
			}
			return true;
		}
	}

	public class BookOfJones : TrapWeapon
	{
		public override void SetDefaults()
		{
			item.damage = 160;
			item.width = 16;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = 3;
			item.noMelee = true;
			item.useStyle = 4;
			item.useAnimation = 40;
			item.useTime = 40;
			item.knockBack = 10f;
			item.scale = 1f;
			item.shoot = mod.ProjectileType("JonesBoulderSummon");
			item.shootSpeed = 14f;
			item.UseSound = SoundID.Item1;
			item.magic = true;
			item.mana = 40;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Book Of Jones");
			Tooltip.SetDefault("'Their turn to go running escaping danger'\nSummons portals above the player that rains down boulders in both directions" +
				"\nCounts as trap damage, doesn't crit");
		}
		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Items/Weapons/Landslide"); }
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 8;// + Main.rand.Next(2);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(360));
				HalfVector2 half = new HalfVector2(player.Center.X+(i - (numberProjectiles / 2)) * 20, player.Center.Y - 200);
				int prog = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI,ai1: ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue));
				Main.projectile[prog].netUpdate = true;
				IdgProjectile.Sync(prog);
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Landslide"), 1);
			recipe.AddIngredient(ItemID.StaffofEarth, 1);
			recipe.AddIngredient(mod.ItemType("ThrowableBoulderTrap"), 100);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}



	}


	public class JonesBoulderSummon : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Enforcing");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 48;
			projectile.timeLeft = 3;
			projectile.height = 48;
			projectile.magic = true;
			projectile.tileCollide = true;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item45, projectile.Center);

			int proj = Projectile.NewProjectile(projectile.Center, new Vector2(projectile.velocity.X, projectile.velocity.Y/3f), mod.ProjectileType("ProjectilePortalJones"), projectile.damage, projectile.knockBack, projectile.owner, ProjectileID.Boulder);
			Main.projectile[proj].penetrate = 2;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);

			return true;
		}

		public override void AI()
		{
			projectile.timeLeft += 2;
			bool cond = projectile.timeLeft == 4;
			for (int num621 = 0; num621 < (cond ? 15 : 1); num621++)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, projectile.velocity.X * (cond ? 1.5f : 0.5f), projectile.velocity.Y * (cond ? 1.5f : 0.5f), 20, Color.Red, 0.5f);
				Main.dust[num622].velocity *= 1f;
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;
			}


			Player player = Main.player[projectile.owner];
			projectile.ai[0] += 1;


			Vector2 speedz = projectile.velocity;
			float atspeed = speedz.Length();
			Vector2 gohere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(projectile.ai[1]) }.ToVector2();
			//ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
			speedz = gohere - projectile.Center;
			speedz.Normalize(); speedz *= atspeed;
			projectile.velocity = speedz;

			if ((projectile.Center- gohere).Length()< atspeed + 8 || projectile.timeLeft>300)
			{
				projectile.Kill();
			}

		}
	}

	public class ProjectilePortalJones : ProjectilePortal
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
			projectile.timeLeft = 70;
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

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(20)) * projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)projectile.ai[0], projectile.damage, projectile.knockBack, owner.whoAmI);
					Main.projectile[proj].magic = true;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}

}

//Trap Acc's
namespace SGAmod.Items.Accessories
{

	public class JindoshBuckler : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jindosh Buckler");
			Tooltip.SetDefault("'Inventive, yet strikingly crude and cruel'\nTrap Damage ignores 50% of enemy defense\nTrap damage may inflict Massive Bleeding\n10% of extra Trap Damage is dealt directly to your enemy's life\nThis ignores defense and damage reduction\n" +
				"Trap Damage increased by 10%\nYou reflect 2 times the damage you take back to melee attackers (Corruption Worlds)\n+20 Max HP and hearts give +5 Health (Crimson Worlds)");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = 8;
			item.defense = 5;
			item.accessory = true;
		}

		public void UpdateAccessoryThing(Player player, bool hideVisual, bool corrcrim)
		{
			player.GetModPlayer<SGAPlayer>().JaggedWoodenSpike = true;
			if (WorldGen.crimson || corrcrim)
				mod.GetItem("HeartGuard").UpdateAccessory(player, hideVisual);
			if (!WorldGen.crimson || corrcrim)
				mod.GetItem("JuryRiggedSpikeBuckler").UpdateAccessory(player, hideVisual);
			mod.GetItem("GoldenCog").UpdateAccessory(player, hideVisual);

		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			UpdateAccessoryThing(player, hideVisual,false);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilkRopeCoil, 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 5);
			recipe.AddIngredient(mod.ItemType("JaggedOvergrownSpike"), 4);
			recipe.AddIngredient(mod.ItemType("GoldenCog"), 1);
			recipe.AddIngredient(mod.ItemType("JuryRiggedSpikeBuckler"), 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SilkRopeCoil, 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 5);
			recipe.AddIngredient(mod.ItemType("JaggedOvergrownSpike"), 4);
			recipe.AddIngredient(mod.ItemType("GoldenCog"), 1);
			recipe.AddIngredient(mod.ItemType("HeartGuard"), 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class JaggedOvergrownSpike : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jagged Overgrown Spike");
			Tooltip.SetDefault("Trap Damage ignores 40% of enemy defense\nTrap Damage may inflict Massive Bleeding");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 7;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().JaggedWoodenSpike = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenSpike, 10);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class GoldenCog : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Golden Cog");
			Tooltip.SetDefault("10% of extra Trap Damage is dealt directly to your enemy's life\nThis ignores defense and damage reduction");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 6;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().GoldenCog = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cog, 100);
			recipe.AddRecipeGroup("SGAmod:Tier4Bars", 5);
			recipe.AddIngredient(mod.ItemType("SharkTooth"), 50);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class HeartGuard : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Heart Guard");
			Tooltip.SetDefault("Trap Damage increased by 10%\n+20 Max HP and hearts give +5 Health");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 4;
			item.defense = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().HeartGuard = true;
			player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.1f;
			player.statLifeMax2 += 20;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddIngredient(ItemID.CrimsonHeart, 1);
			recipe.AddIngredient(ItemID.HeartreachPotion, 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class JuryRiggedSpikeBuckler : TrapWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("'JuryRigged' Spike Buckler");
			Tooltip.SetDefault("Trap Damage increased by 10% and ignores 10% of enemy defense\nYou reflect 2 times the damage you take back to melee attackers");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 4;
			item.defense = 5;
			item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			base.UpdateAccessory(player, hideVisual);
			player.GetModPlayer<SGAPlayer>().JuryRiggedSpikeBuckler = true;
			player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.1f;
			player.thorns += 2f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Spike, 40);
			recipe.AddIngredient(ItemID.ThornsPotion, 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class GrippingGloves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gripping Gloves");
			Tooltip.SetDefault("'For holding onto the big things in your life'\nReduces the movement speed slowdown of Non-Stationary Defenses\nYou can turn around while holding a Non-Stationary Defense\n10% increased Trap Damage");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.10f;
			player.GetModPlayer<SGAPlayer>().grippinggloves = true;
			player.GetModPlayer<SGAPlayer>().SlowDownResist += 2f;
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.AleThrowingGlove); }
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = 15000;
			item.rare = 2;
			item.accessory = true;
		}
	}
	public class HandlingGloves : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Handling Gloves");
			Tooltip.SetDefault("'For handling extreme situations!'\nImmunity to knockback and fire blocks!\n+8 defense while holding a Non-Stationary Defense\nGreatly reduces the movement speed slowdown of Non-Stationary Defenses\nYou can turn around while holding a Non-Stationary Defense\n15% increased Trap Damage and 10% increased Trap Armor Penetration");
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<SGAPlayer>().TrapDamageMul += 0.15f;
			player.GetModPlayer<SGAPlayer>().TrapDamageAP += 0.10f;
			player.GetModPlayer<SGAPlayer>().grippinggloves = true;
			player.GetModPlayer<SGAPlayer>().SlowDownResist += 8f;
			player.noKnockback = true;
			player.fireWalk = true;
			if (SGAmod.NonStationDefenses.ContainsKey(player.HeldItem.type))
				player.statDefense += 8;

		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.AleThrowingGlove); }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Orange;
		}

		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 32;
			item.height = 32;
			item.value = 75000;
			item.rare = 6;
			item.accessory = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("GrippingGloves"), 1);
			recipe.AddIngredient(ItemID.ObsidianShield, 1);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddIngredient(ItemID.HellstoneBar, 5);
			recipe.AddIngredient(mod.ItemType("SharkTooth"), 50);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}