using System.IO;
using System;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.GameContent.Shaders;
using System.Linq;
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
using SGAmod;
using SGAmod.Dimensions;
using SGAmod.Dimensions.NPCs;
using SGAmod.NPCs.DeeperDungeons;

namespace SGAmod.Dimensions
{

    public struct DungeonTile
    {
        public Vector2 vector;
        public int type;
        public int subtype;
        public int hallwayindex;
        public float lightchance;
        public bool floor;
        public int lootroomid;
        //0=normal hallwall (subtype: 1-traps)
        //1=spikey hallwall (subtype: 1-cobwebs, 2-ice hallways, 3-thorns)
        //2=platformed stairs (subtype: 1-traps)
        //3=loot room, doesn't generate minirooms
        //4=hallway leading into loot room

        public DungeonTile(Vector2 vect, int type2, int hallwayindex, bool floor, int lootroomid = 0, int subtype = 0)
        {
            vector = vect;
            type = type2;
            this.hallwayindex = hallwayindex;
            this.lightchance = DeeperDungeon.lightchancefortiles;
            this.floor = floor;
            this.lootroomid = lootroomid;
            this.subtype = subtype;
        }
    }
    public struct DungeonPath
    {
        public Vector2 vector;
        public Vector2 direction;
        public int branchingchance;
        public int branched;

        public DungeonPath(Vector2 vect, Vector2 direction2, int branchingchance, int branched)
        {
            vector = vect;
            direction = direction2;
            this.branchingchance = branchingchance;
            this.branched = branched;
        }
    }
    public struct LootRoomLocation
    {
        public Vector2 vector;
        public Vector2 size;

        public LootRoomLocation(Vector2 vector, Vector2 size)
        {
            this.vector = vector;
            this.size = size;
        }
    }
    public struct Platforms
    {
        public int Y;
        public int hallwayindex;

        public Platforms(int Y, int hallwayindex)
        {
            this.Y = Y;
            this.hallwayindex = hallwayindex;
        }
    }
    public class DeeperDungeon : SGAPocketDim
    {
        public override int width => 1000;
        public override int height => 600;
        public override bool saveSubworld => false;
        public override string DimName => "Deeper Dungeons";
        public override int DimType => 2;
        public override float maxSpawns => 1f;
        public override float spawnRate => Math.Max(0.25f, 0.75f - ((float)SGAWorld.dungeonlevel * 0.10f));

        public static int globallineroomindex = 0;
        public static DeeperDungeon instance;

        public static bool hardMode = false;
        public static bool postPlantera = false;

        public static int DungeonTile = TileID.BlueDungeonBrick;
        public static int DungeonWall = WallID.BlueDungeonUnsafe;
        public static int DungeonWallLoot = WallID.BluegreenWallpaper;
        public static ushort DungeonPlatform = TileID.TeamBlockBluePlatform;
        public static int DungeonLightFloor = 16;
        public static int DungeonLightCeiling = 34;
        public static ushort SpikeType = TileID.Spikes;
        public static int[,] Spikeclustersizes = { { 5, 10 }, { 2, 4 } };
        public static int[] Spikeclusterchance = { 45, 20, 45, 15 };
        public static float[] Spikeclustersizemul = { 0.3f, 0.75f, 0.3f, 0.6f };
        public static int platformmaxsize = 8;
        public static int platformmaxdist = 5;
        public static int platformchance = 5;
        public static int lightschance = 50;
        public static int paintingchance = 1000;
        public static int branchoffchance = 40;
        public static float branchedPathsScale = 1f;
        public static float lightchancefortiles = 1f;

        public static int LootRoomMaxTiles = 75;
        public static int LootRoomTries = 15;

        public static List<Platforms> lastplatformy = new List<Platforms>();
        public static List<LootRoomLocation> LootRooms = new List<LootRoomLocation>();

        public static List<DungeonPath> pathways = new List<DungeonPath>();
        public static List<DungeonPath> pathwayrooms = new List<DungeonPath>();
        public static List<Vector2> pathwayloothalls = new List<Vector2>();

        public override Texture2D GetMapBackgroundImage()
        {
            Texture2D bg = (Texture2D)typeof(Main).GetField("mapBG5Texture", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).GetValue(Main.instance);
            return bg;
        }

        public override int? Music
        {
            get
            {
                return MusicID.Dungeon;
            }
        }

