using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class DungeonBat : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Bat");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.CaveBat]; //5
        }

        public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 18;
            npc.damage = 20;
            npc.defense = 0;
            npc.lifeMax = 32;
            npc.value = 100f;
            npc.aiStyle = 14;
            npc.knockBackResist = 0.3f;
            npc.npcSlots = 0.5f;
            aiType = NPCID.CaveBat;
            animationType = NPCID.CaveBat;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath4;
            npc.buffImmune[BuffID.Confused] = false;
            banner = npc.type;
            bannerItem = mod.ItemType("DungeonBatBanner");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                //Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/DungeonBat_Gore"), 1f);
                Gore.NewGore(npc.position, npc.velocity + new Vector2(npc.spriteDirection * -8, 0), mod.GetGoreSlot("Gores/DungeonBat_Gore"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.Next(250) == 0) //0.4% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.ChainKnife);
            }
            if (Main.rand.Next(100) == 0) //1% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.DepthMeter);
            }
            if (Main.rand.Next(5) == 0) //20% chance
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ItemID.Bone);
            }
        }
    }
}