using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using SGAmod.Generation;
using SGAmod;
using SGAmod.Buffs;
using Idglibrary;
using SGAmod.Dusts;
//using SubworldLibrary;

namespace SGAmod.Items.Consumable
{
	public class FlaskOfBlaze : ModItem
	{
		public virtual int FlaskBuff => ModContent.BuffType<FlaskOfBlazeBuff>();
		public virtual int Period => Main.rand.Next(120, 180);
		public virtual int Debuff => ModContent.BuffType<ThermalBlaze>();
		public virtual int Chance => 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask of Blaze");
			Tooltip.SetDefault("Melee attacks scorch enemies Ablaze");
		}

		public virtual void FlaskEffect(Rectangle rect, Vector2 speed)
		{

			Vector2 start = new Vector2(rect.X, rect.Y);

			if (Main.rand.Next(0, 100) > 20)
				return;

			int dust = Dust.NewDust(start, rect.Width, rect.Height, ModContent.DustType<HotDust>(), 0, 0, 100, default(Color), 1f);
			Main.dust[dust].noGravity = true;
			Main.dust[dust].fadeIn = 0.6f;
			Main.dust[dust].velocity = speed * Main.rand.NextFloat(0.25f, 0.80f);

		}

		public override void SetDefaults()
		{
			item.width = 14;
			item.height = 24;
			item.maxStack = 30;
			item.rare = 2;
			item.value = 2000;
			item.useStyle = 2;
			item.useAnimation = 17;
			item.useTime = 17;
			item.useTurn = true;
			item.UseSound = SoundID.Item3;
			item.consumable = true;
			item.buffType = FlaskBuff;
			item.buffTime = Item.flaskTime;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlaskofFire,1);
			recipe.AddIngredient(ItemID.Ale,1);
			recipe.AddIngredient(null, "FieryShard", 2);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class FlaskOfBlazeBuff : ModBuff
	{
		public virtual FlaskOfBlaze FlaskType => ModContent.GetModItem(ModContent.ItemType<FlaskOfBlaze>()) as FlaskOfBlaze;
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Weapon Imbue: Thermal Blaze");
			Description.SetDefault("Melee attacks scorch enemies Ablaze");
			Main.meleeBuff[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/FlaskofBlazeBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.SGAPly().flaskBuff = FlaskType;
		}
	}

	public class FlaskOfAcid : FlaskOfBlaze
	{
		public override int FlaskBuff => ModContent.BuffType<FlaskOfAcidBuff>();
		public override int Period => Main.rand.Next(5, 20);
		public override int Debuff => ModContent.BuffType<AcidBurn>();
		public override int Chance => 3;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask of Acid");
			Tooltip.SetDefault("Melee attacks have a chance to melt enemies");
		}

		public override void FlaskEffect(Rectangle rect, Vector2 speed)
		{

			Vector2 start = new Vector2(rect.X, rect.Y);

			if (Main.rand.Next(0, 100) > 50)
				return;

			int dust = Dust.NewDust(start, rect.Width, rect.Height, ModContent.DustType<AcidDust>(), 0, 0, 100, default(Color), 0.5f);
			Main.dust[dust].fadeIn = 0.2f;
			Main.dust[dust].velocity = speed * Main.rand.NextFloat(0.7f, 1.20f);

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(null, "VialofAcid", 3);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}
	public class FlaskOfAcidBuff : FlaskOfBlazeBuff
	{
		public override FlaskOfBlaze FlaskType => ModContent.GetModItem(ModContent.ItemType<FlaskOfAcid>()) as FlaskOfBlaze;
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/FlaskofAcidBuff";
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Weapon Imbue: Acid Burn");
			Description.SetDefault("Melee attacks have a chance to melt enemies");
		}
	}


}