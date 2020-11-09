using Idglibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class CirnoWings : ModItem
	{
		int frameCounter = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cirno's Wings");
			Tooltip.SetDefault("Low Flight time, but very high mobility\nGain Immunity to Chilled, Frozen, Frostburn, and Snowfrosted\nTake 20% less damage from cold sources and deal 20% increased cold projectile damage\nCold Damage projectiles inflict Frostburn\nMagic attacks rarely inflict Frostburn" +
				"\nYou gain wing time in the snow biome, "+ Idglib.ColorText(Color.Red, "But halved in the Desert, Jungle, and Underworld due to heat") + "\nInDev Ice Fairy <3");
		}

		public override void SetDefaults()
		{
			sbyte wingslo=item.wingSlot;
			item.CloneDefaults(ItemID.FrozenWings);
			item.width = 26;
			item.height = 38;
			item.value = 300000;
			item.accessory = true;
			item.expert=true;
			item.wingSlot=wingslo;
		}

		/*public override string Texture
		{
			get { return mod.GetTexture("Items/CirnoWings_Wings"); }
		}*/

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			UpdateAccessoryLocal(player, hideVisual);
		}

		public void UpdateAccessoryLocal(Player player, bool hideVisual,bool doBiomeThings=true)
		{
			frameCounter++;
			player.wingTimeMax = player.ZoneSnow ? 100 : 50;
			player.GetModPlayer<SGAPlayer>().CirnoWings = true;

			if (doBiomeThings)
			{
				if (player.ZoneDesert || player.ZoneJungle || player.ZoneUnderworldHeight)
				{
					if (player.wingTime > 3)
						player.wingTime -= 1;
				}
			}
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.55f;
			ascentWhenRising = 0.15f;
			maxCanAscendMultiplier = 1f;
			maxAscentMultiplier = 1f;
			constantAscend = 0.135f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 9f;
			acceleration *= 4.5f;
		}
	}
}