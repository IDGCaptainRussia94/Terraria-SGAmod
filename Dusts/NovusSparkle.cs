using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Dusts
{
	public class NovusSparkle : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.15f;
			dust.noGravity = true;
			//dust.noLight = true;
			dust.scale *= 1.2f;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * -0.15f;
			dust.scale *= 0.95f;
			float light = 0.6f * dust.scale;
			if (dust.alpha==181)
			light*=3.5f;
			Lighting.AddLight(dust.position, (dust.color.R/255f)*light, (dust.color.G/600)*light, (dust.color.B/260)*light);
			if (dust.scale < 0.25f)
			{
				dust.active = false;
			}
			return false;
		}

		/*public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(dust.color.R, dust.color.G, dust.color.B, 180);
		}*/

	}

	public class NovusSparkleBlue : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.velocity *= 0.15f;
			dust.noGravity = true;
			//dust.noLight = true;
			dust.scale *= 1.2f;
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.rotation += dust.velocity.X * -0.15f;
			dust.scale *= 0.95f;
			float light = 0.6f * dust.scale;
			Lighting.AddLight(dust.position, (dust.color.R / 500f) * light, (dust.color.G / 500) * light, (dust.color.B / 160) * light);
			if (dust.scale < 0.25f)
			{
				dust.active = false;
			}
			return false;
		}

		/*public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(dust.color.R, dust.color.G, dust.color.B, 180);
		}*/

	}
}