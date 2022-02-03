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
using SGAmod.Tiles.Monolith;
using SGAmod.Items.Accessories;

namespace SGAmod.Dimensions
{
    public class FilledSpaceArea
    {
        public Vector2 position;
        public int type;
        public int generation;
        public int heightVar = 0;
        public int size = 0;
        public bool generationAsteriods = true;

        public List<FilledSpaceArea> connections;

        //public byte[] allowedToConnectwith = {0};
        public FilledSpaceArea(Vector2 where)
        {
            position = where;
            type = 0;
            generation = 0;
        }
    }

    public struct SecurityEnemy
    {
        public int type;
        public int delay;
        public SecurityEnemy(int type, int delay)
        {
            this.type = type;
            this.delay = delay;
        }
    }

    public class MainStationStructure : SpaceStationStructure
    {

        public MainStationStructure(Vector2 position) : base(position)
        {
            this.position = position;
            StationObjects.Add(this);
            mainBase = true;
            type = 0;
        }

        public override void FillInStation(UnifiedRandom uniRand, List<(Point, int)> floorTiles, List<(Point, int)> platfromTiles)
        {

            int chestCount = floorTiles.Count / 5000;
            int addedChests = 0;

            floorTiles.AddRange(platfromTiles);

            foreach ((Point, int) floor in floorTiles.OrderBy(testby => uniRand.Next()))
            {
                if (addedChests >= chestCount)
                    break;

                if (floor.Item2 > TileRangeExits)
                    continue;

                if ((floor.Item2 > TileRangeWalkwayMin && floor.Item2 < TileRangeWalkwayMax) && uniRand.Next(100) < 50)
                    continue;

                int thechest = WorldGen.PlaceChest(floor.Item1.X, floor.Item1.Y, TileID.Containers, false, 49);
                if (thechest >= 0)
                {
                    SpaceDim.AddStuffToChest(thechest, 1, uniRand);
                    addedChests += 1;
                }
            }
        }
    }








    public enum StationRoomTypes
    {
        BasicFork = 0,

        Main = 9,
        SmolStation = 10,
    }


    public class SpaceStationStructure
    {
        public static List<(Vector2, int)> placesToFillIn = new List<(Vector2, int)>();
        public static List<SpaceStationStructure> StationObjects;
        public static List<(Vector2, Vector2)> ConnectionLines;
        public static int TileRangeInnerDoors = 20000;
        public static int TileRangeExits = 10000;
        public static int TileRangeWalkwayMin = 1000;
        public static int TileRangeWalkwayMax = 3000;
        public static int TileRangeWalkwayHalf = 2000;


        public List<(Vector2, int)> placesToBuild = new List<(Vector2, int)>();

        public int type;
        public List<Point> expansionPoints = new List<Point>();
        public Vector2 position;
        public bool mainBase = false;

        public bool securityActivated = false;
        public int maxSecurity = 5;
        public int securitySpawnDelay = 120;
        public List<SecurityEnemy> securityToSpawn = new List<SecurityEnemy>();
        public List<(SecurityEnemy,Vector3, NPC)> securitySpawned = new List<(SecurityEnemy,Vector3, NPC)>();
        public List<(FilledSpaceArea, Vector2)> roomsToGenerateLater = new List<(FilledSpaceArea, Vector2)>();


        public int thickness = 3;
        public int tileType = TileID.MeteoriteBrick;
        public int tileTypeInside = TileID.CopperPlating;
        public int tileTypePlatform = TileID.TeamBlockWhitePlatform;
        public int tileTypeSolidPlatform = TileID.MartianConduitPlating;
        public int tileTypeLight = TileID.DiamondGemspark;

        public int tileTypeMainDoorTile = TileID.RubyGemspark;

        public int wallType = WallID.IridescentBrick;
        public int wallTypeExitWay = WallID.IridescentBrick;
        public int wallTypeConnector = WallID.TinPlating;

        public int glassType = WallID.BlueStainedGlass;
        public NPC focusCrystal = null;
        public List<NPC> stationDefenses = new List<NPC>();


        public SpaceStationStructure(Vector2 position)
        {
            this.position = position;
            StationObjects.Add(this);
            type = 0;
        }

        public void ResetSecurity()
        {
            securityToSpawn.Clear();
            securitySpawned.Clear();

            foreach (NPC npc in stationDefenses)
            {
                if (npc != null && npc.active)
                {
                    npc.active = false;
                }
            }
            stationDefenses.Clear();
        }

        public virtual void SetupSecurity()
        {

            ResetSecurity();

            for (int i = 0; i < 5; i += 1)
            {
                int typeofenemy = NPCID.GrayGrunt;
                SecurityEnemy secenemy = new SecurityEnemy(typeofenemy, 60);
                securityToSpawn.Add(secenemy);
            }

            for (int i = 0; i < 3; i += 1)
            {
                int typeofenemy = NPCID.MartianOfficer;
                SecurityEnemy secenemy = new SecurityEnemy(typeofenemy, 100);
                securityToSpawn.Add(secenemy);
            }

            securityToSpawn = securityToSpawn.OrderBy(testby => Main.rand.Next()).ToList();
        }

        public void SpawnFocusCrystals()
        {
            if (mainBase)
                return;

            SetupSecurity();

            makecrystal:

            if (focusCrystal == null || !focusCrystal.active)
            {

                int npc = NPC.NewNPC((int)position.X + 8, (int)position.Y + 8 + 32, ModContent.NPCType<StationFocusCrystal>());
                NPC crystal = Main.npc[npc];

                if (crystal != null)
                {
                    focusCrystal = crystal;
                    StationFocusCrystal cryrs = crystal.modNPC as StationFocusCrystal;
                    cryrs.station = this;
                }
            }
            else
            {
                if (focusCrystal != null)
                {
                    focusCrystal.active = false;
                    focusCrystal = null;
                    goto makecrystal;
                }
            }
        }
        public static void UpdateStations()
        {
            foreach (SpaceStationStructure station in SpaceStationStructure.StationObjects)
            {
                station.Update();
            }
        }

        public virtual bool CanOverrideTile(Tile tile)
        {
            return tile.type == tileType || tile.type == tileTypeInside || tile.type == tileTypeLight;
        }

        public void Update()
        {
            if (securityActivated)
            {
                int spawnTime = 30;

                securitySpawnDelay -= 1;

                if (securitySpawnDelay < 1)
                {
                    if (securitySpawned.Count < maxSecurity)
                    {
                        SpawnSecurity();
                    }
                }

                Main.NewText("Security size: " + securitySpawned.Count);

                if (securitySpawned.Count > 0)
                {

                    foreach ((SecurityEnemy, Vector3, NPC) managedEnemySpawns in securitySpawned)
                    {
                        Vector3 data = managedEnemySpawns.Item2;
                        if ((int)data.Z == spawnTime)
                        {
                            NPC.NewNPC((int)data.X, (int)data.Y, managedEnemySpawns.Item1.type);
                        }
                    }

                    securitySpawned = securitySpawned.Select(testby => (testby.Item1, new Vector3(testby.Item2.X, testby.Item2.Y, testby.Item2.Z - 1), testby.Item3)).Where(testby => testby.Item2.Z > -60 && !(testby.Item3 == null || !testby.Item3.active)).ToList();

                }
            }
        }

        private void SpawnSecurity()
        {
            if (securityToSpawn.Count > 0)
            {
                Vector2 place = placesToBuild[Main.rand.Next(placesToBuild.Count)].Item1;
                SecurityEnemy enemy = securityToSpawn[0];

                //NPC.NewNPC((int)place.X, (int)place.Y, enemy.type);
                securitySpawnDelay = enemy.delay;

                securitySpawned.Add((enemy, new Vector3(place, 120), null));

                securityToSpawn.RemoveAt(0);
            }
        }

