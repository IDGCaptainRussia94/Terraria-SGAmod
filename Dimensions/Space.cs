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
using Microsoft.Xna.Framework.Audio;
using SGAmod.Effects;
using SGAmod.Items;
using System.Linq;
using SGAmod.Dimensions.NPCs;
using SGAmod.Items.Consumables;
using SGAmod.Tiles;
using SGAmod.NPCs;
using SGAmod.Dimensions.Tiles;

namespace SGAmod.Dimensions
{
    public class FilledSpaceArea
    {
        public Vector2 position;
        public byte type;
        //public byte[] allowedToConnectwith = {0};
        public FilledSpaceArea(Vector2 where)
        {
            position = where;
            type = 0;
        }
    }

    public class SpaceDim : SGAPocketDim
    {
        public override int width => 3200;
        public override int height => 800;
        public override bool saveSubworld => false;
        public override float spawnRate => 7.50f;

        public override string DimName => "Near Terrarian Orbit";

        public static List<Vector2> EmptySpaces;
        public static List<FilledSpaceArea> FilledSpaces;

        public override UserInterface loadingUI => base.loadingUI;
        NoiseGenerator Noisegen;
        NoiseGenerator Noisegen2freq;
        int noiseScale = 4;
        double[,] noiseGrid;
        double[,] noiseGrid2;
        int[,] poissonGrid;

        public static bool crystalAsteriods = true;
        public static bool SpaceBossIsActive
        {
            get
            {
                int bossindex = NPC.FindFirstNPC(ModContent.NPCType<SpaceBoss>());

                if (bossindex >= 0)
                {
                    SpaceBoss boss = Main.npc[bossindex]?.modNPC as SpaceBoss;
                    if (boss != null && boss.WakingUpStartMusic)
                    {
                        return true;
                    }
                }
                return false;
            }

        }

        public override Texture2D GetMapBackgroundImage()
        {
            return SGAmod.Instance.GetTexture("SpaceMapBackground_NoSun");
        }

        public override int? Music
        {

            get
            {
                if (SpaceBossIsActive)
                    return SGAmod.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Space_Boss");
                return SGAmod.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/SGAmod_Space");
            }

        }
        public void DFSPDS(Point start, UnifiedRandom rand) // Depth First Search Poisson Disc Sampling... I think
        {
            int searchTries = 5;
            int minDistance = 4;
            Vector2 placementDistance = new Vector2(60, 120);
            int retryCount = 100;

            int maxWidth = width / noiseScale;
            int maxHeight = height / noiseScale;

            List<Point> TakenPoints = new List<Point>();

            int hardlimit = 1000000;

            Stack<Point> circlePointsStack = new Stack<Point>();
            circlePointsStack.Push(start);
        backToStart:

            while (circlePointsStack.Count > 0)
            {
                hardlimit -= 1;
                if (TakenPoints.Count > hardlimit)
                    break;

                Point thisPoint = circlePointsStack.Peek();

                proggers.Message = "disks in stack: " + circlePointsStack.Count + " TakenPoints " + TakenPoints.Count;

                // SGAmod.Instance.Logger.Debug(circlePointsStack.Count + "Disk count");

                for (int tries = 0; tries < 999; tries += 1)
                {
                    bool success = true;
                    float angle = rand.NextFloat(MathHelper.TwoPi);

                    minDistance = Math.Max(3, 3 + (int)(MathHelper.Clamp((float)Noisegen2freq.Noise(thisPoint.X + 9999, thisPoint.Y + 77777), -1f, 0.35f) * 16f));

                    float dista = (minDistance * noiseScale) + (rand.NextFloat(placementDistance.X, placementDistance.Y));

                    Point idealLocation = new Point(thisPoint.X + (int)(Math.Cos(angle) * dista), thisPoint.Y + (int)(Math.Sin(angle) * dista));

                    if (!InsideMap(idealLocation.X, idealLocation.Y))
                        continue;

                    Point gridloc = new Point(idealLocation.X / noiseScale, idealLocation.Y / noiseScale);
                    //float gridadd = (float)(noiseGrid[gridloc.X, gridloc.Y]);
                    double noiseadd2 = (double)MathHelper.Clamp((float)Noisegen2freq.Noise(idealLocation.X, idealLocation.Y), -1.00f, 1.00f);
                    double noiseadd = (double)MathHelper.Clamp((float)Noisegen.Noise(idealLocation.X + 9999, idealLocation.Y + 9999), -1.00f, 1.00f);

                    if ((float)Math.Sin((idealLocation.Y / (float)height) * MathHelper.Pi) * 1.50f < 1.3f + (noiseadd2 * 1.25f))
                    {
                        success = false;
                        goto SkipOut;
                    }

                    /*if (Math.Abs(noiseGrid[gridloc.X, gridloc.Y])>rand.NextDouble())
                    {
                        success = false;
                        goto SkipOut;
                    }*/

                    for (int x = gridloc.X - minDistance; x <= gridloc.X + minDistance; x += 1)
                    {
                        for (int y = gridloc.Y - minDistance; y <= gridloc.Y + minDistance; y += 1)
                        {
                            if (x < 0 || x >= maxWidth || y < 0 || y >= maxHeight || poissonGrid[x, y] > 5 + noiseadd * 10.00)//(0.00+(noiseGrid2[gridloc.X, gridloc.Y]*2.00)))
                            {
                                success = false;
                                goto SkipOut;
                            }
                        }
                    }

                SkipOut:

                    if (success)
                    {
                        //Add point
                        TakenPoints.Add(idealLocation);
                        circlePointsStack.Push(idealLocation);
                        for (int x = gridloc.X - minDistance; x <= gridloc.X + minDistance; x += 1)
                        {
                            for (int y = gridloc.Y - minDistance; y <= gridloc.Y + minDistance; y += 1)
                            {
                                poissonGrid[x, y] += (minDistance - Math.Abs(gridloc.X - x)) + (minDistance - Math.Abs(gridloc.Y - y));
                            }
                        }

                        goto backToStart;
                    }

                    if (tries > searchTries)
                    {
                        circlePointsStack.Pop();
                        goto backToStart;
                    }

                }
                goto backToStart;

            }

            if (TakenPoints.Count < retryCount)//need more
            {
                DimDungeonsProxy.DungeonSeeds += 10;
                AGenPass(proggers);
            }

            FilledSpaces = new List<FilledSpaceArea>();
            EmptySpaces = new List<Vector2>();


            foreach (Point pointa in TakenPoints)
            {
                //Point gridloc = new Point(pointa.X / noiseScale, pointa.Y / noiseScale);
                //if (noiseGrid2[gridloc.X, gridloc.Y] > 0.5f)

                Noisegen.Amplitude = 1.20;
                Noisegen.Frequency = 0.75;
                Noisegen.Octaves = 5;
                Noisegen.Persistence = 1f;
                //Only sometimes, make one

                if ((double)MathHelper.Clamp((float)Noisegen.Noise(pointa.X, pointa.Y), -1.00f, 1.00f) > rand.NextDouble() - 0.75)
                {
                    EmptySpaces.Add(pointa.ToVector2() * 16f);
                    continue;
                }

                Noisegen.Amplitude = 1.15;
                Noisegen.Frequency = 1.75;
                Noisegen.Octaves = 5;
                Noisegen.Persistence = 1f;
                FilledSpaces.Add(new FilledSpaceArea(pointa.ToVector2() * 16f));

                MakeAsteriod(pointa, rand);
            }

        }

