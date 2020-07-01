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