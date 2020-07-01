using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	public class CrateBossWeaponMelee : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Touch");
			Tooltip.SetDefault("Attacks always crit and deal double damage VS enemies debuffed with Midas");
		}
		public override void SetDefaults()
		{
			item.damage = 85;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = 7;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 124);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
				//Main.dust[dust].velocity.Normalize();
				//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
				//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
		{
			if (target.HasBuff(BuffID.Midas))
			{
				crit = true;
				damage *= 2;
			}
		}

	}
	public class CrateBossWeaponMagic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Philanthropist's Shower");
			Tooltip.SetDefault("Consumes Coins from the player's inventory\nDamage increases with higher value coins; more valuable coins are used first\nInflicts Midas on enemies\n'and so, I shall make it rain!'");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.magic = true;
			item.width = 34;
			item.mana = 10;
			item.height = 24;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 6;
			item.value = 500000;
			item.rare = 7;
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.shoot = 14;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.useTurn = false;
			Item.staff[item.type] = true;
		}

		public override bool CanUseItem(Player player)
		{
			return (player.CountItem(ItemID.CopperCoin) + player.CountItem(ItemID.SilverCoin) + player.CountItem(ItemID.GoldCoin) + player.CountItem(ItemID.PlatinumCoin) > 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			int taketype = 3;
			int[] types = { ItemID.CopperCoin, ItemID.SilverCoin, ItemID.GoldCoin, ItemID.PlatinumCoin };
			int silver = player.CountItem(ItemID.SilverCoin);
			int gold = player.CountItem(ItemID.GoldCoin);
			int plat = player.CountItem(ItemID.PlatinumCoin);
			taketype = plat > 0 ? 3 : (gold > 0 ? 2 : (silver > 0 ? 1 : 0));
			int coincount = player.CountItem(types[taketype]);
			if (player.CountItem(types[taketype]) > 0)
			{
				player.ConsumeItem(types[taketype]);
				float[,] typesproj = { { (float)ProjectileID.CopperCoin, 1f }, { (float)ProjectileID.SilverCoin, 1.5f }, { (float)ProjectileID.GoldCoin, 2.25f }, { (float)ProjectileID.PlatinumCoin, 5f } };

				int numberProjectiles = 8 + Main.rand.Next(7);
				for (int index = 0; index < numberProjectiles; index = index + 1)
				{
					Vector2 vector2_1 = new Vector2((float)((double)player.position.X + (double)player.width * 0.5 + (double)(Main.rand.Next(201) * -player.direction) + ((double)Main.mouseX + (double)Main.screenPosition.X - (double)player.position.X)), (float)((double)player.position.Y + (double)player.height * 0.5 - 600.0));   //this defines the projectile width, direction and position
					vector2_1.X = (float)(((double)vector2_1.X + (double)player.Center.X) / 2.0) + (float)Main.rand.Next(-200, 201);
					vector2_1.Y -= (float)(100 * (index / 4));
					float num12 = (float)Main.mouseX + Main.screenPosition.X - vector2_1.X;
					float num13 = (float)Main.mouseY + Main.screenPosition.Y - vector2_1.Y;
					if ((double)num13 < 0.0) num13 *= -1f;
					if ((double)num13 < 20.0) num13 = 20f;
					float num14 = (float)Math.Sqrt((double)num12 * (double)num12 + (double)num13 * (double)num13);
					float num15 = item.shootSpeed / num14;
					float num16 = num12 * num15;
					float num17 = num13 * num15;
					float morespeed = 0.75f + ((float)taketype * 0.2f);
					float SpeedX = (num16 * morespeed) + (float)Main.rand.Next(-40, 41) * 0.02f;
					float SpeedY = (num17 * morespeed) + (float)Main.rand.Next(-40, 41) * 0.02f;
					int thisone = Projectile.NewProjectile(vector2_1.X, vector2_1.Y, SpeedX, SpeedY, (int)typesproj[taketype, 0], (int)(typesproj[taketype, 1] * (float)damage), knockBack, Main.myPlayer, 0.0f, 0f);
					Main.projectile[thisone].magic = true;
					Main.projectile[thisone].ranged = false;
					IdgProjectile.AddOnHitBuff(thisone, BuffID.Midas, 60 * 10);
				}



			}
			return false;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(1) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
			}
		}

	}

	public class CrateBossWeaponRanged : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Jackpot");
			Tooltip.SetDefault("Launches money-filled rockets that explode into coins!\nInflicts Midas on enemies\n'Once was a pre-order bonus!'");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.SnowmanCannon);
			item.damage = 40;
			item.width = 48;
			item.height = 48;
			item.useTime = 20;
			item.useAnimation = 20;
			item.knockBack = 6;
			item.value = 1000000;
			item.ranged = true;
			item.rare = 7;
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
			float speed = 8f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(8);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("JackpotRocket"), damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly = true;
				Main.projectile[proj].hostile = false;
				Main.projectile[proj].timeLeft = 600;
				Main.projectile[proj].knockBack = item.knockBack;
			}
			return false;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(1) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
			}
		}


	}

	class CrateBossWeaponThrown : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Avarice");
			Tooltip.SetDefault("Throw coins that influx on one enemy\nInflicts Midas and Shadowflame on enemies\n'Greed is it's own corruption'");
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown = true;
			item.damage = 75;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("AvariceCoin");
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 28;
			item.maxStack = 1;
			item.knockBack = 9;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 10;
			item.useTime = 10;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = 7;
		}


		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}

	public class AvariceCoin : ModProjectile
	{

		int fakeid = ProjectileID.GoldCoin;
		public int guyihit = -10;
		public int cooldown = -10;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avarice Coin");
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (guyihit < 0)
			{
				return null;
			}
			else
			{
				if (guyihit != target.whoAmI && cooldown > 0)
					return false;
				if (cooldown > 0)
					return false;
			}
			return null;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(cooldown);
			writer.Write((short)guyihit);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			cooldown = reader.ReadInt32();
			guyihit = reader.ReadInt16();
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (guyihit < 1)
				guyihit = target.whoAmI;
			cooldown = 15;
			projectile.tileCollide = false;
			target.immune[projectile.owner] = 2;
			target.AddBuff(BuffID.ShadowFlame, 60 * 10);
			target.AddBuff(BuffID.Midas, 60 * 10);
			projectile.netUpdate = true;
		}


		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override void SetDefaults()
		{
			projectile.aiStyle = 18;
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 300;
			projectile.penetrate = 3;
			projectile.tileCollide = true;
			projectile.friendly = true;
			projectile.hostile = false;
			guyihit = -1;
			cooldown = -1;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = fakeid;
			return true;
		}

		public override void AI()
		{
			cooldown -= 1;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			if (guyihit > -1)
			{
				if (cooldown == 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					projectile.Center = Main.npc[guyihit].Center + (randomcircle * 256f);
					projectile.velocity = -randomcircle * 8f;
				}
				if (Main.npc[guyihit].active == false || Main.npc[guyihit].life < 1)
				{
					guyihit = -10;
				}

			}
		}

	}

	public class CrateBossWeaponSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prosperity Rod");
			Tooltip.SetDefault("Summons Midas Portals to shower your enemies in wealth, painfully\nOrdering your minions to attack a target will move the center of the circle to the target and the portals will gain an extra weaker attack VS the closest enemy\nAttacks inflict Midas\n'money money, it acts so funny...'");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 45;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 20, 0, 0);
			item.rare = 7;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("MidasMinionBuff");
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = mod.ProjectileType("MidasPortal");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			return true;
		}

	}

	public class MidasPortal : ModProjectile
	{
		protected float idleAccel = 0.05f;
		protected float spacingMult = 1f;
		protected float viewDist = 400f;
		protected float chaseDist = 200f;
		protected float chaseAccel = 6f;
		protected float inertia = 40f;
		protected float shootCool = 90f;
		protected float shootSpeed;
		protected int shoot;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public sealed override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
		}


		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return false;
		}

		public virtual void CreateDust()
		{
		}

		public virtual void SelectFrame()
		{
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;


			Player player = Main.player[projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<MidasMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<MidasMinionBuff>()))
			{
				projectile.timeLeft = 2;
			}
			Vector2 gothere = player.Center;
			projectile.localAI[0] += 1;

			int target2 = Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile);
			NPC them = Main.npc[target2];
			NPC oldthem = null;

			if (player.HasMinionAttackTargetNPC)
			{
				oldthem = them;
				them = Main.npc[player.MinionAttackTargetNPC];
				gothere = them.Center;
			}

			if (them != null && them.active)
			{
				if ((them.Center - projectile.Center).Length() < 500 && Collision.CanHitLine(new Vector2(projectile.Center.X, projectile.Center.Y), 1, 1, new Vector2(them.Center.X, them.Center.Y), 1, 1) && them.CanBeChasedBy())
				{
					projectile.ai[0] += 1;

					if (projectile.ai[0] % 20 == 0)
					{
						Main.PlaySound(18, (int)projectile.Center.X, (int)projectile.Center.Y, 0, 1f, 0.25f);
						int thisoned = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ProjectileID.GoldCoin, projectile.damage, projectile.knockBack, Main.player[projectile.owner].whoAmI);
						Main.projectile[thisoned].minion = true;
						Main.projectile[thisoned].velocity = (them.Center - projectile.Center);
						Main.projectile[thisoned].velocity.Normalize(); Main.projectile[thisoned].velocity *= 12f; Main.projectile[thisoned].velocity = Main.projectile[thisoned].velocity.RotateRandom(MathHelper.ToRadians(15));
						Main.projectile[thisoned].penetrate = 1;
						Main.projectile[thisoned].ranged = false;
						Main.projectile[thisoned].netUpdate = true;
						IdgProjectile.AddOnHitBuff(thisoned, BuffID.Midas, 60 * 5);
						IdgProjectile.Sync(thisoned);
					}

				}

				if (oldthem != null)
				{
					if ((oldthem.Center - projectile.Center).Length() < 500 && Collision.CanHitLine(new Vector2(projectile.Center.X, projectile.Center.Y), 1, 1, new Vector2(oldthem.Center.X, oldthem.Center.Y), 1, 1) && oldthem.CanBeChasedBy())
					{

						if (projectile.ai[0] % 35 == 0)
						{
							Main.PlaySound(18, (int)projectile.Center.X, (int)projectile.Center.Y, 0, 0.75f, -0.5f);
							int thisoned = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, ProjectileID.SilverCoin, (int)((float)projectile.damage * 0.75f), projectile.knockBack, Main.player[projectile.owner].whoAmI);
							Main.projectile[thisoned].minion = true;
							Main.projectile[thisoned].velocity = (oldthem.Center - projectile.Center);
							Main.projectile[thisoned].velocity.Normalize(); Main.projectile[thisoned].velocity *= 10f; Main.projectile[thisoned].velocity = Main.projectile[thisoned].velocity.RotateRandom(MathHelper.ToRadians(15));
							Main.projectile[thisoned].penetrate = 1;
							Main.projectile[thisoned].ranged = false;
							Main.projectile[thisoned].netUpdate = true;
							IdgProjectile.AddOnHitBuff(thisoned, BuffID.Midas, 60 * 2);
							IdgProjectile.Sync(thisoned);
						}
					}

				}


			}

			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 124);
			Main.dust[dust].scale = 0.7f;
			Main.dust[dust].velocity = projectile.velocity * 0.2f;
			Main.dust[dust].noGravity = true;

			float us = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == projectile.type)
				{ // Make sure the projectile is of the same type as this javelin

					if (i == projectile.whoAmI)
						us = maxus;
					maxus += 1f;

				}

			}
			Vector2 there = player.Center;

			double angles = MathHelper.ToRadians((float)((us / maxus) * 360.00) - 90f);
			float dist = 256f;//Main.rand.NextFloat(54f, 96f);
			Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles)) * dist;
			Vector2 where = gothere + here;

			if ((where - projectile.Center).Length() > 8f)
			{
				projectile.velocity += (where - projectile.Center) * 0.005f;
				projectile.velocity *= 0.975f;
			}
			float maxspeed = Math.Min(projectile.velocity.Length(), 16);
			projectile.velocity.Normalize();
			projectile.velocity *= maxspeed;



			Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3() * 0.78f);

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Javelins/StoneJavelin"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("Terraria/Projectile_" + ProjectileID.CoinPortal);

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.Lerp((projectile.GetAlpha(lightColor) * 0.5f), Color.White, 0.5f); //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(projectile.localAI[0] / 8f);
			timing %= 4;
			timing *= ((tex.Height) / 4);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), color, projectile.velocity.X * 0.04f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

	}
	public class MidasMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			Description.SetDefault("Portals to planes of wealth will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("MidasPortal")] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}