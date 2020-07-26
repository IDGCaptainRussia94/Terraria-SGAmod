using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using AAAAUThrowing;

namespace SGAmod.SkillTree.Survival
{
    public class SkillSurvival : Skill
    {
        public SkillSurvival(List<ushort> linkedSkillsushort = null) : base(linkedSkillsushort)
        {
            skillColor = Color.Orange;
        }

    }
    public class SkillTrueStrike : SkillSurvival
    {
        public SkillTrueStrike(List<ushort> linkedSkillsushort = null) : base(linkedSkillsushort)
        {
            maxlevels = 5;
            buycost = new ushort[] { 100, 100, 150, 150, 200 };//500
            displayname = "True Strike";
            displaytext = "True Melee weapons do 5% more damage per level, but swing 2% slower per level";
            treelocation = new Vector2(0.50f, 2);
        }
        public override void GetWeaponDamage(Item item, ref int damage)
        {
            if (item.shoot < 1)
                damage = (int)((float)damage * getlevels * 0.05f);
        }
        public override void UseTimeMultiplier(Item item, ref float current)
        {
            if (item.melee)
            {
                current += 0.02f * getlevels;
            }
        }
    }

    public class SkillNoSurprises : SkillSurvival
    {
        public SkillNoSurprises(List<ushort> linkedSkillsushort = null) : base(linkedSkillsushort)
        {
            maxlevels = 10;
            unlockcost = 500;
            buycost = new ushort[] { 75, 75, 100, 100, 125, 125, 125, 125, 175, 175 };//1200
            displayname = "No Surprises";
            displaytext = "You gain 5% of your melee+throwing crit chance (a combined value between 0-200) per level as defense while your health is full, this effect has a 5 second cooldown that resets whenever damaged";
            treelocation = new Vector2(0.70f, 2);
        }

        public override void PostUpdateEquips()
        {
            if (player.statLife > player.statLifeMax2 - 1 && player.SGAPly().surprised<1)
            {
                float adder = ((float)player.Throwing().thrownCrit + (float)player.meleeCrit)*(0.05f*getlevels);
                player.statDefense += (int)adder;
            }
        }
        public override void OnPlayerDamage(ref int damage, ref bool crit, NPC npc, Projectile proj)
        {
            player.SGAPly().surprised=Math.Max(player.SGAPly().surprised,60*5);
        }
    }

    public class SkillBulwark : SkillSurvival
    {
        public SkillBulwark(List<ushort> linkedSkillsushort = null) : base(linkedSkillsushort)
        {
            maxlevels = 5;
            unlockcost = 500;
            buycost = new ushort[] {150,150,200,200,300};//1000
            displayname = "Bulwark";
            displaytext = "If a shield is visibly equipped it grants 2 defense per level; this doesn’t stack with other worn shields";
            treelocation = new Vector2(0.30f, 2);
        }

        public override void PostUpdateEquips()
        {
            if (player.shield>0)
            {
                player.statDefense += getlevels*2;
            }
        }
    }


    }
