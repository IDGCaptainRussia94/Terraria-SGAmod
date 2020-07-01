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
            npc.damage = 34;
            npc.defense = 8;
            npc.lifeMax = 20;
            npc.value = 0f;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.noGravity = true;
            npc.aiStyle = 5;
            aiType = NPCID.BeeSmall;
            animationType = NPCID.BeeSmall;
            banner = npc.type;
            bannerItem = mod.ItemType("MaggotFlyBanner");
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fly");
            Main.npcFrameCount[npc.type] = Main.npcFrameCount[NPCID.BeeSmall];
        }
    }
}
