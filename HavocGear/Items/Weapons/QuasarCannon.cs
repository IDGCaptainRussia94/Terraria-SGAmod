using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons.SeriousSam;
using Idglibrary;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class QuasarCannon : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quasar Cannon");
			Tooltip.SetDefault("Launches a ball of energy that explodes nearby enemies on hit\nHold fire to charge up the shot");
		}

		public override void SetDefaults()
		{
			item.damage = 200;
			item.magic = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.sellPrice(0,75,0,0);
			item.rare = 11;
			item.mana = 10;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<QuasarCannonCharging>();
			item.shootSpeed = 0f;
			item.channel=true;
			item.reuseDelay = 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 12);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 8);
			recipe.AddIngredient(ItemID.FragmentVortex, 6);
			recipe.AddIngredient(ItemID.FragmentNebula, 5);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 8);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 2);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[type]<1){
			int proj=Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
		}
			return false;
		}

	}

	public class QuasarCannonCharging : ModProjectile
	{

		public static int chargeuptime=300;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam Charging");
		}

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile=false;
			projectile.friendly=true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
		Player player=Main.player[projectile.owner];

		if (player==null || player.dead)
		projectile.Kill();

		projectile.timeLeft=4;
		player.itemTime=6;
		player.itemAnimation=6;

			bool channeling = ((player.channel || projectile.ai[0] < 5) && !player.noItems && !player.CCed);
			if (Main.netMode == NetmodeID.MultiplayerClient || Main.netMode==NetmodeID.SinglePlayer)
			{
				Vector2 direction = (Main.MouseWorld - player.Center);
				direction.Normalize();
				projectile.velocity = direction;
				projectile.netUpdate = true;
			}
			projectile.Center = player.Center + Vector2.Normalize(projectile.velocity) * 72;
			if (player.statMana >= 10)
			projectile.ai[0] += 1;

			Vector2 directionmeasure = projectile.velocity;

			int num315;

			if (channeling)
			{
				player.ChangeDir((directionmeasure.X > 0).ToDirectionInt());
				player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * player.direction, projectile.velocity.X * player.direction);
			}

		if (projectile.ai[0]>10 && player.CheckMana(10,false,true))
			{

				if (projectile.ai[0]%20==0)
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 94, 0.40f, -0.5f+(projectile.ai[0]/(float)chargeuptime)*1.25f);

				if (projectile.ai[0]<chargeuptime)
				{

					if (projectile.ai[0] % 20 == 0)
						player.CheckMana(8,true,false);

		for (num315 = 0; num315 < 2; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-64,0),Main.rand.Next(-20,6)*player.direction); randomcircle =randomcircle.RotatedBy(directionmeasure.ToRotation());
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X-1,projectile.Center.Y)+randomcircle, 0, 0, 173, 0f, 0f, 100, default(Color), 1f+projectile.ai[0]/300f);

					Main.dust[num622].scale = 1f;
					Main.dust[num622].noGravity=true;
						//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
						Vector2 velxx = projectile.velocity;
						velxx.Normalize(); velxx *= -1f;
						velxx.RotatedByRandom(MathHelper.ToRadians(40));
					Main.dust[num622].velocity.X = velxx.X;
					Main.dust[num622].velocity.Y = velxx.Y;
					Main.dust[num622].velocity *= Main.rand.NextFloat(1f, 2f);
					Main.dust[num622].alpha = 150;
			}
			}else{
		for (num315 = 0; num315 < 2; num315 = num315 + 1)
			{
						Vector2 randomcircle2 = new Vector2(Main.rand.Next(-64, 16), Main.rand.Next(-26, 12)*player.direction); randomcircle2=randomcircle2.RotatedBy(directionmeasure.ToRotation());
						Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X-1,projectile.Center.Y)+ randomcircle2, 0, 0, 173, 0f, 0f, 173, default(Color), 2f);

					Main.dust[num622].scale = 1.5f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = (randomcircle.X + projectile.velocity.X)*2;
					Main.dust[num622].velocity.Y = (randomcircle.Y + projectile.velocity.Y)*2;
					Main.dust[num622].alpha = 100;
			}
			}
		}

		if (projectile.ai[0]==chargeuptime){
		for (num315 = 0; num315 < 60; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X-1,projectile.Center.Y), 0, 0, 173, 0f, 0f, 100, default(Color), 2.5f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X*16f;
					Main.dust[num622].velocity.Y = randomcircle.Y*16f;
					Main.dust[num622].alpha = 200;
			}
		}

		if (!channeling){

				float speed=5f;
				Vector2 perturbedSpeed = (projectile.velocity * speed); // Watch out for dividing by 0 if there is only 1 projectile.

				if (projectile.ai[0] > 30)
				{
					int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("QuasarCannonShot"), projectile.damage, projectile.knockBack, player.whoAmI);
					Main.projectile[proj].penetrate = 1;
					Main.projectile[proj].ai[0] = projectile.ai[0];
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wave_Beam_Charge_Shot").WithVolume(.7f).WithPitchVariance(.25f), projectile.Center);
				}

		projectile.Kill();
		}

	}

}

	public class QuasarCannonShot : ModProjectile
	{
		private float scale = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Beamshot");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override string Texture => "SGAmod/Projectiles/ChargedWave";

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (projectile.localAI[0] < 9999)
				return false;
			Vector2 drawOrigin = Main.projectileTexture[projectile.type].Size() / 2f;
			Color alphacol = projectile.GetAlpha(lightColor);

				for (int k = projectile.oldPos.Length - 1; k > 0; k -= 1)
				{
					float sizer = (projectile.scale - ((float)k/(float)projectile.oldPos.Length)*0.15f)* scale;
					if (sizer > 0)
					{
						Vector2 drawPos = projectile.oldPos[k]+ (projectile.Hitbox.Size()/2f) - Main.screenPosition;
						Color color = alphacol * ((float)(projectile.oldPos.Length - k) / projectile.oldPos.Length);
						spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color*((float)k/(float)projectile.oldPos.Length), projectile.rotation, drawOrigin, sizer, SpriteEffects.None, 0f);
					}
				}
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center-Main.screenPosition, null, alphacol, projectile.rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.magic = true;
			projectile.timeLeft = 1000;
			projectile.extraUpdates = 3;
			aiType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 30+(int)(projectile.ai[0]/10f); num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, default(Color), 3.4f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f+ (projectile.ai[0] / 300f);
				dust3.noGravity = true;
				dust3.scale = 2.0f + (projectile.ai[0] / 300f);
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}

			for (int zz = 0; zz < Main.maxNPCs; zz += 1)
			{
				NPC npc = Main.npc[zz];
				if (!npc.dontTakeDamage && !npc.townNPC && npc.active && npc.life > 0)
				{
					if (npc.Distance(projectile.Center) < 32+ projectile.ai[0])
					{
						Main.PlaySound(SoundID.Item, (int)npc.Center.X, (int)npc.Center.Y, 92, 0.75f, -0.5f);
						Projectile.NewProjectile(npc.Center, Vector2.Zero, mod.ProjectileType("QuaserBoom"), projectile.damage, projectile.knockBack, projectile.owner);
					}
				}
			}

			return true;
		}

		public override bool PreAI()
		{
			if (projectile.localAI[0] < 10000)
			{
				projectile.localAI[0] = 10000;
				projectile.damage = (int)(projectile.damage*(1f + (projectile.ai[0] / 150f)));

			}
			scale = 0.5f + (projectile.ai[0] / ((float)QuasarCannonCharging.chargeuptime))*0.5f;
			return true;
		}

		public override void AI()
		{
			for (float num315 = 0; num315 < projectile.ai[0]/100f; num315 = num315 + 1f)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 173, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 0.7f+(projectile.ai[0]/800f));
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
				dust3.noGravity = true;
				dust3.scale = 1.8f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

		}

	}

	public class QuaserBoom : ModProjectile
	{
		float ranspin = 0;
		float ranspin2 = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blast");
		}

		public void getstuff()
		{

			if (ranspin2 == 0)
			{
				for (float num315 = 5f; num315 < 12f; num315 = num315 + 0.25f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int num316 = Dust.NewDust(new Vector2(projectile.Center.X-16, projectile.Center.Y-16), 32,32, 173, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 0.8f);
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= randomcircle* num315;
					dust3.noGravity = true;
					dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}

				ranspin2 = Main.rand.NextFloat(-0.1f, 0.1f);
				ranspin = Main.rand.NextFloat((float)Math.PI * 2f);
			}
			else
			{
				ranspin += ranspin2;

			}
		}

		public override void SetDefaults()
		{
			projectile.width = 128;
			projectile.height = 128;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
			projectile.localNPCHitCooldown = -1;
			projectile.usesLocalNPCImmunity = true;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+ ProjectileID.MagnetSphereBall); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			getstuff();
			Texture2D tex = Main.projectileTexture[projectile.type];
			float timeleft = ((float)projectile.timeLeft / 60f);
			Vector2 drawPos = ((projectile.Center - Main.screenPosition));
			Color color = Color.Violet * Math.Min(0.75f,timeleft*3);

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 5) / 2f;
			int timing = (int)(projectile.localAI[0] / 3f);
			timing %= 5;
			timing *= ((tex.Height) / 5);

			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), color, ranspin, drawOrigin, Math.Min(3f,(1f - timeleft) * 12f), SpriteEffects.None, 0f);
			return false;
		}


		public override void AI()
		{
			projectile.localAI[0] += 1f;

			if (projectile.ai[0] < 1)
			{
				projectile.ai[0] = 1;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
				for (float num315 = 0; num315 < 40; num315 += 2f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int dustz = 173;
					int num316 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + (randomcircle * (80f - num315) / 3f), 0, 0, dustz, 0, 0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), (float)num315 * 0.1f);
					Main.dust[num316].noGravity = true;
					randomcircle *= (float)Math.Pow((80 - num315) / 5f, 0.75);
					Main.dust[num316].velocity = randomcircle*(2f+ num315/20f);
				}
			}

			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.01f) / 255f, ((255 - projectile.alpha) * 0.025f) / 255f, ((255 - projectile.alpha) * 0.25f) / 255f);
			return;
		}
	}

}
