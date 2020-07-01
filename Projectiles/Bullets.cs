using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Dusts;

namespace SGAmod.Projectiles
{

	public class BlazeBullet : ModProjectile
	{

		double keepspeed = 0.0;
		float homing = 0.03f;
		public Player P;
		private Vector2[] oldPos = new Vector2[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Bullet");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.extraUpdates = 5;
			aiType = ProjectileID.Bullet;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			GameShaders.Armor.GetShaderFromItemId(ItemID.SolarDye).Apply(null);

			Texture2D tex = ModContent.GetTexture("Terraria/Projectile_"+ 51);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length+1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2))*(k!= oldPos.Length - 1 ? 0.5f : 1f);
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			effects(1);
			projectile.type = ProjectileID.Bullet;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0,10)==1)
			target.AddBuff(mod.BuffType("ThermalBlaze"), 60*3);
		}

		public virtual void effects(int type)
		{
			if (type == 1)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 5; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y) + positiondust, projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X + (float)(Main.rand.Next(-50, 50) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-50, 50) / 15f), 50, Main.hslToRgb(0.83f, 0.5f, 0.25f), 0.25f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}

		public override void AI()
		{

			if (Main.rand.Next(0, 8) == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"));
				Main.dust[dust].scale = 0.4f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			projectile.position -= projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = projectile.Center;

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}


	
	}


	public class AcidBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Round");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.timeLeft = 300;
			projectile.extraUpdates = 5;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.CursedBullet;
			return true;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("Terraria/Projectile_" + 51);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 4) == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"));
				Main.dust[dust].scale = 0.5f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			projectile.position -= projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = projectile.Center;

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(mod.BuffType("AcidBurn"), 30 * (Main.rand.Next(0, 3)==1 ? 2 : 1));
		}



	}

	public class AimBotBullet : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aimbot Bullet");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.extraUpdates = 400;
			projectile.penetrate = 3;
			projectile.timeLeft = 400;
			projectile.aiStyle = -1;
			projectile.localNPCHitCooldown = -1;
			projectile.usesLocalNPCImmunity = true;
		}

		public override void AI()
		{

			if (projectile.ai[0] < 1)
			{
				int dir = 1;
				if (Main.myPlayer == projectile.owner)
				{
					NPC target = Main.npc[Idglib.FindClosestTarget(0, Main.MouseWorld, new Vector2(0f, 0f), true, true, true, projectile)];
					if (target != null && Vector2.Distance(target.Center, projectile.Center) < 2000)
					{
						Vector2 projvel=projectile.velocity;
						projectile.velocity = target.Center - projectile.Center;
						projectile.velocity.Normalize(); projectile.velocity *= 8f;
						IdgProjectile.Sync(projectile.whoAmI);
						projectile.netUpdate = true;
					}


				}
				dir = Math.Sign(projectile.velocity.X);
				Main.player[projectile.owner].ChangeDir(dir);
				Main.player[projectile.owner].itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
				//Main.player[projectile.owner].itemRotation = projectile.velocity.ToRotation() * Main.player[projectile.owner].direction;

			}
			projectile.ai[0] += 1;

			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1f);
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.damage = (int)(projectile.damage * 1.20f);
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + ProjectileID.MoonlordBullet); }
		}

	}

	public class TungstenBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tungsten Bullet");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Bullet);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			aiType = ProjectileID.Bullet;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

	}

	public class PortalBullet : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 8;


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spawner");
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + 658; }
		}

		public override void SetDefaults()
		{
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.timeLeft = 24;
			projectile.tileCollide = false;
			aiType = -1;
		}

		public override void Explode()
		{

			if (projectile.timeLeft == openclosetime && projectile.ai[0] > 0)
			{
				Player owner = Main.player[projectile.owner];
				if (owner != null && !owner.dead)
				{

					NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
					if (target != null && Vector2.Distance(target.Center,projectile.Center) < 1500)
					{

						Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 67, 0.25f, 0.5f);

						Vector2 gotohere = new Vector2();
						gotohere = target.Center-projectile.Center;//Main.MouseScreen - projectile.Center;
						gotohere.Normalize();

						Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(10)) * projectile.velocity.Length();
						int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.BulletHighVelocity, projectile.damage, projectile.knockBack / 8f, owner.whoAmI);
						Main.projectile[proj].timeLeft = 180;
						//Main.projectile[proj].penetrate = 1;
						Main.projectile[proj].GetGlobalProjectile<SGAprojectile>().onehit = true; ;
						Main.projectile[proj].netUpdate = true;
						IdgProjectile.Sync(proj);

					}
				}

			}

		}

		public override void AI()
		{
			if (projectile.ai[1] < 100)
			{
				projectile.ai[1] = 100;
				projectile.ai[0] = ProjectileID.BulletHighVelocity;
				Player owner = Main.player[projectile.owner];
				if (owner!=null && Main.myPlayer == owner.whoAmI)
				{
					projectile.Center = Main.MouseWorld;
					projectile.direction = Main.MouseWorld.X > owner.position.X ? 1 : -1;
				}
			}
			base.AI();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			if (scale > 0)
			{
				Texture2D texture = ModContent.GetTexture("Terraria/Projectile_" + 641);
				Texture2D outer = ModContent.GetTexture("Terraria/Projectile_" + 657);
				spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(outer, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, -projectile.rotation, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;


				outer = ModContent.GetTexture("Terraria/Projectile_" + 644);
				spriteBatch.Draw(outer, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f)*0.5f, projectile.rotation*2, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
				spriteBatch.Draw(outer, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f)*0.5f, -projectile.rotation*2, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;


			}
			return false;
		}

	}



}