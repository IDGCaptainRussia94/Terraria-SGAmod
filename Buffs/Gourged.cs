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
	public class Gourged : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Gourged");
			Description.SetDefault("Halved defense");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense /= 2;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.buffImmune[BuffID.Bleeding])
			{
				npc.DelBuff(buffIndex);
				return;
			}
			npc.GetGlobalNPC<SGAnpcs>().Gourged = true;
		}
	}
	public class TechnoCurse : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Techno Curse");
			Description.SetDefault("Technological damage is reduced by 50%");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
}