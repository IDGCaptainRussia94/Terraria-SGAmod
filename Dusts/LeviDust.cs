using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Dusts
{
		public class LeviDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, Main.rand.Next(0, 3) * 10, 14, 10);
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity *= 0.95f;
			dust.scale -= 0.01f;
			dust.color = dust.color * ((float)dust.alpha / 100f);

			dust.alpha -= 1;
			if (dust.alpha < 1)
				dust.scale -= 0.02f;
			if (dust.scale <= 0f)
				dust.active = false;
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(1f,1f,1f, dust.alpha)*0.65f;
		}
	}
}