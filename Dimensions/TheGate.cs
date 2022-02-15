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

    public class TheGate : SGAPocketDim
    {
        public override int width => 800;
        public override int height => 500;
        public override bool saveSubworld => false;

        public override int DimType => 3;

        public override string DimName => "The Gate";

        public override UserInterface loadingUI => base.loadingUI;

        public override int? Music
        {

            get
            {
                return SGAmod.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/Silence");
            }

        }

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


            NoiseGenerator Noisegen = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds);

            Noisegen.Amplitude = 1;
            Noisegen.Frequency *= 0.50;

            int[] tilesz = { TileID.Dirt, TileID.Dirt, TileID.Dirt, TileID.Dirt, TileID.Stone, TileID.Stone, TileID.DiamondGemsparkOff, TileID.SnowFallBlock };

            //Noisegen.Frequency

            for (int x = 0; x < Main.maxTilesX; x += 1)
            {
                for (int y = tileheight; y < Main.maxTilesY; y += 1)
                {
                    double nousey = (double)MathHelper.Clamp((float)Noisegen.Noise(x, y) + 0.40f, -1.00f, 1.00f);
                    Main.tile[x, y].type = TileID.Stone;

                    int index = (int)MathHelper.Clamp((float)(0.50 + nousey / 2.0) * tilesz.Length, 0f, tilesz.Length - 1);
                    Main.tile[x, y].type = (ushort)tilesz[index];
                    Main.tile[x, y].active(true);
                    if (nousey < 0)
                        Main.tile[x, y].type = TileID.Dirt;
                }
            }
            
            List<DungeonTile> gatebackdroptiles = new List<DungeonTile>();

            int roomwidth = 160;
            int roomheight = 80;

            int center = (width / 2);

            for (int y = (height / 2) - (int)(roomheight); y < height; y += 1)
            {

                for (int x = center; x < center + (int)(roomwidth); x += 1)
                {
                    double input = ((double)center) - (x - 80);

                    float sigmoid = 1f-(1f / (1f + (float)Math.Pow(Math.E, -(input / 10.00))));

                    Vector2 there = new Vector2(x, y + sigmoid * 60f);

                    IDGWorldGen.PlaceMulti(there, TileID.BlueDungeonBrick, 8, WallID.BlueDungeonSlab);
                    if (there.Y < (height / 2)+1)
                    {
                        gatebackdroptiles.Add(new DungeonTile(there, 0, 0, there.Y > ((height / 2) - 1), 0));
                    }
                }

                for (int x = center - (int)(roomwidth); x < center; x += 1)
                {
                    double input = ((double)center) - (x + 80);

                    float sigmoid = (1f / (1f + (float)Math.Pow(Math.E, -(input / 10.00))));

                    Vector2 there = new Vector2(x, y + sigmoid * 60f);

                    IDGWorldGen.PlaceMulti(there, TileID.BlueDungeonBrick, 8, WallID.PinkDungeonSlab);
                    if (there.Y < (height / 2)+1)
                    {
                        gatebackdroptiles.Add(new DungeonTile(there, 0, 0, there.Y > ((height/2) - 1), 0));
                    }
                }

            }

            List<Point> platforms = new List<Point>();
            for(int i=0;i< gatebackdroptiles.Count; i += 1) 
            {
                DungeonTile atilz = gatebackdroptiles[i];
                int valuex = (int)atilz.vector.X;
                if (atilz.floor)
                {
                    Main.tile[valuex, (int)atilz.vector.Y+1].type=TileID.TeamBlockYellow;
                }
                    Main.tile[valuex, (int)atilz.vector.Y].active(false);
                    Main.tile[valuex, (int)atilz.vector.Y].wall = (atilz.vector.Y % 20 < 3 ? WallID.PinkDungeonSlab : WallID.BlueDungeonTile);

                if (Math.Abs(valuex - (width / 2)) < 100)
                {
                    if (valuex % 12 == 0)
                    {
                        int valuexx = valuex;
                        Main.tile[valuexx, (int)atilz.vector.Y].wall = 0;
                        WorldGen.PlaceWall(valuexx, (int)atilz.vector.Y, WallID.PalladiumColumn);

                        if ((int)atilz.vector.Y % 30 == ((valuex % 24 == 0) ? 15 : 0))
                        {
                            WorldGen.PlaceObject(valuexx, (int)atilz.vector.Y, TileID.Torches, false, 6);
                                for (int iii = -5; iii < 6; iii += 1)
                                {
                                    platforms.Add(new Point(valuexx + iii, (int)atilz.vector.Y + 2));
                                }
                        }
                    }

                }

            }

            for (int i = 0; i < platforms.Count; i += 1)
            {
                WorldGen.PlaceTile(platforms[i].X, platforms[i].Y, TileID.Platforms, style: 21);
            }

        }

        public override List<GenPass> tasks { get; }

        public TheGate()
        {
            tasks = new List<GenPass>();

            LimitPlayers = 16;

            tasks.Add(new SubworldGenPass(2f, progress =>
            {
                progress.Message = "Loading"; //Sets the text above the worldgen progress bar
                Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
                Main.rockLayer = Main.maxTilesY + 2; //Hides the cavern layer way out of bounds

                AGenPass(progress);


            }));

        }


        public override void Load()
        {
            Main.dayTime = true;
            Main.time = 40000;
        }


	}
   
}
