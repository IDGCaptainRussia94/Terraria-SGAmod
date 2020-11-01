using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
	public class MudBall : ModNPC
	{
		public override void SetDefaults()
		{
			npc.width = 26;
			npc.height = 32;
			npc.damage = 21;
			npc.defense = 7;
			npc.lifeMax = 32;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 40f;
			npc.knockBackResist = 0.7f;
			npc.aiStyle = 2;
            animationType = NPCID.DemonEye;
            aiType = NPCID.DemonEye;
			banner = npc.type;
			bannerItem = mod.ItemType("MudBallBanner");
		}

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Mudball");
            Main.npcFrameCount[npc.type] = 2;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
			for (int num654 = 0; num654 < (npc.life<1 ? 16 : 1); num654++)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
				int num655 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 184, npc.velocity.X + randomcircle.X * 8f, npc.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 2f);
				Main.dust[num655].noGravity = true;
				Main.dust[num655].velocity *= 0.5f;
			}
		}

        public override void NPCLoot()
        {
			for (int num172 = 0; num172 < Main.maxPlayers; num172 += 1)
			{
				Player target = Main.player[num172];
				if (target != null && target.active)
				{
					float damagefalloff = 1f - ((target.Center - npc.Center).Length() / 120f);
					if ((target.Center - npc.Center).Length() < 90f)
					{
						target.AddBuff(BuffID.OgreSpit, 60 + (int)(60f * damagefalloff * 5f));
					}
				}
			}
		}

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			int spawn = Main.rand.Next(1,3);
			return spawn == 2 && SGAUtils.NoInvasion(spawnInfo) && spawnInfo.spawnTileType==mod.TileType("MoistStone") && spawnInfo.player.SGAPly().DankShrineZone ? 0.50f : 0f;
		}
	}
}
