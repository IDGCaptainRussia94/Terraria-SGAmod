using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{

    public class StarfishAmmo : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.Starfish)
            {
                item.shoot = mod.ProjectileType("StarfishProjectile");
                item.ammo = item.type;
            }
        }
    }
    public class StarfishBlaster : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Starfish Blaster");
            Tooltip.SetDefault("Uses starfish as ammo.");
        }

        public override void SetDefaults()
        {
            item.damage = 27;
            item.useTime = 25;
            item.useAnimation = 25;
            item.useStyle = 5;
            item.noMelee = true;
            item.ranged = true;
            item.knockBack = 2;
            item.value = 10;
            item.rare = 8;
            item.UseSound = SoundID.Item11;
            item.autoReuse = true;
            item.shoot = mod.ProjectileType("StarfishProjectile");
            item.shootSpeed = 13f;
            item.useAmmo = 2626;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-4, -2);
        }   
    }
}             