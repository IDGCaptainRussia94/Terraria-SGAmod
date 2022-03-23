using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Idglibrary;

namespace SGAmod.Items.Pets
{

    public class HeartLanternPet : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guardian Heart Lantern");
            Tooltip.SetDefault("Summons a Guardian Heart Lantern to provide light and life regen to nearby players");
        }

        public override void SetDefaults()
        {
            item.damage = 0;
            item.useStyle = 1;
            item.width = 16;
            item.height = 30;
            item.UseSound = SoundID.Item2;
            item.useAnimation = 20;
            item.useTime = 20;
            item.rare = 4;
            item.noMelee = true;
            item.value = Item.sellPrice(0, 5, 0, 0);
            item.shoot = mod.ProjectileType("HeartLanternPetProj");
            item.buffType = mod.BuffType("HeartLanternPetBuff");
            item.noUseGraphic = true;
        }

        public override void UseStyle(Player player)
        {
            if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
            {
                player.AddBuff(item.buffType, 3600, true);
            }
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.HeartLantern, 1);
            recipe.AddIngredient(ItemID.DemonWings, 1);
            recipe.AddIngredient(ItemID.SoulofNight, 5);
            recipe.AddIngredient(mod.ItemType("VirulentBar"), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
        public class HeartLanternPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Heart Lantern Pet");
            Description.SetDefault("Healing you, providing light, and keeping you comfort <3");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            //SGAPlayer modPlayer = (SGAPlayer)player.GetModPlayer(mod, "OphioidPlayer");
            //modPlayer.PetBuff = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("HeartLanternPetProj")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("HeartLanternPetProj"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

    }

    public class HeartLanternPetProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("<3");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override string Texture
        {
            get { return ("Terraria/Item_"+ItemID.HeartLantern); }
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.netImportant = true;
        }

        public override bool PreAI()
        {
                for (int i = 0; i < Main.maxPlayers; i += 1)
                {
                    if (Main.player[i].active && !Main.player[i].dead)
                    {
                        if ((Main.player[i].Center - projectile.Center).Length() < 800)
                        {
                        //Main.player[i].AddBuff(BuffID.HeartLamp, 2);
                        if (!Main.player[i].HasBuff(BuffID.HeartLamp))
                        Main.player[i].lifeRegen += 2;
                        }
                    }
                }

            return true;

            }

        public override void AI()
        {

            projectile.localAI[0] += 1;
            Player player = Main.player[projectile.owner];
            Lighting.AddLight(projectile.Center,Color.Red.ToVector3());
            if (player.HasBuff(mod.BuffType("HeartLanternPetBuff")))
            {
                projectile.timeLeft = 2;
            }

            //Teleport if too far away
            Vector2 PlayPosProjPos = player.position - projectile.position;
            float distance = PlayPosProjPos.Length();
            if (Main.myPlayer == player.whoAmI && distance > 1000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                projectile.position = player.position;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = SGAmod.ExtraTextures[90];
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
            Vector2 drawPos = (((projectile.Center - Main.screenPosition)) + new Vector2(projectile.velocity.X > 0 ? -8f : -2f, 0f)*projectile.rotation.ToRotationVector2())+new Vector2(0f,-2f);
            int timing = (int)(projectile.localAI[0] / 6f);
            timing %= 4;
            timing *= ((tex.Height) / 4);
            spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), lightColor, projectile.rotation, drawOrigin, 0.75f, projectile.velocity.X>0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return true;
        }

    }

    public class StarinabottlePet : HeartLanternPet
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Guardian S.I.A.B");
            Tooltip.SetDefault("Summons a Guardian Star-in-a-bottle to provide light and mana regen to nearby players");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            item.shoot = mod.ProjectileType("StarinabottlePetProj");
            item.buffType = mod.BuffType("StarinabottlePetBuff");
            item.noUseGraphic = true;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.StarinaBottle, 1);
            recipe.AddIngredient(ItemID.AngelWings, 1);
            recipe.AddIngredient(ItemID.SoulofLight, 5);
            recipe.AddIngredient(mod.ItemType("VirulentBar"), 8);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

    }
    public class StarinabottlePetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("S.I.A.B Pet");
            Description.SetDefault("Providing you with light and your mates with some mana regen");
            Main.buffNoTimeDisplay[Type] = true;
            Main.lightPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            //SGAPlayer modPlayer = (SGAPlayer)player.GetModPlayer(mod, "OphioidPlayer");
            //modPlayer.PetBuff = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("StarinabottlePetProj")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, mod.ProjectileType("StarinabottlePetProj"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }

    }

    public class StarinabottlePetProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("<3");
            Main.projFrames[projectile.type] = 1;
            Main.projPet[projectile.type] = true;
            ProjectileID.Sets.LightPet[projectile.type] = true;
        }

        public override string Texture
        {
            get { return ("Terraria/Item_" + ItemID.StarinaBottle); }
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.ZephyrFish);
            aiType = ProjectileID.ZephyrFish;
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.netImportant = true;
        }

        public override bool PreAI()
        {
            for (int i = 0; i < Main.maxPlayers; i += 1)
            {
                if (Main.player[i].active && !Main.player[i].dead)
                {
                    if ((Main.player[i].Center - projectile.Center).Length() < 800)
                    {
                        //Main.player[i].AddBuff(BuffID.HeartLamp, 2);
                        if (!Main.player[i].HasBuff(BuffID.StarInBottle))
                            Main.player[i].manaRegenBonus += 25;
                    }
                }
            }

            return true;

        }

        public override void AI()
        {

            projectile.localAI[0] += 1;
            Player player = Main.player[projectile.owner];
            Lighting.AddLight(projectile.Center, Color.Yellow.ToVector3());
            if (player.HasBuff(mod.BuffType("StarinabottlePetBuff")))
            {
                projectile.timeLeft = 2;
            }

            //Teleport if too far away
            Vector2 PlayPosProjPos = player.position - projectile.position;
            float distance = PlayPosProjPos.Length();
            if (Main.myPlayer == player.whoAmI && distance > 1000f)
            {
                // Whenever you deal with non-regular events that change the behavior or position drastically, make sure to only run the code on the owner of the projectile,
                // and then set netUpdate to true
                projectile.position = player.position;
                projectile.velocity *= 0.1f;
                projectile.netUpdate = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = SGAmod.ExtraTextures[91];
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
            Vector2 drawPos = (((projectile.Center - Main.screenPosition)) + new Vector2(projectile.velocity.X > 0 ? -8f : -2f, 0f) * projectile.rotation.ToRotationVector2()) + new Vector2(0f, -2f);
            int timing = (int)(projectile.localAI[0] / 6f);
            timing %= 4;
            timing *= ((tex.Height) / 4);
            spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), lightColor, projectile.rotation, drawOrigin, 0.75f, projectile.velocity.X > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);
            return true;
        }

    }
}
