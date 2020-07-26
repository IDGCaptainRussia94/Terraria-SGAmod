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

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			if (NPC.CountNPCS(mod.NPCType("Cirno")) < 1 && !SGAWorld.downedCirno)
				tip += "\nBeat Cirno to remove this effect";
			if (NPC.CountNPCS(mod.NPCType("Hellion")) > 0)
				tip += "\nHellion's Army has Cirno using this debuff against you";
		}
	}
}
