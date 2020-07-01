using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Tools
{
	public class MangrovePickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Pickaxe");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Faster jungle Item", "15% faster use speed in the jungle biome"));
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.ZoneJungle)
			return 1.15f;
			return 1f;
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (item.useStyle == 1)
			{
				if (Main.rand.Next(5) == 0)
				{
					SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
					int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, DustID.GrassBlades, 0,0,100, Color.Yellow, 1f);

				}
			}
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MoltenPickaxe);
			item.damage = 8;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 18;
			item.useAnimation = 20;
			item.pick = 56;
			item.useStyle = 1;
			item.knockBack = 3;
			item.value = 3000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddIngredient(null, "DankWood", 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class MangroveAxe : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
       			DisplayName.SetDefault("Mangrove Axe");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MoltenHamaxe);
			item.damage = 12;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 20;
			item.axe = 12;
			item.hammer = 0;
			item.useStyle = 1;
			item.knockBack = 5;
			item.value = 3000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

        	public override void AddRecipes()
        	{
            		ModRecipe recipe = new ModRecipe(mod);
            		recipe.AddIngredient(null, "BiomassBar", 7);
			recipe.AddIngredient(null, "DankWood", 15);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
            		recipe.AddRecipe();
        	}
	}

	public class MangroveHammer : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Hammer");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MoltenHamaxe);
			item.damage = 14;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 20;
			item.hammer = 59;
			item.axe = 0;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 3000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBar", 7);
			recipe.AddIngredient(null, "DankWoodHammer", 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

}