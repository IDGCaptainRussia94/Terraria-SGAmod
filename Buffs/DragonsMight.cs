using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Buffs
{
	public class DragonsMight: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dragon's Might");
			Description.SetDefault("50% increase to all damage types except Summon damage");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.magicDamage += 0.5f;
			player.Throwing().thrownDamage += 0.5f;
			player.meleeDamage += 0.5f;
			player.rangedDamage += 0.5f;
			if (player.buffTime[buffIndex] < 10){
			bool tempimmune = player.buffImmune[BuffID.Weak];
			player.buffImmune[BuffID.Weak] = false;
			player.AddBuff(BuffID.Weak,60*20);
			tempimmune = player.buffImmune[BuffID.Weak] = tempimmune;
			}
		}

		//public override void Update(NPC npc, ref int buffIndex)
		//{
		//	npc.GetGlobalNPC<SGAWorld>(mod).eFlames = true;
		//}
	}
	public class RagnarokBrewBuff : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/MatrixBuff";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ragnarok's Brew");
			Description.SetDefault("Grants increased Apocalyptical Chance for your equiped weapon damage type as your HP drops");
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			double gg = Math.Min(4.00 - (((double)player.statLife / (double)player.statLifeMax) * 4.00), 3.00);

			if (player.HeldItem != null)
			{
				if (player.HeldItem.melee)
					player.GetModPlayer<SGAPlayer>().apocalypticalChance[0] += gg;
				if (player.HeldItem.ranged)
					player.GetModPlayer<SGAPlayer>().apocalypticalChance[1] += gg;
				if (player.HeldItem.magic)
					player.GetModPlayer<SGAPlayer>().apocalypticalChance[2] += gg;
				if (player.HeldItem.thrown)
					player.GetModPlayer<SGAPlayer>().apocalypticalChance[3] += gg;
			}
			player.lavaRose = true;
		}
	}
}
