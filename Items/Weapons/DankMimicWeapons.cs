using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using Terraria.Utilities;
using SGAmod.Effects;

namespace SGAmod.Items.Weapons
{
	public class StickySituationSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sticky Situation");
			Tooltip.SetDefault("Summons a Sludge Pool\nEnemies in the muck are slowed down and receive damage");
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.summon = true;
			item.sentry = true;
			item.width = 24;
			item.height = 30;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.noMelee = true;
			item.knockBack = 2f;
			item.value = Item.buyPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Pink;
			item.mana = 10;
			item.autoReuse = false;
			item.shootSpeed = 0f;
			item.UseSound = SoundID.Item78;
			item.shoot = ModContent.ProjectileType<StickySituationSludge>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse != 2)
			{
				position = Main.MouseWorld;
				Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI, 0f, 0f);
				player.UpdateMaxTurrets();
			}
			return false;
		}
	}

	public class StickySituationSludge2 : StickySituationSludge
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sludge Pool");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

		public override string Texture => "SGAmod/Items/Weapons/StickySituationSludge";

		public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 52;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = 3;
		}
        public override void AI()
        {
			base.AI();
			Projectile owner = Main.projectile[(int)projectile.ai[1]];
            if (!owner.active || (owner.type != ModContent.ProjectileType<StickySituationSludge>() && owner.type != ModContent.ProjectileType<StickySituationSludge2>()))
            {
				projectile.Kill();
            }
        }
    }

		public class StickySituationSludge : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("Sludge Pool");
			//ProjectileID.Sets.MinionTargettingFeature[base.projectile.type] = true;
		}

        public override string Texture => "SGAmod/Items/Weapons/StickySituationSludge";

        public override void SetDefaults()
		{
			projectile.width = 24;
			projectile.height = 52;
			projectile.ignoreWater = true;
			projectile.tileCollide = true;
			projectile.sentry = true;
			projectile.timeLeft = Projectile.SentryLifeTime;
			projectile.penetrate = 3;
		}

		public override void AI()
		{

			if (projectile.ai[0] == 0)
			{
				for (int i = 0; i < 4000; i += 1)
				{

					Point16 tilepos = new Point16((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
					if (!WorldGen.InWorld(tilepos.X, tilepos.Y))
						break;

					Tile tile = Main.tile[tilepos.X, tilepos.Y];

					if (tile.active() && (Main.tileSolid[tile.type] || Main.tileSolidTop[tile.type]))
					{
						break;
						//if (!Collision.CanHitLine(new Vector2(projectile.Center.X, projectile.position.Y + projectile.height), 1, 1, new Vector2(projectile.Center.X, projectile.position.Y + projectile.height + 2), 1, 1))
						//{
						//	break;
						//}
					}
					projectile.position.Y += 1;
				}
				projectile.position.Y -= 32;

				for (int num654 = 0; num654 < 64; num654++)
				{
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize(); Vector2 ogcircle = randomcircle; randomcircle *= (float)(num654 / 10.00);
					int num655 = Dust.NewDust(projectile.position+new Vector2(0,16) + (Vector2.UnitX * -8f), projectile.width + 16, projectile.height + 16, 184, projectile.velocity.X + randomcircle.X * 2f, (projectile.velocity.Y-2f) + randomcircle.Y * 1f, 200, new Color(30, 30, 30, 20), 1f);
					Main.dust[num655].noGravity = true;
				}

			}

			Player player = Main.player[base.projectile.owner];
			projectile.ai[0] += 1;
			if (projectile.ai[0] > 5)
			{
				if (projectile.ai[0] == 6 && projectile.penetrate < 5)
				{
					for (int i = -1; i < 3; i += 2)
					{
						if ((int)projectile.velocity.X == i || (int)projectile.velocity.X == 0)
						{
							Projectile proj = Projectile.NewProjectileDirect(projectile.position + new Vector2(i * 48, 0), new Vector2(i, 0), ModContent.ProjectileType<StickySituationSludge2>(), projectile.damage, 0f, projectile.owner, ai1: projectile.whoAmI);
							proj.penetrate = projectile.penetrate + 1;
							proj.ai[1] = projectile.whoAmI;
							proj.netUpdate = true;
						}
					}
				}

}
				if (projectile.ai[0] % 1 == 0)
				{
					foreach (NPC npc in Main.npc)
					{
						if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
						{
							Rectangle rec1 = new Rectangle((int)projectile.position.X - 24, (int)projectile.Center.Y - 48, projectile.width + 48, (int)projectile.height + 64);
							Rectangle rec2 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
							if (rec1.Intersects(rec2))
							{
								npc.SGANPCs().TimeSlow += 1f;
								npc.SGANPCs().nonStackingImpaled = (int)(projectile.damage*3.75f);
							}

						}
					}

				}

				projectile.position -= projectile.velocity;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D texa = Main.projectileTexture[projectile.type];
			int texsize = texa.Height / 4;
			Rectangle rect = new Rectangle(0, texsize * (int)((projectile.ai[0] / 10f) % 4), texa.Width, texsize);
			spriteBatch.Draw(texa, projectile.Center+new Vector2(0, texsize*2) - Main.screenPosition, rect, lightColor * MathHelper.Clamp(projectile.ai[0] / 6f, 0f, 1f), 0f, new Vector2(texa.Width, texsize) / 2f, new Vector2(1, 1), SpriteEffects.None, 0f);
			return false;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = Vector2.Zero;
			return false;
		}

		public override bool CanDamage()
		{
			return false;
		}
	}

	public class GnatStaff : ModItem
	{
		public virtual float minionSlot => 0.50f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Gnat Staff");
			Tooltip.SetDefault("Summons a Gnat-attracting dung pile to hover above you\nEach summon adds another fly which seeks out different enemies\nThe pile hovers above the enemy targeted when right-clicked");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.knockBack = 0;
			item.mana = 10;
			item.width = 32;
			item.height = 32;
			item.useTime = 24;
			item.useAnimation = 24;
			item.autoReuse = true;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 1, 0, 0);
			item.rare = 4;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<FlyMinionBuff>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<FlySwarmMinion>();
		}
		public override bool CanUseItem(Player player)
		{
			return (((float)player.maxMinions - player.GetModPlayer<SGAPlayer>().GetMinionSlots) > minionSlot) || player.altFunctionUse == 2;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(item.buffType, 2);

			if (player.ownedProjectileCounts[item.shoot] > 0)
            {
				Projectile flies = Main.projectile.FirstOrDefault(projtype => projtype.type == item.shoot && projtype.owner == player.whoAmI);
				SGAPlayer sgaply = player.SGAPly();
				if (((float)player.maxMinions - sgaply.GetMinionSlots) > minionSlot)
                {
					flies.minionSlots += minionSlot;
					flies.netUpdate = true;
				}

			}

			return player.ownedProjectileCounts[item.shoot]<1;
		}

	}

	public class FakeFlyProjectile
    {
		protected Vector2 position;
		protected Vector2 velocity;
		protected float timer = 0;
		protected int chargeTimer = 0;
		public int chargeTimerMax = 30;
		protected int randseed;
		protected Projectile projectile = null;
		protected Rectangle hitbox_;
		public Rectangle hitbox
		{
			get
			{
				return new Rectangle((int)(position.X-hitbox_.Width / 2), (int)(position.Y - hitbox_.Height / 2), hitbox_.Width, hitbox_.Height);
			}
			set
			{
				hitbox_ = value;
			}
		}

		public FakeFlyProjectile(Vector2 spawn,int randseed,Rectangle hitbox)
        {
			position = spawn;
			velocity = Vector2.Zero;
			this.randseed = randseed;
			this.hitbox = hitbox;

		}
		public virtual void Charge(NPC enemy,ref Vector2 flyhere, ref Vector2 hoverloc)
        {
			chargeTimer += 1;
			if (enemy != null)
			{
				flyhere = enemy.Center + hoverloc * 64f;
				if (timer % chargeTimerMax == 0)
				{
					Vector2 vec = (enemy.Center - position);
					float dist = vec.LengthSquared();
					if (dist > 64 * 64 && dist < 300 * 300)
					{
						chargeTimer = 0;
						velocity += Vector2.Normalize(vec) * 15f;
					}

				}
			}

		}
		public void Update(FlySwarmMinion owner,NPC enemy,float percent)
        {
			if (owner == null || !owner.projectile.active)
            {
				return;
            }
			Projectile projectile2 = owner.projectile;

			timer += 1;

			UnifiedRandom rando = new UnifiedRandom(randseed);

			projectile = projectile2;

			float spinrate1 = rando.NextFloat(0.02f, 0.04f)*timer;
			float spinrate2 = rando.NextFloat(0.01f, 0.03f) * timer;
			float spinrate3 = rando.NextFloat(0.005f, 0.03f) * timer;

			Matrix fancyMatrix = Matrix.CreateRotationZ(rando.NextFloat(MathHelper.TwoPi) + spinrate1) * Matrix.CreateRotationY(rando.NextFloat(MathHelper.TwoPi) + spinrate2) * Matrix.CreateRotationX(rando.NextFloat(MathHelper.TwoPi) + spinrate3);

			Vector2 hoverloc = Vector2.Transform(Vector2.One, fancyMatrix);
			Vector2 flyhere = projectile.Center + hoverloc*32f;

			float speedmul = 1f;

			if ((flyhere - position).LengthSquared() > (400 * 400))
				speedmul = 2f;

			Charge(enemy,ref flyhere,ref hoverloc);

			velocity *= owner.flyFriction;

			Vector2 flyNormal = (Vector2.Normalize(flyhere - position) * (owner.flySpeed * speedmul));
			velocity += flyNormal.RotatedBy(Math.Sin(rando.NextFloat(0.005f, 0.04f) * timer) * 0.075f);

			position += velocity;


		}

		public virtual void Draw(Texture2D texa,SpriteBatch spriteBatch, Color lightColor)
		{
			float scale = 1;
			int frame = texa.Height / 4;
			float alpha = MathHelper.Clamp(timer / 10f, 0f, 1f);
			spriteBatch.Draw(texa, position - Main.screenPosition, new Rectangle(0, ((int)((timer / 3f) % 4)) * frame, texa.Width, frame), lightColor * alpha, velocity.X/20f, new Vector2(texa.Width, frame) / 2f, scale, velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			//spriteBatch.Draw(texa, position - Main.screenPosition, new Rectangle(0,0,64,64), lightColor * alpha, -velocity.X / 5f, new Vector2(texa.Width, frame) / 2f, scale, velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}

	}

	public class FlySwarmMinion : ModProjectile
	{
		protected List<FakeFlyProjectile> flies = new List<FakeFlyProjectile>();
		protected int maxMinions => 2+(int)(projectile.minionSlots*2);
		public virtual float flySpeed => 0.35f;
		public virtual float flyFriction => 0.96f;

		public virtual float maxChaseDist => 700f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fly Swarm");
			// Sets the amount of frames this minion has on its spritesheet
			Main.projFrames[projectile.type] = 1;
			// This is necessary for right-click targeting
			ProjectileID.Sets.MinionTargettingFeature[projectile.type] = true;

			// These below are needed for a minion
			// Denotes that this projectile is a pet or minion
			Main.projPet[projectile.type] = true;
			// This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
			ProjectileID.Sets.MinionSacrificable[projectile.type] = true;
			// Don't mistake this with "if this is true, then it will automatically home". It is just for damage reduction for certain NPCs
			ProjectileID.Sets.Homing[projectile.type] = true;
		}

		public override void SetDefaults()
		{
			projectile.width = 16;
			projectile.height = 16;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.minion = true;
			// Amount of slots this minion occupies from the total minion slots available to the player (more on that later)
			projectile.minionSlots = 0.5f;
			// Needed so the minion doesn't despawn on collision with enemies or tiles
			projectile.penetrate = -1;
			projectile.timeLeft = 60;
		}


		// Here you can decide if your minion breaks things like grass or pots
		public override bool? CanCutTiles()
		{
			return false;
		}

		// This is mandatory if your minion deals contact damage (further related stuff in AI() in the Movement region)
		public override bool MinionContactDamage()
		{
			return true;
		}

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
			foreach (FakeFlyProjectile fly in flies)
			{
				if (fly.hitbox.Intersects(targetHitbox))
				return true;
			}
			return false;

        }

		public virtual void MakeFlies()
        {
			for (int i = 0; i < maxMinions; i += 1)
			{
				if (flies.Count < i)
				{
					flies.Add(new FakeFlyProjectile(projectile.Center, projectile.whoAmI * (i + 1), new Rectangle(0, 0, 12, 12)));
					projectile.localAI[0] = 0;
				}
			}
		}

		public virtual void DoPlayerChecks(Player player)
        {
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<FlyMinionBuff>());
			}
			if (player.HasBuff(ModContent.BuffType<FlyMinionBuff>()))
			{
				projectile.timeLeft = 2;
			}

		}

        public override void AI()
		{
			//if (projectile.owner == null || projectile.owner < 0)
			//return;

			MakeFlies();

			int j = 0;
			List<NPC> enemiesNearby = SGAUtils.ClosestEnemies(projectile.Center, maxChaseDist);
			foreach (FakeFlyProjectile fly in flies)
			{
				NPC enemy = enemiesNearby !=null ? (enemiesNearby.Count > 0 ? enemiesNearby[0] : null) : null;
				fly.Update(this, enemy, j / (float)maxMinions);
				if (enemiesNearby!=null && enemiesNearby.Count > 0)
				enemiesNearby.RemoveAt(0);
				j += 1;
			}



			Player player = Main.player[projectile.owner];
			DoPlayerChecks(player);

			Vector2 gothere = player.Center+new Vector2(player.direction*0,-48);

			if (player.HasMinionAttackTargetNPC)
				gothere = Main.npc[player.MinionAttackTargetNPC].Center + new Vector2(0, -48);

			projectile.velocity += (gothere- projectile.Center) /50f;

			projectile.velocity /= 1.15f;

			projectile.localAI[0] += 1;

			//if (player.HasMinionAttackTargetNPC)
		}
		public override string Texture => "SGAmod/NPCs/Dank/Fly";

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = SGAmod.ExtraTextures[95];

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.Lerp((projectile.GetAlpha(lightColor) * 0.5f), Color.Brown, 0.5f); //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(projectile.localAI[0] / 8f);
			timing %= 4;
			timing *= ((tex.Height) / 4);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 4), color*MathHelper.Clamp(1f-projectile.localAI[0]/30f,0.25f,1f), projectile.velocity.X * 0.04f, drawOrigin, (projectile.scale+projectile.minionSlots/10f), SpriteEffects.None, 0f);

			foreach (FakeFlyProjectile fly in flies)
			{
				fly.Draw(Main.projectileTexture[projectile.type], spriteBatch, lightColor);
			}

			return false;
		}

	}
	public class FlyMinionBuff : ModBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Gnat Minions");
			Description.SetDefault("Buzz Swarm Dungpile Buzz!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/GnatStaffBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("FlySwarmMinion")] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}






	public class HorseFlyStaff : GnatStaff
	{
		public virtual float minionSlot => 0.50f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Horsefly Staff");
			Tooltip.SetDefault("Summons a Horsefly-attracting dung pile to hover above you\nEach summon adds another fly which seeks out different enemies\nThe pile hovers above the enemy targeted when right-clicked");
			ItemID.Sets.GamepadWholeScreenUseRange[item.type] = true; // This lets the player target anywhere on the whole screen while using a controller.
			ItemID.Sets.LockOnIgnoresCollision[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 30;
			item.knockBack = 2f;
			item.mana = 12;
			item.width = 32;
			item.height = 32;
			item.useTime = 24;
			item.useAnimation = 24;
			item.autoReuse = true;
			item.useStyle = 1;
			item.value = Item.buyPrice(0, 2, 50, 0);
			item.rare = ItemRarityID.LightPurple;
			item.UseSound = SoundID.Item44;

			// These below are needed for a minion weapon
			item.noMelee = true;
			item.summon = true;
			item.buffType = ModContent.BuffType<FlyMinionBuff2>();
			// No buffTime because otherwise the item tooltip would say something like "1 minute duration"
			item.shoot = ModContent.ProjectileType<HorseFlySwarmMinion>();
		}

	}

	public class FakeHorseFlyProjectile : FakeFlyProjectile
	{
		List<Vector2> oldPos = new List<Vector2>();
		public FakeHorseFlyProjectile(Vector2 spawn, int randseed, Rectangle hitbox) : base(spawn,randseed,hitbox)
		{
			chargeTimerMax = 45;
			for(int i=0;i<10;i+=1)
			oldPos.Add(spawn);

		}
		public override void Charge(NPC enemy, ref Vector2 flyhere, ref Vector2 hoverloc)
		{
			for (int k = oldPos.Count - 1; k > 0; k--)
			{
				oldPos[k] = oldPos[k - 1];
			}

			chargeTimer += 1;
			if (enemy != null)
			{
				flyhere = enemy.Center + hoverloc * 64f;
				if (timer % chargeTimerMax == 0)
				{
					Vector2 vec = (enemy.Center - position);
					float dist = vec.LengthSquared();
					if (dist > 64 * 64 && dist < 600 * 600)
					{
						chargeTimer = 0;
						velocity /= 2f;
						velocity += Vector2.Normalize(vec) * 25f;
					}

				}
			}

		}

		public override void Draw(Texture2D texa, SpriteBatch spriteBatch, Color lightColor)
		{

			oldPos[0] = position;
			TrailHelper trail = new TrailHelper("DefaultPass", SGAmod.Instance.GetTexture("Perlin"));
			trail.color = delegate (float percent)
			{
				return Color.DarkOliveGreen;
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.trailThickness = 4;
			trail.trailThicknessIncrease = 6;
			trail.capsize = new Vector2(6f, 0f);
			trail.strength = Math.Max(1f- chargeTimer/15f,0f);
			trail.DrawTrail(oldPos, position);

			float scale = 1;
			int frame = texa.Height / 4;
			float alpha = MathHelper.Clamp(timer / 10f, 0f, 1f);
			spriteBatch.Draw(texa, position - Main.screenPosition, new Rectangle(0, ((int)((timer / 3f) % 4)) * frame, texa.Width, frame), lightColor * alpha, velocity.X / 20f, new Vector2(texa.Width, frame) / 2f, scale, velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			//spriteBatch.Draw(texa, position - Main.screenPosition, new Rectangle(0,0,64,64), lightColor * alpha, -velocity.X / 5f, new Vector2(texa.Width, frame) / 2f, scale, velocity.X > 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
		}

	}

	public class HorseFlySwarmMinion : FlySwarmMinion
	{
		public override float maxChaseDist => 1000f;
		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Horse Fly Swarm");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.idStaticNPCHitCooldown = 30;
			projectile.usesIDStaticNPCImmunity = true;
		}
		public override void MakeFlies()
		{
			for (int i = 0; i < maxMinions; i += 1)
			{
				if (flies.Count < i)
				{
					flies.Add(new FakeHorseFlyProjectile(projectile.Center, projectile.whoAmI * (i + 1), new Rectangle(0, 0, 18, 18)));
					projectile.localAI[0] = 0;
				}
			}
		}

		public override void DoPlayerChecks(Player player)
		{
			if (player.dead || !player.active)
			{
				player.ClearBuff(ModContent.BuffType<FlyMinionBuff2>());
			}
			if (player.HasBuff(ModContent.BuffType<FlyMinionBuff2>()))
			{
				projectile.timeLeft = 2;
			}

		}

		public override string Texture => "SGAmod/Projectiles/HorseFlySwarmMinion";

	}
	public class FlyMinionBuff2 : FlyMinionBuff
	{
		public override void SetDefaults()
		{
			DisplayName.SetDefault("Horse Fly Minions");
			Description.SetDefault("Buzz Swarm Dungpile Buzz BUZZ!");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool Autoload(ref string name, ref string texture)
		{
			texture = "SGAmod/Buffs/HorseflyStaffBuff";
			return true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			if (player.ownedProjectileCounts[mod.ProjectileType("HorseFlySwarmMinion")] > 0)
			{
				player.buffTime[buffIndex] = 18000;
			}
			else
			{
				player.DelBuff(buffIndex);
				buffIndex--;
			}
		}
	}


}
namespace SGAmod.HavocGear.Items.Weapons
{
	public class Treepeater : ModItem
	{
		public override void SetDefaults()
		{
			item.damage = 58;
			item.ranged = true;
			item.width = 22;
			item.height = 56;
			item.useTime = 26;
			item.useAnimation = 26;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 4;
			item.value = 100000;
			item.rare = 4;
			item.UseSound = SoundID.Item5;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 10f;
			item.useAmmo = AmmoID.Arrow;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Treepeater");
			Tooltip.SetDefault("Arrows shot are extremely fast and inflict Dryad's Bane");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float speed = 8f * player.ArrowSpeed();
			float rotation = MathHelper.ToRadians(2);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			//for (int i = 0; i < 2; i += 1)
			//{

			Vector2 perturbedSpeed = (new Vector2(speedX, speedY) * speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0, 100) / 100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
			speedX = perturbedSpeed.X;
			speedY = perturbedSpeed.Y;

			int proj = Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);
			Main.projectile[proj].friendly = true;
			Main.projectile[proj].hostile = false;
			Main.projectile[proj].timeLeft = 600;
			Main.projectile[proj].extraUpdates += 1;
			Main.projectile[proj].knockBack = item.knockBack;

			IdgProjectile.AddOnHitBuff(proj, BuffID.DryadsWardDebuff, 60 * 7);

			return false;

			//}

		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-8, -4);
		}
		/*public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "DankWood", 25);
            recipe.AddIngredient(null, "DankCore", 1);
            recipe.AddIngredient(null, "VirulentBar", 12);
            recipe.AddIngredient(ItemID.VineRopeCoil, 2);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }*/

	}
}
