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

namespace SGAmod.Tiles.TechTiles
{
    public class LuminousAlter : ModTile,IHopperInterface
    {
        public static LuminousAlterTE FindAlterTE(int i, int j)
        {
            Tile tile = Main.tile[i, j];
            int left = i - tile.frameX / 18;
            int top = j - tile.frameY / 18;

            int index = ModContent.GetInstance<LuminousAlterTE>().Find(left, top);
            if (index == -1)
            {
                return null;
            }
            LuminousAlterTE alter = (LuminousAlterTE)TileEntity.ByID[index];
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
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<LuminousAlterTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
            adjTiles = new int[] { TileID.TinkerersWorkbench };
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Luminous Alter");
            //name.AddTranslation(GameCulture.Chinese, "烤炉");
            AddMapEntry(new Color(235, 216, 222), name);
        }

        public override void MouseOver(int i, int j)
        {
            //Main.LocalPlayer.showItemIconText = "This is test, Demetri!";
            Main.LocalPlayer.showItemIcon = true;

            SGAmod.LuminousAlterItems.TryGetValue(Main.LocalPlayer.HeldItem.type, out LuminousAlterItemClass litem);

            if (litem != null && litem != default)
            {
                Main.LocalPlayer.showItemIcon2 = litem.outputItem;
                Main.LocalPlayer.showItemIconText = "    "+litem.requiredText;
                return;
            }

            Main.LocalPlayer.showItemIcon2 = ModContent.ItemType<Items.Consumable.Debug1>();
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
            LuminousAlterTE alter = FindAlterTE(i, j);
            if (alter!=null)
            alter.Interact(Main.LocalPlayer.inventory[Main.LocalPlayer.selectedItem],out _, Main.LocalPlayer);

            return true;
        }
        public bool HopperInputItem(Item item, Point tilePos, int movementCount)
        {
            LuminousAlterTE alter = FindAlterTE(tilePos.X, tilePos.Y);
            if (alter != null)
            {
                return alter.Interact(item,out _, null,true);
            }

            return false;
        }

        public bool HopperExportItem(ref Item item, Point tilePos, int movementCount)
        {
            LuminousAlterTE alter = FindAlterTE(tilePos.X, tilePos.Y);
            if (alter != null && !alter.heldItem.IsAir && !alter.AcceptItem(alter.heldItem))
            {
                Item takeOutItem;
                bool foundItem = alter.Interact(item, out takeOutItem, null, onlyTakeOut: true,ejectOnly: true);

                if (foundItem)
                    item = takeOutItem;
                return foundItem;
            }

            return false;
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            ModContent.GetInstance<LuminousAlterTE>().Kill(i, j);
            Item.NewItem(i * 16, j * 16, 64, 16, ModContent.ItemType<LuminousAlterItem>(), 1, false, 0, false, false);
        }

        public override void DrawEffects(int x, int y, SpriteBatch spriteBatch, ref Color drawColor, ref int nextSpecialDrawIndex)
        {
            if (Main.tile[x, y].type == base.Type)
            {
                if (nextSpecialDrawIndex < Main.specX.Length)
                {
                    Main.specX[nextSpecialDrawIndex] = x;
                    Main.specY[nextSpecialDrawIndex] = y;
                    nextSpecialDrawIndex += 1;
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
                    Texture2D inner = Main.tileTexture[Type];
                    Texture2D star = ModContent.GetTexture("SGAmod/Tiles/TechTiles/LuminousAlterStar");
                    Rectangle rect = new Rectangle(0, (int)((Main.GlobalTime * 1) % 2) * (star.Height / 2), star.Width, star.Height / 2);
                    Rectangle rect2 = new Rectangle(0, (int)(((Main.GlobalTime * 1)+1) % 2) * (star.Height / 2), star.Width, star.Height / 2);
                    Vector2 offset = zerooroffset + (new Vector2(i, j) * 16) + new Vector2(16, -(0 + (float)Math.Sin(Main.GlobalTime) * 8));

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1, 1, 1));

                    Effect hallowed = SGAmod.HallowedEffect;


                    hallowed.Parameters["prismAlpha"].SetValue(1f);
                    hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
                    hallowed.Parameters["overlayAlpha"].SetValue(0.5f);
                    hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1.5f, 0.15f, 0f));
                    hallowed.Parameters["overlayMinAlpha"].SetValue(0f);

