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
using SGAmod.Tiles.TechTiles;
using Terraria.DataStructures;
using System.Linq;

namespace SGAmod.Items.Weapons.Aurora
{

    public class Skylight : ModItem, IAuroraItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skylight");
            Tooltip.SetDefault("Rend and Sunder all with the fury the heavens!");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.damage = 300;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 6;
            item.useAnimation = 1;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 5;
            item.channel = false;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Cyan;
            //item.UseSound = SoundID.Item20;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<SkylightSlashProj>();
            item.shootSpeed = 3f;
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun


            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
                {
                    return Color.Transparent;
                };

                item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];

                item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle, Color glowcolor)
                {
                    Texture2D texture = Main.itemTexture[item.type];
                    Vector2 origin = texture.Size() / 2f;
                    float timeAdvance = Main.GlobalTime * 2;
                    angle = drawInfo.drawPlayer.itemRotation + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
                    Player drawPlayer = drawInfo.drawPlayer;

                    Vector2 drawHere = drawPlayer.MountedCenter + (angle.ToRotationVector2()) * 38 - Main.screenPosition;

                    DrawData value = new DrawData(texture, drawHere, null, Color.White, MathHelper.PiOver4 + (drawInfo.drawPlayer.direction < 0 ? MathHelper.PiOver2 : 0) + angle, origin, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(value);

                    for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 12f)
                    {
                        DrawData value2 = new DrawData(texture, drawHere+ (i+Main.GlobalTime).ToRotationVector2()*0.20f, null, Color.Red * 0.02f, MathHelper.PiOver4+(drawInfo.drawPlayer.direction < 0 ? MathHelper.PiOver2 : 0) + angle, origin, 1f, drawInfo.spriteEffects, 0);
                        Main.playerDrawData.Add(value2);
                    }

                    Terraria.Utilities.UnifiedRandom rando = new Terraria.Utilities.UnifiedRandom((int)(item.whoAmI * 4554));

                    texture = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];

                    for (float a = 4f; a < 16f; a += 2f)
                    {
                        for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 4f)
                        {
                            float randomier = (float)Math.Sin(Main.GlobalTime* rando.NextFloat(-MathHelper.Pi * 0.5f, MathHelper.Pi * 0.5f))* rando.NextFloat(-MathHelper.Pi * -0.05f, MathHelper.Pi * 0.05f);
                            DrawData value2 = new DrawData(texture, drawHere + (angle.ToRotationVector2()*a) + (i + Main.GlobalTime * 2f).ToRotationVector2(), null, Color.Red * 0.05f, MathHelper.PiOver2 + angle+ randomier, new Vector2(0,0) +texture.Size() / 2f, new Vector2(0.75f,1.25f), drawInfo.spriteEffects, 0);
                            Main.playerDrawData.Add(value2);
                        }
                    }

                };
            }

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectileDirect(position,new Vector2(speedX, speedY)*1f,ModContent.ProjectileType<SkylightSlashProj>(),damage,knockBack,player.whoAmI);
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Arkhalis, 1);
            recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 10);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ItemID.WrathPotion, 1);
            recipe.AddTile(ModContent.TileType<LuminousAlter>());
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }

    public class SkylightSlashProj : ModProjectile,IDrawAdditive,ITrueMeleeProjectile
    {
        private Vector2 skyhere;
        private Color color;
        private Color color2;

        Vector3[] oldPos = new Vector3[10];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skylight Slash");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 32;
            projectile.height = 32;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = false;
            projectile.melee = true;
            projectile.timeLeft = (int)TimeLeft;
            projectile.extraUpdates = 6;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
            //ProjectileID.Sets.TrailCacheLength[projectile.type] = 12;
            //ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override string Texture => "SGAmod/Extra_57b";

        public override bool CanDamage()
        {
            return Math.Abs(projectile.timeLeft-TimeToHitWindow.Item2)>TimeToHitWindow.Item1;
        }

        Player Owner => Main.player[projectile.owner];

        float TimeLeft => 100f;
        (int,int) TimeToHitWindow => (10,45);

        Vector3 TrueLocation
        {
            get
            {
                Matrix swingAround = Matrix.CreateFromYawPitchRoll((-MathHelper.PiOver2*1.80f)+(MathHelper.Pi*(projectile.localAI[0]/ TimeLeft))*1.50f,0,0);
                Matrix swingUpOrDown = swingAround*Matrix.CreateFromYawPitchRoll(0, projectile.ai[0], 0)*Matrix.CreateScale(new Vector3((projectile.velocity.Length()/1f),2f,1f));

                Vector3 transformation = Vector3.Transform(Vector3.UnitX, swingUpOrDown) * 64f;

                Terraria.Utilities.UnifiedRandom rando = new Terraria.Utilities.UnifiedRandom((int)(projectile.ai[0]*4554));

                Vector2 vec2 = new Vector2(transformation.X, transformation.Y).RotatedBy(projectile.velocity.ToRotation() + rando.NextFloat(-0.25f, 0.25f)) + Vector2.Normalize(projectile.velocity) * 48f;

                return Owner.MountedCenter.ToVector3() + new Vector3(vec2.X,vec2.Y, transformation.Z);
            }

        }

        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            if (TrueLocation.Z < 0)
            {
                drawCacheProjsBehindNPCs.Add(index);
            }
        }


        public override void AI()
        {

            projectile.localAI[0]++;

            if (projectile.ai[0] == 0)
            {
                projectile.ai[0] = Main.rand.NextFloat(-MathHelper.Pi * 0.25f, MathHelper.Pi * 0.25f);
                projectile.netUpdate = true;
                for(int i = 0; i < oldPos.Length; i++)
                {
                    oldPos[i] = TrueLocation-Vector3.Normalize(new Vector3(projectile.velocity.X, projectile.velocity.Y,0) * i*320f);
                }
            }

            if (projectile.timeLeft == 200)
            {
                Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 5, 0.25f, -0.85f);
            }
            if (skyhere == default && Main.netMode != NetmodeID.Server)
            {
                float colorz = Main.rand.NextFloat();
                color = Main.hslToRgb(colorz, 0.50f, 0.50f);
                color2 = Main.hslToRgb((colorz + 0.10f) % 1f, 0.50f, 0.75f);
                skyhere = Main.screenPosition + new Vector2(Main.rand.Next(Main.screenWidth) + Main.rand.Next(-128, 129), Main.rand.Next(-200, 128));
            }

            oldPos[0] = TrueLocation;
            if (projectile.numUpdates == 0)
            {
                for (int i = oldPos.Length - 1; i > 0; i -= 1)
                {
                    oldPos[i] = Vector3.Lerp(oldPos[i], oldPos[i - 1],0.750f);
                }
            }


            projectile.Opacity = MathHelper.Clamp((projectile.localAI[0]-10f)/30f,0f,Math.Min(1f,projectile.timeLeft/70f));
            projectile.Center = new Vector2(TrueLocation.X, TrueLocation.Y) - projectile.velocity;

        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            Texture2D glow = ModContent.GetTexture("SGAmod/Glow");

            spriteBatch.Draw(glow, projectile.Center - Main.screenPosition, null, color2 * projectile.Opacity, MathHelper.PiOver2, glow.Size() / 2f, 2f + (TrueLocation.Z / 200f), default, 0);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];

            TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", ModContent.GetTexture("SGAmod/TrailEffect"));
            //trail.projsize = projectile.Hitbox.Size() / 2f;
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -8f);
            trail.coordMultiplier = new Vector2(1f, 3f);
            trail.trailThickness = 8;
            trail.trailThicknessIncrease = 12;
            trail.perspective = false;
            trail.strength = MathHelper.Clamp(projectile.Opacity,0f,1f)*3f;
            trail.color = delegate (float percent)
            {
                return color*MathHelper.Clamp((projectile.localAI[0]- (percent *80f))/ 60f,0f,1f);
            };
            trail.DrawTrail(oldPos.ToList(), projectile.Center);

            trail = new TrailHelper("BasicEffectAlphaPass", ModContent.GetTexture("SGAmod/TrailEffect"));
            //trail.projsize = projectile.Hitbox.Size() / 2f;
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -12f);
            trail.coordMultiplier = new Vector2(1f, 1f);
            trail.trailThickness = 4;
            trail.trailThicknessIncrease = 6;
            trail.perspective = false;
            trail.strength = MathHelper.Clamp(projectile.Opacity, 0f, 1f) * 5f;
            trail.color = delegate (float percent)
            {
                return color * MathHelper.Clamp((projectile.localAI[0] - (percent * 60f)) / 40f, 0f, 1f); ;
            };
            trail.DrawTrail(oldPos.Select(testby => new Vector3(testby.X,testby.Y,testby.Z*1f)).ToList(), projectile.Center);

            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, color * projectile.Opacity, Vector2.Normalize(projectile.Center - new Vector2(oldPos[1].X, oldPos[1].Y)).ToRotation(), tex.Size() / 2f, (1f + (TrueLocation.Z / 128f)) * new Vector2(1f,0.60f), default, 0);
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, 0, tex.Size() / 2f, (1f + (TrueLocation.Z / 128f))*0.20f, default, 0);

            return false;
        }

    }


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
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 10);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ItemID.ShinePotion, 1);
            recipe.AddTile(ModContent.TileType<LuminousAlter>());
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