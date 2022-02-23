using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria.UI;
using System;

namespace SGAmod.Items.Accessories
{
	[AutoloadEquip(EquipType.Wings)]
	public class Joyrider : ModItem
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Joyrider");
			Tooltip.SetDefault("'Minigun powered flight!'\nRapidly Fire bullets below you as you fly!\nRequires Ammo to fly\nDamage and Flight Time are improved by Ammo Damage");
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
			item.useAmmo = AmmoID.Bullet;
			item.shoot = ProjectileID.Bullet;
			item.shootSpeed = 10f;
			item.damage = 20;
			item.ranged = true;
		}

		/*public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
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
		}*/

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			SGAPlayer sgaplayer = player.GetModPlayer<SGAPlayer>();

			int joyriderid = ModContent.ItemType<Joyrider>();

			Item[] possibleJoy = player.armor.Where(testby => testby.type == ModContent.ItemType<Joyrider>())?.ToArray();

			Item joyRider = possibleJoy != null && possibleJoy.Length > 0 ? possibleJoy[0] : null;

			//Check WingSlot if it's enabled

			Mod WingSlot = ModLoader.GetMod("WingSlot");

			if (joyRider == null && WingSlot != null)
			{
				Type wingPlayer = WingSlot.Code.GetType("WingSlot.WingSlotPlayer");
				Type itemSlot = WingSlot.Code.GetType("TerraUI.Objects.UIItemSlot");
				ModPlayer wingply = player.GetModPlayer(WingSlot, "WingSlotPlayer");

				object itemslotinstance = (wingPlayer.GetField("EquipSlot",SGAmod.UniversalBindingFlags).GetValue(wingply));
				Item inWithin = (Item)((itemSlot.GetProperty("Item", SGAmod.UniversalBindingFlags).GetValue(itemslotinstance)));
				if (inWithin != null && !inWithin.IsAir && inWithin.type == joyriderid)
                {
					joyRider = inWithin;
				}
			}

			if (joyRider == null)
				return;


			bool hasTheAmmo = player.HasAmmo(joyRider, true);

			if (hasTheAmmo)
			{
				if (player.controlJump)
				{
					int projType = item.shoot;

					bool canShoot = true;
					int damage = joyRider.damage;
					float knockback = 1;
					float speed = 32f;
					if (projType == ProjectileID.Bullet || projType == 10)
					{
						player.PickAmmo(item, ref projType, ref speed, ref canShoot, ref damage, ref knockback, true);
					}




					player.wingTimeMax = 25 + damage * 5;

					Vector2 velo = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(10f, 12f));


					velo = velo.RotatedBy(MathHelper.ToRadians(player.velocity.X * 2f));

					if (player.velocity.Y != 0 && (sgaplayer.timer % (player.wingTime > 0 ? 3 : 8) == 0 || sgaplayer.timer % (player.wingTime > 0 ? 6 : 11) == 0))
					{

						//player.ConsumeItemRespectInfiniteAmmoTypes(ammo2.type);
						player.PickAmmo(item, ref projType, ref speed, ref canShoot, ref damage, ref knockback, false);
						sgaplayer.JoyrideShake = 6;
						int thisoned = Projectile.NewProjectile(player.Center.X + player.direction * -12, player.Center.Y, velo.X, velo.Y, projType, (int)((float)(damage * 1) * (player.rangedDamage + (player.bulletDamage-1f))), knockback, Main.myPlayer);
					}
                }
                else
                {

				}
			}
			else
			{
				player.wingTimeMax = 0;
				player.wings = 0;
				player.wingTime = 0;
			}

			if (player.velocity.Y == 0)
				player.wingTime = player.wingTimeMax;
		}

        public override void UpdateVanity(Player player, EquipType type)
        {
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