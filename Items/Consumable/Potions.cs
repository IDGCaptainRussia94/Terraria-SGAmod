using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SGAmod.Generation;
using SGAmod;
using Idglibrary;
//using SubworldLibrary;

namespace SGAmod.Items.Consumable
{
	public class ClarityPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clarity Potion");
			Tooltip.SetDefault("Reduces Mana costs by 3%\nWhile you have mana sickness, this is increased to 10%");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.potion = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("ClarityPotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShinePotion);
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.FallenStar,3);
			recipe.AddIngredient(null, "MurkyGel", 4);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class RagnarokBrew : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ragnarok's Brew");
			Tooltip.SetDefault("Boosts your Apocalyptical Chance as your HP drops (up to a max of 3%)");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.potion = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("RagnarokBrewBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater);
			recipe.AddIngredient(null, "Entrophite", 10);
			recipe.AddIngredient(null, "FieryShard", 2);
			recipe.AddIngredient(null, "UnmanedBar", 2);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class CondenserPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Condensing Potion");
			Tooltip.SetDefault("'Helps deal with overheating, allowing you to preform more actions'\nGrants an additonal free Cooldown Stack, however, all new Cooldown Stacks are 15% longer");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.potion = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("CondenserBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HealingPotion,1);
			recipe.AddIngredient(ItemID.StrangeBrew);
			recipe.AddIngredient(null, "FrigidShard", 3);
			recipe.AddIngredient(null, "NoviteBar", 2);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}

	public class RadCurePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Radiation Cure Potion");
			Tooltip.SetDefault("'Radiation is not easy to cure, but thankfully, we have MAGIC!'\nGrants increased Radiation poisoning recovery while no bosses are alive\nGrants 50% increased radiation resistance");
		}

		public override bool CanUseItem(Player player)
		{
			return !(player.HasBuff(Idglib.Instance.BuffType("RadCure")) && player.HasBuff(Idglib.Instance.BuffType("LimboFading")));
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.potion = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = Idglib.Instance.BuffType("RadCure");
			item.buffTime = 60 * 60;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RestorationPotion,3);
			recipe.AddIngredient(ItemID.StrangeBrew);
			recipe.AddIngredient(null, "ManaBattery", 1);
			recipe.AddIngredient(ItemID.ChlorophyteOre,5);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
	public class DragonsMightPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon's Might Potion");
			Tooltip.SetDefault("50% increase to all damage types except Summon damage" +
				"\nLasts 20 seconds, inflicts Weakness after it ends\nThis cannot be stopped by being immune");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.potion = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("DragonsMight");
			item.buffTime = 60*20;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RestorationPotion,2);
			recipe.AddIngredient(ItemID.BottledHoney);
			recipe.AddIngredient(null, "OmniSoul", 2);
			recipe.AddIngredient(null, "Fridgeflame", 2);
			recipe.AddIngredient(null, "MurkyGel", 4);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 20);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this,2);
			recipe.AddRecipe();
		}

		public override void OnConsumeItem(Player player)
		{
			//SLWorld.EnterSubworld("SGAmod_Blank");
			//RippleBoom.MakeShockwave(player.Center,8f,1f,10f,60,1f);
			//Achivements.SGAAchivements.UnlockAchivement("TPD", Main.LocalPlayer);
			//SGAmod.FileTest();
			//NormalWorldGeneration.PlaceCaiburnShrine(player.Center / 16f);
			//WorldGen.placeTrap((int)(player.Center.X / 16f), (int)(player.Center.Y / 16f)+1, 0);
		}
	}

	public class IceFirePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflame Concoction");
			Tooltip.SetDefault("'A Potion of Ice and Fire'" +
				"\nGrants 25% reduced Damage-over-time caused by Debuffs\nGain an extra 15% less damage while you have Frostburn or OnFire! (Both do not stack)\n25% less damage from cold sources, Obsidian Rose effect\n" + Idglib.ColorText(Color.Red, "Removes Immunity to both Frostburn and OnFire!"));
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 20000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.potion = true;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("IceFirePotion");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LavaBucket, 1);
			recipe.AddIngredient(ItemID.WarmthPotion, 2);
			recipe.AddIngredient(null, "CryostalBar", 2);
			recipe.AddIngredient(null, "IceFairyDust", 2);
			recipe.AddIngredient(null, "Fridgeflame", 2);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
}