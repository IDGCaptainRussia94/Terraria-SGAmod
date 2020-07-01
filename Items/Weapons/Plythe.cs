using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Items.Weapons
{
	class Plythe : ModItem
	{
		bool altfired=false;

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Plythe");
			Tooltip.SetDefault("Rapidly throw out seeking Scythes that fly back to the player on hit, can hit up to 3 times\nThey reap that which they sow");
		}
		public override string Texture
		{
			get { return ("SGAmod/Projectiles/Scythe"); }
		}
		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.Throwing().thrown=true;
			item.damage = 125;
			item.crit = 20;
			item.shootSpeed = 45f;
			item.shoot = mod.ProjectileType("Scythe");
			item.useTurn = true;
			//ProjectileID.CultistBossLightningOrbArc
			item.width = 8;
			item.height = 28;
			item.maxStack = 1;
			item.knockBack = 14;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 8;
			item.useTime = 8;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.value = Item.buyPrice(1, 0, 0, 0);
			item.rare = 9;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//speedX *= player.thrownVelocity;
			//speedY *= player.thrownVelocity;
			return true;
		}

		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AvaliScythe"), 1);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 20);
            recipe.AddIngredient(mod.ItemType("CryostalBar"), 15);            
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
            recipe.AddRecipe();
		}
	}
}
