using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SGAmod.Generation;
using SGAmod;
using Idglibrary;
using SGAmod.HavocGear.Items;
using SGAmod.Buffs;
using Microsoft.Xna.Framework.Graphics;
//using SubworldLibrary;

namespace SGAmod.Items.Consumables
{
	public class DeificHealingPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Deific Healing Potion");
			Tooltip.SetDefault("'A source of healing from the same powers that made Draken'\nResets Life Regen to max when consumed");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Expert;
			item.value = 5000;
			item.potion = true;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.healLife = 300;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
		}

        public override bool UseItem(Player player)
        {
			player.lifeRegenTime += 10000;
			player.lifeRegenCount += 10000;
			return true;
		}

        public override Color? GetAlpha(Color lightColor)
        {
            return Main.hslToRgb((Main.GlobalTime*0.75f)%1f,1f,0.75f);
        }

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frameNotUsed, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D texture = mod.GetTexture("Items/Consumables/DeificHealingPotionEffect");
			Vector2 drawOrigin = texture.Size() / 2f;
			position += new Vector2(drawOrigin.X * scale, drawOrigin.Y * scale);

			spriteBatch.Draw(Main.itemTexture[item.type], position, null, GetAlpha(itemColor) == null ? Color.White : (Color)GetAlpha(itemColor), 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, position, null, Color.White, 0f, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{

			Texture2D texture = mod.GetTexture("Items/Consumables/DeificHealingPotionEffect");
			Vector2 drawOrigin = texture.Size() / 2f;

			spriteBatch.Draw(Main.itemTexture[item.type], item.Center-Main.screenPosition, null, GetAlpha(lightColor) == null ? Color.White : (Color)GetAlpha(lightColor), rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, lightColor, rotation, drawOrigin, scale, SpriteEffects.None, 0f);
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SuperHealingPotion, 1);
			recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2);
			recipe.AddIngredient(ModContent.ItemType<ByteSoul>(), 6);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}	
	
	public class ToxicityPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Toxicity Potion");
			Tooltip.SetDefault("'60 IQ dialog of swearing intensifies'\nNearby enemies who are Stinky may infect other enemies when near you\nWhile you are Stinky and take damage, you spit out random swear words\nThis can rarely happen when you damage a Stinky enemy\nThese do area damage to ALL nearby NPCs\nThis is boosted by Thorns and if they are also Stinky\nFurthermore you also gain reduced aggro\nAll Town NPCs sell [i: " + ItemID.StinkPotion + "] to you while under this effect\n" + Idglib.ColorText(Color.Red, "Grants immunity to Intimacy and Lovestruct")+"\n"+Idglib.ColorText(Color.Red, "NPCs become unhappy talking to you and charge more money"));
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.LightRed;
			item.value = 5000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("ToxicityPotionBuff");
			item.buffTime = 60 * 600;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.StinkPotion, 1);
			recipe.AddIngredient(ItemID.ThornsPotion, 1);
			recipe.AddIngredient(ItemID.ShadowScale, 8);
			recipe.AddIngredient(ItemID.VilePowder, 5);
			recipe.AddIngredient(ModContent.ItemType<MurkyGel>(), 6);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
	public class IntimacyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Intimacy Potion");
			Tooltip.SetDefault("'Their hearts will be yours!'\nNearby enemies who are Lovestruct will lose health over time based on your Life Regen\nTown NPCs who are Lovestruct have reduced prices\nWhile you are Lovestruct all hearts heal 10 more health and draw more aggro\nNearby players who are Lovestruct gain 20% of your life regen to their own\nAll Town NPCs sell [i: " + ItemID.LovePotion + "] to you while under this effect\n" + Idglib.ColorText(Color.Red, "Grants immunity to Toxicity and Stinky"));
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.LightRed;
			item.value = 5000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("IntimacyPotionBuff");
			item.buffTime = 60 * 600;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LovePotion, 1);
			recipe.AddIngredient(ItemID.HeartreachPotion, 1);
			recipe.AddIngredient(ItemID.WarmthPotion, 1);
			recipe.AddIngredient(ItemID.TissueSample, 4);
			recipe.AddIngredient(ItemID.LifeCrystal, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}	
	public class TriggerFingerPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trigger Finger Potion");
			Tooltip.SetDefault("Increases the attack speed of non-autofire guns by 15%");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Orange;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("TriggerFingerPotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 5);
			recipe.AddIngredient(ItemID.DesertFossil, 3);
			recipe.AddIngredient(ItemID.IllegalGunParts, 1);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this, 5);
			recipe.AddRecipe();
		}
	}
	public class TrueStrikePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("True Strike Potion");
			Tooltip.SetDefault("Boosts the damage of True Melee weapons by 20%\nThis includes held projectiles, like spears");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Orange;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("TrueStrikePotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Ale,1);
			recipe.AddIngredient(null, "UnmanedOre", 2);
			recipe.AddIngredient(null, "WraithFragment3", 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
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
			item.rare = ItemRarityID.Orange;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("ClarityPotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater,2);
			recipe.AddIngredient(ItemID.Sunflower, 1);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(null, "MurkyGel", 4);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class TinkerPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tinker Potion");
			Tooltip.SetDefault("Greatly reduces the loss from uncrafting");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Orange;
			item.value = 250;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("TinkerPotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BottledMud", 2);
			recipe.AddIngredient(null, "VirulentOre", 2);
			recipe.AddIngredient(null, "DankWood", 6);
			recipe.AddIngredient(null, "DankCore", 1);
			recipe.AddIngredient(null, "VialofAcid", 4);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class TooltimePotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tooltime Potion");
			Tooltip.SetDefault("Greatly increases the knockback of tools");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Blue;
			item.value = 50;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("TooltimePotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BottledMud", 1);
			recipe.AddRecipeGroup("Wood", 5);
			recipe.AddIngredient(ItemID.Acorn, 2);
			recipe.AddIngredient(ItemID.TungstenOre, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BottledMud", 1);
			recipe.AddRecipeGroup("Wood", 3);
			recipe.AddIngredient(ItemID.Acorn, 1);
			recipe.AddIngredient(ItemID.SilverOre, 1);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
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
			item.rare = ItemRarityID.Lime;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("RagnarokBrewBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater,3);
			recipe.AddIngredient(null, "Entrophite", 15);
			recipe.AddIngredient(null, "StygianCore", 1);
			recipe.AddIngredient(null, "FieryShard", 2);
			recipe.AddIngredient(null, "UnmanedOre", 5);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
	public class EnergyPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Energy Potion");
			Tooltip.SetDefault("'A bottled transformer for the road'\n+1 passive Electric Charge Rate\nYour Electric Charge Recharge Delay is halved");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Green;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("EnergyPotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 1);
			recipe.AddIngredient(ItemID.Sunflower);
			recipe.AddIngredient(null, "NoviteOre", 1);
			recipe.AddIngredient(ItemID.Meteorite);
			recipe.AddIngredient(ItemID.RainCloud);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
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
			item.rare = ItemRarityID.Green;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = SGAmod.Instance.BuffType("CondenserPotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledWater, 2);
			recipe.AddIngredient(ItemID.StrangeBrew);
			recipe.AddIngredient(ItemID.CookedMarshmallow);
			recipe.AddIngredient(null, "FrigidShard", 2);
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
			item.rare = ItemRarityID.Lime;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = Idglib.Instance.BuffType("RadCure");
			item.buffTime = 60 * 60;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.RestorationPotion, 2);
			recipe.AddIngredient(ItemID.StrangeBrew);
			recipe.AddIngredient(null, "ManaBattery", 1);
			recipe.AddIngredient(ItemID.ChlorophyteOre, 4);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}

	public class DragonsMightPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dragon's Might Potion");
			Tooltip.SetDefault("50% increase to all damage types except Summon damage" +
				"\nLasts 20 seconds, inflicts Weakness after it ends\nThis cannot be stopped by being immune\nCannot be used while Weakened");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.Lime;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
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
			recipe.AddIngredient(null, "OmniSoul", 2);
			recipe.AddIngredient(null, "Fridgeflame", 2);
			recipe.AddIngredient(null, "MurkyGel", 3);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 20);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this,2);
			recipe.AddRecipe();
		}

        public override bool CanUseItem(Player player)
        {
			return !player.HasBuff(ModContent.BuffType<WorseWeakness>());
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
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = mod.BuffType("IceFirePotionBuff");
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.BottledHoney, 2);
			recipe.AddIngredient(null, "CryostalBar", 1);
			recipe.AddIngredient(null, "IceFairyDust", 1);
			recipe.AddIngredient(null, "Fridgeflame", 3);
			recipe.AddTile(TileID.AlchemyTable);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class PhalanxPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phalanx Potion");
			Tooltip.SetDefault("'Stronger Together'" +
				"\nIncreases your blocking angle on all shields by 15%\nFor every player nearby that has a shield out, you gain 8 defense");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 1;
			item.value = 250;
			item.useStyle = ItemRarityID.Green;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = ModContent.BuffType<PhalanxPotionBuff>();
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			for (int i = 0; i < 2; i += 1)
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(ItemID.IronskinPotion, 1);
				recipe.AddIngredient(ModContent.ItemType<BottledMud>(), 1);
				recipe.AddIngredient(ModContent.ItemType<Glowrock>(), 4);
				recipe.AddIngredient(i == 0 ? ItemID.SilverOre : ItemID.TungstenOre, 2);
				recipe.AddTile(TileID.Bottles);
				recipe.SetResult(this, 2);
				recipe.AddRecipe();
			}
		}
	}

	public class ReflexPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Reflex Potion");
			Tooltip.SetDefault("Increases the period of time you can Just Block with your shield");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = ItemRarityID.LightPurple;
			item.value = 500;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = ModContent.BuffType<ReflexPotionBuff>();
			item.buffTime = 60 * 300;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SwiftnessPotion, 1);
			recipe.AddIngredient(ItemID.Ale, 1);
			recipe.AddIngredient(ModContent.ItemType<Biomass>(), 2);
			recipe.AddTile(TileID.Bottles);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}


}