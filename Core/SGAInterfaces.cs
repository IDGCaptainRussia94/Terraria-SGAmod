using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod
{

	interface IShieldItem
	{

	}
	interface IShieldBashProjectile
	{

	}
	interface IDrawAdditive
	{
		void DrawAdditive(SpriteBatch spriteBatch);
	}

	interface IPostEffectsDraw
	{
		void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale);
	}
	interface IHitScanItem
	{
	}
	interface ITechItem
	{
		/*int MaxElectricCharge();
		int ElectricChargePerUse();
		int ElectricChargeWhileInUse();*/
	}
	interface IHopperInterface
	{
		bool HopperInputItem(Item item,Point tilelocation,int movementCount);
	}	
	interface ISGABoss
	{
		string Trophy();
		bool Chance();
	}

}