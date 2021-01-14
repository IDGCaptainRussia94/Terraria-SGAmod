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
	public class MoneyMismanagement : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Money Mismanagement");
			Description.SetDefault("Your are spending your life away");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().MoneyMismanagement = true;
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
	public class HeavyCrates : ModBuff
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
