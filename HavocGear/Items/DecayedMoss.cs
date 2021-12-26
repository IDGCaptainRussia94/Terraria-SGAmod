using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items
{
    public class DecayedMoss : ModItem
    {
        public override void SetDefaults()
        {

            item.width = 22;
            item.height = 22;
            item.maxStack = 99;

            item.rare = 1;
            item.useAnimation = 15;
            item.useTime = 10;
            item.useStyle = 1;
            item.UseSound = SoundID.Item81;
            item.consumable = true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Decayed Moss");
            Tooltip.SetDefault("Can be used to fertilize saplings");
        }


        public override bool CanUseItem(Player player)
        {
            return TileLoader.IsSapling(Main.tile[Player.tileTargetX, Player.tileTargetY].type);
        }

        public override bool UseItem(Player player)
        {
            if (WorldGen.GrowTree(Player.tileTargetX, Player.tileTargetY))
            {
                WorldGen.TreeGrowFXCheck(Player.tileTargetX, Player.tileTargetY);
            }
            else
            {
                item.stack++;
            }
            return true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<Biomass>(), 2);
            recipe.AddIngredient(ModContent.ItemType<MoistSand>(), 4);
            recipe.AddIngredient(ModContent.ItemType<Weapons.SwampSeeds>(), 6);
            recipe.AddIngredient(ModContent.ItemType<DankCore>(), 1);
            recipe.AddTile(TileID.AlchemyTable);
            recipe.SetResult(this, 12);
            recipe.AddRecipe();
        }

    }
}