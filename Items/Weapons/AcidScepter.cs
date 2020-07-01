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

namespace SGAmod.Items.Weapons
{
	public class AcidScepter : UnmanedPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Scepter");
			Tooltip.SetDefault("Hold the left mouse button to charge up to 10 Acid Venom shots\nRelease to send them flying at the mouse cursor\nHas very slight debuff splash damage");
			Item.staff[item.type] = true; //this makes the useStyle animate as a staff instead of as a gun
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.magic = true;
			item.mana = 5;
			item.width = 40;
			item.height = 40;
			item.useTime = 35;
			item.useAnimation = 35;
            item.channel = true;
			item.useStyle = 5;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 5;
			item.value = 40000;
			item.rare = 3;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("AcidScepterProj");
			item.shootSpeed = 4f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedStaff"), 1);
            recipe.AddIngredient(mod.ItemType("VialofAcid"), 10);
            recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

    public class AcidScepterProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acid Scepter");
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
            projectile.width = 24;
            projectile.height = 24;
            projectile.magic = true;
            projectile.tileCollide = false;
        }

        public override bool? CanHitNPC(NPC target)
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item45, projectile.Center);
            
            return true;
        }

        public override void AI()
        {

            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            Main.dust[DustID2].noGravity = true;
            Player player = Main.player[projectile.owner];
            projectile.ai[1] += 1;
            if (player.dead)
            {
                projectile.Kill();
            }
            else
            {
                if ((projectile.ai[0] > 0 || player.statMana<1) || !player.channel)
                {
                    projectile.ai[0] += 1;
                    if (projectile.ai[0] == 4)
                    {
                        for(int pro = 0; pro < Main.maxProjectiles; pro += 1)
                        {
                            Projectile proj = Main.projectile[pro];
                            if (proj.ai[0] == 0 && proj.type == ModContent.ProjectileType<AcidScepterVenom>() && proj.owner==projectile.owner)
                            {
                                proj.ai[0] = 1;
                                proj.netUpdate = true;
                            }
                        }
                        projectile.Kill();
                    }
                }
                else
                {
                    projectile.timeLeft = 30;
                }
                if (projectile.ai[0] < 3)
                {
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
                    int dir = projectile.direction;
                    player.ChangeDir(dir);
                    player.itemTime = 40;
                    player.itemAnimation = 40;
                    float rotvalue = MathHelper.ToRadians(-90f + (float)Math.Sin(projectile.ai[1] / 10f) * 35f);
                    if (player.manaRegenDelay<15)
                    player.manaRegenDelay = 15;
                    if (projectile.ai[0] > 0)
                    {
                        player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
                    }
                    else
                    {
                        player.itemRotation = MathHelper.ToRadians(-90f*player.direction + (float)Math.Sin(projectile.ai[1] / 10f) * 20f);

                        int counter = 0;

                        for (int pro = 0; pro < Main.maxProjectiles; pro += 1)
                        {
                            Projectile proj = Main.projectile[pro];
                            if (proj.ai[0] == 0 && proj.type == ModContent.ProjectileType<AcidScepterVenom>() && proj.owner == projectile.owner)
                            {
                                counter += 1;
                            }
                        }

                        if (projectile.ai[1] % 20 == 0 && projectile.ai[1]>20 && counter<10)
                        {

                            int thisoned = Projectile.NewProjectile(player.Center.X, player.Center.Y-12, Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f), mod.ProjectileType("AcidScepterVenom"), projectile.damage, projectile.knockBack, Main.myPlayer);
                            IdgProjectile.Sync(thisoned);
                            Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 9, 0.25f, -0.25f);

                            player.statMana -= (int)(5 * player.manaCost);
                            if (player.manaFlower && player.statMana < 5)
                                player.QuickMana();

                        }

                    }

                    projectile.Center = (player.Center + rotvalue.ToRotationVector2() * 26f) + new Vector2(0, -24);
                    projectile.velocity *= 8f;

                }

            }


        }
    }

    public class AcidScepterVenom : SpiderVenom
    {
        private Vector2[] oldPos = new Vector2[6];
        private float[] oldRot = new float[6];
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Acid Blast");
        }

        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 12;
            projectile.height = 12;
            projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
            projectile.hostile = false;
            projectile.friendly = true;
            projectile.tileCollide = true;
            projectile.magic = true;
            projectile.timeLeft = 200;
            projectile.extraUpdates = 5;
            aiType = ProjectileID.Bullet;
        }

        public override void AI()
        {
            if (projectile.ai[0] < 1)
            {
                Player player = Main.player[projectile.owner];
                float rotvalue = MathHelper.ToRadians(-90f + (float)Math.Sin(projectile.ai[1] / 10f) * 20f);
                Vector2 toit = (player.Center + rotvalue.ToRotationVector2() * 26f) + new Vector2(0, -24);


                if ((projectile.Center - toit).Length() > 8)
                {
                    Vector2 norm = toit - projectile.Center;
                    norm.Normalize();
                    projectile.velocity += norm * 0.075f;
                    projectile.velocity *= 0.995f;
                }

                projectile.timeLeft = 200*5;
            }
            else { projectile.ai[0] += 1; }

            if (projectile.ai[0] == 2 || Main.player[projectile.owner].dead)
            {
                Player player = Main.player[projectile.owner];
                Vector2 toit;
                if (Main.player[projectile.owner].dead)
                    toit = projectile.velocity;
                else
                    toit = Main.MouseWorld;
                if (projectile.owner == Main.myPlayer)
                {
                    Vector2 norm = toit- projectile.Center;
                    norm.Normalize();
                    projectile.velocity = norm.RotatedByRandom(MathHelper.ToRadians(25))*9f;
                    projectile.netUpdate = true;

                }

            }


        base.AI();
        }

    }


    }