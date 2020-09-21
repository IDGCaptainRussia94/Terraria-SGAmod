using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;
using System.Linq;

namespace SGAmod.Items.Weapons.Caliburn
{


	public class TrueCaliburn : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Caliburn");
			Tooltip.SetDefault("Summons True Spectral Blades to home in on your mouse cursor for a few seconds or until it hits 4 times, then it returns to you.\nTrue Spectral Blades summon Spectral Swords on hit, and do not hit while returning");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 80;
			item.crit = 10;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 23;
			item.useAnimation = 16;
			item.useStyle = 1;
			item.knockBack = 8;
			item.value=Item.sellPrice(1, 0, 0, 0);
			item.rare = 7;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("CaliburnHomingSwordTrue");
			item.shootSpeed = 20f;
			item.useTurn = false;
			item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			damage = (int)(damage * 1.50);
			Main.PlaySound(SoundID.Item101, player.Center);
			return (player.ownedProjectileCounts[mod.ProjectileType("CaliburnHomingSword")] < 100);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CaliburnTypeA"), 1);
			recipe.AddIngredient(mod.ItemType("CaliburnTypeB"), 1);
			recipe.AddIngredient(mod.ItemType("CaliburnTypeC"), 1);
			recipe.AddIngredient(mod.ItemType("CaliburnCompess"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}


	}


	public class CaliburnTypeA : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			Tooltip.SetDefault("One of the lost swords\nSummons Spectral copies of the sword to strike nearby enemies on swing\nThese do not cause immunity frames, but hit only once");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.crit = 0;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 16;
			item.useAnimation = 22;
			item.reuseDelay = 30;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value=Item.buyPrice(0, 5, 0, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("CaliburnSpectralBlade");
			item.shootSpeed = 20f;
			item.useTurn = false;
			item.autoReuse = true;
		}

		public static void SpectralSummon(Player player, int type, int damage,Vector2 there)
		{

			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(16);

			List<NPC> guys = new List<NPC>();

			for (int a = 0; a < Main.maxNPCs; a++)
			{
				NPC npchim = Main.npc[a];
				Projectile proj = new Projectile();
				proj.SetDefaults(ProjectileID.ChlorophyteBullet);

				if (npchim.active && !npchim.friendly && npchim.CanBeChasedBy() && (npchim.Center - there).Length() < 400)
				{
					guys.Add(npchim);
				}

			}
			if (guys.Count > 0)
			{
				for (int i = 0; i < numberProjectiles; i++)
				{
					NPC theone = guys[Main.rand.Next(0, guys.Count)];

					Vector2 perturbedSpeed = new Vector2(Main.rand.NextFloat(2f, 7f) * (Main.rand.NextBool() ? 1f : -1f), Main.rand.NextFloat(1f, 4f)); // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= Main.rand.NextFloat(0.7f, 2.2f);
					int proj = Projectile.NewProjectile((theone.Center.X - perturbedSpeed.X * ((float)(8f))) + ((perturbedSpeed.X > 0 ? -0.5f : 0.5f) * theone.width), (theone.Center.Y - 64) - perturbedSpeed.Y * 12f, perturbedSpeed.X, perturbedSpeed.Y, type, (int)((float)damage * 1f), 0, player.whoAmI);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].netUpdate = true;
					Main.PlaySound(SoundID.Item18, Main.projectile[proj].Center);
					IdgProjectile.Sync(proj);
				}
			}


		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float rotation = MathHelper.ToRadians(16);

			CaliburnTypeA.SpectralSummon(player,type,damage,player.Center);

