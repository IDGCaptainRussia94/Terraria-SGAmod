using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
//using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;
using SGAmod.Tiles;
using Idglibrary;
using SGAmod.Items.Accessories;

namespace SGAmod.Generation
{
    public class NormalWorldGeneration
    {
        public static void GenAllCaliburnShrine()
        {
            //PlaceCaiburnShrine(new Vector2(80, 30));
            for (int num = 0; num < SGAWorld.CaliburnAlterCoordsX.Length; num++)
            {
                GenCaliburnShrine(num);
            }

        }
        public static void GenCaliburnShrine(int type)
        {
            bool foundspot = false;
            Vector2 here = new Vector2();
            Tile tstart;


            for (int tries = 0; tries < 50000; tries++)
            {
            startover:
                here = new Vector2(WorldGen.genRand.Next(200, Main.maxTilesX - 200), WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow + 400, Main.maxTilesY - 300));
                tstart = Framing.GetTileSafely(here);// tile1.type=TileID.RainCloud; tile1.active(true);
                int buffersizex = 100;
                int buffersizey = 100;
                int x = (int)here.X;
                int y = (int)here.Y;
                int xbuffer = -buffersizex;
                int ybuffer = -buffersizey;
                bool foundspot2 = true;
                ushort[] stoneTypes = new ushort[] { TileID.BlueDungeonBrick, TileID.GreenDungeonBrick, TileID.PinkDungeonBrick, TileID.LihzahrdBrick, (ushort)ModContent.TileType<MoistStone>() };
                for (xbuffer = -buffersizex; xbuffer < buffersizex; xbuffer++)
                {
                    for (ybuffer = -buffersizey; ybuffer < buffersizey; ybuffer++)
                    {
                        Tile tile = Framing.GetTileSafely((int)here.X + xbuffer, (int)here.Y + ybuffer);
                        if (stoneTypes.Any(iii => iii == tile.type))
                        {
                            foundspot2 = false;
                            goto startover;
                        }


                    }

                }
                if (foundspot2)
                {
                    foundspot = true;
                    break;
                }

            }

            if (foundspot)
                NormalWorldGeneration.PlaceCaiburnShrine(here, type);

        }

