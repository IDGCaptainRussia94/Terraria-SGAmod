using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.ModLoader.IO;
using Terraria.Enums;
using SGAmod.Items.Weapons;
using Idglibrary;
using SGAmod.Projectiles;
using System.Linq;
using SGAmod.Effects;
using System.IO;
using Terraria.DataStructures;

namespace SGAmod.Items.Weapons
{
		public class CrystalComet : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crystal Comet");
			Tooltip.SetDefault("Manifests a purple Comet that pierces infinitely, releasing shards as it flies");
		}

        public override void SetDefaults()
		{
			item.damage = 120;
			item.magic = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 40;
			item.mana = 20;
			item.crit = 10;
			item.useAnimation = 40;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0.15f;
			item.value = 100000;
			item.rare = ItemRarityID.Yellow;
			item.autoReuse = true;
			item.UseSound = SoundID.Item105;
			item.shootSpeed = 8f;
			item.shoot = ModContent.ProjectileType<PrismicShowerProj>();
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CrystalStorm, 1); 
			recipe.AddIngredient(mod.ItemType("PrismalBar"), 15);
			recipe.AddTile(mod.TileType("PrismalStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 0f), position.Y + (int)(speedY * 0f), speedX, speedY, type, damage, knockBack, player.whoAmI,ai0: Main.rand.Next(600));
			Main.projectile[probg].ranged = false;
			Main.projectile[probg].magic = true;
			Main.projectile[probg].melee = false;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(0));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			//IdgProjectile.AddOnHitBuff(probg, BuffID.CursedInferno, 60 * 20);
			IdgProjectile.Sync(probg);
			return false;
		}
	}

	public class PrismicShowerProj : ModProjectile
	{
		float strength => Math.Min(projectile.timeLeft / 120f, 1f);
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Prismic Storm");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 40;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;
			projectile.width = 4;
			projectile.height = 4;
			aiType = ProjectileID.Bullet;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.light = 0.1f;
			projectile.timeLeft = 300;
			projectile.magic = true;
			projectile.extraUpdates = 3;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

        public override bool CanDamage()
        {
			return true;
        }

		public override void AI()
		{
			projectile.ai[0] += 1;

			Vector2 offset = Vector2.Normalize(projectile.velocity).RotatedBy(MathHelper.Pi/2);
			float speed = Main.rand.NextFloat(0f, 8f) * (Main.rand.NextBool() ? 1f : -1f);

			if ((int)projectile.ai[0] % 3 == 0)
			{
				Projectile proj = Projectile.NewProjectileDirect(projectile.Center + offset * Main.rand.NextFloat(-64, 64), offset * speed, ProjectileID.CrystalStorm, (int)(projectile.damage), projectile.knockBack, projectile.owner);
				proj.timeLeft = (int)(projectile.timeLeft / 5f);

			}
			Dust num126;
				num126 = Dust.NewDustPerfect(projectile.Center+(offset* Main.rand.NextFloat(-12, 12f)), 112, (offset* Main.rand.NextFloat(-9f, 3f)*(projectile.velocity.X>0 ? -1f : 1f)) + projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-8, 0)), 255-(int)(100f*strength), Color.White, 1f);
				num126.noGravity = true;

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
				return Color.Magenta;
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 13;
			trail.capsize = new Vector2(8f, 0f);
			trail.strength = strength;
			trail.trailThicknessIncrease = 15;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);

			return false;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedBolt";

	}

	public class ShootingStar : CrystalComet
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			Tooltip.SetDefault("Controls a very inspirational Star\nReleases a nova of stars when hitting past your mouse curser\n'The more you know!'");
		}

		public override void SetDefaults()
		{
			item.damage = 750;
			item.magic = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 40;
			item.mana = 20;
			item.crit = 20;
			item.useAnimation = 1;
			item.useTurn = false;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 10f;
			item.value = 100000;
			item.rare = ItemRarityID.Purple;
			item.autoReuse = true;
			item.UseSound = SoundID.Item105;
			item.shootSpeed = 12f;
			item.channel = true;
			item.shoot = ModContent.ProjectileType<ShootingStarProj>();
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = Main.itemTexture[item.type];

				item.GetGlobalItem<ItemUseGlow>().CustomDraw = delegate (Item item, PlayerDrawInfo drawInfo, Vector2 position, float angle,Color glowcolor)
				{
					Texture2D texture = SGAmod.ExtraTextures[110];
					Vector2 origin = texture.Size() / 2f;
					float timeAdvance = Main.GlobalTime*2;
					angle = drawInfo.drawPlayer.itemRotation + (drawInfo.drawPlayer.direction < 0 ? MathHelper.Pi : 0);
					Player drawPlayer = drawInfo.drawPlayer;

					Vector2 drawHere = drawPlayer.MountedCenter+(angle.ToRotationVector2())*32 - Main.screenPosition;
					for (float i = 0f; i < MathHelper.TwoPi; i += MathHelper.PiOver4)
					{
						DrawData value = new DrawData(texture, drawHere + (Vector2.One.RotatedBy(i- timeAdvance)) * 8f, null, glowcolor*0.75f*MathHelper.Clamp(drawPlayer.itemAnimation/60f,0f,1f), -MathHelper.PiOver4+(i- timeAdvance), origin, 0.25f, drawInfo.spriteEffects, 0);
						Main.playerDrawData.Add(value);
					}

				};
			}
	}
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			Color c = Main.hslToRgb((float)(Main.GlobalTime / 5f) % 1f, 0.45f, 0.65f);
			tooltips.Add(new TooltipLine(mod, "Dedicated", Idglib.ColorText(c, "Dedicated to Cringe's meme in IDG's Den")));
		}
		public override Color? GetAlpha(Color lightColor)
        {
            return lightColor.MultiplyRGBA(Color.Lerp(Color.White,Main.DiscoColor,Main.essScale-0.8f));
        }
        public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("CrystalComet"), 1);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 20);
			recipe.AddIngredient(mod.ItemType("DrakeniteBar"), 15);
			recipe.AddIngredient(mod.ItemType("StygianCore"), 2);
			recipe.AddIngredient(ItemID.FragmentSolar, 8);
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}

	public class ShootingStarProj : ModProjectile
	{
		float strength => Math.Min(projectile.timeLeft / 250f, 1f);
		Vector2 there=default;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shooting Star");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 180;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}

		public override void SetDefaults()
		{
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = false;
			projectile.width = 4;
			projectile.height = 4;
			aiType = ProjectileID.Bullet;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.light = 0.1f;
			projectile.timeLeft = 300;
			projectile.magic = true;
			projectile.extraUpdates = 10;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 2;
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.WriteVector2(there);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			there = reader.ReadVector2();
		}

        public override void AI()
		{
			projectile.ai[0] += 1;
			projectile.ai[1] += 1;

			if (projectile.ai[0] < -1000)
            {
				return;
            }

			Player player = Main.player[projectile.owner];
			if (!player.channel || player.dead)
			{
				projectile.ai[0] = -10000;
			}
			else
			{
				projectile.timeLeft = 250;
				Vector2 mousePos = Main.MouseWorld;
				player.itemTime = 60;
				player.itemAnimation = 60;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - projectile.Center;
					if (diff.Length() > 1800 || projectile.ai[1]>1200)
					{

						if (!player.CheckMana(15, true))
						{
							projectile.ai[0] = -10000;
							projectile.netUpdate = true;
							return;
						}
						

						diff.Normalize();
						projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
						Vector2 randAng = Main.rand.NextVector2CircularEdge(1500f, 1500f);
						projectile.Center = mousePos + randAng;
						projectile.velocity = Vector2.Normalize(mousePos-projectile.Center)*(projectile.velocity.Length());
						projectile.netUpdate = true;
						there = Main.MouseWorld;
						projectile.ai[1] = 0;

						Vector2 dir = (there - player.Center);
						player.ChangeDir(dir.X > 0 ? 1 : -1);
						player.itemRotation = dir.ToRotation()-(player.direction<0 ? MathHelper.Pi : 0);

						for (int i = 0; i < projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
						{
							projectile.oldPos[i] = projectile.position;
						}

					}
				}


				if (there != default)
				{
					if (Vector2.Dot(Vector2.Normalize(there - projectile.Center), Vector2.Normalize(projectile.velocity)) < -0.9f)
					{
						projectile.ai[1] = 1000;
						there = default;
						NoiseGenerator noise = new NoiseGenerator(projectile.whoAmI);
						noise.Amplitude = 5;
						noise.Frequency = 0.5;
						for (float i = 0; i < 1f; i += 1f / 15f)
						{
							Vector2 there2 = (projectile.velocity); there.Normalize(); there2 = there2.RotatedBy(i * MathHelper.TwoPi);
							int prog = Projectile.NewProjectile(projectile.Center, Vector2.Normalize(there2) * (2f + (float)noise.Noise((int)(i * 80), (int)projectile.ai[0])) * 5f, ProjectileID.HallowStar, (int)(projectile.damage / 3f), projectile.knockBack / 10f, projectile.owner);
							Main.projectile[prog].timeLeft = 20 + (int)(noise.Noise((int)(i * 40), (int)projectile.ai[0] + 800) * 50);
							Main.projectile[prog].alpha = 150;
							Main.projectile[prog].localNPCHitCooldown = -1;
							Main.projectile[prog].penetrate = 2;
							Main.projectile[prog].usesLocalNPCImmunity = true;
							Main.projectile[prog].netUpdate = true;
						}

					}
				}

			}
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
				return Main.hslToRgb((percent*1f)%1f,0.85f,0.75f);
			};
			trail.projsize = projectile.Hitbox.Size() / 2f;
			trail.coordOffset = new Vector2(0, Main.GlobalTime * -1f);
			trail.trailThickness = 13;
			trail.capsize = new Vector2(8f, 0f);
			trail.strength = strength;
			trail.trailThicknessIncrease = 15;
			trail.DrawTrail(projectile.oldPos.ToList(), projectile.Center);


			for (float xx = -3; xx < 3.5f; xx += 0.5f)
			{
				for (float i = 1f; i < 3; i += 0.4f)
				{
					Texture2D texaz = SGAmod.ExtraTextures[110];
					float scalerz = 0.85f + (float)Math.Cos(Main.GlobalTime * 1.25f * (Math.Abs(xx) + i)) * 0.3f;
					spriteBatch.Draw(texaz, (projectile.Center + ((projectile.velocity.ToRotation() + (float)Math.PI / 4f)).ToRotationVector2() * (xx * 9f)) - Main.screenPosition, null, Color.Yellow * (0.5f / (i + xx)) * 0.25f, projectile.velocity.ToRotation() + (float)Math.PI / 2f, new Vector2(texaz.Width / 2f, texaz.Height / 4f), (new Vector2(1 + i, 1 + i * 1.5f) / (1f + Math.Abs(xx))) * scalerz * projectile.scale, SpriteEffects.None, 0f);
				}
			}

			return false;
		}

		public override string Texture => "SGAmod/Items/Weapons/UnmanedBolt";

	}

}
