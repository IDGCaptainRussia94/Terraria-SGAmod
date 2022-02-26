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
using Microsoft.Xna.Framework.Audio;
using static SGAmod.SGAUtils;


namespace SGAmod.Items.Weapons.Aurora
{

    public class SkyLightSword : ModItem, IAuroraItem
    {

        public static Color[] SkyLightColors => new Color[]{Color.Orange,Color.Cyan,Color.Red,Color.OrangeRed };
        public static int MaxSkylight => 8000;
        public static int SkylightChargeRate => 30;
        public static int SkylightCostPerSlash => 50;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skylight");
            Tooltip.SetDefault("'Rend and Sunder all with the fury of the heavens!'\nPrimary channels energy from the celestial body in the sky\nOnce channeling, you cannot cancel for a short time\nYou can only channel when your energy is depleted\nLaunches celestial slashes that apply stacking damage over time");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostUpdateEquipsEvent += SkylightPlayerUpdate;
            return true;
        }

        public void SkylightPlayerUpdate(SGAPlayer sgaply)
        {
            //sgaply.skylightLightInfused = (Math.Max(sgaply.skylightLightInfused.Item1 - 1,0), sgaply.skylightLightInfused.Item2);
        }

        public override void SetDefaults()
        {
            item.damage = 320;
            item.crit = 0;
            item.melee = true;
            item.width = 40;
            item.height = 40;
            item.useTime = 6;
            item.useAnimation = 6;
            item.useStyle = 5;
            item.noMelee = true; //so the item's animation doesn't do damage
            item.knockBack = 1;
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
                    float skylightInfuse = MathHelper.Clamp((drawPlayer.SGAPly().skylightLightInfused.Item1 / (float)SkyLightSword.MaxSkylight) * 2f, 0f, 1f);
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
                    float stuff = MathHelper.Clamp((sgaply.skylightLightInfused.Item1 / (float)SkyLightSword.MaxSkylight) * 2f, 0f, 1f);
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
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Yellow, "Infused with the fury of the Sun")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Yellow, "Attacks incinerate foes with Daybroken and Lava Burn")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Yellow, "Improves damage by 50% based on remaining charge")));

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
                if (sgaply.skylightLightInfused.Item2 == 3)
                {
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Orange, "Infused with the dark corona of the Eclipse")));
                    tooltips.Add(new TooltipLine(mod, "SkylightTooltip", Idglib.ColorText(Color.Orange, "Causes stacking damage to greatly increase on hit")));
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
            for (int i = 0; i < 2; i += 1)
            {
                ModRecipe recipe = new ModRecipe(mod);
                recipe.AddIngredient(ItemID.Arkhalis, 1);
                recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
                recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 10);
                recipe.AddIngredient(ModContent.ItemType<MoneySign>(), 12);
                recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 16);
                recipe.AddIngredient(i == 0 ? ItemID.WrathPotion : ItemID.RagePotion, 1);
                recipe.AddTile(ModContent.TileType<LuminousAlter>());
                recipe.SetResult(this);
                recipe.AddRecipe();
            }
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

            if (projectile.timeLeft < 30 && (!player.channel || projectile.ai[0] > 0 || sgaply.skylightLightInfused.Item1>= SkyLightSword.MaxSkylight))
            {
                projectile.ai[0] += 1;
                projectile.netUpdate = true;
            }

            if (projectile.ai[1] == 0)
            {
                sgaply.skylightLightInfused.Item2 = (byte)(Main.eclipse ? 3 : 0);
                if (!Main.dayTime)
                    sgaply.skylightLightInfused.Item2 = (byte)(Main.bloodMoon ? 2 : 1);
            }

            if (projectile.ai[0] < 1)
            {
                sgaply.skylightLightInfused.Item1 = (int)MathHelper.Clamp(sgaply.skylightLightInfused.Item1+ SkyLightSword.SkylightChargeRate, 0, SkyLightSword.MaxSkylight);
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

            foreach ((float, float) rotter in rots.OrderBy(testby => testby.Item1))
            {
                Color clorsz = SkyLightSword.SkyLightColors[Owner.SGAPly().skylightLightInfused.Item2] * MathHelper.Clamp(projectile.ai[1] / 30f, 0f, Math.Min(projectile.timeLeft / 30f, 1f));
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
                return SkyLightSword.SkyLightColors[Owner.SGAPly().skylightLightInfused.Item2] * MathHelper.Clamp(projectile.ai[1]/30f,0f,Math.Min(projectile.timeLeft/30f,1f));
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

        float BuffTimer => MathHelper.Clamp((Sgaply.skylightLightInfused.Item1 / (float)SkyLightSword.MaxSkylight) * 5f, 0f, 1f);

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
            target.SGANPCs().AddDamageStack(projectile.damage / 2, 60 * 5);
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
                if (Sgaply.skylightLightInfused.Item2 == 3)
                {
                    target.SGANPCs().AddDamageStack((int)(projectile.damage * BuffTimer), 60 * 5);
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

                Sgaply.skylightLightInfused.Item1 = Math.Max(Sgaply.skylightLightInfused.Item1- SkyLightSword.SkylightCostPerSlash, 0);

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
                Color otherColor = SkyLightSword.SkyLightColors[Sgaply.skylightLightInfused.Item2];


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

            if (Sgaply.skylightLightInfused.Item2 == 3 && BuffTimer > 0)
            {
                trail = new TrailHelper("BasicEffectDarkPass", ModContent.GetTexture("SGAmod/TrailEffect"));
                //trail.projsize = projectile.Hitbox.Size() / 2f;
                trail.coordOffset = new Vector2(0, Main.GlobalTime * -12f);
                trail.coordMultiplier = new Vector2(1f, 1f);
                trail.trailThickness = 6;
                trail.strengthPow = 2f;
                trail.trailThicknessIncrease = 3;
                trail.perspective = false;
                trail.strength = MathHelper.Clamp(projectile.Opacity, 0f, 1f) * 8f* BuffTimer;
                trail.color = delegate (float percent)
                {
                    return Color.White * MathHelper.Clamp((projectile.localAI[0] - (percent * 60f)) / 40f, 0f, 1f);
                };
                trail.DrawTrail(oldPos.Select(testby => new Vector3(testby.X, testby.Y, testby.Z * 1f)).ToList(), projectile.Center);
            }


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

    public class FireFoxGreatbow : ModItem, IAuroraItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("FireFox Greatbow");
            Tooltip.SetDefault("Launches a pair of Sky Foxes that move in sin wave patterns\nAfter striking an enemy, a trail of arrows are left behind that rapidly seek out that enemy");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.ranged = true;
            item.damage = 750;
            item.width = 40;
            item.height = 40;
            item.useTime = 20;
            item.useAnimation = 20;
            item.channel = false;
            item.autoReuse = true;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<FireFoxProj>();
            item.shootSpeed = 1f;
            item.useAmmo = AmmoID.Arrow;

            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
                {
                    return Color.Transparent;
                };

                item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];

                item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle, Color glowcolor)
                {
                    Player drawPlayer = drawInfo.drawPlayer;
                    Vector2 drawHere = drawPlayer.MountedCenter + ((angle - (MathHelper.PiOver4+(drawPlayer.direction < 0 ? MathHelper.PiOver2 : 0))).ToRotationVector2()) * 16 - Main.screenPosition;
                    Texture2D texture = Main.itemTexture[item.type];

                    DrawData value = new DrawData(texture, drawHere, null, Color.White * 1f,(drawInfo.drawPlayer.direction < 0 ? MathHelper.PiOver2 : 0)- MathHelper.PiOver4 + angle, texture.Size()/2f, 1f, drawInfo.spriteEffects, 0);
                    Main.playerDrawData.Add(value);
                };
            }
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 speed = new Vector2(speedX, speedY);
            var snd = Main.PlaySound(SoundID.DD2_BetsyFireballShot, (int)position.X, (int)position.Y);
            if (snd != null)
            {
                snd.Pitch = 0.75f;
            }
            for (int i = -1; i < 2; i += 2)
            {
                Projectile proj = Projectile.NewProjectileDirect(position, Vector2.Normalize(speed) * 10f, ModContent.ProjectileType<FireFoxProj>(), damage, knockBack, player.whoAmI, i, type);
            }
            return false;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Phantasm, 1);
            recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.MangroveBow>(), 1);
            recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 12);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ItemID.ArcheryPotion, 1);
            recipe.AddTile(ModContent.TileType<LuminousAlter>());
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }

    public class FireFoxProj : ModProjectile
    {
        List<(Vector2,int)> foxTail = new List<(Vector2, int)>();
        float lastrot = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foxbow");
        }
        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.BoneArrow);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.light = 0.5f;
            projectile.width = 64;
            projectile.height = 64;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.extraUpdates = 1;
            projectile.tileCollide = false;
            projectile.penetrate = 999;
            projectile.arrow = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;

            ProjectileID.Sets.TrailCacheLength[projectile.type] = 75;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override bool CanDamage()
        {
            return projectile.penetrate > 998;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            projectile.ai[0] = target.whoAmI+500;
            ShotArrows();
        }

        public override void AI()
        {
            projectile.localAI[0]++;
            projectile.spriteDirection = (projectile.velocity.X == 0 ? 1 : Math.Sign(projectile.velocity.X));
            projectile.rotation = projectile.velocity.ToRotation();

            if (!CanDamage())
            {
                projectile.localAI[1] += 1;
                projectile.Opacity -= 1 / 60f;
            }

            if (projectile.localAI[1]>20 && projectile.localAI[1] % 16 == 0)
            {
                Projectile.NewProjectileDirect(projectile.Center, projectile.velocity.RotatedBy(Main.rand.NextFloat(-MathHelper.PiOver2,MathHelper.PiOver2)*0.60f)*3f, ModContent.ProjectileType<FireFoxArrowProj>(), projectile.damage / 3, projectile.knockBack, projectile.owner, projectile.ai[0]-500, projectile.ai[1]);
            }

            //if ((int)projectile.localAI[0] % 3 == 0)
            //{
            //List<NPC> targets = SGAUtils.ClosestEnemies(projectile.Center, 860);

            //if (targets != null && targets.Count > 0)
            //{
            //NPC totarget = targets[0];
            //Vector2 diff = totarget.Center - projectile.Center;

            if (Math.Abs(projectile.ai[0]) < 2)
            {
                float speed = projectile.velocity.Length();

                float speeder = (float)Math.Sin((projectile.localAI[0] + MathHelper.PiOver2) / 17f) * projectile.ai[0];
                projectile.Center += Vector2.Normalize(projectile.velocity).RotatedBy(MathHelper.PiOver2) * speeder * 6f;
                projectile.rotation += speeder / 2f;

            }

            /*for (int i = 0; i < 32; i += 8)
            {
                Vector2 randomcircle = Main.rand.NextVector2Circular(i + 8, i + 8);
                int num655 = Dust.NewDust(projectile.Center + randomcircle, 0, 0, DustID.AncientLight, 0,0, 150, Main.hslToRgb(Main.rand.NextFloat()%1f,1f,0.5f)*1f, 0.32f);
                Main.dust[num655].noGravity = true;
            }*/

            //projectile.velocity = projectile.velocity.ToRotation().AngleTowards(diff.ToRotation(), 0.025f * projectile.Opacity).ToRotationVector2() * speed;
            //}
            //}

            projectile.Opacity *= MathHelper.Clamp(projectile.timeLeft / 150f, -1f, 1f);

            lastrot = projectile.rotation;

            foxTail.Add(((projectile.Center) + lastrot.ToRotationVector2() * -6f, 12));
            foxTail = foxTail.Select(testby => (testby.Item1, testby.Item2 - 1)).Where(testby => testby.Item2 > 0).ToList();

            if (projectile.Opacity <= 0.01f)
            {
                projectile.timeLeft = (int)Math.Min(projectile.timeLeft, 2);
            }
        }

        public void ShotArrows()
        {
            if (projectile.penetrate < 500)
                return;

            projectile.penetrate = 400;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            for (int i = 0; i < projectile.oldPos.Length; i++)
            {
                if (projectile.oldPos[i] == default)
                {
                    projectile.oldPos[i] = projectile.Center-Vector2.Normalize(projectile.velocity)*i;
                }
            }

            TrailHelper trail = new TrailHelper("FadedRainbowEffectPass", ModContent.GetTexture("SGAmod/TiledPerlin"));
            trail.coordMultiplier = new Vector2(0.25f, 1f);// projectile.velocity.Length());
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
            trail.trailThickness = 16;
            trail.projsize = projectile.Hitbox.Size() / 2f;
            trail.strengthPow = 1.25f;
            trail.strength = 1.25f * projectile.Opacity;
            trail.trailThicknessIncrease = 8;
            trail.capsize = new Vector2(6, 6);

            trail.rainbowCoordOffset = new Vector2((Main.GlobalTime + projectile.whoAmI) * 0.05f, Main.GlobalTime * -1.25f);
            trail.rainbowCoordMultiplier = new Vector2(0.5f, 1.25f);
            trail.rainbowColor = new Vector3((Main.GlobalTime+(projectile.whoAmI*7.173f)) / 3f, 0.32f, 0.75f);
            trail.rainbowTexture = SGAmod.PearlIceBackground;// SGAmod.Instance.GetTexture("Voronoi");

            trail.color = delegate (float percent)
            {
                Color traillengthcol = Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent);
                return Color.Lerp(Color.White, Color.White, percent);
            };
            trail.DrawTrail(projectile.oldPos.Select(testby => Vector2.Normalize((projectile.velocity)*8f)+testby).ToList());



            TrailHelper foxtailtrail = new TrailHelper("FadedRainbowEffectPass", ModContent.GetTexture("SGAmod/Stain"));
            foxtailtrail.coordMultiplier = new Vector2(0.25f, 1f);// projectile.velocity.Length());
            foxtailtrail.coordOffset = new Vector2(0,0);
            foxtailtrail.strengthPow = 2.25f;
            foxtailtrail.strength = 4.00f * MathHelper.Clamp(projectile.Opacity*2f,0f,1f);
            foxtailtrail.doFade = false;

            foxtailtrail.rainbowCoordOffset = new Vector2(0, Main.GlobalTime * -0.360f);
            foxtailtrail.rainbowCoordMultiplier = new Vector2(0.05f, 0.25f);
            foxtailtrail.rainbowColor = new Vector3((Main.GlobalTime + (projectile.whoAmI * 7.173f)) / 3f, 0.250f, 0.75f);
            foxtailtrail.rainbowTexture = ModContent.GetTexture("SGAmod/TiledPerlin");

            foxtailtrail.color = delegate (float percent)
            {
                float percet = (((percent/2f) + projectile.whoAmI*0.73983f) - (Main.GlobalTime/5f));
                return Main.hslToRgb((10000+percet) % 1f,1f,0.75f) * (1f-MathHelper.Clamp(percent*1f,0f,1f));
            };
            foxtailtrail.trailThicknessFunction = delegate (float percent)
            {
                float scaler = (MathHelper.Clamp(percent * 2f, 0f, 1f))*6f;
                //scaler *= MathHelper.Clamp(percent * 12f, 0f, 1f);
                return scaler;
            };

            foxtailtrail.DrawTrail(foxTail.Select(testby => testby.Item1+((lastrot + MathHelper.PiOver2).ToRotationVector2() * 0f)).Reverse().ToList());



            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect hallowed = SGAmod.HallowedEffect;

            Texture2D tex = Main.projectileTexture[projectile.type];

            hallowed.Parameters["prismAlpha"].SetValue(1f);
            hallowed.Parameters["overlayTexture"].SetValue(SGAmod.PearlIceBackground);
            hallowed.Parameters["overlayAlpha"].SetValue(0.20f);
            hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1.5f, 0.15f, 0f));
            hallowed.Parameters["overlayMinAlpha"].SetValue(0f);

            hallowed.Parameters["alpha"].SetValue(1f*projectile.Opacity);
            hallowed.Parameters["prismColor"].SetValue(Color.White.ToVector3());
            hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, (-Main.GlobalTime+projectile.whoAmI) / 1f, (Main.GlobalTime+ projectile.whoAmI*7.31f
                ) / 4f));
            hallowed.Parameters["rainbowScale"].SetValue(0.765f);
            hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.2f, 0.2f));
            hallowed.CurrentTechnique.Passes["Prism"].Apply();

            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation + MathHelper.PiOver2 - (projectile.spriteDirection * MathHelper.PiOver2),new Vector2(tex.Width/2f, tex.Height/2f), projectile.scale, projectile.spriteDirection>0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(2f);
            SGAmod.FadeInEffect.Parameters["alpha"].SetValue(projectile.Opacity*0.25f);
            SGAmod.FadeInEffect.CurrentTechnique.Passes["ColorToAlphaPass"].Apply();

            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation + MathHelper.PiOver2 - (projectile.spriteDirection * MathHelper.PiOver2), new Vector2(tex.Width / 2f, tex.Height / 2f), projectile.scale*1.2f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            return false;
        }


    }

    public class FireFoxArrowProj : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Foxbow Arrow");
        }
        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.BoneArrow);
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.light = 0.5f;
            projectile.width = 64;
            projectile.height = 64;
            projectile.ranged = true;
            projectile.timeLeft = 300;
            projectile.extraUpdates = 0;
            projectile.tileCollide = false;
            projectile.penetrate = 1;
            projectile.arrow = true;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;

            ProjectileID.Sets.TrailCacheLength[projectile.type] = 75;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override string Texture => "Terraria/Projectile_"+ProjectileID.CrystalVileShardHead;

        public override bool CanDamage()
        {
            return false;// projectile.localAI[0]>60;
        }

        public override bool PreKill(int timeLeft)
        {
            return true;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            //nil
        }

        public override void AI()
        {
            projectile.localAI[0]++;

            projectile.rotation = MathHelper.Clamp(projectile.localAI[0]/1.25f,0f,MathHelper.TwoPi*5f) +projectile.velocity.ToRotation();

            if (projectile.localAI[0] < 40 || projectile.timeLeft<=30)
            {
                projectile.velocity *= 0.94f;
            }
            else
            {
                NPC totarget = Main.npc[(int)projectile.ai[0]];
                bool exists = totarget != null && totarget.active && totarget.life > 0;

                if (!exists)
                {
                    /*List<NPC> targets = SGAUtils.ClosestEnemies(projectile.Center, 240);
                    if (targets!= null && targets.Count > 0)
                    {
                        totarget = targets[0];
                        projectile.ai[0] = totarget.whoAmI;
                    }*/
                    projectile.timeLeft = 30;
                }

                    if (exists)
                    {
                        projectile.velocity = Vector2.Normalize(projectile.velocity) * MathHelper.Clamp(projectile.velocity.Length() + 0.5f, 0f, 32);
                    Vector2 diff = totarget.Center - projectile.Center;
                    float speed = projectile.velocity.Length();
                        projectile.velocity = projectile.velocity.ToRotation().AngleTowards(diff.ToRotation(), (0.01f+(speed/150f)) * projectile.Opacity).ToRotationVector2()* speed;

                    float dot = Vector2.Dot(Vector2.Normalize(diff), Vector2.Normalize(projectile.velocity));
                    if (projectile.timeLeft > 30 && Vector2.Dot(Vector2.Normalize(diff), Vector2.Normalize(projectile.velocity))>0.99f)
                    {
                        Projectile.NewProjectileDirect(projectile.Center, Vector2.Normalize(projectile.velocity)*64f, (int)projectile.ai[1], projectile.damage,projectile.knockBack, projectile.owner);
                        projectile.timeLeft = 0;
                    }

                }
            }

            projectile.Opacity = MathHelper.Clamp(projectile.timeLeft / 30f, 0f, Math.Min(projectile.localAI[0]/10f,1f));
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];
            Texture2D tex2 = Main.itemTexture[ItemID.JestersArrow];

            spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation-MathHelper.PiOver2, tex2.Size() / 2f, projectile.scale * 1f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * projectile.Opacity, projectile.rotation, tex.Size() / 2f, projectile.scale * 0.5f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);


            return false;
        }


    }


    public class PolarityHalberd : ModItem, IAuroraItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polarity Halberd");
            Tooltip.SetDefault("'Balance the Sun and Moon in a dance of the blade'\nLand hits in Melee mode to build up combos, decays over time without hitting enemies\nGain more attack, life regen, and max HP the higher your combo is\nMissing an attack resets the combo\nAlt fire to swap between combos and beams");
            Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
        }

        public override void SetDefaults()
        {
            item.melee = true;
            item.damage = 300;
            item.crit = 25;
            item.width = 40;
            item.height = 40;
            item.useTime = 10;
            item.useAnimation = 10;
            item.channel = false;
            item.autoReuse = false;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 6;
            item.value = Item.sellPrice(0, 10, 0, 0);
            item.rare = ItemRarityID.Cyan;
            item.noUseGraphic = true;
            item.shoot = ProjectileID.ConfettiMelee;
            item.shootSpeed = 4f;
            if (!Main.dedServ)
            {
                item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
                {
                    return Color.Transparent;
                };

                item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];

                item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle, Color glowcolor)
                {
                    //nothing
                };
            }
        }

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<PolarityHalberdProj>()]<1)
            Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<PolarityHalberdProj>(),0,0,player.whoAmI);
        }

        public override bool AltFunctionUse(Player player)
        {
            return true;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            foreach(Projectile proj in Main.projectile.Where(testby => testby.type == ModContent.ProjectileType<PolarityHalberdProj>()))
            {
                (proj.modProjectile as PolarityHalberdProj).PlayerAttack(player.altFunctionUse == 2 ? true : false,damage,knockBack,new Vector2(speedX, speedY));
            }
            return false;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            add += (1f/player.meleeSpeed)+(player.SGAPly().PolarityHarbPower.Item1 / 1000f);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.NorthPole, 1);
            recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 12);
            recipe.AddIngredient(ItemID.LunarBar, 8);
            recipe.AddIngredient(ItemID.NightOwlPotion, 1);
            recipe.AddTile(ModContent.TileType<LuminousAlter>());
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.MonkStaffT3, 1);
            recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 8);
            recipe.AddIngredient(ItemID.LunarBar, 6);
            recipe.AddIngredient(ItemID.NightOwlPotion, 1);
            recipe.AddTile(ModContent.TileType<LuminousAlter>());
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }

    public class PolarityHalberdProj : ModProjectile,ITrueMeleeProjectile
    {
        float trailLength = 1f;
        float trailAlpha = 1f;
        int comboState = 0;
        int comboHits = 0;
        int weaponMode = 0;
        float weaponModeAngle = 0;
        float weaponModeAngleTo = 0;
        bool canHit = false;
        int fadeSpeed = 1;
        float alpha = 0f;
        Vector2 velocity = Vector2.Zero;
        Vector2 comboPosition = Vector2.Zero;
        public List<(Vector2, float, int,bool)> oldFrontSwings = new List<(Vector2, float, int,bool goon)>();

        public override bool Autoload(ref string name)
        {
            SGAPlayer.PostUpdateEquipsEvent += SGAPlayer_PostUpdateEquipsEvent;
            return true;
        }

        private void SGAPlayer_PostUpdateEquipsEvent(SGAPlayer player)
        {
            //Main.NewText(""player.player.meleeSpeed);
            player.PolarityHarbPower.Item1 = Math.Min(player.PolarityHarbPower.Item1, 1000);
            player.PolarityHarbPower.Item2 -= 1;
            if (player.PolarityHarbPower.Item2 < 1)
            {
                player.PolarityHarbPower.Item1 = Math.Max(player.PolarityHarbPower.Item1 - 1, 0);
            }

            if (player.PolarityHarbPower.Item1 > 0)
            {
                player.player.lifeRegen += (int)(player.PolarityHarbPower.Item1 / 20f);
                player.player.statLifeMax2 += (int)(player.PolarityHarbPower.Item1 / 6f);
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Polarity Halberd Using");
        }
        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.light = 0.5f;
            projectile.width = 64;
            projectile.height = 64;
            projectile.melee = true;
            projectile.timeLeft = 300;
            projectile.extraUpdates = 0;
            projectile.tileCollide = false;
            projectile.penetrate = -1;

            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 1500;

            ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override string Texture
        {
            get { return ("SGAmod/Items/Weapons/Aurora/PolarityHalberd"); }
        }

        public void PlayerAttack(bool altfire,int damage, float knockback,Vector2 velocity)
        {
            Player player = Main.player[projectile.owner];
            projectile.damage = damage;
            projectile.knockBack = knockback;
            SwapAttack(altfire ? 1002 : 100, velocity);
        }

        public void SwapAttack(int type,Vector2 velocity = default)
        {
            Player player = Main.player[projectile.owner];
            bool doit = false;

            if (weaponMode % 2 == 0)
            {
                if (type == 100)
                {
                    type = 1+ comboState % 3;
                    doit = true;
                }
            }

            if (projectile.ai[0] > 0 && projectile.ai[0]<10 && type == 0 && comboHits < 1 && player.SGAPly().PolarityHarbPower.Item1>0)
            {
                CombatText.NewText(new Rectangle((int)player.position.X, (int)player.position.Y, player.width, player.height),Color.Aqua, "Combo Broken!", true, true);
                player.SGAPly().PolarityHarbPower.Item1 = 0;
            }

            if ((projectile.ai[0] < 1 || (type < 50 && comboHits > 0)) || type == 0)
            {
                comboPosition = comboHits < 1 ? player.MountedCenter : projectile.Center;
                projectile.ai[0] = type;
                projectile.ai[1] = 0;
                comboHits = 0;
                if (velocity != default)
                this.velocity = velocity;

                if (doit)
                {
                    for (int i = 0; i < projectile.localNPCImmunity.Length; i += 1)
                    {
                        projectile.localNPCImmunity[i] = 0;
                    }

                    oldFrontSwings = oldFrontSwings.Select(testby => (testby.Item1, testby.Item2, testby.Item3,true)).ToList();

                    comboState += 1;
                    SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_SkyDragonsFurySwing, (int)projectile.Center.X, (int)projectile.Center.Y);
                    if (sound != null)
                    {
                        sound.Pitch = 0.80f;
                    }

                }

            }
        }

        Player Player => Main.player[projectile.owner];

        public void AttackStates()
        {
            Player player = Player;


            //if ((projectile.ai[0] == 0 && projectile.ai[1] >= 20) || projectile.ai[0] >= 1000)
                comboPosition = Player.MountedCenter;

            projectile.ai[1] += 1;
            if (projectile.ai[0] < 1)
            {
                return;
            }

            if (velocity != default)
            player.ChangeDir(Math.Sign(velocity.X));

            if (projectile.ai[0] <20)
            {
                if (projectile.ai[0] == 1)
                ComboJab();
                if (projectile.ai[0] == 2)
                    ComboUpperSwing();
                if (projectile.ai[0] == 3)
                    ComboOverheadSwing();



            }

            if (projectile.ai[0] == 100)
            {
                SkyFuryStyleSwing();
            }

            if (projectile.ai[0] == 1000)
            {
                Intro();
            }

            if (projectile.ai[0] == 1001)
            {
                Outro();
            }

            if (projectile.ai[0] == 1002)
            {
                SwapWeaponMode();
            }
        }

        public void SwapWeaponMode()
        {
            Player player = Player;
            if (projectile.ai[1] == 1)
            {
                weaponMode += 1;
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_MonkStaffSwing, (int)projectile.Center.X, (int)projectile.Center.Y);
                if (sound != null)
                {
                    sound.Pitch = 0.75f;
                }
                weaponModeAngleTo = (weaponModeAngleTo+MathHelper.Pi)% MathHelper.TwoPi;
            }

            projectile.rotation = projectile.rotation.AngleLerp(velocity.ToRotation(), 0.05f);

            projectile.Center = player.MountedCenter + projectile.rotation.ToRotationVector2() * 72f;

            weaponModeAngle = weaponModeAngle.AngleLerp(weaponModeAngleTo, (float)Math.Sin((projectile.ai[1]*(MathHelper.Pi))/20f)*0.20f);
            if (projectile.ai[1] >= 20)
            {
                weaponModeAngle = weaponModeAngleTo;
                SwapAttack(0);
            }
        }

        public void ComboOverheadSwing()//Wide overhead Swing, does melee damage
        {
            Player player = Player;

            float side = 1f;
            if (player.direction < 0)
                side = -1f;

            Vector2[] points = new Vector2[] {
           comboPosition + ((Vector2.Normalize(velocity) * 96f).RotatedBy(side * MathHelper.TwoPi * -0.45f)),
            comboPosition + ((Vector2.Normalize(velocity) * 184f).RotatedBy(side * MathHelper.TwoPi * -0.175f)),
            comboPosition + (Vector2.Normalize(velocity) * 420f),
           comboPosition + ((Vector2.Normalize(velocity) * 184f).RotatedBy(side * MathHelper.TwoPi * 0.065f)),
            comboPosition + ((Vector2.Normalize(velocity) * 184f).RotatedBy(side * MathHelper.TwoPi * 0.105f)),
            comboPosition + ((Vector2.Normalize(velocity) * 184f).RotatedBy(side * MathHelper.TwoPi * 0.205f))
        };

            BezierCurveTuring curve = new BezierCurveTuring(points);

            if (projectile.ai[1] > 5)
                canHit = true;

            Vector2 movefinalloccurve = curve.BezierCurve(MathHelper.Clamp((projectile.ai[1] - 7f) / 12f, 0f, 1f));

            float mover = MathHelper.Clamp(projectile.ai[1] / 5f, 0f, 1f);

            Vector2 movefinalloc = Vector2.Lerp(comboPosition + (Vector2.Normalize(velocity) * 72f), movefinalloccurve, mover);

            projectile.velocity = projectile.rotation.ToRotationVector2() * 0.05f;

            projectile.Center = Vector2.Lerp(projectile.Center, movefinalloc, MathHelper.Clamp(projectile.ai[1] / 6f, 0f, 1f));

            projectile.rotation = projectile.rotation.AngleLerp((projectile.Center - player.Center).ToRotation(), MathHelper.Clamp(projectile.ai[1] / 10f, 0f, 1f));

            fadeSpeed = 0;

            trailAlpha = MathHelper.Clamp((float)Math.Sin((projectile.ai[1] / 20f) * MathHelper.Pi) * 1.0f, 0f, 1f);

            if (projectile.ai[1] > 20)
            {
                SwapAttack(0);
                projectile.ai[1] = 10;
            }
        }


        public void ComboUpperSwing()//Short under Swing, does melee damage
        {
            Player player = Player;

            float side = 1f;
            if (player.direction < 0)
                side = -1f;

            Vector2[] points = new Vector2[]
            {
            comboPosition + ((Vector2.Normalize(velocity) * 72f).RotatedBy(side * MathHelper.TwoPi * 0.25f)),
            comboPosition + (Vector2.Normalize(velocity) * 340f),
            comboPosition + ((Vector2.Normalize(velocity) * 240f).RotatedBy(side * MathHelper.TwoPi * -0.15f)),
           comboPosition + ((Vector2.Normalize(velocity) * 120f).RotatedBy(side * MathHelper.TwoPi * -0.25f))
            };

            BezierCurveTuring curve = new BezierCurveTuring(points);

            if (projectile.ai[1] > 5)
                canHit = true;

            Vector2 movefinalloccurve = curve.BezierCurve(MathHelper.Clamp((projectile.ai[1] - 10f) / 20f, 0f, 1f));

            float mover = MathHelper.Clamp(projectile.ai[1] / 5f, 0f, 1f);

            Vector2 movefinalloc = Vector2.Lerp(comboPosition + (Vector2.Normalize(velocity) * 72f), movefinalloccurve, mover);

            projectile.velocity = projectile.rotation.ToRotationVector2() * 0.05f;

            projectile.Center = Vector2.Lerp(projectile.Center, movefinalloc, MathHelper.Clamp(projectile.ai[1] / 6f, 0f, 1f));

            projectile.rotation = projectile.rotation.AngleLerp((projectile.Center - player.Center).ToRotation(), MathHelper.Clamp(projectile.ai[1] / 10f, 0f, 1f));

            fadeSpeed = 0;

            trailAlpha = MathHelper.Clamp((float)Math.Sin((projectile.ai[1] / 20f) * MathHelper.Pi) * 1.00f, 0f, 1f);

            if (projectile.ai[1] > 20)
            {
                SwapAttack(0);
                projectile.ai[1] = 10;
            }
        }

        public void ComboJab()//Jab, does melee damage
        {
            Player player = Player;

            float side = 1f;
            if (player.direction < 0)
                side = -1f;

            Vector2 movehere1 = comboPosition + ((Vector2.Normalize(velocity) * 84f).RotatedBy(side * MathHelper.TwoPi * -0.125f));// + (Vector2.Normalize(perp) * -32f*side);
            Vector2 movehere2 = comboPosition + (Vector2.Normalize(velocity) * 640f);
            Vector2 movehere3 = comboPosition + ((Vector2.Normalize(velocity) * 8f).RotatedBy(side * MathHelper.TwoPi * 0.125f));// + (Vector2.Normalize(perp) * 160f*side);

            BezierCurveTuring curve = new BezierCurveTuring(new Vector2[] { movehere1, movehere2, movehere3, movehere3 });

            if (projectile.ai[1]>5)
            canHit = true;

            Vector2 movefinalloccurve = curve.BezierCurve(MathHelper.Clamp((projectile.ai[1] - 10f) / 20f, 0f, 1f));

            float mover = MathHelper.Clamp(projectile.ai[1] / 5f, 0f, 1f);

            Vector2 movefinalloc = Vector2.Lerp(comboPosition + (Vector2.Normalize(velocity) * 72f), movefinalloccurve, mover);

            projectile.velocity = projectile.rotation.ToRotationVector2() * 0.05f;

            projectile.Center = Vector2.Lerp(projectile.Center, movefinalloc, MathHelper.Clamp(projectile.ai[1] / 6f, 0f, 1f));

            projectile.rotation = projectile.rotation.AngleLerp((projectile.Center - player.Center).ToRotation(), MathHelper.Clamp(projectile.ai[1] / 10f, 0f, 1f));

            fadeSpeed = 0;

            trailAlpha = MathHelper.Clamp((float)Math.Sin((projectile.ai[1] / 20f) * MathHelper.Pi) * 1.00f, 0f, 1f);

            if (projectile.ai[1] > 20)
            {
                SwapAttack(0);
                projectile.ai[1] = 10;
            }
        }

        public void SkyFuryStyleSwing()//Sky Dragon Fury-like alt swing; spawns lasers, does no melee damage
        {
            Player player = Player;

            float side = 1f;
            if (player.direction < 0)
                side = -1f;

            Vector2 perp = velocity.RotatedBy(MathHelper.PiOver2);
            Vector2 movehere1 = player.MountedCenter + ((Vector2.Normalize(velocity) * 72f).RotatedBy(side * MathHelper.TwoPi * -0.125f));// + (Vector2.Normalize(perp) * -32f*side);
            Vector2 movehere2 = player.MountedCenter + (Vector2.Normalize(velocity) * 160f);
            Vector2 movehere3 = player.MountedCenter + ((Vector2.Normalize(velocity) * 72f).RotatedBy(side * MathHelper.TwoPi * 0.125f));// + (Vector2.Normalize(perp) * 160f*side);

            BezierCurveTuring curve = new BezierCurveTuring(new Vector2[] { movehere1, movehere2, movehere3, movehere3 });

            Vector2 movefinalloccurve = curve.BezierCurve(MathHelper.Clamp((projectile.ai[1] - 10f) / 40f, 0f, 1f));

            float mover = MathHelper.Clamp(projectile.ai[1] / 5f, 0f, 1f);

            //mover = mover*MathHelper.Clamp(5f - (projectile.ai[1] / 15f), 0f, 1f);

            Vector2 movefinalloc = Vector2.Lerp(player.MountedCenter + (Vector2.Normalize(velocity) * 72f), movefinalloccurve, mover);

            projectile.Center = Vector2.Lerp(projectile.Center, movefinalloc, MathHelper.Clamp(projectile.ai[1] / 30f, 0f, 1f));

            projectile.rotation = projectile.rotation.AngleLerp((projectile.Center - player.Center).ToRotation(), MathHelper.Clamp(projectile.ai[1] / 20f, 0f, 1f));

            fadeSpeed = 0;

            if ((projectile.ai[1] + 5) % 5 == 0 && projectile.ai[1] > 5 && projectile.ai[1] < 40)
            {
                Projectile.NewProjectile(projectile.Center, projectile.rotation.ToRotationVector2(), ModContent.ProjectileType<AuraBeamStikeProj>(), projectile.damage/4, projectile.knockBack, projectile.owner);
            }

            trailAlpha = MathHelper.Clamp((float)Math.Sin((projectile.ai[1] / 40f) * MathHelper.Pi) * 1.5f, 0f, 1f);

            if (projectile.ai[1] > 40)
            {
                SwapAttack(0);
                projectile.ai[1] = 10;
            }
        }

        public void Intro()
        {
            Player player = Player;
            Vector2 sun = SGAUtils.SunPosition() + Main.screenPosition;

            if (projectile.ai[1] < 2)
            {
                projectile.Center = sun;
                projectile.scale = 0;
            }

            projectile.scale = MathHelper.Clamp(Vector2.SmoothStep(new Vector2(0, 0), new Vector2(1f, 0), MathHelper.Clamp(projectile.ai[1] / 30f, 0f, 1f)).X, 0f, 1f);

            float percent = MathHelper.Clamp(projectile.ai[1] / 120f, 0f, 1f);

            float percent2 = MathHelper.Clamp(projectile.ai[1] / 100f, 0f, 1f);

            float dist = Vector2.SmoothStep(new Vector2(0, 0), new Vector2(percent, 0), 1f - MathHelper.Clamp(percent, 0f, 1f)).X * 1600f;

            Vector2 playerpos = player.MountedCenter + (projectile.rotation.ToRotationVector2() * (dist + 64f));

            //Vector2 movefinalloc = Vector2.Lerp(sun+ (-projectile.rotation.ToRotationVector2() * (64f)), playerpos, percent);

            alpha = percent2;

            projectile.Center = Vector2.SmoothStep(sun, Vector2.Lerp(projectile.Center, playerpos, percent), percent);

            projectile.rotation += 0.05f + ((1f - percent) * 0.15f);

            trailAlpha = MathHelper.Clamp((float)Math.Sin((projectile.ai[1] / 100f) * MathHelper.Pi) * 2.5f, 0f, 1f);

            if (projectile.ai[1] >= 110)
            {
                SwapAttack(0);
            }
        }

        public void Outro()
        {
            Player player = Player;
            Vector2 sun = SGAUtils.SunPosition() + Main.screenPosition;

                float percent = MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);
                float percent3 = 1f - MathHelper.Clamp((projectile.timeLeft - 20f) / 40f, 0f, 1f);
                float percent2 = 1f - MathHelper.Clamp(projectile.timeLeft / 160f, 0f, 1f);

                alpha = percent;

                projectile.scale = MathHelper.Clamp(Vector2.SmoothStep(new Vector2(0, 0), new Vector2(1f, 0), percent).X, 0f, 1f);

                Vector2 playerpos = (projectile.rotation.ToRotationVector2() * (24f));

                projectile.Center = Vector2.Lerp(projectile.Center, Vector2.SmoothStep(projectile.Center + playerpos, sun, percent3),percent);

                projectile.rotation = projectile.rotation.AngleLerp(-MathHelper.PiOver2, percent2);

                trailAlpha /= 2f;

                if (projectile.ai[1] >= 120)
                {
                    projectile.Kill();
                }
        }

        public override bool? CanHitNPC(NPC target)
        {
            if (!canHit)
                return false;

            return base.CanHitNPC(target);
        }


        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_MonkStaffGroundImpact, (int)projectile.Center.X, (int)projectile.Center.Y);
            if (sound != null)
            {
                sound.Pitch = MathHelper.Clamp(0.75f- comboHits/5f,0f,0.80f);
            }
            Player.SGAPly().PolarityHarbPower.Item1 += (int)(60/ (1f + comboHits));
            Player.SGAPly().PolarityHarbPower.Item2 = Math.Max(Player.SGAPly().PolarityHarbPower.Item2, 100 + comboHits * 20);
            if (SGAmod.ScreenShake<16)
            SGAmod.AddScreenShake(16/(1f+ comboHits*2f),540,projectile.Center);

            comboHits += 1;
        }

        public override bool PreKill(int timeLeft)
        {
            return true;
        }

        public override void AI()
        {

            Player player = Player;
            bool hasSword = player.active && !player.dead && player.HeldItem.type == ModContent.ItemType<PolarityHalberd>();
            projectile.localAI[0] += 1;


            if (projectile.localAI[0] == 1)
            {
                SwapAttack(1000);
            }

            //if (!hasSword)
            //{
            //    return;
            //}

            if (projectile.timeLeft < 85)
            {
                if (projectile.timeLeft > 80)
                {
                    projectile.timeLeft = 80;
                    SwapAttack(1001);
                }
            }

            projectile.velocity *= 0.90f;


            if ((hasSword || projectile.ai[0]!=0) && projectile.ai[0] != 1001)
            projectile.timeLeft = 87;

            player.heldProj = projectile.whoAmI;
            fadeSpeed = 1;

            canHit = false;

            AttackStates();

            if (projectile.ai[0] < 1)
            {
                trailAlpha /= 2f;
                trailAlpha = MathHelper.Clamp(trailAlpha - 0.1f, 0f, 1f);

                projectile.Center = Vector2.Lerp(projectile.Center,player.MountedCenter + Vector2.UnitX.RotatedBy(projectile.rotation) * 72f, MathHelper.Clamp(projectile.ai[1]/30f,0f,1f));
                projectile.Center += player.velocity;

                //comboPosition = player.MountedCenter;

                projectile.rotation += MathHelper.Clamp(projectile.ai[1] / 30f, 0f,1f)*0.05f*player.direction;

            }

            Vector2 pos = projectile.Center;

            if (trailAlpha <= 0)
            {
                oldFrontSwings.Clear();
            }

            bool canmakeone = true;
            if (oldFrontSwings.Count > 0)
            {
                canmakeone = false;
                Vector2 lastpos = oldFrontSwings[0].Item1;
                if (((lastpos - projectile.Center).LengthSquared()) > 0)
                {
                    canmakeone = true;
                }
            }

            if (trailAlpha > 0)
            {
                if (oldFrontSwings.Count < 1 || canmakeone)
                {
                    oldFrontSwings.Insert(0, (pos, projectile.rotation, 40,false));
                }
            }

            oldFrontSwings = oldFrontSwings.Select(testby => (testby.Item1, testby.Item2, testby.Item3 - (testby.Item4 ? 4 : fadeSpeed), testby.Item4)).Where(testby => testby.Item3 > 0).ToList();

            if (oldFrontSwings.Count < 1)
                return;

                trailLength = 0;
            Vector2 previous = oldFrontSwings[0].Item1;
            foreach (Vector2 posx in oldFrontSwings.Select(testby => testby.Item1))
            {
                if (previous != posx)
                {
                    trailLength += (posx - previous).Length();
                    previous = posx;
                }
            }
        }

        public float LaserAlpha
        {
            get
            {
                float alpha2 = MathHelper.Clamp(projectile.localAI[0] / 20f, 0f, MathHelper.Clamp((projectile.timeLeft-60)/20f, 0f,1f));
                return alpha2;
            }

        }

        float BaseRot => projectile.rotation + weaponModeAngle;
        Vector2 FromBasePosition
        {
            get
            {
                Vector2 here = projectile.Center;

                return here;
            }
        }
        Vector2 FromPosition
        {
            get
            {
                Vector2 here = FromBasePosition.RotatedBy(BaseRot- projectile.rotation, Player.MountedCenter);

                return here;
            }
        }
        Vector2 OtherPosition
        {
            get
            {
                Vector2 here = FromBasePosition.RotatedBy(BaseRot- projectile.rotation + MathHelper.Pi, Player.MountedCenter);

                return here;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            //TrailHelper trail.DrawTrail(oldFrontSwings.Select(testby => new Vector3(testby.Item1.X, testby.Item1.Y, 0)).ToList(), projectile.Center);

            TrailHelper trail = new TrailHelper("FadedBasicEffectPass", SGAmod.PearlIceBackground);
            trail.coordMultiplier = new Vector2(0.25f, 1f);// projectile.velocity.Length());
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
            trail.trailThickness = 12;
            trail.strengthPow = 2f;
            trail.strength = trailAlpha * alpha*0.750f;
            trail.trailThicknessIncrease = 4;

            trail.color = delegate (float percent)
            {
                Color traillengthcol = Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent);
                float trax = 1f / trailLength;
                return Color.Lerp(Color.White, Color.White, percent);
            };

            trail.DrawTrail(oldFrontSwings.Select(testby => new Vector3(testby.Item1.X, testby.Item1.Y, 0)).ToList(), projectile.Center);


            trail = new TrailHelper("FadedRainbowEffectPass", SGAmod.Instance.GetTexture("TiledPerlin"));
            trail.coordMultiplier = new Vector2(0.25f, 1f);// projectile.velocity.Length());
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
            trail.trailThickness = 12;
            trail.strength = trailAlpha*0.50f* alpha;
            trail.trailThicknessIncrease = 6;

            trail.color = delegate (float percent)
            {
                Color traillengthcol = Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent);
                return Color.Lerp(Color.White, Color.White, percent);
            };

            trail.rainbowCoordOffset = new Vector2(Main.GlobalTime*0.05f, Main.GlobalTime * -1.25f);
            trail.rainbowCoordMultiplier = new Vector2(0.5f, 1.25f);
            trail.rainbowColor = new Vector3(Main.GlobalTime / 3f, 1f, 0.75f);
            trail.rainbowTexture = SGAmod.Instance.GetTexture("TiledPerlin");

            trail.DrawTrail(oldFrontSwings.Select(testby => new Vector3(testby.Item1.X, testby.Item1.Y, 0)).ToList(), projectile.Center);


            trail = new TrailHelper("FadedBasicEffectPass", SGAmod.Instance.GetTexture("SmallLaser"));
            trail.coordMultiplier = new Vector2(1f, 1f);// projectile.velocity.Length());
            trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
            trail.trailThickness = 12;
            trail.strengthPow = 2f;
            trail.strength = trailAlpha * 2.5f * alpha;
            trail.trailThicknessIncrease = 4;

            trail.color = delegate (float percent)
            {
                Color traillengthcol = Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent);
                return Color.Lerp(Color.White, Color.White, percent);
            };

            trail.DrawTrail(oldFrontSwings.Select(testby => new Vector3(testby.Item1.X, testby.Item1.Y, 0)).ToList(), projectile.Center);


            if (LaserAlpha >= 0)
            {
                List<Vector2> points = new List<Vector2>() { FromPosition + Vector2.Normalize(FromPosition - OtherPosition) * 12f, FromPosition, OtherPosition, OtherPosition + Vector2.Normalize(OtherPosition - FromPosition) * 12f };

                float dotprod = Vector2.Dot((BaseRot).ToRotationVector2(), projectile.rotation.ToRotationVector2());
                float dotprod2 = Vector2.Dot((BaseRot + MathHelper.Pi).ToRotationVector2(), projectile.rotation.ToRotationVector2());
                float[] alphaDOT = new float[] { (0.60f + dotprod * 0.40f) * alpha, (0.60f + dotprod2 * 0.40f) * alpha };

                for (int i = 0; i < 2; i += 1)
                {

                    TrailHelper connect = new TrailHelper("BasicEffectAlphaPass", SGAmod.Instance.GetTexture("SmallLaser"));
                    connect.coordMultiplier = new Vector2(1f, 1.25f);// projectile.velocity.Length());
                    connect.coordOffset = new Vector2(Main.rand.NextFloat(-1f, 1f) * 0.025f, Main.GlobalTime * -1f);
                    connect.trailThickness = 24;
                    connect.strengthPow = 2f;
                    connect.strength = 1.5f * alpha * LaserAlpha * alphaDOT[i];
                    connect.doFade = false;

                    connect.color = delegate (float percent)
                    {
                        Color traillengthcol = Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent);
                        return Color.Lerp(Color.Transparent, Color.White, MathHelper.Clamp((float)Math.Sin(percent * MathHelper.Pi) * 4f, 0f, 1f)) * (i > 0 ? percent : (1 - percent));
                    };

                    connect.DrawTrail(points, projectile.Center);
                }
            }

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = Main.projectileTexture[projectile.type];

            Vector2 offset = new Vector2(tex.Width * 0.80f, tex.Height * 0.20f);
            Player player = Player;

            float dotprod = Vector2.Dot((BaseRot).ToRotationVector2(), projectile.rotation.ToRotationVector2());
            float dotprod2 = Vector2.Dot((BaseRot + MathHelper.Pi).ToRotationVector2(), projectile.rotation.ToRotationVector2());
            float alphaDOT = (0.60f + dotprod * 0.40f) * alpha;
            float alphaDOT2 = (0.60f + dotprod2 * 0.40f) * alpha;

            //Moon
            spriteBatch.Draw(tex, FromPosition - Main.screenPosition, null, Color.White * 0.80f * alphaDOT, BaseRot + MathHelper.PiOver4, offset, projectile.scale, SpriteEffects.None, 0);

            //Sun
            spriteBatch.Draw(tex, OtherPosition - Main.screenPosition, null, Color.White * 0.80f * alphaDOT2, BaseRot + MathHelper.Pi + MathHelper.PiOver4, offset, projectile.scale, SpriteEffects.None, 0);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Effect hallowed = SGAmod.HallowedEffect;

            hallowed.Parameters["prismAlpha"].SetValue(1f * alphaDOT);
            hallowed.Parameters["overlayTexture"].SetValue(SGAmod.PearlIceBackground);
            hallowed.Parameters["overlayAlpha"].SetValue(0.5f * alphaDOT);
            hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1.5f, 0.15f, 0f));
            hallowed.Parameters["overlayMinAlpha"].SetValue(0f);

            hallowed.Parameters["alpha"].SetValue(0.5f * alphaDOT);
            hallowed.Parameters["prismColor"].SetValue(Color.White.ToVector3());
            hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -Main.GlobalTime / 1f, (Main.GlobalTime) / 4f));
            hallowed.Parameters["rainbowScale"].SetValue(1.265f);
            hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.2f, 0.2f));
            hallowed.CurrentTechnique.Passes["Prism"].Apply();

            spriteBatch.Draw(tex, FromPosition - Main.screenPosition, null, Color.White * 0.80f, BaseRot + MathHelper.PiOver4, offset, projectile.scale, SpriteEffects.None, 0);

            hallowed.Parameters["overlayAlpha"].SetValue(0.5f * alphaDOT2);
            hallowed.Parameters["alpha"].SetValue(0.5f * alphaDOT2);
            hallowed.CurrentTechnique.Passes["Prism"].Apply();

            spriteBatch.Draw(tex, OtherPosition - Main.screenPosition, null, Color.White * 0.80f, BaseRot + MathHelper.Pi + MathHelper.PiOver4, offset, projectile.scale, SpriteEffects.None, 0);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


        }

    }

    public class AuraBeamStikeProj : ModProjectile
    {

        int ExplodeTime => 20;
        public int timer = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Auroric Fury");
        }

        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.Starfury);
            aiType = -1;
            projectile.aiStyle = -1;
            projectile.timeLeft = 40;
            projectile.extraUpdates = 2;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.width = 16;
            projectile.height = 16;
            projectile.hide = false;
            projectile.extraUpdates = 0;
            projectile.penetrate = -1;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = -1;
        }

        public override string Texture
        {
            get { return "Terraria/Misc/NebulaSky/Beam"; }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            if (timer < ExplodeTime)
            {
                return false;
            }
            return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center,
                projectile.Center + Vector2.Normalize(projectile.velocity) * 1800);
        }

        public override void AI()
        {
            if (timer < 1)
            {
                Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2 half = new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2(projectile.Center.X, projectile.Center.Y);
                projectile.localAI[0] = ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
            }

            if (timer == ExplodeTime)
            {
                SoundEffectInstance sound = Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 52);
                if (sound != null)
                {
                    sound.Pitch = 0.75f;
                }
            }

            projectile.position -= projectile.velocity;
            timer += 1;
        }
        public override void DrawBehind(int index, List<int> drawCacheProjsBehindNPCsAndTiles, List<int> drawCacheProjsBehindNPCs, List<int> drawCacheProjsBehindProjectiles, List<int> drawCacheProjsOverWiresUI)
        {
            //drawCacheProjsBehindNPCs.Add(index);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {

            Texture2D tex = Main.projectileTexture[projectile.type];

            Vector2 fromposition = new Microsoft.Xna.Framework.Graphics.PackedVector.HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(projectile.localAI[0]) }.ToVector2();

            float trailalpha = MathHelper.Clamp(projectile.timeLeft / 30f, 0f, 1f);

            Color warningcolor = Color.White * trailalpha;

            float spread = (timer / 20f);

            float explodeSize = MathHelper.Clamp((timer - ExplodeTime) / 6f, 0.10f, 1.20f) * trailalpha;

            List<float> sploderEffect = new List<float>();
            for (int i = 0; i < 10; i += 1)
            {
                sploderEffect.Add(((i / 10f) + (Main.GlobalTime * 3f)) % 1f);
            }

            sploderEffect = sploderEffect.OrderBy(testby => testby).ToList();

            foreach (float splodsize in sploderEffect)
            {
                spriteBatch.Draw(tex, fromposition - Main.screenPosition, null, warningcolor * 0.40f, projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2f, tex.Height), new Vector2(explodeSize * splodsize, spread), SpriteEffects.None, 0);
            }

            spriteBatch.Draw(tex, fromposition - Main.screenPosition, null, warningcolor * 0.80f, projectile.velocity.ToRotation() + MathHelper.PiOver2, new Vector2(tex.Width / 2f, tex.Height), new Vector2(0.05f, spread), SpriteEffects.None, 0);

            Texture2D glowStar = mod.GetTexture("Extra_57b");
            Vector2 glowSize = glowStar.Size();

            UnifiedRandom random = new UnifiedRandom(projectile.whoAmI);

            float alphaIK = MathHelper.Clamp(timer / 10f, 0f, 1f) * trailalpha;
            float trailsize2 = MathHelper.Clamp(projectile.timeLeft / 5f, 0f, 1f);


            for (float ff = 1f; ff > 0.25f; ff -= 0.05f)
            {
                Color color = Main.hslToRgb(random.NextFloat(1f), 0.65f, 0.85f);
                float rot = random.NextFloat(0.05f, 0.15f) * (random.NextBool() ? 1f : -1f)*(Main.GlobalTime*8f);
                spriteBatch.Draw(glowStar, fromposition - Main.screenPosition, null, color* alphaIK * 0.75f, random.NextFloat(MathHelper.TwoPi)+ rot, glowSize / 2f, (new Vector2(random.NextFloat(0.15f,0.50f), explodeSize)+new Vector2(ff, ff))* trailsize2*0.75f, SpriteEffects.None, 0);
            }

            return false;
        }

    }


    public class SkyeHook : ModItem, IAuroraItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Oranos Skye Hook");
            Tooltip.SetDefault("'Don't get lied to about wanting the moon again'\nGrab the sun/moon and flail it towards your mouse\nMoon does more damage/knockback, Sun hits a larger area\nRequires you to be on the surface to attack\nWear as a grapple hook to pull yourself towards the sun/moon\nDoesn't reset wing time");
        }

        public override void SetDefaults()
        {
            item.CloneDefaults(ItemID.AmethystHook);
            item.noUseGraphic = true;
            item.damage = 2000;
            item.Throwing().thrown = true;
            item.knockBack = 7f;
            item.useStyle = 5;
            //item.shootSpeed = 10f;
            //item.shoot = 230;
            item.width = 18;
            item.rare = ItemRarityID.Cyan;
            item.height = 28;
            item.UseSound = SoundID.Item15;
            item.useAnimation = 200;
            item.useTime = 200;
            item.rare = 1;
            item.noMelee = true;
            item.value = 20000;

            item.shootSpeed = 18f; // how quickly the hook is shot.
            item.shoot = ModContent.ProjectileType<SkyeHookProjectile>();
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<SkyeHookProjectile>()] < 1 && !Consumables.AcidicEgg.Underground(player);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position, new Vector2(speedX, speedY)*0f, ModContent.ProjectileType<SkyeHookProjectile>(), damage, knockBack,player.whoAmI,ai1: 1);
            return false;
            //return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.LunarHook, 1);
            recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
            recipe.AddIngredient(ModContent.ItemType<OverseenCrystal>(), 20);
            recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 12);
            recipe.AddTile(ModContent.TileType<LuminousAlter>());
            recipe.SetResult(this);
            recipe.AddRecipe();

        }
    }

    public class SkyeHookProjectile : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Skye Hook");
        }

        public override string Texture => "Terraria/Projectile_"+ProjectileID.GemHookDiamond;

        public override void SetDefaults()
        {
            /*	this.netImportant = true;
				this.name = "Gem Hook";
				this.width = 18;
				this.height = 18;
				this.aiStyle = 7;
				this.friendly = true;
				this.penetrate = -1;
				this.tileCollide = false;
				this.timeLeft *= 10;
			*/
            projectile.CloneDefaults(ProjectileID.GemHookAmethyst);
            projectile.damage = 0;
            projectile.Throwing().thrown = true;
        }

        // Use this hook for hooks that can have multiple hooks mid-flight: Dual Hook, Web Slinger, Fish Hook, Static Hook, Lunar Hook
        public override bool? CanUseGrapple(Player player)
        {
            int hooksOut = 0;
            for (int l = 0; l < 1000; l++)
            {
                if (Main.projectile[l].active && Main.projectile[l].owner == Main.myPlayer && Main.projectile[l].type == projectile.type)
                {
                    hooksOut++;
                }
            }
            if (hooksOut > 2) // This hook can have 3 hooks out.
            {
                return false;
            }
            return true;
        }

        public override bool? SingleGrappleHook(Player player)
        {
        	return true;
        }

        public override void UseGrapple(Player player, ref int type)
        {

        }

        public override float GrappleRange()
        {
            return 200f;
        }

        public override void NumGrappleHooks(Player player, ref int numHooks)
        {
            numHooks = 1;
        }

        // default is 11, Lunar is 24
        public override void GrappleRetreatSpeed(Player player, ref float speed)
        {
            speed = 14f;
        }

        public override void GrapplePullSpeed(Player player, ref float speed)
        {
            speed = 12;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreAI()
        {

            projectile.position -= projectile.velocity;
            Vector2 sunmoonpos = Main.screenPosition+SGAUtils.SunPosition();
            Player player = Main.player[projectile.owner];
            Vector2 diff = sunmoonpos - projectile.Center;
            Vector2 diff2 = sunmoonpos - player.Center;
            Vector2 diff3 = Main.MouseWorld - sunmoonpos;
            float speed = 24f;

            if (projectile.ai[1] == 0)
            {
                projectile.velocity = Vector2.Zero;
                if (diff.Length() > 24 && projectile.ai[0] == 0)
                {
                    projectile.Center += Vector2.Normalize(diff) * 32;
                    if (diff.Length() < 96)
                        projectile.ai[0] = 2f;

                }
                else
                {
                    if (projectile.ai[0] != 1)
                    {
                        if (projectile.ai[0] < 3)
                        {
                            var snd = Main.PlaySound(SoundID.DD2_BallistaTowerShot, (int)projectile.Center.X, (int)projectile.Center.Y);
                            if (snd != null)
                            {
                                snd.Pitch = 0.90f;
                            }
                        }
                        projectile.Center = sunmoonpos - Vector2.Normalize(diff2) * 24f;
                        projectile.ai[0] = 3f;
                        float speedx = MathHelper.Clamp((diff2.Length() - 96) / 120f, 0f, 1f);
                        Vector2 tilecol = Collision.TileCollision(player.position, Vector2.Normalize(diff2) * speed * speedx, player.width, player.height, gravDir: (int)player.gravDir);
                        //player.Center += tilecol / 2;
                        player.velocity = tilecol;
                        if (player.controlJump)
                        {
                            projectile.Kill();
                            player.velocity = tilecol / 2f;
                        }
                        //Main.player[projectile.owner].grappling[Main.player[projectile.owner].grapCount] = projectile.whoAmI;
                    }
                }
                projectile.rotation = diff2.ToRotation();
            }
            else
            {
                player.itemTime = 5;
                player.itemAnimation = 5;
                if (projectile.ai[0] == 0)
                {
                    projectile.Center += Vector2.Normalize(diff) * 32;
                    if (diff.Length() < 96)
                    {
                        projectile.ai[0] = 5;
                        projectile.rotation = diff3.ToRotation();
                        projectile.localAI[1] = projectile.rotation;
                        projectile.ai[1] = Main.dayTime ? 1 : 2;
                        var snd = Main.PlaySound(SoundID.DD2_BallistaTowerShot, (int)projectile.Center.X, (int)projectile.Center.Y);
                        if (snd != null)
                        {
                            snd.Pitch = 0.90f;
                        }
                        projectile.netUpdate = true;
                    }

                    projectile.rotation = diff2.ToRotation();
                }
                if (projectile.ai[0] == 5)
                {

                    projectile.localAI[0] += 1;

                    float percent = projectile.localAI[0] / 60f;
                    float percent2 = percent * percent;

                    //float percent3 = MathHelper.Clamp(2f-projectile.localAI[0] / 60f,0f,1f);
                    //float percent4 = percent3 * percent3;

                    float sizer = (float)Math.Sin((percent / 120f) * MathHelper.Pi);

                    projectile.rotation = projectile.rotation.AngleTowards(diff3.ToRotation(), 0.20f * percent2);

                    Vector2 pastPos = projectile.Center;
                    Vector2 gotox = sunmoonpos + projectile.rotation.ToRotationVector2() * (percent2 * diff3.Length());
                    projectile.Center = gotox;

                    if (percent2 > 0.30f)
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Normalize(gotox), ModContent.ProjectileType<HookSunDamage>(), (int)(projectile.damage*(projectile.ai[1] == 2 ? 1.5f : 1f)), projectile.knockBack * (projectile.ai[1] == 2 ? 1.5f : 1f), projectile.owner, MathHelper.Clamp(percent2, 0f, 1f) * (360f * (projectile.ai[1] == 2 ? 0.75f : 1f)));
                        if (SGAmod.ScreenShake < 12f)
                            SGAmod.AddScreenShake(MathHelper.Clamp(percent2, 0f, 1f) * 6f, 960, projectile.Center);
                    }

                    if (percent2 >= 1f)
                    {
                        projectile.ai[0] = 6;
                        projectile.netUpdate = true;
                        Vector2 velo = (gotox - pastPos);
                        projectile.velocity = (Vector2.Normalize(velo) * (MathHelper.Clamp(velo.Length(), 24f, 64f) * 0.45f));
                        projectile.localAI[0] = 0;

                        var snd = Main.PlaySound(SoundID.DD2_BetsyFireballImpact, (int)projectile.Center.X, (int)projectile.Center.Y);
                        if (snd != null)
                        {
                            snd.Pitch = -0.75f;
                        }
                        SGAmod.AddScreenShake(24f, 800, projectile.Center);
                    }
                }

                if (projectile.ai[0] == 6)
                {

                    projectile.localAI[0] += 1;
                    projectile.velocity *= 0.999f;
                    projectile.position += projectile.velocity;

                    float percent = projectile.localAI[0] / 60f;
                    float percent2 = percent * percent;

                    projectile.rotation = (projectile.Center - player.Center).ToRotation();

                    Vector2 gotox = sunmoonpos;// + projectile.rotation.ToRotationVector2() * (percent2 * diff3.Length());
                    projectile.Center += (gotox- projectile.Center)* MathHelper.SmoothStep(0f,1f, percent2);

                    if (percent2 < 0.60f)
                    {
                        Projectile.NewProjectile(projectile.Center, Vector2.Normalize(gotox), ModContent.ProjectileType<HookSunDamage>(), (int)(projectile.damage * (projectile.ai[1] == 2 ? 1.5f : 1f)), projectile.knockBack * (projectile.ai[1] == 2 ? 1.5f : 1f), projectile.owner, MathHelper.Clamp(percent2, 0f, 1f) * (360f * (projectile.ai[1] == 2 ? 0.75f : 1f)));
                        if (SGAmod.ScreenShake < 12f)
                            SGAmod.AddScreenShake(MathHelper.Clamp(percent2, 0f, 1f) * 6f, 960, projectile.Center);
                    }

                    if (percent2 >= 1f)
                    {
                        projectile.ai[0] = 7;
                        projectile.netUpdate = true;
                        projectile.localAI[0] = 0;
                    }
                }


                if (projectile.ai[0] == 7)
                {
                    Vector2 topos = player.MountedCenter - projectile.Center;
                    projectile.localAI[0] += 12;
                    projectile.Center += Vector2.Normalize(topos) * MathHelper.Clamp(projectile.localAI[0] / 180f, 0f, 1f) * 64f;
                    if (topos.Length() < 72)
                    {
                        projectile.Kill();
                    }
                }

            }

            return false;
        }

        public override bool PreDrawExtras(SpriteBatch spriteBatch)
        {
            Player player = Main.player[projectile.owner];
            TrailHelper trail = new TrailHelper("FadedRainbowEffectPass", ModContent.GetTexture("SGAmod/SmallLaser"));
            trail.coordMultiplier = new Vector2(1f, 3f);// projectile.velocity.Length());
            trail.coordOffset = new Vector2(0, Main.GlobalTime * 1f);
            trail.trailThickness = 6;
            //trail.projsize = projectile.Hitbox.Size() / 2f;
            trail.strength = 1f * projectile.Opacity;
            trail.doFade = false;

            trail.rainbowCoordOffset = new Vector2((Main.GlobalTime + projectile.whoAmI) * 0.05f, Main.GlobalTime * -1.25f);
            trail.rainbowCoordMultiplier = new Vector2(0.5f, 1.25f);
            trail.rainbowColor = new Vector3((Main.GlobalTime + (projectile.whoAmI * 7.173f)) / 3f, 0.5f, 0.75f);
            trail.rainbowTexture = SGAmod.PearlIceBackground;

            trail.color = delegate (float percent)
            {
                Color traillengthcol = Color.Lerp(Color.CornflowerBlue, Color.CadetBlue, percent);
                return Color.Lerp(Color.White, Color.White, percent);
            };
            Vector2[] origin = new Vector2[] {projectile.Center, player.MountedCenter};
            trail.DrawTrail(origin.ToList());

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation+MathHelper.PiOver2, tex.Size()/2f, projectile.scale * 1f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

            if (projectile.ai[0] == 5 || projectile.ai[0] == 6)
            {
                float percent = projectile.localAI[0] / 60f;
                percent = percent * percent;

                if (projectile.ai[0] == 6)
                    percent = 1f - percent;

                //float sizer = (float)Math.Sin((percent / 120f) * MathHelper.Pi);

                Texture2D sun = Main.sunTexture;
                Texture2D moon = Main.moonTexture[Main.moonType];

                if (projectile.ai[1] == 1)
                {
                    float sunalpha = MathHelper.Clamp(percent * 8f, 0f, 1f);
                    float percent2 = percent * percent;

                    spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(percent * 8f, 0f, 1f) * MathHelper.Clamp(2f - (percent2*2f),0f,1f), 0, sun.Size() / 2f, projectile.scale * 1f + (percent * 7f), SpriteEffects.None, 0); ;

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                    Effect RadialEffect = SGAmod.RadialEffect;

                    RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Stain"));
                    RadialEffect.Parameters["alpha"].SetValue(percent2*2f);
                    RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.375f, -Main.GlobalTime * 0.275f));
                    RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(4f, 0.25f));
                    RadialEffect.Parameters["ringScale"].SetValue(0.44f);
                    RadialEffect.Parameters["ringOffset"].SetValue(0.22f);
                    RadialEffect.Parameters["ringColor"].SetValue(Color.Orange.ToVector3());
                    RadialEffect.Parameters["tunnel"].SetValue(false);

                    RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

                    float percent3 = percent2;// MathHelper.SmoothStep(0f,1f,MathHelper.Clamp(percent2*2f,0f,1f));

                    spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(percent2 * 4f, 0f, 1f), 0, sun.Size() / 2f, projectile.scale * 1f + (percent3 * 15f), SpriteEffects.None, 0);

                    RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Stain"));
                    RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0.50f+ Main.GlobalTime * 0.375f, -Main.GlobalTime * 0.275f));
                    RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(6f, 0.5f));
                    RadialEffect.Parameters["ringScale"].SetValue(0.26f);
                    RadialEffect.Parameters["ringOffset"].SetValue(0.32f);
                    RadialEffect.Parameters["ringColor"].SetValue(Color.Yellow.ToVector3());
                    RadialEffect.Parameters["alpha"].SetValue(percent2*0.75f);

                    RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

                    spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(percent2 * 4f, 0f, 1f), 0, sun.Size() / 2f, projectile.scale * 1f + (percent3 * 15f), SpriteEffects.None, 0);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


                    Texture2D noise = ModContent.GetTexture("SGAmod/TiledPerlin");

                    SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Lerp(Color.White, Color.Orange, (MathHelper.Clamp(percent * 2f, 0f, 1f))).ToVector4() * percent2);
                    SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(Main.blackTileTexture);
                    SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(1f, 1f));
                    SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Main.GlobalTime * 1f, 0f));
                    SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(20f);

                    SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMap"].Apply();

                    spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(percent2 * 4f, 0f, 1f), 0, sun.Size() / 2f, projectile.scale * 1f + (percent2 * 3f), SpriteEffects.None, 0);

                    SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Red.ToVector4() * percent2);
                    SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(noise);
                    SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.5f, 0.5f));
                    SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Main.GlobalTime * 1.25f,0f));

                    SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

                    spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(sunalpha * 4f, 0f, 1f), 0, sun.Size() / 2f, projectile.scale * 1f + (percent2 * 3f), SpriteEffects.None, 0);

                    noise = ModContent.GetTexture("SGAmod/Voronoi");

                    SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Yellow.ToVector4() * percent2*0.75f);
                    SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(noise);
                    SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.75f, 0.75f));
                    SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(-Main.GlobalTime * 1.15f, 0f));

                    SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

                    spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Color.White * MathHelper.Clamp(sunalpha * 4f, 0f, 1f), 0, sun.Size() / 2f, projectile.scale * 1f + (percent2 * 3f), SpriteEffects.None, 0);

                }

                if (projectile.ai[1] == 2)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                    Rectangle rect = new Rectangle(0, 0, moon.Width, moon.Height / 8);
                    spriteBatch.Draw(moon, projectile.Center - Main.screenPosition, rect, Color.White * MathHelper.Clamp(percent * 8f, 0f, 1f), 0, rect.Size() / 2f, projectile.scale * 1f + (percent * 7f), SpriteEffects.None, 0);

                }


            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            return false;
        }
    }

    public class HookSunDamage : ModProjectile
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sun Damage");
        }

        public override string Texture => "Terraria/Item_" + ItemID.SugarCookie;

        public sealed override void SetDefaults()
        {
            projectile.width = 72;
            projectile.height = 72;
            projectile.tileCollide = false;
            projectile.friendly = true;
            projectile.Throwing().thrown = true;
            // Needed so the minion doesn't despawn on collision with enemies or tiles
            projectile.penetrate = -1;
            projectile.knockBack = 8;
            projectile.timeLeft = 2;
            projectile.hide = true;
            projectile.usesIDStaticNPCImmunity = true;
            projectile.idStaticNPCHitCooldown = 60;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            return (targetHitbox.Center.ToVector2() - projectile.Center).LengthSquared() < projectile.ai[0]* projectile.ai[0];
        }

        // Here you can decide if your minion breaks things like grass or pots
        public override bool? CanCutTiles()
        {
            return false;
        }

        // This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
        public override bool MinionContactDamage()
        {
            return false;
        }
    }

}
