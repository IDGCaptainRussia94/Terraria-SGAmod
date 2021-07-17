using System.IO;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.World.Generation;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.UI;
using Terraria.DataStructures;
using Terraria.GameContent.UI;
using Terraria.Utilities;
using Idglibrary;
using SubworldLibrary;

namespace SGAmod.Dimensions
{
    public class FakeOverworld : SGAPocketDim
    {
        public override int width => 2400;
        public override int height => 1400;
        public override bool saveSubworld => false;

        public override string DimName => "Fake Overworld AKA Test Dim";

        public override UserInterface loadingUI => base.loadingUI;

        public override int? Music
        {

            get
            {
                return MusicID.Title;
            }

        }

        public override int DimType => 100;

        public virtual void AGenPass(GenerationProgress prog)
        {

            UnifiedRandom UniRand = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);
            int lastseed = WorldGen._genRandSeed;
            WorldGen._genRandSeed = DimDungeonsProxy.DungeonSeeds;
            enemyseed = (DimDungeonsProxy.DungeonSeeds);
            prog.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            //Main.spawnTileX = (Main.maxTilesX / 2) / 16;
            prog.Message = "Once was nothing...";
            int tileheight = UniRand.Next(250, 400);
            List<int> surfacelevel = new List<int>();
            int updown = UniRand.Next(0, 2) == 0 ? 1 : -1;



            //Perlin Noise Gen
            //Base Terrain Fill

            NoiseGenerator Noisegen = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds);


            Noisegen.Amplitude = 1;
            Noisegen.Frequency *= 0.50;

            int[] tilesz = { TileID.Obsidian, TileID.Ash, TileID.TinBrick, TileID.AmberGemsparkOff, TileID.AmethystGemsparkOff, TileID.DiamondGemsparkOff, TileID.SnowBlock, TileID.SnowBrick };

            //Noisegen.Frequency

            for (int x = 0; x < Main.maxTilesX; x += 1)
            {
                for (int y = tileheight; y < Main.maxTilesY; y += 1)
                {
                    double nousey = (double)MathHelper.Clamp((float)Noisegen.Noise(x, y) + 0.40f, -1.00f, 1.00f);
                    Main.tile[x, y].type = TileID.Stone;
                    //if (Noisegen.Noise(x,y) > -0.8)
                    //Main.tile[x, y].active(true);
                    //else
                    //Main.tile[x, y].active(false);

                    int index = (int)MathHelper.Clamp((float)(0.50 + nousey / 2.0) * tilesz.Length, 0f, tilesz.Length - 1);
                    //Main.tile[x,y].color((byte)2);
                    Main.tile[x, y].type = (ushort)tilesz[index];
                    Main.tile[x, y].active(true);
                    if (nousey < 0)
                        Main.tile[x, y].active(false);
                }


            }


            //Celular Crap

            for (int passes = 0; passes < 5; passes += 1)
            {
                for (int x = 0; x < Main.maxTilesX; x += 1)
                {
                    for (int y = 0; y < Main.maxTilesY; y += 1)
                    {
                        if (GetTilesAround(x, y, 1) > 4)
                            Main.tile[x, y].active(true);
                        else
                            Main.tile[x, y].active(false);
                    }
                }
            }


            //Hills
            for (int totalpass = 0; totalpass < 3; totalpass += 1)
            {


                float randseed = UniRand.NextFloat(0, 100000);
                float randseed2 = UniRand.NextFloat(0, 100000);

                List<float> Scales = new List<float>();
                List<float> ScalesRate = new List<float>();

                for (int sss = 0; sss < 12; sss += 1)
                {
                    float scalesize = UniRand.NextFloat(2 + sss, 30 + sss);
                    Scales.Add(scalesize);
                    ScalesRate.Add(UniRand.NextFloat(10 + scalesize, 40 + scalesize * 2));
                }

                List<float> ScalesX = new List<float>();
                List<float> ScalesRateX = new List<float>();

                for (int sss = 0; sss < 16; sss += 4)
                {
                    float scalesize = UniRand.NextFloat(2 + sss, 25 + sss);
                    ScalesX.Add(scalesize);
                    ScalesRateX.Add(UniRand.NextFloat(20 + scalesize, 60 + scalesize * 2));
                }


                for (int x = 0; x < Main.maxTilesX; x += 1)
                {
                    int y = (int)(Main.maxTilesY * 0.50f);
                    for (int zz = 0; zz < Scales.Count; zz += 1)
                    {
                        y += (int)(Math.Sin(randseed2 + (x / ScalesRate[zz])) * Scales[zz]);
                    }

                    for (int yy = 0; yy < y; yy += 1)
                    {
                        int xx = x;

                        if (InsideMap(xx, yy))
                            Main.tile[xx, yy].active(false);
                    }

                }
            }

            WorldGen._genRandSeed = lastseed;

        }

        public override List<GenPass> tasks { get; }

        public FakeOverworld()
        {
            tasks = new List<GenPass>();

            tasks.Add(new SubworldGenPass(2f, progress =>
            {
                progress.Message = "Loading"; //Sets the text above the worldgen progress bar
                Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
                Main.rockLayer = Main.maxTilesY + 2; //Hides the cavern layer way out of bounds

                AGenPass(progress);


            }));

        }

        public enum Paints : byte
        {
            None,
            Red,
            Orange,
            Yellow,
            Lime,
            Green,
            Teal,
            Cyan,
            SkyBlue,
            Blue,
            Purple,
            Violet,
            Pink,
            DeepRed,
            DeepOrange,
            DeepYellow,
            DeepLime,
            DeepGreen,
            DeepTeal,
            DeepCyan,
            DeepSkyBlue,
            DeepBlue,
            DeepPurple,
            DeepViolet,
            DeepPink,
            Black,
            White,
            Gray,
            Brown,
            Shadow,
            Negative
        }


        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 40000;
        }


    }

}