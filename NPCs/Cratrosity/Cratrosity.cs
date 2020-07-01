using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs.Cratrosity
{
	[AutoloadBossHead]
	public class Cratrosity : ModNPC
	{

public Vector2 offsetype = new Vector2(0, 0);
public int phase=5;
public int defaultdamage=60;
public int themode=0;
public float compressvar=1;
public float compressvargoal=1;
public int evilcratetype=WorldGen.crimson ? ItemID.CrimsonFishingCrate : ItemID.CorruptFishingCrate;
Vector2 theclostestcrate=new Vector2(0,0);
public int[,] Cratestype=new int[,] {{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}};
public int[] Cratesperlayer=new int[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
public float[,] Cratesdist=new float[,] {{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f},
{0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f,0f}};
public Vector2[,] Cratesvector=new Vector2[,] {{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)},
{new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0),new Vector2(0,0)}};
public double[,] Cratesangle=new double[,] {{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0},
{0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0}};
public int postmoonlord=0;
public float nonaispin=0f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrosity");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}

		public override bool Autoload(ref string name)
		{
			return base.Autoload(ref name);
		}

		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 60;
			npc.defense = 50;
			npc.lifeMax = 5000;
			npc.HitSound = SoundID.NPCHit4;
			npc.DeathSound = SoundID.NPCDeath37;
			npc.knockBackResist = 0f;
			npc.aiStyle = -1;
			npc.boss=true;
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			theclostestcrate=npc.Center;
			npc.value = Item.buyPrice(0, 45, 0, 0);
		}

		public override string Texture
		{
			get { return "Terraria/Item_" + ItemID.GoldenCrate; }
		}

		public override string BossHeadTexture => "Terraria/Item_" + ItemID.GoldenCrate;


		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override bool CheckActive()
		{
			return false;
		}

		public virtual void FalseDeath(int phase)
		{
		//nothing here

		}

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        }

		public override void NPCLoot()
		{
	Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrariacoCrateKey"));
			Achivements.SGAAchivements.UnlockAchivement("Cratrosity", Main.LocalPlayer);
			if (SGAWorld.downedCratrosity==false){
				Idglib.Chat("The hungry video game industry has been tamed! New items are available for buying",244, 179, 66);
			}
			SGAWorld.downedCratrosity = true;
		}


		public override bool CheckDead()
		{
if (npc.life<1 && phase>0){
npc.life=npc.lifeMax;
phase-=1;
Cratrosity origin = npc.modNPC as Cratrosity;
CrateRelease(phase);
FalseDeath(phase);
if (origin.postmoonlord>0){
//do stuff here
}
npc.active=true;
return false;
}else{return true;}

}

		public override void AI()
		{
						Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || Main.dayTime)
				{
				float speed=((-10f));
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y+speed);
					npc.active = false;
				}

			}else{

		npc.ai[0]+=1f;
		Vector2 gohere=new Vector2(P.Center.X,P.Center.Y-220)+ offsetype;
		float thespeed=0.01f;
		float friction=0.98f-phase*0.04f;
		float friction2=0.99f-phase*0.0075f;
		themode-=1;
		npc.ai[1]+=Main.rand.Next(0,5);
		if (npc.ai[1]%2000>850){
		if (System.Math.Abs(npc.ai[2])<300){
		compressvargoal=1;
		int theammount=(npc.ai[2]>0 ? 1: -1)*(offsetype.X>0 ? 1 : -1);
		if (npc.ai[1]%2000<1100){
		gohere=new Vector2(P.Center.X+(theammount*800),P.Center.Y-220);
		npc.velocity=(npc.velocity+((gohere-npc.Center)*thespeed))*0.98f;
		}else{

		npc.velocity=new Vector2(-theammount* ((GetType() == typeof(Cratrogeddon)) ? 30 : 10),0);
		if (npc.ai[0]%15==0){
		List<Projectile> itz=Idglib.Shattershots(npc.Center,P.Center+new Vector2(0,P.Center.Y>npc.Center.Y ? 600 : -600),new Vector2(0,0),ProjectileID.CopperCoin,(int)(npc.damage*(20.00/defaultdamage)),10,0,1,true,0,true,100);
		}
		if (npc.ai[0]%40==0){
		List<Projectile> itz=Idglib.Shattershots(theclostestcrate,P.Center,new Vector2(0,0),ProjectileID.GoldCoin,(int)(npc.damage*(30.00/defaultdamage)),10,0,1,true,0,true,200);
		}
		if (((npc.ai[0]+20)%40==0) && Main.expertMode){
		List<Projectile> itz=Idglib.Shattershots(theclostestcrate,P.Center+new Vector2(0,P.Center.Y>theclostestcrate.Y ? 600 : -600),new Vector2(0,0),ProjectileID.SilverCoin,(int)(npc.damage*(25.00/defaultdamage)),10,0,1,true,0,true,200);
		SGAprojectile modeproj=itz[0].GetGlobalProjectile<SGAprojectile>();
		modeproj.splittingcoins=true;
		modeproj.splithere=P.Center;
		}
		if (phase<4){
		if (npc.ai[0]%8==0){
		Idglib.Shattershots(npc.Center,npc.Center+new Vector2(-npc.velocity.X,0),new Vector2(0,0),ProjectileID.SilverCoin,(int)(npc.damage*(25.00/defaultdamage)),25,0,1,true,0,false,40);
		}}
		themode=300;
							if (offsetype.X >=0)
							{
								if (npc.Center.X < P.Center.X - 700)
								{
									npc.ai[2] = System.Math.Abs(npc.ai[2]);
								}
								if (npc.Center.X > P.Center.X + 700)
								{
									npc.ai[2] = -System.Math.Abs(npc.ai[2]);
								}
							}
							else
							{
								if (npc.Center.X < P.Center.X - 700)
								{
									npc.ai[2] = -System.Math.Abs(npc.ai[2]);
								}
								if (npc.Center.X > P.Center.X + 700)
								{
									npc.ai[2] = System.Math.Abs(npc.ai[2]);
								}
							}
		//npc.ai[1]=1600+(2000-1600);
		//}
		}

		}else{
		Vector2 gogo=P.Center-npc.Center; gogo.Normalize(); gogo=gogo*(8-phase*1);
		if (GetType() == typeof(Cratrogeddon))
		gogo *= 2f;
		npc.velocity=gogo;
		compressvargoal=2;
		}
		}else{
		npc.ai[2]=Main.rand.Next(-600,600);
		if (npc.ai[0]%600<350){
		npc.velocity=(npc.velocity+(((gohere) -npc.Center)*thespeed))*friction;
		compressvargoal=1;

	switch(phase)
	{
		case 5:
		{
		if (npc.ai[0]%30==0){
		Idglib.Shattershots(npc.Center,P.position,new Vector2(P.width,P.height),ProjectileID.CopperCoin,(int)(npc.damage*(20.00/defaultdamage)),10,0,1,true,0,true,150);
		}
		break;
		}
		case 4:
		{
		if (npc.ai[0]%10==0){
		Idglib.Shattershots(npc.Center,P.position,new Vector2(P.width,P.height),ProjectileID.SilverCoin, (int)(npc.damage*(25.00/defaultdamage)),14,0,1,true,0,true,100);
		}
		break;
		}
		case 3:
		{
		if (npc.ai[0]%3==0 && npc.ai[0]%50>38){
		Idglib.Shattershots(npc.Center,P.position,new Vector2(P.width,P.height),ProjectileID.GoldCoin, (int)(npc.damage*(30/defaultdamage)),16,0,1,true,0,true,90);
		}
		if (npc.ai[0]%8==0 && Main.expertMode){
		List<Projectile> itz=Idglib.Shattershots(npc.Center,npc.Center+new Vector2(0,-5),new Vector2(0,0),ProjectileID.SilverCoin,(int)(npc.damage*(25/defaultdamage)),7,360,2,true,npc.ai[0]/20,true,300);
		}
		break;
		}
		case 2:
		{

		if (npc.ai[0]%50==0){
		Idglib.Shattershots(npc.Center,P.position+new Vector2(P.velocity.X,P.velocity.Y),new Vector2(P.width,P.height),ProjectileID.GoldCoin, (int)(npc.damage * (30.00 / defaultdamage)), (int)(npc.damage*(20.00/defaultdamage)),0,1,true,0,true,100);
		Idglib.Shattershots(npc.Center,P.position+new Vector2(P.velocity.X*3,P.velocity.Y*3),new Vector2(P.width,P.height),ProjectileID.SilverCoin, (int)(npc.damage * (20.00 / defaultdamage)), (int)(npc.damage*(20.00/defaultdamage)),0,1,true,0,true,100);
		Idglib.Shattershots(npc.Center,P.position+new Vector2(P.velocity.X*6,P.velocity.Y*6),new Vector2(P.width,P.height),ProjectileID.CopperCoin, (int)(npc.damage * (10.00 / defaultdamage)), (int)(npc.damage*(20.00/defaultdamage)),0,1,true,0,true,100);

		}
		}
		break;

	}


		}else{
		compressvargoal=0.4f;
		Vector2 gogo=P.Center-npc.Center; gogo.Normalize(); gogo=gogo*(30-phase*2);
				if (GetType() == typeof(Cratrogeddon))
				gogo *= 0.3f;
		if (npc.ai[0]%(25+(phase*10))< (GetType() == typeof(Cratrogeddon) ? 20 : 1))
						{
		npc.velocity=(npc.velocity+gogo);
		}
		npc.velocity=npc.velocity*friction2;
		}
	}
}

