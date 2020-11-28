using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.HavocGear.Projectiles;
using SGAmod.Effects;
using Idglibrary;
using System.Linq;
using SGAmod.Buffs;

namespace SGAmod.Items.Weapons
{
	public class CrackedMirror : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cracked Mirror");
			Tooltip.SetDefault("'You can almost hear a petrifying scream coming from the mirror'\nReleases petrifying apparitions around the player on use that petrify enemies\nThese petrified enemies take far more damage to pickaxes\nHowever, far less damage to anything else\nOnly affects specific enemy types\n" + Idglib.ColorText(Color.Orange, "Requires 1 Cooldown stack, adds 60 seconds"));
		}
		public override void SetDefaults()
		{
			item.damage = 0;
			item.noMelee = true;
			item.magic = true;
			item.width = 22;
			item.height = 22;
			item.useTime = 100;
			item.useAnimation = 100;
			item.useStyle = 5;
			item.knockBack = 10;
			item.value = 10000;
			item.rare = 2;
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<CrackedMirrorProj>();
			item.shootSpeed = 2;
			Item.staff[item.type] = true;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool Shoot (Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().AddCooldownStack(60 * 60);
			float numberProjectiles = 10; // 3, 4, or 5 shots
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 40f;
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedBy((i/(float)numberProjectiles)*MathHelper.TwoPi);
				Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, 1, knockBack, player.whoAmI);
			}
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 4);
		}
	}

		public class CrackedMirrorProj : ModProjectile
	{

		float scalePercent => MathHelper.Clamp(projectile.timeLeft / 60f, 0f, Math.Min(projectile.localAI[0] / 25f, 0.75f));
		Vector2 startingloc = default;
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.timeLeft = 300;
			projectile.light = 0.1f;
			projectile.extraUpdates = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Say STONE!");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
		{
			if (startingloc == default)
            {
				startingloc = projectile.Center;
			}

			//projectile.velocity = Collision.TileCollision(projectile.position, projectile.velocity, projectile.width, projectile.height, true);
			projectile.velocity = projectile.velocity.RotatedBy(projectile.localAI[0] / 10000f, Vector2.Zero);

			foreach (NPC enemy in Main.npc.Where(npctest => npctest.active && !npctest.friendly && !npctest.immortal && !npctest.boss && !npctest.noGravity && !npctest.noTileCollide &&
			npctest.Hitbox.Intersects(projectile.Hitbox)))
            {
				enemy.AddBuff(ModContent.BuffType<Petrified>(), 600);
            }

			projectile.localAI[0] += 1;
			int num126 = Dust.NewDust(projectile.position-new Vector2(2,2), Main.rand.Next(projectile.width+6), Main.rand.Next(projectile.height+6), DustID.t_Marble, 0,0, 240, Color.White, scalePercent);
			Main.dust[num126].noGravity = true;
			Main.dust[num126].velocity = projectile.velocity * 0.5f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
			{
				if (projectile.oldPos[i] == default)
					projectile.oldPos[i] = projectile.position;
			}

			TrailHelper trail = new TrailHelper("DefaultPass", mod.GetTexture("noise"));
			trail.color = delegate (float percent)
			{
				return Color.White;
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.capsize = new Vector2(6f, 0f);
			trail.strength = scalePercent;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			return false;
		}
	}

}
