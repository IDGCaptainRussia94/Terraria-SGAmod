using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Accessories
{
	public class Havoc : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Havoc's Fragmented Remains");
            Tooltip.SetDefault("'The remains of PeopleMCNugget's dreams of Havoc mod'\n'now at your will and prehaps, can be put back together'\n25% more damage with Havoc weapons\n3 defense per Havoc accessory equipped (8 in hardmode)\nEffects of Photosynthesizer and Serrated Tooth");
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            Color c = Main.hslToRgb((float)(Main.GlobalTime/4f)%1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(mod,"Dedicated", Idglib.ColorText(c,"Dedicated to PeopleMCNugget")));
			c = Main.hslToRgb((float)(Main.GlobalTime / 3.17f) % 1f, 0.65f, 0.45f);
			tooltips.Add(new TooltipLine(mod, "Dedicated2", Idglib.ColorText(c, "And the amazing spriters who helped him")));
		}

		public override string Texture
		{
			get { return("SGAmod/Items/Weapons/CrateBossWeaponThrown");}
		}

        public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = Item.sellPrice(1, 0, 0, 0);
			item.rare = 9;
			item.accessory = true;
			item.expert=true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
		player.GetModPlayer<SGAPlayer>().Havoc = 1;
		mod.GetItem("Photosynthesizer").UpdateAccessory(player,hideVisual);
		mod.GetItem("SerratedTooth").UpdateAccessory(player,hideVisual);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("SharkTooth"), 100);
			recipe.AddIngredient(mod.ItemType("MurkyGel"), 100);
			recipe.AddIngredient(mod.ItemType("Photosynthesizer"), 1);
			recipe.AddIngredient(mod.ItemType("SerratedTooth"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 15);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}