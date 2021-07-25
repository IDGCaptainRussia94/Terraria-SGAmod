using AAAAUThrowing;
using SGAmod.Tiles;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Valkyrie
{

	[AutoloadEquip(EquipType.Head)]
	public class ValkyrieHelm : ModItem,IAuroraItem
	{
        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += SetBonus;
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Helm");
			Tooltip.SetDefault("15% increased throwing damage and velocity, and 25% increased crit chance\nGrants life regeneration");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0,50,0,0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 20;
			item.lifeRegen = 2;
		}

		public static void SetBonus(SGAPlayer sgaplayer)
		{
			if (sgaplayer.valkyrieSet)
			{
				Player player = sgaplayer.player;
				player.Throwing().thrownDamage += (int)System.Math.Min(player.lifeRegen, player.lifeRegenTime * 0.01f) * 0.01f;

				if (player.Male)
					player.endurance += 0.15f;
				else
					player.wingTimeMax = (int)(player.wingTimeMax * 1.20f);

			}
		}

		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
			player.Throwing().thrownVelocity += 0.15f;
			player.Throwing().thrownDamage += 0.15f;
			player.Throwing().thrownCrit += 25;
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ && !Main.dayTime)
				sgaplayer.armorglowmasks[0] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Head";
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
			recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 25);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddTile(ModContent.TileType<LuminousAlter>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class ValkyrieBreastplate : ValkyrieHelm, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Breastplate");
			Tooltip.SetDefault("20% increased throwing damage, velocity, and throwing attack speed\n75% chance to not consume Throwing Items\nGrants life regeneration");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 30;
			item.lifeRegen = 3;
		}
		public override void UpdateEquip(Player player)
		{
			player.Throwing().thrownVelocity += 0.20f;
			player.Throwing().thrownDamage += 0.20f;
			player.SGAPly().Thrownsavingchance += 0.75f;
			player.SGAPly().ThrowingSpeed += 0.20f;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ && !Main.dayTime)
			{
				sgaplayer.armorglowmasks[1] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Body";
				sgaplayer.armorglowmasks[2] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Arms";
			}
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class ValkyrieLeggings : ValkyrieHelm, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Valkyrie Leggings");
			Tooltip.SetDefault("15% increased throwing damage\n25% increase to movement speed\nFlight time and movement speed improved by 15% at night\nGrants life regeneration");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(0, 50, 0, 0);
			item.rare = ItemRarityID.Cyan;
			item.defense = 15;
			item.lifeRegen = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 1.25f*(!Main.dayTime ? 1.15f : 1f);
			player.accRunSpeed += 1.5f * (!Main.dayTime ? 1.15f : 1f);
			player.Throwing().thrownDamage += 0.15f;

			if (!Main.dayTime)
			player.wingTimeMax = (int)(player.wingTimeMax * 1.15f);
		}
		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ && !Main.dayTime)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/Armors/Valkyrie/" + Name + "_Legs";
		}
	}

}