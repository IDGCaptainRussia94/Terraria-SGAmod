using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            List<int> itemx = Dimensions.DeeperDungeon.CommonItems.ToList();
            itemx.AddRange(Dimensions.DeeperDungeon.RareItems);
            int itemz = itemx[Main.rand.Next(itemx.Count)];

            Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, itemz, itemz == ModContent.ItemType<Items.Weapons.Almighty.Megido>() ? Main.rand.Next(1, 3) : 1);

        }
    }
}