using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Armors
{

	[AutoloadEquip(EquipType.Head)]
	public class SpaceDiverHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Diver Helm");
			Tooltip.SetDefault("5% increased Booster Capacity and +2 Booster Recharge Rate\n+3000 Max Electric Charge\nYou emit blue light while in water\nEffects of Arctic Diving Gear\n" + Idglib.ColorText(Color.Red, "5% less damage"));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 15);
			item.rare = 6;
			item.defense=8;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
        if (player.wet){
		Lighting.AddLight(player.Center, 0.2f, 0.0f, 0.5f);
		}
		player.arcticDivingGear = true;
			sgaplayer.electricChargeMax += 3000;
			player.BoostAllDamage(-0.05f);
			player.GetModPlayer<SGAPlayer>().boosterPowerLeftMax += (int)(10000f * 0.05f);
			player.GetModPlayer<SGAPlayer>().boosterrechargerate += 2;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

			if (!Main.dedServ)
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
        public override bool DrawHead()
        {
            return false;
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 12);
			//recipe.AddIngredient(ItemID.ArcticDivingGear, 1);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class SpaceDiverChestplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Diver Chestplate");
			Tooltip.SetDefault("5% increased Booster Capacity and +3 Booster Recharge Rate\n+3000 Max Electric Charge\nGrants unmatched movement in water\n10% faster item use times\n" + Idglib.ColorText(Color.Red, "10% less damage"));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 15);
			item.rare = 6;
			item.defense=16;
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
            player.arcticDivingGear = true;
            player.accFlipper = true;
			player.accDivingHelm = true;
			player.iceSkate = true;
			player.ignoreWater = true;
            sgaplayer.UseTimeMul += 0.10f;
			sgaplayer.electricChargeMax += 3000;

			player.BoostAllDamage(-0.10f);
			player.GetModPlayer<SGAPlayer>().boosterPowerLeftMax += (int)(10000f * 0.05f);
			player.GetModPlayer<SGAPlayer>().boosterrechargerate += 3;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_ArmsGlow";
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 15);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class SpaceDiverLeggings : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Space Diver Leggings");
			Tooltip.SetDefault("5% increased Booster Capacity and +2 Booster Recharge Rate\n+5 passive Electric Charge Rate while wet\n20% faster movement speed (40% when in water)\n" + Idglib.ColorText(Color.Red, "10% less damage"));
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(gold: 15);
			item.rare = 6;
			item.defense = 8;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed *= 1.20f;
			player.accRunSpeed *= 1.20f;
			player.maxRunSpeed *= 1.20f;
			if (player.wet)
			{
				player.moveSpeed *= 1.20f;
				player.accRunSpeed *= 1.20f;
				player.maxRunSpeed *= 1.20f;
				player.SGAPly().electricrechargerate += 5;

			}
			player.BoostAllDamage(-0.10f);
			player.GetModPlayer<SGAPlayer>().boosterPowerLeftMax += (int)(10000f * 0.05f);
			player.GetModPlayer<SGAPlayer>().boosterrechargerate += 2;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 12);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}


}