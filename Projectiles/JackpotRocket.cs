using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{

	public class JackpotRocket : ModProjectile
	{

		double keepspeed=0.0;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jackpot Rocket");
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
			projectile.ranged = true;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		target.immune[projectile.owner] = 15;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type=ProjectileID.RocketI;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
			for (int num315 = 0; num315 < 40; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X-1, projectile.position.Y)+positiondust, projectile.width, projectile.height, 31, projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/15f), 50, Main.hslToRgb(0.15f, 1f, 1.00f), 2.5f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
		int [] types = {ProjectileID.CopperCoin,ProjectileID.SilverCoin,mod.ProjectileType("FallingGoldCoin"),ProjectileID.PlatinumCoin};
			for (int num315 = 1; num315 < 13; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				float velincrease=((float)(num315+8)/2f);
                int thisone=Projectile.NewProjectile(projectile.Center.X-projectile.velocity.X, projectile.Center.Y-projectile.velocity.Y, randomcircle.X*velincrease, randomcircle.Y*velincrease, types[2], (int)(projectile.damage*0.25), projectile.knockBack, projectile.owner, 0.0f, 0f);
                Main.projectile[thisone].ranged=true;
                Main.projectile[thisone].friendly=projectile.friendly;
                Main.projectile[thisone].hostile=projectile.hostile;
                IdgProjectile.AddOnHitBuff(thisone,BuffID.Midas,60*10);
				Main.projectile[thisone].netUpdate = true;
				IdgProjectile.Sync(thisone);
            }

            int theproj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("Explosion"), (int)((double)projectile.damage * 0.75f), projectile.knockBack, projectile.owner, 0f, 0f);
			Main.projectile[theproj].ranged=true;
			IdgProjectile.AddOnHitBuff(theproj,BuffID.Midas,60*10);

			return true;
		}

		public override void AI()
		{
		Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
		for (int num315 = 0; num315 < 3; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(projectile.position.X-1, projectile.position.Y)+positiondust, projectile.width, projectile.height, mod.DustType("HotDust"), 0f, 0f, 50, Main.hslToRgb(0.10f, 0.5f, 0.75f), 0.8f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity = (-projectile.velocity)+(randomcircle*(0.5f))*((float)num315/3f);
				dust3.velocity.Normalize();
			}

		for (int num315 = 1; num315 < 16; num315 = num315 + 1)
			{
				if (Main.rand.Next(0,100)<25){
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(projectile.position.X-1, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 50,Main.hslToRgb(0.15f, 1f, 1.00f), 0.33f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity = (randomcircle*2.5f*Main.rand.NextFloat())+(projectile.velocity);
				dust3.velocity.Normalize();
			}}

		projectile.ai[0]=projectile.ai[0]+1;
		projectile.velocity.Y+=0.1f;
		projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f; 
		}


	}

	public class FallingGoldCoin : ModProjectile
	{

		int fakeid=ProjectileID.GoldCoin;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gold Coin");
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type=fakeid;
			return true;
		}

		public override void AI()
		{
			projectile.velocity.Y+=0.2f;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

	}

}