        public int AsteriodDensity(Point where,int buffer)
        {
            int ammount = 0;

            for(int x=-buffer; x <= buffer; x += 1)
            {
                for (int y = -buffer; y <= buffer; y += 1)
                {
                    int whereX = Math.Min(width, Math.Max(0,where.X + x));
                    int whereY = Math.Min(width, Math.Max(0, where.Y + y));

                    Tile tile = Framing.GetTileSafely(whereX, whereY);
                    if (tile.active() && Main.tileSolid[tile.type])
                        ammount += 1;
                }
            }
            return ammount;
        }

        public void MakeAsteriod(Point where, UnifiedRandom rand)
        {

            //float angle = rand.NextFloat(MathHelper.TwoPi);

            Vector2 dists = new Vector2(0, 10);
            int size2 = rand.Next(2, 4);
            int spaced = 0;
            int itr = 8;

            if (rand.Next(0, 10) == 0)
            {
                size2 += 2;
                spaced += 8;
                itr = 16;
            }

            for (int i = 0; i < itr; i += 1)
            {

                Vector2 offsetGaussian = Gaussian2D(rand.NextFloat(), rand.NextFloat()) * (i + spaced);

                Point idealLocation = new Point(where.X + (int)offsetGaussian.X, where.Y + (int)offsetGaussian.Y);

                int size = size2 + rand.Next(-1, 2) + (int)MathHelper.Clamp((float)Noisegen.Noise(where.X, where.Y) * 4f, -2f, 3f);

                IDGWorldGen.PlaceMulti(idealLocation, SGAmod.Instance.TileType("Spacerock"), size);

                if (rand.Next(-50, Math.Max(80, 300 - where.Y)) > 0)
                    IDGWorldGen.TileRunner(where.X, where.Y, 5, 12, TileID.Meteorite, false, rand: rand);

                if (rand.Next(-50, Math.Max(-10, 300 - where.Y)) > 0)
                    IDGWorldGen.TileRunner(where.X, where.Y, 4, 15, SGAmod.Instance.TileType("AstrialLuminite"), false, rand: rand);

                if (rand.Next((height - 300) - 50, Math.Max((height - 300) - 10, height)) < where.Y)
                    IDGWorldGen.TileRunner(where.X, where.Y, 4, 15, SGAmod.Instance.TileType("VibraniumCrystalTile"), false, rand: rand);

            }

        }

        //https://github.com/RichardEllicott/GodotSnippets/blob/master/maths/gaussian_2D.gd
        public static Vector2 Gaussian2D(float rand1, float rand2)
        {
            var al1 = Math.Sqrt(-2 * Math.Log(rand1)); // part one
            var al2 = MathHelper.TwoPi * rand2; // part two
            float x = (float)(al1 * Math.Cos(al2));
            float y = (float)(al1 * Math.Sin(al2));
            return new Vector2(x, y);
        }

        //public override int DimType => 100;

        GenerationProgress proggers;

        public void ChooseEnemies()
        {
            EnemySpawnsOverride = delegate (IDictionary<int, float> pool, NPCSpawnInfo spawnInfo, SGAPocketDim pocket)
            {
                    //UnifiedRandom UniRand2 = new UnifiedRandom(pocket.enemyseed);
                    for (int i = 0; i < pool.Count; i += 1)
                {
                    pool[i] = 0f;

                }
                    if (spawnInfo.spawnTileType == ModContent.TileType<VibraniumCrystalTile>())
                pool[ModContent.NPCType<ResonantWisp>()] = 50f;
                pool[ModContent.NPCType<OverseenHead>()] = 1f;
                pool[NPCID.MartianDrone] = 0.25f;
                pool[NPCID.MartianTurret] = 0.2f;

                pocket.chooseenemies = true;
                return 1;
            };
        }

        public override void DoUpdates()
        {
            ChooseEnemies();
        }

        public void SmoothOutAsteriods(UnifiedRandom uniRand,int passesToDo = 4)
        {

            for (int passes = 0; passes < passesToDo; passes += 1)
            {
                for (int y = 0; y < Main.maxTilesY; y += 1)
                {
                    for (int x = 0; x < Main.maxTilesX; x += 1)
                    {
                        Tile tile = Main.tile[x, y];
                        bool asteriodRocks = tile.type == ModContent.TileType<Spacerock>() || tile.type == ModContent.TileType<Spacerock2>() || tile.type == ModContent.TileType<AstrialLuminite>() || tile.type == TileID.Meteorite;

                        if (!asteriodRocks)
                            continue;

                        if (GetTilesAround(x, y, 1) > uniRand.Next(3, 6))
                            Main.tile[x, y].active(true);
                        else
                            Main.tile[x, y].active(false);
                    }
                }
            }
        }

        public virtual void AGenPass(GenerationProgress prog)
        {
            proggers = prog;

            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    Main.tile[x, y].active(false);
                    Main.tile[x, y].type = (ushort)SGAmod.Instance.TileType("Spacerock2");
                    Main.tile[x, y].wall = 0;

                }
            }

