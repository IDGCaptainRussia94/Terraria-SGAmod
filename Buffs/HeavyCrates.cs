using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;

namespace SGAmod.Buffs
{
	public class HeavyCrates: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Heavy Crates");
			Description.SetDefault("Cratrosity's influance is making the crates heavy!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().HeavyCrates = true;
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
}
