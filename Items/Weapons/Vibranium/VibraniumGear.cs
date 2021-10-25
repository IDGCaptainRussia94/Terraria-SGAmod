using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Audio;
using AAAAUThrowing;
using SGAmod.Items.Placeable;
using SGAmod.Effects;
using System.Linq;
using Terraria.DataStructures;

namespace SGAmod.Items.Weapons.Vibranium
{
	public class VibraniumText : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Not an Item");
		}

		public override bool PreDrawTooltipLine(DrawableTooltipLine line, ref int yOffset)
		{
			if (line.mod == "Terraria" && line.Name == "ItemName")
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, Main.UIScaleMatrix);

				Effect hallowed = SGAmod.HallowedEffect;

				Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);

				hallowed.Parameters["alpha"].SetValue(0.5f);
				hallowed.Parameters["prismColor"].SetValue(Color.Lerp(Color.Lerp(Color.Red, Color.Blue, 0.50f + (float)Math.Sin(Main.GlobalTime / 0.5f) / 2.5f),Color.Gray,0.35f).ToVector3());
				hallowed.Parameters["rainbowScale"].SetValue(0.25f);
				hallowed.Parameters["overlayScale"].SetValue(new Vector2(4,2));
				hallowed.Parameters["overlayTexture"].SetValue(SGAmod.Instance.GetTexture("Doom_Harbinger_Resprite_pupil"));
				hallowed.Parameters["overlayProgress"].SetValue(new Vector3((float)Math.Sin(Main.GlobalTime * 8f)>0 ? Main.GlobalTime * 2f : -Main.GlobalTime * 2f, 0.5f, Main.GlobalTime * 3f));
				hallowed.Parameters["overlayAlpha"].SetValue(0.33f);
				hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f, 0f, 0f));
				hallowed.Parameters["overlayMinAlpha"].SetValue(0f);
				hallowed.CurrentTechnique.Passes["Prism"].Apply();

				Utils.DrawBorderString(Main.spriteBatch, line.text, new Vector2(line.X, line.Y), Color.White);
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				return false;
			}
			return true;
		}

		public override bool Autoload(ref string name)
        {
			return GetType() != typeof(VibraniumText) && SGAmod.VibraniumUpdate;
        }
	}
		
		public class QuasarKunai : VibraniumText
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Quaser Kunai");
			Tooltip.SetDefault("Throws resonant knives that bounce between targets, bypass defense, and leave a stacking DoT\nThe knives bounce more against Irradiated targets and do 15% more damage\nWhen fully charged, emits a Gamma Ray burst in both directions\nThis inflicts Severe Radiation to hit enemies");
		}

		public override string Texture => "SGAmod/Items/Weapons/Vibranium/QuasarKunai";

		public override void SetDefaults()
		{
			item.damage = 110;
			item.crit = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 8;
			item.useAnimation = 8;
			item.useStyle = 1;
			item.knockBack = 5;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.value = 500000;
			item.rare = ItemRarityID.Cyan;
			//item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.Throwing().thrown = true;
			item.shoot = ModContent.ProjectileType<GammaBurstProjectileChargeUp>();
			item.shootSpeed = 10f;
			item.channel = true;

			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Main.hslToRgb((Main.GlobalTime * -0.5f) % 1f, 0.8f, 0.75f);
				};

				item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle, Color glowcolor)
				{
					if (drawInfo.drawPlayer.ownedProjectileCounts[item.shoot] > 0)
					{
						var myProjectile1 = Main.projectile.Where(testby => testby.type == item.shoot).ToList();
						if (myProjectile1 != null && myProjectile1.Count > 0 && drawInfo.drawPlayer.channel)
						{
							Projectile myProjectile = myProjectile1[0];
							GammaBurstProjectileChargeUp chargeUp = myProjectile.modProjectile as GammaBurstProjectileChargeUp;

							Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<GammaBurstProjectile>()];
							Vector2 origin = texture.Size() / 2f;
							float timeAdvance = Main.GlobalTime * 2;
							angle = drawInfo.drawPlayer.itemRotation + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
							Player drawPlayer = drawInfo.drawPlayer;

							Vector2 drawHere = drawPlayer.MountedCenter + (angle.ToRotationVector2()) * 0 - Main.screenPosition;
							float measure = MathHelper.TwoPi / chargeUp.MaxKnives;
							float maxValue = MathHelper.Clamp(myProjectile.ai[1] / chargeUp.MaxCharge, 0f, 1f) * (MathHelper.TwoPi);

							for (float i = 0f; i < maxValue; i += measure)
							{
								float peralpha = MathHelper.Clamp(maxValue - (float)i, 0f, measure) / measure;
								Color rainbow = Color.White * peralpha;
								DrawData value = new DrawData(texture, drawHere + (Vector2.One.RotatedBy(i - timeAdvance)) * 32f, null, rainbow * MathHelper.Clamp(drawPlayer.itemAnimation / 4f, 0f, 1f), 3 * MathHelper.PiOver4 + (i - timeAdvance), origin, 1f, drawInfo.spriteEffects, 0);
								Main.playerDrawData.Add(value);
							}

							for (float i = 0f; i < MathHelper.Clamp(myProjectile.ai[1] / chargeUp.MaxCharge, 0f, 1f) * (MathHelper.TwoPi - measure); i += measure)
							{
								float peralpha = MathHelper.Clamp(maxValue - (float)i, 0f, measure) / measure;
								Color rainbow = Color.Lerp(Color.Blue,Color.Red,0.5f+(float)Math.Sin(((drawPlayer.SGAPly().timer / 30f) + i))/2f) * peralpha;
								Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
								DrawData value = new DrawData(tex2, drawHere + (Vector2.One.RotatedBy(i - timeAdvance)) * 32f, null, rainbow * 0.75f * MathHelper.Clamp(drawPlayer.itemAnimation / 4f, 0f, 1f), 3 * MathHelper.PiOver4 + (i - timeAdvance), tex2.Size() / 2f, new Vector2(0.75f, 1f), drawInfo.spriteEffects, 0);
								Main.playerDrawData.Add(value);

							}
						}

					}

				};
			}

		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			/*if (player.altFunctionUse == 2)
			{
				SoundEffectInstance sound = Main.PlaySound(SoundID.Item24, position);
				if (sound != null)
				{
					sound.Pitch = 0.99f;
				}
				sound = Main.PlaySound(SoundID.DD2_GoblinBomberThrow, position);
				if (sound != null)
				{
					sound.Pitch = 0.99f;
				}
				type = ModContent.ProjectileType<GammaBurstProjectile>();
				Vector2 velo = new Vector2(speedX, speedY);
				velo = velo.RotatedByRandom(MathHelper.Pi / 24f);
				speedX = velo.X;
				speedY = velo.Y;
				return true;
			}*/

			return true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			/*Texture2D inner = Main.itemTexture[item.type];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			float[] angles = { MathHelper.Pi / 3f, -MathHelper.Pi / 3f, -MathHelper.Pi / 6f, MathHelper.Pi / 6f, 0 };
			for (int i = 0; i < angles.Length; i += 1)
			{
				spriteBatch.Draw(inner, drawPos, null, Color.White, angles[i], textureOrigin, Main.inventoryScale * 1.25f, SpriteEffects.None, 0f);
			}*/

			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ModContent.ItemType<AuroraTearAwoken>(), 1);
			recipe.AddIngredient(ModContent.ItemType<VibraniumBar>(), 10);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.Weapons.MangroveShiv>(), 100);
			recipe.AddIngredient(ItemID.MagicDagger, 1);
			recipe.AddTile(ModContent.TileType<Tiles.TechTiles.LuminousAlter>());
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

	}

	public class GammaBurstProjectileChargeUp : ModProjectile
	{
		public Player Player => Main.player[projectile.owner];
		public float MaxCharge => 300f;
		public float MaxKnives => 20f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gamma Burst Charging");
		}
		public override string Texture => "SGAmod/Projectiles/QuasarKunaiProj";
		public override bool CanDamage()
		{
			return false;
		}
		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			projectile.timeLeft = 15;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
			projectile.tileCollide = false;
		}
		public override void AI()
		{
			if (!Player.active || Player.dead)
			{
				projectile.Kill();
				return;
			}

			bool channeling = ((Player.channel || (projectile.ai[1] < 5)) && !Player.noItems && !Player.CCed);
			projectile.localAI[0] += 1;

			float chargepercent = projectile.ai[1] / MaxCharge;

			if (channeling)
			{
				projectile.ai[1] = MathHelper.Clamp(projectile.ai[1] + Player.SGAPly().ThrowingSpeed * 3f, 0f, MaxCharge);
				if (Main.netMode != NetmodeID.Server)
				{
					projectile.velocity = Vector2.Normalize(Main.MouseWorld - Player.MountedCenter) * projectile.velocity.Length();
					projectile.netUpdate = true;
				}
				Player.ChangeDir(projectile.velocity.X > 0 ? 1 : -1);
				Player.itemAnimation = Player.itemAnimationMax;
				Player.itemTime = Player.itemAnimationMax;
				projectile.timeLeft = Player.itemAnimationMax;
				projectile.Center = Player.MountedCenter;
				if (projectile.localAI[0] % 10 == 0 && projectile.localAI[0] > -1)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, projectile.Center);
					if (sound != null)
					{
						sound.Pitch = chargepercent * 0.5f;
					}
				}
				if (projectile.ai[1] == MaxCharge && projectile.localAI[0] > -1)
				{
					projectile.localAI[0] = -999999;
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_WitherBeastAuraPulse, projectile.Center);
					if (sound != null)
					{
						sound.Pitch = 0.8f;
					}
				}
			}
			else
			{
				if (projectile.ai[0] < 1)
				{
					int max = 1 + (int)(chargepercent * MaxKnives);
					if (projectile.ai[1] == MaxCharge)
					{
						projectile.ai[0] += 1;
						for (float f = 0; f <= MathHelper.Pi; f += MathHelper.Pi)
							Projectile.NewProjectile(projectile.Center, projectile.velocity.RotatedBy(f), ModContent.ProjectileType<GammBurstBeam>(), projectile.damage* 2 * (int)MaxKnives, projectile.knockBack * 3f, projectile.owner);

						SoundEffectInstance sound2 = Main.PlaySound(SoundID.DD2_DrakinShot, projectile.Center);
						if (sound2 != null)
						{
							sound2.Pitch = 0.8f;
						}
						return;
					}
					for (int i = 0; i < max; i += 1)
					{
						float maxAngle = (MathHelper.Pi * chargepercent) * 0.4f;
						float angle = (i / (float)(max - 1f)) * (maxAngle);
						float angleMax = (maxAngle);

						Vector2 velo = projectile.velocity;
						if (max > 1)
							velo = projectile.velocity.RotatedBy(angle - (angleMax / 2f));

						int proj = Projectile.NewProjectile(projectile.Center, velo.RotatedByRandom(MathHelper.Pi / 64f), ModContent.ProjectileType<GammaBurstProjectile>(), projectile.damage, projectile.knockBack, projectile.owner);
						Main.projectile[proj].localAI[0] = Main.rand.Next(0, 2);
					}

					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_GoblinBomberThrow, projectile.Center);
					if (sound != null)
					{
						sound.Pitch = 0.99f;
					}

				}
				projectile.ai[0] += 1;
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}
	}

	public class GammaBurstProjectile : ModProjectile
	{
		public List<Point> enemiesHit = new List<Point>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gamma Burst");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 16;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.ThrowingKnife);
			projectile.timeLeft = 320;
			projectile.extraUpdates = 3;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.penetrate = 10;
		}
		public override string Texture => "SGAmod/Projectiles/QuasarKunaiProj";

		public override bool PreKill(int timeLeft)
		{
			return true;
		}
		public override bool CanDamage()
		{
			return projectile.timeLeft > 20;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += (int)(target.defense / 2);
			if (target.HasBuff(ModContent.BuffType<Buffs.RadioDebuff>()))
			{
				damage += (int)(damage * 1.15f);
				projectile.penetrate += 1;
			}
		}
		public override bool? CanHitNPC(NPC target)
		{
			if (target.friendly)
				return false;

			return (enemiesHit.FirstOrDefault(testby => target.type == testby.X) == default);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			enemiesHit.Add(new Point(target.whoAmI, 1000000));
			List<NPC> closestnpcs = SGAUtils.ClosestEnemies(projectile.Center, 640, projectile.Center, AddedWeight: enemiesHit, checkCanChase: false);

			target.SGANPCs().AddDamageStack((int)(damage * 1.5f), 200);

			for (float num315 = 4; num315 < 16; num315 = num315 + 1f)
			{
				Vector2 randomcircle = Vector2.Normalize(projectile.velocity) * num315;
				int num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y), 0, 0, DustID.AncientLight, 0f, 0f, 100, Main.hslToRgb((projectile.whoAmI / 30f) % 1f, 1f, 0.75f), 1.5f);
				Main.dust[num622].noGravity = true;
				Main.dust[num622].velocity = randomcircle;
				Main.dust[num622].alpha = 100;
			}

			if (closestnpcs != null && closestnpcs.Count > 0)
			{
				float speed = projectile.velocity.Length();
				projectile.velocity = Vector2.Normalize(closestnpcs[0].Center - projectile.Center) * speed;

				SoundEffectInstance sound = Main.PlaySound(SoundID.Item7, projectile.Center);
				if (sound != null)
				{
					sound.Pitch = -0.5f;
				}
			}

		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (projectile.timeLeft > 20)
				projectile.timeLeft = 20;
			projectile.tileCollide = false;
			projectile.velocity = oldVelocity;
			return false;
		}
		public override bool PreAI()
		{
			//bool cond = projectile.localAI[1]>240;
			//if (!cond)
			if (projectile.timeLeft > 20 && projectile.penetrate < projectile.maxPenetrate - 2)
				projectile.timeLeft = 20;

			projectile.rotation = projectile.velocity.ToRotation() - MathHelper.PiOver2;
			return false;
		}
		public override void PostAI()
		{
			projectile.localAI[1] += 1;
			//projectile.position -= projectile.velocity * MathHelper.Clamp(1f - (projectile.localAI[1] / 30f), 0f, 1f);
			//projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver4;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D inner = Main.projectileTexture[projectile.type];
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);
			Color colorx = (projectile.localAI[0] % 2 == 0 ? Color.Red : Color.Blue);

			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}

			TrailHelper trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("TiledPerlin"));
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(Main.GlobalTime * -2.5f, 0);
			trail.trailThickness = 2;
			trail.trailThicknessIncrease = 4;
			trail.strength = MathHelper.Clamp(projectile.timeLeft / 20f, 0f, 0.5f);
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];

			for (int i = projectile.oldPos.Length - 1; i > 0; i -= 1)
			{
				spriteBatch.Draw(tex2, projectile.oldPos[i] + (new Vector2(projectile.width, projectile.height) / 2f) - Main.screenPosition, null, colorx * 0.75f * MathHelper.Clamp(projectile.timeLeft / 20f, 0f, 1f) * 0.30f * (1f - (i / (float)projectile.oldPos.Length)), projectile.rotation, tex2.Size() / 2f, new Vector2(0.5f, 1f) * projectile.scale, SpriteEffects.FlipVertically, 0);
			}


			spriteBatch.Draw(inner, projectile.Center - Main.screenPosition, null, Color.White * 0.75f * MathHelper.Clamp(projectile.timeLeft / 20f, 0f, 1f), projectile.rotation, textureOrigin, new Vector2(1f, 1f) * projectile.scale, SpriteEffects.FlipVertically, 0);
			spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, colorx * 0.75f * MathHelper.Clamp(projectile.timeLeft / 20f, 0f, 1f) * 0.5f, projectile.rotation, tex2.Size() / 2f, new Vector2(1.5f, 0.75f) * projectile.scale, SpriteEffects.FlipVertically, 0);
			return false;
		}
	}

	internal class GammBurstBeamExplosion
	{
		internal int time;
		internal int timeMax;
		internal float size;
		internal float ringSize;
		internal float angle;
		internal Vector2 loc;

		internal GammBurstBeamExplosion(Vector2 loc, int time, float angle, float size, float ringSize = 0.15f)
		{
			timeMax = time;
			this.time = 0;
			this.size = size;
			this.loc = loc;
			this.angle = angle;
			this.ringSize = ringSize;
		}
		internal bool Update()
		{
			time += 1;
			return time >= timeMax;
		}

	}



	public class GammBurstBeam : ModProjectile
	{
		Vector2 start = default;
		List<GammBurstBeamExplosion> boomList = new List<GammBurstBeamExplosion>();
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 400;
			projectile.light = 0.1f;
			projectile.extraUpdates = 10;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override bool CanDamage()
		{
			return projectile.timeLeft > 60;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gamma Burst");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//projectile.timeLeft -= 5;
			projectile.velocity = oldVelocity * 0.9f;
			return false;
		}
		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage += (int)(target.defense / 2);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.SGANPCs().IrradiatedAmmount = projectile.damage * 3;
			target.AddBuff(mod.BuffType("RadioDebuff"), 60 * 20);
			//stuff
		}

		public override void AI()
		{
			projectile.localAI[0] += 1;
			List<GammBurstBeamExplosion> explocopy = new List<GammBurstBeamExplosion>(boomList);
			foreach (GammBurstBeamExplosion explosion in explocopy)
			{
				if (explosion.Update())
				{
					boomList.Remove(explosion);
				}
			}

			if (projectile.localAI[0] % 30 == 0)
            {
				boomList.Add(new GammBurstBeamExplosion(projectile.Center, 200, projectile.velocity.ToRotation(), 24f, 0.15f));
			}

			/*int num126 = Dust.NewDust(projectile.Center, 0, 0, 173, projectile.velocity.X, projectile.velocity.Y, 0, default(Color), 3.0f);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = projectile.velocity * 0.5f;*/
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Player player = Main.player[projectile.owner];
			if (start == default && player != null)
				start = player.MountedCenter;

			Effect RadialEffect = SGAmod.RadialEffect;
			Texture2D mainTex = mod.GetTexture("GreyHeart");//Main.projectileTexture[projectile.type];

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (GammBurstBeamExplosion explosion in boomList)
			{
				RadialEffect.Parameters["overlayTexture"].SetValue(mod.GetTexture("Space"));
				RadialEffect.Parameters["alpha"].SetValue(MathHelper.Clamp((explosion.timeMax - explosion.time) / 200f, 0f, Math.Min(explosion.time / 60f, 1f)) * MathHelper.Clamp(projectile.timeLeft / 200f, 0f, 1f));
				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f, Main.GlobalTime * 0.275f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 1f));
				RadialEffect.Parameters["ringScale"].SetValue(explosion.ringSize);
				RadialEffect.Parameters["ringOffset"].SetValue(0.20f+(explosion.time/(float)explosion.timeMax)*0.50f);
				RadialEffect.Parameters["ringColor"].SetValue(Color.White.ToVector3());
				RadialEffect.Parameters["tunnel"].SetValue(false);

				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

				spriteBatch.Draw(mainTex, explosion.loc - Main.screenPosition, new Rectangle(11,0,11,22), Color.White, explosion.angle, mainTex.Size() / 2f, new Vector2(0.5f, 1f)*explosion.size, default, 0);
			}

			List<Vector2> places = new List<Vector2>() { start, start + (projectile.Center - start) * 0.5f, projectile.Center };

			TrailHelper trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("Space"));
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -3f);
			trail.coordMultiplier = new Vector2(projectile.velocity.X > 0 ? 1 : -1, 6f);
			trail.trailThickness = 32;
			trail.trailThicknessIncrease = 0;
			trail.doFade = true;
			trail.strength = MathHelper.Clamp(projectile.timeLeft / 300f, 0f, 1f);
			trail.DrawTrail(places, projectile.Center);

			places = new List<Vector2>() { start, start + (projectile.Center - start) * 0.95f, projectile.Center };

			trail = new TrailHelper("FadedBasicEffectPass", mod.GetTexture("Stain"));
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -12f);
			trail.coordMultiplier = new Vector2(projectile.velocity.X > 0 ? 1 : -1, 6f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 16;
			trail.doFade = true;
			trail.strength = MathHelper.Clamp(projectile.timeLeft / 300f, 0f, 1f);
			trail.DrawTrail(places, projectile.Center);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			foreach (GammBurstBeamExplosion explosion in boomList)
			{
				RadialEffect.Parameters["overlayTexture"].SetValue(mod.GetTexture("Space"));
				RadialEffect.Parameters["alpha"].SetValue(MathHelper.Clamp((explosion.timeMax - explosion.time) / 200f, 0f, Math.Min(explosion.time/60f, 1f)) * MathHelper.Clamp(projectile.timeLeft / 200f, 0f, 1f));
				RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime * 0.125f, Main.GlobalTime * 0.275f));
				RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f, 1f));
				RadialEffect.Parameters["ringScale"].SetValue(explosion.ringSize);
                RadialEffect.Parameters["ringOffset"].SetValue(0.20f + (explosion.time / (float)explosion.timeMax) * 0.50f);
				RadialEffect.Parameters["ringColor"].SetValue(Color.White.ToVector3());
				RadialEffect.Parameters["tunnel"].SetValue(false);

				RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

				spriteBatch.Draw(mainTex, explosion.loc - Main.screenPosition, new Rectangle(0, 0, 11, 22), Color.White, explosion.angle, new Vector2(22, 11), new Vector2(0.5f, 1f) * explosion.size, default, 0);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			return false;

		}
	}
}
