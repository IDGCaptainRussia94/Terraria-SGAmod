using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Utilities;
using Terraria.ID;
using Terraria.World.Generation;
using Terraria.ModLoader;
using Terraria.Graphics;
using Idglibrary;

namespace SGAmod.NPCs.Hellion
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Terraria;
    using Terraria.ID;
    using Terraria.ModLoader;


    /*public class Batthing : ModNPC
    {

        Texture2D tex;
        Texture2D armtex;
        float rotoffset = 0f;
        int aioffset = 9990;
        int noact = 0;
        int nomove = 0;
        int slowedspeed = 0;

        public Vector2 localdist;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Core Arm");
        }

        int counter = 0;
        public override void AI()
        {
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
            }
                counter += 1;

            Player ply = Main.player[npc.target];
            if (counter % 1000 < 750)
            {
                if (counter % 200 == 0)
                {
                    Vector2 angle = ply.Center - npc.Center;
                    angle.Normalize();
                    npc.velocity = angle * 12f;

                }
            }
            else
            {
                npc.velocity /= 5f;
                if (counter%70==0)
                    NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, NPCID.CaveBat, npc.whoAmI, 0f, 0f, 0f, 0f, 255);

            }
        }



    }*/

    public class HellionWorm : ModNPC
    {

        Texture2D tex;
        Texture2D armtex;
        float rotoffset = 0f;
        int aioffset = 9990;
        int noact = 0;
        int nomove = 0;
        int slowedspeed = 0;
        bool onlyonce = false;

        public Vector2 localdist;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Core Arm");
        }

        public override void SetDefaults()
        {
            npc.width = 54;
            npc.height = 54;
            npc.damage = 100;
            npc.defense = 20;
            npc.lifeMax = 27000;
            npc.knockBackResist = 0.0f;
            npc.behindTiles = true;
            npc.noTileCollide = true;
            npc.netAlways = false;
            npc.noGravity = true;
            npc.dontCountMe = true;
            npc.HitSound = SoundID.NPCHit1;
        }

        int phase = 0;

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
        }

        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("TechnoCurse"), 60*5);
        }

        public override void ModifyHitByItem(Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
            if (phase > 1)
            {
                damage = (int)(damage * 0.25f);
            }
        }

        public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit)
        {
            if (npc.ai[1] > 0 && Main.npc[(int)npc.ai[1]].active)
                damage = (int)(damage / 2f);
        }

        public override void ModifyHitByProjectile(Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {

            if (phase > 1)
            {
                    if (npc.ai[0]<600)
                        damage = (int)(damage * 0.05f);
                    damage = (int)(damage * 0.75f);
                if (projectile.penetrate != 1)
                {
                    //damage = (int)Math.Pow(damage, 0.925f);
                    damage = (int)(damage * 0.25f);
                }
            }
            if (npc.ai[1] > 0)
            {
                damage = (int)(damage * 0.25f);
                if (projectile.penetrate != 1)
                    damage = (int)(damage * 0.25f);
            }
            else
            {
                //damage = (int)(damage * 5f);

            }
        }

        public virtual void KeepUpright(float dirX, float dirY)
        {
            localdist = new Vector2(dirX, dirY);
        }

        public override bool CheckActive()
        {
            return !Main.npc[(int)npc.ai[3]].active;
        }

        public void WormHead(Player player)
        {

            if (Main.netMode != 1)
            {
                //make parts
                if (npc.ai[0] == 3)
                {

                    /*npc.realLife = npc.whoAmI;

                    int latestNPC = npc.whoAmI;

                    int randomWormLength = Main.rand.Next(15, 15);
                    for (int i = 0; i < randomWormLength; ++i)
                    {
                        int latestNPC2 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HellionWorm"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)latestNPC2].realLife = (int)npc.ai[3];
                        Main.npc[(int)latestNPC2].ai[1] = latestNPC;
                        Main.npc[(int)latestNPC2].ai[3] = npc.ai[3];
                        Main.npc[(int)latestNPC2].netUpdate = true;
                        latestNPC = latestNPC2;
                    }
                    */

                    npc.netUpdate = true;
                }

                //do ai

                Vector2 theplayerdir = player.Center - (npc.Center);
                float maxspeed = 75f;
                float friction = 0.97f;
                float acceerate = 0.4f;
                float acceeratedist = 4000f;
                bool facetoplayer = false;

                if (phase < 2)
                {

                    if (noact < 0)
                    {
                        int whento = phase < 1 ? 160 : 250;

                        if (npc.ai[0] % whento > whento-20)
                        {
                            acceerate = 4f;
                        }
                        else
                        {
                            if (phase > 0)
                                theplayerdir = player.Center - (npc.Center - ((npc.ai[2] + MathHelper.ToRadians(npc.ai[0]) * 2f).ToRotationVector2() * 200f));

                        }


                        if (npc.ai[0] % 800 > 400)
                        {
                            if (phase > 0)
                            {
                                theplayerdir = player.Center - (npc.Center + ((npc.ai[2] + MathHelper.ToRadians(npc.ai[0])).ToRotationVector2() * 1000f));
                            }
                            else
                            {
                                Vector2 aplay = -theplayerdir; theplayerdir.Normalize();
                                theplayerdir = player.Center - (npc.Center + (theplayerdir * 400f));
                            }

                            friction = 0.92f;
                            acceerate = 2.5f;
                            acceeratedist = 1500f;
                            facetoplayer = true;

                            if (npc.ai[0] % 25 == 0)
                                if (phase == 0)
                                    Idglib.Shattershots(npc.Center, npc.Center + npc.rotation.ToRotationVector2(), new Vector2(0, 0), ProjectileID.EyeLaser, 40, 12, 20 + (400 - (npc.ai[0] % 400)) / 20, 2, true, 0, false, 400);
                            if (npc.ai[0] % 125 == 0)
                                if (phase == 1)
                                    Idglib.Shattershots(npc.Center, npc.Center + npc.rotation.ToRotationVector2(), new Vector2(0, 0), ProjectileID.EyeLaser, 25, 8, 20 + (400 - (npc.ai[0] % 400)) / 20, 3, true, 0, false, 400);
                        }
                        else
                        {
                            if (npc.ai[0] % 100 == 0)
                                Idglib.Shattershots(npc.Center, npc.Center + npc.rotation.ToRotationVector2(), new Vector2(0, 0), ProjectileID.CursedFlameHostile, 25, 20, 160, Hellion.GetHellion().phase<1 ? 5 : 3, false, 0, false, 200);


                        }

                    }

                }
                else
                {
                    if (noact<1)
                    facetoplayer = true;
                    for (int k = 0; k < npc.buffTime.Length; k++)
                    {
                        if (npc.buffTime[k]>5)
                        npc.buffTime[k] -= 2;
                    }

                    acceerate = 4f;
                    acceeratedist = 150000f;
                    friction = 0.93f;


                    Vector2 thehelldir = player.Center - (Hellion.GetHellion().npc.Center);

                    NPC master = Main.npc[(int)npc.ai[3]];

                        Vector2 aplay = thehelldir;
                    aplay.Normalize();
                    float ittz = 1400f+(float)Math.Sin((npc.ai[2] * 171.9424f) + npc.ai[0]/15f)*600f;
                    float ittz2 = aioffset % 2 == 0 ? 1f : -1f;
                    float angles = (float)Math.Cos(MathHelper.ToRadians((npc.ai[2] * 351.724f) - npc.ai[0] * (ittz2*8f)))*46f;
                    Vector2 former = theplayerdir;

                    theplayerdir = (master.Center- (aplay * 500 )+ ((aplay * ittz).RotatedBy(angles))) - npc.Center;

                    if ((npc.ai[0] + aioffset * 8) % 1700 > 1280)
                    {
                        acceerate = 8f;
                        former.Normalize();
                        theplayerdir = (master.Center - ((aplay * 3500) + ((former * (ittz*2f)).RotatedBy(angles)))) - npc.Center;

                        if ((npc.ai[0] + aioffset * 6) % 1700 == 1600)
                        {

                            bool fastlaser = (npc.ai[0] + aioffset * 8) % 1700 == 1500;

                            Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
                            Vector2 where2 = player.Center - npc.Center;
                            where2.Normalize();
                            Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                            {
                                float val = current;
                                if (time < 100)
                                    val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.05f);
                                else
                                    val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.01f);

                                return val;
                            };
                            if (!fastlaser)
                            {
                                projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                                {
                                    return current;
                                };
                            }
                            Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                            {

                                Vector2 gothere333 = npc.Center;
                                Vector2 slideover = gothere333 - projpos;
                                current = slideover / 2f;

                                current /= 1.125f;

                                Vector2 speedz = current;
                                float spzzed = speedz.Length();
                                speedz.Normalize();
                                if (spzzed > 100f)
                                    current = (speedz * spzzed);

                                return current;
                            };

                            slowedspeed = 350;
                            Hellion.GetHellion().manualmovement = 50;
                            Func<float, bool> projectilepattern = (time) => (time == 20);

                            int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 200, npc.rotation, mod.ProjectileType("HellionBeam"), projectilepattern, 10f, 160, true);
                            (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                            (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                            Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                            Main.projectile[ize2].hide = true;
                            Main.projectile[ize2].netUpdate = true;

                            if ((npc.Center-player.Center).Length()<100)
                            Spawnlaserportal();

                        }

                    }
                    else
                    {

                        if ((npc.ai[0] + aioffset * 64) % 1200 > 800 && (npc.ai[0] % 3000 > 1400))
                        {
                            nomove = 60;

                            friction = 0.98f;

                            if ((npc.ai[0] + aioffset * 64) % 120 == 0)
                            {
                                Vector2 thingz = player.Center - npc.Center;
                                thingz.Normalize();
                                npc.velocity += thingz * 50f;

                            }
                            if (((npc.ai[0] + aioffset * 64) + 40) % 200 == 0)
                            {
                                Vector2 thingz = player.Center - npc.Center;
                                thingz.Normalize();
                                npc.velocity += thingz * -85f;
                                Idglib.Shattershots(npc.Center, player.Center, new Vector2(0, 0), ProjectileID.CursedFlameHostile, 40, 40, 160, 1, true, 0, false, 200);

                            }
                            former.Normalize();
                            theplayerdir = (master.Center - (aplay * 2500) + ((former * ittz).RotatedBy(angles))) - npc.Center;
                        }
                    }


                }


                if (noact > 0)
                    acceerate = 0.025f;



                float dist = theplayerdir.Length();
                theplayerdir.Normalize();
                if (nomove < 1)
                {
                    if (noact < 160 && nomove < 1)
                        npc.velocity += theplayerdir * (acceerate + (dist / acceeratedist));
                    if (slowedspeed > 0)
                        maxspeed = 2;
                    if (npc.velocity.Length() > maxspeed)
                        npc.velocity = theplayerdir * maxspeed;

                }

                npc.velocity *= friction;

                if (facetoplayer)
                npc.rotation = npc.rotation.AngleLerp((player.Center-npc.Center).ToRotation(), 0.075f);
                else
                npc.rotation = npc.rotation.AngleLerp(theplayerdir.ToRotation(), 0.075f);


            }

        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            return (nomove > 0 || phase < 2);
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            base.OnHitByItem(player, item, damage, knockback, crit);
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            base.OnHitByProjectile(projectile, damage, knockback, crit);
        }

        public void WormSegment(Player player)
        {

            if (noact>0)
                return;

            if (phase == 0)
            {

                if (npc.ai[0] % 800 <= 400)
                {

                    if (npc.ai[0] % 160 == 30 + aioffset * 2)
                    {
                        Vector2 theplayerdir = player.Center - (npc.Center);
                        Idglib.Shattershots(npc.Center, player.Center, new Vector2(0, 0), ProjectileID.ShadowFlame, 30, 25, 60, 1, true, 0, false, 150);

                    }

                }
            }

            if (((npc.ai[0]+(aioffset*210))) % 600 == 400 && npc.ai[0] % 800 < 400)
            {

                Spawnlaserportal();
            }

        }

        public void Spawnlaserportal()
        {
            Vector2 where = npc.Center;
            Vector2 wheretogo2 = new Vector2(128f + ((((-npc.ai[0] + aioffset) * 3.373475f) * 73.174f) % 96f), (npc.ai[0] + aioffset) * 0.73445f);
            Vector2 where2 = Main.player[npc.target].Center - npc.Center;
            where2.Normalize();
            Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
            {
                float val = current;
                val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.025f);

                return val;
            };
            Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
            {
                Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
                float angle = MathHelper.ToRadians(((wheretogo.Y + -time * 1.37f)));
                Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

                Vector2 gothere333 = npc.Center + instore;
                Vector2 slideover = gothere333 - projpos;
                current = slideover / 2.5f;

                current /= 1.125f;

                Vector2 speedz = current;
                float spzzed = speedz.Length();
                speedz.Normalize();
                if (spzzed > 45f)
                    current = (speedz * spzzed);

                return current;
            };
            Func<float, bool> projectilepattern = (time) => (time % 70 == 0);

            int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 30, 250, wheretogo2.Y, ProjectileID.NebulaLaser, projectilepattern, 15f, 200);
            (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
            (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
            Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
            Main.projectile[ize2].netUpdate = true;

        }

        public override void NPCLoot()
        {
            if (!Main.player[npc.target].dead && !onlyonce)
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("ByteSoul"), 1);
            onlyonce = true;
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (damage > 5000000)
            {
                NPCLoot();
            }
            else
            {
                if (tex != null)
                {
                    int size = tex.Width * tex.Height;
                    var datacolors2 = new Color[size];
                    tex.GetData(datacolors2);

                    NPC master = Main.npc[(int)npc.ai[3]];

                    int losehp = (int)Math.Pow((master.lifeMax - master.life) / master.lifeMax, 1.05);

                    for (int i = 0; i < 1 + Math.Min(((damage / 3) + losehp) * 10, 1000); i += 1)
                    {
                        int ran1 = Main.rand.Next(tex.Width);
                        int ran2 = Main.rand.Next(tex.Height);

                        if (datacolors2[ran1 * ran2].A > 0)
                            datacolors2[ran1 * ran2] = Main.hslToRgb(Main.rand.NextFloat(), 0.75f, 0.4f);

                    }

                    tex.SetData(datacolors2);
                }
            }
        }

        public override bool PreAI()
        {

            nomove -= 1;
            slowedspeed -= 1;
            if (aioffset == 9990)
            {
                aioffset = (int)npc.ai[0];
                npc.ai[0] = 0;

                Texture2D atex;

                if (npc.ai[2] > Math.PI)
                {
                    rotoffset = 90f;
                    atex = ModContent.GetTexture("SGAmod/NPCs/Sharkvern/SharkvernHead");
                }
                else
                {
                    /*int[,] spritetype = { { 223,180 },{ 224,0 },{ 516,0 },{ 67,0 },{ 24,0 },{15,0 },{819,0 },{ }


                    };*/
                    int rando = Main.rand.Next(0, 2);
                    atex = ModContent.GetTexture("Terraria/Gore_"+ Main.rand.Next(1, 800));
                    while (atex == null)
                    {
                        ModContent.GetTexture("Terraria/Gore_" + Main.rand.Next(1, 800));
                    }

                    rotoffset = Main.rand.Next(0, 360);
                    npc.scale = Main.rand.NextFloat(1.5f, 2.5f);

                }

                tex = new Texture2D(Main.graphics.GraphicsDevice, atex.Width, atex.Height);

                var datacolors2 = new Color[atex.Width*atex.Height];
                atex.GetData(datacolors2);
                tex.SetData(datacolors2);

                int width = 20;int height = 300;

                armtex = new Texture2D(Main.graphics.GraphicsDevice, width, height);
                var dataColors = new Color[width * height];


                //


                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x += 1)
                    {
                        int output = 32;

                        int inta = Main.rand.Next((int)x * -5, (int)x * 5);
                        output += inta;

                        dataColors[(int)x + y * width] = Main.hslToRgb((((y)+x)/100f)%1f, 0.75f, 0.5f);
                    }

                }

                armtex.SetData(dataColors);

            }
            noact -= 1;

            Player player = Main.player[npc.target];
            if (npc.ai[3] > 0)
                npc.realLife = (int)npc.ai[3];
            if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (npc.target < 0 || npc.target == byte.MaxValue || Main.player[npc.target].dead)
                    npc.active = false;
            }
            else
            {
                npc.ai[0] += 1;
            }
            if (!Main.npc[(int)npc.ai[3]].active)
            {
                npc.life = 0;
                npc.HitEffect(0, 10000000.0);
                npc.active = false;
                return false;
            }
            else { npc.timeLeft = 500;

                NPC owner = Main.npc[(int)npc.ai[3]];

            if (owner.life < (int)(owner.lifeMax * HellionCore.beginphase[0]) && phase==0)
                {
                    phase = 1;
                    noact = 150;
                    npc.ai[0] = (int)npc.ai[2]*350;
                    if (npc.ai[2] > 0)
                    {
                        npc.ai[1] = 0;
                    }
                }

                if (owner.life < (int)(owner.lifeMax * HellionCore.beginphase[1]) && phase == 1)
                {
                    phase = 2;
                    noact = 300;
                    npc.ai[0] = aioffset*3;

                    npc.velocity = new Vector2(Main.rand.Next(-99999,99999), Main.rand.Next(-99999, 99999));

                    npc.velocity.Normalize();
                    npc.velocity *= Main.rand.NextFloat(15f, 64f);

                    npc.ai[1] = 0;
                    npc.ai[2] = Main.rand.NextFloat(0f, MathHelper.ToRadians(360));
                    npc.netUpdate = true;
                }

            }

            if (Main.netMode != 1)
            {
                if (!Main.npc[(int)npc.ai[1]].active || Main.npc[(int)npc.ai[1]].type!=mod.NPCType("HellionWorm"))
                {
                    //npc.life = 0;
                    //npc.HitEffect(0, 10.0);
                    //npc.active = false;
                    WormHead(player);
                    return false;
                }
            }

            //If we are a worm segment: below:

            if (npc.ai[1] < (double)Main.npc.Length)
            {

                Vector2 npcCenter = new Vector2(npc.position.X + (float)npc.width * 0.5f, npc.position.Y + (float)npc.height * 0.5f);

                float dirX = Main.npc[(int)npc.ai[1]].position.X + (float)(Main.npc[(int)npc.ai[1]].width / 2) - npcCenter.X;
                float dirY = Main.npc[(int)npc.ai[1]].position.Y + (float)(Main.npc[(int)npc.ai[1]].height / 2) - npcCenter.Y;
                KeepUpright(dirX, dirY);
                npc.rotation = (float)Math.Atan2(dirY, dirX) + 1.57f;
                float length = (float)Math.Sqrt(dirX * dirX + dirY * dirY);
                float dist = (length - (float)npc.width) / length;
                float posX = dirX * dist;
                float posY = dirY * dist;
                npc.velocity = Vector2.Zero;
                npc.position.X = npc.position.X + posX;
                npc.position.Y = npc.position.Y + posY;

                WormSegment(player);
            }
            return false;

        }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {

            Idglib.coloroverride = Color.White;

            NPC myowner = Main.npc[(int)npc.ai[3]];

            if (armtex != null && phase>1 && noact<200)
            {
                int aval = noact;
                if (aval < 5)
                    aval = 5;
                if (myowner != null && myowner.active && Main.rand.Next(0,aval) <30)
                {
                    Idglib.DrawSkeletronLikeArms(spriteBatch, armtex, npc.Center, myowner.Center, 0f, 0f, MathHelper.Clamp((myowner.Center.X - npc.Center.X) * 0.02f, -1, 1));
                    Vector2 drawPos = ((Idglib.skeletronarmjointpos - Main.screenPosition)) + new Vector2(0f, 0f);
                    Texture2D texture = mod.GetTexture("NPCs/TPD");
                    spriteBatch.Draw(texture, drawPos, null, Main.DiscoColor, npc.spriteDirection + (npc.ai[0] * 0.4f), new Vector2(16, 16), new Vector2(Main.rand.Next(15, 35) / 4f, Main.rand.Next(15, 35) / 4f), SpriteEffects.None, 0f);

                }
            }

            if (tex != null)
            {
                Texture2D texture = tex;//Main.npcTexture[npc.type];
                Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
                Main.spriteBatch.Draw(texture, npc.Center - Main.screenPosition, new Rectangle?(), drawColor, npc.rotation+MathHelper.ToRadians(rotoffset), origin, npc.scale, localdist.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
            return false;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
    }


    public class HellionMonolog : ModNPC
    {

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Monologging");
            Main.npcFrameCount[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.width = 96;
            npc.height = 96;
            npc.damage = 0;
            npc.defense = 10;
            npc.lifeMax = 750000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 0;
            npc.boss = true;
            //aiType = NPCID.BlueSlime;
            //animationType = NPCID.BlueSlime;
            npc.noTileCollide = false;
            npc.noGravity = false;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Silence");
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.value = Item.buyPrice(0, 90, 0, 0);
            npc.netAlways = true;
            npc.chaseable = false;
            npc.dontTakeDamage = true;
        }

        public override bool PreAI()
        {
            Player P = Main.player[npc.target];
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(false);
                P = Main.player[npc.target];
                if (!P.active || P.dead)
                {
                    float speed = ((-10f));
                    npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
                    npc.active = false;
                }

            }
            else
            {
                npc.ai[0] += 1;
                if (npc.ai[0] > 200)
                {
                    npc.velocity += (((P.Center + new Vector2(0, -200)) - npc.Center) / 100f);
                }
                    npc.velocity /= 1.5f;

            }
            Hellion Hellinstance = new Hellion();

            if (npc.ai[0]==120)
                Hellinstance.HellionTaunt("hmmmm...");
            if (npc.ai[0] == 260)
                Hellinstance.HellionTaunt("It seems you have destroyed my most powerful creation...");
            if (npc.ai[0] == 460)
                Hellinstance.HellionTaunt("Your powerful, very powerful, maybe that's why he fled to you.");
            if (npc.ai[0] == 700)
                Hellinstance.HellionTaunt("With that kind of power, we could have been allies, But in the end...");
            if (npc.ai[0] == 1000)
                Hellinstance.HellionTaunt("I have put far too many resources into the project");
            if (npc.ai[0] == 1100)
                Hellinstance.HellionTaunt("And far too many slip ups to have let accured");
            if (npc.ai[0] == 1300)
                Hellinstance.HellionTaunt("So...");
            if (npc.ai[0] == 1500)
                Hellinstance.HellionTaunt("I think it's finally time, I ended this, myself");

            if (npc.ai[0] == 1700)
            {
                NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("Hellion"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                npc.life = 0;
                npc.active = false;
            }



            return false;
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Hellion.HellionTeleport(spriteBatch, npc.Center, 1f, 96);
            return false;
        }

    }














        public class HellionCore : Hellion
    {

        public static float[] beginphase = { 0.70f, 0.40f };
        int hellionmessages = 0;
        int repairs = 0;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hellion Core");
            Main.npcFrameCount[npc.type] = 1;
        }
        public override void SetDefaults()
        {
            npc.width = 96;
            npc.height = 96;
            npc.damage = 0;
            npc.defense = 10;
            npc.lifeMax = 750000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.value = 0f;
            npc.knockBackResist = 0f;
            npc.aiStyle = 0;
            npc.boss = true;
            //aiType = NPCID.BlueSlime;
            //animationType = NPCID.BlueSlime;
            npc.noTileCollide = false;
            npc.noGravity = false;
            music = MusicID.Boss3;
            npc.noTileCollide = true;
            npc.noGravity = true;
            //bossBag = mod.ItemType("SPinkyBag");
            npc.value = Item.buyPrice(0, 90, 0, 0);
            npc.netAlways = true;
            npc.chaseable = false;
        }

        public override void NPCLoot()
        {

            int num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("HellionMonolog"), npc.whoAmI, SGAWorld.downedHellion < 1 ? 0f : 1698f, 0f, 0f, 0f, 255);

            if (SGAWorld.downedHellion < 1)
            {
                SGAWorld.downedHellion = 1;
            }

        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + ProjectileID.Starfury); }
        }

        public override bool? CanBeHitByItem(Player player, Item item)
        {
            return false;
        }

        public override bool? CanBeHitByProjectile(Projectile projectile)
        {
            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Hellion.HellionTeleport(spriteBatch, npc.Center, 1f, 96);
            return false;
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            return;
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.SuperHealingPotion;
        }

        public override void AdvancePhases()
        {
            if (hellionmessages == 0 && npc.life < npc.lifeMax * 0.9)
            {
                HellionTaunt("PIXEL HULL BREACHED! RELEASING REPAIR WRAITHS!", 1);
                hellionmessages = 2;
                repairs += 5;
            }
            if (hellionmessages == 2 && npc.life < npc.lifeMax * 0.8)
            {
                HellionTaunt("You ain't bad! But you won't last much longer");
                hellionmessages = 3;
            }
            if (hellionmessages == 3 && npc.life < npc.lifeMax * 0.75)
            {
                HellionTaunt("HULL SUSTAINED DAMAGE, SUGGESTED COURSE OF ACTION: DODGE!", 1);
                hellionmessages = 4;
            }
            if (hellionmessages == 4 && npc.life < npc.lifeMax * 0.65)
            {
                HellionTaunt("CORRUPTION DETECTED, FURTHER REPAIRS NEEDED!", 1);
                hellionmessages = 5;
                repairs += 5;
            }
            if (hellionmessages == 5 && npc.life < npc.lifeMax * 0.6)
            {
                HellionTaunt("ALERT: TERRARIAN PROVING OVERPOWERING, CHANCES OF SUCCESS DROPPING!", 1);
                HellionTaunt("RELEASING REMAINING REPAIR WRAITHS!", 1);
                hellionmessages = 6;
                repairs += 25;
            }
            if (hellionmessages == 6 && repairs<1)
            {
                HellionTaunt("REPAIR WRAITHS DEPLETED", 1);
                hellionmessages = 7;
            }
            if (hellionmessages<20 && npc.life < npc.lifeMax * 0.15)
            {
                HellionTaunt("SYSTEMS ERROR! SYSTEMS BADLY DAMAGED: RETREAT HIGHLY SUGGESTED!",1);
                hellionmessages = 20;
            }

            if (npc.life < (int)(npc.lifeMax * HellionCore.beginphase[0]) && phase == 0)
            {
                phase = 1;
                HellionTaunt("Engauge Seperate Protocall!");
            }
            if (npc.life < (int)(npc.lifeMax * HellionCore.beginphase[1]) && phase == 1)
            {
                npc.ai[0] = 0;
                phase = 2;
                HellionTaunt("Begin Planetary Ravanger mode!");
            }
            if (phase == 2 && npc.ai[0] == 80)
                HellionTaunt("Time to meet your end!");
            if (phase==2 && npc.ai[0]==200)
                HellionTaunt("Now witness the power that ended your friend's world!");

        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.75f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }
        public override bool PreAI()
        {
            if (!Main.expertMode)
            {
                HellionTaunt("NO U, normie difficulty");
                npc.active = false;
            }
            Player P = Main.player[npc.target];
            if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(false);
                P = Main.player[npc.target];
                if (!P.active || P.dead)
                {
                    float speed = ((-10f));
                    npc.velocity = new Vector2(npc.velocity.X, npc.velocity.Y + speed);
                    npc.active = false;
                }

            }
            else
            {
                AdvancePhases();
                npc.netUpdate = true;
                npc.timeLeft = 99999;
                manualmovement -= 1;

                npc.ai[0] += 1;

                if (repairs > 0)
                {
                    if (npc.ai[0] % 200 == 0)
                    {
                        int num154 = NPC.NewNPC((int)(npc.position.X + (float)(npc.width / 2)), (int)npc.position.Y + npc.height / 2, mod.NPCType("HealingDrone"), npc.whoAmI, 0f, 0f, 0f, 0f, 255);
                        Main.npc[num154].target = npc.target;
                        Main.npc[num154].lifeMax = (int)(npc.lifeMax * 0.004);
                        Main.npc[num154].life = Main.npc[num154].lifeMax;
                        Main.npc[num154].netUpdate = true;

                        repairs -= 1;
                    }

                }


                if (npc.ai[0] == 1)
                {

                    int latestNPC = 0;
                    for (int i = 0; i < 60; ++i)
                    {
                        int npc2;

                        npc2 = NPC.NewNPC((int)npc.Center.X + Main.rand.Next(-400, 400), (int)npc.Center.Y + Main.rand.Next(-400, 400), mod.NPCType("HellionWorm"));//NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("HellionWorm"), npc.whoAmI, 0, latestNPC);
                        Main.npc[(int)npc2].realLife = npc.whoAmI;
                        Main.npc[(int)npc2].ai[3] = npc.whoAmI;
                        Main.npc[(int)npc2].ai[0] = i;
                        if (i % 20 == 0)
                            Main.npc[(int)npc2].ai[2] = ((float)Math.PI * 2f) + MathHelper.ToRadians(i * (360 / 60));
                        Main.npc[(int)npc2].ai[1] = latestNPC;
                        Main.npc[(int)npc2].netUpdate = true;
                        latestNPC = npc2;
                    }
                }


                Vector2 gothere = npc.Center+new Vector2(0,1);
                int divider = 1;
                float accelerate = 250f;


                if (phase < 2)
                {

                    for (int i = 0; i < Main.maxNPCs; i += 1)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == mod.NPCType("HellionWorm"))
                        {
                            divider += 1;
                            gothere += Main.npc[i].Center;
                        }
                    }
                    gothere /= divider;

                }
                else
                {
                    accelerate = 2500;
                    gothere = P.Center + new Vector2(600 * ((npc.ai[0] % 1400 > 700) ? 1f : -1f), 0);

                    if (npc.ai[0] < 600 && npc.ai[0]>=100)
                        npc.life = (int)Math.Min(npc.life+((float)((npc.lifeMax / 2f) * (1f-HellionCore.beginphase[1])/500f)), npc.lifeMax);

                    if (npc.ai[0] % 10 == 0 && npc.ai[0] % 1850 > 1500)
                    {

                        Vector2 where = npc.Center;
                        Vector2 wheretogo2 = new Vector2(320f+(npc.ai[0] * 3739.6792f) % 960f, (npc.ai[0] * 6834.392f) % 360f);
                        Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                        {
                            float val = current;
                                val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.15f);

                            return val;
                        };
                        Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                        {
                            Vector2 wheretogo = playerpos+new Vector2((float)Math.Cos(wheretogo2.Y), (float)Math.Sin(wheretogo2.Y))* wheretogo2.X;

                            Vector2 gothere333 = wheretogo;
                            Vector2 slideover = gothere333 - projpos;
                            current = slideover / 2f;

                            current /= 3f;

                            Vector2 speedz = current;
                            float spzzed = speedz.Length();
                            speedz.Normalize();
                            if (spzzed > 25f)
                                current = (speedz * spzzed);

                            return current;
                        };
                        Func<float, bool> projectilepattern = (time) => (time>120 && time%80==0);

                        int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 75, 200, wheretogo2.Y, ProjectileID.DesertDjinnCurse, projectilepattern, 1f, 200);
                        (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                        (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                        Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                        Main.projectile[ize2].netUpdate = true;

                    }

                    if (npc.ai[0] % 20 == 0 && npc.ai[0] % 900 > 650)
                    {

                        Vector2 where = npc.Center;
                        Vector2 wheretogo2 = P.Center + new Vector2(320f + (npc.ai[0] * 3739.6792f) % 960f, (npc.ai[0] * 6834.392f) % 360f);
                        Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                        {
                            float val = current;
                            val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.15f);

                            return val;
                        };
                        Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                        {
                            Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);

                            Vector2 gothere333 = wheretogo;
                            Vector2 slideover = gothere333 - projpos;
                            current = slideover / 5f;

                            current /= 3f;

                            Vector2 speedz = current;
                            float spzzed = speedz.Length();
                            speedz.Normalize();
                            if (spzzed > 30f)
                                current = (speedz * spzzed);

                            return current;
                        };
                        Func<float, bool> projectilepattern = (time) => (time > 120 && time %30 == 0);

                        int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 50, 240, wheretogo2.Y, ProjectileID.NebulaLaser, projectilepattern, 15f, 400);
                        (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                        (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                        Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                        Main.projectile[ize2].netUpdate = true;

                    }


                    if (npc.ai[0] % 400<30)
                    accelerate = 400;

                }

                Vector2 gogo = gothere - npc.Center;
                Vector2 gogo2 = gogo; gogo2.Normalize();

                if ((npc.ai[0] % 1200 < 900) && npc.ai[0] % 1200 != 950)
                {
                    if (manualmovement<1)
                    npc.velocity += gogo/ accelerate;

                }
                else
                {

                    if (npc.ai[0] % 1200 == 950)
                    {

                        for (int rotz = 0; rotz < 360; rotz += 360 / 3)
                        {

                            Vector2 where = npc.Center - (new Vector2(((npc.ai[0] % 40) == 0) ? 1f : -1f, 0f) * 80f);
                            Vector2 wheretogo2 = new Vector2(96f, rotz);
                            Vector2 where2 = P.Center - npc.Center;
                            where2.Normalize();
                            Func<Vector2, Vector2, float, float, float> projectilefacing = delegate (Vector2 playerpos, Vector2 projpos, float time, float current)
                            {
                                float val = current;
                                if (time<100)
                                val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.04f);
                                else
                                val = current.AngleLerp((playerpos - projpos).ToRotation(), 0.015f);

                                return val;
                            };
                            Func<Vector2, Vector2, float, Vector2, Vector2> projectilemoving = delegate (Vector2 playerpos, Vector2 projpos, float time, Vector2 current)
                            {
                                Vector2 wheretogo = new Vector2(wheretogo2.X, wheretogo2.Y);
                                float angle = MathHelper.ToRadians(((wheretogo.Y + time * 2f)));
                                Vector2 instore = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * wheretogo.X;

                                Vector2 gothere333 = Hellion.GetHellion().npc.Center + instore;
                                Vector2 slideover = gothere333 - projpos;
                                current = slideover / 2f;

                                current /= 1.125f;

                                Vector2 speedz = current;
                                float spzzed = speedz.Length();
                                speedz.Normalize();
                                if (spzzed > 25f)
                                    current = (speedz * spzzed);

                                return current;
                            };
                            Func<float, bool> projectilepattern = (time) => (time == 20);

                            int ize2 = ParadoxMirror.SummonMirror(where, Vector2.Zero, 100, 250, wheretogo2.Y, mod.ProjectileType("HellionBeam"), projectilepattern, 3f, 200, true);
                            (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilefacing = projectilefacing;
                            (Main.projectile[ize2].modProjectile as ParadoxMirror).projectilemoving = projectilemoving;
                            Main.PlaySound(SoundID.Item, (int)Main.projectile[ize2].position.X, (int)Main.projectile[ize2].position.Y, 33, 0.25f, 0.5f);
                            Main.projectile[ize2].netUpdate = true;

                        }
                    }


                }
                npc.velocity /= 1.02f;

            }
            return false;

        }

        public override bool CheckDead()
        {
            return true;
        }

        public override void SendExtraAI(BinaryWriter writer)
        {
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
        }
    }

    public class HealingDrone : DPSDrones
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Repair Support Wraith");
            Main.npcFrameCount[npc.type] = 1;
            NPCID.Sets.NeedsExpertScaling[npc.type] = true;
        }
        public override void SetDefaults()
        {
            npc.width = 16;
            npc.height = 16;
            npc.damage = 10;
            npc.defense = 0;
            npc.lifeMax = 10000;
            npc.HitSound = SoundID.NPCHit5;
            npc.DeathSound = SoundID.NPCDeath6;
            npc.knockBackResist = 0.05f;
            npc.aiStyle = -1;
            npc.boss = false;
            animationType = 0;
            npc.noTileCollide = true;
            npc.noGravity = true;
            npc.value = Item.buyPrice(0, 1, 0, 0);
        }
        public override void AI()
        {

            //npc.netUpdate = true;
            NPC master = Main.npc[(int)npc.ai[1]];
            Player P = Main.player[npc.target];
            Hellion hell = Hellion.GetHellion();

            if (hell == null)
            {
                npc.active = false;
                return;
            }

            NPC guy = Main.npc[(int)npc.ai[1]];

            bool checksem = master.active && !master.dontTakeDamage && !master.friendly && master.type != npc.type && master.whoAmI != npc.whoAmI
                 && !master.immortal && master.chaseable;

                if (master == null || master.active==false || !checksem)
                {
                    List<int> stuffs = new List<int>();
                    for (int i = 0; i < Main.maxNPCs; i += 1)
                    {
                        if (Main.npc[i]!=null)
                        if (Main.npc[i].active)
                        if (!Main.npc[i].dontTakeDamage && !Main.npc[i].friendly && Main.npc[i].type!=npc.type && !master.immortal && master.chaseable)
                        stuffs.Add(i);
                    }

                if (stuffs.Count > 0)
                {
                    npc.ai[1] = stuffs[(Main.rand.Next(0, stuffs.Count))];
                }
                else
                {
                    if (hell != null)
                    {
                        npc.ai[1] = hell.npc.whoAmI;
                    }
                    else
                    {
                        return;
                    }
                }

                //if (!P.active || P.dead)
                //    npc.active = false;

            }




            if (npc.target< 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
            {
                npc.TargetClosest(false);
                P = Main.player[npc.target];

            }
            else
            {
                if (master != null)
                {


                    npc.ai[0] += 1;
                    if (npc.ai[0] == 1)
                    {
                        npc.ai[1] = Main.rand.NextFloat(0.0025f, 0.0075f) * (Main.rand.Next(0, 2) == 1 ? 1f : -1f);
                        npc.ai[2] = Main.rand.NextFloat(0f, MathHelper.ToRadians(360));
                        npc.netUpdate = true;

                    }
                    npc.ai[2] += npc.ai[1];
                    Vector2 tothere = master.Center;
                    tothere += new Vector2((float)Math.Cos(npc.ai[2]), (float)Math.Sin(npc.ai[2])) * (200);
                    tothere -= npc.Center;
                    tothere.Normalize();
                    npc.velocity += tothere * 1f;

                    if (npc.ai[0] % 180 == 0)
                    {
                        Idglib.Shattershots(npc.position, master.Center, new Vector2(0, 0), ProjectileID.DD2DarkMageHeal, 50, 12, 0, 1, true, 0, true, 300);
                        if (master.type == mod.NPCType("HellionWorm"))
                            hell.npc.life = Math.Min(hell.npc.lifeMax, hell.npc.life + 200);
                        if (master.type == mod.NPCType("Hellion"))
                            hell.npc.life = Math.Min(hell.npc.lifeMax, hell.npc.life + 500);
                    }

                }


                float fric = 0.97f;
                npc.velocity = npc.velocity * fric;
            }

        }

    }


    }
