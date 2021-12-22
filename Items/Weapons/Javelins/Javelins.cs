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
    public class CrimsonCatastrophe : SanguineBident, IJablinItem
    {

        public override float Stabspeed => 10f;
        public override float Throwspeed => 25f;
        public override int Penetrate => 8;
        public override float Speartype => 10;
        public override int[] Usetimes => new int[] { 25, 15 };
        public override string[] Normaltext => new string[] { "A twisted form of thrown blood lust that explodes your foe's blood out from their wounds", "Throws 3 Jab-lins that inflict area damage against foes that are Massively Bleeding", "In Addition, crits against enemies afflicted with Everlasting Suffering", "On proc the Bleeding and Everlasting Suffering are removed, with a delay before retrigger", "Doesn't effect the enemy the Jab-lin is stuck to", "Primary Fire flies far and fast, and inflicts Massive Bleeding", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Catastrophe");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 320;
            item.width = 32;
            item.crit = 10;
            item.height = 32;
            item.knockBack = 5;
            item.value = Item.buyPrice(gold: 50);
            item.rare = ItemRarityID.Red;
            item.consumable = false;
            item.maxStack = 1;
            item.UseSound = SoundID.Item1;
        }

        public static void BloodyExplosion(NPC enemy, Projectile projectile)
        {
            if (enemy.GetGlobalNPC<SGAnpcs>().crimsonCatastrophe > 0)
                return;

            int bleed = SGAmod.Instance.BuffType("MassiveBleeding");
            int els = SGAmod.Instance.BuffType("EverlastingSuffering");
            float damazz = (Main.DamageVar((float)projectile.damage * 2f));
            bool crit = false;

            int buffindex = enemy.FindBuffIndex(bleed);
            if (buffindex >= 0)
                enemy.DelBuff(buffindex);

            if (enemy.HasBuff(els))
            {
                buffindex = enemy.FindBuffIndex(els);
                crit = true;
                enemy.DelBuff(buffindex);
            }

            projectile.localAI[0] = 20;
            enemy.GetGlobalNPC<SGAnpcs>().crimsonCatastrophe = 60;

            for (int num315 = -40; num315 < 43; num315 = num315 + (crit ? 1 : 3))
            {
                int dustType = DustID.LunarOre;//Main.rand.Next(139, 143);
                int dustIndex = Dust.NewDust(enemy.Center + new Vector2(-16, -16) + ((Main.rand.NextFloat(0, MathHelper.TwoPi)).ToRotationVector2() * num315), 32, 32, dustType);
                Dust dust = Main.dust[dustIndex];
                dust.velocity.X = projectile.velocity.X * (num315 * 0.02f);
                dust.velocity.Y = projectile.velocity.Y * (num315 * 0.02f);
                dust.velocity.RotateRandom(Math.PI / 2.0);
                dust.scale *= 1f + Main.rand.NextFloat(0.2f, 0.5f) / (1f + (num315 / 15f)) + (crit ? 1f : 0f);
                dust.fadeIn = 0.25f;
                dust.noGravity = true;
                Color mycolor = crit ? Color.CornflowerBlue : Color.OrangeRed;
                dust.color = mycolor;
                dust.alpha = 20;
            }

            enemy.StrikeNPC((int)damazz, 0f, 0, crit, true, true);

            if (Main.netMode != 0)
            {
                NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, enemy.whoAmI, damazz, 16f, (float)1, 0, 0, 0);
            }

        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SanguineBident"), 1);
            recipe.AddIngredient(mod.ItemType("TerraTrident"), 1);
            recipe.AddIngredient(ItemID.DayBreak, 1);
            recipe.AddIngredient(mod.ItemType("StygianCore"), 2);
            recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 15);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }


    }

    public class TerraTrident : SanguineBident, IJablinItem
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
            item.damage = 42;
            item.width = 32;
            item.height = 32;
            item.crit = 10;
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
            recipe.AddIngredient(mod.ItemType("Contagion"), 1);
            recipe.AddIngredient(mod.ItemType("ThrownTrident"), 1);
            recipe.AddIngredient(ItemID.UnholyTrident, 1);
            recipe.AddIngredient(ItemID.Gungnir, 1);
            recipe.AddIngredient(mod.ItemType("ThermalJavelin"), 300);
            recipe.AddIngredient(mod.ItemType("OmniSoul"), 12);
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
            for (int i = 0; i < 25; i += 1)
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
            projectile.rotation = ((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f) - MathHelper.ToRadians(45);

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

    public class SanguineBident : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 3.6f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 3;
        public override float Speartype => 8;
        public override int[] Usetimes => new int[] { 25, 10 };
        public override string[] Normaltext => new string[] { "Launch 3 projectiles on throw at foes", "Impaled targets may leach life, more likely to leach from bleeding targets", "Melee strikes will make enemies bleed", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sanguine Bident");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 50;
            item.width = 32;
            item.height = 32;
            item.knockBack = 5;
            item.value = Item.buyPrice(gold: 5);
            item.rare = 7;
            item.consumable = false;
            item.maxStack = 1;
        }
        public override void OnThrow(int type, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type2, ref int damage, ref float knockBack, JavelinProj madeproj)
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

                        if (Speartype == (int)JavelinType.CrimsonCatastrophe)
                            Main.projectile[thisoneddddd].aiStyle = (-100 + i);


                       (Main.projectile[thisoneddddd].modProjectile as JavelinProj).maxstick = madeproj.maxstick;
                        (Main.projectile[thisoneddddd].modProjectile as JavelinProj).maxStickTime = madeproj.maxStickTime;
                        Main.projectile[thisoneddddd].penetrate = madeproj.projectile.penetrate;
                        Main.projectile[thisoneddddd].netUpdate = true;
                        IdgProjectile.Sync(thisoneddddd);

                    }
                }

            }
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            switch (rand.Next(16))
            {
                case 1:
                    return PrefixID.Demonic;
                case 2:
                    return PrefixID.Frenzying;
                case 3:
                    return PrefixID.Dangerous;
                case 4:
                    return PrefixID.Savage;
                case 5:
                    return PrefixID.Furious;
                case 6:
                    return PrefixID.Terrible;
                case 7:
                    return PrefixID.Awful;
                case 8:
                    return PrefixID.Dull;
                case 9:
                    return PrefixID.Unhappy;
                case 10:
                    return PrefixID.Unreal;
                case 11:
                    return PrefixID.Shameful;
                case 12:
                    return PrefixID.Heavy;
                case 13:
                    return PrefixID.Zealous;
                case 14:
                    return mod.PrefixType("Tossable");
                case 15:
                    return mod.PrefixType("Impacting");
                default:
                    return mod.PrefixType("Olympian");
            }
        }

        public override bool ConsumeItem(Player player)
        {
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Trident, 1);
            recipe.AddIngredient(mod.ItemType("CrimsonJavelin"), 150);
            recipe.AddIngredient(ItemID.Vertebrae, 10);
            recipe.AddIngredient(ItemID.Ectoplasm, 8);
            recipe.AddIngredient(mod.ItemType("StygianCore"), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }

    public class SwampSovnya: SanguineBident, IJablinItem,IDankSlowText
    {
        public override float Stabspeed => 4.00f;
        public override float Throwspeed => 10f;
        public override int Penetrate => 5;
        public override float Speartype => 12;
        public override int[] Usetimes => new int[] { 25, 6 };
        public override string[] Normaltext => new string[] {"'Hunt or be hunted'", "Thrown Jab-libs inflict Dank Slow", "Jabs crit slowed targets and remove the debuff, increasing damage based on slow up to 5X","Additionally, this weapon does 25% increased direct and DOT damage to poison-immune enemies", "Is considered a Jab-lin, but non consumable and able to have prefixes" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Sovnya");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 60;
            item.width = 32;
            item.height = 32;
            item.crit = 0;
            item.knockBack = 3;
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
            //nothing
        }

    }

    public class ThermalJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 4.00f;
        public override float Throwspeed => 14f;
        public override int Penetrate => 5;
        public override float Speartype => 11;
        public override int[] Usetimes => new int[] { 20, 6 };
        public override string[] Normaltext => new string[] { "Applies Thermal Blaze to your enemies"};
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Thermal Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 56;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 50;
            item.rare = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("UnmanedBar"), 2);
            recipe.AddIngredient(mod.ItemType("FieryShard"), 1);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
        }

    }

    public class ShadowJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.70f;
        public override float Throwspeed => 1f;
        public override int Penetrate => 5;
        public override float Speartype => 7;
        public override int[] Usetimes => new int[] { 25, 7 };
        public override string[] Normaltext => new string[] { "Made from evil Jab-lins and the dark essence emitted by a shadow key, attacks may inflict Shadowflame","The Shadow Key is NOT consumed on craft!", "Javelins accelerates forward, is not affected by gravity until it hits a target" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Shadow Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 32;
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

    public class PearlWoodJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 3.00f;
        public override float Throwspeed => 13f;
        public override int Penetrate => 5;
        public override float Speartype => 6;
        public override int[] Usetimes => new int[] { 20, 8 };
        public override string[] Normaltext => new string[] { "The Hallow's wrath makes stars fall down on jabbed or impaled targets","Stars scale damage with your Damage over Time boosts" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("PearlWood Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 36;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 50;
            item.rare = 5;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Pearlwood, 10);
            recipe.AddIngredient(ItemID.CrystalShard, 3);
            recipe.AddIngredient(ItemID.UnicornHorn, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 200);
            recipe.AddRecipe();
        }

    }


    public class DynastyJavelin : StoneJavelin, IJablinItem
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
            item.damage = 28;
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

    public class AmberJavelin : StoneJavelin, IJablinItem
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
            item.damage = 20;
            item.width = 24;
            item.height = 24;
            item.knockBack = 4;
            item.value = 30;
            item.rare = 3;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.PalmWood, 10);
            recipe.AddIngredient(ItemID.FossilOre, 2);
            recipe.AddIngredient(ItemID.Amber, 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 250);
            recipe.AddRecipe();
        }

    }

        public class CorruptionJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.50f;
        public override float Throwspeed => 9f;
        public override float Speartype => 3;
        public override int Penetrate => 4;
        public override int[] Usetimes => new int[] { 30, 10 };
        public override string[] Normaltext => new string[] { "Made from corrupt materials" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Corruption Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 15;
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
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }

    }

    public class CrimsonJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.20f;
        public override float Throwspeed => 8f;
        public override float Speartype => 2;
        public override int Penetrate => 4;
        public override int[] Usetimes => new int[] { 40, 15 };
        public override string[] Normaltext => new string[] { "Made from bloody materials" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Crimson Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 20;
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
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }

    }

    public class IceJavelin : StoneJavelin, IJablinItem
    {

        public override float Stabspeed => 1.20f;
        public override float Throwspeed => 6f;
        public override float Speartype => 1;
        public override int[] Usetimes => new int[] { 40, 15 };
        public override string[] Normaltext => new string[] { "Made from cold materials, attacks may inflict Frostburn" };
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ice Jab-lin");
        }
        public override void SetDefaults()
        {
            base.SetDefaults();
            item.damage = 14;
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
            recipe.AddIngredient(ItemID.IceBlock, 10);
            recipe.AddIngredient(mod.ItemType("FrigidShard"), 1);
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 150);
            recipe.AddRecipe();
        }

    }


}
