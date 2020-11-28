using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Tools
{
	public class BoreicPickaxe : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Pickaxe");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			tooltips.Add(new TooltipLine(mod, "Faster Icey Item", "25% faster use speed in the snow biome"));
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MythrilPickaxe);
			item.useTime = 10;
		}
		
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 10);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 10);
			recipe.AddTile(TileID.IceMachine);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(7) == 0)
			{
				SGAPlayer sgaplayer = player.GetModPlayer(mod,typeof(SGAPlayer).Name) as SGAPlayer;
				int num316 = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 92, 0f, 0f, 50, Main.hslToRgb(0.6f, 0.9f, 1f), 0.5f);
				Main.dust[num316].noGravity = true;
			}
		}

		public override float UseTimeMultiplier(Player player)
		{
			if (player.ZoneSnow)
			return 1.25f;
			return 1f;
		}

	}

	public class BoreicDrill : BoreicPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MythrilDrill);
			item.shoot = mod.ProjectileType("BoreicDrillProj");
			item.useTime = 10;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 8);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 8);
			recipe.AddTile(TileID.IceMachine);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
		public class BoreicHamaxe : BoreicPickaxe
		{
			public override void SetStaticDefaults()
			{
				DisplayName.SetDefault("Boreic Hamaxe");
			}

			public override void SetDefaults()
			{
				item.CloneDefaults(ItemID.MythrilWaraxe);
				item.hammer = 50;
			item.damage = 20;
			item.useTime = 10;
			item.useAnimation = 20;
		}

			public override void AddRecipes()
			{
				ModRecipe recipe = new ModRecipe(mod);
				recipe.AddIngredient(mod.ItemType("CryostalBar"), 12);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 8);
			recipe.AddTile(TileID.IceMachine);
				recipe.SetResult(this);
				recipe.AddRecipe();
			}

		}

	public class BoreicJacksaw : BoreicPickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Jacksaw");
		}

		public override void SetDefaults()
		{
			item.damage = 26;
			item.melee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 7;
			item.useAnimation = 25;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.axe = 17;
			item.hammer = 85;
			item.tileBoost++;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 3000;
			item.rare = 4;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("BoreicJacksawProj");
			item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 12);
			recipe.AddIngredient(mod.ItemType("FrigidShard"), 8);
			recipe.AddTile(TileID.IceMachine);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class BoreicDrillProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BoreicDrillProj"); }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.MythrilDrill);
		}

	}

	public class BoreicJacksawProj : ModProjectile
	{
		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BoreicJacksawProj"); }
		}
		public override void SetDefaults()
		{
			projectile.width = 22;
			projectile.height = 56;
			projectile.aiStyle = 20;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.hide = true;
			projectile.ownerHitCheck = true;
			projectile.melee = true;
		}
	}

}