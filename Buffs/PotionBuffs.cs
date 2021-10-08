using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Idglibrary;
using AAAAUThrowing;
using System.Linq;

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
			if (player.buffTime[buffIndex] < 10)
			{
			player.AddBuff(ModContent.BuffType<WorseWeakness>(),60*20);
			}
		}
	}
	public class ToxicityPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Toxicity");
			Description.SetDefault("Grants various buffs based around Stinky for the player\n'Things die by just by being around you when you smell, rude!'");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().toxicity += 1;
			player.buffImmune[BuffID.Lovestruck] = true;
			player.buffImmune[ModContent.BuffType<IntimacyPotionBuff>()] = true;

		}
	}
	public class IntimacyPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Intimacy");
			Description.SetDefault("Grants various buffs based around Lovestruct for the player\n'Enemies willing hand over their life essences to your alluring ensare'\n'Meanwhile, friends gain!'");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().intimacy += 1;
			player.buffImmune[BuffID.Stinky] = true;
			player.buffImmune[ModContent.BuffType<ToxicityPotionBuff>()] = true;
		}
	}
	public class TriggerFingerPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Trigger Finger");
			Description.SetDefault("Non-autofire guns fire 15% faster");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().triggerFinger += 0.15f;
		}
	}		
	public class TrueStrikePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("True Strike");
			Description.SetDefault("True Melee weapons do 20% more damage");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().trueMeleeDamage += 0.20f;
		}
	}	
	public class ClarityPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Clarity");
			Description.SetDefault("3% reduced mana costs, 10% reduced mana costs while you are Mana sick");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.manaCost -= 0.03f;
			if (player.manaSick)
				player.manaCost -= 0.07f;
		}
	}
	public class TooltimePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tooltime!");
			Description.SetDefault("Your tools have greatly increased knockback!");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			//null
		}
	}
	public class TinkerPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Tinker");
			Description.SetDefault("Your uncrafting net less is reduced");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().uncraftBoost = 10;
		}
	}
	public class RagnarokBrewBuff : ModBuff
	{
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
		}
	}
	public class CondenserPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Condenser");
			Description.SetDefault("1 free Action Cooldown Stack, 15% longer cooldown times");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().MaxCooldownStacks += 1;
			player.SGAPly().actionCooldownRate += 1.15f;
		}
	}
	public class EnergyPotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Energy");
			Description.SetDefault("+1 passive Electric Charge Rate, Recharge delay is halved");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().electricrechargerate += 1;
			player.SGAPly().electricChargeReducedDelay *= 0.5f;
		}
	}

	public class PhalanxPotionBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Phalanx Potion");
			Description.SetDefault("Shield block angle is improved!\n+8 defense per nearby player holding a shield");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			foreach(Player player2 in Main.player.Where(testby => testby.active && !testby.dead && (testby.Center-player.Center).LengthSquared()<600*600 && testby.HeldItem != null && testby.HeldItem.modItem != null && testby.HeldItem.modItem is IShieldItem))
			{
				player.statDefense += 8;
            }
			player.SGAPly().shieldBlockAngle += 0.15f;
		}
	}

	public class ReflexPotionBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_" + BuffID.PaladinsShield;
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Reflex Potion");
			Description.SetDefault("React for longer with your shield to Just Block!");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().shieldBlockTime += 10;
		}
	}

	public class ConsumeHellBuff : ModBuff
	{

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Consumed Hell");
			Description.SetDefault("Seems like the Underworld can access the rest of the world via your mouth");
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.AddBuff(BuffID.OnFire, 2);
		}
	}
	public class IceFirePotionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Fridgeflame Concoction");
			Description.SetDefault("Reduced Damage over time at a cost of losing immunities");
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.GetModPlayer<SGAPlayer>().IceFire = true;
			player.lavaRose = true;
		}
	}
	public class InvincibleBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			//texture = "Terraria/Buff_" + BuffID.ShadowDodge;
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Invincible");
			Description.SetDefault("Damage is currently completely prevented\n'that one time you aren't defeated during a cutscene!'");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().invincible = true;

		}

	}
	public class ManaRegenFakeBuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_"+BuffID.ManaRegeneration;
			return true;
		}
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mana Regen");
			Description.SetDefault("Mana Regeneration is greatly improved!");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.manaRegenBonus += 25;
			player.manaRegenDelayBonus++;
		}
	}
}
