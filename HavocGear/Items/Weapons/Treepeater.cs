using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Idglibrary;

namespace SGAmod.HavocGear.Items.Weapons
{
    public class Treepeater : ModItem
    {
        public override void SetDefaults()
        {
            item.damage = 58;
            item.ranged = true;
            item.width = 22;
            item.height = 56;
            item.useTime = 26;
            item.useAnimation = 26;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 4;
            item.value = 100000;
            item.rare = 4;
            item.UseSound = SoundID.Item5;
            item.autoReuse = true;
            item.shoot = 10;
            item.shootSpeed = 10f;
            item.useAmmo = AmmoID.Arrow;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Treepeater");
            Tooltip.SetDefault("Arrows shot are extremely fast and inflict Dryad's Bane");
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float speed = 8f;
            float rotation = MathHelper.ToRadians(2);
            position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

            //for (int i = 0; i < 2; i += 1)
            //{

            Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
            speedX = perturbedSpeed.X;
            speedY = perturbedSpeed.Y;

            int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
            Main.projectile[proj].friendly = true;
            Main.projectile[proj].hostile = false;
            Main.projectile[proj].timeLeft = 600;
            Main.projectile[proj].extraUpdates += 1;
            Main.projectile[proj].knockBack = item.knockBack;

           IdgProjectile.AddOnHitBuff(proj, BuffID.DryadsWardDebuff, 60 * 7);

            return false;

            //}

        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-8, -4);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DankWood", 25);
            recipe.AddIngredient(null, "DankCore", 1);
            recipe.AddIngredient(null, "VirulentBar", 12);
            recipe.AddIngredient(ItemID.VineRopeCoil, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
}
