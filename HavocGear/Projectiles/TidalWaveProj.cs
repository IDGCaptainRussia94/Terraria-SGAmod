using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary.Bases;

namespace SGAmod.HavocGear.Projectiles
{
    public class TidalWaveProj : ProjectileSpearBase
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Tidal Wave");
        }

        public override void SetDefaults()
        {
            projectile.width = 30;
            projectile.height = 30;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.scale = 1.2f;
            projectile.aiStyle = 19;
            projectile.melee = true;
            projectile.timeLeft = 90;
            projectile.hide = true;

            movein=0.8f;
            moveout=0.2f;
            thrustspeed=3f;
        }

        public override void MakeProjectile()
        {
            Vector2 center=new Vector2(projectile.position.X+(float)(projectile.width / 2),projectile.position.Y+(float)(projectile.width / 2));
                Vector2 launchvector=new Vector2((float)(Math.Cos(truedirection)),(float)(Math.Sin(truedirection)));
                int num54 = Projectile.NewProjectile(center.X+launchvector.X*42,center.Y+launchvector.Y*42, launchvector.X*8, launchvector.Y*8, mod.ProjectileType("TidalWaveProj2"), 1, 0f);
                Main.projectile[num54].damage=(int)(projectile.damage);
                Main.projectile[num54].owner=projectile.owner;
        }


    }

}