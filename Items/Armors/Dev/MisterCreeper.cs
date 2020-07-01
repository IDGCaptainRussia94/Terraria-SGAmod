using Microsoft.Xna.Framework;
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
	public class MisterCreeperHead : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Crowning Attire");
		}
		public virtual void InitEffects()
		{
			item.defense = 35;
			item.rare = 10;
		}
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			tag["vanity"] = item.vanity;
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			item.vanity = tag.GetBool("vanity");
			if (!item.vanity)
			{
				item.vanity = false;
				InitEffects();
			}
		}
		public override void NetSend(BinaryWriter writer)
		{
			BitsByte flags = new BitsByte();
			flags[0] = item.vanity;
			writer.Write(flags);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			bool beforevanity = item.vanity;
			item.vanity = reader.ReadBoolean();
			if (beforevanity != item.vanity && item.vanity==false)
			{
			InitEffects();
			}

		}

		public virtual void AddEffects(Player player)
		{
			player.thrownCost33 = true;
			player.thrownDamage += 0.25f;
			player.meleeDamage += 0.25f;
			player.meleeSpeed += 0.25f;
			player.GetModPlayer<SGAPlayer>().ThrowingSpeed += 0.15f;
		}
		public virtual List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "25% increased melee and throwing damage"));
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "33% to not consume thrown items, 25% increased melee swing speed"));
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "15% increased throwing rate"));
			return tooltips;
		}

		public override void UpdateInventory(Player player)
		{
			if (item.vanity)
			{
				if (player.GetModPlayer<SGAPlayer>().devpower>0)
				{
					item.vanity = false;
					//Client Side
					if (Main.myPlayer == player.whoAmI)
					{
						CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height), Color.Orange, "???!!!", false, false);
						Main.PlaySound(29, (int)player.position.X, (int)player.position.Y, 105, 1f, -0.6f);
					}
					InitEffects();




				}


			}
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
			item.defense=0;
			item.vanity = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (!item.vanity)
			tooltips=AddText(tooltips);
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "Great for impersonating an explosive fella who draws too many swords"));
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 4) % 1f, 0.4f, 0.45f);
			tooltips.Add(new TooltipLine(mod, "IDG Dev Item", Idglib.ColorText(c, "MisterCreeper's (Legecy) Dev Armor")));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MisterCreeperBody : MisterCreeperHead
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Slick Suit");
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
			item.defense = 50;
			item.rare = 10;
			item.lifeRegen = 10;
		}
		public override void AddEffects(Player player)
		{
			player.thrownCrit += 20;
			player.meleeCrit += 10;
			player.noKnockback = true;
			player.endurance += 0.15f;
		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "10% Increased Melee Crit, 20% increased throwing Crit"));
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "Immunity to Knockback, greatly increased Life Regen"));
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "Extra 15% Endurance"));
			return tooltips;
		}

	}

	[AutoloadEquip(EquipType.Legs)]
	public class MisterCreeperLegs : MisterCreeperHead
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mister Creeper's Business Casual");
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
			item.defense = 30;
			item.rare = 10;
		}
		public override void AddEffects(Player player)
		{
			player.maxRunSpeed += 2f;
			player.accRunSpeed += 2f;
			player.runAcceleration += 0.5f;
			player.wingTimeMax = (int)((float)player.wingTimeMax*(1.20f));
			player.GetModPlayer<SGAPlayer>().Noselfdamage = true;
		}
		public override List<TooltipLine> AddText(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "Do you don't take ANY self damage (includes fall and explosive damage)"));
			tooltips.Add(new TooltipLine(mod, "MisterCreeper", "Movement speed increased and Flight time improved by 20%"));
			return tooltips;
		}

	}


}