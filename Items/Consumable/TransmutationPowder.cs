using SGAmod.Tiles;
using SGAmod.Dusts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using static Terraria.ModLoader.ModContent;
using Idglibrary;
using Terraria.DataStructures;
using System.Linq;

namespace SGAmod.Items.Consumable
{

	class TransmutationPowder : ModItem
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Transmutation Powder");
			Tooltip.SetDefault("Converts Novus Ore to Novite Ore and vice-versa");
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.shootSpeed = 3f;
			item.shoot = mod.ProjectileType("TransmutationPowderSpray");
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 28;
			item.maxStack = 30;
			item.consumable = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 40;
			item.useTime = 40;
			item.noMelee = true;
			item.autoReuse = false;
			item.value = Item.buyPrice(0, 0, 0, 25);
			item.rare = ItemRarityID.Blue;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 1);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 1);
			recipe.AddIngredient(ItemID.VilePowder, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this,3);
			recipe.AddRecipe();

			recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 1);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 1);
			recipe.AddIngredient(ItemID.ViciousPowder, 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 3);
			recipe.AddRecipe();
		}

		public override bool CanUseItem(Player player)
		{
			return true;
		}

	}

	public class TransmutationPowderSpray : ModProjectile
	{
		public List<Point16> whatconverted;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Transmutation Powder");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Accessories/Canister4"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public TransmutationPowderSpray()
		{
			whatconverted = new List<Point16>();
			whatconverted.Add(new Point16(0, 0));
			whatconverted.Add(new Point16(0, 0));
			whatconverted.Add(new Point16(0, 0));
		}

		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.friendly = true;
			projectile.alpha = 255;
			projectile.penetrate = -1;
			projectile.extraUpdates = 2;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.timeLeft = 100;
		}

		public override void AI()
		{
			int dustType = DustType<TornadoDust>();
			if (projectile.owner == Main.myPlayer)
			{
				ConvertTiles((int)(projectile.position.X + (float)(projectile.width / 2)) / 16, (int)(projectile.position.Y + (float)(projectile.height / 2)) / 16, 2);

				int dist = 12 * 12;
				foreach (Item itemtoConvert in Main.item.Where(testby => !testby.newAndShiny && (testby.type == ModContent.ItemType<UnmanedOre>() || testby.type == ModContent.ItemType<NoviteOre>()) && (testby.Center-projectile.Center).LengthSquared()<dist))
				{

					itemtoConvert.type = itemtoConvert.type == ModContent.ItemType<UnmanedOre>() ? ModContent.ItemType<NoviteOre>() : ModContent.ItemType<UnmanedOre>();
					int stack = itemtoConvert.stack;
					itemtoConvert.SetDefaults(itemtoConvert.type);
					itemtoConvert.newAndShiny = true;
					itemtoConvert.stack = stack;

					for (int a = 0; a < 6; a++)
					{
						int dustIndex = Dust.NewDust(itemtoConvert.Hitbox.TopLeft(), itemtoConvert.Hitbox.Width, itemtoConvert.Hitbox.Height, dustType, 0f, 0f, 150, default(Color), 1f);
						Dust dust = Main.dust[dustIndex];
						dust.noGravity = true;
						dust.velocity.X *= 2f;
						dust.velocity.Y *= 2f;
						dust.scale *= 3f;
					}
				}

			}
			if (projectile.ai[0] > 7f)
			{
				float dustScale = 1f;
				if (projectile.ai[0] == 8f)
				{
					dustScale = 0.2f;
				}
				else if (projectile.ai[0] == 9f)
				{
					dustScale = 0.4f;
				}
				else if (projectile.ai[0] == 10f)
				{
					dustScale = 0.6f;
				}
				else if (projectile.ai[0] == 11f)
				{
					dustScale = 0.8f;
				}
				projectile.ai[0] += 1f;
				for (int i = 0; i < 1; i++)
				{
					int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, dustType, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, 100, default(Color), 1f);
					Dust dust = Main.dust[dustIndex];
					dust.noGravity = true;
					dust.scale *= 1.75f;
					dust.velocity.X *= 2f;
					dust.velocity.Y *= 2f;
					dust.scale *= dustScale;
				}
			}
			else
			{
				projectile.ai[0] += 1f;
			}
			projectile.rotation += 0.3f * (float)projectile.direction;
		}

		public void ConvertTiles(int i, int j, int size = 4)
		{
			for (int k = i - size; k <= i + size; k++)
			{
				for (int l = j - size; l <= j + size; l++)
				{
					if (WorldGen.InWorld(k, l, 1) && Math.Abs(k - i) + Math.Abs(l - j) < Math.Sqrt(size * size + size * size))
					{
						int type = (int)Main.tile[k, l].type;

						if (Main.tile[k, l].active())
						{
							if (type == mod.TileType("UnmanedOreTile") || type == mod.TileType("NoviteOreTile"))
							{
								if (whatconverted.Find(typ2e => new Point16(k,l).X == typ2e.X && new Point16(k, l).Y == typ2e.Y).X<1)
								{
									Main.tile[k, l].type = (type == mod.TileType("NoviteOreTile") ? (ushort)mod.TileType("UnmanedOreTile") : (ushort)mod.TileType("NoviteOreTile"));
									WorldGen.SquareTileFrame(k, l, true);
									NetMessage.SendTileSquare(-1, k, l, 1);

									int dustType = DustType<TornadoDust>();

									whatconverted.Add(new Point16(k, l));

									for (int a = 0; a < 6; a++)
									{
										int dustIndex = Dust.NewDust(new Vector2(k, l) * 16, 16, 16, dustType, 0f, 0f, 150, default(Color), 1f);
										Dust dust = Main.dust[dustIndex];
										dust.noGravity = true;
										dust.velocity.X *= 2f;
										dust.velocity.Y *= 2f;
										dust.scale *= 3f;
									}
								}

							}
						}
					}
				}
			}
		}
	}
}
