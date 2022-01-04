using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles.Pets
{
	public class MiniCopperWraith : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Mini Copper Wraith");
			Main.projFrames[projectile.type] = 5;
			Main.projPet[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Parrot);//Vanilla Projectile_208
			projectile.width = 18;
			projectile.height = 36;
			aiType = ProjectileID.Parrot;
			drawOffsetX = -12;
			drawOriginOffsetY -= 2;
		}

		public override bool PreAI()
		{
			Player player = Main.player[projectile.owner];
			player.parrot = false; // Relic from aiType
			return true;
		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];
			if (player.HasBuff(ModContent.BuffType<Buffs.Pets.MiniCopperWraithBuff>()))
			{
				projectile.timeLeft = 2;
			}
		}
	}
}