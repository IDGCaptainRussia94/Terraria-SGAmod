using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ClayMore : ModItem
	{
        public override void SetDefaults()
		{
			base.SetDefaults();

            item.damage = 24;
            item.width = 19;
			item.height = 22;
            item.melee = true;
            item.rare = 6;
            item.useStyle = 1;
            item.useAnimation = 25;
            item.autoReuse = true;
            item.useTime = 26;
            item.useTurn = true;
            item.knockBack = 9;
            item.value = 1000;
            item.consumable = false;
            item.UseSound = SoundID.Item1;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Clay-More");
    }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.ClayBlock, 50);
            recipe.AddIngredient(ItemID.Gel, 15);
            recipe.AddIngredient(ItemID.Vine, 2);
	    recipe.AddTile(TileID.Anvils);
	    recipe.SetResult(this);
            recipe.AddRecipe();
        }

	}
}
