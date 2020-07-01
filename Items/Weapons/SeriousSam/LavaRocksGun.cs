using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;

namespace SGAmod.Items.Weapons.SeriousSam
{
	public class LavaRocksGun : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Rocks Gun");
			Tooltip.SetDefault("Launches Molten rocks that split apart when hitting a target");
		}
		
		public override void SetDefaults()
		{
			item.useStyle = 5;
			item.autoReuse = true;
			item.useAnimation = 20;
			item.useTime = 20;
			item.width = 50;
			item.height = 20;
			item.shoot = 338;
			item.UseSound = SoundID.Item11;
			item.damage = 75;
			item.crit = 10;
			item.shootSpeed = 8f;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 20, 0, 0);
			item.knockBack = 7f;
			item.rare = 7;
			item.magic = true;
			item.mana = 8;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 15);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 2);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 3);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 5);
			recipe.AddIngredient(ItemID.JackOLanternLauncher, 1);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-4, -6);
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
			float speed=8f;
			float numberProjectiles = 3;
			float rotation = MathHelper.ToRadians(7);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0,100)/100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj=Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("LavaRocks"), damage, knockBack, player.whoAmI,Main.rand.Next(0,3));
				Main.projectile[proj].friendly=true;
				Main.projectile[proj].hostile=false;
				Main.projectile[proj].timeLeft=600;
				Main.projectile[proj].knockBack=item.knockBack;
				if (i > 0)
				{
					Main.projectile[proj].width = 15;
					Main.projectile[proj].height = 15;
				}
				Main.projectile[proj].netUpdate = true;
			}
			return false;
		}
				
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            if (Main.rand.Next(1) == 0)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 15);
            }
        }
	
	}

	public class LavaRocks : ModProjectile
	{

		bool hittile = false;
		public virtual bool hitwhilefalling => false;
		public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lava Rocks");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 18;
			projectile.height = 18;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.magic = true;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);
			ArmorShaderData shader = GameShaders.Armor.GetShaderFromItemId(ItemID.SolarDye);
			//HairShaderData shader = GameShaders.Hair.GetShaderFromItemId(ItemID.LeinforsAccessory);
			shader.Apply(null);
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			Texture2D texture = ModContent.GetTexture("Terraria/Projectile_"+(424 + projectile.ai[0]));
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor* trans, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.immune[projectile.owner] = 15;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.Fireball;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 40; num315 = num315 + 1)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle*=Main.rand.NextFloat(2f, 6f);
				int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0,0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), projectile.scale*2f);
				Main.dust[num316].noGravity = false;
				Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y);		
			}
			if (projectile.width > 16)
			{
				for (int num315 = 1; num315 < 4; num315 = num315 + 1)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					float velincrease = Main.rand.NextFloat(4f,8f);
					int thisone = Projectile.NewProjectile(projectile.Center.X - projectile.velocity.X, projectile.Center.Y - projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ModContent.ProjectileType<LavaRocks>(), (int)(projectile.damage * 0.75), projectile.knockBack, projectile.owner, 0.0f, 0f);
					Main.projectile[thisone].netUpdate = true;
					Main.projectile[thisone].friendly = projectile.friendly;
					Main.projectile[thisone].hostile = projectile.hostile;
					Main.projectile[thisone].width = Main.projectile[thisone].width - 6;
					Main.projectile[thisone].height = Main.projectile[thisone].height - 6;
					if (hittile)
					Main.projectile[thisone].velocity.Y = -Math.Abs(Main.projectile[thisone].velocity.Y);
					IdgProjectile.Sync(thisone);
				}
			}

			int theproj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((double)projectile.damage * 0.15f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].magic = projectile.magic;

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			hittile = true;
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.velocity.Y<0 && hitwhilefalling)
			return false;
			if (projectile.ai[1] < 5)
			return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{

			Tile tile = Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16];
			if (tile!=null)
			if (tile.liquid > 64)
			projectile.Kill();

			projectile.scale = ((float)projectile.width / 24f);
			for (int i = 0; i < projectile.scale + 0.5; i++)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"));
				Main.dust[dust].scale = 2.25f*projectile.scale;
				Main.dust[dust].noGravity = true;
				Main.dust[dust].color = Main.dust[dust].color * 0.25f;
				Main.dust[dust].velocity = -projectile.velocity * (float)(Main.rand.Next(20, 100 + (i * 40)) * 0.005f);
			}




			projectile.velocity.Y += 0.1f;
			projectile.rotation += projectile.velocity.X*0.5f;//(float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			projectile.ai[1] += 1;
		}


	}


}