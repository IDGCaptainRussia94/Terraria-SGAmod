using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs
{
	[AutoloadBossHead]
	public class SPinky : ModNPC
	{
	int aicounter=0;
	int pushtimes=0;
	int trytorun=0;
	int phase=0;
	int father=0;
	int fatherphase=0;
	int fathercharge=-150;
	int fatherhp=0;
		float dpscap = 0;
		int generalcounter = 0;
		float drawdist = 0;
		int radpoison = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Supreme Pinky");
			Main.npcFrameCount[npc.type] = 5;
			NPCID.Sets.MustAlwaysDraw[npc.type] = true;
		}
		public override void SetDefaults()
		{
			npc.width = 16;
			npc.height = 16;
			npc.damage = 90;
			npc.defense = 60;
			npc.lifeMax = 50000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.aiStyle = 1;
			npc.netAlways = true;
			npc.boss=true;
			aiType = NPCID.BlueSlime;
			animationType = NPCID.BlueSlime;
			npc.noTileCollide = false;
			npc.noGravity = false;
			music = MusicID.Boss2;
			bossBag = mod.ItemType("SPinkyBag");
			npc.value = Item.buyPrice(1, 0, 0, 0);
		}

		public int owner
		{
			get
			{
				return (int)npc.ai[0];
			}
			set
			{
				npc.ai[0] = value;
			}
		}

		/*public override bool PreNPCLoot()
		{
			if (npc.GetType()==typeof(SPinky))
			npc.GivenName = "Surpreme Pinky...?";
			return true;
		}*/

		public override void OnHitPlayer(Player target, int damage, bool crit)
		{
			if (Main.expertMode)
			{
				if (npc.boss)
				target.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationThree").Type, 60 * 5);
				else
				target.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationTwo").Type, 60*10);

			}
		}

		public Matrix TransformationMatrix(Texture2D tex, Vector2 DrawPosition, float Rotation,Vector2 Scale, bool FlipHorizontally= false,Vector2 Origin = default(Vector2))
		{
			if (Origin == default(Vector2))
				Origin = new Vector2(tex.Width, tex.Height) / 2;

				return Matrix.CreateTranslation(-new Vector3(tex.Width * Scale.X / 2, 0, 0))
					* Matrix.CreateScale(new Vector3(FlipHorizontally ? -1 : 1, 1, 1))
					* Matrix.CreateTranslation(new Vector3(tex.Width * Scale.X / 2, 0, 0))
					* Matrix.CreateTranslation(-new Vector3(Origin, 0))
					* Matrix.CreateScale(new Vector3(Scale, 0))
					* Matrix.CreateRotationZ(Rotation)
					* Matrix.CreateTranslation(new Vector3(DrawPosition, 0));

		}

	public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 drawPos = npc.Center - Main.screenPosition;
			if (GetType()!=typeof(SPinky) || !(drawdist>0f))
				return;
			float inrc = Main.GlobalTime / 30f;

			for (int i = 0; i < 720; i += 1)
			{
				//double angle = ((1f + i / 10f)) + 2.0 * Math.PI * (i / ((double)10f));
				//double angle = (double)(1f+(inrc + (i / 10f))) + (2.0 * Math.PI) * (double)(i / (10f));
				float angle = (2f * (float)Math.PI / 720f * i)+ inrc;
				float dist = 1600f;
				Vector2 thisloc = new Vector2((float)(Math.Cos(angle) * dist), (float)(Math.Sin(angle) * dist));


				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
				ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.NegativeDye);
				//Color glowingcolors1 = Main.hslToRgb(0.6f, 0.9f, 0.9f);//(inrc + (((float)i) / 480f)) / 640f
				Color glowingcolors1 = Main.hslToRgb((float)(((float)i/720f) +Main.GlobalTime / 50) % 1, 0.9f, 0.65f);
				//shader.UseColor(glowingcolors1.R, glowingcolors1.G, glowingcolors1.B);
				DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), drawPos + thisloc + new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100)), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 512, 512)), Microsoft.Xna.Framework.Color.White, inrc*15f, new Vector2(256f, 256f), 5f, SpriteEffects.None, 0);
				//DrawData value9 = new DrawData(TextureManager.Load("Images/Misc/Perlin"), new Vector2(300f, 300f), new Microsoft.Xna.Framework.Rectangle?(new Microsoft.Xna.Framework.Rectangle(0, 0, 600, 600)), Microsoft.Xna.Framework.Color.White * 1f, npc.rotation, new Vector2(600f, 600f), 1, SpriteEffects.None, 0);
				Texture2D texture = ModContent.GetTexture("Terraria/Projectile_" + 540);

				shader.UseOpacity(0.25f);
				shader.Apply(null, new DrawData?(value9));
				//GameShaders.Misc["ForceField"].Apply(new DrawData?(value9));
				//Main.pixelShader.CurrentTechnique.Passes[0].Apply();

				spriteBatch.Draw(texture, drawPos + thisloc, null, (glowingcolors1 * 0.15f)* drawdist, npc.spriteDirection * (inrc * 0.1f), new Vector2(texture.Width / 2f, texture.Height / 2f), new Vector2(1f, 1f), SpriteEffects.None, 0f);
				//value9.Draw(Main.spriteBatch);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);


		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}
		public override void NPCLoot()
		{
			if (npc.boss){
			int expert=0;
				if (Main.expertMode){
				expert=1;
				npc.DropBossBags();
				}else{
					for (int i = 0; i <= 30; i++){
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("LunarRoyalGel"));
					}
				}
			}

			//float targetX = npc.Center.X;
			//float targetY = npc.Center.Y;
			//NPC.NewNPC((int)npc.Center.X + 13, (int)npc.Center.Y - 2, mod.NPCType("GraySlime6"));
			//NPC.NewNPC((int)npc.Center.X - 13, (int)npc.Center.Y - 2, mod.NPCType("GraySlime6"));
			Achivements.SGAAchivements.UnlockAchivement("SPinky", Main.LocalPlayer);
			if (!SGAWorld.downedSPinky)
				SGAWorld.AdvanceHellionStory();
			SGAWorld.downedSPinky=true;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.SuperHealingPotion;
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
					float speed = ((-10f));
					npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
					npc.active = false;
					if (father > 0)
					{
						Main.npc[father].active = false;
						Main.npc[father].velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);

					}

				}

			}
			else{
				if (Main.expertMode)
				P.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff("RadiationOne").Type, 1);

				if (GetType()==typeof(SPinky))
					npc.GivenName = "Supreme Pinky";
				else
					npc.GivenName = "Doppelgangers";


				npc.netUpdate = true;
