using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{

	public class UnmanedBolt : ModProjectile
	{

		double keepspeed=0.0;
		float homing=0.05f;
		public float beginhoming=20f;
		public Player P;
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
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 15; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("NovusSparkle"), projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/15f), 50, Main.hslToRgb(0.4f, 0f, 0.15f), 2.4f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
			}
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		if (!target.friendly)
		projectile.Kill();
		}

		public override void AI()
		{
		for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("NovusSparkle"), 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Main.dust[num316].noGravity = true;
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
			}

		projectile.ai[0]=projectile.ai[0]+1;
		if (projectile.ai[0]<2){
		keepspeed=(projectile.velocity).Length();
		}
		NPC target=Main.npc[Idglib.FindClosestTarget(0,projectile.Center,new Vector2(0f,0f),true,true,true,projectile)];
		if (target!=null){
		if ((target.Center-projectile.Center).Length()<500f){
		if (projectile.ai[0]<(250f) && projectile.ai[0]>(beginhoming)){
projectile.velocity=projectile.velocity+(projectile.DirectionTo(target.Center)*((float)keepspeed*homing));
projectile.velocity.Normalize();
projectile.velocity=projectile.velocity*(float)keepspeed;
}}

if (projectile.ai[0]>250f){
projectile.Kill();
}




}



		}

	}
}