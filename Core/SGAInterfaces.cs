using System;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod
{
	public interface IDevItem
	{
		(string, string) DevName();

	}
	public interface IDedicatedItem
	{
		string DedicatedItem();
	}
	public interface IRadioactiveItem
	{
		int RadioactiveHeld();
		int RadioactiveInventory();
	}
	public interface IShieldItem
	{

	}
	public interface IPotionCantBeInfinite
	{

	}
	public interface IDevArmor
	{

	}
	public interface IRustBurnText
	{

	}
	public interface IDankSlowText
	{

	}
	public interface IRadioactiveDebuffText
	{

	}
	public interface IShieldBashProjectile
	{

	}
	public interface INoHitItem
	{

	}
	public interface IAuroraItem
	{

	}
	public interface IManifestedItem
    {

    }
	public interface IJablinItem
	{

	}
	public interface IMangroveSet
	{

	}
	public interface IBreathableWallType
	{

	}
	public interface IHellionDrop
	{
		int HellionDropAmmount();
		int HellionDropType();


	}
	public interface INonDestructableProjectile
	{

	}
	public interface IConsumablePickup
	{

	}
	public interface ITrueMeleeProjectile
	{

	}
	public interface IDrawAdditive
	{
		void DrawAdditive(SpriteBatch spriteBatch);
	}

	public interface IPostEffectsDraw
	{
		void PostEffectsDraw(SpriteBatch spriteBatch,float drawScale);
	}
	public interface IDrawThroughFog
	{
		void DrawThroughFog(SpriteBatch spriteBatch);
	}
	public interface IHitScanItem
	{

	}
	public interface ITechItem
	{
		float ElectricChargeScalingPerUse();

		/*int MaxElectricCharge();
		int ElectricChargePerUse();
		int ElectricChargeWhileInUse();*/
	}
	public interface ISGABoss
	{
		string Trophy();
		bool Chance();

		string RelicName();

		void NoHitDrops();

		string MasterPet();
		bool PetChance();

	}

}