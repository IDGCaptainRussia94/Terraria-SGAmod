using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons.SeriousSam;

namespace SGAmod.Items.Tools
{
	public class NoviteDrill : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Drill");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MythrilDrill);
			item.shoot = ModContent.ProjectileType<NoviteDrillProj>();
			item.useAnimation = 40;
			item.value = Item.buyPrice(0, 0, 50, 0);
			item.useTime = 12;
			item.damage = 8;
			item.pick = 55;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 12);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class NoviteDrillProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/NoviteDrillProj"); }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.MythrilDrill);
		}

	}

	public class NoviteChainsaw : NoviteDrill
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Chainsaw");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.MythrilDrill);
			item.damage = 7;
			item.melee = true;
			item.width = 40;
			item.height = 40;
			item.useTime = 15;
			item.useAnimation = 15;
			item.axe = 60;
			item.pick = 0;
			item.useStyle = 1;
			item.knockBack = 6;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<NoviteChainsawProj>();
		}

	}

	public class NoviteChainsawProj : NoviteDrillProj
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Chainsaw");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/NoviteChainsawProj"); }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.MythrilDrill);
		}

	}

}