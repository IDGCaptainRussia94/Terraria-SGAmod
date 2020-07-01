using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.NPCs.Sharkvern
{
    public class AquaSurge : ModNPC
    {
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Aqua Surge");
		    Main.npcFrameCount[npc.type] = 1;
		}

	    public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 28;
            npc.damage = 26;
            npc.defense = 18; 
            npc.lifeMax = 300;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            animationType = ItemID.SoulofNight;
			npc.value = 60f;
            npc.knockBackResist = 0.5f;
            npc.aiStyle = 44;
            aiType = NPCID.Wraith; 
            Main.npcFrameCount[npc.type] = 4;
    	}

		public override void FindFrame(int frameHeight)
		{
			npc.spriteDirection = npc.direction;
	    }

       public override void AI()
        {
            npc.ai[0]++;
            Player P = Main.player[npc.target];
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(true);
            }
            npc.netUpdate = true;
			
			npc.ai[1]++;
            if (npc.ai[1]%75==0 && Main.netMode!=1) 
            {
     			float Speed = 15f;  
                Vector2 vector8 = new Vector2(npc.position.X + (npc.width / 2), npc.position.Y + (npc.height / 2));
                int damage = 35; 
                int type = mod.ProjectileType("WaterTornado");  
                Main.PlaySound(23, (int)npc.position.X, (int)npc.position.Y, 17);
                float rotation = (float)Math.Atan2(vector8.Y - (P.position.Y + (P.height * 0.5f)), vector8.X - (P.position.X + (P.width * 0.5f)));
                int num54 = Projectile.NewProjectile(vector8.X, vector8.Y, (float)((Math.Cos(rotation) * Speed) * -1), (float)((Math.Sin(rotation) * Speed) * -1), type, damage, 0f, 0);
                Main.projectile[num54].damage=(int)(npc.damage/2);
                npc.netUpdate=true;
            }

            if (npc.ai[1]%500==0 && npc.ai[1]>0 && Main.expertMode){
            List<Projectile> itz=Idglib.Shattershots(npc.Center,npc.Center+new Vector2(0,200),new Vector2(0,0),ProjectileID.WaterBolt,(int)(npc.damage),4f,180,5,false,0,true,300);
            Main.PlaySound(2, (int)npc.position.X, (int)npc.position.Y, 21);
            }

            return;
			
			for (int k = 0; k < 255; k++)
			{
				Player player = Main.player[k];
				if (!player.dead)
				{
					return;
				}
				else
				{
					npc.life = 0;
					npc.active = false;
					return;
				}
			}
            
        }

	}   
}