npc.timeLeft=99999;
npc.active=true;

if (npc.aiStyle<0){
//Vector2 ownerloc=npc.ai[0].Center;
}

Vector2 ploc=P.Center;
Vector2 meloc=npc.Center;
float moveup=0;
int isboss=0;
if (npc.boss==true){
isboss=1;
}
pushtimes=pushtimes+1;
if (pushtimes>200+(isboss*30)){
pushtimes=-100+(Main.rand.Next(120));
moveup=Main.rand.Next(-50,200);
}
Vector2 dist=ploc-meloc;
int adder=0;
if (npc.aiStyle==19){
adder=90;
}
if (npc.aiStyle==52){
adder=60;
}
if (phase==1){
npc.velocity.Normalize();
npc.velocity=npc.velocity*(dist.Length()/300);
}if (phase==3){
npc.velocity.Normalize();
npc.velocity=npc.velocity*(dist.Length()/40);
}
if (isboss>0){
if (father>0){
fatherhp=Main.npc[father].life;
Vector2 fatherloc=Main.npc[father].Center;
fathercharge=fathercharge+1;
if (fathercharge>170){
fathercharge=0;
}
if (fathercharge>140 || fathercharge<0){
Main.npc[father].noTileCollide = true;
Main.npc[father].noGravity = true;
if (ploc.X>fatherloc.X){
Main.npc[father].velocity=new Vector2(14f,(((ploc.Y+8)-fatherloc.Y)/13)+moveup);
}else{
Main.npc[father].velocity=new Vector2(-14f,(((ploc.Y+8)-fatherloc.Y)/13)+moveup);
}
}else{
Main.npc[father].noTileCollide = true;
Main.npc[father].noGravity = true;
}

}}

				/*if ((fatherhp>0 && (Main.npc[father].ai[0]!=npc.whoAmI || Main.npc[father].active==false))){
								father=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y - 32, 50);
							Main.npc[father].life=fatherhp;
							Main.npc[father].lifeMax=(int)(npc.lifeMax*2.75);
							Main.npc[father].boss=true;
							Main.npc[father].aiStyle=87 ;
							Main.npc[father].ai[0]=npc.whoAmI;
							Main.npc[father].defense=550;
							Main.npc[father].damage=60;
				Main.NewText("<Supreme Pinky> FATHER, COME BACK!",255, 100, 255);
				}*/



				if (GetType()==typeof(SPinky))
				{
					if (NPC.CountNPCS(mod.NPCType("SPinkyClone"))+NPC.CountNPCS(NPCID.KingSlime) < 1 && npc.aiStyle != 69)
						generalcounter += 1;
					else
						generalcounter = 0;

					if (generalcounter % 300 > 200)
					{
						if (phase < 2)
						{
							/*for (int i = 0; i < itz.Count; i += 1) {
								itz[i].friendly = false;
								itz[i].hostile = true;
								itz[i].netUpdate = true;
								}*/
							int staff = generalcounter + 15;
								if (generalcounter % 20 == 0)
									Idglib.Shattershots(npc.Center + new Vector2(Main.rand.Next(-100, 100), 0), P.Center, new Vector2(0, 0), ProjectileID.DemonScythe, 50, 1f, 120, 14, false, 0, true, 220);
							if (staff % 30 == 0)
								Idglib.Shattershots(npc.Center + new Vector2(Main.rand.Next(-100, 100), 0), P.Center, new Vector2(0, 0), ProjectileID.DemonScythe, 50, 5f, 50, 3, true, 0, true, 220);

						}
					}

					if (phase > 1)
					{
						if (generalcounter % 300 > 200 && generalcounter % 10 == 0)
						{
							Vector2 here = (P.Center - npc.Center);
							here.Normalize();
							List<Projectile> itz = Idglib.Shattershots(npc.Center + (here * 1600f), P.Center, new Vector2(0, 0), ProjectileID.DemonScythe, 50, 10f, 70, 2, true, 0, true, 220);

						}

						if (generalcounter % 300 > 100 && generalcounter % 150 == 0)
						{
							Vector2 here = (P.Center - npc.Center);
							here.Normalize();
							Idglib.Shattershots(npc.Center + new Vector2(Main.rand.Next(-100, 100), 0), P.Center, new Vector2(0, 0), ProjectileID.SaucerMissile, 25, 35f, 180, 2, true, 0, false, 220);

						}

					}

				}
				else
				{

					generalcounter += 1;

					if (npc.aiStyle == 87 || npc.aiStyle == 63)
					{
						int modez = (Main.expertMode ? 1 : 2);
						if (generalcounter % ((50 + npc.aiStyle)* modez) == 0)
						{
							List<Projectile> itz = Idglib.Shattershots(npc.Center, P.Center, new Vector2(0, 0), ProjectileID.NebulaBolt, 30, 14f, 40, 2, true, 0, true, 200);

						}
					}
				}

				dpscap /= 1.05f;

				if (aicounter != 0 || drawdist>0)
				drawdist += (1f - drawdist) * 0.01f;
				if (dist.Length()>(2200-(isboss*800)) || (aicounter==0 && dist.Length()>600) || pushtimes>190-adder){
if (aicounter!=0 && isboss==1 && dist.Length()>1600){
P.AddBuff(21, 120);
P.AddBuff(160, 150);
//P.velocity=P.velocity*10;
//P.velocity=P.velocity/11;
P.mount.SetMount(4,P,false);
if (trytorun==0){
trytorun=1;
Main.NewText("<Supreme Pinky> YOU AIN`T GOING ANYWHERE",255, 100, 255);
}
}

if (ploc.X>meloc.X){
npc.velocity=new Vector2(4f,(((ploc.Y+8)-meloc.Y)/8)+moveup);
}else{
npc.velocity=new Vector2(-4f,(((ploc.Y+8)-meloc.Y)/8)+moveup);
}
npc.velocity.Normalize();
npc.velocity=npc.velocity*12;
npc.noTileCollide = true;
npc.noGravity = true;
}else{
if (npc.aiStyle!=63 && npc.aiStyle!=91 && npc.aiStyle!=23 && npc.aiStyle!=4 && npc.aiStyle!=69 && npc.aiStyle!=52 && npc.aiStyle!=19){
npc.noTileCollide = false;
npc.noGravity = false;
}}
if (npc.aiStyle==63 || npc.aiStyle==91 || npc.aiStyle==23 || npc.aiStyle==4 || npc.aiStyle==69 || npc.aiStyle==52|| npc.aiStyle==19){
npc.noTileCollide = true;
npc.noGravity = true;
}
if (npc.aiStyle==19){
if (Main.rand.Next(300)<4){
npc.velocity=new Vector2((float)(-6f+Main.rand.Next(12)),(float)(-6f+Main.rand.Next(12)));
}
}


			if (npc.boss==true){
			npc.defense=((NPC.CountNPCS(mod.NPCType("SPinkyClone")))*100)+fatherphase;
			if (((NPC.CountNPCS(mod.NPCType("SPinkyClone"))>0) && Main.rand.Next(30)<290) || fatherphase==2){
			npc.dontTakeDamage=true;
			if (fatherphase==2){
//npc.life=npc.life+20;
}
}else{
			npc.dontTakeDamage=false;
}

			if (Main.expertMode){
						if (radpoison < 5 && (P.statLifeMax2 < P.statLifeMax * 0.15) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 5;
							Main.NewText("<Supreme Pinky> ALMOST FINISHED, SOON YOU SHALL BE NOTHING BUT SLIME FOR THE PRINCESS!", 255, 100, 255);
						}
						if (radpoison < 4 && (P.statLifeMax2 < P.statLifeMax * 0.3) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 4;
							Main.NewText("<Supreme Pinky> CONVERSION AT 70%!", 255, 100, 255);
						}
						if (radpoison < 3 && (P.statLifeMax2 < P.statLifeMax * 0.5) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 3;
							Main.NewText("<Supreme Pinky> YOUR BODY MELTS, CONVERSION AT 50%!", 255, 100, 255);
						}
						if (radpoison < 2 && (P.statLifeMax2 < P.statLifeMax*0.75) && P == Main.LocalPlayer && Main.netMode != 1)
						{
							radpoison = 2;
							Main.NewText("<Supreme Pinky> THE CONVERSION PROCESS IS PROCEEDING NICELY", 255, 100, 255);
						}

			if ((Main.npc[father]==null) || father<1){
			/*if (fatherphase==1){
                Main.NewText("<Supreme Pinky> NNNOOOOOOOO!!!! I'M DONE PLAYING GAMES!",255, 100, 255);
			npc.defDamage=200;
			npc.damage=200;
			fatherphase=90;
			}*/
			if (fatherphase==0){
			fatherphase=1;
								Main.NewText("<Supreme Pinky> EXPERT HUH? YOU WILL SOON BE CONVERTED!",255, 100, 255);
				/*
                father=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y - 32, 50);
			Main.npc[father].life=(int)(npc.lifeMax*0.9);
			Main.npc[father].lifeMax=(int)(npc.lifeMax*0.9);
			Main.npc[father].boss=true;
			Main.npc[father].aiStyle=87 ;
			Main.npc[father].ai[0]=npc.whoAmI;
			Main.npc[father].defense=550;
			Main.npc[father].damage=60;*/
			}
			if (fatherphase==2){
                Main.NewText("<Supreme Pinky> FATHER NOOOOOO... waaahhhhh....",255, 100, 255);
			fatherphase=3;
			}
			}else{
			if (Main.npc[father].life<1){
			Main.npc[father].StrikeNPCNoInteraction(999999,0,0);
			father=0;
			}
			}

			if (npc.life<npc.lifeMax*0.24 && fatherphase==1){
			fatherphase=2;

            father=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y - 32, 50);
			Main.npc[father].life=(int)(npc.lifeMax*0.9);
			Main.npc[father].lifeMax=(int)(npc.lifeMax*0.9);
			Main.npc[father].boss=true;
			Main.npc[father].aiStyle=87;
			Main.npc[father].ai[0]=npc.whoAmI;
			Main.npc[father].defense=550;
			Main.npc[father].damage=60;

			Main.npc[father].defense=50;
			Main.npc[father].aiStyle=30;
							Main.npc[father].netUpdate = true;
							Main.NewText("<Supreme Pinky> OHHH, HE'S HURTING ME, HELP ME FATHER!",255, 100, 255);
			}



			}
				if (npc.life<npc.lifeMax*0.95){
				aicounter=aicounter+1;					
			if (aicounter==30){
			int newguy1=NPC.NewNPC((int)npc.Center.X - 13, (int)npc.Center.Y - 2, mod.NPCType("SPinkyClone"));					
                int newguy2=NPC.NewNPC((int)npc.Center.X - 13, (int)npc.Center.Y - 2, mod.NPCType("SPinkyClone"));
			Main.npc[newguy1].life=npc.lifeMax/8;
			Main.npc[newguy1].lifeMax=npc.lifeMax/8;
			Main.npc[newguy2].life=npc.lifeMax/8;
			Main.npc[newguy2].lifeMax=npc.lifeMax/8;
			Main.npc[newguy1].boss=false;
			Main.npc[newguy2].boss=false;
			Main.npc[newguy1].aiStyle=87;
			Main.npc[newguy2].aiStyle=87;
			Main.npc[newguy1].ai[0]=npc.whoAmI;
			Main.npc[newguy2].ai[0]=npc.whoAmI;
							Main.npc[newguy2].netUpdate = true;
							Main.npc[newguy1].netUpdate = true;
							npc.aiStyle=15;
                Main.NewText("<Supreme Pinky> REAL MEN WEAR PINK! ",255, 100, 255);
			}
			}

				if (npc.life<npc.lifeMax*0.75){
				if (aicounter<90000){
				aicounter=90000;
			int newguy3=NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));					
                int newguy4=NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy3].life=npc.lifeMax/4;
			Main.npc[newguy3].lifeMax=npc.lifeMax/4;
			Main.npc[newguy4].life=npc.lifeMax/4;
			Main.npc[newguy4].lifeMax=npc.lifeMax/4;
			Main.npc[newguy3].boss=false;
			Main.npc[newguy4].boss=false;
			Main.npc[newguy3].defense=0;
			Main.npc[newguy4].defense=0;
			Main.npc[newguy3].aiStyle=41;
			Main.npc[newguy4].aiStyle=41;
			Main.npc[newguy3].ai[0]=npc.whoAmI;
			Main.npc[newguy4].ai[0]=npc.whoAmI;
							Main.npc[newguy3].netUpdate = true;
							Main.npc[newguy4].netUpdate = true;
							npc.aiStyle=96;
			phase=1;
                Main.NewText("<Supreme Pinky> YOU DARE RESIST THE PINK!?! ",255, 100, 255);
			}
			}

				if (npc.life<npc.lifeMax*0.50 || aicounter>94050){
				if (aicounter<100000){
				aicounter=100000;			
                int newguy666=NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy666].life=npc.lifeMax/8;
			Main.npc[newguy666].lifeMax=npc.lifeMax/8;
			Main.npc[newguy666].life=npc.lifeMax/8;
			Main.npc[newguy666].lifeMax=npc.lifeMax/8;
			Main.npc[newguy666].boss=false;
			Main.npc[newguy666].defense=0;
			Main.npc[newguy666].aiStyle=97;
			Main.npc[newguy666].ai[0]=npc.whoAmI;
							Main.npc[newguy666].netUpdate = true;


							int newguy667=NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy667].life=npc.lifeMax/8;
			Main.npc[newguy667].lifeMax=npc.lifeMax/8;
			Main.npc[newguy667].life=npc.lifeMax/8;
			Main.npc[newguy667].lifeMax=npc.lifeMax/8;
			Main.npc[newguy667].boss=false;
			Main.npc[newguy667].defense=0;
			Main.npc[newguy667].aiStyle=97;
			Main.npc[newguy667].ai[0]=npc.whoAmI;
							Main.npc[newguy667].netUpdate = true;


							int newguy668=NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y - 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy668].life=npc.lifeMax/8;
			Main.npc[newguy668].lifeMax=npc.lifeMax/8;
			Main.npc[newguy668].life=npc.lifeMax/8;
			Main.npc[newguy668].lifeMax=npc.lifeMax/8;
			Main.npc[newguy668].boss=false;
			Main.npc[newguy668].defense=0;
			Main.npc[newguy668].aiStyle=97;
			Main.npc[newguy668].ai[0]=npc.whoAmI;
							Main.npc[newguy668].netUpdate = true;


							int newguy669=NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y - 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy669].life=npc.lifeMax/8;
			Main.npc[newguy669].lifeMax=npc.lifeMax/8;
			Main.npc[newguy669].life=npc.lifeMax/8;
			Main.npc[newguy669].lifeMax=npc.lifeMax/8;
			Main.npc[newguy669].boss=false;
			Main.npc[newguy669].defense=0;
			Main.npc[newguy669].aiStyle=97;
			Main.npc[newguy669].ai[0]=npc.whoAmI;
							Main.npc[newguy669].netUpdate = true;
							phase =2;
			npc.aiStyle=15;
                Main.NewText("<Supreme Pinky> YOU WILL BECOME PART OF THE PINK!!",255, 100, 255);
			}
			}

				if (npc.life<npc.lifeMax*0.30 && aicounter>100150){
				if (aicounter<150000){
				aicounter=150000;
			npc.aiStyle=96;
			npc.damage=50;
                int newguy670=NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"),npc.whoAmI,0,npc.whoAmI);
			Main.npc[newguy670].life=npc.lifeMax/5;
			Main.npc[newguy670].lifeMax=npc.lifeMax/5;
			Main.npc[newguy670].boss=false;
			Main.npc[newguy670].defense=0;
			Main.npc[newguy670].aiStyle=48;
			Main.npc[newguy670].ai[0]=npc.whoAmI;
							Main.npc[newguy670].netUpdate = true;

							int newguy671=NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"),npc.whoAmI,0,npc.whoAmI);
			Main.npc[newguy671].life=npc.lifeMax/5;
			Main.npc[newguy671].lifeMax=npc.lifeMax/5;
			Main.npc[newguy671].boss=false;
			Main.npc[newguy671].defense=0;
			Main.npc[newguy671].aiStyle=48;
			Main.npc[newguy671].ai[0]=npc.whoAmI;
							Main.npc[newguy671].netUpdate = true;
							phase =3;

                Main.NewText("<Supreme Pinky> THE PINK WILL NOT BE DENIED!",255, 100, 255);
			}
			}


				if (npc.life<npc.lifeMax*0.23 && aicounter>100150){
				if (aicounter<200000){
				aicounter=200000;
			npc.aiStyle=63;
			npc.damage=50;
                int newguy670=NPC.NewNPC((int)npc.Center.X + 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy670].life=npc.lifeMax/4;
			Main.npc[newguy670].lifeMax=npc.lifeMax;			Main.npc[newguy670].boss=false;
			Main.npc[newguy670].defense=10;
			Main.npc[newguy670].aiStyle=4;
			Main.npc[newguy670].ai[0]=npc.whoAmI;
							Main.npc[newguy670].netUpdate = true;

							int newguy671=NPC.NewNPC((int)npc.Center.X - 80, (int)npc.Center.Y + 40, mod.NPCType("SPinkyClone"));
			Main.npc[newguy671].life=npc.lifeMax/4;
			Main.npc[newguy671].lifeMax=npc.lifeMax;
			Main.npc[newguy671].boss=false;
			Main.npc[newguy671].defense=10;
			Main.npc[newguy671].aiStyle=4;
			Main.npc[newguy671].ai[0]=npc.whoAmI;
							Main.npc[newguy671].netUpdate = true;

							phase =4;
                Main.NewText("<Supreme Pinky> WE WILL NOT STOP!!",255, 100, 255);
			}
			}

			if (npc.life<npc.lifeMax*0.20 && npc.aiStyle!=69 && !npc.dontTakeDamage){
			npc.aiStyle=69;
			npc.damage=20;
			npc.ai[0]=0;
			npc.defense=300;
                Main.NewText("<Supreme Pinky> PINKY WILL NOT DIE AGAIN!",255, 100, 255);
			}



			
				