        public static void SetupDungeon(DeeperDungeon dung)
        {
            DeeperDungeon.instance = dung;
            DeeperDungeon.DungeonTile = TileID.BlueDungeonBrick;
            DeeperDungeon.DungeonWall = WallID.BlueDungeonUnsafe;
            DeeperDungeon.DungeonPlatform = TileID.TeamBlockBluePlatform;
            DeeperDungeon.DungeonWallLoot = WallID.BluegreenWallpaper;
            DeeperDungeon.Spikeclustersizes[0, 0] = 5; DeeperDungeon.Spikeclustersizes[0, 1] = 10; DeeperDungeon.Spikeclustersizes[1, 0] = 2; DeeperDungeon.Spikeclustersizes[1, 1] = 4;
            DeeperDungeon.Spikeclusterchance[0] = 45; DeeperDungeon.Spikeclusterchance[1] = 20; DeeperDungeon.Spikeclusterchance[2] = 45; DeeperDungeon.Spikeclusterchance[3] = 15;
            DeeperDungeon.Spikeclustersizes = new[,] { { 5, 10 }, { 2, 4 } };
            DeeperDungeon.Spikeclusterchance = new[] { 45, 20, 45, 15 };
            DeeperDungeon.Spikeclustersizemul = new[] { 0.3f, 0.75f, 0.3f, 0.6f };
            DeeperDungeon.platformmaxsize = 8;
            DeeperDungeon.platformmaxdist = 5;
            DeeperDungeon.platformchance = 15;
            DeeperDungeon.lightschance = 50;
            DeeperDungeon.branchoffchance = 20;
            DeeperDungeon.paintingchance = 20000;
            DeeperDungeon.globallineroomindex = 0;
            DeeperDungeon.lightchancefortiles = 1f;
            DeeperDungeon.LootRoomMaxTiles = 160;
            DeeperDungeon.LootRoomTries = 10;
            DeeperDungeon.branchedPathsScale = 1f;
            DeeperDungeon.pathways.Clear();
            DeeperDungeon.pathwayrooms.Clear();
            DeeperDungeon.pathwayloothalls.Clear();
            DeeperDungeon.LootRooms.Clear();

            dung.PlaceRoomBlock(new UnifiedRandom(dung.enemyseed), true);

            dung.LimitPlayers = 16;

            dung.chooseenemies = false;

            dung.EnemySpawnsOverride = delegate (IDictionary<int, float> pool, NPCSpawnInfo spawnInfo, SGAPocketDim pocket)
            {
                UnifiedRandom UniRand = new UnifiedRandom(pocket.enemyseed);
                for (int i = 0; i < pool.Count; i += 1)
                {
                    pool[i] = 0f;

                }

                WeightedRandom<Vector2> rando = new WeightedRandom<Vector2>();
                rando.Add(new Vector2(NPCID.AngryBones, 0.25f), 1f);
                if (UniRand.Next(0, 1) == 0)
                    rando.Add(new Vector2(NPCID.AngryBonesBig, 0.25f), 0.9f);
                if (UniRand.Next(0, 1) == 0)
                    rando.Add(new Vector2(NPCID.AngryBonesBigMuscle, 0.25f), 0.9f);
                if (UniRand.Next(0, 1) == 0)
                    rando.Add(new Vector2(NPCID.AngryBonesBigHelmet, 0.25f), 0.9f);
                if (UniRand.Next(0, 2) == 0)
                {
                    rando.Add(new Vector2(NPCID.PantlessSkeleton, 0.4f), 1.2f);
                    if (UniRand.Next(0, 2) == 0)
                        rando.Add(new Vector2(NPCID.Skeleton, 0.8f), 1.5f);
                    if (UniRand.Next(0, 2) == 0)
                        rando.Add(new Vector2(ModContent.NPCType<SkeletonCrossbower>(), 0.8f), 1.5f);
                }

                if (UniRand.Next(0, 2) == 0)
                {
                    rando.Add(new Vector2(NPCID.CursedSkull, 0.25f), 0.8f);
                    rando.Add(new Vector2(NPCID.DungeonSlime, 0.05f), 0.6f);
                    rando.Add(new Vector2(NPCID.DarkCaster, 0.25f), 0.8f);
                    rando.Add(new Vector2(ModContent.NPCType<HellCaster>(), 0.2f), 0.5f);
                }
                else
                {
                    rando.Add(new Vector2(NPCID.FireImp, 0.25f), 0.8f);
                    rando.Add(new Vector2(NPCID.BoneSerpentHead, 0.05f), 0.6f);
                    rando.Add(new Vector2(NPCID.Hellbat, 0.25f), 0.8f);
                    rando.Add(new Vector2(ModContent.NPCType<ChaosCaster>(), 0.2f), 0.5f);
                }

                if (SGAWorld.dungeonlevel > 2)
                {
                    rando.Add(new Vector2(NPCID.ZombieElf, 0.13f + (SGAWorld.dungeonlevel - 3) * 0.2f), 0.4f + ((SGAWorld.dungeonlevel - 3) * 0.2f));
                    rando.Add(new Vector2(NPCID.Scarecrow1, 0.13f + (SGAWorld.dungeonlevel - 3) * 0.2f), 0.4f + ((SGAWorld.dungeonlevel - 3) * 0.2f));
                }

                rando.Add(new Vector2(ModContent.NPCType<DungeonBat>(), 0.3f), 0.8f);
                rando.Add(new Vector2(ModContent.NPCType<FastSkeleton>(), 0.3f), 0.8f);

                if (hardMode)
                {
                    rando.Add(new Vector2(NPCID.BlazingWheel, 0.05f), 0.2f);
                    rando.Add(new Vector2(NPCID.SpikeBall, 0.05f), 0.2f);
                    if (UniRand.Next(0, 1) == 0)
                        rando.Add(new Vector2(ModContent.NPCType<LaserSkeleton>(), 0.25f), 0.7f);
                    if (UniRand.Next(0, 1) == 0)
                        rando.Add(new Vector2(ModContent.NPCType<SkeletonGunner>(), 0.5f), 0.6f);
                    if (UniRand.Next(0, 2) == 0)
                        rando.Add(new Vector2(NPCID.SkeletonArcher, 0.8f), 1.5f);
                    if (SGAWorld.dungeonlevel > 5)
                    {
                        if (UniRand.Next(0, 2) == 0)
                            rando.Add(new Vector2(ModContent.NPCType<DungeonMimic>(), 0.2f + (SGAWorld.dungeonlevel - 5) * 0.2f), 0.2f);
                    }
                }

                if (postPlantera)
                {
                    int enemySet = UniRand.Next(0, 3);
                    if (enemySet == 0)
                    {
                        rando.Add(new Vector2(NPCID.BlueArmoredBones, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.BlueArmoredBonesMace, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.BlueArmoredBonesNoPants, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.BlueArmoredBonesSword, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.Necromancer, 0.25f), 0.6f);
                        rando.Add(new Vector2(NPCID.NecromancerArmored, 0.25f), 0.6f);
                        rando.Add(new Vector2(NPCID.SkeletonCommando, 0.25f), 0.8f);
                        rando.Add(new Vector2(NPCID.Paladin, 0.3f), 0.6f);        
                    }
                    if (enemySet == 1)
                    {
                        rando.Add(new Vector2(NPCID.RustyArmoredBonesAxe, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.RustyArmoredBonesFlail, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.RustyArmoredBonesSword, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.RustyArmoredBonesSwordNoArmor, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.RaggedCaster, 0.25f), 0.6f);
                        rando.Add(new Vector2(NPCID.RaggedCasterOpenCoat, 0.25f), 0.6f);
                        rando.Add(new Vector2(NPCID.SkeletonSniper, 0.25f), 0.8f);
                        rando.Add(new Vector2(NPCID.GiantCursedSkull, 0.2f), 0.4f);
                    }
                    if (enemySet == 2)
                    {
                        rando.Add(new Vector2(NPCID.HellArmoredBones, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.HellArmoredBonesSpikeShield, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.HellArmoredBonesMace, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.HellArmoredBonesSword, 0.8f), 1.3f);
                        rando.Add(new Vector2(NPCID.DiabolistRed, 0.25f), 0.6f);
                        rando.Add(new Vector2(NPCID.DiabolistWhite, 0.25f), 0.6f);
                        rando.Add(new Vector2(NPCID.TacticalSkeleton, 0.25f), 0.8f);
                        rando.Add(new Vector2(NPCID.GiantCursedSkull, 0.2f), 0.4f);
                    }
                    rando.Add(new Vector2(NPCID.BoneLee, 0.25f), 0.8f);
                    rando.Add(new Vector2(NPCID.DungeonSpirit, 0.3f + SGAWorld.dungeonlevel * 0.2f), 0.3f + SGAWorld.dungeonlevel * 0.2f);
                    if (UniRand.Next(0, 1) == 0)
                        rando.Add(new Vector2(ModContent.NPCType<EvilCaster>(), 0.25f), 0.7f);
                    else
                        rando.Add(new Vector2(ModContent.NPCType<RuneCaster>(), 0.25f), 0.4f);
                }


                for (int i = 0; i < 5; i += 1)
                {
                    Vector2 index = rando.Get();
                    pool[(int)index.X] = index.Y;
                    rando.needsRefresh = true;
                }
                pocket.chooseenemies = true;
                return 1;
            };
        }


        public virtual void PlaceRoomBlock(UnifiedRandom unirand, bool alwaysdo = false)
        {
            if (unirand.Next(0, 500000) < 1 || alwaysdo)
            {
                int[] tilestopick = { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick };
                int[] wallsBlue = { WallID.BlueDungeonUnsafe, WallID.BlueDungeonSlabUnsafe, WallID.BlueDungeonTileUnsafe };
                int[] wallsGreen = { WallID.GreenDungeonUnsafe, WallID.GreenDungeonSlabUnsafe, WallID.GreenDungeonTileUnsafe };
                int[] wallsPink = { WallID.PinkDungeonUnsafe, WallID.PinkDungeonSlabUnsafe, WallID.PinkDungeonTileUnsafe };
                int picker = unirand.Next(0, 3);
                int picker2 = unirand.Next(0, 3);

                if (tilestopick[picker] == TileID.BlueDungeonBrick)
                {
                    DeeperDungeon.DungeonWall = wallsBlue[picker2];
                }
                if (tilestopick[picker] == TileID.GreenDungeonBrick)
                {
                    DeeperDungeon.DungeonWall = wallsGreen[picker2];
                }
                if (tilestopick[picker] == TileID.PinkDungeonBrick)
                {
                    DeeperDungeon.DungeonWall = wallsPink[picker2];
                }

                DeeperDungeon.DungeonTile = tilestopick[picker];
            }
        }


