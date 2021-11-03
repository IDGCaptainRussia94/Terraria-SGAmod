using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;
using SGAmod.NPCs.Cratrosity;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod.Items.Weapons
{
	public class Autoclicker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autoclicker");
			Tooltip.SetDefault("Summons Cursers to click on enemies\nClicks may spawn a cookie, more likely with more max sentry summons\nCan pickup the cookie to gain health, minion range, and a click rate buff\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 30 seconds each"));
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 100;
			item.knockBack = 5f;
			item.mana = 5;
			item.width = 32;
			item.height = 32;
			item.useTime = 4;
			item.noUseGraphic = true;
			item.useAnimation = 4;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.noUseGraphic = true;
			//item.UseSound = Main.soundt;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<AutoclickerMinionBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<AutoclickerMinion>();
			item.shootSpeed = 32f;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			Main.PlaySound(SoundID.MenuTick,(int)player.Center.X, (int)player.Center.Y,0);
			player.AddBuff(item.buffType, 2);

			/*foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<AutoclickerMinion>()))
			{
				AutoclickerMinion click = (AutoclickerMinion)proj.modProjectile;
				click.DoClick();
			}*/
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 75);
			recipe.AddRecipeGroup("Fragment", 15);
			recipe.AddIngredient(ItemID.Mouse, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AutoclickerMinion : ModProjectile
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
			DisplayName.SetDefault("Autoclicker");
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
			projectile.friendly = true;
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 1f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
			projectile.knockBack = 8;
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

		Player ThePlayer => Main.player[projectile.owner];
		int clickDelay = 0;

		bool ClickerBoost
        {
            get
            {
				return ThePlayer.HasBuff(ModContent.BuffType<AutoclickerSpeedBuff>());
			}
        }

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			Player player = ThePlayer;

			int attackrate = 40;


			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<AutoclickerMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<AutoclickerMinionBuff>()))
			{
				projectile.timeLeft = 2;
			}
			Vector2 there = player.Center;
			float dist = projectile.ai[1]<1 ? 64 : 8f;
			projectile.localAI[0] += 1;
			projectile.ai[0] += 1;
			projectile.ai[1] -= 1;
			clickDelay -= 1;

			float id = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == projectile.type)
				{ // Make sure the projectile is of the same type as this javelin

					if (i == projectile.whoAmI)
						id = maxus;
					maxus += 1f;

				}
			}
			projectile.localAI[1] = id/maxus;

			NPC them = null;
			Entity focusOn = player;

			if (player.HasMinionAttackTargetNPC)
			{
				them = Main.npc[player.MinionAttackTargetNPC];
				there = them.Center;
				focusOn = them;
			}
			else
			{
				List<NPC> enemies = SGAUtils.ClosestEnemies(projectile.Center, 2200, projectile.ai[1] > 0 ? projectile.Center : player.Center,checkWalls: false);
				if (enemies != null && enemies.Count > 0)
				{
					enemies = enemies.OrderBy(testby => testby.life).ToList();
					them = enemies[(int)id%enemies.Count];
					there = them.Center;
					focusOn = them;
				}
			}

			float angles = ((id / (float)maxus) * MathHelper.TwoPi)-player.SGAPly().timer/150f;
			Vector2 here = new Vector2((float)Math.Cos(angles), (float)Math.Sin(angles)) * dist;
			Vector2 where = there + here;
			Vector2 todist = (where - projectile.Center);// +(focusOn != null ? focusOn.velocity : Vector2.Zero);
			Vector2 todistreal = (there - projectile.Center);

			float lookat = todist.ToRotation();

			if (them == null)
			{
				lookat = (focusOn.Center - projectile.Center).ToRotation();
			}

			if (todistreal.Length() > 0.01f)
			{
				if (todistreal.Length() > 600f)
				{
					projectile.velocity += Vector2.Normalize(todist) *MathHelper.Clamp(todist.Length()/6f,0f,64f);
					projectile.velocity *= 0.940f;

				}
                else
                {
					projectile.Center += todist * (projectile.ai[1] > -(attackrate/(ClickerBoost ? 10f : 5f)) ? 0.25f : 0.98f);
					projectile.velocity *= 0.820f;
				}
			}
			if (projectile.velocity.Length() > 1f)
			{
				float maxspeed = Math.Min(projectile.velocity.Length(), 16 + (todist.Length() / 4f));
				projectile.velocity.Normalize();
				projectile.velocity *= maxspeed;
			}

			if (todistreal.Length() > 160f && (projectile.ai[1] < 0 || them == null))
			{
				if (them != null)
                {
					lookat = 0;
				}
				projectile.rotation = projectile.rotation.AngleLerp(lookat, 0.05f);
			}
			else
			{
				lookat = (focusOn.Center - projectile.Center).ToRotation();
				projectile.rotation = projectile.rotation.AngleLerp(lookat, 0.15f);
				if (them != null)
				{
					if (player.SGAPly().timer % (int)(attackrate * (ClickerBoost ? 0.5f : 1f)) == (int)(((id * (attackrate / maxus)))*(ClickerBoost ? 0.5f : 1f)))
					{
						DoClick();
					}
					if (clickDelay == 0)
					{
						Main.PlaySound(SoundID.MenuTick, (int)projectile.Center.X, (int)projectile.Center.Y, 0);

						if (!them.IsDummy() && Main.rand.Next(500) < player.maxTurrets)
                        {
							bool cookieNearby = false;
							int range = 900 + (ClickerBoost ? 600 : 0);
							foreach(Item item in Main.item.Where(testby => testby.active && testby.type == ModContent.ItemType<ClickerCookie>() && (testby.Center-them.Center).LengthSquared()< (range * range)))
                            {
								cookieNearby = true;
							}
							if (!cookieNearby)
							Item.NewItem(them.Center, ModContent.ItemType<ClickerCookie>(),prefixGiven: PrefixID.Menacing,noGrabDelay: true);
                        }
						int damage = (int)(projectile.damage * (ClickerBoost ? 0.60f : 1f));
						//them.SGANPCs().AddDamageStack(damage,60*5);
						Projectile.NewProjectile(them.Center, projectile.rotation.ToRotationVector2(), ModContent.ProjectileType<AutoclickerClickProj>(), damage,projectile.knockBack,projectile.owner);


						/*int damazz = (Main.DamageVar((float)projectile.damage));
						them.StrikeNPC(damazz, projectile.knockBack, them.direction, false, false);
						player.addDPS(damazz);
						if (Main.netMode != NetmodeID.SinglePlayer)
						{
							NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, them.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
						}*/
					}
				}

			}

			projectile.velocity *= 0.96f;

			Lighting.AddLight(projectile.Center, Color.White.ToVector3() * 0.78f);

		}

		public void DoClick()
		{
			if (projectile.ai[1] < 1 && clickDelay<0)
			{
				projectile.ai[1] = ClickerBoost ? 10 : 20;
				clickDelay = (int)(projectile.ai[1] / 2);
			}
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Autoclicker"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.White;
			if (ClickerBoost)
			{
				for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
				{
					spriteBatch.Draw(tex, drawPos + Vector2.UnitX.RotatedBy(f) * 3f, null, Main.hslToRgb((projectile.localAI[1] + (Main.GlobalTime/3f))%1f,1f,0.75f) * 0.32f, projectile.rotation + MathHelper.PiOver2, drawOrigin, projectile.scale / 2f, SpriteEffects.None, 0f);
				}
			}
			spriteBatch.Draw(tex, drawPos, null, color, projectile.rotation+MathHelper.PiOver2, drawOrigin, projectile.scale/2f, SpriteEffects.None, 0f);
			return false;
		}

	}

	public class AutoclickerClickProj : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Autoclicker");
		}

		public override string Texture => "Terraria/Item_" + ItemID.SugarCookie;

		public sealed override void SetDefaults()
		{
			projectile.width = 72;
			projectile.height = 72;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.hide = true;
			projectile.minion = true;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = 1;
			projectile.knockBack = 8;
			projectile.timeLeft = 2;
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
	}

	public class ClickerCookie : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cookie");
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}
		public override string Texture => "SGAmod/Items/Consumables/ClickerCookie";
		public override void SetDefaults()
        {
			item.maxStack = 1;
        }

		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange += 160;
		}

		public override bool CanPickup(Player player)
        {
			return (player.SGAPly().AddCooldownStack(30, 1, true));
        }

        public override bool OnPickup(Player player)
		{
			if (player.SGAPly().AddCooldownStack(60 * 30, 1))
			{
				player.HealEffect(50);
				player.netLife = true;
				player.statLife += 50;
				player.AddBuff(ModContent.BuffType<AutoclickerSpeedBuff>(),60*20);
				SoundEffectInstance snd = Main.PlaySound(SoundID.Item,(int)player.Center.X, (int)player.Center.Y, 2);
				if (snd != null)
                {
					snd.Pitch = 0.75f;
				}
			}
			return false;
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

        Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			Vector2 drawPos = item.position-Main.screenPosition;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.20f)
			{
				spriteBatch.Draw(inner, drawPos-Vector2.UnitY.RotatedBy(rotation)*10f, null, Color.Yellow * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.25f, i * MathHelper.TwoPi, textureOrigin,(0.5f + 1.75f * (((Main.GlobalTime / 2f) + i) % 1f))*1f, SpriteEffects.None, 0f);
			}

			return true;
		}

	}

	public class AutoclickerSpeedBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cookie Power!");
			Description.SetDefault("Cursers click faster");
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_"+BuffID.SugarRush;
			return true;
		}
	}


	public class AutoclickerMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Autoclicker");
			Description.SetDefault("Portals from Planes of Wealth will fight for you");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MidasMinionBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<AutoclickerMinion>()] > 0)
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