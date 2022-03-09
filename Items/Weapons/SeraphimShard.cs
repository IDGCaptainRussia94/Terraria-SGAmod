using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using SGAmod.Effects;
using Microsoft.Xna.Framework.Audio;
using Idglibrary;

namespace SGAmod.Items.Weapons
{

	public class SeraphimShard : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraphim Shard");
			Tooltip.SetDefault("'Some things are better left forgotten...'\nThis shard summons powerful copies to protect its user\n"+ Idglib.ColorText(Color.Red, "At a cost, you lose 50 max life per active shard\nSummon can't be used if life dropped exceeds max vanilla health"));
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 100;
			item.knockBack = 4f;
			item.mana = 25;
			item.width = 32;
			item.height = 32;
			item.useTime = 36;
			item.useAnimation = 36;
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.value = Item.buyPrice(0, 15, 0, 0);
			item.rare = ItemRarityID.LightPurple;
			item.noUseGraphic = true;
			//item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<SeraphimShardBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType< SeraphimShardProjectile>();
			/*if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];

				item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle, Color glowcolor)
				{
					Texture2D texture = Main.itemTexture[item.type];
					Vector2 origin = texture.Size() / 2f;
					float timeAdvance = drawInfo.drawPlayer.itemAnimation/drawInfo.drawPlayer.itemAnimationMax;
					angle = drawInfo.drawPlayer.itemRotation + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
					Player drawPlayer = drawInfo.drawPlayer;

					Vector2 drawHere = drawPlayer.MountedCenter + new Vector2(drawPlayer.direction * 16f, drawPlayer.gravDir * -26f) - Main.screenPosition;

					DrawData value = new DrawData(texture, drawHere, null, Color.White * 0.75f * MathHelper.Clamp(1f-timeAdvance, 0f, 1f), 0, origin, timeAdvance*2f, drawInfo.spriteEffects, 0);
					//Main.playerDrawData.Add(value);

				};
			}*/
		}

        public override bool CanUseItem(Player player)
        {
			int count = Main.projectile.Where(currentProjectile => currentProjectile.active
	&& currentProjectile.owner == Main.myPlayer
	&& currentProjectile.type == item.shoot).ToList().Count;

			return player.statLifeMax>100+(count*50);
        }

        public override Color? GetAlpha(Color lightColor)
        {
			return Main.hslToRgb((Main.GlobalTime / 4f) % 1f, 1f, 0.85f)*0.75f;
        }
        public override string Texture
		{
			get { return ("SGAmod/Projectiles/SeraphimShardProjectile"); }
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position = player.MountedCenter + new Vector2(player.direction * 16f, -player.gravDir * 26f);
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);
			SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_BetsySummon, (int)position.X, (int)position.Y);
			if (sound != null)
				sound.Pitch += 0.50f;

			return true;
		}

	}

	public class SeraphimShardProjectile : ModProjectile,IDrawAdditive
	{
		float startupDelay => MathHelper.Clamp((projectile.localAI[0] - 75f) / 150f, 0f, 1f);
		Color prismColor => Main.hslToRgb(((projectile.whoAmI / 255f) * 100f) % 1f, 1f, 0.85f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seraphim Shard");
			Main.projFrames[projectile.type] = 1;
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;
			Main.projPet[projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			ProjectileID.Sets.Homing[projectile.type] = true;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public sealed override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.minion = true;
			projectile.minionSlots = 1f;
			projectile.penetrate = -1;
			projectile.localNPCHitCooldown = 60;
			projectile.usesLocalNPCImmunity = true;
			projectile.timeLeft = 60;
			projectile.extraUpdates = 1;
		}
		public override bool CanDamage() => projectile.ai[0]>0;
		public override bool MinionContactDamage() => projectile.ai[0] > 0;
		float SeekDist => 720f;
		float AttackDist => 280f;
		float HoverDist => 128f;
		float us = 0f;
		float maxus = 0f;
		Player player;
		private void DoAttack(NPC enemy)
        {
			int attackperiod = 32;
			int attackperiod2 = 48+(int)maxus*2;
			int delay = (int)((us / maxus) * attackperiod2);


			if (projectile.ai[0] < -45 && projectile.Distance(enemy.Center)<AttackDist && (Main.player[projectile.owner].SGAPly().timer+delay) % attackperiod2 == 0)
            {
				projectile.ai[0] = attackperiod;
				projectile.localAI[1] = 0;

			}
			if (projectile.ai[0] > 0)
			{
				if (projectile.ai[0] < 2)
                {
					Vector2 dotProduct = enemy.Center - projectile.Center;
					if (Vector2.Dot(Vector2.Normalize(dotProduct), Vector2.Normalize(projectile.velocity)) > .50f && Collision.CanHitLine(enemy.Center, 1, 1, projectile.Center, 1, 1))
					projectile.ai[0] = 2;
				}

				projectile.velocity *= 0.99f;

				if (projectile.ai[0] == attackperiod-10)
				{
					SoundEffectInstance sound = Main.PlaySound(SoundID.DD2_GhastlyGlaivePierce, (int)projectile.Center.X, (int)projectile.Center.Y);
					if (sound != null)
						sound.Pitch += 0.50f;
				}

				if (projectile.ai[0] > attackperiod-10)
				{
					projectile.rotation = projectile.rotation.AngleTowards((enemy.Center - projectile.Center).ToRotation()+MathHelper.PiOver2, 3.5f);
                }
                else
                {
					projectile.rotation = projectile.rotation.AngleTowards((enemy.Center - projectile.Center).ToRotation() + MathHelper.PiOver2, 0.1f);

					projectile.velocity += (projectile.rotation- MathHelper.PiOver2).ToRotationVector2();
					if (projectile.velocity.Length() > 8f + enemy.velocity.Length())
                    {
						projectile.velocity = Vector2.Normalize(projectile.velocity) * (8f + enemy.velocity.Length());
					}

				}

				projectile.localAI[1] = Math.Min(projectile.localAI[1]+2.50f,100f);
			}

		}

		public override bool Autoload(ref string name)
		{
			SGAPlayer.PostUpdateEquipsEvent += PostUpdatePlayer;
			return true;
		}

		public void PostUpdatePlayer(SGAPlayer sgaplayer)
        {
			int count = Main.projectile.Where(currentProjectile => currentProjectile.active
				&& currentProjectile.owner == Main.myPlayer
				&& currentProjectile.type == projectile.type).ToList().Count;
			//sgaplayer.player.statManaMax2 -= count * 50;
			sgaplayer.player.statLifeMax2 -= count * 50;
		}

		public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			player = Main.player[projectile.owner];
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<SeraphimShardBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<SeraphimShardBuff>()))
			{
				projectile.timeLeft = 2;
			}

			bool toplayer = true;
			Vector2 gothere = player.Center+new Vector2(player.direction*-32,0);
			projectile.localAI[0] += 1;
			projectile.localAI[1] -= 1;
			if (startupDelay < 0)
            {
				projectile.ai[0] = 0;
			}
			projectile.ai[0] -= 1;

			List<NPC> closestnpcs = new List<NPC>();
			
			for(int i = 0; i < Main.maxNPCs; i += 1)
			{
				//bool colcheck= Collision.CheckAABBvLineCollision(Main.npc[i].position, new Vector2(Main.npc[i].width, Main.npc[i].height), Main.npc[i].Center,projectile.Center)
				//	&& Collision.CanHitLine(Main.npc[i].Center,0,0, projectile.Center,0,0);

				if (Main.npc[i].active && !Main.npc[i].friendly && !Main.npc[i].townNPC && !Main.npc[i].dontTakeDamage && Main.npc[i].CanBeChasedBy() &&
					Collision.CheckAABBvLineCollision(Main.npc[i].position, new Vector2(Main.npc[i].width, Main.npc[i].height), Main.npc[i].Center, projectile.Center)
					&& Collision.CanHitLine(Main.npc[i].Center, 0, 0, projectile.Center, 0, 0)
					&& (Main.npc[i].Center-player.Center).Length()< SeekDist)
					closestnpcs.Add(Main.npc[i]);
			}

			//int it=player.grappling.OrderBy(n => (Main.projectile[n].active ? 0 : 999999) + Main.projectile[n].timeLeft).ToArray()[0];
			NPC them = closestnpcs.Count<1 ? null : closestnpcs.ToArray().OrderBy(npc => projectile.Distance(npc.Center)).ToList()[0];
			NPC oldthem = null;

			if (player.HasMinionAttackTargetNPC)
			{
				oldthem = them;
				them = Main.npc[player.MinionAttackTargetNPC];
				//gothere = them.Center + new Vector2(them.direction * 96, them.direction==0 ? -96 : 0);
			}

			if (them != null && them.active)
			{
				toplayer = false;
				//if (!player.HasMinionAttackTargetNPC)
				gothere = them.Center + Vector2.Normalize(projectile.Center- them.Center) * HoverDist;
			}
			maxus = 0;
			us = 0;
			for (int i = 0; i < Main.maxProjectiles; i++)
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active
				&& currentProjectile.owner == Main.myPlayer
				&& currentProjectile.type == projectile.type)
				{
					if (i == projectile.whoAmI)
						us = maxus;
					maxus += 1f;
				}
			}
			Vector2 there = player.Center;

			int timer = player.SGAPly().timer * 2;
			double angles = MathHelper.ToRadians(((float)((us / maxus) * 360.00) - 90f)+ timer);
			float dist = 16f;//Main.rand.NextFloat(54f, 96f);
			float aval = (float)timer+ (us*83f);
			Vector2 here;
			if (!toplayer && startupDelay>0)
			{
				here = (new Vector2((float)Math.Sin(aval / 60f) * 6f, 20f * ((float)Math.Sin(aval / 70f)))).RotatedBy((them.Center - gothere).ToRotation());
				if (projectile.ai[0]<1)
				projectile.rotation = projectile.rotation.AngleTowards(0, 0.1f);

				DoAttack(them);
			}
			else
			{
				float anglz = (float)(Math.Cos(MathHelper.ToRadians(aval)) * player.direction) / 4f;
				if (startupDelay>0)
				projectile.rotation = projectile.rotation.AngleTowards(((player.direction * 45) + anglz), 0.05f);
				//gothere -= (Vector2.UnitX * player.direction) * maxus;
				here = new Vector2((float)Math.Cos(angles) / 2f, (float)Math.Sin(angles)) * dist;
			}

			if (projectile.ai[0] < -15)
			{

				Vector2 where = gothere + here;
				Vector2 difference = where - projectile.Center;

				if ((where - projectile.Center).Length() > 0f)
				{
					if (toplayer)
					{
						projectile.velocity += (where - projectile.Center) * 0.25f;
						projectile.velocity *= 0.725f;
					}
					else
					{
						projectile.velocity += (where - projectile.Center) * 0.005f;
						projectile.velocity *= 0.925f;
					}
				}

				float maxspeed = Math.Min(projectile.velocity.Length(), 12 + (toplayer ? player.velocity.Length() : 0)) * startupDelay;
				projectile.velocity.Normalize();
				projectile.velocity *= maxspeed;
			}

			Lighting.AddLight(projectile.Center, prismColor.ToVector3() * 0.78f);

		}

		public override string Texture
		{
			get { return ("SGAmod/Projectiles/SeraphimShardProjectile"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Player player = Main.player[projectile.owner];

			float angleadd = MathHelper.Clamp(-projectile.ai[0] / 45f, 0f, 1f);
			float velAdd = angleadd*(projectile.velocity.X) * 0.07f;

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;
			Vector2 drawPos = projectile.Center - Main.screenPosition;
			Color color = Color.Lerp((projectile.GetAlpha(lightColor) * 0.5f), prismColor, 0.75f);

			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("noise"));
			trail.color = delegate (float percent)
			{
				return color;
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.strength = startupDelay;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			Texture2D tex2 = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];

			if (projectile.localAI[1] > 0)
			{
				for (int i = projectile.oldPos.Length-1; i > 0; i -= 1)
				{
					spriteBatch.Draw(tex2, projectile.oldPos[i]+(new Vector2(projectile.width,projectile.height)/2f) - Main.screenPosition, null, (prismColor* 0.50f) * MathHelper.Clamp(projectile.localAI[1] / 75f, 0f, 1f) * (1f-(i / (float)projectile.oldPos.Length)), velAdd+projectile.rotation + MathHelper.Pi, tex2.Size() / 2f, projectile.scale, default, 0);
				}
			}

			Vector2 drawPos2 = projectile.Center - Main.screenPosition;
			spriteBatch.Draw(tex2, drawPos2, null, prismColor * 1f, velAdd + projectile.rotation + MathHelper.Pi, tex2.Size() / 2f, projectile.scale, default, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect hallowed = SGAmod.HallowedEffect;

			hallowed.Parameters["alpha"].SetValue(MathHelper.Clamp((projectile.localAI[0]) / 90f, 0f, 1f));
			hallowed.Parameters["prismAlpha"].SetValue(1f);
			hallowed.Parameters["prismColor"].SetValue(color.ToVector3());
			hallowed.Parameters["overlayTexture"].SetValue(mod.GetTexture("Perlin"));
			float perc = ((projectile.whoAmI / 255f) * 100f) % 1f;
			hallowed.Parameters["overlayProgress"].SetValue(new Vector3(0, Main.GlobalTime / 4f, (Main.GlobalTime / 5f)+ perc));
			hallowed.Parameters["overlayAlpha"].SetValue(0.75f);
			hallowed.Parameters["overlayStrength"].SetValue(new Vector3(1f,0f, 0f));
			hallowed.Parameters["overlayMinAlpha"].SetValue(0.25f);
			hallowed.Parameters["rainbowScale"].SetValue(0.25f);
			hallowed.Parameters["overlayScale"].SetValue(new Vector2(0.15f, 0.25f));
			hallowed.CurrentTechnique.Passes["Prism"].Apply();

			spriteBatch.Draw(tex, drawPos, null, Color.White, velAdd + projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);


			return false;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			Texture2D tex = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
			Vector2 drawPos = projectile.Center - Main.screenPosition;

			float angleadd = MathHelper.Clamp(1f - (projectile.ai[0] / 45f), 0f, 1f);
			float velAdd = angleadd * (projectile.velocity.X) * 0.07f;

			spriteBatch.Draw(tex, drawPos, null, prismColor * 0.25f, velAdd + projectile.rotation - MathHelper.Pi, tex.Size() / 2f, projectile.scale / 1.5f, default, 0);
		}

	}

	public class SeraphimShardBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Seraphim Shards");
			Description.SetDefault("'What's a Terraprisma?'");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}
		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/SeraphimShardBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<SeraphimShardProjectile>()] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}

}