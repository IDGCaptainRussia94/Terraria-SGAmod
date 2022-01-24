using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Idglibrary;
using SGAmod.Items.Placeable;
using SGAmod.Items.Weapons.Vibranium;
using SGAmod.Items.Accessories;
using Terraria.Utilities;
using SGAmod.Items.Placeable.DankWoodFurniture;

namespace SGAmod.HavocGear.Items
{
	public class MoistSand : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("MoistSand");
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Moist Sand");
			Tooltip.SetDefault("'expect nothing else from sand thrown into water'");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<MoistSand>());
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(ItemID.SandBlock, 1);
			recipe.AddRecipe();
		}

	}
	public class BottledMud : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bottled Mud");
			Tooltip.SetDefault("'brown and full of sedimental value'");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 14;
			item.maxStack = 99;
			item.value = 50;
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle);
			recipe.AddRecipeGroup("SGAmod:Mud", 3);
			recipe.needWater = true;
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
	public class VirulentBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Bar");
			Tooltip.SetDefault("Condensed life essence in bar form");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 14;
			item.maxStack = 99;
			item.value = 1000;
			item.rare = 5;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("VirulentBarTile");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BiomassBar>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VirulentOre>(), 3);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}
	public class VirulentOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Ore");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 99;
			item.value = 100;
			item.rare = ItemRarityID.Pink;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("VirulentOre");
		}

	}
	public class BiomassBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosyte Bar");
			Tooltip.SetDefault("A hardened bar made from parasitic biomass reacting from murky gel and moss");
		}

		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 14;
			item.maxStack = 99;
			item.value = 100;
			item.rare = ItemRarityID.Green;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("BiomassBarTile");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/BiomassBar"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType < Biomass>(), 5);
			recipe.AddIngredient(ModContent.ItemType < MurkyGel>(),2);
			recipe.AddIngredient(ModContent.ItemType<DecayedMoss>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Weapons.SwampSeeds>(), 2);
			recipe.AddIngredient(ModContent.ItemType<MoistSand>(), 1);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
	public class Biomass : ModItem
	{
		public override void SetDefaults()
		{

			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.rare = ItemRarityID.Green;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("Biomass");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Biomass"); }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Photosyte");
			Tooltip.SetDefault("'Parasitic plant matter'\nIs found largely infesting clouds where it can gain the most sunlight");
		}

	}
	public class DankWood : ModItem
	{
		public override void SetDefaults()
		{
			item.value = 50;
			item.rare = ItemRarityID.Blue;
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBlock>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Wood");
			Tooltip.SetDefault("It smells odd...");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWoodFence>(), 4);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<BrokenDankWoodFence>(), 4);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWoodWall>(), 4);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWoodPlatform>(), 2);
			recipe.SetResult(this);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<SwampWoodWall>(), 4);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class DankCore : ModItem
	{
		public override void SetDefaults()
		{
			item.value = 2500;
			item.rare = 2;
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Core");
			Tooltip.SetDefault("'Dark, Dank, Dangerous...'");
		}

	}

	public class MurkyGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Murky Gel");
			Tooltip.SetDefault("Extra sticky, stinky too");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/MurkyGel"); }
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 50;
			item.rare = 3;
		}
	}
	public class FieryShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fiery Shard");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.maxStack = 99;
			item.value = 25;
			item.rare = 3;
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
			item.alpha = 30;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Orange.ToVector3() * 0.55f * Main.essScale);
		}
	}
}

namespace SGAmod.Items
{

