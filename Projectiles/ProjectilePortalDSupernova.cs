using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.Projectiles
{
    public class ProjectilePortalDSupernova : ProjectilePortal
    {
        public int counterfire=0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nova");
        }


        public virtual int projectilerate => 35;
        public virtual float angleoffset => 0;
        public virtual int manacost => 4;
        public virtual int startrate => 90;
        public virtual int drainrate => 5;
        public virtual int portalprojectile => mod.ProjectileType("UnmanedBolt");
        public virtual float portaldistfromsword => 60f;
        public virtual float velmulti => 8f;

        private int ogdamage = 0;

        public override void SetDefaults()
        {
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 10;
            projectile.light = 0.5f;
            projectile.width = 24;
            projectile.height = 24;
            projectile.tileCollide = false;
            projectile.timeLeft = 90;
        }

        public virtual void WhileFiring()
        {
            //nothin



        }

        public override void AI()
        {

            projectile.ai[0] = portalprojectile;
            base.AI();

            Player player = Main.player[projectile.owner];
            if (player == null || (!player.channel || player.dead || (player.statMana < manacost || (player.HasBuff(BuffID.ManaSickness) && player.buffTime[player.FindBuffIndex(BuffID.ManaSickness)]>60*6))))
            {
                if (projectile.timeLeft > 29)
                {
                    projectile.timeLeft = 29;
                }
            }
            else
            {
                if (counter - takeeffectdelay > 0)
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
                        player.HeldItem.noMelee = true;
                    }
                    int dir = projectile.direction;
                    player.ChangeDir(dir);
                    player.itemTime = 40;
                    player.itemAnimation = 40;
                    player.HeldItem.useStyle = 5;
                    player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir)+(angleoffset* player.direction);
                    projectile.damage=player.GetWeaponDamage(player.HeldItem);

                    WhileFiring();

                    projectile.Center = player.Center + (projectile.velocity * portaldistfromsword);
                    projectile.velocity *= velmulti;


                    if (player.manaRegenDelay<15)
                    player.manaRegenDelay = 15;
                    if (projectile.timeLeft < timeleftfirerate && player.channel)
                    {
                        if (ogdamage == 0)
                        {
                            ogdamage = projectile.damage;
                        }
                        else
                        {
                            projectile.damage = (int)(ogdamage * (1f - player.manaSickReduction));
                        }
                        Main.PlaySound(SoundID.Item20, player.Center);
                        projectile.timeLeft = Math.Max(startrate - (int)counterfire * drainrate, projectilerate);
                        counterfire += 1;
                        player.statMana -= (int)(manacost * player.manaCost);
                        if (player.manaFlower && player.statMana < manacost)
                            player.QuickMana();

                    }
                }
                else
                {
                  if (player == null || (!player.channel || player.dead || player.statMana < manacost))
                  projectile.Kill();

                }
            }


        }




    }


}