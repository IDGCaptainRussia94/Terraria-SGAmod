using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs
{

	public class SandscorchedSlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandscorched Slime");
			Main.npcFrameCount[npc.type] = 2;
		}

        	public override void SetDefaults()
        	{
            	npc.lifeMax = 800;
            	npc.defense = 22;
            	npc.damage = 58;
            	npc.scale = 1f;
            	npc.width = 36;
            	npc.height = 26;
            	animationType = 1;
            	npc.aiStyle = 1;
		npc.knockBackResist = 0.4f;
		npc.buffImmune[BuffID.OnFire] = true;
		npc.buffImmune[BuffID.CursedInferno] = true;
		npc.buffImmune[BuffID.ShadowFlame] = true;
		npc.buffImmune[BuffID.Poisoned] = true;
		npc.buffImmune[BuffID.Venom] = true;
		npc.buffImmune[mod.BuffType("ThermalBlaze")] = true;
            	npc.npcSlots = 0.1f;
            	npc.netAlways = true;
            	npc.HitSound = SoundID.NPCHit1;
            	npc.DeathSound = SoundID.NPCDeath1;
            	npc.value = Item.buyPrice(0, 0,5);
        	}

		public override void AI()
		{
			for (int k = 0; k < 1; k++)
            		{
                		int dust = Dust.NewDust(npc.position - new Vector2(8f, 8f), npc.width + 5, npc.height + 4, mod.DustType("HotDust"), 0.6f, 0.5f, 0, default(Color), 1.0f);
				Main.dust[dust].noGravity = true;				
				Main.dust[dust].velocity *= 0.0f;
            		}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"), Main.rand.Next(1, 2));
		}
        	
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("ThermalBlaze"), 200, true);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY-3];
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && !NPC.BusyWithAnyInvasionOfSorts() && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.player.ZoneDesert && Main.hardMode ? 0.25f : 0f;
		}

        }
}