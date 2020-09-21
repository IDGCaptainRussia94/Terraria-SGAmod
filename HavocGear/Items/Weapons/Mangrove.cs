using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Collections.Generic;
using SGAmod.HavocGear.Items.Tools;


namespace SGAmod.HavocGear.Items.Weapons
{
	public class MangroveBow : MangrovePickaxe
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Bow");
			Tooltip.SetDefault("Shoots 2 arrows offsetted at angles at the cost of 1");
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);
			tooltips.Add(new TooltipLine(mod, "Item Sets", "Having more of the mangrove weapon set in your inventory improves the damage"));
			tooltips.Add(new TooltipLine(mod, "Item Sets", "Up to 50% with all the weapons"));
		}

		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			List<int> mangroves = new List<int>();
			mangroves.Add(mod.ItemType("MangroveStriker"));
			mangroves.Add(mod.ItemType("MangroveShiv"));
			mangroves.Add(mod.ItemType("MangroveStaff"));
			mangroves.Add(mod.ItemType("MangroveBow"));
			mangroves.Add(mod.ItemType("MossYoyo"));

			float bonusdamage = 0;

			foreach(int itemtype in mangroves)
			{
				if (player.CountItem(itemtype) > 0)
				{
					if (itemtype != item.type)
					{
						bonusdamage += 0.125f;
					}
				}
			}
			add+= bonusdamage;
		}

		public override void SetDefaults()
		{
			item.damage = 23;
			item.ranged = true;
			item.width = 20;
			item.height = 32;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 25000;
			item.rare = 5;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 65f;
			item.useAmmo = AmmoID.Arrow;
		}

        	public override void AddRecipes()
        	{
            		ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DankWoodBow", 1);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
            		recipe.AddRecipe();
        	}
		
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float numberProjectiles = 2 + Main.rand.Next(1);
			float rotation = MathHelper.ToRadians(10);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			}
			return false;
		}
	}

	public class MangroveStriker : MangroveBow
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Striker");
			Tooltip.SetDefault("Attacks may inflict Dryad's Bane");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.melee = true;
			item.width = 36;
			item.height = 36;
			item.useTime = 19;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 7;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if (Main.rand.Next(0, 100) < 25)
			target.AddBuff(BuffID.DryadsWardDebuff, 60 * 4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "DankWoodSword", 1);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class MangroveStaff : MangroveBow
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Staff");
			Tooltip.SetDefault("Shoots 3 fast moving homing mangrove orbs");
		}

		public override void SetDefaults()
		{
			item.damage = 10;
			item.magic = true;
			item.mana = 6;
			item.width = 38;
			item.height = 40;
			item.useTime = 10;
			item.useAnimation = 30;
			Item.staff[item.type] = true;
			item.useStyle = 5;
			item.knockBack = 0;
			item.value = 25000;
			item.rare = 3;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("MangroveStaffOrb");
			item.shootSpeed = 12f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			return true;
		}

	}

	public class MangroveShiv : MangroveStaff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mangrove Shiv");
		}

		public override void SetDefaults()
		{
			item.damage = 20;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 3;
			item.useAnimation = 6;
			item.useStyle = 3;
			item.knockBack = 6;
			item.value = 25000;
			item.rare = 3;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "BiomassBar", 7);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

}
