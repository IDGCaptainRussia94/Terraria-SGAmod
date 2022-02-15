using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class FlamingSkull : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flaming Skull");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.DemonEye]; //2
        }

        public override void SetDefaults()
        {
            npc.width = 22;
            npc.height = 22;
            npc.damage = 35;
            npc.defense = 0;
            npc.lifeMax = 100;
            npc.value = 100f;
            npc.aiStyle = 2;
            npc.knockBackResist = 0.2f;
            npc.buffImmune[BuffID.OnFire] = true;
            aiType = NPCID.DemonEye;
            animationType = NPCID.DemonEye;
            npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath14;
            npc.buffImmune[BuffID.Confused] = false;
            banner = npc.type;
            bannerItem = mod.ItemType("FlamingSkullBanner");
        }
        public override void PostAI()
        {
            if (Main.rand.NextBool())
            {
                Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Fire);
            }
            Lighting.AddLight(npc.Center, 0.26f, 0.08f, 0.02f);
        }
        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust killDust = Main.dust[Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 2, 2)];
                    killDust.noGravity = true;
                    Dust killDust2 = killDust;
                    killDust2.velocity *= 2f;
                }
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, mod.GetGoreSlot("Gores/HellCaster_Head"), 1f);
            }
        }
        public override Color? GetAlpha(Color newColor)
        {
            return new Color(255, 255, 255, 255);
        }
        public override void OnHitPlayer(Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.OnFire, 120);
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
        }
    }
}