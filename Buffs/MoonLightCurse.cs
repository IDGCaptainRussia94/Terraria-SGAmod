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
			npc.GetGlobalNPC<SGAnpcs>().MoonLightCurse = true;
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