        public void MakeConnectionWalkways(UnifiedRandom uniRand, FilledSpaceArea theLocation,ref List<(Vector2, int)> tempPlacesToBuild)
        {
            if (theLocation.connections == default)
                return;

            List<(Vector2, int)> corridorsToBuild = new List<(Vector2, int)>();

            Point pos = (theLocation.position / 16).ToPoint();

            foreach (FilledSpaceArea connection in theLocation.connections)
            {
                Point connectionpos = (connection.position / 16).ToPoint();
                List<Point> line = IDGWorldGen.GetLine(pos, connectionpos);

                int index = 0;

                foreach(Point buildPoint in line)
                {
                    for (int x = -1; x < 2; x += 1)
                    {
                        for (int y = -1; y < 2; y += 1)
                        {
                            Point newPoint = new Point(buildPoint.X + x, buildPoint.Y + y);

                            int walkwayValue = (index) %20>16 ? TileRangeWalkwayHalf : TileRangeWalkwayMin;
                            int finalIndex = walkwayValue + index;

                            //Locked Door into Main
                            if ((index > (int)(line.Count/2)-2 && index < (int)(line.Count / 2) +2) && connection.type == (int)StationRoomTypes.Main)
                            {
                                finalIndex = SpaceStationStructure.TileRangeInnerDoors;
                            }

                            corridorsToBuild.Add((newPoint.ToVector2() * 16, finalIndex));
                        }
                    }
                    index += 1;
                }

            }

            tempPlacesToBuild.AddRange(corridorsToBuild.ToList());

        }

        public void MakeExitWalkways(UnifiedRandom uniRand, FilledSpaceArea theLocation,int heightPlatformDifferance, ref List<(Vector2, int)> tempPlacesToBuild)
        {
            //Create walkways (exits)
            if (theLocation.type == (int)StationRoomTypes.SmolStation)
            {
                Vector2 centerPos = position == default ? theLocation.position : position;
                int size = theLocation.size;
                int openspace = 0;
                int walkwaylength = 0;
                for (int side = -1; side < 2; side += 2)
                {
                    for (int xx = size - 8; xx < size + 64; xx += 1)
                    {
                        int allOpen = 0;
                        int realx = xx * side;

                        walkwaylength += 1;

                        for (int yy = -1; yy < 2; yy += 1)
                        {
                            int realy = yy - 2;
                            Vector2 thereAreThey = centerPos + new Vector2(realx * 16, ((realy + heightPlatformDifferance) * 16));
                            Point loc = (thereAreThey / 16).ToPoint();
                            Tile tile = Framing.GetTileSafely(loc.X, loc.Y);

                            if (!tile.active())
                            {
                                allOpen += 1;
                            }

                            Vector2 placeHere = thereAreThey;// theLocation.position + new Vector2(realx * 16, realy * 16);
                            tempPlacesToBuild.Add((placeHere, TileRangeExits + walkwaylength));
                        }

                        if (openspace > 0)
                            openspace -= 1;


                        if (allOpen > 2)
                            openspace += 8;

                        if (openspace > 6)
                        {
                            /*
                            if (theLocation.type>5)
                            {
                                Vector2 thereAreThey2 = theLocation.position + new Vector2(realx * 16, ((0 + heightPlatformDifferance) * 16));
                                FilledSpaceArea area2 = new FilledSpaceArea(thereAreThey2);
                                area2.type = (theLocation.type-1;

                                GenerateStationCore(uniRand, area2,0);
                            }
                            */

                            goto gotoNext;
                        }
                    }
                gotoNext:
                    openspace = 0;
                }
            }
        }

        public void GenerateStationCore(UnifiedRandom uniRand, FilledSpaceArea theLocation,Vector2 position,bool keepTrackOnly = true,bool addObjectsPhase = false)
        {

            int heightPlatformDifferance = theLocation.heightVar;// uniRand.Next(-4, 6);
            int size = theLocation.size;
            int size16 = size * 16;
            int lowerlimit = 16;
            int upperlimit = -16;

            int defaultSize = 16 * 16;

            Vector2 centerPos = position == default ? theLocation.position : position;

            List<(Vector2, int)> tempPlacesToBuild = new List<(Vector2, int)>();

            if (keepTrackOnly)
            {

                //Asteriod Area
                if (theLocation.generationAsteriods)
                {
                    for (int dister = 120 + (size16 - defaultSize); dister < 480 + (size16 - defaultSize); dister += 120)
                    {
                        for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
                        {
                            Vector2 offsetter = centerPos + f.ToRotationVector2() * dister;

                            Point locaa = new Point((int)offsetter.X / 16, (int)offsetter.Y / 16);
                            SpaceDim.MakeAsteriod(locaa, uniRand);
                        }
                    }
                }

                SpaceDim.FilledSpacesTakenByStations.Remove(theLocation);

                roomsToGenerateLater.Add((theLocation, position));
                goto gototheend;
            }

            centerPos.Y -= upperlimit + lowerlimit;

            //Create Circle Shape
            for (int x = -size16; x <= size16; x += 1)
            {
                for (int y = -size16; y <= size16; y += 1)
                {
                    int realx = x;
                    int realy = Math.Min(Math.Max(upperlimit, y), lowerlimit);

                    Vector2 placeHere = centerPos + new Vector2(realx * 16, realy * 16);
                    if ((placeHere - centerPos).Length() < size16)
                    {
                        tempPlacesToBuild.Add((placeHere, 0));
                    }
                }
            }

            MakeConnectionWalkways(uniRand,theLocation,ref tempPlacesToBuild);

            MakeExitWalkways(uniRand, theLocation, heightPlatformDifferance, ref tempPlacesToBuild);

            if (addObjectsPhase == false)
            {

                #region fillIns
                //Fill in
                foreach ((Vector2, int) there in tempPlacesToBuild)
                {
                    IDGWorldGen.PlaceMulti(there.Item1 / 16, tileType, thickness);
                }

                //Fill in-outline side, add wall types
                foreach ((Vector2, int) there in tempPlacesToBuild)
                {
                    int walltype = -1;

                    if (there.Item2 < TileRangeWalkwayMin || there.Item2 >= TileRangeExits)
                    {
                        walltype = there.Item2 >= TileRangeExits ? wallTypeExitWay : wallType;
                    }
                    else
                    {
                        //Make Connector Glass Types
                        if (there.Item2 >= TileRangeWalkwayMin)
                        {
                            Tile tile = Framing.GetTileSafely((int)(there.Item1.X / 16), (int)(there.Item1.Y / 16));
                            if (tile.wall < 1)
                            {
                                walltype = there.Item2 >= TileRangeWalkwayHalf ? glassType : wallTypeConnector;
                            }
                        }
                    }

                    IDGWorldGen.PlaceMulti(there.Item1 / 16, tileTypeInside, thickness - 1, walltype);
                }

                //Add Glass walls/Dye Solid tiles
                foreach ((Vector2, int) there in tempPlacesToBuild)
                {
                    float dister = (there.Item1 - centerPos).Length();

                    if (there.Item2 < 2 && Math.Sin(dister / 16f) > 0)
                        Main.tile[(int)there.Item1.X / 16, (int)there.Item1.Y / 16].wall = (ushort)glassType;

                    for (int x = -thickness; x <= thickness; x += 1)
                    {
                        for (int y = -thickness; y <= thickness; y += 1)
                        {
                            Point vexx = (there.Item1 / 16).ToPoint();
                            Tile tile = Main.tile[vexx.X + x, vexx.Y + y];

                            if (tile.type == tileType)
                                tile.color((byte)Paints.White);

                            if (tile.type == tileTypeInside)
                                tile.color((byte)Paints.White);
                        }
                    }


                }


            }


            if (addObjectsPhase)
            {


                //Add lights/Bubble enterances/exits

                List<Point> openings = new List<Point>();
                foreach ((Vector2, int) there in tempPlacesToBuild)
                {
                    if (there.Item2 > 2 && there.Item2 % 8 > 5)
                    {
                        Point here = ((there.Item1 / 16) + new Vector2(0, -1)).ToPoint();
                        Tile theTile = Framing.GetTileSafely(here.X, here.Y);

                        if (theTile.type == tileTypeInside)
                        {
                            theTile.type = (ushort)tileTypeLight;

                        }


                    }


                    //Add Tunnel bubble exits
                    if (theLocation.type == (int)StationRoomTypes.SmolStation && there.Item2 > TileRangeExits+2)
                    {

                        Vector2[] coordz = { new Vector2(-8, 0), new Vector2(8, 0) };

                        foreach (Vector2 herethere in coordz)
                        {
                            Point here = ((there.Item1 / 16) + herethere).ToPoint();
                            Tile theTile = Framing.GetTileSafely(here.X, here.Y);
                            if (!theTile.active() && theTile.wall < 1)
                            {
                                foreach (Point line in IDGWorldGen.GetLine((there.Item1 / 16).ToPoint(), here))
                                {
                                    Tile theTile2 = Framing.GetTileSafely(line.X, line.Y);
                                    if (CanOverrideTile(theTile2))// || Main.tileMerge[ModContent.TileType<Spacerock>()][theTile2.type])
                                    {
                                        //theTile2.active(false);
                                        openings.Add(line);

                                        /*
                                        if (theTile2.type == tileType || theTile2.type == tileTypeInside)
                                        {
                                            theTile2.type = TileID.Bubble;
                                            theTile2.color((byte)PaintID.Teal);
                                        }
                                        else
                                        {
                                            theTile2.active(false);
                                        }
                                        */

                                    }
                                }
                            }
                        }
                    }
                }


                //Add 1-block bubble walls in recorded places
                Point[] coordz2 = { new Point(-1, 0), new Point(1, 0) };
                int tileBubbleType = TileID.Bubble;

                foreach (Point opening in openings)
                {
                    //Change outer layer into Bubble Tiles
                    foreach (Point herethere in coordz2)
                    {
                        Tile theTileToBubble = Framing.GetTileSafely(opening.X, opening.Y);

                        Point here = new Point(opening.X + herethere.X, opening.Y + herethere.Y);
                        Tile theTile = Framing.GetTileSafely(here.X, here.Y);
                        if (!theTile.active() && theTile.wall < 1 && theTileToBubble.wall < 1 && theTileToBubble.type == tileType)
                        {
                            theTileToBubble.active(true);
                            theTileToBubble.type = (ushort)tileBubbleType;
                            theTileToBubble.color((byte)PaintID.Teal);
                        }
                    }

                    //Add bubblegun walls next to Bubble tiles
                    foreach (Point herethere in coordz2)
                    {
                        Tile theTileToBubble = Framing.GetTileSafely(opening.X, opening.Y);

                        Point here = new Point(opening.X + herethere.X, opening.Y + herethere.Y);
                        Tile theTile = Framing.GetTileSafely(here.X, here.Y);

                        if (theTileToBubble.type == tileTypeInside && theTile.type == tileBubbleType)
                        {
                            theTileToBubble.wall = WallID.BubblegumBlock;
                            theTileToBubble.wallColor((byte)PaintID.White);
                        }
                    }
                }

                    #endregion

                    //Clear Insides/Platform in the middle
                    #region ClearOutInsides

                    int rectsizeDiffer = 8;
                Rectangle platform = new Rectangle((int)(centerPos.X / 16) - size + rectsizeDiffer, (int)(centerPos.Y / 16) + heightPlatformDifferance, (size * 2) - (rectsizeDiffer * 2) + 1, 1);
                Rectangle platformEdge = new Rectangle((int)(centerPos.X / 16) - size, (int)(centerPos.Y / 16) + heightPlatformDifferance, size * 2, 1);

                foreach ((Vector2, int) there in tempPlacesToBuild)
                {
                    Point therePoint = new Point((int)there.Item1.X / 16, (int)there.Item1.Y / 16);
                    Tile tile = Framing.GetTileSafely(therePoint.X, therePoint.Y);

                    bool clearArea = true;

                    //Add (Main) Doors
                    if (there.Item2  == TileRangeInnerDoors)
                    {
                        Tile door1Tile = tile;
                        door1Tile.type = (ushort)tileTypeMainDoorTile;
                        door1Tile.active(true);
                        clearArea = false;
                    }


                    //Station Circle Room with 1 platform
                    if (there.Item2 < 1)
                    {

                        if (clearArea)
                        {
                            tile.active(false);
                        }

                        if (platformEdge.Contains(therePoint))
                        {
                            if (platform.Contains(therePoint))
                            {
                                WorldGen.PlaceTile(therePoint.X, therePoint.Y, tileTypeSolidPlatform);
                                //tile.slope(7);
                                continue;
                            }
                            WorldGen.PlaceTile(therePoint.X, therePoint.Y, tileTypePlatform);
                            continue;
                        }
                    }
                }
            }

            SGAmod.Instance.Logger.Debug(centerPos);
            #endregion

            placesToBuild.AddRange(tempPlacesToBuild);

            return;
            gototheend:

            SpaceDim.StationSpaces.Add(theLocation);

            //WorldGen.DungeonStairs((int)theLocation.position.X/16, (int)theLocation.position.Y/16,TileID.BlueDungeonBrick,WallID.BlueDungeonUnsafe);
        }

