using Terraria;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Terraria.ID;

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
			Main.buffNoTimeDisplay[Type] = true;
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Pressured=true;
		}
	}
	public class CleansedPerception : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Histoplasma");
			Description.SetDefault("The doors of perception have been cleansed!");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/CleansedPerceptionBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//if (Main.netMode != NetmodeID.Server)
			//	Main.buffTexture[ModContent.BuffType<CleansedPerception>()] = Main.buffTexture[Main.rand.Next(Main.buffTexture.Length)];
		}
	}
	public class ShieldBreak: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shield Break");
			Description.SetDefault("No Electric Charge/Barrier Regen\nTaking off an Energy Shield will hurt the player");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/ShieldBreak";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Shieldbreak = true;
		}
	}
	public class PlaceHolderDebuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/BrokenImmortalityDebuff";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Place Holder Debuff");
			Description.SetDefault("Your not suppose to see this! No Seriously this debuff is NEVER meant to be active! It is Swapped out instantly to stack same-type debuffs!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class BIPBuff : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/BrokenImmortalityDebuff";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Broken Immortality");
			Description.SetDefault("You've lost your godly defense!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class WorseWeakness : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_" + BuffID.Weak;
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Consumed Weakness");
			Description.SetDefault("That potion has left you massively drained...");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.meleeSpeed -= 0.051f;
			player.statDefense -= 15;
			player.moveSpeed -= 0.2f;
			player.SGAPly().UseTimeMul -= 0.25f;

		}
	}
}
