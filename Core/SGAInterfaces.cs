using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod
{

	interface IDrawAdditive
	{
		void DrawAdditive(SpriteBatch spriteBatch);
	}
	interface IHitScanItem
	{
	}
	interface ISGABoss
	{
		string Trophy();
		bool Chance();
	}

}