        public List<FilledSpaceArea> StemNewStationSegments(UnifiedRandom uniRand, FilledSpaceArea theLocation,int maxRooms = 3,int type = 0)
        {
            List<FilledSpaceArea> areas = new List<FilledSpaceArea>();
            //Sprawl more Stations
            #region recursiveGeneration
            if (theLocation.type < 10 && theLocation.generation < 8)
            {
                float dist = 2000 * 2000;
                float distMin = (theLocation.size)*16+320;

                int tries = 0;
                int failType = 0;
                int maxGens = 3;

                int maxChains = maxRooms;// theLocation.generation < 1 ? 3 : 2;

                List<FilledSpaceArea> filledAreas = new List<FilledSpaceArea>(SpaceDim.FilledSpacesTakenByStations);

                for (int chains = 0; chains < maxChains; chains += 1)
                {

                    FilledSpaceArea chosenArea = null;

                    filledAreas = SpaceDim.FilledSpacesTakenByStations.Where(testby => (testby.position - theLocation.position).LengthSquared() < dist && (testby.position - theLocation.position).LengthSquared() > (distMin* distMin))
    .Where(testby => testby.position != theLocation.position).ToList();






                    foreach (FilledSpaceArea areaToCheck in filledAreas)
                    {
                        bool canDoIt = true;
                        foreach (FilledSpaceArea areaToCheck2 in SpaceDim.StationSpaces)
                        {
                            if (areaToCheck2.position == theLocation.position)
                                continue;


                            int buffersizeLines = 8;
                            int buffersizeCircles = 8;

                            float sizerx = (areaToCheck2.size + theLocation.size + buffersizeCircles) * 16;
                            float sizery = (areaToCheck.size + theLocation.size + buffersizeCircles) * 16;


                            //Spheres do not touch

                            if ((areaToCheck2.position - theLocation.position).LengthSquared() < (sizerx * sizerx) || (areaToCheck2.position - areaToCheck.position).LengthSquared() < (sizery * sizery))
                            {
                                canDoIt = false;
                                break;
                            }

                            Vector2 out1;
                            Vector2 out2;

                            //We do not cut into a sphere trying to go to the destination
                            if (Idglib.FindLineCircleIntersections(areaToCheck2.position.X, areaToCheck2.position.Y, (areaToCheck2.size + buffersizeLines) * 16, theLocation.position, areaToCheck.position, out out1, out out2) > 0)
                            {
                                canDoIt = false;
                                break;
                            }

                            //We do not overlap other connections
                            foreach ((Vector2, Vector2) linePair in SpaceStationStructure.ConnectionLines)
                            {
                                Vector2 outIntersection;
                                bool bool1;
                                bool bool2;
                                Idglib.FindLineLineIntersection(linePair.Item1, linePair.Item2, theLocation.position, areaToCheck.position, out bool1, out bool2, out outIntersection, out out1, out out2);

                                if (bool2 && !(outIntersection == theLocation.position || outIntersection == areaToCheck.position))
                                {
                                    canDoIt = false;
                                    goto skiploop;
                                }
                            }
                        }

                        skiploop:

                        if (canDoIt)
                        {
                            chosenArea = areaToCheck;
                            goto endthis;
                        }

                        tries += 1;
                    }

                

                endthis:

                    if (chosenArea != null)
                    {

                        chosenArea.generation = theLocation.generation + 1;
                        chosenArea.size = 16 - uniRand.Next(-2, 4);
                        chosenArea.heightVar = uniRand.Next(-4, 6) / (1 + (chosenArea.generation / 2));
                        chosenArea.type = type;

                        if (chosenArea.connections == default)
                        chosenArea.connections = new List<FilledSpaceArea>();

                        chosenArea.connections.Add(theLocation);
                        SpaceStationStructure.ConnectionLines.Add((theLocation.position, chosenArea.position));

                        //if (chosenArea.generation > maxGens-1)
                        //    chosenArea.type = 10;

                        areas.Add(chosenArea);
                        SGAmod.Instance.Logger.Debug("Generated Precced Station: " + chosenArea.generation);
                        GenerateStationCore(uniRand, chosenArea, chosenArea.position);


                    }
                    else
                    {
                        SGAmod.Instance.Logger.Debug("New Station Failed to Generate: tries " + tries + " : " + filledAreas.Count + " : " + SpaceDim.StationSpaces.Count + ": testing is done");
                    }

                }

            }
            return areas;
            #endregion


        }

