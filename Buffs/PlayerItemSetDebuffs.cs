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
			longerExpertDebuff = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().Pressured=true;
		}
	}
	public class ShieldBreak: ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Shield Break");
			Description.SetDefault("No Electric Charge Regen\nTaking off an Energy Shield will hurt the player");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/AcidBurn";
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
			texture = "SGAmod/Buffs/BuffTemplate";
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Place Holder Debuff");
			Description.SetDefault("Your not suppose to see this!");
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}
	}
	public class BIPBuff : ModBuff
	{

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/BuffTemplate";
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
