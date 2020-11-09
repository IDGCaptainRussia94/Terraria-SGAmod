using Microsoft.Xna.Framework;
using Terraria;
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
            int spawn = Main.rand.Next(1, 10);
            return spawn == 3 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.5f : 0f;
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
            npc.value = 1000f;
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BigMimicHallow];
            aiType = NPCID.BigMimicHallow;
            animationType = NPCID.BigMimicHallow;
            banner = npc.type;
            bannerItem = mod.ItemType("DankMimicBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Mimic");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BigMimicHallow];
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int rand = Main.rand.Next(1, 95);
            return Main.hardMode && rand == 45 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.5f : 0f;
        }

        public override void NPCLoot()
        {
            int rand = Main.rand.Next(2);
            if (rand == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("DankCore"),Main.rand.Next(4,11));
            }
            if (rand == 1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Treepeater"));
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
            aiType = NPCID.WalkingAntlion;
            animationType = NPCID.Zombie;
            banner = npc.type;
            bannerItem = mod.ItemType("GiantLizardBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Giant Lizard");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Zombie];
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int rand = Main.rand.Next(4);
            return Main.hardMode && rand == 1 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.5f : 0f;
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
            npc.HitSound = SoundID.NPCHit9;
            npc.DeathSound = SoundID.NPCDeath11;
            npc.value = 200f;
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
            return spawnInfo.water && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.player.SGAPly().DankShrineZone ? 2.5f : 0f;
        }
    }

    public class MudMummy : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Mummy);
            npc.lifeMax = 180;
            npc.value = 1000f;
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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
            return Main.hardMode && tile == (mod.TileType("MoistStone")) ? 0.25f : 0f;
        }
    }
}