if (aicounter==90500){
Main.NewText("<Supreme Pinky> MY CHIDREN! ASSIST YOUR PRINCESS!",255, 100, 255);
}
if (aicounter==100550){
Main.NewText("<Supreme Pinky> HE'S STRONG, SLIMES TO ME!",255, 100, 255);
}
if (aicounter==200550){
Main.NewText("<Supreme Pinky> MY CHILDREN! I NEED YOU ALL!",255, 100, 255);
}

if (aicounter>90560 && aicounter<90820){
if (aicounter%18==0){
for (int i = 0; i <= 2; i++)
{
                int newguy5=NPC.NewNPC((int)npc.Center.X + 150-Main.rand.Next(300), (int)npc.Center.Y + 30, 1);
			Main.npc[newguy5].life=npc.lifeMax/80;
			Main.npc[newguy5].lifeMax=npc.lifeMax/80;
			Main.npc[newguy5].life=npc.lifeMax/80;
			Main.npc[newguy5].lifeMax=npc.lifeMax/80;
			Main.npc[newguy5].boss=false;
			Main.npc[newguy5].defense=90;
			if (aicounter>90650){
			Main.npc[newguy5].aiStyle=23;
			}else{
			Main.npc[newguy5].aiStyle=44;
			Main.npc[newguy5].knockBackResist = 0.9f;
			}
			Main.npc[newguy5].damage=50;
								Main.npc[newguy5].netUpdate = true;
							}
}


}

