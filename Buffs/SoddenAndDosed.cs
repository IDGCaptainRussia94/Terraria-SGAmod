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
	public class Sodden : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sodden");
			Description.SetDefault("You are coated in a piss");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			//if (npc.HasBuff(BuffID.Ichor))
				//npc.DelBuff(BuffID.Ichor);
			npc.GetGlobalNPC<SGAnpcs>().Sodden = true;
		}
	}
	public class SoddenSlow : Sodden
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Sodden");
			Description.SetDefault("You are coated in a piss");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/Sodden";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().Sodden = true;
			npc.GetGlobalNPC<SGAnpcs>().TimeSlow += 0.20f;
		}
	}
	public class DosedInGas : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Doused In Gas");
			Description.SetDefault("You are coated in a highly flammable substance");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().DosedInGas = true;
		}
	}
}