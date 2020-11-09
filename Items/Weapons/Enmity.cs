using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics;
using ReLogic.Graphics;
using Terraria.UI.Chat;
using SGAmod.Projectiles;
using SGAmod.NPCs.Hellion;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Items.Weapons
{
	public class Enmity : ModItem, IHitScanItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enmity");
			Tooltip.SetDefault("'Live by the sword, die by the sword'\n'Ashes to Ashes, Dust to Dust...'-the Blade of Hellion, forged from the finest blades of this world\nRequires mana to swing, melee hits with the blade inflict bypassing Sundered Defense and summon Fireworks around hit targets\nTapping swings lv2 True Moonlight waves, Can tap up to 2 times to summon 2 portals at once\nUnleashes total laser hell...\nThe damage of these are less than the melee damage but are improved by your magic damage multiplier");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 1500;
			item.crit = 5;
			item.melee = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 15;
			item.useAnimation = 10;
			item.useStyle = 10;
			item.knockBack = 15;
			item.value = Item.sellPrice(10,0,0,0);
			item.shootSpeed = 28f;
			item.shoot = mod.ProjectileType("ProjectilePortalEnmity");
			item.rare = 12;
			item.UseSound = SoundID.Item71;
			item.autoReuse = false;
			item.useTurn = false;
			item.channel = true;
			item.mana = 40;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Enmity_Glow");
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Main.hslToRgb((Main.GlobalTime*1.5f)%1f,0.8f,0.75f);
				};
			}

		}

		public override bool CanUseItem(Player player)
		{
			if (player.statMana < 30 || player.ownedProjectileCounts[mod.ProjectileType("ProjectilePortalEnmity")] > 2)
			{
				return false;
			}
			else 
			{
				//item.useTime = 15;
				//item.useAnimation = 10;
				return true;
			}
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			item.noMelee = false;
			item.useStyle = 1;

			float speed = 1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;
			Main.PlaySound(SoundID.Item, player.Center, 45);
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X*1.5f, perturbedSpeed.Y * 1.5f, mod.ProjectileType("MoonlightWaveLv2"), (int)((float)damage * 0.20f), knockBack / 3f, player.whoAmI);
				Main.projectile[proj].melee = true;
				Main.projectile[proj].magic = false;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.Sync(proj);

				//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
			}
			//SGAPlayer.LimitProjectiles(player, 0, new ushort[] {(ushort)mod.ProjectileType("ProjectilePortalRealityShaper") });
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{

			for (int a = 0; a < 3; a += 1)
			{
				for (int i = 0; i < 360; i += 120)
				{
					float angle = MathHelper.ToRadians(i+(a*45));
					Vector2 hereas = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * (200f + (160f * a));
					hereas += target.Center;
					Vector2 gohere = (target.Center - hereas); gohere.Normalize(); gohere *= (16f*a);
					int proj = Projectile.NewProjectile(hereas, gohere, mod.ProjectileType("ProjectilePortalEmnityHit"), (int)(damage * 0.25f), knockBack, player.whoAmI, 167 + Main.rand.Next(4));
					Main.projectile[proj].magic = false;
					Main.projectile[proj].melee = true;
					Main.projectile[proj].timeLeft = 70;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);

				}
			}

			IdgNPC.AddBuffBypass(target.whoAmI,mod.BuffType("SunderedDefense"), 60 * 12);
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			/*for (int num475 = 0; num475 < 3; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
				Main.dust[dust].noGravity = true;
			}*/

			for (int num475 = 3; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 15f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 3f) + ((player.direction) * player.itemRotation.ToRotationVector2() * (float)num475);
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new HellionItems(mod);
			recipe.AddIngredient(mod.ItemType("RealityShaper"), 1);
			recipe.AddIngredient(mod.ItemType("BrimflameHarbinger"), 1);
			recipe.AddIngredient(mod.ItemType("GalacticInferno"), 1);
			recipe.AddIngredient(ItemID.TerraBlade, 1);
			recipe.AddIngredient(mod.ItemType("CelestialFlare"), 1);
			recipe.AddIngredient(mod.ItemType("TrueMoonlight"), 1);
			recipe.AddIngredient(mod.ItemType("SOATT"), 1);
			recipe.AddIngredient(mod.ItemType("TrueCaliburn"), 1);
			recipe.AddIngredient(ItemID.AviatorSunglasses, 1);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 500);
			recipe.AddIngredient(mod.ItemType("HellionSummon"), 1);
			recipe.AddIngredient(mod.ItemType("CodeBreakerHead"), 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			string text = tooltips[0].text;
			string newline = "";
					for (int i = 0; i < text.Length; i += 1)
					{
						newline += Idglib.ColorText(Main.hslToRgb((((-Main.GlobalTime*6f)+i)/(10f)) % 1f, 0.75f, Main.rand.NextFloat(0.25f, 0.5f)), text[i].ToString());
					}
				tooltips[0].text = newline;
		}


	}

	public class ProjectilePortalEmnityHit : ProjectilePortal
	{
		public override int takeeffectdelay => 0;
		public override float damagescale => 1f;
		public override int penetrate => 1;
		public override int openclosetime => 16;

		public override void Explode()
		{

			if (projectile.timeLeft == timeleftfirerate && projectile.ai[0] > 0)
			{
				Player owner = Main.player[projectile.owner];
				if (owner != null && !owner.dead)
				{

					Vector2 gotohere = new Vector2();
					gotohere = projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(3)) * projectile.velocity.Length();
					int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), (int)projectile.ai[0], (int)(projectile.damage * damagescale), projectile.knockBack / 10f, owner.whoAmI);
					Main.projectile[proj].magic = true;
					Main.projectile[proj].timeLeft = 300;
					Main.projectile[proj].penetrate = penetrate;
					IdgProjectile.Sync(proj);
				}

			}

		}

	}


	public class ProjectilePortalEnmity : ProjectilePortalDSupernova
	{
		public override int openclosetime => 20;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nova");
		}

		public override int projectilerate => 25;
		public override int manacost => 9;
		public override int portalprojectile => mod.ProjectileType("CirnoBolt");
		public override int takeeffectdelay =>  Main.player[projectile.owner].HeldItem.useTime;
		public override float damagescale => 0.4f * Main.player[projectile.owner].magicDamage;
		public override int penetrate => 1;
		public override int startrate => 60;
		public override int drainrate => 5;
		public override int timeleftfirerate => 20;
		public override float portaldistfromsword => 128f;

		public int everyother = 0;

		public int chargeup = 0;

		public override void Explode()
		{
			chargeup += 1;

			if (projectile.timeLeft == timeleftfirerate && projectile.ai[0] > 0)
			{
				Player owner = Main.player[projectile.owner];

				if (owner != null && !owner.dead && owner.channel)
				{
					everyother += 1;
					everyother %= 3;

					Vector2 gotohere = new Vector2();
					gotohere = projectile.velocity;//Main.MouseScreen - projectile.Center;
					gotohere.Normalize();

					float accuracy = Math.Max(0.005f, 1f - ((chargeup) / 500f));

					Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(0)) * projectile.velocity.Length();

					for (int i = 2; i < 8; i += 1)
					{

						int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), (new Vector2(perturbedSpeed.X, perturbedSpeed.Y) * 2f).RotatedByRandom((MathHelper.ToRadians((i * 75))) * accuracy).RotatedByRandom(MathHelper.ToRadians(160f)* accuracy), mod.ProjectileType("RainbowBolt"), (int)((projectile.damage*0.60f) * damagescale), projectile.knockBack / 10f, owner.whoAmI);
						Main.projectile[proj].magic = true;
						Main.projectile[proj].minion = false;
						IdgProjectile.Sync(proj);
					}


					if (everyother == 2)
					{

						Vector2 backthere = new Vector2(-100, 0).RotatedByRandom(MathHelper.ToRadians(80));

						//int proj2 = Projectile.NewProjectile(backthere, gohere, mod.ProjectileType("ProjectilePortalRealityShaperHit"), (int)(projectile.damage * damagescale), projectile.knockBack / 10f, owner.whoAmI, mod.ProjectileType("HotRound"));

						Func<Vector2, Vector2, float, float, Projectile, float> projectilefacingmore = delegate (Vector2 playerpos, Vector2 projpos, float time, float current, Projectile proj)
						{
							float val = current;
							if (projectile.active)
							{
								if (time < 100)
									val = current.AngleLerp(projectile.velocity.ToRotation() + proj.ai[1], 0.15f);
								else
									val = current.AngleLerp(projectile.velocity.ToRotation() + proj.ai[1], 0.05f);

							}
							return val;
						};
						Func<Vector2, Vector2, float, Vector2, Projectile, Vector2> projectilemovingmore = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current, Projectile proj)
					   {


						   if (projectile.active)
						   {
							   Vector2 normspeed = projectile.velocity;
							   normspeed.Normalize();

							   Vector2 gothere333 = (playerpos + backthere.RotatedBy(projectile.velocity.ToRotation())) - normspeed * 128f;
							   Vector2 slideover = gothere333 - projpos;
							   current = slideover / 2f;
						   }

						   current /= 1.125f;
						   if (projectile.active)
						   {

							   Vector2 speedz = current;
							   float spzzed = speedz.Length();
							   speedz.Normalize();
							   if (spzzed > 100f)
								   current = (speedz * spzzed);
						   }
						   else
						   {
							   proj.timeLeft = Math.Min(proj.timeLeft, 20);
						   }

						   return current;
					   };

						Func<float, bool> projectilepattern = (time) => (time == 20);

						int ize2 = ParadoxMirror.SummonMirror(owner.Center, Vector2.Zero, (int)((projectile.damage*3) * damagescale), 200, projectile.velocity.ToRotation(), mod.ProjectileType("HellionBeam"), projectilepattern, 2.5f, 145, true);
						(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacingmore = projectilefacingmore;
						(Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemovingmore = projectilemovingmore;
						Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
						Main.projectile[ize2].owner = projectile.owner;
						Main.projectile[ize2].aiStyle = -2;
						Main.projectile[ize2].ai[1] = Main.rand.NextFloat(-MathHelper.ToRadians(20), MathHelper.ToRadians(20));
						Main.projectile[ize2].friendly = true;
						Main.projectile[ize2].hostile = false;
						Main.projectile[ize2].usesLocalNPCImmunity = true;
						Main.projectile[ize2].localNPCHitCooldown = 15;
						Main.projectile[ize2].netUpdate = true;


						IdgProjectile.Sync(ize2);
					}

				}

			}

		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.tileCollide = false;
			projectile.timeLeft = 38;
		}

	}

	public class RainbowBolt : ModProjectile
	{
		Color rainbows = Color.White;
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.magic = true;
			projectile.timeLeft = 200;
			projectile.light = 0.1f;
			projectile.extraUpdates = 300;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
			rainbows = Main.hslToRgb(((Main.rand.NextFloat() * 0.40f) + Main.GlobalTime) % 1f, 0.9f, 0.75f);
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enmity Bolt");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			for (int k = 0; k < 4; k++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 1f;
				int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, rainbows, 2.0f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].velocity *= 0.5f;
			}


			return true;
		}

		public override void AI()
		{
			Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
			int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, rainbows, 1.5f);
			Main.dust[num655].noGravity = true;
			Main.dust[num655].velocity *= 0.5f;

			if (projectile.localAI[1] == 0f)
			{
				projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			}
		}
	}


}