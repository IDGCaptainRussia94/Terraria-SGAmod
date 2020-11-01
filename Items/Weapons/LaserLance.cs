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
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	public class LaserLance : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Lance");
			Tooltip.SetDefault("Spin a Space Lance around yourself to damage enemies\nwhen it hits an enemy it shoots a piercing laser in their direction, doing half the damage");
		}
		
		public override void SetDefaults()
		{
			item.damage = 18;
			item.crit = 0;
			item.melee = true;
			item.width = 34;    
            item.height = 24;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 1;
			item.value = 100000;
			item.rare = 4;
	        item.shootSpeed = 8f;
			item.noUseGraphic = true;
            item.noMelee = true; 
			item.shoot = mod.ProjectileType("LaserLanceProjectile");
            item.shootSpeed = 7f;
			item.UseSound = SoundID.Item1;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class LaserLanceProjectile : ModProjectile
	{
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Lance");
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.ai[0] > 5f)
			{
				Vector2 oldPos = projectile.Center + new Vector2(0, -60);
				oldPos = oldPos.RotatedBy(projectile.rotation);
				oldPos.Normalize();
				oldPos *= 48f;

					projHitbox.X = (int)(projectile.Center.X + oldPos.X-25);
					projHitbox.Y = (int)(projectile.Center.Y + oldPos.Y-25);
					projHitbox.Width = 50;
					projHitbox.Width = 50;
				if (projHitbox.Intersects(targetHitbox))
					{
						return true;
					}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{

			Vector2 oldPos = projectile.Center + new Vector2(0, -60);
			oldPos = oldPos.RotatedBy(projectile.rotation);
			oldPos.Normalize();
			oldPos *= 96f;

			Vector2 torot = oldPos;
			torot.Normalize();

			int thisoned = Projectile.NewProjectile(projectile.Center+oldPos, torot*8f, ProjectileID.ScutlixLaser, (int)(projectile.damage / 2), knockback, Main.myPlayer);
			//Main.projectile[thisoned].usesLocalNPCImmunity = true;
			//Main.projectile[thisoned].localNPCHitCooldown = -1;
			Main.projectile[thisoned].penetrate = 2;
			Main.projectile[thisoned].knockBack = 0f;
			IdgProjectile.Sync(thisoned);

			Main.PlaySound(SoundID.Item91, projectile.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/LaserLance");
			Vector2 drawOrigin = new Vector2(tex.Width / 2f, tex.Height+16);

			int len = Math.Min(oldRot.Length,1+(int)projectile.ai[0]);

			//oldPos.Length - 1
			for (int k = (len - 1); k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(len + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction<0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
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

					projectile.Kill();
					return;
			}


			if (projectile.ai[1] < 1) {
				projectile.ai[0] = Math.Min(projectile.ai[0] + 0.1f, 16f);
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
					basep.direction= Main.MouseWorld.X > basep.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				basep.heldProj = projectile.whoAmI;

				projectile.position -= projectile.velocity;
				//basep.bodyFrame.Y = basep.bodyFrame.Height * (2 + (((int)(projectile.localAI[0] / 40)) % 3));
				basep.itemRotation = ((projectile.rotation + MathHelper.ToRadians(-45)).ToRotationVector2()).ToRotation();
				projectile.Center = basep.Center;
			}

			projectile.localAI[1] += (1+projectile.ai[0]/8);
			projectile.localAI[0] += (projectile.ai[0] + 5f);
			if (projectile.localAI[1] > 60)
			{
				projectile.localAI[1] = 0;
				Main.PlaySound(SoundID.Item,(int)projectile.Center.X, (int)projectile.Center.Y, 15,1f, projectile.ai[0]/30f);
			}
			projectile.rotation = MathHelper.ToRadians(projectile.localAI[0] * projectile.direction);
			//projectile.damage = (int)(subdamage * (1+(projectile.ai[0] / 15f)));

		}


	}

}