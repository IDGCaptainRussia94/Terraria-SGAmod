using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Items;
using SGAmod.Items.Weapons.Technical;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons
{
	public class Xenith : ModItem,IHellionDrop
	{
		int IHellionDrop.HellionDropAmmount() => 1;
		int IHellionDrop.HellionDropType() => ModContent.ItemType<Xenith>();

		public static int[] XenithBowTypes
        {
            get
            {
				int[] types = new int[]{
				ItemID.CopperBow,
				ItemID.BeesKnees,
				ItemID.HellwingBow,
				ItemID.Marrow,
				ItemID.IceBow,
				ItemID.PulseBow,
				ItemID.DD2PhoenixBow,
				ModContent.ItemType<DeltaWing>(),
				ItemID.DD2BetsyBow,
				ItemID.Phantasm,
				ModContent.ItemType<HavocGear.Items.Weapons.Shadeflare>(),
				};

				return types;

			}
        }
        public override bool Autoload(ref string name)
        {
            SGAmod.PostUpdateEverythingEvent += SGAmod_PostUpdateEverythingEvent;
			return true;
        }

        private void SGAmod_PostUpdateEverythingEvent()
        {
			Main.itemTexture[item.type] = Main.itemTexture[XenithBowTypes[Main.rand.Next(XenithBowTypes.Length)]];
		}

        public override string Texture => "Terraria/Projectile_" + ProjectileID.ShadowFlameArrow;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xenith");
			Tooltip.SetDefault("'Basically Zenith bow'");
		}
		
		public override void SetDefaults()
		{
			item.damage = 100;
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
			item.shoot = ModContent.ProjectileType<XenithCharging>();
			item.channel = true;
			item.shootSpeed = 50f;
			item.useAmmo = AmmoID.Arrow;
			/*if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Shadeflare_Glow");
			}*/
		}

        public override void HoldItem(Player player)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<XenithBowProj>()] < 1)
            {
				Projectile.NewProjectile(player.Center, Vector2.Zero, ModContent.ProjectileType<XenithBowProj>(), 1, 1,player.whoAmI);
            }
        }

        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			foreach (int type in XenithBowTypes)
			{
				recipe.AddIngredient(type, 1);
			}
			recipe.AddIngredient(ModContent.ItemType<DrakeniteBar>(), 8);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			type = ModContent.ProjectileType<XenithCharging>();
			return true;
		}
	}

	public class XenithCharging : HavocGear.Items.Weapons.ShadeflareCharging
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
				float rate = 0.5f;



				return (rate, 0f);
			}
		}
		//public override int FireCount => 600;
		int chargeUpTimer=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Xenith Charging");
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
			foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.owner == projectile.owner && testby.type == ModContent.ProjectileType<XenithBowProj>()))
			{
				proj.damage = projectile.damage;
				XenithBowProj proj2 = proj.modProjectile as XenithBowProj;
				proj.rotation = projectile.velocity.ToRotation();
				proj2.projectile.ai[0] = 5;
				proj2.projectile.netUpdate = true;
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
			/*
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

				Color colorz = f < 2 ? (f < 1 ? Color.White : Color.Lime) : Color.Purple;
				effect.Parameters["colorTo"].SetValue(colorz.ToVector4());
				effect.Parameters["colorFrom"].SetValue(Color.Black.ToVector4());

				effect.CurrentTechnique.Passes["TextureBlend"].Apply();

				Main.spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, projectile.velocity.ToRotation(), mainTex.Size() / 2f, (2f-(f/2f))*new Vector2(0.5f+perc*0.50f,0.5f+(perc* perc * 1.5f)), default, 0);
			}

			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			*/

			return false;
        }

    }

		public class XenithBowProj : ModProjectile
	{
		public Player Owner => Main.player[projectile.owner];
		public int TimeToShootPerBow => 30;
		public class HoveringBow
        {
			public XenithBowProj owner;
			public Player player;
			public int index = 0;
			public int time = 0;
			public int timeSinceAttack = 0;
			public int IndexMax => Xenith.XenithBowTypes.Length;
			public int BowType => Xenith.XenithBowTypes[index];
			public float rotation = 0;
			public float swapPositions = 0;
			public Vector2 _position;
			public Vector2 Position
            {
                get
                {
					return Vector2.SmoothStep(_position,bowPosition, MathHelper.Clamp(swapPositions,0f,1f));
                }
                set
                {
					_position = value;
                }
            }
			public Vector2 bowPosition = default;

			public SpriteEffects SpriteDirection => rotation % MathHelper.TwoPi > MathHelper.Pi ? SpriteEffects.FlipVertically : 0;

			public HoveringBow(XenithBowProj owner,Player player,Vector2 position,int index)
            {
				this.owner = owner;
				this.player = player;
				this._position = position;
				this.index = index;
			}

			public void UpdateShooting()
			{
				swapPositions = Math.Min(swapPositions + 0.05f,1f);

				rotation = rotation.AngleLerp(owner.projectile.rotation,0.20f);
				int timer = (int)(((index / (float)IndexMax)* owner.TimeToShootPerBow) +player.SGAPly().timer);
				if (timer % owner.TimeToShootPerBow == 0)
				{
					if (player.HasAmmo(player.HeldItem, true))
					{

						timeSinceAttack = 0;
						Item item = new Item();
						item.SetDefaults(BowType);

						Main.PlaySound(item.UseSound,(int)owner.projectile.Center.X,(int)owner.projectile.Center.Y);

						int projType = item.shoot;

						bool canShoot = true;
						int damage = owner.projectile.damage;
						float knockback = owner.projectile.knockBack;
						float speed = 32f;
						if (projType == ProjectileID.WoodenArrowFriendly || projType == 10)
						{
							if (item.type == ItemID.MoltenFury)
								projType = ProjectileID.FireArrow;

							player.PickAmmo(item, ref projType, ref speed, ref canShoot, ref damage, ref knockback, false);
						}

						bool hide = false;

						if (item.type == ModContent.ItemType<HavocGear.Items.Weapons.Shadeflare>())
							projType = ProjectileID.ShadowFlameArrow;
						if (item.type == ItemID.PulseBow)
							projType = ProjectileID.PulseBolt;
						if (item.type == ItemID.DD2BetsyBow)
							projType = ProjectileID.DD2BetsyArrow;
						if (item.type == ItemID.Marrow)
						{
							speed += 64;
							projType = ProjectileID.BoneArrow;
						}
						if (item.type == ItemID.IceBow)
							projType = ProjectileID.FrostArrow;
						if (item.type == ItemID.DD2PhoenixBow)
						{
							foreach (Projectile proj2 in Main.projectile.Where(testby => testby.active && testby.owner == player.whoAmI && testby.type == ProjectileID.DD2PhoenixBow))
							{
								proj2.Kill();
							}
							hide = true;
							damage = damage / 3;

							//projType = ProjectileID.DD2PhoenixBowShot;
						}
						if (item.type == ItemID.Phantasm)
						{
							foreach(Projectile proj2 in Main.projectile.Where(testby => testby.active && testby.owner == player.whoAmI && testby.type == ProjectileID.Phantasm))
                            {
								proj2.Kill();
							}
							hide = true;
							damage = damage / 3;
						}

						Projectile proj = Projectile.NewProjectileDirect(Position, rotation.ToRotationVector2() * speed*player.ArrowSpeed(), projType,damage,knockback, owner.projectile.owner);
						proj.Center = Position;
						proj.usesIDStaticNPCImmunity = true;
						proj.idStaticNPCHitCooldown = 15;
						if (hide)
							proj.Opacity=0f;
						proj.timeLeft = Math.Min(proj.timeLeft, 300);
					}

				}
			}

			public void Draw(SpriteBatch sb,Color lighting)
            {
				Texture2D tex = Main.itemTexture[BowType];

				sb.Draw(tex, Position - Main.screenPosition, null, lighting, rotation, tex.Size() / 2f, 1, SpriteDirection, 0);

			}
		}

		public List<HoveringBow> bows = new List<HoveringBow>();

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("XenithBowProj");
		}
		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.light = 0f;
			projectile.width = 16;
			projectile.height = 16;
			projectile.ranged = true;
			projectile.timeLeft = 22;
			projectile.extraUpdates = 0;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.arrow = true;
			projectile.netImportant = true;

			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;

			ProjectileID.Sets.TrailCacheLength[projectile.type] = 15;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
		}

		public override string Texture => "Terraria/Projectile_" + ProjectileID.PhantasmArrow;

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{
			projectile.localAI[0]++;
			projectile.ai[0] -= 1;

			projectile.Opacity = MathHelper.Clamp(projectile.timeLeft / 20f, 0f, Math.Min(projectile.localAI[0] / 15f, 1f));

			bool staySummoned = true;

			if (projectile.timeLeft <= 20)
			{
				staySummoned = false;
				projectile.velocity *= 0.94f;
			}

			if (Owner == null || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<Xenith>())
            {
				staySummoned = false;
			}

			if (!staySummoned)
			{
				return;
			}

			if (projectile.localAI[0] == 1)
            {
				for (int i = 0; i < Xenith.XenithBowTypes.Length; i += 1)
				{
					HoveringBow bow = new HoveringBow(this, Owner, Owner.Center, i);
					bows.Add(bow);
				}
			}

			if (!Main.dedServ)
			{
				if (projectile.ai[0] < 1)
				{
					projectile.rotation = (Main.MouseWorld - projectile.Center).ToRotation();
					projectile.netUpdate = true;
				}
			}

			float followSpeedRate = projectile.ai[0] > 0 ? 15f : 1f;


			foreach (HoveringBow bow in bows)
            {
				bow.time++;
				bow.timeSinceAttack++;
				float percentOne = 1f / (float)bow.IndexMax;
				float percent = bow.index / (float)bow.IndexMax;

				float rotangle = (percent * MathHelper.TwoPi) + bow.time / 32f;
				rotangle %= MathHelper.TwoPi;
				bow._position = projectile.Center+Vector2.UnitX.RotatedBy(rotangle) * new Vector2(256f, 64f);

				Matrix transMatrix = Matrix.CreateScale(1f, 3f, 0f) * Matrix.CreateRotationZ(-MathHelper.PiOver2 + (percent * MathHelper.Pi)+ ((percentOne * MathHelper.Pi)*0.5f)) * Matrix.CreateRotationZ(projectile.rotation) * Matrix.CreateTranslation(projectile.Center.X, projectile.Center.Y, 0);

				bow.bowPosition = Vector2.Transform(Vector2.UnitX*128f,transMatrix);

				if (projectile.ai[0] > 0)
				{
					bow.UpdateShooting();
                }
                else
                {
					bow.swapPositions = Math.Max(bow.swapPositions - 0.05f,0);
					bow.rotation %= MathHelper.TwoPi;
					bow.rotation = bow.rotation.AngleLerp(rotangle, MathHelper.Clamp(bow.timeSinceAttack/300f,0f,1f));
				}

			}

			Vector2 gotohere = Owner.MountedCenter + (projectile.ai[0]>0 ? Vector2.Zero : new Vector2(0, -96f));

			projectile.velocity *= 0.96f;


			projectile.Center += (gotohere - projectile.Center) / (24f/followSpeedRate);
			projectile.velocity += (gotohere - projectile.Center)/(320f/ followSpeedRate);

			projectile.timeLeft = 22;

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];

			Effects.TrailHelper trail = new Effects.TrailHelper("FadedBasicEffectAlphaPass", mod.GetTexture("SmallLaser"));
			trail.color = delegate (float percent)
			{
				return Main.hslToRgb((percent+Main.GlobalTime/3f)%1f,1f,0.75f);
			};

			trail.projsize = Vector2.Zero;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.coordMultiplier = new Vector2(1f, 3f);
			trail.trailThickness = 16f;
			trail.trailThicknessIncrease = 0;
			trail.doFade = false;
			trail.connectEnds = true;
			trail.strength = 1f* projectile.Opacity;

			List<Vector2> spots = bows.Select(testby => testby.Position).ToList();

			trail.DrawTrail(spots, projectile.Center);

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.Black * projectile.Opacity*0.5f, projectile.rotation-MathHelper.PiOver2, tex.Size() / 2f, projectile.scale, projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);

			foreach (HoveringBow bow in bows.OrderBy(testby => testby.Position.Y))
			{
				bow.Draw(spriteBatch, lightColor* projectile.Opacity);
			}

				return false;
		}

	}

}
