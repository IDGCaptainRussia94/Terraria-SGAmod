using Microsoft.Xna.Framework;
using AAAAUThrowing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.ThrowingKnives
{
	public class ShadowflameKnifeThrowing : ModItem
	{
		public override string Texture => "Terraria/Item_" + ItemID.ShadowFlameKnife;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame Knife");
			Tooltip.SetDefault("Inflicts Showflame on hit\nThrowing damage & non-consumable");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.ShadowFlameKnife);
			item.melee = false;
			item.Throwing().thrown = true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[proj].Throwing().thrown = true;
			Main.projectile[proj].melee = false;
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowFlameKnife);
			recipe.AddTile(ModContent.TileType<Tiles.ReverseEngineeringStation>());
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
	public class VampireKnife : ModItem
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.VampireKnife;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Vampire Knife");
			Tooltip.SetDefault("Life stealing knives");
		}
		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.VampireKnives);
			item.melee = false;
			item.Throwing().thrown = true;
			item.consumable = true;
			item.maxStack = 999;
			item.value = Item.sellPrice(copper: 50);
			item.shoot = ProjectileID.VampireKnife;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[proj].Throwing().thrown = true;
			Main.projectile[proj].melee = false;
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ThrowingKnife, 100);
			recipe.AddIngredient(ModContent.ItemType<Entrophite>(), 1);
			recipe.AddRecipeGroup("SGAmod:EvilBossMaterials", 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 100);
			recipe.AddRecipe();
		}
	}
}