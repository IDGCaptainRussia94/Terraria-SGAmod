using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
	public class Landslide1 : ModProjectile
	{
        bool abovePlayer;
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.timeLeft = 2000;
            projectile.tileCollide = false;
        }

        public void spinspin()
        {
            if (projectile.ai[0] == 0)
            {
                projectile.ai[0] = Main.rand.NextFloat(-0.2f, 0.2f);
            }
            else
            {
                projectile.rotation += projectile.ai[0];
            }



        }

        public override bool PreKill(int timeLeft)
        {
            for (int num654 = 0; num654 < 10; num654++)
            {
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
                int num655 = Dust.NewDust(projectile.position + Vector2.UnitX * -20f, projectile.width + 40, projectile.height, DustID.t_Lihzahrd, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 3f, 100, new Color(30, 30, 30, 20), 2f);
                Main.dust[num655].noGravity = true;
                Main.dust[num655].velocity *= 0.5f;
            }
            return true;
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Landslide");
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (abovePlayer == true)
            {
                return false;
            }
            return true;
        }

        public override void AI()
        {
            spinspin();
            projectile.velocity.X *= 0.98f;
            projectile.velocity.Y *= 1.00025f;
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] == 0f)
            {
                if (player.position.Y > projectile.position.Y)
                {
                    abovePlayer = true;
                    projectile.tileCollide = false;
                }
                else if (player.position.Y < projectile.position.Y)
                {
                    abovePlayer = false;
                    projectile.tileCollide = true;
                }
            }
        }
	}

    public class Landslide2 : Landslide1
    {
        bool abovePlayer;
        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 2;
            projectile.timeLeft = 1000;
            projectile.tileCollide = false;
        }
    }

    public class Landslide3 : Landslide1
    {
        bool abovePlayer;
        public override void SetDefaults()
        {
            projectile.width = 36;
            projectile.height = 36;
            projectile.aiStyle = -1;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 3;
            projectile.timeLeft = 1000;
        }

        public override void AI()
        {
            spinspin();
            projectile.velocity.X *= 0.95f;
            projectile.velocity.Y *= 1.001f;
            Player player = Main.player[projectile.owner];
            if (projectile.ai[0] == 0f)
            {
                if (player.position.Y > projectile.position.Y)
                {
                    abovePlayer = true;
                    projectile.tileCollide = false;
                }
                else if (player.position.Y < projectile.position.Y)
                {
                    abovePlayer = false;
                    projectile.tileCollide = true;
                }
                /*if (projectile.velocity.Y > 5)
                {
                    projectile.velocity.Y = 5;
                }*/
                if (Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16].active())
                {
                    projectile.alpha = 150;
                }
                else
                {
                    projectile.alpha = 0;
                }
            }
        }
    }
}