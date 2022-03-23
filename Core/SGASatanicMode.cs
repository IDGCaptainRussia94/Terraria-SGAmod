using Microsoft.Xna.Framework;
using System.Linq;
using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Idglibrary;
using ReLogic.Graphics;
using SGAmod.Items.Consumables;

namespace SGAmod
{
    public partial class SGAmod : Mod
    {
        public static float overpoweredModBaseValue = 0f;
        public static float overpoweredModBaseHardmodeValue = 0f;
        public static bool playCheatEffectOnce = false;

        internal static bool cheating = false;
        internal static bool TotalCheating => Main.netMode == NetmodeID.SinglePlayer && !Main.gameMenu && (Main.LocalPlayer.SGAPly().SatanPlayer || cheating || SGAWorld.cheating || AprilFoolsMode);

        internal static bool DevDisableCheating => Main.netMode != NetmodeID.SinglePlayer || (Main.LocalPlayer != null && Main.LocalPlayer.HasItem(ModContent.ItemType<Debug13>()) && Main.LocalPlayer.inventory[49].type == ModContent.ItemType<Debug13>());
        internal static bool DRMMode
        {
            get
            {
                return Main.netMode == NetmodeID.SinglePlayer && (SGAWorld.NightmareHardcore > 0 || (!DevDisableCheating && (TotalCheating)));
            }
        }
        internal static double EndTimes => 60 * 60 * 24.0;
        internal static double LocalPlayerPlayTime => Main.ActivePlayerFileData.GetPlayTime().TotalSeconds;
        internal static float PlayingPercent => AprilFoolsMode ? 2.5f : MathHelper.Clamp((float)(LocalPlayerPlayTime / EndTimes), 0f, 1f);
        internal static float OverpoweredMod
        {
            get
            {
                //Main.NewText("test: "+Main.ActivePlayerFileData.GetPlayTime().TotalSeconds);
                float scaleOverTime = MathHelper.Clamp(PlayingPercent, 0f, 1f);
                return Main.netMode == NetmodeID.SinglePlayer && (SGAConfig.Instance.OPmods || ((TotalCheating) && !DevDisableCheating)) ? (overpoweredModBaseValue + (Main.hardMode ? overpoweredModBaseHardmodeValue : 0f))
                    * scaleOverTime
                    : 0;
            }
        }

        public void CalcOpMods()
        {
            overpoweredModBaseValue = ((ModLoader.GetMod("AlchemistNPC") != null ? 0.75f : 0) + (ModLoader.GetMod("Luiafk") != null ? 1.5f : 0) + (ModLoader.GetMod("Fargowiltas") != null ? 0.5f : 0) + (ModLoader.GetMod("FargowiltasSouls") != null ? 1.5f : 0)) + (ModLoader.GetMod("Antisocial") != null ? 7.5f : 0) + (ModLoader.GetMod("AlchemistNPCLite") != null ? 0.75f : 0) + (ModLoader.GetMod("AlchemistNPC") != null ? 1.5f : 0);
            overpoweredModBaseHardmodeValue = (ModLoader.GetMod("Wingslot") != null ? 0.75f : 0);

            overpoweredModBaseValue += (Calamity.Item1 ? 2f : 0)+ (Thorium.Item1 ? 0.50f : 0);
            overpoweredModBaseHardmodeValue += (Calamity.Item1 ? 1f : 0);

        }

    }



    class SGASatanicGlobalItem : GlobalItem
    {
        public static void RemoveCheatItemCraftingRecipes()
        {
            Mod luiafk = ModLoader.GetMod("Luiafk");
            if ((luiafk != null))
            {
                for (var i = 0; i < Recipe.numRecipes; i++)
                {
                    Recipe recipe = Main.recipe[i];
                    Item itemz = recipe.createItem;
                    if (itemz.modItem != null)
                    {
                        ModItem mitem = itemz.modItem;
                        SGAmod.Instance.Logger.Debug("has mod item: " + mitem.GetType().FullName);
                        string nullname = mitem.GetType().Namespace;
                        if (nullname.Length - nullname.Replace("Images.Items.Potions", "").Length > 0)
                        {
                            recipe.RemoveRecipe();
                        }
                    }

                }
            }
        }

