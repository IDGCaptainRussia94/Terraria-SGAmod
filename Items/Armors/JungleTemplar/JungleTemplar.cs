using AAAAUThrowing;
using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.JungleTemplar
{

	[AutoloadEquip(EquipType.Head)]
	public class JungleTemplarHelmet : ModItem
	{

		public override bool Autoload(ref string name)
        {
			if (GetType() == typeof(JungleTemplarHelmet))
			{
				SGAPlayer.PreUpdateMovementEvent += SetBonusMovement;
			}
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Templar Helmet");
			Tooltip.SetDefault("20% increased Throwing crit chance and 10% increased Technological damage\n25% increase to not consume throwable\n+2000 Max Electric Charge\n+3 electric charge regen rate if exposed to the sun\n" + Idglib.ColorText(Color.Red, "5% less overall crit chance"));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,15,0,0);
			item.rare = ItemRarityID.Yellow;
			item.defense = 25;
		}
        public override bool DrawHead()
        {
			return GetType() == typeof(JungleTemplarHelmet) ? false : base.DrawHead();
        }
        public static void ActivatePrecurserPower(SGAPlayer sgaply)
		{
			if (sgaply.AddCooldownStack(60 * 80, 2))
			{
				sgaply.player.AddBuff(ModContent.BuffType<PrecurserPowerBuff>(), 1200);
				SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_EtherianPortalOpen, (int)sgaply.player.Center.X, (int)sgaply.player.Center.Y);
				if (sound != null)
				{
					sound.Pitch = 0.85f;
				}

				SoundEffectInstance sound2 = Main.PlaySound(SoundID.Zombie, (int)sgaply.player.Center.X, (int)sgaply.player.Center.Y, 35);
				if (sound2 != null)
				{
					sound2.Pitch = -0.5f;
				}

				for (int i = 0; i < 50; i += 1)
				{
					int dust = Dust.NewDust(sgaply.player.Hitbox.TopLeft() + new Vector2(0, -8), sgaply.player.Hitbox.Width, sgaply.player.Hitbox.Height + 8, DustID.AncientLight);
					Main.dust[dust].scale = 2f;
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = (sgaply.player.velocity * Main.rand.NextFloat(0.75f, 1f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-1.2f, 1.2f)) * Main.rand.NextFloat(1f, 3f);
				}
			}
		}

		public static void SetBonusMovement(SGAPlayer sgaplayer)
		{
			if (sgaplayer.jungleTemplarSet.Item1)
			{
				if (sgaplayer.player.velocity.Y != 0)
                {
					float gravity = sgaplayer.player.velocity.Y > 0.50f ? 0.50f : 0.25f;
					sgaplayer.player.velocity += Vector2.UnitY * sgaplayer.player.gravDir * Player.defaultGravity * gravity;
					if (gravity > 0.25f)
					{
						sgaplayer.player.maxFallSpeed += 5;
                    }
                }
                else
                {
					sgaplayer.player.thorns += 3f;
					sgaplayer.player.noKnockback = true;
                }
			}
		}

		public static void SetBonus(SGAPlayer sgaplayer)
		{

			sgaplayer.player.powerrun = true;

			if (sgaplayer.ShieldType == 0)
				sgaplayer.ShieldType = 100;

			if (sgaplayer.jungleTemplarSet.Item2)
			{
				sgaplayer.player.Throwing().thrownDamage *= sgaplayer.techdamage;

				if (sgaplayer.ConsumeElectricCharge(9, 300, true))
				{
					sgaplayer.player.shinyStone = true;
				}
			}

			if (!sgaplayer.ConsumeElectricCharge(1000, 0, false, false))
            {
				sgaplayer.player.AddBuff(ModContent.BuffType<Buffs.LavaBurnLight>(),150*(Main.expertMode ? 1 : 2));
			}

		}

		public Color ArmorGlow(Player player, int index)
		{
			return Color.White * (player.SGAPly().jungleTemplarSet.Item2 ? 1f : 0.5f);
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.SGAPly();
			sgaplayer.techdamage += 0.10f;
			sgaplayer.electricChargeMax += 2000;
			player.Throwing().thrownCrit += 20;
			sgaplayer.Thrownsavingchance += 0.25f;
			player.BoostAllDamage(0, -5);

			if (Collision.CanHitLine(player.Center, 0, 0, new Vector2(player.Center.X, 0), 0, 0))
            {
				sgaplayer.electricrechargerate += 3;
			}

		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			if (GetType() == typeof(JungleTemplarHelmet))
			{
				SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowcolor[0] = ArmorGlow;
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 8);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell);
			if (GetType() != typeof(JungleTemplarLeggings))
			{
				recipe.AddIngredient(ItemID.Ruby, 1);
				recipe.AddIngredient(ItemID.Topaz, 1);
			}
			else
			{
				if (GetType() == typeof(JungleTemplarLeggings))
				{
					recipe.AddIngredient(ItemID.AsphaltBlock, 50);
				}
			}
			recipe.AddIngredient(ItemID.LihzahrdBrick, 50);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	[AutoloadEquip(EquipType.Body)]
	public class JungleTemplarChestplate : JungleTemplarHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Templar Chestplate");
			Tooltip.SetDefault("25% increased throwing damage, +2 electric charge regen rate\nMax Lava time is increased by 10 seconds\n" + Idglib.ColorText(Color.Red, "5% less overall crit chance"));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.defense = 32;
			item.lifeRegen = 0;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.SGAPly();
			player.lavaMax += 600;
			//sgaplayer.lavaBurn = true;
			sgaplayer.techdamage += 0.10f;
			player.Throwing().thrownDamage += 0.25f;
			sgaplayer.electricrechargerate += 2;
			player.BoostAllDamage(0, -5);
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
			sgaplayer.armorglowcolor[1] = ArmorGlow;
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class JungleTemplarLeggings : JungleTemplarHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jungle Templar Leggings");
			Tooltip.SetDefault("15% increased Throwing crit chance\n20% boost to throwing item use speed while grounded\nMax Lava time is increased by 5 seconds\nBeing submerged in lava grants +5 electric charge regen rate\n" + Idglib.ColorText(Color.Red, "5% less overall crit chance"));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.Yellow;
			item.defense = 18;
			item.lifeRegen = 0;
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.SGAPly();
			player.lavaMax += 600;
			if (player.velocity.Y == 0)
			sgaplayer.ThrowingSpeed += 0.20f;
			player.Throwing().thrownCrit += 15;
			player.BoostAllDamage(0, -5);

			if (player.lavaWet)
            {
				sgaplayer.electricrechargerate += 5;
            }
		}
	}

	public class PrecurserPowerBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Precursor's Power");
			Description.SetDefault("Ancient energy powers you up!\nGain shiny stone recovery even while moving, but consumes electric charge!\nExtreme energy causes harm if not wearing the armor set");
			Main.debuff[Type] = false;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/PrecursorsPowerBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			SGAPlayer sgaply = player.SGAPly();
			sgaply.jungleTemplarSet.Item2 = true;

			if (!sgaply.jungleTemplarSet.Item1)
            {
				sgaply.badLifeRegen -= 50;
            }

			int dust = Dust.NewDust(player.Hitbox.TopLeft() + new Vector2(0, -8), player.Hitbox.Width, player.Hitbox.Height + 16, 36);
			Main.dust[dust].scale = 3f;
			Main.dust[dust].noGravity = true;
			Main.dust[dust].alpha = 240;
			Main.dust[dust].velocity = (player.velocity * Main.rand.NextFloat(0.4f, 1.2f)) + Vector2.UnitX.RotatedBy(-MathHelper.PiOver2 + Main.rand.NextFloat(-0.6f, 0.6f)) * Main.rand.NextFloat(1f, 4f);


		}
	}

}