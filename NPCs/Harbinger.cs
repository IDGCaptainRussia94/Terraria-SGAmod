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

namespace SGAmod.NPCs
{
	public class Harbinger : ModNPC
	{
		int oldtype=0;
		int [] orbitors=new int[20];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Doom Harbinger");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void SetDefaults()
		{
			npc.width = 24;
			npc.height = 24;
			npc.damage = 0;
			npc.defense = 50;
			npc.lifeMax = 30000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0.2f;
			npc.aiStyle = -1;
			npc.boss=true;
			music=MusicID.Boss5;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 50, 0, 0);
		}

        public override void BossLoot(ref string name, ref int potionType)
        {
        potionType=ItemID.GreaterHealingPotion;
        }	

		public override string Texture
		{
			get { return("SGAmod/NPCs/TPD");}
		}	

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return true;
		}

		public override void NPCLoot()
		{

			if (Math.Abs(npc.ai[2]) > 200)
			{
				if (npc.ai[0]>6)
				{
					npc.type = oldtype;
					NPC myguy = Main.npc[(int)npc.ai[1]];
					myguy.active = false;
					Achivements.SGAAchivements.UnlockAchivement("Harbinger", Main.LocalPlayer);
					if (SGAWorld.downedHarbinger == false)
					{
						Idglib.Chat("Your end is nigh...", 15, 15, 150);
						Idglib.Chat("Robbed figures have been seen near the dungeon.", 20, 20, 125);
						SGAWorld.downedHarbinger = true;
					}
				}
				else
				{
				npc.boss = false;

				}

			}
			else
			{
				npc.boss = false;
				Idglib.Chat("You are not ready for this...", 15, 15, 150);
				for (int i = 180; i <= 361; i+=180)
				{
					int harbinger2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, npc.type);
					Main.npc[harbinger2].ai[2] = i;
					Main.npc[harbinger2].netUpdate = true;
				}

			}
        }

		public override void AI()
		{
			if (oldtype<1){
			oldtype=npc.type;
			}
			npc.netUpdate = true;
						Player P = Main.player[npc.target];
			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active || Main.dayTime)
			{
				npc.TargetClosest(false);
				P = Main.player[npc.target];
				if (!P.active || P.dead || Main.dayTime)
				{
					npc.active=false;
					Main.npc[(int)npc.ai[1]].active=false;
				}
				}else{

				Double angleval=MathHelper.ToRadians(npc.ai[2]);
				Vector2 itt = P.Center+new Vector2((float)Math.Cos(angleval), (float)Math.Sin(angleval))*300f;

				if (Math.Abs(npc.ai[2]) > 0)
				{
					npc.ai[2] += 1f;
					npc.GivenName = "Doom Harbingers";
				float speedz=Main.npc[(int)npc.ai[1]].velocity.Length();
				Vector2 poz = Main.npc[(int)npc.ai[1]].position;
				if (NPC.CountNPCS(npc.type) > 1)
				Main.npc[(int)npc.ai[1]].position += (((itt - poz) / 10f)*(speedz/30f));


				}
					



			npc.ai[0]+=1;
			if (npc.ai[0]==2){
			//npc.ai[2]=NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, NPCID.MoonLordHead);
			//Main.npc[(int)npc.ai[2]].aiStyle=-1;
			//Main.npc[(int)npc.ai[2]].timeLeft=999999;
			//Main.npc[(int)npc.ai[2]].dontTakeDamage=true;
			//Main.npc[(int)npc.ai[2]].boss=false;
			npc.type=NPCID.MoonLordCore;
			npc.ai[1]=NPC.NewNPC((int)P.Center.X, (int)P.Center.Y-200, NPCID.MoonLordFreeEye);
			Main.npc[(int)npc.ai[1]].ai[3]=npc.whoAmI;
			Main.npc[(int)npc.ai[1]].lifeMax=npc.lifeMax;
			Main.npc[(int)npc.ai[1]].life=npc.lifeMax;
			Main.npc[(int)npc.ai[1]].defense = npc.defense;
					Main.npc[(int)npc.ai[1]].defDefense = npc.defDefense;
					Main.npc[(int)npc.ai[1]].netUpdate = true;
			}
			//Main.npc[(int)npc.ai[1]].active=true;
			//Main.npc[(int)npc.ai[1]].aiStyle=1;
			NPC myguy=Main.npc[(int)npc.ai[1]];
			if (myguy.active==false){
			npc.type=oldtype;
			npc.StrikeNPCNoInteraction(9999, 0f, 0, false, false, false);
			}else{
			if (npc.ai[0]>1){
			if (npc.life>(int)(npc.lifeMax/1.5) || NPC.CountNPCS(npc.type) > 1)
			npc.ai[0]=Math.Min(npc.ai[0]+1,5);
			if (npc.ai[0]>60){
							if (npc.ai[0] == 65)
							{
								Idglib.Chat(npc.ai[2] > 0 ? "VERY Impressive, you just might stand a chance..." : "Most impressive child...", 15, 15, 150);
								if (npc.ai[2] > 0)
								{
									myguy.life = (int)(npc.lifeMax / 1.55);
								}
							}

				if (npc.ai[0]%600==62){
				for (int i = 0; i < 5; i++){
				int newb=Projectile.NewProjectile(npc.Center,npc.Center,ProjectileID.PhantasmalSphere,60,5,Main.myPlayer, 0f, (float)npc.whoAmI);
				orbitors[i]=newb;
				Main.projectile[orbitors[i]].timeLeft=1000;
				}
			}


				if (npc.ai[0]>65){
				for (int i = 0; i < 5; i++){
				if (orbitors[i]>0 && Main.projectile[orbitors[i]].active){
					if (orbitors[i]>0 && Main.projectile[orbitors[i]]!=null && Main.projectile[orbitors[i]].timeLeft>120){
					double angle=(npc.ai[0]/5)+ 2.0* Math.PI * (i / ((double)5f));
					float timeleft=Main.projectile[orbitors[i]].timeLeft;
				Main.projectile[orbitors[i]].Center=npc.Center+new Vector2((float)(Math.Cos(angle) * (150f-(150f/(timeleft-1450f)))),(float)(Math.Sin(angle) * (150f-(150f/(timeleft-1450f)))));
				Main.projectile[orbitors[i]].velocity=new Vector2((float)(Math.Cos(angle) * 8f),(float)(Math.Sin(angle) * 8f));
				}
				}
				}
			}


							if (npc.ai[0] % 600 == 62 && npc.ai[2] > 5)
							{
								for (int i = 6; i <= 10; i++)
								{
									int newb = Projectile.NewProjectile(npc.Center, npc.Center, ProjectileID.PhantasmalSphere, 60, 5, Main.myPlayer, 0f, (float)npc.whoAmI);
									orbitors[i] = newb;
									Main.projectile[orbitors[i]].timeLeft = 1000;
								}
							}

							if (npc.ai[0] > 65 && npc.ai[2] > 5)
							{
								for (int i = 6; i <= 10; i++)
								{
									if (orbitors[i] > 0 && Main.projectile[orbitors[i]].active)
									{
										if (orbitors[i] > 0 && Main.projectile[orbitors[i]] != null && Main.projectile[orbitors[i]].timeLeft > 120)
										{
											double angle = (-npc.ai[0] / 45f) + 2.0 * Math.PI * (i / ((double)5f));
											float timeleft = Main.projectile[orbitors[i]].timeLeft;
											Main.projectile[orbitors[i]].Center = npc.Center + new Vector2((float)(Math.Cos(angle) * (550f - (550f / (timeleft - 1450f)))), (float)(Math.Sin(angle) * (550f - (550f / (timeleft - 1450f)))));
											Main.projectile[orbitors[i]].velocity = new Vector2((float)(Math.Cos(angle) * 24f), (float)(Math.Sin(angle) * 24f));
										}
									}
								}
							}


							for (int k = 0; k < Main.maxPlayers; k++)
			{
				Player player = Main.player[k];
				if (player!=null && player.active)
				{
                player.AddBuff(BuffID.MoonLeech, 30, true); 
				}
			}
		}


				npc.position=myguy.Center;
				npc.life=System.Math.Max(myguy.life,200);
				myguy.dontTakeDamage=false;
				myguy.aiStyle=81;
				//myguy.Name="Doom Harbinger";
			}
			npc.timeLeft=30;
		}




		}

		}




		public override bool CanHitPlayer(Player ply, ref int cooldownSlot){
			return false;
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
		return false;
		}


public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{
return false;
}


	}
}

