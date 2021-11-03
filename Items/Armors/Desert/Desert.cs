using AAAAUThrowing;
using Idglibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Armors.Desert
{

	[AutoloadEquip(EquipType.Head)]
	public class DesertHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nomadic Hood");
			Tooltip.SetDefault("5% increased throwing crit chance");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 5000;
			item.rare = 1;
			item.defense = 1;
		}
		public override void UpdateEquip(Player player)
		{
			player.Throwing().thrownCrit += 5;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HardenedSand, 20);
			recipe.AddIngredient(ItemID.Feather, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Body)]
	public class DesertShirt : DesertHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nomadic Shirt");
			Tooltip.SetDefault("5% increased throwing damage");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 4000;
			item.rare = 1;
			item.defense = 2;
		}
		public override void UpdateEquip(Player player)
		{
			player.Throwing().thrownDamage += 0.05f;
		}
		public override void DrawHands(ref bool drawHands, ref bool drawArms)
		{
			drawHands = true;
			drawArms = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HardenedSand, 10);
			recipe.AddIngredient(ItemID.Cobweb, 25);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	[AutoloadEquip(EquipType.Legs)]
	public class DesertPants : DesertHelmet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nomadic Leggings");
			Tooltip.SetDefault("20% increase to movement speed on sand");
		}
		public override void SetDefaults()
		{
			item.width = 18;
			item.height = 18;
			item.value = 5000;
			item.rare = 1;
		}
		public override void UpdateEquip(Player player)
		{
			if (player.velocity.Y == 0)
			{
				for (int i = 0; i < 6; i += 1)
				{
					Point therePointpre = new Point((int)player.MountedCenter.X, (int)player.position.Y);
					Point therePoint = new Point(therePointpre.X >> 4, (therePointpre.Y >> 4) + i);
					Tile tile = Framing.GetTileSafely(therePoint);
					if (WorldGen.InWorld(therePoint.X, therePoint.Y) && tile.active() && (TileID.Sets.Conversion.Sand[tile.type] || TileID.Sets.Conversion.HardenedSand[tile.type]))
					{
						//Main.NewText("tests");
						player.moveSpeed += 1.20f;
						player.accRunSpeed += 1.20f;
						break;
					}
				}
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.HardenedSand, 20);
			recipe.AddIngredient(ItemID.SandBlock, 10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class ManifestedSandTosser : ModItem,IManifestedItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sand Tosser");
			Tooltip.SetDefault("Throws sand from your inventory");
		}
        public override string Texture => "Terraria/Projectile_"+ProjectileID.SandBallFalling;

        public override void SetDefaults()
		{
			//item.CloneDefaults(ItemID.ManaFlower);
			item.width = 12;
			item.height = 24;
			item.rare = ItemRarityID.Blue;
			item.value = 0;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.Throwing().thrown = true;
			item.damage = 10;
			item.shootSpeed = 4f;
			item.shoot = ProjectileID.SandBallGun;
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 12;
			item.height = 24;
			item.knockBack = 1;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 12;
			item.useTime = 12;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.useAmmo = AmmoID.Sand;
		}
		public static void DrawManifestedItem(Item item,SpriteBatch spriteBatch, Vector2 position, Rectangle frame,float scale)
        {
			Texture2D inner = Main.itemTexture[item.type];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			for (float i = 0; i < 1f; i += 0.20f)
			{
				spriteBatch.Draw(inner, drawPos, null, Color.White * (1f - ((i + (Main.GlobalTime / 2f)) % 1f)) * 0.75f, i * MathHelper.TwoPi, textureOrigin, Main.inventoryScale * (0.5f + 3.00f * (((Main.GlobalTime / 2f) + i) % 1f)), SpriteEffects.None, 0f);
			}

		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			DrawManifestedItem(item,spriteBatch, position, frame, scale);

			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].Throwing().thrown = true;
			Main.projectile[probg].ranged = false;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			Main.projectile[probg].owner = player.whoAmI;
			SGAprojectile modeproj = Main.projectile[probg].GetGlobalProjectile<SGAprojectile>();
			modeproj.myplayer = player;
			Main.projectile[probg].netUpdate = true;
			IdgProjectile.Sync(probg);
			return false;
		}

	}

}