        public virtual void PlatformBlockType(Tile thetile, ref int platformtype)
        {
            if (thetile.type == TileID.BlueDungeonBrick || thetile.type == TileID.TeamBlockBluePlatform)
                platformtype = TileID.TeamBlockBluePlatform;
            if (thetile.type == TileID.GreenDungeonBrick || thetile.type == TileID.TeamBlockGreenPlatform)
                platformtype = TileID.TeamBlockGreenPlatform;
            if (thetile.type == TileID.PinkDungeonBrick || thetile.type == TileID.TeamBlockPinkPlatform)
                platformtype = TileID.TeamBlockPinkPlatform;

        }
        public virtual void AGenPass(GenerationProgress prog)
        {
            startover:

            UnifiedRandom UniRand = new UnifiedRandom(DimDungeonsProxy.DungeonSeeds);
            enemyseed = (DimDungeonsProxy.DungeonSeeds);
            int lastseed = WorldGen._genRandSeed;
            WorldGen._genRandSeed = DimDungeonsProxy.DungeonSeeds;
            prog.Message = "Loading"; //Sets the text above the worldgen progress bar
            Main.worldSurface = Main.maxTilesY - 2; //Hides the underground layer just out of bounds
            Main.rockLayer = Main.maxTilesY; //Hides the cavern layer way out of bounds
            //Main.spawnTileX = (Main.maxTilesX / 2) / 16;
            prog.Message = "Going deeper...";
            prog.Value = 0f;
            int formerseed = WorldGen._lastSeed;
            DeeperDungeon.SetupDungeon(this);
            List<DungeonTile> allareas = new List<DungeonTile>();

            for (int x = 0; x < Main.maxTilesX; x += 1)
            {
                for (int y = 0; y < Main.maxTilesY; y += 1)
                {
                    Tile thetile = Framing.GetTileSafely(x, y);
                    thetile.active(true);
                    thetile.type = TileID.Dirt;
                }
            }

            //DeeperDungeon.MakeRoom(new Vector2((Main.maxTilesX / 2),(Main.maxTilesY / 2)), new Vector2(20,20), ref allareas,UniRand);

            int subtype = 0;

            //Make Serpentine-like hallways

            List<Vector2> previousareas = new List<Vector2>();
            Rectangle bounds = new Rectangle(100, 100, width - 200, height - 200);

            Vector2 randomloc;
            int maxhallway = 30;
            int hallwaycount = 0;
            randomloc = new Vector2(UniRand.Next(120, width - 240), UniRand.Next(120, height - 240));
            Vector2 flowpath = (new Vector2(width, height) / 2) - randomloc;
            Vector2 end = randomloc;
            flowpath.Normalize();
            end = randomloc;

            backtostart:

            previousareas.Add(randomloc);
            Main.spawnTileX = (int)(randomloc.X);
            Main.spawnTileY = (int)(randomloc.Y);
            for (int i = 30; i < 120; i += 3)
            {
                Vector2 gothere = (flowpath.ToRotation() + MathHelper.ToRadians(UniRand.Next(-i, i))).ToRotationVector2();
                float dist = UniRand.NextFloat(40, 100);
                Vector2 tryhere = randomloc + (gothere * dist);

                //DeeperDungeon.pathways

                //previousareas.Find(thisdist => (thisdist- tryhere).Length()

                DungeonPath dedung = new DungeonPath(new Vector2(-99999, 0), new Vector2(-99999, 0), 0, 0);
                dedung = DeeperDungeon.pathways.Find(thisdist => (thisdist.vector - tryhere).Length() < 30);

                if (bounds.Contains((int)tryhere.X, (int)tryhere.Y) && (dedung.vector.X > -50))
                {
                    flowpath = tryhere - randomloc;
                    flowpath.Normalize();

                    subtype = 0;
                    if (UniRand.Next(0, 3) == 0)
                    {
                            subtype = 1;
                    }

                    MakeRoomLine(randomloc, tryhere, new Vector2(16, 16), ref allareas, UniRand, filltype: Math.Abs(flowpath.Y) > 0.75f ? 2 : 0, stepsize: UniRand.Next(8, 12), subtype: subtype);
                    randomloc = tryhere;
                    hallwaycount += 1;
                    if (hallwaycount > maxhallway)
                    {
                        goto endit;
                    }
                    goto backtostart;
                }

            }

        endit:

            end = DeeperDungeon.pathways[3].vector;

            //end is the start, place exit portal there

            NPC.NewNPC((int)(end.X * 16f), (int)(end.Y * 16f), ModContent.NPCType<DungeonPortalDeeperDungeons>());

            //MakeRoomLine(new Vector2(100, 100), new Vector2(width-200, 200), new Vector2(12, 12), ref allareas, UniRand,filltype: 0);
            //MakeRoomLine(new Vector2(width - 200, 200), new Vector2(width - 200, height-50), new Vector2(10, 10), ref allareas, UniRand, filltype: 2);


            prog.Message = "Branching Paths";


            //Make Branched Paths
            DeeperDungeon.branchoffchance = 40;
            DeeperDungeon.lightchancefortiles = 0.5f;
            for (int repeat = 0; repeat < 3; repeat += 1)
            {
                Vector2 roomsize = new Vector2(10 + UniRand.Next(0, 8) - repeat, 10 + UniRand.Next(0, 8) - repeat) * DeeperDungeon.branchedPathsScale;
                for (int i = 0; i < DeeperDungeon.pathways.Count; i += 1)
                {

                    if (UniRand.Next(0, DeeperDungeon.pathways[i].branchingchance) == 0)
                    {
                        float angle = MathHelper.ToRadians(UniRand.NextBool() ? UniRand.Next(60, 120) : -UniRand.Next(60, 120));
                        Vector2 where = (DeeperDungeon.pathways[i].direction.RotatedBy(angle));
                        where.Normalize();
                        Vector2 offsetangle = DeeperDungeon.pathways[i].vector + (where * UniRand.NextFloat(40, 90));
                        Rectangle rect = new Rectangle(60, 60, width - 120, height - 120);
                        Rectangle rect2 = new Rectangle((int)(offsetangle.X - roomsize.X / 2), (int)(offsetangle.Y - roomsize.X / 2), (int)roomsize.X, (int)roomsize.Y);

                        if (rect.Intersects(rect2))
                        {
                            subtype = 0;

                            int filltype = UniRand.NextBool() && Math.Abs(where.Y) > 0.75f ? 2 : (UniRand.Next(5)<2 ? 0 : 1);

                            if (UniRand.Next(0, 2) == 0)
                                subtype = 1;

                            if (filltype == 1)
                            {

                                if (UniRand.Next(0, 20) == 0)
                                {
                                    subtype = 2;
                                }
                                else
                                {
                                    if (UniRand.Next(0, 15) == 0)
                                        subtype = 3;
                                }
                            }

                            MakeRoomLine(DeeperDungeon.pathways[i].vector, offsetangle, roomsize, ref allareas, UniRand, filltype: filltype, maxgencheck: 7, gen: repeat, stepsize: 4, branched: repeat + 1, subtype: subtype);

                        }

                    }
                    prog.Value = ((float)(repeat * DeeperDungeon.pathways.Count) + (float)i) / ((float)DeeperDungeon.pathways.Count * 3) * 0.25f;
                }
            }


                DeeperDungeon.PlaceLootRooms(UniRand, ref allareas);

            allareas = allareas.Distinct().ToList();

            prog.Message = "Carving Rooms...";

            //Post fill in           

            List<DungeonTile> lootrooms = new List<DungeonTile>();

            for (int x = 0; x < allareas.Count; x += 1)
            {
                Tile thetile = Framing.GetTileSafely((int)allareas[x].vector.X, (int)allareas[x].vector.Y);
                thetile.active(false);
                thetile.type = TileID.BlueDungeonBrick;
                if (allareas[x].type == 4)
                {
                    DeeperDungeon.pathwayloothalls.Add(allareas[x].vector);
                }
                if (allareas[x].type == 3)
                {
                    lootrooms.Add(allareas[x]);
                }
                prog.Value = 0.25f + ((float)x / (float)allareas.Count) * 0.25f;
            }

            prog.Message = "Adding pointy edges...";


            AddSpikes(UniRand, ref allareas);

            int maxtiles = width * height;
            int cob = 0;


            prog.Message = "Cobweb Police";
            for (int xx = 0; xx < width; xx += 1)
            {
                for (int yy = 0; yy < height; yy += 1)
                {
                    if (Main.tile[xx, yy].type == TileID.Cobweb)
                        cob += 1;

                }
            }

            if (cob > maxtiles / 6)
            {
                //Gen failed, start over
                DimDungeonsProxy.DungeonSeeds += 1;
                goto startover;
            }




                prog.Value = 0.5f;
            prog.Message = "Hiding the Loot...";

            //Fill Loot Rooms
            for (int x = 0; x < lootrooms.Count; x += 1)
            {
                Tile thetile = Framing.GetTileSafely((int)lootrooms[x].vector.X, (int)lootrooms[x].vector.Y);
                IDGWorldGen.PlaceMulti(lootrooms[x].vector, DeeperDungeon.DungeonTile, 2, DeeperDungeon.DungeonWall);
            }
            for (int x = 0; x < DeeperDungeon.pathwayloothalls.Count; x += 1)
            {
                Tile thetile = Framing.GetTileSafely((int)DeeperDungeon.pathwayloothalls[x].X, (int)DeeperDungeon.pathwayloothalls[x].Y);
                thetile.active(false);
            }
            for (int x = 0; x < lootrooms.Count; x += 1)
            {
                Tile thetile = Framing.GetTileSafely((int)lootrooms[x].vector.X, (int)lootrooms[x].vector.Y);
                thetile.wall = (ushort)DeeperDungeon.DungeonWallLoot;
                if (lootrooms[x].floor)
                {
                    Tile thetile2 = Framing.GetTileSafely((int)lootrooms[x].vector.X, (int)lootrooms[x].vector.Y + 1);
                    if (!thetile2.active())
                        WorldGen.PlaceTile((int)lootrooms[x].vector.X, (int)lootrooms[x].vector.Y + 1, DeeperDungeon.DungeonPlatform);
                }
                thetile.active(false);
            }

            //Smooth Ice chunks
            prog.Message = "Smoothing (possible) Ice N Thorns";

            for (int passes = 0; passes < 4; passes += 1)
            {
                for (int y = 0; y < Main.maxTilesY; y += 1)
                {
                    for (int x = 0; x < Main.maxTilesX; x += 1)
                    {
                        if (Main.tile[x, y].type != TileID.BreakableIce)// && Main.tile[x, y].type != TileID.JungleThorns && Main.tile[x, y].type != TileID.CorruptThorns && Main.tile[x, y].type != TileID.CrimtaneThorns)
                            continue;

                        if (GetTilesAround(x, y, 1) > UniRand.Next(3, 6))
                            Main.tile[x, y].active(true);
                        else
                            Main.tile[x, y].active(false);
                    }
                }
            }



            //Add the chests and deco to loot rooms


            prog.Value = 0.70f;
            prog.Message = "Lootin it up";

            int choice2 = UniRand.Next(0, DeeperDungeon.LootRooms.Count);
            for (int thisone = 0; thisone < DeeperDungeon.LootRooms.Count; thisone += 1)
            {
                int choice = thisone;
                int size = (int)DeeperDungeon.LootRooms[choice].size.X;
                int achest = WorldGen.PlaceChest((int)DeeperDungeon.LootRooms[choice].vector.X + UniRand.Next(-size / 3, size / 3),
                    (int)(DeeperDungeon.LootRooms[choice].vector.Y + ((int)DeeperDungeon.LootRooms[choice].size.Y) / 2) - 1, 21, false, choice2 == choice ? 4 : 1);

                DeeperDungeon.AddStuffToChest(achest, choice2 == choice ? 2 : 0, UniRand);

                WorldGen.PlaceObject((int)DeeperDungeon.LootRooms[choice].vector.X, (int)(DeeperDungeon.LootRooms[choice].vector.Y - ((int)DeeperDungeon.LootRooms[choice].size.Y) / 2), TileID.Chandeliers, false, 20);


                List<int> barTypes = new List<int>();
                barTypes.Add(4); barTypes.Add(5); barTypes.Add(6); barTypes.Add(7); barTypes.Add(9); barTypes.Add(10); barTypes.Add(SGAmod.Instance.TileType("UnmanedBarTile")); barTypes.Add(SGAmod.Instance.TileType("NoviteBarTile")); barTypes.Add(SGAmod.Instance.TileType("BiomassBarTile"));


                for (int thisone22 = -4; thisone22 < 5; thisone22 += 8)
                {
                    WorldGen.PlaceObject((int)DeeperDungeon.LootRooms[choice].vector.X + thisone22, (int)(DeeperDungeon.LootRooms[choice].vector.Y - ((int)DeeperDungeon.LootRooms[choice].size.Y) / 2), TileID.Banners, false, WorldGen.genRand.Next(10));

                    int offset = WorldGen.genRand.Next(-8, 0) + 4;
                    int barType = barTypes[WorldGen.genRand.Next(barTypes.Count)];
                    for (int xx = WorldGen.genRand.Next(-2, 0) - 1; xx < WorldGen.genRand.Next(2) + 2; xx += 1)
                    {
                        for (int yy = 0; yy < (WorldGen.genRand.Next(3) + 2) - Math.Abs(xx); yy += 1)
                        {
                            WorldGen.PlaceObject((int)DeeperDungeon.LootRooms[choice].vector.X + xx + thisone22, (int)(DeeperDungeon.LootRooms[choice].vector.Y + ((int)DeeperDungeon.LootRooms[choice].size.Y) / 2) - yy, barType > 100 ? barType : TileID.MetalBars, false, barType > 100 ? 0 : barType);
                        }
                    }
                }
            }

            prog.Value = 0.75f;
            prog.Message = "Walk Ways";

            AddPlatforms(UniRand, ref allareas);

            prog.Value = 0.90f;
            prog.Message = "Lights, Deco, and Lootin goin' Up!";

            AddLights(UniRand, ref allareas);

            AddPaintnings(UniRand, ref allareas);

            //hardMode = false;
            //postPlantera = false;

            WorldGen._genRandSeed = lastseed;

        }

