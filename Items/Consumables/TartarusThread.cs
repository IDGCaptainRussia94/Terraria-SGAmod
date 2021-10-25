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

namespace SGAmod.Items.Consumables
{



	public class TartarusThread : ModItem
	{
		AStarPathFinder finder = new AStarPathFinder();
		//This finds the path back through the maze in Seed of Evil via backtracking from the closest node (isn't perfect, works fine enough thou)
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