			/*List<NPC> guys = new List<NPC>();

			for (int a = 0; a < Main.maxNPCs; a++)
			{
				NPC npchim = Main.npc[a];
				Projectile proj = new Projectile();
				proj.SetDefaults(ProjectileID.ChlorophyteBullet);

				if (npchim.active && !npchim.friendly && !npchim.dontTakeDamage && npchim.CanBeChasedBy(proj) && (npchim.Center-player.Center).Length()<400)
				{
					guys.Add(npchim);
				}

			}
			if (guys.Count > 0)
			{
				for (int i = 0; i < numberProjectiles; i++)
				{
					NPC theone = guys[Main.rand.Next(0, guys.Count)];

					Vector2 perturbedSpeed = new Vector2(Main.rand.NextFloat(2f, 7f) * (Main.rand.NextBool() ? 1f : -1f), Main.rand.NextFloat(1f, 4f)); // Watch out for dividing by 0 if there is only 1 projectile.
					perturbedSpeed *= Main.rand.NextFloat(0.7f, 2.2f);
					int proj = Projectile.NewProjectile((theone.Center.X- perturbedSpeed.X*((float)(8f)))+ ((perturbedSpeed.X > 0 ? -0.5f : 0.5f) *theone.width), (theone.Center.Y-64)- perturbedSpeed.Y*12f, perturbedSpeed.X, perturbedSpeed.Y, type, (int)((float)damage * 1f), 0, player.whoAmI);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].netUpdate = true;
					Main.PlaySound(SoundID.Item18, Main.projectile[proj].Center);
					IdgProjectile.Sync(proj);
				}
			}*/

