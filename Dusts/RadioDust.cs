	using System;
	using System.IO;
	using System.Collections.Generic;
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
	using Terraria;
using Terraria.Utilities;
	using Terraria.ModLoader;
	using Terraria.ID;

namespace SGAmod.Dusts
{

	public class RadioDust : ModDust
	{
		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 28, 28);
		}

		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.velocity *= 0.95f;
			dust.scale -= 0.02f;
			dust.color = dust.color * ((float)dust.alpha / 100f);

			dust.alpha -= 1;

			if (dust.alpha % 40 == 0)
				dust.frame.Y += 28;
			if (dust.alpha < 1)
				dust.scale -= 0.02f;
			if (dust.scale <= 0f || dust.frame.Y >= 28*3)
				dust.active = false;
			return false;
		}

		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return new Color(lightColor.R, lightColor.G, lightColor.B, dust.alpha);
		}
	}

	public class ViralDust : RadioDust
    {
        public override bool Autoload(ref string name, ref string texture)
        {
			texture = "SGAmod/Items/Accessories/LostNotes";
			return true;
        }

		public override void OnSpawn(Dust dust)
		{
			dust.noGravity = true;
			dust.frame = new Rectangle(0, 0, 24, 32);
		}

	}

}