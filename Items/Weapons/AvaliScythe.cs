using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Items.Weapons;
using SGAmod.HavocGear.Items.Weapons;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	public class AvaliScythe : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avali Scythe");
			Tooltip.SetDefault("Spins a duo-scythe around the player that speeds up over time" +
				"\nAfter holding for a short while, release to throw the weapon\nWhen thrown, Does up to double the damage and deals throwing damage" + "\n'The weapon of the fallen dragon'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.crit = 10;
			item.melee = true;
			item.width = 34;
			item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 8;
			item.value = 100000;
			item.rare = 5;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("AvaliScytheProjectile");
			item.shootSpeed = 7f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 8);
			recipe.AddRecipeGroup("SGAmod:Tier4Bars", 6);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AvaliScytheProjectile : ModProjectile
	{
		protected float[] oldRot = new float[6];
		protected int subdamage;
		protected virtual float maxSpin => 16f;
		protected virtual float chargeUpRate => 0.2f;
		protected virtual float spinMulti => 1f;

		protected virtual float throwMulti => 1f;

		protected virtual int spinSoundDelay => 120;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avali Scythe");
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{

			Vector2[] oldPos = new Vector2[2];
			oldPos[0] = projectile.Center + new Vector2(48, 0);
			oldPos[0] = oldPos[0].RotatedBy(MathHelper.ToRadians(45));
			oldPos[0] = oldPos[0].RotatedBy(projectile.rotation);
			oldPos[0].Normalize();
			oldPos[0] *= 48f;
			oldPos[1] = oldPos[0].RotatedBy(MathHelper.ToRadians(180));

			foreach (Vector2 position in oldPos)
			{
				projHitbox.X = (int)(projectile.Center.X + position.X);
				projHitbox.Y = (int)(projectile.Center.Y + position.Y);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/AvaliScythe");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/AvaliScythe"; }
		}

		public override void AI()
		{
			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
			}
			oldRot[0] = projectile.rotation;

			Player basep = Main.player[projectile.owner];

			if (basep == null || basep.dead)
			{
				projectile.Kill();
				return;
			}
			if ((!basep.channel || basep.dead) && projectile.ai[1] < 1)
			{
				if (projectile.ai[0] < 10)
				{
					projectile.Kill();
					return;


				}
				else
				{
					projectile.ai[1] = 1;
					projectile.velocity *= projectile.ai[0] * throwMulti * basep.Throwing().thrownVelocity;
					projectile.timeLeft = 300;
					projectile.melee = false;
					projectile.Throwing().thrown = true;
					projectile.damage = (int)(projectile.damage * basep.Throwing().thrownDamage);
					basep.itemAnimation = 60;
					basep.itemTime = 60;
					Main.PlaySound(SoundID.Item71, basep.Center);
					projectile.damage = (int)(projectile.damage * (1 + (projectile.ai[0] / 15f)));
				}
			}


			if (projectile.ai[1] < 1)
			{
				subdamage = projectile.damage;
				projectile.ai[0] = Math.Min(projectile.ai[0] + chargeUpRate, maxSpin);
				basep.itemAnimation = 5;
				basep.itemTime = 5;
				projectile.timeLeft = 30;
			}

			if (projectile.ai[1] < 1)
			{
				if (projectile.owner == Main.myPlayer)
				{
					Vector2 mousePos = Main.MouseWorld;
					Vector2 diff = mousePos - basep.Center;
					diff.Normalize();
					projectile.velocity = diff;
					basep.direction = Main.MouseWorld.X > basep.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}

				basep.heldProj = projectile.whoAmI;

				projectile.position -= projectile.velocity;
				basep.bodyFrame.Y = basep.bodyFrame.Height * (2 + (((int)((projectile.localAI[0] * spinMulti) / 40)) % 3));
				projectile.Center = basep.Center;
			}
			projectile.localAI[0] += spinMulti * (projectile.ai[0] + 5f) * projectile.direction;
			projectile.localAI[1] += (1 + projectile.ai[0] / 8);
			if (projectile.localAI[1] > spinSoundDelay && GetType() == typeof(RedPhasebrandProjectile))
			{
				projectile.localAI[1] = 0;
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1f, projectile.ai[0] / 40f);
			}
			projectile.rotation = MathHelper.ToRadians(projectile.localAI[0]);

		}


	}

}

