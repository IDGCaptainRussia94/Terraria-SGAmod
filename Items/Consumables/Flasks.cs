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
using System.Collections.Generic;
//using SubworldLibrary;

namespace SGAmod.Items.Consumables
{
	public class FlaskOfBlaze : ModItem
	{
		public virtual int FlaskBuff => ModContent.BuffType<FlaskOfBlazeBuff>();
		public virtual int Period => Main.rand.Next(120, 180);
		public virtual int Debuff => ModContent.BuffType<ThermalBlaze>();
		public virtual int Chance => 0;
		public virtual void OnRealHit(Player player, Projectile proj, NPC npc,int damage)
        {

        }
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
			Tooltip.SetDefault("Melee attacks have a chance to melt enemies\nMelee Damage is reduced by 10% due to acid dulling the weapon");
		}

		public override void FlaskEffect(Rectangle rect, Vector2 speed)
		{

			Vector2 start = new Vector2(rect.X, rect.Y);

			if (Main.rand.Next(0, 100) > 50)
				return;

			int dust = Dust.NewDust(start, rect.Width, rect.Height, BuffID.HeartLamp, 0, 0, 100, default(Color), 0.5f);
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
			Description.SetDefault("Melee attacks have a chance to melt enemies\nMelee Damage is reduced by 10% due to acid dulling the weapon");
		}

