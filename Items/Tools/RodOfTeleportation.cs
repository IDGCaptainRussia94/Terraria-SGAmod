using Idglibrary;
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{
	public class RodOfTeleportation : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod Of Teleportation");
			Tooltip.SetDefault("Left Click to teleport, costs Action Cooldown Stacks instead of Health\nStill applies Chaos State\nCan telefrag enemies when you don't have Chaos State, grants a small period of IFrames\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds each"));
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			item.width = 24;
			item.height = 32;
			item.useTime = 70;
			item.useTime = 20;
			item.useAnimation = 20;
			item.noMelee = true;
			item.damage = 1000;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.UseSound = SoundID.Item6;
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ProjectileID.TruffleSpore;
			item.shootSpeed = 12f;
		}

		public override bool CanUseItem(Player player)
		{
			return Weapons.Shields.DiscordShield.CanRoDTeleport(player) && player.SGAPly().AddCooldownStack(60*7, 1, testOnly: true);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int invtime = 10;
			bool chaos = player.HasBuff(BuffID.ChaosState);

            Weapons.Shields.DiscordShield.RoDTeleport(player,0);

			if (!chaos)
			{
				foreach (NPC enemy in Main.npc.Where(testby => testby.active && !testby.friendly && !testby.dontTakeDamage && testby.Hitbox.Intersects(player.Hitbox)))
				{
					int damag2e = Main.DamageVar((int)damage);
					enemy.StrikeNPC(damag2e, 20, player.direction, Main.hardMode);
				}
				invtime = 30;
			}

			player.SGAPly().realIFrames = Math.Max(invtime, player.SGAPly().realIFrames);

			player.SGAPly().AddCooldownStack(60*60);
			return false;
		}
	}
}