        public override List<GenPass> tasks { get; }

        public DeeperDungeon()
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
            Main.time = 40000;
        }

        public static void MakeRoomLine(Vector2 locstart, Vector2 locend, Vector2 size, ref List<DungeonTile> passdown, UnifiedRandom UniRand, float stepsize = 10, int gen = 0, int buffersize = 8
            , int filltype = 1, int makenewroomchance = 100, int maxgencheck = 5, int branched = 0, int subtype = 0)
        {
            Vector2 thedist = locend - locstart;
            Vector2 pointer = locstart;
            Vector2 normal = thedist; normal.Normalize();
            float dist = thedist.Length();
            while (dist > 0)
            {
                pointer += normal * stepsize;
                dist -= stepsize;
                DeeperDungeon.MakeRoom(pointer, size, ref passdown, UniRand, gen, buffersize, filltype, makenewroomchance, maxgencheck, subtype);
                DeeperDungeon.pathways.Add(new DungeonPath(pointer, normal, DeeperDungeon.branchoffchance, branched));
            }
            if (filltype != 4 && filltype != 3 && branched > 0)
                DeeperDungeon.pathwayrooms.Add(new DungeonPath(pointer, normal, 1 + (DeeperDungeon.branchoffchance), branched));
            DeeperDungeon.globallineroomindex += 1;

        }