        //Fills in the rooms proper
        public static void GenerateAllStationRooms(UnifiedRandom uniRand)
        {
            foreach (SpaceStationStructure station in SpaceStationStructure.StationObjects)
            {
                foreach ((FilledSpaceArea, Vector2) room in station.roomsToGenerateLater)
                {
                    station.GenerateStationCore(uniRand, room.Item1, room.Item2, false);
                }
                foreach ((FilledSpaceArea, Vector2) room in station.roomsToGenerateLater)
                {
                    station.GenerateStationCore(uniRand, room.Item1, room.Item2, false,true);
                }
            }
        }

        //Per Station fill-ins
        public virtual void FillInStation(UnifiedRandom uniRand, List<(Point, int)> floorTiles, List<(Point, int)> platfromTiles)
        {

            //Put a chest on solid tiles
            bool addedChest = false;
            foreach ((Point, int) floor in floorTiles.OrderBy(testby => uniRand.Next()))
            {
                if (addedChest)
                    break;

                if (floor.Item2 > TileRangeExits)
                    continue;

                int thechest = WorldGen.PlaceChest(floor.Item1.X, floor.Item1.Y, TileID.Containers, false, 48);
                if (thechest >= 0)
                {
                    SpaceDim.AddStuffToChest(thechest, 0, uniRand);
                    addedChest = true;
                }
            }
        }

        //Fill in all stations
        public static void FillInAllStations(UnifiedRandom uniRand)
        {

            //Get "floors" in the station, these include the platforms
            List<(Point, int)> solidFloorsInAllStations = new List<(Point, int)>();
            foreach (SpaceStationStructure station in SpaceStationStructure.StationObjects)
            {
                List<(Point, int)> solidFloorsInThisStation = new List<(Point, int)>();
                List<(Point, int)> platformFloorsInThisStation = new List<(Point, int)>();

                station.placesToBuild = station.placesToBuild.Distinct().ToList();

                foreach ((Vector2, int) there in station.placesToBuild)
                {
                    Point therePoint = new Point((int)there.Item1.X / 16, (int)there.Item1.Y / 16);
                    Tile tile = Framing.GetTileSafely(therePoint.X, therePoint.Y);

                    if (station.CanOverrideTile(tile) || Main.tileMerge[ModContent.TileType<Spacerock>()][tile.type])
                        tile.active(false);
                }

                //place stuff

                foreach ((Vector2, int) there in station.placesToBuild)
                {
                    Point therePoint = new Point((int)there.Item1.X / 16, (int)there.Item1.Y / 16);
                    Tile tile = Framing.GetTileSafely(therePoint.X, therePoint.Y);

                    Point therePointBelow = new Point(therePoint.X, therePoint.Y + 1);
                    Tile tileBelow = Framing.GetTileSafely(therePointBelow.X, therePointBelow.Y);

                    //On solid tiles only
                    if ((Main.tileSolid[tileBelow.type]) && tileBelow.active())
                        solidFloorsInThisStation.Add((therePoint, there.Item2));

                    //On Platforms only
                    if ((Main.tileSolidTop[tileBelow.type]) && tileBelow.active())
                        platformFloorsInThisStation.Add((therePoint, there.Item2));

                }

                station.FillInStation(uniRand, solidFloorsInThisStation, platformFloorsInThisStation);//Add stuff that's per station

                solidFloorsInAllStations.AddRange(solidFloorsInThisStation);
                solidFloorsInAllStations.AddRange(platformFloorsInThisStation);
            }



            //Place stuff that can be in any station

            foreach ((Point, int) floor in solidFloorsInAllStations.OrderBy(testby => uniRand.Next()))
            {
                if (floor.Item2 < TileRangeExits)
                {
                    WorldGen.PlaceObject(floor.Item1.X, floor.Item1.Y, ModContent.TileType<CelestialMonolith>());
                    Tile tile = Framing.GetTileSafely(floor.Item1.X, floor.Item1.Y);

                    if (tile.type == ModContent.TileType<CelestialMonolith>())
                    {
                        CelestialMonolithTE inst = ModContent.GetInstance<CelestialMonolithTE>();

                        inst.Hook_AfterPlacement(floor.Item1.X, floor.Item1.Y, inst.Type, 0,0);
                        goto theend;
                    }
                }
            }

            theend:
            SGAmod.Instance.Logger.Debug("Done adding stuff to stations");

        }

        public void GenerateMainStationStructure(UnifiedRandom uniRand, FilledSpaceArea AreaToSpawnMainBaseAt)
        {
            //finished WIP content
            return;

            FilledSpaceArea ChosenMainBaseLocation = AreaToSpawnMainBaseAt;
            ChosenMainBaseLocation.type = (int)(StationRoomTypes.Main);
            ChosenMainBaseLocation.generation = 0;
            ChosenMainBaseLocation.size = 24;
            ChosenMainBaseLocation.heightVar = uniRand.Next(-1, 1);

            //Make the main, central core room
            GenerateStationCore(uniRand, ChosenMainBaseLocation, ChosenMainBaseLocation.position);


            //Generate 1st ones
            List<FilledSpaceArea> firstLayerAreas = StemNewStationSegments(uniRand, ChosenMainBaseLocation,3, (int)(StationRoomTypes.BasicFork));

            foreach (FilledSpaceArea areaHere in firstLayerAreas)
            {
                areaHere.generation = 1;

                //Generate 2nd layer exits
                foreach (FilledSpaceArea areaHere2ndLayer in StemNewStationSegments(uniRand, areaHere, 2, (int)(StationRoomTypes.BasicFork)))
                {
                    //nil
                }
            }


            SpaceStationStructure.placesToFillIn.AddRange(placesToBuild);
        }

    }





    public class SpaceDim : SGAPocketDim
    {
        public override int width => 3200;
        public override int height => 800;
        public override bool saveSubworld => false;
        public override float spawnRate => 7.50f;

        public override string DimName => "Near Terrarian Orbit";
        public static bool postMoonLord = false;

        public static List<Vector2> EmptySpaces;
        public static List<FilledSpaceArea> FilledSpaces;
        public static List<FilledSpaceArea> FilledSpacesTakenByStations;
        public static List<FilledSpaceArea> StationSpaces;

        public override UserInterface loadingUI => base.loadingUI;
        public static NoiseGenerator Noisegen;
        public static NoiseGenerator Noisegen2freq;
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

        public int AsteriodDensity(Point where, int buffer)
        {
            int ammount = 0;

            for (int x = -buffer; x <= buffer; x += 1)
            {
                for (int y = -buffer; y <= buffer; y += 1)
                {
                    int whereX = Math.Min(width, Math.Max(0, where.X + x));
                    int whereY = Math.Min(width, Math.Max(0, where.Y + y));

                    Tile tile = Framing.GetTileSafely(whereX, whereY);
                    if (tile.active() && Main.tileSolid[tile.type])
                        ammount += 1;
                }
            }
            return ammount;
        }