if (aicounter>100550 && aicounter<101550){
if (aicounter%32==0){
                int newguy55=NPC.NewNPC((int)P.Center.X + Main.rand.Next(-300,300), (int)P.Center.Y - 320, 16,npc.whoAmI);
			Main.npc[newguy55].life=npc.lifeMax/25;
			Main.npc[newguy55].lifeMax=npc.lifeMax/25;
			Main.npc[newguy55].boss=false;
			Main.npc[newguy55].defense=40;
			Main.npc[newguy55].noTileCollide = true;
			Main.npc[newguy55].noGravity = true;
			Main.npc[newguy55].aiStyle=49;
			Main.npc[newguy55].damage=55;
							Main.npc[newguy55].netUpdate = true;

							int newguy16=NPC.NewNPC((int)P.Center.X - 800, (int)P.Center.Y - 30, 1);
			Main.npc[newguy16].life=npc.lifeMax/30;
			Main.npc[newguy16].lifeMax=npc.lifeMax/30;
			Main.npc[newguy16].boss=false;
			Main.npc[newguy16].defense=50;
			Main.npc[newguy16].noTileCollide = true;
			Main.npc[newguy16].noGravity = true;
			Main.npc[newguy16].aiStyle=10;
			Main.npc[newguy16].damage=63;
							Main.npc[newguy16].netUpdate = true;

							int newguy116=NPC.NewNPC((int)P.Center.X + 800, (int)P.Center.Y - 30, 1);
			Main.npc[newguy116].life=npc.lifeMax/30;
			Main.npc[newguy116].lifeMax=npc.lifeMax/30;
			Main.npc[newguy116].boss=false;
			Main.npc[newguy116].defense=60;
			Main.npc[newguy116].noTileCollide = true;
			Main.npc[newguy116].noGravity = true;
			Main.npc[newguy116].aiStyle=10;
			Main.npc[newguy116].damage=63;
							Main.npc[newguy16].netUpdate = true;

						}
}

