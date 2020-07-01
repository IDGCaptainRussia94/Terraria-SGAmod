using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using AAAAUThrowing;

namespace SGAmod.HavocGear.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]

    public class DankWoodHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Helmet");
            Tooltip.SetDefault("3% increased critical strike chance");
        }

        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 20;
            item.value = 10000;
            item.rare = 1;
            item.defense = 2;
        }

        public override void UpdateEquip(Player player)
        {
            player.magicCrit += 3;
            player.rangedCrit += 3;
            player.meleeCrit += 3;
            player.Throwing().thrownCrit += 3;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DankWood", 50);
            recipe.AddIngredient(null, "DankCore", 1);
            recipe.AddTile(TileID.WorkBenches);

            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
    [AutoloadEquip(EquipType.Body)]
    public class DankWoodChest : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Chestplate");
            Tooltip.SetDefault("7.5% increased item use rate");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.value = 10000;
            item.rare = 1;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.UseTimeMul += 0.075f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DankWood", 30);
            recipe.AddIngredient(null, "DankCore", 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }

    [AutoloadEquip(EquipType.Legs)]
    public class DankLegs : ModItem
	{
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Leggings");
            Tooltip.SetDefault("Improved movement speed");
        }

        public override void SetDefaults()
		{
			item.width = 18;
            item.height = 12;
			item.value = 10000;
			item.rare = 1;
			item.defense = 1;
		}

		public override void UpdateEquip(Player player)
		{
            player.maxRunSpeed += 0.2f;
            player.accRunSpeed += 0.2f;
            player.runAcceleration += 0.05f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DankWood", 25);
            recipe.AddIngredient(null, "DankCore", 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}