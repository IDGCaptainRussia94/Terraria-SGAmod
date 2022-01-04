using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Items.Weapons;
using SGAmod.HavocGear.Items.Weapons;
using AAAAUThrowing;
using SGAmod.Effects;

namespace SGAmod.Items.Weapons
{

	public class AvaliScythe : SeriousSamWeapon,ITechItem
	{
		public float ElectricChargeScalingPerUse() => GetType() == typeof(CyberScythe) ? 0.01f : 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avali Scythe");
			Tooltip.SetDefault("Spins a duo-scythe around the player that speeds up over time" +
				"\nAfter holding for a short while, release to throw the weapon\nWhen thrown, Does up to double the damage and deals throwing damage" + "\n'The weapon of the fallen dragon'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 25;
			item.crit = 10;
			item.melee = true;
			item.width = 34;
			item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 8;
			item.value = 100000;
			item.rare = 5;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("AvaliScytheProjectile");
			item.shootSpeed = 7f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			if (GetType() == typeof(AvaliScythe))
			if (!Main.gameMenu)
			{
				Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/GlowMasks/AvaliScythe_Glow");
				Texture2D tex = Main.itemTexture[item.type];
				Vector2 textureOrigin = new Vector2(texGlow.Width / 2, texGlow.Height / 2);
				spriteBatch.Draw(tex, item.position+new Vector2(16,-12) - Main.screenPosition, null, lightColor, rotation, textureOrigin, scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(texGlow, item.position + new Vector2(16, -12) - Main.screenPosition, null, Color.White, rotation, textureOrigin, scale, SpriteEffects.None, 0f);
					return false;
                }
			return true;
		}
		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			/*if (!Main.gameMenu)
			{
				Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/GlowMasks/AvaliScythe_Glow");
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texGlow.Width / 2, texGlow.Height / 2);
				spriteBatch.Draw(texGlow, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale* scale, SpriteEffects.None, 0f);
			}*/
			return true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 8);
			recipe.AddRecipeGroup("SGAmod:Tier4Bars", 6);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class AvaliScytheProjectile : ModProjectile
	{
		protected float[] oldRot = new float[6];
		protected int subdamage;
		protected virtual float maxSpin => 16f;
		protected virtual float chargeUpRate => 0.2f;
		protected virtual float spinMulti => 1f;

		protected virtual float throwMulti => 1f;

		protected virtual int spinSoundDelay => 120;
		protected bool letItGo = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Avali Scythe");
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{

			Vector2[] oldPos = new Vector2[2];
			oldPos[0] = projectile.Center + new Vector2(48, 0);
			oldPos[0] = oldPos[0].RotatedBy(MathHelper.ToRadians(45));
			oldPos[0] = oldPos[0].RotatedBy(projectile.rotation);
			oldPos[0].Normalize();
			oldPos[0] *= 48f;
			oldPos[1] = oldPos[0].RotatedBy(MathHelper.ToRadians(180));

			foreach (Vector2 position in oldPos)
			{
				projHitbox.X = (int)(projectile.Center.X + position.X);
				projHitbox.Y = (int)(projectile.Center.Y + position.Y);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/AvaliScythe");
			Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/GlowMasks/AvaliScythe_Glow");
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				spriteBatch.Draw(texGlow, drawPos, null, Color.White * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/AvaliScythe"; }
		}

		protected virtual void PlaySound()
        {
			//nil
		}

		public override void AI()
		{
			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
			}
			oldRot[0] = projectile.rotation;

			Player basep = Main.player[projectile.owner];

			if (basep == null || basep.dead)
			{
				projectile.Kill();
				return;
			}
			if ((!basep.channel || basep.dead || letItGo) && projectile.ai[1] < 1)
			{
				if (projectile.ai[0] < 10)
				{
					projectile.Kill();
					return;


				}
				else
				{
					projectile.ai[1] = 1;
					projectile.velocity *= projectile.ai[0] * throwMulti * basep.Throwing().thrownVelocity;
					projectile.timeLeft = 300+(GetType() == typeof(CyberScytheProjectile) ? 100 : 0);
					projectile.melee = false;
					projectile.Throwing().thrown = true;
					projectile.damage = (int)(projectile.damage * basep.Throwing().thrownDamage);
					basep.itemAnimation = 60;
					basep.itemTime = 60;
					Main.PlaySound(SoundID.Item71, basep.Center);
					projectile.damage = (int)(projectile.damage * (1 + (projectile.ai[0] / 15f)));
					projectile.netUpdate = true;
				}
			}

			if (projectile.ai[1] < 1)
			{

				subdamage = projectile.damage;
				projectile.ai[0] = Math.Min(projectile.ai[0] + chargeUpRate, maxSpin);
				basep.itemAnimation = 5;
				basep.itemTime = 5;
				projectile.timeLeft = projectile.extraUpdates + 2;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 mousePos = Main.MouseWorld;
					Vector2 diff = mousePos - basep.Center;
					diff.Normalize();
					projectile.velocity = diff;
					basep.direction = Main.MouseWorld.X > basep.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}

				basep.heldProj = projectile.whoAmI;

				projectile.position -= projectile.velocity;
				basep.bodyFrame.Y = basep.bodyFrame.Height * (2 + (((int)((projectile.localAI[0] * spinMulti) / 40)) % 3));
				projectile.Center = basep.Center;
			}
			projectile.localAI[0] += spinMulti * (projectile.ai[0] + 5f) * projectile.direction;
			projectile.localAI[1] += (1 + projectile.ai[0] / 8);

			PlaySound();

			projectile.rotation = MathHelper.ToRadians(projectile.localAI[0]);

		}


	}

