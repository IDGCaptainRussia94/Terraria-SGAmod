using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using System.IO;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;
using SGAmod.Projectiles;

namespace SGAmod.Items.Weapons
{

	public class SolisNova : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Solis Nova");
			Tooltip.SetDefault("Conjures the power of a gravitational Galaxy through channeling mana\nAfter charging enough, gains the ability to slow enemies down and pull them in\nGravity is based on Galaxy size, enemy distance, and enemy knockback resistance\nWhen fully charged, releasing dispurses the nebula over a massive area\n'fear the mighty pull of the Pastel-Colored Galaxy!'");
		}

		public override void SetDefaults()
		{
			item.damage = 75;
			item.magic = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 18;
			item.value = 75000;
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item100;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("SolisNovaproj");
			item.shootSpeed = 16f;
			item.mana = 15;
			item.channel = true;
			Item.staff[item.type] = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/RodofEnforcement_Glow");
			}
		}

		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-18, -4);
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("FieryMoon"), 1);
			recipe.AddIngredient(ItemID.LunarBar, 12);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddIngredient(ItemID.FragmentSolar, 8);
			recipe.AddIngredient(ItemID.FragmentNebula, 10);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			//Vector2 normz = new Vector2(speedX, speedY); normz.Normalize();
			//position += normz * 24f;

			
			return true;
		}
	}



	public class SolisNovaproj : ModProjectile
	{
		float growsize = 16f;
		bool largeenough = false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rod of Enforcing");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_" + 14); }
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.light = 0.5f;
			projectile.width = 64;
			projectile.height = 64;
			projectile.penetrate = -1;
			projectile.magic = true;
			projectile.tileCollide = false;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (growsize<32f)
			return false;
			return null;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item45, projectile.Center);

			return true;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			damage = (int)((float)damage * (1f+(Math.Min(growsize, 250f) /30f)));
		}

		public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
		{
			Vector2 there = targetHitbox.Center.ToVector2() - projectile.Center;
			there.Normalize();
			there *= growsize * 1.4f;
			there += projectile.Center;

			float point = 0f;
			// Run an AABB versus Line check to look for collisions, look up AABB collision first to see how it works
			// It will look for collisions on the given line using AABB
			return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), projectile.Center,
				there, 28, ref point) || projHitbox.Intersects(targetHitbox);
		}

		public override void AI()
		{

			//int DustID2 = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, mod.DustType("AcidDust"), projectile.velocity.X * 1f, projectile.velocity.Y * 1f, 20, default(Color), 1f);

			bool cond = projectile.ai[1] < 1 || projectile.timeLeft == 2 || (growsize >= 64f && !largeenough);
			for (int num621 = 0; num621 < (cond ? (projectile.timeLeft == 2 && growsize>160 ? 1500 : 100) : 2); num621++)
			{

				Vector2 vectz = new Vector2(Main.rand.Next(-9000, 9000), Main.rand.Next(-9000, 9000));
				vectz.Normalize();

				float aversize = growsize;

				int num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y)+(vectz*Main.rand.NextFloat(0f,1f)* aversize), 0, 0, 6, vectz.X * (cond ? 1f : 0f), vectz.Y * (cond ? 1f : 0f), 80, Main.hslToRgb(((Main.GlobalTime / 10f) + 0.50f) % 1f, 1f, 0.75f), 1f);
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;

				vectz = new Vector2(Main.rand.Next(-9000, 9000), Main.rand.Next(-9000, 9000));
				vectz.Normalize();

				num622 = Dust.NewDust(new Vector2(projectile.Center.X, projectile.Center.Y) + (vectz * Main.rand.NextFloat(0.75f, 1.2f)* (aversize +64f)), 0, 0, 6, Vector2.Zero.X, Vector2.Zero.Y, 80, Main.hslToRgb(((Main.GlobalTime / 10f) + 0f) % 1f, 1f, 0.75f), 1f);
				Main.dust[num622].velocity = -vectz* Main.rand.NextFloat(0.5f, 3f)*(cond ? -3f : 1f);
				if (Main.rand.Next(2) == 0)
				{
					Main.dust[num622].scale = 0.5f;
					Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
				}
				Main.dust[num622].noGravity = true;
			}

			Player player = Main.player[projectile.owner];
			projectile.ai[1] += 1;
			projectile.position -= projectile.velocity;

			if (player.dead)
			{
				projectile.Kill();
			}
			else
			{

				player.manaRegenDelay = (int)(player.maxRegenDelay * 30);

				if (growsize >= 64f)
				{
					if (!largeenough)
					Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 73, 1f, -0.50f);

					largeenough = true;

					for (int i = 0; i < Main.maxNPCs; i += 1)
					{
						NPC guy = Main.npc[i];
						if (guy != null && guy.active && !guy.friendly && !guy.townNPC && guy.CanBeChasedBy())
						{
							Vector2 there = guy.Center - projectile.Center;
							there.Normalize();
							float distance = (guy.Center - projectile.Center).Length();
							float maxdist = (160f + growsize * 2f);
							if (distance < maxdist)
							{
								guy.GetGlobalNPC<SGAnpcs>().TimeSlow += (0.20f + (growsize / 100f)) * (1f - (distance / maxdist));

								float point = 0f;

								if (Collision.CanHitLine(guy.Center, 8, 8, projectile.Center, 8, 8))
								{
									float speed = (1f + (growsize / 10f));
									guy.position -= there * Math.Min(distance, speed * MathHelper.Clamp(guy.knockBackResist * 3f, 0.10f, 1f) * (1f - (distance / maxdist)));
								}
							}
						}
					}
				}

				if (((projectile.ai[0] > 0 || player.statMana < 1) || !player.channel) && projectile.ai[1]>1)
				{
					projectile.ai[0] += 1;
					if (projectile.ai[0] == 1)
					{
						Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 113, 0.5f, -0.25f);

					}
					if (growsize < 159f)
					{
						growsize *= 0.98f;
					}
					else
					{
						growsize += 5f;
					}
				}


				else
				{
					if (projectile.ai[0] < 1)
					{

						if (projectile.ai[1] % 6 == 0)
						{
							player.statMana -= (int)(0.20f + (growsize / 20f) * player.manaCost);
							if (player.statMana < 1 && player.manaFlower)
								player.QuickMana();
						}

						growsize += 0.25f;

						if (player.HasBuff(BuffID.ManaSickness))
							growsize *= 0.997f;

						if (growsize>160f)
						growsize = 160f;

						Vector2 mousePos = Main.MouseWorld;
						if (projectile.owner == Main.myPlayer && projectile.ai[1] < 2)
						{
							projectile.Center = mousePos;
							projectile.netUpdate = true;
						}
						if (projectile.owner == Main.myPlayer && mousePos!= projectile.Center)
						{
							Vector2 diff2 = mousePos - projectile.Center;
							diff2.Normalize();
							projectile.velocity = diff2 * 0f;
							projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
							projectile.netUpdate = true;

						}

						projectile.timeLeft = 80;

						int dir = projectile.direction;
						player.ChangeDir(dir);
						player.itemTime = 40;
						player.itemAnimation = 38;


						Vector2 distz = projectile.Center - player.Center;
						player.itemRotation = (float)Math.Atan2(distz.Y * dir, distz.X*dir);
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			Texture2D sun = SGAmod.ExtraTextures[100];
			//Main.spriteBatch.End();
			//Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.ZoomMatrix);

			float growth = ((growsize / 3f) + 1f);
			float alpha2 = 1f;

			for (float i = 1; i < Math.Min(growth, 50f); i += 0.25f)
			{
				float scale = 1f + Math.Max(growsize-160f, 0f)/60f;
				float timefraq = (float)projectile.timeLeft / 80f;
				float alpha = MathHelper.Clamp(((growsize / 3f) + 1f)/(i+ 0.25f),0f, 1f);
				spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Main.hslToRgb((((Main.GlobalTime/10f)+0.50f)+ i/75f) %1f,1f,0.75f) * (alpha2 * (0.05f * alpha)), (-Main.GlobalTime+i / 10f), new Vector2(sun.Width / 2f, sun.Height / 2f), (new Vector2(i*timefraq, i*0.75f)/4f)*(Math.Min(growsize/64f,1f))* scale, SpriteEffects.None, 0f);
				spriteBatch.Draw(sun, projectile.Center - Main.screenPosition, null, Main.hslToRgb(((Main.GlobalTime / 10f) + i / 75f) %1f, 1f, 0.75f) * (alpha2 * (0.05f * alpha)), (-Main.GlobalTime + i/10f)+(float)Math.PI, new Vector2(sun.Width / 2f, sun.Height / 2f), (new Vector2(i*0.75f, i*timefraq)/4f)*(Math.Min(growsize / 64f, 1f))* scale, SpriteEffects.None, 0f);
			}

			return false;
		}

		//public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//	base.PostDraw(spriteBatch, lightColor);
		//}

	}



}
