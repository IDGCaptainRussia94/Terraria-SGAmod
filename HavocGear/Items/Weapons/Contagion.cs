using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
    public class Contagion : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Contagion");
            Tooltip.SetDefault("Shoots a piercing wave of acid");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.damage = 40;
            item.melee = true;
            item.noMelee = true;
            item.useTurn = true;
            item.noUseGraphic = true;
            item.useAnimation = 20;
            item.useStyle = 5;
            item.useTime = 20;
            item.knockBack = 3f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.maxStack = 1;
            item.value = 100000;
            item.rare = 6;
            item.shoot = mod.ProjectileType("ContagionProj");
            item.shootSpeed = 11f;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "Mossthorn", 1);
            recipe.AddIngredient(null, "TidalWave", 1);
            recipe.AddIngredient(null, "VialofAcid", 12);
            recipe.AddIngredient(mod.ItemType("VirulentBar"), 10);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}