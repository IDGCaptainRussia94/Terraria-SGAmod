using System;
using System.Threading;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.Collections.Generic;
using Terraria.ModLoader;
using System.IO;
using Terraria.GameContent.Events;
using System.Linq;
using Terraria.ModLoader.Engine;
using Terraria.DataStructures;
using System.Threading.Tasks;
using Terraria.Utilities;
using SGAmod.Dimensions;

namespace SGAmod.Items.Consumable
{

	public class PathNode
	{

		public PathNode previousLocation;
		public int distanceFromStart;
		public int distanceToEnd;
		public Point16 location;
		public PathNode(Point16 loc,int distanceFromStart,int distanceToEnd, PathNode prev = default)
		{
			previousLocation = prev;

			this.distanceFromStart = distanceFromStart;
			this.distanceToEnd = distanceToEnd;
			location = loc;
		}
	}
	enum PathState
    {
		Ready,
		Calculating,
		Finished,
		Failed
    }
	public class AStarPathFinder
    {
		public static bool Debug => true;
		public static Point16[] RoseCompass = { new Point16(1, 0), new Point16(1, -1), new Point16(0, -1), new Point16(-1, -1), new Point16(-1, 0), new Point16(-1, 1), new Point16(0, 1), new Point16(1, 1) };
		public static int[] RoosCompassDist = { 10, 14 };

		public Point16 startingPosition = new Point16(0, 0);

