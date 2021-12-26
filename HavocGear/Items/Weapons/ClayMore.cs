using Microsoft.Xna.Framework;
using SGAmod.Projectiles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ClayMore : ModItem
	{
		int clayCounter = 0;
		public override void SetDefaults()
		{
			base.SetDefaults();

			item.damage = 24;
			item.width = 19;
			item.height = 22;
			item.melee = true;
			item.rare = 2;
			item.useStyle = 1;
			item.useAnimation = 25;
			item.autoReuse = true;
			item.useTime = 26;
			item.useTurn = true;
			item.knockBack = 9;
			item.value = 1000;
			item.shoot = 10;
			item.shootSpeed = 10f;
			item.consumable = false;
			item.UseSound = SoundID.Item1;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clay-More");
			Tooltip.SetDefault("Launches Clay from the player's inventory at a distance\nEvery 10th clay launches a clay pot instead\nThe sword swings slower when launching clay");
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ClayBlock, 30);
			recipe.AddIngredient(mod.ItemType("BottledMud"), 2);
			recipe.AddIngredient(ItemID.Gel, 15);
			recipe.AddIngredient(ItemID.Vine, 2);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		private bool HasClay(Player player)
        {
			return ((player.Center - Main.MouseWorld).Length() > 300 && player.HasItem(ItemID.ClayBlock));
		}
        public override float MeleeSpeedMultiplier(Player player)
        {
			return player.SGAPly().claySlowDown>0 ? 0.75f : 1f;
		}
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			float rotation = MathHelper.Pi / 16f;
			Vector2 speed = new Vector2(speedX, speedY);
			Vector2 perturbedSpeed = (new Vector2(1f, 1f) * speed).RotatedBy((-rotation/2f)+(Main.rand.NextFloat()* rotation)); // Watch out for dividing by 0 if there is only 1 projectile.
			if (HasClay(player))
			{
				clayCounter += 1;
				clayCounter = clayCounter % 10;
				player.ConsumeItem(ItemID.ClayBlock);
				int typeproj = clayCounter == 9 ? ModContent.ProjectileType<ClayPot>() : ModContent.ProjectileType<ClayBall>();
				Projectile clayball = Projectile.NewProjectileDirect(position, perturbedSpeed, typeproj, damage/2, knockBack/5f, player.whoAmI);
				player.SGAPly().claySlowDown = (int)(player.itemTime * 1.25f);
				player.itemTime = (int)(player.itemTime * 1.25f);
			}

			return false;
        }

    }
}
namespace SGAmod.Projectiles
{

	public class ClayBall : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ball of Clay");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.MudBall);
			projectile.aiStyle = -1;
			projectile.timeLeft = 300;
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = 1;
			projectile.melee = true;
			aiType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.aiStyle = -1;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Dirt, projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Color.SandyBrown, 1.5f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (!target.friendly)
				projectile.Kill();
		}

		public override void AI()
		{
			projectile.rotation += ((projectile.velocity.Length()/16f) * Math.Sign(-0.1f + (projectile.velocity.X * 1.1f)))*0.1f;
			projectile.velocity.Y += 0.25f;
			for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Dirt, 0f, 0f, 50, Color.SandyBrown, 1.0f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}

		}

	}

	public class ClayPot : ClayBall
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Pot of Clay?!");
		}

        public override string Texture => "Terraria/Item_"+ItemID.ClayPot;

        public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 24;
			projectile.height = 24;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.aiStyle = -1;
			Main.PlaySound(SoundID.Shatter, (int)projectile.position.X, (int)projectile.position.Y, 0);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.Dirt, projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 15f), 50, Color.SandyBrown, 1.5f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}

			for (int num315 = 0; num315 < 4; num315 = num315 + 1)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 2000)); randomcircle.Normalize();
				float velincrease = Main.rand.NextFloat(2f,6f);
				int thisone = Projectile.NewProjectile(projectile.Center.X - projectile.velocity.X, projectile.Center.Y - projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ModContent.ProjectileType<ClayBall>(), (int)(projectile.damage * 0.5f), projectile.knockBack, projectile.owner, 0.0f, 0f);
			}

			return true;
		}

	}

}