        public static void MakeAsteriod(Point where, UnifiedRandom rand)
        {

            //float angle = rand.NextFloat(MathHelper.TwoPi);

            Vector2 dists = new Vector2(0, 10);
            int size2 = rand.Next(2, 4);
            int spaced = 0;
            int itr = 8;
            int width = Main.maxTilesX;
            int height = Main.maxTilesY;

            if (rand.Next(0, 10) == 0)
            {
                size2 += 2;
                spaced += 8;
                itr = 16;
            }

            bool fragmentAsteriod = postMoonLord && rand.Next(10) < 1;
            Point boundsLower = new Point(where.X, where.Y);
            Point boundsUpper = new Point(where.X, where.Y);

            for (int i = 0; i < itr; i += 1)
            {

                Vector2 offsetGaussian = Gaussian2D(rand.NextFloat(), rand.NextFloat()) * (i + spaced);

                Point idealLocation = new Point(where.X + (int)offsetGaussian.X, where.Y + (int)offsetGaussian.Y);

                Point OreSpreadLoc = rand.Next(100) < 25 ? idealLocation : where;

                int size = size2 + rand.Next(-1, 2) + (int)MathHelper.Clamp((float)Noisegen.Noise(where.X, where.Y) * 4f, -2f, 3f);

                IDGWorldGen.PlaceMulti(idealLocation, (ushort)ModContent.TileType<Spacerock2>(), size);

                if (rand.Next(-50, Math.Max(80, 300 - where.Y)) > 0)
                {
                    if (fragmentAsteriod)
                    {
                        boundsLower.X = Math.Min(boundsLower.X, OreSpreadLoc.X);
                        boundsLower.Y = Math.Min(boundsLower.Y, OreSpreadLoc.Y);

                        boundsUpper.X = Math.Max(boundsUpper.X, OreSpreadLoc.X);
                        boundsUpper.Y = Math.Max(boundsUpper.Y, OreSpreadLoc.Y);
                    }

                    IDGWorldGen.TileRunner(OreSpreadLoc.X, OreSpreadLoc.Y, 5, 12, fragmentAsteriod ? TileID.Adamantite : TileID.Meteorite, false, rand: rand);
                }

                if (postMoonLord && rand.Next(-50, Math.Max(-10, 300 - where.Y)) > 0)
                    IDGWorldGen.TileRunner(OreSpreadLoc.X, OreSpreadLoc.Y, 4, 15, ModContent.TileType<AstrialLuminite>(), false, rand: rand);

                if (rand.Next((height - 300) - 50, Math.Max((height - 300) - 10, height)) < where.Y)
                    IDGWorldGen.TileRunner(OreSpreadLoc.X, OreSpreadLoc.Y, 4, 15, ModContent.TileType<VibraniumCrystalTile>(), false, rand: rand);

            }

            if (fragmentAsteriod)
            {
                int[] oreType = { TileID.LunarBlockNebula, TileID.LunarBlockSolar, TileID.LunarBlockStardust, TileID.LunarBlockVortex };
                ushort myFragementOre = (ushort)oreType[rand.Next(oreType.Length)];

                for (int x = boundsLower.X - 30; x < boundsUpper.X + 60; x++)
                {
                    for (int y = boundsLower.Y - 30; y < boundsUpper.Y + 60; y++)
                    {
                        if (WorldGen.InWorld(x, y))
                        {
                            Tile tile = Framing.GetTileSafely(x, y);

                            if (tile.type == TileID.Adamantite)
                                tile.type = myFragementOre;
                            //tile.color((byte)Paints.DeepRed);
                        }
                    }

                }

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

        public void SmoothOutAsteriods(UnifiedRandom uniRand, int passesToDo = 4)
        {

            for (int passes = 0; passes < passesToDo; passes += 1)
            {
                for (int y = 0; y < Main.maxTilesY; y += 1)
                {
                    for (int x = 0; x < Main.maxTilesX; x += 1)
                    {
                        Tile tile = Main.tile[x, y];
                        bool asteriodRocks = tile.type == ModContent.TileType<Spacerock>() || tile.type == ModContent.TileType<Spacerock2>() || tile.type == ModContent.TileType<AstrialLuminite>() || tile.type == TileID.Meteorite
                             || tile.type == TileID.LunarBlockSolar || tile.type == TileID.LunarBlockNebula || tile.type == TileID.LunarBlockVortex || tile.type == TileID.LunarBlockStardust;

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

        public void MakeSmallStations(UnifiedRandom uniRand, int idealCount = 5)
        {
            List<FilledSpaceArea> areas = new List<FilledSpaceArea>(FilledSpaces);
            //SpaceStationStructure.placesToFillIn.Clear();

            for (int i = 0; i < idealCount; i += 1)
            {
                int distance = 3200;
            goback:
                List<FilledSpaceArea> area = areas.Where(testby => StationSpaces.Where(testby2 => (testby2.position - testby.position).Length() < distance).Count() < 1).OrderBy(testby => uniRand.Next()).ToList();
                if (area.Count < 1)
                {
                    distance -= 10;
                    goto goback;
                }

                //SpaceStationStructure.placesToBuild.Clear();

                //Generation single Stations, with only exits (no prceed-generation)

                FilledSpaceArea theLocation = area[0];
                theLocation.type = (int)(StationRoomTypes.SmolStation);
                theLocation.generation = 0;
                theLocation.size = 16;
                theLocation.heightVar = uniRand.Next(-4, 6);

                SpaceStationStructure outpost = new SpaceStationStructure(theLocation.position);
                outpost.GenerateStationCore(uniRand, theLocation, theLocation.position);

                SpaceStationStructure.placesToFillIn.AddRange(outpost.placesToBuild);


            }

        }

        public void MakeMainStation(UnifiedRandom uniRand, int worldBuffer = 4200, int MinWorldBuffer = 720)
        {
            FilledSpacesTakenByStations = new List<FilledSpaceArea>(FilledSpaces);

            foreach (Vector2 space in SpaceDim.EmptySpaces)
            {
                FilledSpacesTakenByStations.Add(new FilledSpaceArea(space));
            }


            List<FilledSpaceArea> areas = new List<FilledSpaceArea>();
            SpaceStationStructure.placesToFillIn.Clear();

            bool leftOrRight = uniRand.NextBool();

            Rectangle inner = new Rectangle(0, 0, (Main.maxTilesX * 16) - worldBuffer * 2, (Main.maxTilesY * 16));
            Rectangle outer = new Rectangle(0, 1200, (Main.maxTilesX * 16) - MinWorldBuffer * 2, (Main.maxTilesY * 16) - 2400);



            areas = FilledSpaces.Where(testby => outer.Contains(testby.position.ToPoint()) && !inner.Contains(testby.position.ToPoint())).OrderBy(testby => uniRand.Next()).ToList();

            MainStationStructure outpost = new MainStationStructure(areas[0].position);

            outpost.mainBase = true;

            outpost.GenerateMainStationStructure(uniRand, areas[0]);

            SpaceStationStructure.placesToFillIn.AddRange(outpost.placesToBuild);




        }



        public virtual void AGenPass(GenerationProgress prog)
        {
            proggers = prog;

            for (int x = 0; x < width; x += 1)
            {
                for (int y = 0; y < height; y += 1)
                {
                    Main.tile[x, y].active(false);
                    Main.tile[x, y].type = (ushort)ModContent.TileType<Spacerock2>();
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

            FilledSpaces = new List<FilledSpaceArea>();
            EmptySpaces = new List<Vector2>();

            DFSPDS(new Point(width / 2, height / 2), UniRand);

            #region oldstuff
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
            #endregion

            Rectangle rect = new Rectangle(400, 200, width - 800, height - 400);

            FilledSpaces = FilledSpaces.Where(testby => rect.Contains(new Point((int)testby.position.X / 16, (int)testby.position.Y / 16))).ToList();

            //Gen space station, thing here

            prog.Message = "Inviting Interstellar Guests";

            StationSpaces = new List<FilledSpaceArea>();
            SpaceStationStructure.StationObjects = new List<SpaceStationStructure>();
            SpaceStationStructure.ConnectionLines = new List<(Vector2, Vector2)>();

            MakeMainStation(UniRand);

            prog.Message = "Inviting Lesser Guests";

            MakeSmallStations(UniRand);

            EmptySpaces = EmptySpaces.Where(testby => rect.Contains(new Point((int)testby.X / 16, (int)testby.Y / 16)) && AsteriodDensity(new Point((int)testby.X / 16, (int)testby.Y / 16), 8) < 20 && Main.tile[(int)testby.X / 16, (int)testby.Y / 16].wall <= 0).ToList();

            //Cellular Crap

            prog.Message = "Smoothing Asteroids";

            SmoothOutAsteriods(UniRand);

            prog.Message = "Inviting Interstellar Guests In";

            SpaceStationStructure.GenerateAllStationRooms(UniRand);
            prog.Message = "Filling Guests";
            SpaceStationStructure.FillInAllStations(UniRand);

            Vector2 bossplace = EmptySpaces[0];

            NPC.NewNPC((int)bossplace.X, (int)bossplace.Y, ModContent.NPCType<SpaceBoss>());

            prog.Message = "Finishing Up";

            postMoonLord = false;

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
            SpaceStationStructure.UpdateStations();
            SGAmod.NoGravityItems = true;
            SGAmod.NoGravityItemsTimer = 10;
        }

        public override void Load()
        {
            Main.dayTime = false;
            Main.time = 0;
        }

        public static void AddStuffToChest(int chestid, int loottype, UnifiedRandom unirand)
        {

            if (chestid > -1)
            {

                List<int> lootmain = new List<int> { ItemID.MetalShelf,ItemID.MeteoriteBrick,ItemID.SilverCoin, ItemID.FrostCore };
                List<int> lootSemiRare = new List<int> {ItemID.FrostCore, ItemID.AncientBattleArmorMaterial };

                List<int> lootrare = new List<int> { ModContent.ItemType<ConcussionDevice>(), ModContent.ItemType<ExperimentalPathogen>(), ItemID.GoblinTech,ItemID.REK, ModContent.ItemType<PlasmaCell>()};

                int index = 0;
                int chestItem = 0;

                if (loottype == 0)//Outpost Chest
                {
                    if (unirand.Next(100) < 25)
                        lootmain.Add(ItemID.GoldCoin);
                    if (unirand.Next(100) < 10)
                        lootmain.Add(ModContent.ItemType<EmptyPlasmaCell>());

                    index = unirand.Next(0, lootrare.Count);
                    Main.chest[chestid].item[chestItem].SetDefaults(lootrare[index]);
                    Main.chest[chestid].item[chestItem].stack = 1;
                    chestItem += 1;
                }

                if (loottype == 0)//Main Station Chest
                {
                    if (unirand.Next(100) < 25)
                        lootmain.Add(ItemID.GoldCoin);

                    index = unirand.Next(0, lootrare.Count);
                    Main.chest[chestid].item[chestItem].SetDefaults(lootrare[index]);
                    Main.chest[chestid].item[chestItem].stack = 1;
                    chestItem += 1;
                }



                //Misc items
                for (int kk = 0; kk < unirand.Next(2, 3 + (Main.expertMode ? 6 : 3)); kk += 1)
                {
                    if (unirand.Next(0, 100) < 35)
                    {
                        index = unirand.Next(0, lootmain.Count);
                        Main.chest[chestid].item[chestItem].SetDefaults(lootmain[index]);
                        Main.chest[chestid].item[chestItem].stack = unirand.Next(2, unirand.Next(4, Main.expertMode ? 8 : 5));
                        chestItem += 1;
                    }

                    if (unirand.Next(0, 100) < 5)
                    {
                        index = unirand.Next(0, lootSemiRare.Count);
                        Main.chest[chestid].item[chestItem].SetDefaults(lootSemiRare[index]);
                        Main.chest[chestid].item[chestItem].stack = unirand.Next(1, Main.expertMode ? 4 : 3);
                        chestItem += 1;
                    }

                }
            }

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

        public static void StarryNebulaSky()
        {
            VertexBuffer vertexBuffer;

            UnifiedRandom alwaysthesame = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);

            int starterType = 15;

            for (int type = starterType; type > 0; type -= 1)
            {

                Vector2 parallex = new Vector2(Main.GlobalTime * alwaysthesame.NextFloat(0.02f, 0.04f), 0f);
                    float aspectRato = Main.screenWidth / Main.screenHeight;

                Effect effect = SGAmod.TrailEffect;

                effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Vector2.One) * WVP.Projection());
                effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("Space"));
                effect.Parameters["coordOffset"].SetValue(parallex);
                effect.Parameters["coordMultiplier"].SetValue(new Vector2(0.50f, 0.50f)*new Vector2(aspectRato,1f)*new Vector2(alwaysthesame.NextFloat(0.80f,1.75f), alwaysthesame.NextFloat(0.80f, 1.75f)));
                effect.Parameters["strength"].SetValue(0.05f+((starterType-type) / 60f)*2f);

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];

                Vector3 screenPos = new Vector3(-16, 0, 0);

                Color colorsa = Color.PaleTurquoise;

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
                effect.CurrentTechnique.Passes[type == starterType ? "BasicEffectPass" : "BasicEffectAlphaPass"].Apply();
                Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

            }

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

                if (!SpaceDim.SpaceBossIsActive && Main.rand.Next(0, 100) <= 2)
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
                if (npc2.modNPC != null)
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
            npc.defense = 250;
            npc.life = 5000;
            npc.lifeMax = 5000;
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
            if (GetType() != typeof(StationFocusCrystal))
            {
                //TileID.ExposedGems
                npc.type = NPCID.MeteorHead;
                if (GetType() == typeof(OverseenHeadBossShield))
                    Item.NewItem(npc.position, npc.width, npc.height, ItemID.Heart);
                else
                    Item.NewItem(npc.position, npc.width, npc.height, ModContent.ItemType<Glowrock>(), Main.rand.Next(1, 4));
            }

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

    public class StationFocusCrystal : OverseenHead, IDrawThroughFog
    {
        public SpaceStationStructure station;
        List<Player> playersInRange = new List<Player>();
        NPC boss = null;
        public float hitVisual = 0f;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Helios Focus Crystal");
        }

        public override string Texture => "SGAmod/Dimensions/Space/StationFocusCrystal";

        public override void SetDefaults()
        {
            npc.friendly = false;
            npc.defense = 50;
            npc.life = 7500;
            npc.lifeMax = 7500;
            npc.width = 64;
            npc.height = 64;
            npc.damage = 0;
            npc.aiStyle = -1;
            npc.noGravity = true;
            npc.knockBackResist = 0f;
        }

        public override bool CheckActive()
        {
            return false;
        }

        public override bool CheckDead()
        {
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, (int)npc.Center.X, (int)npc.Center.Y);
            if (sound != null)
            {
                sound.Pitch += Main.rand.NextFloat(0f, 0.25f);
            }
            hitVisual = 1f;
        }

        public override void AI()
        {
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            npc.localAI[0] += 1;
            hitVisual = Math.Max(hitVisual - 0.1f, 0f);

            npc.dontTakeDamage = true;

            int checkRange = 2400 * 2400;
            int checkRange2 = 420 * 420;

            playersInRange = Main.player.Where(testby => testby.active && !testby.dead && (testby.Center - npc.Center).LengthSquared() < checkRange).ToList();


            foreach (Player player in playersInRange)
            {
                if ((player.Center - npc.Center).LengthSquared() < checkRange)
                {
                    if ((player.Center - npc.Center).LengthSquared() < checkRange2)
                    {
                        npc.dontTakeDamage = false;
                    }

                    player.AddBuff(ModContent.BuffType<Buffs.MiningFatigue>(), 2);
                }

            }

            boss = null;

            if (SpaceDim.SpaceBossIsActive)
            {
                boss = Main.npc[NPC.FindFirstNPC(ModContent.NPCType<SpaceBoss>())];
                SpaceBoss trueBoss = boss.modNPC as SpaceBoss;

                float toCheckDist = 2400;
                float healScaling = 1;

                if (trueBoss.shieldeffect > 0)
                {
                    toCheckDist *= 3f;
                    healScaling /= 3f;
                }

                if (trueBoss.goingDark > 0 || trueBoss.DyingState || trueBoss.Sleeping || boss.ai[3] > 1 || (boss.Center-npc.Center).Length()> toCheckDist)
                {
                    boss = null;
                }
                if (boss != null)
                {
                    float healRate = (npc.lifeMax / (60f*30f)) * healScaling;
                    boss.life = Math.Min(boss.life + (int)healRate, boss.lifeMax);
                }
            }
            if (SpaceDim.crystalAsteriods)
                npc.localAI[1] = Math.Min(npc.localAI[1] + 0.005f, 1f);


            }

        public override void NPCLoot()
        {

            if (SpaceDim.crystalAsteriods)
            {
                Item.NewItem(npc.position+npc.Hitbox.Size()/2, 0,0, ModContent.ItemType<HeliosFocusCrystal>(), 1);
            }

            SoundEffectInstance snd = Main.PlaySound(SoundID.Shatter, (int)npc.Center.X, (int)npc.Center.Y);

            if (snd != null)
            {
                snd.Pitch = Main.rand.NextFloat(-0.75f, -0.25f);
            }

            for (int num1181 = 0; num1181 < 20; num1181++)
            {
                float num1182 = (float)num1181 / 20f;
                Vector2 vector123 = new Vector2(Main.rand.NextFloat() * 10f, 0f).RotatedBy(num1182 * -(float)Math.PI + Main.rand.NextFloat() * 0.1f - 0.05f);
                Gore gore8 = Gore.NewGoreDirect(npc.Center + vector123 * 3f, vector123, Utils.SelectRandom<int>(Main.rand, mod.GetGoreSlot("Gores/HeliosFocusCrystal_Gore1"), mod.GetGoreSlot("Gores/HeliosFocusCrystal_Gore2"), mod.GetGoreSlot("Gores/HeliosFocusCrystal_Gore3"), mod.GetGoreSlot("Gores/HeliosFocusCrystal_Gore4")));
                if (gore8.velocity.Y > 0f)
                {
                    Gore gore9 = gore8;
                    Gore gore2 = gore9;
                    gore2.velocity *= -0.5f;
                }
                if (gore8.velocity.Y < -5f)
                {
                    gore8.velocity.Y *= 0.8f;
                }
                gore8.velocity.Y *= 1.1f;
                gore8.velocity.X *= 0.88f;
            }


            for (float num475 = 4; num475 < 24f; num475 += 0.1f)
            {
                Vector2 startloc = (npc.Center);
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, ModContent.DustType<Dusts.LeviDust>());

                float anglehalf2 = Main.rand.NextFloat(MathHelper.TwoPi);

                Main.dust[dust].scale = 3f - Math.Abs(num475) / 16f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle * num475);
                Main.dust[dust].noGravity = true;
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            if (SGAmod.fogDrawNPCsCounter < 1)
            {
                DrawThroughFog(spriteBatch);
            }

            return false;
        }

        public void DrawThroughFog(SpriteBatch spriteBatch)
        {

            float overallAlpha = 1f;// MathHelper.Clamp(npc.ai[2] / 50f, 0f, 1f);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            if (playersInRange.Count > 0 || boss != null)
            {
                List<(Entity,Type)> ents = new List<(Entity, Type)>();
                ents.AddRange(playersInRange.Select(testby => (testby as Entity, testby.GetType())).ToList());
                if (boss != null)
                    ents.Add((boss,boss.GetType()));

                foreach ((Entity,Type) ent in ents)
                {
                    bool theBoss = (ent.Item2 == typeof(NPC));

                    List<Vector2> drawPoints = new List<Vector2>() { npc.Center, ent.Item1.Center };
                    Vector2 dist = drawPoints[1] - drawPoints[0];

                    for (float i = -1; i < 2; i += 1.5f)
                    {

                        TrailHelper trail2 = new TrailHelper("FadedBasicEffectAlphaPass", mod.GetTexture("TiledPerlin"));
                        trail2.projsize = Vector2.Zero;
                        trail2.coordOffset = new Vector2(((Main.GlobalTime / 6f)*i) + (npc.whoAmI * 9.245f), -Main.GlobalTime * 0.5f);
                        trail2.coordMultiplier = new Vector2(0.1f, dist.Length() / 512f);
                        trail2.doFade = false;
                        trail2.trailThickness = 16f;
                        trail2.strengthPow = 1.50f;
                        trail2.strength = 6f;
                        if (theBoss)
                        {
                            trail2.color = delegate (float percent)
                            {
                                return Color.MediumAquamarine*0.60f* (1f - (percent / 2f));
                            };
                        }
                        else
                        {
                            trail2.color = delegate (float percent)
                            {
                                return Color.OrangeRed* (1f-(percent/2f));
                            };
                        }
                        trail2.DrawTrail(drawPoints, npc.Center);
                    }
                }
            }


        Vector2 poz = npc.Center - Main.screenPosition;
            float size = 320f;

            if (poz.X > -size && poz.X < Main.screenWidth + size && poz.Y > -size && poz.Y < Main.screenHeight + size)
            {
                List<(float, float,float)> layers = new List<(float, float,float)>();

                for (float f2 = 1; f2 <= 3; f2 += 1f)
                {
                    for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
                    {
                        float extraAngle = ((f2-1f) / 3f) * MathHelper.Pi/2f;
                        float rotter = (Main.GlobalTime / 3f)+f+ extraAngle;

                        float realAnglez = (f/3f)+((MathHelper.TwoPi/3f)*(f2-1f))+(Main.GlobalTime / 3);

                        rotter = MathHelper.SmoothStep(rotter, realAnglez, npc.localAI[1]);

                        layers.Add((f,f2, rotter));
                    }
                }


                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                Texture2D mainTex = Main.npcTexture[npc.type];
                Texture2D otherTex = mod.GetTexture("Dimensions/Space/StationFocusCrystalSmall");
                Texture2D glowTex = mod.GetTexture("Glow");
                Texture2D glowTex2 = mod.GetTexture("GlowOrb");
                Vector2 halfGlow = glowTex.Size() / 2f;
                ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.StardustDye);

                Color colorz = Color.White;

                spriteBatch.Draw(glowTex2, npc.Center - Main.screenPosition, null, Color.White * 0.50f * overallAlpha, 0, glowTex2.Size()/2f, new Vector2(1f, 1f) * 1f, SpriteEffects.None, 0);

                if (npc.localAI[1] < 1)
                {
                    foreach ((float, float, float) rot in layers)
                    {
                        float f = rot.Item1;
                        float f2 = rot.Item2;
                        float rotter = rot.Item3;

                        float dister = MathHelper.SmoothStep((40f + (f2 * 8f)) + (float)Math.Sin((f2 * 2f) + (Main.GlobalTime * 2f)) * 10f, 42f, npc.localAI[1]);

                        float addedAngle = MathHelper.SmoothStep(0f, MathHelper.Pi, npc.localAI[1]);

                        spriteBatch.Draw(glowTex, npc.Center + (rotter.ToRotationVector2() * (dister)) - Main.screenPosition, null, Color.White * 1.50f * overallAlpha*(1f- npc.localAI[1]), rotter + MathHelper.PiOver2 + addedAngle, halfGlow, new Vector2(1f, 1f) * 1.25f, SpriteEffects.None, 0);
                    }
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                int intdexer = 0;
                UnifiedRandom randoz = new UnifiedRandom(npc.whoAmI);

                foreach ((float, float, float) rot in layers)
                {
                    float f = rot.Item1;
                    float f2 = rot.Item2;
                    float rotter = rot.Item3;

                    DrawData value7 = new DrawData(otherTex, new Vector2(30f, 30f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(randoz.Next(10000), randoz.Next(10000), otherTex.Width, otherTex.Height)), Microsoft.Xna.Framework.Color.White, npc.rotation, glowTex.Size() / 2f, npc.scale * 5f, SpriteEffects.None, 0);

                    float dister = MathHelper.SmoothStep((40f + (f2 * 8f)) + (float)Math.Sin((f2*2f) + (Main.GlobalTime * 2f)) * 10f, 42f, npc.localAI[1]);

                    float addedAngle = MathHelper.SmoothStep(0f, MathHelper.Pi, npc.localAI[1]);

                    stardustsshader.UseColor(Main.hslToRgb(((rotter/MathHelper.TwoPi)+ (f / MathHelper.TwoPi)) % 1f,1f-(npc.localAI[1]/1.5f),0.75f).ToVector3() * 1f);
                    stardustsshader.UseOpacity(1f);
                    stardustsshader.Apply(null, new DrawData?(value7));

                    intdexer += 1;

                    spriteBatch.Draw(otherTex, npc.Center + (rotter.ToRotationVector2() * (dister)) - Main.screenPosition, null, Color.White * 1f * overallAlpha, rotter+MathHelper.PiOver2+ addedAngle, otherTex.Size()/2f, new Vector2(1f, 1f), SpriteEffects.None, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 6f)
                {
                    spriteBatch.Draw(mainTex, npc.Center + (Vector2.UnitX.RotatedBy(f + Main.GlobalTime * 2f) * (4f+(hitVisual*2f)))-Main.screenPosition, null, Main.hslToRgb(f / MathHelper.TwoPi, 1f, 0.75f), 0, mainTex.Size() / 2f, 1f, SpriteEffects.None, 0f);
                }

                
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

               /* 
                DrawData value9 = new DrawData(otherTex, new Vector2(30f, 30f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, otherTex.Width, otherTex.Height)), Microsoft.Xna.Framework.Color.White, npc.rotation, glowTex.Size() / 2f, npc.scale * 5f, SpriteEffects.None, 0);
                stardustsshader.UseColor(colorz.ToVector3() * 0.60f);
                stardustsshader.UseOpacity(0.20f);
                stardustsshader.Apply(null, new DrawData?(value9));

                spriteBatch.Draw(mainTex, npc.Center - Main.screenPosition, null, Color.White * 1f * overallAlpha, 0, mainTex.Size() / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0);
               */
                
                Effect hallowed = SGAmod.HallowedEffect;

     /*           
hallowed.Parameters["prismColor"].SetValue(Color.White.ToVector3());
hallowed.Parameters["prismAlpha"].SetValue(1f);
hallowed.Parameters["overlayStrength"].SetValue(new Vector3(2f, 0.10f, Main.GlobalTime / 4f));
hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
hallowed.Parameters["rainbowScale"].SetValue(0.8f);
     */

                hallowed.Parameters["alpha"].SetValue(1f);

                hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f,1f));
                hallowed.Parameters["overlayTexture"].SetValue(ModContent.GetTexture("SGAmod/Voronoi"));
                hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, 0, 0f));
                hallowed.Parameters["overlayAlpha"].SetValue( (1f-(npc.life / (float)npc.lifeMax))*1f);
                hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f,1f,1f));

