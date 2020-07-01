using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Murk
{
	public class SwampSlime : ModNPC
	{
		public override void SetDefaults()
		{
			npc.width = 38;
			npc.height = 32;
			npc.damage = 14;
			npc.defense = 6;
			npc.lifeMax = 100;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 60f;
			npc.knockBackResist = 1.1f;
			npc.aiStyle = 1;
            npc.alpha = 0;
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
			aiType = NPCID.Crimslime;
			animationType = NPCID.BlueSlime;
		}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dank Slime");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BlueSlime];
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            for (int i = 0; i < 20; i++) //this i a for loop tham make the dust spawn , the higher is the value the more dust will spawn
            {
                int dust = Dust.NewDust(new Vector2(npc.position.X, npc.position.Y), npc.width, npc.height, 0, npc.velocity.X * 0f, npc.velocity.Y * 0f, 80, default(Color), 1f);   //this make so when this projectile disappear will spawn dust, change PinkPlame to what dust you want from Terraria, or add mod.DustType("CustomDustName") for your custom dust
                Main.dust[dust].noGravity = false; //this make so the dust has no gravity
                Main.dust[dust].velocity *= 1f;
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            target.AddBuff(BuffID.Poisoned,60*3);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY];
            return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && SGAWorld.downedMurk>1 && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.player.ZoneJungle ? 0.12f : 0f;
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(4)<1)
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Gel, Main.rand.Next(4));
            if (Main.rand.Next(1+NPC.CountNPCS(mod.NPCType("Murk"))*4) < 1)
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MurkyGel"), Main.rand.Next(3));
            if (Main.rand.Next(2 + NPC.CountNPCS(mod.NPCType("Murk")) * 4) < 1)
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("Biomass"), Main.rand.Next(3));
        }
    }
}