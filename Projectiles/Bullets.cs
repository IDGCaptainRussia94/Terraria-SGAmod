using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Dusts;
using SGAmod.Effects;
using System.Linq;
using Microsoft.Xna.Framework.Audio;
using Terraria.DataStructures;

namespace SGAmod.Projectiles
{

	public class BlazeBullet : ModProjectile
	{

		double keepspeed = 0.0;
		float homing = 0.03f;
		public Player P;
		private Vector2[] oldPos = new Vector2[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Bullet");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.extraUpdates = 5;
			aiType = ProjectileID.Bullet;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_"+14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			//Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			//GameShaders.Armor.GetShaderFromItemId(ItemID.SolarDye).Apply(null);

			Texture2D tex = mod.GetTexture("Items/Weapons/Ammo/BlazeBullet");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length+1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2))*(k!= oldPos.Length - 1 ? 0.5f : 1f);
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}

			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			effects(1);
			projectile.type = ProjectileID.Bullet;
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0,10)==1)
			target.AddBuff(mod.BuffType("ThermalBlaze"), 60*3);
		}

		public virtual void effects(int type)
		{
			if (type == 1)
			{
				Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
				Vector2 positiondust = Vector2.Normalize(new Vector2(projectile.velocity.X, projectile.velocity.Y)) * 8f;
				for (int num315 = 0; num315 < 5; num315 = num315 + 1)
				{
					int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y) + positiondust, projectile.width, projectile.height, mod.DustType("HotDust"), projectile.velocity.X + (float)(Main.rand.Next(-50, 50) / 15f), projectile.velocity.Y + (float)(Main.rand.Next(-50, 50) / 15f), 50, Main.hslToRgb(0.83f, 0.5f, 0.25f), 0.25f);
					Main.dust[num316].noGravity = true;
					Dust dust3 = Main.dust[num316];
					dust3.velocity *= 0.7f;
				}

			}

		}

		public override void AI()
		{

			if (Main.rand.Next(0, 8) == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("HotDust"));
				Main.dust[dust].scale = 0.4f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			projectile.position -= projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = projectile.Center;

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}


	
	}


	public class AcidBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[6];
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Round");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.timeLeft = 300;
			projectile.extraUpdates = 5;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.CursedBullet;
			return true;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = mod.GetTexture("Items/Weapons/Ammo/AcidBullet");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void AI()
		{

			if (Main.rand.Next(0, 4) == 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"));
				Main.dust[dust].scale = 0.5f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
			}

			projectile.position -= projectile.velocity * 0.8f;

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = projectile.Center;

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(0, 3) < 2)
				target.AddBuff(mod.BuffType("AcidBurn"), 30 * (Main.rand.Next(0, 3)==1 ? 2 : 1));
		}

	}

	public class SeekerBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[20];
		private float[] oldRot = new float[20];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeker Bullet");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.timeLeft = 1200;
			projectile.extraUpdates = 10;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			if (timeLeft>0)
			projectile.type = ProjectileID.SandBallGun;
			return true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SeekerBullet"); }
		}

		public override void AI()
		{
			bool seeking = false;
			NPC target = null;
			if (projectile.ai[1] <= 0)
            {
				projectile.ai[1] = projectile.velocity.Length();
			}
			Player owner = Main.player[projectile.owner];

			if (owner.HasMinionAttackTargetNPC)
            {
				target = Main.npc[owner.MinionAttackTargetNPC];
				if (!target.dontTakeDamage && (target.Center - projectile.Center).LengthSquared() < 600 * 600)
				{
					seeking = true;
				}
			}

			if (seeking)
			{
				Vector2 velocityShift = Vector2.Normalize(target.Center - projectile.Center);
				projectile.velocity += velocityShift * 0.15f;
				projectile.velocity = Vector2.Normalize(projectile.velocity) * MathHelper.Clamp(projectile.velocity.Length(), 0, projectile.ai[1]);

				if (Main.rand.Next(0, 4) == 1)
				{
					int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 32);
					Main.dust[dust].scale = 0.5f;
					Main.dust[dust].noGravity = false;
					Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(60, 100) * 0.01f);
				}
			}

			projectile.Opacity = MathHelper.Clamp(projectile.timeLeft / 60f, 0f, 1f);

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}
			oldPos[0] = projectile.Center;

			projectile.position -= projectile.velocity * 0.9f;

			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//if (Main.rand.Next(0, 3) < 2)
			//	target.AddBuff(mod.BuffType("AcidBurn"), 30 * (Main.rand.Next(0, 3) == 1 ? 2 : 1));
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = mod.GetTexture("Items/Weapons/Ammo/SeekerBullet");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = 1f-(k/(float)(oldPos.Length));
				spriteBatch.Draw(tex, drawPos, null, color * alphaz*projectile.Opacity, projectile.rotation, drawOrigin, projectile.scale*0.75f, SpriteEffects.None, 0f);
			}
			spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, Color.White, 0, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			return false;
		}

	}

	public class SoundboundBullet : ModProjectile
	{
		private Vector2[] oldPos = new Vector2[30];
		private float[] oldRot = new float[30];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Seeker Bullet");
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.timeLeft = 900;
			projectile.extraUpdates = 4;
			projectile.penetrate = 2;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = ProjectileID.Bullet;
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type = ProjectileID.LostSoulFriendly;
			return true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Ammo/SoulboundBullet"); }
		}

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
			Player owner = Main.player[projectile.owner];

			for (float f = 0; f < 20f; f += 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y)-(Vector2.Normalize(projectile.velocity)*f*5f), projectile.width, projectile.height, 186);
				Main.dust[dust].scale = 1f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = projectile.velocity * Main.rand.NextFloat(0.01f,0.1f);
			}

			owner.Hurt(PlayerDeathReason.ByOther(5), (int)(owner.statDefense*2.00),0,cooldownCounter: 1);

			SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled,(int)projectile.Center.X, (int)projectile.Center.Y, 39);
			if (snd!= null)
            {
				snd.Pitch = -0.50f;
			}

			projectile.Kill();

			return false;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			Player owner = Main.player[projectile.owner];
			if (owner.active && !owner.dead && owner.statLife < owner.statLifeMax2 * 0.25f)
			{
				//projectile.vampireHeal(50, target.Center);
			}

			projectile.tileCollide = false;
			projectile.ai[0] = Main.rand.NextFloat(-1f, 1f) * 0.75f;
			projectile.ai[1] = MathHelper.TwoPi * 500;
			projectile.localAI[0] = Main.rand.NextFloat(-1f, 1f) * 0.10f;
			projectile.localAI[1] = Main.rand.NextFloat(0.1f, 1f) * 0.01f;
			projectile.netUpdate = true;

			owner.MinionAttackTargetNPC = target.whoAmI;

			SoundEffectInstance snd = Main.PlaySound(SoundID.NPCKilled, (int)projectile.Center.X, (int)projectile.Center.Y, 39);
			if (snd != null)
			{
				snd.Pitch = 0.25f;
			}
			projectile.damage = 0;
		}

		public override void AI()
		{

			if (oldPos[0] == default)
			{
				for (int i = 0; i < oldPos.Length; i += 1)
				{
					oldPos[i] = projectile.Center;
				}
			}

			Player owner = Main.player[projectile.owner];

			for (int k = oldPos.Length - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}

			oldPos[0] = projectile.Center;


			if (projectile.ai[0] != 0)
			{
				projectile.velocity += Vector2.Normalize(owner.Center - projectile.Center).RotatedBy(projectile.ai[0]+(float)Math.Sin(projectile.ai[1]) * projectile.localAI[0]) * 0.35f;
				projectile.velocity *= 0.990f;

				projectile.velocity += Vector2.Normalize(owner.Center - projectile.Center) * 1f*(1f-projectile.Opacity);

				projectile.Opacity -= 0.005f;
				projectile.ai[1] += projectile.localAI[1];

				if (projectile.Opacity <= 0)
                {

					if (owner.active && !owner.dead && owner.statLife < owner.statLifeMax2 * 0.25f)
					{
						bool magic = projectile.magic;
						projectile.magic = true;
						projectile.ghostHeal(25, projectile.Center);
						projectile.magic = magic;
					}

					projectile.Kill();
                }
			}


			projectile.position -= projectile.velocity * 0.5f;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = mod.GetTexture("Items/Weapons/Ammo/SoulboundBullet");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			/*for (int k = oldPos.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
				Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}*/

			//TrailHelper trailEffect = new TrailHelper("BasicEffectAlphaPass", Main.extraTexture[21]);
			TrailHelper trailEffect = new TrailHelper("DefaultPass", mod.GetTexture("Noise"));
			Color color = Color.Lerp(Color.CornflowerBlue, Color.PaleTurquoise, projectile.Opacity);
			trailEffect.color = delegate (float percent)
			{
				return color;
			};
			trailEffect.strength = projectile.Opacity*MathHelper.Clamp(projectile.timeLeft/60f,0f,1f)*2f;
			trailEffect.trailThickness = 2f;
			trailEffect.coordMultiplier = new Vector2(1f, 2f);
			trailEffect.coordOffset = new Vector2(0, Main.GlobalTime * -2f);
			trailEffect.trailThicknessIncrease = 2f;
			trailEffect.capsize = new Vector2(4f, 4f);

			trailEffect.DrawTrail(oldPos.ToList());

			//if (projectile.ai[0] == 0)
			//spriteBatch.Draw(tex, projectile.Center-Main.screenPosition, null, Color.AliceBlue * 1f, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);


			return false;
		}

}