			return false;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Moredamnage", "Damage improves by 25% per spirit defeated"));
			tooltips.Add(new TooltipLine(mod, "Moredamnage", "Damage increase: "+ (SGAWorld.downedCaliburnGuardians-1)*25+"%"));
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			if (SGAWorld.downedCaliburnGuardians > 1)
			add += (float)((SGAWorld.downedCaliburnGuardians - 1) * 0.25f);
		}

	}

	public class CaliburnTypeB : CaliburnTypeA
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			Tooltip.SetDefault("One of the lost swords\nSummons a spectral blade to home in on your mouse cursor for a few seconds or until it hits 3 times, then it returns to you.\nYou can only summon 1 Sword at a time and it does not hit while returning");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.crit = 0;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 40;
			item.useAnimation = 30;
			item.reuseDelay = 30;
			item.useStyle = 1;
			item.knockBack = 8;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("CaliburnHomingSword");
			item.shootSpeed = 20f;
			item.useTurn = false;
			item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			damage *= 2;
			return (player.ownedProjectileCounts[mod.ProjectileType("CaliburnHomingSword")] < 1);
		}


	}

	public class CaliburnHomingSwordTrue : CaliburnHomingSword
	{
		public override float maxspeed => 8f;
		public override float minspeed => 3f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.light = 1f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.extraUpdates = 2;
			projectile.penetrate = 1005;
			projectile.timeLeft = 400;
			projectile.tileCollide = false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			CaliburnTypeA.SpectralSummon(Main.player[projectile.owner], mod.ProjectileType("CaliburnSpectralBlade"), damage, projectile.Center);
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB"; }
		}
	}


		public class CaliburnHomingSword : ModProjectile
	{
		private float[] oldRot = new float[12];
		private Vector2[] oldPos = new Vector2[12];
		private int subdamage;
		public float appear = 1f;
		public float gtime = 0;
		public virtual float maxspeed => 6f;
		public virtual float minspeed => 3f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
		}


		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.light = 1f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.extraUpdates = 2;
			projectile.penetrate = 999;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			projectile.timeLeft = 600;
			projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB"; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{

				if (GetType() == typeof(CaliburnHomingSwordTrue))
				{
					lightColor = Main.hslToRgb(((gtime+ (k*0.5f)) / 15f) % 1f, 0.9f, 0.75f);
				}


				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, ((lightColor * alphaz) * (appear))*0.2f, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.None : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
				if (projectile.penetrate < 600)
				return false;
			return base.CanHitNPC(target);
		}

		public virtual void trailingeffect()
		{

			if (gtime<5f)
			gtime = Main.GlobalTime;

			Rectangle hitbox = new Rectangle((int)projectile.position.X - 10, (int)projectile.position.Y - 10, projectile.height + 10, projectile.width + 10);

			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
				oldPos[k] = oldPos[k - 1];

				if (Main.rand.Next(0, 10) == 1)
				{
					int typr = mod.DustType("TornadoDust");
					bool itt;
					itt = (GetType() == typeof(CaliburnHomingSwordTrue));
					if (itt)
					{
						typr = 124;
					}

					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, typr);
					Main.dust[dust].scale = 0.75f * appear;
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 normvel = projectile.velocity;
					normvel.Normalize(); normvel *= 16f;

					Main.dust[dust].velocity = (randomcircle / 1f) + (-normvel);
					Main.dust[dust].noGravity = true;
					if (itt)
					{
						Main.dust[dust].color = Main.hslToRgb((gtime / 15f)%1f, 0.75f, Main.rand.NextFloat(0.45f,0.75f));
					}

				}

			}

		}

		public override void AI()
		{

			trailingeffect();

			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.ToRadians(45);

			oldRot[0] = projectile.rotation;
			oldPos[0] = projectile.Center;

			if (projectile.timeLeft < 100 || projectile.penetrate < 997) {
			projectile.timeLeft = 100;
			projectile.penetrate = 500;
			}
			projectile.velocity *= 0.99f;
			Player owner = Main.player[projectile.owner];
			if (Main.myPlayer == owner.whoAmI) {
				Vector2 diff = Main.MouseWorld - projectile.Center;
				if (projectile.penetrate < 997)
				diff = owner.Center - projectile.Center;
				diff.Normalize();
				projectile.velocity += diff*0.15f;
				projectile.netUpdate = true;
			}

			if (projectile.velocity.Length() < minspeed)
			{
				projectile.velocity.Normalize(); projectile.velocity *= minspeed;
			}

			if (projectile.velocity.Length() > maxspeed)
			{
				projectile.velocity.Normalize(); projectile.velocity *= maxspeed;
			}

			if (projectile.penetrate < 600)
				appear = Math.Max(appear - 0.0025f, 0.0f);

			if ((projectile.penetrate < 600 && (owner.Center - projectile.Center).Length() < 32) || appear<0.1)
				projectile.Kill();

		}

	}


	public class CaliburnSpectralBlade : ModProjectile
	{
		private float[] oldRot = new float[3];
		private int subdamage;
		public float appear = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, (lightColor * alphaz)*Math.Min((float)projectile.timeLeft/30f,Math.Min(appear,1f)), oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.penetrate < 1000)
			return false;
			return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate = 50;
		}

		public override void AI()
		{
			if (projectile.penetrate < 1000)
			{
				if (projectile.timeLeft > 30)
					projectile.timeLeft = 30;
				projectile.velocity /= 1.15f;
			}


			Rectangle hitbox = new Rectangle((int)projectile.position.X - 10, (int)projectile.position.Y - 10, projectile.height + 10, projectile.width + 10);

			appear += 0.15f;
			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];

				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 124);
				Main.dust[dust].scale = 0.5f;
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Vector2 normvel = projectile.velocity;
				normvel.Normalize(); normvel *= 7f;


				if ((projectile.timeLeft) < 31)
				{
					Main.dust[dust].scale *= ((float)projectile.timeLeft / 30f);
					normvel *= ((float)projectile.timeLeft / 30f);
				}

				Main.dust[dust].velocity = (randomcircle / 1f) + (-normvel);
				Main.dust[dust].noGravity = true;

			}
			oldRot[0] = projectile.rotation;

			projectile.rotation += projectile.velocity.X*0.1f;
			projectile.velocity=projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.velocity.X/3f));

		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.penetrate = 1000;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown=-1;
			projectile.timeLeft = 150;
			projectile.tileCollide = false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeA"; }
		}

	}


		public class CaliburnTypeC : CaliburnTypeA
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn");
			Tooltip.SetDefault("One of the lost swords\nFlings crystal shards from the blade.");
			Item.staff[item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			item.damage = 25;
			item.crit = 15;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 3;
			item.useAnimation = 21;
			item.reuseDelay = 30;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = Item.buyPrice(0, 5, 0, 0);
			item.rare = 2;
	        item.UseSound = SoundID.Item1;
	        item.shoot=mod.ProjectileType("SurtCharging");
	        item.shootSpeed=20f;
			item.useTurn = false;
	     	item.autoReuse = true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

				float speed = 1.5f;
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(16);
				//Main.PlaySound(SoundID.Item, player.Center,45);

			float speedvel = new Vector2(speedX, speedY).Length();

			Vector2 eree = player.itemRotation.ToRotationVector2();
			eree *= player.direction;
			speedX = eree.X* speedvel;
			speedY = eree.Y* speedvel;

			position += eree * 45f;

			for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				perturbedSpeed.RotatedBy(MathHelper.ToRadians(-45));
				perturbedSpeed *= Main.rand.NextFloat(0.8f, 1.2f);
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CrystalShard, (int)((float)damage * 0.4f), 0, player.whoAmI);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].hostile = false;
					Main.projectile[proj].friendly = true;
					Main.projectile[proj].netUpdate = true;
					Main.projectile[proj].timeLeft = 180;
					Main.projectile[proj].localAI[0] = 1f;
					Main.projectile[proj].velocity = perturbedSpeed;
					IdgProjectile.Sync(proj);
				}

			return false;
		}


	public override void MeleeEffects(Player player, Rectangle hitbox)
	{

		for (int num475 = 0; num475 < 3; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.PinkCrystalShard);
		Main.dust[dust].scale=0.5f+(((float)num475)/3.5f);
		Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
		Main.dust[dust].velocity=(randomcircle/2f)+((player.itemRotation.ToRotationVector2()*5f).RotatedBy(MathHelper.ToRadians(-90)));
		Main.dust[dust].noGravity=true;
		}

		Lighting.AddLight(player.position, 0.9f, 0.1f, 0.5f);
	}
	
	


	}


	public class CorrodedShield : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corroded Shield");
			Tooltip.SetDefault("'A treasure belonging to a former adventurer you'd rather not use but it looks useful'\nAllows you to block 25% of damage from the source by pointing the shield in the general direction\nAttack with the shield to bash-dash, gaining IFrames and hit enemies are Acid Burned\nCan only hit 5 targets, bash-dash ends prematural after the 5th\nCan be held out like a torch and used normally by holding shift");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.crit = 15;
			item.melee = true;
			item.width = 54;
			item.height = 32;
			item.useTime = 70;
			item.useAnimation = 60;
			item.reuseDelay = 80;
			item.useStyle = 1;
			item.knockBack = 5;
			item.noUseGraphic = true;
			Item.sellPrice(0, 5, 0, 0);
			item.rare = 3;
			item.UseSound = SoundID.Item7;
			item.shoot = 10;
			item.shootSpeed = 10f;
			item.useTurn = false;
			item.autoReuse = false;
			item.expert = true;
			item.noMelee = true;
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			glowstick = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[mod.ProjectileType("CorrodedShieldProjDash")] < 1;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = mod.ProjectileType("CorrodedShieldProjDash");
			return true;
		}


	}

	public class CorrodedShieldProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CorrodedShieldProj");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 10;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];
			bool heldone = player.HeldItem.type != mod.ItemType("CorrodedShield") && player.HeldItem.type != mod.ItemType("CapShield");
			if (projectile.ai[0] > 0 || (player.HeldItem == null || heldone) || player.dead || player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] > 0)
			{
				projectile.Kill();
			}
			else
			{
				if (projectile.timeLeft < 3)
					projectile.timeLeft = 3;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					projectile.velocity = diff;
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);

				Vector2 direction = (projectile.velocity);
				Vector2 directionmeasure = direction;

				player.heldProj = projectile.whoAmI;

				projectile.velocity.Normalize();

				player.bodyFrame.Y = player.bodyFrame.Height * 3;
				if (directionmeasure.Y - Math.Abs(directionmeasure.X) > 25)
					player.bodyFrame.Y = player.bodyFrame.Height * 4;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -25)
					player.bodyFrame.Y = player.bodyFrame.Height * 2;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -160)
					player.bodyFrame.Y = player.bodyFrame.Height * 5;
				player.direction = (directionmeasure.X > 0).ToDirectionInt();

				projectile.Center = player.Center + (projectile.velocity * 10f);
				projectile.velocity *= 8f;

			}
		}
		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor*projectile.Opacity, projectile.velocity.ToRotation() + (facingleft ? MathHelper.ToRadians(0) : MathHelper.ToRadians(180)), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);
			return false;
		}

	}

	public class CorrodedShieldProjDash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CorrodedShieldProj");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.hostile = false;
			projectile.penetrate = 5;
			projectile.light = 0.5f;
			projectile.width = 64;
			projectile.height = 64;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("AcidBurn"), (int)(60 * 1.50));
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];

			bool heldone= player.HeldItem.type != mod.ItemType("CorrodedShield");
			if (projectile.ai[0] > 0 || (player.HeldItem == null || heldone) || player.dead)
			{
				projectile.Kill();
			}
			else
			{
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.ai[1] < 1)
				{
				int dir = projectile.direction;
				player.ChangeDir(dir);
					player.velocity = projectile.velocity;
					player.velocity.Y /= 2f;
					player.immune = true;
					player.immuneTime = 30;
					player.GetModPlayer<SGAPlayer>().realIFrames = 30;

				}
				player.velocity.X = projectile.velocity.X;
				projectile.Center = player.Center;



				projectile.ai[1] += 1;

			}
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB"; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}

	}

	public class CapShield : CorrodedShield
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Captain America's Shield");
			Tooltip.SetDefault("Functions similarly to Corroded Shield, however:\nCharge up to enable a powerful dash!\nThis dash may be cancelled early by unequiping the shield\nAlt Fire lets you throw the shield, which will bounce between nearby enemies\nYou cannot use your shield while it is thrown, gains +1 bounces per 30 defense\nAllows you to block 50% of damage from the source by pointing the shield in the general direction\n'Stars and Stripes!'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.crit = 25;
			item.width = 54;
			item.height = 32;
			item.useTime = 70;
			item.melee = true;
			item.useAnimation = 60;
			item.reuseDelay = 80;
			item.useStyle = 1;
			item.knockBack = 5;
			item.noUseGraphic = true;
			Item.sellPrice(0, 5, 0, 0);
			item.rare = 9;
			item.UseSound = SoundID.Item7;
			item.shoot = 10;
			item.shootSpeed = 20f;
			item.useTurn = false;
			item.autoReuse = false;
			item.expert = true;
			item.channel = true;
			item.noMelee = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CorrodedShield"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddRecipeGroup("Fragment", 15);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddIngredient(ItemID.RedDye, 1);
			recipe.AddIngredient(ItemID.SilverDye, 1);
			recipe.AddIngredient(ItemID.BlueDye, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.channel = false;
			}
			else
			{
				item.channel = true;
			}
				return player.ownedProjectileCounts[mod.ProjectileType("CapShieldProjDash")] < 1 && player.ownedProjectileCounts[mod.ProjectileType("CapShieldToss")] < 1;
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = mod.ProjectileType("CapShieldProjDash");
			if (player.altFunctionUse == 2)
			{
				player.itemAnimation /= 3;
				player.itemTime /= 3;
				damage = (int)(damage);
				type = mod.ProjectileType("CapShieldToss");
				int thisoned = Projectile.NewProjectile(position.X, position.Y, speedX*player.thrownVelocity, speedY * player.thrownVelocity, type, damage, knockBack, Main.myPlayer);
				Main.projectile[thisoned].thrown = true;
				Main.projectile[thisoned].melee = false;
				Main.projectile[thisoned].netUpdate = true;
				IdgProjectile.Sync(thisoned);
				return false;
			}
			return true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
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
				valuez.Insert(1, "Melee/Throwing ");
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
				int thecrit = Main.GlobalTime % 3f >= 1.5f ? Main.LocalPlayer.meleeCrit : Main.LocalPlayer.thrownCrit;
				string thecrittype = Main.GlobalTime % 3f >= 1.5f ? "Melee " : "Throwing ";
				valuez.Insert(0, thecrit + "% " + thecrittype);
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add -= player.meleeDamage;
			add += ((player.thrownDamage*1.5f) + player.meleeDamage)/2f;
		}


	}

	public class CapShieldProj : CorrodedShieldProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CapShieldProj");
		}

	}

	public class CapShieldProjDash : CorrodedShieldProjDash
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("CapShieldProjDash");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 30;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0.5f;
			projectile.width = 64;
			projectile.height = 64;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override bool CanDamage()
		{
			return projectile.ai[1]>0;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.AddBuff(mod.BuffType("AcidBurn"), (int)(60 * 1.50));
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];
			if ((player.HeldItem == null || player.HeldItem.type != mod.ItemType("CapShield")) || player.dead)
			{
				projectile.Kill();
			}
			else
			{

				if (player.channel)
				{
					if (projectile.ai[0] < 150)
					{
						projectile.ai[0] += 1;
					}
					projectile.timeLeft += 1;
					for (float new1 = 0; new1 < 360; new1 = new1 + 360/10f)
					{
						if (projectile.ai[0] * (360f / 150f) >= new1) {
							float angle = new1;
							Vector2 angg = player.velocity.RotatedBy(MathHelper.ToRadians(angle))+(angle+MathHelper.ToRadians(angle)).ToRotationVector2()*10f;
							int DustID2 = Dust.NewDust(new Vector2(player.Center.X-8, player.Center.Y-8), 16, 16, DustID.AncientLight, 0, 0, 20, Color.Silver, 1f);
							Main.dust[DustID2].velocity = new Vector2(angg.X * 0.75f, angg.Y * 0.75f);
							Main.dust[DustID2].noGravity = true;
						}
						//projectile.position -= projectile.velocity;
						projectile.Center = player.Center;
					}
				}
				else
				{
					if (projectile.ai[1] == 0)
					{
						projectile.damage = (int)(projectile.damage * (1f + projectile.ai[0] / 20f));
						player.itemTime += (int)(projectile.ai[0] / 5f);
						player.itemAnimation += (int)(projectile.ai[0] / 5f);
						projectile.timeLeft += (int)(projectile.ai[0] / 3f);
					}
					if (projectile.ai[1] < (projectile.ai[0] / 5f) + 3)
					{
						if (projectile.ai[1] % 2 == 0)
						{
							Vector2 mousePos = Main.MouseWorld;

							if (projectile.owner == Main.myPlayer)
							{
								Vector2 diff = mousePos - player.Center;
								Vector2 velox = projectile.velocity;
								projectile.velocity = diff;
								projectile.velocity.Normalize();
								projectile.velocity *= velox.Length();
								projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
								projectile.netUpdate = true;
							}

							int dir = projectile.direction;
							player.ChangeDir(dir);
							player.velocity = projectile.velocity * (1 + (projectile.ai[0] / 120f));
							//player.velocity.Y = (float)Math.Pow(player.velocity.Y,0.80);
							//player.velocity.Y /= 2f;
							player.immune = true;
							player.immuneTime = 30;
							player.GetModPlayer<SGAPlayer>().realIFrames = 30;
						}
					}
					else
					{
						projectile.velocity.Y *= 0.98f;
					}
					player.maxFallSpeed *= 5f;
					player.velocity.X = projectile.velocity.X*(1f + (projectile.ai[0] / 120f));
					//player.velocity.Y = projectile.velocity.Y;

					for (float jj = 2; jj < 14; jj += 2)
					{
						for (float new1 = -1f; new1 < 2f; new1 = new1 + 2f)
						{
							float angle = 90;
							Vector2 velo = player.velocity;
							velo.Normalize();
							Vector2 angg = velo.RotatedBy(angle * new1);
							int DustID2 = Dust.NewDust(new Vector2(projectile.Center.X-8, projectile.Center.Y-8), 16, 16, DustID.AncientLight, 0, 0, 20, jj< 5 ? Color.White : new1 < 0 ? Color.Red : Color.Blue, 1f+(14f-jj)/14);
							Main.dust[DustID2].velocity = new Vector2(angg.X * jj, angg.Y * jj);
							Main.dust[DustID2].noGravity = true;
						}
					}

					projectile.Center = player.Center+projectile.velocity;



					projectile.ai[1] += 1;

				}

			}
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CaliburnTypeB"; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}

	}

	public class CapShieldToss : ModProjectile
	{

		List<int> bouncetargets = new List<int>();
		float hittime = 200f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("MERICA!");
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
			projectile.thrown = true;
			projectile.timeLeft = 3;
			projectile.penetrate = 20;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 20;
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
			hittime = 150f;
			projectile.ai[1] = FindClosestTarget(projectile.Center, new Vector2(0f, 0f));
			if (projectile.ai[0]>30)
			projectile.ai[0] -= 30;
			//IdgNPC.AddBuffBypass(target.whoAmI,mod.BuffType("InfinityWarStormbreaker"),300);
		}

		public int FindClosestTarget(Vector2 loc, Vector2 size)
		{
			float num170 = 1000000;
			NPC num171 = null;

			for (int num1722 = 0; num1722 < Main.maxNPCs; num1722 += 1)
			{
				int num172 = num1722;
				if (Main.npc[num172].active && !Main.npc[num172].friendly && !Main.npc[num172].townNPC && Main.npc[num172].CanBeChasedBy() && projectile.localNPCImmunity[num1722]<1)
				{
					float num173 = Main.npc[num172].position.X + (float)(Main.npc[num172].width / 2);
					float num174 = Main.npc[num172].position.Y + (float)(Main.npc[num172].height / 2);
					float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
					if (Main.npc[num172].active)
					{

						//(Collision.CanHit(new Vector2(loc.X, loc.Y), 1, 1, Main.npc[num172].position, Main.npc[num172].width, Main.npc[num172].height) || block == false)
						if (num175 < num170)
						{
							int result = 0;
							result = bouncetargets.Find(x => x == num172);
							if (result < 1)
							{
								num170 = num175;
								num171 = Main.npc[num172];
							}
						}
					}
				}
			}
			if (num170 > 900)
			{
				projectile.penetrate = 5;
				return -1;
			}

			return num171.whoAmI;

		}

		public override void AI()
		{

			Lighting.AddLight(projectile.Center, Color.Aquamarine.ToVector3() * 0.5f);

			hittime = Math.Max(1f, hittime / 1.5f);
			;
			float dist2 = 24f;

			//Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			for (double num315 = 0; num315 < Math.PI + 0.04; num315 = num315 + Math.PI)
			{
				Vector2 thisloc = new Vector2((float)(Math.Cos((projectile.rotation + Math.PI / 2.0) + num315) * dist2), (float)(Math.Sin((projectile.rotation + Math.PI / 2.0) + num315) * dist2));
				int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y) + thisloc, projectile.width, projectile.height, DustID.AncientLight, 0f, 0f, 50, num315 < 0.01 ? Color.Blue : Color.Red, 1.5f);
				Main.dust[num316].noGravity = true;
				Main.dust[num316].velocity = thisloc / 30f;
			}

			projectile.ai[0] = projectile.ai[0] + 1;
			if (projectile.ai[0] == 1)
			{
				projectile.penetrate += (int)((float)Main.player[projectile.owner].statDefense/30f);
				projectile.ai[1] = FindClosestTarget(projectile.Center, new Vector2(0f, 0f));

			}
			projectile.velocity.Y += 0.1f;
			if ((projectile.ai[0] > 90f || (projectile.penetrate<10 && projectile.ai[0]>20)) && !Main.player[projectile.owner].dead)
			{
				Vector2 dist = (Main.player[projectile.owner].Center - projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();
				projectile.velocity += distnorm * (5f+ ((float)projectile.timeLeft/40f));
				projectile.velocity /= 1.25f;
				//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));
				if (dist.Length() < 80)
					projectile.Kill();

				projectile.timeLeft += 1;
			}
			projectile.timeLeft += 1;


			if (projectile.ai[1] > -1)
			{
				NPC target = Main.npc[(int)projectile.ai[1]];

				if (target != null && projectile.penetrate > 9)
				{
				projectile.Center += (projectile.DirectionTo(target.Center) * (projectile.ai[0] > 8f ? (50f * Main.player[projectile.owner].thrownVelocity)/hittime : 12f));
				}
			}

			projectile.rotation += 0.38f;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Caliburn/CapShield"; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Items/Weapons/Caliburn/CapShield");
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}


	}


}