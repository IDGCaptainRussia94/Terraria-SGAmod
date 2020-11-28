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

	public class Cratrogeddon: Cratrosity, ISGABoss
	{
		private int pmlphase=0;
		private int pmlphasetimer=0;
		private List<int> summons=new List<int>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cratrogeddon");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}

		public override string Texture
		{
			get { return "SGAmod/NPCs/Cratrosity/TitanCrate"; }
		}

		public override string BossHeadTexture => "SGAmod/NPCs/Cratrosity/TitanCrate";

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.SuperHealingPotion;
		}

		public override bool CheckActive()
		{
			return false;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.damage = 50;
			npc.defense = 50;
			npc.lifeMax = 40000;
			npc.value = Item.buyPrice(1, 0, 0, 0);
			//music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Evoland 2 OST - Track 46 (Ceres Battle)");
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
		}

		public override void NPCLoot()
		{
			if (Main.expertMode)
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("TerrariacoCrateKeyUber"));
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MoneySign"), Main.rand.Next(40, Main.expertMode ? 85 : 65));
			Achivements.SGAAchivements.UnlockAchivement("Cratrogeddon", Main.LocalPlayer);
			if (SGAWorld.downedCratrosityPML == false)
			{
				SGAWorld.AdvanceHellionStory();
			}
			SGAWorld.downedCratrosityPML = true;
		}


		public override void OrderOfTheCrates(Player P)
		{

			if (pmlphase==2 && pmlphasetimer>-300){

nonaispin=nonaispin+0.6f;
for (int a = 0; a < phase; a=a+1){
Cratesperlayer[a]=4+(a*4);
for (int i = 0; i < Cratesperlayer[a]; i=i+1){

		float boost=250f+(a*50f);
		Vector2 gohere = ((boost*(new Vector2((float)Math.Cos(Cratesangle[a,i]), (float)Math.Sin(Cratesangle[a,i])))))+P.Center;
		Cratesangle[a,i] = (a%2==0 ? 1 : -1)*((nonaispin*0.01f)*(1f+a/3f)) + 2.0 * Math.PI * ((i / (double)Cratesperlayer[a]));
		Cratesdist[a,i]= compressvar*20f+((float)a*20f)*compressvar;

		if (a==1 && pmlphasetimer%200<150 && pmlphasetimer<2200 && pmlphasetimer>1200){

	}else{
		Cratesvector[a,i]+= (gohere-Cratesvector[a,i])/2f;
	}
		Vector2 it=new Vector2(P.Center.X-Cratesvector[a,i].X,P.Center.Y-Cratesvector[a,i].Y);
		it.Normalize();


		if (a==0 && pmlphasetimer%80==i*5 && pmlphasetimer<1100 && pmlphasetimer>0){
		Idglib.Shattershots(Cratesvector[a,i],Cratesvector[a,i]+new Vector2(it.X*((i+1)%2),it.Y*(i%2))*50,new Vector2(0,0),ProjectileID.SilverCoin, 35,(float)3,0,1,true,0,false,300);
		Idglib.Shattershots(Cratesvector[a,i],Cratesvector[a,i]+new Vector2(it.X*((i)%2),it.Y*((i+1)%2))*50,new Vector2(0,0),ProjectileID.SilverCoin,35,(float)4,0,1,true,0,false,300);
		}

		if (a==1 && pmlphasetimer%200<40 && pmlphasetimer<2200 && pmlphasetimer>1200 && pmlphasetimer%20==i){
		Idglib.Shattershots(Cratesvector[a,i],Cratesvector[a,i]+it*50,new Vector2(0,0),ProjectileID.GoldCoin, 45,(float)12,0,1,true,0,false,300);
		}

		if (a==2 && pmlphasetimer%180==i*10 && pmlphasetimer>2200 && pmlphasetimer<2800){
		Idglib.Shattershots(Cratesvector[a,i],Cratesvector[a,i]+it*50,new Vector2(0,0),ProjectileID.CopperCoin,25,(float)3,0,1,true,0,false,200);
		}

}}


			}else{
		base.OrderOfTheCrates(P);

	}
		}

		public override void CrateRelease(int phase)
		{
		base.CrateRelease(phase);
		}

		public override void FalseDeath(int phase)
		{
		pmlphase=pmlphase+1;
		pmlphasetimer=1100;
		if (pmlphase==2){pmlphasetimer=3000;}
		Idglib.Chat("Impressive, but not good enough",144, 79, 16);
			if (pmlphase == 1)
			{
				summons.Insert(0, mod.NPCType("CratrosityCrateOfSlowing"));
			}
			if (pmlphase == 2)
			{
				summons.Insert(0, mod.NPCType("CratrosityCrateOfSlowing"));
				summons.Insert(0, mod.NPCType("CratrosityCrateOfPoisoned"));
			}
			if (pmlphase == 3)
			{
				summons.Insert(0, mod.NPCType("CratrosityCrateOfWitheredArmor"));
				summons.Insert(0, mod.NPCType("CratrosityCrateOfWitheredWeapon"));
			}
			if (pmlphase > 3)
			{
				summons.Insert(0, mod.NPCType("CratrosityCrateOfPoisoned"));
				summons.Insert(0, mod.NPCType("CratrosityCrateOfSlowing"));
				summons.Insert(0, mod.NPCType("CratrosityCrateOfWitheredArmor"));
				summons.Insert(0, mod.NPCType("CratrosityCrateOfWitheredWeapon"));
			}
			for (int ii = 0; ii < summons.Count; ii += 1)
			{
				int spawnedint = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, summons[0]); summons.RemoveAt(0);
				NPC him = Main.npc[spawnedint];
				him.life = (int)(npc.life * 0.75f);
				him.lifeMax = (int)(npc.lifeMax * 0.75f);
			}
		}

		public override void AI()
		{
		Player P = Main.player[npc.target];
		Cratrosity origin = npc.modNPC as Cratrosity;
		pmlphasetimer--;
		npc.dontTakeDamage=false;
		if (pmlphasetimer > 0) {
			npc.localAI[0] = 5;

		//phase 1
				if (pmlphase==1){
		OrderOfTheCrates(P);
		origin.compressvargoal=4f;
		origin.themode=1;
		npc.rotation=Idglib.LookAt(npc.Center,P.Center);
		npc.dontTakeDamage=true;
		npc.velocity=(npc.velocity*0.97f);
		if (pmlphasetimer<1000){
		Vector2 it=new Vector2(P.Center.X-npc.Center.X,P.Center.Y-npc.Center.Y);
		it.Normalize();
		if (pmlphasetimer%120==0){
		npc.velocity=(it*(30f-pmlphasetimer*0.02f));
		}
		if (pmlphasetimer%120<60 && pmlphasetimer%20==0){
		Idglib.Shattershots(npc.Center,npc.Center+it*50,new Vector2(0,0),ProjectileID.NanoBullet,40,(float)6,80,3,true,0,false,600);
		Idglib.PlaySound(13,npc.Center, 0);
		}
		}
		}
		//phase 2
		if (pmlphase==2){
		npc.dontTakeDamage=true;
		OrderOfTheCrates(P);
		npc.velocity=(npc.velocity*0.77f);
		Vector2 it=new Vector2(P.Center.X-npc.Center.X,P.Center.Y-npc.Center.Y);
		it.Normalize();
		npc.velocity+=it*0.3f;
		npc.Opacity+=(0.1f-npc.Opacity)/30f;
	}

				//phase 3
				if (pmlphase==3){
					npc.ai[0] = 0;

					if (pmlphasetimer > 1000)
					{
						int spawnedint = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CratrosityNight"));
						spawnedint = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("CratrosityLight"));
						pmlphasetimer = 105;
						Idglib.Chat("Give in to Temptation!", 144, 79, 16);
					}

					if (NPC.CountNPCS(mod.NPCType("Cratrosity"))>0)
					{
					pmlphasetimer = 100;


					}

							npc.dontTakeDamage=true;
		base.OrderOfTheCrates(P);
		npc.velocity=(npc.velocity*0.77f);
		Vector2 it=new Vector2(P.Center.X-npc.Center.X,P.Center.Y-npc.Center.Y);
		it.Normalize();
		npc.velocity+=it*0.3f;
		npc.Opacity+=(0.1f-npc.Opacity)/30f;


	}

				//phase 4
				if (pmlphase > 3)
				{
					if (pmlphasetimer < 300)
						npc.dontTakeDamage = false;

					npc.velocity /= 1.4f;
					if (pmlphasetimer > 1000)
					{
						pmlphasetimer = 400;
					}

				}


			}
			else{
		npc.rotation=npc.rotation*0.85f;
		npc.Opacity += (1f - npc.Opacity) / 30f;
		base.AI();
		}


			if (phase < 1)
			{
				int val;
				val = (int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.WoodenCrate.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.IronCrate.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.GoldenCrate.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.DungeonFishingCrate.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.JungleFishingCrate.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + evilcratetype.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.HallowedFishingCrate.ToString()))) +
