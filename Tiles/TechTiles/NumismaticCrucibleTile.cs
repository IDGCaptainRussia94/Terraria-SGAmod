
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Terraria.DataStructures;
//using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
using Terraria.ModLoader.IO;
using SGAmod.Items.Placeable.TechPlaceable;
using SGAmod.Items.Consumables;
using Idglibrary;

namespace SGAmod.Tiles.TechTiles
{
    public class NumismaticCrucibleTile : ModTile, IHopperInterface
    {
        public static NumismaticCrucibleTE FindNumismaticCrucibleTE(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i - tile.frameX / 18;
            int top = j - tile.frameY / 18;

            int index = ModContent.GetInstance<NumismaticCrucibleTE>().Find(left, top);
            if (index == -1)
            {
                return null;
            }
            NumismaticCrucibleTE alter = (NumismaticCrucibleTE)TileEntity.ByID[index];
            return alter;

        }

        public override void SetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileLavaDeath[Type] = false;
            Main.tileTable[Type] = false;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16 };
            TileObjectData.newTile.AnchorBottom = new Terraria.DataStructures.AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.StyleHorizontal = false;
            TileObjectData.newTile.DrawYOffset = 0;
            //TileObjectData.newTile.StyleWrapLimit = 36;
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<NumismaticCrucibleTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
            //adjTiles = new int[] { TileID.TinkerersWorkbench };
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Numismatic Crucible");
            //name.AddTranslation(GameCulture.Chinese, "烤炉");
            AddMapEntry(new Color(235, 216, 222), name);
        }

        public override void MouseOver(int i, int j)
        {
            //Main.LocalPlayer.showItemIconText = "This is test, Demetri!";
            //

            //Main.LocalPlayer.showItemIcon2 = ModContent.ItemType<Items.Consumable.Debug1>();
            //nil
        }

        public override bool NewRightClick(int i, int j)
        {
            /*int dust = Dust.NewDust(new Vector2(i, j) * 16, 0, 0, DustID.PurpleCrystalShard);
            Main.dust[dust].scale = 3f;
            Main.dust[dust].noGravity = true;*/

            Tile tile = Framing.GetTileSafely(i, j);

            //int x = i - (tile.frameX / 18);
            //int y = j - (tile.frameY / 18);
            NumismaticCrucibleTE alter = FindNumismaticCrucibleTE(i, j);
            if (alter != null)
                alter.Interact(Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem], out _, Main.LocalPlayer);

            return true;
        }
        public bool HopperInputItem(Item item, Point tilePos, int movementCount, ref bool testOnly)
        {
            NumismaticCrucibleTE alter = FindNumismaticCrucibleTE(tilePos.X, tilePos.Y);
            if (alter != null)
            {
                if (testOnly)
                {
                    return true;
                }
                return alter.Interact(item, out _, null, true);
            }

            return false;
        }

        public bool HopperExportItem(ref Item item, Point tilePos, int movementCount, ref bool testOnly)
        {
            NumismaticCrucibleTE alter = FindNumismaticCrucibleTE(tilePos.X, tilePos.Y);
            if (alter != null && !alter.heldItem.IsAir && !alter.AcceptItem(alter.heldItem))
            {
                Item takeOutItem;
                bool foundItem = alter.Interact(item, out takeOutItem, null, onlyTakeOut: true, ejectOnly: true);

                if (foundItem)
                    item = takeOutItem;
                return foundItem;
            }

            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<NumismaticCrucibleTE>().Kill(i, j);
            Item.NewItem(i * 16, j * 16, 64, 16, ModContent.ItemType<NumismaticCrucibleItem>(), 1, false, 0, false, false);
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
            Tile tile = Framing.GetTileSafely(i, j);
            Texture2D tex = Main.tileTexture[tile.type];
            if (Main.tile[i, j].type == base.Type)
            {
                Vector2 offset = zerooroffset + (new Vector2(i, j) * 16);
                spriteBatch.Draw(tex, offset - Main.screenPosition, new Rectangle((tile.frameX / 18) * 18, (tile.frameY / 18) * 18, 18, 18), Color.White.MultiplyRGBA(Lighting.GetColor(i, j)), 0, Vector2.Zero, new Vector2(1f, 1f), SpriteEffects.None, 0f);
            }
            return true;
        }

        public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            Tile tilehere = Main.tile[x, y];
            if (tilehere.type == base.Type)
            {
                if (tilehere.frameX == 0 && tilehere.frameY == 0)
                {
                    if (nextSpecialDrawIndex < Main.specX.Length)
                    {
                        Main.specX[nextSpecialDrawIndex] = x;
                        Main.specY[nextSpecialDrawIndex] = y;
                        nextSpecialDrawIndex += 1;
                    }
                }
            }
        }

        public override void SpecialDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Vector2 zerooroffset = Main.drawToScreen ? Vector2.Zero : new Vector2((float)Main.offScreenRange);
            Tile tile = Framing.GetTileSafely(i, j);
            if (Main.tile[i, j].type == base.Type)
            {
                if (tile.frameX == 0 && tile.frameY == 0)
                {
                    NumismaticCrucibleTE crucibleTE = FindNumismaticCrucibleTE(i, j);
                    if (crucibleTE != null)
                    {

                        Vector2 offset = zerooroffset + (new Vector2(i, j) * 16) + new Vector2(32, 32);

                        if (!crucibleTE.heldItem.IsAir)
                        {
                            int npctype = ((SoulJar)crucibleTE.heldItem.modItem).npcTypeToUse;
                            if (npctype >= 0)
                            {

                                Texture2D npctex = Main.npcTexture[npctype];
                                if (npctex == null && npctype<Main.maxNPCTypes)
                                    npctex = ModContent.GetTexture("Terraria/NPC_" + npctype);


                                if (npctex != null)
                                {

                                    int count = npctex.Height / Main.npcFrameCount[npctype];

                                    Vector2 framesize = new Vector2(npctex.Width, count);

                                    spriteBatch.Draw(npctex, offset - new Vector2(16, 16) - Main.screenPosition, new Rectangle(0, (int)framesize.Y * (int)((Main.GlobalTime * 10) % Main.npcFrameCount[npctype]), (int)framesize.X, (int)framesize.Y), Color.White.MultiplyRGBA(Lighting.GetColor(i, j)), Main.GlobalTime / 2f, framesize / 2f, 32f / framesize.Y, SpriteEffects.None, 0f);
                                }
                            }
                        }

                        //Main.NewText("test");
                        Texture2D bartex = Main.colorBarTexture;
                        spriteBatch.Draw(bartex, offset - Main.screenPosition, null, Color.White.MultiplyRGBA(Lighting.GetColor(i, j)), -MathHelper.PiOver2, new Vector2(0, 0), new Vector2(32f / bartex.Width, 0.5f), SpriteEffects.None, 0f);
                        spriteBatch.Draw(bartex, offset - new Vector2(32, 32) - Main.screenPosition, new Rectangle(0, 0, bartex.Width, bartex.Height), Color.White.MultiplyRGBA(Lighting.GetColor(i, j)), 0, new Vector2(0, bartex.Height), new Vector2(32f / bartex.Width, 0.25f), SpriteEffects.None, 0f);
                        if (crucibleTE.StoredMoney > 0)
                        {
                            float percent = (crucibleTE.StoredMoney / (float)crucibleTE.MaxStoredMoney);
                            //SGAmod.Instance.Logger.Debug("the z: " + crucibleTE.StoredMoney);
                            //SGAmod.Instance.Logger.Debug("the xx: " + crucibleTE.MaxStoredMoney);

                            spriteBatch.Draw(bartex, offset - Main.screenPosition, new Rectangle(0, 0, (int)(bartex.Width * percent), bartex.Height), Color.Goldenrod.MultiplyRGBA(Lighting.GetColor(i, j)), -MathHelper.PiOver2, new Vector2(0, 0), new Vector2(32f / bartex.Width, 0.5f), SpriteEffects.None, 0f);
                        }
                        if (crucibleTE.npcValue > 0)
                        {
                            float percent = (crucibleTE.chargingProcess / (float)crucibleTE.npcValue);
                            spriteBatch.Draw(bartex, offset - new Vector2(32, 32) - Main.screenPosition, new Rectangle(0, 0, (int)(bartex.Width * percent), bartex.Height), Color.Lime.MultiplyRGBA(Lighting.GetColor(i, j)), 0, new Vector2(0, bartex.Height), new Vector2(32f / bartex.Width, 0.25f), SpriteEffects.None, 0f);
                        }

                    }

                    //Put stuff here later


                }
            }
        }
    }

        public class NumismaticCrucibleTE : LuminousAlterTE
    {
        public NPC NPCtoSpawn
        {
            get
            {
                SoulJar heldjar = heldItem.modItem as SoulJar;
                if (heldjar != null)
                {
                    NPC npc = new NPC();
                    npc.SetDefaults(heldjar.npcTypeToUse);
                    return npc;
                }

                return null;
            }

        }
        public override int ProcessRate => 50;
        public int InterestCost => 50;
        public int npcValue = -1;
        protected int maxStoredMoney = Item.buyPrice(0,1);
        public int MaxStoredMoney
        {
            get
            {
                return maxStoredMoney * 1;
            }
            set
            {
                maxStoredMoney = value;
            }
        }
        protected int storedMoney = 0;
        public int StoredMoney
        {
            get
            {
                return storedMoney;
            }
            set
            {
                storedMoney = value;
            }
        }

        public override bool Interact(Item item, out Item takeOutItem, Player player = null, bool noTakeOut = false, bool onlyTakeOut = false, bool ejectOnly = false)
        {
            takeOutItem = null;
            if (!item.IsAir)
            {
                bool money = item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin || item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin;
                if (money && AcceptItem(item))
                {
                    return InsertItem(item,player);
                }

            }
            return base.Interact(item, out takeOutItem, player, noTakeOut, onlyTakeOut, ejectOnly);
        }

        public override bool AcceptItem(Item item)
        {

            bool money = item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin || item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin;

            bool jar = item.type == ModContent.ItemType<SoulJarFull>();

            if (item != null && (money || (jar && item.modItem is SoulJar)))
            {
                if (jar)
                {
                    SoulJar jaris = item.modItem as SoulJar;
                    NPC npc = new NPC();
                    npc.SetDefaults(jaris.npcTypeToUse);
                    if (npc.value < 1)
                    {
                        return false;
                    }
                }
                if (money)
                {
                    //Main.NewText((int)((item.value * item.stack) * 0.20));
                    if (storedMoney + (int)((item.value * item.stack) * 0.20) > MaxStoredMoney)
                    {
                        return false;
                    }
                }

                return true;

            }
            return false;
        }

        public override bool InsertItem(Item item,Player player)
        {
            Item playerItem = item;

            bool money = item.type == ItemID.CopperCoin || item.type == ItemID.SilverCoin || item.type == ItemID.GoldCoin || item.type == ItemID.PlatinumCoin;
            bool jar = item.type == ModContent.ItemType<SoulJarFull>();

            if (!playerItem.IsAir)
            {
                if (money)
                {
                    storedMoney += (int)((playerItem.value * playerItem.stack)*0.20);
                    SoundEffectInstance sound = Main.PlaySound(SoundID.CoinPickup, Position.X * 16, Position.Y * 16);
                    if (sound != null)
                        sound.Pitch += 0.50f;
                }
                if (jar)
                {
                    heldItem = playerItem.Clone();
                    npcValue = 0;
                    chargingProcess = 0;
                    SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, Position.X * 16, Position.Y * 16);
                    if (sound != null)
                        sound.Pitch += 0.50f;
                }
                playerItem.TurnToAir();

                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendTileSquare(Main.myPlayer, Position.X, Position.Y, 2);
                    //NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, Position.X, Position.Y, Type, 0f, 0, 0, 0);
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);

                }
                return true;
            }
            return false;
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            ItemIO.Send(heldItem, writer);
            writer.Write((int)chargingProcess);
            writer.Write((int)npcValue);
            writer.Write((int)storedMoney);

        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            ItemIO.Receive(heldItem, reader);
            chargingProcess = (int)reader.ReadInt32();
            npcValue = (int)reader.ReadInt32();
            storedMoney = reader.ReadInt32();

        }

        public override TagCompound Save()
        {
            TagCompound baseCompound = base.Save();
            baseCompound.Add("storedMoney", (int)storedMoney);

            return baseCompound;

        }
        public override TagCompound DoLoad(TagCompound tag)
        {
            TagCompound baseCompound = base.DoLoad(tag);
            storedMoney = baseCompound.GetInt("storedMoney");

            return baseCompound;
        }

        public override bool TileCheck(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            bool valid = tile.active() && tile.frameX == 0 && tile.frameY == 0 && tile.type == ModContent.TileType<NumismaticCrucibleTile>();
            if (!valid)
            {
                DebugText("Deleted");
            }
            return valid;// && tile.frameX % 36 == 0 && tile.frameY == 0;
        }

        public NumismaticCrucibleTE() : base()
        {

        }

        public void SimulateNPCDeath()
        {
            Vector2 there = (Position.ToVector2() + new Vector2(1f, -0f)) * 16;
            NPC tempnpc = NPCtoSpawn;
            tempnpc.Center = there;
            //tempnpc.value = 1;

            List<Item> before = Main.item.ToList();

            tempnpc.NPCLoot();

            List<Item> after = Main.item.Except(before).ToList();
            List<(int,Item)> itemsToSpawn = new List<(int, Item)>();

            foreach(Item newItem in after)
            {
                Item olditem = Main.item[newItem.whoAmI];
                bool money = newItem.type == ItemID.CopperCoin || newItem.type == ItemID.SilverCoin || newItem.type == ItemID.GoldCoin || newItem.type == ItemID.PlatinumCoin;
                if (money)
                {
                    newItem.TurnToAir();
                    NetMessage.SendData(MessageID.SyncItem, -1, -1, null, olditem.whoAmI);
                }
                else
                {
                    itemsToSpawn.Add((newItem.whoAmI, newItem));
                }
            }

            if (itemsToSpawn.Count > 0)
            {
                //Attempts to send a hopper output below it 1st, else the item is dropped
                Point checkCoords = new Point(Position.X, Position.Y + 2);

                Tile modtile = Framing.GetTileSafely(checkCoords.X, checkCoords.Y);

                foreach ((int,Item) itemtodroporHopper in itemsToSpawn)
                {
                    if (ModContent.GetModTile(modtile.type) is ModTile modTile)
                    {
                        if (modTile != null && modTile is IHopperInterface)
                        {
                            bool teststatus = false;
                            if ((modTile as IHopperInterface).HopperInputItem(itemtodroporHopper.Item2, checkCoords, 0, ref teststatus))
                            {
                                Item olditem = itemtodroporHopper.Item2;//Main.item[itemtodroporHopper.Item1];
                                olditem.TurnToAir();
                                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, olditem.whoAmI);
                            }
                        }
                    }

                }
            }

            //NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item2);

            HopperTile.CleanUpGlitchedItems();
        }

        public override void Update()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (updateTimer % 60 == 0)
                {
                    /*SGAmod.Instance.Logger.Debug("the item: " + (heldItem != null ? (""+heldItem.type) : " item is null"));
                    SGAmod.Instance.Logger.Debug("the money: " + StoredMoney);
                    SGAmod.Instance.Logger.Debug("the moneymax: " + MaxStoredMoney);
                    SGAmod.Instance.Logger.Debug("the process: " + chargingProcess);*/

                    //Main.NewText("the money: " + StoredMoney);
                    if (heldItem != null)
                    {
                        //Main.NewText("the item: "+ heldItem.type);

                        if (AcceptItem(heldItem))
                        {
                            if (npcValue < 1)
                            {
                                npcValue = (int)NPCtoSpawn.value;
                            }

                            int rate = (int)(ProcessRate * SGAConfig.Instance.InfusionTime);

                            if (StoredMoney > rate)
                            {
                                storedMoney -= rate;
                                chargingProcess += rate;
                                //Main.NewText("the ProcessRate: " + chargingProcess + " out of " + npcValue);;

                                if (chargingProcess >= npcValue)
                                {
                                    SimulateNPCDeath();
                                    chargingProcess -= npcValue;
                                }
                            }
                        }
                        NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, ID, Position.X, Position.Y);
                    }
                    //NetMessage.SendTileSquare(Main.myPlayer, Position.X, Position.Y, 2);
                }
            }

            base.Update();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction)
        {

            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                NetMessage.SendTileSquare(Main.myPlayer, i + 1, j + 1, 4);
                NetMessage.SendData(MessageID.TileEntityPlacement, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }

            int num = Place(i, j);
            DebugText("Placed");
            return num;
        }
    }
}