npc.defense=(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.WoodenCrate.ToString())))*5+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.IronCrate.ToString())))*6+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.GoldenCrate.ToString())))*6+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.DungeonFishingCrate.ToString())))*10+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.JungleFishingCrate.ToString())))*10+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+evilcratetype.ToString())))*10+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.HallowedFishingCrate.ToString())))*10+
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate"+ItemID.FloatingIslandFishingCrate.ToString())))*(30);
npc.defense*=Main.expertMode ? 4 : 2;
npc.defense+=Main.expertMode ? 20 : 0;
OrderOfTheCrates(P);



		}

				public virtual void OrderOfTheCrates(Player P)
		{
nonaispin=nonaispin+1f;
compressvar+=(compressvargoal-compressvar)/20;
float themaxdist=99999f;
for (int a = 0; a < phase; a=a+1){
Cratesperlayer[a]=4+(a*4);
for (int i = 0; i < Cratesperlayer[a]; i=i+1){
		Cratesangle[a,i] = (a%2==0 ? 1 : -1)*((nonaispin*0.01f)*(1f+a/3f)) + 2.0 * Math.PI * ((i / (double)Cratesperlayer[a]));
		Cratesdist[a,i]= compressvar*20f+((float)a*20f)*compressvar;
		//if (compressvar<1.9){
		//Cratesvector[a,i]= Cratesdist[a,i] * (new Vector2((float)Math.Cos(Cratesangle[a,i]), (float)Math.Sin(Cratesangle[a,i]))) + npc.Center;//npc.Size / 2f;
		//}else{
		float theexpand=0f;

		if (themode>0){
		theexpand=(((i/1f)*(a+1f)))*(themode/30f);
		}
		Cratesvector[a,i]+= ((Cratesdist[a,i] * (new Vector2((float)Math.Cos(Cratesangle[a,i]), (float)Math.Sin(Cratesangle[a,i]))) + npc.Center)-Cratesvector[a,i])/(theexpand+(System.Math.Max((((compressvar)-1)*(2+(a*1))),1)));//npc.Size / 2f;
		float sinner=npc.ai[0]+((float)(i*5)+(a*14));
		float sinner2=(float)(10f+(Math.Sin(sinner/30f)*7f));
		if (compressvar>1.01){
		int [] projtype={ProjectileID.PlatinumCoin,ProjectileID.PlatinumCoin,ProjectileID.PlatinumCoin,ProjectileID.GoldCoin,ProjectileID.SilverCoin,ProjectileID.CopperCoin};
		int [] projdamage={25,30,30,50,60,60};
		float [] projspeed={1f,1f,1f,9f,8f,7f};
		if (a==phase-1){
		if (sinner2<4 && (npc.ai[0]+(i*4))%30==0){
		List<Projectile> itz=Idglib.Shattershots(Cratesvector[a,i],P.position,new Vector2(P.width,P.height),projtype[a+1],(int)((double)npc.damage*((double)projdamage[a+1]/(double)defaultdamage)),projspeed[a+1],0,1,true,0,false,110);
		if (projtype[a+1]==ProjectileID.PlatinumCoin){itz[0].aiStyle=18; IdgProjectile.AddOnHitBuff(itz[0].whoAmI,BuffID.ShadowFlame,60*10);}
		}}

		Cratesvector[a,i]+=((P.Center-Cratesvector[a,i])/(sinner2))*((System.Math.Max(compressvar-1,0)*1));
		}
		float dist=(P.Center-Cratesvector[a,i]).Length();
		if (dist<themaxdist){themaxdist=dist;
		theclostestcrate=Cratesvector[a,i];
		}
		//}
		Cratestype[a,i]= ItemID.WoodenCrate;
		if (a==3){
		Cratestype[a,i]= ItemID.IronCrate;
		if (npc.ai[3]>100000 && i%2==0){
		Cratestype[a,i]= ItemID.HallowedFishingCrate;
		}else{
		if (npc.ai[3]<-100000 && i%2==0){
		Cratestype[a,i]= evilcratetype;
		}}
		}
		if (a==2){
		Cratestype[a,i]= (i%2==0) ? ItemID.JungleFishingCrate : ItemID.DungeonFishingCrate;
		}
		if (a==1){
		Cratestype[a,i]= (i%2==0) ? ItemID.HallowedFishingCrate : (evilcratetype);
		}
		if (a==0){
		Cratestype[a,i]= ItemID.FloatingIslandFishingCrate;
		}

			if (!(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime))
			{
			//if (npc.ai[0]%600<350){
				//if ((npc.ai[0]/100)%Cratesperlayer[a]==i){
			//List<Projectile> projs=SgaLib.Shattershots(Cratesvector[a,i],P.position,new Vector2(P.width,P.height),ProjectileID.CopperCoin,35,12,0,1,true,0,true,150);
			}
		
}}

		}

		/*public override bool CanHitPlayer(Player ply, ref int cooldownSlot){
			return true;
		}
		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return CanBeHitByPlayer(player);
		}
		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return CanBeHitByPlayer(Main.player[projectile.owner]);
		}
		private bool? CanBeHitByPlayer(Player P){
		return true;
		}*/
				public virtual void CrateRelease(int layer)
		{
		double cratehp=npc.lifeMax*0.50;
for (int i = 0; i < Cratesperlayer[layer]; i=i+1){
	int spawnedint=NPC.NewNPC((int)Cratesvector[layer,i].X,(int)Cratesvector[layer,i].Y, mod.NPCType("CratrosityCrate"+Cratestype[layer,i].ToString()));
	NPC spawned=Main.npc[spawnedint];
	spawned.value=Cratestype[layer,i];
	spawned.damage=(int)spawned.damage*(npc.damage/defaultdamage);
	if (Cratestype[layer,i]==ItemID.WoodenCrate){
	spawned.aiStyle=10;
	spawned.knockBackResist=0.98f;
	spawned.lifeMax=(int)cratehp/5;
	spawned.life=(int)cratehp/5;
	spawned.damage=(int)50*(npc.damage/defaultdamage);
}
	if (Cratestype[layer,i]==ItemID.IronCrate){
	spawned.aiStyle=23;
	spawned.knockBackResist=0.99f;
	spawned.lifeMax=(int)(cratehp*0.30);
	spawned.life=(int)(cratehp*0.30);
	spawned.damage=(int)60*(npc.damage/defaultdamage);
}
	if (Cratestype[layer,i]==ItemID.DungeonFishingCrate || Cratestype[layer,i]==ItemID.JungleFishingCrate){
	spawned.aiStyle=Cratestype[layer,i]==ItemID.DungeonFishingCrate ? 56 : 86;
	spawned.knockBackResist=Cratestype[layer,i]==ItemID.DungeonFishingCrate ? 0.92f : 0.96f;
	spawned.lifeMax=Cratestype[layer,i]==ItemID.DungeonFishingCrate ? (int)(cratehp*0.4) : (int)(cratehp*0.30);
	spawned.life=Cratestype[layer,i]==ItemID.DungeonFishingCrate ? (int)(cratehp*0.4) : (int)(cratehp*0.30);
	spawned.damage=(Cratestype[layer,i]==ItemID.DungeonFishingCrate ? (int)(80) : (int)(80))*((int)npc.damage/defaultdamage);
}
	if (Cratestype[layer,i]==ItemID.HallowedFishingCrate || Cratestype[layer,i]==evilcratetype){
	spawned.aiStyle=Cratestype[layer,i]==ItemID.HallowedFishingCrate ? 63 : 62;
	spawned.knockBackResist=Cratestype[layer,i]==ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
	spawned.lifeMax=Cratestype[layer,i]==ItemID.HallowedFishingCrate ? (int)(cratehp*0.60) : (int)(cratehp*0.45);
	spawned.life=Cratestype[layer,i]==ItemID.HallowedFishingCrate ? (int)(cratehp*0.60) : (int)(cratehp*0.45);
	spawned.damage=(Cratestype[layer,i]==ItemID.HallowedFishingCrate ? (int)(100) : (int)(40))*((int)npc.damage/defaultdamage);;
}
	if (layer==0){
	spawned.knockBackResist=Cratestype[layer,i]==ItemID.HallowedFishingCrate ? 0.92f : 0.96f;
	spawned.lifeMax=(int)(cratehp*0.65);
	spawned.life=(int)(cratehp*0.65);
}
//CratrosityCrate crate=npc.modNPC();
//CratrosityCrate crate=(CratrosityCrate)spawned.modNPC;
//crate.cratetype=Cratestype[layer,i];
}
}

			public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("MoneyMismanagement"), 250, true);
		}

public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
for (int a = 0; a < phase; a=a+1){
for (int i = 0; i < Cratesperlayer[a]; i=i+1){
Color glowingcolors1 = Main.hslToRgb((float)(npc.ai[0]/30)%1, 1f, 0.9f);
//Texture2D texture = mod.GetTexture("Items/IronSafe");
Texture2D texture = Main.itemTexture[Cratestype[a,i]];
//Main.getTexture("Terraria/Item_" + ItemID.IronCrate);
//Vector2 drawPos = npc.Center-Main.screenPosition;
Vector2 drawPos = Cratesvector[a,i] - Main.screenPosition;
spriteBatch.Draw(texture, drawPos, null, lightColor, (float)Cratesangle[a,i], new Vector2(16,16),new Vector2(1,1), SpriteEffects.None, 0f);
}}
return true;
}


	}
}

