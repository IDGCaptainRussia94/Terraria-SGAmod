using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SGAmod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class PrismalBooster : ModItem
	{
		int frameCounter = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismal Booster");
			Tooltip.SetDefault("Offers excellent mobility and stacks with the Space Diver armor\n" + SGAGlobalItem.pboostertextbase2);
		}

		public override void SetDefaults()
		{
			sbyte wingslo = item.wingSlot;
			item.CloneDefaults(ItemID.WingsVortex);
			item.width = 26;
			item.height = 38;
			item.value = 300000;
			item.accessory = true;
			item.wingSlot = wingslo;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Boost", SGAGlobalItem.pboostertextboost));
		}

		/*public override string Texture
		{
			get { return mod.GetTexture("Items/CirnoWings_Wings"); }
		}*/

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.wingTimeMax = 150;
			player.GetModPlayer<SGAPlayer>().SpaceDiverWings += 1.5f;

			if (player.velocity.Y == 0f || !player.controlJump)
			{
				frameCounter = 0;
				player.wingFrame = 0;
			}
		}

		public override bool WingUpdate(Player player, bool hideVisual)
		{
			frameCounter++;

			if (player.velocity.Y == 0f || !player.controlJump)
			{
				frameCounter = 0;
				player.wingFrame = 0;
			}
			else
			{
					if (player.wingTime > 0)
						player.wingFrame = 1 + (int)(frameCounter / 2) % 3;
					else
						player.wingFrame = 1 + (int)(frameCounter / 4) % 3;

					return true;
			}
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 15);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.35f;
			ascentWhenRising = 0.2f;
			maxCanAscendMultiplier = 1.5f;
			maxAscentMultiplier = 1.5f;
			constantAscend = 0.35f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 7f;
			acceleration *= 3f;
		}
	}
}