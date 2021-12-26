using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{
	public class Braxsaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Braxsaw");
			Tooltip.SetDefault("Mine through the oldest fabric of the universe!");
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.melee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 1;
			item.useAnimation = 18;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.pick = 250;
			item.axe = 150;
			item.tileBoost += 5;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 3000000;
			item.rare = 11;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("BraxsawProj");
			item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 32);
			recipe.AddIngredient(ModContent.ItemType<LunarRoyalGel>(), 10);
			recipe.AddIngredient(ItemID.Drax, 1);
			recipe.AddIngredient(ModContent.ItemType <BoreicDrill>(), 1);
			recipe.AddIngredient(ModContent.ItemType <HavocGear.Items.Tools.VirulentDrill>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class BraxsawProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BraxsawProj"); }
		}

		public override void SetDefaults()
		{
			int[] itta = { ProjectileID.SolarFlareDrill, ProjectileID.NebulaDrill , ProjectileID.StardustDrill , ProjectileID.VortexDrill };
			projectile.CloneDefaults(itta[Main.rand.Next(0, itta.Length)]);
		}

	}

}