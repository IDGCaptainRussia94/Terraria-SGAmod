using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Dimensions;
using Idglibrary;
using System.IO;
using System.Linq;
using SGAmod.HavocGear.Items.Weapons;
using SGAmod.Items.Weapons;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Buffs;

namespace SGAmod.NPCs.Murk
{
    [AutoloadBossHead]
    public class Murk : ModNPC, ISGABoss
    {
        public string Trophy() => "MurkTrophy";
        public bool Chance() => Main.rand.Next(0, 10) == 0;
        public string RelicName() => "Murk";
        public void NoHitDrops() { }

        int perhit = 0; 
        int counter = 0;
        public int gasshift = 0;
        public int gastimer = 0;
        int smackdown = 0;
        int attacktype = 0;

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            counter=reader.ReadInt32();
            gasshift=reader.ReadInt32();
            gastimer=reader.ReadInt32();
            smackdown = reader.ReadInt32();
            attacktype = reader.ReadInt32();
        }
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(counter);
            writer.Write(gasshift);
            writer.Write(gastimer);
            writer.Write(smackdown);
            writer.Write(attacktype);
        }
        public override void SetDefaults()
        {
            npc.aiStyle = -1;
            npc.lifeMax = 6000;
            if (Main.hardMode)
                npc.lifeMax = 12500;
            npc.damage = 100;
            npc.defense = 14;
            npc.knockBackResist = 0.0f;
            npc.dontTakeDamage = false;
            npc.npcSlots = 50f;
            npc.width = 126;
            npc.height = 134;
            npc.boss = true;
            npc.lavaImmune = true;
            npc.noGravity = false;
            npc.noTileCollide = false;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.HitSound = SoundID.NPCHit1;
            npc.alpha = 0;
            //aiType = NPCID.KingSlime;
            animationType = NPCID.KingSlime;
            NPCID.Sets.MustAlwaysDraw[npc.type] = true;
            music = mod.GetSoundSlot(SoundType.Music, "Sounds/Music/Murk");
            bossBag = mod.ItemType("MurkBossBag");
            npc.value = Item.buyPrice(0, 7, 50, 0);
            npc.buffImmune[mod.BuffType("AcidBurn")] = true;
            npc.buffImmune[BuffID.Poisoned] = true;
            npc.buffImmune[BuffID.Venom] = true;
        }

        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.625f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Murk");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.KingSlime];
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.RestorationPotion;
        }

        public void CustomBehavior(ref float ai)
        {
            Player player = Main.player[npc.target];
        }

        public override void NPCLoot()
        {

            SGAWorld.downedMurk = Main.hardMode ? 2 : 2;
            SGAWorld.GenVirulent();

            Player[] plz = Main.player.Where(testby => testby.active && testby.armor[0].type == ModContent.ItemType<Items.Armors.Engineer.EngineerHead>() && testby.armor[1].type == ModContent.ItemType<Items.Armors.Engineer.EngineerChest>() && testby.armor[2].type == ModContent.ItemType<Items.Armors.Engineer.EngineerLegs>()).ToArray();
            if (plz.Length > 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.BustlingFungus>());

            for (int i = 0; i < (Main.hardMode ? 2 : 1); i += 1)
            {
                if (Main.expertMode)
                {
                    npc.DropBossBags();
                }
                else
                {

                    for (int f = 0; f < (Main.rand.Next(30, 45)); f = f + 1)
                    {
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("MurkyGel"));
                    }

                    int choice = Main.rand.Next(7);
                    if (choice == 0)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < MurkFlail>());
                    else if (choice == 1)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < Mossthorn>());
                    else if (choice == 2)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < Landslide>());
                    else if (choice == 3)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < Mudmore>());
                    else if (choice == 4)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType < SwarmGrenade>(), Main.rand.Next(40, 100));
                    else if (choice == 5)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<GnatStaff>());
                    else if (choice == 6)
                        Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<SwarmGun>());

                }
            }
            Achivements.SGAAchivements.UnlockAchivement("Murk", Main.LocalPlayer);
            if (SGAWorld.downedMurk < 2 && SGAWorld.downedCaliburnGuardians<3)
                Idglib.Chat("The Moist Stone around Dank Shrines has weakened and can be broken.", 75, 225, 75);
        }


        public override void AI()
        {
            float num644 = 1f;
            bool flag65 = false;
            bool flag66 = false;
            bool touchingground = false;
            bool nightmare = SGAWorld.NightmareHardcore > 0;
            float lifePercent = nightmare ? 1.01f : 0.85f;
            gastimer -= 1;
            smackdown += 1;
            if (gastimer < 1)
                gasshift = Math.Max(gasshift - 1, 0);

            npc.aiAction = 0;
            npc.localAI[0] += npc.localAI[0] > -1 ? 1 : -1;
            int dustype = 184;//mod.DustType("MangroveDust");
            if (npc.localAI[0] > 0)
                dustype = 184;

            npc.noGravity = false;
            npc.SGANPCs().overallResist = 1f;
            if (gasshift > 0)
            {
                npc.SGANPCs().overallResist = 0.5f;

                float scaledSize = 1.5f+MathHelper.Clamp((Math.Abs(npc.localAI[0])-400)/200f,0f,0.5f);
                Vector2 center = npc.Center;
                //if (npc.target > -1 && Main.player[npc.target].active)
                //    center = new Vector2(center.X, Main.player[npc.target].Center.Y);

                for (int i = 0; i < Main.maxPlayers; i += 1)
                {
                    if (Main.player[i] != null && Main.player[i].active)
                    {
                        if (Main.player[i].Distance(center) < 5000)
                        {
                            Main.player[i].GetModPlayer<SGADimPlayer>().lightSize = 4000 - (int)((float)gasshift * (4000f / 500f));
                            if (Main.player[i].Distance(center) > ((gasshift) * scaledSize) && gasshift > 400)
                            {
                                Main.player[i].AddBuff(BuffID.Rabies, 2);
                                Main.player[i].AddBuff(BuffID.Venom, 2);
                                Main.player[i].AddBuff(BuffID.Suffocation, 2);
                                Main.player[i].AddBuff(BuffID.Weak, 2);
                                Main.player[i].AddBuff(ModContent.BuffType<MurkyDepths>(), 2);
                                Main.player[i].AddBuff(ModContent.BuffType<Buffs.PiercedVulnerable>(), 2);
                                Main.player[i].AddBuff(ModContent.BuffType<Buffs.EverlastingSuffering>(), 2);

                            }
                        }
                    }
                }
                if (SGAConfigClient.Instance.Murklite && gasshift > 300)
                {
                    float randomrot = Main.rand.NextFloat(MathHelper.TwoPi);
                    for (float num654 = 0; num654 < MathHelper.TwoPi; num654 += MathHelper.TwoPi/10f)
                    {
                        Vector2 there = (Vector2.UnitX * ((gasshift) * scaledSize)).RotatedByRandom(MathHelper.TwoPi);
                        int num655 = Dust.NewDust(npc.position + there, 0, 0, dustype, 0, 0, dustype, new Color(30, 30, 30, 20), 2f);
                        Main.dust[num655].noGravity = true;
                        Main.dust[num655].velocity = Vector2.UnitX.RotatedByRandom(MathHelper.TwoPi);
                    }
                }

                SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(center.X, center.Y, gasshift * (scaledSize*2f))));
            }

            if (npc.ai[3] == 0f && npc.life > 0)
            {
                npc.ai[3] = (float)npc.lifeMax;

                if (Main.netMode != 1)
                {
                    int num664 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, mod.NPCType("Murk2"), 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num664].ai[1] = npc.whoAmI;
                    Main.npc[num664].netUpdate = true;
                    if (Main.netMode == 2 && num664 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

                }

            }

            if ((int)npc.localAI[3] == 0 && Main.netMode != 1)
            {
                npc.ai[0] = -100f;
                npc.TargetClosest(true);
                npc.netUpdate = true;
            }

            npc.localAI[3] = Math.Max(npc.localAI[3] - 1f, 1f);

            if (Main.player[npc.target].dead)
            {
                npc.TargetClosest(true);
                if (Main.player[npc.target].dead)
                {
                    npc.timeLeft = 0;
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = 1;
                    }
                    else
                    {
                        npc.direction = -1;
                    }
                }
            }
            else
            {
            npc.timeLeft = 100;
            }
            
            if (!Main.player[npc.target].dead && npc.ai[2] >= 300f && npc.ai[1] < 5f && npc.velocity.Y == 0f)
            {
                npc.ai[2] = 0f;
                npc.ai[0] = 0f;
                npc.ai[1] = 5f;
                if (Main.netMode != 1)
                {
                    npc.TargetClosest(false);
                    Point point5 = npc.Center.ToTileCoordinates();
                    Point point6 = Main.player[npc.target].Center.ToTileCoordinates();
                    Vector2 vector65 = Main.player[npc.target].Center - npc.Center;


                    int num645 = 10;
                    int num646 = 0;
                    int num647 = 7;
                    int num648 = 0;
                    bool flag67 = false;
                    if (vector65.Length() > 2000f)
                    {
                        flag67 = true;
                        num648 = 100;
                    }
                    while (!flag67 && num648 < 100)
                    {
                        num648++;
                        int num649 = Main.rand.Next(point6.X - num645, point6.X + num645 + 1);
                        int num650 = Main.rand.Next(point6.Y - num645, point6.Y + 1);
                        if ((num650 < point6.Y - num647 || num650 > point6.Y + num647 || num649 < point6.X - num647 || num649 > point6.X + num647) && (num650 < point5.Y - num646 || num650 > point5.Y + num646 || num649 < point5.X - num646 || num649 > point5.X + num646) && !Main.tile[num649, num650].nactive())
                        {
                            int num651 = num650;
                            int num652 = 0;
                            bool flag68 = Main.tile[num649, num651].nactive() && Main.tileSolid[(int)Main.tile[num649, num651].type] && !Main.tileSolidTop[(int)Main.tile[num649, num651].type];
                            if (flag68)
                            {
                                num652 = 1;
                            }
                            else
                            {
                                while (num652 < 150 && num651 + num652 < Main.maxTilesY)
                                {
                                    int num653 = num651 + num652;
                                    bool flag69 = Main.tile[num649, num653].nactive() && Main.tileSolid[(int)Main.tile[num649, num653].type] && !Main.tileSolidTop[(int)Main.tile[num649, num653].type];
                                    if (flag69)
                                    {
                                        num652--;
                                        break;
                                    }
                                    num652++;
                                }
                            }
                            num650 += num652;
                            bool flag70 = true;
                            if (flag70 && Main.tile[num649, num650].lava())
                            {
                                flag70 = false;
                            }
                            if (flag70 && !Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
                            {
                                flag70 = false;
                            }
                            if (flag70)
                            {
                                npc.localAI[1] = (float)(num649 * 16 + 8);
                                npc.localAI[2] = (float)(num650 * 16 + 16);
                                break;
                            }
                        }
                    }
                    if (num648 >= 100)
                    {
                        Vector2 bottom = Main.player[(int)Player.FindClosest(npc.position, npc.width, npc.height)].Bottom;
                        npc.localAI[1] = bottom.X;
                        npc.localAI[2] = bottom.Y;
                    }
                }
            }
            if (!Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0))
            {
                npc.ai[2] += 1f;
            }
            if (Math.Abs(npc.Top.Y - Main.player[npc.target].Bottom.Y) > 320f)
            {
                npc.ai[2] += 1f;
            }

            if (Main.netMode != 1)
            {
                bool flag80 = false;
                int num70 = 0;
                num70 = num70 + 1;
                Vector2 vector65 = Main.player[npc.target].Center - npc.Center;
                bool flag71 = false;
                bool flag81 = true;

                if (npc.life < npc.lifeMax / 3 && Main.expertMode == true)
                {
                    npc.damage = 200;
                    npc.defense = 30;
                    npc.knockBackResist = 0.0f;
                    if (npc.life <= 1000)
                    {
                        flag80 = true;
                    }
                }
                else if (npc.life < npc.lifeMax / 2 && Main.expertMode == false)
                {
                    npc.damage = 150;
                    npc.defense = 20;
                }

                if (npc.life <= npc.lifeMax / 5)
                {
                    if (flag81 == true)
                    {
                        flag71 = true;
                    }
                }

                if (flag71 == true)
                {
                    npc.defense = 10;
                    flag81 = false;
                    flag71 = false;
                    if (counter++ == 10)
                    {

                    }
                }

                /*if (!Main.player[npc.target].ZoneJungle)
                {
                    npc.defense += 75;
                }*/

                if (Main.expertMode == true && npc.life <= npc.lifeMax / 10)
                {
                    if (num70 >= 75)
                    {
                        npc.life += 5;
                        num70 = 0;
                    }
                }
                //if (!Main.player[npc.target].ZoneJungle)
                //npc.defense=100;
            }

            if (npc.ai[1] == 5f)
            {
                flag65 = true;
                npc.aiAction = 1;
                npc.ai[0] += 1f;
                num644 = MathHelper.Clamp((60f - npc.ai[0]) / 60f, 0f, 1f);
                num644 = 0.5f + num644 * 0.5f;
                if (npc.ai[0] >= 60f)
                {
                    flag66 = true;
                }
                /* if (npc.ai[0] == 60f)
                {
                    Gore.NewGore(npc.Center + new Vector2(-40f, (float)(-(float)npc.height / 2)), npc.velocity, 734, 1f);
                } */
                if (npc.ai[0] >= 60f && Main.netMode != 1)
                {
                    npc.Bottom = new Vector2(npc.localAI[1], npc.localAI[2]);
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                }
                if (Main.netMode == 1 && npc.ai[0] >= 120f)
                {
                    npc.ai[1] = 6f;
                    npc.ai[0] = 0f;
                }
                if (!flag66)
                {
                    for (int num654 = 0; num654 < 10; num654++)
                    {
                        int num655 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, dustype, npc.velocity.X, npc.velocity.Y, 100, new Color(30, 30, 30, 20), 2f);
                        Main.dust[num655].noGravity = true;
                        Main.dust[num655].velocity *= 0.5f;
                    }
                }
            }
            else if (npc.ai[1] == 6f)
            {
                flag65 = true;
                npc.aiAction = 0;
                npc.ai[0] += 1f;
                num644 = MathHelper.Clamp(npc.ai[0] / 30f, 0f, 1f);
                num644 = 0.5f + num644 * 0.5f;
                if (npc.ai[0] >= 30f && Main.netMode != 1)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.netUpdate = true;
                    npc.TargetClosest(true);
                }
                if (Main.netMode == 1 && npc.ai[0] >= 60f)
                {
                    npc.ai[1] = 0f;
                    npc.ai[0] = 0f;
                    npc.TargetClosest(true);
                }
                for (int num656 = 0; num656 < 10; num656++)
                {
                    int num657 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, dustype, npc.velocity.X, npc.velocity.Y, 100, new Color(30, 30, 30, 20), 2f);
                    Main.dust[num657].noGravity = true;
                    Main.dust[num657].velocity *= 2f;
                }
            }

            npc.dontTakeDamage = (((npc.hide = flag66) || (NPC.CountNPCS(mod.NPCType("BossFlyMiniboss1")) > 0 && Main.hardMode))) || (gasshift % 500 != 0 && gastimer>0);
            if (Main.hardMode)
                npc.GivenName = "Murk: Lord of the flies";

            if (npc.velocity.Y == 0f)
            {
                touchingground = true;
                if (!Main.player[npc.target].dead)
                {
                    if (Main.player[npc.target].Center.X < npc.Center.X)
                    {
                        npc.direction = -1;
                    }
                    else
                    {
                        npc.direction = 1;
                    }
                }

                npc.localAI[3] = 1f;

                npc.velocity.X = npc.velocity.X * 0.8f;
                if ((double)npc.velocity.X > -0.1 && (double)npc.velocity.X < 0.1)
                {
                    npc.velocity.X = 0f;
                }
                
                if (smackdown < 0)
                {
                    smackdown = Main.rand.Next(800,1300)+(int)(npc.life/(float)npc.lifeMax)*500;
                    //smackdown = -1050;
                    //npc.velocity.Y = -10;
                    npc.ai[0] = -250;

                    Main.PlaySound(SoundID.Item14, npc.Center);

                    for (int k = -32; k < 35; k += 4)
                    {
                        for (int i = -1; i < 3; i += 2)
                        {
                            List<Projectile> itz = Idglib.Shattershots(npc.Center + new Vector2(0, k), npc.Center + new Vector2(1000 * i, k), new Vector2(0,0), ProjectileID.MudBall, 70, 30f, 5, 2, true, 0, true, 300);
                            foreach (Projectile proj in itz)
                            {
                                proj.aiStyle = -5;
                            }
                        }
                    }

                    for (float gg = -4f; gg < 4.26f; gg += 0.25f)
                    {
                        Gore.NewGore(npc.Center + new Vector2(Main.rand.NextFloat(-32, 32), Main.rand.NextFloat(-16, 16)), new Vector2(gg,Main.rand.NextFloat(-3,3)), Main.rand.Next(61, 64), (5f- Math.Abs(gg))/4f);
                    }

                }

                if (!flag65)
                {
                    Player target2 = Main.player[npc.target];
                    if (npc.life < (npc.lifeMax * lifePercent) && npc.localAI[0] > -600 && NPC.CountNPCS(mod.NPCType("BossFlyMiniboss1")) < 1 && (npc.Distance(target2.MountedCenter) < 1000 || gasshift > 0))
                    {
                        npc.ai[0] = -100f;
                        npc.ai[2] = 0f;
                        if (npc.localAI[0] > 0)
                            npc.localAI[0] = -1;
                        //npc.dontTakeDamage=true;
                        npc.defense *= 2;

                        if (gasshift < 500)
                        {
                            npc.localAI[0] += 1;
                            gasshift += 1;
                            gastimer = 120 + (int)((1f - ((float)npc.life / (float)npc.lifeMax)) * (Main.hardMode ? 800f : 500f));
                            if (gasshift == 3)
                            {
                                attacktype = npc.life < npc.lifeMax * 0.35 && Main.hardMode ? 1 : 0;
                            }
                        }
                        else
                        {
                            if (attacktype == 0) 
                            {
                                if ((-npc.localAI[0]) % 15 == 0)
                                {
                                    npc.TargetClosest(true);
                                    npc.netUpdate = true;
                                    Player target = Main.player[npc.target];
                                    List<Projectile> itz2 = Idglib.Shattershots(npc.Center, target.position, new Vector2(target.width / 2, target.height / 2), ProjectileID.PoisonFang, Main.hardMode ? 25 : 15, 15f, 0, 1, true, 0f, false, 300);
                                    IdgProjectile.AddOnHitBuff(itz2[0].whoAmI, BuffID.Poisoned, 60 * 5);
                                    var sound = Main.PlaySound(SoundID.Item111, npc.Center);

                                    for (int num656 = 0; num656 < 15; num656++)
                                    {
                                        int num657 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 184, itz2[0].velocity.X, itz2[0].velocity.Y, 100, new Color(80, 80, 80, 100), 1.5f);
                                        Main.dust[num657].noGravity = true;

                                        Main.dust[num657].velocity = (new Vector2(itz2[0].velocity.X, itz2[0].velocity.Y) * 2) * ((float)num656 / 15);
                                    }
                                }
                            }
                            if (attacktype > 0)
                            {
                                attacktype += 1;
                                bool boolz = false;// (attacktype + 9) % Math.Max(10, (60 - (int)(attacktype / 5))) == 0;
                                int input = Math.Max(10, (60 - (int)(attacktype / 5)));
                                if (((attacktype-10) % input == 0 || boolz) && npc.localAI[0] > -500 && attacktype>10)
                                {
                                    npc.TargetClosest(true);
                                    npc.netUpdate = true;
                                    Player target = Main.player[npc.target];
                                    if (!boolz || input<12)
                                    {
                                        var sound = Main.PlaySound(SoundID.Item111, npc.Center);
                                        if (sound != null)
                                        {
                                            sound.Pitch += 0.50f;
                                        }
                                    }

                                    float adder = Math.Max(0, attacktype-200);
                                    float angle = ((adder + (boolz ? 19 : 0)) / (837f / MathHelper.TwoPi));

                                    for (float f = 0f; f < MathHelper.TwoPi; f += MathHelper.PiOver4)
                                    {

                                        Vector2 shootthere = Vector2.One.RotatedBy(f+angle);
                                        if (boolz && input>12)
                                        {
                                            for (int xx = 0; xx < 640; xx += 8)
                                            {
                                                int num657 = Dust.NewDust(npc.Center+ shootthere*xx, 0,0, mod.DustType("MangroveDust"), -shootthere.X, -shootthere.Y, 150, new Color(30, 30, 30, 20), 1f);
                                                Main.dust[num657].noGravity = true;
                                                Main.dust[num657].velocity *= 2f;
                                            }
                                        }
                                        else
                                        {
                                            if (!boolz)
                                            {
                                                List<Projectile> itz2 = Idglib.Shattershots(npc.Center, npc.Center + shootthere, Vector2.Zero, ModContent.ProjectileType<MurkTelegraphedAttack>(), Main.hardMode ? 25 : 15, 1f, 0, 1, true, 0f, false, 200);
                                                IdgProjectile.AddOnHitBuff(itz2[0].whoAmI, BuffID.Poisoned, 60 * 5);

                                                Vector2 vectoing = Vector2.TransformNormal(itz2[0].velocity, Matrix.CreateScale(1f, 1f, 1f));
                                                itz2[0].velocity = vectoing * 12f;

                                                for (int num656 = 0; num656 < 15; num656++)
                                                {
                                                    int num657 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, 184, vectoing.X * 1f, vectoing.Y * 1f, 100, new Color(80, 80, 80, 100), 1.5f);
                                                    Main.dust[num657].noGravity = true;

                                                    Main.dust[num657].velocity = (new Vector2(itz2[0].velocity.X, itz2[0].velocity.Y) * 2) * ((float)num656 / 15);
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                        }

                        for (int xx = 0; xx < 5; xx++)
                        {
                            int num657 = Dust.NewDust(npc.position + Vector2.UnitX * -20f, npc.width + 40, npc.height, mod.DustType("MangroveDust"), npc.velocity.X, npc.velocity.Y, 100, new Color(30, 30, 30, 20), 1f);
                            Main.dust[num657].noGravity = true;
                            Main.dust[num657].velocity *= 2f;
                        }

                    }

                    npc.ai[0] += 2f;
                    if ((double)npc.life < (double)npc.lifeMax * 0.8 || Main.hardMode || Main.expertMode || nightmare)
                    {
                        npc.ai[0] += 1f;
                    }
                        if ((double)npc.life < (double)npc.lifeMax * 0.6 || Main.hardMode || nightmare)
                        {
                            npc.ai[0] += 1f;
                        }
                    if (NPC.CountNPCS(mod.NPCType("BossFlyMiniboss1")) < 1 || nightmare)
                    {
                        if ((double)npc.life < (double)npc.lifeMax * 0.4 || Main.hardMode || nightmare)
                        {
                            npc.ai[0] += 2f;
                        }
                        if ((double)npc.life < (double)npc.lifeMax * 0.2 || nightmare)
                        {
                            npc.ai[0] += 3f;
                        }
                    }

                    if ((double)npc.life < (double)npc.lifeMax * 0.1 || nightmare)
                    {
                        npc.ai[0] += 4f;
                    }
                    
                    if (npc.ai[0] >= 0f)
                    {
                        float mathit = Math.Abs((Main.player[npc.target].Center.X + (Main.player[npc.target].velocity.X * 3f)) - npc.Center.X);
                        npc.netUpdate = true;
                        npc.TargetClosest(true);
                        float speedboost = 0f;
                        if (npc.ai[1] == 3f)
                        {
                            //High Jump
                            speedboost = Math.Max((mathit - 160f) / (Main.hardMode ? 128f : 160f), 0f);
                            npc.velocity.Y = -13;

                            if (!Main.hardMode)
                                npc.localAI[3] = 160f;
                            npc.velocity.X = npc.velocity.X + (3.5f + speedboost) * (float)npc.direction;
                            npc.ai[0] = -200f;
                            npc.ai[1] = 0f;
                        }
                        else if (npc.ai[1] == 2f)
                        {
                            //Forward Jump
                            speedboost = Math.Max((mathit - 500f) / 300f, 0f);
                            float distx = 1f + speedboost;
                            npc.localAI[3] = 120f;
                            npc.velocity.Y = -6f;
                            npc.velocity.X = npc.velocity.X + (npc.localAI[0] < 0 ? 10f : 7.5f) * distx * (float)npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                        }
                        else
                        {
                            //Normal Jump, can reach high players
                            speedboost = Math.Max((mathit - 700f) / 260f, Main.hardMode ? 2f : 0f);
                            npc.velocity.Y = -8f;
                            float distz = Main.player[npc.target].Center.Y - npc.Center.Y;
                            if (distz < -96)
                            {
                                float jumpHeight = ((Main.player[npc.target].Center.Y - 500) - npc.Center.Y);

                                float gravity = Player.defaultGravity;

                                Vector2 velo = new Vector2(0, (float)Math.Sqrt(-2.0f * gravity * jumpHeight));

                                npc.velocity.Y = velo.Y;

                                //npc.velocity.Y += (distz - 100) / 60f;
                            }
                            npc.velocity.X = npc.velocity.X + ((npc.localAI[0] < 0 ? 6f : 4f) + speedboost) * (float)npc.direction;
                            npc.ai[0] = -120f;
                            npc.ai[1] += 1f;
                        }
                    }
                    else if (npc.ai[0] >= -30f)
                    {
                        npc.aiAction = 1;
                    }
                }
            }
            else
            {
                if (smackdown < -200 && smackdown>=-1000)//FLY DOWN, SMACK!
                {
                    int slamtime = Main.hardMode ? -900 : -850;

                    npc.noGravity = true;
                    if (smackdown > slamtime)
                    {
                        npc.velocity.Y += 2.0f;
                        if (npc.velocity.Y > 16)
                            npc.velocity.Y = 16;

                        for (int i = 0; i < 4; i += 1)
                        {
                            int num658x = Dust.NewDust(npc.position, npc.width, npc.height, dustype, Main.rand.NextFloat(-16f, 16f), -npc.velocity.Y * 3f, 100, new Color(30, 30, 30, 20), npc.scale * ((i / 2f)) + 0.75f);
                            Main.dust[num658x].noGravity = true;
                        }

                    }
                    else
                    {
                        if (smackdown == slamtime-30)
                        {
                            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_DarkMageCastHeal, -1, -1);
                            if (sound != null)
                            {
                                sound.Volume = 0.99f;
                                sound.Pitch += 0.5f;
                            }

                            for (int i = 0; i < 16; i += 1)
                            {
                                int num658x = Dust.NewDust(npc.position, npc.width, npc.height, dustype, 0, 0, 100, new Color(30, 30, 30, 20), npc.scale * ((i / 2f)) + 1.75f);
                                Main.dust[num658x].velocity = Main.rand.NextVector2CircularEdge(12f, 12f);
                                Main.dust[num658x].noGravity = true;
                            }
                        }
                        if (smackdown < -880)
                        {
                            for (int i = 0; i < 4; i += 1)
                            {
                                int num658x = Dust.NewDust(npc.position, npc.width, npc.height, dustype, Main.rand.NextFloat(-16f, 16f), -npc.velocity.Y * 3f, 100, new Color(30, 30, 30, 20), npc.scale * ((i / 2f)) + 0.75f);
                                Main.dust[num658x].noGravity = true;
                            }
                        }
                        npc.velocity.Y -= 0.25f;
                        npc.velocity /= 1.25f;
                    }
                }

                if (npc.localAI[3] > 5f && npc.localAI[3] < 100)
                    npc.velocity.X *= 0.95f;
                if (npc.target < 255 && ((npc.direction == 1 && npc.velocity.X < 3f) || (npc.direction == -1 && npc.velocity.X > -3f)))
                {
                    if ((npc.direction == -1 && (double)npc.velocity.X < 0.1) || (npc.direction == 1 && (double)npc.velocity.X > -0.1))
                    {
                        npc.velocity.X = npc.velocity.X + 0.2f * (float)npc.direction;
                    }
                    else
                    {
                        npc.velocity.X = npc.velocity.X * 0.93f;
                    }
                }
            }



            if (Math.Abs(npc.velocity.Y) < 8f && npc.ai[1] == 0 && npc.localAI[0] % 2 == 0 && !touchingground && npc.localAI[0] < 0 && smackdown>0)
            {
                double angle = ((double)(npc.localAI[0] / 13f)) + 2.0 * Math.PI;
                List<Projectile> itz = Idglib.Shattershots(npc.Center, npc.Center, new Vector2(0, 16), ProjectileID.Stinger, Main.hardMode ? 25 : 15, 5f, 0, 1, true, (float)angle, true, 300);
                if (Main.expertMode && Main.hardMode)
                {
                    foreach (Projectile proj in itz)
                    {
                        proj.velocity.X *= 3f;
                        proj.netUpdate = true;
                    }
                }
                if (Main.expertMode && Main.hardMode)
                {
                    itz = Idglib.Shattershots(npc.Center, npc.Center, new Vector2(0, 16), ProjectileID.Stinger, Main.hardMode ? 25 : 15, 5f, 0, 1, true, (float)-angle, true, 300);
                    foreach (Projectile proj in itz)
                    {
                        proj.velocity.X *= 2f;
                        proj.netUpdate = true;
                    }
                }
            }
            else
            {
                if (smackdown > 2000 && npc.velocity.Y>6f)
                {
                    smackdown = -1000;
                }

            }
            if (npc.localAI[0] < -3000)
                npc.localAI[0] = 5;

            //Vector2 vector3 = Collision.WaterCollision(npc.position, new Vector2(0,16), npc.width, npc.height, false, false, false);
            if (npc.wet)
            {
                npc.life = (int)MathHelper.Clamp(npc.life + 1, 0, npc.lifeMax);
            }


            int num658 = Dust.NewDust(npc.position, npc.width, npc.height, dustype, npc.velocity.X, npc.velocity.Y, 100, new Color(30, 30, 30, 20), npc.scale * 1.2f);
            Main.dust[num658].noGravity = true;
            Main.dust[num658].velocity *= 0.5f;
            if (npc.life > 0)
            {
                float num659 = (float)npc.life / (float)npc.lifeMax;
                num659 = num659 * 0.5f + 0.75f;
                num659 *= num644;
                if (num659 != npc.scale)
                {
                    npc.position.X = npc.position.X + (float)(npc.width / 2);
                    npc.position.Y = npc.position.Y + (float)npc.height;
                    npc.scale = num659;
                    npc.width = (int)(98f * npc.scale);
                    npc.height = (int)(92f * npc.scale);
                    npc.position.X = npc.position.X - (float)(npc.width / 2);
                    npc.position.Y = npc.position.Y - (float)npc.height;
                }
                if (Main.netMode != 1)
                {
                    int num660 = (int)((double)npc.lifeMax * (Main.hardMode ? 0.03 : 0.04));
                    if ((float)(npc.life + num660) < npc.ai[3])
                    {
                        npc.ai[3] = (float)npc.life;
                        int num661 = Main.rand.Next(1, 2);
                        for (int num662 = 0; num662 < num661; num662++)
                        {
                            int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
                            int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
                            int num663 = NPCID.JungleSlime;
                            if (Main.hardMode)
                                num663 = NPCID.SpikedJungleSlime;
                            if (Main.rand.NextBool())
                                num663 = ModContent.NPCType<SwampSlime>();
                            if ((npc.localAI[0] < 0 && Main.expertMode) || ((SGAmod.DRMMode) && Main.rand.Next(2) == 0))
                            {
                                if (Main.rand.Next(0, 100) < 20)
                                    num663 = NPCID.SpikedJungleSlime;
                                if (num663 == NPCID.JungleSlime || num663 == ModContent.NPCType<SwampSlime>() || (SGAmod.DRMMode && Main.rand.Next(3) == 0))
                                    num663 = ModContent.NPCType<BossFly3>();
                            }

                            int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                            Main.npc[num664].SetDefaults(num663, -1f);
                            Main.npc[num664].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
                            Main.npc[num664].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
                            Main.npc[num664].ai[0] = (float)(-1000 * Main.rand.Next(3));
                            Main.npc[num664].ai[1] = 0f;
                            if (num663 == ModContent.NPCType<BossFly3>())
                                Main.npc[num664].ai[1] = npc.whoAmI;

                            Main.npc[num664].netUpdate = true;
                            if (Main.netMode == 2 && num664 < 200)
                            {
                                NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                            }
                        }
                        return;
                    }
                }
            }
        }

        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            Texture2D tex = SGAmod.ExtraTextures[59];
            Vector2 size = tex.Size() / 2f;

            BlendState blind = new BlendState
            {

                ColorSourceBlend = Blend.SourceColor,
                ColorDestinationBlend = Blend.InverseSourceColor,

                ColorBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.SourceColor,

                AlphaDestinationBlend = Blend.SourceColor,
                AlphaBlendFunction = BlendFunction.Subtract

            };

            if (gasshift > 0 && gasshift < 500 && gastimer>0)
            {

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointWrap, DepthStencilState.DepthRead, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
                for (float i = 0; i < 20f; i += 0.25f)
                {
                    float alpha = Math.Min((500f-gasshift) / (150f+i*3f), 1f);
                    Vector2 scale = (Vector2.One * i) * ((gasshift+gasshift*0.05f) / 20f);
                    spriteBatch.Draw(tex, npc.Center - Main.screenPosition, null, Color.Pink*0.03f, Main.GlobalTime + i, size, scale*alpha, SpriteEffects.None, 0);
                }

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, default, default, default, null, Main.GameViewMatrix.ZoomMatrix);
            }
        }

        public override bool CheckDead()
        {
            return true;
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            if (gasshift % 500 != 0)
                return false;

            return base.CanHitPlayer(target, ref cooldownSlot);
        }

        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            perhit += 1;
            if (Main.netMode != 1)
            {
                if (NPC.CountNPCS(mod.NPCType("Fly")) > 30 || perhit < 5)
                {
                    return;
                }
                perhit = 0;
                int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
                int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
                int num663 = mod.NPCType("Fly");

                int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                if (Main.netMode == 2 && num664 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }

        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            perhit += 1;
            if (Main.netMode != 1)
            {
                if (NPC.CountNPCS(mod.NPCType("Fly")) > 30 || perhit < 3)
                {
                    return;
                }
                perhit = 0;
                int x = (int)(npc.position.X + (float)Main.rand.Next(npc.width - 32));
                int y = (int)(npc.position.Y + (float)Main.rand.Next(npc.height - 32));
                int num663 = ModContent.NPCType<Fly>();

                int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                if (npc.life < npc.lifeMax * 0.35 && Main.hardMode)
                {
                    Main.npc[num664].aiStyle = 86;
                    Main.npc[num664].netUpdate = true;
                }

                if (Main.netMode == 2 && num664 < 200)
                {
                    NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                }
            }
        }
    }

    public class Fly : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 10;
            npc.height = 10;
            npc.damage = 12;
            npc.defense = 0;
            npc.lifeMax = 15;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = 5;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
        }

        public override bool PreNPCLoot()
        {
            NPCLoader.blockLoot.Add(ItemID.Heart);
            NPCLoader.blockLoot.Add(ItemID.Star);
            return false;
        }

        public override bool PreAI()
        {
            npc.ai[3] += 1;
            if ((int)npc.ai[3] == 1)
            {
                if (Main.hardMode)
                {
                    npc.lifeMax /= 2;
                    npc.life /= 2;
                }
                if ((int)npc.ai[3] == 1 && npc.aiStyle == 86)
                {
                    npc.damage = (int)(npc.damage*1.15f);
                }
                npc.netUpdate = true;
            }
            if (npc.ai[3] > 20)
            {
                Murk master = Main.npc.FirstOrDefault(testnpc => testnpc.active && testnpc.type == ModContent.NPCType<Murk>())?.modNPC as Murk ?? null;
                if (master!=null)
                {
                    if (master.gastimer > 200)
                    {
                        npc.ai[3] -= 1;
                    }
                }

                if (npc.ai[3] > 60 * (Main.expertMode ? 10 : 25))
                {
                    Player target = Main.player[npc.target];
                    List<Projectile> itz2 = Idglib.Shattershots(npc.Center, target.position, new Vector2(target.width / 2, target.height / 2), ProjectileID.HornetStinger, (int)((float)npc.damage / (Main.hardMode ? 9f : 3f)), 16f, 0, 1, true, 0f, false, 300);
                    Main.PlaySound(SoundID.Item42, npc.Center);
                    npc.active = false;
                }
            }
            return true;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fly");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }

    }

    public class Murk2 : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 10;
            npc.height = 10;
            npc.damage = 5;
            npc.defense = 0;
            npc.lifeMax = 500000;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            npc.dontTakeDamage = true;
            npc.immortal = true;
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Murk/Fly"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Murk Spawns");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }

        public override bool CheckActive()
        {
            NPC master = Main.npc[(int)npc.ai[1]];
            return (!master.active || npc.ai[1] < 1);
        }

        public override void AI()
        {

            npc.ai[0] += 1;
            double angle = ((double)(npc.ai[0] / 13f)) + 2.0 * Math.PI;
            NPC Master = Main.npc[(int)npc.ai[1]];
            if (!Master.active || npc.ai[1] < 1)
                npc.active = false;
            npc.Center = Master.Center;

            if (Master.life < Master.lifeMax * 0.35 && npc.ai[2] == 0)
            {
                if (Main.netMode != 1)
                {
                    Idglib.Chat("Murk calls for backup with a killer fly swarm!", 103, 128, 79);
                    Main.PlaySound(SoundID.Roar, npc.Center);
                    int x = (int)(Master.position.X + (float)Main.rand.Next(Master.width - 32));
                    int y = (int)(Master.position.Y + (float)Main.rand.Next(Master.height - 32));
                    int num663 = mod.NPCType("BossFlyMiniboss1");

                    int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num664].ai[1] = Master.whoAmI;
                    Main.npc[num664].life = (int)((double)(Master.lifeMax * (Main.hardMode ? 1.5 : 0.5)));
                    Main.npc[num664].lifeMax = Main.npc[num664].life;
                    Main.npc[num664].damage = Main.hardMode ? (Main.expertMode ? 80 : 50) : 25;
                    Main.npc[num664].netUpdate = true;
                    if (!Main.hardMode)
                        Main.npc[num664].dontTakeDamage = true;
                    if (Main.netMode == 2 && num664 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }

                    if (Main.hardMode)
                    {
                        x = (int)(Master.position.X + (float)Main.rand.Next(Master.width - 32));
                        y = (int)(Master.position.Y + (float)Main.rand.Next(Master.height - 32));
                        num663 = ModContent.NPCType<BossFly3>();

                        if (Main.expertMode)
                        {
                            for (int i = 0; i < 10; i += 1)
                            {

                                num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                                Main.npc[num664].ai[0] = Main.rand.NextFloat(50, 450);
                                Main.npc[num664].ai[2] = 10 + (i * 12);
                                Main.npc[num664].ai[1] = Master.whoAmI;
                                Main.npc[num664].damage = 50;
                                Main.npc[num664].netUpdate = true;
                                Main.npc[num664].dontTakeDamage = true;
                                if (Main.netMode == 2 && num664 < 200)
                                {
                                    NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                                }
                            }

                        }


                    }

                    npc.ai[2] = 1;
                    npc.netUpdate = true;
                }
            }



            if (Main.netMode != 1 && npc.ai[0] % 300 == 0 && NPC.CountNPCS(ModContent.NPCType<BossFly2>()) < (Main.expertMode ? 2 : 1) && Master.localAI[0] < 0)
            {
                if ((Master.modNPC as Murk).gastimer<1)
                {
                    int x = (int)(Master.position.X + (float)Main.rand.Next(Master.width - 32));
                    int y = (int)(Master.position.Y + (float)Main.rand.Next(Master.height - 32));
                    int num663 = ModContent.NPCType<BossFly2>();

                    int num664 = NPC.NewNPC(x, y, num663, 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num664].ai[1] = Master.whoAmI;
                    Main.npc[num664].netUpdate = true;
                    if (Main.netMode == 2 && num664 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }
                }
            }


        }

    }

    public class BossFly1 : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 10;
            npc.height = 10;
            npc.damage = 5;
            npc.defense = 2;
            npc.lifeMax = 50;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
        }

        public override bool PreNPCLoot()
        {
            NPCLoader.blockLoot.Add(ItemID.Heart);
            NPCLoader.blockLoot.Add(ItemID.Star);
            return false;
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Murk/Fly"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fly Swarm");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = ((int)(npc.ai[0] / 3));
            npc.frame.Y %= 4;
            npc.frame.Y *= frameHeight;
        }

        public override void AI()
        {

            npc.ai[0] += 1;
            npc.ai[2] += 1;
            float maxspeed = 8f;
            NPC Master = Main.npc[(int)npc.ai[1]];
            if (!Master.active || npc.ai[1]+1< 1)
                npc.active = false;

            Vector2 masterloc = Master.Center - new Vector2(0, 0);
            bool flyaway = false;

            npc.TargetClosest();

            if (npc.ai[0] % 1100 > 700)
            {
                masterloc = Main.player[npc.target].Center;
                maxspeed = MathHelper.Clamp((masterloc-npc.Center).Length()/64f, 6f, 15f);

            }
            else
            {

                if (this.GetType() == typeof(BossFlyMiniboss1) && npc.ai[2]%300==0 && npc.ai[0]>700 && Main.expertMode && (NPC.CountNPCS(ModContent.NPCType<Murk>())<1 || Main.hardMode))
                {

                    Player target = Main.player[npc.target];
                    List<Projectile> itz2 = Idglib.Shattershots(npc.Center, target.position, new Vector2(target.width / 2, target.height / 2), ProjectileID.Stinger, (int)((float)npc.damage/4f), 8f, 0, 1, true, 0f, false, 300);
                    Main.PlaySound(SoundID.Item42, npc.Center);
                }

            }

            if (Main.player[npc.target].dead)
            {
                flyaway = true;
            }

            if (flyaway)
            masterloc = -(Master.Center - npc.Center);

            Vector2 masterdist=(masterloc-npc.Center);
        Vector2 masternormal=masterdist; masternormal.Normalize();

        npc.velocity += masternormal*0.25f;
        npc.direction=(npc.velocity.X>0f).ToDirectionInt();
        if (npc.velocity.Length() > maxspeed) {npc.velocity.Normalize(); npc.velocity *= maxspeed; }

        }

    }

    public class BossFly2 : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 10;
            npc.height = 10;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 200;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Murk/Fly"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Bomber Fly");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = ((int)(npc.ai[0] / 3));
            npc.frame.Y %= 4;
            npc.frame.Y *= frameHeight;
        }

        private void ResetBomb()
        {
            npc.ai[0] = Main.rand.Next(50, 200);
            npc.ai[3] = Projectile.NewProjectile(npc.Center.X + Main.rand.Next(-8, 8), npc.Center.Y - 40f, 0f, 0f, ModContent.ProjectileType<LessStickyOgreBall>(), 15, 0f, 0);
            Main.projectile[(int)npc.ai[3]].damage = 15;
            npc.velocity.Y /= 3f;
            npc.netUpdate = true;
        }

        public override void AI()
        {

            npc.ai[0] += 1;
            NPC Master = Main.npc[(int)npc.ai[1]];
            if (!Master.active || npc.ai[1] + 1 < 1)
            {
                npc.active = false;
                return;
            }

            npc.TargetClosest(true);
            float maxSpeed = Main.hardMode ? 15f : 8f;

            Vector2 masterloc = (new Vector2(Master.position.X + (Master.width / 2), Master.position.Y + (Master.height / 2)));
            Vector2 distomaster = masterloc - npc.Center;

            if (npc.ai[3] < 1 && (npc.ai[0] == 2 || (distomaster.Length() < 64f && npc.ai[0] > 500))) { ResetBomb(); }
            Projectile carryproj = Main.projectile[(int)npc.ai[3]];
            Player target = Main.player[npc.target];

            if (carryproj != null && npc.ai[3] > 0)
            {
                if (carryproj.type == ModContent.ProjectileType<LessStickyOgreBall>() && carryproj.active)
                {
                    carryproj.timeLeft = 200;
                    carryproj.position = new Vector2(npc.position.X, npc.position.Y + (npc.height));
                    carryproj.velocity.X = npc.velocity.X / 2f;
                    carryproj.velocity.Y = npc.velocity.Y / 1.5f;
                    masterloc = (Main.player[npc.target].position) + new Vector2(target.width / 2, -260);
                    npc.velocity.Y -= 0.05f;
                    if (Math.Abs(masterloc.X - npc.Center.X) < 64 && distomaster.Y > 100)
                    {
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                }
                else { npc.ai[3] = 0; npc.netUpdate = true; }
            }
            else { npc.ai[3] = 0; npc.netUpdate = true; }
            if (Master.modNPC is Murk murky)
            {
                if (murky.gastimer > 5)
                    masterloc = (new Vector2(Master.position.X + (Master.width / 2), Master.position.Y + (Master.height / 2)))+Vector2.One.RotatedBy((npc.whoAmI*3974.3797f) + (npc.ai[0]/50f)*(maxSpeed/8f))*240f;
            }

            Vector2 masterdist = (masterloc - npc.Center);
            Vector2 masternormal = masterdist; masternormal.Normalize();

            npc.velocity += masternormal * (Main.hardMode ? 0.45f : 0.25f);
            npc.velocity /= 1.01f;
            npc.direction = (npc.velocity.X > 0f).ToDirectionInt();
            if (npc.velocity.Length() > maxSpeed) { npc.velocity.Normalize(); npc.velocity *= maxSpeed; }

        }

    }

    public class BossFly3 : BossFly2
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 10;
            npc.height = 10;
            npc.damage = 0;
            npc.defense = 0;
            npc.lifeMax = 175;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.noTileCollide = true;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Murk/Fly"); }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Airlifting Fly");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }

        public override void FindFrame(int frameHeight)
        {
            npc.frame.Y = ((int)(npc.ai[0] / 3));
            npc.frame.Y %= 4;
            npc.frame.Y *= frameHeight;
        }

        private void ResetBomb()
        {

            if (npc.ai[0] < 800 && npc.ai[0]!=2)
            {
                return;
            }
            npc.ai[0] = 150;

            int num663 = NPCID.JungleSlime;
            if (Main.rand.Next(0, 100) < 40 && Main.hardMode && Main.expertMode)
                num663 = NPCID.SpikedJungleSlime;
            if (Main.rand.Next(0,100)<25)
            num663 = ModContent.NPCType<SwampSlime>();

            int num664 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, num663, 0, 0f, 0f, 0f, 0f, 255);
            Main.npc[num664].SetDefaults(num663, -1f);
            Main.npc[num664].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
            Main.npc[num664].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
            Main.npc[num664].ai[0] = (float)(-1000 * Main.rand.Next(3));
            Main.npc[num664].ai[1] = 0f;
            npc.ai[3] = num664;

            Main.npc[num664].netUpdate = true;
            if (Main.netMode == 2 && num664 < 200)
            {
                NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
            }


            npc.velocity.Y /= 3f;
            npc.netUpdate = true;
        }

        public override void AI()
        {

            npc.ai[0] += 1;
            NPC Master = Main.npc[(int)npc.ai[1]];
            if (!Master.active || npc.ai[1]+1 < 1 || npc.dontTakeDamage)
                npc.active = false;

            npc.TargetClosest(true);

            Vector2 masterloc = (new Vector2(Master.position.X + (Master.width / 2), Master.position.Y + (Master.height / 2)));
            Vector2 distomaster = masterloc - npc.Center;

            if (npc.ai[3] < 1 && (npc.ai[0] == 2 || (distomaster.Length() < 64f && npc.ai[0] > 500))) { ResetBomb(); }
            NPC carryproj = Main.npc[(int)npc.ai[3]];
            Player target = Main.player[npc.target];

            if (carryproj != null && npc.ai[3] > 0)
            {
                if (carryproj.active)
                {
                    carryproj.position = new Vector2(npc.position.X, npc.position.Y + (npc.height));
                    carryproj.velocity.X = npc.velocity.X * 0.75f;
                    carryproj.velocity.Y = -npc.velocity.Y / 1f;
                    masterloc = (Main.player[npc.target].position) + new Vector2(target.width / 2, -260);
                    npc.velocity.Y -= 0.05f;
                    if (Math.Abs(masterloc.X - npc.Center.X) < 64 && distomaster.Y > 100)
                    {
                        npc.ai[3] = 0;
                        npc.netUpdate = true;
                    }
                }
                else { npc.ai[3] = 0; npc.netUpdate = true; }
            }
            else { npc.ai[3] = 0; npc.netUpdate = true; }


                    if (!carryproj.active)
                    {
                        if (npc.ai[3] < 1 && (npc.ai[0] > -88200))
                        {
                            foreach (NPC npc2 in Main.npc)
                            {
                                if (npc2.active && (npc2.type == ModContent.NPCType<SwampSlime>() || npc2.type == NPCID.SpikedJungleSlime || npc2.type == NPCID.JungleSlime) && npc2.Distance(npc.Center)<64)
                                {
                                    npc.ai[0] = 0;
                                    npc.ai[3] = npc2.whoAmI;
                                    npc.netUpdate = true;
                                }

                            }
                        }
                    }


            Vector2 masterdist = (masterloc - npc.Center);
            Vector2 masternormal = masterdist; masternormal.Normalize();

            npc.velocity /= 1.025f;

            npc.velocity += masternormal * Main.rand.NextFloat(0.01f,0.45f);
            npc.direction = (npc.velocity.X > 0f).ToDirectionInt();
            if (npc.velocity.Length() > 12f) { npc.velocity.Normalize(); npc.velocity *= 12f; }

        }

    }

    public class BossFlyMiniboss1 : BossFly1
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Killer Fly Swarm");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 10;
            npc.height = 10;
            npc.damage = 40;
            npc.defense = 15;
            npc.lifeMax = 5000;
            npc.value = 0f;
            npc.noGravity = true;
            npc.aiStyle = -1;
            npc.boss = true;
            npc.noTileCollide = true;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;

        }
        public override void ScaleExpertStats(int numPlayers, float bossLifeScale)
        {
            npc.lifeMax = (int)(npc.lifeMax * 0.8f * bossLifeScale);
            npc.damage = (int)(npc.damage * 0.6f);
        }

        public override bool PreNPCLoot()
        {
            NPCLoader.blockLoot.Add(ItemID.Heart);
            NPCLoader.blockLoot.Add(ItemID.Star);
            return true;
        }

        public override void NPCLoot()
        {
            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("RoilingSludge"));
            if (SGAWorld.downedMurk<1)
            SGAWorld.downedMurk = 1;
        }

        public override string Texture
        {
            get { return ("SGAmod/NPCs/Murk/Fly"); }
        }

        public override void AI()
        {
            //Main.NewText(npc.Center);
            if (npc.ai[2]<5)
            {
                int prev=npc.whoAmI;
                int num664;
                float val = 10;
                for (int i = 0; i < ((NPC.CountNPCS(ModContent.NPCType<Murk>()) >0 && !Main.hardMode) ? 10 : 20);i+=1)
                {

                    int num663 = ModContent.NPCType<BossFlyMiniboss1>();

                    num664 = NPC.NewNPC((int)npc.Center.X, (int)npc.Center.Y, num663, 0, 0f, 0f, 0f, 0f, 255);
                    Main.npc[num664].SetDefaults(num663, -1f);
                    Main.npc[num664].velocity.X = (float)Main.rand.Next(-15, 16) * 0.1f;
                    Main.npc[num664].velocity.Y = (float)Main.rand.Next(-30, 1) * 0.1f;
                    Main.npc[num664].ai[0] = Main.rand.NextFloat(50, 450);
                    Main.npc[num664].ai[1] = prev;
                    Main.npc[num664].ai[2] = val+(i*8);
                    Main.npc[num664].damage = npc.damage;
                    Main.npc[num664].dontTakeDamage = npc.dontTakeDamage;
                    Main.npc[num664].realLife = npc.whoAmI;
                    prev = num664;

                    Main.npc[num664].netUpdate = true;
                    if (Main.netMode == 2 && num664 < 200)
                    {
                        NetMessage.SendData(23, -1, -1, null, num664, 0f, 0f, 0f, 0, 0, 0);
                    }
                    npc.netUpdate = true;


                }
                if (Main.npc[(int)npc.ai[1]].active)
                {
                    if (Main.npc[(int)npc.ai[1]].type == ModContent.NPCType<Murk>())
                        Main.npc[(int)npc.ai[1]].timeLeft = 500;

                }
                if (NPC.CountNPCS(ModContent.NPCType<Murk>()) < 1)
                npc.ai[1] = prev;
                if (npc.ai[2]<1)
                npc.ai[2] = val;
                npc.aiStyle = -2;
            }
            if (npc.target > -1 && !Main.player[npc.target].dead)
                npc.timeLeft = 60;

            base.AI();
        }


    }

    public class PoisonStack : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Swamp Rot");
            Description.SetDefault("Slowly losing health");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "SGAmod/Buffs/PoisonStack";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SGAPlayer>().badLifeRegen += 4;
        }
    }

    public class MurkyDepths : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Murky Depths");
            Description.SetDefault("Take 50% more damage from all sources");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = true;
            longerExpertDebuff = true;
        }

        public override bool Autoload(ref string name, ref string texture)
        {
            texture = "SGAmod/Buffs/MurkyDepths";
            return true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SGAPlayer>().MurkyDepths = true;
        }

    }

        public class LessStickyOgreBall : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rot Ball");
        }
        public override string Texture => "Terraria/Projectile_" + ProjectileID.DD2OgreSpit;
        public override void SetDefaults()
        {
            projectile.CloneDefaults(ProjectileID.DD2OgreSpit);
            projectile.width = 12;
            projectile.height = 12;
            projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = true;
            projectile.hide = false;
            projectile.alpha = 0;
            projectile.timeLeft = 100000;
            aiType = -1;
            //projectile.aiStyle = -1;
        }

        public override void AI()
        {
            projectile.rotation = 0;
            for (int num149 = 0; num149 < 2; num149++)
            {
                if (Main.rand.Next(5) != 0)
                {
                    int num150 = Utils.SelectRandom<int>(Main.rand, 4, 256);
                    Dust dust7 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, num150, projectile.velocity.X, projectile.velocity.Y, 100)];
                    dust7.velocity = dust7.velocity / 4f + projectile.velocity / 2f;
                    dust7.scale = 0.8f + Main.rand.NextFloat() * 0.4f;
                    dust7.position = projectile.Center;
                    dust7.position += new Vector2(projectile.width * 2, 0f).RotatedBy((float)Math.PI * 2f * Main.rand.NextFloat()) * Main.rand.NextFloat();
                    dust7.noLight = true;
                    if (dust7.type == 4)
                    {
                        dust7.color = new Color(80, 170, 40, 120);
                    }
                }
            }
        }

        public override bool PreKill(int timeLeft)
        {

            for (int num161 = 0; num161 < 120; num161++)
            {
                int num162 = Utils.SelectRandom<int>(Main.rand, 4, 256);
                Dust dust68 = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, num162, 0f, 0f, 100)];
                dust68.scale = 0.8f + Main.rand.NextFloat() * 0.6f;
                dust68.fadeIn = 0.5f;
                Dust dust69 = dust68;
                Dust dust2 = dust69;
                dust2.velocity *= 4.5f;
                dust68.noLight = true;
                if (dust68.velocity.Y > 0f)
                {
                    dust69 = dust68;
                    dust2 = dust69;
                    dust2.velocity *= -0.5f;
                }
                if (dust68.type == 4)
                {
                    dust68.color = new Color(80, 170, 40, 120);
                }
            }
            for (int num163 = 0; num163 < 10; num163++)
            {
                Gore.NewGoreDirect(projectile.Center, new Vector2(MathHelper.Lerp(-5f, 5f, Main.rand.NextFloat()), (0f - Main.rand.NextFloat()) * 5f), 1024);
            }
            for (int num164 = 0; num164 < 10; num164++)
            {
                Gore.NewGoreDirect(projectile.Center, new Vector2(MathHelper.Lerp(-5f, 5f, Main.rand.NextFloat()), (0f - Main.rand.NextFloat()) * 5f), 1025);
            }
            for (int num165 = 0; num165 < 10; num165++)
            {
                Gore.NewGoreDirect(projectile.Center, new Vector2(MathHelper.Lerp(-5f, 5f, Main.rand.NextFloat()), (0f - Main.rand.NextFloat()) * 5f), 1026);
            }
            for (int num166 = 0; num166 < 20; num166++)
            {
                Gore.NewGoreDirect(projectile.Center, new Vector2(MathHelper.Lerp(-0.5f, 0.5f, Main.rand.NextFloat()), (0f - Main.rand.NextFloat()) * 2f), 1026);
            }
            if (Main.netMode != NetmodeID.Server)
            {
                Player player = Main.player[Main.myPlayer];
                if (!player.dead && player.active && (player.Center - projectile.Center).Length() < 300f)
                {
                    player.AddBuff(ModContent.BuffType<PlaceHolderDebuff>(), 60*15);
                    if (player.FindBuffIndex(ModContent.BuffType<PlaceHolderDebuff>()) >= 0)
                    {
                        player.buffType[player.FindBuffIndex(ModContent.BuffType<PlaceHolderDebuff>())] = ModContent.BuffType<PoisonStack>();
                    }
                }
            }


            return false;
        }

    }

    public class LessStickyOgreBallOneTick : LessStickyOgreBall
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Rot Ball (1 tick)");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            projectile.timeLeft = 3;
            projectile.alpha = 255;
        }

    }

    public class MurkTelegraphedAttack : PinkyWarning
    {
        protected override Color color => Color.Lime;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Warning Forever! (Murky Flavor)");
        }
        public override void SetDefaults()
        {
            //projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
            projectile.width = 16;
            projectile.height = 16;
            projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
            projectile.hostile = true;
            projectile.friendly = false;
            projectile.tileCollide = false;
            projectile.timeLeft = 150;
            projectile.extraUpdates = 1;
            aiType = -1;
            projectile.aiStyle = -1;
        }

        public override void AI()
        {
            base.AI();
            if (projectile.timeLeft == 80)
            {
                List<Projectile> itz2 = Idglib.Shattershots(projectile.Center, projectile.Center + projectile.velocity, Vector2.Zero, ProjectileID.PoisonFang, projectile.damage, 16f, 0, 1, true, 0f, false, 200);
                IdgProjectile.AddOnHitBuff(itz2[0].whoAmI, BuffID.Poisoned, 60 * 5);
            }
        }

    }


    }