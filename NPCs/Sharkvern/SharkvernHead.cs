using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs.Sharkvern
{
    [AutoloadBossHead]
    public class SharkvernHead : ModNPC
    {

        public int sergedout=0;
        public int rage=0;
        public bool touchwater=false;
        public Vector2 Summoncenter=new Vector2(0,0);
        public List<int> averagey;
        public int timer = 0;
        public bool ramwater = true;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkvern");
            Main.npcFrameCount[npc.type] = 1;
        }
        
        public override void SetDefaults()
        {
            npc.lifeMax = 50000;        
            npc.damage = 80;    
            npc.defense = 10;        
            npc.knockBackResist = 0f;
            npc.width = 52; 
            npc.height = 66; 
            npc.boss = true;
            npc.lavaImmune = true;      
            npc.noGravity = true;          
            npc.noTileCollide = true;       
            npc.HitSound = SoundID.NPCHit1;
            npc.behindTiles = true;
            npc.DeathSound = SoundID.NPCDeath1;
            Main.npcFrameCount[npc.type] = 1;
            npc.npcSlots = 1f;
            npc.netAlways = true;
            bossBag = mod.ItemType("SharkvernBag");
            npc.value = Item.buyPrice(0, 25, 0, 0);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        }

        public override void NPCLoot()
        {
            if (Main.expertMode)
            {
               npc.DropBossBags();
            }
            else
            {
                int lLoot = (Main.rand.Next(0,4));
                if (lLoot == 0)
                {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SkytoothStormSpell"));
                }
                if (lLoot == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Jaws"));
                }
                if (lLoot == 2)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SnappyShark"));
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SharkTooth"), 99);
                }
                if (lLoot == 3)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SharkBait"), Main.rand.Next(50, 100));
                }
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SharkTooth"), Main.rand.Next(50,100));

                List<int> types=new List<int>();
                types.Insert(types.Count,ItemID.SharkFin); 
                types.Insert(types.Count,ItemID.Seashell); 
                types.Insert(types.Count,ItemID.Starfish); 
                types.Insert(types.Count,ItemID.SoulofFlight);
                types.Insert(types.Count,ItemID.Coral); 

                for (int f = 0; f < (Main.expertMode ? 150 : 75); f=f+1){
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, types[Main.rand.Next(0,types.Count)]);
                }

            }
            if(Main.rand.Next(7) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SharkvernMask"));
            }
            if(Main.rand.Next(10) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SharkvernTrophy"));
            }


            //if (SLWorld.currentSubworld!=null)
                //SubworldCache.AddCache("SGAmod", "SGAWorld", "downedSharkvern", SGAWorld.downedSharkvern);

            Achivements.SGAAchivements.UnlockAchivement("Sharkvern", Main.LocalPlayer);
            SGAWorld.downedSharkvern = true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(ramwater);
            writer.Write(timer);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            ramwater = reader.ReadBoolean();
            timer = reader.ReadInt32();
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.650f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }
        
        public override bool PreAI()
        {
            if (averagey == null)
                averagey = new List<int>();

            if (Main.netMode != 1)
            {
                if (npc.ai[0] == 0)
                {
                   
                    npc.realLife = npc.whoAmI;
                  
                    int latestNPC = npc.whoAmI;

                    int randomWormLength = Main.rand.Next(1, 1);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernNeck"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody2"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                        latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernBody3"), npc.whoAmI, 0, latestNPC,ai3: npc.whoAmI);
                        Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                    }
                    latestNPC = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("SharkvernTail"), npc.whoAmI, 0, latestNPC, ai3: npc.whoAmI);
                    Main.npc[(int)latestNPC].realLife = npc.whoAmI;
                    Main.npc[(int)latestNPC].ai[3] = npc.whoAmI;

                    npc.ai[0] = 1;
                    npc.netUpdate = true;
                }
            }
            touchwater=false;

            int minTilePosX = (int)(npc.position.X / 16.0) - 1;
            int maxTilePosX = (int)((npc.position.X + npc.width) / 16.0) + 2;
            int minTilePosY = (int)(npc.position.Y / 16.0) - 1;
            int maxTilePosY = (int)((npc.position.Y + npc.height) / 16.0) + 2;
            if (minTilePosX < 0)
                minTilePosX = 0;
            if (maxTilePosX > Main.maxTilesX)
                maxTilePosX = Main.maxTilesX;
            if (minTilePosY < 0)
                minTilePosY = 0;
            if (maxTilePosY > Main.maxTilesY)
                maxTilePosY = Main.maxTilesY;

            bool collision = false;
            for (int i = minTilePosX-5; i < maxTilePosX+5; ++i)
            {
                for (int j = minTilePosY - 5; j < maxTilePosY + 10; ++j)
                {
                    if (Main.tile[i, j] != null)
                    {
                        if ((int)Main.tile[i, j].liquid > 64)
                        {
                            touchwater = true;
                        }
                    }
                }
             }

            if (ramwater == false)
            {
                for (int i = minTilePosX - 5; i < maxTilePosX + 5; ++i)
                {
                    for (int j = minTilePosY - 5; j < maxTilePosY; ++j)
                    {
                        if (Main.tile[i, j] != null)
                        {
                            if ((int)Main.tile[i, j].liquid > 240)
                            {
                                if (ramwater == false)
                                {
                                    if (npc.velocity.Y > 6)
                                    {
                                        ramwater = true;
                                        for (float xx = 6f; xx < 30f; xx += 0.5f)
                                        {
                                            int proj2 = Projectile.NewProjectile(npc.Center, new Vector2(Main.rand.NextFloat(-8f, 8f), -Main.rand.NextFloat(0, xx)), mod.ProjectileType("RandomOceanCrap"), 30, 4);
                                            Main.projectile[proj2].friendly = false;
                                            Main.projectile[proj2].hostile = true;
                                            Main.projectile[proj2].netUpdate = true;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }

            for (int i = minTilePosX; i < maxTilePosX; ++i)
            {
                for (int j = minTilePosY; j < maxTilePosY; ++j)
                {
                    if (Main.tile[i, j] != null && (Main.tile[i, j].nactive() && (Main.tileSolid[(int)Main.tile[i, j].type] || Main.tileSolidTop[(int)Main.tile[i, j].type] && (int)Main.tile[i, j].frameY == 0) || (int)Main.tile[i, j].liquid > 64))
                    {
                        Vector2 vector2;
                        vector2.X = (float)(i * 16);
                        vector2.Y = (float)(j * 16);
                        if (npc.position.X + npc.width > vector2.X && npc.position.X < vector2.X + 16.0 && (npc.position.Y + npc.height > (double)vector2.Y && npc.position.Y < vector2.Y + 16.0))
                        {
                            collision = true;
                            if (Main.rand.Next(100) == 0 && Main.tile[i, j].nactive())
                                WorldGen.KillTile(i, j, true, true, false);
                        }
                    }
                }
            }
           
            if (!collision)
            {
                Rectangle rectangle1 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
                int maxDistance = 1000;
                bool playerCollision = true;
                for (int index = 0; index < 255; ++index)
                {
                    if (Main.player[index].active)
                    {
                        Rectangle rectangle2 = new Rectangle((int)Main.player[index].position.X - maxDistance, (int)Main.player[index].position.Y - maxDistance, maxDistance * 2, maxDistance * 2);
                        if (rectangle1.Intersects(rectangle2))
                        {
                            playerCollision = false;
                            break;
                        }
                    }
                }
                if (playerCollision)
                    collision = true;
            }

            
            float speed = 15f+(Math.Min(Math.Max(0,rage/100f),6));

            float acceleration = 1f+(Math.Min(Math.Max(0,rage/300f),3));

            npc.position=new Vector2(MathHelper.Clamp(npc.position.X,150f,(float)(Main.maxTilesX*16)-150f),npc.position.Y);

            Vector2 npcCenter = new Vector2(npc.position.X + npc.width * 0.5f, npc.position.Y + npc.height * 0.5f);
            float targetXPos = Main.player[npc.target].position.X + (Main.player[npc.target].width / 2);
            float targetYPos = Main.player[npc.target].position.Y + (Main.player[npc.target].height / 2);

            timer += 1;


            if (timer % 1000 > 700)
            {
                collision = true;
                targetYPos -= 800;
                if (timer % 1000 == 849)
                {
                    ramwater = false;
                    npc.netUpdate = true;
                }
                if (timer % 1000 > 850)
                {
                    targetYPos += 4800;
                }
            }

            if (npc.ai[3]>0){
                timer = 0;
            targetXPos = (float)(Main.maxTilesX*0.6f);
            if (npc.Center.X>(Main.maxTilesX/2)*16)
            targetXPos=(float)(Main.maxTilesX*16)-(Main.maxTilesX*0.6f);
            targetYPos = averagey[0];
            //targetYPos=(float)SGAWorld.RaycastDownWater((int)targetXPos/16,(int)(1),50)*16;
            Summoncenter =new Vector2(targetXPos,targetYPos-500f);
            double angle=(npc.ai[3]/30f)+ 2.0* Math.PI;
            targetXPos+=(float)((Math.Cos(angle) * 800f) * -1f);
            targetYPos+=(float)((Math.Sin(angle) * 400f) * -1f)-500f;
            }

            if (Main.player[npc.target].dead){
            targetYPos=100000f;
            }

            float targetRoundedPosX = (float)((int)(targetXPos / 16.0) * 16);
            float targetRoundedPosY = (float)((int)(targetYPos / 16.0) * 16);
            npcCenter.X = (float)((int)(npcCenter.X / 16.0) * 16);
            npcCenter.Y = (float)((int)(npcCenter.Y / 16.0) * 16);
            float dirX = targetRoundedPosX - npcCenter.X;
            float dirY = targetRoundedPosY - npcCenter.Y;

            float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
            
            if (!collision && npc.ai[3]<1)
            {
                npc.TargetClosest(true);
                npc.velocity.Y = npc.velocity.Y + 0.11f;
                if (npc.velocity.Y > speed)
                    npc.velocity.Y = speed;
                if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.4)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    else
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                }
                else if (npc.velocity.Y == speed)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                }
                else if (npc.velocity.Y > 4.0)
                {
                    if (npc.velocity.X < 0.0)
                        npc.velocity.X = npc.velocity.X + acceleration * 0.9f;
                    else
                        npc.velocity.X = npc.velocity.X - acceleration * 0.9f;
                }
            }
            
            else
            {
                if (timer % 1000 <= 700)
                {
                    if (npc.soundDelay == 0)
                    {
                        float num1 = length / 40f;
                        if (num1 < 10.0)
                            num1 = 10f;
                        if (num1 > 20.0)
                            num1 = 20f;
                        npc.soundDelay = (int)num1;
                        Main.PlaySound(15, (int)npc.position.X, (int)npc.position.Y, 1);
                    }
                }
                float absDirX = Math.Abs(dirX);
                float absDirY = Math.Abs(dirY);
                float newSpeed = speed / length;
                dirX = dirX * newSpeed;
                dirY = dirY * newSpeed;
                if (npc.velocity.X > 0.0 && dirX > 0.0 || npc.velocity.X < 0.0 && dirX < 0.0 || (npc.velocity.Y > 0.0 && dirY > 0.0 || npc.velocity.Y < 0.0 && dirY < 0.0))
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration;
                    if (npc.velocity.Y < dirY)
                        npc.velocity.Y = npc.velocity.Y + acceleration;
                    else if (npc.velocity.Y > dirY)
                        npc.velocity.Y = npc.velocity.Y - acceleration;
                    if (Math.Abs(dirY) < speed * 0.2 && (npc.velocity.X > 0.0 && dirX < 0.0 || npc.velocity.X < 0.0 && dirX > 0.0))
                    {
                        if (npc.velocity.Y > 0.0)
                            npc.velocity.Y = npc.velocity.Y + acceleration * 2f;
                        else
                            npc.velocity.Y = npc.velocity.Y - acceleration * 2f;
                    }
                    if (Math.Abs(dirX) < speed * 0.2 && (npc.velocity.Y > 0.0 && dirY < 0.0 || npc.velocity.Y < 0.0 && dirY > 0.0))
                    {
                        if (npc.velocity.X > 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration * 2f;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration * 2f;
                    }
                }
                else if (absDirX > absDirY)
                {
                    if (npc.velocity.X < dirX)
                        npc.velocity.X = npc.velocity.X + acceleration * 1.1f;
                    else if (npc.velocity.X > dirX)
                        npc.velocity.X = npc.velocity.X - acceleration * 1.1f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                    {
                        if (npc.velocity.Y > 0.0)
                            npc.velocity.Y = npc.velocity.Y + acceleration;
                        else
                            npc.velocity.Y = npc.velocity.Y - acceleration;
                    }
                }
                else
                {
                    if (npc.velocity.Y < dirY)
                        npc.velocity.Y = npc.velocity.Y + acceleration * 1.1f;
                    else if (npc.velocity.Y > dirY)
                        npc.velocity.Y = npc.velocity.Y - acceleration * 1.1f;
                    if (Math.Abs(npc.velocity.X) + Math.Abs(npc.velocity.Y) < speed * 0.5)
                    {
                        if (npc.velocity.X > 0.0)
                            npc.velocity.X = npc.velocity.X + acceleration;
                        else
                            npc.velocity.X = npc.velocity.X - acceleration;
                    }
                }
            }
     
            npc.rotation = (float)Math.Atan2(npc.velocity.Y, npc.velocity.X) + 1.57f;
            Vector2 capvelo=npc.velocity;
            capvelo.Normalize();
            float speedmove=15f-(length/800f);
            if (npc.ai[3]>0)
            speedmove=speedmove/2f;
            if (npc.velocity.Length()<capvelo.Length()*speedmove){npc.velocity=capvelo*speedmove;}


            
            if (collision)
            {
                if (npc.localAI[0] != 1)
                    npc.netUpdate = true;
                npc.localAI[0] = 1f;
            }
            else
            {
                if (npc.localAI[0] != 0.0)
                    npc.netUpdate = true;
                npc.localAI[0] = 0.0f;
            }
            if ((npc.velocity.X > 0.0 && npc.oldVelocity.X < 0.0 || npc.velocity.X < 0.0 && npc.oldVelocity.X > 0.0 || (npc.velocity.Y > 0.0 && npc.oldVelocity.Y < 0.0 || npc.velocity.Y < 0.0 && npc.oldVelocity.Y > 0.0)) && !npc.justHit)
                npc.netUpdate = true;

        int num254 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
        int num255 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
                /*if (Main.tile[num254, num255 - 1] == null)
                {
                    Main.tile[num254, num255 - 1] = new Tile();
                }
                 if (Main.tile[num254, num255 - 1].active())
                 {
                if (Main.tile[num254, num255 - 1].liquid > 128)
                {
                rage=0;
                }}*/

             rage=rage+(length<2000 && npc.ai[3]<1 ? 1 : 0);
             if (touchwater==true){
            rage=Math.Max(-150,rage-5);
            if (Main.expertMode)
            rage=((int)Math.Max(((1f-((float)npc.life/(float)npc.lifeMax))*350f)-150f,rage-4));
             }
             npc.damage=Math.Min(npc.defDamage,Math.Max(180,(int)rage/2));
             sergedout=sergedout-1;
             bool anyalive=false;

            if (npc.ai[3] <-399)
            {
                npc.ai[3] -= 1;
                if (npc.ai[3] < -60 * 60)
                {
                    npc.ai[3] = 0;
                }
            }

            if (npc.life > npc.lifeMax * 0.50)
            {
                averagey.Add((int)npc.Center.Y);
            }

            if (npc.ai[3]<1000 && npc.ai[3]>-1 && npc.life<(int)npc.lifeMax*0.50){
            npc.ai[3]+=1;

                if (averagey.Count > 2)
                {
                    int thereat = 0;
                    for(int i=0;i< averagey.Count; i += 1)
                    {
                        thereat += averagey[i];
                    }
                    thereat = (int)(thereat / averagey.Count);
                    averagey.Clear();
                    averagey.Add(thereat);
                }

            if (npc.ai[3]>150){
            if (npc.ai[3]%160==0 && Main.netMode!=1){
            int him=NPC.NewNPC((int)Summoncenter.X, (int)Summoncenter.Y, mod.NPCType("SharvernMinion"));
            Main.npc[him].ai[3]=Main.rand.Next(0,2000);
            }

             for (int i = 0; i < 5; ++i){
                double devider=(i / ((double)5f));
                double angle=(npc.ai[3]/15)+ 2.0* Math.PI * devider;
               Vector2 thecenter=new Vector2((float)((Math.Cos(angle) * 150)), (float)((Math.Sin(angle) * 80)));
               thecenter = thecenter.RotatedByRandom(MathHelper.ToRadians(10));
           int DustID2 = Dust.NewDust(Summoncenter+(thecenter*4.5f), 0, 0, mod.DustType("TornadoDust"), thecenter.X*0.8f, thecenter.X*0.8f, 20, default(Color), 2.5f);
            Main.dust[DustID2].noGravity = true;
            Main.dust[DustID2].velocity = new Vector2(thecenter.X*0.2f, thecenter.Y*0.2f)*-1f;
            }

            for (int i = 0; i < 10; ++i){
                double devider=(i / ((double)10f));
                double angle=(npc.ai[3]/30)+ 2.0* Math.PI * devider;
               Vector2 thecenter=new Vector2((float)((Math.Cos(angle) * 150)), (float)((Math.Sin(angle) * 80)));
           int DustID2 = Dust.NewDust(Summoncenter+(thecenter*0.5f), 0, 0, mod.DustType("TornadoDust"), thecenter.X*0.7f, thecenter.X*0.7f, 20, default(Color), 1.5f);
            Main.dust[DustID2].noGravity = true;
            Main.dust[DustID2].velocity = new Vector2(thecenter.X*0.04f, thecenter.Y*0.04f);
            Main.dust[DustID2].color=Color.Aqua;
            }

            for (int i = 0; i < 360; i+=Main.rand.Next(10,45)){
                double devider=(i / ((double)360f));
                double angle=2.0* Math.PI * devider;
               Vector2 thecenter=new Vector2((float)((Math.Cos(angle) * 150)), (float)((Math.Sin(angle) * 80)));
           int DustID2 = Dust.NewDust(Summoncenter+(thecenter*0.5f), 0, 0, mod.DustType("TornadoDust"), thecenter.X*0.7f, thecenter.X*0.7f, 20, default(Color), 3f);
            Main.dust[DustID2].noGravity = true;
            Main.dust[DustID2].velocity = new Vector2(0,0);
            Main.dust[DustID2].color=Color.Aqua;
            }
          }

            if (npc.ai[3]>999)
            npc.ai[3]=-500;
            }
        

             if (rage==700 && Main.netMode!=1)
             Idglib.Chat("The Sharkvern beckens you to return it to the sea!",50, 50, 255);
            for (int k = 0; k < Main.maxPlayers; k++)
            {
                Player player = Main.player[k];
                if (player != null)
                {
                    if (player.active && !player.dead)
                    {
                        anyalive = true;
                        if (rage > 700)
                            player.AddBuff(BuffID.PotionSickness, 30, true);
                    }
                }
            }
        if (anyalive==false)
          {
                npc.life = 0;
                npc.active = false;
          }

        return false;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
                if (rage>200)
                player.AddBuff(BuffID.Bleeding, rage, true);
                if (rage>300)
                player.AddBuff(mod.BuffType("MassiveBleeding"), rage, true);         
            }
       
       public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation, origin, npc.scale, npc.velocity.X>0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            scale = 1.9f;   
            return null;
        }
    }

        public class SharvernMinion : ModNPC
    {

        int framevar=0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flying Shark");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override bool CheckActive()
        {
            return NPC.CountNPCS(mod.NPCType("SharkvernHead"))<1;
        }

        public override string Texture
        {
            get { return "Terraria/Npc_" + NPCID.Shark; }
        }
        
        public override void SetDefaults()
        {
        npc.CloneDefaults(NPCID.Shark);
        npc.damage=50;
        npc.noGravity = true; 
        npc.knockBackResist = 0.9f;
        npc.lifeMax = 1500;
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.ai[3]%10==0)
            framevar=framevar+1;
            if (framevar>3)
            framevar=0;
            npc.frame.Y=framevar*frameHeight;
        }

           public override void AI()
        {    
            npc.spriteDirection=npc.velocity.X>0 ? 1 : -1;
        if (!Main.player[npc.target].dead){
            Vector2 mix=Main.player[npc.target].Center-npc.Center;
            mix.Normalize();
            npc.velocity+=mix*0.03f;
            //npc.velocity+=new Vector2(0,-0.20f);
            npc.ai[3]+=1;

            if (npc.ai[3]%800>600){

            npc.noTileCollide=true;
            int DustID2 = Dust.NewDust(npc.Center, 0, 0, mod.DustType("TornadoDust"), (npc.velocity.X*-2f)-0.5f, npc.velocity.Y*-0.5f, 20, default(Color), 1.5f);
            Main.dust[DustID2].noGravity = true;
            if (npc.ai[3]%800<785)
            npc.velocity*=0.75f;
            npc.velocity+=mix*2f;
            }else{
            if (npc.ai[3]%600==0 && Main.expertMode){
            List<Projectile> itz=Idglib.Shattershots(npc.Center,Main.player[npc.target].Center,new Vector2(0,0),ProjectileID.SapphireBolt,50,8f,1,1,true,0,true,300);
            }
            npc.noTileCollide=false;
            }
            Vector2 capvelo=npc.velocity;
            capvelo.Normalize();
            npc.velocity=capvelo*Math.Min(npc.velocity.Length(),20f);
            npc.velocity*=0.97f;



             }
        }
    }

    public class RandomOceanCrap : ModProjectile
    {

        int fakeid = ProjectileID.FrostShard;
        int fishtype = 0;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Random Ocean Crap");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.CloneDefaults(fakeid);
            projectile.width = 32;
            projectile.height = 32;
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.penetrate = 1;
            projectile.thrown = true;
            projectile.extraUpdates = 0;
            projectile.tileCollide = true;
            projectile.aiStyle = -1;
            int[] types = { ItemID.Fish, ItemID.Trout, ItemID.TundraTrout, ItemID.ReaverShark, ItemID.Goldfish, ItemID.Ebonkoi,ItemID.CrimsonTigerfish, ItemID.FishStatue, ItemID.OldShoe, ItemID.MirageFish, ItemID.PrincessFish,ItemID.FrostDaggerfish };
            fishtype = types[Main.rand.Next(types.Length)];
        }

        public override string Texture
        {
            get { return "Terraria/Projectile_" + fakeid; }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D tex = ModContent.GetTexture("Terraria/Item_" + fishtype);
            Vector2 drawOrigin = new Vector2(tex.Width, tex.Height)/2;
            Vector2 drawPos = ((projectile.Center - Main.screenPosition));
            spriteBatch.Draw(tex, drawPos,null, lightColor, MathHelper.ToRadians(0) + projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
                Main.dust[dust].scale = 2.50f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity *= 2f;
            }
            return true;
        }

        public override void AI()
        {
            projectile.velocity.Y += 0.20f;
            if (Main.rand.Next(0, 2) == 1)
            {
                int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
                Main.dust[dust].scale = 0.8f;
                Main.dust[dust].noGravity = false;
                Main.dust[dust].velocity = projectile.velocity * 0.4f;
            }
            projectile.rotation += ((float)projectile.velocity.Length()*Math.Sign(projectile.velocity.X))*0.01f;
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(BuffID.Wet, 60 * 5);
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Wet, 60 * 5);
        }

    }


}
