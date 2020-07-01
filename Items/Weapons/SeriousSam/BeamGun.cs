using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;
using System;
using System.Collections.Generic;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Idglibrary;


namespace SGAmod.Items.Weapons.SeriousSam
{

	public class SeriousSamWeapon : ModItem
	{

		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(SeriousSamWeapon);
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			// Get the vanilla damage tooltip
			TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
			if (tt != null)
			{
				string[] thetext = tt.text.Split(' ');
				string newline = "";
				List<string> valuez = new List<string>();
				foreach (string text2 in thetext)
				{
					valuez.Add(text2 + " ");
				}
				valuez.Insert(1, "Technological ");
				foreach (string text3 in valuez)
				{
					newline += text3;
				}
				tt.text = newline;
			}
		}
		public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			//add += player.GetModPlayer<SGAPlayer>().techdamage-1f;
			mult = mult*player.GetModPlayer<SGAPlayer>().techdamage;
		}
	}


	public class BeamGun : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
            DisplayName.SetDefault("Beam Gun");
            Tooltip.SetDefault("Fires a beam that rebounds off walls and enemies to other enemies, up to 5 times\nThese rebounds do not crit");
			SGAmod.UsesPlasma.Add(SGAmod.Instance.ItemType("BeamGun"), 1000);
		}

        public override void SetDefaults()
        {
            item.damage = 90;
            item.magic = true;
            item.width = 48;
            item.height = 28;
            item.useTime = 5;
            item.useAnimation = 5;
            item.useStyle = 5;
            item.noMelee = true;
            item.knockBack = 0;
            item.value = 1000000;
			item.rare = 9;
            item.autoReuse = true;
            item.shoot = ModContent.ProjectileType<BeamGunHolding>();
            item.shootSpeed = 1f;
			item.channel = true;
			item.noUseGraphic = true;
        }

		public override bool CanUseItem(Player player)
		{
			if (player.ownedProjectileCounts[ModContent.ProjectileType<BeamGunHolding>()] > 0)
				return false;
			SGAPlayer modply = player.GetModPlayer<SGAPlayer>();
			return (modply.RefilPlasma());
		}

		public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6, 0);
        }

		public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "CryostalBar", 15);
			recipe.AddIngredient(null, "PlasmaCell", 10);
			recipe.AddIngredient(null, "AdvancedPlating", 15);
			recipe.AddIngredient(ItemID.HeatRay, 1);
			recipe.AddIngredient(mod.ItemType("OmegaSigil"), 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
            recipe.SetResult(this);
      		recipe.AddRecipe();
        }

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int numberProjectiles = 1;
			position = player.Center;
			Vector2 offset = new Vector2(speedX, speedY);
			offset.Normalize();
			offset *= 16f;
			//position += offset;


				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
				float scale = 1f;// - (Main.rand.NextFloat() * .2f);
				perturbedSpeed = perturbedSpeed * scale; 
				int prog=Projectile.NewProjectile(position.X+ offset.X, position.Y+ offset.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI);


			return false;
		}
	}

	public class BeamGunHolding : ModProjectile
	{
		public virtual bool bounce => true;
		//public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Gun");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.timeLeft = 3;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override void AI()
		{
			projectile.localAI[0] += 1f;

			Player player = Main.player[projectile.owner];

			if (player != null && player.active)
			{

				SGAPlayer modply = player.GetModPlayer<SGAPlayer>();

				if (projectile.ai[0] > 0 || !player.channel || player.dead || modply.plasmaLeftInClip<1)
				{
					if (modply.plasmaLeftInClip < 1)
					{
						player.itemTime = 120;
						player.itemAnimation = 120;
					}
					projectile.Kill();
				}
				else
				{

					Vector2 mousePos = Main.MouseWorld;

					if (projectile.owner == Main.myPlayer)
					{
						Vector2 diff = mousePos - player.Center;
						diff.Normalize();
						projectile.velocity = diff;
						projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						projectile.netUpdate = true;
					}

					int dir = projectile.direction;
					player.ChangeDir(dir);
					projectile.direction = dir;

					player.heldProj = projectile.whoAmI;
					player.itemRotation = (float)Math.Atan2(projectile.velocity.Y * dir, projectile.velocity.X * dir);
					projectile.rotation = player.itemRotation-MathHelper.ToRadians(90);
					projectile.Center = (player.Center+new Vector2(dir*6, 0))+ (projectile.velocity*10f);


					modply.plasmaLeftInClip -= 1;


					projectile.position -= projectile.velocity;
					projectile.timeLeft = 3;
					player.itemAnimation = 3;
					player.itemTime = 3;
					Vector2 position = projectile.Center;
					Vector2 offset = new Vector2(projectile.velocity.X, projectile.velocity.Y);
					offset.Normalize();
					offset *= 16f;

					Vector2 perturbedSpeed = projectile.velocity.RotatedByRandom(MathHelper.ToRadians(0));
					float scale = 1f;// - (Main.rand.NextFloat() * .2f);
					perturbedSpeed = perturbedSpeed * scale;
					int prog = Projectile.NewProjectile(position.X + offset.X, position.Y + offset.Y, perturbedSpeed.X, perturbedSpeed.Y, ModContent.ProjectileType<BeamGunProjectile>(), projectile.damage, projectile.knockBack, player.whoAmI);
					//IdgProjectile.Sync(prog);
				}
			}
			else
			{
				projectile.Kill();
			}

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{


			Texture2D tex = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProj");
			Texture2D texGlow = ModContent.GetTexture("SGAmod/Items/Weapons/SeriousSam/BeamGunProjGlow");
			SpriteEffects effects = SpriteEffects.FlipHorizontally;
			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 2) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 0f);
			Color color = projectile.GetAlpha(lightColor) * 1f; //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = (int)(projectile.localAI[0] / 3f);
			timing %= 2;
			timing *= ((tex.Height) / 2);
			spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 2), color, projectile.rotation, drawOrigin, projectile.scale, projectile.direction<1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);
			spriteBatch.Draw(texGlow, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 2), Color.White, projectile.rotation, drawOrigin, projectile.scale, projectile.direction < 1 ? effects : (SpriteEffects.FlipVertically | SpriteEffects.FlipHorizontally), 0f);

			return false;
		}


	}


	public class BeamGunProjectile : ModProjectile
	{
		public virtual bool bounce => true;
		//public virtual float trans => 1f;
		public Player P;
		public Vector2 lasthit;
		public List<int> bouncetargets = new List<int>();
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Gun");
		}

		public List<int> GetAllActive()
		{
			List<int> starter=new List<int>();

			for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
			{
				//(Main.npc[num172].CanBeChasedBy(projectile, false)
				if (Main.npc[num172].active && !Main.npc[num172].friendly)
				if (!Main.npc[num172].dontTakeDamage && !Main.npc[num172].townNPC && Main.npc[num172].CanBeChasedBy())
				starter.Add(num172);



			}
			return starter;
		}

				public NPC FindClosestTarget(Vector2 loc, Vector2 size, List<int> them, bool block = true, bool friendlycheck = true, bool chasecheck = false)
		{
			int num;
			float num170 = 1000000;
			NPC num171 = null;

			for (int num1722 = 0; num1722 < them.Count; num1722 +=1)
				{
				int num172 = them[num1722];
					float num173 = Main.npc[num172].position.X + (float)(Main.npc[num172].width / 2);
					float num174 = Main.npc[num172].position.Y + (float)(Main.npc[num172].height / 2);
					float num175 = Math.Abs(loc.X + (float)(size.X / 2) - num173) + Math.Abs(loc.Y + (float)(size.Y / 2) - num174);
					if (Main.npc[num172].active)
					{

					//(Collision.CanHit(new Vector2(loc.X, loc.Y), 1, 1, Main.npc[num172].position, Main.npc[num172].width, Main.npc[num172].height) || block == false)
					if (num175 < num170)
						{
						int result = bouncetargets.Find(x => x == num172);
						if (result < 1)
						{
							num170 = num175;
							num171 = Main.npc[num172];
						}
						}
					}
				}
			if (num170 > 400)
				return null;

			return num171;

		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.magic = true;
			projectile.timeLeft = 1000;
			projectile.penetrate = 2;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.scale = 0.5f;
			projectile.extraUpdates = 1000;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.timeLeft < 2)
				return false;

			return base.CanHitNPC(target);
		}

		public override bool CanHitPlayer(Player target)
		{
			if (projectile.timeLeft < 2)
				return false;
				
				return base.CanHitPlayer(target);
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			lasthit = projectile.Center;
			target.immune[projectile.owner] = 5;
			bouncetargets.Add(target.whoAmI);
			projectile.Kill();
		}


		public bool onlyonce = true;

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			lasthit = projectile.Center;
			projectile.Kill();
			return false;
		}

		public override bool PreKill(int timeLeft)
		{

			if (!onlyonce)
			return false;

			int prog = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, 0, 0, mod.ProjectileType("BeamGunProjectileVisual"), 0, 0, Main.player[projectile.owner].whoAmI);
			Main.projectile[prog].ai[0] = projectile.localAI[0];
			Main.projectile[prog].ai[1] = projectile.localAI[1];
			Main.projectile[prog].netUpdate = true;

			List<int> them = GetAllActive();

			if (lasthit!=null) {
			projectile.Center = lasthit;
			Vector2 lastpos = projectile.Center;

				for (int i = 0; i < 5; i += 1)
				{
					NPC him = FindClosestTarget(lastpos, new Vector2(0, 0), them);
					if (him != null)
					{
						int prog2 = Projectile.NewProjectile(him.Center.X, him.Center.Y, 0, 0, mod.ProjectileType("BeamGunProjectileVisual"), 0, 0, Main.player[projectile.owner].whoAmI);
						Main.projectile[prog2].ai[0] = lastpos.X;
						Main.projectile[prog2].ai[1] = lastpos.Y;
						Main.projectile[prog2].netUpdate = true;
						if (him.immune[projectile.owner] < 1)
						{
							int daxmage = Main.DamageVar((float)projectile.damage);
							Main.player[projectile.owner].ApplyDamageToNPC(him, daxmage, projectile.knockBack, him.Center.X > lastpos.X ? 1 : -1, false);
							//him.StrikeNPC(projectile.damage, projectile.knockBack, (him.Center.X > lastpos.X ? 1 : -1));
							him.immune[projectile.owner] = 8;
							if (Main.netMode != 0)
							{
								NetMessage.SendData(MessageID.StrikeNPC, -1, -1, null, him.whoAmI, (float)daxmage, 16f, (float)1, 0, 0, 0);
							}
						}
						bouncetargets.Add(him.whoAmI);
						lastpos = him.Center;
					}

				}

			}
			onlyonce = false;
			return true;
		}

		public override void AI()
		{

			if (projectile.localAI[1] == 0)
			{
				//HalfVector2 half = new HalfVector2(projectile.Center.X, projectile.Center.Y);
				projectile.localAI[0] = projectile.Center.X;
				projectile.localAI[1] = projectile.Center.Y;
			}


		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}


	}

	public class BeamGunProjectileVisual : ModProjectile
	{
		public virtual bool bounce => true;
		//public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Beam Gun");
		}

		public override bool? CanHitNPC(NPC target)
		{
			return false;
		}

		public override bool CanHitPlayer(Player target)
		{
			return false;
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 24;
			projectile.height = 24;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.magic = true;
			projectile.timeLeft = 2;
			projectile.penetrate = -1;
			aiType = ProjectileID.WoodenArrowFriendly;
			projectile.damage = 0;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Vector2 basepoint = new Vector2(projectile.ai[0], projectile.ai[1]);

			int width = 32;int height = 1000;

			Texture2D beam = new Texture2D(Main.graphics.GraphicsDevice, width, height);
			var dataColors = new Color[width * height];


			///


			for (int y = 0; y < height; y++)
			{
				for (float x = 0f; x < 5f; x++)
				{
					float offset2 = MathHelper.ToRadians((x/5f)*360f) + (y * 0.1f);
					float adder = (x * 9f)+ (y*3f);
					float beamvar = width / 4f+(((float)Math.Sin((Main.GlobalTime*23f)+ adder))*2f);
					int output = (width/2)+(int)(Math.Sin((Main.GlobalTime*-32.25f)+(offset2))*((float)beamvar));
					//output += 6;
					//Main.NewText(output);


					dataColors[output + y * width] = Color.Aquamarine;//;Main.hslToRgb(Main.rand.NextFloat(),0.75f,0.5f);



				}
			}


			///


			beam.SetData(0, null, dataColors, 0, width * height);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			Texture2D tex = ModContent.GetTexture("Terraria/Chain6");

			Vector2 drawOrigin = new Vector2(tex.Width, tex.Height / 4) / 2f;
			Vector2 drawPos = ((projectile.Center - Main.screenPosition)) + new Vector2(0f, 4f);
			Color color = Color.Lerp((projectile.GetAlpha(lightColor) * 0.5f), Color.White, 0.5f); //* ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
			int timing = 0;
			timing *= ((tex.Height) / 5);
			float alpha = 1f;
			Idglib.DrawTether(beam, basepoint, projectile.Center,alpha, 1,1,Color.White);
			//spriteBatch.Draw(tex, drawPos, new Rectangle(0, timing, tex.Width, (tex.Height - 1) / 5), color* alpha,0f, drawOrigin, projectile.scale, SpriteEffects.None, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);

			return false;
		}


	}

}
