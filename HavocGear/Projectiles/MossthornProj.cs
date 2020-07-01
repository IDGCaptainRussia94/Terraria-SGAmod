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
    public class MossthornProj : ProjectileSpearBase
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mossthorn");
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

            movein=3f;
            moveout=3f;
            thrustspeed=3.0f;
        }

        public new float movementFactor
        {
            get { return projectile.ai[0]; }
            set { projectile.ai[0] = value; }
        }
    }
}