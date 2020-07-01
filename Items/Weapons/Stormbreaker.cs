using System.Collections.Generic;
using Terraria;
using System;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	class Stormbreaker : ModItem
	{
		bool altfired=false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Stormbreaker");
			Tooltip.SetDefault("Left click to guide the Stormbreaker at enemies and deal an additional Squareroot of their max life on hit\nRight click to hold the hammer up and smite your foes, the cost is 15 mana (before mana cost reductions) per foe to be smited\nfoes must be marked via primary fire (40% chance if immune) or wet to be smited\n2 more bolts are summoned during a rainstorm, but overall are less accurate\n'atleast it's not yet another Infinity Gauntlet'");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			if (Main.LocalPlayer.GetModPlayer<SGAPlayer>().devempowerment[1] > 0)
			{
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "--- Enpowerment bonus ---"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "10% increased damage on Primary"));
				tooltips.Add(new TooltipLine(mod, "DevEmpowerment", "Secondary will always summon lightning as if it were raining"));
			}
			Color c = Main.hslToRgb((float)(Main.GlobalTime/4)%1f, 0.4f, 0.45f);
            //string potion="[i:" + ItemID.RedPotion + "]";
            tooltips.Add(new TooltipLine(mod,"IDG Debug Item", Idglib.ColorText(c, "Mister Creeper's (Legecy) Dev Weapon")));
        }
		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.thrown=true;
			item.damage = 750;
			item.shootSpeed = 45f;
			item.shoot = mod.ProjectileType("Stormbreakerproj");
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
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = 12;
			item.channel = true;
			item.expert=true;
		}

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.GetModPlayer<SGAPlayer>().devempowerment[1]>0)
			{
				damage = (int)(damage * 1.10);
			}
			return true;
		}

		public override bool CanUseItem(Player player)
        {

        altfired=player.altFunctionUse == 2 ? true : false;

        if (altfired){
        	if (player.statMana<(int)(30f*player.manaCost))
        	return false;
			item.useAnimation = 45;
			item.useTime = 45;
			item.useStyle = 4;
			item.UseSound = SoundID.Item44;
			item.channel = false;
			item.shoot = mod.ProjectileType("Stormbreaker2");
			item.useTurn = false;

        }else{
			item.useStyle = 1;
			item.shootSpeed = 45f;
			item.shoot = mod.ProjectileType("Stormbreakerproj");
			//ProjectileID.CultistBossLightningOrbArc
			item.UseSound = SoundID.Item1;
			item.useAnimation = 10;
			item.useTime = 10;
			item.channel = true;
			item.autoReuse = true;
			item.useTurn = true;
		}
		if (player.ownedProjectileCounts[item.shoot]>0)
        return false;

        return true;
        }

		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.PossessedHatchet, 1);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 25);
            recipe.AddIngredient(mod.ItemType("StarMetalBar"), 30);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 10);
			recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}


	public class Stormbreakerproj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stormbreaker");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Stormbreaker"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.thrown = true;
			projectile.timeLeft = 120;
			projectile.penetrate = 50;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.penetrate < 10)
				return false;
			else
				return base.CanHitNPC(target);
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += (int)Math.Pow((double)target.lifeMax, 0.5);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.velocity *= -0.5f;
			Vector2 dist = (target.Center - projectile.Center);
			Vector2 distnorm = dist; distnorm.Normalize();
			projectile.velocity -= distnorm * 30f;
			target.immune[projectile.owner] = 7;
			target.AddBuff(mod.BuffType("InfinityWarStormbreaker"), 600);
			if (Main.rand.Next(0, 100) < 40)
				IdgNPC.AddBuffBypass(target.whoAmI, mod.BuffType("InfinityWarStormbreaker"), 400);
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			Player player = Main.player[projectile.owner];




			for (int num315 = 0; num315 < 1; num315 = num315 + 1)
			{
				int num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + positiondust, 0, 0, 185, 0f, 0f, 100, default(Color), 1f);
				Main.dust[num622].velocity *= 1f;

				Main.dust[num622].noGravity = true;
				Main.dust[num622].color = Main.hslToRgb((float)(Main.GlobalTime / 5) % 1, 0.9f, 1f);
				Main.dust[num622].color.A = 10;
				Main.dust[num622].velocity.X = projectile.velocity.X / 3 + (Main.rand.Next(-50, 51) * 0.005f);
				Main.dust[num622].velocity.Y = projectile.velocity.Y / 3 + (Main.rand.Next(-50, 51) * 0.005f);
				Main.dust[num622].alpha = 100; ;
			}

			projectile.ai[0] = projectile.ai[0] + 1;
			bool channeling = ((projectile.ai[0] < 299 && (player.channel || projectile.ai[0] < 29)) && !player.noItems && !player.CCed);
			if (projectile.ai[0] < 300 && projectile.ai[0] < 600 && (projectile.penetrate < 10 || !channeling))
				projectile.ai[0] = 300;
			projectile.timeLeft = 999;

			if (!Main.player[projectile.owner].dead)
			{
				Vector2 flyto = Main.MouseWorld;//new Vector2( ((float)Main.mouseX + (float)Main.screenPosition.X - (float)player.position.X), ((float)Main.mouseY + (float)Main.screenPosition.Y - (float)player.position.Y) );
				Vector2 dist = (Main.player[projectile.owner].Center - projectile.Center);
				Vector2 distnorm = dist; distnorm.Normalize();

				Vector2 flytodist = (flyto - projectile.Center);
				Vector2 flytodistnorm = flytodist; flytodistnorm.Normalize();

				if (projectile.ai[0] > 299)
				{
					flytodist = dist;
					flytodistnorm = distnorm;
				}

				if (Main.LocalPlayer == player)
				{
					projectile.velocity += flytodistnorm * 8f;
					projectile.velocity /= 1.05f;
					float maxspeed = 38f * (1f + ((player.thrownVelocity - 1f) / 2f));
					if (projectile.velocity.Length() > maxspeed)
					{
						projectile.velocity.Normalize(); projectile.velocity *= maxspeed;
					}
					projectile.netUpdate = true;
				}
				//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));


				if (projectile.ai[0] < 600)
				{
					player.itemTime = (int)MathHelper.Clamp(player.itemTime, 5, 1111);
					player.itemAnimation = (int)MathHelper.Clamp(player.itemAnimation, 5, 1111);
				}

				if (dist.Length() < 64f && projectile.ai[0] > 299)
				{
					projectile.ai[0] = 600;
					if ((player.itemTime < 2) || Main.player[projectile.owner].dead)
						projectile.Kill();
				}

			}
			else { projectile.Kill(); }

			/*NPC target=Main.npc[Idglib.FindClosestTarget(0,projectile.Center,new Vector2(0f,0f),true,true,true,projectile)];
			if (target!=null && projectile.penetrate>9){
			if ((target.Center-projectile.Center).Length()<500f){

	projectile.Center+=(projectile.DirectionTo(target.Center)*12f);

	}}*/


			projectile.rotation = projectile.rotation.AngleLerp(((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f) + (float)(Math.PI / -4.0), 0.2f);
		}






	}

	public class Stormbreaker2 : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stormbreaker");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Stormbreaker"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.thrown = true;
			projectile.timeLeft = 120;
			projectile.penetrate = 10;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override void AI()
		{
			Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			Player owner = Main.player[projectile.owner];

			if (projectile.ai[0] < 1f && owner.direction != 1)
				projectile.rotation *= -1f;


				owner.manaRegenDelay = (int)(owner.maxRegenDelay * 15);

			projectile.ai[0] += 1;



			projectile.velocity = new Vector2(0f, owner.itemAnimation > 12 ? -16f : 0);
			projectile.Center = owner.Center + new Vector2(-24f + (owner.direction == 1 ? 10f : -5f), -16f);
			if (owner.itemAnimation < 8)
				projectile.Center += new Vector2(0, (int)((8.0 - (double)owner.itemAnimation) * 2.5));

			if (owner.itemAnimation < 2)
				projectile.Kill();


			projectile.rotation += (((float)(Math.PI / -4.0)) - projectile.rotation) / 6f;
			if (owner.itemAnimation == 30)
			{
				for (int i = 0; i < Main.maxNPCs; i++)
				{
					NPC him = Main.npc[i];
					if (him.active)
					{
						if (him.GetGlobalNPC<SGAnpcs>().InfinityWarStormbreakerint > 0 || him.GetGlobalNPC<SGAnpcs>().DosedInGas || him.dripping)
						{
							if (owner.statMana > 0)
							{
								owner.statMana -= (int)(10f * owner.manaCost);
								owner.manaRegenDelay = 180;
								int rainmeansmore = (Main.raining || owner.GetModPlayer<SGAPlayer>().devempowerment[1] > 0) ? 2 : 0;

								for (int x = 0; x < rainmeansmore + 1; x++)
								{

									float rotation = MathHelper.ToRadians(3);
									Vector2 speed = new Vector2(0f, 72f);
									Vector2 perturbedSpeed = speed.RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) * 0.02f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
									Vector2 starting = new Vector2(him.Center.X + ((-200 + Main.rand.Next(0, 400)) * rainmeansmore), ((-150 + Main.rand.Next(0, 200)) * rainmeansmore) + him.Center.Y - Main.rand.Next(200, 540));
									int proj = Projectile.NewProjectile(starting.X,starting.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.CultistBossLightningOrbArc, (int)((projectile.damage * 0.50f)*(1f - owner.manaSickReduction)), 15f, Main.player[projectile.owner].whoAmI, (him.Center - starting).ToRotation());
									Main.projectile[proj].friendly = true;
									Main.projectile[proj].hostile = false;
									Main.projectile[proj].penetrate = -1;
									Main.projectile[proj].timeLeft = 300;
									//Main.projectile[proj].usesLocalNPCImmunity = true;
									Main.projectile[proj].localNPCHitCooldown = 8;
									Main.projectile[proj].thrown = true;
									IdgProjectile.Sync(proj);

									for (int q = 0; q < 50; q++)
									{
										int dust = Dust.NewDust(Main.projectile[proj].position - new Vector2(100, 0), 200, 12, DustID.Smoke, 0f, 0f, 100, Main.hslToRgb(0.6f, 0.8f, 0.28f), 4f);
										Main.dust[dust].noGravity = true;
										//Main.dust[dust].velocity *= 1.8f;
										//Main.dust[dust].velocity.Y -= 0.5f;
									}


								}

							}
						}
					}
				}
			}

		}






	}



}
