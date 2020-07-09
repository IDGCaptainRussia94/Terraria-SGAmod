using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Terraria.Utilities;
using Idglibrary;
using Idglibrary.Bases;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons.Javelins
{

    public class TerraTrident : SanguineBident
    {

        public override float Stabspeed => 3.6f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 4;
        public override float Speartype => 9;
        public override int[] Usetimes => new int[] { 25, 10 };
        public override string[] Normaltext => new string[] { "Jabs launch Terra Forks that do not cause immunity frames", "Weaker Terra Forks are unleashed through impaled targets", "These weaker forks pierce less times and travel short distances", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Trident");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 50;
            item.width = 32;
            item.height = 32;
            item.knockBack = 5;
            item.value = Item.buyPrice(gold: 5);
            item.rare = ItemRarityID.Lime;
            item.consumable = false;
            item.maxStack = 1;
            item.UseSound = SoundID.Item1;
        }
        public override void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj madeproj)
        {
            //nothing
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("ThrownTrident"), 1);
            recipe.AddIngredient(ItemID.UnholyTrident, 1);
            recipe.AddIngredient(ItemID.Gungnir, 1);
            recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }

    public class TerraTridentProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Terra Trident");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 4;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = true;
            projectile.penetrate = 5;
            projectile.alpha = 120;
            projectile.timeLeft = 500;
            projectile.light = 0.75f;
            projectile.extraUpdates = 2;
            projectile.localNPCHitCooldown = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.ignoreWater = true;
        }
        public override bool PreKill(int timeLeft)
        {
            for(int i = 0; i < 25; i += 1)
            {
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.AncientLight, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f, 150, default(Color), 1.5f);
                Main.dust[dustIndex].velocity += projectile.velocity * 0.3f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].color = Color.Lime;
            }
            Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 10, 1f, 0.25f);
            return true;
        }
        public override void AI()
        {
            projectile.rotation = ((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f)-MathHelper.ToRadians(45);

            if (Main.rand.Next(1) == 0)
            {
                float velmul = Main.rand.NextFloat(0.1f, 0.25f);
                int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.AncientLight, projectile.velocity.X * velmul, projectile.velocity.Y * velmul, 200, default(Color), 0.7f);
                Main.dust[dustIndex].velocity += projectile.velocity * 0.3f;
                Main.dust[dustIndex].noGravity = true;
                Main.dust[dustIndex].color = Color.Lime;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
            for (int k = 0; k < projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
                Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
                spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            }
            return true;
        }
    }

    public class SanguineBident : StoneJavelin
    {

        public override float Stabspeed => 3.6f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 3;
        public override float Speartype => 8;
        public override int[] Usetimes => new int[] { 25, 10 };
        public override string[] Normaltext => new string[] {"Launch 3 projectiles on throw at foes", "Impaled targets may leach life, more likely to leach from bleeding targets","Melee strikes will make enemies bleed","Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine Bident");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 42;
            item.width = 32;
            item.height = 32;
            item.knockBack = 5;
            item.value = Item.buyPrice(gold: 5);
            item.rare = 7;
            item.consumable = false;
            item.maxStack = 1;
        }
        public override void OnThrow(int type,Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj madeproj)
        {
            if (type == 1)
            {
                Vector2 normalizedspeed = new Vector2(speedX, speedY);
                normalizedspeed.Normalize();
                normalizedspeed *= (Throwspeed * player.Throwing().thrownVelocity);
                normalizedspeed.Y -= Math.Abs(normalizedspeed.Y * 0.1f);
                if (player.altFunctionUse == 2)
                {
                    for (int i = -15; i < 16; i += 30)
                    {
                        Vector2 perturbedSpeed = ((new Vector2(normalizedspeed.X, normalizedspeed.Y)).RotatedBy(MathHelper.ToRadians(i))).RotatedByRandom(MathHelper.ToRadians(10)) * 0.85f;
                        float scale = 1f - (Main.rand.NextFloat() * .01f);
                        perturbedSpeed = perturbedSpeed * scale;
                        type2 = mod.ProjectileType("JavelinProj");

                        int thisoneddddd = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type2, damage, knockBack, Main.myPlayer);
                        Main.projectile[thisoneddddd].ai[1] = Speartype;
                        Main.projectile[thisoneddddd].melee = false;
                        Main.projectile[thisoneddddd].Throwing().thrown = true;

                        (Main.projectile[thisoneddddd].modProjectile as JavelinProj).maxstick = madeproj.maxstick;
                        Main.projectile[thisoneddddd].penetrate = madeproj.projectile.penetrate;
                        Main.projectile[thisoneddddd].netUpdate = true;
                        IdgProjectile.Sync(thisoneddddd);

                    }
                }

            }
            
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("CrimsonJavelin"), 150);
            recipe.AddIngredient(ItemID.Vertebrae, 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 8);
            recipe.AddIngredient(ItemID.Trident, 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
    public class ShadowJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 1f;
        public override int Penetrate => 5;
        public override float Speartype => 7;
        public override int[] Usetimes => new int[] { 25, 12 };
        public override string[] Normaltext => new string[] { "Made from evil Jab-lins and the dark essence emited by a shadow key, attacks may inflict shadowflame", "Javelins accelerates forward, is not affected by gravity until it hits a target" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 25;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 40;
            item.rare = 4;
        }
        public override void AddRecipes()
        {
            ShadowJavelinRecipe recipe = new ShadowJavelinRecipe(mod);
            recipe.AddIngredient(ItemID.ShadowKey, 1);
            recipe.AddRecipeGroup("SGAmod:EvilJavelins", 50);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }

    }

    public class PearlWoodJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 11f;
        public override int Penetrate => 5;
        public override float Speartype => 6;
        public override int[] Usetimes => new int[] { 25, 8 };
        public override string[] Normaltext => new string[] { "The Hallow's wrath makes stars fall down on jabbed or impaled targets" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PearlWood Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 32;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 50;
            item.rare = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pearlwood, 5);
            recipe.AddIngredient(ItemID.CrystalShard, 2);
            recipe.AddIngredient(ItemID.UnicornHorn, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }

    }


    public class DynastyJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 4;
        public override float Speartype => 5;
        public override int[] Usetimes => new int[] { 35, 12 };
        public override string[] Normaltext => new string[] { "The Battle calls!", "Summons extra Dynasty Javelins to fall from the sky when they damage an enemy" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dynasty Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 17;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 30;
            item.rare = 3;
        }
        public override void AddRecipes()
        {
            //null
        }
    }

    public class AmberJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 8;
        public override float Speartype => 4;
        public override int[] Usetimes => new int[] { 25, 12 };
        public override string[] Normaltext => new string[] { "Made from sandy materials, Sticks into targets for longer" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Amber Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 18;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 30;
            item.rare = 3;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PalmWood, 5);
            recipe.AddIngredient(ItemID.Sandstone, 10);
            recipe.AddIngredient(ItemID.Amber, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }

    }

        public class CorruptionJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.50f;
        public override float Throwspeed => 9f;
        public override float Speartype => 3;
        public override int[] Usetimes => new int[] { 30, 10 };
        public override string[] Normaltext => new string[] { "Made from corrupt materials" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corruption Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 12;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 25;
            item.rare = 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Ebonwood, 5);
            recipe.AddIngredient(ItemID.EbonstoneBlock, 10);
            recipe.AddIngredient(ItemID.DemoniteBar, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }

    }

    public class CrimsonJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.20f;
        public override float Throwspeed => 8f;
        public override float Speartype => 2;
        public override int[] Usetimes => new int[] { 40, 15 };
        public override string[] Normaltext => new string[] { "Made from bloody materials" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 16;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 25;
            item.rare = 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Shadewood, 5);
            recipe.AddIngredient(ItemID.CrimstoneBlock, 10);
            recipe.AddIngredient(ItemID.CrimtaneBar, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }

    }

    public class IceJavelin : StoneJavelin
    {

        public override float Stabspeed => 1.20f;
        public override float Throwspeed => 6f;
        public override float Speartype => 1;
        public override int[] Usetimes => new int[] { 40, 15 };
        public override string[] Normaltext => new string[] { "Made from cold materials, attacks may inflict frostburn" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 11;
            item.width = 24;
            item.height = 24;
            item.knockBack = 5;
            item.value = 15;
            item.rare = 2;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.BorealWood, 5);
            recipe.AddIngredient(ItemID.IceBlock, 20);
            recipe.AddIngredient(mod.ItemType("FrigidShard"), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 50);
            recipe.AddRecipe();
        }

    }


}