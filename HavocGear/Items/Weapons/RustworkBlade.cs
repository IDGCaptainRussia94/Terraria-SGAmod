using SGAmod.Buffs;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class RustworkBlade : ModItem,IRustBurnText
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rustwork Blade");
            Tooltip.SetDefault("Applies Rustburn on hit\nThe Debuff duration scales up with the weapon's damage");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();

            item.damage = 5;
            item.width = 19;
            item.height = 22;
            item.melee = true;
            item.rare = 2;
            item.useStyle = 1;
            item.useAnimation = 20;
            item.useTime = 20;
            item.autoReuse = true;
            item.useTurn = true;
            item.knockBack = 3;
            item.value = 5000;
            item.consumable = false;
            item.UseSound = SoundID.Item19;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            RustBurn.ApplyRust(target, (2 + damage) * 80);
        }

    }
}
