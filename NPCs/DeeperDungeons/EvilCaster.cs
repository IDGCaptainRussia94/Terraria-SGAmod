using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class EvilCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Evil Caster");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DesertDjinn]; //16
        }

        public override void SetDefaults()
        {
            npc.width = 18;
            npc.height = 48;
            npc.damage = 40;
            npc.defense = 7;
            npc.lifeMax = 160;
            npc.value = 190f;
            npc.aiStyle = 8;
            npc.knockBackResist = 0.3f;
            npc.buffImmune[BuffID.CursedInferno] = true;
            npc.buffImmune[BuffID.Ichor] = true;
            aiType = NPCID.DesertDjinn;
            animationType = NPCID.DesertDjinn;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.buffImmune[BuffID.Confused] = false;
            banner = npc.type;
            bannerItem = mod.ItemType("EvilCasterBanner");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, mod.GetGoreSlot("Gores/EvilCaster_Head"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * -16, 0), npc.velocity, mod.GetGoreSlot("Gores/EvilCaster_Arm"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, mod.GetGoreSlot("Gores/EvilCaster_Arm"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, mod.GetGoreSlot("Gores/EvilCaster_Leg"), 1f);
            }
        }

        public override void NPCLoot()
        {
            
            if (Main.rand.Next(100) < 98) //98% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone, Main.rand.Next(1, 3));
            }
            if (Main.rand.Next(65) == 0) //1.53% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.GoldenKey);
            }
            if (Main.rand.Next(250) == 0) //0.4% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.BoneWand);
            }
            if (Main.rand.Next(300) == 0) //0.33% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ClothierVoodooDoll);
            }
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.TallyCounter);
            }
            if (Main.rand.Next(2) == 0) //50% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.CursedFlame, Main.rand.Next(1, 2));
            }
            if (Main.rand.Next(2) == 0) //50% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Ichor, Main.rand.Next(1, 2));
            }
        }
    }
}