        public override void Update(Player player, ref int buffIndex)
        {
			base.Update(player, ref buffIndex);
			player.meleeDamage -= 0.10f;
        }
    }
	public class FlaskOfLifeLeech : FlaskOfBlaze
	{
		public override int FlaskBuff => ModContent.BuffType<FlaskOfLifeLeechBuff>();
		public override int Period => 2;
		public override int Debuff => BuffID.AmmoBox;
		public override int Chance => 2;

		public override void OnRealHit(Player player, Projectile proj, NPC npc, int damage)
		{


			if (player.HasBuff(ModContent.BuffType<LifeLeechDebuff>()))
				return;

			if (proj == null || Main.rand.Next(100) < ((proj.modProjectile != null && proj.modProjectile is ITrueMeleeProjectile) ? 100 : 5));
			{
				Projectile projectile = new Projectile();
				projectile.Center = npc.Center;
				projectile.owner = player.whoAmI;
				projectile.vampireHeal((int)(damage), npc.Center);
				player.AddBuff(ModContent.BuffType<LifeLeechDebuff>(), 300+((int)(damage / 2f)));
			}
		}

		public static string LifeStealLine => "Melee attacks have a chance to life steal on hit\nChance is much lower with non-true melee hits\nSuccessfully life stealing will incure a delay before you can life steal again";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask of Life Leech");
			Tooltip.SetDefault(LifeStealLine);
		}

		public override void FlaskEffect(Rectangle rect, Vector2 speed)
		{

			Vector2 start = new Vector2(rect.X, rect.Y);

			if (Main.rand.Next(0, 100) > 90)
				return;

			int dust = Dust.NewDust(start, rect.Width, rect.Height, 242, 0, 0, 100, default(Color), 0.5f);
			Main.dust[dust].fadeIn = 0.2f;
			Main.dust[dust].velocity = speed * Main.rand.NextFloat(0.7f, 1.20f);

			if (Main.rand.Next(100) < 20)
            {
				Vector2 value = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11))+speed;
				value.Normalize();
				int num45 = Gore.NewGore(new Vector2(rect.X, rect.Y) + new Vector2(Main.rand.Next(rect.Width),Main.rand.Next(rect.Height)), value * Main.rand.Next(3, 6) * 0.33f, 331, (float)Main.rand.Next(20, 60) * 0.01f);
				Main.gore[num45].sticky = false;
				Main.gore[num45].velocity *= 0.4f;
				Main.gore[num45].velocity.Y -= 0.6f;
			}

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.Mushroom, 1);
			recipe.AddIngredient(ModContent.ItemType<HopeHeart>(), 1);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class FlaskOfLifeLeechBuff : FlaskOfBlazeBuff
	{
		public override FlaskOfBlaze FlaskType => ModContent.GetModItem(ModContent.ItemType<FlaskOfLifeLeech>()) as FlaskOfBlaze;
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/FlaskOfLifeLeechBuff";
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Weapon Imbue: Life Leech");
			Description.SetDefault(FlaskOfLifeLeech.LifeStealLine);
		}

        public override void Update(Player player, ref int buffIndex)
        {
			base.Update(player, ref buffIndex);
		}
    }

	public class FlaskOfSoulSap : FlaskOfBlaze
	{
		public override int FlaskBuff => ModContent.BuffType<FlaskOfSoulSapBuff>();
		public override int Period => 2;
		public override int Debuff => ModContent.BuffType<SoulSapDebuff>();
		public override int Chance => 2;

		public override void OnRealHit(Player player, Projectile proj, NPC npc, int damage)
		{

			if (npc.HasBuff(ModContent.BuffType<SoulSapDebuff>()))
            {
				int index = npc.FindBuffIndex(ModContent.BuffType<SoulSapDebuff>());
				if (npc.buffTime[index]>10)
				return;
			}

			if (proj == null || Main.rand.Next(100) < ((proj.modProjectile != null && proj.modProjectile is ITrueMeleeProjectile) ? 100 : 20));
			{
				if (npc.SGANPCs().flaskCooldown < 1)
				{
					Item.NewItem(npc.Center, ItemID.Star);
					//npc.buffType[npc.buffType.Length - 1] = ModContent.BuffType<SoulSapDebuff>();
					//npc.buffTime[npc.buffType.Length - 1] = 8 * 60;
					IdgNPC.AddBuffBypass(npc.whoAmI, ModContent.BuffType<SoulSapDebuff>(), 60 * 8);
					npc.SGANPCs().flaskCooldown = 8*60;

					for (int i = 0; i < 25; i += 1)
					{
						Vector2 value = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
						value.Normalize();
						int num45 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), value * Main.rand.NextFloat(3, 6), 17, (float)Main.rand.Next(20, 60) * 0.01f);
						Main.gore[num45].sticky = false;
					}
				}
			}
		}

		public static string SoulSapLine => "Melee attacks may spawn mana stars on hit\nTrue Melee have a much higher chance\nWhen spawned, enemies cannot drop more stars for 8 seconds";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask of Soul Sap");
			Tooltip.SetDefault(SoulSapLine);
		}

		public override void FlaskEffect(Rectangle rect, Vector2 speed)
		{

			Vector2 start = new Vector2(rect.X, rect.Y);

			if (Main.rand.Next(0, 100) > 50)
				return;

			int dust = Dust.NewDust(start, rect.Width, rect.Height, DustID.AncientLight, 0, 0, 100, Color.Blue, 0.5f);
			Main.dust[dust].fadeIn = 0.2f;
			Main.dust[dust].velocity = speed * Main.rand.NextFloat(0.7f, 1.20f);

			if (Main.rand.Next(100) < 8)
			{
				Vector2 value = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11)) + speed;
				value.Normalize();
				int num45 = Gore.NewGore(new Vector2(rect.X, rect.Y) + new Vector2(Main.rand.Next(rect.Width), Main.rand.Next(rect.Height)), value * Main.rand.Next(3, 6) * 0.33f, 17, (float)Main.rand.Next(20, 60) * 0.01f);
				Main.gore[num45].sticky = false;
				Main.gore[num45].velocity *= 0.4f;
				Main.gore[num45].velocity.Y -= 0.6f;
			}

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Bottle, 1);
			recipe.AddIngredient(ItemID.FallenStar, 1);
			recipe.AddIngredient(ModContent.ItemType<HopeHeart>(), 1);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class FlaskOfSoulSapBuff : FlaskOfBlazeBuff
	{
		public override FlaskOfBlaze FlaskType => ModContent.GetModItem(ModContent.ItemType<FlaskOfSoulSap>()) as FlaskOfBlaze;
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/FlaskOfSoulSapBuff";
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Weapon Imbue: Soul Sap");
			Description.SetDefault(FlaskOfSoulSap.SoulSapLine);
		}

		public override void Update(Player player, ref int buffIndex)
		{
			base.Update(player, ref buffIndex);
		}
	}



	public class LifeLeechDebuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_"+BuffID.MoonLeech;
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Life Leeched");
			Description.SetDefault("Your flask cannot leach for a while");
			Main.debuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{

		}
	}

	public class SoulSapDebuff : ModBuff
	{
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "Terraria/Buff_" + BuffID.MoonLeech;
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Soul Sapped");
			Description.SetDefault("Cannot drop stars");
			Main.debuff[Type] = true;
		}

        public override void Update(NPC npc, ref int buffIndex)
        {
            //
        }
    }

	public class FlaskOfhallucinogenics : FlaskOfBlaze
	{
		public override int FlaskBuff => ModContent.BuffType<FlaskOfhallucinogenicsBuff>();
		public override int Period => 1;
		public override int Debuff => BuffID.ScutlixMount;
		public override int Chance => 4;

		public override void OnRealHit(Player player, Projectile proj, NPC npc, int damage)
		{

			if (proj == null || Main.rand.Next(100) < ((proj.modProjectile != null && proj.modProjectile is ITrueMeleeProjectile) ? 100 : 20))
			{
				IdgNPC.AddBuffBypass(npc.whoAmI, ModContent.BuffType<IllusionDebuff>(), 60 * 15);

				if (npc.HasBuff(ModContent.BuffType<IllusionDebuff>()))
				{
					for (int i = 0; i < 25; i += 1)
					{
						Vector2 value = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
						value.Normalize();
						int num45 = Gore.NewGore(npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)), value * Main.rand.NextFloat(3, 6), Main.rand.Next(426, 428), (float)Main.rand.Next(20, 60) * 0.01f);
						Main.gore[num45].sticky = false;
					}
				}
			}
		}

		public static string hallucinogenicsLine => "Melee attacks may wrack their minds\nTrue Melee have a much higher chance of Wracking Minds";

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flask of hallucinogenics");
			Tooltip.SetDefault(hallucinogenicsLine);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine line = new TooltipLine(mod, "IllusionDebuff", Buffs.IllusionDebuff.IllusionDebuffText);
			line.overrideColor = Color.MediumPurple;
			tooltips.Add(line);
		}
		public override Color? GetAlpha(Color lightColor)
		{
			return Color.MediumPurple * ((float)(0.5f + Math.Sin(Main.GlobalTime / 2f) * 0.35f));
		}

		public override void FlaskEffect(Rectangle rect, Vector2 speed)
		{

			Vector2 start = new Vector2(rect.X, rect.Y);

			for (int i = 0; i < 3; i += 1)
			{
				if (Main.rand.Next(0, 100) > 90)
					return;

				int dust = Dust.NewDust(start, rect.Width, rect.Height, 173, 0, 0, 100, Color.MediumPurple, 0.5f);
				Main.dust[dust].fadeIn = 0.25f;
				Main.dust[dust].alpha = 180;
				Main.dust[dust].velocity = speed * Main.rand.NextFloat(0.7f, 1.20f);
			}

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.BottledMud>(), 1);
			recipe.AddIngredient(ItemID.GlowingMushroom, 3);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.SwampSeeds>(), 1);
			recipe.AddTile(TileID.ImbuingStation);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class FlaskOfhallucinogenicsBuff : FlaskOfBlazeBuff
	{
		public override FlaskOfBlaze FlaskType => ModContent.GetModItem(ModContent.ItemType<FlaskOfhallucinogenics>()) as FlaskOfhallucinogenics;
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/FlaskOfSoulSapBuff";
			return true;
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			DisplayName.SetDefault("Weapon Imbue: Hallucinogenics");
			Description.SetDefault(FlaskOfhallucinogenics.hallucinogenicsLine);
		}

        public override void ModifyBuffTip(ref string tip, ref int rare)
        {
			tip += "\n"+Buffs.IllusionDebuff.IllusionDebuffText;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			base.Update(player, ref buffIndex);
		}
	}

}