using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;

namespace SGAmod.Items.Weapons.Technical
{

	public class EngineerSentrySummon : NoviteTowerSummon, ITechItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Engineering Rod");
			Tooltip.SetDefault("Deploys a TR12 Gauss Auto-Turret infront of you\nCam only deploy when the hologram is Blue");
		}

		public override void SetDefaults()
		{
			item.damage = 32;
			item.summon = true;
			item.sentry = true;
			item.width = 24;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 0.5f;
			item.value = Item.buyPrice(0, 1, 25, 0);
			item.rare = ItemRarityID.Orange;
			item.autoReuse = false;
			item.useTurn = false;
			item.shootSpeed = 0f;
			//item.UseSound = SoundID.Item78;
			item.shoot = ModContent.ProjectileType<EngineerSentryProj>();
		}

		Vector2 DeployArea(Player player) => player.MountedCenter + new Vector2(player.direction * 32f, 0);

		public override void HoldItem(Player player)
		{				
				Vector2 pos = DeployArea(player);
				bool valid = TurretPositionValid(item, player, ref pos);
				Projectile.NewProjectile(pos.X, pos.Y, player.direction, 0, ModContent.ProjectileType<EngineerSentryProjHologram>(), 0, 0, player.whoAmI, 0f, valid ? 1 : 0);
		}

		public static bool TurretPositionValid(Item item,Player player,ref Vector2 where)
        {
			int pushYUp = -1;
			player.FindSentryRestingSpotBetter(where, out var worldX, out var worldY, out pushYUp);

			where.X = worldX;
			where.Y = worldY- pushYUp;

			if (Collision.CanHitLine(player.MountedCenter,8,8, where, 8, 8) && (where-player.MountedCenter).Length()< 72)
            {
				return true;

            }
			return false;
        }


		public override bool CanUseItem(Player player)
        {
			Vector2 pos = DeployArea(player);
			return TurretPositionValid(item,player, ref pos);
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				Vector2 pos = DeployArea(player);
				TurretPositionValid(item, player, ref pos);
				Projectile.NewProjectile(pos.X, pos.Y, player.direction, 0, type, damage, knockBack, player.whoAmI, 0f, 0f);
				player.UpdateMaxTurrets();
			}
			return false;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<NoviteTowerSummon>(), 1);
			recipe.AddIngredient(ModContent.ItemType<AdvancedPlating>(), 5);
			recipe.AddIngredient(ModContent.ItemType<ManaBattery>(), 2);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
	}

	public class EngineerSentryProj : NoviteTower
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("RoR2 Sentry");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.FrostHydra);
			projectile.width = 32;
			projectile.height = 32;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.aiStyle = -1;
		}

		Vector2 LookFrom => projectile.Center+new Vector2(0,-8);

		public override void AI()
		{

			Player player = Main.player[base.projectile.owner];
			projectile.localAI[0] += 1;

			if (projectile.localAI[0] == 1)
            {
				if (projectile.velocity.X < 0)
				{
					projectile.spriteDirection = -1;
					projectile.rotation = MathHelper.Pi;
					projectile.ai[1] = MathHelper.Pi;
				}

				if (projectile.localAI[0] == 1)
				{
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/RoR2sndTurretDeploy").WithVolume(0.75f).WithPitchVariance(.15f), projectile.Center);
				}
				projectile.velocity.X = 0;
			}

			if (projectile.localAI[0] > 10)
			{
				bool solidtiles = false;
				Point tilehere = ((projectile.position) / 16).ToPoint();
				tilehere.Y += 2;
				for (int i = 0; i < 3; i += 1)
				{
					Tile tile = Framing.GetTileSafely(tilehere.X + i, tilehere.Y);
					if (WorldGen.InWorld(tilehere.X+i, tilehere.Y) && tile != null && tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]))
					{
						solidtiles = true;
						break;
					}
				}

				if (solidtiles)
				{
					projectile.velocity = new Vector2(projectile.velocity.X, 0);
				}
				else
				{
					projectile.velocity = new Vector2(projectile.velocity.X, projectile.velocity.Y + 0.25f);
				}
			}

			if (projectile.localAI[0] < 120)
				return;

			float aimTo = projectile.ai[1];

			Vector2 dotRotation = projectile.rotation.ToRotationVector2();

			List<NPC> enemies = SGAUtils.ClosestEnemies(LookFrom, 640);

			if (enemies != null && enemies.Count > 0)
            {
				float bulletspeed = 8f;
				NPC target = enemies[0];
				Vector2 dist;// = target.Center - LookFrom;

				//Vector3 aimpos = SGAUtils.PredictAimingPos(LookFrom.ToVector3(), target.Center.ToVector3(), target.velocity.ToVector3(), bulletspeed, 0f);

				Vector2 offset = dotRotation.RotatedBy(MathHelper.PiOver2 * (projectile.spriteDirection)) * -6f;
				dist = SGAUtils.PredictiveAim(bulletspeed*4f, LookFrom, target.Center, target.velocity, false)- (LookFrom+ offset);

				float toRotation = dist.ToRotation();
				projectile.netUpdate = true;

				aimTo = toRotation;

				if (Vector2.Dot(dotRotation, Vector2.Normalize(dist)) > 0.98f && projectile.localAI[1]<1)
                {
					projectile.localAI[1] = 1;
					Projectile proj = Projectile.NewProjectileDirect(LookFrom+ offset + dotRotation * 24f, dotRotation* bulletspeed, ModContent.ProjectileType<EngineerSentryShotProj>(),projectile.damage,projectile.knockBack+2,projectile.owner);
					proj.rotation = proj.velocity.ToRotation();
					Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/RoR2sndTurretFire").WithVolume(0.25f).WithPitchVariance(.25f), projectile.Center);
				}


			}

			projectile.rotation = projectile.rotation.AngleTowards(aimTo, 0.15f);

			if (projectile.localAI[1] > 0)//Firing animation
				projectile.localAI[1]++;

			if (projectile.localAI[1] > 30)//Firerate
            {
				projectile.localAI[1] = 0;
			}

			projectile.spriteDirection = dotRotation.X < 0 ? -1 : 1;

			if (Main.rand.Next(100) < 1)
            {
				projectile.ai[1] = Main.rand.NextFloat(-0.75f, 0.75f)+(projectile.rotation.ToRotationVector2().X>0 ? 0 : MathHelper.Pi);
				projectile.netUpdate = true;
			}

		}

		public virtual void DrawTurret(SpriteBatch spriteBatch, Color lightColor,Vector2 offset = default)
        {
			float alpha = MathHelper.Clamp(projectile.localAI[0] / 30f, 0f, 1f);
			Texture2D turretTex = ModContent.GetTexture("SGAmod/Items/Weapons/Technical/EngineerSentryProj");
			Texture2D baseTex = ModContent.GetTexture("SGAmod/Items/Weapons/Technical/EngineerSentryStand");
			Texture2D glowTex = ModContent.GetTexture("SGAmod/Items/GlowMasks/EngineerSentryProjGlow");
			Vector2 offset2 = offset == default ? Vector2.Zero : offset;

			int frame = (int)(projectile.localAI[1] / 3);
			if (frame > 4)
				frame = 0;

			Vector2 turretOrig = new Vector2(18, projectile.spriteDirection>0 ? 14 : (turretTex.Height/5)-14);
			Rectangle turretFrame = new Rectangle(0, frame * (turretTex.Height / 5), turretTex.Height, turretTex.Height / 5);
			Vector2 scaleInAnimation = new Vector2(MathHelper.SmoothStep(0.5f,1f, MathHelper.Clamp((projectile.localAI[0]-10) / 20f, 0f, 1f)), MathHelper.SmoothStep(0.5f, 1f, MathHelper.Clamp((projectile.localAI[0]) / 20f, 0f, 1f)));
			float riseAnimation = MathHelper.SmoothStep(12f, 0f, MathHelper.Clamp((projectile.localAI[0]-40) / 60f, 0f, 1f));
			float anglelerp = MathHelper.PiOver2.AngleLerp(projectile.rotation, MathHelper.SmoothStep(0f, 1f, MathHelper.Clamp((projectile.localAI[0] - 90) / 25f, 0f, 1f)));


			spriteBatch.Draw(baseTex, projectile.Center+ offset2 - Main.screenPosition, null, lightColor * alpha, 0f, baseTex.Size() / 2f, projectile.scale* scaleInAnimation, SpriteEffects.None, 0f);

			for (int i = 0; i < 2; i += 1)
			{
				Texture2D tex = i == 0 ? turretTex : glowTex;
				Color glowColor = i == 0 ? lightColor : Color.White;
				spriteBatch.Draw(tex, LookFrom + offset2 + new Vector2(0, riseAnimation) - Main.screenPosition, turretFrame, glowColor * alpha, anglelerp, turretOrig, projectile.scale * new Vector2(1f, scaleInAnimation.X), projectile.spriteDirection < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
			}

		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			DrawTurret(spriteBatch, lightColor);
			return false;
		}
	}

	public class EngineerSentryProjHologram : EngineerSentryProj
	{
        public override string Texture => "SGAmod/Items/Weapons/Technical/EngineerSentryProj";
        public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("RoR2 Hologram Sentry");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.timeLeft = 2;
			projectile.aiStyle = -1;
			projectile.penetrate = 1;
		}

        public override void AI()
		{
			projectile.localAI[0] = 150;
			projectile.spriteDirection = Math.Sign(projectile.velocity.X);
			projectile.rotation = MathHelper.PiOver2 - (projectile.spriteDirection * MathHelper.PiOver2);
			projectile.position -= projectile.velocity;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			SGAmod.FadeInEffect.Parameters["fadeColor"].SetValue(((projectile.ai[1] > 0 ? Color.Aqua : Color.Red) * 1.0f).ToVector3());
			SGAmod.FadeInEffect.Parameters["alpha"].SetValue(0.30f+(float)Math.Sin(Main.GlobalTime*4f)*0.20f);



			for (float f = 0; f <= 8; f += 1)
			{
				SGAmod.FadeInEffect.CurrentTechnique.Passes[f>4 ? "LumaRecolorPass" : "LumaRecolorAlphaPass"].Apply();
				Vector2 randomizer = Main.rand.Next(100) < 1 ? Main.rand.NextVector2Circular(8,8) : default;
				DrawTurret(spriteBatch, (projectile.ai[1] > 0 ? Color.Aqua : Color.Red) * 1.0f, randomizer);
			}

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);


			return false;
		}

	}

		public class EngineerSentryShotProj : NPCs.Hellion.HellionCorePlasmaAttack
    {
		public override Color Color => Color.White*0.20f;
		public override Color Color2 => Color.Lime*0.50f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Engie Plasma");
		}
		public override string Texture => "SGAmod/Items/Weapons/Technical/EngineerSentryShotProj";

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.CloneDefaults(ProjectileID.ImpFireball);

			projectile.aiStyle = -1;
			projectile.tileCollide = false;
			projectile.extraUpdates = 3;
			projectile.timeLeft = 300;
			projectile.penetrate = 1;
			projectile.width = 12;
			projectile.height = 12;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 6;
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
			projectile.localAI[0] = 100;
		}

        public override bool PreKill(int timeLeft)
        {
            return base.PreKill(timeLeft);
        }

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			//hjkkj
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.penetrate = 10000;
		}

		public override bool CanDamage()
		{
			return projectile.penetrate < 10;
		}

		public override void AI()
		{
			projectile.localAI[0] += 1;
			projectile.ai[0] += 1;

			if (projectile.penetrate > 10)
            {
				projectile.timeLeft = Math.Min(projectile.timeLeft, 90);
				projectile.timeLeft -= 3;

			}

			projectile.rotation = projectile.velocity.ToRotation();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			base.PreDraw(spriteBatch, lightColor);
			Texture2D texture = Main.projectileTexture[projectile.type];

			float alpha = MathHelper.Clamp(projectile.localAI[0] / 30f, 0f, 1f);

			float timeLeft = Math.Min(projectile.timeLeft / 90f, 1f)* alpha;

			float maxtrail2 = (float)(projectile.oldPos.Length - 1f)/2f;

			Vector2 drawOrigin2 = texture.Size() / 2f;

			float detail = 1f;

			for (float f = maxtrail2; f > 1; f -= 0.5f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				float alphaShot = 1f-((f-1f) / maxtrail2);
				spriteBatch.Draw(texture, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color.White* timeLeft * alphaShot*0.25f, rot + MathHelper.PiOver2, drawOrigin2, projectile.scale* alphaShot, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White * timeLeft, projectile.rotation + MathHelper.PiOver2, drawOrigin2, projectile.scale, SpriteEffects.None, 0f);

			return false;
			/*
			Texture2D texture = Main.projectileTexture[projectile.type];
			Texture2D textureGlow = ModContent.GetTexture("SGAmod/Glow");

			float realVelocity = (MathHelper.Clamp((projectile.ai[0] - 300) / 300, 0f, 1f));
			float realAlpha = MathHelper.Clamp(projectile.localAI[0] / 300, 0f, 1f);
			float timeLeft = Math.Min(projectile.timeLeft / 50f, 1f);

			Vector2 drawOrigin2 = texture.Size() / 2f;
			Vector2 drawOrigin3 = textureGlow.Size() / 2f;

			//Color color = Color.White;
			//Color color2 = Color.Purple;

			float alpha = MathHelper.Clamp(projectile.localAI[0] / 30f, 0f, 1f);

			float scale = 2f - MathHelper.Clamp(projectile.localAI[0] / 70f, 0f, 1f);

			float scaledpre = MathHelper.Clamp((projectile.localAI[0] - 20) / 45f, 0f, 1f) * scale;

			float detail = 1f;// + projectile.velocity.Length();

			float maxtrail = (float)(projectile.oldPos.Length - 15f);
			float maxtrail2 = (float)(projectile.oldPos.Length - 1f);

			for (float f = maxtrail2; f >= 3f; f -= 0.25f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				spriteBatch.Draw(texture, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color * timeLeft * (1f / detail) * (1f - (f / maxtrail2)) * alpha, rot + MathHelper.PiOver2, drawOrigin2, new Vector2(0.2f, 2.0f) * scaledpre, SpriteEffects.None, 0f);
			}

			for (float f = maxtrail; f >= 1f; f -= 0.5f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				spriteBatch.Draw(texture, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color * timeLeft * (1f / detail) * (1f - (f / maxtrail)) * alpha, rot + MathHelper.PiOver2, drawOrigin2, scaledpre, SpriteEffects.None, 0f);
			}
			for (float f = maxtrail; f >= 1f; f -= 0.5f)
			{
				Vector2 pos = Vector2.Lerp(projectile.oldPos[(int)f - 1], projectile.oldPos[(int)f], f % 1f);
				float rot = projectile.oldRot[(int)f - 1];
				spriteBatch.Draw(textureGlow, pos + (projectile.Hitbox.Size() / 2f) - Main.screenPosition, null, Color2 * timeLeft * (0.75f / detail) * (1f - (f / maxtrail)) * alpha, rot + MathHelper.PiOver2, drawOrigin3, new Vector2(0.4f, 1.0f) * scaledpre, SpriteEffects.None, 0f);
			}

			return false;
			*/
		}
	}


}
