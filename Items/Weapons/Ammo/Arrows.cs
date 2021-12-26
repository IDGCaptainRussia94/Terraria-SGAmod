using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.Ammo
{
	public class UnmanedArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novus Arrow");
			Tooltip.SetDefault("Arrows slightly home in on nearby enemies");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/UnmanedArrow"); }
		}
		public override void SetDefaults()
		{
			item.damage = 9;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 1.5f;
			item.value = 10;
			item.rare = 1;
			item.shoot = mod.ProjectileType("UnmanedArrow");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 2.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 50);
			recipe.AddIngredient(mod.ItemType("CopperWraithNotch"), 1);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}
	public class UnmanedArrow2 : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Notchvos Arrow");
			Tooltip.SetDefault("Improved arrows that better home in on nearby enemies\nArrows travel in the reverse direction after hitting an enemy\nCan hit a total of 2 times, ignores invulnerability frames");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/UnmanedArrow2"); }
		}
		public override void SetDefaults()
		{
			item.damage = 12;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 2f;
			item.value = 25;
			item.rare = 3;
			item.shoot = mod.ProjectileType("UnmanedArrow2");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 3.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedArrow"), 50);
			recipe.AddIngredient(mod.ItemType("CobaltWraithNotch"), 2);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}

	public class PitchArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pitched Arrow");
			Tooltip.SetDefault("Inflicts Oiled on enemies\nHas a small chance to affect immune enemies for half the debuff time\nOiled enemies will be ignited for an extended time from Thermal Blaze and take even more damage");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/PitchArrow"); }
		}
		public override void SetDefaults()
		{
			item.damage = 10;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 2f;
			item.value = 25;
			item.rare = 3;
			item.shoot = mod.ProjectileType("PitchArrow");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 3.5f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.WoodenArrow, 50);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 1);
			recipe.AddIngredient(mod.ItemType("MurkyGel"), 3);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 50);
			recipe.AddRecipe();
		}
	}

	public class DosedArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doused Arrow");
			Tooltip.SetDefault("Has a chance to inflict Doused on enemies\nUse in combo with burning weapons to ignite foes\nExplodes against burning targets, otherwise it will bounce off them once\nSometimes inflict Oiled (bypassing immunity) and homes in on enemies");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/DosedArrow"); }
		}
		public override void SetDefaults()
		{
			item.damage = 20;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 4f;
			item.value = 500;
			item.rare = 7;
			item.shoot = mod.ProjectileType("DosedArrow");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 12f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PitchArrow"), 100);
			recipe.AddIngredient(mod.ItemType("UnmanedArrow2"), 100);
			recipe.AddIngredient(mod.ItemType("GasPasser"), 1);
			recipe.AddIngredient(mod.ItemType("LuminiteWraithNotch"), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 200);
			recipe.AddRecipe();
		}
	}

	public class WraithArrow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wrath Arrow");
			Tooltip.SetDefault("Inflicts Betsy's Curse, and explodes like Aerial Bane Arrows on hit (no extra arrows are spawned however)\n'Because of course we need this too :-p'");
		}
		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/WraithArrow"); }
		}
		public override void SetDefaults()
		{
			item.damage = 15;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 4f;
			item.value = 500;
			item.rare = ItemRarityID.Lime;
			item.shoot = mod.ProjectileType("WraithArrow");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 18f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ShadowJavelinRecipe(mod);
			recipe.AddIngredient(ItemID.HellfireArrow, 250);
			recipe.AddIngredient(ItemID.WrathPotion, 1);
			recipe.AddIngredient(ItemID.DefenderMedal, 5);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 2);
			recipe.AddIngredient(mod.ItemType("OmegaSigil"), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this, 250);
			recipe.AddRecipe();
		}
	}
	public class DankArrow : ModItem,IDankSlowText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Arrow");
			Tooltip.SetDefault("");
		}
		public override void SetDefaults()
		{
			item.damage = 5;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.knockBack = 0f;
			item.value = 50;
			item.rare = 1;
			item.shoot = mod.ProjectileType("DankArrow");   //The projectile shoot when your weapon using this ammo
			item.shootSpeed = 5f;                  //The speed of the projectile
			item.ammo = AmmoID.Arrow;              //The ammo class this ammo belongs to.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("DankWood"), 25);
			recipe.AddIngredient(mod.ItemType("DankCore"), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 250);
			recipe.AddRecipe();
		}
	}
	public class PrismicArrow : PrismicBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lesser Prismic Arrow");
			Tooltip.SetDefault("Shots cycle through your 2nd and 3rd ammo slots while placed in your first\nDefaults to a weak wooden arrow\nHas a 66% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.WoodenArrow); }
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodenArrow);
			item.damage = 2;
			item.ranged = true;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.value = 100;
			item.rare = 5;
			item.shoot = ProjectileID.WoodenArrowFriendly;   //The projectile shoot when your weapon using this ammo
			item.ammo = AmmoID.Arrow;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:Tier1Bars", 1);
			recipe.AddRecipeGroup("SGAmod:Tier2Bars", 1);
			recipe.AddRecipeGroup("SGAmod:Tier3Bars", 1);
			recipe.AddRecipeGroup("SGAmod:Tier4Bars", 1);
			recipe.AddIngredient(mod.ItemType("WraithFragment3"), 2);
			recipe.AddIngredient(ItemID.WoodenArrow, 150);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}

	public class PrismalArrow : PrismalBullet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Arrow");
			Tooltip.SetDefault("Highly increased damage over its precursor\nCycles through your ammo slots when placed in your first; defaults to Wooden Arrows\nHas a 75% to not consume the fired ammo type");
		}
		public override string Texture
		{
			get { return ("Terraria/Item_" + ItemID.WoodenArrow); }
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.WoodenArrow);
			item.damage = 18;
			item.ranged = true;
			item.maxStack = 999;
			item.consumable = true;             //You need to set the item consumable so that the ammo would automatically consumed
			item.value = 1000;
			item.rare = 10;
			item.shoot = ProjectileID.WoodenArrowFriendly;   //The projectile shoot when your weapon using this ammo
			item.ammo = AmmoID.Arrow;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 1);
			recipe.AddIngredient(mod.ItemType("PrismicArrow"), 150);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 150);
			recipe.AddRecipe();
		}
	}
}
