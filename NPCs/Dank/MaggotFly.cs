using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Dank
{
    public class MaggotFly : ModNPC
    {
        public override void SetDefaults()
        {
            npc.CloneDefaults(NPCID.BeeSmall);
            npc.width = 40;
            npc.height = 30;
            npc.damage = 84;
            npc.defense = 25;
            npc.lifeMax = 500;
            npc.knockBackResist = 0f;
            npc.value = 1500f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.aiStyle = 5;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
            banner = npc.type;
            bannerItem = mod.ItemType("MaggotFlyBanner");
        }

        public override void HitEffect(int hitDirection, double damage)
        {
            if (npc.life < 1)
            {
                Gore.NewGore(npc.Center + new Vector2(npc.spriteDirection * 4, 0), npc.velocity, mod.GetGoreSlot("Gores/MaggotFly_gib"), 1f);
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Maggot Fly");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }
    }
}
