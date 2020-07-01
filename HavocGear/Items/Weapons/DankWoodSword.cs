using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class DankWoodSword : ModItem
	{
        public override void SetDefaults()
		{
			base.SetDefaults();

            item.damage = 10;
            item.width = 32;
			item.height = 32;
            item.melee = true;
            item.useTurn = false;
            item.rare = 1;
            item.useStyle = 1;
            item.useAnimation = 20;
           	item.knockBack = 3;
            item.useTime = 20;
            item.consumable = false;
            item.UseSound = SoundID.Item1;
        }

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Wood Sword");
      Tooltip.SetDefault("Crits against foes slowed by Dank Slow");
    }

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if (target.GetGlobalNPC<SGAnpcs>().DankSlow)
                crit = true;
        }


        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DankWood", 25);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
}   
