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
            npc.DeathSound = SoundID.NPCDeath4;
            npc.value = 600f;
            npc.noGravity = true;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 14;
            animationType = NPCID.CaveBat;
            aiType = NPCID.CaveBat;
            banner = npc.type;
            bannerItem = mod.ItemType("SwampBatBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Bat");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CaveBat];
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center, npc.velocity, mod.GetGoreSlot("Gores/SwampBat_gib"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(25) == 0)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ChainKnife);
            }
            if (Main.rand.Next(4) == 0 && SGAWorld.downedMurk>1)
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MurkyGel"), Main.rand.Next(6));
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType == mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.75f : 0f;
        }
    }
}
