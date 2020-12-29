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
	public class LavaBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Lava Burn");
			Description.SetDefault("Magma melts your skin\nObsidian Skin is disabled");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_"+BuffID.Burning;
			return true;
		}
        public override void Update(Player player, ref int buffIndex)
        {
			player.lavaImmune = false;
			player.SGAPly().lavaBurn = true;

		}
        public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().lavaBurn = true;
		}
	}
	public class MoonLightCurse : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Moon Light Curse");
			Description.SetDefault("Defense and life are shattered");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MoonLightCurse";
			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.SGANPCs().MoonLightCurse = true;
			npc.SGANPCs().reducedDefense += 50;
		}
	}
	public class PiercedVulnerable : MoonLightCurse
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Pierced N Vulnerable");
			Description.SetDefault("Defense is reduced");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 10;
		}
		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.SGANPCs().reducedDefense += 10;
		}
	}
	public class SnapFade : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("SnapFade");
			Description.SetDefault("");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().Snapfading = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MoonLightCurse";
			return true;
		}
	}
	public class EverlastingSuffering : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Everlasting Suffering");
			Description.SetDefault("Damage over time is greatly increased");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MoonLightCurse";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().ELS = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().ELS = true;
		}
	}
}