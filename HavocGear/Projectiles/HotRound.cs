using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.HavocGear.Projectiles
{

	public class HotRound : ModProjectile
	{

		public int stickin=-1;
		public Player P;
		public Vector2 offset;
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
			projectile.hostile=false;
			projectile.friendly=true;
			projectile.tileCollide = true;
			projectile.penetrate = 200;
			projectile.magic = true;
			projectile.timeLeft = 4*60;
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
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/10f), 50, Main.hslToRgb(0.4f, 0f, 0.95f), 2.75f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
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
					foundsticker = true;
					projectile.Kill();
					break;
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

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
			for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"), 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.95f), 0.9f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}
			if (stickin > -1)
			{
				NPC himz = Main.npc[stickin];
				projectile.tileCollide = false;

				if (himz != null && himz.active)
				{
					projectile.Center = (himz.Center-offset) - (projectile.velocity*0.2f);
					himz.AddBuff(BuffID.OnFire, 3);
				}
				else
				{
					projectile.Kill();

				}

			}

		}

	}
}