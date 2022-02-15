using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items.Weapons;
using AAAAUThrowing;
using System.IO;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using SGAmod.HavocGear.Items.Weapons;

namespace SGAmod.Items.Weapons
{
	public class RevolverBase : ModItem
	{
		public bool forcedreload = false;
		public virtual int RevolverID => 0;
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(RevolverBase);
		}

		public override bool CanUseItem(Player player)
		{
			forcedreload = false;
			item.autoReuse = false;

			if (player.GetModPlayer<SGAPlayer>().ReloadingRevolver > 0 || forcedreload)
				return false;

			if (!player.SGAPly().ConsumeAmmoClip(false)) { item.UseSound = SoundID.Item98; forcedreload = true; item.useTime = 4; item.useAnimation = 4; item.noUseGraphic = true; }
			return true;
		}

		public override void HoldItem(Player player)
		{
			if (player.itemAnimation < 1)
			{
				SGAPlayer sgaply = player.SGAPly();
				int val = 6;
				SGAmod.UsesClips.TryGetValue(player.HeldItem.type, out val);
				if (sgaply.ammoLeftInClipMax != val + sgaply.ammoLeftInClipMaxAddedAmmo)
				{
					sgaply.ammoLeftInClip = 0;
				}

				sgaply.ammoLeftInClipMaxLastHeld = sgaply.ammoLeftInClipMax;
			}
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "RevolverHomingPenalty", Idglib.ColorText(Color.Red, "Damage from Homing ammo is reduced by 75%")));
		}
		public bool HomingAmmo(int type)
        {
			return ProjectileID.Sets.Homing[type];
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			damage = HomingAmmo(type) ? (int)(damage * 0.25f) : damage;
			return false;
        }
    }
	public class DragonRevolver : RevolverBase,IDevItem
	{
		bool altfired = false;
		public override int RevolverID => mod.ProjectileType("DragonRevolverReloading");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Serpent's Redemption");
			Tooltip.SetDefault("Hold Left Click and hover your mouse over targets to mark them for execution: releasing a dragon-fire burst on them!\nYou may mark targets as long as you have ammo in the clip and nothing is blocking your way\nUp to 6 targets may be marked for execution; a target that resists however can be marked more than once\nThe explosion is unable to crit but hits several times\nAlt Fire shoots 3 accurate rounds at once if the bullet does not pierce more than 3 times, otherwise 1\nThe extra bullets do only 50% base damage\n'Thy time has come'ith for dragon slayers, repent!'");
			SGAmod.UsesClips.Add(SGAmod.Instance.ItemType("DragonRevolver"), 6);
		}
		public (string, string) DevName()
		{
			return ("IDGCaptainRussia94", "");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[0] > 0)
			{
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Primary Explosion is larger"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Secondary fires faster"));
			}
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Revolver);
			item.damage = 2000;
			item.width = 48;
			item.height = 48;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 10;
			item.value = Item.sellPrice(2, 0, 0, 0);
			item.rare = 12;
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.useAmmo = AmmoID.Bullet;
			item.autoReuse = false;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.noUseGraphic = false;
			item.UseSound = mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot");
			item.useStyle = 5;
			item.expert = true;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (sgaplayer.ReloadingRevolver > 0)
				return false;

				if (player.ownedProjectileCounts[mod.ProjectileType("RevolverTarget")] > 0)
				return false;

			altfired = player.altFunctionUse == 2 ? false : true;
			forcedreload = false;
			item.noUseGraphic = false;

			if (altfired && sgaplayer.ConsumeAmmoClip(false))
			{
				item.useAnimation = 5;
				item.useTime = 5;
				item.useStyle = 5;
				item.UseSound = SoundID.Item35;
				item.channel = true;
				item.shoot = mod.ProjectileType("DragonRevolverAiming");
			} else {
				item.useStyle = 5;
				int firerate = sgaplayer.devempowerment[0] > 0 ? 45 : 60;
				item.useTime = firerate;
				item.useAnimation = firerate;
				item.UseSound = SoundID.Item38;
				item.channel = false;
				item.shoot = 10;
				if (!sgaplayer.ConsumeAmmoClip(false)) { item.UseSound = SoundID.Item98; forcedreload = true; item.useTime = 4; item.useAnimation = 4; item.noUseGraphic = true; }
			}
			return true;
		}

		public override bool ConsumeAmmo(Player player)
		{
			return (item.shoot != mod.ProjectileType("DragonRevolverAiming"));
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			//base.Shoot(player,ref position,ref speedX,ref speedY,ref type,ref damage,ref knockBack);
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

			if (!altfired && sgaplayer.ConsumeAmmoClip(false))
			{
				sgaplayer.ConsumeAmmoClip(true);
				Projectile proj = new Projectile();
				proj.SetDefaults(type);

				if (proj.penetrate < 4 && proj.penetrate > -1)
				{

					for (int i = 0; i < 2; i += 1)
					{
						int thisoned = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)(damage / 2), knockBack, Main.myPlayer);
					}
				}
			}

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				player.itemTime = 50;
				player.itemAnimation = 50;
				if (forcedreload) {
					player.itemTime = 1;
					player.itemAnimation = 1;
				}
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				// Main.projectile[thisone].spriteDirection=normalizedspeed.X>0f ? 1 : -1;
				//Main.projectile[thisone].rotation=(new Vector2(speedX,speedY)).ToRotation();

				return !forcedreload;
			}

			if (altfired) {
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("DragonRevolverAiming"), 1, 0f, Main.myPlayer, 0.0f, 0f);
				return false;
			}

			//if (sgaplayer.ammoLeftInClip > 0)
			//{
			//}
			return (sgaplayer.ConsumeAmmoClip(false));
		}




		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("TheJacob"), 1);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 25);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 20);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 8);
			recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class DragonRevolverAiming : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DragonRevolver"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			projectile.damage = 1;
			aiType = 0;
		}

		public override bool? CanCutTiles() { return false; }

		public override bool? CanHitNPC(NPC target)
		{
			Player player = Main.player[projectile.owner];
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			int ownedproj = player.ownedProjectileCounts[mod.ProjectileType("RevolverTarget")];
			if (!target.HasBuff(mod.BuffType("Targeted")) && !target.friendly && sgaplayer.ConsumeAmmoClip(false) && ownedproj < 6 && projectile.ai[0] < 1 && (Collision.CanHitLine(new Vector2(target.Center.X, target.Center.Y), 1, 1, new Vector2(player.Center.X, player.Center.Y), 1, 1))) {
				return true;
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player player = Main.player[projectile.owner];
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			int ownedproj = player.ownedProjectileCounts[mod.ProjectileType("RevolverTarget")];

			IdgNPC.AddBuffBypass(target.whoAmI, mod.BuffType("Targeted"), 3, false);
			int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, mod.ProjectileType("RevolverTarget"), 0, 0f, projectile.owner, 0.0f, 0f);
			Main.projectile[thisone].ai[0] = target.whoAmI;
			Main.projectile[thisone].netUpdate = true;
			sgaplayer.ConsumeAmmoClip();
			//Main.PlaySound(mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot"),(int)Main.player[projectile.owner].position.X,(int)Main.player[projectile.owner].position.Y,1,1.15f,((float)ownedproj)/4f);
			//Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot").WithVolume(1.1f).WithPitchVariance(.25f));
			Main.PlaySound(SoundLoader.customSoundType, (int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot"), 1.15f, ((float)-0.4 + (ownedproj) / 6f));
		}

		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];

			if (projectile.ai[0] > 1000f || player.dead)
			{
				projectile.Kill();
			}
			if (!player.channel || projectile.ai[0] > 0) {
				projectile.ai[0] += 1;
				if (projectile.ai[0] < 5f)
					projectile.ai[0] = 79f;
			}
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 diff = mousePos - player.Center;
				diff.Normalize();
				projectile.velocity = diff;
				if (projectile.ai[0] < 50f)
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				projectile.netUpdate = true;
				projectile.Center = mousePos;
			}
			int dir = projectile.direction;
			player.ChangeDir(dir);
			player.itemTime = 5;
			player.itemAnimation = 5;
			if (projectile.ai[0] < 50f)
				player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);



			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(projectile.position, 16, 16, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}

			projectile.timeLeft = 2;


			if (projectile.ai[0] > 65) { projectile.ai[0] = 58;
				int ownedproj = player.ownedProjectileCounts[mod.ProjectileType("RevolverTarget")];
				Projectile thetarget;
				thetarget = null;
				for (int i = 0; i < Main.maxProjectiles; i++)
				{
					Projectile him = Main.projectile[i];
					if (him.type == mod.ProjectileType("RevolverTarget")) {
						if (him.active && him.owner == projectile.owner) {
							thetarget = him;
							break;
						} } }
				if (thetarget != null) {
					Vector2 angle = thetarget.Center - Main.player[projectile.owner].Center;
					projectile.direction = angle.X > player.position.X ? 1 : -1;
					player.itemRotation = (float)Math.Atan2(angle.Y * dir, angle.X * dir);
					angle.Normalize();
					int proj = Projectile.NewProjectile(thetarget.Center.X, thetarget.Center.Y, 0f, 0f, mod.ProjectileType("SlimeBlast"), (int)(player.GetModPlayer<SGAPlayer>().devempowerment[0] > 0 ? 4000 : 4000 * (player.rangedDamage)), 15f, projectile.owner, 0f, 0f);
					Main.projectile[proj].direction = projectile.direction;
					Main.projectile[proj].ranged = true;
					if (player.GetModPlayer<SGAPlayer>().devempowerment[0] > 0)
					{
						Main.projectile[proj].width += 128;
						Main.projectile[proj].height += 128;
						Main.projectile[proj].Center -= new Vector2(64, 64);


					}
					Main.projectile[proj].netUpdate = true;
					Main.PlaySound(SoundID.Item45, thetarget.Center);
					Main.PlaySound(SoundID.Item41, player.Center);
					thetarget.Kill();
				} else {
					Main.PlaySound(SoundID.Item63, player.Center);
					player.itemTime = 80;
					player.itemAnimation = 80;
					projectile.Kill();
				}
			}



		}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return true;
		}

	}


	public class RevolverTarget : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/FieryMoon"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 32;
			projectile.height = 32;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 100;
			projectile.penetrate = -1;
			projectile.scale = 1f;
			aiType = 0;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			NPC target = Main.npc[(int)projectile.ai[0]];

			if (!target.active || player.dead)
			{
				projectile.Kill();
			}
			projectile.Center = target.Center;
			IdgNPC.AddBuffBypass(target.whoAmI, mod.BuffType("Targeted"), 3, false);
			projectile.timeLeft = 3;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color glowingcolors1 = Color.Red;//Main.hslToRgb((float)lightColor.R*0.08f,(float)lightColor.G*0.08f,(float)lightColor.B*0.08f);
			spriteBatch.Draw(Main.blackTileTexture, drawPos, new Rectangle(0, 0, 120, 10), glowingcolors1, projectile.rotation, new Vector2(60, 5), new Vector2(1, 1), SpriteEffects.None, 0f);
			spriteBatch.Draw(Main.blackTileTexture, drawPos, new Rectangle(0, 0, 10, 120), glowingcolors1, projectile.rotation, new Vector2(5, 60), new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

	}

	public class TheJacob : RevolverBase
	{
		bool altfired = false;
		public override int RevolverID => mod.ProjectileType("TheJacobReloading");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Jakob");
			Tooltip.SetDefault("Right click to fan the hammer-rapidly fire the remaining clip with less accuracy\nIf it took more than 1 shot, you weren't using a Jakob's!'");
			SGAmod.UsesClips.Add(SGAmod.Instance.ItemType("TheJacob"), 6);
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Revolver);
			item.damage = 200;
			item.width = 48;
			item.height = 48;
			item.useTime = 40;
			item.useAnimation = 40;
			item.knockBack = 10;
			item.value = 10000;
			item.rare = 5;
			item.crit = 15;
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.useAmmo = AmmoID.Bullet;
			item.autoReuse = false;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.noUseGraphic = false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (sgaplayer.ReloadingRevolver > 0)
				return false;


			altfired = player.altFunctionUse == 2 ? true : false;
			forcedreload = false;
			item.noUseGraphic = false;

			if (altfired && sgaplayer.ConsumeAmmoClip(false))
			{
				item.useAnimation = 2000;
				item.useTime = 10;
				item.UseSound = SoundID.Item38;
			}
			else
			{
				item.useTime = 40;
				item.useAnimation = 40;
				item.UseSound = SoundID.Item38;
				if (!sgaplayer.ConsumeAmmoClip(false)) { item.UseSound = SoundID.Item98; forcedreload = true; item.useTime = 4; item.useAnimation = 4; item.noUseGraphic = true; }
			}
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.ammoLeftInClip -= 1;
			if (item.useAnimation > 1000)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(20));
				speedX = perturbedSpeed.X;
				speedY = perturbedSpeed.Y;
				Main.PlaySound(SoundID.Item38, player.Center);
			}
			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				player.itemTime = 40;
				player.itemAnimation = 40;
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				// Main.projectile[thisone].spriteDirection=normalizedspeed.X>0f ? 1 : -1;
				//Main.projectile[thisone].rotation=(new Vector2(speedX,speedY)).ToRotation();
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}




		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RevolverUpgrade"), 1);
			recipe.AddIngredient(ItemID.HallowedBar, 8);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("GuerrillaPistol"), 1);
			recipe.AddIngredient(ItemID.HallowedBar, 10);
			recipe.AddIngredient(ItemID.SoulofFright, 20);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class RevolverUpgrade : RevolverBase
	{
		bool altfired = false;
		public override int RevolverID => mod.ProjectileType("TheRevolverReloading");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Revolving West");
			Tooltip.SetDefault("Right click to fire an extra bullet at the closest enemy\nBut this halves the damage of both bullets");
			SGAmod.UsesClips.Add(SGAmod.Instance.ItemType(GetType().Name), 6);
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.Revolver);
			item.damage = 42;
			item.width = 48;
			item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.knockBack = 10;
			item.value = 50000;
			item.rare = 3;
			item.noMelee = true;
			item.useAmmo = AmmoID.Bullet;
			item.autoReuse = false;
			item.shoot = 10;
			item.shootSpeed = 40f;
			item.noUseGraphic = false;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{
			if (player.GetModPlayer<SGAPlayer>().ReloadingRevolver > 0)
				return false;
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			altfired = player.altFunctionUse == 2 ? true : false;
			forcedreload = false;
			item.noUseGraphic = false;

			if (altfired)
			{
				item.useAnimation = 16;
				item.useTime = 16;
				item.UseSound = SoundID.Item38;
			}
			else
			{
				item.useTime = 12;
				item.useAnimation = 12;
				item.UseSound = SoundID.Item38;
			}
			if (!sgaplayer.ConsumeAmmoClip(false)) { item.UseSound = SoundID.Item98; forcedreload = true; item.useTime = 4; item.useAnimation = 4; item.noUseGraphic = true; }
			return true;
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-3, 2);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.ConsumeAmmoClip();
			if (player.altFunctionUse == 2)
			{
				damage = (int)(damage * 0.5f);
				int target2 = Idglib.FindClosestTarget(0, position, new Vector2(0, 0));
				NPC them = Main.npc[target2];
				Vector2 where = them.Center - position;
				where.Normalize();
				Vector2 perturbedSpeed = new Vector2(where.X, where.Y) * (new Vector2(speedX, speedY).Length() * 1.25f);


				if (them.active && (them.Center - player.Center).Length() > 800)
				{
					perturbedSpeed = new Vector2(speedX, speedY) * 1.25f;
				}

				Main.PlaySound(SoundID.Item38, player.Center);
				int thisoned = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, Main.myPlayer);
			}
			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				player.itemTime = 40;
				player.itemAnimation = 40;
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}




		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Revolver, 1);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	/*
	public class TheRevolverReloading : ClipWeaponReloading
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.Revolver); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 30;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.penetrate = 10;
			projectile.scale = 0.7f;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

	}

	public class TheJacobReloading : ClipWeaponReloading
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/TheJacob"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 30;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.scale = 0.7f;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

	}

	public class DragonRevolverReloading : ClipWeaponReloading
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/DragonRevolver"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 30;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 180;
			projectile.penetrate = 10;
			projectile.scale = 0.7f;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

	}
	*/

		public class ClipWeaponReloading : ModProjectile
	{
		private string tex;
		private int timeLeft;
		public int ammoMax;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public ClipWeaponReloading(string tex,int timeLeft=100,int ammoMax = 6)
        {
			this.tex = tex;
			this.timeLeft = timeLeft;
			this.ammoMax = ammoMax;
		}
        public override bool CanDamage()
        {
            return false;
        }
        public override bool CloneNewInstances => true;

		public static void SetupRevolverHoldingTypes()
        {
			SGAmod.Instance.AddProjectile("TheRevolverReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/RevolverUpgrade",ammoMax: 6));
			SGAmod.Instance.AddProjectile("TheJacobReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/TheJacob",150, ammoMax: 6));
			SGAmod.Instance.AddProjectile("DragonRevolverReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/DragonRevolver",200, ammoMax: 6));
			SGAmod.Instance.AddProjectile("GuerrillaPistolReloading", new ClipWeaponReloading("SGAmod/HavocGear/Items/Weapons/GuerrillaPistol", ammoMax: 6));
			SGAmod.Instance.AddProjectile("GunarangReloading", new ClipWeaponReloading("SGAmod/Items/Weapons/Gunarang", ammoMax: 6));

			SGAmod.Instance.AddProjectile("RustyRifleReloading", new ClipWeaponReloading("SGAmod/HavocGear/Items/Weapons/RustyRifle", 150, ammoMax: 4));

		}

		public override bool Autoload(ref string name)
		{
			return false;
		}

		public override string Texture
		{
			get { return (tex); }
		}

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.thrown = true;
			projectile.timeLeft = timeLeft;
			projectile.penetrate = 10;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = 8;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 offset = new Vector2(projectile.spriteDirection<0 ? tex.Width-8 : 8, tex.Height / 2);
			if (Main.player[projectile.owner].itemAnimation < 1)
			spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, lightColor*Math.Min(projectile.timeLeft/20f,1f), projectile.rotation, offset,projectile.scale, projectile.spriteDirection < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
			return false;
		}

		public override bool PreAI()
		{
			Player owner = Main.player[projectile.owner];
			if (owner == null)
				projectile.Kill();
			return true;
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			Player owner = Main.player[projectile.owner];
			if (owner == null)
				projectile.Kill();
			if (owner.dead)
				projectile.Kill();

			if (owner.itemAnimation > 0)
			{
				projectile.timeLeft += 1;
				if (owner.itemAnimation == 1)
					projectile.timeLeft = (int)((float)projectile.timeLeft / owner.GetModPlayer<SGAPlayer>().RevolverSpeed);
			}
			else
			{
				owner.GetModPlayer<SGAPlayer>().ReloadingRevolver = 3;
				projectile.spriteDirection = (owner.direction > 0).ToDirectionInt();
				owner.heldProj = projectile.whoAmI;
				projectile.ai[0] += 1;
				projectile.velocity = new Vector2(0f, 0f);
				//projectile.rotation = projectile.rotation.AngleLerp((float)(Math.PI/-(4.0*(double)projectile.spriteDirection)),0.15f);
				owner.bodyFrame.Y = owner.bodyFrame.Height * 3;

				if (projectile.timeLeft == 18)
				{
					SGAPlayer sgaplayer = owner.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
					sgaplayer.ammoLeftInClip = ammoMax+sgaplayer.ammoLeftInClipMaxStack;
					sgaplayer.ammoLeftInClipMax = sgaplayer.ammoLeftInClip;
					sgaplayer.ammoLeftInClipMaxAddedAmmo = sgaplayer.ammoLeftInClipMaxStack;
					Main.PlaySound(SoundID.Item65, owner.Center);
				}

				/*if (owner.velocity.X<0)
				owner.direction=-1;
				projectile.spriteDirection=owner.direction;*/
			}

			//projectile.velocity=new Vector2(projectile.velocity.X,0f);
			projectile.Center = owner.Center + new Vector2(projectile.spriteDirection * 6, -4f);

		}


	}

		public class Gunarang : RevolverBase
	{
		public override int RevolverID => mod.ProjectileType("GunarangReloading");
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Throws the gun, bounces off walls once, shoots at enemies it hits\n'When the gun just gets a little loose...'");
			DisplayName.SetDefault("Gunarang");
			SGAmod.UsesClips.Add(item.type, 6);
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.damage = 60;
			item.ranged = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 15;
			item.useTime = 15;
			item.useStyle = 1;
			item.knockBack = 1f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 1;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<SpecterangProj>();
			item.shootSpeed = 10f;
			item.useAmmo = AmmoID.Bullet;
		}
        public override bool ConsumeAmmo(Player player)
        {
            return false;
        }
        public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<GunarangProj>()] > 0)
				return false;

			item.UseSound = SoundID.Item1;
			item.useTime = 15;
			item.useAnimation = 15;
			item.noUseGraphic = true;

			return base.CanUseItem(player);
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add -= player.rangedDamage;
			add += (player.meleeDamage + player.rangedDamage) / 2f;
			base.ModifyWeaponDamage(player, ref add, ref mult, ref flat);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				foreach (string text2 in thetext)
				{
					valuez.Add(text2 + " ");
				}
				valuez.RemoveAt(1);
				valuez.Insert(1, "Melee/Ranged ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}

			tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				int counter = 0;
				foreach (string text2 in thetext)
				{
					counter += 1;
					if (counter > 1)
						valuez.Add(text2 + " ");
				}
				int thecrit = Main.GlobalTime % 3f >= 1.5f ? Main.LocalPlayer.meleeCrit : Main.LocalPlayer.rangedCrit;
				string thecrittype = Main.GlobalTime % 3f >= 1.5f ? "Melee " : "Ranged ";
				valuez.Insert(0, thecrit + "% " + thecrittype);
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.SGAPly();

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			if (sgaplayer.ConsumeAmmoClip(false))
			{
				int thisone = Projectile.NewProjectile(position.X, position.Y, speedX / player.meleeSpeed, speedY / player.meleeSpeed, ModContent.ProjectileType<GunarangProj>(), damage, knockBack, Main.myPlayer);
				(Main.projectile[thisone].modProjectile as GunarangProj).ammoType = type;
				Main.projectile[thisone].netUpdate = true;
			}
			return false;
		}

	}

	public class GunarangProj : SpecterangProj
	{
		protected override int ReturnTime => 30;
		protected override int ReturnTimeNoSlow => 70;
		protected override float SolidAmmount => 6f;
		public int ammoType = -1;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gunarang");
		}

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			ammoType = reader.ReadInt32();
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write(ammoType);
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Gunarang"); }
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 24;
			projectile.height = 24;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.tileCollide = true;
		}

		public override void AI()
        {
            base.AI();

			Player owner = Main.player[projectile.owner];

			if (owner != null && owner.active)
			{
				if (projectile.ai[1] > 0)
				{
					NPC target = Main.npc[(int)projectile.ai[1] - 1];
					if (target != null && target.active)
					{
						if (projectile.ai[0] % 15 == 0)
						{

							int ammotype = (int)owner.GetModPlayer<SGAPlayer>().myammo;
							if (ammotype > 0 && owner.HasItem(ammotype))
							{
								Item ammo2 = new Item();
								ammo2.SetDefaults(ammotype);
								int ammo = ammo2.shoot;
								int damageproj = projectile.damage;
								float knockbackproj = projectile.knockBack;
								float sppez = 16f;
								if (ammo2.modItem != null)
									ammo2.modItem.PickAmmo(owner.HeldItem, owner, ref ammo, ref sppez, ref projectile.damage, ref projectile.knockBack);
								int type = ammo;

								if (owner.SGAPly().ConsumeAmmoClip())
								{
									owner.ConsumeItemRespectInfiniteAmmoTypes(ammo2.type);
									Projectile.NewProjectile(projectile.Center, Vector2.Normalize(target.Center - projectile.Center) * 16f, type, projectile.damage, projectile.knockBack,projectile.owner);

									SoundEffectInstance sound = Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 41);
									if (sound != null)
										sound.Pitch += 0.50f;
								}
							}

						}
                    }
                    else
                    {
						if (projectile.ai[1] > 0)
						projectile.ai[1] = 0;
                    }

				}
			}
		}

        public override bool CanDamage()
		{
			return true;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			projectile.ai[1] = target.whoAmI+1;

			Vector2 angledif = Vector2.Normalize(target.Center - projectile.Center);

			float leftOrRight = 1;

			if (angledif.ToRotation() - projectile.velocity.ToRotation() > 0)
				leftOrRight = -1;

			projectile.velocity = projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.Pi / 10f, MathHelper.Pi / 10f) +leftOrRight*MathHelper.PiOver2) *1f;
			projectile.netUpdate = true;
		}
        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Main.PlaySound(SoundID.Item10, projectile.Center);
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y;
			}
			projectile.tileCollide = false;
			projectile.netUpdate = true;
			return false;
        }
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D tex2 = ModContent.GetTexture("SGAmod/Items/GlowMasks/Gunarang_Glow");
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, tex.Size() / 2f, new Vector2(1f, 1f) * projectile.scale, default, 0);
			spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * 1f, projectile.rotation, tex.Size() / 2f, new Vector2(1f, 1f) * projectile.scale, default, 0);

			return false;
		}

	}

}
namespace SGAmod.HavocGear.Items.Weapons
{

