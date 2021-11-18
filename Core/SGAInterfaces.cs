using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod
{
	interface IDevItem
	{
		(string, string) DevName();

	}
	interface IRadioactiveItem
	{
		int RadioactiveHeld();
		int RadioactiveInventory();
	}
	interface IShieldItem
	{

	}

	interface IRustBurnText
	{

	}

	interface IDankSlowText
	{

	}
	interface IRadioactiveDebuffText
	{

	}
	interface IShieldBashProjectile
	{

	}
	interface INoHitItem
	{

	}
	interface IAuroraItem
	{

	}	
	interface IManifestedItem
    {

    }
	interface IJablinItem
	{

	}
	interface IMangroveSet
	{

	}
	interface INonDestructableProjectile
	{

	}
	interface IConsumablePickup
	{

	}
	interface ITrueMeleeProjectile
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
	interface ISGABoss
	{
		string Trophy();
		bool Chance();

		string RelicName();

		void NoHitDrops();

	}

}