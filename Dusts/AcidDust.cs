using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.Dusts
{
	public class AcidDust : ModDust
	{
		float fly=0f;
		public override void OnSpawn(Dust dust)
		{
			dust.velocity.Y = Main.rand.Next(-10, 6) * 0.1f;
			dust.velocity.X *= 0.3f;
			dust.scale *= 1.5f;
			//fly = Main.rand.NextFloat(-1f, 1f);
		}

		public override bool MidUpdate(Dust dust)
		{
			if (!dust.noGravity)
			{
				dust.velocity.Y += 0.05f;
				dust.velocity.X += (0.05f * (dust.velocity.X > 0 ? -1f : 1f));
			}
			if (!dust.noLight)
			{
				float strength = dust.scale * 1.4f;
				if (strength > 1f)
				{
					strength = 1f;
				}
				Lighting.AddLight(dust.position, 0.01f * strength, 0.1f * strength, 0.01f * strength);
			}
			return true;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, 25);
		}
	}
}