using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;

namespace SGAmod.Buffs
{
	public class NoFly: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Snowfrosted");
			Description.SetDefault("Cirno's presence is making flight difficult...");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().NoFly=true;
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
}
