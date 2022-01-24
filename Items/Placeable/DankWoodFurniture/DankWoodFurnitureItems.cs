using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.HavocGear.Items;

namespace SGAmod.Items.Placeable.DankWoodFurniture
{
    #region Dank Wood Workbench
    public class DankWoodWorkbench : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 14;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodWorkbench>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
    #endregion
    #region Dank Wood Chair
    public class DankWoodChair : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodChair>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 4);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Toilet
	public class DankWoodToilet : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 26;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 150;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodToilet>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 6);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Table
	public class DankWoodTable : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodTable>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Dresser
	public class DankWoodDresser : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded dresser.");
		}

		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodDresser>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 16);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Bed
	public class DankWoodBed : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 28;
			item.height = 20;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 2000;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBed>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 15);
			recipe.AddIngredient(ItemID.Silk, 5);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Sofa
	public class DankWoodSofa : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 38;
			item.height = 24;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodSofa>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 5);
			recipe.AddIngredient(ItemID.Silk, 2);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Lantern
	internal class DankWoodLantern : ModItem
    {
        public override void SetDefaults()
        {
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.useAnimation = 15;
            item.useTime = 10;
			item.autoReuse = true;
            item.maxStack = 99;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodLantern>();
            item.width = 10;
            item.height = 24;
            item.value = 150;
        }
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 6);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
    #endregion
    #region Dank Wood Lamp
    internal class DankWoodLamp : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 99;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodLamp>();
			item.width = 10;
			item.height = 24;
			item.value = 500;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Bookcase
	public class DankWoodBookcase : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded bookcase.");
		}

		public override void SetDefaults()
		{
			item.width = 24;
			item.height = 32;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBookcase>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 20);
			recipe.AddIngredient(ItemID.Book, 10);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Torch
	public class DankWoodTorch : ModItem
	{
		public override void SetStaticDefaults()
		{
			//Tooltip.SetDefault("This is a modded torch.");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 12;
			item.maxStack = 999;
			item.holdStyle = 1;
			item.noWet = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodTorch>();
			item.flame = true;
			item.value = 50;
		}

		public override void HoldItem(Player player)
		{
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Fire);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.75f, 0.30f);
		}

		public override void PostUpdate()
		{
			if (!item.wet)
			{
				Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 1f, 0.75f, 0.30f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 1);
			recipe.AddIngredient(ItemID.Gel, 1);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
			recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankCore>(), 1);
			recipe.AddIngredient(ItemID.Torch, 3);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Clock
	public class DankWoodClock : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 26;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodClock>();
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 10);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 3);
			recipe.AddIngredient(ItemID.Glass, 6);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Piano
	public class DankWoodPiano : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodPiano>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bone, 4);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 15);
			recipe.AddIngredient(ItemID.Book, 1);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Sink
	public class DankWoodSink : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 30;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodSink>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 6);
			recipe.AddIngredient(ItemID.WaterBucket, 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Dank Wood Bathtub
	public class DankWoodBathtub : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 30;
			item.height = 22;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.value = 300;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.DankWoodBathtub>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<DankWood>(), 14);
			recipe.AddTile(TileID.Sawmill);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Novite Candle
	internal class NoviteCandle : ModItem
	{
		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 20;
			item.maxStack = 999;
			item.holdStyle = 1;
			item.noWet = true;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.NoviteCandle>();
			item.flame = true;
			item.value = 0; //candles have no value for some reason.
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 1);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void HoldItem(Player player)
		{
			if (Main.rand.Next(player.itemAnimation > 0 ? 40 : 80) == 0)
			{
				Dust.NewDust(new Vector2(player.itemLocation.X + 16f * player.direction, player.itemLocation.Y - 14f * player.gravDir), 4, 4, DustID.Fire);
			}
			Vector2 position = player.RotatedRelativePoint(new Vector2(player.itemLocation.X + 12f * player.direction + player.velocity.X, player.itemLocation.Y - 14f + player.velocity.Y), true);
			Lighting.AddLight(position, 1f, 0.75f, 0.30f);
		}

		public override void PostUpdate()
		{
			if (!item.wet)
			{
				Lighting.AddLight((int)((item.position.X + item.width / 2) / 16f), (int)((item.position.Y + item.height / 2) / 16f), 1f, 0.75f, 0.30f);
			}
		}

		public override void AutoLightSelect(ref bool dryTorch, ref bool wetTorch, ref bool glowstick)
		{
			dryTorch = true;
		}
	}
	#endregion
	#region Novite Chandelier
	internal class NoviteChandelier : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 99;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.NoviteChandelier>();
			item.width = 30;
			item.height = 28;
			item.value = 3000;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 4);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 4);
			recipe.AddIngredient(ItemID.Chain);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
	#region Novite Candelabra
	internal class NoviteCandelabra : ModItem
	{
		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTurn = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.autoReuse = true;
			item.maxStack = 99;
			item.consumable = true;
			item.createTile = ModContent.TileType<Tiles.DankWoodFurniture.NoviteCandelabra>();
			item.width = 22;
			item.height = 28;
			item.value = 1500;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteBar>(), 3);
			recipe.AddIngredient(ModContent.ItemType<DankWoodTorch>(), 3);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	#endregion
}