public class NoviteBullet : ModProjectile
{
private Vector2[] oldPos = new Vector2[6];
private float[] oldRot = new float[6];
public override void SetStaticDefaults()
{
DisplayName.SetDefault("Novite Round");
}

public override void SetDefaults()
{
//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
projectile.width = 12;
projectile.height = 12;
projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
projectile.hostile = false;
projectile.friendly = true;
projectile.tileCollide = true;
projectile.ranged = true;
projectile.timeLeft = 3000;
projectile.extraUpdates = 5;
aiType = ProjectileID.Bullet;
}

public override bool PreKill(int timeLeft)
{
projectile.type = ProjectileID.CursedBullet;
return true;
}

public override string Texture
{
get { return ("SGAmod/Items/Weapons/Ammo/NoviteBullet"); }
}

public override void AI()
{

if (Main.rand.Next(0, 20) == 1)
{
	int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, DustID.GoldCoin);
	Main.dust[dust].scale = 0.20f;
	Main.dust[dust].noGravity = false;
	Main.dust[dust].velocity = projectile.velocity * (float)(Main.rand.Next(0, 100) * 0.01f);
}

projectile.position -= projectile.velocity * 0.8f;

for (int k = oldPos.Length - 1; k > 0; k--)
{
	oldPos[k] = oldPos[k - 1];
}
oldPos[0] = projectile.Center;

projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;

Player player = Main.player[projectile.owner];

if (projectile.ai[0] == 0 && player!=null)
{
	List<NPC> enemies = SGAUtils.ClosestEnemies(projectile.Center,300);

	if (enemies != null && enemies.Count > 0 && player.SGAPly().ConsumeElectricCharge(30,30))
	{
		NPC enemy = enemies[0];
		projectile.ai[0] = 1;
		projectile.velocity = Vector2.Normalize(enemy.Center - projectile.Center) * projectile.velocity.Length();
		Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 67, 0.25f, 0.5f);
	}

}
}

