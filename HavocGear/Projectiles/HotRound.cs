using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.HavocGear.Projectiles
{
	//Give explosion on expire
	public class HotRound : ModProjectile
	{

		public int stickin = -1;
		public Player P;
		public Vector2 offset;
		public float boomGlow = 0f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orb thingy from Asterism");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = 200;
			projectile.magic = true;
			projectile.timeLeft = 4 * 60;
			projectile.scale = 0.75f;
			aiType = 0;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(stickin);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			stickin = reader.ReadInt32();
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 20; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 10f), 50, Main.hslToRgb(0.4f, 0f, 0.95f), 1f);
				Main.dust[num316].noGravity = true;
				Vector2 velop = new Vector2(projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 10f));
				Main.dust[num316].velocity = velop / 8f;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.75f;
			}

			if (timeLeft < 2 && stickin>=0)
			{
				var snd = Main.PlaySound(SoundID.Item14, projectile.Center);
				if (snd != null)
				{
					snd.Pitch = 0.75f;
					if (SGAmod.ScreenShake < 20)
					SGAmod.AddScreenShake(16, 1280, projectile.Center);
				}

				Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 2f, ModContent.ProjectileType<HeatedBlowBackShot>(), projectile.damage * 3, projectile.knockBack * 3f, projectile.owner);

				for (float gg = 1f; gg < 7.26f; gg += 0.25f)
				{
					Vector2 velo = Main.rand.NextVector2CircularEdge(gg, gg) * 2f;
					Gore.NewGore(projectile.Center + new Vector2(0, 0), velo, Main.rand.Next(61, 64), (5f - Math.Abs(gg)) / 3f);

					velo = Main.rand.NextVector2CircularEdge(gg, gg);
					int gorer = Gore.NewGore(projectile.Center + new Vector2(0, 0), Vector2.Zero, Main.rand.Next(61, 64), (5f - Math.Abs(gg)) / 3f);
					if (gorer >= 0)
						Main.gore[gorer].velocity = (Vector2.Normalize(projectile.velocity) * ((gg * 2f) + 3f)) + (velo / 1f);

				}
			}

			if (stickin > -1)
			{
				NPC himz = Main.npc[stickin];
				if (himz != null && himz.active)
				{
					himz.AddBuff(mod.BuffType("ThermalBlaze"), 60 * 3);
				}
			}
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.penetrate < 100)
				return false;
			return base.CanHitNPC(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			bool foundsticker = false;
			target.immune[Main.player[projectile.owner].whoAmI] = 1;
			int numfound = 0;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (i != projectile.whoAmI // Make sure the looped projectile is not the current javelin
					&& currentProjectile.active // Make sure the projectile is active
					&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
					&& currentProjectile.type == projectile.type // Make sure the projectile is of the same type as this javelin
					&& currentProjectile.modProjectile is HotRound HoterRound // Use a pattern match cast so we can access the projectile like an ExampleJavelinProjectile
					&& HoterRound.stickin == target.whoAmI)
				{
					numfound += 1;
					if (numfound > 2)
					{
						foundsticker = true;
						projectile.Kill();
						break;
					}
				}

			}

			if (!foundsticker)
			{

				if (projectile.penetrate > 1)
				{
					projectile.penetrate = 50;
					offset = (target.Center - projectile.Center);
					stickin = target.whoAmI;
					projectile.netUpdate = true;
				}
			}
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.Orange.ToVector3() * 0.75f);

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

			if (stickin > -1)
			{
				boomGlow = (2.75f - boomGlow)/(1f+(projectile.timeLeft/30f));
				boomGlow = Math.Min(boomGlow, 1f);

				NPC himz = Main.npc[stickin];
				projectile.tileCollide = false;

				if (himz != null && himz.active)
				{
					projectile.Center = (himz.Center - offset) - (projectile.velocity * 0.2f);
					if (GetType() == typeof(HotRound))
					himz.AddBuff(BuffID.OnFire, 3);
				}
				else
				{
					projectile.Kill();
				}

				for (int num315 = 0; num315 < 3; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y) + Vector2.Normalize(projectile.velocity) * Main.rand.NextFloat(0f, 32f), projectile.width, projectile.height, mod.DustType("HotDust"), 0,0, 50, Main.hslToRgb(0.4f, 0f, 0.95f), boomGlow/2f);
					Main.dust[num316].noGravity = true;
					Vector2 velop = new Vector2(projectile.velocity.X + (float)(Main.rand.Next(-250, 250) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-250, 250) / 10f));
					Main.dust[num316].velocity = velop / 8f;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.75f;
				}

			}
            else
            {
				for (int num315 = 0; num315 < 2; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.95f), 0.9f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.3f;
				}
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if (stickin >= 0)
			{
				Texture2D tex = Main.projectileTexture[projectile.type];
				Texture2D tex2 = ModContent.GetTexture("SGAmod/Glow");


				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.OrangeRed*boomGlow, projectile.rotation, tex2.Size() / 2f, projectile.scale, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

				Effect fadeIn = SGAmod.FadeInEffect;

				fadeIn.Parameters["alpha"].SetValue(boomGlow);
				fadeIn.Parameters["strength"].SetValue(boomGlow);
				fadeIn.Parameters["fadeColor"].SetValue(Color.Orange.ToVector3());
				fadeIn.Parameters["blendColor"].SetValue(Color.White.ToVector3());

				fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null,Color.White, projectile.rotation, tex.Size()/2f, projectile.scale, default, 0);

				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			}
		}

	}

	public class HeatedBlowBackShot : Dimensions.NPCs.SpaceBossBasicShot, IDrawAdditive
	{
        protected override Color BoomColor => Color.Lerp(Color.Red, Color.Orange, projectile.timeLeft/120f);
		public override bool DrawFlash => true;
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blow Back Shot");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.timeLeft = 120;
			projectile.light = 0.75f;
			projectile.penetrate = -1;
			projectile.localNPCHitCooldown = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.magic = true;
			projectile.extraUpdates = 5;
		}

		public override void AI()
		{
			projectile.localAI[0] += 1;

			if (projectile.localAI[0] == 1)
			{
				startOrg = projectile.Center;
			}

			if (projectile.ai[0] < 15f)
			{
				projectile.ai[0] += 0.05f;
			}
		}

        public override bool PreKill(int timeLeft)
        {
			return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
			target.AddBuff(mod.BuffType("ThermalBlaze"), 60 * 3);
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			return false;
		}

	}

}