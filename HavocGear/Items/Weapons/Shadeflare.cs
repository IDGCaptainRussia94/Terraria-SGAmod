using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Items;
using SGAmod.Items.Weapons.Technical;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Shadeflare : ModItem
	{
		private int varityshot=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeflare");
			Tooltip.SetDefault("Opens a rift that unleashes torrents of piercing Shadowflame arrows\n75% chance to not consume ammo when firing an ammo type arrow");
		}

		public override void SetDefaults()
		{
			item.damage = 75;
			item.ranged = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 15;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 50000;
			item.rare = ItemRarityID.Purple;
			item.UseSound = SoundID.Item17;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<ShadeflareCharging>();
			item.channel = true;
			item.shootSpeed = 50f;
			item.useAmmo = AmmoID.Arrow;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Shadeflare_Glow");
			}
		}

        	public override void AddRecipes()
        	{
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.ShadowFlameBow, 1);
			recipe.AddIngredient(ItemID.DarkShard, 1);
			recipe.AddIngredient(ItemID.FragmentVortex, 8);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 16);
			recipe.AddIngredient(ItemID.SoulofNight, 8);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
            recipe.AddRecipe();
		}

	/*public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.useStyle = 5;
				item.useTime = 18;
				item.useAnimation = 20;
				item.damage = 85;
				item.shoot = ProjectileID.WoodenArrowFriendly;
			}
			else
			{
				item.useStyle = 5;
				item.useTime = 18;
				item.useAnimation = 20;
				item.damage = 85;
				item.shoot = ProjectileID.WoodenArrowFriendly;
			}
return base.CanUseItem(player);
}*/

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			type = ModContent.ProjectileType<ShadeflareCharging>();
			return true;

			varityshot +=1;
			varityshot%=3;

			float speed=0.7f;
			float numberProjectiles = 12;
			float rotation = MathHelper.ToRadians(36);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 10f;

		if (varityshot==1){
			numberProjectiles = 6;
			rotation = MathHelper.ToRadians(12);
			speed=0.80f;
		}

		if (varityshot==2){
			numberProjectiles = 3;
			rotation = MathHelper.ToRadians(3);
			speed=0.95f;
		}
			speed *= player.ArrowSpeed();

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1))) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile shadow = Projectile.NewProjectileDirect(position, perturbedSpeed, ProjectileID.ShadowFlameArrow, damage, knockBack, player.whoAmI);
				shadow.usesLocalNPCImmunity = true;
				shadow.localNPCHitCooldown = -1;
				shadow.netUpdate = true;
			}

			if (type == ProjectileID.WoodenArrowFriendly)
			{
				type = ProjectileID.ShadowFlameArrow;
			}

			speedX/=5f;
			speedY/=5f;
				//Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)(damage*1.5), knockBack, player.whoAmI);
				damage=(int)(damage*1.5);

			
			//type = ModContent.ProjectileType<ShadeflareCharging>();
			return true;
		}
	}

	public class ShadeflareCharging : NovaBlasterCharging
    {
		int varityshot=0;
		public override int chargeuptime => 400;
		public override float velocity => 72f;
		public override float spacing => 16f;
		public override int fireRate => 30;
		public override (float, float) AimSpeed
		{


			get
			{
				float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);
				float rate = 0.15f - (0.07f * perc);



				return (rate, 0f);
			}
		}
		//public override int FireCount => 600;
		int chargeUpTimer=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeflare Charging");
		}

		public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowFlameArrow;

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.ranged = true;
			aiType = 0;
		}

		public override void ChargeUpEffects()
		{
			chargeUpTimer += 1;

			if (!player.HasAmmo(player.HeldItem, true))
				return;

			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			if (chargeUpTimer % (int)(12- (perc * 10f)) == 0)
			{

				varityshot += 1;
				//varityshot %= 5;

				Vector2 shootdirection = Vector2.Normalize(projectile.velocity);

				float speed = 1f;
				float numberProjectiles = 12;
				float rotation = MathHelper.ToRadians(36);

				if (varityshot%5 == 1)
				{
					speed = 0.45f;
				}

				if (varityshot % 5 == 2)
				{
					speed = 0.65f;
				}

				if (varityshot % 5 == 3)
				{
					speed = 0.75f;
				}
				if (varityshot % 5 == 4)
				{
					speed = 0.9f;
				}
				speed *= player.ArrowSpeed() * (26f+(perc*16f));


				int type = ProjectileID.ShadowFlameArrow;
				bool arrowtype = false;
				int damage = projectile.damage;
				float kb = projectile.knockBack;
				if (varityshot % 3 == 0)
                {
					type = ProjectileID.WoodenArrowFriendly;
					bool tr = true;
					player.PickAmmo(player.HeldItem, ref type, ref speed, ref tr, ref damage, ref kb,Main.rand.Next(4)!=0);
					arrowtype = true;
					var snd = Main.PlaySound(SoundID.Item5, projectile.Center);
					if (snd != null)
					{
						snd.Pitch = -0.75f;
					}
				}
				float f = 4+(perc* perc * 8);
				Vector2 offset1 = (shootdirection * Main.rand.NextFloat(-f, f)/2f);
				Projectile shadow = Projectile.NewProjectileDirect(projectile.Center+ offset1 + shootdirection.RotatedBy(MathHelper.PiOver2) * Main.rand.NextFloat(-f, f) * 6f, shootdirection * speed, type, damage, projectile.knockBack, player.whoAmI);
				if (!arrowtype)
				{
					shadow.usesLocalNPCImmunity = true;
					shadow.penetrate += 15;
					shadow.maxPenetrate += 15;
					shadow.extraUpdates = 1;
					shadow.localNPCHitCooldown = -1;
					shadow.timeLeft = 300;
					shadow.netUpdate = true;
				}

			}

		}

		public override bool DoChargeUp()
		{
			return true;
		}

		public override void FireWeapon(Vector2 direction)
		{
			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			projectile.Kill();
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			BlendState blind = new BlendState
			{

				ColorSourceBlend = Blend.Zero,
				ColorDestinationBlend = Blend.InverseSourceColor,

				AlphaSourceBlend = Blend.Zero,
				AlphaDestinationBlend = Blend.InverseSourceColor

			};

			float perc = MathHelper.Clamp(projectile.ai[0] / (float)chargeuptime, 0f, 1f);

			Texture2D mainTex = SGAmod.ExtraTextures[96];

			float alpha2 = perc * MathHelper.Clamp(chargeUpTimer, 0f, 1f);

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, blind, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Effect effect = SGAmod.TextureBlendEffect;

			for (int f = 0; f < 3; f += 1)
			{

				if (f == 2)
                {
					spriteBatch.End();
					spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
				}

				effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["coordOffset"].SetValue(new Vector2(0f, 0f));
				effect.Parameters["noiseMultiplier"].SetValue(new Vector2(1f, 1f));
				effect.Parameters["noiseOffset"].SetValue(new Vector2(0f, 0f));

				effect.Parameters["Texture"].SetValue(SGAmod.Instance.GetTexture("SmallLaser"));
				effect.Parameters["noiseTexture"].SetValue(SGAmod.Instance.GetTexture(f == 3 ? "SmallLaser" : "Extra_49c"));
				effect.Parameters["noiseProgress"].SetValue(Main.GlobalTime+f);
				effect.Parameters["textureProgress"].SetValue(0);
				effect.Parameters["noiseBlendPercent"].SetValue(1f);
				effect.Parameters["strength"].SetValue(MathHelper.Clamp(alpha2*3f,0f,1f));
				effect.Parameters["alphaChannel"].SetValue(false);

				Color colorz = f < 2 ? (f < 1 ? Color.White : Color.Lime) : Color.Purple;
				effect.Parameters["colorTo"].SetValue(colorz.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, projectile.velocity.ToRotation(), mainTex.Size() / 2f, (2f-(f/2f))*new Vector2(0.5f+perc*0.50f,0.5f+(perc* perc * 1.5f)), default, 0);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			return false;
        }

    }

		public class ShadeflameArrow : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeflame Arrow");
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.ShadowFlameArrow);
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.light = 0f;
			projectile.width = 16;
			projectile.height = 16;
			projectile.ranged = true;
			projectile.timeLeft = 260;
			projectile.extraUpdates = 2;
			projectile.tileCollide = false;
			projectile.penetrate = 4;
			projectile.arrow = true;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;

			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override string Texture => "Terraria/Projectile_" + ProjectileID.PhantasmArrow;

		public override bool CanDamage()
		{
			return projectile.penetrate>1 && projectile.timeLeft>30;
		}

		public override void AI()
		{
			projectile.localAI[0]++;

			projectile.rotation = projectile.velocity.ToRotation();//MathHelper.Clamp(projectile.localAI[0] / 1.25f, 0f, MathHelper.TwoPi * 5f) + 

			if (projectile.penetrate < 2 || projectile.timeLeft <= 60)
			{
				projectile.velocity *= 0.94f;
			}
			else
			{

			}

			projectile.Opacity = MathHelper.Clamp(projectile.timeLeft / 60f, 0f, Math.Min(projectile.localAI[0] / 15f, 1f));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<ShadeflareCharging>()];


			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.Black * projectile.Opacity*0.25f, projectile.rotation - MathHelper.PiOver2, tex.Size() / 2f, new Vector2(1.25f, 1.5f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.Magenta * projectile.Opacity*0.5f, projectile.rotation + MathHelper.PiOver2, tex2.Size() / 2f, projectile.scale * 1f, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.Purple * projectile.Opacity * 0.5f, projectile.rotation-MathHelper.PiOver2, tex.Size() / 2f, new Vector2(1f,1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);


			return false;
		}

	}

}