        public bool IsItemCheating(Item item)
        {
            if (SGAmod.DevDisableCheating && Dimensions.SGAPocketDim.WhereAmI == null)
                return false;

            if ((SGAmod.Fargos.Item1 || SGAmod.Luiafk.Item1))
            {
                if (item.modItem != null)
                {
                    Type modtype = item.modItem.GetType();
                    string nullname = modtype.Namespace;
                    if (nullname.Length - nullname.Replace("Fargowiltas.Items.Explosives", "").Length > 0)
                    {
                        if (modtype.Name != "BoomShuriken" && modtype.Name != "AutoHouse" && modtype.Name != "LihzahrdInstactuationBomb" && modtype.Name != "MiniInstaBridge")
                            return true;
                    }
                    if (nullname.Length - nullname.Replace("Items.AutoBuilders", "").Length > 0)
                    {
                        if (modtype.Name != "PrisonBuilder")
                            return true;
                    }
                }
            }
            return false;
        }

        public static bool DoCheatVisualEffect(Player player, bool cheatingOn = true)
        {
            if (!SGAmod.TotalCheating && !SGAmod.playCheatEffectOnce)
            {
                Main.NewText("You were warned", Color.Red);
                var snd = Main.PlaySound(SoundID.Item123, -1, -1);
                if (snd != null)
                {
                    snd.Pitch = -0.25f;
                }

                snd = Main.PlaySound(SoundID.Zombie, -1, -1, 96);
                if (snd != null)
                {
                    snd.Pitch = -0.5f;
                }

                string[] warnings = { "YOU", "WERE", "WARNED" };

                for (int i = 0; i < 100; i += 1)
                {
                    int timeIn = i * 10;
                    NPCs.Hellion.HellionInsanity youWereWarned = new NPCs.Hellion.HellionInsanity(warnings[i % warnings.Length], 100 + (i * 26), Main.rand.Next(1000, 1100) - timeIn);
                    youWereWarned.scale = Vector2.One * 1f;
                    youWereWarned.shaking = 8;
                    youWereWarned.flipped = MathHelper.Pi;
                    youWereWarned.timeIn = timeIn;
                    Dimensions.DimDungeonsProxy.madness.Add(youWereWarned);
                }

                ScreenExplosion explode = SGAmod.AddScreenExplosion(player.Center, 600, 1.5f, 1600);

                if (explode != null)
                {
                    explode.warmupTime = 16;
                    explode.decayTime = 200;
                    explode.strengthBasedOnPercent = delegate (float percent)
                    {
                        explode.where = Main.LocalPlayer.Center;
                        float fader = MathHelper.Clamp(percent, 0f, 2f - (percent * 2f)) * 1f;
                        float fader2 = MathHelper.Clamp(percent, 0f, 3f - (percent * 3f)) * 1f;

                        if (SGAmod.ScreenShake < percent * 16f)
                            SGAmod.AddScreenShake((0.5f + (percent * 2f)) * MathHelper.Clamp(8f - (percent * 8f), 0f, 1f));

                        if (SGAWorld.modtimer % 30 == 0 && percent < 0.75f)
                        {
                            SGAmod.AddScreenShake(4);
                            var sound = Main.PlaySound(SGAmod.Instance.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Heartbeat").WithPitchVariance(.025f), new Vector2(-1, -1));
                            if (sound != null)
                            {
                                sound.Pitch -= (0.20f + (percent * 0.70f));
                            }
                        }
                        if (SGAWorld.modtimer % 60 == 0 && percent < 0.99f && percent > 0.4f)
                        {
                            var sound = Main.PlaySound(SoundID.Zombie, -1, -1, 105);
                            if (sound != null)
                            {
                                sound.Pitch = 0.90f-Terraria.Utils.InverseLerp(0.4f,0.99f, percent, true)*1.80f;
                            }
                        }

                        explode.alpha = MathHelper.Clamp((float)Math.Pow((percent * 3.15f) - 2f, 1f), 0.1f, 1f);

                        float glow = MathHelper.SmoothStep(0f, 0.80f, (float)Math.Pow(fader * 1.5f, 2.5f));
                        float glow2 = MathHelper.SmoothStep(0f, 1f, (float)Math.Pow(fader * 2.5f, 1.16f));

                        Main.BlackFadeIn = (int)(((glow2 * 255f) * MathHelper.Clamp(explode.alpha * 1.45f, 0f, 1f)) * MathHelper.Clamp(percent * 5f, 0f, 1f));

                        if (fader > 0.5f)
                            MoonlordDeathDrama.RequestLight(glow * MathHelper.Clamp((percent * 1.5f) - 0.75f, 0f, 1f), player.Center);

                        return fader * 1f;

                    };

                }

                if (cheatingOn)
                    SGAmod.cheating = true;
                return true;
            }
            return false;
        }
        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (line.mod == "SGAmod" && line.Name == "CheatingReadThis")
            {
                float alpha = MathHelper.Clamp((float)Math.Sin(Main.GlobalTime * 1.5f), 0f, 1f);
                Vector2 vec = Main.rand.NextVector2Circular(6f, 6f) * alpha;
                line.X += (int)vec.X;
                line.Y += (int)vec.Y;

                ThereIsNoMercyThereIsNoInnocenceOnlyDegreesOfGuilt.DrawnLine(line, ref yOffset);
                DynamicSpriteFontExtensionMethods.DrawString(Main.spriteBatch, Main.fontMouseText, line.text, new Vector2(line.X, line.Y), Color.White * ((1f - alpha) * 0.60f), 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                //Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y+64f), Color.White * (1f - alpha));
                return false;
            }
            return true;
        }

        public bool CheckForCheatyPlaythroughRuiningItems(Item item, Player player)
        {
            return true;
            if (IsItemCheating(item))
            {
                if (Dimensions.SGAPocketDim.WhereAmI != null)
                {
                    return false;
                }
                return DoCheatVisualEffect(player);

            }
            return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            return true;

            if (!SGAmod.cheating && !SGAWorld.cheating)
            {
                if (IsItemCheating(item))
                {
                    if (player.controlUp)
                    {
                        if (CheckForCheatyPlaythroughRuiningItems(item, player))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return true;
        }
    }

    public partial class SGASatanicNPCs : GlobalNPC
    {

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void AI(NPC npc)
        {

            if ((npc.friendly || NPCID.Sets.TownCritter[npc.type] || npc.catchItem > 0) && npc.aiStyle != 69 && !npc.townNPC && SGAmod.TotalCheating && npc.type != ModContent.NPCType<NPCs.TownNPCs.Draken>() && !SGAmod.DevDisableCheating && Main.rand.Next((int)SGAmod.LocalPlayerPlayTime) > 21600 && Main.rand.Next(150) < 1)
            {
                npc.aiStyle = 69;
                npc.damage = 25;
                npc.defDamage = 25;
                npc.dontTakeDamageFromHostiles = true;
                npc.friendly = false;
                npc.catchItem = -1;
            }
        }
    }

        public class ThereIsNoMercyThereIsNoInnocenceOnlyDegreesOfGuilt : Items.Consumables.TrueCopperWraithNotch
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Satanic Prayer");
            //Tooltip.SetDefault("");
        }

        public static bool DrawnLine(DrawableTooltipLine line, ref int yOffset)
        {
            Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.Black);

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

            Effect hallowed = SGAmod.HallowedEffect;

            for (float ff = 0; ff < MathHelper.TwoPi; ff += MathHelper.TwoPi / 4f)
            {
                //Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontDeathText, line.text, loc.X, loc.Y, Color.Red, Color.Black, Vector2.Zero, 1f);
                Vector2 loc = new Vector2(line.X, line.Y) + (ff).ToRotationVector2() * 2f;
                Utils.DrawBorderString(Main.spriteBatch, line.text, loc, Color.Red);
            }

            hallowed.Parameters["alpha"].SetValue(1f);
            hallowed.Parameters["prismAlpha"].SetValue(1f);
            hallowed.Parameters["prismColor"].SetValue(Color.Red.ToVector3());
            hallowed.Parameters["rainbowScale"].SetValue(0.25f);
            hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));
            hallowed.Parameters["overlayTexture"].SetValue(ModContent.GetTexture("SGAmod/Fire"));
            hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, -Main.GlobalTime / 14f, 0));
            hallowed.Parameters["overlayAlpha"].SetValue(1.5f);
            hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0.02f, Main.GlobalTime / 2f));
            hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
            hallowed.CurrentTechnique.Passes["PrismNoRainbow"].Apply();

            Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.Red * 0.25f);


            hallowed.Parameters["alpha"].SetValue(0.5f);
            hallowed.Parameters["prismAlpha"].SetValue(1f);
            hallowed.Parameters["prismColor"].SetValue(Color.Black.ToVector3());
            hallowed.Parameters["rainbowScale"].SetValue(0f);
            hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime / 11f, 0));
            hallowed.Parameters["overlayMinAlpha"].SetValue(0.20f);
            hallowed.CurrentTechnique.Passes["PrismNoRainbow"].Apply();

            Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White * 0.25f);


            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if (SGAmod.LocalPlayerPlayTime > 60 && !Main.LocalPlayer.SGAPly().SatanPlayer)
            {
                tooltips.Add(new TooltipLine(mod, "ItsOver", "'Your chance has passed...'"));
                return;
            }

            if (Main.LocalPlayer.SGAPly().SatanPlayer)
            {
                tooltips.Add(new TooltipLine(mod, "CheatingReadThis", "Trial Completion Percent: " + Math.Round(SGAmod.PlayingPercent*100f,2)+"%"));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Many curses of Nightmode mode are thrust onto you...")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Things become worse over time as the trial completes:")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Enemies are growing stronger")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "You suffer radiation per hit")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Buff Burning triggers with less buffs and causes you to lose your accessories for a short time")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Critters may try to kill you")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Sharkvern's Drowning Presence is active during rain and Cirno's Snowfrosted is active during blizzards")));
                return;
            }

            if (!SGAmod.TotalCheating)
            {
                tooltips.Add(new TooltipLine(mod, "CheatingReadThis", "'Walk the path of a true sadomasochist...'"));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Makes the game gradually hell over the course of 24 hours")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "Can only be toggled off after 24 hours of real time playtime")));
                tooltips.Add(new TooltipLine(mod, "Cheating", Idglib.ColorText(Color.Red, "can only be used on new players with under a minute of playtime")));
                tooltips.Add(new TooltipLine(mod, "CheatingReadThis", "This mode is EXTREMELY UNFAIR, only use if playing with overpowered/unbalanced mods!"));
            }
        }

        
        public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
        {
            if (line.mod == "Terraria" && line.Name != "Cheating" && line.Name != "CheatingReadThis")
            return DrawnLine(line, ref yOffset);
            return true;
        }
        

        public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            float scaleinandout = 3f + MathHelper.Clamp(1f + ((float)Math.Sin(Main.GlobalTime * 2f) * 2f), 0f, 3f) * 0.50f;
            Vector2 slotSize = new Vector2(52f, 52f) * scale;
            position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
            Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;

            slotSize.X /= 1.0f;
            slotSize.Y = -slotSize.Y / 4f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            int otherIndex = 0;
            for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
            {
                otherIndex += 1;
                spriteBatch.Draw(Main.itemTexture[item.type], drawPos + (Vector2.UnitX.RotatedBy(((f) + Main.GlobalTime * 2f) * (otherIndex % 2 == 0 ? 1f : -1f)) * (scaleinandout * 1f)), null, Color.Black * 0.15f, 0, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale / 2f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);

            otherIndex = 0;
            for (float f = 0; f < MathHelper.TwoPi; f += MathHelper.TwoPi / 12f)
            {
                otherIndex += 1;
                spriteBatch.Draw(Main.itemTexture[item.type], drawPos + (Vector2.UnitX.RotatedBy(((f) + Main.GlobalTime * 2f) * (otherIndex % 2 == 0 ? 1f : -1f)) * (scaleinandout + 0.5f)), null, Color.White * 0.30f, 0, Main.itemTexture[item.type].Size() / 2f, Main.inventoryScale / 2f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.UIScaleMatrix);
            return true;
        }

        public override Color? GetAlpha(Color lightColor)
        {
            return Color.White;
        }

        public override string Texture
        {
            get { return ("SGAmod/Items/Consumables/Repent"); }
        }

        public override void SetDefaults()
        {
            item.width = 14;
            item.height = 14;
            item.maxStack = 1;
            item.rare = ItemRarityID.Quest;
            item.value = Item.sellPrice(gold: 1);
            item.useStyle = 2;
            item.useAnimation = 32;
            item.useTime = 32;
            item.useTurn = true;
            //item.UseSound = SoundID.Item123;
            item.consumable = false;
        }

        public override bool CanUseItem(Player player)
        {
            return (SGAmod.LocalPlayerPlayTime<60 || (player.SGAPly().SatanPlayer && SGAmod.PlayingPercent >= 1f));
        }
        public override bool UseItem(Player player)
        {
            bool onOff = !player.SGAPly().SatanPlayer;

            if (SGAmod.PlayingPercent < 1f)
                onOff = true;

            player.SGAPly().ToogleSatanicMode(onOff);
            return true;
        }
    }


}
