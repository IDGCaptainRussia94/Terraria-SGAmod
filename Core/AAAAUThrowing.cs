#define ThrowingCompat

using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.Utilities;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SGAmod
{

    public class UThrowingProjectile : GlobalProjectile
    {
        public bool thrown = false;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }


#if ThrowingCompat
        //throwing compatible, for now
        public override bool PreAI(Projectile projectile)
        {
            return true;

            if (projectile.Throwing().thrown)
                projectile.thrown = true;
                    return true;
        }
#endif

    }

    public class UThrowingWeapon : GlobalItem
    {
        public bool thrown = false;

        public UThrowingWeapon()
        {
            thrown = false;
        }

        public override bool InstancePerEntity => true;

        public override GlobalItem Clone(Item item, Item itemClone)
        {
            UThrowingWeapon myClone = (UThrowingWeapon)base.Clone(item, itemClone);
            myClone.thrown = item.thrown;
            myClone.thrown = thrown;
            return myClone;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            return;
            #region oldcode
            UThrowingPlayer thrownPlayer = Main.LocalPlayer.Throwing();
            UThrowingWeapon weapon = item.Throwing();
            if (weapon.thrown)
            {
                // Get the vanilla damage tooltip
                TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
                if (tt != null)
                {
                    // We want to grab the last word of the tooltip, which is the translated word for 'damage' (depending on what langauge the player is using)
                    // So we split the string by whitespace, and grab the last word from the returned arrays to get the damage word, and the first to get the damage shown in the tooltip
                    string[] splitText = tt.text.Split(' ');
                    string damageValue = splitText.First();
                    string damageWord = splitText.Last();
                    // Change the tooltip text
                    tt.text = damageValue + " (U)Thrown " + damageWord;
                }
                // Get the vanilla crit tooltip
                tt = tooltips.FirstOrDefault(x => x.Name == "CritChance" && x.mod == "Terraria");
                if (tt != null)
                {
                    string[] thetext = tt.text.Split(' ');
                    string newline = "";
                    List<string> valuez = new List<string>();
                    int counter = 0;
                    foreach (string text2 in thetext)
                    {
                        counter += 1;
                        if (counter > 1)
                            valuez.Add(text2 + " ");
                    }
                    int thecrit = ThrowingUtils.DisplayedCritChance(item);
                    string thecrittype = "(U)Thrown ";
                    valuez.Insert(0, thecrit + "% " + thecrittype);
                    foreach (string text3 in valuez)
                    {
                        newline += text3;
                    }
                    tt.text = newline;
                }
            }
            #endregion
        }
        public override bool ConsumeItem(Item item, Player player)
        {
            if (item.Throwing().thrown)
            {
                return TrapDamageItems.SavingChanceMethod(player);
            }
            else
            {
                return true;
            }
        }

        /*public override bool ConsumeItem(Item item, Player player)
        {
            return true;
            UThrowingWeapon weapon = item.Throwing();
            UThrowingPlayer thrownPlayer = player.Throwing();
            if (weapon.thrown)
            {
                if (Main.rand.Next(0, 100) < 33 && thrownPlayer.thrownCost33)
                    return false;
                if (Main.rand.Next(0, 100) < 50 && thrownPlayer.thrownCost50)
                    return false;
            }
            return true;
        }*/

        public override void HoldItem(Item item, Player player)
        {
            return;
            #region oldcode
            UThrowingWeapon weapon = item.Throwing();
            UThrowingPlayer thrownPlayer = player.Throwing();
            if (weapon.thrown)
            {
                thrownPlayer.thrownCrit += item.crit;
            }
            #endregion
        }


        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            return true;
            #region oldcode
            UThrowingWeapon weapon = item.Throwing();
            UThrowingPlayer thrownPlayer = player.Throwing();
            if (weapon.thrown)
            {
                speedX *= thrownPlayer.thrownVelocity;
                speedY *= thrownPlayer.thrownVelocity;
            }
            return true;
            #endregion;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            return;
            #region oldcode
            UThrowingWeapon weapon = item.Throwing();
            UThrowingPlayer thrownPlayer = player.Throwing();
            if (weapon.thrown)
            {
                mult = thrownPlayer.thrownDamage;
            }
#endregion;
        }

        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
            return base.PrefixChance(item, pre, rand);

            if (item.consumable && item.maxStack<2 && pre==-3)
			return false;
            return base.PrefixChance(item, pre, rand);
        }

        public override int ChoosePrefix(Item item,UnifiedRandom rand)
        {
            return base.ChoosePrefix(item, rand);
            #region oldcode
            if (!item.Throwing().thrown && !item.thrown)
            {
                return base.ChoosePrefix(item, rand);
            }
            Mod sga = ModLoader.GetMod("SGAmod");
            if (sga != null)
            {
                //Ain't gonna lie, it's not pretty, but it works for now
                //Will likely wait for Relogic/TML what they do about this
                if (Main.rand.Next(0, 10) < 1)
                {
                    List<string> stuff = new List<string>();
                    stuff.Add("Tossable");
                    stuff.Add("Impacting");
                    stuff.Add("Olympian");
                    return sga.PrefixType(stuff[Main.rand.Next(0, stuff.Count)]);
                }
            }
            switch (rand.Next(13))
            {
                case 0:
                    return PrefixID.Heavy;
                case 1:
                    return PrefixID.Demonic;
                case 2:
                    return PrefixID.Frenzying;
                case 3:
                    return PrefixID.Dangerous;
                case 4:
                    return PrefixID.Savage;
                case 5:
                    return PrefixID.Furious;
                case 6:
                    return PrefixID.Terrible;
                case 7:
                    return PrefixID.Awful;
                case 8:
                    return PrefixID.Dull;
                case 9:
                    return PrefixID.Unhappy;
                case 10:
                    return PrefixID.Unreal;
                case 11:
                    return PrefixID.Shameful;
                case 12:
                    return PrefixID.Zealous;
            }
            return PrefixID.Pointy;

            #endregion
        }
    }

    public static class ThrowingUtils
    {
        public static float thrownDamage(this Player player)
        {
            return player.GetModPlayer<UThrowingPlayer>().thrownDamage;
        }
        public static float thrownVelocity(this Player player)
        {
            return player.GetModPlayer<UThrowingPlayer>().thrownVelocity;
        }
        public static int thrownCrit(this Player player)
        {
            return player.GetModPlayer<UThrowingPlayer>().thrownCrit;
        }
        public static bool thrownCost33(this Player player)
        {
            return player.GetModPlayer<UThrowingPlayer>().thrownCost33;
        }
        public static bool thrownCost50(this Player player)
        {
            return player.GetModPlayer<UThrowingPlayer>().thrownCost50;
        }
        public static bool thrown(this Item item)
        {
            return item.thrown;
            if (item.modItem == null)
                return false;
            return item.GetGlobalItem<UThrowingWeapon>().thrown;
        }
        public static bool thrown(this Projectile projectile)
        {
            return projectile.thrown;
            #region oldcode
            if (projectile.modProjectile == null)
                return false;
            return projectile.GetGlobalProjectile<UThrowingProjectile>().thrown;
            #endregion
        }







        public static UThrowingPlayer Throwing(this Player player)
        {
            return player.GetModPlayer<UThrowingPlayer>();
        }
        public static UThrowingWeapon Throwing(this Item item)
        {
            return item.GetGlobalItem<UThrowingWeapon>();
        }
        public static UThrowingProjectile Throwing(this Projectile projectile)
        {
            return projectile.GetGlobalProjectile<UThrowingProjectile>();
        }
        public static int DisplayedCritChance(Item item)
        {
            if ((Main.LocalPlayer.HeldItem.type != item.type))
            {
                return Main.LocalPlayer.Throwing().thrownCrit + item.crit;
            }
            else
            {
                return (Main.LocalPlayer.Throwing().thrownCrit - (Main.LocalPlayer.HeldItem.crit)) + item.crit;
            }
        }
    }

        public class UThrowingPlayer : ModPlayer
    {
        public bool thrownCost33
        {
            get
            {
                return player.thrownCost33;
            }

            set
            {
                player.thrownCost33 = value;
            }
        }
        public bool thrownCost50
        {
            get
            {
                return player.thrownCost50;
            }

            set
            {
                player.thrownCost50 = value;
            }
        }

        public float thrownVelocity
        {
            get
            {
                return player.thrownVelocity;
            }

            set
            {
                player.thrownVelocity = value;
            }
        }
            
        public float thrownDamage
        {
            get
            {
                return player.thrownDamage;
            }

            set
            {
                player.thrownDamage = value;
            }
        }
        public int thrownCrit
        {
            get
            {
                return player.thrownCrit;
            }

            set
            {
                player.thrownCrit = value;
            }
        }

        public override void ResetEffects()
        {
        }

        #region oldcode
#if ThrowingCompat
        //throwing compatible, for now
        public override void PostUpdateEquips()
        {

            return;
            int pre1 = thrownCrit;
            float pre2 = thrownDamage;
            bool pre3 = thrownCost33;
            bool pre4 = thrownCost50;
            float pre5 = thrownVelocity;

            //Modded
            thrownCrit += player.thrownCrit - 4;
            thrownDamage += player.thrownDamage - 1f;
            if (player.thrownCost33 == true)
                thrownCost33 = true;
            if (player.thrownCost50 == true)
                thrownCost50 = true;
            thrownVelocity += player.thrownVelocity - 1f;

            //Vanilla
            player.thrownCrit += pre1-4;
            player.thrownDamage += pre2 - 1f;
            if (pre3 == true)
                player.thrownCost33 = true;
            if (pre4 == true)
                player.thrownCost50 = true;
            player.thrownVelocity += pre5 - 1f;
        }
#endif
        #endregion
    }

    public class UThrowingNPCs : GlobalNPC
    {

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            return;
            #region oldcode
            UThrowingProjectile weapon = projectile.Throwing();
            UThrowingPlayer thrownPlayer = Main.player[projectile.owner].Throwing();
            if (projectile.owner > -1 && projectile.owner < 255)
            {
                if (weapon.thrown)
                {
                    //npc.AddBuff(BuffID.OnFire,10);
                    crit = false;
                    if (Main.rand.Next(0, 100) < thrownPlayer.thrownCrit && !projectile.trap)
                    {
                        crit = true;
                        //npc.AddBuff(BuffID.CursedInferno, 60);
                    }
                }
            }
            #endregion
        }
    }
}