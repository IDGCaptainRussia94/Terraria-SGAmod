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
using SGAmod.Buffs;
using Terraria.Utilities;

namespace SGAmod.Items.Weapons.Aurora
{

    public class Skylight : ModItem, IAuroraItem
    {

        public static Color[] SkyLightColors => new Color[]{Color.Orange,Color.Cyan,Color.Red,Color.Yellow };
        public static int MaxSkylight => 8000;
        public static int SkylightChargeRate => 30;
        public static int SkylightCostPerSlash => 50;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skylight");
            Tooltip.SetDefault("Rend and Sunder all with the fury the heavens!\nPrimary channels energy from the celestial body in the sky\nOnce channeling, you cannot cancel for a short time\nYou can only channel when your energy is depleted\nSecondary launches a celestial slash");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostUpdateEquipsEvent += SkylightPlayerUpdate;
            return true;
        }

        public void SkylightPlayerUpdate(SGAPlayer sgaply)
        {
            sgaply.skylightLightInfused = (Math.Max(sgaply.skylightLightInfused.Item1 - 1,0), sgaply.skylightLightInfused.Item2);
        }

        public override void SetDefaults()
        {
            item.damage = 400;
            item.crit = 20;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 6;
            item.useAnimation = 6;
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
                    float itemrot = drawInfo.drawPlayer.itemRotation;
                    float itemalpha = 1f;
                    Player drawPlayer = drawInfo.drawPlayer;
                    float skylightInfuse = MathHelper.Clamp((drawPlayer.SGAPly().skylightLightInfused.Item1 / (float)Skylight.MaxSkylight) * 2f, 0f, 1f);
                    //float skylightInfuse = drawPlayer.SGAPly().skylightLightInfused.Item1 / (float)Skylight.MaxSkylight;

                    Projectile[] these = Main.projectile.Where(testby => testby.active && testby.type == ModContent.ProjectileType<SkylightSlashProj>() && testby.owner == drawPlayer.whoAmI).ToArray();
                    these = these.OrderBy(testby => testby.timeLeft).ToArray();

                    if (true)
                    {

                        itemalpha = MathHelper.Clamp((drawPlayer.itemAnimation / (float)drawPlayer.itemAnimationMax) * 1.00f, 0f, 1f);
                        if (these.Length > 0)
                        {
                            angle = (these[0].Center - drawInfo.drawPlayer.MountedCenter).ToRotation();// + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
                        }
                        if (drawPlayer.ownedProjectileCounts[ModContent.ProjectileType<SkylightChargeUpProj>()] > 0)
                        {
                            angle = itemrot + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
                        }

                        Vector2 drawHere = drawPlayer.MountedCenter + (angle.ToRotationVector2()) * 38 - Main.screenPosition;

                        DrawData value = new DrawData(texture, drawHere, null, Color.White * itemalpha, MathHelper.PiOver4 + (drawInfo.drawPlayer.direction < 0 ? MathHelper.PiOver2 : 0) + angle, origin, 1f, drawInfo.spriteEffects, 0);
                        Main.playerDrawData.Add(value);

                        for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 12f)
                        {
                            DrawData value2 = new DrawData(texture, drawHere + (i + Main.GlobalTime).ToRotationVector2() * 0.20f, null, SkyLightColors[drawPlayer.SGAPly().skylightLightInfused.Item2] * skylightInfuse * itemalpha * 0.02f, MathHelper.PiOver4 + (drawInfo.drawPlayer.direction < 0 ? MathHelper.PiOver2 : 0) + angle, origin, 1f, drawInfo.spriteEffects, 0);
                            Main.playerDrawData.Add(value2);
                        }

                        Terraria.Utilities.UnifiedRandom rando = new Terraria.Utilities.UnifiedRandom((int)(item.whoAmI * 4554));

                        texture = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];

