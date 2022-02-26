using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.ThrowingKnives
{
	public class FrostburnKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/FrostburnKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Frostburn Knife");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IceTorch);
			}
			Lighting.AddLight(projectile.Center, 0.0f, 0.36f, 0.5f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(BuffID.Frostburn, 240);
			}
		}
	}
	public class AcidKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/AcidKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Acid Knife");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.AcidDust>(), 0, 0, 0, default, 0.5f);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.AcidDust>(), 0, 0, 0, default, 0.5f);
			}
			Lighting.AddLight(projectile.Center, 0.05f, 0.25f, 0.01f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(ModContent.BuffType<Buffs.AcidBurn>(), 300);
			}
		}
	}
	public class CursedKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/CursedKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cursed Knife");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.CursedTorch);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.CursedTorch);
			}
			Lighting.AddLight(projectile.Center, 0.10f, 0.39f, 0.05f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(BuffID.CursedInferno, 360);
			}
		}
	}
	public class IchorKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/IchorKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ichor Knife");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IchorTorch);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.IchorTorch);
			}
			Lighting.AddLight(projectile.Center, 0.5f, 0.40f, 0.15f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(BuffID.Ichor, 360);
			}
		}
	}
	public class BlazingKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/BlazingKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Blazing Knife");
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.HotDust>(), 0, 0, 0, default, 0.5f);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.HotDust>(), 0, 0, 0, default, 0.5f);
			}
			Lighting.AddLight(projectile.Center, 1.0f, 0.40f, 0.20f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(ModContent.BuffType<Buffs.ThermalBlaze>(), 360);
			}
		}
	}
	public class DankKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/DankKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dank Knife");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.penetrate = 3;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.MangroveDust>(), 0, 0, 0, default, 0.5f);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(SoundID.Dig, projectile.position);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, ModContent.DustType<Dusts.MangroveDust>(), 0, 0, 0, default, 0.5f);
			}
			Lighting.AddLight(projectile.Center, 0.25f, 0.25f, 0.05f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.NextBool())
			{
				target.AddBuff(ModContent.BuffType<Buffs.DankSlow>(), 300);
			}
		}
	}
	public class VenomKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/VenomKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Venom Knife");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 13;
			projectile.height = 13;
			projectile.penetrate = 3;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Venom);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			else
			{
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(SoundID.Dig, projectile.position);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.NextBool())
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.Venom);
			}
			Lighting.AddLight(projectile.Center, 0.18f, 0.13f, 0.27f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Venom, 420);
		}
	}
	public class NanoKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/NanoKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nano Knife");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 13;
			projectile.height = 13;
			projectile.penetrate = 3;
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BlueTorch);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			Main.PlaySound(SoundID.Dig, projectile.position);
			//Copied from Nano Bullet (1.4.1), modified slightly
			//"Smart Bouncing". Basically it bounces in the direction of a nearby enemy.
			//This knife is still affected by gravity, so it may miss sometimes.
			projectile.ai[1] += 1f;
			if (projectile.ai[1] == 1f || projectile.ai[1] == 2f)
			{
				projectile.damage = (int)((float)projectile.damage * 0.66f);
			}
			if (projectile.ai[1] >= 3f)//number of bounces allowed
			{
				projectile.Kill();
				return false;
			}
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = oldVelocity.X * -1f;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = oldVelocity.Y * -1f;
			}
			int newTarget = FindTargetWithLineOfSight();
			if (newTarget != -1)
			{
				NPC nPC2 = Main.npc[newTarget];
				projectile.Distance(nPC2.Center);
				projectile.velocity = projectile.DirectionTo(nPC2.Center).SafeNormalize(-Vector2.UnitY) * projectile.velocity.Length();
				projectile.netUpdate = true;
			}
			return false;
		}
		//Copied from vanila (1.4) Projectiles.cs
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			float newMaxRange = maxRange;
			int result = -1;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				bool nPCCanBeChased = nPC.CanBeChasedBy(this);
				if (projectile.localNPCImmunity[i] != 0)
				{
					nPCCanBeChased = false;
				}
				if (nPCCanBeChased)
				{
					float projDist = projectile.Distance(Main.npc[i].Center);
					if (projDist < newMaxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height))
					{
						newMaxRange = projDist;
						result = i;
					}
				}
			}
			return result;
		}
		public override void PostAI()
		{
			if (Main.rand.Next(4) == 0)
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.BlueTorch);
			}
			Lighting.AddLight(projectile.Center, 0.5f, 0.41f, 0.44f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Confused, 240);
		}
	}
	public class SolarKnifeProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/SolarKnife";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solar Knife");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.penetrate = 5;
			aiType = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			//projectile.spriteDirection = projectile.direction;
			projectile.ai[0] += 1f; // Use a timer to wait 20 ticks before applying gravity.
			if (projectile.ai[0] >= 20f) //Default Throwing knife is 20f
			{
				projectile.ai[0] = 20f;
				projectile.velocity.Y += 0.2f; //Default Throwing knife is 0.4f
				projectile.velocity.X *= 0.985f;  //Default Throwing knife is 0.97f
			}
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.SolarFlare);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				Main.PlaySound(SoundID.Item14, projectile.position);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, ProjectileID.SolarWhipSwordExplosion, projectile.damage / 2, projectile.knockBack, projectile.owner);
				projectile.Kill();
			}
			else
			{
				Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
				Main.PlaySound(SoundID.Item10, projectile.position);
				if (projectile.velocity.X != oldVelocity.X)
				{
					projectile.velocity.X = -oldVelocity.X;
				}
				if (projectile.velocity.Y != oldVelocity.Y)
				{
					projectile.velocity.Y = -oldVelocity.Y;
				}
			}
			return false;
		}
		public override void PostAI()
		{
			if (Main.rand.Next(3) == 0)
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.SolarFlare);
			}
			Lighting.AddLight(projectile.Center, 1f, 1f, 0.5f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(BuffID.Daybreak, 480);
			if (projectile.penetrate <= 1)
			{
				Main.PlaySound(SoundID.Item14, projectile.position);
				Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, projectile.velocity.X * 0.2f, projectile.velocity.Y * 0.2f, ProjectileID.SolarWhipSwordExplosion, projectile.damage / 2, projectile.knockBack, projectile.owner);
			}
		}
	}
	public class StarMetalKnivesProjectile : ThrowingKnifeBaseProjectle
	{
		public override string Texture => "SGAmod/Items/Weapons/ThrowingKnives/StarMetalKnivesProjectile";
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Star Metal Knife");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = -1;
			projectile.penetrate = 5;
			aiType = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.extraUpdates = 1;
		}
		public override void AI()
		{
			projectile.rotation = (float)Math.Atan2(projectile.velocity.Y, projectile.velocity.X) + 1.57f;
			//projectile.spriteDirection = projectile.direction;
			projectile.ai[0] += 1f; // Use a timer to wait 20 ticks before applying gravity.
			if (projectile.ai[0] >= 20f) //Default Throwing knife is 20f
			{
				projectile.ai[0] = 20f;
				projectile.velocity.Y += 0.2f; //Default Throwing knife is 0.4f
				projectile.velocity.X *= 0.985f;  //Default Throwing knife is 0.97f
			}
			if (projectile.velocity.Y > 16f)
			{
				projectile.velocity.Y = 16f;
			}
		}
		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.LunarOre, 0, 0, 0, default, 0.5f);
			//If collide with tile, reduce the penetrate.
			projectile.penetrate--;
			if (projectile.penetrate <= 0)
			{
				projectile.Kill();
			}
			Main.PlaySound(SoundID.Dig, projectile.position);
			//Copied from Nano Bullet (1.4.1), modified slightly
			//"Smart Bouncing". Basically it bounces in the direction of a nearby enemy.
			//This knife is still affected by gravity, so it may miss sometimes.
			projectile.ai[1] += 1f;
			if (projectile.ai[1] >= 1f && projectile.ai[1] <= 4f)
			{
				projectile.damage = (int)((float)projectile.damage * 0.75f);
			}
			if (projectile.ai[1] >= 5f)//number of bounces allowed
			{
				projectile.Kill();
				return false;
			}
			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = oldVelocity.X * -1f;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = oldVelocity.Y * -1f;
			}
			int newTarget = FindTargetWithLineOfSight();
			if (newTarget != -1)
			{
				NPC nPC2 = Main.npc[newTarget];
				projectile.Distance(nPC2.Center);
				projectile.velocity = projectile.DirectionTo(nPC2.Center).SafeNormalize(-Vector2.UnitY) * projectile.velocity.Length();
				projectile.netUpdate = true;
			}
			return false;
		}
		//Copied from vanila (1.4) Projectiles.cs
		public int FindTargetWithLineOfSight(float maxRange = 800f)
		{
			float newMaxRange = maxRange;
			int result = -1;
			for (int i = 0; i < 200; i++)
			{
				NPC nPC = Main.npc[i];
				bool nPCCanBeChased = nPC.CanBeChasedBy(this);
				if (projectile.localNPCImmunity[i] != 0)
				{
					nPCCanBeChased = false;
				}
				if (nPCCanBeChased)
				{
					float projDist = projectile.Distance(Main.npc[i].Center);
					if (projDist < newMaxRange && Collision.CanHit(projectile.position, projectile.width, projectile.height, nPC.position, nPC.width, nPC.height))
					{
						newMaxRange = projDist;
						result = i;
					}
				}
			}
			return result;
		}
		public override void PostAI()
		{
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, DustID.LunarOre, 0, 0, 0, default, 0.5f);
			}
			Lighting.AddLight(projectile.Center, 0.5f, 0.5f, 0.5f);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			target.AddBuff(ModContent.BuffType<Buffs.MoonLightCurse>(), 120);
		}
	}
}