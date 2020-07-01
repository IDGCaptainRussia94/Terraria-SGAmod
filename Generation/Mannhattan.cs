using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;

namespace SGAmod.Generation
{
    public class Mannhattan
    {

        private static void RemoveGeneration(string type,List<GenPass> tasks){

            int jungleGen = tasks.FindIndex(genpass => genpass.Name.Equals(type));
            if (jungleGen!=null){
tasks[jungleGen] = new PassLegacy(type+" (removed) ", delegate (GenerationProgress progress)
{
    //nothing to see here
   WorldGen.TileRunner(Main.spawnTileX, Main.spawnTileY + 12, 6, Main.rand.Next(1, 3), TileID.Dirt, true, 0f, 0f, true, true);
});
}

}

public static void GenMannhattan(List<GenPass> tasks){
RemoveGeneration("Mount Caves",tasks);
RemoveGeneration("Sand",tasks);
RemoveGeneration("Dirt Wall Backgrounds",tasks);
RemoveGeneration("Floating Islands",tasks);
RemoveGeneration("Grass",tasks);
RemoveGeneration("Jungle",tasks);
RemoveGeneration("Marble",tasks);
RemoveGeneration("Granite",tasks);
RemoveGeneration("Full Desert",tasks);
RemoveGeneration("Underworld",tasks);
RemoveGeneration("Jungle Temple",tasks);
RemoveGeneration("Hives",tasks);
RemoveGeneration("Grass Wall",tasks);
RemoveGeneration("Guide",tasks);
RemoveGeneration("Webs And Honey",tasks);
RemoveGeneration("Vines",tasks);
RemoveGeneration("Flowers",tasks);

RemoveGeneration("Jungle Chests",tasks);
RemoveGeneration("Hellforge",tasks);
RemoveGeneration("Jungle Plants",tasks);
RemoveGeneration("Spreading Grass",tasks);
RemoveGeneration("Planting Trees",tasks);
RemoveGeneration("Moss",tasks);
RemoveGeneration("Piles",tasks);
RemoveGeneration("Spider Caves",tasks);
RemoveGeneration("Ice Walls",tasks);
RemoveGeneration("Temple",tasks);


            int LivingTreesIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Micro Biomes"));
           // if (LivingTreesIndex != -1)
            //{
                //tasks.Insert(LivingTreesIndex + 1
                    int inter=2;
                    //while(tasks[inter]!=null){
                    //tasks.RemoveAt(inter);
                    //inter=inter+1;
                //}
                tasks.Insert(LivingTreesIndex+1, new PassLegacy("Post Terrain", delegate (GenerationProgress progress)
                {
                    inter=inter+1;
                    progress.Message = "Testing World Generation";
                    NewWorldGeneration(0);
                }));
                tasks.Insert(LivingTreesIndex+2, new PassLegacy("Post Terrain2", delegate (GenerationProgress progress)
                {
                    inter=inter+1;
                    progress.Message = "Testing World Generation";
                    NewWorldGeneration(1);
                }));
        }


public static void NewWorldGeneration(int phase){

if (phase==0){

int smoothitout=WorldGen.genRand.Next(180, 200);
int smoothitoutcounter=0;
            for (int x = 0; x < Main.maxTilesX; x++)
            {
            int y=0;
            int z=0;
for (z=0; z < smoothitout; z++){ 
Tile tile = Framing.GetTileSafely(x,z);
tile.active(false);
WorldGen.KillWall(x,z);

}
Tile tile2 = Framing.GetTileSafely(x,z);
tile2.type=TileID.PlatinumBrick;
tile2.active(true);

smoothitoutcounter-=1;
if (smoothitoutcounter<1){
smoothitoutcounter=WorldGen.genRand.Next(10, 30);
smoothitout=smoothitout+(3-WorldGen.genRand.Next(0, 6));
}
}

            for (int x = 0; x < Main.maxTilesX; x++)
            {
            int y=0;
            int safetynet=Main.maxTilesY+7;
            while (!Main.tile[x,y].active() && safetynet>0){y=y+1; safetynet=safetynet-1;}

    for (int z=0; z < WorldGen.genRand.Next(20, 25); z++){ 
safetynet=safetynet-1;
Tile tile = Framing.GetTileSafely(x,y+1);
tile.type=TileID.GoldBrick;
tile.active(true);
y=y+1;
}
}

}

if (phase==1){
            float widthScale = (Main.maxTilesX / 4200f);
            int numberToGenerate = 2;//WorldGen.genRand.Next(1, (int)(2f * widthScale));
            for (int k = 0; k < numberToGenerate; k++)
            {
                int x=WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                int y=0;
                bool success = false;
                int attempts = 0;
                while (!success)
                {
                    x=WorldGen.genRand.Next(200, Main.maxTilesX - 200);
                    attempts++;
                    if (attempts > 1000)
                    {
                      success=true;  
                    continue;
                    }

                    while(!Main.tile[x,y].active()){y=y+1;}


                    if (Main.tile[x,y].active()){
                        success=true;
                    for (int width = -120; width < 121; width++)
                    {
                    for (int height = -120; height < 121; height++)
                    {
                        if (WorldGen.InWorld(x+width,y+height,5)){
                    Tile tile = Framing.GetTileSafely(x+width,y+height);
                    tile.type=TileID.PlatinumBrick;
                    tile.active(true);
                    }}}}



                }
            }

}
















}



	}
}