                    Color lightcolor = Lighting.GetColor(i, j);
                    float brightness = Math.Max((float)lightcolor.R, Math.Max((float)lightcolor.G, (float)lightcolor.B)) / 256f;
                    if (brightness<0.9f)
                    {
                        hallowed.Parameters["alpha"].SetValue(1f-brightness);
                        hallowed.Parameters["prismColor"].SetValue(Color.White.ToVector3());
                        hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -Main.GlobalTime / 1f, (Main.GlobalTime) / 16f));
                        hallowed.Parameters["rainbowScale"].SetValue(1.265f);
                        hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.1f,0.1f));
                        hallowed.CurrentTechnique.Passes["Prism"].Apply();

                        spriteBatch.Draw(inner, zerooroffset + (new Vector2(i, j) * 16) - Main.screenPosition, new Rectangle(0, 0, 16, 16), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        spriteBatch.Draw(inner, zerooroffset + (new Vector2(i + 1, j) * 16) - Main.screenPosition, new Rectangle(18, 0, 16, 16), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        spriteBatch.Draw(inner, zerooroffset + (new Vector2(i, j + 1) * 16) - Main.screenPosition, new Rectangle(0, 18, 16, 16), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                        spriteBatch.Draw(inner, zerooroffset + (new Vector2(i + 1, j + 1) * 16) - Main.screenPosition, new Rectangle(18, 18, 16, 16), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0f);
                    }

                    hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
                    hallowed.Parameters["alpha"].SetValue(Main.dayTime ? 0.25f : 0.30f);
                    hallowed.Parameters["prismColor"].SetValue(Main.hslToRgb((((i + j) / 10f) + Main.GlobalTime / 4f) % 1f, 1f, 0.75f).ToVector3());
                    hallowed.Parameters["overlayAlpha"].SetValue(1f);
                    hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1.5f, 0.15f, 0f));
                    hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.2f,0.2f));

                    for (float f = 3; f > 1f; f -= 0.25f)
                    {
                        hallowed.Parameters["rainbowScale"].SetValue(0.05f+(f/2f));
                        hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -Main.GlobalTime / 1f, (Main.GlobalTime) / 8f));
                        hallowed.CurrentTechnique.Passes["Prism"].Apply();
                        spriteBatch.Draw(star, offset - Main.screenPosition, rect, Color.White, 0, new Vector2(rect.Width, rect.Height) / 2f, new Vector2(f, f), SpriteEffects.None, 0f);
                    }
                    hallowed.Parameters["rainbowScale"].SetValue(1f);
                    hallowed.Parameters["overlayScale"].SetValue(new Vector2(1, 1));

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.CreateScale(1, 1, 1));

                    LuminousAlterTE alter = FindAlterTE(i, j);
                    if (alter != null)
                    {
                        if (alter.heldItem != null && !alter.heldItem.IsAir)
                        {
                            UncraftClass.DrawItem(spriteBatch, alter.heldItem.type, offset - Main.screenPosition, alter.heldItem.stack > 1 ? alter.heldItem.stack : 0);
                            return;
                        }
                    }
                    spriteBatch.Draw(star, offset - Main.screenPosition, rect, Color.White, 0, new Vector2(rect.Width, rect.Height) / 2f, new Vector2(1f, 1f), SpriteEffects.None, 0f);
                }
            }
        }
    }

    public class OneItemSlotTE : ModTileEntity
    {
        public Item heldItem;
        public int updateTimer = 0;

        public OneItemSlotTE()
        {
            heldItem = new Item();
            heldItem.TurnToAir();
        }

        public virtual bool Interact(Item item, out Item takeOutItem, Player player = null, bool noTakeOut = false, bool onlyTakeOut = false, bool ejectOnly = false)
        {
            takeOutItem = null;
            if (heldItem.IsAir)
            {
                if (!onlyTakeOut && item!=null && AcceptItem(item))
                {
                    InsertItem(item,player);

                    if (player!=null)
                    player.GetModPlayer<LuminousAlterPlayer>().AlterTileTE = this;

                    return true;
                }
                return false;
            }
            else
            {
                if (!noTakeOut)
                {
                    takeOutItem = ExitItem(ejectOnly);
                    return true;
                }
                return false;
            }
        }

        public virtual bool AcceptItem(Item item)
        {
            return true;
        }

        public virtual bool InsertItem(Item item,Player player)
        {
            return true;
        }
        public virtual Item ExitItem(bool ejectOnly = false)
        {
            return null;
        }

        public override void OnKill()
        {
            ExitItem();
        }

        public override void Update()
        {
            updateTimer += 1;
            //LuminousAlterTE.DebugText(""+updateTimer);
        }

        public virtual bool TileCheck(int i, int j)
        {
            return false;
        }

        public override bool ValidTile(int i, int j)
        {
            return TileCheck(i, j);
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            ItemIO.Send(heldItem, writer);
            writer.Write(updateTimer);
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            ItemIO.Receive(heldItem, reader);
            updateTimer = reader.Read();
        }

        public override TagCompound Save()
        {
            TagCompound tag = new TagCompound
            {
                { "heldItem", ItemIO.Save(heldItem) },
                {"updateTimer",updateTimer }
            };
            return tag;

        }
        public virtual TagCompound DoLoad(TagCompound tag)
        {
            ItemIO.Load(heldItem, tag.GetCompound("heldItem"));
            updateTimer = tag.GetInt("updateTimer");
            return tag;
        }
        public override void Load(TagCompound tag)
        {
            DoLoad(tag);
        }

    }

        public class LuminousAlterTE : OneItemSlotTE
    {
        public int chargingProcess = 0;
        public ushort clientChargingTimer = 0;
        private LuminousAlterItemClass tempItemData;
        public LuminousAlterItemClass itemData;
        public int ProcessRate => 60;
        public static void DebugText(string text)
        {
            //Main.NewText(text);
        }
        public override bool AcceptItem(Item item)
        {
            LuminousAlterItemClass findClass;

            if (item != null && Position.Y <= 400 && Collision.CanHitLine((Position.ToVector2()*16),1,1,new Vector2(Position.X*16,0),1,1) && SGAmod.LuminousAlterItems.TryGetValue(item.type, out findClass))
            {
                if (item.stack >= findClass.stackCost && findClass.SpecialCondition())
                {
                    tempItemData = findClass;
                    return true;
                }

            }
            return false;
        }

        public override bool InsertItem(Item item,Player player)
        {
            Item playerItem = item;
            if (!playerItem.IsAir)
            {
                itemData = tempItemData;
                heldItem = playerItem.DeepClone();
                chargingProcess = 0;
                playerItem.TurnToAir();
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, Position.X * 16, Position.Y * 16);
                if (sound != null)
                    sound.Pitch += 0.50f;

                if (Main.netMode == 1)
                {
                    NetMessage.SendTileSquare(Main.myPlayer, Position.X, Position.Y, 2);
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, Position.X, Position.Y, Type, 0f, 0, 0, 0);
                }
                return true;
            }
            return false;
        }
        public override Item ExitItem(bool ejectOnly = false)
        {
            if (!heldItem.IsAir)
            {
                Item thisOne;
                if (!ejectOnly)
                {
                    int item2 = Item.NewItem(Vector2.Zero, Vector2.Zero, heldItem.type, heldItem.stack);
                    Main.item[item2] = heldItem.DeepClone();
                    Main.item[item2].favorited = false;
                    Main.item[item2].newAndShiny = false;
                    Main.item[item2].position = (Position.ToVector2() + new Vector2(0.5f, -2f)) * 16;
                    thisOne = Main.item[item2];
                }
                else
                {
                    thisOne = heldItem.DeepClone();
                }

                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, thisOne.whoAmI);

                heldItem.TurnToAir();
                SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_CrystalCartImpact, Position.X * 16, Position.Y * 16);
                if (sound != null)
                    sound.Pitch -= 0.50f;

                if (Main.netMode == 1)
                {
                    NetMessage.SendTileSquare(Main.myPlayer, Position.X, Position.Y, 2);
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, Position.X, Position.Y, Type, 0f, 0, 0, 0);
                }
                return thisOne;
            }
            return null;
        }

        public override void NetSend(BinaryWriter writer, bool lightSend)
        {
            ItemIO.Send(heldItem, writer);
            writer.Write(chargingProcess);
        }

        public override void NetReceive(BinaryReader reader, bool lightReceive)
        {
            ItemIO.Receive(heldItem, reader);
            chargingProcess = reader.ReadUInt16();
        }

        public override TagCompound Save()
        {
            TagCompound baseCompound = base.Save();
            baseCompound.Add("chargingProcess", chargingProcess);
            baseCompound.Add("clientChargingTimer", (int)clientChargingTimer);

            return baseCompound;

        }
        public override TagCompound DoLoad(TagCompound tag)
        {
            TagCompound baseCompound = base.DoLoad(tag);
            chargingProcess = baseCompound.GetInt("chargingProcess");
            clientChargingTimer = (ushort)baseCompound.GetInt("clientChargingTimer");
            SGAmod.LuminousAlterItems.TryGetValue(heldItem.type, out itemData);


            return baseCompound;
        }

        public override bool TileCheck(int i, int j)
        {
            Tile tile = Framing.GetTileSafely(i, j);
            bool valid = tile.active() && tile.frameX == 0 && tile.frameY == 0 && tile.type == ModContent.TileType<LuminousAlter>();
            if (!valid)
            {
                DebugText("Deleted");
            }
            return valid;// && tile.frameX % 36 == 0 && tile.frameY == 0;
        }

        public LuminousAlterTE() : base()
        {

        }

        public void ItemInfusion()
        {
            Vector2 there = (Position.ToVector2() + new Vector2(0.5f, -2f)) * 16;
            int output = itemData.outputItem;
            if (output == ItemID.FragmentSolar)
            {
                output = (new int[] { ItemID.FragmentSolar, ItemID.FragmentVortex, ItemID.FragmentNebula, ItemID.FragmentStardust})[Main.rand.Next(4)];
            }
            int item2 = Item.NewItem(there, Vector2.Zero, output, itemData.stackMade);
            Main.item[item2].favorited = false;
            Main.item[item2].newAndShiny = true;
            //Main.item[item2].position = (Position.ToVector2() + new Vector2(0.5f, -2f)) * 16;
            heldItem.stack -= itemData.stackCost;

            if (heldItem.stack < 1)
            {
                heldItem.TurnToAir();
                InsertItem(Main.item[item2], null);
            }
            else
            {

                //Attempts to send a hopper output below it 1st, else the item is dropped
                Point checkCoords = new Point(Position.X, Position.Y+2);

                Tile modtile = Framing.GetTileSafely(checkCoords.X, checkCoords.Y);

                if (ModContent.GetModTile(modtile.type) is ModTile modTile)
                {
                    if (modTile != null && modTile is IHopperInterface)
                    {
                        (modTile as IHopperInterface).HopperInputItem(Main.item[item2], checkCoords, 0);
                    }
                }

                NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item2);
            }
        }

        public override void Update()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (updateTimer % 60 == 0)
                {
                    if (heldItem != null && itemData != default)
                    {
                        if (AcceptItem(heldItem))
                        {
                            chargingProcess += (int)(ProcessRate*SGAConfig.Instance.InfusionTime);
                            if (chargingProcess >= itemData.infusionTime)
                            {
                                ItemInfusion();

                                clientChargingTimer = 60;
                                chargingProcess -= itemData.infusionTime;
                            }
                            if (chargingProcess>0)
                            clientChargingTimer = 150;
                        }
                    }
                    //NetMessage.SendTileSquare(Main.myPlayer, Position.X, Position.Y, 2);
                    NetMessage.SendData(MessageID.TileEntitySharing, -1, -1, null, Position.X, Position.Y, Type, 0f, 0, 0, 0);
                }
            }

            if (Main.netMode < NetmodeID.Server)
            {
                if (clientChargingTimer > 0)
                {
                    if (clientChargingTimer == 120)
                    {
                        if (itemData != null)
                        {
                            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, Position.X * 16, Position.Y * 16);
                            if (sound != null)
                                sound.Pitch = MathHelper.Clamp(-0.75f + ((clientChargingTimer / (float)itemData.infusionTime) * 1.50f),-0.75f,0.75f);
                        }
                    }

                    if (clientChargingTimer % 6 == 0)
                    {
                        int dust = Dust.NewDust(new Vector2(Position.X, Position.Y) * 16, 16, 16, DustID.PurpleCrystalShard);
                        Main.dust[dust].scale = 3f;
                        Main.dust[dust].noGravity = true;
                    }
                    clientChargingTimer -= 1;
                }
            }

            base.Update();
        }

        public override void PostGlobalUpdate()
        {
            /*if (Main.netMode < NetmodeID.Server)
            {
                if (clientChargingTimer > 0)
                {
                    if (clientChargingTimer == 120)
                    {
                        if (itemData != null)
                        {
                            SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, Position.X * 16, Position.Y * 16);
                            if (sound != null)
                                sound.Pitch = -0.75f + ((clientChargingTimer / (float)itemData.infusionTime) * 1.50f);
                        }
                    }

                    if (clientChargingTimer % 6 == 0)
                    {
                        int dust = Dust.NewDust(new Vector2(Position.X, Position.Y) * 16, 16, 16, DustID.PurpleCrystalShard);
                        Main.dust[dust].scale = 3f;
                        Main.dust[dust].noGravity = true;
                    }
                    clientChargingTimer -= 1;
                }
            }*/
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

    public class LuminousAlterPlayer : ModPlayer
    {
        public ModTileEntity AlterTileTE;
        int counter = 0;

        public override void PreUpdateMovement()
        {
            counter += 1;
            //If your UI is visble, for some reason SGAmod would not let me access theirs despite it existing :/
            //if (SGAmod.CustomUIMenu.visible)
            //{
            if (AlterTileTE == null || (((AlterTileTE.Position.ToVector2() + new Vector2(1f, 0)) * 16) - player.Center).Length() > 120)
            {
                if (AlterTileTE != null)
                    LuminousAlterTE.DebugText("Untoggled");
                AlterTileTE = null;

                //Un-toggle UI here


            }
        }

    }


}