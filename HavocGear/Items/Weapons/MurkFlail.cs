using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class MurkFlail : ModItem
    {
        public override void SetDefaults()
        {

            item.width = 30;
            item.height = 10;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 3;
            item.noMelee = true; 
            item.useStyle = 5;
            item.useAnimation = 20; 
            item.useTime = 44;
            item.knockBack = 8f;
            item.damage = 35;
            item.scale = 2f;
            item.noUseGraphic = true; 
            item.shoot = mod.ProjectileType("MurkFlailBall");
            item.shootSpeed = 15.1f;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.channel = true;
        }

    public override void SetStaticDefaults()
    {
            DisplayName.SetDefault("Mudrock Crasher");
            Tooltip.SetDefault("Impacts against walls at high enough speeds unleash several dank blasts which inflict Dank Slow\nEnemies who are immune to poison are also immune to Dank Slow");
        }

    }

}
