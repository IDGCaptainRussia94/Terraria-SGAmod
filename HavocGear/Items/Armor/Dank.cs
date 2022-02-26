using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]

    public class DankWoodHelm : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Wood Helmet");
            Tooltip.SetDefault("4% increased critical strike chance\n15% DoT resistance");
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
            player.BoostAllDamage(0, 4);
            player.SGAPly().DoTResist *= 0.85f;
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
            Tooltip.SetDefault("8% increased item use rate, improved life regen\n25% DoT resistance");
        }

        public override void SetDefaults()
        {
            item.width = 28;
            item.height = 22;
            item.value = 10000;
            item.rare = 1;
            item.lifeRegen = 1;
            item.defense = 3;
        }

        public override void UpdateEquip(Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.UseTimeMul += 0.08f;
            sgaplayer.DoTResist *= 0.75f;
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
            Tooltip.SetDefault("20% Improved movement speed and faster acceleration\n10% DoT resistance");
        }

        public override void SetDefaults()
		{
			item.width = 18;
            item.height = 12;
			item.value = 10000;
			item.rare = 1;
			item.defense = 2;
		}

		public override void UpdateEquip(Player player)
		{
            player.moveSpeed += 0.2f;
            player.accRunSpeed += 0.05f;
            player.SGAPly().DoTResist *= 0.90f;
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