            UnifiedRandom UniRand = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);
            int lastseed = WorldGen._genRandSeed;
            WorldGen._genRandSeed = DimDungeonsProxy.DungeonSeeds;
            enemyseed = (DimDungeonsProxy.DungeonSeeds);
            prog.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            //Main.spawnTileX = (Main.maxTilesX / 2) / 16;
            prog.Message = "Leaving Atmosphere...";
            int tileheight = UniRand.Next(250, 400);

            ChooseEnemies();

             //Perlin Noise Gen
             //Base Terrain Fill

             Noisegen = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds);

            Noisegen.Amplitude = 1.75;
            Noisegen.Frequency *= 0.40;
            Noisegen.Octaves = 5;
            Noisegen.Persistence = 1f;

            Noisegen.UseGradient = true;
            Noisegen.InterpolateType = 3;

            Noisegen2freq = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds + 1);

            Noisegen2freq.Amplitude = 1.25;
            Noisegen2freq.Frequency *= 0.55;
            Noisegen2freq.Octaves = 4;
            Noisegen2freq.Persistence = 1f;
            Noisegen2freq.InterpolateType = 2;
            Noisegen2freq.UseGradient = true;

            noiseGrid = new double[width / noiseScale, height / noiseScale];
            noiseGrid2 = new double[width / noiseScale, height / noiseScale];
            poissonGrid = new int[width / noiseScale, height / noiseScale];

            for (int x = 0; x < width / noiseScale; x += 1)
            {
                for (int y = 0; y < height / noiseScale; y += 1)
                {
                    poissonGrid[x, y] = 0;
                    //noiseGrid[x,y] = (double)MathHelper.Clamp((float)Noisegen2freq.Noise(x, y), -1.00f, 1.00f);
                    //noiseGrid2[x, y] = (double)MathHelper.Clamp((float)Noisegen.Noise(x, y), -1.00f, 1.00f);
                }
            }

            Point baseLocation = new Point(UniRand.Next(400 + (width - 800)), tileheight);

            Main.spawnTileX = baseLocation.X;
            Main.spawnTileY = baseLocation.Y;

            for (int xx = -4; xx < 4; xx += 1)
            {
                for (int yy = 0; yy < 2; yy += 1)
                {
                    Main.tile[Main.spawnTileX + xx, Main.spawnTileY + yy].type = TileID.MartianConduitPlating;
                    Main.tile[Main.spawnTileX + xx, Main.spawnTileY + yy].active(true);
                }
            }

            prog.Message = "Filling up the sky with rocks";
            DFSPDS(new Point(width / 2, height / 2), UniRand);


            //Noisegen.Frequency

            /*

            float maxPercent = (float)(Main.maxTilesX * Main.maxTilesY);

            for (int x = 0; x < Main.maxTilesX; x += 2)
            {
                for (int y = 0; y < Main.maxTilesY; y += 2)
                {
                    prog.Value = ((y + (x * Main.maxTilesY)) / maxPercent) * 1f;
                    prog.Message = "Filling up the sky with rocks";

                    double nousetopbottom = (double)MathHelper.Clamp((float)Noisegen2freq.Noise(x, y), -1.00f, 1.00f);
                    double nousey = (double)MathHelper.Clamp((float)(Noisegen.Noise(x, y) + nousetopbottom / 3f) - 1.00f, -1.00f, 1.00f);

                    nextone:

                    for (int xx = 0; xx < 2; xx += 1)
                    {
                        for (int yy = 0; yy < 2; yy += 1)
                        {

                            int xxx = xx + x;
                            int yyy = yy + y;

                            if (!InsideMap(xxx, yyy))
                                continue;

                            Main.tile[xxx, yyy].type = (ushort)SGAmod.Instance.TileType("Spacerock");

                            if (nousey < 0 - (nousetopbottom / 1f))
                                continue;

                            //if (Noisegen.Noise(x,y) > -0.8)
                            //Main.tile[x, y].active(true);
                            //else
                            //Main.tile[x, y].active(false);

                            if (Math.Abs(nousetopbottom) > 0.85f - (1.1f * Math.Sin(((y / (float)Main.maxTilesY)) * MathHelper.Pi)))
                            {
                                int[] tilesz = { SGAmod.Instance.TileType("Spacerock"), SGAmod.Instance.TileType("Spacerock"), SGAmod.Instance.TileType("Spacerock"), SGAmod.Instance.TileType("Spacerock"),

                            UniRand.Next(0, 8) == 0 ? TileID.Meteorite : SGAmod.Instance.TileType("Spacerock"),
                            UniRand.Next(0, 5) == 0 ? TileID.Meteorite : SGAmod.Instance.TileType("Spacerock"),
                            UniRand.Next(0, 3) == 0 ? TileID.Meteorite : SGAmod.Instance.TileType("Spacerock"),TileID.Meteorite,TileID.Meteorite};

                                int index = (int)MathHelper.Clamp((float)(0.50 + (nousey + (nousetopbottom / 3f)) / 2.5) * tilesz.Length, 0f, tilesz.Length - 1);

                                //Main.tile[x,y].color((byte)2);
                                Main.tile[xxx, yyy].type = (ushort)tilesz[index];
                                Main.tile[xxx, yyy].active(true);
                            }
                        }
                    }
                }
            }*/


            //Gen space station, thing here

            //Cellular Crap

            prog.Message = "Smoothing Asteroids";

            SmoothOutAsteriods(UniRand);

            Rectangle rect = new Rectangle(400, 200, width - 800, height - 400);

            EmptySpaces = EmptySpaces.Where(testby => rect.Contains(new Point((int)testby.X / 16, (int)testby.Y / 16)) && AsteriodDensity(new Point((int)testby.X / 16, (int)testby.Y / 16), 8) < 20 && Main.tile[(int)testby.X / 16, (int)testby.Y / 16].wall<=0).ToList();
            Vector2 bossplace = EmptySpaces[0];

            NPC.NewNPC((int)bossplace.X, (int)bossplace.Y, ModContent.NPCType<SpaceBoss>());

            prog.Message = "Finishing Up";

            WorldGen._genRandSeed = lastseed;

        }

        public override List<GenPass> tasks { get; }

        public SpaceDim()
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

        public override void Load()
        {
            Main.dayTime = false;
            Main.time = 0;
        }
    }

    public class SpaceSky : CustomSky
    {
        private Random _random = new Random();
        private bool _isActive;
        private float[] xoffset = new float[200];
        private Color acolor = Color.Gray;
        Effect effect => SGAmod.TrailEffect;
        public Vector2 sunPosition;
        public Vector2 sunOrigPosition;
        public Vector2 sunBossPosition;
        public float skyalpha = 1f;
        public float darkalpha = 0f;
        public float sunIsApprouching = 0f;

        public override void OnLoad()
        {
            sunOrigPosition = new Vector2((Main.screenWidth / 2), (Main.screenHeight / 8));
        }

        public override void Update(GameTime gameTime)
        {
            int bossIndex = NPC.FindFirstNPC(ModContent.NPCType<SpaceBoss>());
            SpaceBoss boss = bossIndex >= 0 ? Main.npc[bossIndex].modNPC as SpaceBoss : null;

            skyalpha = MathHelper.Clamp(skyalpha + (boss != null && boss.goingDark > 0 ? -0.0015f : 0.005f), 0f, 1f);
            darkalpha = MathHelper.Clamp(darkalpha + (boss != null && boss.goingDark > 0 ? 0.0025f : (boss != null && boss.DyingState ? -0.075f : -0.005f)), 0f, 1f);

            sunPosition = sunOrigPosition;

            if (boss != null && bossIndex >= 0 && !boss.DyingState)
            {
                sunBossPosition = boss.npc.Center;
                sunIsApprouching = MathHelper.Clamp(1f - (boss.countdownToTheEnd / (100f * 60f)), 0f, 1f);
            }
            else
            {
                sunIsApprouching = MathHelper.Clamp(sunIsApprouching - (1f / 300f), 0f, 1f);
            }

            sunPosition = Vector2.Lerp(sunOrigPosition, Vector2.Lerp(sunOrigPosition, Vector2.Lerp(sunOrigPosition, sunBossPosition - Main.screenPosition, sunIsApprouching), sunIsApprouching), sunIsApprouching);

            //acolor = Main.hslToRgb(0f, 0.0f, 0.5f);
        }

        public override Color OnTileColor(Color inColor)
        {
            Color colorsa = Color.Lerp(Main.hslToRgb(0, 0, 0.20f), Color.OrangeRed, MathHelper.Clamp((sunIsApprouching - 0.15f) * 1.25f, 0f, 1f));
            return colorsa;
        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {

            /*basicEffect.World = WVP.World();
            basicEffect.View = WVP.View(Main.GameViewMatrix.Zoom);
            basicEffect.Projection = WVP.Projection();
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = SGAmod.ExtraTextures[21];*/

            Matrix identity = Matrix.Identity;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, identity);

            spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, identity);

            int bossIndex = NPC.FindFirstNPC(ModContent.NPCType<SpaceBoss>());
            SpaceBoss boss = bossIndex >= 0 ? Main.npc[bossIndex].modNPC as SpaceBoss : null;
            bool drawBoss = boss != null && boss.npc.active;


            VertexBuffer vertexBuffer;

            UnifiedRandom alwaysthesame = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);

            for (int type = 0; type < 2; type += 1)
            {

                Vector2 parallex = new Vector2(alwaysthesame.NextFloat(), alwaysthesame.NextFloat()) + (type == 0 ? Vector2.Zero : (new Vector2(Main.screenPosition.X, Main.screenPosition.Y * 2.5f) / 500000f));// new Vector2(Main.screenPosition.X / 9000f, -Main.GlobalTime * 0.1f);

                effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Vector2.One) * WVP.Projection());
                effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture(type == 0 ? "TiledPerlin" : "Space"));
                effect.Parameters["coordOffset"].SetValue(parallex);
                effect.Parameters["coordMultiplier"].SetValue(new Vector2(0.25f, 0.45f));
                effect.Parameters["strength"].SetValue(type == 0 ? 1f : 5f);

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

                Vector3 screenPos = new Vector3(-16, 0, 0);
                float skymove = ((Math.Max(Main.screenPosition.Y - 8000, 0)) / (Main.maxTilesY * 16f));

                Color colorsa = Color.Lerp(Color.Lerp(Color.Black, Main.hslToRgb(alwaysthesame.NextFloat(), 1f, 0.75f), 0.1f), Color.OrangeRed, MathHelper.Clamp((sunIsApprouching - 0.10f) * 1.45f, 0f, 1f));

                vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-16, 0, 0), colorsa, new Vector2(0, 0));
                vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), colorsa, new Vector2(0, 1));
                vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), colorsa, new Vector2(1, 0));

                vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, Main.screenHeight, 0), colorsa, new Vector2(1, 1));
                vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), colorsa, new Vector2(0, 1));
                vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), colorsa, new Vector2(1, 0));

                vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

                Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;
                effect.CurrentTechnique.Passes[type == 0 ? "BasicEffectPass" : "BasicEffectAlphaPass"].Apply();
                Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

            }

            if (skyalpha > 0)
            {

                if (maxDepth >= 0 && minDepth < 0)
                {
                    //spriteBatch.Draw(Main.blackTileTexture, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), (Color.Black * 0.8f), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, identity);

                if (drawBoss)
                    boss.AlterSky(this, 0, spriteBatch, minDepth, maxDepth);

                //ArmorShaderData shader3 = GameShaders.Armor.GetShaderFromItemId(ItemID.MartianArmorDye); shader3.Apply(null);

                //Stars

                for (float i = 0.03f; i <= 0.32f; i += 0.02f)
                {
                    for (int xy = 0; xy < 30; xy += 1)
                    {

                        //SGAmod.Instance.GetTexture("Extra_57b");
                        Texture2D texx = Main.starTexture[alwaysthesame.Next(Main.starTexture.Length)];

                        Vector2 scale = (Vector2.One * 0.4f) + (new Vector2(i) * 2f);
                        Vector2 loc = new Vector2(alwaysthesame.Next(-64, Main.screenWidth + 64), alwaysthesame.Next(-64, Main.screenHeight + 64));
                        //if (loc.X > -64 && loc.Y > -64 && loc.X < Main.screenWidth + 64 && loc.Y < Main.screenHeight + 64)
                        //{

                        Star star = Main.star[alwaysthesame.Next(0, Main.star.Length)];

                        spriteBatch.Draw(texx, loc, null, (Color.White * MathHelper.Clamp(i, 0f, 1f)) * skyalpha, star.rotation, texx.Size() / 2f, scale * star.scale, SpriteEffects.None, 0f);
                        //}
                    }
                }

                if (drawBoss)
                    boss.AlterSky(this, 1, spriteBatch, minDepth, maxDepth);

                if (maxDepth >= 0 && minDepth < 0)//Sun
                {
                    if (sunIsApprouching > 0.2f)
                    {

                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, identity);

                        Color colors = Color.Lerp(Color.Transparent, Color.OrangeRed, sunIsApprouching - 0.20f);

                        /*vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-16, 0, 0), colors, new Vector2(0, 0));
                        vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), colors, new Vector2(0, 0));
                        vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), colors, new Vector2(0, 0));

                        vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, Main.screenHeight, 0), colors, new Vector2(0, 0));
                        vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), colors, new Vector2(0, 0));
                        vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), colors, new Vector2(0, 0));

                        vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                        vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

                        Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                        Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                        effect.CurrentTechnique.Passes["BasicEffectPass"].Apply();
                        Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);*/

                    }

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.DepthRead, RasterizerState.CullNone, null, identity);
                    Texture2D sun = Main.sunTexture;

                    float closenessScale = 1f + ((float)Math.Pow(((sunIsApprouching/30f)) * 500f, 4.80f) / 800f);
                    float closenessScale2 = closenessScale + ((float)Math.Pow(sunIsApprouching * 2500f, 1.20f) / 800f);

                    Texture2D inner = SGAmod.Instance.GetTexture("Extra_57b");//Main.extraTexture[57];


                    Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

                    Color lights = Color.Lerp(Color.White, Color.Orange, sunIsApprouching);

                    for (float i = 0; i < 1f; i += 0.10f)
                    {
                        spriteBatch.Draw(inner, sunPosition, null, ((lights * 0.6f * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.5f) * 0.50f) * skyalpha, i * MathHelper.TwoPi, textureOrigin, (2.5f * (0.5f + 3.00f * (((Main.GlobalTime / 2f) + i) % 1f))) * closenessScale2, SpriteEffects.None, 0f);
                    }

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, identity);

                    spriteBatch.Draw(sun, sunPosition, null, Color.Lerp(Color.White, Color.Orange, sunIsApprouching) * skyalpha, 0, sun.Size() / 2f, 1.5f * closenessScale, SpriteEffects.None, 0f);

                    //texture mappedTexture;
                    //float2 mappedTextureMultiplier;
                    //float2 mappedTextureOffset;

                    float alphaeffectSunShader = MathHelper.Clamp((closenessScale-1f)/3f, 0f, 1f);
                    float alphaeffectSunRayShader = MathHelper.Clamp((closenessScale - 1.65f) / 3f, 0f, 1f);

                    if (alphaeffectSunShader > 0)
                    {
                        Texture2D noise = ModContent.GetTexture("SGAmod/TiledPerlin");
                        Vector2 sunspotmoved = sunPosition+new Vector2(1.6f, 1.6f);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, identity);

                        Effect RadialEffect = SGAmod.RadialEffect;

                        RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Stain"));
                        RadialEffect.Parameters["alpha"].SetValue(alphaeffectSunShader);
                        RadialEffect.Parameters["texOffset"].SetValue(new Vector2(0,-Main.GlobalTime * 0.275f));
                        RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(1f, 0.5f));
                        RadialEffect.Parameters["ringScale"].SetValue(0.24f);
                        RadialEffect.Parameters["ringOffset"].SetValue(0.45f);
                        RadialEffect.Parameters["ringColor"].SetValue(Color.Lerp(Color.Yellow, Color.Red, sunIsApprouching * 1.25f).ToVector3());
                        RadialEffect.Parameters["tunnel"].SetValue(false);

                        RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

                        spriteBatch.Draw(noise, sunspotmoved, null, Color.White, 0, noise.Size() / 2f, 0.320f * closenessScale, SpriteEffects.None, 0f);

                        for (float ff = -1f; ff < 2; ff += 2)
                        {
                            RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("TrailEffect"));
                            RadialEffect.Parameters["alpha"].SetValue(alphaeffectSunRayShader);
                            RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f*ff, (-Main.GlobalTime+(ff>0 ? 343 : 0)) * 0.750f));
                            RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(10f, 1.5f));
                            RadialEffect.Parameters["ringScale"].SetValue(0.70f);
                            RadialEffect.Parameters["ringOffset"].SetValue(0.40f);
                            RadialEffect.Parameters["ringColor"].SetValue(Color.Lerp(Color.Yellow, Color.Red, sunIsApprouching / 1.10f).ToVector3());
                            RadialEffect.Parameters["tunnel"].SetValue(false);

                            RadialEffect.CurrentTechnique.Passes["RadialAlpha"].Apply();

                            spriteBatch.Draw(noise, sunspotmoved, null, Color.White, 0, noise.Size() / 2f, 0.280f * closenessScale, SpriteEffects.None, 0f);
                        }



                        SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Lerp(Color.White,Color.Orange, sunIsApprouching*2f).ToVector4() * alphaeffectSunShader);
                        SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(Main.blackTileTexture);
                        SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(1f, 1f));
                        SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(0, 0));
                        SGAmod.SphereMapEffect.Parameters["softEdge"].SetValue(20f);

                        SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMap"].Apply();

                        spriteBatch.Draw(noise, sunspotmoved, null, Color.White, 0, noise.Size() / 2f, 0.132f * closenessScale, SpriteEffects.None, 0f);

                        SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Red.ToVector4() * alphaeffectSunShader * 0.25f);
                        SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(ModContent.GetTexture("SGAmod/TiledPerlin"));
                        SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(0.5f, 0.5f));
                        SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(Main.GlobalTime / 32f, 0));

                        SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

                        spriteBatch.Draw(noise, sunspotmoved, null, Color.White, 0, noise.Size() / 2f, 0.132f * closenessScale, SpriteEffects.None, 0f);

                        SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Orange.ToVector4() * alphaeffectSunShader*0.25f);
                        SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(ModContent.GetTexture("SGAmod/Voronoi"));
                        SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(4f, 4f));
                        SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(Main.GlobalTime / 20f, 0));

                        SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

                        spriteBatch.Draw(noise, sunspotmoved, null, Color.White, 0, noise.Size() / 2f, 0.132f * closenessScale, SpriteEffects.None, 0f);

                        for (float f = 0.20f; f < 1f; f += 0.25f)
                        {
                            SGAmod.SphereMapEffect.Parameters["colorBlend"].SetValue(Color.Lerp(Color.Yellow, Color.Red, sunIsApprouching/1.10f).ToVector4() * (f / 2f)* alphaeffectSunShader);
                            SGAmod.SphereMapEffect.Parameters["mappedTexture"].SetValue(ModContent.GetTexture("SGAmod/TiledPerlin"));
                            SGAmod.SphereMapEffect.Parameters["mappedTextureMultiplier"].SetValue(new Vector2(f, f));
                            SGAmod.SphereMapEffect.Parameters["mappedTextureOffset"].SetValue(new Vector2(Main.GlobalTime / (10f + (f * 8f)), 0));

                            SGAmod.SphereMapEffect.CurrentTechnique.Passes["SphereMapAlpha"].Apply();

                            spriteBatch.Draw(noise, sunspotmoved, null, Color.White, 0, noise.Size() / 2f, 0.132f * closenessScale, SpriteEffects.None, 0f);
                        }

                    }

                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

                if (drawBoss)
                    boss.AlterSky(this, 2, spriteBatch, minDepth, maxDepth);

                /*for (float i = 0.04f; i < 0.35f; i += 0.005f)
    {
        for (int x = -alwaysthesame.Next(900, 3200); x < ((Main.maxTilesX * 16) * i) + Main.screenWidth; x += alwaysthesame.Next(900, 3200))
        {
            for (int y = -alwaysthesame.Next(900, 3200); y < ((Main.maxTilesY * 16) * i) + Main.screenHeight; y += alwaysthesame.Next(900, 3200))
            {
                Vector2 loc = ((-Main.screenPosition * i) + new Vector2(x, y));
                if (loc.X > -64 && loc.Y > -64 && loc.X < Main.screenWidth + 64 && loc.Y < Main.screenHeight + 64)
                {
                    spriteBatch.Draw(texx, loc, new Rectangle(0, 0, texx.Width, texx.Height / 13), (Color.White * MathHelper.Clamp(i, 0f, 1f)), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                }

            }
        }
    }*/

            }

            if (drawBoss)
                boss.AlterSky(this, 3, spriteBatch, minDepth, maxDepth);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

        }

        public override float GetCloudAlpha()
        {
            return 0f;
        }

        public override void Activate(Vector2 position, params object[] args)
        {
            this._isActive = true;
        }

        public override void Deactivate(params object[] args)
        {
            this._isActive = false;
        }

        public override void Reset()
        {
            this._isActive = false;
        }

        public override bool IsActive()
        {
            return this._isActive;
        }
    }

    public interface IMineableAsteriod
    {
        void DrawAsteriod(SpriteBatch spriteBatch, Color lightColor);

        void DrawAsteriodGlow(SpriteBatch spriteBatch, Color lightColor);

        void MineAsteriod(Item pickaxe,bool ChainMine);
        void AsteriodLoot();


    }

    public class MineableAsteriodOverseenCrystal : MineableAsteriod, IMineableAsteriod
    {
        public int[] gems;
        public int gemtype = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mineable Overseen Asteriod");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            asteriodHealth = 500;
        }

        public override void AI()
        {
            glowColor = Color.Lerp(Color.Blue, Color.White, 0.25f);
            base.AI();
        }

        protected override void _AsteriodLoot()
        {
            base._AsteriodLoot();

            for (int i = 0; i < 1 + (projectile.width / 4); i += 1)
            {
                Item.NewItem(projectile.position, projectile.width, projectile.height, ModContent.ItemType<OverseenCrystal>(), Main.rand.Next(1, 3));
            }
        }


        protected override void _DrawAsteriod(SpriteBatch spriteBatch, Color lightColor)
        {
             
            Texture2D tex = Main.itemTexture[ModContent.ItemType<OverseenCrystal>()];

            Vector2 vec = tex.Size() / 2f;
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            UnifiedRandom rand = new UnifiedRandom(projectile.whoAmI);

            Color colormix = Color.Lerp(lightColor, Color.SkyBlue, 0.75f);

            for (int i = 0; i < projectile.width * 0.80; i += 1)
            {
                Vector2 offset = Vector2.One.RotatedBy(rand.NextFloat(MathHelper.TwoPi));

                spriteBatch.Draw(tex, drawPos + (offset * (projectile.width + 4) * (projectile.width < 20 ? (rand.NextFloat(0.60f, 0.80f)) : (rand.NextFloat(0.40f, 0.60f)))).RotatedBy(projectile.rotation), null, colormix * rand.NextFloat(0.45f, 0.65f), projectile.rotation + (MathHelper.PiOver2) + offset.ToRotation(), vec, projectile.scale, SpriteEffects.None, 0f);
            }

            base._DrawAsteriod(spriteBatch, lightColor);

            for (int i = 0; i < projectile.width * 0.40; i += 1)
            {
                Vector2 offset = Vector2.One.RotatedBy(rand.NextFloat(MathHelper.TwoPi));

                spriteBatch.Draw(tex, drawPos + (offset * (projectile.width + 4) * (projectile.width < 20 ? (rand.NextFloat(0.1f, 1f)) : (rand.NextFloat(0.10f, 0.80f)))).RotatedBy(projectile.rotation), null, colormix * rand.NextFloat(0.25f, 0.75f), projectile.rotation + (MathHelper.PiOver2) + offset.ToRotation(), vec, projectile.scale, SpriteEffects.None, 0f);
            }

        }

    }

    public class MineableAsteriodCrystal : MineableAsteriod, IMineableAsteriod
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mineable Crystal Asteriod");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            asteriodHealth = 500;
        }

        public override void AI()
        {
            base.AI();
            if (projectile.ai[0] < 1)
            {
                glowColor = Color.Lerp(Color.Purple, Color.White, 0.50f);
            }
        }

        protected override void _AsteriodLoot()
        {
            base._AsteriodLoot();

            for (int i = 0; i < 1 + (projectile.width / 4); i += 1)
            {
                Item.NewItem(projectile.position, projectile.width, projectile.height, ItemID.CrystalShard, Main.rand.Next(1, 3));
            }
        }

        protected override void _DrawAsteriod(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = Main.itemTexture[(int)projectile.ai[0]];
            Texture2D textile = SGAmod.ExtraTextures[114];

            Vector2 vec = tex.Size() / 2f;
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            UnifiedRandom rand = new UnifiedRandom(projectile.whoAmI);

            for (int i = 0; i < projectile.width * 0.75; i += 1)
            {
                Vector2 offset = Vector2.One.RotatedBy(rand.NextFloat(MathHelper.TwoPi));

                int partof = rand.Next(textile.Width / 18);

                Point thisPart = new Point(partof * 18, 0);

                spriteBatch.Draw(textile, drawPos + (offset * (projectile.width + 4) * (projectile.width < 20 ? (rand.NextFloat(0.80f, 0.90f)) : (rand.NextFloat(0.60f, 0.80f)))).RotatedBy(projectile.rotation), new Rectangle(thisPart.X, thisPart.Y, 16, 16), Color.White, projectile.rotation + (MathHelper.PiOver2) + offset.ToRotation(), vec, projectile.scale, SpriteEffects.None, 0f);
            }

            base._DrawAsteriod(spriteBatch, lightColor);

        }

    }

    public class MineableAsteriodGem : MineableAsteriod, IMineableAsteriod
    {
        public int[] gems;
        public int gemtype = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mineable Gem Asteriod");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            asteriodHealth = 500;
        }

        public override void AI()
        {
            base.AI();
            if (projectile.ai[0] < 1)
            {
                gems = new int[] { ItemID.Amethyst, ItemID.Topaz, ItemID.Sapphire, ItemID.Emerald, ItemID.Ruby, ItemID.Diamond, ItemID.Amber };
                gemtype = Main.rand.Next((int)gems.Length);
                projectile.ai[0] = gems[gemtype];
                Color[] colors = new Color[] { Color.Purple, Color.Yellow, Color.Blue, Color.Lime, Color.Red, Color.Aquamarine, Color.Orange };
                //SGAmod.GemColors.TryGetValue(gemtype, out Color colorgem);
                glowColor = Color.Lerp(colors[gemtype], Color.White, 0.50f);
            }
        }

        protected override void _AsteriodLoot()
        {
            base._AsteriodLoot();

            for (int i = 0; i < 1 + (projectile.width / 4); i += 1)
            {
                Item.NewItem(projectile.position, projectile.width, projectile.height, (int)projectile.ai[0], Main.rand.Next(1, 3));
            }
        }


        protected override void _DrawAsteriod(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = Main.itemTexture[(int)projectile.ai[0]];
            Texture2D textile = SGAmod.ExtraTextures[113];

            Vector2 vec = tex.Size() / 2f;
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            UnifiedRandom rand = new UnifiedRandom(projectile.whoAmI);

            for (int i = 0; i < projectile.width * 0.75; i += 1)
            {
                Vector2 offset = Vector2.One.RotatedBy(rand.NextFloat(MathHelper.TwoPi));

                int partof = rand.Next(3);
                int partof2 = gemtype * 18;

                Point thisPart = new Point(partof2, partof * 18);

                spriteBatch.Draw(textile, drawPos + (offset * (projectile.width + 4) * (projectile.width < 20 ? (rand.NextFloat(0.80f, 0.90f)) : (rand.NextFloat(0.60f, 0.80f)))).RotatedBy(projectile.rotation), new Rectangle(thisPart.X, thisPart.Y, 16, 16), Color.White, projectile.rotation + (MathHelper.PiOver2) + offset.ToRotation(), vec, projectile.scale, SpriteEffects.None, 0f);
            }

            /*for (int i = 0; i < projectile.width*0.75; i += 1)
            {
                Vector2 offset = Vector2.One.RotatedBy(rand.NextFloat(MathHelper.TwoPi));

                spriteBatch.Draw(tex, drawPos + (offset * projectile.width*(rand.NextFloat(0.5f,0.85f))).RotatedBy(projectile.rotation), null, lightColor, projectile.rotation + (MathHelper.PiOver2)+ offset.ToRotation(), vec, projectile.scale, SpriteEffects.None, 0f);
            }*/

            base._DrawAsteriod(spriteBatch, lightColor);

        }

    }

    public class MineableAsteriod : ModProjectile, IMineableAsteriod
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mineable Asteriod");
        }

        public int miningTargeted = 0;
        public int asteriodHealth = 300;
        public float rotateAngle = 0;
        public Color glowColor = Color.White;
        public bool splitting = true;
        public int npc = -1;

        public static void SpawnAsteriods()
        {
            Player player = Main.LocalPlayer;

            if (Main.rand.Next(0, 100) == 0)
            {

                //int maxAsteriods = Main.projectile.Where(testby => testby?.modProjectile is IMineableAsteriod).Count();

                if (Main.rand.Next(0, 3) == 0)
                    return;

                Vector2[] edges = {
                    new Vector2(Main.rand.Next(-Main.screenWidth-160, Main.screenWidth + 160), -Main.screenHeight-Main.rand.Next(80, 160)),
                    new Vector2(Main.rand.Next(-Main.screenWidth-160, Main.screenWidth + 160), Main.screenHeight+Main.rand.Next(80, 160)),

                    new Vector2(-Main.screenWidth-Main.rand.Next(80, 160),Main.rand.Next(-Main.screenHeight-160, Main.screenHeight + 160)),
                    new Vector2(Main.screenWidth+Main.rand.Next(80, 160),Main.rand.Next(-Main.screenHeight-160, Main.screenHeight + 160)),
            };

                Vector2 chosenspot = edges[Main.rand.Next(edges.Length)];
                Vector2 velocity = Vector2.Normalize((chosenspot + player.Center) - player.Center);

                velocity = velocity.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * Main.rand.NextFloat(0.5f, 2f);

                int projtype = ModContent.ProjectileType<MineableAsteriod>();

                if (Main.rand.Next(0, 8) == 1)
                    projtype = ModContent.ProjectileType<MineableAsteriodCrystal>();

                if (Main.rand.Next(0, 5) == 1)
                    projtype = ModContent.ProjectileType<MineableAsteriodGem>();


                if (Main.rand.Next(0, 10) == 1)
                {
                    //&& SpaceDim.crystalAsteriods)
                    if (!SGAmod.SpaceBossActive || SpaceDim.crystalAsteriods)
                        projtype = ModContent.ProjectileType<MineableAsteriodOverseenCrystal>();
                }


                Projectile proj = Projectile.NewProjectileDirect(player.Center + chosenspot, velocity, projtype, 0, 0);

                if (Main.rand.Next(0, 100) <= 2)
                {
                    int npc = NPC.NewNPC((int)proj.Center.X, (int)proj.Center.Y, ModContent.NPCType<OverseenHeadAsteriod>());
                    MineableAsteriod grabrock = proj.modProjectile as MineableAsteriod;
                    grabrock.npc = npc;
                    grabrock.projectile.damage = 40;
                    grabrock.projectile.timeLeft = 99999;
                    grabrock.projectile.hostile = true;
                    grabrock.projectile.friendly = false;
                    grabrock.projectile.ignoreWater = false;
                }
            }

        }

        public int vanityLook = 0;

        public override string Texture => "SGAmod/Projectiles/FieryRock";

        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 10;
            projectile.width = 24;
            projectile.height = 24;
            projectile.tileCollide = true;
            projectile.timeLeft = 60 * 30;
            projectile.damage = 0;
            projectile.ignoreWater = false;
            rotateAngle = Main.rand.NextFloat(0.01f, 0.05f) * (Main.rand.NextBool() ? 1f : -1f);
            vanityLook = Main.rand.Next(0, 100);
        }

        public override bool CanDamage()
        {
            return true;
        }

        public void DrawAsteriod(SpriteBatch spriteBatch, Color lightColor) => _DrawAsteriod(spriteBatch, lightColor);
        public void DrawAsteriodGlow(SpriteBatch spriteBatch, Color lightColor) => _DrawAsteriodGlow(spriteBatch, lightColor);
        public void MineAsteriod(Item pickaxe, bool chainMine = true) => _MineAsteriod(pickaxe, chainMine);
        public void AsteriodLoot() => _AsteriodLoot();

        protected virtual void _MineAsteriod(Item pickaxe,bool chainMine=true)
        {
            int minedamage = pickaxe.pick;
            asteriodHealth -= minedamage;

            if (chainMine)
            {
                int dist = 64 * 64;
                foreach (Projectile asteriod in Main.projectile.Where(testby => testby.active && testby.whoAmI != projectile.whoAmI && testby.modProjectile != null && testby.modProjectile is IMineableAsteriod && (testby.Center-projectile.Center).LengthSquared()<dist).OrderBy(testby => (testby.Center - projectile.Center).LengthSquared()))
                {
                    if (projectile.whoAmI != asteriod.whoAmI)
                    {
                        IMineableAsteriod asteriod2 = asteriod.modProjectile as IMineableAsteriod;
                        Item copyPick = pickaxe.Clone();
                        copyPick.pick /= 2;
                        asteriod2.MineAsteriod(copyPick, false);
                    }
                }

            }

            if (asteriodHealth < 1)
            {
                AsteriodLoot();
                projectile.Kill();
            }
            else
            {
                SoundEffectInstance snd = Main.PlaySound(SoundID.Tink, (int)projectile.Center.X, (int)projectile.Center.Y);
                if (snd != null)
                {
                    //snd.Pitch = -0.50f;
                }
            }
        }

        protected virtual void _AsteriodLoot()
        {
            List<(int, int)> loot = new List<(int, int)>();

            for (int i = 0; i < 1 + (projectile.width / 3); i += 1)
            {
                List<int> stuff = new List<int>();
                for (int a = 0; a < 2; a += 1)
                {
                    stuff.Add(ItemID.FossilOre); stuff.Add(ItemID.Meteorite); stuff.Add(ItemID.Meteorite); stuff.Add(ItemID.StoneBlock); stuff.Add(ItemID.StoneBlock); stuff.Add(ItemID.StoneBlock);
                    stuff.Add(ModContent.ItemType<Glowrock>());
                }

                if (Main.rand.Next(0, 5) == 0)
                    stuff.Add(ModContent.ItemType<BubblePickup>());

                if (Main.rand.Next(-5, 6) == 0)
                    stuff.Add(ItemID.Amber);
                if (Main.rand.Next(-5, 6) == 0)
                    stuff.Add(ItemID.Amethyst);
                if (Main.rand.Next(-5, 10) == 0)
                    stuff.Add(ItemID.Diamond);
                if (Main.rand.Next(-5, 6) == 0)
                    stuff.Add(ItemID.Emerald);
                if (Main.rand.Next(-5, 6) == 0)
                    stuff.Add(ItemID.Ruby);
                if (Main.rand.Next(-5, 6) == 0)
                    stuff.Add(ItemID.Sapphire);
                if (Main.rand.Next(-5, 6) == 0)
                    stuff.Add(ItemID.Topaz);
                if (Main.rand.Next(-5, 15) == 0)
                    stuff.Add(ItemID.DD2ElderCrystal);

                int itemtype = stuff[Main.rand.Next(stuff.Count)];

                loot.Add((itemtype, itemtype == ModContent.ItemType<BubblePickup>() ? 1 : Main.rand.Next(1, 5)));
            }

            SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_MonkStaffGroundImpact, (int)projectile.Center.X, (int)projectile.Center.Y);
            if (snd != null)
            {
                snd.Pitch = -0.50f;
            }

            foreach ((int, int) stuff in loot)
                Item.NewItem(projectile.position, projectile.width, projectile.height, stuff.Item1, stuff.Item2);

            for (int num654 = 0; num654 < 12; num654++)
            {
                Vector2 dir = (Vector2.UnitX).RotateRandom(MathHelper.TwoPi);
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
                int num655 = Dust.NewDust(projectile.Center + dir * projectile.width, 0, 0, 23, dir.X * (num654 / 2f), dir.Y * (num654 / 2f), 100, new Color(30, 30, 30, 20), 2f);
                Main.dust[num655].noGravity = true;
                Main.dust[num655].scale = projectile.width / 10f;

            }

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
                SoundEffectInstance snd = Main.PlaySound(SoundID.Item10, (int)projectile.Center.X, (int)projectile.Center.Y);
                if (snd != null)
                {
                    snd.Pitch = -0.75f;
                }
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
            if (npc >= 0 && Main.npc[npc].active)
            {
                NPC npc2 = Main.npc[npc];
                npc2.StrikeNPC(npc2.lifeMax * 2, 0, 0);
            }

            if (!splitting)
                return true;

            for (int i = 0; i < Main.rand.Next(1, 3); i += 1)
            {
                if (projectile.width > 20)
                {
                    Vector2 velocity = projectile.velocity.RotatedByRandom(MathHelper.TwoPi) + (Vector2.One.RotatedByRandom(MathHelper.TwoPi) / 5f);
                    velocity *= Main.rand.NextFloat(-2f, 2f);

                    Projectile proj = Projectile.NewProjectileDirect(projectile.Center, projectile.velocity + velocity, projectile.type, projectile.damage / 2, projectile.knockBack);
                    proj.width /= 2;
                    proj.height /= 2;
                    proj.ai[0] = projectile.ai[0];
                    ((MineableAsteriod)(proj.modProjectile)).glowColor = glowColor;

                    if (GetType() == typeof(MineableAsteriodGem))
                        ((MineableAsteriodGem)(proj.modProjectile)).gemtype = ((MineableAsteriodGem)this).gemtype;
                }
            }

            return true;
        }

        public override void AI()
        {
            if (npc >= 0 && Main.npc[npc].active)
            {
                NPC npc2 = Main.npc[npc];
                projectile.Center = npc2.Center;
                projectile.velocity = npc2.velocity;
                projectile.tileCollide = false;
                (npc2.modNPC as OverseenHeadAsteriod).timer = 3;
            }
            else
            {
                projectile.tileCollide = true;
                npc = -1;
            }

            miningTargeted = (int)MathHelper.Clamp(miningTargeted - 1, 0, 20);
            projectile.rotation += rotateAngle;

            if (projectile.damage > 0 && npc < 0)
            {
                if (projectile.velocity.Length() > 3f)
                    projectile.velocity *= 0.97f;
                else
                    projectile.damage = 0;
            }

            //int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 20, default(Color), 1f);
            //Main.dust[DustID2].noGravity = true;
        }

        protected virtual void _DrawAsteriod(SpriteBatch spriteBatch, Color lightColor)
        {
            lightColor = Lighting.GetColor((int)projectile.Center.X >> 4, (int)projectile.Center.Y >> 4);

            int vanitytype = (3 + (vanityLook % 3));
            Texture2D tex;

            Color blending = Color.White;

            if (projectile.width < 20)
                tex = mod.GetTexture(vanityLook < 10 ? (vanityLook < 5 ? "Dimensions/Space/MeteorSmall2" : "Dimensions/Space/MeteorSmall") : "Dimensions/Space/MeteorSmall" + (3 + (vanityLook % 3)));
            else
                tex = mod.GetTexture(vanityLook < 10 ? (vanityLook < 5 ? "Dimensions/Space/MeteorLarge2" : "Dimensions/Space/MeteorLarge") : "Dimensions/Space/MeteorLarge" + (3 + (vanityLook % 3)));

            if (GetType() == typeof(MineableAsteriodOverseenCrystal))
            {
                if (projectile.width < 20)
                    blending = Color.PaleTurquoise;
                else
                    tex = mod.GetTexture((new string[3] { "Dimensions/Space/GlowAsteriod", "Dimensions/Space/GlowAsteriodalt", "Dimensions/Space/GlowAsteriodalt2" })[vanityLook%3]);
            }

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 2f);
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            Rectangle size = new Rectangle(0, 0, tex.Width, (int)(drawOrigin.Y));
            Rectangle sizeOutline = new Rectangle(0, (int)(drawOrigin.Y), tex.Width, (int)(drawOrigin.Y));

            //Main.spriteBatch.End();
            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(tex, drawPos, size, lightColor.MultiplyRGB(blending), projectile.rotation, drawOrigin / 2f, projectile.scale, SpriteEffects.None, 0f);
            spriteBatch.Draw(tex, drawPos, sizeOutline, Color.Red * (miningTargeted / 20f), projectile.rotation, drawOrigin / 2f, projectile.scale, SpriteEffects.None, 0f);
        }

        protected virtual void _DrawAsteriodGlow(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D texGlow = mod.GetTexture("GlowOrb");
            Vector2 drawPos = projectile.Center - Main.screenPosition;

            spriteBatch.Draw(texGlow, drawPos, null, glowColor * 0.5f, 0, texGlow.Size() / 2f, projectile.scale * (projectile.width / 64f), SpriteEffects.None, 0f);
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            int fill = 0;
            fill = 3;
        }
    }

    public class AsteriodDraw : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Draw Asteriod");
        }
        public override void SetDefaults()
        {
            Projectile refProjectile = new Projectile();
            refProjectile.SetDefaults(ProjectileID.Boulder);
            aiType = ProjectileID.Boulder;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.penetrate = 10;
            projectile.width = 24;
            projectile.height = 24;
            projectile.tileCollide = true;
            projectile.timeLeft = 2;
            projectile.damage = 0;
        }

        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            /*
            Texture2D sunText = Main.sunTexture;
            foreach (FilledSpaceArea area in SpaceDim.FilledSpaces)
            {
                spriteBatch.Draw(sunText, area.position - Main.screenPosition, null, Color.White, 0, sunText.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }

            foreach (Vector2 area in SpaceDim.EmptySpaces)
            {
                spriteBatch.Draw(sunText, area - Main.screenPosition, null, Color.Blue, 0, sunText.Size() / 2f, 1f, SpriteEffects.None, 0f);
            }
            */

            List<Projectile> Asteriods = Main.projectile.Where(testby => testby.active && testby?.modProjectile is IMineableAsteriod).ToList();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile roid in Asteriods)
            {
                IMineableAsteriod asteriod = (IMineableAsteriod)roid.modProjectile;
                asteriod.DrawAsteriodGlow(spriteBatch,lightColor);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            foreach (Projectile roid in Asteriods)
            {
                IMineableAsteriod asteriod = (IMineableAsteriod)roid.modProjectile;
                asteriod.DrawAsteriod(spriteBatch, lightColor);
            }

            return false;
        }

        public override string Texture => "SGAmod/Projectiles/FieryRock";

    }

    public class OverseenHeadAsteriod : OverseenHead
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseen Asteriod");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.dontTakeDamage = false;
            npc.defense = 100;
            npc.life = 2000;
            npc.lifeMax = 2000;
            npc.damage = 0;
            timer = 5;
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale += 0.250f;
            position += new Vector2(0,16f);
            return true;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return false;
        }

    }

        public class OverseenHead : ModNPC
    {
        public int timer = Int32.MaxValue;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Overseen Head");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.CloneDefaults(NPCID.MeteorHead);

            npc.knockBackResist = 0.25f;
            npc.life *= 3;
            npc.lifeMax *= 3;
            //npc.defense += 15;
            //npc.defDamage += 15;

            npc.aiStyle = 74;

            if (Main.rand.Next(0, 3) < 2 || SpaceDim.SpaceBossIsActive)
            {
                npc.aiStyle = Main.rand.NextBool() ? 86 : 56;
            }
        }

        public override bool CheckActive()
        {
            if (NPC.CountNPCS(ModContent.NPCType<SpaceBoss>()) > 0)
                return false;

            return base.CheckActive();
        }
        public override bool PreNPCLoot()
        {
            //TileID.ExposedGems
            npc.type = NPCID.MeteorHead;
            if (GetType() == typeof(OverseenHeadBossShield))
                Item.NewItem(npc.position, npc.width, npc.height, ItemID.Heart);
            else
                Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Glowrock>(), Main.rand.Next(1, 4));

            return true;
        }
        public override string Texture => "SGAmod/Dimensions/Space/OverseenHead";

        public override void AI()
        {
            timer -= 1;
            if (timer < 1)
            {
                npc.StrikeNPC(npc.lifeMax * 2, 0, 0);
            }
            if (npc.aiStyle != 56)
            {
                int num825 = Dust.NewDust(npc.position, npc.width, npc.height, 180);
                Dust dust40 = Main.dust[num825];
                Dust dust2 = dust40;
                dust2.velocity *= 0.1f;
                Main.dust[num825].scale = 1.3f;
                Main.dust[num825].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            Texture2D tex = Main.npcTexture[npc.type];

            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height);
            Vector2 drawPos = npc.Center - Main.screenPosition;

            Texture2D texGlow = mod.GetTexture("GlowOrb");

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);


            spriteBatch.Draw(texGlow, drawPos, null, Color.Blue * 0.75f*npc.Opacity, 0, texGlow.Size() / 2f, 0.15f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            spriteBatch.Draw(tex, drawPos, null, Color.Lerp(lightColor,Color.White, 0.5f) * npc.Opacity, npc.rotation, drawOrigin / 2f, npc.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            //
        }

    }
    public class BubblePickup : EnchantedBubble, IConsumablePickup
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Air Bubble");
            ItemID.Sets.ItemNoGravity[item.type] = false;
        }

        public override bool OnPickup(Player player)
        {
            player.SGAPly().RestoreBreath(20*item.stack);
            return false;
        }

    }


}