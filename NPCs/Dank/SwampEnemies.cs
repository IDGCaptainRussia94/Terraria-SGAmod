using Idglibrary;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{

    public class FlySwarm : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Bee);
            npc.width = 28;
            npc.height = 28;
            npc.damage = 8;
            npc.defense = 2;
            npc.lifeMax = 10;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = 5;
            aiType = NPCID.Bee;
            animationType = NPCID.Bee;
            banner = npc.type;
            bannerItem = mod.ItemType("FlySwarmBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fly Swarm");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Bee];
        }

        public override bool CheckDead()
        {
            int y = (int)(npc.position.Y);
            int x = (int)(npc.position.X + 20);
            for (int i = 0; i < Main.rand.Next(4, 6); i++)
            {
                int num5 = NPC.NewNPC(x, y, mod.NPCType("Fly"), 0, 0f, 0f, 0f, 0f, 255);
                Main.npc[num5].velocity.X = Main.rand.Next(-3, 4);
                Main.npc[num5].velocity.Y = Main.rand.Next(-3, 4);
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int spawn = Main.rand.Next(1, 6);
            return spawn == 3 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 2f : 0f;
        }
    }
    public class SwampBigMimic : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BigMimicHallow);
            npc.damage = 126;
            npc.defense = 34;
            npc.lifeMax = 3500;
            npc.value = 15000f;
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BigMimicHallow];
            aiType = NPCID.BigMimicHallow;
            animationType = NPCID.BigMimicHallow;
            banner = npc.type;
            bannerItem = mod.ItemType("DankMimicBanner");
            npc.rarity = 2;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Mimic");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BigMimicHallow];
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int rand = Main.rand.Next(1, 95);
            return Main.hardMode && rand == 45 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.75f : 0f;
        }

        public override void NPCLoot()
        {
            int rand = Main.rand.Next(5);
            if (rand == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Treepeater"));
            }
            if (rand == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SwampSovnya"));
            }        
            if (rand == 2)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("SludgeBomb"), Main.rand.Next(40, 120));
            }        
            if (rand == 3)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("EarthbreakerShield"));
            }         
            if (rand == 4)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StickySituationSummon"));
            }
        }
    }

    public class SwampLizard : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 78;
            npc.height = 27;
            npc.damage = 73;
            npc.defense = 34;
            npc.lifeMax = 300;
            npc.value = 100f;
            npc.aiStyle = 3;
            aiType = NPCID.Unicorn;
            animationType = NPCID.BloodZombie; //Changed from Zombie to BloodZombie
            npc.HitSound = SoundID.NPCHit1; //New addition
            // npc.DeathSound = SoundID.NPCDeath1; //New addition
            banner = npc.type;
            bannerItem = mod.ItemType("GiantLizardBanner");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, mod.GetGoreSlot("Gores/GiantLizard_head_gib"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * -16, 0), npc.velocity, mod.GetGoreSlot("Gores/GiantLizard_tail_gib"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, mod.GetGoreSlot("Gores/GiantLizard_leg_gib"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, mod.GetGoreSlot("Gores/GiantLizard_leg_gib"), 1f);

            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Lizard");
            Main.npcFrameCount[npc.type] = 9; //Changed from 8 to 9
        }

        public override void NPCLoot()
        {
            Microsoft.Xna.Framework.Audio.SoundEffectInstance snd = Main.PlaySound(SoundID.DD2_WyvernDeath, (int)npc.Center.X, (int)npc.Center.Y);
            if (snd != null)
            {
                snd.Pitch = -0.50f;
            }

            if (Main.rand.Next(100) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LizardEgg);
            }
            if (Main.rand.Next(5) == 0 && SGAWorld.downedMurk > 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Biomass"), Main.rand.Next(1, 12));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int rand = Main.rand.Next(2);
            return Main.hardMode && rand == 1 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 1.25f : 0f;
        }
    }

    public class BlackLeech : ModNPC
    {
        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 8;
            npc.damage = 7;
            npc.defense = 2;
            npc.lifeMax = 5;
            npc.noTileCollide = false;
            npc.noGravity = true;
            npc.npcSlots = 0.15f;
            npc.HitSound = SoundID.NPCHit9;
            npc.DeathSound = SoundID.NPCDeath11;
            npc.value = 0f;
            npc.aiStyle = 3;
            Main.npcFrameCount[npc.type] = 2;
            banner = npc.type;
            bannerItem = mod.ItemType("BlackLeechBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Black Leech");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void AI()
        {
            npc.rotation = npc.velocity.ToRotation();
            Player player = Main.player[npc.target]; npc.TargetClosest(true);
            if (npc.ai[0] == 0f)
            {
                if (npc.wet)
                {
                    npc.noGravity = true;
                    if (player.wet)
                    {
                        if (player.position.X < npc.position.X)
                        {
                            npc.velocity.X -= 0.05f;
                            if (npc.velocity.X < -3)
                            {
                                npc.velocity.X = -3;
                            }
                        }
                        else
                        {
                            npc.velocity.X += 0.05f;
                            if (npc.velocity.X > 3)
                            {
                                npc.velocity.X = 3;
                            }
                        }
                        if (player.position.Y < npc.position.Y)
                        {
                            npc.velocity.Y -= 0.05f;
                            if (npc.velocity.Y < -3)
                            {
                                npc.velocity.Y = -3;
                            }
                        }
                        else
                        {
                            npc.velocity.Y += 0.05f;
                            if (npc.velocity.Y < 3)
                            {
                                npc.velocity.Y = 3;
                            }
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    npc.noGravity = false;
                }
            }
            else if (!player.dead)
            { 
                if (npc.ai[0] == 1f)
                {
                    npc.position = player.position;
                }
            }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            npc.ai[0] = 1f;
            target.AddBuff(BuffID.Bleeding, 360);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return spawnInfo.water && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.player.SGAPly().DankShrineZone ? 3.5f : 0f;
        }
    }

    public class MudMummy : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Mummy);
            npc.lifeMax = 600;
            npc.value = 1500f;
            aiType = NPCID.Mummy;
            animationType = NPCID.Mummy;
            banner = npc.type;
            bannerItem = mod.ItemType("MudMummyBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mud Mummy");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Mummy];
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(0, 3)==0)
            {
                int rand = Main.rand.Next(2);
                if (rand == 0)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.LightShard);
                }
                if (rand == 1)
                {
                    Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DarkShard);
                }
            }
            else
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DankCore"), 1);
            }
        }

        public override void FindFrame(int frameHeight)
        {
            if (npc.rotation >= MathHelper.Pi)
            {
                npc.localAI[3] += Math.Abs(npc.velocity.X/8f);
                npc.frame.Y = npc.frame.Y + frameHeight*((int)(npc.localAI[3])%8);
            }
        }

        public override bool PreAI()
        {
            Player target = Main.player[npc.target];

            bool doRest = true;

            if (npc.aiAction > 100)
                doRest = false;

            if (doRest)
            {
                npc.rotation /= 2f;
                if (target != null && target.active && !target.dead)
                {
                    if (Collision.CanHitLine(npc.Center, 1, 1, target.Center, 1, 1))
                    {
                        npc.aiAction += 3;
                    }
                }
                if (Main.rand.Next(0, 3) == 0)
                {
                    if (Main.netMode != 1)
                    {
                        npc.aiAction += Main.rand.Next(1, 4);
                        NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, npc.whoAmI, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }

            if (!doRest)
            {

                int directiontogo = npc.aiAction > 5400 ? 1 : -1;

                if (npc.aiAction < 5000 || (npc.aiAction > 5400 || (npc.aiAction > 5000 && npc.velocity.Y<-400)))
                {
                    bool hit = false;
                    List<Vector2> there = new List<Vector2>();
                    Vector2 gotothere = Vector2.Zero;
                    for (int i = 0; i < 100; i += 1)
                    {
                        Point16 here = new Point16((int)(npc.Center.X / 16), (int)(npc.Center.Y / 16) + (i * directiontogo));
                        if (WorldGen.InWorld(here.X, here.Y))
                        {
                            there.Add(here.ToVector2());
                            if (!Collision.CanHitLine(npc.Center, 1, 1, here.ToVector2() * 16, 1, 1))
                            {
                                hit = true;
                                gotothere = here.ToVector2() * 16;
                                break;
                            }

                        }

                    }

                    if (hit)
                    {
                        for (int i = 0; i < there.Count; i += 1)
                        {
                            int DustID2 = Dust.NewDust(there[i]*16 - new Vector2(npc.width, npc.height) / 2f, npc.width, npc.height, mod.DustType("MangroveDust"), Main.rand.NextFloat(-4f,4f), -0.20f + npc.velocity.Y * 0.2f, 20, default(Color), 1f);
                            Main.dust[DustID2].noGravity = true;
                        }
                        npc.Center = gotothere + new Vector2(0, (npc.height+4) * -directiontogo);

                        npc.aiAction = 5000;
                        if (directiontogo == 1)
                        {
                            npc.aiAction = -400;
                        }

                    }
                    else
                    {
                        npc.aiAction = -200;
                        return true;
                    }

                }

                npc.rotation = MathHelper.Pi;
                npc.aiAction += 1;
                npc.velocity.Y -= 0.5f;
                npc.velocity.X /= 1.05f;

                if (Math.Abs(npc.velocity.X)<0.25 && npc.velocity.Y <= 0 && !Collision.CanHitLine(npc.Center,1,1,npc.Center-new Vector2(0,24),1,1))
                {
                    npc.velocity.Y = 5;
                }

                npc.spriteDirection = -Math.Sign(target.Center.X - npc.Center.X);

                npc.velocity.X += Math.Sign(target.Center.X - npc.Center.X) / 8f;

                if (target != null && target.active && !target.dead)
                {
                    if (!Collision.CanHitLine(npc.Center, 1, 1, target.Center, 1, 1))
                    {
                        npc.aiAction += 3;
                    }
                    else
                    {
                        if (Math.Abs(target.Center.X - npc.Center.X) < 64 || Main.rand.Next(0,4) == 0)
                        {
                            if (npc.aiAction % 80 == 0)
                            {
                                int num5 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, Main.rand.Next(0,2) == 0 ? ModContent.NPCType<BlackLeech>() : ModContent.NPCType<Murk.Fly>(), 0, 0f, 0f, 0f, 0f, 255);
                                Main.npc[num5].velocity.X = Main.rand.Next(-3, 4);
                                Main.npc[num5].velocity.Y = Main.rand.Next(-3, 4);
                                if (Main.netMode == 2 && num5 < 200)
                                {
                                    NetMessage.SendData(MessageID.SyncNPC, -1, -1, null, num5, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }
;
                        }
                        else
                        {
                            if (npc.aiAction % 40 == 0)
                            {
                                List<Projectile> itz = Idglib.Shattershots(npc.Center + new Vector2(0, 16), target.Center, Vector2.Zero, ProjectileID.MudBall, 25, 8f, 0, 1, true, 0, true, 300);
                                foreach (Projectile proj in itz)
                                {
                                    proj.aiStyle = -5;
                                }
                            }
                        }
                    }
                }

            }

            return doRest;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            if (spawnInfo.player.ZoneJungle && Main.hardMode)
                return 0.01f;

            return Main.hardMode ? ((tile == mod.TileType("MoistStone") || TileID.Sets.Mud[tile]) ? 0.15f : 0f) : 0f;
        }
    }
}