		public List<PathNode> Path = new List<PathNode>();
		public int recursionLimit = 10000;
		public int seed = -1;
		public int wallsWeight = 0;
		public int state = (int)PathState.Ready;
		static public int Heuristic(Point16 a, Point16 b)
		{
			return (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y)) * 10;
		}
		public void AStarTiles(Point16 EndPoint, int stepSize)
		{
			Path.Clear();

			Task.Run(delegate ()
			{
				state = (int)PathState.Calculating;
				int seed2 = seed;
				if (seed == -1)
					seed2 = (int)Main.GlobalTime * 7894;

				UnifiedRandom uniRand = new UnifiedRandom(seed2);
				EndPoint = (EndPoint.ToVector2() / stepSize).ToPoint16() * new Point16(stepSize);
				Point16 StartPoint = (startingPosition.ToVector2() / stepSize).ToPoint16() * new Point16(stepSize);

				if (Main.tile[StartPoint.X, StartPoint.Y].active() || Main.tile[EndPoint.X, EndPoint.Y].active())
					return;

				List<PathNode> openCells = new List<PathNode>();
				List<PathNode> closedCells = new List<PathNode>();
				int CurrentCost = 0;

				openCells.Add(new PathNode(StartPoint, CurrentCost, Heuristic(StartPoint, EndPoint)));

				int RecursionCount = 0;

			Recursion:
				//return;
				//Magic priority sorter!
				openCells = openCells.OrderBy(order => ((order.distanceFromStart + order.distanceToEnd) * 100) + uniRand.Next(8)).ToList();

				PathNode checkCell = openCells[0];
				openCells.RemoveAt(0);
				closedCells.Insert(0, checkCell);

				if (AStarPathFinder.Debug)
				{
					int dust = Dust.NewDust(checkCell.location.ToVector2() * 16, 0, 0, DustID.PurpleCrystalShard);
					Main.dust[dust].scale = 2f;
					Main.dust[dust].velocity = Vector2.Zero;
					Main.dust[dust].noGravity = true;
				}

				//End Reached
				if (checkCell.location == EndPoint || RecursionCount > recursionLimit)
				{
					if (RecursionCount > recursionLimit)
						state = (int)PathState.Failed;

					goto EndHere;
				}


				//Open cells around the current cell
				for (int i = 0; i < RoseCompass.Length; i += 1)
				{
					int dist = RoosCompassDist[i % 2] * stepSize;
					Point16 pointcheck = new Point16(stepSize, stepSize) * RoseCompass[i];
					Point16 newPoint = checkCell.location + pointcheck;

					Tile tile = Main.tile[newPoint.X, newPoint.Y];
					bool solidWall = tile.active() && Main.tileSolid[tile.type];
					int extraCost = solidWall ? wallsWeight : 0;


					if (!solidWall || wallsWeight > 0)
					{
						int endDist = Heuristic(newPoint, EndPoint);//(int)(EndPoint - newPoint).ToVector2().Length() * 10;
																	//int startDist = (int)(StartPoint - newPoint).ToVector2().Length() * 10;
						int startDist = checkCell.distanceFromStart + dist + extraCost;

						PathNode thisOne = closedCells.FirstOrDefault(test => test.location == newPoint);
						if (thisOne == default)
						{
							if (openCells.FirstOrDefault(test => test.location == newPoint) == default)
							{
								PathNode NewCell = new PathNode(newPoint, startDist, endDist, checkCell);
								openCells.Add(NewCell);

								/*dust = Dust.NewDust(NewCell.location.ToVector2() * 16, 0, 0, DustID.SparksMech);
								Main.dust[dust].scale = 5f;
								Main.dust[dust].velocity = Vector2.Zero;
								Main.dust[dust].noGravity = true;*/
							}
						}
						else
						{
							if (thisOne.distanceFromStart + thisOne.distanceToEnd > checkCell.distanceFromStart + checkCell.distanceToEnd)
							{
								closedCells.RemoveAt(0);
								checkCell.distanceFromStart = thisOne.distanceFromStart;
								checkCell.previousLocation = thisOne;
								closedCells.Insert(0, checkCell);

								/*dust = Dust.NewDust(checkCell.location.ToVector2() * 16, 0, 0, DustID.Blood);
								Main.dust[dust].scale = 1f;
								Main.dust[dust].velocity = Vector2.Zero;
								Main.dust[dust].noGravity = true;*/
							}

						}

					}
				}
				RecursionCount += 1;

				goto Recursion;

			EndHere:

				//Draw a line
				int testx = 0;
				List<PathNode> finalPoints = new List<PathNode>();
				while (checkCell.previousLocation != default)
				{
					finalPoints.Add(checkCell);
					checkCell = checkCell.previousLocation;
				}

				if (state!=(int)PathState.Failed)
				state = (int)PathState.Finished;

				Path = new List<PathNode>(finalPoints);

				if (AStarPathFinder.Debug)
				{
					for (int i = 0; i < 500; i += 1)
					{
						foreach (PathNode node in finalPoints)
						{
							if (Main.LocalPlayer.DistanceSQ(node.location.ToVector2() * 16) < 1000 * 1000)
							{
								int dustx = Dust.NewDust(node.location.ToVector2() * 16, 0, 0, DustID.PurpleCrystalShard);
								Main.dust[dustx].scale = 2f;
								Main.dust[dustx].velocity = Vector2.Zero;
								Main.dust[dustx].noGravity = true;
							}
						}

						//Thread.Sleep(50);
					}
				}

			});

		}

	}

	public class TartarusThread : ModItem
	{
		AStarPathFinder finder = new AStarPathFinder();
		public void GraphFinder(MazeRoom EndPoint, Point16 StartPoint)
		{
			Task.Run(delegate ()
			{

				MazeRoom currentPoint = Limborinth.MazeGraphPoints.OrderBy(testthem => (testthem.loc - StartPoint).ToVector2().LengthSquared()).ToArray()[0];

                List<Point16> finalPoints = new List<Point16>
                {
                    currentPoint.loc
                };

                while (currentPoint.ConnectedRoom != default)
				{
					finalPoints.Add(currentPoint.loc);
					currentPoint = currentPoint.ConnectedRoom;

				}

				Main.NewText(finalPoints.Count);

				Vector2 previousVector=Vector2.Zero;
				Point16 lastPoint = finalPoints[0];
				for (int i = 0; i < 200; i += 1)
				{
					foreach (Point16 point in finalPoints)
					{
						Vector2 lastpointasVector = lastPoint.ToVector2();
						previousVector = Vector2.Normalize(point.ToVector2() - lastpointasVector);
						for (float f = 0; f < 1f;f += 0.05f) 
						{
							Vector2 there = Vector2.Lerp(point.ToVector2() * 16, lastpointasVector*16f,f);
							if (Main.LocalPlayer.DistanceSQ(there) < 1000 * 1000)
							{
								int dustx = Dust.NewDust(there, 0, 0, DustID.PurpleCrystalShard);
								Main.dust[dustx].scale = 2f;
								Main.dust[dustx].velocity = previousVector * 0f;
								Main.dust[dustx].noGravity = true;
							}
						}
						lastPoint = point;
					}

					Thread.Sleep(50);

				}

			});

		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tartarus Thread");
			Tooltip.SetDefault("A* debug item\nHold Shift to set the starting point");

		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 14;
			item.maxStack = 30;
			item.rare = 8;
			item.value = 1000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item9;
			item.consumable = true;
		}

		public override bool UseItem(Player player)
		{

			if (SGAPocketDim.WhereAmI == typeof(Limborinth))
            {
				GraphFinder(Limborinth.MazeGraphPoints[0], (player.Center / 16).ToPoint16());
				return true;
            }

			Point16 pos = new Point16((int)player.Center.X >> 4, (int)player.Center.Y >> 4);
			if (finder.startingPosition.X == 0 || Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftShift)) 
			{
				finder.startingPosition = pos;
				Main.NewText("Tile Position Set! " + finder.startingPosition.X + " | " + finder.startingPosition.Y);
				return true;
			}
			finder.wallsWeight = 250;

			finder.AStarTiles(pos,1);

			return true;

		}
		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.BlackThread; }
		}

	}

}