using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class Surt : ModItem
	{
		bool altfired=false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Surt");
			Tooltip.SetDefault("A molten blade of a cursed demon lord\nHold left click to lift the sword by the blade end and release to plunge it into the ground, unleashing a molten eruption!\nHold for longer for a stronger effect, you will be set on fire\nHold Right click to rapidly swing, throwing out waves of immense heat. You swing faster while on fire.\nTargets hit by the blade are set on fire.");
			Item.staff[item.type] = true; 
		}
		
		public override void SetDefaults()
		{
			item.damage = 150;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 40;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 5;
			Item.sellPrice(0, 50, 0, 0);
			item.rare = 8;
	        item.UseSound = SoundID.Item1;
	        item.shoot=mod.ProjectileType("SurtCharging");
	        item.shootSpeed=30f;
			item.useTurn = true;
	     	item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new StarMetalRecipes(mod);
			recipe.AddIngredient(mod.ItemType("ClayMore"), 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddIngredient(mod.ItemType("PrimordialSkull"), 1);
			recipe.AddIngredient(ItemID.HellstoneBar, 15);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			target.AddBuff(BuffID.OnFire, 60 * (GetType()==typeof(BrimflameHarbinger) ? 8 : 5));
		}

		public override bool CanUseItem(Player player)
		{

			altfired = player.altFunctionUse == 2 ? true : false;

			if (!altfired)
			{
				item.autoReuse = true;
				item.channel = true;
				item.useStyle = 5;
				item.noMelee = true;
				item.noUseGraphic = true;
				if (!Main.dedServ)
				{
					item.GetGlobalItem<ItemUseGlow>().glowTexture = null;
				}
			}
			else
			{
				item.autoReuse = false;
				item.channel = false;
				item.noMelee = false;
				item.useStyle = 1;
				item.noUseGraphic = false;
				if (!Main.dedServ)
				{
					item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Surt_Glow");
				}
			}

			return true;
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.HasBuff(BuffID.OnFire))
			return 2f;
			return 1f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (altfired)
			{
				float speed = 2.0f;
				float numberProjectiles = 1;
				float rotation = MathHelper.ToRadians(8);
				Vector2 speedz = new Vector2(speedX, speedY);
				speedz.Normalize(); speedz *= 30f; speedX = speedz.X; speedY = speedz.Y;

				position += Vector2.Normalize(speedz) * 45f;
				Main.PlaySound(SoundID.Item, player.Center,45);
				for (int i = 0; i < numberProjectiles; i++)
				{
					Vector2 perturbedSpeed = (speedz * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
					int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("HeatWave"), (int)((float)damage * 0.20f), knockBack / 3f, player.whoAmI);
					Main.projectile[proj].melee = true;
					Main.projectile[proj].magic = false;
					Main.projectile[proj].netUpdate = true;
					IdgProjectile.Sync(proj);

					//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}

			}
			return !altfired;
		}


	public override void MeleeEffects(Player player, Rectangle hitbox)
	{

	//if (player.ownedProjectileCounts[mod.ProjectileType("TrueMoonlightCharging")]>0)
	//return;

		for (int num475 = 0; num475 < 5; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("HotDust"));
		Main.dust[dust].scale=0.5f+(((float)num475)/3.5f);
		Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
		Main.dust[dust].velocity=(randomcircle/2f)+((player.itemRotation.ToRotationVector2()*2f).RotatedBy(MathHelper.ToRadians(-90)));
		Main.dust[dust].noGravity=true;
		}

		Lighting.AddLight(player.position, 0.9f, 0.1f, 0.1f);
	}
	
	


	}

	public class BrimflameHarbinger : Surt
	{
		bool altfired = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Brimflame Harbinger");
			Tooltip.SetDefault("'It's Brimflame, not Brimestone Flames!, and its one of the few times Hellstone bars are OK to farm!'" +
				"\nHold left click to lift the sword by the blade end and release to plunge it into the ground, unleashing an EXTREME molten eruption!\nHold for longer for a stronger effect, you will be set on fire\nHold Right click to rapidly swing, throwing out waves of Brimflame blasts and immense heat. You swing faster while on fire.\nTargets hit by the blade are set on fire for longer.");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 160;
			item.melee = true;
			item.width = 54;
			item.crit = 15;
			item.height = 54;
			item.useTime = 25;
			item.useAnimation = 15;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value=Item.sellPrice(0, 25, 0, 0);
			item.rare = 10;
			item.UseSound = SoundID.Item1;
			item.shoot = mod.ProjectileType("BrimflameCharging");
			item.shootSpeed = 30f;
			item.useTurn = true;
			item.autoReuse = false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new StarMetalRecipes(mod);
			recipe.AddIngredient(mod.ItemType("Surt"), 1);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 5);
			recipe.AddIngredient(mod.ItemType("CalamityRune"), 2);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 50);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 2);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override bool CanUseItem(Player player)
		{

			altfired = player.altFunctionUse == 2 ? true : false;

			if (!altfired)
			{
				item.autoReuse = true;
				item.channel = true;
				item.useStyle = 5;
				item.noMelee = true;
				item.noUseGraphic = true;
				if (!Main.dedServ)
				{
					item.GetGlobalItem<ItemUseGlow>().glowTexture = null;
				}
			}
			else
			{
				item.autoReuse = false;
				item.channel = false;
				item.noMelee = false;
				item.useStyle = 1;
				item.noUseGraphic = false;
				if (!Main.dedServ)
				{
					item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/BrimflameHarbinger_Glow");
				}
			}

			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			if (altfired)
			{
				float speed = 0.5f;
				float numberProjectiles = 3;
				float rotation = MathHelper.ToRadians(20);
				Vector2 speedz = new Vector2(speedX, speedY);
				speedz.Normalize(); speedz *= 30f; speedX = speedz.X; speedY = speedz.Y;

				position += Vector2.Normalize(speedz) * 45f;
				Main.PlaySound(SoundID.Item, player.Center, 45);
				for (int i = 0; i < numberProjectiles; i++)
				{
					if (i != 1)
					{
						Vector2 perturbedSpeed = (speedz * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))).RotatedBy(MathHelper.Lerp(-rotation/4f, rotation/4f, (float)Main.rand.Next(0, 100) / 100f)); // Watch out for dividing by 0 if there is only 1 projectile.
						int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("HeatWave"), (int)((float)damage * 0.5f), knockBack / 3f, player.whoAmI);
						Main.projectile[proj].melee = true;
						Main.projectile[proj].magic = false;
						Main.projectile[proj].netUpdate = true;
						IdgProjectile.Sync(proj);
					}

					Vector2 perturbedSpeed2 = (speedz * speed).RotatedBy(MathHelper.Lerp(-rotation/2, rotation/2f, i / (numberProjectiles - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
					int proj2 = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed2.X * 2f, perturbedSpeed2.Y * 2f, mod.ProjectileType("BoulderBlast"), (int)((float)damage * 1f), knockBack / 3f, player.whoAmI);
					Main.projectile[proj2].melee = true;
					Main.projectile[proj2].magic = false;
					Main.projectile[proj2].timeLeft = 15;
					Main.projectile[proj2].usesLocalNPCImmunity = true;
					Main.projectile[proj2].localNPCHitCooldown = -1;
					Main.projectile[proj2].netUpdate = true;
					IdgProjectile.Sync(proj2);

					//NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, proj);
				}

			}
			return !altfired;
		}


		public override void MeleeEffects(Player player, Rectangle hitbox)
		{

			//if (player.ownedProjectileCounts[mod.ProjectileType("TrueMoonlightCharging")]>0)
			//return;

			for (int num475 = 0; num475 < 5; num475++)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 235);
				Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				Main.dust[dust].velocity = (randomcircle / 2f) + ((player.itemRotation.ToRotationVector2() * 2f).RotatedBy(MathHelper.ToRadians(-90)));
				Main.dust[dust].noGravity = true;
			}

			Lighting.AddLight(player.position, 0.9f, 0.1f, 0.1f);
		}


	}
	public class SurtWave3 : SurtWave
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SurtWave");
		}
	}
	public class SurtWave : ModProjectile
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("SurtWave");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/CrateBossWeaponThrown"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 6;
			height = 2;
			fallThrough = false;
			return true;
		}

		public override void SetDefaults()
		{
			projectile.timeLeft = 200;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.extraUpdates = 30;
		}

		public override void AI()
		{
			//int rayloc = Idglib.RaycastDown((int)(projectile.Center.X + 4) / 16, (int)(projectile.Center.Y - 36f) / 16) * 16;
			//if ((rayloc - 16) - (projectile.position.Y) > 70 || (rayloc - 16) - (projectile.position.Y) < -30)
				//projectile.Kill();
			//projectile.position.X += projectile.velocity.X*1;
			//projectile.position.Y += -16;

			projectile.ai[0] += 1;

			if (projectile.ai[0] % 4 == 0 && GetType()==typeof(SurtWave3))
			{
				Vector2 where = new Vector2(Main.rand.Next(-60, 60), Main.rand.Next(-180, 180));
				int proj2 = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y - 128)+ where, new Vector2(0, -8), mod.ProjectileType("BoulderBlast"), (int)((float)projectile.damage * 0.75f), projectile.knockBack / 3f, projectile.owner);
				Main.projectile[proj2].melee = true;
				Main.projectile[proj2].magic = false;
				Main.projectile[proj2].usesLocalNPCImmunity = true;
				Main.projectile[proj2].localNPCHitCooldown = -1;
				Main.projectile[proj2].netUpdate = true;
				IdgProjectile.Sync(proj2);
			}

			if (projectile.ai[0] % 24 == 0)
			{
				int thisoned = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y-128), new Vector2(0, 1), ModContent.ProjectileType<SurtWave2>(), projectile.damage, projectile.knockBack * 1f, Main.player[projectile.owner].whoAmI);
				//Main.projectile[thisoned].timeLeft = 25;
				//Main.projectile[thisoned].melee = true;
			}
		}

	}

	public class SurtWave2 : ModProjectile
	{

		int leveleffect = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Surt Wave 2");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/CrateBossWeaponThrown"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 6;
			height = 2;
			fallThrough = false;
			return true;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.timeLeft = 600;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.extraUpdates = 300;
			projectile.velocity = new Vector2(0, 1);
		}

		public override void Kill(int timeLeft)
		{
			if (timeLeft > 300 && timeLeft<590)
			{
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 74, 0.5f, 0);
				int thisoned = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, -6), ModContent.ProjectileType<SurtExplosion>(), (int)((float)projectile.damage * 0.15f), projectile.knockBack * 0.5f, Main.player[projectile.owner].whoAmI);
			}
		}

	}


	public class SurtRocks : LavaRocks
	{
		public float trans2 = -0.2f;
		public override bool hitwhilefalling => true;
		public override float trans => Math.Max(0,trans2);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Rocks");
		}

		public override void AI()
		{
			trans2 = Math.Min(1f, trans2 + 0.05f);
			base.AI();
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
			projectile.magic = false;
			projectile.melee = true;
		}

	}

	public class SurtExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Boom");
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Vector2 basepoint = (new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(projectile.localAI[0]) }.ToVector2())+new Vector2(0,8);

			Texture2D tex = SGAmod.ExtraTextures[97];
			spriteBatch.Draw(tex, basepoint - Main.screenPosition, null, Color.White*((float)projectile.timeLeft/25f), projectile.rotation, new Vector2(tex.Width / 2f, tex.Height), new Vector2(projectile.scale, projectile.scale), SpriteEffects.None, 0f);

			return false;
		}

		public override void SetDefaults()
		{
			projectile.width = 96;
			projectile.height = 96;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.timeLeft = 25;
			projectile.melee = true;
			projectile.localNPCHitCooldown = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.netImportant = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Surt"); }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.immune[projectile.owner] = 2;
		}

		public override void AI()
		{
				if (projectile.localAI[1] == 0) {
					HalfVector2 half = new HalfVector2(projectile.Center.X, projectile.Center.Y);
					projectile.localAI[0] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
					projectile.localAI[1] = 1;
				SGAmod.AddScreenShake(24f, 320, projectile.Center);
			}
			projectile.scale += 0.2f;

			projectile.ai[0] += 1;

			Vector2 basepoint = (new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(projectile.localAI[0]) }.ToVector2()) + new Vector2(0, -8);


			Lighting.AddLight(basepoint, 2f*(Color.Yellow.ToVector3()*((float)projectile.timeLeft / 24f)));
			Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3());

			if (projectile.ai[0]<4)
			{

				int thisoned = Projectile.NewProjectile(new Vector2(projectile.Center.X, basepoint.Y), new Vector2(Main.rand.NextFloat(-5f, 5f), Main.rand.NextFloat(-8f, -3f)-(projectile.ai[0] / 0.75f)), ModContent.ProjectileType<SurtRocks>(), projectile.damage*6, projectile.knockBack * 2f, Main.player[projectile.owner].whoAmI);
				IdgProjectile.Sync(thisoned);

			}

		}

	}
	public class BrimflameCharging : SurtCharging
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/BrimflameHarbinger"); }
		}


	}
	public class SurtCharging : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("You should not see this");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Surt"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = ModContent.GetTexture(this.Texture);
			spriteBatch.Draw(tex, (projectile.Center + new Vector2(projectile.direction * 10,-24))-Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
			tex = ModContent.GetTexture("SGAmod/Items/GlowMasks/Surt_Glow");
			if (GetType()==typeof(BrimflameCharging))
			tex = ModContent.GetTexture("SGAmod/Items/GlowMasks/BrimflameHarbinger_Glow");

			spriteBatch.Draw(tex, (projectile.Center + new Vector2(projectile.direction * 10, -24)) - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(tex.Width / 2f, tex.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);

			return false;
		}

		public override bool? CanHitNPC(NPC target)
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
			projectile.timeLeft = 90;
			projectile.penetrate = -1;
			projectile.netImportant = true;
			aiType = 0;
		}

		public override void AI()
		{
			Vector2 mousePos = Main.MouseWorld;
			Player player = Main.player[projectile.owner];

			if (projectile.ai[0] > 1000f || player.dead)
			{
				projectile.Kill();
			}
			projectile.localAI[1] += 1f;
			if ((!player.channel || projectile.ai[0] > 0))
			{


				projectile.ai[0] += 1;
				projectile.netUpdate = true;
				projectile.velocity /= 2f;
				if (projectile.ai[0] < 2)
				{
					projectile.ai[1] = projectile.timeLeft;
					if (projectile.ai[1] < 130)
					{
						player.itemAnimation = 5;
						player.itemTime = 5;
						projectile.Kill();
						return;
					}

					projectile.timeLeft /= 4;
					projectile.timeLeft += 90;
				}
				else
				{

				if (projectile.ai[0] > 20f && projectile.ai[0]<1000f)
					{
						projectile.ai[0] = 1000f;

						int rayloc = Idglib.RaycastDown((int)(player.Center.X) / 16, (int)(player.Center.Y - 8f) / 16) * 16;
						if (!((rayloc - 16) - (projectile.position.Y) > 30 || (rayloc - 16) - (projectile.position.Y) < -30))
						{

							//player.Hurt(PlayerDeathReason.ByCustomReason("Testing"), 5, projectile.direction, true, false, false, -1);
							Vector2 pos = player.Center;//new Vector2((int)(player.Center.X/16), (int)(player.Center.Y/16)) * 16;

							float damagemul = 1f+ (projectile.ai[1]-130)/250f;

							int thisoned = Projectile.NewProjectile(pos, new Vector2(projectile.direction * 4, 0), GetType() == typeof(BrimflameCharging) ? ModContent.ProjectileType<SurtWave3>() : ModContent.ProjectileType<SurtWave>(), (int)(projectile.damage* damagemul)*3, projectile.knockBack * 2f, Main.player[projectile.owner].whoAmI);
							Main.projectile[thisoned].timeLeft = (int)projectile.ai[1];
							Main.PlaySound(SoundID.Item, player.Center, 74);

						}

					}

				}
			}
			else
			{
				if (projectile.timeLeft < 300)
				{
					projectile.timeLeft += (GetType() == typeof(BrimflameCharging) ? 3 : 2);
				}
				if (projectile.localAI[1]%10==0 && projectile.timeLeft>129 && projectile.timeLeft < 300)
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 102, 0.25f, -0.5f+(float)projectile.timeLeft / 250f);



			}
			projectile.rotation = MathHelper.ToRadians(135);
			// Multiplayer support here, only run this code if the client running it is the owner of the projectile
			if (projectile.owner == Main.myPlayer)
			{
				Vector2 diff = mousePos - player.Center;
				diff.Normalize();

				if (projectile.ai[0] < 1f)
				projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
				projectile.netUpdate = true;
				projectile.Center = mousePos;
			}
			if (projectile.ai[0] < 1f)
			projectile.velocity = new Vector2(projectile.direction*2, -((float)projectile.timeLeft - 60f))*0.15f;

			projectile.Center = player.Center;
			player.heldProj = projectile.whoAmI;
			player.itemAnimation = 60;
			player.itemTime = 60;
			player.AddBuff(BuffID.OnFire, (int)MathHelper.Clamp((int)(((float)projectile.timeLeft - 260f) * 5f),1,450));
			int dir = projectile.direction;
			player.itemRotation = (MathHelper.ToRadians(90)+MathHelper.ToRadians(projectile.velocity.Y*5f)) * dir;
			player.ChangeDir(dir);


		}

	}



		}