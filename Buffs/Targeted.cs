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
	public class Targeted : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/BuffTemplate";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Doused In Gas");
			Description.SetDefault("You are coated in a highly flammable substance");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}

	public class Marked : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/BuffTemplate";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Doused In Gas");
			Description.SetDefault("You are coated in a highly flammable substance");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
        public override void Update(NPC npc, ref int buffIndex)
        {
			SGAnpcs npcs = npc.GetGlobalNPC<SGAnpcs>();
			npcs.marked = true;
			npcs.damagemul += 0.10f;

		}
    }
}