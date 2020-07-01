using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Projectiles
{
	public class ContagionProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Contagion");
		}

		public override void SetDefaults()
		{
			projectile.width = 42;
			projectile.height = 42;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.tileCollide = false;
			projectile.ignoreWater = true;
			projectile.penetrate = -1;
			projectile.ownerHitCheck = true;
			projectile.aiStyle = 19;
			projectile.melee = true;  
			projectile.timeLeft = 90;
			projectile.hide = true;
		}

		public override void AI()
		{
			Main.player[projectile.owner].direction = projectile.direction;
			Main.player[projectile.owner].heldProj = projectile.whoAmI;
			Main.player[projectile.owner].itemTime = Main.player[projectile.owner].itemAnimation;
			projectile.position.X = Main.player[projectile.owner].position.X + (float)(Main.player[projectile.owner].width / 2) - (float)(projectile.width / 2);
			projectile.position.Y = Main.player[projectile.owner].position.Y + (float)(Main.player[projectile.owner].height / 2) - (float)(projectile.height / 2);
			projectile.position += projectile.velocity * projectile.ai[0];
			if (Main.rand.Next(4) == 0)
			{
				int dustIndex = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, ModContent.DustType<Dusts.AcidDust>(), 0f, 0f, 254, default(Color), 0.3f);
				Main.dust[dustIndex].velocity += projectile.velocity * 0.5f;
				Main.dust[dustIndex].velocity *= 0.5f;
				return;
			}
			if(projectile.ai[0] == 0f)
			{
				projectile.ai[0] = 3f;
				projectile.netUpdate = true;
			}
			if(Main.player[projectile.owner].itemAnimation < Main.player[projectile.owner].itemAnimationMax / 3)
			{
				projectile.ai[0] -= 2.4f;
				if (projectile.localAI[0] == 0f && Main.myPlayer == projectile.owner)
				{
					projectile.localAI[0] = 1f;
					Projectile.NewProjectile(projectile.Center.X + projectile.velocity.X, projectile.Center.Y + projectile.velocity.Y, projectile.velocity.X * 1f, projectile.velocity.Y * 1f, mod.ProjectileType("AcidSpear"), (int)((double)projectile.damage * 0.5), projectile.knockBack * 0.85f, projectile.owner, 0f, 0f);
				}
			}
			else
			{
				projectile.ai[0] += 0.95f;
			}
			
			if(Main.player[projectile.owner].itemAnimation == 0)
			{
				projectile.Kill();
			}
			
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 2.355f;
			if(projectile.spriteDirection == -1)
			{
				projectile.rotation -= 1.57f;
			}
			Lighting.AddLight(projectile.position, 0.3f, 0.5f, 0f);
		}
    }
}