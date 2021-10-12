using AAAAUThrowing;
using Microsoft.Xna.Framework;
using SGAmod.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Mandala
{

	[AutoloadEquip(EquipType.Head)]
	public class MandalaHood : ModItem
	{
        public override bool Autoload(ref string name)
        {
			//if (GetType() == typeof(ValkyrieHelm))
				//SGAPlayer.PostUpdateEquipsEvent += SetBonus;

			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mandala Hood");
			Tooltip.SetDefault("+1 minion slot\n15% increased Summon Damage and Summon weapon Use Speed\nAdditional 10 defense and minion slot in Subworlds");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,15,0,0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 12;
			item.lifeRegen = 0;
		}

		/*public static void SetBonus(SGAPlayer sgaplayer)
		{

		}*/

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;

			player.maxMinions += 1;
			player.minionDamage += 0.15f;
			sgaplayer.summonweaponspeed += 0.15f;

			if (Dimensions.SGAPocketDim.WhereAmI != null)
			{
				player.statDefense += 10;
				player.maxMinions += 1;
			}

		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 40);
			recipe.AddIngredient(ModContent.ItemType<OmniSoul>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class MandalaChestplate : MandalaHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mandala Chestplate");
			Tooltip.SetDefault("+2 minion slots\n20% increased Summon Damage and Summon weapon Use Speed\nAdditional life regen in Subworlds");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 20;
			item.lifeRegen = 0;
		}
		public override void UpdateEquip(Player player)
		{
			player.minionDamage += 0.20f;
			player.maxMinions += 2;
			player.SGAPly().summonweaponspeed += 0.20f;

			if (Dimensions.SGAPocketDim.WhereAmI != null)
            {
				player.lifeRegen += 6;
            }
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/GlowMasks/" + Name + "_GlowArms";
			}
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class MandalaLeggings : MandalaHood
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mandala Leggings");
			Tooltip.SetDefault("+1 minion slot\n15% increased Summon Damage and Summon weapon use Speed\nAdditional flying speed and wing time in Subworlds");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 10;
			item.lifeRegen = 0;
		}
		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
			player.minionDamage += 0.15f;
			player.SGAPly().summonweaponspeed += 0.15f;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
	}

}