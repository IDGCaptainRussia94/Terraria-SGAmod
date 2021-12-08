using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Idglibrary;

namespace SGAmod.Tiles
{  
        public class CaliburnAltar : ModTile
    {
        public virtual string myitem => "CaliburnTypeA";
        public virtual int summontype => 0;
        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileTable[Type] = false;
            dustType = DustID.Grass;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.LavaDeath = false;
            TileObjectData.newTile.WaterDeath = false;
            TileID.Sets.CanBeClearedDuringGeneration[Type] = false;
            //TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<BannerRackTE>().Hook_AfterPlacement, -1, 0, true);
            //TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.addTile(Type);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Caliburn Altar");
            //name.AddTranslation(GameCulture.Chinese, "烤炉");
            AddMapEntry(new Color(227, 216, 195), name);
        }

        public override bool CanExplode(int i, int j)
        {
            return false;
        }

        public override void MouseOver(int i, int j)
        {
            Main.LocalPlayer.showItemIcon2 = mod.ItemType(myitem);
            Main.LocalPlayer.showItemIconText = "";
            Main.LocalPlayer.showItemIcon = true;
        }

        public override bool NewRightClick(int i, int j)
        {


            for (int z = 0; z < Main.maxPlayers; z += 1)
            {
                if (Main.player[z].active && !Main.player[z].dead)
                {

                    if (SGAWorld.downedCaliburnGuardiansPoints > 0)
                    {
                        if (Main.myPlayer == z)
                            Main.NewText("You are worthy, claim your reward...", 50, 150, 50);
                        return true;
                    }

                    if ((Main.player[z].Center - new Vector2(i*16, j*16)).Length() < 160)
                    {
                        if (Main.player[z].statLife >= 200 && NPC.CountNPCS(mod.NPCType("CaliburnGuardian")) < 1)
                        {
                            if (SGAWorld.downedCaliburnGuardiansPoints > 0)
                            {
                            }
                            else
                            {
                                if (Main.netMode < 1)
                                {
                                    NPC.NewNPC(i * 16, j * 16, mod.NPCType("CaliburnGuardian"), 0, 0, 0, summontype);
                                }
                                else
                                {
                                    ModPacket packet = mod.GetPacket();
                                    packet.Write((ushort)999);
                                    packet.Write((int)(i * 16));
                                    packet.Write((int)(j * 16));
                                    packet.Write(ModContent.NPCType<NPCs.Wraiths.CaliburnGuardian>());
                                    packet.Write(0);
                                    packet.Write(0);
                                    packet.Write(summontype);
                                    packet.Write(0);
                                    packet.Write(z);
                                    packet.Send();

                                }
                                if (Main.myPlayer == z)
                                    Main.NewText("Let see if you're worthy...", 50, 150, 50);
                                return true;
                            }
                        }
                        else
                        {
                            if (Main.netMode < NetmodeID.Server && Main.myPlayer == z)
                                Main.NewText("You will not survive pulling this, come back when you have meat on your bones...", 100, 255, 100);
                        }
                    }

                }
            }


            return false;
        }

        public override bool CanKillTile(int i, int j, ref bool blockDamaged)
        {
            return (SGAWorld.downedCaliburnGuardiansPoints > 0);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            Player him=null;
            /*for (int z = 0; z < Main.maxPlayers; z += 1)
            {
                if (Main.player[z].active && !Main.player[z].dead)
                {
                    if ((Main.player[z].Center - new Vector2(i*16, j*16)).Length() < 200 && Main.player[z].statLife >= 220)
                    {
                        him=Main.player[z];
                    }
                }
            }*/
            if (SGAWorld.downedCaliburnGuardiansPoints>0 && !fail)
            {
                //him.Hurt(new PlayerDeathReason(), 1, him.direction, false, true);

                if (Main.tile[i, j].frameX == 0 && Main.tile[i, j].frameY == 0)
                {
                    //Item.NewItem(i * 16, j * 16, 48, 48, mod.ItemType(myitem), 1, false, 0, false, false);
                    NPC npc = new NPC();
                    npc.DropItemInstanced(new Vector2(i * 16, j * 16), new Vector2(48, 48), mod.ItemType(myitem),1,false);
                    SGAWorld.downedCaliburnGuardiansPoints -= 1;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        NetMessage.SendData(MessageID.WorldData);
                    }
                }
                return;
            }
        }
    }

    public class CaliburnAltarB : CaliburnAltar
    {
        public override string myitem => "CaliburnTypeB";
        public override int summontype => 1;
        public override void SetDefaults()
        {
            base.SetDefaults();
        }

    }

    public class CaliburnAltarC : CaliburnAltar
    {
        public override string myitem => "CaliburnTypeC";
        public override int summontype => 2;
        public override void SetDefaults()
        {
            base.SetDefaults();
        }
    enum MessageType : byte
    {
        ClientNPC
    }
}
