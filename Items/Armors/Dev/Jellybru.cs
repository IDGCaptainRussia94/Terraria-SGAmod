using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Items.Armors.Dev
{

	[AutoloadEquip(EquipType.Head)]
	public class JellybruHelmet : MisterCreeperHead
	{
		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostUpdateEquipsEvent += SetBonus;
			return true;
		}
		public override Color AwakenedColors => Color.Purple;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellybru's Mask");
		}
		public override void InitEffects()
		{
			item.defense = 12;
			item.rare = 10;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
		{
			if (sgaplayer.jellybruSet)
			{
				Player player = sgaplayer.player;

				//Main.NewText(player.manaCost);

				float thepercent = 1f-MathHelper.Clamp(0.5f,0.5f,1f);

				int percentLife = (int)((player.statLifeMax2) * thepercent);

				percentLife = (int)((percentLife * 2) * player.magicDamage);

				sgaplayer.energyShieldAmmountAndRecharge.Item2 += percentLife;
				sgaplayer.energyShieldReservation += (1f - sgaplayer.energyShieldReservation) * thepercent;
				sgaplayer.ShieldType = 1001;

				if (!sgaplayer.EnergyDepleted)
                {
					Item itemxx = new Item();
					itemxx.SetDefaults(ItemID.AnkhCharm);
					player.VanillaUpdateVanityAccessory(itemxx);
                }

			}

		}

		public override bool DrawHead()
		{
			return false;
		}

		public static Color IDGGlow(Player player, int index)
		{
			return Main.hslToRgb(((Main.GlobalTime + (index / 2f)) * 1.25f) % 1f, 0.8f, 0.75f) * 0.75f;
		}

		public override void AddEffects(Player player)
		{
			player.magicDamage += 0.14f;
			player.magicCrit += 14;
			player.statManaMax2 += 50;
			player.manaRegenBonus += player.SGAPly().EnergyDepleted ? 250 : 100;
			player.manaRegenDelayBonus += 2;

		}

		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Jellybru", "+14% magic damage and crit chance, +50 Mana"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", "Mana regen is greatly improved, Regen delay is reduced"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Mana regen is improved to extreme levels!")));
			return tooltips;
		}

		public override void UpdateEquip(Player player)
		{
			if (!item.vanity)
			{
				AddEffects(player);
			}
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 1;
			item.defense = 0;
			item.vanity = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!item.vanity)
				tooltips = AddText(tooltips);
			tooltips.Add(new TooltipLine(mod, "Jellybru", "Great for impersonating..."));
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 4) % 1f, 0.4f, 0.45f);
			tooltips.Add(new TooltipLine(mod, "Jellybru Dev Item", Idglib.ColorText(c, "Jellybru's dev armor")));
		}
	}

	//sgaplayer.armorglowcolor[0] = delegate (Player player2, int index)
				//{
				////	return IDGHead.IDGGlow(player2, index);
				//};

[AutoloadEquip(EquipType.Body)]
		public class JellybruChestplate : JellybruHelmet
	{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Jellybru's Purpled Padding");
			}
			public override void SetDefaults()
			{
				item.width = 18;
				item.height = 18;
				item.value = 10000;
				item.rare = 1;
				item.defense = 0;
				item.vanity = true;
			}
			public override void InitEffects()
			{
				item.defense = 20;
				item.rare = 10;
				item.lifeRegen = 3;
			}
			public override void AddEffects(Player player)
			{
			SGAPlayer sgaply = player.SGAPly();
				player.magicDamage += 0.16f;
				player.manaCost *= 0.80f;
				player.statManaMax2 += 100;

			if (sgaply.EnergyDepleted)
			{
				player.nebulaLevelDamage = 3;
				player.nebulaLevelLife = 3;
				player.nebulaLevelMana = 3;
			}
		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
			{
			tooltips.Add(new TooltipLine(mod, "Jellybru", "+16% magic damage, +100 Mana"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", "magic cost reduced by 20%, minorly increased Life Regen"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Gain the powers of the nebula pillar!")));
			return tooltips;
			}

	}

		[AutoloadEquip(EquipType.Legs)]
		public class JellybruLeggings : JellybruHelmet
	{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Jellybru's Bre's Breeches");
			}
			public override void SetDefaults()
			{
				item.width = 18;
				item.height = 18;
				item.value = 10000;
				item.rare = 1;
				item.defense = 0;
				item.vanity = true;
			}

			public override void InitEffects()
			{
				item.defense = 8;
				item.rare = 10;
			}
		public override void AddEffects(Player player)
		{

			SGAPlayer sgaplayer = player.SGAPly();

			player.magicDamage += 0.15f;
			player.statManaMax2 += 50;
			player.moveSpeed += sgaplayer.EnergyDepleted ? 6f : 2f;
			player.accRunSpeed += sgaplayer.EnergyDepleted ? 6f : 2f;

		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Jellybru", "+15% magic damage, +50 Mana"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", "Movement and horizontal flight speed increased"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Gain a great speed increase!")));
			return tooltips;
		}
	}
}