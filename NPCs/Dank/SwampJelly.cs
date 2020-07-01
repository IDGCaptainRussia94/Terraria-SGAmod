using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class SwampJelly : ModNPC
	{
		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 28;
			npc.damage = 24;
			npc.defense = 2;
			npc.lifeMax = 18;
            npc.HitSound = SoundID.NPCHit25;
            npc.DeathSound = SoundID.NPCDeath18;
            npc.noGravity = true;
			npc.value = 80f;
			npc.knockBackResist = 0.5f;
			npc.aiStyle = 18;
			aiType = NPCID.GreenJellyfish;
			animationType = NPCID.GreenJellyfish;
		}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Swamp Jellyfish");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.GreenJellyfish];
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Glowstick, Main.rand.Next(4));
        }

        public override bool CheckDead()
        {
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SwampJelly_1"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SwampJelly_2"), 1f);
            Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SwampJelly_3"), 1f);
            return true;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            int x = spawnInfo.spawnTileX;
            int y = spawnInfo.spawnTileY;
            int tile = (int)Main.tile[x, y].type;
			return SGAUtils.NoInvasion(spawnInfo) && spawnInfo.water && spawnInfo.player.SGAPly().DankShrineZone ? 1f : 0f;
		}
	}
}
