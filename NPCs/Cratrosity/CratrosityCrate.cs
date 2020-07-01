using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Cratrosity
{
	public class CratrosityCrate: ModNPC
	{
		public int counter=0;
		public int cratetype=ItemID.WoodenCrate;
		public Vector2 apointzz=new Vector2(0,0);

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Servent of Microtransactions");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(0,4)<1 || npc.value>2999){
	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, (int)npc.value);
}
        }
		public override string Texture
		{
			get { return "Terraria/Item_" + cratetype; }
		}
		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 40;
			npc.defense = 0;
			npc.lifeMax = 2000;
			npc.HitSound = SoundID.NPCHit7;
			npc.DeathSound = SoundID.NPCDeath43;
			npc.value = 0f;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			aiType = -1;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = 40000f;
		}
		public override void AI()
		{
		npc.timeLeft=900;
		if (apointzz==new Vector2(0,0)){
		apointzz=new Vector2(Main.rand.Next(-100,100),Main.rand.Next(-100,100));
		}
		counter=counter+Main.rand.Next(-3,15);
		int npctype=mod.NPCType("Cratrosity");
		int npctype2=mod.NPCType("Cratrogeddon");
		if (NPC.CountNPCS(npctype2)>0){npctype=mod.NPCType("Cratrogeddon");}
		if (NPC.CountNPCS(npctype)>0){
		NPC myowner=Main.npc[NPC.FindFirstNPC(npctype)];
		if (counter%600<450){
		npc.velocity=(npc.velocity+((myowner.Center+apointzz-(npc.position))*0.02f)*0.035f)*0.99f;
	}

	}else{npc.active=false;}

	}

		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("Microtransactions"), 200, true);
		}




	}
}

