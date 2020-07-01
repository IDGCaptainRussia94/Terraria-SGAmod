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

	public class SandscorchedGolem : ModNPC
	{
		float framevar=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sandscorched Golem");
			Main.npcFrameCount[npc.type] = 4;
		}

        	public override void SetDefaults()
        	{
            	npc.lifeMax = 1000;
            	npc.defense = 22;
            	npc.damage = 65;
            	npc.scale = 1f;
            	npc.width = 48;
            	npc.height = 56;
            	animationType = -1;
            	npc.aiStyle = 3;
		npc.knockBackResist = 0.4f;
		npc.buffImmune[BuffID.Poisoned] = true;
		npc.buffImmune[BuffID.Venom] = true;
		npc.buffImmune[BuffID.OnFire] = true;
		npc.buffImmune[BuffID.ShadowFlame] = true;
		npc.buffImmune[BuffID.CursedInferno] = true;
		npc.buffImmune[mod.BuffType("ThermalBlaze")] = true;
            	npc.npcSlots = 0.1f;
            	npc.netAlways = true;
            	npc.HitSound = SoundID.NPCHit7;
            	npc.DeathSound = SoundID.NPCDeath6;
            	npc.value = Item.buyPrice(0, 0,50);
        	}

		public override void AI()
		{
			npc.TargetClosest(false);
			for (int k = 0; k < 1; k++)
            		{
                		int dust = Dust.NewDust(npc.position - new Vector2(8f, 8f), npc.width + 6, npc.height + 8, mod.DustType("HotDust"), 0.6f, 0.5f, 0, default(Color), 1.0f);
				Main.dust[dust].noGravity = true;				
				Main.dust[dust].velocity *= 0.0f;
            		}
            		Player target=Main.player[npc.target];
            	if (target!=null && (!target.dead)){
            	npc.spriteDirection=target.position.X-npc.position.X>0f ? 1 : -1;
            	}else{
            	npc.spriteDirection=npc.velocity.X>0 ? 1 : -1;
           		}
           		npc.velocity+=new Vector2((float)npc.spriteDirection*0.25f,0f);
           		 npc.velocity=new Vector2(MathHelper.Clamp(npc.velocity.X/1.05f,-10f,10f),npc.velocity.Y);
			if (npc.velocity.Length()<2)
				npc.velocity.Y =-5f;

			framevar +=Math.Abs(npc.velocity.X)*0.05f;
		}

				public override void FindFrame(int frameHeight)
		{
			npc.frame.Y=(((int)framevar%4))*npc.height;
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
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && !NPC.BusyWithAnyInvasionOfSorts() && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.player.ZoneDesert && Main.hardMode ? 0.15f : 0f;
		}


        }
}