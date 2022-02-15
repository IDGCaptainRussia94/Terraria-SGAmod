using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.HavocGear.Items.Accessories;
using Idglibrary;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod.Items.Accessories.Charms
{
	public class NoHitCharmlv1 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Amulet of Diehard Cataclysm");
			Tooltip.SetDefault("'Embrace the suffering, indulge on the reward'\n'Truly, only for the worthy... And the british'\n25% more Expertise is earned and respawn instantly outside of boss fights\n" + Idglib.ColorText(Color.Red, "You die in one hit, IFrames cause great damage over time") +"\n" + Idglib.ColorText(Color.Red, "Most if not all methods of death prevention are disabled") +"\n" + Idglib.ColorText(Color.Red, "Death claims you if you attempt to break the curse...") + "\n+An exception to the formentioned rule are Just Blocks\nThis item doesn't take effect til 3 seconds after spawning to prevent soft-locks");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 10));
		}

		public override string Texture => "SGAmod/Items/Accessories/Charms/NoHitCharmlv1";

        public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
			item.rare = ItemRarityID.Cyan;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return lightColor;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (sgaplayer.NoHitCharmTimer > 180)
			{
				if (sgaplayer.NoHitCharmTimer<100000)
				{
					sgaplayer.NoHitCharmTimer = 1000000;
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_PhantomPhoenixShot, player.MountedCenter);
					if (sound != null)
					{
						sound.Pitch = 0.5f;
					}

					sound = Main.PlaySound(29, (int)player.MountedCenter.X, (int)player.MountedCenter.Y, 105, 1f, -0.6f);
					if (sound != null)
					{
						sound.Pitch = 0.75f;
					}

					for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
					{
						Vector2 offset = f.ToRotationVector2();
						int dust = Dust.NewDust(player.MountedCenter + (offset * 16f), 0, 0, DustID.Vortex);
						Main.dust[dust].scale = 1.5f;
						Main.dust[dust].noGravity = true;
						Main.dust[dust].velocity = f.ToRotationVector2() * 6f;
					}
				}
					sgaplayer.NoHitCharm = true;
				player.shadowDodge = false;
				player.blackBelt = false;
			}
		}
		public override void AddRecipes()
		{
			//none
		}

	}
	public class MiningCharmlv1 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Amulet Tier 1");
			Tooltip.SetDefault("25% increased mining/hammering/chopping speed\n"+Idglib.ColorText(Color.Red, "There is a very rare chance to consume an extra item whenever items are consumed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			if (GetType().Name.Length - GetType().Name.Replace("lv3","").Length>0)
			tooltips.Add(new TooltipLine(mod, "Lv3Charm", "Includes the functionality of Mechanical Minecart"));
		}

		public override bool CanEquipAccessory(Player player, int slot)
		{
			bool canequip = true;
			for (int x = 3; x < 8 + player.extraAccessorySlots; x++)
			{
				if (player.armor[x].modItem != null) {
					Type myclass = player.armor[x].modItem.GetType();
					if (myclass.BaseType == typeof(MiningCharmlv1) || myclass == typeof(MiningCharmlv1)) {

						//if (myType==mod.ItemType("MiningCharmlv1") || myType==mod.ItemType("MiningCharmlv2") || myType == mod.ItemType("MiningCharmlv3")){
						canequip = false;
						break;
					} } }
			return canequip;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMulPickaxe += 0.25f;
			if (!sgaplayer.tpdcpu)
				sgaplayer.consumeCurse += 2;

		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddRecipeGroup("SGAmod:Tier1Pickaxe", 1);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 15);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class EnhancingCharmlv1 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enhancing Amulet Tier 1");
			Tooltip.SetDefault("buffs are 25% longer and debuffs are 25% shorter\n" + Idglib.ColorText(Color.Red, "Potion Sickness is not affected, and is also 10% longer"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0); ;
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.EnhancingCharm = 4;
			if (!sgaplayer.tpdcpu)
			sgaplayer.potionsicknessincreaser = 10;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddIngredient(ItemID.LesserHealingPotion, 5);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 15);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	public class AnticipationCharmlv1 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Amulet Tier 1");
			Tooltip.SetDefault("When a boss fight starts, you are healed by 100 HP, but only every 2 minutes and while " + Idglib.ColorText(Color.Green, "Anticipation") + " is low" +
				"\nDuring a boss fight, you build up " + Idglib.ColorText(Color.Green, "Anticipation") + ", which causes your held weapon to do more damage, this caps at a 25% increase\n" +
	"You lose half your " + Idglib.ColorText(Color.Green, "Anticipation") + " when hurt, and passively drains while no boss is alive\n"+Idglib.ColorText(Color.Red, "Damage you take is increased by 20%"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0); ;
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.anticipationLevel = 1;
			if (!sgaplayer.tpdcpu)
				sgaplayer.damagetaken += 0.20f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddIngredient(ItemID.CopperBroadsword, 1);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 15);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
	public class ReservationCharmlv1 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reservation Amulet Tier 1");
			string capper = (GetType() == typeof(ReservationCharmlv3) ? "75" : GetType() == typeof(ReservationCharmlv2) ? "50" : "25");
			Tooltip.SetDefault("Converts "+ capper + "% of your max HP into a regenerating barrier that blocks damage\nThis barrier's max strength is improved by your tech multiplier\n" + Idglib.ColorText(Color.Red, capper+"% of your HP is capped")+"\n" +Idglib.ColorText(Color.Red,"Damage is applied to shields before any armor reduction")+"\n" + Idglib.ColorText(Color.Red, "Inflicts Shield Break on deplete"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.accessory = true;
			item.mountType = 6;
		}
		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostCharmsUpdateEquipsEvent += PostAccessoryUpdate;
			return base.Autoload(ref name);
		}

		protected void PostAccessoryUpdate(SGAPlayer sgaplayer)
        {
			Player player = sgaplayer.player;

			//Main.NewText(sgaplayer.energyShieldAmmountAndRecharge.Item2);
			if (sgaplayer.GetEnergyShieldAmmountAndRecharge.Item2 > 0)
			{
				if (sgaplayer.energyShieldAmmountAndRecharge.Item3 < 1 && !sgaplayer.Shieldbreak)
				{
					sgaplayer.ShieldRecharge();
					if (sgaplayer.energyShieldAmmountAndRecharge.Item3==0)
						sgaplayer.StartShieldRecharge();

					float rechargerate = (sgaplayer.jellybruSet ? 600f : 180f);
					float multime = 1f;
					PlayerHooks.NaturalLifeRegen(player, ref multime);
					rechargerate /= multime;

					sgaplayer.energyShieldAmmountAndRecharge.Item1 = (int)MathHelper.Clamp((int)Math.Ceiling(sgaplayer.energyShieldAmmountAndRecharge.Item1 + (sgaplayer.energyShieldAmmountAndRecharge.Item2 / rechargerate)), 0, sgaplayer.energyShieldAmmountAndRecharge.Item2);
				}
			}

			if (sgaplayer.energyShieldReservation > 0)
            {
				int percent = (int)(player.statLifeMax2 * (1f-sgaplayer.energyShieldReservation));
				if (player.statLife > percent)
                {
					player.statLife = percent;
				}
            }

		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			float thepercent = GetType() == typeof(ReservationCharmlv3) ? 0.75f : GetType() == typeof(ReservationCharmlv2) ? 0.50f : 0.25f;

			int percentLife = (int)((player.statLifeMax2) * thepercent);
			sgaplayer.energyShieldAmmountAndRecharge.Item2 += percentLife;
			sgaplayer.energyShieldReservation += (1f- sgaplayer.energyShieldReservation)*thepercent;

			if (sgaplayer.ShieldType<1000)
			sgaplayer.ShieldType = 1000;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EmptyCharm"), 1);
			recipe.AddIngredient(ItemID.Minecart, 1);
			recipe.AddIngredient(ItemID.Gel, 10);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 2);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 6);
			recipe.AddIngredient(ModContent.ItemType<WraithFragment3>(), 8);
			recipe.AddRecipeGroup("SGAmod:Tier5Bars", 5);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class ReservationCharmlv2 : ReservationCharmlv1
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Reservation Amulet Tier 2");
		}


		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ReservationCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class ReservationCharmlv3 : ReservationCharmlv2
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Reservation Amulet Tier 3");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("ReservationCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 12);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 15);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}


	public class AnticipationCharmlv2 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Amulet Tier 2");
			Tooltip.SetDefault("When a boss fight starts, you are healed by 200 HP, but only every 2 minutes and while " + Idglib.ColorText(Color.Green, "Anticipation") + " is low" +
				"\nDuring a boss fight, you build up " + Idglib.ColorText(Color.Green, "Anticipation") + ", which causes your held weapon to do more damage, this caps at a 50% increase\n" +
	"You lose half your " + Idglib.ColorText(Color.Green, "Anticipation") + " when hurt, and passively drains while no boss is alive\n" + Idglib.ColorText(Color.Red, "Damage you take is increased by 30%"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.anticipationLevel = 2;
			if (!sgaplayer.tpdcpu)
				sgaplayer.damagetaken += 0.30f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AnticipationCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AnticipationCharmlv3 : MiningCharmlv1
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Anticipation Amulet Tier 3");
			Tooltip.SetDefault("When a boss fight starts, you are healed by 300 HP, but only every 2 minutes and while " + Idglib.ColorText(Color.Green, "Anticipation") + " is low" +
				"\nDuring a boss fight, you build up " + Idglib.ColorText(Color.Green, "Anticipation") + ", which causes your held weapon to do more damage, this caps at a near 100% increase\n" +
	"You lose half your " + Idglib.ColorText(Color.Green, "Anticipation") + " when hurt, and passively drains while no boss is alive\n" + Idglib.ColorText(Color.Red, "Damage you take is increased by 40%"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.anticipationLevel = 3;
			if (!sgaplayer.tpdcpu)
				sgaplayer.damagetaken += 0.40f;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AnticipationCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 12);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 15);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class EnhancingCharmlv2 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enhancing Amulet Tier 2");
			Tooltip.SetDefault("buffs are 50% longer and debuffs are 33% shorter\n" + Idglib.ColorText(Color.Red, "Potion Sickness is not affected, and is also 15% longer"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.EnhancingCharm = 3;
			if (!sgaplayer.tpdcpu)
				sgaplayer.potionsicknessincreaser = 9;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EnhancingCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class EnhancingCharmlv3 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Enhancing Amulet Tier 3");
			Tooltip.SetDefault("buffs are 100% longer and debuffs are 50% shorter\n" + Idglib.ColorText(Color.Red, "Potion Sickness is not affected, and is also 20% longer"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.EnhancingCharm = 2;
			if (!sgaplayer.tpdcpu)
				sgaplayer.potionsicknessincreaser = 8;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("EnhancingCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 12);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 15);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class MiningCharmlv2 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Amulet Tier 2");
			Tooltip.SetDefault("50% increased mining/hammering/chopping speed\n" + Idglib.ColorText(Color.Red, "There is a semi-rare chance to consume an extra item whenever items are consumed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.accessory = true;
			item.mountType = 6;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMulPickaxe += 0.50f;
			if (!sgaplayer.tpdcpu)
				sgaplayer.consumeCurse += 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("MiningCharmlv1"), 1);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 15);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 10);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 6);
			recipe.AddIngredient(ItemID.HallowedBar, 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class MiningCharmlv3 : MiningCharmlv1
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mining Amulet Tier 3");
			Tooltip.SetDefault("100% increased mining/hammering/chopping speed\n" + Idglib.ColorText(Color.Red, "There is an uncommon chance to consume an extra item whenever items are consumed"));
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.accessory = true;
			item.mountType = 11;
			item.expert = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			sgaplayer.UseTimeMulPickaxe += 1f;
			if (!sgaplayer.tpdcpu)
				sgaplayer.consumeCurse += 4;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("MiningCharmlv2"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 2);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 12);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 15);
			recipe.AddIngredient(ItemID.MinecartMech, 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	/*
	public class TidalCharm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Amulet");
			Tooltip.SetDefault("Increases your max Breath by 5 bubbles");
		}

		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostUpdateEquipsEvent += DefenseBoost;
			return true;
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 24;
			item.rare = ItemRarityID.Orange;
			item.value = Item.sellPrice(0, 0, 75, 0);
			item.mountType = MountID.MineCart;
			item.accessory = true;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DeepSkyBlue, Terraria.Localization.Language.GetTextValue("ItemTooltip.Flipper")+" while it is raining")));
			tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DeepSkyBlue, "Most status effects that trigger while wet will trigger here")));
			tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.Red, "But you are always drowning")));

			if (!SGAWorld.tidalCharmUnlocked)
				tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DarkBlue, "Its full potential is locked behind the Tempest Sharkvern")));
			else
				tooltips.Add(new TooltipLine(mod, "tidalcharm", Idglib.ColorText(Color.DeepSkyBlue, "Tempest Sharkvern's defeat allows this to take effect when it is not raining")));
		}

		public void DefenseBoost(SGAPlayer sgaply)
		{
			if (sgaply.tidalCharm >= 2)
			{
				Player player = sgaply.player;
			}
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.breathMax += 100;
			player.accFlipper = true;
			SGAPlayer sgaply = player.GetModPlayer<SGAPlayer>();
			if (((Main.raining) || SGAWorld.tidalCharmUnlocked))
			{
				player.GetModPlayer<SGAPlayer>().tidalCharm = 2;
				if (!player.SGAPly().tpdcpu)
				{
					player.AddBuff(ModContent.BuffType<TidalDrown>(), 60 * 10);
				}
            }
			

		}

	}

	public class TidalDrown : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Water Bending Presence");
			Description.SetDefault("Water Density is thick enough to swim in the air as if it were water! But you are losing breath\nAll forms of infinite water breathing are disabled");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_"+BuffID.Suffocation;
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (!player.SGAPly().tpdcpu)
			{
				player.GetModPlayer<SGAPlayer>().permaDrown = true;
				player.GetModPlayer<SGAPlayer>().drownRate += 2;
			}
			player.GetModPlayer<SGAPlayer>().tidalCharm = 2;
		}
	}
	*/

}