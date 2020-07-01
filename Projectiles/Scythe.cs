using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using AAAAUThrowing;

namespace SGAmod.Projectiles
{

	public class Scythe : ModProjectile
	{

		float hittime = 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scythe");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 64;
			projectile.height = 64;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile=false;
			projectile.friendly=true;
			projectile.tileCollide = false;
			projectile.Throwing().thrown = true;
			projectile.timeLeft=120;
			projectile.penetrate = 12;
			aiType = 0;
			drawOriginOffsetX = 8;
			drawOriginOffsetY = -8;
		}

	public override bool? CanHitNPC(NPC target){
		if (projectile.penetrate<10)
		return false;
		else
		return base.CanHitNPC(target);
	}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		projectile.velocity*=-1f;
		target.immune[projectile.owner] = 5;
			hittime = 150f;
		//IdgNPC.AddBuffBypass(target.whoAmI,mod.BuffType("InfinityWarStormbreaker"),300);
		}

		public override void AI()
		{

			Lighting.AddLight(projectile.Center, Color.Aquamarine.ToVector3() * 0.5f);

			hittime = Math.Max(1f, hittime/1.5f);
;
			float dist2 = 64f;

			//Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
		for (double num315 = 0; num315 < Math.PI+0.04; num315 = num315 + Math.PI)
			{
				Vector2 thisloc = new Vector2((float)(Math.Cos((projectile.rotation+ Math.PI / 2.0) + num315) * dist2), (float)(Math.Sin((projectile.rotation+Math.PI/2.0) + num315) * dist2));
				int num316 = Dust.NewDust(new Vector2(projectile.position.X-1, projectile.position.Y)+ thisloc, projectile.width, projectile.height, mod.DustType("NovusSparkleBlue"), 0f, 0f, 50, Color.White, 1.5f);
				Main.dust[num316].noGravity = true;
				Main.dust[num316].velocity = thisloc/30f;
			}

		projectile.ai[0]=projectile.ai[0]+1;
		projectile.velocity.Y+=0.1f;
		if (projectile.ai[0]>14f && !Main.player[projectile.owner].dead){
		Vector2 dist=(Main.player[projectile.owner].Center-projectile.Center);
		Vector2 distnorm=dist; distnorm.Normalize();
		projectile.velocity+=distnorm*5f;
		projectile.velocity/=1.05f;
		//projectile.Center+=(dist*((float)(projectile.timeLeft-12)/28));
		if (dist.Length()<80)
		projectile.Kill();
		}

		NPC target=Main.npc[Idglib.FindClosestTarget(0,projectile.Center,new Vector2(0f,0f),true,true,true,projectile)];
		if (target!=null && projectile.penetrate>9){
		if ((target.Center-projectile.Center).Length()<500f){

projectile.Center+=(projectile.DirectionTo(target.Center)*(projectile.ai[0] > 14f ? (50f* Main.player[projectile.owner].thrownVelocity) / hittime : 12f));

}}

projectile.rotation += 0.38f;
}


		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texture = mod.GetTexture("Projectiles/ScytheGlow");
			Texture2D texture2 = mod.GetTexture("Projectiles/Scythe");
			spriteBatch.Draw(texture2, projectile.Center - Main.screenPosition, null, lightColor, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0f);
			spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.White, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}


	}

}