	public class CyberScythe : AvaliScythe,ITechItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyber Scythe");
			Tooltip.SetDefault("Spins a duo-Cybernetic Scythe around the player\nThe blades expand to reach your mouse cursor's distance\nDoes more damage further out and hits more often as it charges up" +
				"\nAfter holding for a short while, release to throw the weapon\nWhen thrown, Does throwing damage and deals more\nConsumes Electric Charge; requires more the longer you charge it up");
			Item.staff[item.type] = true;
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Technical/CyberScythe"; }
		}

        public override bool CanUseItem(Player player)
        {

			return base.CanUseItem(player) && player.SGAPly().ConsumeElectricCharge(200, 80);

		}

        public override void SetDefaults()
		{
			item.damage = 250;
			item.melee = true;
			item.width = 34;
			item.height = 24;
			item.crit = 15;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 8;
			item.value = 5000000;
			item.rare = ItemRarityID.Red;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("CyberScytheProjectile");
			item.shootSpeed = 7f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("RedPhasebrand"), 1);
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 10);
			recipe.AddIngredient(mod.ItemType("LunarRoyalGel"), 15);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 8);
			recipe.AddIngredient(ItemID.LunarBar, 6);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class CyberScytheProjectile : AvaliScytheProjectile, IDrawAdditive
	{
		protected override float maxSpin => 24f;
		protected override float chargeUpRate => 0.05f/5f;
		protected override float spinMulti => 0.16f;
		protected override float throwMulti => 0.1f;
		protected override int spinSoundDelay => 320;

		private bool start=false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyber Scythe");
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Technical/CyberScythe"; }
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 48;
			projectile.height = 48;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 4;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			return false;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];

			if (!start)
            {
				start = true;
				for (float i = 0; i < MathHelper.TwoPi; i += MathHelper.Pi)
				{
					Projectile projectile2 = Projectile.NewProjectileDirect(projectile.Center,i.ToRotationVector2(), ModContent.ProjectileType<CyberScytheProjectileEdge>(), projectile.damage, projectile.knockBack, projectile.owner, 0, projectile.whoAmI);
				}

			}

			if (owner!=null && !owner.SGAPly().ConsumeElectricCharge((int)projectile.ai[0]/4, 30) && projectile.ai[1]<1)
			{
				letItGo = true;
				return;
			}

			base.AI();
		}

		protected override void PlaySound()
		{
			if (projectile.localAI[1] > spinSoundDelay)
			{
				projectile.localAI[1] = 0;
				Main.PlaySound(SoundID.Zombie, (int)projectile.Center.X, (int)projectile.Center.Y, 66, 1f, -0.25f+(projectile.ai[0] / 30f));
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{

			for (float u = 0; u < MathHelper.TwoPi; u += MathHelper.Pi)
			{
				Vector2 oldPos = new Vector2(1f, 0);
				oldPos = oldPos.RotatedBy(u + (projectile.rotation + (0f)));
				oldPos.Normalize();
				oldPos *= 24f;

				Vector2 torot = oldPos;
				torot.Normalize();

				int thisoned = Projectile.NewProjectile(projectile.Center + oldPos, torot * 8f, ProjectileID.ScutlixLaser, (int)(projectile.damage / 3), knockback, Main.myPlayer);
				//Main.projectile[thisoned].usesLocalNPCImmunity = true;
				//Main.projectile[thisoned].localNPCHitCooldown = -1;
				Main.projectile[thisoned].penetrate = 2;
				Main.projectile[thisoned].knockBack = 0f;
				Main.projectile[thisoned].usesIDStaticNPCImmunity = true;
				Main.projectile[thisoned].idStaticNPCHitCooldown = 30;
				Main.projectile[thisoned].netUpdate = true;
				IdgProjectile.Sync(thisoned);
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{

		}
	}

	public class CyberScytheProjectileEdge : ModProjectile
	{
		Projectile master;
		BasicEffect basicEffect;
		public CyberScytheProjectileEdge()
		{
			if (!Main.dedServ)
			basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cyber Scythe");
		}
		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/Technical/BladeEdge"; }
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 4;
			projectile.timeLeft = 10;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 10;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			master = Main.projectile[(int)projectile.ai[1]];
			projectile.timeLeft = 10;
			if (!owner.active || owner.dead || !master.active || master.type != ModContent.ProjectileType<CyberScytheProjectile>())
				projectile.active = false;

				/*for (int i = 0; i < 20; i += 1)
				{
					int dust2 = Dust.NewDust(projectile.position, projectile.width, projectile.height, 33);
					Main.dust[dust2].scale = 2.5f;
					Main.dust[dust2].noGravity = false;
					Main.dust[dust2].velocity = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-12f, 1f));
				}*/

				if (owner != null)
            {

				if (projectile.localAI[0] % (projectile.extraUpdates + 1) == 0)
				{

					if (Main.netMode != NetmodeID.Server && master.ai[1] < 1)
					{
						float speedrate = (1000f / (((master.ai[0]*2f) * owner.meleeSpeed) + 1));
						projectile.ai[0] += ((Main.MouseWorld - owner.MountedCenter).Length() - projectile.ai[0]) / speedrate;
						projectile.ai[0] = Math.Max(Math.Min(160, master.ai[0] * 20), Math.Min(projectile.ai[0], 600f / owner.meleeSpeed));
						projectile.netUpdate = true;
					}
				}
				if (master.ai[1] > 0)
                {
					if (!projectile.ignoreWater)
					{
						projectile.localAI[1] = projectile.ai[0];
						projectile.ignoreWater = true;
					}

					projectile.ai[0] /= 1.004f;
                }
                else
                {
					projectile.localNPCHitCooldown = (int)Math.Max(80f/(1+master.ai[0]/2),4);
				}

				projectile.localAI[0] += 1;
				float angle = master.rotation+projectile.velocity.ToRotation()+((MathHelper.Pi/20f)* (float)projectile.numUpdates);
				projectile.rotation = angle;
				Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));

				projectile.Center = master.Center + vector * projectile.ai[0];


			}

		}

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			damage = (int)(damage*(1f + (projectile.ai[0] / 300f)));
		}

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			if (master != null)
			{

				float angle2 = master.rotation + projectile.velocity.ToRotation();
				angle2 -= master.direction * 0.1f;
				Vector2 vector2 = new Vector2((float)Math.Cos(angle2), (float)Math.Sin(angle2));
				Vector2 center2 = master.Center + vector2 * (projectile.ai[0]);//+Math.Min(48,projectile.ai[0]/1.75f));

				Vector2 goto2 = center2;
				Vector2 prev2 = goto2;
				Vector2 point = center2;
				Vector2 anglex = Vector2.Normalize(master.Center - center2).RotatedBy(MathHelper.Pi/2f);
				float length = (master.Center - center2).Length();
				float div2 = 1f * (float)(length / 60f);
				float div = 1f / (float)(length / 80f);

				VertexBuffer vertexBuffer;

				basicEffect.World = WVP.World();
				basicEffect.View = WVP.View(Main.GameViewMatrix.Zoom);
				basicEffect.Projection = WVP.Projection();
				basicEffect.VertexColorEnabled = true;
				basicEffect.TextureEnabled = true;
				basicEffect.Texture = Main.extraTexture[21];


				int totalcount = 3 + (int)div2;

				VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[((totalcount+1)*6)];

				Vector3[] prevcoords = { Vector3.One , Vector3.One };

				for (int k = 1; k < totalcount+1; k += 1) 
				{
					float fraction = (float)k / (float)totalcount;
					float fractionPlus = (float)(k+1d) / (float)totalcount;
					point = Vector2.Lerp(center2, master.Center, fraction);

					float scaler = (float)Math.Sin(fraction * Math.PI);
					float timer1 = (float)Math.Sin(Main.GlobalTime*17f + (fraction * 24f* div)) * (scaler * 8);
					float timer2 = (float)Math.Sin(Main.GlobalTime*6f + (fraction * 78f* div)) * (scaler * 30);

					//timer2 *= projectile.ai[0] / 300f;

					goto2 = point + (anglex * (timer1 + timer2))*Math.Min(length/300f,1f)*(k>0 ? 1f : 0f);

					if (k >= totalcount)
						goto2 = master.Center;

					//You want prims, you get prims!

					Vector2 normal = Vector2.Normalize(goto2 - prev2);
					Vector3 left = (normal.RotatedBy(MathHelper.Pi / 2f)* (6)).ToVector3();
					Vector3 right = (normal.RotatedBy(-MathHelper.Pi / 2f)* (6)).ToVector3();

					Vector3 drawtop = (goto2+new Vector2(k*0, 0) - Main.screenPosition).ToVector3();
					Vector3 drawbottom = (prev2 + new Vector2(k * 0, 0) - Main.screenPosition).ToVector3();

					if (prevcoords[0] == Vector3.One)
                    {
						prevcoords = new Vector3[2] { drawbottom + left, drawbottom + right};
					}

					float timer = Main.GlobalTime/99f;

					Color color = Color.Lerp(Color.Magenta, Color.MediumAquamarine, fraction);
					Color color2 = Color.Lerp(Color.Magenta, Color.MediumAquamarine, fractionPlus);

					vertices[0 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], color, new Vector2(0,1));
					vertices[1 + (k * 6)] = new VertexPositionColorTexture(drawtop + right, color2, new Vector2(1,0));
					vertices[2 + (k * 6)] = new VertexPositionColorTexture(drawtop + left, color2, new Vector2(0,0));

					vertices[3 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], color, new Vector2(0,1));
					vertices[4 + (k * 6)] = new VertexPositionColorTexture(prevcoords[1], color, new Vector2(1,1));
					vertices[5 + (k * 6)] = new VertexPositionColorTexture(drawtop + right, color2, new Vector2(1,0));

					prevcoords = new Vector3[2] { drawtop + left, drawtop + right };

					//Idglib.DrawTether(SGAmod.ExtraTextures[21], prev2, goto2, 1f, 0.25f, 1f, Color.Magenta);

					prev2 = goto2;

				}

				vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
				vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

				Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

				RasterizerState rasterizerState = new RasterizerState();
				rasterizerState.CullMode = CullMode.None;
				Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

				foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
					Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0,((totalcount+1)*2));
				}

				for (float i = 0; i < 8; i += 0.5f)
				{
					float angle = master.rotation + projectile.velocity.ToRotation() + ((-MathHelper.Pi / 40f) * (float)i)*master.direction;
					Vector2 vector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
					Vector2 center = master.Center + vector * projectile.ai[0];
					Texture2D tex = Main.projectileTexture[projectile.type];
					spriteBatch.Draw(tex, center - Main.screenPosition, null, lightColor*(1f-(i/ 8f)), angle, new Vector2(14, master.direction < 0 ? 14 : tex.Height-14), projectile.scale, master.direction < 0 ? SpriteEffects.FlipVertically : SpriteEffects.None, 0f);
				}
			}
				return false;
        }
    }

	public class LaserLance : SeriousSamWeapon,ITechItem
	{
		public float ElectricChargeScalingPerUse() => 1f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Lance");
			Tooltip.SetDefault("Spin a Space Lance around yourself to damage enemies\nWhen it hits an enemy it shoots a piercing laser in their direction, doing half the damage");
		}

		public override void SetDefaults()
		{
			item.damage = 18;
			item.crit = 0;
			item.melee = true;
			item.width = 34;
			item.height = 24;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.knockBack = 1;
			item.value = 100000;
			item.rare = 4;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("LaserLanceProjectile");
			item.shootSpeed = 7f;
			item.UseSound = SoundID.Item1;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.MeteoriteBar, 10);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class LaserLanceProjectile : ModProjectile
	{
		private float[] oldRot = new float[6];
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Lance");
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 36;
			projectile.height = 36;
			projectile.melee = true;
			projectile.tileCollide = false;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.ai[0] > 5f)
			{
				Vector2 oldPos = projectile.Center + new Vector2(0, -60);
				oldPos = oldPos.RotatedBy(projectile.rotation-MathHelper.PiOver2);
				oldPos.Normalize();
				oldPos *= 64f;

				projHitbox.X = (int)(projectile.Center.X + oldPos.X - 25);
				projHitbox.Y = (int)(projectile.Center.Y + oldPos.Y - 25);
				projHitbox.Width = 50;
				projHitbox.Width = 50;
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{

			Vector2 oldPos = projectile.Center + new Vector2(0, -60);
			oldPos = oldPos.RotatedBy(projectile.rotation - MathHelper.PiOver2);
			oldPos.Normalize();
			oldPos *= 140f;

			Vector2 torot = oldPos;
			torot.Normalize();

			int thisoned = Projectile.NewProjectile(projectile.Center + oldPos, torot * 8f, ProjectileID.ScutlixLaser, (int)(projectile.damage / 2), knockback, Main.myPlayer);
			//Main.projectile[thisoned].usesLocalNPCImmunity = true;
			//Main.projectile[thisoned].localNPCHitCooldown = -1;
			Main.projectile[thisoned].penetrate = 2;
			Main.projectile[thisoned].knockBack = 0f;
			IdgProjectile.Sync(thisoned);

			Main.PlaySound(SoundID.Item91, projectile.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/LaserLance");
			Vector2 drawOrigin = new Vector2(tex.Width / 2f, tex.Height + 4);

			int len = Math.Min(oldRot.Length, 1 + (int)projectile.ai[0]);

			//oldPos.Length - 1
			for (int k = (len - 1); k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(len + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, null, lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public override string Texture
		{
			get { return "SGAmod/Items/Weapons/AvaliScythe"; }
		}

		public override void AI()
		{
			for (int k = oldRot.Length - 1; k > 0; k--)
			{
				oldRot[k] = oldRot[k - 1];
			}
			oldRot[0] = projectile.rotation;

			Player basep = Main.player[projectile.owner];

			if (basep == null || basep.dead)
			{
				projectile.Kill();
				return;
			}
			if ((!basep.channel || basep.dead) && projectile.ai[1] < 1)
			{

				projectile.Kill();
				return;
			}


			if (projectile.ai[1] < 1)
			{
				projectile.ai[0] = Math.Min(projectile.ai[0] + 0.1f, 16f);
				basep.itemAnimation = 5;
				basep.itemTime = 5;
				projectile.timeLeft = 30;
			}
			if (projectile.ai[1] < 1)
			{
				if (projectile.owner == Main.myPlayer)
				{
					Vector2 mousePos = Main.MouseWorld;
					Vector2 diff = mousePos - basep.Center;
					diff.Normalize();
					projectile.velocity = diff;
					basep.direction = Main.MouseWorld.X > basep.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				basep.heldProj = projectile.whoAmI;

				projectile.position -= projectile.velocity;
				//basep.bodyFrame.Y = basep.bodyFrame.Height * (2 + (((int)(projectile.localAI[0] / 40)) % 3));
				basep.itemRotation = ((projectile.rotation + MathHelper.ToRadians(-45)).ToRotationVector2()).ToRotation();
				projectile.Center = basep.Center;
			}

			projectile.localAI[1] += (1 + projectile.ai[0] / 8);
			projectile.localAI[0] += (projectile.ai[0] + 5f);
			if (projectile.localAI[1] > 60)
			{
				projectile.localAI[1] = 0;
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1f, projectile.ai[0] / 30f);
			}
			projectile.rotation = MathHelper.ToRadians(projectile.localAI[0] * projectile.direction);
			//projectile.damage = (int)(subdamage * (1+(projectile.ai[0] / 15f)));

		}


	}

}

namespace SGAmod.HavocGear.Items.Weapons
{

	public class RedPhasebrand : AvaliScythe,ITechItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Phasebrand");
			Tooltip.SetDefault("Spins a duo-phase Phasesaber around the player that speeds up over time\nShoots lasers from both sides on hit" +
				"\nAfter holding for a short while, release to throw the weapon\nWhen thrown, Does more damage based on charge and deals throwing damage" + "\n'The Darkside of close combat...'");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 35;
			item.melee = true;
			item.width = 34;
			item.height = 24;
			item.useTime = 12;
			item.useAnimation = 12;
			item.useStyle = 5;
			item.knockBack = 8;
			item.value = 100000;
			item.rare = 5;
			item.shootSpeed = 8f;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.shoot = mod.ProjectileType("RedPhasebrandProjectile");
			item.shootSpeed = 7f;
			item.UseSound = SoundID.Item8;
			item.channel = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("AvaliScythe"), 1);
			recipe.AddIngredient(mod.ItemType("LaserLance"), 1);
			recipe.AddIngredient(ItemID.RedPhasesaber, 1);
			recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}

	public class RedPhasebrandProjectile : AvaliScytheProjectile, IDrawAdditive
	{
		protected override float maxSpin => 24f;
		protected override float chargeUpRate => 0.075f;
		protected override float spinMulti => 0.5f;
		protected override float throwMulti => 0.5f;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Red Phasebrand");
		}
		public override string Texture
		{
			get { return "SGAmod/Items/GlowMasks/RedPhasebrand_Glow"; }
		}
		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = -1;
			projectile.light = 0f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.extraUpdates = 1;
			drawHeldProjInFrontOfHeldItemAndArms = false;
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			if (projectile.ai[0] < 10)
				return false;

			Vector2[] oldPos = new Vector2[2];
			oldPos[0] = projectile.Center + new Vector2(48, 0);
			oldPos[0] = oldPos[0].RotatedBy(MathHelper.ToRadians(45 + (projectile.direction > 0 ? 90 : 0)));
			oldPos[0] = oldPos[0].RotatedBy(projectile.rotation);
			oldPos[0].Normalize();
			oldPos[0] *= 48f;
			oldPos[1] = oldPos[0].RotatedBy(MathHelper.ToRadians(180));

			foreach (Vector2 position in oldPos)
			{
				projHitbox.X = (int)(projectile.Center.X + position.X);
				projHitbox.Y = (int)(projectile.Center.Y + position.Y);
				if (projHitbox.Intersects(targetHitbox))
				{
					return true;
				}
			}
			return false;
		}

		protected override void PlaySound()
		{
			if (projectile.localAI[1] > spinSoundDelay)
			{
				projectile.localAI[1] = 0;
				Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 15, 1f, projectile.ai[0] / 40f);
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{

			for (float u = 0; u < MathHelper.TwoPi; u += MathHelper.Pi)
			{
				Vector2 oldPos = new Vector2(1f, 0);
				oldPos = oldPos.RotatedBy(u + (projectile.rotation + (0f)));
				oldPos.Normalize();
				oldPos *= 24f;

				Vector2 torot = oldPos;
				torot.Normalize();

				int thisoned = Projectile.NewProjectile(projectile.Center + oldPos, torot * 8f, ProjectileID.ScutlixLaser, (int)(projectile.damage / 3), knockback, Main.myPlayer);
				//Main.projectile[thisoned].usesLocalNPCImmunity = true;
				//Main.projectile[thisoned].localNPCHitCooldown = -1;
				Main.projectile[thisoned].penetrate = 2;
				Main.projectile[thisoned].knockBack = 0f;
				Main.projectile[thisoned].usesIDStaticNPCImmunity = true;
				Main.projectile[thisoned].idStaticNPCHitCooldown = 30;
				Main.projectile[thisoned].netUpdate = true;
				IdgProjectile.Sync(thisoned);
			}

			Main.PlaySound(SoundID.Item91, projectile.Center);
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width / 2, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, tex.Width / 2, tex.Height), lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				//spriteBatch.Draw(tex, drawPos, new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height), Color.White * MathHelper.Clamp(projectile.ai[0] / 15f, 0f, 1f) * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
			return false;
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{

			Texture2D tex = Main.projectileTexture[projectile.type];
			Vector2 drawOrigin = new Vector2(tex.Width / 2, tex.Height) / 2f;

			//oldPos.Length - 1
			for (int k = oldRot.Length - 1; k >= 0; k -= 1)
			{
				Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
				//Color color = Color.Lerp(Color.Lime, lightColor, (float)k / (oldPos.Length + 1));
				float alphaz = (1f - (float)(k + 1) / (float)(oldRot.Length + 2)) * 1f;
				//spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, tex.Width / 2, tex.Height), lightColor * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
				spriteBatch.Draw(tex, drawPos, new Rectangle(tex.Width / 2, 0, tex.Width / 2, tex.Height), Color.White * MathHelper.Clamp(projectile.ai[0] / 15f, 0f, 1f) * alphaz, oldRot[k], drawOrigin, projectile.scale, projectile.direction < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f);
			}
		}
	}

}
