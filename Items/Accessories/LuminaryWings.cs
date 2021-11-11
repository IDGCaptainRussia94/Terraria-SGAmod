using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent.Generation;

namespace SGAmod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class LuminaryWings : DemonSteppers,IAuroraItem
	{
		int frameCounter = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminary Wings");
			Tooltip.SetDefault("'A gift from the heavens for a worthy vessel.' Grants superior wingtime!\nEffects of (Most of) Cirno's Wings, and Prismal Booster (hide this accessory to disable Booster)\nYour movement speed is increased and your max fall speed is doubled");
		}

		public override void SetDefaults()
		{
			sbyte wingslo = item.wingSlot;
			item.CloneDefaults(ItemID.WingsVortex);
			item.width = 26;
			item.height = 38;
			item.value = 2500000;
			item.accessory = true; 
			item.rare = ItemRarityID.Purple;
			item.expert = true;
			item.wingSlot = wingslo;
		}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.Add(new TooltipLine(mod, "Boost", SGAGlobalItem.pboostertextboost));
		}

		/*public override string Texture
		{
			get { return mod.GetTexture("Items/CirnoWings_Wings"); }
		}*/

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			//base.UpdateAccessory(player, true);
			Lighting.AddLight(player.Center, Color.HotPink.ToVector3() * 2.5f * Main.essScale);
			(mod.GetItem("CirnoWings") as CirnoWings).UpdateAccessoryLocal(player, true, false);
			if (!hideVisual)
				player.GetModPlayer<SGAPlayer>().SpaceDiverWings += 1.5f;
			int y_bottom_edge = (int)(player.position.Y + (float)player.height + 16f) / 16;
			int x_edge = (int)(player.Center.X) / 16;

			Tile mytile = Framing.GetTileSafely(x_edge, y_bottom_edge);

			//ModContent.GetInstance<DemonSteppers>().UpdateAccessory(player, false);

			if (mytile.active() || player.velocity.Y == 0)
			{
				if (!player.GetModPlayer<SGAPlayer>().Walkmode)
				{
					player.accRunSpeed += 3f;
					player.runAcceleration += 0.15f;
				}
				player.rocketBoots = 2;
			}
			player.maxFallSpeed += 10f;
			player.wingTimeMax = 300;


		}

		public override bool WingUpdate(Player player, bool hideVisual)
		{
			frameCounter += 1;
			if (!player.controlJump)
				return false;

			int y_bottom_edge = (int)(player.position.Y + (float)player.height + 16f) / 16;
			int x_edge = (int)(player.Center.X) / 16;

			Tile mytile = Framing.GetTileSafely(x_edge, y_bottom_edge);

			if (!mytile.active() && Math.Abs(player.velocity.Y) > 0)
			{

				if (player.wingFrameCounter > 0)
				{
					int DustID2 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, 72, player.velocity.X * 0.6f, (player.velocity.Y + 4f) * 0.6f, 150, Main.hslToRgb((float)(frameCounter / 30f) % 1, 1f, 0.9f) * 0.2f, 1f);
					Main.dust[DustID2].noGravity = true;
					int num316 = Dust.NewDust(new Vector2(player.position.X, player.position.Y), player.width, player.height, mod.DustType("NovusSparkleBlue"), player.velocity.X * 0.1f, (player.velocity.Y) * 0.1f, 50, Color.White, 1.5f);
					Main.dust[num316].noGravity = true;
					Main.dust[DustID2].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
					Main.dust[num316].shader = GameShaders.Armor.GetSecondaryShader(player.cWings, player);
				}

			}

			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(mod.ItemType("DemonSteppers"), 1);
			recipe.AddIngredient(mod.ItemType("CirnoWings"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBooster"), 1);
			recipe.AddIngredient(mod.ItemType("AuroraTearAwoken"), 2);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 40);
			recipe.AddIngredient(mod.ItemType("OmniSoul"), 30);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 25);
			recipe.AddIngredient(mod.ItemType("MoneySign"), 20);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{

			ascentWhenFalling = 0.85f;
			ascentWhenRising = 0.35f;
			maxCanAscendMultiplier = player.jumpSpeedBoost > 0 ? 1.50f : 2.25f;
			maxAscentMultiplier = player.jumpSpeedBoost > 0 ? 1.50f : 2.25f;
			constantAscend = 0.235f;
			if (player.SGAPly().Walkmode)
			{
				ascentWhenFalling /= 2;
				ascentWhenRising /= 2;
				maxCanAscendMultiplier /= 2;
				maxAscentMultiplier /= 2;
				constantAscend /= 2;
			}
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 12f;
			acceleration *= 8f;
			if (player.SGAPly().Walkmode)
            {
				speed /= 2f;
				acceleration /= 2f;
			}

		}
	}
}