        public static void PlaceCaiburnHallway(Vector2 placementspot, int width, int height, int direction, ref List<Vector2> deways, int generation, int tiletype, int walltype)
        {
            Tile tstart = Framing.GetTileSafely(placementspot);

            int buffersizex = width + WorldGen.genRand.Next(3);
            int buffersizey = height + WorldGen.genRand.Next(3);
            int x = (int)placementspot.X;
            int y = (int)placementspot.Y;
            int xbuffer = -buffersizex;
            int ybuffer = -buffersizey;
            //ushort[] stoneTypes = new ushort[] { TileID.Stone, TileID.Dirt, TileID.Mud, TileID.ClayBlock };
            for (xbuffer = -buffersizex; xbuffer < buffersizex; xbuffer++)
            {
                for (ybuffer = -buffersizey; ybuffer < buffersizey; ybuffer++)
                {
                    Tile tile = Framing.GetTileSafely((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer);
                    IDGWorldGen.PlaceMulti(placementspot + new Vector2(xbuffer, ybuffer), tiletype, 4, walltype);
                    deways.Add(new Vector2((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer));
                    if (generation > 1000)
                    {
                        if (xbuffer < 2 && xbuffer > -2)
                        {
                            deways.Add(new Vector2((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer - 1));
                            deways.Add(new Vector2((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer - 2));
                            deways.Add(new Vector2((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer - 3));
                        }



                    }
                }

            }

            for (int there = 0; there < 4; there++)
            {
                if (there != direction && WorldGen.genRand.Next(0, direction == 3 ? 50 : 100) > generation)
                {
                    Vector2 edge = new Vector2(width, 0);
                    int width2 = 8 + WorldGen.genRand.Next(7);
                    int height2 = 4 + WorldGen.genRand.Next(3);
                    if (direction == 1)
                    {
                        width2 = 3 + WorldGen.genRand.Next(1);
                        height2 = 5 + WorldGen.genRand.Next(12);
                        edge = new Vector2(0, -height);
                        edge += new Vector2(0, -height2);
                    }
                    else if (direction == 2)
                    {
                        edge = new Vector2(-width, 0);
                        edge += new Vector2(-width2, 0);
                    }
                    else if (direction == 3)
                    {
                        width2 = 3 + WorldGen.genRand.Next(1);
                        height2 = 4 + WorldGen.genRand.Next(12);
                        edge = new Vector2(0, height);
                        edge += new Vector2(0, height2);
                    }
                    else
                    {
                        edge += new Vector2(width2, 0);
                    }

                    Tile tile = Framing.GetTileSafely((int)(placementspot + edge * 2f).X, (int)(placementspot + edge * 2f).Y);

                    if (tile.type != TileID.GoldBrick && tile.type != TileID.SilverBrick && tile.type != TileID.CopperBrick && tile.type != SGAmod.Instance.TileType("MoistStone")
                        && tile.type != TileID.LihzahrdBrick && tile.type != TileID.BlueDungeonBrick && tile.type != TileID.GreenDungeonBrick && tile.type != TileID.PinkDungeonBrick)
                        PlaceCaiburnHallway(placementspot + edge, width2, height2, there, ref deways, generation + 2, tiletype, walltype);

                }
                else
                {
                    if (generation < 1000 && generation > 35)
                    {
                        //tiletype = TileID.CopperBrick;
                        //walltype = WallID.CopperBrick;
                        PlaceCaiburnHallway(placementspot + new Vector2(0, -10), 5, 10, 1, ref deways, 1500, tiletype, walltype);
                        break;
                    }

                }

            }


        }

        public static void PlaceCaiburnShrine(Vector2 placementspot, int type)
        {
            Tile tstart = Framing.GetTileSafely(placementspot);
            int heighestTile = Main.maxTilesY;

            List<Vector2> deways = new List<Vector2>();
            List<Vector2> dewaysMainroom = new List<Vector2>();

            int buffersizex = 12 + WorldGen.genRand.Next(7);
            int buffersizey = 8 + WorldGen.genRand.Next(3);
            int x = (int)placementspot.X;
            int y = (int)placementspot.Y;
            int xbuffer = -buffersizex;
            int ybuffer = -buffersizey;
            //ushort[] stoneTypes = new ushort[] { TileID.Stone, TileID.Dirt, TileID.Mud, TileID.ClayBlock };
            for (xbuffer = -buffersizex; xbuffer < buffersizex; xbuffer++)
            {
                for (ybuffer = -buffersizey; ybuffer < buffersizey; ybuffer++)
                {
                    Tile tile = Framing.GetTileSafely((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer);
                    IDGWorldGen.PlaceMulti(placementspot + new Vector2(xbuffer, ybuffer), SGAmod.Instance.TileType("MoistStone"), 4, SGAmod.Instance.WallType("SwampWall"));
                    dewaysMainroom.Add(new Vector2((int)placementspot.X + (int)xbuffer, (int)placementspot.Y + (int)ybuffer));

                }

            }

            int t1 = SGAmod.Instance.TileType("MoistStone");
            int t2 = SGAmod.Instance.WallType("SwampWall");

            PlaceCaiburnHallway(placementspot + new Vector2(buffersizex * 1, 0), 12, 6, 0, ref deways, 0, t1, t2);
            PlaceCaiburnHallway(placementspot + new Vector2(-buffersizex * 1, 0), 12, 6, 2, ref deways, 0, t1, t2);

            for (int aaa = 0; aaa < deways.Count; aaa++)
            {
                if ((int)deways[aaa].Y < heighestTile)
                    heighestTile = (int)deways[aaa].Y;

                Tile tile = Framing.GetTileSafely((int)deways[aaa].X, (int)deways[aaa].Y);
                tile.active(false);
            }

            HashSet<Point16> removes = new HashSet<Point16>();
            foreach (Vector2 point in deways.Where(testby => testby.Y < heighestTile + 3))
            {
                Point16 point2 = point.ToPoint16();
                if (WorldGen.InWorld(point2.X - 4, point2.Y-2) && WorldGen.InWorld(point2.X + 4, point2.Y-2))
                {
                    if (Main.tile[point2.X - 4, point2.Y-2].type == ModContent.TileType<MoistStone>() && Main.tile[point2.X + 4, point2.Y-2].type == ModContent.TileType<MoistStone>() &&
                        Main.tile[point2.X - 4, point2.Y].active() && Main.tile[point2.X - 4, point2.Y].type == ModContent.TileType<MoistStone>() && Main.tile[point2.X + 4, point2.Y].active() && Main.tile[point2.X + 4, point2.Y].type == ModContent.TileType<MoistStone>())
                    {
                        removes.Add(new Point16(point2.X, point2.Y - 1));
                        removes.Add(new Point16(point2.X, point2.Y-2));
                        removes.Add(new Point16(point2.X, point2.Y - 3));
                    }
                }
            }

            foreach (Point16 point2 in removes)
            {
                Tile tile = Framing.GetTileSafely(point2.X, point2.Y);
                tile.type = (ushort)ModContent.TileType<Biomass>();
                tile.active(false);
            }

            for (int aaa = 0; aaa < deways.Count; aaa++)
            {
                if (WorldGen.genRand.Next(0, 100) < 5)
                {
                    Tile tile = Framing.GetTileSafely((int)deways[aaa].X, (int)deways[aaa].Y - 1);
                    if (tile.active())
                        WorldGen.PlaceObject((int)deways[aaa].X, (int)deways[aaa].Y, TileID.HangingLanterns, false, 16);

                }
                if (WorldGen.genRand.Next(0, 100) < 5)
                {
                    Tile tile = Framing.GetTileSafely((int)deways[aaa].X, (int)deways[aaa].Y + 1);
                    if (tile.active())
                        WorldGen.placeTrap((int)deways[aaa].X, (int)deways[aaa].Y, 0);

                }
                if (WorldGen.genRand.Next(0, 100) < 2)
                {
                    Tile tile1 = Framing.GetTileSafely((int)deways[aaa].X, (int)deways[aaa].Y + 1);
                    Tile tile2 = Framing.GetTileSafely((int)deways[aaa].X + 1, (int)deways[aaa].Y + 1);
                    Tile tile3 = Framing.GetTileSafely((int)deways[aaa].X + 1, (int)deways[aaa].Y);
                    Tile tile4 = Framing.GetTileSafely((int)deways[aaa].X, (int)deways[aaa].Y);

                    Vector2 findone = Vector2.Zero;

                    findone = dewaysMainroom.Find(location => new Rectangle((int)location.X - 1, (int)location.Y - 1, 3, 3).Intersects(new Rectangle((int)deways[aaa].X - 1, (int)deways[aaa].Y - 1, 3, 3)));

                    if (tile1.active() && tile2.active() && !tile3.active() && !tile4.active() && findone == Vector2.Zero)
                    {

                        int thechest = WorldGen.PlaceChest((int)deways[aaa].X, (int)deways[aaa].Y, 21, false, 12);

                        if (thechest > 0)
                        {
                            List<int> loot = new List<int> { 2344, 2345, 2346, 2347, 2348, 2349, 2350, 2351, 2352, 2353, 2354, 2355, 2356, 2359, 301, 302, 303, 304, 305, 288, 289, 290, 291, 292, 293, 294, 295, 296, 297, 298, 299, 300, 226, 188, 189, 110, 28 };

                            List<int> lootmain = new List<int> { SGAWorld.WorldIsNovus ? SGAmod.Instance.ItemType("UnmanedOre") : SGAmod.Instance.ItemType("NoviteOre"), SGAmod.Instance.ItemType("DankWood"), SGAmod.Instance.ItemType("DankWood"), SGAmod.Instance.ItemType("Biomass"), SGAmod.Instance.ItemType("DankWood"), ItemID.SilverCoin, ItemID.LesserManaPotion };
                            List<int> lootrare = new List<int> { SGAmod.Instance.ItemType("DankCore"), SGAmod.Instance.ItemType("DankCore") };
                            List<int> dankrare = new List<int> { SGAmod.Instance.ItemType("DankWoodShield"), SGAmod.Instance.ItemType("MurkyCharm") };
                           int e = 0;

                            for (int kk = 0; kk < 2 + (Main.expertMode ? 1 : 0); kk += 1)
                            {
                                //for (int i = 0; i < WorldGen.genRand.Next(15, Main.expertMode ? 25 : 30); i += 1)
                                //{
                                int index = WorldGen.genRand.Next(0, loot.Count);
                                Main.chest[thechest].item[e].SetDefaults(loot[index]);
                                Main.chest[thechest].item[e].stack = WorldGen.genRand.Next(1, Main.expertMode ? 3 : 2);
                                //}
                                e += 1;
                            }
                            if (WorldGen.genRand.Next(0, 100) < 25)
                            {
                                int index = WorldGen.genRand.Next(0, lootrare.Count);
                                Main.chest[thechest].item[e].SetDefaults(lootrare[index]);
                                Main.chest[thechest].item[e].stack = WorldGen.genRand.Next(1, Main.expertMode ? 3 : 1);
                                //}
                                e += 1;
                            }
                            if (WorldGen.genRand.Next(0, 20) == 0)
                            {
                                int index = WorldGen.genRand.Next(0, dankrare.Count);
                                Main.chest[thechest].item[e].SetDefaults(dankrare[index]);
                                Main.chest[thechest].item[e].stack = 1;
                                //}
                                e += 1;
                            }                            
                            for (int kk = 0; kk < 3 + (Main.expertMode ? 1 : 0); kk += 1)
                            {
                                //for (int i = 0; i < WorldGen.genRand.Next(15, Main.expertMode ? 25 : 30); i += 1)
                                //{
                                int index = WorldGen.genRand.Next(0, lootmain.Count);
                                Main.chest[thechest].item[e].SetDefaults(lootmain[index]);
                                Main.chest[thechest].item[e].stack = WorldGen.genRand.Next(15, Main.expertMode ? 25 : 45);

                                //}
                                e += 1;
                            }

                        }

                        ///WorldGen.PlaceChestDirect((int)deways[aaa].X, (int)deways[aaa].Y, (ushort)SGAmod.Instance.TileType("OvergrownChest"), 0, 0);
                    }



                }
            }

            for (int aaa = 0; aaa < dewaysMainroom.Count; aaa++)
            {
                Tile tile = Framing.GetTileSafely((int)dewaysMainroom[aaa].X, (int)dewaysMainroom[aaa].Y);
                //Chest.DestroyChest((int)deways[aaa].X, (int)deways[aaa].Y);
                WorldGen.KillTile((int)deways[aaa].X, (int)deways[aaa].Y);
                tile.active(false);
            }

            for (int kk = 3; kk < 6; kk += 1)
            {
                Main.tile[(int)placementspot.X, (int)placementspot.Y + buffersizey - kk].active(false);
                for (int xx = 0; xx < 6; xx += 1)
                {
                    Main.tile[(int)placementspot.X - xx, (int)placementspot.Y + buffersizey - kk].active(false);
                    Main.tile[(int)placementspot.X + xx, (int)placementspot.Y + buffersizey - kk].active(false);
                }
            }
            Main.tile[(int)placementspot.X, (int)placementspot.Y + buffersizey - 1].active(true);
            for (int xx = 0; xx < 4; xx += 1)
            {
                Main.tile[(int)placementspot.X - xx, (int)placementspot.Y + buffersizey - 1].active(true);
                Main.tile[(int)placementspot.X + xx, (int)placementspot.Y + buffersizey - 1].active(true);
            }

            Main.tile[(int)placementspot.X - 2, (int)placementspot.Y + buffersizey - 2].active(true);
            Main.tile[(int)placementspot.X - 1, (int)placementspot.Y + buffersizey - 2].active(true);
            Main.tile[(int)placementspot.X, (int)placementspot.Y + buffersizey - 2].active(true);
            Main.tile[(int)placementspot.X + 1, (int)placementspot.Y + buffersizey - 2].active(true);
            Main.tile[(int)placementspot.X + 2, (int)placementspot.Y + buffersizey - 2].active(true);

            Point offset = new Point(0, -3);
            int altertype = type == 0 ? SGAmod.Instance.TileType("CaliburnAltar") : (type == 1 ? SGAmod.Instance.TileType("CaliburnAltarB") : SGAmod.Instance.TileType("CaliburnAltarC"));
            WorldGen.PlaceObject((int)placementspot.X + offset.X, (int)placementspot.Y + buffersizey + offset.Y, altertype, false, 0);
            SGAWorld.CaliburnAlterCoordsX[type] = (int)placementspot.X * 16;
            SGAWorld.CaliburnAlterCoordsY[type] = (int)placementspot.Y * 16;


        }




        public static void TempleChambers()
        {

            int[] templecord = { 1000000, 1000000, -1000000, -1000000 };
            bool firstone = false;

            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    Tile tile = Framing.GetTileSafely(x, y);
                    if (Main.tile[x, y].type == TileID.LihzahrdBrick)
                    {
                        templecord[0] = Math.Min(templecord[0], x);
                        templecord[1] = Math.Min(templecord[1], y);
                        templecord[2] = Math.Max(templecord[2], x);
                        templecord[3] = Math.Max(templecord[3], y);

                    }
                }
            }

            Tile tile1 = Framing.GetTileSafely(templecord[0], templecord[1]);// tile1.type=TileID.RainCloud; tile1.active(true);
            Tile tile2 = Framing.GetTileSafely(templecord[2], templecord[1]);// tile2.type=TileID.RainCloud; tile2.active(true);
            Tile tile3 = Framing.GetTileSafely(templecord[0], templecord[3]);// tile3.type=TileID.RainCloud; tile3.active(true);
            Tile tile4 = Framing.GetTileSafely(templecord[2], templecord[3]);// tile4.type=TileID.RainCloud; tile4.active(true);


            for (int rooms = 0; rooms < 3 + WorldGen.genRand.Next(4); rooms++)
            {
                bool thisplacegood = false;
                int buffersizex = 6;
                int buffersizey = 4;
                int[] theplace = { 0, 0 };
                for (int tries = 0; tries < 500; tries++)
                {
                    buffersizex = 10 + WorldGen.genRand.Next(7);
                    buffersizey = 8 + WorldGen.genRand.Next(3);
                    thisplacegood = true;
                    int x = templecord[0] + WorldGen.genRand.Next(templecord[2] - templecord[0]);
                    int y = templecord[1] + WorldGen.genRand.Next(templecord[3] - templecord[1]);
                    int xbuffer = -buffersizex;
                    int ybuffer = -buffersizey;
                    for (xbuffer = -buffersizex; xbuffer < buffersizex; xbuffer++)
                    {
                        for (ybuffer = -buffersizey; ybuffer < buffersizey; ybuffer++)
                        {
                            if (Main.tile[x + xbuffer, y + ybuffer].type != TileID.LihzahrdBrick || !Main.tile[x + xbuffer, y + ybuffer].active())
                            {
                                thisplacegood = false;
                                break;
                            }
                        }
                    }

                    if (thisplacegood == true)
                    {
                        theplace[0] = x; theplace[1] = y;
                        break;
                    }
                }




                if (thisplacegood == true)
                {
                    for (int xfiller = -buffersizex + 3; xfiller < buffersizex - 3; xfiller++)
                    {
                        for (int yfiller = -buffersizey + 3; yfiller < buffersizey - 3; yfiller++)
                        {
                            Tile tileroomout = Framing.GetTileSafely(theplace[0] + xfiller, theplace[1] + yfiller); tileroomout.active(false);

                        }
                    }
                    if (firstone == false)
                    {
                        WorldGen.PlaceObject(theplace[0], theplace[1] + buffersizey - 4, SGAmod.Instance.TileType("PrismalStation"), true, 0);
                        firstone = true;
                    }
                    else
                    {
                        int thechest = WorldGen.PlaceChest(theplace[0], theplace[1] + buffersizey - 4, 21, false, 16);
                        if (thechest > 0)
                        {
                            List<int> lootmain = new List<int> { ItemID.SuperManaPotion, ItemID.SuperHealingPotion, ItemID.HellstoneBar, ItemID.GoldCoin };
                            List<int> lootextrabonus = new List<int> { ItemID.Arkhalis, ItemID.LizardEgg, ItemID.PlatinumCoin, ItemID.ReindeerBells, ItemID.SuperAbsorbantSponge, ItemID.BottomlessBucket, ItemID.TheAxe };
                            int e = 0;
                            for (int kk = 0; kk < 2 + (Main.expertMode ? 1 : 0); kk += 1)
                            {
                                //for (int i = 0; i < WorldGen.genRand.Next(15, Main.expertMode ? 25 : 30); i += 1)
                                //{
                                int index = WorldGen.genRand.Next(0, lootmain.Count);
                                Main.chest[thechest].item[e].SetDefaults(lootmain[index]);
                                Main.chest[thechest].item[e].stack = WorldGen.genRand.Next(15, Main.expertMode ? 25 : 30);

                                //}
                                e += 1;
                            }
                            int index2 = WorldGen.genRand.Next(0, lootextrabonus.Count);
                            Main.chest[thechest].item[e].SetDefaults(lootextrabonus[index2]);

                            //Tile tiletest = Framing.GetTileSafely(theplace[0],theplace[1]); tiletest.type=TileID.RainCloud; tiletest.active(true);
                        }

                    }

                }

            }














        }



    }
}