if (aicounter>200550 && aicounter<202550){
if (aicounter%160==0){
                int newguy5=NPC.NewNPC((int)npc.Center.X - 80, (int)P.Center.Y - 350, 71);
			Main.npc[newguy5].life=npc.lifeMax/9;
			Main.npc[newguy5].lifeMax=npc.lifeMax/9;
			Main.npc[newguy5].life=npc.lifeMax/9;
			Main.npc[newguy5].lifeMax=npc.lifeMax/9;
			Main.npc[newguy5].boss=false;
			Main.npc[newguy5].defense=70;
			Main.npc[newguy5].noTileCollide = true;
			Main.npc[newguy5].noGravity = true;
			Main.npc[newguy5].aiStyle=43;
			Main.npc[newguy5].damage=87;
							Main.npc[newguy5].netUpdate = true;
						}
					}
if (aicounter>200550){
if (aicounter%60==0){
                int newguy316=NPC.NewNPC((int)npc.Center.X + 80, (int)P.Center.Y - 350, 81,npc.whoAmI,0,npc.whoAmI);
			Main.npc[newguy316].life=npc.lifeMax/60;
			Main.npc[newguy316].lifeMax=npc.lifeMax/60;
			Main.npc[newguy316].boss=false;
			Main.npc[newguy316].defense=90;
			Main.npc[newguy316].noTileCollide = true;
			Main.npc[newguy316].noGravity = true;
			Main.npc[newguy316].aiStyle=9;
			Main.npc[newguy316].ai[3]=npc.whoAmI;
			Main.npc[newguy316].damage=63;
							Main.npc[newguy316].netUpdate = true;

						}
}




}

			if (npc.boss==false){
			npc.dontTakeDamage=false;
			if (npc.ai[0]==null || npc.life<1){
			npc.StrikeNPCNoInteraction(999999,0,0);
			npc.active=false;
			}
			if (npc.aiStyle==87){
			if (aicounter<99999 && npc.life<npc.lifeMax*0.6){
			aicounter=100000;
			int newguy1=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));					
			Main.npc[newguy1].life=(int)(npc.lifeMax*0.75);
			Main.npc[newguy1].lifeMax=(int)(npc.lifeMax*0.75);
			Main.npc[newguy1].boss=false;
			Main.npc[newguy1].aiStyle=63;
			Main.npc[newguy1].ai[0]=npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> JOIN THE PINK SIDE OF THE FORCE!",255, 100, 255);
					}

				}

			if (npc.aiStyle==48){
			if (aicounter<99999 && npc.life<npc.lifeMax*0.4){
			aicounter=100000;
			int newguy1=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));					
			Main.npc[newguy1].life=(int)(npc.lifeMax*0.50);
			Main.npc[newguy1].lifeMax=(int)(npc.lifeMax*0.50);
			Main.npc[newguy1].boss=false;
			Main.npc[newguy1].aiStyle=41;
			Main.npc[newguy1].ai[0]=npc.ai[0];
							Main.npc[newguy1].netUpdate = true;

							int newguy2=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));					
			Main.npc[newguy2].life=(int)(npc.lifeMax*0.75);
			Main.npc[newguy2].lifeMax=(int)(npc.lifeMax*0.75);
			Main.npc[newguy2].boss=false;
			Main.npc[newguy2].aiStyle=87;
			Main.npc[newguy2].ai[0]=npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> PPPIIINNNKKKK!!!",255, 100, 255);
					}

				}

			if (npc.aiStyle==4){
			if (aicounter<99999 && npc.life<npc.lifeMax*0.15){
			aicounter=100000;
			int newguy1=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));					
			Main.npc[newguy1].life=(int)(npc.lifeMax*0.50);
			Main.npc[newguy1].lifeMax=(int)(npc.lifeMax*0.50);
			Main.npc[newguy1].boss=false;
			Main.npc[newguy1].aiStyle=11;
			Main.npc[newguy1].ai[0]=npc.ai[0];
							Main.npc[newguy1].netUpdate = true;

							int newguy2=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));					
			Main.npc[newguy2].life=(int)(npc.lifeMax*0.50);
			Main.npc[newguy2].lifeMax=(int)(npc.lifeMax*0.50);
			Main.npc[newguy2].boss=false;
			Main.npc[newguy2].aiStyle=11;
			Main.npc[newguy2].ai[0]=npc.ai[0];
							Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> PINK PINK PINK PINK!",255, 100, 255);
					}

				}


			if (npc.aiStyle==41){
			if (aicounter<99999 && npc.life<npc.lifeMax*0.4){
			aicounter=100000;
			int newguy1=NPC.NewNPC((int)npc.Center.X - 0, (int)npc.Center.Y + 200, mod.NPCType("SPinkyClone"));					
			Main.npc[newguy1].life=(int)(npc.lifeMax*0.75);
			Main.npc[newguy1].lifeMax=(int)(npc.lifeMax*0.75);
			Main.npc[newguy1].boss=false;
			Main.npc[newguy1].aiStyle=38;
			Main.npc[newguy1].ai[0]=npc.ai[0];
			Main.npc[newguy1].netUpdate = true;
							Main.NewText("<Supreme Pinky> THE PINK WILL CONSUME YOU!",255, 100, 255);
					}

				}




			}


			}
		}




		public override bool CheckActive()
		{
			return npc.life<1;
		}
		public override bool CheckDead()
		{
		return true;
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			damage = (int)Math.Pow(damage, 0.75);
		}
		public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (GetType() == typeof(SPinkyClone))
			{
				damage = (int)Math.Pow(damage, 0.9);
				if (npc.aiStyle != 69)
					damage = (int)(damage / (1 + (dpscap / 750)));
				dpscap += damage;

			}
			else
			{
				damage = (int)Math.Pow(damage, 0.75);
				if (npc.aiStyle != 69)
					damage = (int)(damage / (1 + (dpscap / 500)));
				dpscap += damage;
			}
		}




	}

	[AutoloadBossHead]
	public class SPinkyClone : SPinky
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Not Supreme Pinky");
			Main.npcFrameCount[npc.type] = 5;
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			npc.netAlways = false;
			npc.boss = false;
		}

		public override void NPCLoot()
		{
			//nothing
		}

		public override string Texture
		{
			get { return ("SGAmod/NPCs/SPinky"); }
		}

		public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
		{
			//sss
		}

		/*public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			//sss
		}*/


	}


	}

