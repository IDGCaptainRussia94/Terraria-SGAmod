using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Buffs
{
	public class InfinityWarStormbreaker : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("IWS");
			Description.SetDefault("Players arn't meant to have this debuff!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().InfinityWarStormbreaker = true;
		}
	}
	public class NinjaSmokedDebuff : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ninja Smoked");
			Description.SetDefault("Enemies more likely to dodge your attacks");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().NinjaSmoked = true;
		}
	}

	public class SunderedDefense : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sundered Defense");
			Description.SetDefault("Your immunity frames are wrecked");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.immuneTime = Math.Max(player.immuneTime - 3, 0);
			player.GetModPlayer<SGAPlayer>().SunderedDefense = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().SunderedDefense = true;
		}
	}
	public class BIPBuff : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Broken Immortality");
			Description.SetDefault("You've lost your godly defense!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class ConsumeHellBuff : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_"+BuffID.OnFire;
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Consumed Hell");
			Description.SetDefault("Seems like the underworld can access the rest of the world via your mouth");
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.AddBuff(BuffID.OnFire, 2);
		}
	}
	public class IceFirePotion : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MatrixBuff";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Fridgeflame Concoction");
			Description.SetDefault("Reduced Damage over time at a cost of losing immunities");
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().IceFire = true;
			player.lavaRose = true;
		}
	}
	public class DankSlow : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dank Slow");
			Description.SetDefault("Players arn't meant to have this debuff!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override bool ReApply(NPC npc, int time, int buffIndex)
		{
			npc.buffTime[buffIndex] = (int)Math.Pow(npc.buffTime[buffIndex]+(int)time, 0.98);
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().TimeSlow += (npc.buffTime[buffIndex]/(60f*5f));
			npc.GetGlobalNPC<SGAnpcs>().DankSlow = true;
		}
	}

}