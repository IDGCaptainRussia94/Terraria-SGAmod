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
using Idglibrary;

namespace SGAmod.NPCs
{

	public class TidalElemental : ModNPC
	{
		private int framevar=0;
		public int outofwater=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tidal Elemental");
			Main.npcFrameCount[npc.type] = 4;
		}

        	public override void SetDefaults()
        	{
            	npc.lifeMax = 800;
            	npc.defense = 4;
            	npc.damage = 32;
            	npc.scale = 1f;
            	npc.width = 86;
            	npc.height = 86;
            	animationType = -1;
            	npc.aiStyle = -1;
				npc.knockBackResist = 0.4f;
            	npc.npcSlots = 0.1f;
            	npc.netAlways = true;
            	npc.HitSound = SoundID.NPCHit1;
            	npc.DeathSound = SoundID.NPCDeath6;
            	npc.value = Item.buyPrice(gold: 1);
            	npc.noTileCollide = false;
				npc.noGravity = true;
				npc.netUpdate = true;
        	}

		public override void AI()
		{

		int num254 = (int)(npc.position.X + (float)(npc.width / 2)) / 16;
		int num255 = (int)(npc.position.Y + (float)(npc.height / 2)) / 16;
				npc.ai[0]=npc.ai[0]+1;
				npc.ai[1]=0;
				if (Main.tile[num254, num255+1]!=null && Main.tile[num254, num255 +1].liquid > 64)
				{
				npc.ai[1]=1;
				}
				if (Main.tile[num254, num255 - 1] == null)
				{
					Main.tile[num254, num255 - 1] = new Tile();
				}
				if (Main.tile[num254, num255 + 1] == null)
				{
					Main.tile[num254, num255 + 1] = new Tile();
				}
				if (Main.tile[num254, num255 + 2] == null)
				{
					Main.tile[num254, num255 + 2] = new Tile();
				}
				num255 = (int)(npc.position.Y + (float)(npc.height)) / 16;
				if (Main.tile[num254, num255+1].active()){
				if (Main.tile[num254, num255+1].liquid<64){
				npc.ai[1]=-1;
				}}

				if (npc.ai[1]==-1 && outofwater<300){
				npc.velocity=new Vector2(npc.velocity.X,npc.velocity.Y-Main.rand.Next(6,15));
				}


			Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
				npc.spriteDirection=npc.velocity.X>0 ? 1 : -1;
				}

			}else{
				Vector2 dista = P.Center-npc.Center;
				Vector2 dista2 = P.Center-npc.Center;
				//Vector2 dista2=dista;
				dista2.Normalize();
				npc.velocity+=(dista2*(npc.ai[1]==1 ? 0.2f : 0.4f));
				npc.spriteDirection=dista2.X>0 ? 1 : -1;
				if (outofwater<300){
				outofwater+=(npc.ai[1]==0 ? 1 : 0)+((Collision.CanHitLine(new Vector2(npc.Center.X, npc.Center.Y), 16, 32, new Vector2(P.Center.X, P.Center.Y), 16, 32)) ? 0 : 1);
				}
				if (npc.ai[1]==-1 && outofwater>299 && dista.Y<-30f){
				npc.velocity=new Vector2(npc.velocity.X,npc.velocity.Y-Main.rand.Next(6,15));
				}
				if (npc.ai[0]%400>250 && npc.ai[0]%60==0 && Collision.CanHitLine(new Vector2(npc.Center.X, npc.Center.Y), 4, 4, new Vector2(P.Center.X, P.Center.Y), 4, 4)){
				List<Projectile> itz=Idglib.Shattershots(npc.Center,P.position,new Vector2(P.width,P.height),mod.ProjectileType("ThrownTrident"),20,8,0,1,true,0,true,400);
				itz[0].damage/=2;
				}

			}

			if (outofwater>299){
			outofwater+=1;
			if (outofwater==300){
			Vector2 dista2 = P.Center-npc.Center;
			npc.velocity=new Vector2(npc.velocity.X,dista2.Y>-100 ? 16f : -16f);
			}
			if (outofwater>350)
			npc.ai[1]=1;
			if (outofwater>500){
			outofwater=0;

			}
			npc.noTileCollide=true;
			}else{
			npc.noTileCollide=false;
			}
			npc.velocity=npc.velocity*0.98f;

			if (npc.velocity.Length()>14){
			npc.velocity.Normalize();
			npc.velocity=npc.velocity*14;
			}
			//Main.NewText(""+outofwater,255,255,255);
			if (npc.ai[1]==1){
			npc.rotation=0f;
			}if (npc.ai[1]==0){
			npc.velocity=new Vector2(npc.velocity.X,npc.velocity.Y+1f);
			npc.rotation=(npc.velocity.Y*0.03f)*npc.spriteDirection;
			}
			npc.noGravity = npc.ai[1]==1 ? true : false;
		}

				public override void FindFrame(int frameHeight)
		{

			if (npc.ai[0]%15==0)
			framevar=framevar+1;
			if (framevar>3)
			framevar=0;
			npc.frame.Y=framevar*npc.height;
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			Tile tile = Main.tile[spawnInfo.spawnTileX, spawnInfo.spawnTileY-3];
			bool canspawn=tile.liquid>63 ? true : false;
			return !spawnInfo.playerInTown && !NPC.BusyWithAnyInvasionOfSorts() && !spawnInfo.invasion && !Main.pumpkinMoon && !Main.snowMoon && !Main.eclipse && spawnInfo.spawnTileY < Main.rockLayer && spawnInfo.player.ZoneBeach && canspawn && NPC.downedBoss1 ? 0.05f : 0f;
		}

		public override void NPCLoot()
		{

			List<int> types=new List<int>();
			types.Insert(types.Count,0);
			types.Insert(types.Count,1);
			types.Insert(types.Count,2);
			types.Insert(types.Count, 3);
			int pick =types[Main.rand.Next(0,types.Count)];
			if (pick==0){
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("StarfishBlaster"), 1);
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, 2626, Main.rand.Next(50, 100));
			}
			if (pick==1){
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ThrownTrident"), 1);
			}
			if (pick==2){
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TidalWave"), 1);
			}
			if (pick==3){
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TidalCharm"), 1);
			}		
		}
    }
}