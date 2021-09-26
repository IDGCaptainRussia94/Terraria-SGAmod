using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

//This file houses all the "Follow me" worm parts of the boss, I have GREATLY reduced the file size by parenting them

namespace SGAmod.NPCs.Sharkvern
{    
    public class SharkvernBase : ModNPC
    {
        public Color sharkGlowColor = Color.Transparent;

        public override void DrawEffects(ref Color drawColor)
        {
            sharkGlowColor = drawColor;
            base.DrawEffects(ref drawColor);
        }

        public override bool Autoload(ref string name)
        {
            return GetType() != typeof(SharkvernBase);
        }
    }

    public class SharkvernTail : SharkvernBase
    {

    public Vector2 localdist;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sharkvern");
		}
		
		public override void SetDefaults()
        {
            npc.width = 52;     
            npc.height = 66;    
            npc.damage = 25;
            npc.defense = 5;
            npc.lifeMax = 27000;
            npc.knockBackResist = 0.0f;
            npc.behindTiles = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.chaseable = true;
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            Player player = Main.player[projectile.owner];
            if (projectile.penetrate!=1 && (!SGAprojectile.IsTrueMelee(projectile, player)))
            damage = (int)(damage * (GetType() == typeof(SharkvernTail) ? 1f : 0.75f));
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(0, 0), npc.velocity / 2f, mod.GetGoreSlot("Gores/Sharkvern_tail_gib"), 1f);
            }
        }

        public virtual void KeepUpright(float dirX, float dirY)
        {
        localdist=new Vector2(dirX,dirY);
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)npc.ai[3]].active;
        }
        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return !npc.dontTakeDamage;
        }

        public static void DoDust(NPC npc)
        {
            float devider = Main.rand.NextFloat(0,1f);
            float angle = MathHelper.TwoPi * devider;
            Vector2 thecenter = new Vector2((float)((Math.Cos(angle) * 80)), (float)((Math.Sin(angle) * 80)));
            thecenter = thecenter.RotatedByRandom(MathHelper.ToRadians(20));
            int DustID2 = Dust.NewDust(npc.Center + (thecenter * 2.5f), 0, 0, SGAmod.Instance.DustType("TornadoDust"), thecenter.X * 0.8f, thecenter.X * 0.8f, 255-(int)(npc.Opacity*255f), default(Color), 1.5f);
            Main.dust[DustID2].noGravity = true;
            Main.dust[DustID2].velocity = new Vector2(thecenter.X * 0.2f, thecenter.Y * 0.2f) * -1f;

        }

        public override bool PreAI()
        {

            npc.Opacity = MathHelper.Clamp(npc.Opacity + (npc.dontTakeDamage ? -0.01f : 0.02f), 0.2f, 1f);

            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                npc.TargetClosest(true);
            if (!Main.npc[(int)npc.ai[3]].active){
            npc.timeLeft = 0;
            npc.active=false;
          }else{npc.timeLeft=500;}

            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[1]].active)
                {
                    npc.life = 0;
                    npc.HitEffect(0, 10.0);
                    npc.active = false;
                }
            }

            npc.dontTakeDamage = Main.npc[(int)npc.ai[1]].dontTakeDamage;
            if (npc.dontTakeDamage)
                DoDust(npc);

            if (npc.ai[1] < (double)Main.npc.Length)
            {
               
                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);
               
                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                KeepUpright(dirX,dirY);
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - (float)npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;
            }
            return false;
        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D texture = Main.npcTexture[npc.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            //Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor*npc.Opacity, npc.rotation, origin, npc.scale,localdist.X>0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;   
        }
    }


    public class SharkvernBody : SharkvernTail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkvern");
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Sharkvern/SharkvernBody2"); }
        }

        public override void SetDefaults()
        {
            npc.width = 52;
            npc.height = 48;
            npc.damage = 36;
            npc.defense = 25;
            npc.lifeMax = 27000;
            npc.knockBackResist = 0.0f;
            npc.behindTiles = true;
            npc.noTileCollide = true;
            npc.netAlways = true;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.HitSound = SoundID.NPCHit1;
            npc.chaseable = false;
        }

        public override bool PreAI()
        {
            base.PreAI();
            return false;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                if (GetType() == typeof(SharkvernBody))
                    Gore.NewGore(npc.Center + new Vector2(0, 0), npc.velocity / 2f, mod.GetGoreSlot("Gores/Sharkvern_body_gib"), 1f);

                if (GetType() == typeof(SharkvernNeck))
                {
                    Gore.NewGore(npc.Center + new Vector2(16, 0).RotatedBy(npc.rotation), npc.velocity / 2f, mod.GetGoreSlot("Gores/Sharkvern_body_gib"), 1f);
                    Gore.NewGore(npc.Center + new Vector2(16, 0).RotatedBy(npc.rotation), npc.velocity / 2f, mod.GetGoreSlot("Gores/Sharkvern_fin_gib"), 1f);
                }
                if (GetType() == typeof(SharkvernBody) || GetType() == typeof(SharkvernBody3))
                    Gore.NewGore(npc.Center + new Vector2(0, 0), npc.velocity / 2f, mod.GetGoreSlot("Gores/Sharkvern_body_gib"), 1f);
            }

            if (npc.life > 0 && Main.netMode != 1 && Main.rand.Next(1) == 0)
            {
                SharkvernHead jawsbrain = Main.npc[(int)npc.ai[3]].modNPC as SharkvernHead;
                float percent = Main.npc[(int)npc.ai[3]].life;
                float percent2 = Main.npc[(int)npc.ai[3]].lifeMax;
                if (jawsbrain.sergedout < 1 && (percent / percent2) < 0.8f)
                {
                    jawsbrain.sergedout = (int)(60f * (8f + ((percent / percent2) * (Main.expertMode ? 15f : 25f))));
                    int randomSpawn = Main.rand.Next(0);
                    if (randomSpawn == 0)
                    {
                        randomSpawn = mod.NPCType("AquaSurge");
                    }

                    int num660 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)(npc.position.Y + (float)npc.height), randomSpawn, 0, 0f, 0f, 0f, 0f, 255);
                }
            }
        }
    }


    public class SharkvernBody2 : SharkvernBody
    {

                public override string Texture
        {
            get { return ("SGAmod/NPCs/Sharkvern/SharkvernBody3"); }
        }
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 52;             
            npc.height = 54;           
            npc.damage = 36;
            npc.defense = 45;
            npc.lifeMax = 27000;
        }

    }

        public class SharkvernBody3 : SharkvernBody
    {

                public override string Texture
        {
            get { return("SGAmod/NPCs/Sharkvern/SharkvernBody4"); }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 52;             
            npc.height = 52;           
            npc.damage = 36;
            npc.defense = 45;
            npc.lifeMax = 27000;
        }

    }


    public class SharkvernNeck : SharkvernBody
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Sharkvern");
        }

        public override string Texture
        {
            get { return("SGAmod/NPCs/Sharkvern/SharkvernBody1"); }
        }
        
        public override void SetDefaults()
        {
            base.SetDefaults();
            npc.width = 52;             
            npc.height = 56;           
            npc.damage = 36;
            npc.defense = 45;
            npc.lifeMax = 27000;
        }

        public override bool PreAI()
        {
        base.PreAI();
            if (npc.ai[0] % 600 == 3)  //Npc spawn rate
            {
            //NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("DarkProbe"));  //NPC name
            }
            //No idea what npc was was meant to be, :/ -IDG
            //trying to spawn non-existant stuff can corrupt saves, this is here incase it's used

           return false;
        }


    }



}
