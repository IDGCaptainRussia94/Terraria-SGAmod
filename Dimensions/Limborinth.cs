using System.IO;
using System;
using System.Linq;
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
using SGAmod.Dimensions.Tiles;

namespace SGAmod.Dimensions
{
    public class MazeRoom
    {
        public int gen;
        public Point16 loc;
        public bool deadEnd;
        public Point16 Offset;
        public int roomIndex;
        public MazeRoom ConnectedRoom = default;
        public Point16 Position => loc + Offset;

        public MazeRoom previousRoom = default;
        public MazeRoom(Point16 loc, int gen,Point16 Offset, int roomIndex, bool deadEnd = false)
        {
            this.gen = gen;
            this.loc = loc;
            this.Offset = Offset;
            this.roomIndex = roomIndex;
            this.deadEnd = deadEnd;
        }

    }

    public class LimborinthLoad : UIDefaultSubworldLoad
    {
        public static LimborinthLoad instance;
        float turning = 0f;
        float grow = 0f;
        public override void OnInitialize()
        {
            turning = 0f;
            instance = this;
            base.OnInitialize();
        }
        public override void Update(GameTime gameTime)
        {

        }
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            //SLWorld.progress.TotalProgress;
            base.DrawSelf(spriteBatch);
            grow = MathHelper.Clamp(grow + (1f/1200f),0f,1f);
            turning += MathHelper.TwoPi / 180f;
            UnifiedRandom alwaysthesame = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);

            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, null, null, null, Main.UIScaleMatrix);

            Vector2 loc = new Vector2(Main.screenWidth, Main.screenHeight) / 2f;
            Texture2D texx = ModContent.GetTexture("SGAmod/Items/WatchersOfNull");
            Vector2 offset = new Vector2(texx.Width, texx.Height / 13) / 2f;
            //if (SLWorld.progress != null)
            spriteBatch.Draw(texx, loc, new Rectangle(0, 0, texx.Width, texx.Height / 13), Color.White, 0, offset, Vector2.One * grow * 20f, SpriteEffects.None, 0f);


            // spriteBatch.End();
            //spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.UIScaleMatrix);

            Effect RadialEffect = SGAmod.RadialEffect;

            Texture2D mainTex = SGAmod.Instance.GetTexture("GreyHeart");//Main.projectileTexture[projectile.type];

            RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Space"));
            RadialEffect.Parameters["alpha"].SetValue(1f);
            RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.025f, Main.GlobalTime * 0.175f));
            RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(3f, 1.5f));
            RadialEffect.Parameters["ringScale"].SetValue(0.85f);
            RadialEffect.Parameters["ringOffset"].SetValue(1f);
            RadialEffect.Parameters["ringColor"].SetValue(Color.Red.ToVector3());
            RadialEffect.Parameters["tunnel"].SetValue(true);

            RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

            spriteBatch.Draw(mainTex, new Vector2(Main.screenWidth, Main.screenHeight) /2f, null, Color.White, 0, mainTex.Size() / 2f, Main.screenWidth/ 16f, default, 0);

            RadialEffect.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));
            RadialEffect.Parameters["alpha"].SetValue(0.5f);
            RadialEffect.Parameters["texOffset"].SetValue(new Vector2(Main.GlobalTime * 0.0125f, Main.GlobalTime * 0.205f));
            RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(4.2f, 0.25f));
            RadialEffect.Parameters["ringScale"].SetValue(1.15f);
            RadialEffect.Parameters["ringOffset"].SetValue(1.25f);
            RadialEffect.Parameters["ringColor"].SetValue(Color.DarkRed.ToVector3());
            RadialEffect.Parameters["tunnel"].SetValue(true);

            RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

            spriteBatch.Draw(mainTex, new Vector2(Main.screenWidth, Main.screenHeight) / 2f, null, Color.White, 0, mainTex.Size() / 2f, Main.screenWidth / 16f, default, 0);


            spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
        }

    }

        public class Limborinth : SGAPocketDim
    {
        public override int width => 3200;
        public override int height => 2400;
        public override bool saveSubworld => false;

        public override string DimName => "Seed of Evil";
        public override UIState loadingUIState => new LimborinthLoad();

        public override UserInterface loadingUI => base.loadingUI;

        public override Texture2D GetMapBackgroundImage()
        {
            return SGAmod.Instance.GetTexture("SeedOfEvilMapBackground");
        }

        public override int? Music
        {

            get
            {
                return SGAmod.Instance.GetSoundSlot(SoundType.Music, "Sounds/Music/Silence");
            }

        }

        public static HashSet<Point16> InnerArenaTiles;
        public static List<MazeRoom> MazeGraphPoints;
        public override void Unload()
        {
            InnerArenaTiles.Clear();
            MazeGraphPoints.Clear();
        }

        private Point16 Flow(Point16 point,int scalesize)
        {
            if (scalesize<1000)
            return new Point16(0, 0);

            double noise = (Noisegen.Noise(point.X, point.Y)-0.5)* scalesize;
            double noise2 = (Noisegen.Noise(point.X-50000, point.Y-50000)-0.5)* scalesize;
            return new Point16((int)noise, (int)noise2);
        }

        private bool Avoid(Point16 point)
        {
            for (int xx = -roomsize; xx <= roomsize; xx += 1)
            {
                for (int yy = -roomsize; yy <= roomsize; yy += 1)
                {
                    Tile tile = Main.tile[point.X + xx, point.Y + yy];
                    if (tile.type == TileID.Adamantite)
                        return true;
                }
            }

            return false;

        }

        public override int DimType => 100;

        NoiseGenerator Noisegen;
        UnifiedRandom UniRand;
        int sizeCheck = 40;
        int roomsize = 6;
        int bufferSize = 100;
        int flowSize = 32;
        int surfaceDeep = 200;
        int maxGen = 0;
        int gen = 0;
        int noiseDetail = 3;
        public Rectangle BossRoom;
        public Rectangle BossRoomInside;
        public Point16 currentPosition;
        public Point16 startingPoint;
        public MazeRoom LastRoom=null;
        //HashSet<MazeRoom> mazePainter;
        List<MazeRoom> mazePainter;
        List<MazeRoom> mazeRooms;

        public virtual void AGenPass(GenerationProgress prog)
        {
            UniRand = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);
            int lastseed = WorldGen._genRandSeed;
            WorldGen._genRandSeed = DimDungeonsProxy.DungeonSeeds;
            enemyseed = (DimDungeonsProxy.DungeonSeeds);
            prog.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            //Main.spawnTileX = (Main.maxTilesX / 2) / 16;
            prog.Message = "Once was nothing...";

        Restart:

            //Rectangle BoxedArea = new Rectangle(bufferSize, bufferSize + surfaceDeep, width - bufferSize * 2, (height - surfaceDeep) - (bufferSize * 2));

            //currentPosition = new Point16(width / 2 + (UniRand.Next(12) * sizeCheck) - sizeCheck * 6, height-200);
            currentPosition = new Point16(width / 2, height / 2);

            BossRoom = new Rectangle(currentPosition.X-160, currentPosition.Y-100, 320, 200);
            BossRoomInside = new Rectangle(BossRoom.Center.X - 100, BossRoom.Center.Y - 60, 200, 120);

            currentPosition = new Point16(currentPosition.X + (UniRand.NextBool() ? 1 : -1) * ((BossRoom.Width / 2) + sizeCheck / 2), currentPosition.Y);


            int xloc = 200 + UniRand.Next(width - 400);
            Vector2 center = new Vector2(width, height) / 2f;
            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    prog.Message = ((x * y) + y) / (float)(width * height)*100f + "%";
                    float dist = (new Vector2(x, y) - center).LengthSquared();
                    Main.tile[x, y].type = (BossRoom.Contains(x, y) || dist>(height* height) / 8f) ? TileID.Adamantite : TileID.CobaltBrick;
                    Main.tile[x, y].active(true);
                }
            }

            for (int i = 0; i < 70; i += 1)
            {
                int randomx = UniRand.Next(Main.maxTilesX);
                int randomy = UniRand.Next(surfaceDeep, Main.maxTilesY);
                Vector2 vex = new Vector2(randomx, randomy);
                if ((currentPosition.ToVector2() - vex).LengthSquared() > bufferSize * bufferSize)
                    IDGWorldGen.TileRunner(randomx, randomy, (double)UniRand.Next(30, 100), UniRand.Next(15, 45) + i, TileID.Adamantite, false, 0f, 0f, true, true,UniRand);
            }

            Noisegen = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds);
            Noisegen.Amplitude = 1;
            Noisegen.Frequency *= 0.50;

            gen = 0;

            mazePainter = new List<MazeRoom>();// HashSet<MazeRoom>();
            mazeRooms = new List<MazeRoom>();

            MazeGenerator(currentPosition, 0, 20000, prog);

            NoiseGenerator Noisegen2 = new NoiseGenerator(DimDungeonsProxy.DungeonSeeds);
            Noisegen2.Amplitude = 1;
            Noisegen2.Frequency *= 0.050;

            int adder = 0;
            int noisy = 0;

            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    Tile tile = Main.tile[x, (height-1)-y];
                    tile.type = TileID.CobaltBrick;
                    tile.active(false);

                    prog.Value = (((x + (y*x)) / (float)(width * height)) * 1f);
                    float dist = (new Vector2(x, y) - center).LengthSquared();
                    if (!(dist > (height * height) / 8f))
                    {
                        tile.type = (ushort)ModContent.TileType<HardenedFabric>();
                        tile.active(true);
                    }
                    int dister = (height * height) / 5;

                    if (adder == 0)
                        noisy = (int)(Noisegen2.Noise(x, y) * (1000 * 1000));

                        adder = (adder+1)%noiseDetail;

                    if ((tile.wall == WallID.DirtUnsafe || tile.wall == WallID.DirtUnsafe1 || tile.wall == WallID.DirtUnsafe2 || tile.wall == WallID.DirtUnsafe3 || tile.wall == WallID.DirtUnsafe4) || (dist > noisy+(dister+(y*y)/4f)))
                    {
                        tile.type = (ushort)ModContent.TileType<HardenedFabric>();
                        tile.wall = 0;
                        tile.active(true);
                    }

                }
            }


            MazePainter();

            RoomFiller();

            PokeExits(5);

            BossBox();

            Main.spawnTileX = width / 2;
            Main.spawnTileY = 100;

            //Celular Crap

            /*for(int passes = 0; passes < 5; passes += 1) {
                for (int x = 0; x < Main.maxTilesX; x += 1)
                {
                    for (int y = 0; y < Main.maxTilesY; y += 1)
                    {
                        if (GetTilesAround(x,y,1)>4)
                        Main.tile[x, y].active(true);
                        else
                        Main.tile[x, y].active(false);
                    }
                }
            }*/

        }

        private void MazeGenerator(Point16 starting,int roomIndex, int maxsize,GenerationProgress prog)
        {

            Stack<MazeRoom> mazeStack = new Stack<MazeRoom>();
            maxGen = 0;
            int index = 0;

            currentPosition = starting;
            MazeRoom mazz = new MazeRoom(currentPosition, gen, Flow(currentPosition, flowSize), roomIndex);
            mazeStack.Push(mazz);
            if (roomIndex == 0)
            {
                startingPoint = mazz.loc + mazz.Offset;
                IDGWorldGen.PlaceMulti(startingPoint.ToPoint(), TileID.Adamantite, roomsize * 3);
            }

        ResetMove:
            //And now, we gen a maze with depth first!
            //Ah Adamantite, my good friend! Please help me keep track of where we've been!

            while (mazeStack.Count > 0 && index < maxsize)
            {
                MazeRoom whereWeAre = mazeStack.Peek();
                //gen = whereWeAre.gen;
                maxGen = Math.Max(maxGen, gen);

                Point16 newpoint = currentPosition;

                prog.Value = MathHelper.Clamp(mazeStack.Count / 100f, 0f, 1f);
                prog.Message = whereWeAre.loc.X + ":" + whereWeAre.loc.Y + " Gen " + gen;


                int offset = UniRand.Next(4);

                for (int i = 0; i < 4; i += 1)
                {
                    //Check Card Directions

                    //Point16 cardOffset = (DarkSector.Cardinals[(offset + i) % DarkSector.Cardinals.Length]) * new Point16(sizeCheck, sizeCheck);
                    Point16 cardOffset = (Vector2.UnitX* sizeCheck).RotatedBy(UniRand.NextFloat(MathHelper.TwoPi)).ToPoint16();
                    //(pointhere * sizeCheck).ToPoint16();//(DarkSector.Cardinals[(offset + i) % DarkSector.Cardinals.Length])*sizeCheck;
                    Point16 checkHere = currentPosition + (cardOffset);

                    if (checkHere.X < sizeCheck || checkHere.X > width - sizeCheck || checkHere.Y < sizeCheck || checkHere.Y > height - sizeCheck)
                        continue;

                    if (Avoid(checkHere))
                        continue;

                    //Found one, moving

                    // Main.tile[checkHere.X, checkHere.Y].type = TileID.Adamantite;
                    //Main.tile[checkHere.X, checkHere.Y].active(true);

                    //Place down some stuff to mark this area so we don't move back here
                    IDGWorldGen.PlaceMulti(checkHere.ToPoint(), TileID.Adamantite, roomsize * 3);
                    
                    //Draw line and connect the dots
                    Point check1 = (currentPosition + Flow(currentPosition, flowSize)).ToPoint();
                    Point check2 = (checkHere + Flow(checkHere, flowSize)).ToPoint();

                    foreach (Point there in IDGWorldGen.GetLine(check1, check2))
                    {
                        Point16 there2 = new Point16(there.X, there.Y);
                        if (InsideMap(there2.X, there2.Y))
                        {
                            Tile tileline = Main.tile[there2.X, there2.Y];
                            //tileline.color((byte)FakeOverworld.Paints.Negative);
                            mazePainter.Add(new MazeRoom(there2, gen, new Point16(0, 0), roomIndex));
                        }
                    }

                    index += 1;
                    gen += 1;
                    MazeRoom mazepoint = new MazeRoom(checkHere, gen, Flow(currentPosition, flowSize), roomIndex);
                    mazepoint.ConnectedRoom = whereWeAre;

                    if (LastRoom == null || gen > LastRoom.gen)
                    LastRoom = mazepoint;

                    mazeStack.Push(mazepoint);
                    mazeRooms.Add(whereWeAre);

                    currentPosition = checkHere;

                    goto ResetMove;

                }

                //Dead end, time to step back

                MazeRoom stepBack = mazeStack.Pop();
                MazeRoom previous = stepBack;
                if (mazeStack.Count > 1)
                {
                    previous = mazeStack.Peek();
                    previous.deadEnd = false;
                }

                if (!mazeRooms.Any(tester => tester.loc == currentPosition))
                    mazeRooms.Add(new MazeRoom(currentPosition, gen, Flow(currentPosition, flowSize), roomIndex, true));

                gen -= 1;
                currentPosition = previous.loc;

            }

            mazePainter.OrderBy(orderby => orderby.gen);
            mazeRooms.OrderBy(orderby => orderby.gen);

            /*MazeRoom spawnRoom = mazeRooms[mazeRooms.Count - 1];
            Main.spawnTileX = spawnRoom.loc.X + spawnRoom.Offset.X;
            Main.spawnTileY = spawnRoom.loc.Y + spawnRoom.Offset.Y;*/

        }

        private void PokeExits(int numExits)
        {
            int buffersmoller = height - 10;
            int distanceToCenter = (buffersmoller * buffersmoller);

            Vector2 center = new Vector2(width / 2, height / 2);

            roomsize = 5;

            foreach (MazeRoom outermostRooms in mazeRooms.Where(testby => testby.gen>maxGen*0.75f).OrderBy(testby => 0-(new Vector2(testby.loc.X, testby.loc.Y) - new Vector2(width / 2, height / 2)).LengthSquared()).Take(numExits))
            {
                Point check1 = outermostRooms.loc.ToPoint();
                Point pointvec = (Vector2.Normalize(check1.ToVector2() - center) * 32).ToPoint();
                Point check2 = new Point(check1.X+pointvec.X, check1.Y+pointvec.Y);



                for (int xx = -roomsize; xx <= roomsize; xx += 1)
                {
                    for (int yy = -roomsize; yy <= roomsize; yy += 1)
                    {
                        foreach (Point there in IDGWorldGen.GetLine(check1, check2))
                        {
                            Point16 there2 = new Point16(there.X+xx, there.Y+yy);
                            if (InsideMap(there2.X, there2.Y))
                            {
                                Tile tileline = Main.tile[there2.X, there2.Y];
                                if (tileline.active())
                                {
                                    tileline.active(false);
                                    tileline.type = 0;// TileID.AmberGemspark;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void MazePainter()
        {
            foreach (MazeRoom mazeRoom in mazePainter)
            {
                roomsize = UniRand.Next(2, UniRand.Next(2, 8));
                for (int xx = -roomsize; xx < roomsize + 1; xx += 1)
                {
                    for (int yy = -roomsize; yy < roomsize + 1; yy += 1)
                    {
                        Point16 loc = mazeRoom.loc + mazeRoom.Offset + new Point16(xx, yy);
                        if (InsideMap(loc.X, loc.Y))
                        {
                            Tile tileline = Main.tile[loc.X, loc.Y];
                            //tileline.type = TileID.Diamond;
                            tileline.active(false);

                            if (mazeRoom.gen > maxGen * 0.75)
                            {
                                tileline.wall = 0;
                                continue;
                            }

                            if (mazeRoom.gen > maxGen * 0.5)
                            {
                                tileline.wall = (ushort)SGAmod.Instance.WallType("CorruptionFakeWall");
                                continue;
                            }

                            if (mazeRoom.gen > maxGen * 0.25)
                            {
                                tileline.wall = (ushort)SGAmod.Instance.WallType("CrimsonFakeWall");
                                continue;
                            }

                            tileline.wall = (ushort)SGAmod.Instance.WallType("NullWall");
                            //WallID.Sets.
                            //tileline.color((byte)FakeOverworld.Paints.Negative);
                        }
                    }
                }
            }
        }

        Point16 Middle => new Point16(width / 2, height / 2);

        public void RoomFiller()
        {
            MazeGraphPoints = new List<MazeRoom>(mazeRooms);

            List<MazeRoom> OuterRooms = (mazeRooms.FindAll(testby => testby.gen > maxGen * 0.75)).OrderBy(randomize => UniRand.Next(10000)).ToList();
            List<MazeRoom> CorruptionRooms = (mazeRooms.FindAll(testby => testby.gen > maxGen * 0.5 && testby.gen <= maxGen * 0.75)).OrderBy(randomize => UniRand.Next(10000)).ToList();
            List<MazeRoom> CrimsonRooms = (mazeRooms.FindAll(testby => testby.gen > maxGen * 0.25 && testby.gen <= maxGen * 0.50)).OrderBy(randomize => UniRand.Next(10000)).ToList();
            List<MazeRoom> InnerRooms = (mazeRooms.FindAll(testby => testby.gen <= maxGen * 0.25)).OrderBy(randomize => UniRand.Next(10000)).ToList();

            EvilSegmentRooms(CorruptionRooms,0);

            EvilSegmentRooms(CrimsonRooms, 1);


            foreach (MazeRoom mazeRoom in InnerRooms)
            {
                if (mazeRoom.deadEnd)
                {
                    for (int xx = -2; xx < 2; xx += 1)
                    {
                        for (int yy = -2; yy < 2; yy += 1)
                        {
                            Point16 loc = mazeRoom.loc + mazeRoom.Offset + new Point16(xx, yy);
                            if (InsideMap(loc.X, loc.Y))
                            {
                                Tile tileline = Main.tile[loc.X, loc.Y];

                                tileline.type = TileID.DiamondGemspark;
                                tileline.active(true);
                                tileline.color((byte)FakeOverworld.Paints.Negative);
                            }
                        }
                    }
                }
            }
        }

        public void EvilSegmentRooms(List<MazeRoom> ListOfRooms,int type)
        {
            //Do Sub-Boss Room Gen

            float bossArenaSize = 80;
            Vector2 CorruptionRoomAngleAverage = Vector2.Zero;
            foreach (MazeRoom room in ListOfRooms)
            {
                CorruptionRoomAngleAverage += ((room.loc - Middle).ToVector2());
            }
            CorruptionRoomAngleAverage /= ListOfRooms.Count;

            CorruptionRoomAngleAverage = (Vector2.Normalize(CorruptionRoomAngleAverage) * 500f) + Middle.ToVector2();

            List<MazeRoom> MiniBossRoomsBossArena = new List<MazeRoom>();
            while (MiniBossRoomsBossArena.Count < 10)
            {
                MiniBossRoomsBossArena = ListOfRooms.FindAll(testby => (testby.loc.ToVector2() - CorruptionRoomAngleAverage).Length() < bossArenaSize).ToList();
                bossArenaSize += 4;
            }

            Vector2 AveragePoint = Vector2.Zero;

            List<Point> ArenaPoints = new List<Point>();
            int minx = width, miny = height, maxx = 0, maxy = 0;

            //Do Room Carve out
            foreach (MazeRoom room in MiniBossRoomsBossArena)
            {
                AveragePoint += room.loc.ToVector2();
                //IDGWorldGen.PlaceMulti(room.loc.ToPoint(), TileID.Cloud, 8);
                //IDGWorldGen.PlaceMulti(room.loc.ToPoint(), -1, 6);
                minx = Math.Min(minx, room.loc.X); miny = Math.Min(miny, room.loc.Y);
                maxx = Math.Max(maxx, room.loc.X); maxy = Math.Max(maxy, room.loc.Y);
                ArenaPoints.Add(room.loc.ToPoint());
                for (int i = 0; i < 2; i += 1)
                {
                    foreach (MazeRoom room2 in MiniBossRoomsBossArena)
                    {
                        foreach (Point there in IDGWorldGen.GetLine(room.loc.ToPoint(), room2.loc.ToPoint()))
                        {
                            int sizer2 = 8;
                            int sizer = sizer2 - i * 2;

                            for (int x = -sizer; x <= sizer; x += 1)
                            {
                                for (int y = -sizer; y <= sizer; y += 1)
                                {
                                    Tile tile = Main.tile[there.X + x, there.Y + y];
                                    if (i < 1)
                                    {
                                        if (tile.active())
                                            tile.type = type == 0 ? TileID.AmethystGemspark : TileID.AmberGemspark;
                                    }
                                    else
                                    {
                                        tile.active(false);
                                        if (tile.wall == 0)
                                            tile.wall = type == 0 ? WallID.AmethystGemsparkOff : WallID.AmberGemsparkOff;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            AveragePoint /= MiniBossRoomsBossArena.Count;
            List<MazeRoom> ListofOrderedRooms  = ListOfRooms.OrderBy(testby => UniRand.Next(ListOfRooms.Count)).ToList();
            ListofOrderedRooms.Insert(0, new MazeRoom(AveragePoint.ToPoint16(), 0, new Point16(0, 0), 0, true));

            //place orbs/hearts
            int index = 0;
            int heart = type == 0 ? 0 : 1;
            foreach (MazeRoom lootroom in ListofOrderedRooms)
            {
                if (index < 3)
                {
                    IDGWorldGen.PlaceMulti(lootroom.loc.ToPoint(), type == 0 ? TileID.AmethystGemspark : TileID.AmberGemspark, 5, type == 0 ? WallID.AmethystGemspark : WallID.AmberGemspark);
                    IDGWorldGen.PlaceMulti(lootroom.loc.ToPoint(), -1, 3, type == 0 ? WallID.AmethystGemspark : WallID.AmberGemspark);
                    for (int num36 = 0; num36 < 2; num36++)
                    {
                        for (int num37 = 0; num37 < 2; num37++)
                        {
                            int num38 = lootroom.loc.X + num36;
                            int num39 = lootroom.loc.Y + num37;
                            Main.tile[num38, num39].active(active: true);
                            Main.tile[num38, num39].slope(0);
                            Main.tile[num38, num39].halfBrick(halfBrick: false);
                            Main.tile[num38, num39].type = 31;
                            Main.tile[num38, num39].frameX = (short)(num36 * 18 + 36 * heart);
                            Main.tile[num38, num39].frameY = (short)(num37 * 18 + 36 * 0);
                        }
                    }
                }

                index += 1;
            }

        }

        private void BossBox()
        {

            HashSet<Point16> BossBox = new HashSet<Point16>();

            MazeRoom EndPoint = MazeGraphPoints[0];
            MazeRoom BossRoomPoint = new MazeRoom(new Point16(BossRoom.Center.X, BossRoom.Center.Y), 0, new Point16(0, 0), 0, true);
            EndPoint.previousRoom = BossRoomPoint;
            MazeGraphPoints.Insert(0, BossRoomPoint);

            for (int xx = BossRoom.X; xx < BossRoom.Width + BossRoom.X; xx += 1)
            {
                for (int yy = BossRoom.Y; yy < BossRoom.Height + BossRoom.Y; yy += 1)
                {
                    Vector2 vectordist = (BossRoom.Center.ToVector2() - new Vector2(xx, yy));
                    if (BossRoomInside.Contains(xx, yy) || UniRand.NextFloat(vectordist.Length(), 100) < 100)
                    {
                        IDGWorldGen.PlaceMulti(new Point(xx, yy), TileID.RubyGemspark, UniRand.Next(2, 6), -1, true);
                        BossBox.Add(new Point16(xx, yy));
                    }
                }
            }
            InnerArenaTiles = new HashSet<Point16>(BossBox);
            foreach (Point16 point in BossBox)
            {
                Tile tileline = Main.tile[point.X, point.Y];
                tileline.color((byte)FakeOverworld.Paints.Shadow);
                //if (BossRoomInside.Contains(point.X, point.Y))
                //{
                tileline.active(false);
                tileline.wall = tileline.wall = (ushort)SGAmod.Instance.WallType("NullWallBossArena");
                //}
            }
            if (roomsize < 5)
                roomsize = 5;

            foreach (Point point in IDGWorldGen.GetLine(startingPoint.ToPoint(), BossRoom.Center))
            {

                for (int xx = -(roomsize + 2); xx <= roomsize + 2; xx += 1)
                {
                    for (int yy = -roomsize; yy < roomsize + 1; yy += 1)
                    {
                        if (InsideMap(point.X + xx, point.Y + yy))
                        {
                        Tile tileline = Main.tile[point.X + xx, point.Y + yy];
                        tileline.active(false);

                            if (BossRoom.Contains(point.X, point.Y))
                            {
                                if (tileline.wall != (ushort)SGAmod.Instance.WallType("NullWall") && tileline.wall != (ushort)SGAmod.Instance.WallType("NullWallBossArena"))
                                {
                                    tileline.wall = (ushort)SGAmod.Instance.WallType("NullWall");
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool InsideMap(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Main.maxTilesX && y < Main.maxTilesY;
        }

        public override List<GenPass> tasks { get; }

        public Limborinth()
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
            Main.dayTime = true;
            Main.time = 40000;
        }


	}
   
}
