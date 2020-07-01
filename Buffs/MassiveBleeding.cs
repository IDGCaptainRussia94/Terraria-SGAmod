using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.NPCs;

namespace SGAmod.Buffs
{
	public class MassiveBleeding: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Massive Bleeding");
			Description.SetDefault("You are bleeding out");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().MassiveBleeding=true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			if (npc.buffImmune[BuffID.Bleeding])
			{
				npc.DelBuff(buffIndex);
				return;
			}
			npc.GetGlobalNPC<SGAnpcs>().MassiveBleeding=true;
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
}
