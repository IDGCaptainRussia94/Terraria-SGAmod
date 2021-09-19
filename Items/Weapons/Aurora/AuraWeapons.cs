using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Tools;
using SGAmod.NPCs.SpiderQueen;
using Idglibrary;
using SGAmod.Effects;

namespace SGAmod.Items.Weapons.Aurora
{
    public class TheNorthernShine : ModItem,IAuroraItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Northern Shine");
            Tooltip.SetDefault("Hold to cast rays of light down from the heavens!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 500;
            item.magic = true;
            item.mana = 25;
            item.width = 40;
            item.height = 40;
            item.useTime = 35;
            item.useAnimation = 35;
            item.channel = true;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 5;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<TheNorthernShineProj>();
            item.shootSpeed = 4f;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarFlareBook, 1);
            recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 190);
            recipe.AddIngredient(ItemID.ShinePotion, 1);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class TheNorthernShineProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sky Light Using");
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + 14); }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 10;
            projectile.light = 0.5f;
            projectile.width = 48;
            projectile.height = 48;
            projectile.magic = true;
            projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            //Main.PlaySound(SoundID.Item45, projectile.Center);

            return true;
        }

        public override void AI()
        {

            Player player = Main.player[projectile.owner];
            projectile.ai[1] += 1;
            if (player.dead)
            {
                projectile.Kill();
            }
            else
            {
                if ((projectile.ai[0] > 0 || player.statMana < 1) || !player.channel)
                {
                    projectile.ai[0] += 1;
                }
                else
                {
                    player.itemTime = 10;
                    player.itemAnimation = 10;

                    Vector2 mousePos = Main.MouseWorld;

                    if (projectile.owner == Main.myPlayer)
                    {
                        Vector2 diff = mousePos - player.Center;
                        diff.Normalize();
                        projectile.velocity = diff;
                        projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                        projectile.netUpdate = true;
                        projectile.Center = mousePos;
                    }
                    projectile.position -= projectile.Center;
                    projectile.timeLeft = 2;
                    int dir = projectile.direction;
                    player.ChangeDir(dir);

                    if (projectile.ai[0] > 0)
                    {
                        //player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
                    }
                    else
                    {
                        player.itemRotation = ((MathHelper.PiOver4*0.45f)*player.direction + projectile.velocity.ToRotation()+(player.direction<1 ? MathHelper.Pi : 0));

                        int counter = 0;

                        /*for (int pro = 0; pro < Main.maxProjectiles; pro += 1)
                        {
                            Projectile proj = Main.projectile[pro];
                            if (proj.ai[0] == 0 && proj.type == ModContent.ProjectileType<AcidScepterVenom>() && proj.owner == projectile.owner)
                            {
                                counter += 1;
                            }
                        }*/

                        if (projectile.ai[1] % 10 == 0 && projectile.ai[1] > 10 && player.CheckMana(player.HeldItem,10,true))
                        {

                            int thisoned = Projectile.NewProjectile(Main.MouseWorld.X, Main.MouseWorld.Y, Main.rand.NextFloat(-0.25f, 0.25f), Main.rand.NextFloat(-0.25f, 0.25f), ModContent.ProjectileType<TheNorthernShineProjBeam>(), projectile.damage, projectile.knockBack, Main.myPlayer);
                            IdgProjectile.Sync(thisoned);
                        }

                    }

                    projectile.Center = (player.Center) + new Vector2(0, -8);
                    projectile.velocity *= 8f;

                }

            }


        }
    }

    public class TheNorthernShineProjBeam : ModProjectile
    {
        private Vector2 skyhere;
        private Color color;
        private Color color2;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("The Northern Shine Proj Beam");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 96;
            projectile.height = 96;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.magic = true;
            projectile.timeLeft = 400;
            projectile.extraUpdates = 4;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override bool CanDamage()
        {
            return Math.Abs(projectile.timeLeft - 200) < 80;
        }

        public override void AI()
        {

            if (projectile.timeLeft == 200)
            {
                Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 5, 0.25f, -0.85f);
            }
            if (skyhere == default && Main.netMode != NetmodeID.Server)
            {
                float colorz = Main.rand.NextFloat();
                color = Main.hslToRgb(colorz, 0.50f,0.75f);
                color2 = Main.hslToRgb((colorz+0.25f)%1f, 0.75f,0.75f);
                skyhere = Main.screenPosition + new Vector2(Main.rand.Next(Main.screenWidth) + Main.rand.Next(-128, 129), Main.rand.Next(-200, 128));
            }
            if (projectile.ai[0] == 0)
            projectile.ai[0] = Main.rand.NextFloat(-1f,1f)*0.2f;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            Texture2D texstar = Main.projectileTexture[ModContent.ProjectileType<Dimensions.NPCs.SpaceBossHomingShot>()];

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect fadeIn = SGAmod.FadeInEffect;

            float difference = MathHelper.Clamp(1f - Math.Abs(projectile.timeLeft - 200) / 50f, 0f, 1f);
            float differencesin = (float)Math.Sin((projectile.timeLeft / 400f) * MathHelper.Pi);

            Vector2 tothere = projectile.Center - skyhere;

            fadeIn.Parameters["alpha"].SetValue(0.75f*differencesin);
            fadeIn.Parameters["strength"].SetValue(0f);
            fadeIn.Parameters["blendColor"].SetValue(color.ToVector3());
            fadeIn.Parameters["fadeOffset"].SetValue(new Vector2(0f, differencesin * 0.15f));
            fadeIn.Parameters["fadeMultiplier"].SetValue(new Vector2(1f, 5f));

            fadeIn.CurrentTechnique.Passes["SmoothYFadePass"].Apply();

            float angleoff = (float)Math.Cos((projectile.timeLeft / 400f) * MathHelper.Pi);

            spriteBatch.Draw(tex, skyhere - Main.screenPosition, null, Color.Aqua* differencesin, -MathHelper.PiOver2 + tothere.ToRotation()+(angleoff* projectile.ai[0]), new Vector2(tex.Width / 2f, 0), new Vector2(differencesin*2f, tothere.Length()/(float)tex.Height) * differencesin, default, 0);

            fadeIn.Parameters["alpha"].SetValue(0.50f * differencesin);
            fadeIn.Parameters["blendColor"].SetValue(color2.ToVector3());
            fadeIn.Parameters["fadeOffset"].SetValue(new Vector2(0f, 0.15f+(differencesin * 0.15f)));
            fadeIn.Parameters["fadeMultiplier"].SetValue(new Vector2(1f, 8f));

            fadeIn.CurrentTechnique.Passes["SmoothYFadePass"].Apply();

            spriteBatch.Draw(tex, skyhere - Main.screenPosition, null, Color.Aqua * differencesin, -MathHelper.PiOver2 + tothere.ToRotation() + (angleoff * projectile.ai[0]), new Vector2(tex.Width / 2f, 0), new Vector2(differencesin * 1f, tothere.Length() / (float)tex.Height) * differencesin, default, 0);

            fadeIn.Parameters["alpha"].SetValue(difference*0.20f);
            fadeIn.Parameters["strength"].SetValue(1f);
            fadeIn.Parameters["fadeColor"].SetValue(color.ToVector3());
            fadeIn.Parameters["blendColor"].SetValue(color.ToVector3());
            fadeIn.Parameters["fadeOffset"].SetValue(new Vector2(0f, 0f));
            fadeIn.Parameters["fadeMultiplier"].SetValue(new Vector2(1f, 100f));

            fadeIn.CurrentTechnique.Passes["SmoothYFadePass"].Apply();
            spriteBatch.Draw(texstar, projectile.Center - Main.screenPosition, null, Color.White * difference, 0, texstar.Size() / 2f, difference * 5f, default, 0);

            fadeIn.Parameters["alpha"].SetValue(difference * 1f);
            fadeIn.CurrentTechnique.Passes["SmoothYFadePass"].Apply();

            spriteBatch.Draw(texstar, projectile.Center - Main.screenPosition, null, Color.White * difference, MathHelper.PiOver2, texstar.Size() / 2f, difference * 3f, default, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
             //stuff
        }
    }
}