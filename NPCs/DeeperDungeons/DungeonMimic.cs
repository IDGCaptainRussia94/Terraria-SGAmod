using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.DeeperDungeons
{
    public class DungeonMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dungeon Mimic");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.Mimic];
        }

        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.Mimic);
            aiType = NPCID.Mimic;
            animationType = NPCID.Mimic;
            npc.buffImmune[BuffID.Confused] = false;
            banner = Item.NPCtoBanner(NPCID.Mimic);
            bannerItem = Item.BannerToItem(banner);
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 16, 0), npc.velocity, Main.rand.Next(61, 63), 1f); //Smoke gore
            }
        }

        public override void NPCLoot()
        {
            switch(Main.rand.Next(10))
            {
            case 0:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.RingOfRespite>());
                break;
            case 1:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.NinjaSash>());//Ninja's Stash
                break;
            case 2:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Auras.StoneBarrierStaff>());
                break;
            case 3:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.DiesIraeStone>());
                break;
            case 4:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.MagusSlippers>());
                break;
            case 5:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Auras.BeserkerAuraStaff>());//Berserker Aura Staff
                break;
            case 6:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.EnchantedFury>());
                break;
            case 7:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.CardDeckPersona>());//Hearts of the Cards
                break;
            case 8:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Accessories.YoyoTricks>());//Professional's Drop
                break;
            case 9:
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Items.Weapons.Almighty.Megido>(), Main.rand.Next(1, 3));
                break;
            default:
                break;
            }
        }
    }
}