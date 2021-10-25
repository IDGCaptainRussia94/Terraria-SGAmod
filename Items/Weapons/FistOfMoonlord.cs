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
using Idglibrary;
using System.Linq;

namespace SGAmod.Items.Weapons
{
	public class FistOfMoonlord : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fist Of Moon Lord");
			Tooltip.SetDefault("'Punches shit into next week'\nHold attack to direct Moonlord's arm, release to let go\nThe fist does more damage when let go");
			Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(15, 2));
		}
		public override void SetDefaults()
		{
			item.damage = 1000;
			item.noMelee = true;
			item.noUseGraphic = true;
			item.melee = true;
			item.width = 22;
			item.channel = true;
			item.height = 22;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = 5;
			item.knockBack = 30;
			item.value = 2000000;
			item.rare = 12;
			item.UseSound = SoundID.Item72;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("FistOfMoonlordProjectile2");
			item.shootSpeed = 10;
			Item.staff[item.type] = true;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[mod.ProjectileType("FistOfMoonlordProjectile2")]<1;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			type = mod.ProjectileType("FistOfMoonlordProjectile2");
			Projectile.NewProjectile(position.X, position.Y, 0, 0, type,damage, knockBack, player.whoAmI);
			return false;
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-1, 4);
		}
	}

	public class FistOfMoonlordProjectile : ModProjectile
	{
		Player Owner => Main.player[projectile.owner];
		int[] armSizes, armOrigins;
		public override void SetDefaults()
		{
			projectile.width = 128;
			projectile.height = 128;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.light = 0.1f;
			projectile.extraUpdates = 6;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 30;
			projectile.netImportant = true;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture => "Terraria/NPC_" + NPCID.MoonLordHand;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fist o' Lord 'o Moon");
		}

		public override bool CanDamage()
		{
			return projectile.velocity.Length()>8;
		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			hitDirection = Math.Sign(projectile.velocity.X);
			if (hitDirection == 0)
				hitDirection = 1;

			knockback *= (projectile.velocity.Length() / 120f);
			projectile.ai[1] /= 5f;
			damage = (int)(damage*(Owner.channel ? MathHelper.Clamp(projectile.ai[1] / (120f / Owner.meleeSpeed), 0.33f,1f) : 1f) *projectile.velocity.Length());
			if (projectile.ai[1] < 30)
			{
				damage = (int)(damage * 0.50);
				damage = (int)(damage * 0.25f);
            }
            else
            {
				SGAmod.AddScreenShake((float)Math.Pow(damage,0.75f)*0.025f,720, target.Center);
			}

		}

        public override void AI()
		{
			armSizes = new int[] { 400, 400 };
			armOrigins = new int[] { 0, 42 };
			int totallength = (armSizes[0] - armOrigins[0]) + (armSizes[1] - armOrigins[1]);

			if (!Owner.active || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<FistOfMoonlord>())
			{
				projectile.Kill();
				return;
			}

			projectile.ai[0] -= 1;
			Projectile proj = default;
			List<Projectile> projs = Main.projectile.Where(testby => testby.active && testby.owner == projectile.owner && testby.type == ModContent.ProjectileType<FistOfMoonlordProjectile2>()).ToList();
			if (projs.Count > 0)
			{
				projectile.ai[1] += 1;
				proj = projs[0];
				if (proj.ai[0] > 0)
                {
					projectile.ai[0] = 2;
					projectile.damage = proj.damage;
					projectile.knockBack = proj.damage;
				}
            }
            else
            {
				projectile.ai[1] /= 1.2f;
			}

			projectile.Opacity = MathHelper.Clamp(projectile.Opacity + (projectile.ai[0] > -60 ? 0.005f : -0.005f),0.0f,1f);

			if (Main.netMode != NetmodeID.Server)
			{
				Vector2 iWantToGoThere = Main.MouseWorld;
				if ((iWantToGoThere-Owner.MountedCenter).LengthSquared() > totallength* totallength)
                {
					iWantToGoThere = Owner.MountedCenter+Vector2.Normalize(iWantToGoThere - Owner.MountedCenter) * (totallength-1);
				}

				Vector2 toThere = iWantToGoThere - projectile.Center;
				if (projectile.ai[0]>0)
				{
					float maxspeed = MathHelper.Clamp(projectile.ai[1] / (120f / Owner.meleeSpeed), 0f, 1.0f);
					if (toThere.LengthSquared() > 32 + (projectile.velocity.LengthSquared() * 5f))
						projectile.velocity += Vector2.Normalize(toThere) * ((maxspeed / Owner.meleeSpeed) / Owner.meleeSpeed);
					else
						projectile.velocity *= 0.80f;
				}

				projectile.velocity *= (projectile.ai[0] < 1 ? 0.99f : 0.95f);


			}

			projectile.timeLeft = 3;
			projectile.position += (Owner.velocity/(projectile.extraUpdates+1));

			Vector2 diffs = projectile.Center - Owner.MountedCenter;
			Owner.ChangeDir(Math.Sign(1 + Math.Sign(diffs.X) * 2));

			Owner.bodyFrame.Y = Owner.bodyFrame.Height * 3;
			if (diffs.Y - Math.Abs(diffs.X) > 25)
				Owner.bodyFrame.Y = Owner.bodyFrame.Height * 4;
			if (diffs.Y + Math.Abs(diffs.X) < -25)
				Owner.bodyFrame.Y = Owner.bodyFrame.Height * 2;
			if (diffs.Y + Math.Abs(diffs.X) < -160)
				Owner.bodyFrame.Y = Owner.bodyFrame.Height * 5;

			if (diffs.Length() > totallength - 1)
            {
				projectile.Center = Owner.MountedCenter+Vector2.Normalize(diffs)*(totallength - 1);
            }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			float handangle = DrawMoonlordHand(spriteBatch, Owner.MountedCenter, projectile.Center);

			Texture2D armTex2 = Main.extraTexture[18];
			Texture2D handTex = Main.projectileTexture[projectile.type];
			Rectangle rect = new Rectangle(0, (int)(MathHelper.Clamp((projectile.Opacity*5f),0,3))*(handTex.Height / 4), handTex.Width, handTex.Height / 4);
			Main.spriteBatch.Draw(armTex2, projectile.Center+new Vector2(2,0) - Main.screenPosition, new Rectangle?(), Color.White* projectile.Opacity, handangle+MathHelper.PiOver2, armTex2.Size() / 2f, projectile.scale+0.25f, Owner.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0); ;
			Main.spriteBatch.Draw(handTex, projectile.Center - Main.screenPosition, rect, Color.White * projectile.Opacity, handangle + MathHelper.PiOver2, new Vector2(handTex.Width, (handTex.Height/4f)/0.75f) / 2f, projectile.scale, Owner.direction > 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0); ;

			return false;
		}

		public float DrawMoonlordHand(SpriteBatch spriteBatch, Vector2 drawHere, Vector2 drawThere)
		{
			SpriteEffects spriteEffects = SpriteEffects.None;

			float returnfangle = 0;

			float angleoffset = MathHelper.PiOver2;
			Texture2D armTex = Main.extraTexture[14];
			Texture2D armTex2 = Main.extraTexture[15];

			float scale = 1;
			float armLength = armSizes[0] - armOrigins[0];
			float armLength2 = armSizes[1] - armOrigins[1];
			Vector2 dist = drawThere - drawHere;
			Vector2 normal = Vector2.Normalize(dist);
			Vector2 hand2loc = drawHere + normal * Math.Min(dist.Length(), ((armLength + armLength2)) - 1);

			Vector2 CirclePoint1, CirclePoint2;
			if (Idglib.FindCircleCircleIntersections(drawHere, armLength, hand2loc, armLength2, out CirclePoint1, out CirclePoint2) > 0)
			{
				Vector2 Circlepos = Owner.direction > 0 ? CirclePoint2 : CirclePoint1;
				Vector2 elbowloc = Circlepos;
				Vector2 normal2 = elbowloc - drawHere;
				Vector2 normal3 = drawThere - elbowloc;

				Vector2 origin1 = new Vector2(armTex.Width / 2f, armLength);
				Vector2 origin2 = new Vector2(armTex2.Width / 2f, armLength2);

				returnfangle = normal3.ToRotation();

				spriteBatch.Draw(armTex, drawHere - Main.screenPosition, null, Color.White * projectile.Opacity, normal2.ToRotation() + angleoffset, origin1, scale, spriteEffects, 0f);
				spriteBatch.Draw(armTex2, Circlepos - Main.screenPosition, null, Color.White * projectile.Opacity, normal3.ToRotation() + angleoffset, origin2, scale, spriteEffects, 0f);

				//spriteBatch.Draw(TestTex, drawHere - Main.screenPosition, null, Color.White, 0, TestTex.Size() / 2f, scale, spriteEffects, 0f);
				//spriteBatch.Draw(TestTex, drawHere + dist1Tracker - Main.screenPosition, null, Color.White, 0, TestTex.Size() / 2f, scale, spriteEffects, 0f);
				//spriteBatch.Draw(TestTex, drawHere + dist1Tracker + dist2Tracker - Main.screenPosition, null, Color.White, 0, TestTex.Size() / 2f, scale, spriteEffects, 0f);
			}

			return returnfangle;

			//spriteBatch.Draw(armTex, drawHere - Main.screenPosition, null, Color.White, angle + angleoffset, origin, scale, spriteEffects, 0f);

			//spriteBatch.Draw(armTex, arm2pos - Main.screenPosition, null, Color.White, angle2, origin, scale, spriteEffects, 0f);
		}

	}

	public class FistOfMoonlordProjectile2 : ModProjectile
	{
		Player Owner => Main.player[projectile.owner];
		public override void SetDefaults()
		{
			projectile.width = 4;
			projectile.height = 4;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.timeLeft = 300;
			projectile.tileCollide = false;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.netImportant = true;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

        public override bool CanDamage()
        {
            return false;
        }

        public override void AI()
		{
			if (!Owner.active || Owner.dead || Owner.HeldItem.type != ModContent.ItemType<FistOfMoonlord>() || (!Owner.channel))
			{
				projectile.Kill();
				return;
			}
			projectile.timeLeft = 2;
			Owner.itemTime = 6;
			Owner.itemAnimation = 6;
			Owner.itemAnimationMax = 6;
			projectile.ai[0] += 1000;

		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fist o' Lord 'o Moon with more PUNCH");
		}

	}

}