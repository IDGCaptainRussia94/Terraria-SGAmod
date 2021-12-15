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
using Terraria.GameContent.UI;
using System.Reflection;
using SGAmod.Items.Weapons.Trap;
using SGAmod.Items.Accessories;
using Idglibrary;
using AAAAUThrowing;
using Terraria.Utilities;
using SGAmod.Buffs;
using SGAmod.Items.Armors;
using SGAmod.Items.Armors.Vibranium;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using SGAmod.Items.Armors.Dev;
using SGAmod.Items;
using SGAmod.Tiles.TechTiles;
using SGAmod.HavocGear.Items;

namespace SGAmod
{
    public class SGAGlobalItem : GlobalItem
    {
        public static string pboostertextbase2 = "While you have wing time, hold DOWN while flying to boost in a direction\nHold LEFT or RIGHT to cap your vertical speed and greatly increase horizontal fly speeds\nHold only DOWN to quickly fly upwards, else rapidly fall downwards with no wingtime left";
        public static string pboostertext = "";
        public static string pboostertextboost = "";
        public static string apocalypticaltext
        {
            get
            {
                Player player = Main.LocalPlayer;
                SGAPlayer modplayer = player.GetModPlayer<SGAPlayer>();
                int whichone = (int)Main.GlobalTime % 4;
                string[] theones = { "Melee", "Ranged", "Magic", "Throwing" };
                string text = Math.Round(modplayer.apocalypticalChance[whichone],2) + "% " + theones[whichone] + " Apocalyptical Chance";

                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    text += "\nApocalyptical Strength: " + (modplayer.apocalypticalStrength * 100f) + "%";
                    text += "\nAn Apocalyptical is when your crit-crits, resulting in 3X damage";
                    text += "\nItems and effects may add special effects on top of this";
                    text += "\nApocalyptical Strength however only really boosts these effects rather than the damage of the crit";
                    text += "\nStrength of 200% would only boost your damage increase up to 400% from 300%, but effects would be doubled";


                }
                else
                {
                    text += " (Hold LEFT CONTROL for more info)";

                }
                return text;

            }

        }
        public static string techtext
        {
            get
            {
                Player player = Main.LocalPlayer;
                SGAPlayer modplayer = player.GetModPlayer<SGAPlayer>();

                string text = "";

                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    text += "\nTech Weapons have their damage boosted by your tech multiplier";
                    text += "\nIf Electric Charge falls below 50% your damage will be reduced";
                    text += "\nDamage can fall down to -50% damage at no charge";
                    text += "\nAll Tech Weapons consume a small ammount of Electric Charge on use";

                }
                else
                {
                    text += " (Hold LEFT CONTROL for more info)";

                }
                return text;

            }

        }

        public override bool CanUseItem(Item item, Player player)
        {
            if ((player.SGAPly().ReloadingRevolver > 0) && item.damage > 0)
            {
                return false;
            }
            else
            {
                return base.CanUseItem(item, player);
            }
        }

        public override bool PreDrawTooltipLine(Item item, DrawableTooltipLine line, ref int yOffset)
        {
            if (item.modItem != null && item.modItem is IAuroraItem)
            {
                if (line.mod == "Terraria" && line.Name == "ItemName")
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

                    Effect hallowed = SGAmod.HallowedEffect;

                    Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

                    hallowed.Parameters["alpha"].SetValue(0.5f);
                    hallowed.Parameters["prismColor"].SetValue(Items.Placeable.TechPlaceable.LuminousAlterItem.AuroraLineColor.ToVector3());
                    hallowed.Parameters["rainbowScale"].SetValue(0.25f);
                    hallowed.Parameters["overlayScale"].SetValue(new Vector2(1f, 1f));
                    hallowed.Parameters["overlayTexture"].SetValue(SGAmod.PearlIceBackground);
                    hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime / 14f, 0));
                    hallowed.Parameters["overlayAlpha"].SetValue(1.5f);
                    hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0f, 0f));
                    hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
                    hallowed.CurrentTechnique.Passes["PrismNoRainbow"].Apply();

                    Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
                    return false;
                }
            }
            return true;
        }


        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (SGAmod.anysubworld)
            {
                if (item.type == ItemID.Bomb || item.type == ItemID.Dynamite || item.type == ItemID.BombFish || item.type == ItemID.StickyBomb || item.type == ItemID.BouncyBomb || item.type == ItemID.BouncyDynamite || item.type == ItemID.StickyDynamite)
                {
                    tooltips.Add(new TooltipLine(mod, "BombHint", "Use these to destroy blocking walls of spikes"));
                }
            }

            if (item.modItem != null)
            {
                if (item.owner > -1)
                {
                    SGAPlayer sgaply = (Main.player[item.owner].GetModPlayer<SGAPlayer>());
                    pboostertextboost = "\nCurrent boost: " + sgaply.SpaceDiverWings;
                    pboostertext = pboostertextbase2 + pboostertextboost;
                }
                var myType = (item.modItem).GetType();
                var n = myType.Namespace;
                string asastring = (string)n;
                //int ishavocitem = (asastring.Split('.').Length - 1);
                int ishavocitem = asastring.Length - asastring.Replace("HavocGear.", "").Length;
                if (ishavocitem > 0)
                {
                    Color c = Main.hslToRgb(0.9f, 0.5f, 0.35f);
                    tooltips.Add(new TooltipLine(mod, "HavocItem", Idglib.ColorText(c, "Former Havoc mod item")));
                }
                if (SGAmod.UsesPlasma.ContainsKey(item.type))
                {
                    Color c = Main.hslToRgb(0.7f, 0.15f, 0.7f);
                    tooltips.Add(new TooltipLine(mod, "PlasmaItem", Idglib.ColorText(c, "This weapon uses plasma cells for recharging")));
                }

                if (item.modItem is IManifestedItem)
                {
                    string tt = "This is a placeholder sprite";
                    Color c = Main.hslToRgb(0f, 0.75f, 0.7f);
                    tooltips.Add(new TooltipLine(mod, "Manifested Item", Idglib.ColorText(Color.Yellow, "This item is a manifestion of your armor set, bound to you")));
                }
                if (item.modItem is IMangroveSet)
                {
                    tooltips.Add(new TooltipLine(mod, "MangroveSet", "Having more of the mangrove weapon set in your inventory improves the damage"));
                    tooltips.Add(new TooltipLine(mod, "MangroveSet", "Up to 50% with all the weapons"));
                }

                if (item.modItem is INoHitItem)
                {
                    tooltips.Add(new TooltipLine(mod, "NoHitItem",Idglib.ColorText(Items.Placeable.Relics.SGAPlacableRelic.RelicColor,"No-hit exclusive")));
                }
                if (item.modItem is IRadioactiveItem radactive)
                {
                    string tt = "This is a placeholder sprite";
                    Color c = Main.hslToRgb(0f, 0.75f, 0.7f);
                    tooltips.Add(new TooltipLine(mod, "RadioactiveItemText", Idglib.ColorText(Color.Lime, "This item is Radioactive")));
                    if (radactive.RadioactiveHeld() > 0)
                    {
                        tooltips.Add(new TooltipLine(mod, "RadioactiveItemText", Idglib.ColorText(Color.Lime, "You suffer Radiation "+ radactive.RadioactiveHeld()+" while holding this")));
                    }
                    if (radactive.RadioactiveInventory() > 0)
                    {
                        tooltips.Add(new TooltipLine(mod, "RadioactiveItemText", Idglib.ColorText(Color.Lime, "You suffer Radiation " + radactive.RadioactiveInventory() + " while this is in your inventory")));
                    }
                }

                int ammoclip = Main.LocalPlayer.SGAPly().IsRevolver(item);
                if (ammoclip > 0)
                {
                    Color c = Main.hslToRgb(0.7f, 0.15f, 0.7f);
                    tooltips.Add(new TooltipLine(mod, "Clip Item", Idglib.ColorText(c, ammoclip == 2 ? "Counts as a revolver: Automatically Reloads itself when held" : "This weapon has a clip and requires manual reloading")));
                }

                if (item?.modItem is IRustBurnText)
                {
                    tooltips.Add(new TooltipLine(mod, "RustBurnText", RustBurn.RustText));
                }

                if (item?.modItem is IDankSlowText)
                {
                    tooltips.Add(new TooltipLine(mod, "DankSlowText", DankSlow.DankText));
                }
                if (item.modItem is IRadioactiveDebuffText)
                {
                    tooltips.Add(new TooltipLine(mod, "RadioactiveDebuffText", RadioDebuff.RadioactiveDebuffText));
                }
                if (item?.modItem is IDevItem)
                {
                    (string, string) dev = ((IDevItem)item.modItem).DevName();
                    Color c = Main.hslToRgb((float)(Main.GlobalTime / 4) % 1f, 0.4f, 0.45f);
                    tooltips.Add(new TooltipLine(mod, "IDG Dev Item", Idglib.ColorText(c, dev.Item1 + "'s "+(dev.Item2+ (dev.Item2 != "" ? " " : "")) + "dev weapon")));
                }


            }

            if (item.type == ItemID.ManaRegenerationPotion && (SGAConfig.Instance.ManaPotionChange || SGAWorld.NightmareHardcore>0))
            {
                tooltips.Add(new TooltipLine(mod, "ManaRegenPotionOPPlzNerf", Idglib.ColorText(Color.Red, "Mana Sickness decays very slowly")));
                tooltips.Add(new TooltipLine(mod, "ManaRegenPotionOPPlzNerf", Idglib.ColorText(Color.Red, "Max Mana is reduced by 60")));
            }

            if (SGAWorld.downedWraiths < 1)
            {

                RecipeFinder finder = new RecipeFinder();
                finder.AddTile(TileID.Furnaces);
                List<Recipe> reclist = finder.SearchRecipes();

                Recipe foundone = reclist.Find(rec => rec.createItem.type == item.type);

                if (foundone != null)
                {
                    Color c = Main.hslToRgb(0.5f, 0.10f, 0.1f);
                    tooltips.Add(new TooltipLine(mod, "Wraithclue", Idglib.ColorText(c, "Crafting this will anger something...")));
                }
            }
            if (item.type == ItemID.LunarBar)
            {
                if (SGAWorld.downedWraiths < 4)
                {
                    Color c = Main.hslToRgb(0.5f, 0.20f, 0.7f);
                    tooltips.Add(new TooltipLine(mod, "Wraithclue", Idglib.ColorText(c, "A very strong being has locked it away from your possession, talk to the guide")));
                }
            }

            if (item?.modItem is ITechItem)//Tech items
            {
                // Get the vanilla damage tooltip
                TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
                if (tt != null)
                {
                    string[] thetext = tt.text.Split(' ');
                    string newline = "";
                    List<string> valuez = new List<string>();
                    foreach (string text2 in thetext)
                    {
                        valuez.Add(text2 + " ");
                    }
                    valuez.Insert(1, "technological ");
                    foreach (string text3 in valuez)
                    {
                        newline += text3;
                    }
                    tt.text = newline;
                }
            }

        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ModContent.ItemType <Items.Armors.Desert.DesertHelmet>() && body.type == ModContent.ItemType<Items.Armors.Desert.DesertShirt>() && legs.type == ModContent.ItemType <Items.Armors.Desert.DesertPants>())
            {
                return "Desert";
            }
            if (head.type == ModContent.ItemType<HavocGear.Items.Armor.DankWoodHelm>() && body.type == ModContent.ItemType<HavocGear.Items.Armor.DankWoodChest>() && legs.type == ModContent.ItemType<HavocGear.Items.Armor.DankLegs>())
            {
                return "Dank";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Novus.UnmanedHood>() && body.type == ModContent.ItemType<Items.Armors.Novus.UnmanedBreastplate>() && legs.type == ModContent.ItemType<Items.Armors.Novus.UnmanedLeggings>())
            {
                return "Novus";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Novite.NoviteHelmet>() && body.type == ModContent.ItemType<Items.Armors.Novite.NoviteChestplate>() && legs.type == ModContent.ItemType<Items.Armors.Novite.NoviteLeggings>())
            {
                return "Novite";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Acid.AcidHelmet>() && body.type == ModContent.ItemType<Items.Armors.Acid.AcidChestplate>() && legs.type == ModContent.ItemType<Items.Armors.Acid.AcidLeggings>())
            {
                return "Acid";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Engineer.EngineerHead>() && body.type == ModContent.ItemType<Items.Armors.Engineer.EngineerChest>() && legs.type == ModContent.ItemType<Items.Armors.Engineer.EngineerLegs>())
            {
                return "Engineer";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Blazewyrm.BlazewyrmHelm>() && body.type == ModContent.ItemType<Items.Armors.Blazewyrm.BlazewyrmBreastplate>() && legs.type == ModContent.ItemType<Items.Armors.Blazewyrm.BlazewyrmLeggings>())
            {
                return "Blazewyrm";
            }
            if (head.type == ModContent.ItemType<HavocGear.Items.Armor.MangroveHelmet>() && body.type == ModContent.ItemType<HavocGear.Items.Armor.MangroveChestplate>() && legs.type == ModContent.ItemType<HavocGear.Items.Armor.MangroveGreaves>())
            {
                return "Mangrove";
            }                      
            if ((head.type == ModContent.ItemType<Items.Armors.HallowedVisor>() || head.type == ModContent.ItemType<Items.Armors.AncientHallowedVisor>()) && body.type == ItemID.HallowedPlateMail && body.type == ItemID.HallowedGreaves)
            {
                return "Hallowed";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Magatsu.MagatsuHood>() && body.type == ModContent.ItemType<Items.Armors.Magatsu.MagatsuRobes>() && legs.type == ModContent.ItemType<Items.Armors.Magatsu.MagatsuPants>())
            {
                return "Magatsu";
            }
            if (head.type == ModContent.ItemType<Items.Armors.JungleTemplar.JungleTemplarHelmet>() && body.type == ModContent.ItemType<Items.Armors.JungleTemplar.JungleTemplarChestplate>() && legs.type == ModContent.ItemType<Items.Armors.JungleTemplar.JungleTemplarLeggings>())
            {
                return "JungleTemplar";
            }            
            if (head.type == ModContent.ItemType<Items.Armors.SpaceDiver.SpaceDiverHelmet>() && body.type == ModContent.ItemType<Items.Armors.SpaceDiver.SpaceDiverChestplate>() && legs.type == ModContent.ItemType<Items.Armors.SpaceDiver.SpaceDiverLeggings>())
            {
                return "SpaceDiver";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Mandala.MandalaHood>() && body.type == ModContent.ItemType<Items.Armors.Mandala.MandalaChestplate>() && legs.type == ModContent.ItemType<Items.Armors.Mandala.MandalaLeggings>())
            {
                return "Mandala";
            }
            if (head.type == ModContent.ItemType<Items.Armors.Valkyrie.ValkyrieHelm>() && body.type == ModContent.ItemType<Items.Armors.Valkyrie.ValkyrieBreastplate>() && legs.type == ModContent.ItemType<Items.Armors.Valkyrie.ValkyrieLeggings>())
            {
                return "Valkyrie";
            }                 
            if (head.type == ModContent.ItemType<Items.Armors.Illuminant.IlluminantHelmet>() && body.type == ModContent.ItemType<Items.Armors.Illuminant.IlluminantChestplate>() && legs.type == ModContent.ItemType<Items.Armors.Illuminant.IlluminantLeggings>())
            {
                return "Illuminant";
            }                
            int[] vibraniumSet = { ModContent.ItemType<Items.Armors.Vibranium.VibraniumMask>(), ModContent.ItemType<Items.Armors.Vibranium.VibraniumHelmet>(), ModContent.ItemType<Items.Armors.Vibranium.VibraniumHeadgear>(), ModContent.ItemType<Items.Armors.Vibranium.VibraniumHood>(), ModContent.ItemType<Items.Armors.Vibranium.VibraniumHat>() };
            if (vibraniumSet.Any(testby => testby == head.type) && body.type == mod.ItemType("VibraniumChestplate") && legs.type == mod.ItemType("VibraniumLeggings"))
            {
                return "Vibranium";
            }
            if (!head.vanity && !body.vanity && !legs.vanity)
            {
                if (head.type == ModContent.ItemType <Items.Armors.Dev.MisterCreeperHead>() && body.type == ModContent.ItemType < Items.Armors.Dev.MisterCreeperBody>() && legs.type == ModContent.ItemType < Items.Armors.Dev.MisterCreeperLegs>())
                {
                    return "MisterCreeper";
                }
                if (head.type == mod.ItemType("IDGHead") && body.type == mod.ItemType("IDGBreastplate") && legs.type == mod.ItemType("IDGLegs"))
                {
                    return "IDG";
                }
                if (head.type == ModContent.ItemType<Items.Armors.Dev.JellybruHelmet>() && body.type == mod.ItemType("JellybruChestplate") && legs.type == mod.ItemType("JellybruLeggings"))
                {
                    return "Jellybru";
                }
            }
            return "";
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
            if (set == "Desert")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }
                player.setBonus = "Press the 'Toggle Recipe' (" + s + ") to Summon up a short-lived Sandstorm\n---"+Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 45 seconds")+ "\nIncreased movement, 10% crit, and 35% extra throwing velocity in a Sandstorm\nImmunity to Mighty Winds\nManifested weapon: Sand Tosser";
                player.buffImmune[BuffID.WindPushed] = true;
                sgaplayer.desertSet = true;
                if (Sandstorm.Happening && player.ZoneSandstorm)
                {
                    player.Throwing().thrownVelocity += 0.35f;
                    player.Throwing().thrownCrit += 10;
                    player.moveSpeed += 3f;
                    player.accRunSpeed += 4f;
                }
                sgaplayer.manifestedWeaponType = ModContent.ItemType<Items.Armors.Desert.ManifestedSandTosser>();
            }
            if (set == "Dank")
            {
                player.setBonus = "10% of the sum of all damage types is added to your current weapon's attack\nyou regen life faster while on the surface during the day";
                sgaplayer.Dankset = 3;
            }
            if (set == "Novus")
            {
                player.setBonus = "Novus items emit more light when used and deal 20% more damage\nGain an additional free Cooldown Stack";
                sgaplayer.Novusset = 3;
                sgaplayer.MaxCooldownStacks += 1;
            }
            if (set == "Novite")
            {
                player.setBonus = "Gain a movement bonus based on current charge\nWhen you take damage you discharge a chain bolt at a nearby enemy\nThis costs 750 Electic Charge and is scaled based on Defense and Technological Damage";
                sgaplayer.Noviteset = 3;
            }
            if (set == "Acid")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }

                player.setBonus = "Press the 'Toggle Recipe' (" + s + ") Hotkey to activate Hunger of Fames for a short time\nAll throwing weapons get coated in acid for the 1st hit, but resets your life regeneration" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stacks, adds 60 seconds") + "\nPoison, Venom, and Acid Burn all lower enemy defence by an extra 5\n ";
                sgaplayer.acidSet = (true, sgaplayer.acidSet.Item2);
            }
            if (set == "Engineer")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }
                player.setBonus = "Hold JUMP to hover at an expense to Electric Charge\nManifested weapon: Engie Controls\nPress the 'Toggle Recipe' (" + s + ") Hotkey to toggle jetpack mode";
                sgaplayer.manifestedWeaponType = ModContent.ItemType<Items.Armors.Engineer.ManifestedEngieControls>();
            }
            if (set == "Blazewyrm")
            {
                player.setBonus = "True melee crits create a very powerful explosion equal to triple the damage dealt\nEach strike requires a free Cooldown Stack, and adds one for 12 seconds\n20% increased melee damage against enemies inflicted with Thermal Blaze" +
                        "\nImmune to fireblocks as well as immunity to On Fire! and Thermal Blaze\nGain an additional free Cooldown Stack";
                player.fireWalk = true;
                player.buffImmune[BuffID.OnFire] = true;///
                player.buffImmune[mod.BuffType("ThermalBlaze")] = true;
                sgaplayer.Blazewyrmset = true;
                sgaplayer.MaxCooldownStacks += 1;
            }
            if (set == "Mangrove")
            {
                player.setBonus = "Crit throwing attacks grant Dryad's Blessing and spawn Mangrove Orbs from you that seek out enemies\nYou are limited to 4 of these Orbs at a time\nWhile you are in the jungle:\n-Greatly Increased regeneration" +
    "\n-Gain an additional Free Cooldown Stack";

                sgaplayer.Mangroveset = true;
                if (player.ZoneJungle)
                {
                    player.lifeRegen += 5;
                    player.SGAPly().MaxCooldownStacks += 1;
                }
            }
            if (set == "Hallowed")
            {
                player.setBonus = "25% faster throwing item use and chance to not consume throwable";
                sgaplayer.ThrowingSpeed += 0.25f;
                sgaplayer.Thrownsavingchance += 0.25f;
            }
            if (set == "Magatsu")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }
                player.setBonus = "Press the 'Toggle Recipe' (" + s + ") Hotkey to deploy a shadowy decoy to distract enemies\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 60 seconds") + "\nOnly one decoy may be active at a time\nManifest your own N0ll Watchers to sense nearby enemies and 'watch' them\nYou get one watcher per free minion slot\nScoring an Apocalyptical against a watched enemy spreads a copy of damage nearby\nN0ll Watchers won't see you as an enemy, thus won't give chase on sight\n+2% increased Apocalyptical Chance, +40% increased Apocalyptical Strength\nGain an additional free Cooldown Stack";

                Items.Armors.Magatsu.MagatsuHood.SetBonus(sgaplayer);
                sgaplayer.MaxCooldownStacks += 1;
            }

            if (set == "JungleTemplar")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }

                player.setBonus = "Press the 'Toggle Recipe' (" + s + ") Hotkey to activate Precurser's Power for a short time\nRepairs wounds at full speed even while moving, but consumes Electric Charge\nAlso during this, throwing damage is increased by Tech Damage scaling" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stacks, adds 80 seconds") + "\nYou gain the ability to run as if on Asphalt, also and fall down faster\nReflect 3X damage and gain knockback immunity while grounded" + Idglib.ColorText(Color.Red, "Gravity is increased, wings are less effective, Shield Break can happen")+ "\n" + Idglib.ColorText(Color.Red, "User is submerged in lava at low Electric Charge") + "\nGain an additional free Cooldown Stack";

                sgaplayer.MaxCooldownStacks += 1;
                sgaplayer.jungleTemplarSet.Item1 = true;
                Items.Armors.JungleTemplar.JungleTemplarHelmet.SetBonus(sgaplayer);
            }
            if (set == "SpaceDiver")
            {
                string text1 = Idglib.ColorText(Color.Red, "90% reduced breath meter regen");
                string text2 = Idglib.ColorText(Color.Red, "You've adapted to pressurized air, removing the armor set will greatly harm you");
                player.setBonus = "Receive Endurance and Defense based on breath left (40% Endurance and 100 Defense at full breath)\nTaking damage will drain your breath meter based on the faction of life lost\nReceive no damage when damaged with a full breath meter\nTechnological damage increased and Electric Consumption reduced by 25%\n" + SGAGlobalItem.pboostertext + text1 + " \n" + text2;
                sgaplayer.SpaceDiverset = true;
                sgaplayer.SpaceDiverWings += 0.5f;
                sgaplayer.techdamage += 0.25f;
                sgaplayer.electricChargeCost *= 0.75f;
            }
            if (set == "Mandala")
            {
                player.setBonus = "Makes you a disciple of the Shadowspirits\nOne will follow you and grant you its powers\nManifested weapon: Overseer's Tutelage";
                sgaplayer.mandalaSet.Item1 = true;
                sgaplayer.manifestedWeaponType = ModContent.ItemType<Items.Armors.Mandala.ManifestedMandalaControls>();
            }
            if (set == "Valkyrie")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }

                string text2 = "Press the 'Toggle Recipe' (" + s + ") Hotkey to active Ragnarök for a time based off current life regeneration\nThrowing speed and apocalyptical chance are increased"+ Idglib.ColorText(Color.Red,"But resets life regen while active")+"\n" + Idglib.ColorText(Color.Orange, "Requires 2 Cooldown stack, adds 150 seconds") + "\n";

                string text1 = "Gain a throwing damage increase based on your current life regen\nMale Characters gain 15% Endurance\nFemale Characters gain 20% more flight time\n"+text2;
                player.setBonus = text1 + "\nGain an additional free Cooldown Stack";
                sgaplayer.valkyrieSet.Item1 = true;
                sgaplayer.MaxCooldownStacks += 1;
            }
            if (set == "Illuminant")
            {
                string text1 = "Reduces all new Action Cooldown Stacks by 20%\nEach Active Action Cooldown Stack grants 4% (6% Summon) damage and 2% crit chance\nThere is a 25% chance to not add a new Action Cooldown Stack whenever one would be applied\nAll Vanilla Prefixes on accessories are twice as effective";
                player.setBonus = text1;
                sgaplayer.illuminantSet.Item1 = 5;
            }            
            if (set == "Vibranium")
            {
                string s = "Not Binded!";
                foreach (string key in SGAmod.ToggleRecipeHotKey.GetAssignedKeys())
                {
                    s = key;
                }
                string text1 = "Press the 'Toggle Recipe' (" + s + ") Hotkey to toggle an Asphalt skybridge below your feet\nYou can land on this bridge while falling down\nHold Down to fall through\nThis consumes electric charge while active, " + Idglib.ColorText(Color.Red, "and will trigger a shield break on deplete");

                s = "Not Binded!";

                string text2 = "If while holding the AutoSelect key, phase a wall around where you aim instead\nThis wall is treated as solid tiles in most cases\nThis consumes electric charge on activate and damage, will trigger a shield break on deplete";
                player.setBonus = text1 + "\n" + text2 + "\nGain an additional free Cooldown Stack";
                sgaplayer.MaxCooldownStacks += 1;
                sgaplayer.vibraniumSet = true;
                Items.Armors.Vibranium.VibraniumChestplate.VibraniumSetBonus(sgaplayer);
            }
            if (set == "MisterCreeper")
            {
                player.setBonus = "Any sword that doesn't shoot a projectile is swung 50% faster and deals crits when you are falling downwards\nWhen you take damage, you launch a damaging high velocity grenade at what hit you\nThese grenades are launched even during immunity frames if your touching an enemy\nDrinking a healing potion launches a ton of bouncy grenades in all directions" +
                    "\nTaking lethal damage will cause you to light your fuse, killing you IF you fail to kill anyone with your ending explosion in a few seconds!\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 180 seconds") + "\nCreeper's explosive throw and Stormbreaker are empowered\n \n ";
                sgaplayer.MisterCreeperset = true;
                sgaplayer.devempowerment[1] = 3;
            }
            if (set == "IDG")
            {
                player.setBonus = "Minions cause less immunity frames, the enemy targeted by a minion weapon is Digi-Cursed\nDigi-Curse causes enemies to take 10% increased damage from all sources\n" +
                    "Minion Damage builds up Digi-Stacks, which increase ranged damage\nMax Stacks boosts ranged damage by 100%\nFurthermore, copies of your current bullet type are fired from your Stacks\nThese copies do 50% of the base projectile's damage\nPlus the copy consumes a percentage of Stacks based on the damage\nAny projectiles below 100 damage will not produce a copy\nSerpent's Redemption is empowered\n \n ";
                sgaplayer.IDGset = true;
                sgaplayer.digiStacksMax += 100000;
                sgaplayer.devempowerment[0] = 3;
            }
            if (set == "Jellybru")
            {
                player.setBonus = "Reserves 50% of your max HP x2 as an energy barrier\nDamage to your barrier is reduced by Mana Cost Reduction\nThese barriers are boosted by your Magic and Tech Damage Scaling\nBarriers fully recharge in 6 seconds" + Idglib.ColorText(Color.PaleTurquoise, "When Shield Up: Gain Ankh Charm effects") + "\nThe Jelly Brew and Aegisalt Aetherstone are empowered\n \n ";
                sgaplayer.jellybruSet = true;
                sgaplayer.devempowerment[2] = 3;
            }        
        }

        public bool NovusCoreCheck(Player player, Item item)
        {
            SGAPlayer sgaply = player.SGAPly();
            if (sgaply.novusBoost > 300)
                return true;

            if (sgaply.novusBoost < 15)
            {
                if (player.SGAPly().Novusset > 3)
                {
                    RecipeFinder finder = new RecipeFinder();
                    finder.AddIngredient(mod.ItemType("UnmanedBar"));
                    List<Recipe> reclist = finder.SearchRecipes();

                    Recipe foundone = reclist.Find(rec => rec.createItem.type == item.type);

                    if (foundone != null)
                    {
                        /*if (sgaply.novusBoost < 3331)
                        {
                            var snd = Main.PlaySound(SoundID.DD2_KoboldIgnite, player.Center);
                            if (snd != null)
                            {
                                snd.Pitch = -0.500f;
                            }
                            for (int i = 0; i < 16; i += 1)
                            {
                                int dust = Dust.NewDust(player.Hitbox.TopLeft(), player.Hitbox.Width, player.Hitbox.Height, mod.DustType("NovusSparkle"));
                                Main.dust[dust].color = new Color(180, 60, 140);
                                Main.dust[dust].alpha = 181;
                            }
                        }*/
                        sgaply.novusBoost = 450;
                        return true;
                    }

                    finder = new RecipeFinder();
                    finder.AddIngredient(mod.ItemType("FieryShard"));
                    reclist = finder.SearchRecipes();

                    Recipe foundanother = reclist.Find(rec => rec.createItem.type == item.type);

                    if (foundanother != null)
                    {
                        /*if (sgaply.novusBoost < 3331)
                        {
                            var snd = Main.PlaySound(SoundID.DD2_KoboldIgnite, player.Center);
                            if (snd != null)
                            {
                                snd.Pitch = -0.750f;
                            }
                            for (int i = 0; i < 32; i += 1)
                            {
                                int dust = Dust.NewDust(player.Hitbox.TopLeft(), player.Hitbox.Width, player.Hitbox.Height, mod.DustType("NovusSparkle"));
                                Main.dust[dust].color = new Color(180, 60, 140);
                                Main.dust[dust].alpha = 181;
                            }
                        }*/
                        sgaply.novusBoost = 600;
                        return true;
                    }

                }
                sgaply.novusBoost = 200;
            }
            return false;

        }

        public override void MeleeEffects(Item item, Player player, Rectangle hitbox)
        {
            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
            sgaplayer.FlaskEffects(hitbox, player.velocity);
            if (Main.rand.Next(7) == 0)
            {
                if (NovusCoreCheck(player, item))
                {
                    int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("NovusSparkle"));
                    Main.dust[dust].color = new Color(180, 60, 140);
                    Main.dust[dust].alpha = 181;
                }
            }
            if (!player.frostBurn && player.SGAPly().glacialStone && item.melee && !item.noMelee && !item.noUseGraphic && Main.rand.Next(2) == 0)
            {
                int num288 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 135, player.velocity.X * 0.2f + (float)(player.direction * 3), player.velocity.Y * 0.2f, 100, default(Color), 2.5f);
                Main.dust[num288].noGravity = true;
                Main.dust[num288].velocity *= 0.7f;
                Main.dust[num288].velocity.Y -= 0.5f;
            }
        }

        public override bool ReforgePrice(Item item, ref int reforgePrice, ref bool canApplyDiscount)
        {
            if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
                Main.LocalPlayer.SGAPly().skillMananger.ReforgePrice(item, ref reforgePrice, ref canApplyDiscount);
            return true;
        }

        [System.Obsolete]
        public override void GetWeaponDamage(Item item, Player player, ref int damage)
        {

            float basemul = 1f;

            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

            if (item.shoot < 1 && item.melee && item.pick + item.axe + item.hammer < 1)
                damage = (int)((float)damage * sgaplayer.trueMeleeDamage);

            if (NovusCoreCheck(player, item))
                basemul += 0.1f;

            if (item.modItem != null)
            {
                var myType = (item.modItem).GetType();
                var n = myType.Namespace;
                string asastring = (string)n;
                int ishavocitem = asastring.Length - asastring.Replace("HavocGear.", "").Length;
                if (ishavocitem > 0 && sgaplayer.Havoc > 0)
                {
                    damage = (int)(damage * 1.15);
                }
            }

            if (sgaplayer.Dankset > 0)
            {
                damage = (int)(damage + (damage * ((player.magicDamage + player.minionDamage + player.rangedDamage + player.meleeDamage + player.Throwing().thrownDamage) - 5f) * (sgaplayer.Dankset > 3 ? 0.05f : 0.10f)));
            }

            if (sgaplayer.IDGset && sgaplayer.digiStacks > 0 && item.ranged)
            {
                damage = damage + (int)((float)damage * ((float)sgaplayer.digiStacks / (float)sgaplayer.digiStacksMax) * 1.00f);
            }

            damage = (int)(damage * (float)basemul);

            if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
                player.SGAPly().skillMananger.GetWeaponDamage(item, ref damage);

        }

        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if (item.type == ItemID.NebulaPickup1 || item.type == ItemID.NebulaPickup2 || item.type == ItemID.NebulaPickup3)
            {
                if (player.GetModPlayer<SGAPlayer>().BoosterMagnet)
                {
                    grabRange = 400;
                }
            }
            if (player.SGAPly().graniteMagnet)
            {
                grabRange += 48;
            }
            if (item.type == ModContent.ItemType<Dimensions.BubblePickup>() && player.SGAPly().terraDivingGear)
            {
                grabRange += 400;
            }            
            if (item.maxStack > 1 && ((item.modItem != null && (item.Throwing().thrown || item.modItem is IJablinItem)) || item.thrown) && player.armor[0].type == ModContent.ItemType<VibraniumHat> ())
            {
                grabRange += (int)(720 * player.Throwing().thrownVelocity);
            }
        }

        public override bool GrabStyle(Item item, Player player)
        {

            bool boostedrealin = ((item.type == ItemID.NebulaPickup1 || item.type == ItemID.NebulaPickup2 || item.type == ItemID.NebulaPickup3) && player.GetModPlayer<SGAPlayer>().BoosterMagnet) || 
                (item.type == ModContent.ItemType<Dimensions.BubblePickup>() && player.SGAPly().terraDivingGear);


            if (boostedrealin)
            {
                    Vector2 vectorItemToPlayer = player.Center - item.Center;
                    Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * 0.1f;
                    item.velocity = item.velocity + movement;
                    item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);
            }
            else
            {
                float speed = 0.025f;
                bool pullIn = player.SGAPly().graniteMagnet;
                if (item.maxStack > 1 && ((item.modItem != null && (item.Throwing().thrown || item.modItem is IJablinItem)) || item.thrown) && player.armor[0].type == ModContent.ItemType<VibraniumHat>())
                {
                    speed += 0.75f;
                    pullIn = true;
                }
                if (pullIn)
                {
                    Vector2 vectorItemToPlayer = player.Center - item.Center;
                    Vector2 movement = vectorItemToPlayer.SafeNormalize(default(Vector2)) * speed;
                    item.velocity = item.velocity + movement;
                    item.velocity = Collision.TileCollision(item.position, item.velocity, item.width, item.height);

                    if ((player.SGAPly().timer + item.whoAmI) % 10 == 0 && player.armor[0].type == ModContent.ItemType<VibraniumHat>() && player.ownedProjectileCounts[ModContent.ProjectileType<VibraniumThrownExplosion>()] < 20)
                    {
                        Projectile.NewProjectile(item.Center.X, item.Center.Y, 0, 0, ModContent.ProjectileType<VibraniumThrownExplosion>(), (int)((100 * player.Throwing().thrownDamage) * MathHelper.Clamp((item.stack / 25f), 0.5f, 3f)), 0, player.whoAmI, 0f, 0f);
                    }

                    return true;
                }
            }


            return false;
        }

        public override bool UseItem(Item item, Player player)
        {
            SGAPlayer sga = player.SGAPly();
            if (item.healMana > 0 && sga.restorationFlower)
            {
                if (player.statMana + item.healMana >= player.statManaMax2)
                {
                    int difference = (player.statMana + item.healMana) - player.statManaMax2;
                    player.AddBuff(ModContent.BuffType<ManaRegenFakeBuff>(), difference * 2);
                }
            }

            if (item.healLife > 0)
            {
                if (player.statLife + item.healLife >= player.statLifeMax2 && sga.restorationFlower)
                {
                    int difference = (player.statLife + item.healLife) - player.statLifeMax2;
                    player.AddBuff(BuffID.RapidHealing, difference * 5);
                }

                if (player.GetModPlayer<SGAPlayer>().MisterCreeperset)
                {

                    for (int gg = 0; gg < 30; gg += 1)
                    {
                        Vector2 myspeed = Main.rand.NextFloat(0f, MathHelper.TwoPi).ToRotationVector2();
                        myspeed *= Main.rand.NextFloat(14f, 22f);
                        int prog = Projectile.NewProjectile(player.Center.X, player.Center.Y, myspeed.X, myspeed.Y, ProjectileID.BouncyGrenade, 1000, 10f, player.whoAmI);
                        Main.projectile[prog].thrown = true; Main.projectile[prog].ranged = false; Main.projectile[prog].netUpdate = true;
                        IdgProjectile.Sync(prog);
                    }

                }
            }
            if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
                player.SGAPly().skillMananger.UseItem(item);
            return base.UseItem(item, player);
        }

        public override bool OnPickup(Item item, Player player)
        {
            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;

            if (item.type == ItemID.Heart || item.type == ItemID.CandyApple || item.type == ItemID.CandyCane)
            {
                if (sgaplayer.EALogo)
                    player.QuickSpawnItem(ItemID.SilverCoin, 8);
                if (sgaplayer.HeartGuard)
                {
                    player.HealEffect(5);
                    player.netLife = true;
                    player.statLife += 5;
                }
                if (sgaplayer.intimacy > 0 && player.HasBuff(BuffID.Lovestruck))
                {
                    player.HealEffect(10);
                    player.netLife = true;
                    player.statLife += 10;
                }
            }
            if (item.type == ItemID.Star || item.type == ItemID.SugarPlum || item.type == ItemID.SoulCake)
            {
                if (sgaplayer.EALogo)
                    player.QuickSpawnItem(ItemID.SilverCoin, 4);

                if (player.SGAPly().starCollector && player.HasBuff(BuffID.ManaSickness))
                {
                    int index = player.FindBuffIndex(BuffID.ManaSickness);
                    if (index >= 0)
                        player.buffTime[index] -= 60;

                    if (player.statManaMax2 >= player.statManaMax2)
                    {
                        player.HealEffect(5);
                        player.netLife = true;
                        player.statLife += 5;
                    }

                }
            }
            //lifesteal/gain
            //NetMessage.SendData(66, -1, -1, null, num492, (float)num497, 0f, 0f, 0, 0, 0);


            if (sgaplayer.MidasIdol > 0 && sgaplayer.MidasIdol < 3)
            {
                /*int[] count = {player.CountItem(ItemID.CopperCoin), player.CountItem(ItemID.SilverCoin), player.CountItem(ItemID.GoldCoin), player.CountItem(ItemID.PlatinumCoin) };

                 int totalcount = 0;
                 foreach(int coincount in  count){
                 totalcount += coincount;
                 }
                 if (totalcount>0)*/
                
                if (ItemID.Sets.NeverShiny[item.type])
                {
                    sgaplayer.midasMoneyConsumed += (item.value*item.stack)/5;

                    if (item.type == ItemID.CopperCoin)
                    {
                        Main.PlaySound(SoundID.CoinPickup, (int)player.position.X, (int)player.position.Y, 0, 1f, -0.5f);
                        if (sgaplayer.MidasIdol == 2)
                            player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.Campfire : BuffID.Swiftness, 60 * 5);
                        else
                            player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.MagicPower : BuffID.Titan, 60 * 3);
                        return false;
                    }
                    if (item.type == ItemID.SilverCoin)
                    {
                        Main.PlaySound(SoundID.CoinPickup, (int)player.position.X, (int)player.position.Y, 0, 1f, -0.6f);
                        if (sgaplayer.MidasIdol == 2)
                            player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.Endurance : BuffID.Honey, 60 * 5);
                        else
                            player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.Wrath : BuffID.Rage, 60 * 5);
                        return false;
                    }
                    if (item.type == ItemID.GoldCoin)
                    {
                        Main.PlaySound(SoundID.CoinPickup, (int)player.position.X, (int)player.position.Y, 0, 1f, -0.7f);
                        player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.DryadsWard : BuffID.Ironskin, 60 * 10);
                        player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.Wrath : BuffID.Rage, 60 * 20);
                        return false;
                    }
                    if (item.type == ItemID.PlatinumCoin)
                    {
                        Main.PlaySound(SoundID.CoinPickup, (int)player.position.X, (int)player.position.Y, 0, 1f, -0.8f);
                        player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.RapidHealing : BuffID.ShadowDodge, 60 * 30);
                        player.AddBuff(Main.rand.Next(0, 2) == 0 ? BuffID.SolarShield2 : BuffID.IceBarrier, 60 * 30);
                        return false;
                    }
                }


            }
            return base.OnPickup(item, player);
        }

        public override void HorizontalWingSpeeds(Item item, Player player, ref float speed, ref float acceleration)
        {
            int plyleggings = player.armor[2].type;

            if (plyleggings == ModContent.ItemType<Items.Armors.Dev.JellybruLeggings>())
            {
                float boost = player.SGAPly().EnergyDepleted ? 2f : 1f;
                speed += 1.5f * boost;
                acceleration += 0.15f * boost;
            }

            if (plyleggings == ModContent.ItemType<Items.Armors.Mandala.MandalaLeggings>() && Dimensions.SGAPocketDim.WhereAmI != null)
            {
                speed += 2f;
                acceleration += 0.20f;
            }

            if (player.SGAPly().jungleTemplarSet.Item1)
            {
                speed *= 0.75f;
                acceleration *= 0.75f;
            }

        }

        public override void VerticalWingSpeeds(Item item, Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            if (player.SGAPly().jungleTemplarSet.Item1)
            {
                maxCanAscendMultiplier *= 0.75f;
                constantAscend *= 0.75f;
                ascentWhenRising *= 0.25f;
            }
        }

        public override void OnConsumeAmmo(Item item, Player player)
        {
            if (item.ammo > -1 && player.armor[0].type == ModContent.ItemType<VibraniumHelmet>())
            {
                player.SGAPly().AddElectricCharge(item.damage/5);
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {

            if (item.damage < 1)
                return 1f;

            float usetimetemp = 1f;
            SGAPlayer sgaplayer = player.GetModPlayer(mod, typeof(SGAPlayer).Name) as SGAPlayer;
            if (item.pick + item.hammer + item.axe > 0) {
                usetimetemp *= sgaplayer.UseTimeMulPickaxe;
            }
            if (item.ranged && item.useAmmo == AmmoID.Bullet && item.autoReuse == false)
            {
                usetimetemp += sgaplayer.triggerFinger - 1f;
            }


            if (item.summon)
            {
                usetimetemp *= (sgaplayer.summonweaponspeed);
            }
            if ((item.Throwing().thrown || item.thrown) && item.type != ItemID.Beenade) {
                usetimetemp *= sgaplayer.ThrowingSpeed;
            }
            /*ModItem mitem = item.modItem;
            bool hasshoot = false;
            if (mitem != null)
            {
                hasshoot = mitem.GetType().GetMethod("Shoot").GetMethodBody().ToString().Length > 5;
                Main.NewText(mitem.GetType().GetMethod("Shoot").GetMethodBody().);
            }*/
            if (sgaplayer.MisterCreeperset)
            {
                if (item.shoot < 1 && item.melee && item.pick + item.axe + item.hammer < 1)
                {
                    usetimetemp += 1.5f;
                }
            }

            if ((Main.netMode < 1 || SGAmod.SkillRun > 1) && SGAmod.SkillRun > 0)
                player.SGAPly().skillMananger.UseTimeMultiplier(item, ref usetimetemp);
            return (usetimetemp * sgaplayer.UseTimeMul);
        }

        public override void PickAmmo(Item weapon, Item ammo, Player player, ref int type, ref float speed, ref int damage, ref float knockback)
        {
            player.GetModPlayer<SGAPlayer>().myammo = ammo.type;

            if (player.SGAPly().russianRoulette && Main.rand.Next(6) == 0)
            {
                player.SGAPly().MisterCreeperset = false;
                player.Hurt(PlayerDeathReason.ByPlayer(player.whoAmI), damage, -player.direction, true, false, true, 1);
                knockback = 0;
                speed = 0.5f;
                type = ProjectileID.BlowupSmoke;
            }
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            SGAPlayer sgaply = player.SGAPly();
            if (item.useAmmo > 0 && sgaply.russianRoulette)
            {
                if (item.ranged && item.useAmmo != AmmoID.Rocket && item.useAmmo != AmmoID.Bullet && item.useAmmo != AmmoID.Arrow)
                {
                    if (player.armor[0].type == ModContent.ItemType<VibraniumHelmet>())
                        add += 0.30f;
                }

                add += item?.modItem is Items.Weapons.RevolverBase ? 1f : 0.5f;
            }

            if (item.modItem != null)
            {

                if (item.modItem is ITechItem)
                {
                    float value = sgaply.electricChargeMax>0 ? sgaply.electricCharge / ((float)sgaply.electricChargeMax) : 0;
                    float floaterdam = (sgaply.techdamage * (MathHelper.Clamp(value * 4f, 0f, 1f)));
                    add = Math.Max(add+(floaterdam - 1f),0f);
                }

                if (item.modItem is IMangroveSet)
                {
                    List<int> mangroves = new List<int>();
                    mangroves.Add(mod.ItemType("MangroveStriker"));
                    mangroves.Add(mod.ItemType("MangroveShiv"));
                    mangroves.Add(mod.ItemType("MangroveStaff"));
                    mangroves.Add(mod.ItemType("MangroveBow"));
                    mangroves.Add(mod.ItemType("MossYoyo"));

                    float bonusdamage = 0;

                    foreach (int itemtype in mangroves)
                    {
                        if (player.CountItem(itemtype) > 0)
                        {
                            if (itemtype != item.type)
                            {
                                bonusdamage += 0.125f;
                            }
                        }
                    }
                    add += bonusdamage;
                }
            }
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();

            if (item?.modItem is ITechItem)
            {
                sgaplayer.ConsumeElectricCharge(5+(int)((damage * sgaplayer.techdamage) * 1f), 60);
            }

            if ((item.useAmmo == AmmoID.Gel) && player.GetModPlayer<SGAPlayer>().FridgeflameCanister)
            {

                int probg = Projectile.NewProjectile(position.X + (int)(speedX * 2f), position.Y + (int)(speedY * 2f), speedX, speedY, mod.ProjectileType("IceFlames"), (int)(damage * 0.75), knockBack, player.whoAmI);
                Main.projectile[probg].ranged = item.ranged;
                Main.projectile[probg].magic = item.magic;
                Main.projectile[probg].friendly = true;
                Main.projectile[probg].hostile = false;
                Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(15));
                Main.projectile[probg].velocity.X = perturbedSpeed.X * 0.6f;
                Main.projectile[probg].velocity.Y = perturbedSpeed.Y * 0.6f;
                Main.projectile[probg].owner = player.whoAmI;
                SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
                modeproj.myplayer = player;
                IdgProjectile.Sync(probg);

            }
            return true;
        }

        public static void AwardSGAmodDevArmor(Player player)
        {
            Mod mod = SGAmod.Instance;

            string[] texts = {"Wooo!","Congrats!","You're Winner!","You did it!","Unusual Unboxed!","Yay!" };

            for(int i = 0; i < 32; i += 1)
            {
                CombatText.NewText(new Rectangle((int)Main.screenPosition.X, (int)Main.screenPosition.Y,Main.screenWidth,Main.screenHeight), Main.DiscoColor, texts[Main.rand.Next(texts.Length)], true);
            }

            int itt = Projectile.NewProjectile(player.Center, new Vector2(0, -5), ProjectileID.RocketFireworkYellow,100,0, player.whoAmI);
            Main.projectile[itt].timeLeft = 72;
            itt = Projectile.NewProjectile(player.Center, new Vector2(2, -4), ProjectileID.RocketFireworkGreen, 100, 0, player.whoAmI);
            Main.projectile[itt].timeLeft = 72;
            itt = Projectile.NewProjectile(player.Center, new Vector2(-2, -4), ProjectileID.RocketFireworkBlue, 100, 0, player.whoAmI);
            Main.projectile[itt].timeLeft = 72;

            switch (Main.rand.Next(0,3))
            {
                case 2:
                    player.QuickSpawnItem(ModContent.ItemType<JellybruHelmet>(), 1);
                    player.QuickSpawnItem(ModContent.ItemType < JellybruChestplate>(), 1);
                    player.QuickSpawnItem(ModContent.ItemType < JellybruLeggings>(), 1);
                    break;

                case 1:
                    player.QuickSpawnItem(ModContent.ItemType < MisterCreeperHead>(), 1);
                    player.QuickSpawnItem(ModContent.ItemType < MisterCreeperBody>(), 1);
                    player.QuickSpawnItem(ModContent.ItemType < MisterCreeperLegs>(), 1);
                    break;

                default:
                    player.QuickSpawnItem(ModContent.ItemType < IDGHead>(), 1);
                    player.QuickSpawnItem(ModContent.ItemType < IDGBreastplate>(), 1);
                    player.QuickSpawnItem(ModContent.ItemType < IDGLegs>(), 1);
                    break;
            }
        }

        public override void OpenVanillaBag(string context, Player player, int arg)
        {
            if (context == "bossBag")
            {

                if (arg == ItemID.GolemBossBag && Main.rand.Next(100) < 20)
                    player.QuickSpawnItem(mod.ItemType("Upheaval"), 1);
                if (arg == ItemID.MoonLordBossBag)
                {
                    player.QuickSpawnItem(ModContent.ItemType<EldritchTentacle>(), Main.rand.Next(20, 40));
                    if (Main.rand.Next(6)<1)
                    player.QuickSpawnItem(ModContent.ItemType<Items.Weapons.SoulPincher>());

                }
                if (arg == ItemID.BossBagBetsy)
                    player.QuickSpawnItem(mod.ItemType("OmegaSigil"), 1);
                if (arg == ItemID.WallOfFleshBossBag && Main.rand.Next(100) <= 10)
                    player.QuickSpawnItem(mod.ItemType("Powerjack"), 1);
            }
        }

        public override void RightClick(Item item, Player player)
        {
            if (item.type == ItemID.FloatingIslandFishingCrate && Main.rand.Next(5) == 0)
            {
                player.QuickSpawnItem(ModContent.ItemType<StarCollector>());
            }   
        }

        public override void Update(Item item, ref float gravity, ref float maxFallSpeed)
        {
            if (item.type == ItemID.SandBlock && item.velocity.Y == 0 && item.wet && Main.rand.Next(100)<1)
            {
                int item2 = Item.NewItem(item.Center, ModContent.ItemType<MoistSand>(), 1);
                Item item3 = Main.item[item2];
                if (item3 != null && item3.active)
                {
                    //item3.velocity = Vector2.Zero;
                    item3.noGrabDelay = 0;
                    if (Main.netMode != NetmodeID.SinglePlayer)
                    {
                        NetMessage.SendData(MessageID.SyncItem, -1, -1, null, item.whoAmI);
                    }
                    item.stack--;

                    if (item.stack < 1)
                        item.TurnToAir();
                    return;
                }
            }
            HopperTile.HandleItemHoppers(item);
        }

        public override void UpdateInventory(Item item,Player player)
        {
            if (item.buffType > 0 && SGAmod.overpoweredMod>1f)
            {
                item.maxStack = 29;
            }

            string[] buffTier = { "", "RadiationOne", "RadiationTwo", "RadiationThree" };

            if (item.modItem != null && item.modItem is IRadioactiveItem)
            {
                int buffID = (item.modItem as IRadioactiveItem).RadioactiveInventory() - (player.SGAPly().grippinggloves > 1 ? 1 : 0);
                int indexer = (int)MathHelper.Clamp(buffID, 0, buffTier.Length - 1);

                if (indexer > 0)
                    player.AddBuff(ModLoader.GetMod("IDGLibrary").GetBuff(buffTier[indexer]).Type, 2);
            }

            if (item.modItem != null && item.modItem is IManifestedItem)
            {
                if (player.inventory[player.selectedItem] != item)
                {
                    item.active = false;
                    //item.TurnToAir();
                    item.type = 0;
                }
            }

        }

        public override bool NewPreReforge(Item item)
        {
            return base.NewPreReforge(item);
        }

    }


    public class SGAUpgradableItemInstance : GlobalItem
    {

        public int toolType = 0;
        public override bool InstancePerEntity => true;

        public override bool CanUseItem(Item item, Player player)
        {
            if (item.GetGlobalItem<SGAUpgradableItemInstance>().toolType == 1)
            {
                return player.SGAPly().ConsumeElectricCharge(Math.Max(item.pick, Math.Max(item.hammer, item.axe)), 120,false,true);
            }
            return true;
        }

        public override void HoldItem(Item item, Player player)
        {
            if (player.SGAPly().timer%30==0 && item.GetGlobalItem<SGAUpgradableItemInstance>().toolType == 1)
            {
                Projectile finder;
                finder = Main.projectile.FirstOrDefault(testby => testby.active && testby.owner == player.whoAmI && testby.aiStyle == 20);
                if (finder != default)
                {
                    if (!player.SGAPly().ConsumeElectricCharge(Math.Max(item.pick, Math.Max(item.hammer, item.axe)), 120))
                        finder.Kill();
                }
            }
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (item.GetGlobalItem<SGAUpgradableItemInstance>().toolType == 1)
            {
                return 1.25f;
            }
            return 1f;
        }

        public override bool NeedsSaving(Item item)
        {
            if (item.GetGlobalItem<SGAUpgradableItemInstance>().toolType > 0)
                return true;
            return false;
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)item.GetGlobalItem<SGAUpgradableItemInstance>().toolType);
        }
        public override void NetReceive(Item item, BinaryReader reader)
        {
            item.GetGlobalItem<SGAUpgradableItemInstance>().toolType = (int)reader.ReadByte();
        }
        public override TagCompound Save(Item item)
        {
            TagCompound tag = new TagCompound
            {
                ["toolType"] = item.GetGlobalItem<SGAUpgradableItemInstance>().toolType
            };
            return tag;
        }
        public override void Load(Item item, TagCompound tag)
        {
            SGAUpgradableItemInstance upgrades = item.GetGlobalItem<SGAUpgradableItemInstance>();
            if (tag.ContainsKey("toolType"))
                upgrades.toolType = tag.GetInt("toolType");
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            SGAUpgradableItemInstance myClone = (SGAUpgradableItemInstance)base.Clone(item, itemClone);
            myClone.toolType = toolType;
            return myClone;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.GetGlobalItem<SGAUpgradableItemInstance>().toolType == 1)
                tooltips.Add(new TooltipLine(mod, "PowerToolTip", Idglib.ColorText(Color.SkyBlue,"Power Tools Upgrade Applied")));
        }
    }











        public class TrapPrefixAccessory : TrapPrefix
    {

        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public TrapPrefixAccessory()
        {
        }
        public TrapPrefixAccessory(float armorbreak, float damage,int misc = 0)
        {
            this.armorbreak = armorbreak;
            this.damage = damage;
            this.misc = misc;
        }
        public override bool CanRoll(Item item)
        {
            return item.accessory;
        }
        public override float RollChance(Item item)
        {
            return 1f;
        }

    }

    public class ThrowerPrefix : TrapPrefix
    {

        public override PrefixCategory Category { get { return PrefixCategory.Custom; } }

        public ThrowerPrefix()
        {
        }
        public ThrowerPrefix(float armorbreak, float damage, float thrownvelocity, float throweruserate)
        {
            this.thrownvelocity = thrownvelocity;
            this.throweruserate = throweruserate;
            this.armorbreak = armorbreak;
            this.damage = damage;
        }
        public override float RollChance(Item item)
        {
            return 1f;
        }
        public override bool CanRoll(Item item)
        {
            return item.thrown || item.Throwing().thrown;
        }
    }

    public class ThrowerPrefixAccessory : TrapPrefix
    {

        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public ThrowerPrefixAccessory()
        {
        }
        public ThrowerPrefixAccessory(float armorbreak = 0f, float damage = 0f, float thrownvelocity = 0f, float throweruserate = 0f, float throwersavingchance = 0f, double apocochance = 0.0,float apocostrength=0f)
        {
            this.thrownvelocity = thrownvelocity;
            this.throweruserate = throweruserate;
            this.armorbreak = armorbreak;
            this.damage = damage;
            this.throwersavingchance = throwersavingchance;
            this.apocochance = apocochance;
            this.apocochancestrength = apocostrength;
        }
        public override bool CanRoll(Item item)
        {
            return item.accessory;
        }


    }
    public class UberPrefixAccessory : TrapPrefix
    {
        private float[] uber = {0f,0f,0f,0f,0f,0f,0f};
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public UberPrefixAccessory()
        {
        }
        public UberPrefixAccessory(float armorbreak = 0f, float damage = 0f, float thrownvelocity = 0f, float throweruserate = 0f, float throwersavingchance = 0f, double apocochance = 0.0,
            float damageMult=0f, float knockbackMult = 0f, float useTimeMult = 0, float scaleMult = 0, float shootSpeedMult = 0, float manaMult = 0f, int critBonus=0)
        {
            this.thrownvelocity = thrownvelocity;
            this.throweruserate = throweruserate;
            this.armorbreak = armorbreak;
            this.damage = damage;
            this.throwersavingchance = throwersavingchance;
            this.apocochance = apocochance;
            uber[0] = damageMult;
            uber[1] = knockbackMult;
            uber[2] = useTimeMult;
            uber[3] = scaleMult;
            uber[4] = shootSpeedMult;
            uber[5] = manaMult;
            uber[6] = critBonus;

        }
        public override bool CanRoll(Item item)
        {
            return false;// Main.LocalPlayer.GetModPlayer<SGAPlayer>().ExpertiseCollectedTotal>10000;
        }
        public override float RollChance(Item item)
        {
            return 0.5f;
        }
        public override void Apply(Item item)
        {
            Main.NewText("UBER PREFIX!!!");
            TrapDamageItems myitem = item.GetGlobalItem<TrapDamageItems>();
            if (item.damage > 0)
            {
                item.damage += (int)(item.damage * uber[0]);
            }
            myitem.damageacc += uber[0];
            if (item.knockBack > 0)
                item.knockBack += uber[1];
            if (item.useTime > 0)
            {
                item.useTime = (int)(item.useTime / (1 + uber[2]));
                item.useAnimation = (int)(item.useTime / (1 + uber[2]));
            }
            item.scale = (int)(item.scale * (1 + uber[3]));
            if (item.shoot>0)
            item.shootSpeed += uber[4];
            if (item.mana > 0)
                item.mana = (int)(item.mana / (1 + uber[5]));
            item.crit += (int)uber[6];
            myitem.damagecrit += (int)uber[6];

            int itt = Projectile.NewProjectile(Main.LocalPlayer.Center, new Vector2(0,-5), ProjectileID.FireworkFountainRainbow, 0,0);
            Main.projectile[itt].timeLeft = 90;
            base.Apply(item);
        }

    }    
    public class EAPrefixAccessory : TrapPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public EAPrefixAccessory()
        {
        }
        public EAPrefixAccessory(float greed = 0f)
        {
            this.greed = greed;
        }
        public override bool CanRoll(Item item)
        {
            //return Main.LocalPlayer.HasItem(mod.ItemType("EALogo"))
            if (Main.LocalPlayer != null && !Main.gameMenu)
                return Main.LocalPlayer.GetModPlayer<SGAPlayer>().EALogo;
            else
                return false;
        }
        public override float RollChance(Item item)
        {
            return 5f;
        }

    }

    public class ShieldPrefix : TrapPrefix
    {

        public ShieldPrefix(int misc)
        {
            this.misc = misc;
        }

        public override bool CanRoll(Item item)
        {
            if (item.modItem != null)
            {
                if (item.modItem is IShieldItem)
                    return true;
            }

            return false;
        }

    }

    public class BustedPrefix : TrapPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.AnyWeapon; } }

        public BustedPrefix(int misc)
        {
            this.misc = misc;
        }
        public override bool CanRoll(Item item)
        {
            return true;
        }
        public override float RollChance(Item item)
        {
            return 1f;
        }
    }
    public class BustedAccessoryPrefix : BustedPrefix
    {
        public override PrefixCategory Category { get { return PrefixCategory.Accessory; } }

        public BustedAccessoryPrefix(int misc) : base(misc) { }
    }

    public class TrapPrefix : ModPrefix
    {
        public float armorbreak = 0f;
        public float damage = 0f;
        public float damageacc = 0f;
        public int damagecrit = 0;
        public float thrownvelocity = 0f;
        public float throweruserate = 0f;
        public float throwersavingchance = 0f;
        public double apocochance = 0.0;
        public float greed = 0f;
        public float apocochancestrength = 0f;
        public int misc = 0;

        public static byte? GetBustedPrefix
        {
            get
            {
                List<ModPrefix> modz = ModPrefix.GetPrefixesInCategory(PrefixCategory.AnyWeapon).Where(thisprefix => thisprefix is TrapPrefix && (thisprefix as TrapPrefix).misc == 2).ToList();
                if (modz != null && modz.Count > 0)
                {
                    return modz[0].Type;
                }
                return null;
            }
        }

        public override PrefixCategory Category { get { return PrefixCategory.AnyWeapon; } }
        public TrapPrefix()
        {
        }
        public TrapPrefix(float armorbreak, float damage)
        {
            this.armorbreak = armorbreak;
            this.damage = damage;
        }
        public override bool Autoload(ref string name)
        {
            if (base.Autoload(ref name))
            {
                if (GetType() == typeof(BustedPrefix))
                {
                    //mod.AddPrefix("Busted", new BustedPrefix(2));
                    //mod.AddPrefix("Screwed Up", new BustedAccessoryPrefix(2));
                }

                if (GetType() == typeof(ShieldPrefix))
                {
                    //mod.AddPrefix("Defensive", new ShieldPrefix(3));
                }
                if (GetType() == typeof(TrapPrefix))
                {
                    if (!SGAConfig.Instance.BestPrefixes)
                    {
                        mod.AddPrefix("Edged", new TrapPrefix(0.05f, 0.05f));
                        mod.AddPrefix("Sundering", new TrapPrefix(0.08f, 0.10f));
                        mod.AddPrefix("Undercut", new TrapPrefix(0.12f, 0.10f));
                    }
                    mod.AddPrefix("Razor Sharp", new TrapPrefix(0.2f, 0.15f));
                }
                if (GetType() == typeof(TrapPrefixAccessory))
                {
                    mod.AddPrefix("Defensive", new ShieldPrefix(3));

                    mod.AddPrefix("Busted", new BustedPrefix(2));
                    mod.AddPrefix("Screwed Up", new BustedAccessoryPrefix(2));

                    if (!SGAConfig.Instance.BestPrefixes)
                    {
                        mod.AddPrefix("Tinkering", new TrapPrefixAccessory(0f, 0.04f));
                        mod.AddPrefix("Knowledgeable", new TrapPrefixAccessory(0.06f, 0f));
                        mod.AddPrefix("Dungeoneer's", new TrapPrefixAccessory(0.04f, 0.05f));
                    }
                    mod.AddPrefix("Goblin Tinker's Own", new TrapPrefixAccessory(0.05f, 0.075f));
                    mod.AddPrefix("Energized", new TrapPrefixAccessory(0,0,1));
                }
                if (GetType() == typeof(ThrowerPrefix))
                {
                    if (!SGAConfig.Instance.BestPrefixes)
                    {
                        mod.AddPrefix("Tossable", new ThrowerPrefix(0f, 0f, 0.1f, 0.15f));
                        mod.AddPrefix("Impacting", new ThrowerPrefix(0f, 0f, 0.15f, 0.2f));
                    }
                    mod.AddPrefix("Olympian", new ThrowerPrefix(0f, 0f, 0.25f, 0.4f));
                }
                if (GetType() == typeof(ThrowerPrefixAccessory))
                {
                    if (!SGAConfig.Instance.BestPrefixes)
                    {
                        mod.AddPrefix("Lightweight", new ThrowerPrefixAccessory(0f, 0f, 0.025f, 0.025f, 0f, 0, 0));
                        mod.AddPrefix("Slinger's", new ThrowerPrefixAccessory(0f, 0f, 0.04f, 0.02f, 0.01f, 0, 0));
                        mod.AddPrefix("Pocketed", new ThrowerPrefixAccessory(0f, 0f, 0.02f, 0.03f, 0.015f, 0, 0));
                        mod.AddPrefix("Conserving", new ThrowerPrefixAccessory(0f, 0f, 0.0f, 0.0f, 0.05f, 0, 0));
                    }
                    mod.AddPrefix("Roguish", new ThrowerPrefixAccessory(0f, 0f, 0.06f, 0.05f,0.02f,0,0));

                    if (!SGAConfig.Instance.BestPrefixes)
                    {
                        mod.AddPrefix("Doomsayer", new ThrowerPrefixAccessory(0f, 0f, 0f, 0f, 0f, 0.5f, 0.05f));
                    }
                        mod.AddPrefix("Horseman's", new ThrowerPrefixAccessory(0f, 0f, 0f, 0f, 0f, 1f, 0.075f));

                    mod.AddPrefix("Disordered", new ThrowerPrefixAccessory(0.05f, 0.075f, 0f, 0f, 0f, 0.25f, 0.06f));
                    mod.AddPrefix("Rioter's", new ThrowerPrefixAccessory(0f, 0f, 0.04f, 0.03f, 0f, 0.25f, 0.04f));
                }
                /*if (GetType() == typeof(UberPrefixAccessory))
                {
                    mod.AddPrefix("Horsemassssssn's", new UberPrefixAccessory());
                    mod.AddPrefix("Darksider", new UberPrefixAccessory(0,0,0,0,0.075f,0.75f,0.03f,0.1f,0f,0f,0.05f,0f,2));
                    //mod.AddPrefix("Darksider", new UberPrefixAccessory(apocochance: 0.75,damageMult: 0.03f,throwersavingchance: 0.075f,shootSpeedMult: 0.05f,manaMult: 0.04f,knockbackMult: 0.1f,critBonus: 2));
                    mod.AddPrefix("Uber", new UberPrefixAccessory(damageMult: 0.05f, throwersavingchance: 0.10f, shootSpeedMult: 0.15f, manaMult: 0.05f, knockbackMult: 0.2f,useTimeMult: 0.075f,critBonus: 5,thrownvelocity: 0.05f));
                }*/
                if (GetType() == typeof(EAPrefixAccessory))
                {
                    mod.AddPrefix("Greedy", new EAPrefixAccessory(0.025f));
                    mod.AddPrefix("Grubby", new EAPrefixAccessory(0.05f));
                    mod.AddPrefix("Share Holding", new EAPrefixAccessory(0.075f));
                }          
            }
            return false;
        }

        public override bool CanRoll(Item item)
        {
            if (item.modItem != null)
            {
                Type myclass = item.modItem.GetType();
                if (myclass.BaseType == typeof(TrapWeapon) || myclass.IsSubclassOf(typeof(TrapWeapon)))
                    return true;
            }

            return false;
        }

        public override float RollChance(Item item)
        {
            return 1f;
        }

        public override void Apply(Item item)
        {
            TrapDamageItems myitem = item.GetGlobalItem<TrapDamageItems>();
            myitem.armorbreak = armorbreak;
            myitem.damage2 = damage;
            myitem.throweruserate = throweruserate;
            myitem.thrownvelocity = thrownvelocity;
            myitem.thrownsavingchance = throwersavingchance;
            myitem.apocochance = apocochance;
            myitem.greed = greed;
             myitem.damageacc = damageacc;
             myitem.damagecrit = damagecrit;
             myitem.apocochancestrength = apocochancestrength;
            myitem.misc = misc;
            /*if (item.damage > 0)
          {
              item.damage = (int)(item.damage * (1f + damage));
          }*/
            //myitem.damage = damage;
        }
    }

    public class TrapDamageItems : GlobalItem
    {
        public float armorbreak = 0f;
        public float damage2 = 0f;
        public float throweruserate = 0f;
        public float thrownvelocity = 0f;
        public float thrownsavingchance = 0f;
        public float greed = 0f;
        public float damageacc = 0f;
        public int damagecrit = 0;
        public int misc = 0;
       public float apocochancestrength = 0f;
         public double apocochance = 0;
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override GlobalItem Clone(Item item, Item itemClone)
        {
            TrapDamageItems myClone = (TrapDamageItems)base.Clone(item, itemClone);
            myClone.armorbreak = armorbreak;
            myClone.damage2 = damage2;
            myClone.throweruserate = throweruserate;
            myClone.thrownvelocity = thrownvelocity;
            myClone.thrownsavingchance = thrownsavingchance;
            myClone.apocochance = apocochance;
            myClone.greed = greed;
            myClone.damageacc = damageacc;
            myClone.damagecrit = damagecrit;
            myClone.misc = misc;
            myClone.apocochancestrength=apocochancestrength;
            return myClone;
        }


        public override bool NewPreReforge(Item item)
        {
            damage2 = 0f;
            armorbreak = 0f;
            throweruserate = 0f;
            thrownvelocity = 0f;
            thrownsavingchance = 0f;
            apocochance = 0.0;
            greed = 0f;
            damageacc = 0f;
            damagecrit = 0;
            apocochancestrength = 0;
            misc = 0;
            return base.NewPreReforge(item);
        }

        public static bool SavingChanceMethod(Player player, bool accountforbuiltinchecks = false)
        {
            if (accountforbuiltinchecks)
            {
                if (Main.rand.Next(100) < 33 && player.Throwing().thrownCost33)
                    return false;
                if (Main.rand.Next(100) < 50 && player.Throwing().thrownCost50)
                    return false;
            }


            return Main.rand.Next(100) > (int)(player.GetModPlayer<SGAPlayer>().Thrownsavingchance * 100f);

        }

        public override void OnConsumeItem(Item item, Player player)
        {
            SGAPlayer sga = player.SGAPly();
            if (Main.rand.Next(0, 100) < sga.consumeCurse && !sga.tpdcpu)
                item.stack -= 1;
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            if (item.Throwing().thrown || item.thrown)
            {
                return TrapDamageItems.SavingChanceMethod(player);
            }
            else
            {
                return true;
            }
        }

        public override bool CanEquipAccessory(Item item, Player player, int slot)
        {
            if (misc != 2)
                return base.CanUseItem(item, player);
            else
                return false;
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (misc != 2)
                return base.CanUseItem(item, player);
            else
                return false;
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            player.GetModPlayer<SGAPlayer>().TrapDamageAP += armorbreak;
            player.GetModPlayer<SGAPlayer>().TrapDamageMul += damage2;
            player.GetModPlayer<SGAPlayer>().ThrowingSpeed += throweruserate;
            player.GetModPlayer<SGAPlayer>().Thrownsavingchance += thrownsavingchance;
            player.GetModPlayer<SGAPlayer>().greedyperc += greed;
            player.GetModPlayer<SGAPlayer>().apocalypticalStrength += apocochancestrength;
            if (misc == 1)
                player.GetModPlayer<SGAPlayer>().electricrechargerate += 1;

            player.magicDamage += damageacc; player.minionDamage += damageacc; player.rangedDamage += damageacc; player.meleeDamage += damageacc; player.Throwing().thrownDamage += damageacc;
            player.magicCrit += damagecrit; player.rangedCrit += damagecrit; player.meleeCrit += damagecrit; player.Throwing().thrownCrit += damagecrit;
            for (int i = 0; i < player.GetModPlayer<SGAPlayer>().apocalypticalChance.Length; i += 1)
                player.GetModPlayer<SGAPlayer>().apocalypticalChance[i] += apocochance;
            player.Throwing().thrownVelocity += thrownvelocity;
        }
        public override void UpdateInventory(Item item, Player player)
        {
            if (player.HeldItem == item)
            {
                player.GetModPlayer<SGAPlayer>().TrapDamageAP += armorbreak;
                player.GetModPlayer<SGAPlayer>().TrapDamageMul += damage2;
                player.GetModPlayer<SGAPlayer>().ThrowingSpeed += throweruserate;
                player.Throwing().thrownVelocity += thrownvelocity;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {

            if (damage2 > 0)
            {
                string line2 = "% extra trap damage (while held)";
                if (item.accessory)
                    line2 = "% extra trap damage";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + ((damage2) * 100f) + line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (armorbreak > 0)
            {
                string line2 = "%";
                if (item.accessory)
                    line2 = "% trap damage";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + ((armorbreak) * 100f) + line2 + " armor piercing");
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (throweruserate > 0)
            {
                string line2 = "% throwing rate";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + ((throweruserate) * 100f) + line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (thrownvelocity > 0)
            {
                string line2 = "% throwing velocity";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + ((thrownvelocity) * 100f) + line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (thrownsavingchance > 0)
            {
                string line2 = "% chance to not consume thrown item";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + ((thrownsavingchance) * 100f) + line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (greed > 0)
            {
                string line2 = "% shop discounts";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + (greed*100f) + line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (apocochancestrength > 0)
            {
                string line2 = "% Apocalyptical Strength";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + (apocochancestrength*100) + line2);
                line.isModifier = true;
                tooltips.Add(line);
            }              
            if (apocochance > 0)
            {
                string line2 = "% Apocalyptical Chance";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", "+" + (apocochance) + line2);
                line.isModifier = true;
                tooltips.Add(line);
                tooltips.Add(new TooltipLine(mod, "apocthing", SGAGlobalItem.apocalypticaltext));
            }
            if (misc == 1)
            {
                string line2 = "+1 passive Electric Charge Rate";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (misc == 2)
            {
                string line2 = Idglib.ColorText(Color.Red,"This item is busted and needs to be reforged to be used");
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
            if (misc == 3)
            {
                string line2 = "+5% Shield Block";
                TooltipLine line = new TooltipLine(mod, "SGAPrefixline", line2);
                line.isModifier = true;
                tooltips.Add(line);
            }
        }
        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((int)damage2 * 1000);
            writer.Write((int)armorbreak * 1000);
            writer.Write((int)thrownvelocity * 1000);
            writer.Write((int)throweruserate * 1000);
            writer.Write((int)thrownsavingchance * 1000);
            writer.Write(apocochance);
            writer.Write((int)greed * 1000);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            damage2 = (float)(reader.ReadInt32() / 1000f);
            armorbreak = (float)(reader.ReadInt32() / 1000f);
            thrownvelocity = (float)(reader.ReadInt32() / 1000f);
            throweruserate = (float)(reader.ReadInt32() / 1000f);
            thrownsavingchance = (float)(reader.ReadInt32() / 1000f);
            apocochance = reader.ReadDouble();
            greed = (float)(reader.ReadInt32() / 1000f);
        }


    }

    public class EnchantmentGlobalItem : GlobalItem
    {
        public int catalyst = 0;

        public EnchantmentGlobalItem()
        {
            catalyst = 0;
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            EnchantmentGlobalItem myClone = (EnchantmentGlobalItem)base.Clone(item, itemClone);
            myClone.catalyst = 0;
            return myClone;
        }

        public override void SetDefaults(Item item)
        {
            EnchantmentGlobalItem itemz = item.GetGlobalItem<EnchantmentGlobalItem>();
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!SGAmod.EnchantmentsUpdate)
                return;

            if (SGAmod.EnchantmentCatalyst != null)
            {
                EnchantmentCraftingMaterial valuz;
                bool find = SGAmod.EnchantmentCatalyst.TryGetValue(item.type, out valuz);
                if (find)
                {
                    if (SGAmod.CustomUIMenu.visible && SGAmod.CustomUIMenu.EnchantingCataylstPanel.item.type == item.type)
                        tooltips.Add(new TooltipLine(mod, "EnchantmentCatalyst", Idglib.ColorText(Color.Purple, valuz.text)));
                    else
                        tooltips.Add(new TooltipLine(mod, "EnchantmentCatalyst", Idglib.ColorText(Color.Purple, "This item could be used as a Catalyst Agent in enchanting")));
                }
                find = SGAmod.EnchantmentFocusCrystal.TryGetValue(item.type, out valuz);
                if (find)
                {
                    if (SGAmod.CustomUIMenu.visible && SGAmod.CustomUIMenu.EnchantingStationsPanels[2].item.type == item.type)
                        tooltips.Add(new TooltipLine(mod, "EnchantmentCatalyst", Idglib.ColorText(Color.Purple, valuz.text)));
                    else
                        tooltips.Add(new TooltipLine(mod, "EnchantmentCatalyst", Idglib.ColorText(Color.Purple, "This item could be used as a Focusing Crystal in enchanting")));
                }
            }
        }

    }

}
