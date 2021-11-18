using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
	public class SwampSeeds : ModItem
	{
		public override void SetDefaults()
		{
			item.autoReuse = true;

			item.useTurn = true;
			item.useStyle = 1;
			item.useAnimation = 15;
			item.useTime = 10;
			item.maxStack = 99;
			item.consumable = true;
			item.placeStyle = 0;
			item.width = 12;
			item.height = 14;
			item.value = 10;
			item.createTile = mod.TileType("SwampGrassGrow");
		}

    public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Dank Seeds");
      Tooltip.SetDefault("'Harvested from Dank Grass, grows on Moist Stone'");
    }


        public override bool UseItem(Player player)
        {
            if (Main.tile[Player.tileTargetX, Player.tileTargetY].type == ModContent.TileType<Tiles.MoistStone>())
            {
                Main.tile[Player.tileTargetX, Player.tileTargetY].type = (ushort)mod.TileType("SwampGrass");
            }
            else
            {
                item.stack++;
            }
            return true;
        }
	}
}
