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

namespace SGAmod.NPCs.Wraiths
{

	public class SkeletronClothier : ModNPC
	{
		public int bossvaluetimer = 0;
		public int phase = 0;
		public int aioverridetimer=0;
		public int aiattackstatething = 0;
		public Vector2 previousgo;
		public Vector2 Startloc;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeletron?");
			Main.npcFrameCount[npc.type] = 1;
			NPCID.Sets.NeedsExpertScaling[npc.type] = true;
		}
		public override void SetDefaults()
		{
			npc.width = 64;
			npc.height = 72;
			npc.damage = 10;
			npc.defense = 0;
			npc.lifeMax = 10000;
			npc.HitSound = SoundID.NPCHit5;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.knockBackResist = 1f;
			npc.aiStyle = -1;
			npc.boss = true;
			music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Copperig");
			//music =MusicID.Boss5;
			animationType = 0;
			npc.noTileCollide = true;
			npc.noGravity = true;
			npc.value = Item.buyPrice(0, 1, 0, 0);
		}
		public override string Texture
		{
			get { return ("Terraria/NPC_" + NPCID.SkeletronHead); }
		}

		public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
		{
			npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
			npc.damage = (int)(npc.damage * 0.6f);
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.LesserRestorationPotion;
		}

		public override void NPCLoot()
		{
			//filler
		}
		public bool UpdateAI()
		{

			if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
			{
				npc.TargetClosest(false);
				Player P = Main.player[npc.target];
				if (!P.active || P.dead)
				{
					return false;
				}
			}
			return true;
		}
		public override bool PreAI()
		{
			if (Main.netMode < 1 && Main.LocalPlayer.name != "giuy")
				npc.active = false;


			npc.dontTakeDamage = false;
			if (bossvaluetimer < 1)
			{
				bossvaluetimer = 1;
				Startloc = npc.Center;
			}
			bool endreturn = true;
			//npc.netUpdate = true;
			Player P = Main.player[npc.target];
			float lifepercent = (float)npc.life / (float)npc.lifeMax;

			if (npc.ai[0] == 0f)
				npc.ai[0] = 1f;

				npc.dontTakeDamage = NPC.CountNPCS(NPCID.SkeletronHand) > 0;

			if (UpdateAI())
			{
				Vector2 gothere = P.Center - npc.Center;
				float gotheredist = gothere.Length();
				gothere.Normalize();


				if (aioverridetimer < 1)
					aiattackstatething = 0;
				npc.aiStyle = 11;
				bossvaluetimer += 1;
				aioverridetimer -= 1;
				npc.defense = npc.defDefense;

				int positivetimer = Math.Abs(aioverridetimer);

				//Spin to Win dash
				if (!npc.dontTakeDamage && phase == 1)
				{
					int offset = 700;
					int timevalue = (positivetimer - offset);
					if (positivetimer % 1600 > 0 && positivetimer % 1600 < 800)
					{
						endreturn = false;
						npc.rotation += Math.Min(timevalue % 1600, 100) /500f;
						npc.velocity /= 2f;

						if (positivetimer % 1600 > 100)
						{

							if (timevalue % 100 == 0)
								previousgo = gothere;

							if (timevalue % 100 < 70)
							{
								npc.velocity = previousgo * 30f;
							}
						}

					}
				}


				//Move to Center
				if (aiattackstatething > 0 && aioverridetimer>0)
				{
					endreturn = false;
					if (aiattackstatething < 10)
					{
						npc.velocity /= 1.5f;
						npc.rotation=-npc.velocity.X/10f;

						if (aioverridetimer < 150)
						{
							Vector2 gothere2 = Startloc - npc.Center;
							float len = gothere2.Length();
							gothere2.Normalize();
							if (len>32)
							npc.velocity += gothere2 * Math.Min(8f, len);
						}

						//Seletron arms
						if (aiattackstatething==1 && aioverridetimer == 60)
						{
							int num154 = NPC.NewNPC((int)Startloc.X+400, (int)Startloc.Y - 100, 36, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
							Main.npc[num154].ai[0] = -1f;
							Main.npc[num154].ai[1] = (float)npc.whoAmI;
							Main.npc[num154].target = npc.target;
							Main.npc[num154].netUpdate = true;
							num154 = NPC.NewNPC((int)Startloc.X - 400, (int)Startloc.Y - 100, 36, npc.whoAmI, 0f, 0f, 0f, 0f, 255);
							Main.npc[num154].ai[0] = 1f;
							Main.npc[num154].ai[1] = (float)npc.whoAmI;
							Main.npc[num154].ai[3] = 150f;
							Main.npc[num154].target = npc.target;
							Main.npc[num154].netUpdate = true;
						}
					}
				}


				if (lifepercent<0.9 && phase == 0)
				{
					phase = 1;
					npc.defense = 100;
					aiattackstatething = 1;
					aioverridetimer = 200;
				}


			}
			else
			{
				npc.aiStyle = 11;
			}
			return endreturn;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex = Main.npcTexture[npc.type];
			spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.Gray, npc.rotation, tex.Size() / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}
	}
}