	public class Glowrock : ModItem, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowrock");
			Tooltip.SetDefault("These rocks seem to give the Asteriods a glow; Curious.\nExtract it via an Extractinator for some goodies!\nDoesn't have much other use, outside of illegal interests");
			ItemID.Sets.ExtractinatorMode[item.type] = item.type;
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
		}

        public override void ExtractinatorUse(ref int resultType, ref int resultStack)
        {
			if (Main.rand.Next(8) < 4)
				return;

			WeightedRandom<(int, int)> WR = new WeightedRandom<(int, int)>();
			
			if (NPC.downedPlantBoss)
			{
				WR.Add((ItemID.Ectoplasm, Main.rand.Next(1, 1)), 1);
			}

			if (NPC.downedMoonlord)
				WR.Add((ItemID.LunarOre, Main.rand.Next(1, 3)), 1);

			WR.Add((ItemID.SoulofLight, 1), 1);
			WR.Add((ItemID.SoulofNight, 1), 1);
			WR.Add((ItemID.DarkBlueSolution, Main.rand.Next(1, 9)),0.50);
			WR.Add((ItemID.BlueSolution, Main.rand.Next(1, 9)), 0.50);

			WR.needsRefresh = true;
			(int, int) thing = WR.Get();
			resultType = thing.Item1;
			resultStack = thing.Item2;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Blue.ToVector3() * 0.55f);
		}

        public int RadioactiveHeld()
        {
			return 2;
        }

        public int RadioactiveInventory()
        {
			return 1;
        }
    }

	public class CelestineChunk : ModItem, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestine Chunk");
			Tooltip.SetDefault("Inert and radioactive Luminite...");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.autoReuse = true;
			item.consumable = true;
			item.value = 0;
			item.rare = ItemRarityID.Blue;
		}

		public override string Texture => "Terraria/Item_" + ItemID.LunarOre;

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.Lerp(Color.DarkGray, Color.Gray, 0.50f + (float)Math.Sin(Main.GlobalTime / 2f) / 2f);
        }

        public override void AddRecipes()
        {
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarOre, 1);
			recipe.AddIngredient(this, 4);
			recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(ItemID.LunarBar, 2);
			recipe.AddRecipe();
		}

        public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.White.ToVector3() * 0.55f);
		}

		public int RadioactiveHeld()
		{
			return 2;
		}

		public int RadioactiveInventory()
		{
			return 1;
		}
	}

	public class OverseenCrystal : ModItem, IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Overseen Crystal");
			Tooltip.SetDefault("Celestial Shards manifested from Phaethon's creators; resonates with charged forgotten spirits\nMay be used to fuse several strong materials together with ease\nSurely a shady dealer will also be interested in trading for these...");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 1000;
			item.rare = ItemRarityID.Blue;
		}

		public int RadioactiveHeld()
		{
			return 3;
		}

		public int RadioactiveInventory()
		{
			return 2;
		}

		public override void AddRecipes()
		{

			int tileType = ModContent.TileType<Tiles.ReverseEngineeringStation>();

			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<UnmanedOre>(), 2);
			recipe.AddIngredient(ModContent.ItemType<NoviteOre>(), 2);
			recipe.AddIngredient(this, 4);
			recipe.AddTile(tileType);
			recipe.SetResult(ModContent.GetInstance<PrismalOre>(),8);
			recipe.AddRecipe();

			/*recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientFabricItem>(), 4);
			recipe.AddIngredient(ItemID.CrystalShard, 1);
			recipe.AddIngredient(this, 2);
			recipe.AddTile(tileType);
			recipe.SetResult(ModContent.GetInstance<VibraniumCrystal>(), 1);
			recipe.AddRecipe();*/

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientFabricItem>(), 5);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 2);
			recipe.AddIngredient(this, 2);
			recipe.AddTile(tileType);
			recipe.SetResult(ModContent.GetInstance<VibraniumPlating>(), 2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 1);
			recipe.AddIngredient(this, 2);
			recipe.AddTile(tileType);
			recipe.SetResult(ModContent.GetInstance<OmniSoul>(), 2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FossilOre, 2);
			recipe.AddIngredient(this, 1);
			recipe.AddTile(tileType);
			recipe.SetResult(ItemID.DefenderMedal, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HallowedBar, 4);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.AddIngredient(this, 5);
			recipe.AddTile(tileType);
			recipe.SetResult(ModContent.GetInstance<Consumables.DivineShower>(), 1);
			recipe.AddRecipe();

			/*ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<StarCollector>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 3);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<RustedBulwark>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2);
			recipe.AddIngredient(ItemID.ArmorPolish, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<EnchantedShieldPolish>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 3);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<MurkyCharm>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<MagusSlippers>());
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:VanillaAccessory", 2);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 4);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(ModContent.GetInstance<MagusSlippers>());
			recipe.AddRecipe();*/

		}

	}
	public class VibraniumCrystal : ModItem,IRadioactiveItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Crystal");
			Tooltip.SetDefault("'Makes a humming sound while almost shaking out your hands'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 500;
			item.rare = ItemRarityID.Red;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("VibraniumCrystalTile");
		}
        public override bool Autoload(ref string name)
        {
            return SGAmod.VibraniumUpdate;
        }

		public int RadioactiveHeld()
		{
			return 3;
		}

		public int RadioactiveInventory()
		{
			return 3;
		}

	}
	public class VibraniumPlating : VibraniumCrystal
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Plating");
			Tooltip.SetDefault("'Dark cold steel; it constantly vibrates to the touch'");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 400;
			item.rare = ItemRarityID.Purple;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.VibraniumPlatingTile>();
		}
	}
	public class VibraniumBar : VibraniumText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vibranium Bar");
			Tooltip.SetDefault("'This alloy is just barely stable enough to not phase out of existance'");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 2500;
			item.rare = ItemRarityID.Purple;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("VibraniumBarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("VibraniumCrystal"), 3);
			recipe.AddIngredient(mod.ItemType("VibraniumPlating"), 3);
			recipe.AddIngredient(ItemID.LunarBar, 2);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.needLava = true;
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
		public override bool Autoload(ref string name)
		{
			return SGAmod.VibraniumUpdate;
		}
	}

	public class IceFairyDust : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ice Fairy Dust");
			Tooltip.SetDefault("It doesn't feel like it's from this universe");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 75;
			item.rare = 5;
		}
	}
	public class FrigidShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frigid Shard");
			Tooltip.SetDefault("Raw essence of ice");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 0;
			item.rare = 1;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Aqua.ToVector3() * 0.25f);
		}
	}	
	public class Fridgeflame : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflame");
			Tooltip.SetDefault("Alloy of hot and cold essences");
		}

		public override void SetDefaults()
		{
			item.width = 22;
			item.height = 22;
			item.maxStack = 99;
			item.value = 200;
			item.rare = 6;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.White.ToVector3() * 0.65f * Main.essScale);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 1);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 1);
			recipe.needLava = true;
			recipe.needWater = true;
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class VialofAcid : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vial of Acid");
			Tooltip.SetDefault("Highly Corrosive");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 100;
			item.rare = 2;
		}
	}
	public class OmniSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Omni Soul");
			Tooltip.SetDefault("'The essence of essences combined'");
			// ticksperframe, frameCount
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(5, 4));
			ItemID.Sets.AnimatesAsSoul[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.SoulofSight);
			item.width = refItem.width;
			item.height = refItem.height;
			item.maxStack = 999;
			item.value = 1000;
			item.rare = 6;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime/3f)%1f, 0.85f, 0.50f);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.SoulofLight, 1);
			recipe.AddIngredient(ItemID.SoulofNight, 1);
			recipe.AddIngredient(ItemID.SoulofFlight, 1);
			recipe.AddIngredient(ItemID.SoulofFright, 1);
			recipe.AddIngredient(ItemID.SoulofMight, 1);
			recipe.AddIngredient(ItemID.SoulofSight, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Main.hslToRgb((Main.GlobalTime / 3f)%1f, 0.85f, 0.80f).ToVector3() * 0.55f * Main.essScale);
		}
	}
	public class Entrophite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entrophite");
			Tooltip.SetDefault("Corrupted beyond the veils of life");
		}
		public override void SetDefaults()
		{
			item.value = 100;
			item.rare = ItemRarityID.Lime;
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Dimensions.Tiles.EntrophicOre>();
		}

	}

	public class WovenEntrophite : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Woven Entrophite");
			Tooltip.SetDefault("Suprisingly strong, after being interlaced with souls");
		}

		public override void SetDefaults()
		{
			item.value = 250;
			item.rare = ItemRarityID.Lime;
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("WovenEntrophiteTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<OmniSoul>(), 1);
			recipe.AddIngredient(ModContent.ItemType<Entrophite>(), 10);
			recipe.AddTile(TileID.Loom);
			recipe.SetResult(this, 10);
			recipe.AddRecipe();
		}

	}

	public class AdvancedPlating : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Advanced Plating");
			Tooltip.SetDefault("Advanced for the land of Terraria's standards, that is");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 1000;
			item.rare = ItemRarityID.Green;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.AdvancedPlatingTile>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 2);
			recipe.AddIngredient(ItemID.Wire, 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this,3);
			recipe.AddRecipe();
		}
	}
	public class ManaBattery : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mana Battery");
			Tooltip.SetDefault("Encapsulated mana to be used as a form of energy for techno weapons");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 26;
			item.value = 15000;
			item.rare = 3;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 3);
			recipe.AddIngredient(ItemID.ManaCrystal, 1);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 3);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class PlasmaCell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Plasma Cell");
			Tooltip.SetDefault("Heated plasmic energy resides within");
		}

		public override void SetDefaults()
		{
			item.maxStack = 20;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = ItemRarityID.Yellow;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 2);
			recipe.AddIngredient(ModContent.ItemType<WraithFragment4>(), 2);
			recipe.AddIngredient(ModContent.ItemType<ManaBattery>(), 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 5);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 5);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EmptyPlasmaCell>(), 1);
			recipe.AddIngredient(ItemID.MeteoriteBar, 2);
			recipe.AddIngredient(ModContent.ItemType<VialofAcid>(), 3);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EmptyPlasmaCell>(), 1);
			recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 2);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();

		}
	}
	public class EmptyPlasmaCell : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Plasma Cell");
			Tooltip.SetDefault("Casing not yet filled with plasma");
		}

		public override void SetDefaults()
		{
			item.maxStack = 20;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0,0,10,0);
			item.rare = ItemRarityID.LightRed;
		}
	}
	public class CryostalBar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cryostal Bar");
			Tooltip.SetDefault("Condensed ice magic has formed into this bar");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = 1000;
			item.rare = 5;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("CryostalBarTile");
		}
	}
	public class EldritchTentacle : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Eldritch Tentacle");
			Tooltip.SetDefault("Remains of an eldritch deity\nMay be used alongside fragments to craft all of Moonlord's drops");
		}

		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 24;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 9;
		}
	}	
	public class IlluminantEssence : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Illuminant Essence");
			Tooltip.SetDefault("'Shards of Heaven'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
        public override string Texture => "SGAmod/Items/IlluminantEssence";
        public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.HotPink.ToVector3() * 0.55f * Main.essScale);
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 11;
		}
	}	
	public class AuroraTear : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aurora Tear");
			Tooltip.SetDefault("'Auroric Energy from the Banshee, it seems to be inert...'");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Lerp(Color.BlueViolet, Color.HotPink, (float)Math.Sin((Main.essScale-0.70f)/0.30f)).ToVector3() * 0.75f * Main.essScale);
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 0, 50, 0);
			item.rare = 11;
		}
	}

	public class AuroraTearAwoken : ModItem, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Awoken Aurora Tear");
			Tooltip.SetDefault("'Bustling with awoken, Luminous energy'");
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.Lerp(Color.BlueViolet, Color.HotPink, (float)Math.Sin((Main.essScale - 0.70f) / 0.30f)).ToVector3() * 0.85f * Main.essScale);
		}
		public override void SetDefaults()
		{
			item.maxStack = 30;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.noMelee = true; //so the item's animation doesn't do damage
			item.value = 0;
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item35;
		}
	}

	public class LunarRoyalGel : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunar Royal Gel");
			Tooltip.SetDefault("From the moon-infused Pinky");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 6));
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 16;
			item.height = 16;
			item.value = 100000;
			item.rare = 9;
		}
	}
	public class AncientFabricItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ancient Fabric");
			Tooltip.SetDefault("Strands of Reality, predating back to the Big Bang");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.DarkRed.ToVector3() * 0.15f * Main.essScale);
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = 10;
		}
	}

	public class WatchersOfNull : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("01001110 01010101 01001100 01001100");
			Tooltip.SetDefault("'Essence of N0ll Watchers, watching...'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(7, 13));
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 32;
			item.height = 32;
			item.value = 100000;
			item.rare = 10;
		}
	}

	public class CosmicFragment: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cosmic Fragment");
			Tooltip.SetDefault("The core of a celestial experiment; it holds unmatched power\nUsed to make Dev items");
			ItemID.Sets.ItemIconPulse[item.type] = true;
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 16;
			item.height = 16;
			item.value = 0;
			item.rare = 9;
			item.expert=true;
		}

		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange *= 5;
		}

		public override bool GrabStyle(Player player)
		{
			Vector2 vectorItemToPlayer = player.Center - item.Center;
			Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
			item.velocity = item.velocity + movement;
			item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
			return true;
		}

		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Color.WhiteSmoke.ToVector3() * 0.55f * Main.essScale);
		}

	}

	public class EmptyCharm: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Empty Amulet");
			Tooltip.SetDefault("An empty amulet necklace, ready for enchanting");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 20;
			item.height = 20;
			item.value = 10000;
			item.rare = 0;
			item.consumable = false;
		}
	}

	public class StarMetalMold: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Mold");
			Tooltip.SetDefault("A mold used to make Wraith Cores, it seems fit to mold bars from heaven\nIs not consumed in crafting Star Metal Bars");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 20;
			item.height = 20;
			item.value = 0;
			item.rare = 8;
			item.consumable = false;
		}
	}

	public class StarMetalBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Bar");
			Tooltip.SetDefault("'This bar is a glimming white sliver that shimmers with stars baring the color of pillars'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 20;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 25, 0);
			item.rare = 9;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("StarMetalBarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new StarMetalRecipes(mod);
			recipe.AddIngredient(mod.ItemType("StarMetalMold"), 1);
			recipe.AddIngredient(ItemID.LunarOre, 1);
			recipe.AddRecipeGroup("Fragment", 4);
			recipe.SetResult(this,4);
			recipe.AddRecipe();
		}

	}
	public class DrakeniteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Drakenite Bar");
			Tooltip.SetDefault("A Bar forged from the same powers that created Draken...");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 20;
			item.height = 20;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = 9;
			item.consumable = false;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("DrakeniteBarTile");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					line.overrideColor = Color.Lerp(Color.DarkGreen, Color.White, 0.5f + (float)Math.Sin(Main.GlobalTime * 8f));
				}
			}
		}
		public static Texture2D[] staticeffects = new Texture2D[32];
		public static void CreateTextures()
		{
			if (!Main.dedServ)
			{
				Texture2D atex = ModContent.GetTexture("SGAmod/Items/DrakeniteBarHalf");
				int width = atex.Width; int height = atex.Height;
				for (int index = 0; index < staticeffects.Length; index++)
				{
					Texture2D tex = new Texture2D(Main.graphics.GraphicsDevice, width, height);

					var datacolors2 = new Color[atex.Width * atex.Height];
					atex.GetData(datacolors2);
					tex.SetData(datacolors2);

					DrakeniteBar.staticeffects[index] = new Texture2D(Main.graphics.GraphicsDevice, width, height);
					Color[] dataColors = new Color[atex.Width * atex.Height];


					for (int y = 0; y < height; y++)
					{
						for (int x = 0; x < width; x += 1)
						{
							if (Main.rand.Next(0, 16) == 1)
							{
								int therex = (int)MathHelper.Clamp((x), 0, width);
								int therey = (int)MathHelper.Clamp((y), 0, height);
								if (datacolors2[(int)therex + therey * width].A > 0)
								{

									dataColors[(int)therex + therey * width] = Main.hslToRgb(Main.rand.NextFloat(0f, 1f) % 1f, 0.6f, 0.8f) * (0.5f);
								}
							}
							if (Main.rand.Next(0, 8) > Math.Abs(x-(index-8)))
							{
								int therex = (int)MathHelper.Clamp((x), 0, width);
								int therey = (int)MathHelper.Clamp((y), 0, height);
								if (datacolors2[(int)therex + therey * width].A > 0)
								{
									dataColors[(int)therex + therey * width] = Main.hslToRgb(((float)(index-8)/ (float)width) % 1f, 0.9f, 0.75f)*(0.80f*(1f-(Math.Abs((float)x - ((float)index -8f))/8f)));
								}
							}


						}

					}

					DrakeniteBar.staticeffects[index].SetData(dataColors);
				}
			}

		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,
	Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = DrakeniteBar.staticeffects[(int)(Main.GlobalTime*20f)%DrakeniteBar.staticeffects.Length];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale*2f, SpriteEffects.None, 0f);
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.LunarBar, 1);
			recipe.AddIngredient(mod.ItemType("ByteSoul"), 10);
			recipe.AddIngredient(mod.ItemType("WatchersOfNull"), 1);
			recipe.AddIngredient(mod.ItemType("AncientFabricItem"), 25);
			recipe.AddIngredient(mod.ItemType("HopeHeart"), 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}

	public class CopperWraithNotch: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Wraith Notch");
			Tooltip.SetDefault("Intact remains of the Copper Wraith's animated armor");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 20;
			item.rare = ItemRarityID.White;
		}
	}
	public class CobaltWraithNotch: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Wraith Notch");
			Tooltip.SetDefault("Intact remains of the Cobalt Wraith's animated armor, stronger than before");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 200;
			item.rare = ItemRarityID.Pink;
		}
	}
	public class LuminiteWraithNotch: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Wraith Notch");
			Tooltip.SetDefault("Intact remains of the Luminate Wraith's special armor");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
		}
	}
	public class WraithFragment: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Wraith Shard");
			Tooltip.SetDefault("The remains of a weak wraith; it is light and conductive");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 5;
			item.rare = ItemRarityID.White;
		}
	}
	public class WraithFragment2: WraithFragment
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Wraith Shard");
			Tooltip.SetDefault("The remains of a weak wraith; it is soft and malleable");
		}
	}

	public class WraithFragment3: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bronze Alloy Wraith Shard");
			Tooltip.SetDefault("Tin and copper combined through the fires of a hellforge; thus stronger than a standard shard");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 25;
			item.rare = ItemRarityID.Orange;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("WraithFragment"), 2);
			recipe.AddIngredient(ItemID.TinOre, 4);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this,2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("WraithFragment2"), 2);
			recipe.AddIngredient(ItemID.CopperOre, 4);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(this,2);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddIngredient(this, 1);
			recipe.AddIngredient(ItemID.LivingFireBlock, 3);
			recipe.AddTile(TileID.Hellforge);
			recipe.SetResult(mod.ItemType("FieryShard"));
			recipe.AddRecipe();

		}
	}

	public class WraithFragment4 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Wraith Shard");
			Tooltip.SetDefault("The remains of a stronger wraith; applyable uses in alloys and highly resistant to corrosion");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 30;
			item.rare = ItemRarityID.Green;
		}
	}

	public class UnmanedBar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Bar");
			Tooltip.SetDefault("This alloy of Novus and the power of the wraiths have awakened some of its dorment power\nMay be interchanged for iron bars in some crafting recipes");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 16;
			item.height = 16;
			item.value = 25;
			item.rare = ItemRarityID.Blue;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("UnmanedBarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedOre"), 4);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards",3);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this,2);
			recipe.AddRecipe();
		}
	}
	public class UnmanedOre: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Ore");
			Tooltip.SetDefault("Stone laden with doment power...");
		}
	public override void SetDefaults()
        {
		item.width = 16;
		item.height = 16;
		item.maxStack = 999;
		item.value = 10;
		item.rare = ItemRarityID.Blue;
		item.alpha = 0;
		item.useTurn = true;
		item.autoReuse = true;
		item.useAnimation = 15;
		item.useTime = 10;
		item.useStyle = ItemUseStyleID.SwingThrow;
		item.consumable = true;
		item.createTile = mod.TileType("UnmanedOreTile");

		}
	}
	public class NoviteOre : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Ore");
			Tooltip.SetDefault("Brassy scrap metal from a time along ago, might be of electronical use...");
		}
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.maxStack = 999;
			item.value = 10;
			item.rare = ItemRarityID.Blue;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("NoviteOreTile");

		}
	}
	public class NoviteBar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Bar");
			Tooltip.SetDefault("This Brassy alloy reminds you of 60s scifi");
		}
		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = 16;
			item.height = 16;
			item.value = 25;
			item.rare = ItemRarityID.Blue;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("NoviteBarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteOre"), 4);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 3);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 2);
			recipe.AddRecipe();
		}
	}
	public class MoneySign : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raw Avarice");
			Tooltip.SetDefault("'pure greed'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 75000;
			item.rare = ItemRarityID.Red;
		}
	}

	public class ByteSoul : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Soul of Byte");
			Tooltip.SetDefault("'remains of the Hellion Core'");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 4));
			ItemID.Sets.ItemNoGravity[item.type] = true;
			ItemID.Sets.ItemIconPulse[item.type] = true;
		}
		/*public override string Texture
		{
			get { return ("Terraria/Item_"+Main.rand.Next(0,2000)); }
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb(Main.rand.NextFloat(0f, 1f), 0.75f, 0.65f);
		}*/
		public override void PostUpdate()
		{
			Lighting.AddLight(item.Center, Main.hslToRgb((-Main.GlobalTime+(item.whoAmI*7.4231f))%1f,0.92f,0.85f).ToVector3() * 0.5f);
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 10000;
			item.rare = ItemRarityID.Red;
		}
	}

	public class AssemblyStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Assembly Star");
			Tooltip.SetDefault("'Raw assembly code forged directly from Draken'\nCan be used to craft previously uncraftable items");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override string Texture
		{
			get { return "Terraria/SunOrb"; }
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Orange*MathHelper.Clamp((float)(Math.Sin(Main.GlobalTime)/2)+1f,0,1);
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 14;
			item.height = 14;
			item.value = 0;
			item.rare = ItemRarityID.Quest;
		}
	}

	public class StygianCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stygian Star");
			Tooltip.SetDefault("'Torn from Stygian Veins with a mining tool, this star is burning fabric made manifest...'");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = 50000;
			item.maxStack = 10;
			item.rare = ItemRarityID.Red;
		}
		public override string Texture
		{
			get { return "Terraria/Sun"; }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.Magenta*0.50f;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);
			

			for (float i = 0; i < 1f; i += 0.10f)
			{
				spriteBatch.Draw(inner, drawPos, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.5f)*0.50f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 1.75f * (((Main.GlobalTime / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}

			return true;
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

			Texture2D inner = Main.itemTexture[ModContent.ItemType<AssemblyStar>()];

			Vector2 slotSize = new Vector2(52f, 52f);
			Vector2 position = item.Center-Main.screenPosition;

			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.10f)
			{
				spriteBatch.Draw(inner, position, null, (Color.DarkMagenta * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.5f) * 0.50f, i * MathHelper.TwoPi, textureOrigin, 1f * (0.5f + 1.75f * (((Main.GlobalTime / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(Main.itemTexture[item.type],position,null,alphaColor,rotation, Main.itemTexture[item.type].Size()/2f, 128f/256f, SpriteEffects.None, 0f);

			return false;
		}

	}

	public class HopeHeart : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hopeful Heart");
			Tooltip.SetDefault("'There is always hope in the darkness...'\nRestores 30 lost max HP when picked up\nIs collected if your barely missing any life instead\nCannot be picked up while a boss is alive");
		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Consumables/HopefulHeartItem"; }
		}
		public override bool CanPickup(Player player)
        {
            return !IdgNPC.bossAlive;
        }
        public override bool OnPickup(Player player)
        {
			if (player.GetModPlayer<IdgPlayer>().radationAmmount<5)
            {
				return true;
			}
			UseItem2(player);
			return false;
		}
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
			Lighting.AddLight(item.Center / 16f, (Color.PaleGoldenrod * 0.5f).ToVector3());
        }
        public void UseItem2(Player player)
        {
			Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 4, 0.75f, -0.65f);
			player.HealEffect(30*item.stack,true);
			player.GetModPlayer<IdgPlayer>().radationAmmount = Math.Max(player.GetModPlayer<IdgPlayer>().radationAmmount - (30 * item.stack), 0);
			item.TurnToAir();
        }

	}

	public class PrismalBar: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Bar");
			Tooltip.SetDefault("It radiates the true energy of Novus");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 20;
			item.height = 20;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = ItemRarityID.Yellow;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = mod.TileType("PrismalBarTile");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalOre"), 4);
			recipe.AddTile(TileID.AdamantiteForge);
			recipe.SetResult(this,1);
			recipe.AddRecipe();
		}

	}

	public class PrismalOre: ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Ore");
			Tooltip.SetDefault("The power inside is cracked wide open, ready to be used");
		}
	public override void SetDefaults()
        {
		item.width = 16;
		item.height = 16;
		item.maxStack = 99;
		item.value = 7500;
		item.rare = ItemRarityID.Yellow;
		item.alpha = 0;
		item.useTurn = true;
		item.autoReuse = true;
		item.useAnimation = 15;
		item.useTime = 10;
		item.useStyle = ItemUseStyleID.SwingThrow;
		item.consumable = true;
		item.createTile = mod.TileType("PrismalTile");

	}
		public override string Texture
		{
			get { return ("SGAmod/Items/PrismalOre2"); }
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedOre"), 8);
			recipe.AddIngredient(mod.ItemType("NoviteOre"), 8);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 1);
			recipe.AddIngredient(mod.ItemType("Fridgeflame"), 3);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 2);
			recipe.AddIngredient(ItemID.CrystalShard, 3);
			recipe.AddIngredient(ItemID.BeetleHusk, 1);
			recipe.AddTile(mod.GetTile("PrismalStation"));
			recipe.SetResult(this, 16);
			recipe.AddRecipe();
		}

	}

	public class HeliosFocusCrystal : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Helios Focus Crystal");
			Tooltip.SetDefault("An intact focus crystal used to empower Phaethon\nCould be useful for crafting your own empowerment devices");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 0;
			item.rare = ItemRarityID.Yellow;
			item.maxStack = 30;
			//item.damage = 1;
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime/4f) % 1f, 1f, 0.75f);
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.DD2ElderCrystal); }
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Vector2 slotSize = new Vector2(52f, 52f) * scale;
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = Vector2.Zero;

			slotSize.X /= 1.0f;
			slotSize.Y = -slotSize.Y / 4f;
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
			{
				spriteBatch.Draw(Main.itemTexture[item.type], drawPos+(Vector2.UnitX.RotatedBy(f+Main.GlobalTime*2f)*3f), null, Main.hslToRgb(f/MathHelper.TwoPi,1f,0.75f), 0, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			spriteBatch.Draw(Main.itemTexture[item.type], drawPos, null, drawColor, 0, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

			SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(2f);
			SGAmod.FadeInEffect.Parameters["alpha"].SetValue(0.50f);
			SGAmod.FadeInEffect.CurrentTechnique.Passes["ColorToAlphaPass"].Apply();

			spriteBatch.Draw(Main.itemTexture[item.type], drawPos, null, drawColor, 0, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale*1/15f, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
	}

	public class EntropyTransmuter : ModItem
	{
		static internal int MaxEntropy = 100000;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Entropy Transmuter");
			Tooltip.SetDefault("As enemies die near you, the Transmuter absorbs their life essences\nWhich converts Converts Demonite or Crimtane ore in your inventory into Entrophite\nConverts a maximum of 20 per full charge");
		}
		public override void SetDefaults()
		{
			item.value = 0;
			item.rare = ItemRarityID.Green;
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Entropy", "Entropy Collected: "+Main.LocalPlayer.GetModPlayer<SGAPlayer>().entropyCollected + "/" + MaxEntropy));
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Entrophite"), 250);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 1);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 15);
			recipe.AddIngredient(ItemID.Diamond, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class EALogo : ModItem
	{

		public override void SetDefaults()
		{
			item.value = 1000000;
			item.rare = ItemRarityID.Cyan;
			item.width = 16;
			item.height = 16;
			item.maxStack = 1;
			item.expert = true;
		}

		public override void UpdateInventory(Player player)
		{
			player.GetModPlayer<SGAPlayer>().EALogo = true;
			if (player.taxMoney >= Item.buyPrice(0, 10, 0, 0))
			{
				player.taxMoney = 0;
				player.QuickSpawnItem(ItemID.GoldCoin,10);
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EA Logo");
			Tooltip.SetDefault("Lets you charge maximum micro-transactions against your town NPCs\nWhile in your inventory: you can reforge unique prefixes for accessories\nYou automatically collect taxes while you have a Tax Collector\nPicking up Hearts and Mana Stars gives you money\nPress the 'Collect Taxes' hotkey to collect a gold coin from your tax collector's purse\n'EA! It's NOT in the game, that's DLC!'");
		}

	}

	public class TheWholeExperience : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("'The Whole Experience'");
			Tooltip.SetDefault("While in your inventory, specific cutscenes and events will replay\nLuminite Wraith will be summoned in his pre-Moonlord stage\nKiller Fly Swarm will be summoned instead of Murk\nHellion will replay her monolog after Hellion Core");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
		public static bool Check()
        {
			foreach(Player player in Main.player)
            {
				if (player.HasItem(ModContent.ItemType<TheWholeExperience>()) || player.HasItem(ModContent.ItemType<TheWholeExperienceEX>()))
					return true;
            }
			return false;
		}
		public static bool CheckHigherTier(bool highertier = false)
		{
			foreach (Player player in Main.player)
			{
				if (player.HasItem(ModContent.ItemType<TheWholeExperienceEX>()))
					return true;
			}
			return false;
		}
		public override string Texture
		{
			get { return "Terraria/UI/Camera_7"; }
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 14;
			item.height = 14;
			item.value = 0;
			item.rare = ItemRarityID.Quest;
		}
	}

	public class TheWholeExperienceEX : TheWholeExperience
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("'The Whole Experience EX'");
			Tooltip.SetDefault("Same effects as 'The Whole Experience', but now prevents leaving subworlds on death\nSubworld bosses will reset when you die and you respawn in place");
			ItemID.Sets.ItemNoGravity[item.type] = true;
		}
        public override Color? GetAlpha(Color lightColor)
        {
			return Main.hslToRgb(Main.GlobalTime%1f,1f,0.75f);
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<TheWholeExperience>(), 1);
			recipe.AddIngredient(ModContent.ItemType<WatchersOfNull>(), 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}

		public class DungeonSplunker : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dungeon Splunker");
			Tooltip.SetDefault("While in your inventory, allows you to use pickaxes in the Deeper Dungeons");
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb((Main.GlobalTime * 0.916f) % 1f, 0.8f, 0.75f);
		}
		public override string Texture
		{
			get { return "Terraria/UI/Cursor_10"; }
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 14;
			item.height = 14;
			item.value = 0;
			item.rare = ItemRarityID.Quest;
		}
	}
	public class ShadowLockBox : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadow LockBox");
			Tooltip.SetDefault("Right click to open, must have a Shadow Key\n'Yes, this is literally just placeholder 1.4 content'");
		}
		public override void SetDefaults()
		{
			item.maxStack = 1;
			item.width = 14;
			item.height = 14;
			item.value = 0;
			item.rare = ItemRarityID.Quest;
		}
		public override bool CanRightClick()
		{
			return Main.LocalPlayer.HasItem(ItemID.ShadowKey);
		}

		public override void RightClick(Player player)
		{
			List<int> lootrare = new List<int> { ItemID.DarkLance, ItemID.Sunfury, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.HellwingBow };

			player.QuickSpawnItem(lootrare[Main.rand.Next(lootrare.Count)]);
		}


	}
	public class HellionCheckpoint1 : ModItem
	{
		protected virtual Color color => Color.Lime;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 1: Pedant of Doubt");
			Tooltip.SetDefault("'And there without, is not without doubt, of your failure...'\nPlace your inventory to allow Reality's Sunder summon Hellion at post Goblin Army\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 25% less items\nWill consume one when summoned"));
		}
        public override bool Autoload(ref string name)
        {
			return false;
        }
        public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 0;
			item.rare = -12;
			item.expert = true;
			item.maxStack = 30;
			//item.damage = 1;
		}
		public override string Texture => "Terraria/Item_"+ItemID.AlphabetStatue1;
		public override Color? GetAlpha(Color lightColor)
		{
			return Main.hslToRgb(Main.GlobalTime % 1f, 1f, 0.75f);
		}
		public override void GrabRange(Player player, ref int grabRange)
		{
			grabRange *= 32;
		}
		public override bool GrabStyle(Player player)
		{
			Vector2 vectorItemToPlayer = player.Center - item.Center;
			Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.25f;
			item.velocity = item.velocity + movement;
			item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
			return true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			Texture2D inner = Main.itemTexture[item.type];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width, inner.Height) / 2f;

			for (float f = 0f; f < 4; f += 0.25f)
			{
				spriteBatch.Draw(inner, drawPos+new Vector2(Main.rand.NextFloat(-f,f), Main.rand.NextFloat(-f, f)), null, (Color)GetAlpha(drawColor)*0.10f, 0, textureOrigin, Main.inventoryScale * 1, SpriteEffects.None, 0f);
			}

			spriteBatch.Draw(inner, drawPos+new Vector2((-0.50f+Main.GlobalTime%1)*16f* scale,0), null, color * 1f, 0, textureOrigin, Main.inventoryScale * 0.50f, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class HellionCheckpoint2 : HellionCheckpoint1
	{
		protected override Color color => Color.Purple;
		public override string Texture => "Terraria/Item_" + ItemID.AlphabetStatue2;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 2: Rotten Desires");
			Tooltip.SetDefault("'And there without, is not without doubt, of your failure...'\nPlace your inventory to allow Reality's Sunder summon Hellion at post Pirate Army\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 50% less items\nWill consume one when summoned"));
		}
	}

	public class HellionCheckpoint3 : HellionCheckpoint1
	{
		protected override Color color => Color.Red;
		public override string Texture => "Terraria/Item_" + ItemID.AlphabetStatue3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 3: Climax of Eternity");
			Tooltip.SetDefault("'And there without, is not without doubt, of your failure...'\nPlace your inventory to allow Reality's Sunder summon Hellion at post Festive Moons Army\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 75% less items\nWill consume one when summoned"));
		}
	}

	public class HellionCheckpoint4 : HellionCheckpoint1
	{
		protected override Color color => Color.Black;
		public override string Texture => "Terraria/Item_" + ItemID.AlphabetStatue4;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lession 4: Epilogue");
			Tooltip.SetDefault("'The End'\nPlace your inventory to allow Reality's Sunder summon Hellion with 1 HP\n" + Idglib.ColorText(Color.Red, "Hellion will not drop her crown, and will drop 90% less items\nWill consume one when summoned"));
		}
	}

	public class FinalGem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Final Gem");
			Tooltip.SetDefault("While in your inventory, empowers the Gucci Guantlet to its true full power\nFavorite to disable all the gems");
		}
		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 40;
			item.value = 0;
			item.rare = -12;
			item.expert = true;
			item.maxStack = 1;
			//item.damage = 1;
		}
        public override void UpdateInventory(Player player)
        {
			if (!item.favorited)
			player.SGAPly().finalGem = 3;
        }

        public override string Texture
		{
			get { return ("Terraria/Extra_57"); }
		}

        public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//texture mappedTexture;
			//float2 mappedTextureMultiplier;
			//float2 mappedTextureOffset;

			/*texture _VoronoiTex;
			float4 _CellColor = float4(1, .75, 0, 1); // Orange
			float4 _EdgeColor = float4(1, .5, 0, 1); // Yellow-Orange
			float2 _CellSize = float2(1.5, 2.0);
			float _ScrollSpeed = 0.04;
			float _FadeSpeed = 3;
			float _ColorScale = 1.5652475842498528; // .7*sqrt(5)
			float _Time; // Pass the time in seconds into here
			*/

			Texture2D tex = ModContent.GetTexture("SGAmod/voronoismol");

			SGAmod.VoronoiEffect.Parameters["_CellColor"].SetValue(Color.Black.ToVector4() * 1f);
			SGAmod.VoronoiEffect.Parameters["_EdgeColor"].SetValue(Color.Lerp(Color.Orange,Color.Yellow,0.50f).ToVector4() * 1f);
			SGAmod.VoronoiEffect.Parameters["_CellSize"].SetValue(new Vector2(1.5f,2f)*1f);
			SGAmod.VoronoiEffect.Parameters["_ScrollSpeed"].SetValue(Main.GlobalTime/40000f);
			SGAmod.VoronoiEffect.Parameters["_FadeSpeed"].SetValue(3f);
			SGAmod.VoronoiEffect.Parameters["_ColorScale"].SetValue(1.5652475842498528f);
			SGAmod.VoronoiEffect.Parameters["_Time"].SetValue(Main.GlobalTime*1f);

			SGAmod.VoronoiEffect.CurrentTechnique.Passes["Star"].Apply();

			spriteBatch.Draw(tex, item.Center - Main.screenPosition, null, Color.White, 0, tex.Size() / 2f, 1f, SpriteEffects.None, 0f);


			return false;
        }

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			float maxsize = 20;
			Texture2D[] gems = Main.gemTexture;
			Texture2D myTex = Main.itemTexture[item.type];
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor*0.25f, Main.GlobalTime / 1f, myTex.Size() / 2f, scale * 2.5f * Main.essScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor * 0.25f, -Main.GlobalTime / 1f, myTex.Size() / 2f, scale * 2.5f * Main.essScale, SpriteEffects.None, 0f);

			for (int i = 0; i < maxsize; i += 1)
			{
				Texture2D inner = gems[i % gems.Length];
				Double Azngle = i+(Main.GlobalTime/8f);
				Vector2 here = new Vector2((float)Math.Cos(Azngle), (float)Math.Sin(Azngle)) * (i * 2f);
				float scaler = (1f - (float)((float)i / maxsize));
				spriteBatch.Draw(inner, position + (new Vector2(14f, 14f)) + here, null, Color.Lerp(drawColor, Color.MediumPurple, 0.25f) * scaler, Main.GlobalTime *= (i % 2 == 0 ? -1f : 1f), new Vector2(inner.Width / 2, inner.Height / 2), scale * scaler, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor, Main.GlobalTime/1f, myTex.Size()/2f, scale * 1.5f * Main.essScale, SpriteEffects.None, 0f);
			spriteBatch.Draw(myTex, position + new Vector2(14f, 14f), frame, drawColor, -Main.GlobalTime/1f, myTex.Size() / 2f, scale * 1.5f * Main.essScale, SpriteEffects.None, 0f);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
			return false;
		}
	}


}

