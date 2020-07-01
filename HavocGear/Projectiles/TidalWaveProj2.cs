using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
    public class TidalWaveProj2 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Water");
        }

        public override void SetDefaults()
        {
            projectile.width = 10;
            projectile.height = 20;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            projectile.penetrate = 1;
            projectile.ownerHitCheck = true;
            projectile.aiStyle = 0;
            projectile.melee = true;
            projectile.timeLeft = 60;
        }

        public override void AI()
        {
        if (Main.rand.Next(0,30)<20){
        int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = projectile.velocity*0.4f;
        projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;  
          }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
                Main.dust[dust].scale = 1.25f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= 3f;
            }
        }
    }
}