public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{

Texture2D tex = Main.projectileTexture[projectile.type];
Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

//oldPos.Length - 1
for (int k = oldPos.Length - 1; k >= 0; k -= 1)
{
	Vector2 drawPos = ((oldPos[k] - Main.screenPosition)) + new Vector2(0f, 0f);
	Color color = Color.Lerp(Color.White, lightColor, (float)k / (oldPos.Length + 1));
	float alphaz = (1f - (float)(k + 1) / (float)(oldPos.Length + 2)) * 1f;
	spriteBatch.Draw(tex, drawPos, null, color * alphaz, projectile.rotation, drawOrigin, projectile.scale* alphaz, SpriteEffects.None, 0f);
}
return false;
}

public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
{
//null
}



}

public class AimBotBullet : ModProjectile
{
public override void SetStaticDefaults()
{
DisplayName.SetDefault("Aimbot Bullet");
}

public override void SetDefaults()
{
projectile.CloneDefaults(ProjectileID.Bullet);
projectile.width = 12;
projectile.height = 12;
projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
projectile.hostile = false;
projectile.friendly = true;
projectile.tileCollide = true;
projectile.ranged = true;
projectile.extraUpdates = 400;
projectile.penetrate = 3;
projectile.timeLeft = 400;
projectile.aiStyle = -1;
projectile.localNPCHitCooldown = -1;
projectile.usesLocalNPCImmunity = true;
}

public override void AI()
{

if (projectile.ai[0] < 1)
{
	int dir = 1;
	if (Main.myPlayer == projectile.owner)
	{
		NPC target = Main.npc[Idglib.FindClosestTarget(0, Main.MouseWorld, new Vector2(0f, 0f), true, true, true, projectile)];
		if (target != null && Vector2.Distance(target.Center, projectile.Center) < 2000)
		{
			Vector2 projvel=projectile.velocity;
			projectile.velocity = target.Center - projectile.Center;
			projectile.velocity.Normalize(); projectile.velocity *= 8f;
			IdgProjectile.Sync(projectile.whoAmI);
			projectile.netUpdate = true;
		}


	}
	dir = Math.Sign(projectile.velocity.X);
	Main.player[projectile.owner].ChangeDir(dir);
	Main.player[projectile.owner].itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
	//Main.player[projectile.owner].itemRotation = projectile.velocity.ToRotation() * Main.player[projectile.owner].direction;

}
projectile.ai[0] += 1;

Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= 0.1f;
int num655 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 206, projectile.velocity.X + randomcircle.X * 8f, projectile.velocity.Y + randomcircle.Y * 8f, 100, new Color(30, 30, 30, 20), 1f);
Main.dust[num655].noGravity = true;
Main.dust[num655].velocity *= 0.5f;
}

