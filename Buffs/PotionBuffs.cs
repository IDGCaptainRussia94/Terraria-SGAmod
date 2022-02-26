using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.NPCs;
using Idglibrary;

using System.Linq;

namespace SGAmod.Buffs
{
	public class DragonsMight : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Dragon's Might");
			Description.SetDefault("30% increase to all damage types except Summon damage, which gets 50%");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = true;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.buffTime[buffIndex] > 5)
			{
				player.BoostAllDamage(0.30f);
				player.minionDamage += 0.20f;
			}
			if (player.buffTime[buffIndex] < 20)
			{
			player.AddBuff(ModContent.BuffType<WorseWeakness>(),60*30);
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
			Description.SetDefault("Non-autofire guns fire 25% faster");
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			Main.debuff[Type] = false;
		}
		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().triggerFinger += 0.25f;
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
		double Boost(Player player) => Math.Max(Math.Min(4.00 - (((double) player.statLife / (double) player.statLifeMax2) * 4.00), 4.00),1.00);
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Ragnarok's Brew");
			Description.SetDefault("Grants increased Apocalyptical Chance for your equiped weapon damage type as your HP drops");
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
		}

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
			tip += "\nCurrent Boosts: " + Math.Round(Boost(Main.LocalPlayer),2) + "% Apoco Chance, "+ Math.Round(Boost(Main.LocalPlayer)*25f,1)+"% Apoco Strength";
		}

        public override void Update(Player player, ref int buffIndex)
		{
			float gg = (float)Boost(player);

			if (player.HeldItem != null)
			{
				SGAPlayer sgaply = player.SGAPly();
				player.SGAPly().apocalypticalStrength += gg*0.25f;
				if (player.HeldItem.melee) 
				{
					sgaply.apocalypticalChance[0] += gg;
					return;
				}
				if (player.HeldItem.ranged) 
				{
					sgaply.apocalypticalChance[1] += gg;
					return;
				}
				if (player.HeldItem.magic) 
				{
					sgaply.apocalypticalChance[2] += gg;
					return;
				}
				player.SGAPly().apocalypticalChance[3] += gg;

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
			Description.SetDefault("25% increased passive Electric Charge Rate, Recharge delay is halved");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			//player.SGAPly().electricrechargerate += 1;
			player.SGAPly().electricRechargeRateMul += 0.25f;
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
			//texture = "Terraria/Buff_" + BuffID.PaladinsShield;
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

				float hasbuffs = (player.HasBuff(BuffID.OnFire) || player.HasBuff(BuffID.Frostburn) ? 0.60f : 0.75f);

			player.SGAPly().DoTResist *= hasbuffs;

				//if (player.lifeRegen < 0)
				//	player.lifeRegen = (int)(player.lifeRegen * (0.80 - hasbuffs));

			player.buffImmune[BuffID.OnFire] = false;
				player.buffImmune[BuffID.Frostburn] = false;

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
