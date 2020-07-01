using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class Maggot : ModNPC
	{
        int counter, counter2 = 0;
		public override void SetDefaults()
		{
			npc.width = 20;
			npc.height = 20;
			npc.damage = 0;
			npc.defense = 0;
			npc.lifeMax = 5;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.noTileCollide = false;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 66;
			aiType = NPCID.Worm;
			animationType = NPCID.Worm;
            banner = npc.type;
            bannerItem = mod.ItemType("MaggotBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maggot");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Worm];
        }

        public override bool PreAI()
        {
            if (counter++ == 1000)
            {
                npc.Transform(mod.NPCType("MaggotFly"));
                counter = 0;
            }
            if (counter2++ == 15)
            {
                npc.scale += 0.01f;
                counter2 = 0;
            }
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            int spawn = Main.rand.Next(100);
            return Main.hardMode && spawnInfo.spawnTileY < Main.rockLayer && (SGAUtils.NoInvasion(spawnInfo) && spawn == 24 || (spawnInfo.spawnTileType==mod.TileType("MoistStone") && spawn<4)) ? 1f : 0f;
        }
    }
}