                        for (float a = 4f; a < 16f; a += 2f)
                        {
                            for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.TwoPi / 4f)
                            {
                                float randomier = (float)Math.Sin(Main.GlobalTime * rando.NextFloat(-MathHelper.Pi * 0.5f, MathHelper.Pi * 0.5f)) * rando.NextFloat(-MathHelper.Pi * -0.05f, MathHelper.Pi * 0.05f);
                                DrawData value2 = new DrawData(texture, drawHere + (angle.ToRotationVector2() * a) + (i + Main.GlobalTime * 2f).ToRotationVector2(), null, SkyLightColors[drawPlayer.SGAPly().skylightLightInfused.Item2] * skylightInfuse * itemalpha * 0.05f, MathHelper.PiOver2 + angle + randomier, new Vector2(0, 0) + texture.Size() / 2f, new Vector2(0.75f, 1.25f), drawInfo.spriteEffects, 0);
                                Main.playerDrawData.Add(value2);
                            }
                        }
                    }
                };
            }
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            SGAPlayer sgaply = Main.LocalPlayer.SGAPly();
            if (sgaply.skylightLightInfused.Item1 > 0)
            {
                if (sgaply.skylightLightInfused.Item2 == 0)
                {
                    float stuff = MathHelper.Clamp((sgaply.skylightLightInfused.Item1 / (float)Skylight.MaxSkylight) * 2f, 0f, 1f);
                    add += stuff * 0.50f;
                }
            }
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            SGAPlayer sgaply = Main.LocalPlayer.SGAPly();
            if (sgaply.skylightLightInfused.Item1 > 0)
            {
                if (sgaply.skylightLightInfused.Item2 == 0)
                {
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Orange, "Infused with the fury of the Sun")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Orange, "Attacks incinerate foes with Daybroken and Lava Burn")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Orange, "Improves damage by 50% based on remaining charge")));

                }
                if (sgaply.skylightLightInfused.Item2 == 1)
                {
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Cyan, "Infused with the cursing rays of the Moon")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Cyan, "Attacks melt foes with Moonlight Curse")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Cyan, "Improves attack range by 100% based on remaining charge")));
                }
                if (sgaply.skylightLightInfused.Item2 == 2)
                {
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Red, "Infused with the sanguine desires of the Blood Moon")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Red, "Attacks will life steal")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Red, "Improves attack range by 150% based on remaining charge and life lost")));
                }

            }

        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool CanUseItem(Player player)
        {
            item.channel = true;
            if (player.altFunctionUse == 2 || player.SGAPly().skylightLightInfused.Item1 > 0)
            {
                item.channel = false;
            }

            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {

            if (player.altFunctionUse == 2 || player.SGAPly().skylightLightInfused.Item1 > 0)
            {
                Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY) * 1f, ModContent.ProjectileType<SkylightSlashProj>(), damage, knockBack, player.whoAmI);
            }
            else
            {
                Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY) * 1f, ModContent.ProjectileType<SkylightChargeUpProj>(), damage, knockBack, player.whoAmI);
            }
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

    public class SkylightChargeUpProj : ModProjectile, IDrawAdditive
    {
        public override string Texture => "SGAmod/Glow";

        public override bool CanDamage()
        {
            return false;
        }

        Player Owner => Main.player[projectile.owner];

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skylight Charging");
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
            projectile.timeLeft = 200;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override void AI()
        {
            Vector2 mousePos = Main.MouseWorld;
            SGAPlayer sgaply = Owner.SGAPly();
            Player player = Owner;

            if (player.dead)
            {
                projectile.Kill();
            }

            if (projectile.timeLeft < 30 && (!player.channel || projectile.ai[0] > 0 || sgaply.skylightLightInfused.Item1>=Skylight.MaxSkylight))
            {
                projectile.ai[0] += 1;
                projectile.netUpdate = true;
            }

            if (projectile.ai[1] == 0)
            {
                sgaply.skylightLightInfused.Item2 = 0;
                if (!Main.dayTime)
                    sgaply.skylightLightInfused.Item2 = (byte)(Main.bloodMoon ? 2 : 1);
            }

            if (projectile.ai[0] < 1)
            {
                sgaply.skylightLightInfused.Item1 = (int)MathHelper.Clamp(sgaply.skylightLightInfused.Item1+Skylight.SkylightChargeRate, 0,Skylight.MaxSkylight);
                if (projectile.timeLeft<30)
                projectile.timeLeft = 30;
            }

            // Multiplayer support here, only run this code if the client running it is the owner of the projectile
            if (projectile.owner == Main.myPlayer)
            {
                Vector2 diff = mousePos - player.Center;
                diff.Normalize();
                projectile.velocity = diff;
                if (projectile.ai[0] < 50f)
                    projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
                projectile.netUpdate = true;
                projectile.Center = mousePos;
            }

            int dir = projectile.direction;
            player.ChangeDir(dir);

            projectile.Center = Owner.MountedCenter + new Vector2(0, -Owner.gravDir * 48f);

            if (projectile.ai[0] < 1)
            {
                player.itemAnimationMax = 30;
                if (player.itemTime < 30)
                    player.itemTime = 30;
                if (player.itemAnimation < 30)
                    player.itemAnimation = 30;
                player.itemRotation = (float)Math.Atan2(-32f * dir, 0f * dir);
            }

                projectile.ai[1] += 1;
        }

        public void DrawAdditive(SpriteBatch spriteBatch)
        {
            Texture2D glow = ModContent.GetTexture("SGAmod/Glow");
            Vector2[] spots = { projectile.Center, SGAUtils.SunPosition() + Main.screenPosition };

            UnifiedRandom rando = new UnifiedRandom(projectile.whoAmI);

            List<(float, float)> rots = new List<(float, float)>();

            for (float f = 0; f < 1f; f += 0.05f)
            {
                rots.Add(((f + (Main.GlobalTime* rando.NextFloat(0.75f, 2f))) % 1f, rando.NextFloat(MathHelper.TwoPi) + (Main.GlobalTime * rando.NextFloat(-1f, 1f) * 0.01f)));
            }

            rots = rots.OrderBy(testby => testby.Item1).ToList();

            foreach ((float, float) rotter in rots)
            {
                Color clorsz = Skylight.SkyLightColors[Owner.SGAPly().skylightLightInfused.Item2] * MathHelper.Clamp(projectile.ai[1] / 30f, 0f, Math.Min(projectile.timeLeft / 30f, 1f));
                spriteBatch.Draw(glow, Vector2.Lerp(spots[0], spots[1], 1f-rotter.Item1) - Main.screenPosition, null, clorsz*MathHelper.Clamp(5f-(rotter.Item1*5f),0f,Math.Min((rotter.Item1 * 5f),1f)), rotter.Item2, glow.Size() / 2f, rotter.Item1, default, 0);
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];

            Vector2[] spots = { projectile.Center, SGAUtils.SunPosition()+Main.screenPosition};
            List<Vector2> trailPos = new List<Vector2>(spots);

            TrailHelper trail = new TrailHelper("BasicEffectAlphaPass", ModContent.GetTexture("SGAmod/TrailEffect"));
            //trail.projsize = projectile.Hitbox.Size() / 2f;
            trail.coordOffset = new Vector2(0, Main.GlobalTime * 8f);
            trail.coordMultiplier = new Vector2(1f, 3f);
            trail.trailThickness = 8;
            trail.trailThicknessIncrease = 12;
            trail.perspective = false;
            trail.strength = MathHelper.Clamp(projectile.Opacity, 0f, 1f) * 3f;
            trail.color = delegate (float percent)
            {
                return Skylight.SkyLightColors[Owner.SGAPly().skylightLightInfused.Item2] * MathHelper.Clamp(projectile.ai[1]/30f,0f,Math.Min(projectile.timeLeft/30f,1f));
            };
            trail.DrawTrail(trailPos.ToList(), projectile.Center);

            return false;

        }
    }

        public class SkylightSlashProj : ModProjectile,IDrawAdditive, ITrueMeleeProjectile
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
            return Math.Abs(projectile.timeLeft-TimeToHitWindow.Item2)<TimeToHitWindow.Item1;
        }

        Player Owner => Main.player[projectile.owner];
        SGAPlayer Sgaply => Owner.SGAPly();

        float TimeLeft => 100f;
        (int,int) TimeToHitWindow => (15,55);

        float BuffTimer => MathHelper.Clamp((Sgaply.skylightLightInfused.Item1 / (float)Skylight.MaxSkylight) * 5f, 0f, 1f);

        Vector3 TrueLocation
        {
            get
            {
                Matrix swingAround = Matrix.CreateFromYawPitchRoll((-MathHelper.PiOver2*1.80f)+(MathHelper.Pi*(projectile.localAI[0]/ TimeLeft))*1.50f,0,0);

                float rangeboost = 1f;
                if (Sgaply.skylightLightInfused.Item2 == 1)
                    rangeboost = 1f+(BuffTimer * 1.00f);
                if (Sgaply.skylightLightInfused.Item2 == 2)
                    rangeboost = 1f + (1.50f*(BuffTimer * (1f-(Owner.statLife/(float)Owner.statLifeMax2))));


                Matrix swingUpOrDown = swingAround*Matrix.CreateFromYawPitchRoll(0, projectile.ai[0], 0)*Matrix.CreateScale(new Vector3((projectile.velocity.Length()/1f)*rangeboost, 2f,1f));

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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Sgaply.skylightLightInfused.Item1 > 0)
            {
                if (Sgaply.skylightLightInfused.Item2 == 0)
                {
                    target.AddBuff(BuffID.Daybreak, (int)(200f * BuffTimer));
                    target.AddBuff(ModContent.BuffType<LavaBurn>(), (int)(200f * BuffTimer));
                }
                if (Sgaply.skylightLightInfused.Item2 == 1)
                {
                    target.AddBuff(ModContent.BuffType<MoonLightCurse>(), (int)(200f * BuffTimer));
                }
                if (Sgaply.skylightLightInfused.Item2 == 2)
                {
                    Projectile projectile2 = new Projectile();
                    projectile2.Center = target.Center;
                    projectile2.owner = projectile.owner;
                    projectile2.vampireHeal((int)(80), target.Center);
                }

            }
        }


        public override void AI()
        {
            projectile.localAI[0]++;

            if (projectile.ai[0] == 0)
            {
                projectile.ai[0] = Main.rand.NextFloat(-MathHelper.Pi * 0.25f, MathHelper.Pi * 0.25f);
                projectile.netUpdate = true;

                Sgaply.skylightLightInfused.Item1 = Math.Max(Sgaply.skylightLightInfused.Item1- Skylight.SkylightCostPerSlash, 0);

                for(int i = 0; i < oldPos.Length; i++)
                {
                    oldPos[i] = TrueLocation-Vector3.Normalize(new Vector3(projectile.velocity.X, projectile.velocity.Y,0) * i*320f);
                }
            }

            if (projectile.timeLeft == 50)
            {
                Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 5, 0.25f, -0.85f);
            }

            if (skyhere == default && Main.netMode != NetmodeID.Server)
            {
                Color otherColor = Skylight.SkyLightColors[Sgaply.skylightLightInfused.Item2];


                float colorz = Main.rand.NextFloat();
                color = Color.Lerp(Main.hslToRgb(colorz, 0.60f, 0.80f),otherColor, BuffTimer);
                color2 = Main.hslToRgb((colorz + 0.10f) % 1f, 0.40f, 0.90f);
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