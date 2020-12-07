using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;

namespace SGAmod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class Joyrider : ModItem
	{
		int frameCounter = 0;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Joyrider");
			Tooltip.SetDefault("'Minigun powered flight!'\nRapidly Fire bullets below you as you fly!\nRequires Ammo to fly, uses the last ammo type you fired, and always shoots basic bullets\nDamage and Flight Time are improved by Ammo Damage");
		}

		public override void SetDefaults()
		{
			sbyte wingslo = item.wingSlot;
			item.CloneDefaults(ItemID.WingsVortex);
			item.width = 26;
			item.height = 38;
			item.value = 200000;
			item.accessory = true;
			item.wingSlot = wingslo;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.Jetpack];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
			}
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.Jetpack];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();

			int ammotype = (int)sgaplayer.myammo;
			if (ammotype > 0)
			{
				Item ammo2 = new Item();
				ammo2.SetDefaults(ammotype);
				if (player.CountItem(ammotype) > 0)
				{
					player.wingTimeMax = 80 + ammo2.damage*5;

					Vector2 velo = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(10f, 12f));


					velo = velo.RotatedBy(MathHelper.ToRadians(player.velocity.X * 2f));

					if ((sgaplayer.timer % (player.wingTime > 0 ? 3 : 8) == 0 || sgaplayer.timer % (player.wingTime > 0 ? 6 : 11) == 0) && player.controlJump)
					{

						player.ConsumeItem(ammo2.type);
						sgaplayer.JoyrideShake = 6;
						int thisoned = Projectile.NewProjectile(player.Center.X + player.direction * -12, player.Center.Y, velo.X, velo.Y, ammo2.shoot, (int)((float)(ammo2.damage * 1.5f) * player.rangedDamage * player.bulletDamage), ammo2.knockBack, Main.myPlayer);
					}
				}

			}
			else
			{
				player.wingTimeMax = 0;
				player.wings = 0;
				player.wingTime = 0;
			}
			player.GetModPlayer<SGAPlayer>().CustomWings = 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Megashark, 1);
			recipe.AddIngredient(ItemID.ChainGun, 1);
			recipe.AddIngredient(ItemID.Jetpack, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising,
			ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
		{
			ascentWhenFalling = 0.15f;
			ascentWhenRising = 0.3f;
			maxCanAscendMultiplier = 1.45f;
			maxAscentMultiplier = 1.45f;
			constantAscend = 0.35f;
		}

		public override void HorizontalWingSpeeds(Player player, ref float speed, ref float acceleration)
		{
			speed = 10f;
			acceleration *= 1.5f;
		}
	}
}