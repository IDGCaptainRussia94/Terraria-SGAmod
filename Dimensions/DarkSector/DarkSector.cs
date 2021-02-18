using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using AAAAUThrowing;
using SGAmod.Effects;
using SGAmod.SkillTree.Survival;
using Terraria.Utilities;
using Terraria.DataStructures;
using Microsoft.Win32;
using System.Threading;
using System.Runtime.InteropServices.WindowsRuntime;
using Terraria.ID;
using Idglibrary;
using System.Runtime.Remoting.Messaging;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Buffs;

namespace SGAmod.Dimensions
{
    public abstract class AbstractCloneable : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }

    public class DarkSectorTile : AbstractCloneable
    {
        public Point16 position;
        public Point16 myDirection;
        public readonly int seed;
        public bool outerlayer = false;
        public readonly DarkSector myDarkSector;
        public int gen = 0;

        public DarkSectorTile(DarkSector myDarkSector, int x, int y, Point16 myDirection, UnifiedRandom rand, int gen = 1)
        {
            position = new Point16(x, y);
            this.myDarkSector = myDarkSector;
            this.seed = myDarkSector.seed;
            this.gen = gen;

            myDarkSector.sectors.Add(this);

            Spread();
        }
        public void Spread()
        {
            for(int i=0; i < DarkSector.Cardinals.Length; i += 1)
            {
                Point16 direction = DarkSector.Cardinals[i];
                    direction = DarkSector.Cardinals[myDarkSector.rand.Next(0, DarkSector.Cardinals.Length)];

                Point16 location = position;
                for (int zxx = 0; zxx < myDarkSector.scaleSize; zxx += 1)
                    location += direction;

                if (myDirection != direction)
                {
                    if (myDarkSector.sectors.Find(search => search.position == location) == default || gen<2)
                    {
                        if (myDarkSector.growthconditions(myDarkSector, location.X, location.Y, gen + 1))
                        {
                            DarkSectorTile newsector = new DarkSectorTile(myDarkSector, location.X, location.Y, direction, myDarkSector.rand, gen + 1);
                        }
                    }
                }
            }
        }

        public void DrawTile(Vector2 size,Texture2D tex)
        {
            int scalecheck = 64 * myDarkSector.scaleSize;
            int posx4x = position.X << 4;

            if (posx4x < Main.screenPosition.X + Main.screenWidth + scalecheck && posx4x > Main.screenPosition.X  - scalecheck)
            {
                int posy4x = position.Y << 4;
                if (posy4x < Main.screenPosition.Y + Main.screenHeight + scalecheck && posy4x > Main.screenPosition.Y - scalecheck)
                {
                    float atime = (float)SGAWorld.modtimer/90f;
                    float atime2 = (float)(SGAWorld.modtimer-900f) / 70f;
                    float add1 = ((float)Math.Cos(atime + position.Y / 20f) * 25f);
                    float add2 = ((float)Math.Sin(atime / 1.25f + position.X / 30f) * 19f);
                    float alpha2 = (float)Math.Sin(((-atime2 * 0.25f)+((add1+add2) / 40f)));
                    float alpha = MathHelper.Clamp(alpha2, myDarkSector.hasCompass ? 0.1f : 0.25f, myDarkSector.hasCompass ? 0.35f : 0.85f)/5f;

                    Vector2 scale = new Vector2(1f + (float)Math.Sin(atime * 1.25f + (position.X - position.Y)/10f)*0.5f, 1f + (float)Math.Sin(atime + (position.Y + position.X) / 10f) * 0.5f);
                    Main.spriteBatch.Draw(tex, (new Vector2(posx4x, posy4x))-Main.screenPosition, default, Color.White * alpha, 0, size, scale*myDarkSector.scaleSize, SpriteEffects.None, 0f);

                }
            }

        }

        /*public void TestParticle()
        {

            //if (Main.rand.Next(0,20)<1)
            //Dust.NewDustPerfect(position.ToVector2()*16, 173, Vector2.Zero, 0, Color.Blue, 1f);
        }*/

    }


    public class DarkSector
    {
        public static readonly Point16[] Cardinals = { new Point16(0, -1), new Point16(0, 1), new Point16(-1, 0), new Point16(1, 0) };
        public int seed;
        public UnifiedRandom rand;
        public List<DarkSectorTile> sectors;
        public Func<DarkSector,int, int, int, bool> growthconditions;
        public Point16 position;
        public bool done = false;
        public Vector4 BoundingBox;
        public int scaleSize = 2;
        private int flavortext;
        public bool hasCompass = false;
        Task initTask;

        public DarkSector(int x, int y, Func<DarkSector, int, int, int, bool> growthconditions = default, int seed = default,int scaleSize = 2,int flavortext = 1)
        {
            this.flavortext = flavortext;
            sectors = new List<DarkSectorTile>();

            position = new Point16(x, y);
            this.scaleSize = scaleSize;

            if (seed == default)
                seed = WorldGen._genRandSeed+position.GetHashCode();

            this.seed = seed;

            if (growthconditions == default)
            {
                growthconditions = delegate (DarkSector darksector,int X, int Y, int Gen)
                {
                    if (Gen < darksector.rand.Next(420,1600))
                    return true;

                    return false;
                };

            }

            this.growthconditions = growthconditions;

            initTask = Task.Run(DarkSectorGen);

            DimDingeonsWorld.darkSectors.Add(this);

        }

        private void DarkSectorGen()
        {
                System.Diagnostics.Stopwatch stopwatch = System.Diagnostics.Stopwatch.StartNew();
                if (flavortext == 2)
                    Main.NewText("Beginning Dark Sector Generation");

            rand = new UnifiedRandom(seed);

            new DarkSectorTile(this, position.X, position.Y, new Point16(0, scaleSize), rand, 0);

            PostData();

            done = true;
            stopwatch.Stop();
            if (flavortext == 1)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    Idglib.Chat("A Darkness emerges from beneath the folds existence...", 120, 15, 15);
                }

            }
                if (flavortext == 2)
            {
                Main.NewText("Dark Sector Generated, Completed in " + stopwatch.ElapsedMilliseconds);
                Main.NewText("Sector Count " + sectors.Count);
            }

        }

        public bool DarkSectorZone(Player player)
        {
            Rectangle rect = new Rectangle((int)BoundingBox.X, (int)BoundingBox.Y, (int)BoundingBox.Z - (int)BoundingBox.X, (int)BoundingBox.W - (int)BoundingBox.Y);
            Rectangle rect2 = new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y, Main.screenWidth, Main.screenHeight);
            rect2.Inflate(256,256);

            if (rect.Intersects(rect2))
            {
                return true;
            }

            return false;
        }

        public bool PlayerInside(Player player,int radius = 2)
        {
            if (!done)
                return false;

            if (!DarkSectorZone(player) || player.SGAPly().timer%4<3)
                return false;



            return PointInside(player.Center, radius);
        }

        public bool PointInside(Vector2 pos, int radius = 2)
        {

            if (!done)
                return false;

            radius += (scaleSize - 1) * 2;

            foreach (DarkSectorTile sectortile in sectors)
            {
                if (pos.X >= (sectortile.position.X - radius) * 16 && pos.X < (sectortile.position.X + radius) * 16)
                {
                    if (pos.Y >= (sectortile.position.Y - radius) * 16 && pos.Y < (sectortile.position.Y + radius) * 16)
                    {
                        return true;
                    }
                }

            }
            return false;
        }

        public void Draw(Vector2 size,Texture2D tex)
        {
            if (!done)
                return;

            hasCompass = (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<Items.Consumable.CaliburnCompess>() || Main.LocalPlayer.HasItem(ModContent.ItemType<Items.Consumable.CaliburnCompess>())) && SGAWorld.darknessVision;

            if (DarkSectorZone(Main.LocalPlayer))
            {
                foreach (DarkSectorTile sectortile in sectors)
                {
                    sectortile.DrawTile(size,tex);
                }
            }

        }

        public void PostUpdate()
        {
            if (!done)
                return;

            /*if (DarkSectorZone(Main.LocalPlayer))
            {
                foreach (DarkSectorTile sectortile in sectors)
                {
                    sectortile.TestParticle();
                }
            }*/

        }

        public void PostData()
        {
            BoundingBox = new Vector4(position.X, position.Y, position.X, position.Y);
            foreach (DarkSectorTile sectortile in sectors)
            {
                if (sectortile.position.X < BoundingBox.X)
                    BoundingBox.X = sectortile.position.X;
                if (sectortile.position.Y < BoundingBox.Y)
                    BoundingBox.Y = sectortile.position.Y;
                if (sectortile.position.X > BoundingBox.Z)
                    BoundingBox.Z = sectortile.position.X;
                if (sectortile.position.Y > BoundingBox.W)
                    BoundingBox.W = sectortile.position.Y;

            }
            BoundingBox *= 16f;
        }
    }

}
