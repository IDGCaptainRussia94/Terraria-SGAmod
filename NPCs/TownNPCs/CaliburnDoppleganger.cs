/*using Microsoft.Xna.Framework;
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


	public class CaliburnDopplegangerBoss : ModNPC
	{
		public override void SetStaticDefaults()
		{

			DisplayName.SetDefault("Caliburn Boss");
			Main.npcFrameCount[npc.type] = 1;
		}

		public override void SetDefaults()
		{
			npc.CloneDefaults(NPCID.MeteorHead);
			npc.friendly = false;
			npc.aiStyle = -1;
			npc.width = 32;
			npc.height = 32;
			npc.lifeMax = 100000;
			npc.defense = 999;
			npc.damage = 1;
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			return false;
		}

		public override bool? CanBeHitByItem(Player player, Item item)
		{
			return false;
		}

		public override bool? CanBeHitByProjectile(Projectile projectile)
		{
			return false;
		}

		public override void AI()
		{
			npc.TargetClosest(false);
			Player ply = Main.player[npc.target];

			if (npc.ai[1] == 0)
			{
				npc.ai[1]=NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y,NPCID.Guide)+1;
				Main.npc[(int)npc.ai[1] - 1].friendly = false;

			}

			if (npc.target < 0 || npc.target == 255 || ply.dead || !ply.active)
			{
				npc.active = false;
				Main.npc[(int)npc.ai[1]-1].active = false;
			}
			else
			{
				NPC myguy = Main.npc[(int)npc.ai[1] - 1];
				if (!myguy.active)
				{
					npc.StrikeNPCNoInteraction(99999999, 0, 0);
				}
				else
				{
					npc.Center = ply.Center;
				}
			}

		}

		public override string Texture
		{
			get
			{
				return ("Terraria/npc_" + 1);
			}
		}

	}

		public class CaliburnDoppleganger : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Caliburn Guardian");
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Guide];
		}

		public override string Texture
		{
			get
			{
				return ("Terraria/npc_"+1);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			int framecount = Main.npcFrameCount[NPCID.Guide];
				Texture2D tex = Main.npcTexture[npc.type];
				Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / framecount) / 2f;
				Vector2 drawPos = ((npc.Center - Main.screenPosition)) + new Vector2(0f, -12f);
				Color color = drawColor;
				spriteBatch.Draw(tex, drawPos, npc.frame, color, 0, drawOrigin, npc.scale, npc.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0f);

			return false;
			}

			public override void SetDefaults()
        	{
			npc.CloneDefaults(NPCID.Guide);
            	npc.lifeMax = 800;
            	npc.defense = 22;
            	npc.damage = 58;
				npc.scale = 1f;
				npc.boss = true;
            	npc.width = 36;
            	npc.height = 26;
			npc.townNPC = false;
			npc.friendly = false;
			npc.knockBackResist = 0.4f;
			npc.buffImmune[BuffID.OnFire] = true;
            	npc.npcSlots = 0.1f;
            	npc.netAlways = true;
            	npc.HitSound = SoundID.NPCHit1;
            	npc.DeathSound = SoundID.NPCDeath1;
            	npc.value = Item.buyPrice(0, 1);
        	}

		public override void AI()
		{
			npc.type = NPCID.Guide;
			npc.townNPC = false;
			npc.friendly = false;

			for (int k = 0; k < 1; k++)
            		{
                		int dust = Dust.NewDust(npc.position - new Vector2(8f, 8f), npc.width + 5, npc.height + 4, mod.DustType("HotDust"), 0.6f, 0.5f, 0, default(Color), 1.0f);
				Main.dust[dust].noGravity = true;				
				Main.dust[dust].velocity *= 0.0f;
            		}
		}

		public override void NPCLoot()
		{
			Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("FieryShard"), Main.rand.Next(1, 2));
		}
        	
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			player.AddBuff(mod.BuffType("ThermalBlaze"), 200, true);
		}



        }
}
 */