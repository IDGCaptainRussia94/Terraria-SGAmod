using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;


namespace SGAmod.Items.Weapons.Auras
{

	public class StoneBarrierStaff : AuraStaffBase
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stone Barrier Staff");
			Tooltip.SetDefault("Summons a Stone Golem to project an Auric Barrier around the player");
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


			if (thetarget > -1 && Main.projectile[thetarget].active && Main.projectile[thetarget].type==item.shoot)
			{
				AuraMinionStone shoot = Main.projectile[thetarget].modProjectile as AuraMinionStone;
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Power Level: "+ shoot.thepower));
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Passive: Grants 1 defense to players per Power Level"));
				tooltips.Add(new TooltipLine(mod, "Bonuses", "Lv1: Applies Dryad's bane onto enemies, Dryad's Blessing onto town NPCs"));

				if (shoot.thepower >= 2.0)
					tooltips.Add(new TooltipLine(mod, "Bonuses", "Lv2: Applies Dryad's Blessing onto Players"));
				if (shoot.thepower >= 3.0)
					tooltips.Add(new TooltipLine(mod, "Bonuses", "Lv3: Grants Players immunity to Poisoned, Venom, and Oozed"));

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
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.rare = 1;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = mod.BuffType("AuraBuffStone");
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<AuraMinionStone>();
		}

	}

	public class AuraMinionStone : AuraMinion
	{

		protected override int BuffType => ModContent.BuffType<AuraBuffStone>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			Main.projFrames[projectile.type] = 1;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
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

		public override void AuraAI(Player player)
		{
			Lighting.AddLight(projectile.Center, Color.ForestGreen.ToVector3() * 0.78f);
		}

		public override void InsideAura<T>(T type, Player player)
		{
			if (type is NPC)
			{
				NPC himas = (type as NPC);
				himas.AddBuff(himas.townNPC || himas.friendly ? BuffID.DryadsWard : BuffID.DryadsWardDebuff, 3);
			}
			if (type is Player alliedplayer && player.IsAlliedPlayer(alliedplayer))
			{
				alliedplayer.statDefense += (int)thepower;
				if (thepower >= 2)
				{
					alliedplayer.AddBuff(BuffID.DryadsWard, 2);
				}
				if (thepower >= 3)
				{
					Player player2 = (type as Player);
					if (player2.HasBuff(BuffID.Poisoned))
						player2.DelBuff(player.FindBuffIndex(BuffID.Poisoned));
					if (player2.HasBuff(BuffID.Venom))
						player2.DelBuff(player.FindBuffIndex(BuffID.Venom));
					if (player2.HasBuff(BuffID.OgreSpit))
						player2.DelBuff(player.FindBuffIndex(BuffID.OgreSpit));
				}
			}
		}

		public override void AuraEffects(Player player,int type)
		{

			for (float i = 0; i < 360; i += 360f / projectile.minionSlots)
			{
				float angle = MathHelper.ToRadians(i + projectile.localAI[0] * 2f);
				Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
				Vector2 loc = loc2 * thesize;


				if (type == 0)
				{
					float velmul = Main.rand.NextFloat(0.75f, 1f);
					Vector2 vels = loc2.RotatedBy(-90) * 4f* velmul;

					int dustIndex = Dust.NewDust(projectile.Center+loc-new Vector2(6,6), 12,12, DustID.AncientLight, 0, 0, 150, default(Color), 0.75f);
					Main.dust[dustIndex].velocity = vels+player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Lime;
				}

				if (type == 1)
				{
					Texture2D tex = SGAmod.HellionTextures[7];
					int frame = (int)((projectile.localAI[0] + (i/3f))/5f);
					frame %= 5;

					Main.spriteBatch.Draw(tex, (projectile.Center+ loc) - Main.screenPosition, new Rectangle(0, frame* (int)tex.Height / 5, tex.Width, (int)tex.Height/5), Color.White, angle+MathHelper.ToRadians(90), new Vector2(tex.Width/2f, (tex.Height/5f)/2f), projectile.scale,SpriteEffects.None, 0f);

				}

			}

			if (type == 0)
			{
				for (float i = 0; i < 5f; i += 1f)
				{
					float angle = MathHelper.ToRadians(Main.rand.NextFloat(0, 360));
					Vector2 loc2 = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 loc = loc2 * thesize;

					Vector2 vels = loc2.RotatedBy(-90) * 0f;

					int dustIndex = Dust.NewDust(projectile.Center + loc, 0, 0, DustID.AncientLight, 0, 0, 150, default(Color), 0.65f);
					Main.dust[dustIndex].velocity = vels + player.velocity;
					Main.dust[dustIndex].noGravity = true;
					Main.dust[dustIndex].color = Color.Lime;
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			AuraEffects(Main.player[projectile.owner], 1);
			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/Auras/StoneGolem");
			spriteBatch.Draw(tex, projectile.Center+new Vector2(0,-32+(float)Math.Sin(projectile.localAI[0]/30f)*4f)-Main.screenPosition, null, lightColor, 0, new Vector2(tex.Width, tex.Height)/2f, projectile.scale, Main.player[projectile.owner].direction > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			return false;
		}

	}

	public class AuraBuffStone : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Stone Golem Aura");
			Description.SetDefault("A stone golem projects the blessing of the dryad!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			return base.Autoload(ref name, ref texture);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("AuraMinionStone")] > 0)
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