        public static void MakeRoom(Vector2 loc, Vector2 size, ref List<DungeonTile> passdown, UnifiedRandom UniRand, int gen = 0, int buffersize = 8, int filltype = 1, int makenewroomchance = 100, int maxgencheck = 5, int subtype2 = 0)
        {
            for (int x = 0; x < size.X; x += 1)
            {
                for (int y = 0; y < size.Y; y += 1)
                {
                    Vector2 offset = new Vector2(size.X, size.Y) / -2;
                    Vector2 here = new Vector2(loc.X + (x + offset.X), loc.Y + (y + offset.Y));
                    Tile thetile = Framing.GetTileSafely((int)here.X, (int)here.Y);

                    int thewall = DeeperDungeon.DungeonWall;

                    IDGWorldGen.PlaceMulti(here, DeeperDungeon.DungeonTile, buffersize, thewall);

                    DeeperDungeon.instance.PlaceRoomBlock(UniRand);

                    passdown.Add(new DungeonTile(here, filltype, DeeperDungeon.globallineroomindex, y == size.Y - 1, subtype: subtype2));
                    //thetile.active(true);
                    //thetile.type = TileID.Dirt;

                    if (UniRand.Next(0, maxgencheck) > gen && UniRand.Next(0, makenewroomchance) == 1 && filltype != 3)
                    {
                        Vector2 newsize = new Vector2(UniRand.Next(-5, 5), UniRand.Next(-5, 5));
                        Vector2 anewsize = (size + newsize);
                        if (anewsize.X < 2)
                            anewsize.X = 2;
                        if (anewsize.Y < 2)
                            anewsize.Y = 2;

                        DeeperDungeon.MakeRoom(here, anewsize / 1.5f, ref passdown, UniRand, gen + 2, buffersize, filltype, makenewroomchance, maxgencheck, subtype2);
                    }
                }
            }
        }