namespace SGAmod.HavocGear.Items.Weapons
{

	public class RedPhasebrand : AvaliScythe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Phasebrand");
			Tooltip.SetDefault("Spins a duo-phase Phasesaber around the player that speeds up over time\nShoots lasers from both sides on hit" +
				"\nAfter holding for a short while, release to throw the weapon\nWhen thrown, Does more damage based on charge and deals throwing damage" + "\n'The Darkside of close combat...'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 35;
			item.melee = true;
			item.width = 34;
			item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 8;
			item.value = 100000;
			item.rare = 5;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("RedPhasebrandProjectile");
			item.shootSpeed = 7f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AvaliScythe"), 1);
			recipe.AddIngredient(mod.ItemType("LaserLance"), 1);
			recipe.AddIngredient(ItemID.RedPhasesaber, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class RedPhasebrandProjectile : AvaliScytheProjectile, IDrawAdditive
	{
		protected override float maxSpin => 24f;
		protected override float chargeUpRate => 0.075f;
		protected override float spinMulti => 0.5f;
		protected override float throwMulti => 0.5f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Phasebrand");
		}
		public override string Texture
		{
			get { return "SGAmod/Items/GlowMasks/RedPhasebrand_Glow"; }
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.ai[0] < 10)
				return false;

			Vector2[] oldPos = new Vector2[2];
			oldPos[0] = projectile.Center + new Vector2(48, 0);
			oldPos[0] = oldPos[0].RotatedBy(MathHelper.ToRadians(45 + (projectile.direction > 0 ? 90 : 0)));
			oldPos[0] = oldPos[0].RotatedBy(projectile.rotation);
			oldPos[0].Normalize();
			oldPos[0] *= 48f;
			oldPos[1] = oldPos[0].RotatedBy(MathHelper.ToRadians(180));

			foreach (Vector2 position in oldPos)
			{
				projHitbox.X = (int)(projectile.Center.X + position.X);
				projHitbox.Y = (int)(projectile.Center.Y + position.Y);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{

			for (float u = 0; u < MathHelper.TwoPi; u += MathHelper.Pi)
			{
				Vector2 oldPos = new Vector2(1f, 0);
				oldPos = oldPos.RotatedBy(u + (projectile.rotation + (0f)));
				oldPos.Normalize();
				oldPos *= 24f;

				Vector2 torot = oldPos;
				torot.Normalize();

				int thisoned = Projectile.NewProjectile(projectile.Center + oldPos, torot * 8f, ProjectileID.ScutlixLaser, (int)(projectile.damage / 3), knockback, Main.myPlayer);
				//Main.projectile[thisoned].usesLocalNPCImmunity = true;
				//Main.projectile[thisoned].localNPCHitCooldown = -1;
				Main.projectile[thisoned].penetrate = 2;
				Main.projectile[thisoned].knockBack = 0f;
				Main.projectile[thisoned].usesIDStaticNPCImmunity = true;
				Main.projectile[thisoned].idStaticNPCHitCooldown = 30;
				Main.projectile[thisoned].netUpdate = true;
				IdgProjectile.Sync(thisoned);
			}

			Main.PlaySound(SoundID.Item91, projectile.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width / 2, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, tex.Width / 2, tex.Height), lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				//spriteBatch.Draw(tex, drawPos, new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height), Color.White * MathHelper.Clamp(projectile.ai[0] / 15f, 0f, 1f) * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width / 2, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				//spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, tex.Width / 2, tex.Height), lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				spriteBatch.Draw(tex, drawPos, new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height), Color.White * MathHelper.Clamp(projectile.ai[0] / 15f, 0f, 1f) * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
	}

}