using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Shadeflare : ModItem
	{
		private int varityshot=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeflare");
			Tooltip.SetDefault("Unleashes torrents of shadowflame arrows");
		}

		public override void SetDefaults()
		{
			item.damage = 95;
			item.ranged = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 10000;
			item.rare = 11;
			item.UseSound = SoundID.Item17;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.useAmmo = AmmoID.Arrow;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Shadeflare_Glow");
			}
		}

        	public override void AddRecipes()
        	{
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("StarMetalBar"), 20);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddIngredient(ItemID.ShadowFlameBow, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
            		recipe.AddRecipe();
        	}

	/*public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useStyle = 5;
				item.useTime = 18;
				item.useAnimation = 20;
				item.damage = 85;
				item.shoot = ProjectileID.WoodenArrowFriendly;
			}
			else
			{
				item.useStyle = 5;
				item.useTime = 18;
				item.useAnimation = 20;
				item.damage = 85;
				item.shoot = ProjectileID.WoodenArrowFriendly;
			}
return base.CanUseItem(player);
}*/

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			varityshot+=1;
			varityshot%=3;

			float speed=0.7f;
			float numberProjectiles = 12;
			float rotation = MathHelper.ToRadians(36);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;

		if (varityshot==1){
			numberProjectiles = 6;
			rotation = MathHelper.ToRadians(12);
			speed=0.80f;
		}

		if (varityshot==2){
			numberProjectiles = 3;
			rotation = MathHelper.ToRadians(3);
			speed=0.95f;
		}

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, ProjectileID.ShadowFlameArrow, damage, knockBack, player.whoAmI);
			}

			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ProjectileID.ShadowFlameArrow;
			}

			speedX/=5f;
			speedY/=5f;
				//Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)(damage*1.5), knockBack, player.whoAmI);
				damage=(int)(damage*1.5);
			return true;
		}
	}
}
