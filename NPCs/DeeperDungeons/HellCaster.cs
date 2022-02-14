using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class HellCaster : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hell Caster");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.FireImp]; //10
        }

        public override void SetDefaults()
        {
            npc.width = 24;
            npc.height = 48;
            npc.damage = 30;
            npc.defense = 9;
            npc.lifeMax = 75;
            npc.value = 150f;
            npc.aiStyle = 8;
            npc.knockBackResist = 0.4f;
            npc.buffImmune[BuffID.OnFire] = true;
            aiType = NPCID.FireImp;
            animationType = NPCID.FireImp;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath2;
            npc.buffImmune[BuffID.Confused] = false;
            banner = npc.type;
            bannerItem = mod.ItemType("HellCasterBanner");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * -16, 0), npc.velocity, mod.GetGoreSlot("Gores/HellCaster_Arm"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, mod.GetGoreSlot("Gores/HellCaster_Arm"), 1f);
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 8, 0), npc.velocity, mod.GetGoreSlot("Gores/HellCaster_Leg"), 1f);
            }
        }

        public override void NPCLoot()
        {
            if (Main.rand.NextBool())
            {
                NPC.NewNPC((int)npc.position.X, (int)npc.position.Y, mod.NPCType("FlamingSkull"));
                Main.PlaySound(SoundID.Zombie, npc.position, 53);
            }
            else
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, mod.GetGoreSlot("Gores/HellCaster_Head"), 1f);
            }
            
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
        }
    }
}