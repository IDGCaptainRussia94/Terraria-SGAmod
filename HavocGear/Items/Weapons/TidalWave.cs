using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons 
{
    public class TidalWave : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Wave");
            Tooltip.SetDefault("Shoots a short range water ball");
        }

        public override void SetDefaults()
        {
            item.width = 10;
            item.height = 10;
            item.damage = 12;
            item.melee = true;
            item.noMelee = true;
            item.useTurn = true;
            item.noUseGraphic = true;
            item.useAnimation = 30;
            item.useStyle = 5;
            item.useTime = 30;
            item.knockBack = 7f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = false;
            item.maxStack = 1;
            item.value = 10000;
            item.rare = 1;
            item.shoot = mod.ProjectileType("TidalWaveProj");
            item.shootSpeed = 9f;
        }
    }
}