using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class SwampBat : ModNPC
	{
		public override void SetDefaults()
		{
			npc.width = 30;
			npc.height = 22;
			npc.damage = 15;
			npc.defense = 5;
			npc.lifeMax = 29;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 600f;
            npc.noGravity = true;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 14;
            animationType = NPCID.CaveBat;
            aiType = NPCID.CaveBat;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Bat");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CaveBat];
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ChainKnife);
            }
            if (Main.rand.Next(2) == 0 && SGAWorld.downedMurk>1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MurkyGel"), Main.rand.Next(10));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.25f : 0f;
        }
    }
}
