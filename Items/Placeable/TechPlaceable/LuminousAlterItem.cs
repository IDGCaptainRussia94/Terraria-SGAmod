using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;
using System;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumables;
using Terraria.DataStructures;

namespace SGAmod.Items.Placeable.TechPlaceable
{

	public class LuminousAlterCraftingHint : ModItem
	{
		/*public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (" + Name2[0] + ")");
			Tooltip.SetDefault(Idglib.ColorText(Color.PaleTurquoise, "'" + Name2[1] + "'") + Idglib.ColorText(Color.PaleGoldenrod, " : Composed by " + Name2[2]));
		}*/

		/// </summary>
		/// 

		static Func<bool> BloodSunCondition = delegate ()
		{
			return ((!Main.dayTime && Main.bloodMoon) || Main.eclipse) && !Main.raining;
		};
		static Func<bool> BloodmoonCondition = delegate ()
		{
			return !Main.dayTime && !Main.raining && Main.bloodMoon;
		};

		static Func<bool> BlackSunCondition = delegate ()
		{
			return !Main.raining && Main.eclipse;
		};

		public static void InitLuminousCrafting()
		{

			AddLuminousAlterRecipe(ModContent.ItemType<AuroraTear>(),ModContent.ItemType<AuroraTearAwoken>(), 60 * 300, 1);
			AddLuminousAlterRecipe(ItemID.FallenStar,ModContent.ItemType<PrismaticBansheeStar>(), 60 * 120, 20,1);
			AddLuminousAlterRecipe(ItemID.PinkGel, ModContent.ItemType<Prettygel>(), 60 * 45, 20, 1);
			AddLuminousAlterRecipe(ItemID.SoulofLight, ModContent.ItemType<IlluminantEssence>(), 60 * 8, 1);
			AddLuminousAlterRecipe(ItemID.Mushroom, ItemID.GlowingMushroom, 60 * 12, 1, 10);
			AddLuminousAlterRecipe(ItemID.Gel, ItemID.PinkGel, 60 * 30, 1, 1);
			AddLuminousAlterRecipe(ModContent.ItemType<OmniSoul>(), ItemID.FragmentSolar, 60 * 10, 1, 1);
			AddLuminousAlterRecipe(ItemID.Meteorite, ItemID.LunarOre, 60 * 10, 4, 4);
			AddLuminousAlterRecipe(ItemID.StoneBlock, ModContent.ItemType<Glowrock>(), 60 * 2, 1, 1);
			AddLuminousAlterRecipe(ItemID.CrystalShard, ModContent.ItemType<OverseenCrystal>(), 60 * 20, 5, 1);

			AddLuminousAlterRecipe(ModContent.ItemType<Weapons.SwordofTheBlueMoon>(), ModContent.ItemType<Weapons.TrueMoonlight>(), 60 * 600, 1, 1);
			AddLuminousAlterRecipe(ItemID.RottenEgg, ModContent.ItemType<Weapons.RottenEggshels>(), 60 * 10, 10, 50);

			string requiredText = "Eclipse/Blood Moon Required";

			AddLuminousAlterRecipe(ModContent.ItemType<AncientFabricItem>(), ModContent.ItemType<StygianCore>(), 60 * 60, 50, 1, BloodSunCondition, requiredText);
			AddLuminousAlterRecipe(ModContent.ItemType<EntropyTransmuter>(), ModContent.ItemType<CalamityRune>(), 60 * 10, 1, 1, BloodSunCondition, requiredText);
			AddLuminousAlterRecipe(ItemID.ShinyStone, ModContent.ItemType<CalamityRune>(), 60 * 60, 1, 1, BloodSunCondition, requiredText);


			requiredText = "Blood Moon Required";

			AddLuminousAlterRecipe(ItemID.DemoniteBar, ItemID.CrimtaneBar, 60 * 5, 1, 1, BloodmoonCondition, requiredText);
			AddLuminousAlterRecipe(ItemID.ShadowScale, ItemID.TissueSample, 60 * 6, 1, 1, BloodmoonCondition, requiredText);
			AddLuminousAlterRecipe(ItemID.CursedFlame, ItemID.Ichor, 60 * 8, 1, 1, BloodmoonCondition, requiredText);
			AddLuminousAlterRecipe(ItemID.CorruptSeeds, ItemID.CrimsonSeeds, 60 * 5, 1, 1, BloodmoonCondition, requiredText);


			requiredText = "Solar Eclipse Required";

			AddLuminousAlterRecipe(ItemID.CrimtaneBar, ItemID.DemoniteBar, 60 * 5, 1, 1, BlackSunCondition,requiredText);
			AddLuminousAlterRecipe(ItemID.TissueSample, ItemID.ShadowScale, 60 * 6, 1, 1, BlackSunCondition, requiredText);
			AddLuminousAlterRecipe(ItemID.Ichor, ItemID.CursedFlame, 60 * 8, 1, 1, BlackSunCondition, requiredText);
			AddLuminousAlterRecipe(ItemID.CrimsonSeeds, ItemID.CorruptSeeds, 60 * 5, 1, 1, BlackSunCondition, requiredText);

		}

