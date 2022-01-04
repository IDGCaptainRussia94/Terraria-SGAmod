using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Projectiles.Pets;
using SGAmod.Buffs.Pets;

namespace SGAmod.Items.Pets
{
	public class FrozenBow : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frozen Bow");
			Tooltip.SetDefault("Summons a pet ice fairy");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 28;
			item.height = 26;
			item.shoot = ModContent.ProjectileType<CutestIceFairy>();
			item.buffType = ModContent.BuffType<CutestIceFairyBuff>();
			item.rare = ItemRarityID.LightRed;
			item.expert = true; //Change to Master Mode in 1.4
			item.value = 250000;
			item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class SpiderlingEggs : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Spiderling Eggs");
			Tooltip.SetDefault("Summons a pet acidic spiderling");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 30;
			item.height = 26;
			item.shoot = ModContent.ProjectileType<AcidicSpiderling>();
			item.buffType = ModContent.BuffType<AcidicSpiderlingBuff>();
			item.rare = ItemRarityID.Green;
			item.expert = true;
			item.value = 250000;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class CopperTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Copper Tack");
			Tooltip.SetDefault("Summons a pet Copper Wraith");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 20;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<MiniCopperWraith>();
			item.buffType = ModContent.BuffType<MiniCopperWraithBuff>();
			item.rare = ItemRarityID.Blue;
			item.expert = true;
			item.value = 250000;
			item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class TinTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Tin Tack");
			Tooltip.SetDefault("Summons a pet Tin Wraith");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 20;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<MiniTinWraith>();
			item.buffType = ModContent.BuffType<MiniTinWraithBuff>();
			item.rare = ItemRarityID.Blue;
			item.expert = true;
			item.value = 250000;
			item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class CobaltTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cobalt Tack");
			Tooltip.SetDefault("Summons a pet Cobalt Wraith");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 20;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<MiniCobaltWraith>();
			item.buffType = ModContent.BuffType<MiniCobaltWraithBuff>();
			item.rare = ItemRarityID.LightRed;
			item.expert = true;
			item.value = 250000;
			item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class PalladiumTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Palladium Tack");
			Tooltip.SetDefault("Summons a pet Palladium Wraith");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 20;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<MiniPalladiumWraith>();
			item.buffType = ModContent.BuffType<MiniPalladiumWraithBuff>();
			item.rare = ItemRarityID.LightRed;
			item.expert = true;
			item.value = 250000;
			item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class LuminiteTack : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Luminite Tack");
			Tooltip.SetDefault("Summons a pet Luminite Wraith");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 20;
			item.height = 22;
			item.shoot = ModContent.ProjectileType<MiniLuminiteWraith>();
			item.buffType = ModContent.BuffType<MiniLuminiteWraithBuff>();
			item.rare = ItemRarityID.Purple;
			item.expert = true;
			item.value = 250000;
			item.UseSound = SoundID.Item46;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class VisitantStar : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Visitant Star");
			Tooltip.SetDefault("Summons a pet Prismic Visitant");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 22;
			item.height = 30;
			item.shoot = ModContent.ProjectileType<PrismicVisitant>();
			item.buffType = ModContent.BuffType<PrismicVisitantBuff>();
			item.rare = ItemRarityID.Purple;
			item.expert = true;
			item.value = 250000;
			item.UseSound = SoundID.Item25;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class ImperatorialOdious : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Imperatorial Odious");
			Tooltip.SetDefault("Summons a pet Slime Baron");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 18;
			item.height = 28;
			item.shoot = ModContent.ProjectileType<SlimeBaron>();
			item.buffType = ModContent.BuffType<SlimeBaronBuff>();
			item.rare = ItemRarityID.Orange;
			item.expert = true;
			item.value = 250000;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
	public class MonarchicalCate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Monarchical Cate");
			Tooltip.SetDefault("Summons a pet Slime Duchess");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DD2PetGato);
			item.width = 26;
			item.height = 28;
			item.shoot = ModContent.ProjectileType<SlimeDuchess>();
			item.buffType = ModContent.BuffType<SlimeDuchessBuff>();
			item.rare = ItemRarityID.Purple;
			item.expert = true;
			item.value = 250000;
		}

		public override void UseStyle(Player player)
		{
			if (player.whoAmI == Main.myPlayer && player.itemTime == 0)
			{
				player.AddBuff(item.buffType, 3600, true);
			}
		}
	}
}