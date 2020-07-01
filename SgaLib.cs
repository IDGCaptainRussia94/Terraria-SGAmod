using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.World.Generation;


namespace SGAmod
{
    public class SgaLib
    {


//this was copied & edited from the source code for Cholo bullets, sue me
    public static Player FindClosestPlayer(Vector2 loc,Vector2 size)
    {
                    int num;
                    float num170=1000000;
                    int num171=0;
                     for (int num172 = 0; num172 < 200; num172 = num + 1)
                    {
                            float num173 = Main.player[num172].position.X + (float)(Main.player[num172].width / 2);
                            float num174 = Main.player[num172].position.Y + (float)(Main.player[num172].height / 2);
                            float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
                            if (num175 < num170 && Collision.CanHit(new Vector2(loc.X, loc.Y), 1, 1, Main.player[num172].position, Main.player[num172].width, Main.player[num172].height))
                            {
                                num170 = num175;
                                num171 = num172;
                            }
                        num = num172;
                    }
                    return Main.player[num171];
                }


    /// <summary>
    /// Combines duplicate items and checks if the player has enough. Workaround for duplicate item recipe bug.
    /// Returns true if the player has enough of the item.
    /// </summary>
    /// <param name="recipe"></param>
    /// <returns></returns>
    /// this was borrowed from https://github.com/SaerusTierialis/tModLoader_ExperienceAndClasses
    public static bool EnforceDuplicatesInRecipe(ModRecipe recipe)
    {
        List<int> types = new List<int>();
        List<int> stacks = new List<int>();
        Item[] ingedients = recipe.requiredItem;
        int ind;
        for (int i = 0; i < ingedients.Length; i++)
        {
            ind = types.IndexOf(ingedients[i].type);
            if (ind >= 0)
            {
                stacks[ind] += ingedients[i].stack;
            }
            else
            {
                types.Add(ingedients[i].type);
                stacks.Add(ingedients[i].stack);
            }
        }
        int count;
        for (int i = 0; i < types.Count; i++)
        {
            count = Main.LocalPlayer.CountItem(types[i], stacks[i]);
            if (count > 0 & count < stacks[i])
            {
                return false;
            }
        }

        return true;
    }


            public static void PlaySound(int type,Vector2 here, int style)
        {
            if (Main.netMode != 2)
            {

                    Main.PlaySound(type, (int)here.X, (int)here.Y, style);
            }
        }


        public static void Chat(string message,byte color1,byte color2,byte color3)
        {
            if (Main.netMode != 2)
            {
                string text = message;
                Main.NewText(text, color1, color3, color3);
            }
            else
            {
                NetworkText text = NetworkText.FromLiteral(message);
                NetMessage.BroadcastChatMessage(text, new Color(color1, color2, color3));
            }
        }



            public static float LookAt(Vector2 here, Vector2 there)
        {

                float rotation = (float)Math.Atan2(here.Y - there.Y, here.X-there.X);
                return rotation;

        }



public static List<Projectile> Shattershots(Vector2 here,Vector2 there,Vector2 widthheight,int type,int damage,float Speed,float spread,int count,bool centershot,float globalangularoffset,bool tilecollidez,int timeleft){
//if (Main.netMode!=1){
List<Projectile> returns=new List<Projectile>();
//List<Part> parts = new List<Part>();
                Vector2 vector8 = new Vector2(here.X + (0), here.Y + (0));
                //int type = mod.ProjectileType("EnkiduWind");
                //Main.PlaySound(2, (int)here.X, (int)here.Y, 12);
                float rotation = (float)Math.Atan2(vector8.Y - (there.Y + (widthheight.X * 0.5f)), vector8.X - (there.X + (widthheight.Y * 0.5f)));
                            spread = spread * (0.0174f);
                            float baseSpeed = (float)Math.Sqrt((float)((Math.Cos(rotation) * Speed) * -1) * (float)((Math.Cos(rotation) * Speed) * -1) + (float)((Math.Sin(rotation) * Speed) * -1) * (float)((Math.Sin(rotation) * Speed) * -1));
                            double startAngle = Math.Atan2((float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1));
                            double deltaAngle = spread/count;
                            double offsetAngle;
                            int i;
                            for (i = 0; i < count;i++ )
                            {
                                offsetAngle = (startAngle+globalangularoffset) + deltaAngle * i;
                                double offsetAngle2 = (startAngle+globalangularoffset) - (deltaAngle * i);                              
                                if (centershot==true || i>0){
                                int proj=Projectile.NewProjectile(vector8.X, vector8.Y, baseSpeed*(float)Math.Sin(offsetAngle), baseSpeed*(float)Math.Cos(offsetAngle), type, damage, Speed, 0);
                                Main.projectile[proj].friendly=false;
                                Main.projectile[proj].hostile=true;
                                Main.projectile[proj].tileCollide = tilecollidez;
                                Main.projectile[proj].timeLeft = timeleft;
                                returns.Insert(returns.Count,Main.projectile[proj]);
                            }
                                if (i>0){
                                int proj2=Projectile.NewProjectile(vector8.X, vector8.Y, baseSpeed*(float)Math.Sin(offsetAngle2), baseSpeed*(float)Math.Cos(offsetAngle2), type, damage, Speed, 0);
                                Main.projectile[proj2].friendly=false;
                                Main.projectile[proj2].hostile=true;
                                Main.projectile[proj2].tileCollide = tilecollidez;
                                Main.projectile[proj2].timeLeft = timeleft;
                                returns.Insert(returns.Count,Main.projectile[proj2]);
                            }
                            }


return returns;
//}
}
//end of function

public static List<NPC> FindNPCs(int type){
List<NPC> returns=new List<NPC>();
for (int i = 0; i < 200;i++ )
{
if (Main.npc[i].type==type){
returns.Insert(returns.Count,Main.npc[i]);
}
}

return returns;
}



}}