public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
{
projectile.damage = (int)(projectile.damage * 1.20f);
}

public override string Texture
{
get { return ("Terraria/Projectile_" + ProjectileID.MoonlordBullet); }
}

}

public class TungstenBullet : ModProjectile
{
private Vector2[] oldPos = new Vector2[6];
private float[] oldRot = new float[6];
public override void SetStaticDefaults()
{
DisplayName.SetDefault("Tungsten Bullet");
}

public override void SetDefaults()
{
projectile.CloneDefaults(ProjectileID.Bullet);
projectile.width = 12;
projectile.height = 12;
projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
projectile.hostile = false;
projectile.friendly = true;
projectile.tileCollide = true;
projectile.ranged = true;
aiType = ProjectileID.Bullet;
}

public override string Texture
{
get { return ("Terraria/Projectile_" + 14); }
}

}

public class PortalBullet : ProjectilePortal
{
public override int takeeffectdelay => 0;
public override float damagescale => 1f;
public override int penetrate => 1;
public override int openclosetime => 8;


public override void SetStaticDefaults()
{
DisplayName.SetDefault("Spawner");
}

public override string Texture
{
get { return "Terraria/Projectile_" + 658; }
}

public override void SetDefaults()
{
projectile.width = 32;
projectile.height = 32;
projectile.friendly = true;
projectile.timeLeft = 24;
projectile.tileCollide = false;
aiType = -1;
}

public override void Explode()
{

if (projectile.timeLeft == openclosetime && projectile.ai[0] > 0)
{
	Player owner = Main.player[projectile.owner];
	if (owner != null && !owner.dead)
	{

		NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
		if (target != null && Vector2.Distance(target.Center,projectile.Center) < 1500)
		{

			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 67, 0.25f, 0.5f);

			Vector2 gotohere = new Vector2();
			gotohere = target.Center-projectile.Center;//Main.MouseScreen - projectile.Center;
			gotohere.Normalize();

			Vector2 perturbedSpeed = new Vector2(gotohere.X, gotohere.Y).RotatedByRandom(MathHelper.ToRadians(10)) * projectile.velocity.Length();
			int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(perturbedSpeed.X, perturbedSpeed.Y), ProjectileID.BulletHighVelocity, projectile.damage, projectile.knockBack / 8f, owner.whoAmI);
			Main.projectile[proj].timeLeft = 180;
			//Main.projectile[proj].penetrate = 1;
			Main.projectile[proj].GetGlobalProjectile<SGAprojectile>().onehit = true; ;
			Main.projectile[proj].netUpdate = true;
			IdgProjectile.Sync(proj);

		}
	}

}

}

public override void AI()
{
if (projectile.ai[1] < 100)
{
	projectile.ai[1] = 100;
	projectile.ai[0] = ProjectileID.BulletHighVelocity;
	Player owner = Main.player[projectile.owner];
	if (owner!=null && Main.myPlayer == owner.whoAmI)
	{
		projectile.Center = Main.MouseWorld;
		projectile.direction = Main.MouseWorld.X > owner.position.X ? 1 : -1;
	}
}
base.AI();
}

public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
{

if (scale > 0)
{
	Texture2D texture = SGAmod.ExtraTextures[99];
	Texture2D outer = SGAmod.ExtraTextures[101];
	spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, projectile.rotation, new Vector2(texture.Width / 2, texture.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
	spriteBatch.Draw(outer, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f) * 0.5f, -projectile.rotation, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;


	outer = SGAmod.ExtraTextures[102];
	spriteBatch.Draw(outer, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f)*0.5f, projectile.rotation*2, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;
	spriteBatch.Draw(outer, projectile.Center - Main.screenPosition, null, Color.Lerp(Color.Yellow, lightColor, 0.75f)*0.5f, -projectile.rotation*2, new Vector2(outer.Width / 2, outer.Height / 2), new Vector2(1, 1) * scale, SpriteEffects.None, 0f); ;


}
return false;
}

}



}