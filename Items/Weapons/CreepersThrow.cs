using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class CreepersThrow : ModItem
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Explosive Throw");
			Tooltip.SetDefault("Controls a yoyo shaped creeper that lights a fuse when near enemies and explodes violently shortly after\nHowever, watch out as you can hurt yourself from the creeper's explosion");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
			{
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "40% increased damage"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Creates smaller explosions leading up to the larger one"));
			}
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 4) % 1f, 0.4f, 0.45f);
            //string potion="[i:" + ItemID.RedPotion + "]";
            tooltips.Add(new TooltipLine(mod, "IDG Debug Item", Idglib.ColorText(c, "Mister Creeper's other (Legecy) Dev Weapon")));
        }

        public override void SetDefaults()
        {
            Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);                                 
            item.damage = 250;
            item.useTime = 16;
            item.useAnimation = 16;
            item.useStyle = 5;
			item.crit = 10;
            item.channel = true;
            item.melee = true;
			item.noMelee = true;
            item.knockBack = 2.5f;
			item.value = Item.sellPrice(0, 75, 0, 0);
			item.rare = 11;
			item.expert = true;
            item.noUseGraphic = true;
			item.autoReuse = true;
            item.UseSound = SoundID.Item19;
            item.shoot = mod.ProjectileType("CreepersThrowProj");
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(ItemID.FragmentSolar, 10);
			recipe.AddIngredient(ItemID.ExplosivePowder, 100);
			recipe.AddIngredient(ItemID.ChlorophyteBar, 10);
			recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			if (player.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
			add += 0.40f;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
		    return false;
	    }
    }

	public class CreepersThrowProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creeper's Throw");
			ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 999f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 500f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 20f;
		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/CreepersThrow"); }
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 10;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			if (owner != null && !owner.dead)
			{
				if (projectile.localAI[1] < 0)
					projectile.localAI[1] += 1;


				if (projectile.localAI[1] < 1)
				{
					NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
					if (target != null && Vector2.Distance(target.Center, projectile.Center) < 72)
					{
						projectile.localAI[1] = 1;


					}
				}
				else
				{
					projectile.localAI[1] += 1;

					int dustIndexsmoke = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndexsmoke].noGravity = true;
					Main.dust[dustIndexsmoke].position = projectile.Center + new Vector2(0f, (float)(-(float)projectile.height / 2)).RotatedBy((double)projectile.rotation, default(Vector2)) * 1.1f;
					dustIndexsmoke = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
					Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
					Main.dust[dustIndexsmoke].noGravity = true;
					Main.dust[dustIndexsmoke].position = projectile.Center + new Vector2(0f, (float)(-(float)projectile.height / 2 - 6)).RotatedBy((double)projectile.rotation, default(Vector2)) * 1.1f;

					if (projectile.localAI[1] >40 && projectile.localAI[1]<120 && projectile.localAI[1]%25==0 && owner.GetModPlayer<SGAPlayer>().devempowerment[1]>0)
					{

							int thisone = Projectile.NewProjectile(projectile.Center.X - 100, projectile.Center.Y - 100, Vector2.Zero.X, Vector2.Zero.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage * 2, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
							Main.projectile[thisone].timeLeft = 2;
							Main.projectile[thisone].width = 200;
							Main.projectile[thisone].penetrate = 1;
							Main.projectile[thisone].height = 200;
							Main.projectile[thisone].scale = 0.001f;
							Main.projectile[thisone].netUpdate = true;

					}

					if (projectile.localAI[1] == 121)
					{
						projectile.localAI[1] = -60;
						for (int i = 0; i < 359; i += 36)
						{
							double angles = MathHelper.ToRadians(i);
							float randomx = 64f;//Main.rand.NextFloat(54f, 96f);
							Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

							int thisone = Projectile.NewProjectile(projectile.Center.X + (here.X * randomx) - 100, projectile.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage*1, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
							Main.projectile[thisone].timeLeft = 2;
							Main.projectile[thisone].width = 200;
							Main.projectile[thisone].height = 200;
							Main.projectile[thisone].scale = 0.001f;
							Main.projectile[thisone].netUpdate = true;

						}
					}
					if (projectile.localAI[1] == 120)
					{

						for (int i = 0; i < 359; i += 72)
						{
							double angles = MathHelper.ToRadians(i);
							float randomx = 48f;//Main.rand.NextFloat(54f, 96f);
							Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles));

							int thisone = Projectile.NewProjectile(projectile.Center.X + (here.X * randomx) - 100, projectile.Center.Y + (here.Y * randomx) - 100, here.X, here.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage*1, projectile.knockBack, Main.player[projectile.owner].whoAmI, 0.0f, 0f);
							Main.projectile[thisone].timeLeft = 2;
							Main.projectile[thisone].width = 200;
							Main.projectile[thisone].penetrate = 1;
							Main.projectile[thisone].height = 200;
							Main.projectile[thisone].scale = 0.001f;
							Main.projectile[thisone].netUpdate = true;

						}


					}

				}

			}
		}

	}
		
		public class CreepersThrowBoom : ModProjectile
		{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Creeper's KaBoom");
			}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/CreepersThrow"); }
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (!target.friendly && !target.dontTakeDamage && target.immune[Main.player[projectile.owner].whoAmI] > 0)
				return true;
			return base.CanHitNPC(target);
		}

		public override void SetDefaults()
			{
			projectile.CloneDefaults(ProjectileID.GrenadeIII);
			projectile.scale = 0.001f;
			projectile.melee = true;
			//projectile.penetrate = 1;
			aiType = ProjectileID.GrenadeIII;
			}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			target.immune[projectile.owner] = 1;
		}

		public override bool PreKill(int timeLeft)
			{
				projectile.type = ProjectileID.GrenadeIII;
				return true;
			}
	}

	public class CreepersThrowBoom2 : CreepersThrowBoom
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Creeper's KaBoom");
		}

        public override void SetDefaults()
        {
            base.SetDefaults();
			projectile.timeLeft = 3;
			projectile.width = 200;
			projectile.height = 200;
			projectile.scale = 0.001f;
			projectile.timeLeft = 2;
			projectile.penetrate = 1;
		}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (target.life - damage < 0)
			{
				Main.player[projectile.owner].statLife += 50;
				Main.player[projectile.owner].GetModPlayer<SGAPlayer>().creeperexplosion = 0;
			}
		}


	}



}