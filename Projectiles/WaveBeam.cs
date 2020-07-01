using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{

	public class WaveBeam : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam");
		}

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile=false;
			projectile.friendly=true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
		Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 3f;

		if (projectile.ai[0]==0){
		projectile.ai[0]=1;
				int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("WaveProjectile"), projectile.damage, projectile.knockBack, projectile.owner);
				Main.projectile[proj].timeLeft=projectile.timeLeft;
				Main.projectile[proj].penetrate=1;
				Main.projectile[proj].ai[0]=(float)Math.PI;
				Main.projectile[proj].ai[1]=(float)projectile.whoAmI;

				proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X, projectile.velocity.Y, mod.ProjectileType("WaveProjectile"), projectile.damage, projectile.knockBack, projectile.owner);
				Main.projectile[proj].timeLeft=projectile.timeLeft;
				Main.projectile[proj].penetrate=1;
				Main.projectile[proj].ai[0]=(float)-Math.PI;
				Main.projectile[proj].ai[1]=(float)projectile.whoAmI;
				//IdgProjectile.AddOnHitBuff(proj,BuffID.OnFire,60*10);
		}






		}

	}

	public class WaveProjectile : ModProjectile
	{

		Vector2 facing;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile=false;
			projectile.friendly=true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreKill(int timeLeft)
		{

		for (int num315 = 0; num315 < 25; num315 = num315 + 1)
			{
					int num622 = Dust.NewDust(new Vector2(projectile.position.X-1,projectile.position.Y), projectile.width, projectile.height, 185, 0f, 0f, 100, default(Color), 0.75f);
					Main.dust[num622].velocity *= 1f;

					Main.dust[num622].noGravity = true;
					Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTime/5)%1, 0.9f, 1f);
					Main.dust[num622].color.A=10;
					Main.dust[num622].velocity.X = facing.X + (Main.rand.Next(-250, 251) * 0.025f);
					Main.dust[num622].velocity.Y = facing.Y + (Main.rand.Next(-250, 251) * 0.025f);
					Main.dust[num622].alpha = 200;
			}

			return true;
		}

		public override void AI()
		{
		Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 3f;
		projectile.ai[0]+=0.2f*(projectile.ai[0]>0 ? 1f : -1f);

		projectile.Center=Main.projectile[(int)projectile.ai[1]].Center;
		double angle=((double)projectile.ai[0])+ 2.0* Math.PI;

		float veladd2=(Main.projectile[(int)projectile.ai[1]].velocity).ToRotation()+(float)(Math.PI/2.0);
		Vector2 veladd=new Vector2((float)Math.Cos(veladd2),(float)Math.Sin(veladd2))*(64f*(float)Math.Sin(angle));


		projectile.velocity=Main.projectile[(int)projectile.ai[1]].velocity+(veladd);

		facing=Main.projectile[(int)projectile.ai[1]].velocity+(-veladd/12f);


		for (int num315 = 0; num315 < 1; num315 = num315 + 1)
			{
					int num622 = Dust.NewDust(new Vector2(projectile.position.X-1,projectile.position.Y)+projectile.velocity, projectile.width, projectile.height, 185, 0f, 0f, 100, default(Color), 0.75f);
					Main.dust[num622].velocity *= 1f;

					Main.dust[num622].noGravity = true;
					Main.dust[num622].color=Main.hslToRgb((float)(Main.GlobalTime/5)%1, 0.9f, 1f);
					Main.dust[num622].color.A=10;
					Main.dust[num622].velocity.X = facing.X/5 + (Main.rand.Next(-50, 51) * 0.025f);
					Main.dust[num622].velocity.Y = facing.Y/5 + (Main.rand.Next(-50, 51) * 0.025f);
					Main.dust[num622].alpha = 200;
			}

			projectile.rotation = (float)Math.Atan2((double)facing.Y, (double)facing.X) + 1.57f; 



		}


	}

}