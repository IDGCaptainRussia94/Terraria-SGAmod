using Microsoft.Xna.Framework;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Idglibrary;


namespace SGAmod.Items.Weapons.SeriousSam
{
	public class SBCCannon : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("SBC Cannon");
            Tooltip.SetDefault("Charge up piercing cannon balls that do a huge amount of damage\nBut lose power with each enemy they pass through, exploding when they run out of damage\nCharge longer for more speed and much more damage!\nCannonballs explode against enemies immune to knockback, and do not crit \nDamage is increased instead based on crit chance, and the explosion however can crit\nUses Lead Cannonballs as ammo\n'lets get Serious!'");
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SBCCannonHolding>()] > 0)
				return false;
			return true;
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			add += (float)player.rangedCrit / 100f;
		}

		public override void SetDefaults()
        {
            item.damage = 200;
            item.ranged = true;
            item.width = 48;
            item.height = 28;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 400000;
			item.rare = 6;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SBCCannonHolding>();
            item.shootSpeed = 1f;
			item.channel = true;
			item.noUseGraphic = true;
			item.useAmmo = ModContent.ItemType<LeadCannonball>();
		}

		/*public override bool CanUseItem(Player player)
		{
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			return (modply.RefilPlasma());
		}*/

		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cannon, 1);
			recipe.AddIngredient(ItemID.StarCannon, 1);
			recipe.AddIngredient(null, "WraithFragment4", 8);
			recipe.AddIngredient(null, "AdvancedPlating", 8);
			recipe.AddIngredient(null, "VirulentBar", 4);
			recipe.AddIngredient(ItemID.MeteoriteBar, 8);
			recipe.AddIngredient(ItemID.HallowedBar, 6);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
            recipe.SetResult(this);
      		recipe.AddRecipe();
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;


				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
				float scale = 1f;// - (Main.rand.NextFloat() * .2f);
				perturbedSpeed = perturbedSpeed * scale; 
				int prog=Projectile.NewProjectile(position.X+ offset.X, position.Y+ offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                IdgProjectile.Sync(prog);


			return false;
		}
	}

	public class SBCCannonHolding : ModProjectile
	{
		//public virtual float trans => 1f;
		public Player P;
		public virtual float chargeuprate => 3f;
		public virtual string soundcharge => "Sounds/Custom/Cannon/Prepare";
		public virtual string soundfire => "Sounds/Custom/Cannon/Fire";
		public virtual int cooldowntime => 90;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cannon");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
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
			projectile.ranged = true;
			projectile.timeLeft = 3;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/SeriousSam/SBCCannonProj"; }
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];

			if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				if (projectile.ai[0] > 3000 || player.dead)
				{
					projectile.Kill();
				}
				else
				{

					Vector2 mousePos = Main.MouseWorld;

					///Holding

					if (projectile.owner == Main.myPlayer)
					{
						Vector2 diff = mousePos - player.Center;
						diff.Normalize();
						if (player.channel && projectile.ai[0]< 200 && projectile.ai[1] < 1)
						{
							projectile.velocity = diff;
							projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						}
						projectile.netUpdate = true;
					}

					int dir = projectile.direction;
					player.ChangeDir(dir);
					projectile.direction = dir;

					player.heldProj = projectile.whoAmI;

					bool isholding = (player.channel && projectile.ai[0] < 200 && projectile.ai[1] < 1);

					if (isholding)
					{
						player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
						projectile.rotation = player.itemRotation - MathHelper.ToRadians(90);
						projectile.timeLeft = cooldowntime;
						projectile.ai[0] += chargeuprate;
						if ((int)projectile.ai[0]==(int)(chargeuprate*3f))
						Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, soundcharge).WithVolume(.7f).WithPitchVariance(.25f), projectile.Center);
						//float speedzz = projectile.velocity.Length();
						//projectile.velocity.Normalize();
						//projectile.velocity*=(speedzz+0.05f);
					}
					projectile.Center = (player.Center+new Vector2(dir*6, 0))+ (projectile.velocity*12f)-(projectile.velocity*(0.08f *projectile.ai[0]));
					projectile.position -= projectile.velocity;
					player.itemTime = 2;
					player.itemAnimation = 2;

					//Projectiles



					if (!isholding)
					{
						projectile.velocity /= 1.15f;

						if (projectile.ai[1] < 1)
						{
							Vector2 position = projectile.Center;
							Vector2 offset = new Vector2(projectile.velocity.X, projectile.velocity.Y);
							offset.Normalize();
							offset *= 16f;
							offset += projectile.velocity;

							float damagescale = (projectile.damage * (1f + (projectile.ai[0] / 30f)));

							Vector2 perturbedSpeed = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(0));
							Vector2 perturbedSpeed2 = perturbedSpeed;
							perturbedSpeed2.Normalize();
							float basespeed = 1f + (GetType() == typeof(SBCCannonHoldingMK2) ? 0.4f : -0.4f);
							float scale = 1.5f+ ((projectile.ai[0]/30f)* basespeed);// - (Main.rand.NextFloat() * .2f);
							perturbedSpeed = perturbedSpeed * (scale*4f);
							perturbedSpeed += perturbedSpeed2;
							offset -= perturbedSpeed;
							perturbedSpeed /= 2f;
							int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("SBCBall"), (int)damagescale, projectile.knockBack, player.whoAmI,0f,(float)projectile.damage);
							IdgProjectile.Sync(prog);
							Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, soundfire).WithVolume(.7f).WithPitchVariance(.25f), projectile.Center);
						}


						projectile.ai[1] = 1;

					}
				}
			}
			else
			{
				projectile.Kill();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{


			Texture2D tex = Main.projectileTexture[projectile.type];
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width/2f, tex.Height / 2f);
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
			Color color = projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			spriteBatch.Draw(tex, drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction<1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);

			return false;
		}


	}

	public class SBCBall : ModProjectile
	{
		bool hittile = false;
		public virtual bool hitwhilefalling => false;
		public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cannon Ball");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.SpikyBall);
			projectile.width = 18;
			projectile.height = 18;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.thrown = false;
			projectile.extraUpdates = 2;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/LeadCannonball"; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Vector2 scaler = new Vector2(Main.rand.NextFloat(0.75f, 1.25f), Main.rand.NextFloat(0.75f, 1.25f) * 0.5f + (projectile.velocity.Length() / 10f));
			Main.spriteBatch.Draw(ModContent.GetTexture("SGAmod/HavocGear/Projectiles/HeatWave"), projectile.Center - Main.screenPosition, new Rectangle?(), Color.White * MathHelper.Clamp((projectile.velocity.Length()-6f) / 4f, 0f, 1f), projectile.velocity.ToRotation()+MathHelper.ToRadians(90),
				new Vector2(ModContent.GetTexture("SGAmod/HavocGear/Projectiles/HeatWave").Width * 0.5f, ModContent.GetTexture("SGAmod/HavocGear/Projectiles/HeatWave").Height * 0.5f), scaler, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, projectile.rotation, origin, projectile.scale, facingleft ? effect : SpriteEffects.None, 0);

			return false;
		}

		public override bool PreAI()
		{

			for (int zz = 0; zz < Main.maxNPCs; zz += 1)
			{
				NPC npc = Main.npc[zz];
				if (!npc.dontTakeDamage && !npc.townNPC && npc.active && npc.life > 0)
				{
					Rectangle rech = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
					Rectangle rech2 = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					if (rech.Intersects(rech2))
					{
						if (projectile.localNPCImmunity[npc.whoAmI] < 1)
							npc.immune[projectile.owner] = 0;
					}
				}
			}

			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.localNPCImmunity[target.whoAmI] = 90;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			crit = false;
			projectile.damage -= target.life;
			if (projectile.damage < 1 || target.knockBackResist == 0f)
				projectile.Kill();
		}

		public override bool PreKill(int timeLeft)
		{

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("CannonBoom"), (int)(projectile.ai[1]), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].ranged = true;
			Main.projectile[theproj].melee = false;
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Math.Abs(oldVelocity.Length()) > 2)
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Cannon/Bounce").WithVolume(.7f).WithPitchVariance(.25f), projectile.Center);

			return true;
		}

		public override void AI()
		{

			/*for (int i = 0; i < projectile.scale + 0.5; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"));
				Main.dust[dust].scale = 2.25f * projectile.scale;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.dust[dust].color * 0.25f;
				Main.dust[dust].velocity = -projectile.velocity * (float)(Main.rand.Next(20, 100 + (i * 40)) * 0.005f);
			}*/

			if (Math.Abs(projectile.velocity.Length()) < 0.5f){
			projectile.Kill();
			}




			projectile.velocity.Y -= 0.15f;
			projectile.rotation += projectile.velocity.X * 0.25f;//(float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			projectile.ai[1] += 0.5f;
		}


	}

	public class CannonBoom : ModProjectile
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
				ranspin2 = Main.rand.NextFloat(-0.2f, 0.2f);
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
		}

		public override string Texture
		{
			get { return ("SGAmod/HavocGear/Projectiles/BoulderBlast"); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.timeLeft < 56)
				return false;
			else
				return null;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			getstuff();
			Texture2D tex = ModContent.GetTexture("Terraria/FlameRing");
			float timeleft = ((float)projectile.timeLeft / 60f);
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 3) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition));
			Color color = Color.White * timeleft; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(projectile.localAI[0] / 3f);
			timing %= 3;
			timing *= ((tex.Height) / 3);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 3), color, ranspin, drawOrigin, (1f-timeleft)*2f, SpriteEffects.None, 0f);
		}


		public override void AI()
		{
			projectile.localAI[0] += 1f;

			if (projectile.ai[0] < 1)
			{
				projectile.ai[0] = 1;
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
				for (float num315 = 0; num315 < 80; num315 += 0.25f)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					int dustz = DustID.Fire;
					if (Main.rand.Next(0, 20) < 5)
						dustz = mod.DustType("HotDust");
					int num316 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y)+ (randomcircle * (80f-num315) / 3f), 0,0, dustz, 0, 0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), (float)num315 * 0.1f);
					Main.dust[num316].noGravity = true;
					randomcircle *= (float)Math.Pow((80-num315) / 5f,0.75);
					Main.dust[num316].velocity = randomcircle;
				}
			}

			Lighting.AddLight(projectile.Center, ((255 - projectile.alpha) * 0.01f) / 255f, ((255 - projectile.alpha) * 0.025f) / 255f, ((255 - projectile.alpha) * 0.25f) / 255f);
			return;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(mod.BuffType("ThermalBlaze"), 300);
		}
	}

	public class LeadCannonball : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lead Cannonball");
			Tooltip.SetDefault("Cast-steel Cannonballs made from a Novus-Lead Alloy; for use with the SBC Cannon and MK2");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Ammo/LeadCannonball"; }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Cannonball, 100);
			recipe.AddIngredient(ItemID.LeadBar, 3);
			recipe.AddIngredient(null, "UnmanedBar", 2);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this,15);
			recipe.AddRecipe();
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 30;
			item.consumable = true;
			item.knockBack = 1.5f;
			item.value = 500;
			item.rare = 5;
			item.ammo = item.type;
		}

		public override void UpdateInventory(Player player)
		{
			item.maxStack = 30;
			//I don't think so Fargo, not this one
		}

	}

	public class SBCCannonMK2 : SBCCannon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SBC Cannon MK2");
			Tooltip.SetDefault("SBC Cannon improved with a pressure gauge, bindings, and lunar materials\nCharges and recovers after firing faster, and launches cannonballs faster than it's precurser\nCharge up piercing cannon balls that do a huge ammount of damage\nBut lose power with each enemy they pass through, exploding when they run out of damage\nCharge longer for more speed and much more damage!\nCannonballs explode against enemies immune to knockback, and do not crit\nDamage is increased instead based on crit chance, and the explosion however can crit\nUses Lead Cannonballs as ammo\n'LETS GET SERIOUS!!'");
		}

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SBCCannonHoldingMK2>()] > 0)
				return false;
			return true;
		}


		public override void SetDefaults()
		{
			item.damage = 2500;
			item.ranged = true;
			item.width = 48;
			item.height = 28;
			item.useTime = 5;
			item.useAnimation = 5;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 0;
			item.value = 1000000;
			item.rare = 11;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<SBCCannonHoldingMK2>();
			item.shootSpeed = 1f;
			item.channel = true;
			item.noUseGraphic = true;
			item.useAmmo = ModContent.ItemType<LeadCannonball>();
		}

		/*public override bool CanUseItem(Player player)
		{
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			return (modply.RefilPlasma());
		}*/

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-6, 0);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "SBCCannon", 1);
			recipe.AddIngredient(ItemID.ActuationAccessory, 1);
			recipe.AddIngredient(ItemID.PressureTrack, 5);
			recipe.AddIngredient(ItemID.Chain, 25);
			recipe.AddIngredient(ItemID.LihzahrdPressurePlate, 2);
			recipe.AddIngredient(null, "StarMetalBar", 10);
			recipe.AddIngredient(null, "DrakeniteBar", 15);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;


			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
			float scale = 1f;// - (Main.rand.NextFloat() * .2f);
			perturbedSpeed = perturbedSpeed * scale;
			int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			IdgProjectile.Sync(prog);


			return false;
		}
	}


	public class SBCCannonHoldingMK2 : SBCCannonHolding
	{
		//public virtual float trans => 1f;
		public Player P;
		public override float chargeuprate => 4f;
		public override string soundcharge => "Sounds/Custom/Cannon/Prepare";
		public override string soundfire => "Sounds/Custom/Cannon/Fire";
		public override int cooldowntime => 70;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cannon");
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/SeriousSam/SBCCannonProjMK2"; }
		}

	}


	}
