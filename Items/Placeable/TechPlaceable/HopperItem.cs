using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Placeable.TechPlaceable
{
	public class LiquidationHopperItem : HopperItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Liquidation Hopper");
			Tooltip.SetDefault("Sells items into 10% of their worth and outputs the coins\nCan only process 10 items from a stack at a time\nWill not function if the total value is under 10 copper\nRetains all previous functionality of a Hopper");
		}
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 10);
			item.rare = ItemRarityID.Yellow;
			item.createTile = mod.TileType("LiquidationHopperTile");
			item.placeStyle = 0;
			item.mech = true;
		}

        public override Color? GetAlpha(Color lightColor)
        {
			return Color.Yellow;
        }

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.GoldCoin];
				Vector2 drawPos = item.Center;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, lightColor, 0f, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);
			}
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,
	Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.GoldCoin];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, Color.White, 0f, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HopperItem>(), 1);
			recipe.AddIngredient(ItemID.GoldCoin, 5);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class HopperChestItem : HopperItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Chest Hopper");
			Tooltip.SetDefault("Hopper but with added functionality to pull items from chests and compatible outputting tiles");
		}
        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 10);
			item.rare = ItemRarityID.LightPurple;
			item.createTile = mod.TileType("ChestHopperTile");
			item.placeStyle = 0;
			item.mech = true;
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.cursorTextures[7];
				Vector2 drawPos = item.Center;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, lightColor, 0f, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);
			}
		}

        public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor,
	Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.cursorTextures[7];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale * 1f, SpriteEffects.None, 0f);
			}
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HopperItem>(), 1);
			recipe.AddIngredient(ItemID.GoldenKey, 1);
			recipe.AddRecipeGroup("SGAmod:Chests", 1);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}	
	public class HopperItem : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Hopper");
			Tooltip.SetDefault("Picks up items that fall onto its top, can be placed mid-air\nHoppers place items into chests and other chained Hoppers\nHammer the hopper to change its output position\nHoppers can be disabled by sending a wire signal to them");
		}
        public override void SetDefaults()
		{
			item.width = 30;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 10);
			item.rare = 1;
			item.createTile = mod.TileType("HopperTile");
			item.placeStyle = 0;
		}
        public override string Texture => "SGAmod/Items/Placeable/TechPlaceable/HopperItem";

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("UnmanedBar"), 3);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 3);
			recipe.AddIngredient(ItemID.MetalSink, 1);
			recipe.AddTile(TileID.HeavyWorkBench);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
}