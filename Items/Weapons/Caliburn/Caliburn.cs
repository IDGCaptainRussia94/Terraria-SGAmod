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
using AAAAUThrowing;
using Terraria.Utilities;

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

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {

			if (pre == -1)
				item.prefix = (byte)TrapPrefix.GetBustedPrefix;

			return true;
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
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CaliburnBlade1");
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * 0.25f;
				};
			}
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
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CaliburnBlade2");
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * 0.25f;
				};
			}
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
			ProjectileID.Sets.Homing[projectile.type] = true;
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
			ProjectileID.Sets.Homing[projectile.type] = true;
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
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CaliburnBlade3");
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * 0.25f;
				};
			}
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


}