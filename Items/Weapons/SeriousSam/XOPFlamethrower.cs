using Microsoft.Xna.Framework;
using Terraria;
using System;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Weapons.SeriousSam
{
	public class XOPFlamethrower : SeriousSamWeapon, ITechItem
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("XOP Flamethrower");
            Tooltip.SetDefault("Spreads sticky flames that ignore enemy defense, bounce off walls\nFlames don't intefere with your other weapons, and burn even fire immune enemies\nImmune enemies will burn for only half as long");
		}

        public override void SetDefaults()
        {
            item.damage = 22;
            item.ranged = true;
            item.width = 48;
            item.height = 28;
            item.useTime = 1;
            item.useAnimation = 10;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 600000;
			item.crit = 10;
            item.rare = 7;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<XOPFlames>();
            item.shootSpeed = 8f;
            item.useAmmo = AmmoID.Gel;
        }

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, -4);
        }

		public override bool ConsumeAmmo(Player player)
		{
			if (player.itemAnimation > 1)
			return false;
			return base.ConsumeAmmo(player);
		}

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EldMelter, 1);
			recipe.AddIngredient(null, "FieryShard", 10);
			recipe.AddIngredient(null, "AdvancedPlating", 10);
			recipe.AddIngredient(ItemID.LihzahrdPowerCell, 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
            recipe.SetResult(this);
      		recipe.AddRecipe();
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			position += offset;


			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(4));
				float scale = 1f - (Main.rand.NextFloat() * .2f);
				perturbedSpeed = perturbedSpeed * scale; 
				int prog=Projectile.NewProjectile(position.X+ offset.X, position.Y+ offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
                IdgProjectile.Sync(prog);
			}
			return false;

			/*Vector2 muzzleOffset = Vector2.Normalize(new Vector2(speedX, speedY)) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
			return true;*/
		}
	}

	public class XOPFlames : ModProjectile
	{
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Napalm Flames");
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
			projectile.ranged = true;
			projectile.timeLeft = 120;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.scale = 0.5f;
			projectile.usesIDStaticNPCImmunity = true;
			projectile.idStaticNPCHitCooldown = 4;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//target.immune[projectile.owner] = 5;
			IdgNPC.AddBuffBypass(target.whoAmI, ModContent.BuffType<NapalmBurn>(), 60 * (target.buffImmune[ModContent.BuffType<NapalmBurn>()] ? 4 : 8));
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			{
				//Main.PlaySound(SoundID.Item10, projectile.Center);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
				projectile.velocity /= 2f;
			}
			return false;
		}

		public override void AI()
		{
			projectile.scale += 0.01f;
			projectile.width = (int)((float)18f * projectile.scale);
			projectile.height = (int)((float)18f * projectile.scale);

			Tile tile = Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16];
			if (tile != null)
				if (tile.liquid > 64)
				{
					for (int num315 = 0; num315 < 40; num315 = num315 + 1)
					{
						Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
						randomcircle *= Main.rand.NextFloat(0f, 2f);
						int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0, 0, 50, Main.hslToRgb(0.15f, 1f, 1.00f), projectile.scale * 1.5f);
						Main.dust[num316].noGravity = true;
						Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y);
					}


					projectile.Kill();
				}

			Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() * 0.75f);

			
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += target.defense/2;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D tex = SGAmod.ExtraTextures[94];

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.Lerp((projectile.GetAlpha(lightColor) * 0.5f), Color.White, 0.5f); //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = 5-((int)(((float)projectile.timeLeft / 120f) *6f));
			timing *= ((tex.Height) / 5);
			float alpha = Math.Min(0.5f + (projectile.timeLeft / 120f), 1f);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), color* alpha, projectile.velocity.X * 0.04f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			return false;
		}


	}

	public class NapalmBurn : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Napalm Burn");
			Description.SetDefault("Sticky Fiery Death");
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/ThermalBlaze";
			return true;
		}

		public override void Update(NPC npc, ref int buffIndex)
		{
			npc.GetGlobalNPC<SGAnpcs>().Napalm = true;
		}
	}


}
