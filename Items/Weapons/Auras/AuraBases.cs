using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Weapons.Auras
{
	public class AuraStaffBase : ModItem
	{
		public int prog = -1;
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(AuraStaffBase);
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
			item.buffType = mod.BuffType("AuraBuffStone");
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = mod.ProjectileType("AuraMinionStone");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
				tooltips.Add(new TooltipLine(mod, "AuraUse", "Reusing the item consumes an extra minion slot and increases the current Aura Strength"));
			//tooltips.Add(new TooltipLine(mod, "AuraUse", "Alt Fire to relocate the Aura, Alt Fire again to return it to you"));
		}

		public override bool CanUseItem(Player player)
		{
			return (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) >= 1);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			if (player.ownedProjectileCounts[type] > 0)
			{
				for(int i = 0; i < Main.maxProjectiles; i += 1)
				{
					if (Main.projectile[i].type == item.shoot && Main.projectile[i].owner == player.whoAmI && Main.projectile[i].active)
					{
						if (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) >= 1)
						{
							Main.projectile[i].ai[0] += 1f;
							Main.projectile[i].netUpdate = true;
						}
					}
				}
				//Main.NewText("" + (player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots));
				return false;
			}

			return true;
		}
	}

	public class AuraMinion : ModProjectile
	{
		protected virtual int BuffType => ModContent.BuffType<AuraBuffStone>();
		protected virtual float AuraSize => 160;
		protected float thesize = 0;
		public float thepower = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Midas Portal");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[projectile.type] = 1;
			// This is necessary for right-click targeting
			//ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public virtual float CalcAuraPower(Player player)
		{
			thepower = (player.minionDamage * (1f + (projectile.minionSlots / 3f)));
			return thepower;
		}

		public virtual float CalcAuraSize(Player player)
		{
			return AuraSize * (float)Math.Pow((double)CalcAuraPower(player),0.80);
		}

		public override void SetDefaults()
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

		public virtual void InsideAura<T>(T type, Player player) where T : Entity
		{


		}

		public virtual void AuraEffects(Player player, int type)
		{

		}

		public virtual void AuraAI(Player player)
		{
			Lighting.AddLight(projectile.Center, Color.ForestGreen.ToVector3() * 0.78f);
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			if (!player.dead && player.active)
			{
				for (int i = 0; i < Main.maxPlayers; i += 1)
				{
					if (Main.player[i].active && !Main.player[i].dead && (Main.player[i].Center - projectile.Center).Length() < thesize)
						InsideAura(Main.player[i], player);
				}

				for (int i = 0; i < Main.maxNPCs; i += 1)
				{
					if (Main.npc[i].active && Main.npc[i].active && (Main.npc[i].Center - projectile.Center).Length() < thesize)
						InsideAura(Main.npc[i], player);
				}
			}

				return true;
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			Player player = Main.player[projectile.owner];
			if (player.HasBuff(BuffType))
			{
				projectile.timeLeft = 2;
			}
			if (player.dead || !player.active)
			{
				player.ClearBuff(BuffType);
				return;
			}

			projectile.Center = player.Center;

			projectile.minionSlots = projectile.ai[0]+1;

			thesize=CalcAuraSize(player);

			AuraAI(player);

			AuraEffects(player,0);
			Vector2 gothere = player.Center;
			projectile.localAI[0] += 1;


		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Auras/StoneGolem"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

	}

}