(int)(NPC.CountNPCS(mod.NPCType("CratrosityCrate" + ItemID.FloatingIslandFishingCrate.ToString())));
				val += NPC.CountNPCS(mod.NPCType("CratrosityCrateOfWitheredWeapon")) +
					NPC.CountNPCS(mod.NPCType("CratrosityCrateOfWitheredArmor")) +
					NPC.CountNPCS(mod.NPCType("CratrosityCrateOfPoisoned")) +
					NPC.CountNPCS(mod.NPCType("CratrosityCrateOfSlowing"));
				if (val > 0)
					npc.dontTakeDamage = true;
			}



		}
	


	}


	public class CratrosityCrateOfWitheredWeapon : CratrosityCrateOfSlowing
	{
		protected override int BuffType => BuffID.WitheredWeapon;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Crate Of Withered Weapon");
			Main.npcFrameCount[npc.type] = 1;
		}
	}
	public class CratrosityCrateOfWitheredArmor : CratrosityCrateOfSlowing
	{
		protected override int BuffType => BuffID.WitheredArmor;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Crate Of Withered Armor");
			Main.npcFrameCount[npc.type] = 1;
		}
	}

	public class CratrosityCrateOfPoisoned : CratrosityCrateOfSlowing
	{
		protected override int BuffType => BuffID.Venom;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Crate Of Venom");
			Main.npcFrameCount[npc.type] = 1;
		}
	}

	public class CratrosityCrateOfSlowing : CratrosityCrate
	{
		protected virtual int BuffType => BuffID.Slow;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crate Of Slowing");
			Main.npcFrameCount[npc.type] = 1;
			npc.life = 150000;
			npc.lifeMax = 150000;
		}


		public override void NPCLoot()
		{
			return;
		}

		public override string Texture
		{
			get { return "Terraria/Buff_" + BuffType; }
		}


		public override void AI()
		{
			base.AI();
			int npctype = mod.NPCType("Cratrogeddon");
			if (Hellion.Hellion.GetHellion()!=null)
			npctype = mod.NPCType("Hellion");
			if (NPC.CountNPCS(npctype) > 0)
			{

				for (int i = 0; i <= Main.maxPlayers; i++)
				{
					Player thatplayer = Main.player[i];
					if (thatplayer.active && !thatplayer.dead)
					{
					thatplayer.AddBuff(BuffType, 2);
					}
				}

				NPC myowner = Main.npc[NPC.FindFirstNPC(npctype)];
				npc.ai[0] += Main.rand.Next(0, 4);
				npc.netUpdate = true;
				npc.velocity = npc.velocity * 0.95f;
				if (myowner.ai[0] % 350 > 250) { npc.velocity = npc.velocity * 0.45f; }
				if (myowner.ai[0] % 150 == 140)
				{
					Player P = Main.player[myowner.target];
					List<Projectile> itz = Idglib.Shattershots(npc.Center, P.position, new Vector2(P.width, P.height), ProjectileID.PlatinumCoin, 45, 8, 0, 1, true, 0, false, 220);
					itz[0].aiStyle = 5;
				}
			}
			else
			{
				npc.active = false;

			}

		}

	}


}

