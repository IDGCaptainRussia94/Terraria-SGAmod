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
	public class JellybruHelmet : MisterCreeperHead, IDevArmor
	{
		public override bool Autoload(ref string name)
		{
			if (GetType() == typeof(JellybruHelmet))
			{
				SGAPlayer.PostUpdateEquipsEvent += SetBonus;
			}
			return true;
		}
		public override Color AwakenedColors => Color.Purple;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jellybru's Mask");
		}
		public override void InitEffects()
		{
			item.defense = 8;
			item.rare = ItemRarityID.Purple;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
		{
			if (sgaplayer.jellybruSet)
			{
				Player player = sgaplayer.player;

				float thepercent = 0.5f;

				int percentLife = (int)((player.statLifeMax2) * thepercent);

				percentLife = (int)((percentLife * 2) * player.magicDamage);

				sgaplayer.energyShieldAmmountAndRecharge.Item2 += percentLife;
				//Main.NewText(sgaplayer.energyShieldReservation);
				sgaplayer.energyShieldReservation += (1f - sgaplayer.energyShieldReservation) * thepercent;

				sgaplayer.ShieldType = 1001;

				if (!sgaplayer.EnergyDepleted)
				{
					Item itemxx = new Item();
					itemxx.SetDefaults(ItemID.AnkhCharm);
					bool falsebool = false; bool falsebool2 = false; bool falsebool3 = false;
					player.VanillaUpdateAccessory(0,itemxx,false,ref falsebool,ref falsebool2, ref falsebool3);
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
			player.SGAPly().DoTResist += 0.30f;
		}

		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Jellybru", "+14% magic damage and crit chance, +50 Mana"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", "Mana regen is greatly improved, Regen delay is reduced"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.Red, "30% Increased DoT damage taken")));
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
		public class JellybruChestplate : JellybruHelmet, IDevArmor
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
				item.defense = 10;
				item.rare = ItemRarityID.Purple;
				item.lifeRegen = 3;
			}
			public override void AddEffects(Player player)
			{
			SGAPlayer sgaply = player.SGAPly();
				player.magicDamage += 0.16f;
				player.manaCost *= 0.80f;
				player.statManaMax2 += 100;
			player.SGAPly().DoTResist += 0.50f;

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
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.Red, "50% Increased DoT damage taken")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Gain the powers of the nebula pillar!")));
			return tooltips;
			}

	}

		[AutoloadEquip(EquipType.Legs)]
		public class JellybruLeggings : JellybruHelmet, IDevArmor
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
			item.defense = 6;
			item.rare = ItemRarityID.Purple;
		}
		public override void AddEffects(Player player)
		{

			SGAPlayer sgaplayer = player.SGAPly();

			player.magicDamage += 0.15f;
			player.statManaMax2 += 50;
			player.moveSpeed += sgaplayer.EnergyDepleted ? 6f : 2f;
			player.accRunSpeed += sgaplayer.EnergyDepleted ? 6f : 2f;
			player.SGAPly().DoTResist += 0.20f;

		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Jellybru", "+15% magic damage, +50 Mana"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", "Movement and horizontal flight speed increased"));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.Red, "20% Increased DoT damage taken")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "--When Shield Down--")));
			tooltips.Add(new TooltipLine(mod, "Jellybru", Idglib.ColorText(Color.PaleTurquoise, "Gain a great speed increase!")));
			return tooltips;
		}
	}
}