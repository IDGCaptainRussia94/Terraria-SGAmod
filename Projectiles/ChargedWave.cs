using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;

namespace SGAmod.Projectiles
{

	public class ChargedWave : ModProjectile
	{

		double keepspeed=0.0;
		float homing=0.04f;
		public float beginhoming=5f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Charged Beamshot");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
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
			for (int num315 = 0; num315 < 60; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, projectile.velocity.X+(float)(Main.rand.Next(-250,250)/15f), projectile.velocity.Y+(float)(Main.rand.Next(-250,250)/15f), 50, default(Color), 3.4f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.7f;
				dust3.noGravity = true;
				dust3.scale = 2.0f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}
			return true;
		}

		public override void ModifyHitNPC (NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
		if (Main.rand.Next(0,100)<50)
		crit=true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		if (!target.friendly && target.realLife<0){
		int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0f, 0f, mod.ProjectileType("WaveBeamStun"), damage, knockback, Main.player[projectile.owner].whoAmI);
		Main.projectile[proj].timeLeft=300;
		HalfVector2 half=new HalfVector2(target.position.X,target.position.Y);
		Main.projectile[proj].ai[0]=ReLogic.Utilities.ReinterpretCast.UIntAsFloat(half.PackedValue);
		Main.projectile[proj].ai[1]=target.whoAmI;
		if (target.boss)
		Main.projectile[proj].timeLeft=80;
		Main.projectile[proj].netUpdate=true;
		projectile.Kill();
		}}

		public override void AI()
		{
		for (int num315 = 0; num315 < 2; num315 = num315 + 1)
			{
				int num316 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 226, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity *= 0.3f;
				dust3.noGravity = true;
				dust3.scale = 1.8f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
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




}

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;



		}

	}

	public class WaveBeamStun : ModProjectile
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stunning shot");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
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

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void AI()
		{

		NPC enemy=Main.npc[(int)projectile.ai[1]];
		if (enemy==null)
		projectile.Kill();
		if (enemy.active==false)
		projectile.Kill();

		Vector2 lockhere = new HalfVector2() { PackedValue = ReLogic.Utilities.ReinterpretCast.FloatAsUInt(projectile.ai[0]) }.ToVector2();
		projectile.position=enemy.position;
		enemy.position=lockhere;

		for (int num315 = 0; num315 < 1; num315 = num315 + 1)
			{
				Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
				int num316 = Dust.NewDust(new Vector2(enemy.position.X+(enemy.width/2), enemy.position.Y+(enemy.height/2))+((randomcircle*1.5f)*((float)Math.Pow(enemy.width*enemy.height,0.5))), 8, 8, 226, 0f, 0f, 50, Main.hslToRgb(0.4f, 0f, 0.15f), 1.7f);
				Dust dust3 = Main.dust[num316];
				dust3.velocity = -randomcircle*2;
				dust3.noGravity = true;
				dust3.scale = 0.25f;
				dust3.fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
			}


		}

	}


}