using Microsoft.Xna.Framework;
using AAAAUThrowing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.ThrowingKnives
{
	public class EnchantedCarver : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Inflicts Betsy's Curse for 5 seconds\nKnives bounce off of enemies and tiles\nNon-consumable");
		}
		public override void SetDefaults()
		{
			item.shootSpeed = 12f;
			item.damage = 70;
			item.crit = 6;
			item.knockBack = 4f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 10;
			item.useTime = 10;
			item.width = 32;
			item.height = 48;
			item.maxStack = 1;
			item.rare = ItemRarityID.Yellow;

			item.consumable = false;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
			item.Throwing().thrown = true;

			item.UseSound = SoundID.Item109;
			item.value = Item.sellPrice(gold: 4);
			item.shoot = ModContent.ProjectileType<EnchantedCarverProjectile>();
		}
	}
	public class EnchantedCarverProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmodDev/Projectiles/EnchantedCarverProjectile";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("EnchantedCarver");
		}
		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 24;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.Throwing().thrown = true;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.penetrate = 4;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.extraUpdates = 1;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.OrangeTorch);
			projectile.ai[0] += 20f;
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(SoundID.Dig, projectile.position);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
		public override void AI()
		{
			//projectile.rotation += 0.2f * (float)projectile.direction; //If you want it to rotate instead
			projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
			projectile.spriteDirection = projectile.direction;
			projectile.ai[0] += 1f; // Use a timer to wait 40 ticks before applying gravity.
			if (projectile.ai[0] >= 40f)
			{
				projectile.ai[0] = 40f;
				projectile.velocity.Y += 0.2f;
				projectile.velocity.X *= 0.98f;
			}
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
		}
		public override void PostAI()
		{
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.OrangeTorch);
			}
			Lighting.AddLight(projectile.Center, 0.95f, 0.56f, 0.1f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.BetsysCurse, 300);
			if (projectile.velocity.X != 0)
			{
				projectile.velocity.X = -projectile.velocity.X;
			}
			if (projectile.velocity.Y != 0)
			{
				projectile.velocity.Y = -projectile.velocity.Y;
			}
		}
	}
}