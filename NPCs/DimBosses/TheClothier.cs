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
			npc.width = 32;
			npc.height = 32;
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
			npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
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
		private bool UpdateAI()
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

		private void DoAI()
        {

			Vector2 gothere = P.Center - npc.Center;
			float gotheredist = gothere.Length();
			gothere.Normalize();


		}

		Player P;
		float lifePercent;
		bool endReturn;

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
			endReturn = false;
			//npc.netUpdate = true;
			P = Main.player[npc.target];
			lifePercent = (float)npc.life / (float)npc.lifeMax;

			if (npc.ai[0] == 0f)
				npc.ai[0] = 1f;

			npc.dontTakeDamage = NPC.CountNPCS(NPCID.SkeletronHand) > 0;

			if (UpdateAI())
			{
				DoAI();
			}
			else
			{
				npc.aiStyle = 11;
			}
				return endReturn;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D tex2 = Main.npcTexture[NPCID.Clothier];
			Texture2D tex = Main.npcTexture[npc.type];

			int Clothcount = Main.npcFrameCount[NPCID.Clothier];
			spriteBatch.Draw(tex2, npc.Center - Main.screenPosition, null, Color.Gray * 0.50f, npc.rotation, tex.Size() / 2f, new Vector2(tex2.Width, tex2.Height/Clothcount)/2f, SpriteEffects.None, 0f);
			if (endReturn)
			spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.Gray*0.50f, npc.rotation, tex.Size() / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}
	}
}