		public static void CreateRecipeItems()
		{
			SGAmod.Instance.AddItem("AlterCraft_Moon", new LuminousAlterCraftingHint("Moon", "Requires Clear Moonlight", "Terraria/Moon_0"));
			SGAmod.Instance.AddItem("AlterCraft_BloodMoon", new LuminousAlterCraftingHint("Blood Moon", "Requires Blood Moon", "Terraria/Moon_0", Color.Red));
			SGAmod.Instance.AddItem("AlterCraft_Eclipse", new LuminousAlterCraftingHint("Eclipse", "Requires Eclipse", "Terraria/Sun3"));
			SGAmod.Instance.AddItem("AlterCraft_BloodSun", new LuminousAlterCraftingHint("Blood Sun", "Requires Blood Moon or Eclipse", "Terraria/Sun", Color.Red));
			SGAmod.Instance.AddItem("AlterCraft_Time", new LuminousAlterCraftingHint("Time", "Seconds to infuse", "Terraria/Item_" + ItemID.Timer1Second));

			Idglib.AbsentItemDisc.Add(SGAmod.Instance.ItemType("AlterCraft_Moon"), "No, you don't need the real moon ya dummy, this is just a guide");
			Idglib.AbsentItemDisc.Add(SGAmod.Instance.ItemType("AlterCraft_BloodMoon"), "No, you don't need the real blood moon ya dummy, this is just a guide");
			Idglib.AbsentItemDisc.Add(SGAmod.Instance.ItemType("AlterCraft_Eclipse"), "This Eclipse item isn't obtainable, this is just a guide");
			Idglib.AbsentItemDisc.Add(SGAmod.Instance.ItemType("AlterCraft_BloodSun"), "No, you don't need the a... well this techically doesn't exist, but anyways this is just a guide");
			Idglib.AbsentItemDisc.Add(SGAmod.Instance.ItemType("AlterCraft_Time"), "This isn't a recipe you craft, read it more carefully and use the tile with a right click");

		}

		public static void AddLuminousAlterRecipe(int catalyst, int outputItem, int time, int numIn = 1, int numOut = 1, Func<bool> Cond = default, string requiredText = "")
		{
			SGAmod.LuminousAlterItems.Add(catalyst, new LuminousAlterItemClass(outputItem, time, numIn, numOut, Cond, requiredText));

			if (SGAConfig.Instance.LuminousCraftingRecipes)
			{

				ModRecipe modRecipe = new ModRecipe(SGAmod.Instance);

				//modRecipe.AddIngredient(ModContent.ItemType<LuminousAlterItem>());
				modRecipe.AddIngredient(catalyst, numIn);

				if (Cond == BloodSunCondition)
					modRecipe.AddIngredient(SGAmod.Instance.ItemType("AlterCraft_BloodSun"));
				else if (Cond == BlackSunCondition)
					modRecipe.AddIngredient(SGAmod.Instance.ItemType("AlterCraft_Eclipse"));
				else if (Cond == BloodmoonCondition)
					modRecipe.AddIngredient(SGAmod.Instance.ItemType("AlterCraft_BloodMoon"));
				else
					modRecipe.AddIngredient(SGAmod.Instance.ItemType("AlterCraft_Moon"));

				modRecipe.AddIngredient(SGAmod.Instance.ItemType("AlterCraft_Time"), (int)Math.Ceiling(time / 60f));
				modRecipe.AddTile(ModContent.TileType<Tiles.TechTiles.LuminousAlter>());
				modRecipe.SetResult(outputItem, numOut);
				modRecipe.AddRecipe();

			}
		}

		public override bool CloneNewInstances => true;

		string[] name2;
		string texture;
		Color color;

		public LuminousAlterCraftingHint(string Name, string Disc, string texture, Color color = default)
		{
			name2 = new string[2] { Name, Disc };
			this.texture = texture;
			this.color = color == default ? Color.White : color;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(name2[0]);
			Tooltip.SetDefault(name2[1]);
			ItemID.Sets.ItemNoGravity[item.type] = true;
			if (texture == "Terraria/Moon_0")
			{
				Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(6, 8));
			}
		}
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 32;
			item.value = 0;
			item.maxStack = 1;
			item.rare = ItemRarityID.White;
		}
		public override string Texture
		{
			get { return texture; }
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return color;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					//Color rainbowColor = Main.hslToRgb((Main.GlobalTime * 0.085f) % 1f, 1f, 0.75f);
					line.overrideColor = LuminousAlterItem.AuroraLineColor;
				}
			}
		}

        public override bool Autoload(ref string name)
		{
			return false;
		}
	}
	public class LuminousAlterItem : ModItem, IAuroraItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminous Alter");
			Tooltip.SetDefault("When exposed to a clear night sky: transmutes items with Starlight\nConverts items into other items over a period of time\nCan corrupt specific items instead when the moon turns red or black");
		}
		public override void SetDefaults()
		{
			item.maxStack = 999;
			item.width = 26;
			item.height = 14;
			item.value = Item.sellPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.Cyan;
			item.alpha = 0;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.TechTiles.LuminousAlter>();
		}
		public static Color AuroraLineColor
        {
            get
            {
				Color rainbowColor = Main.hslToRgb((Main.GlobalTime * 0.085f) % 1f, 1f, 0.75f);
				return Color.Lerp(rainbowColor, Color.Lerp(Color.Pink, Color.Aqua, 0.5f + (float)Math.Sin(Main.GlobalTime * 0.735f)), 0.5f + (float)Math.Sin(Main.GlobalTime * 0.375f));
			}
        }
		/*public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					Color rainbowColor = Main.hslToRgb((Main.GlobalTime * 0.085f) % 1f, 1f, 0.75f);
					line.overrideColor = AuroraLineColor;
				}
			}
		}*/
		public override void AddRecipes()
		{		
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 12);
			recipe.AddIngredient(mod.ItemType("AuroraTear"), 1);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 12);
			recipe.AddTile(TileID.LihzahrdAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}