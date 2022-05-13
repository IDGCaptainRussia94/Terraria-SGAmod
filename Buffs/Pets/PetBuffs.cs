using Terraria;
using Terraria.ModLoader;
using SGAmod.Projectiles.Pets;

namespace SGAmod.Buffs.Pets
{
	public class CutestIceFairyBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Cutest Ice Fairy");
			Description.SetDefault("Now isn't she the cutest?");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<CutestIceFairy>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<CutestIceFairy>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class AcidicSpiderlingBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Acidic Spiderling");
			Description.SetDefault("Warning: Bites may result in super powers");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<AcidicSpiderling>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<AcidicSpiderling>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class MiniCopperWraithBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Copper Wraith");
			Description.SetDefault("A Mini Copper Wraith is following you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MiniCopperWraith>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<MiniCopperWraith>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class MiniTinWraithBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Tin Wraith");
			Description.SetDefault("A Mini Tin Wraith is following you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MiniTinWraith>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<MiniTinWraith>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class MiniCobaltWraithBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Cobalt Wraith");
			Description.SetDefault("A Mini Cobalt Wraith is following you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MiniCobaltWraith>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<MiniCobaltWraith>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class MiniPalladiumWraithBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Palladium Wraith");
			Description.SetDefault("A Mini Palladium Wraith is following you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MiniPalladiumWraith>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<MiniPalladiumWraith>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class MiniLuminiteWraithBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Mini Luminite Wraith");
			Description.SetDefault("A Mini Luminite Wraith is following you");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<MiniLuminiteWraith>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<MiniLuminiteWraith>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class PrismicVisitantBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Prismic Visitant");
			Description.SetDefault("Not likey to scream");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<PrismicVisitant>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<PrismicVisitant>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class SlimeBaronBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slime Baron");
			Description.SetDefault("Somehow still apart of the royalty");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<SlimeBaron>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<SlimeBaron>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
	public class SlimeDuchessBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Slime Duchess");
			Description.SetDefault("Event singularities not included");
			Main.buffNoTimeDisplay[Type] = true;
			Main.vanityPet[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.buffTime[buffIndex] = 18000;
			bool petProjectileNotSpawned = player.ownedProjectileCounts[ModContent.ProjectileType<SlimeDuchess>()] <= 0;
			if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
			{
				Projectile.NewProjectile(player.position.X + (float)(player.width / 2), player.position.Y + (float)(player.height / 2), 0f, 0f, ModContent.ProjectileType<SlimeDuchess>(), 0, 0f, player.whoAmI, 0f, 0f);
			}
		}
	}
}