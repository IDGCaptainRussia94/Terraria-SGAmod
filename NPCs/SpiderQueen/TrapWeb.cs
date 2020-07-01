using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.NPCs.SpiderQueen
{
    public class TrapWeb : ModProjectile
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Trapping Web");
		}
		
		public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;      
            projectile.friendly = false;
			projectile.hostile = true;
			projectile.penetrate = 10;
			projectile.light = 0.5f;
            projectile.width = 24;
            projectile.height = 24;
            projectile.tileCollide=true;
            projectile.penetrate = 1;
        }

        public override string Texture
        {
            get { return ("Terraria/Item_"+ItemID.Cobweb); }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;
            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                Main.PlaySound(SoundID.Item10,projectile.Center);
                if (projectile.velocity.X != oldVelocity.X)
                {
                    projectile.velocity.X = -oldVelocity.X;
                }
                if (projectile.velocity.Y != oldVelocity.Y)
                {
                    projectile.velocity.Y = -oldVelocity.Y;
                }
            }
            return false;
        }

	    public override bool PreKill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item45, projectile.Center);

            int i = (int)projectile.Center.X/16;
            int j = (int)projectile.Center.Y/16;
            for (int x = -8; x <= 8; x++)
            {
                for (int y = -8; y <= 8; y++)
                {
                    //WorldGen.Convert(i + x, j + y, 0, 0);
                    Tile tile = Main.tile[i + x, j + y];
                    if (tile.type==0 && Main.rand.NextFloat(0,(new Vector2(x, y)).Length()) < 1)
                    {
                        tile.type = TileID.Cobweb;
                        tile.active(true);
                        NetMessage.SendTileRange(Main.myPlayer, i + x, j + y, 1, 1);
                    }
                }
            }

            for (int num315 = 0; num315 < 80; num315 = num315 + 1)
            {
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                randomcircle *= Main.rand.NextFloat(0f, 6f);
                int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y), projectile.width, projectile.height, DustID.Web, 0, 0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), projectile.scale * 2f);
                Main.dust[num316].noGravity = false;
                Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y);
            }

            return true;
        }

		
		public override void AI()
        {

            projectile.rotation = ((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f)+MathHelper.ToRadians(90);

            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height,DustID.Web, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1.5f);
            Main.dust[DustID2].noGravity = true;
		}
    }
}