	public class RustyRifle : RevolverBase
	{
		public override int RevolverID => mod.ProjectileType("RustyRifleReloading");
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rusty Rifle");
			Tooltip.SetDefault("Consumes all bullets in the clip to increase damage");
			SGAmod.UsesClips.Add(item.type, 4);
		}

		public override void SetDefaults()
		{
			item.damage = 8;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 7;
			item.value = 10000;
			item.rare = 1;
			item.UseSound = SoundID.Item40;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
		}

		public override bool CanUseItem(Player player)
		{
			item.noUseGraphic = false;
			item.UseSound = SoundID.Item11;
			item.useTime = 30;
			item.useAnimation = 30;
			return base.CanUseItem(player);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			SGAPlayer sgaplayer = player.SGAPly();
			int clipsize = sgaplayer.ammoLeftInClip;
			sgaplayer.ConsumeAmmoClip(true, clipsize);
			damage *= clipsize;

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
	}

	public class GuerrillaPistol : RevolverBase
	{
		public override int RevolverID => mod.ProjectileType("GuerrillaPistolReloading");
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Shoots a powerful, high velocity bullet");
			DisplayName.SetDefault("Guerrilla Pistol");
			SGAmod.UsesClips.Add(item.type, 6);
		}

		public override void SetDefaults()
		{
			item.damage = 7;
			item.ranged = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 1;
			item.UseSound = SoundID.Item11;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 16f;
			item.useAmmo = AmmoID.Bullet;
		}

        public override bool CanUseItem(Player player)
        {
			item.noUseGraphic = false;
			item.UseSound = SoundID.Item11;
			item.useTime = 30;
			item.useAnimation = 30;
			return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
			if (type == ProjectileID.Bullet)
			{
				type = ProjectileID.BulletHighVelocity;
			}
			SGAPlayer sgaplayer = player.SGAPly();
			sgaplayer.ConsumeAmmoClip();

			if (!sgaplayer.ConsumeAmmoClip(false) || forcedreload)
			{
				int thisone = Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, RevolverID, 0, knockBack, Main.myPlayer, 0.0f, 0f);
				return !forcedreload;
			}
			return (sgaplayer.ConsumeAmmoClip(false));
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-2, 0);
		}
	}
}