        public static void AddSpikes(UnifiedRandom UniRand, ref List<DungeonTile> allareas)
        {

            for (int x = 0; x < allareas.Count; x += 1)
            {

                if ((allareas[x].type == 0 || allareas[x].type == 2) && allareas[x].subtype == 1)
                {
                    if (UniRand.Next(0, 300) == 0)
                        WorldGen.placeTrap((int)allareas[x].vector.X, (int)allareas[x].vector.Y, 0);
                }

                if (allareas[x].type < 2 && allareas[x].subtype == 2)
                {
                    if (UniRand.Next(0, 15) == 0)
                        IDGWorldGen.PlaceMulti(allareas[x].vector, -1500, UniRand.Next(2, UniRand.Next(4, 8)), WallID.IceUnsafe);

                    if (UniRand.Next(0, 75) == 0)
                        IDGWorldGen.PlaceMulti(allareas[x].vector, TileID.BreakableIce, UniRand.Next(2, 5), replacetile: false);
                }

                if (allareas[x].type < 2 && allareas[x].subtype == 3)
                {
                    if (UniRand.Next(0, 30) == 0)
                    {
                        IDGWorldGen.PlaceMulti(allareas[x].vector, -1500, UniRand.Next(2, UniRand.Next(3, 6)), WallID.JungleUnsafe);
                    }

                    if (UniRand.Next(0, 30) == 0)
                    {
                        //WorldGen.GrowSpike((int)allareas[x].vector.X, (int)allareas[x].vector.Y, TileID.JungleThorns, TileID.JungleGrass);
                        IDGWorldGen.TileRunner((int)allareas[x].vector.X, (int)allareas[x].vector.Y, 2, 8, TileID.JungleThorns, true, 0, 0, false, true, UniRand);
                        //IDGWorldGen.PlaceMulti(allareas[x].vector, TileID.JungleThorns, UniRand.Next(1, 3), replacetile: false);
                    }
                }

                if (allareas[x].type == 1)
                {
                    if (UniRand.Next(0, allareas[x].subtype == 1 || allareas[x].subtype == 2 ? 20 : 150) == 0)
                    {
                        DeeperDungeon.Spikeclustersizes = new[,] { { 15, 32 }, { 6, 12 } };
                        DeeperDungeon.Spikeclusterchance = new[] { 80, 80, 80, 80 };
                        DeeperDungeon.Spikeclustersizemul = new[] { 0.3f, 0.75f, 0.5f, 0.6f };
                    }
                    if (UniRand.Next(0, 30) == 0)
                    {
                        DeeperDungeon.Spikeclustersizes = new[,] { { 5, 10 }, { 2, 4 } };
                        DeeperDungeon.Spikeclusterchance = new[] { 45, 20, 45, 15 };
                        DeeperDungeon.Spikeclustersizemul = new[] { 0.3f, 0.75f, 0.3f, 0.6f };
                    }

                    Vector2[] where = { new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1) };
                    for (int i = 0; i < 4; i += 1)
                    {
                        if (UniRand.Next(0, DeeperDungeon.Spikeclusterchance[i]) == 1)
                        {
                            Tile thetile = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X), (int)(allareas[x].vector.Y + where[i].Y));
                            if (thetile.active() && !DeeperDungeon.instance.IsSpike(thetile.type, 0))
                            {
                                ushort type = DeeperDungeon.SpikeType;
                                float[] sizer = DeeperDungeon.Spikeclustersizemul;
                                if (allareas[x].subtype == 1 || allareas[x].subtype == 2 || allareas[x].subtype == 3)
                                {
                                    type = TileID.Cobweb;
                                    if (allareas[x].subtype == 2)
                                        type = TileID.BreakableIce;
                                    if (allareas[x].subtype == 3)
                                        type = TileID.JungleThorns;

                                    sizer[0] *= 1.75f;
                                    sizer[1] *= 1.75f;
                                    sizer[2] *= 1.75f;
                                    sizer[3] *= 1.75f;
                                }
                                IDGWorldGen.TileRunner((int)allareas[x].vector.X, (int)allareas[x].vector.Y, (double)(UniRand.Next(DeeperDungeon.Spikeclustersizes[0, 0], DeeperDungeon.Spikeclustersizes[0, 1]) * sizer[i]), (int)(UniRand.Next(DeeperDungeon.Spikeclustersizes[1, 0], DeeperDungeon.Spikeclustersizes[1, 1]) * sizer[i]), type, true, 0f, 0f, false, false, UniRand);
                                //Main.tile[(int)allareas[x].vector.X, (int)allareas[x].vector.Y].type = (int)TileID.Spikes;
                                //Main.tile[(int)allareas[x].vector.X, (int)allareas[x].vector.Y].active(true);
                            }
                        }
                    }
                }
            }

        }

        public static void PlaceLootRooms(UnifiedRandom UniRand, ref List<DungeonTile> allareas)
        {

            //Place Loot Rooms
            for (int i = 0; i < DeeperDungeon.pathwayrooms.Count; i += 1)
            {
                Vector2 there = DeeperDungeon.pathwayrooms[i].vector + (DeeperDungeon.pathwayrooms[i].direction.RotatedBy(MathHelper.ToRadians(UniRand.Next(-90, 90))) * 30);
                Rectangle rect2 = new Rectangle((int)(there.X - 20), (int)(there.Y - 10), 40, 20);
                int tryit = 0;
                int tilecounter = 0;
            goback:
                tilecounter = 0;

                if (tryit > DeeperDungeon.LootRoomTries)
                    return;

                there = DeeperDungeon.pathwayrooms[i].vector + (DeeperDungeon.pathwayrooms[i].direction.RotatedBy(MathHelper.ToRadians(UniRand.Next(-90, 90))) * (20 + (tryit) * 3));
                rect2 = new Rectangle((int)(there.X - 20), (int)(there.Y - 10), 40, 20);
                for (int x = rect2.X; x < rect2.X + rect2.Width; x += 1)
                {
                    for (int y = rect2.Y; y < rect2.Y + rect2.Height; y += 1)
                    {
                        Tile thetile = Framing.GetTileSafely((int)MathHelper.Clamp(x, 0, Main.maxTilesX), (int)MathHelper.Clamp(y, 0, Main.maxTilesY));
                        if (!(DeeperDungeon.instance).IsDirt(thetile.type))
                        {
                            tilecounter += 1;
                            if (tilecounter > DeeperDungeon.LootRoomMaxTiles)
                            {
                                tryit += 1;
                                goto goback;
                            }
                        }

                    }
                }

                Rectangle rect = new Rectangle(80, 80, Main.maxTilesX - 160, Main.maxTilesY - 160);
                if (rect.Intersects(rect2))
                {
                    MakeRoomLine(DeeperDungeon.pathwayrooms[i].vector, there, new Vector2(6, 6), ref allareas, UniRand, filltype: 4, gen: 6, stepsize: 3);


                    Vector2 size = new Vector2(20, 10);
                    DeeperDungeon.LootRooms.Add(new LootRoomLocation(there, size));
                    DeeperDungeon.MakeRoom(there, size, ref allareas, UniRand, 0, 8, filltype: 3);

                }

            }


        }

        public static bool PlatformCheck(int index, UnifiedRandom UniRand, List<DungeonTile> areas)
        {

            if (lastplatformy.Count > 0)
            {
                for (int plat = 0; plat < lastplatformy.Count; plat += 1)
                {
                    if (Math.Abs(areas[index].vector.Y - DeeperDungeon.lastplatformy[plat].Y) < DeeperDungeon.platformmaxdist && areas[index].hallwayindex == DeeperDungeon.lastplatformy[plat].hallwayindex)
                    {
                        return false;
                    }
                }
            }
            return true;

        }

        public static void AddPlatforms(UnifiedRandom UniRand, ref List<DungeonTile> allareas)
        {

            List<DungeonTile> shuffledareas = new List<DungeonTile>();
            List<DungeonTile> shuffledareas1 = new List<DungeonTile>();
            for (int z = 0; z < allareas.Count; z += 1)
            {
                if (allareas[z].type == 2)
                    shuffledareas1.Add(allareas[z]);
            }

            for (int z = 0; z < shuffledareas1.Count; z += 1)
            {
                int index = UniRand.Next(0, shuffledareas1.Count);
                shuffledareas.Add(shuffledareas1[index]);
                shuffledareas1.RemoveAt(index);
            }

            for (int z = 0; z < shuffledareas.Count; z += 1)
            {
                if (UniRand.Next(0, DeeperDungeon.platformchance) == 1)
                {
                    if (PlatformCheck(z, UniRand, shuffledareas))
                    {
                        List<Vector2> there = new List<Vector2>();
                        there.Add(new Vector2(shuffledareas[z].vector.X, shuffledareas[z].vector.Y));
                        bool touchingoneend = false;
                        int expandto = UniRand.Next(0, 2) == 1 ? -1 : 1;
                        int platformtype = DeeperDungeon.DungeonPlatform;
                        for (int zzz = -1; zzz < 2; zzz += 2)
                        {
                            for (int zz = 0; zz < DeeperDungeon.platformmaxsize; zz += 1)
                            {
                                Tile thetile = Framing.GetTileSafely((int)(shuffledareas[z].vector.X + (zz * zzz * expandto)), (int)(shuffledareas[z].vector.Y));
                                if (thetile.active() && !DeeperDungeon.instance.IsSpike(thetile.type, 0))
                                {
                                    DeeperDungeon.instance.PlatformBlockType(thetile, ref platformtype);
                                    touchingoneend = true;
                                    break;
                                }
                                else
                                {
                                    there.Add(new Vector2((int)(shuffledareas[z].vector.X + (zz * zzz * expandto)), (int)(shuffledareas[z].vector.Y)));
                                }
                            }
                        }
                        if (touchingoneend)
                        {
                            lastplatformy.Add(new Platforms((int)shuffledareas[z].vector.Y, shuffledareas[z].hallwayindex));
                            for (int zz = 0; zz < there.Count; zz += 1)
                            {
                                //Main.tile[(int)there[zz].X, (int)there[zz].Y].type = DeeperDungeon.DungeonPlatform;
                                WorldGen.PlaceTile((int)(int)there[zz].X, (int)(int)there[zz].Y, platformtype);
                                //Main.tile[(int)there[zz].X, (int)there[zz].Y].active(true);
                            }
                        }
                    }
                }
            }
            lastplatformy.Clear();

        }
        public static void AddLights(UnifiedRandom UniRand, ref List<DungeonTile> allareas)
        {

            for (int x = 0; x < allareas.Count; x += 1)
            {

                if (allareas[x].type == 2 || allareas[x].type == 1 || allareas[x].type == 0)
                {
                    Vector2[] where = { new Vector2(0, -1), new Vector2(0, 1) };
                    for (int i = 0; i < 2; i += 1)
                    {
                        if (UniRand.Next(0, (int)((float)DeeperDungeon.lightschance * allareas[x].lightchance)) == 0)
                        {
                            Tile thetile = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X), (int)(allareas[x].vector.Y + where[i].Y));
                            Tile thetile2 = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X + 1), (int)(allareas[x].vector.Y + where[i].Y));
                            Tile thetile3 = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X - 1), (int)(allareas[x].vector.Y + where[i].Y));
                            Tile thetile4 = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X), (int)(allareas[x].vector.Y + where[i].Y+1));
                            Tile thetile5 = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X), (int)(allareas[x].vector.Y + where[i].Y+2));
                            if (thetile.active() && thetile2.active() && thetile3.active()
                              && !(DeeperDungeon.instance.IsSpike(thetile.type, 1) || DeeperDungeon.instance.IsSpike(thetile2.type, 1) || DeeperDungeon.instance.IsSpike(thetile3.type, 1) || DeeperDungeon.instance.IsSpike(thetile4.type, 1) || DeeperDungeon.instance.IsSpike(thetile5.type, 1)))
                            {
                                if (i == 0 && allareas[x].type <2)
                                {
                                    int typeofhang = 34;
                                    if (allareas[x].type == 1)
                                        typeofhang = 21;

                                    WorldGen.PlaceObject((int)allareas[x].vector.X, (int)allareas[x].vector.Y, TileID.Chandeliers, false, typeofhang);
                                }
                                if (i == 1)
                                {
                                    if (UniRand.Next(0, allareas[x].type == 2 ? 60 : 30) == 0)//Rare Chance for Chest, rarer on platforms
                                    {
                                        bool locked = UniRand.Next(0, 3) == 0;
                                        int thechest = WorldGen.PlaceChest((int)allareas[x].vector.X, (int)allareas[x].vector.Y, 21, false, locked ? 2 : 1);
                                        DeeperDungeon.AddStuffToChest(thechest, locked ? 1 : 0, UniRand);
                                    }
                                    else
                                    {
                                        int alter = 0;
                                        int randomize = -1;
                                        int styleOfThing = 16;
                                        int typeOfThing = TileID.Candelabras;

                                        if (allareas[x].type == 2)
                                        {
                                            //Water candles!
                                            typeOfThing = TileID.WaterCandle;
                                            styleOfThing = 0;
                                            if (UniRand.Next(100) < 95)
                                                goto dontplaceem;

                                            goto placeem;
                                        }

                                        if (UniRand.Next(100) < 50)
                                        {
                                            typeOfThing = TileID.LargePiles;
                                            styleOfThing = UniRand.Next(6);
                                            if (UniRand.Next(100) < 50)
                                            {
                                                styleOfThing = UniRand.Next(22, 26);
                                            }

                                            if (UniRand.Next(100) < 25)
                                            {
                                                styleOfThing = UniRand.Next(16, 21);
                                            }
                                            //styleOfThing = 16;
                                            //alter = 0;
                                        }

                                    placeem:

                                        WorldGen.PlaceObject((int)allareas[x].vector.X, (int)allareas[x].vector.Y, typeOfThing, false, styleOfThing, alter, randomize);

                                    dontplaceem:
                                        typeOfThing = 999;
                                    }
                                }
                            }
                        }
                    }

                    //Stuff that hangs off walls


                    if (allareas[x].type < 2)
                    {
                        where = new Vector2[] { new Vector2(-1, 0), new Vector2(1, 0) };
                        for (int i = 0; i < 2; i += 1)
                        {
                            if (UniRand.Next(0, (int)((float)DeeperDungeon.lightschance * allareas[x].lightchance)) == 0)
                            {
                                Tile thetile = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X), (int)(allareas[x].vector.Y + where[i].Y));
                                Tile thetile2 = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X + 1), (int)(allareas[x].vector.Y + where[i].Y));
                                Tile thetile3 = Framing.GetTileSafely((int)(allareas[x].vector.X + where[i].X - 1), (int)(allareas[x].vector.Y + where[i].Y));
                                if (thetile.active()
                                  && !DeeperDungeon.instance.IsSpike(thetile.type, 1) && !DeeperDungeon.instance.IsSpike(thetile2.type, 1) && !DeeperDungeon.instance.IsSpike(thetile3.type, 1))
                                {

                                    if ((thetile.type == TileID.BlueDungeonBrick || thetile.type == TileID.GreenDungeonBrick || thetile.type == TileID.PinkDungeonBrick)
                                        && (thetile2.type == TileID.BlueDungeonBrick || thetile2.type == TileID.GreenDungeonBrick || thetile2.type == TileID.PinkDungeonBrick))
                                    {

                                        int typeofhang = 5;
                                        if (thetile.wall == WallID.BlueDungeonUnsafe)
                                            typeofhang = 1;
                                        if (thetile.wall == WallID.GreenDungeonUnsafe)
                                            typeofhang = 3;
                                        if (thetile.wall == WallID.PinkDungeonUnsafe)
                                            typeofhang = 4;
                                        //if (allareas[x].type == 1)
                                        //    typeofhang = 21;

                                        WorldGen.PlaceObject((int)allareas[x].vector.X, (int)allareas[x].vector.Y, TileID.Torches, false, typeofhang);
                                    }
                                }
                            }
                        }
                    }

                }
            }

        }

        public static void AddPaintnings(UnifiedRandom UniRand, ref List<DungeonTile> allareas)
        {
            List<DungeonTile> newtiles2 = allareas;
            List<DungeonTile> newtiles = new List<DungeonTile>();


            /*for (int z = 0; z < newtiles2.Count; z += 1)
            {
                int index = UniRand.Next(0, newtiles2.Count);
                newtiles.Add(newtiles2[index]);
                newtiles2.RemoveAt(index);
            }*/

            for (int x = 0; x < allareas.Count; x += 1)
            {
                if (UniRand.Next(0, (int)(DeeperDungeon.paintingchance)) == 0)
                {

                    //DungeonTile dedung = new DungeonTile(new Vector2(-99999, 0), 0, 0, false);
                    //dedung = newtilescopy.Find(thisdist => (thisdist.vector - newtiles[x].vector).Length() < 300);

                    //if (dedung.vector.X < -8000)
                    //{
                    WorldGen.PlaceObject((int)allareas[x].vector.X, (int)allareas[x].vector.Y, TileID.Painting6X4, false, Main.rand.Next(0, 32));

                    // newtilescopy.Add(newtiles[x]);

                    //}
                }
            }

        }

        public static int[] CommonItems => new int[] { SGAmod.Instance.ItemType("RingOfRespite"), SGAmod.Instance.ItemType("StoneBarrierStaff"), SGAmod.Instance.ItemType("NinjaSash"), SGAmod.Instance.ItemType("DiesIraeStone"), SGAmod.Instance.ItemType("MagusSlippers"), SGAmod.Instance.ItemType("YoyoTricks"), SGAmod.Instance.ItemType("Megido") };
        public static int[] RareItems => new int[] { SGAmod.Instance.ItemType("BenchGodsFavor"), SGAmod.Instance.ItemType("PortalEssence"), SGAmod.Instance.ItemType("DungeonSplunker"), SGAmod.Instance.ItemType("InterdimensionalPartyHat") };
        public static int[] ShadowItems => new int[] {SGAmod.Instance.ItemType("BeserkerAuraStaff"), SGAmod.Instance.ItemType("EnchantedFury"), SGAmod.Instance.ItemType("CardDeckPersona") };
        //public static int[] ShadowItemsVanilla => new int[] { ItemID.DarkLance, ItemID.Sunfury, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.HellwingBow};

    public static void AddStuffToChest(int chestid, int loottype,UnifiedRandom unirand)
        {

            if (chestid > -1)
            {
                List<int> loot = new List<int> { 2344, 2345, 2346, 2347, 2348, 2349, 2350, 2351, 2352, 2353, 2354, 2355, 2356, 2359, 301, 302, 303, 304, 305, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 226, 188, 189, 110, 28 };

                List<int> lootmain = new List<int> { unirand.NextBool() ? SGAmod.Instance.ItemType("UnmanedBar") : SGAmod.Instance.ItemType("NoviteBar"), SGAmod.Instance.ItemType("WraithFragment3"), ItemID.SilverCoin, ItemID.RestorationPotion,ItemID.ManaPotion, ItemID.StrangeBrew,ItemID.Bomb };
                List<int> lootrare = new List<int> { SGAmod.Instance.ItemType("DankCore"), SGAmod.Instance.ItemType("CondenserPotion"),SGAmod.Instance.ItemType("TinkerPotion"), SGAmod.Instance.ItemType("RagnarokBrew"), SGAmod.Instance.ItemType("DankCrate"), SGAmod.Instance.ItemType("Megido"),ItemID.GreaterHealingPotion,ItemID.GoldCoin,ItemID.Dynamite };
                int e = 0;

                if (SGAWorld.downedSpiderQueen)
                    lootmain.Add(SGAmod.Instance.ItemType("VialofAcid"));
                if (SGAWorld.downedMurk > 1)
                    lootmain.Add(SGAmod.Instance.ItemType("MurkyGel"));

                if (loottype < 2)//Not Shadow Chest/Unlocked Gold Chest
                {
                    if (unirand.Next(0, 100) < 10 + (SGAWorld.dungeonlevel * 5))
                    {
                        int[] theitem = CommonItems;
                        int commisionitem = theitem[unirand.Next(0, theitem.Length)];
                        Main.chest[chestid].item[e].SetDefaults(commisionitem);
                        Main.chest[chestid].item[e].stack = (commisionitem == SGAmod.Instance.ItemType("Megido") ? unirand.Next(4,13) : 1);
                        e += 1;
                    }
                    if (unirand.Next(0, 90) < 3 + (SGAWorld.dungeonlevel * 1))
                    {
                        int[] choices = RareItems;
                        Main.chest[chestid].item[e].SetDefaults(choices[unirand.Next(choices.Length)]);
                        Main.chest[chestid].item[e].stack = 1;
                        e += 1;
                    }
                    if (unirand.Next(0, 100) < 10 + (SGAWorld.dungeonlevel * 10))
                    {
                        int index = unirand.Next(0, lootrare.Count);
                        Main.chest[chestid].item[e].SetDefaults(lootrare[index]);
                        Main.chest[chestid].item[e].stack = unirand.Next(1, Main.expertMode ? 8 : 4);
                        e += 1;
                    }
                    for (int kk = 0; kk < 3 + (Main.expertMode ? 1 : 0); kk += 1)
                    {
                        int index = unirand.Next(0, lootmain.Count);
                        Main.chest[chestid].item[e].SetDefaults(lootmain[index]);
                        Main.chest[chestid].item[e].stack = unirand.Next(1, Main.expertMode ? 6 : 4);
                        e += 1;
                    }

                    if (loottype == 1)//Locked GoldChest
                    {
                        lootrare = new List<int> {ItemID.LockBox, ItemID.GoldenKey };// { ItemID.Muramasa, ItemID.MagicMissile, ItemID.CobaltShield, ItemID.AquaScepter, ItemID.BlueMoon, ItemID.Handgun, ItemID.Valor };
                        for (int ii = 0; ii < lootrare.Count; ii += 1)
                        {
                            int index = ii;// unirand.Next(0, lootrare.Count);
                            Main.chest[chestid].item[e].SetDefaults(lootrare[index]);
                            Main.chest[chestid].item[e].stack = 1;
                            e += 1;
                        }
                    }

                }

                if (unirand.Next(0, 100) < 50)
                {
                    Main.chest[chestid].item[e].SetDefaults(unirand.Next(0, 100) <= 25 ? ItemID.StickyBomb : ItemID.Bomb);
                    Main.chest[chestid].item[e].stack = unirand.Next(5, 10);
                    e += 1;
                }

                if (loottype == 2)//Shadow Chest
                {

                    //lootrare = new List<int> { ItemID.DarkLance, ItemID.Sunfury, ItemID.Flamelash, ItemID.FlowerofFire, ItemID.HellwingBow, SGAmod.Instance.ItemType("BeserkerAuraStaff"), SGAmod.Instance.ItemType("EnchantedFury") };

                    int index = unirand.Next(0, ShadowItems.Length);
                    Main.chest[chestid].item[e].SetDefaults(ShadowItems[index]);
                    Main.chest[chestid].item[e].stack = 1;
                    e += 1;

                    Main.chest[chestid].item[e].SetDefaults(ModContent.ItemType<Items.ShadowLockBox>());
                    Main.chest[chestid].item[e].stack = 1;
                    e += 1;

                    lootmain = new List<int> {SGAmod.Instance.ItemType("DragonsMightPotion"),SGAmod.Instance.ItemType("TimePotion"), SGAmod.Instance.ItemType("WraithFragment3"), ItemID.GoldCoin, ItemID.LesserRestorationPotion, ItemID.LifeforcePotion,
                         SGAmod.Instance.ItemType("BiomassBar"), SGAmod.Instance.ItemType("WraithFragment3"), ItemID.Dynamite, ItemID.LesserRestorationPotion, ItemID.LifeforcePotion, ItemID.GoldBar
                        ,ItemID.PlatinumBar,ItemID.PinkGel};

                    for (int kk = 0; kk < 3 + (Main.expertMode ? 6 : 3); kk += 1)
                    {
                        if (unirand.Next(0, 100) < 25 + (SGAWorld.dungeonlevel * 5))
                        {
                            index = unirand.Next(0, lootmain.Count);
                            Main.chest[chestid].item[e].SetDefaults(lootmain[index]);
                            Main.chest[chestid].item[e].stack = unirand.Next(2, unirand.Next(4, Main.expertMode ? 8 : 5));
                            e += 1;
                        }

                    }

                }


                if (chestid > 0)
                    loot.Add(ItemID.LesserRestorationPotion);

                for (int kk = 0; kk < 6 + (Main.expertMode ? 4 : 0); kk += 1)
                {
                    if (unirand.Next(0, 100) < 2 + ((SGAWorld.dungeonlevel+chestid) * 2))
                    {
                        int index = unirand.Next(0, loot.Count);
                        Main.chest[chestid].item[e].SetDefaults(loot[index]);
                        Main.chest[chestid].item[e].stack = unirand.Next(1, Main.expertMode ? 3 : 2);
                        e += 1;

                    }

                }

            }

        }


    }

}
