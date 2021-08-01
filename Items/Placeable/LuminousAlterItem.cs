using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ID;
using Idglibrary;
using Terraria;
using System;
using SGAmod.Items.Accessories;
using SGAmod.Items.Consumable;

namespace SGAmod.Items.Placeable
{

	public class LuminousAlterCraftingHint : ModItem
	{
		/*public override void SetStaticDefaults() {
			DisplayName.SetDefault("Music Box (" + Name2[0] + ")");
			Tooltip.SetDefault(Idglib.ColorText(Color.PaleTurquoise, "'" + Name2[1] + "'") + Idglib.ColorText(Color.PaleGoldenrod, " : Composed by " + Name2[2]));
		}*/

		/// </summary>
		/// 
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
			AddLuminousAlterRecipe(ItemID.RottenEgg, ModContent.ItemType<Weapons.RottenEggshels>(), 60 * 10, 10, 50);

			Func<bool> BloodSunCondition = delegate ()
			{
				return ((!Main.dayTime && Main.bloodMoon) || Main.eclipse) && !Main.raining;
			};

			AddLuminousAlterRecipe(ModContent.ItemType<AncientFabricItem>(), ModContent.ItemType<StygianCore>(), 60 * 60, 50, 1, BloodSunCondition);
			AddLuminousAlterRecipe(ModContent.ItemType<EntropyTransmuter>(), ModContent.ItemType<CalamityRune>(), 60 * 10, 1, 1, BloodSunCondition);
			AddLuminousAlterRecipe(ItemID.ShinyStone, ModContent.ItemType<CalamityRune>(), 60 * 60, 1, 1, BloodSunCondition);

			Func<bool> BloodmoonCondition = delegate ()
			{
				return !Main.dayTime && !Main.raining && Main.bloodMoon;
			};

			AddLuminousAlterRecipe(ItemID.DemoniteBar, ItemID.CrimtaneBar, 60 * 5, 1, 1, BloodmoonCondition);
			AddLuminousAlterRecipe(ItemID.ShadowScale, ItemID.TissueSample, 60 * 6, 1, 1, BloodmoonCondition);
			AddLuminousAlterRecipe(ItemID.CursedFlame, ItemID.Ichor, 60 * 8, 1, 1, BloodmoonCondition);
			AddLuminousAlterRecipe(ItemID.CorruptSeeds, ItemID.CrimsonSeeds, 60 * 5, 1, 1, BloodmoonCondition);

			Func<bool> BlackSunCondition = delegate ()
			{
				return !Main.raining && Main.eclipse;
			};

			AddLuminousAlterRecipe(ItemID.CrimtaneBar, ItemID.DemoniteBar, 60 * 5, 1, 1, BlackSunCondition);
			AddLuminousAlterRecipe(ItemID.TissueSample, ItemID.ShadowScale, 60 * 6, 1, 1, BlackSunCondition);
			AddLuminousAlterRecipe(ItemID.Ichor, ItemID.CursedFlame, 60 * 8, 1, 1, BlackSunCondition);
			AddLuminousAlterRecipe(ItemID.CrimsonSeeds, ItemID.CorruptSeeds, 60 * 5, 1, 1, BlackSunCondition);

		}

		public static void AddLuminousAlterRecipe(int catalyst,int outputItem,int time,int numIn = 1,int numOut = 1,Func<bool> Cond = default)
        {
			SGAmod.LuminousAlterItems.Add(catalyst, new LuminousAlterItemClass(outputItem, time, numIn, numOut, Cond));
		}

		public override bool CloneNewInstances => true;

		private string itemName;
		private string texture;

		public LuminousAlterCraftingHint(string itemName, string texture)
		{
			this.itemName = itemName;
			this.texture = texture;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			foreach (TooltipLine line in tooltips)
			{
				if (line.mod == "Terraria" && line.Name == "ItemName")
				{
					//Color rainbowColor = Main.hslToRgb((Main.GlobalTime * 0.085f) % 1f, 1f, 0.75f);
					line.overrideColor = LuminousAlter.AuroraLineColor;
				}
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault(itemName);
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
		}
        public override string Texture => texture;
        public override bool Autoload(ref string name)
		{
			return false;
		}
	}
	public class LuminousAlter : ModItem, IAuroraItem
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
			item.createTile = mod.TileType("LuminousAlter");
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
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}