                hallowed.CurrentTechnique.Passes["TextureMixAdditive"].Apply();             

                spriteBatch.Draw(mainTex, npc.Center - Main.screenPosition, null, Color.White * 1f * overallAlpha, 0, mainTex.Size() / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0);

                if (hitVisual > 0)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                    Effect fadeIn = SGAmod.FadeInEffect;

                    fadeIn.Parameters["alpha"].SetValue(hitVisual);
                    fadeIn.Parameters["strength"].SetValue(1f);
                    fadeIn.Parameters["fadeColor"].SetValue(Color.White.ToVector3());
                    fadeIn.Parameters["blendColor"].SetValue(Color.White.ToVector3());

                    fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();
                    spriteBatch.Draw(mainTex, npc.Center - Main.screenPosition, null, Color.White * 1f * overallAlpha, 0, mainTex.Size() / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0);

                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            }
        }

    }

    public class FallingSpaceRock : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Falling Comet");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        }

        public override string Texture => "Terraria/Projectile_607";

        public override void SetDefaults()
        {
            projectile.width = 24;
            projectile.height = 24;
            projectile.friendly = false;
            projectile.hostile = true;
            projectile.tileCollide = true;
            projectile.alpha = 40;
            projectile.timeLeft = 9000;
            projectile.light = 0.75f;
            projectile.extraUpdates = 2;
        }

        public override bool PreKill(int timeLeft)
        {
            SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_BetsysWrathImpact, (int)projectile.Center.X, (int)projectile.Center.Y);

            if (snd != null)
            {
                snd.Pitch = Main.rand.NextFloat(0.25f, 0.60f);
            }

            float velocityAngle = projectile.velocity.ToRotation().AngleLerp(MathHelper.PiOver2, 0.75f);

            for (int i = 0; i < 60; i += 1)
            {
                Vector2 offset = (velocityAngle+(MathHelper.Pi+Main.rand.NextFloat(-0.35f,0.35f))).ToRotationVector2();
                int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BlueCrystalShard);
                Main.dust[dust].scale = 1.5f;
                Main.dust[dust].alpha = 150;
                Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(1f * Main.rand.NextFloat(0f, 3f)*(i/5f));
                Main.dust[dust].noGravity = true;
            }

            for (int num1181 = 0; num1181 < 20; num1181++)
            {
                float num1182 = (float)num1181 / 15f;
                Vector2 vector123 = new Vector2((num1182*0.80f)+0.15f, 0f).RotatedBy(Main.rand.NextFloat(-0.25f,0.25f));
                Gore gore = Gore.NewGoreDirect(projectile.position, Vector2.Zero, Utils.SelectRandom<int>(Main.rand, 375, 376, 377), 0.75f);

                if (gore != null)
                {
                    gore.velocity = vector123.RotatedBy(velocityAngle + MathHelper.Pi)*12f;
                    //gore.velocity.Y -= 2f;
                    //gore.timeLeft = 90;
                }
            }


            for (int num1181 = 0; num1181 < 20; num1181++)
            {
                float num1182 = (float)num1181 / 20f;
                Vector2 vector123 = new Vector2(Main.rand.NextFloat() * 10f, 0f).RotatedBy(num1182 * -(float)Math.PI + Main.rand.NextFloat() * 0.1f - 0.05f);
                int itemz = Item.NewItem(projectile.Center+ vector123*3, ModContent.ItemType<Glowrock>(), Main.rand.Next(1, 4));
                if (itemz >= 0)
                {
                    Main.item[itemz].velocity = (Vector2.Normalize(vector123)*0.50f)+(vector123*0.75f);
                }
            }

            return true;
        }

        public override void AI()
        {
            projectile.localAI[0] += 1;

            if (projectile.localAI[0] == 1)
            {
                projectile.localAI[1] = Main.rand.Next(10000);
            }

            if (projectile.localAI[0] % 60 == 0)
            {
                SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, 7);

                if (snd != null)
                {
                    snd.Pitch = -0.75f+(projectile.timeLeft/9000f);
                }
            }

            projectile.rotation += ((projectile.localAI[1] - 5000f) / 5000f) * (MathHelper.Pi * 0.01f);
            projectile.spriteDirection = projectile.velocity.X > 0 ? 1 : -1;
            projectile.velocity = projectile.velocity.RotatedBy((float)Math.Sin((projectile.localAI[0] + projectile.localAI[1]) / 120f) * 0.002f);

            Vector2 offset = Vector2.Normalize(projectile.velocity.RotatedByRandom(MathHelper.Pi / 12f)).RotatedBy(MathHelper.Pi);
            int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.BlueCrystalShard);
            Main.dust[dust].scale = 0.5f;
            Main.dust[dust].alpha = 150;
            Main.dust[dust].velocity = Vector2.Normalize(offset) * (float)(1f * Main.rand.NextFloat(0f, 3f));
            Main.dust[dust].noGravity = true;

        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

            bool near = false;
            int bufferRange2 = 240;
            Rectangle bufferrect2 = new Rectangle((int)Main.screenPosition.X - bufferRange2, (int)Main.screenPosition.Y - bufferRange2, (int)Main.screenPosition.X + Main.screenWidth + bufferRange2, (int)Main.screenPosition.Y + Main.screenHeight + bufferRange2);

            if (!bufferrect2.Contains(projectile.Center.ToPoint()))
            {
                return false;
            }


            Texture2D trailTex = SGAmod.ExtraTextures[110];
            Texture2D asteriodTex = mod.GetTexture("Dimensions/Space/BlueAsteroidSmall" + (projectile.localAI[1]%2 > 0 ? "" : "2"));
            float alphaFade = Math.Min(projectile.timeLeft/300f,1f);
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(trailTex.Width / 2f, trailTex.Height / 6f);

            float lastpos = (-projectile.velocity).ToRotation();

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            Color colorz = Color.PaleTurquoise;

            Texture2D glowTex = mod.GetTexture("GlowOrb");
            ArmorShaderData stardustsshader = GameShaders.Armor.GetShaderFromItemId(ItemID.VortexDye);

            DrawData value8 = new DrawData(glowTex, new Vector2(30f, 30f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 64, 64)), Microsoft.Xna.Framework.Color.White, projectile.rotation, glowTex.Size() / 2f, projectile.scale * 5f, SpriteEffects.None, 0);
            stardustsshader.UseColor(colorz.ToVector3());
            stardustsshader.UseOpacity(0.15f);
            stardustsshader.Apply(null, new DrawData?(value8));


            for (int i = projectile.oldPos.Length - 1; i > 0; i -= 1)
            {
                float alpha = 1f-(i / (float)projectile.oldPos.Length);
                Vector2 pos1 = projectile.oldPos[i] + (projectile.Hitbox.Size() / 2f);
                spriteBatch.Draw(trailTex, pos1 - Main.screenPosition, null, Color.Aqua * alphaFade* alpha * 0.20f, lastpos + MathHelper.PiOver2, origin, (1f-(i/ (float)projectile.oldPos.Length)),SpriteEffects.None, 0f);
                if (i > 0)
                {
                    Vector2 pos2 = projectile.oldPos[i-1] + (projectile.Hitbox.Size() / 2f);
                    lastpos = (pos2 - pos1).ToRotation();
                }
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


            origin = new Vector2(asteriodTex.Width, asteriodTex.Height / 2) / 2f;

            spriteBatch.Draw(asteriodTex, projectile.Center - Main.screenPosition, new Rectangle(0, 0, asteriodTex.Width, asteriodTex.Height / 2), Color.White * alphaFade * 1f, projectile.rotation, origin, 1f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);


            return false;
        }

    }


}