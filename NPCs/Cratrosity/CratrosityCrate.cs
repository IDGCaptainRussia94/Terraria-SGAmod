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
	public class CratrosityCrate : ModNPC
	{
		public int counter = 0;
		private int realcounter = 0;
		public int cratetype = ItemID.WoodenCrate;
		public Vector2 apointzz = new Vector2(0, 0);
		protected virtual int CrateIndex => ItemID.WoodenCrate;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Servent of Microtransactions");
			Main.npcFrameCount[npc.type] = 1;
		}
		public override void NPCLoot()
		{
			if (Main.rand.Next(0, 4) < 1 || cratetype > 2999)
			{
				Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, CrateIndex);
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
			npc.lifeMax = 7500;
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
			npc.timeLeft = 900;
			realcounter += 1;
			if (apointzz == new Vector2(0, 0))
			{
				apointzz = new Vector2(Main.rand.Next(-100, 100), Main.rand.Next(-100, 100));
			}
			counter = counter + Main.rand.Next(-3, 15);
			int npctype = mod.NPCType("Cratrosity");
			int npctype2 = mod.NPCType("Cratrogeddon");
			int npctype3 = mod.NPCType("Hellion");
			if (NPC.CountNPCS(npctype2) > 0) { npctype = mod.NPCType("Cratrogeddon"); }
			if (Hellion.Hellion.GetHellion() != null) { npctype = Hellion.Hellion.GetHellion().npc.type; }
			if (NPC.CountNPCS(npctype) > 0)
			{
				NPC myowner = Main.npc[NPC.FindFirstNPC(npctype)];
				if (counter % 600 < 450)
				{
					npc.velocity = (npc.velocity + ((myowner.Center + apointzz - (npc.position)) * 0.02f) * 0.035f) * 0.99f;
				}

			}
			else { npc.active = false; }

		}
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return realcounter>100;
        }

        public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("Microtransactions"), 200, true);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Texture2D mainTex = Main.itemTexture[CrateIndex];
			if (GetType() == typeof(CratrosityCrateOfSlowing) || GetType().IsSubclassOf(typeof(CratrosityCrateOfSlowing)))
				mainTex = Main.npcTexture[npc.type];
			//if (GetType() == typeof(CratrosityCrateDankCrate))
			//mainTex = ModContent.GetTexture(Texture);

			Main.spriteBatch.Draw(mainTex, npc.Center - Main.screenPosition, null, drawColor, 0, mainTex.Size()/2f, npc.scale, default, 0);
			return false;
		}


    }
}

