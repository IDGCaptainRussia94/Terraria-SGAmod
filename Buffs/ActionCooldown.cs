using Terraria;
using Terraria.ModLoader;
using System;
using Idglibrary;
using SGAmod.NPCs;

namespace SGAmod.Buffs
{
	public class ActionCooldown: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Action Cooldown");
			Description.SetDefault("Cannot use a special item yet");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().ActionCooldown = true;
		}
	}
	public class BossHealingCooldown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Anticipation Cooldown");
			Description.SetDefault("Your pre-boss fight healing cannot activate again");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
		}


		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/ActionCooldown";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (IdgNPC.bossAlive)
				player.buffTime[buffIndex] = Math.Max(player.buffTime[buffIndex], 2);
		}
	}


}
