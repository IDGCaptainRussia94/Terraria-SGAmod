using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;

namespace SGAmod.Buffs
{
	public class Microtransactions: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Microtransactions");
			Description.SetDefault("Losing money every second");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Microtransactions = true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().Mircotransactions = true;
		}
	}
}
