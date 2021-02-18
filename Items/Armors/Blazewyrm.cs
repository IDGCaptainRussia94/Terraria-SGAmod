using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors
{

	[AutoloadEquip(EquipType.Head)]
	public class BlazewyrmHelm : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazewyrm Helm");
			Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n20% faster melee speed and 16% more melee damage");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 5;
			item.defense = 10;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
			sgaplayer.armorglowmasks[0] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void UpdateEquip(Player player)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
			player.meleeSpeed += 0.20f;
			player.meleeDamage += 0.16f;
			sgaplayer.apocalypticalChance[0] += 1.0;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MoltenHelmet, 1);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 6);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "accapocalypticaltext", SGAGlobalItem.apocalypticaltext));
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class BlazewyrmBreastplate : BlazewyrmHelm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazewyrm Breastplate");
			Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n12% increased melee crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 8;
			item.defense = 14;
		}
		public override void UpdateEquip(Player player)
		{
			player.meleeCrit += 12;
			player.SGAPly().apocalypticalChance[0] += 1.0;
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
			recipe.AddIngredient(ItemID.MoltenBreastplate, 1);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 8);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 12);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class BlazewyrmLeggings : BlazewyrmHelm
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazewyrm Leggings");
			Tooltip.SetDefault("1% increased Melee Apocalyptical Chance\n25% increase to movement speed\nEven faster in lava");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 10000;
			item.rare = 5;
			item.defense = 8;
		}

		public override void UpdateVanity(Player player, EquipType type)
		{
			SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
			if (!Main.dedServ)
				sgaplayer.armorglowmasks[3] = "SGAmod/Items/GlowMasks/" + Name + "_Glow";
		}
		public override void UpdateEquip(Player player)
		{
			player.moveSpeed += 1.25f;
			player.accRunSpeed += 1.5f;
			if (player.lavaWet)
			{
				player.moveSpeed *= 1.2f;
				player.accRunSpeed *= 1.2f;
			}
			player.SGAPly().apocalypticalChance[0] += 1.0;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MoltenGreaves, 1);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 6);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}


}