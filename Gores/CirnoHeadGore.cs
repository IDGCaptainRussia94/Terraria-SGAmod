using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SGAmod.Gores
{
    internal class CirnoHeadGore : ModGore
    {
        public override void OnSpawn(Gore gore)
        {
            gore.timeLeft = 600;
            gore.frame = 1;
            gore.numFrames = 1;
        }        

        public override bool Update(Gore gore)
        {
            if (gore.timeLeft > 600) gore.timeLeft = 600;

            var color = Color.Aqua * MathHelper.Clamp((gore.timeLeft-160) / 80f, 0f, 1f);

            if (Main.rand.NextFloat(MathHelper.Clamp((gore.timeLeft - 160) / 80f, 0f, 1f) * 100) < 98)
            {
                Dust dust = Dust.NewDustPerfect(gore.position + new Vector2(12, 12) + (Vector2.One.RotatedByRandom(MathHelper.TwoPi) * 16f) * Main.rand.NextFloat(), 59, Vector2.One.RotatedByRandom(MathHelper.TwoPi) * Main.rand.NextFloat(1f), 255 - (int)(MathHelper.Clamp((gore.timeLeft - 160) / 180f, 0f, 1f) * 255f), color, 1.7f);
                dust.noGravity = false;
                Lighting.AddLight(gore.position, color.ToVector3() * 0.3f);
            }
            return true;
        }
    }
}