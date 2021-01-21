using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons.Auras
{
	public class BeserkerAuraStaff : AuraStaffBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Berserker Aura Staff");
			Tooltip.SetDefault("Summons Beserker Gauntlets around the player to boost their attack power\nBut in your rage, you forget to breath");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			int thetarget = -1;
			if (Main.LocalPlayer.ownedProjectileCounts[item.shoot] > 0)
			{
				for (int i = 0; i < Main.maxProjectiles; i += 1)
				{
					if (Main.projectile[i].active && Main.projectile[i].type == item.shoot && Main.projectile[i].owner == Main.LocalPlayer.whoAmI)
					{
						thetarget = i;
						break;
					}
				}
			}

			if (thetarget > -1 && Main.projectile[thetarget].active && Main.projectile[thetarget].type == item.shoot)
			{
				AuraMinionBeserker shoot = Main.projectile[thetarget].modProjectile as AuraMinionBeserker;
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Power Level: " + shoot.thepower));
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Power scales the breath drain and damage bonuses"));
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Grants 5% increased damage to allies in range per Power Level"));
				tooltips.Add(new TooltipLine(mod, "Bonuses", "However their breath drains faster"));
			}
		}

		public override void SetDefaults()
		{
			item.damage = 0;
			item.knockBack = 3f;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("AuraBuffBeserker");
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<AuraMinionBeserker>();
		}

	}

	public class AuraMinionBeserker : AuraMinion
	{

		protected override int BuffType => ModContent.BuffType<AuraBuffBeserker>();
		protected override float AuraSize => 60;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			Main.projFrames[projectile.type] = 1;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.tileCollide = false;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
		}

		public override float CalcAuraPower(Player player)
		{
			thepower = 1f+(player.minionDamage * (projectile.minionSlots / 2f));
			return thepower;
		}

		public override void AuraAI(Player player)
		{
			Lighting.AddLight(projectile.Center, Color.Red.ToVector3() * 0.78f);
		}

		public override void InsideAura<T>(T type, Player player)
		{

			if (type is Player)
			{
				SGAPlayer theply = (type as Player).SGAPly();
				theply.beserk[0] = 5;
				theply.beserk[1] = (int)((float)thepower*1f);

				theply.player.meleeDamage += (float)(theply.beserk[1] * 0.05f);
				theply.player.magicDamage += (float)(theply.beserk[1] * 0.05f);
				theply.player.minionDamage += (float)(theply.beserk[1] * 0.05f);
				theply.player.rangedDamage += (float)(theply.beserk[1] * 0.05f);
				theply.player.Throwing().thrownDamage += (float)(theply.beserk[1] * 0.05f);
				SGAmod.BoostModdedDamage(theply.player, (float)(theply.beserk[1] * 0.05f),0);

			}

		}

		public override void AuraEffects(Player player, int type)
		{

			for (float i = 0; i < 360; i += 360f / projectile.minionSlots)
			{
				float angle = MathHelper.ToRadians(i + projectile.localAI[0] * -2f);
				Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				Vector2 loc = loc2 * thesize;
				Vector2 loc3 = loc2;
				loc3.Normalize();

				if (type == 1)
				{
					Texture2D tex = Main.itemTexture[ItemID.FireGauntlet];
					int frame = (int)((projectile.localAI[0] + (i / 3f)) / 5f);
					frame %= 1;

					Main.spriteBatch.Draw(tex, (projectile.Center + loc) - Main.screenPosition, new Rectangle(0, frame * (int)tex.Height / 1, tex.Width, (int)tex.Height / 1), Color.Red*0.5f, angle + MathHelper.ToRadians(90), new Vector2(tex.Width / 2f, (tex.Height / 5f) / 2f), projectile.scale, SpriteEffects.None, 0f);

				}

			}

			if (type == 0)
			{
				for (float i = 0; i < 8f; i += 1f)
				{
					float angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
					Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 loc = loc2 * thesize;

					Vector2 vels = loc2.RotatedBy(-90) * 0f;

					int dustIndex = Dust.NewDust(projectile.Center + loc, 0, 0, DustID.Fire, 0, 0, 150, default(Color), 0.75f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Red;
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			AuraEffects(Main.player[projectile.owner], 1);
			return false;
		}

	}

	public class AuraBuffBeserker : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Beserker Aura");
			Description.SetDefault("The Aura enrages your attacks, but you forget to breath");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("AuraMinionBeserker")] > 0)
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