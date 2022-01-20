using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.Graphics;
using Terraria.Graphics.Shaders;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Tiles;

namespace SGAmod.Items.Weapons.Ammo
{
	public class CondensedGelPack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Condensed Gel Pack");
			Tooltip.SetDefault("Stored gel that acts as unlimited Gel for flamethrowers\nSlows rate of firing by 10% due to the need to uncondense\nThere is a rare chance to spawn gel when using this ammo");
		}
		public override void SetDefaults()
		{
			item.damage = 15;
			item.ranged = true;
			item.width = 8;
			item.height = 8;
			item.maxStack = 1;
			item.consumable = true;
			item.knockBack = 1.5f;
			item.value = 0;
			item.rare = ItemRarityID.Pink;
			item.shoot = ProjectileID.Flames;
			item.shootSpeed = 2.5f;
			item.ammo = AmmoID.Gel;
		}

		public override bool ConsumeAmmo(Player player)
		{
			player.AddBuff(ModContent.BuffType<GelPackDebuff>(), 30);
			if (Main.rand.Next(100) < 1)
			{
				player.QuickSpawnItem(ItemID.Gel);
				return true;
			}
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Gel, 200);
			recipe.AddIngredient(ModContent.ItemType<PlasmaCell>(), 3);
			recipe.AddTile(ModContent.TileType<ReverseEngineeringStation>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class GelPackDebuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_" + BuffID.Slimed;
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Gel Uncondensing");
			Description.SetDefault("10% slower item use rate");
			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().UseTimeMul -= 0.10f;
		}
	}

}
