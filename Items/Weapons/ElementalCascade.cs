using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Tools;
using Idglibrary;
using SGAmod.Buffs;
using SGAmod.NPCs.Hellion;

namespace SGAmod.Items.Weapons
{
	public class ElementalCascade : ModItem
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Cascade");
			Tooltip.SetDefault("Unleashes 4 elemental beams in cardinal directions towards the mouse cursor, swapping elements with each fire\nthe beams bounce off walls and are non solid until they stop moving, and deal different debuffs to enemies");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.mana = 15;
			item.width = 40;
			item.height = 40;
			item.useTime = 50;
			item.useAnimation = 50;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 6;
			item.UseSound = SoundID.Item78;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("UnmanedBolt");
			item.shootSpeed = 4f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 5);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 5);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 5);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 5);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.Bookcases);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			projectiletype += 1;
			projectiletype = projectiletype % 4;
			type = mod.ProjectileType("ElementalCascadeShot");

			for (int i = 0; i < 4; i += 1)
			{

				Vector2 speez = new Vector2(speedX, speedY);
				speez=speez.RotatedBy(MathHelper.ToRadians(90 *i));
				Vector2 offset = speez;
				offset.Normalize();
				offset *= 48f;
			int probg = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speez.X, speez.Y, type, damage, knockBack, player.whoAmI, ((i + projectiletype) % 4));
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speez.X, speez.Y).RotatedByRandom(MathHelper.ToRadians(5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
				Main.projectile[probg].netUpdate = true;

				IdgProjectile.Sync(probg);

			}


			return false;

		}


	}
	public class LunarCascade : ElementalCascade
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Cascade");
			Tooltip.SetDefault("Unleashes several beams in a complete circle around the player that travel far and effectively melt enemies\nthe beams bounce off walls and are non solid until they stop moving\nBeams deal different powerful debuffs to enemies");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 60;
			item.magic = true;
			item.mana = 30;
			item.width = 40;
			item.height = 40;
			item.useTime = 5;
			item.useAnimation = 50;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 10;
			item.UseSound = SoundID.Item78;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("UnmanedBolt");
			item.shootSpeed = 8f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ElementalCascade"), 1);
			recipe.AddRecipeGroup("Fragment", 10);
			recipe.AddIngredient(ItemID.LunarBar, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			projectiletype += 1;
			projectiletype = projectiletype % 4;
			type = mod.ProjectileType("LunarCascadeShot");

			Vector2 speez = new Vector2(speedX, speedY);
			speez = speez.RotatedBy(MathHelper.ToRadians((float)player.itemAnimation * (360f / player.itemAnimationMax)));
			Vector2 offset = speez;
			offset.Normalize();
			offset *= 48f;
			int probg = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speez.X, speez.Y, type, damage, knockBack, player.whoAmI, (projectiletype));
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speez.X, speez.Y).RotatedByRandom(MathHelper.ToRadians(5));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			Main.projectile[probg].netUpdate = true;

			IdgProjectile.Sync(probg);

			return false;

		}

	}

	public class HelionCascade : LunarCascade
	{
		int projectiletype = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion's Cascade");
			Tooltip.SetDefault("Unleashes several beams in a complete spiral around the player that travel far, absolutely melting enemies\nthe beams pass through walls and are non solid until they stop moving\nBeams deal different very powerful debuffs to enemies");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 70;
			item.magic = true;
			item.mana = 80;
			item.width = 40;
			item.height = 40;
			item.useTime = 90;
			item.useAnimation = 90;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 10000;
			item.rare = 11;
			item.UseSound = SoundID.Item84;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("HellionCascadeShotPlayer");
			item.shootSpeed = 9f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new HellionItems(mod);
			recipe.AddIngredient(mod.ItemType("LunarCascade"), 1);
			recipe.AddRecipeGroup("Fragment", 10);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 200);
			//recipe.AddIngredient(mod.ItemType("HellionSummon"), 1);
			recipe.AddIngredient(mod.ItemType("DrakeniteBar"), 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override string Texture
		{
			get { return ("Terraria/Item_"+ItemID.SpellTome); }
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			projectiletype += 1;
			projectiletype = projectiletype % 4;

			for (int a = 0; a < 360; a += 360 / 4)
			{
				for (int i = -4; i < 5; i += 8)
				{
					Vector2 speez = new Vector2(speedX, speedY);
					speez = speez.RotatedBy(MathHelper.ToRadians(a+(i>0 ? 45 : 0)));
					Vector2 offset = speez;
					offset.Normalize();
					offset *= 48f;
					int probg = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, speez.X, speez.Y, type, damage*12, knockBack, player.whoAmI, (projectiletype),(float)i/1.5f);
					Main.projectile[probg].friendly = true;
					Main.projectile[probg].hostile = false;
					Main.projectile[probg].netUpdate = true;
					IdgProjectile.Sync(probg);

				}
			}

			return false;

		}

	}

	public class LunarCascadeShot : ElementalCascadeShot
	{
		public override int stopmoving => 240;
		public override int fadeinouttime => 30;

		//public Color[] colors = { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
		//public int[] buffs = { ModContent.BuffType<ThermalBlaze>(), BuffID.ShadowFlame, ModContent.BuffType<AcidBurn>(), BuffID.Ichor };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Cascade");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0.25f;
			projectile.width = 24;
			projectile.timeLeft = 400;
			projectile.height = 24;
			projectile.extraUpdates = 1;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			//buffs = new int[4] { BuffID.Daybreak, mod.BuffType("EverlastingSuffering"), mod.BuffType("AcidBurn"), mod.BuffType("MoonLightCurse") };
			colors = new Color[4] { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
			buffs = new int[4] { mod.BuffType("ThermalBlaze"), BuffID.ShadowFlame, BuffID.CursedInferno, BuffID.Ichor};
		}

	}

	public class HellionCascadeShotPlayer : ElementalCascadeShot
	{
		public override int stopmoving => 540;
		public override int fadeinouttime => 30;
		public Vector2 whereat;

		//public Color[] colors = { Color.Orange, Color.Purple, Color.LimeGreen, Color.Yellow };
		//public int[] buffs = { ModContent.BuffType<ThermalBlaze>(), BuffID.ShadowFlame, ModContent.BuffType<AcidBurn>(), BuffID.Ichor };

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hellion Cascade");
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0.25f;
			projectile.width = 24;
			projectile.timeLeft = 1000;
			projectile.height = 24;
			projectile.extraUpdates = 3;
			projectile.magic = true;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 40;
			buffs = new int[4] { BuffID.Daybreak, mod.BuffType("EverlastingSuffering"), mod.BuffType("AcidBurn"), mod.BuffType("MoonLightCurse") };
		}

		public override void AI()
		{
			if (projectile.velocity.Length() > 0)
			{
				if (whereat == null)
				{
					whereat = Main.player[projectile.owner].Center;
				}
				projectile.ai[1] *= 0.990f;
				float ogspeed = projectile.velocity.Length();
				projectile.velocity=projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.ai[1]), whereat);
				projectile.velocity.Normalize();
				projectile.velocity *= ogspeed;
			}
			base.AI();
		}

	}

	public class ElementalCascadeShot : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental Cascade");
		}

		private List<Vector2> oldPos = new List<Vector2>();
		public Color[] colors = { Color.Orange, Color.Purple, Color.LawnGreen, Color.Aqua };
		public int[] buffs = {BuffID.OnFire,BuffID.ShadowFlame,BuffID.DryadsWardDebuff,BuffID.Frostburn};

		public virtual int stopmoving => 90;
		public virtual int fadeinouttime => 30;
		public virtual int bufftime => 60 * 8;

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1000;
			projectile.light = 0.25f;
			projectile.width = 24;
			projectile.timeLeft = 60 * 3;
			projectile.height = 24;
			projectile.magic = true;
			projectile.tileCollide = true;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			foreach (Vector2 position in oldPos)
			{
				projHitbox.X = (int)position.X;
				projHitbox.Y = (int)position.Y;
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(buffs[(int)projectile.ai[0]],bufftime);
			/*if (this.GetType() == typeof(LunarCascadeShot))
			{
				target.immune[projectile.owner] -= 5;
			}*/
		}

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			target.AddBuff(buffs[(int)projectile.ai[0]],bufftime);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = SGAmod.ExtraTextures[96];
			float fadin = MathHelper.Clamp(1f-((float)projectile.timeLeft-stopmoving) / fadeinouttime, 0.1f,0.75f);
			if (projectile.timeLeft<(int)fadeinouttime)
				fadin = ((float)projectile.timeLeft/ fadeinouttime) *0.75f;
			for (int i = 0; i < oldPos.Count; i += 1)
			{
				Color thecolor = colors[(int)projectile.ai[0]];
			if (GetType()==typeof(HellionCascadeShot) || GetType() == typeof(HellionCascadeShot2) || GetType() == typeof(HellionCascadeShotPlayer))
				thecolor = Main.hslToRgb((((i+ projectile.ai[0]*26f)/80f) + (-Main.GlobalTime / 0.6f))% 1f, 0.85f,0.7f);
				Vector2 drawPos = oldPos[i] - Main.screenPosition;
				spriteBatch.Draw(texture, drawPos, null, Color.Lerp(lightColor, thecolor, 0.75f)* fadin, 1, new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(0.4f, 0.4f), SpriteEffects.None, 0f);
			}
			return false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.velocity.Length() > 0f || projectile.timeLeft < fadeinouttime)
				return false;
			return base.CanHitNPC(target);
		}

		public override bool CanHitPlayer(Player target)
		{
			if (projectile.velocity.Length()>0f || projectile.timeLeft < fadeinouttime)
			return false;
			return base.CanHitPlayer(target);
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + 5; }
		}


		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			{
				Main.PlaySound(SoundID.Item10, projectile.Center);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}

		public override void AI()
		{
			if (projectile.timeLeft < stopmoving+ (fadeinouttime/2))
			{
				projectile.velocity = default(Vector2);
			}
			else
			{
				oldPos.Add(projectile.Center);
			}



		}
	}


}