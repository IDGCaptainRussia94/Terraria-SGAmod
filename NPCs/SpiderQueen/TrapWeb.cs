using System;
using System.Collections.Generic;
using System.Linq;
using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
                    if (!tile.active() && Main.rand.NextFloat(0,(new Vector2(x, y)).Length()) < 1)
                    {
                        if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
                        {
                            AcidicWebTile.checkForWebRemoval = true;
                            AcidicWebTile.websToRemove.Add(new Point(i + x, j + y));
                        }
                        //tile.type = (ushort)ModContent.TileType<AcidicWebTile>();
                        WorldGen.PlaceTile(i + x, j + y,(ushort)ModContent.TileType<AcidicWebTile>());
                        tile.active(true);
                        NetMessage.SendTileRange(Main.myPlayer, i + x, j + y, 1, 1);
                    }
                }
            }

            if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
            {
                AcidicWebTile.websToRemove = AcidicWebTile.websToRemove.Distinct().ToList();
            }

                for (int num315 = 0; num315 < 80; num315 = num315 + 1)
            {
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                randomcircle *= Main.rand.NextFloat(0f, 6f);
                int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y), projectile.width, projectile.height, DustID.Web, 0, 0, 50, Color.Lime, projectile.scale * 2f);
                Main.dust[num316].noGravity = false;
                Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y);
            }

            return true;
        }

		
		public override void AI()
        {
            projectile.rotation = ((float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f)+MathHelper.ToRadians(90);

            int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height,DustID.Web, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, Color.Lime, 1.5f);
            Main.dust[DustID2].noGravity = true;
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, null, Color.Lime, 0f, Main.projectileTexture[projectile.type].Size()/2f, 1, SpriteEffects.None, 0f);

            return false;
        }
    }


    public class AcidicWebTile : ModTile
    {
        public static List<Point> websToRemove = new List<Point>();
        public static bool checkForWebRemoval = false;

        public static void RemoveWebs()
        {
            if (!checkForWebRemoval)
                return;

            if (NPC.CountNPCS(ModContent.NPCType<SpiderQueen>()) > 0)
                return;

            if (Main.netMode == NetmodeID.SinglePlayer || Main.dedServ)
            {
                for (int i = 0; i < 1+(websToRemove.Count / 50); i += 1)
                {

                    websToRemove = websToRemove.OrderBy(testby => Main.rand.Next()).ToList();

                    AcidicWebTile.checkForWebRemoval = true;

                    Point tilecoord = websToRemove[0];

                    Tile tile = Framing.GetTileSafely(tilecoord.X, tilecoord.Y);

                    if (tile.type == ModContent.TileType<AcidicWebTile>())
                    {
                        WorldGen.KillTile(tilecoord.X, tilecoord.Y);

                        NetMessage.SendTileRange(Main.myPlayer, tilecoord.X, tilecoord.Y, 1, 1);
                    }

                    websToRemove.RemoveAt(0);
                }

                if (websToRemove.Count < 1)
                {
                    AcidicWebTile.checkForWebRemoval = false;
                }
            }


        }

        public override bool Autoload(ref string name, ref string texture)
        {
            SGAmod.PostUpdateEverythingEvent += RemoveWebs;
            texture = "Terraria/Tiles_" + TileID.Cobweb;
            return true;
        }
        public override void SetDefaults()
        {
            TileID.Sets.NotReallySolid[Type] = true;
            Main.tileCut[Type] = true;
            Main.tileNoFail[Type] = true;
            Main.tileSolid[Type] = false;
            soundType = SoundID.Grass;
            soundStyle = 0;
            mineResist = 1f;
            dustType = DustID.Web;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = true;
            ModTranslation name = CreateMapEntryName();
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
            Vector2 location = (new Vector2(i, j) * 16)+zerooroffset;
            Tile tile = Main.tile[i, j];

            NoiseGenerator Noisegen = new NoiseGenerator(WorldGen._lastSeed);
            Noisegen.Amplitude = 0.50f;
            Noisegen.Frequency *= 1.00;

            spriteBatch.Draw(Main.tileTexture[tile.type], location - Main.screenPosition, new Rectangle(tile.frameX,tile.frameY,16,16), Color.Lerp(Color.DarkGreen*0.250f,Color.Lime,0.50f+ (float)Noisegen.Noise((int)(location.X + Main.GlobalTime * 10f), (int)(location.Y+Main.GlobalTime*10f))), 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);


            return false;
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].active())
            {
                g = Color.Lime.G * 0.25f;
            }
        }

    }


}