using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;

namespace SGAmod.Buffs
{
	public class Pressured: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Pressured");
			Description.SetDefault("You've been breathing Pressurized air; removing your suit is going to deal great damage");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Pressured=true;
		}

		/*public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>(mod).Pressured=true;
		}*/

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
}
