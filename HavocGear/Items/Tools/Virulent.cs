using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Tools
{
	public class VirulentPickaxe : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
       			DisplayName.SetDefault("Virulent Pickaxe");
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Faster jungle Item", "25% faster use speed in the jungle biome"));
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.ZoneJungle)
				return 1.25f;
			return 1f;
		}

		public override void SetDefaults()
		{
			item.damage = 15;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 25;
			item.useAnimation = 20;
			item.pick = 150;
			item.useStyle = 1;
			item.knockBack = 4;
			item.value = 3000;
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        	public override void AddRecipes()
        	{
            		ModRecipe recipe = new ModRecipe(mod);
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MangrovePickaxe", 1);
			recipe.AddIngredient(null, "VirulentBar", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class VirulentDrill : VirulentPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Drill");
		}

		public override void SetDefaults()
		{
			item.damage = 26;
			item.melee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 7;
			item.useAnimation = 25;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.pick = 150;
			item.tileBoost++;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 3000;
			item.rare = 4;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("VirulentDrill");
			item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VirulentBar", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class VirulentHamaxe : VirulentPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Hamaxe");
		}

		public override void SetDefaults()
		{
			item.damage = 42;
			item.melee = true;
			item.width = 48;
			item.height = 48;
			item.useTime = 30;
			item.useAnimation = 20;
			item.axe = 17;
			item.hammer = 85;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 3000;
			item.rare = 4;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "MangroveHammer", 1);
			recipe.AddIngredient(null, "MangroveAxe", 1);
			recipe.AddIngredient(null, "VirulentBar", 5);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
	public class VirulentJacksaw : VirulentPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Virulent Jacksaw");
		}

		public override void SetDefaults()
		{
			item.damage = 26;
			item.melee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 7;
			item.useAnimation = 25;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.axe = 17;
			item.hammer = 85;
			item.tileBoost++;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 3000;
			item.rare = 4;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("VirulentJacksaw");
			item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "VirulentBar", 15);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}


}