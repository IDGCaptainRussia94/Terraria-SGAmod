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

namespace SGAmod.Items.Weapons
{
	public class ShadeflameStaff : CorruptedTome
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadeflame Staff");
			Tooltip.SetDefault("Spews a stream of Shadow Flames\nFlames bounce off walls and fall to the ground for a while");
		}

		public override void SetDefaults()
		{
			item.damage = 4;
			item.magic = true;
			item.width = 40;
			item.height = 20;
			item.mana = 5;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0.0f;
			item.value = 25000;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			//item.UseSound = SoundID.Item34;
			item.shootSpeed = 4f;
			Item.staff[item.type] = true;
			item.shoot = ModContent.ProjectileType<ShadeFlameProj>();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.itemAnimation%4==0)
				Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 34,1f,Main.rand.NextFloat(-0.25f,0.25f));
			Vector2 formerposition = position;
				position += Vector2.Normalize(new Vector2(speedX, speedY))*42f;
			if (Collision.CanHit(position, 3, 3, formerposition, 3, 3))
			{
				int probg = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				Main.projectile[probg].velocity.X = perturbedSpeed.X;
				Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			}
			return false;
		}
	}

	public class ShadeFlameProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowflame");
		}

		public override void SetDefaults()
		{
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;
			projectile.width = 4;
			projectile.height = 4;
			aiType = ProjectileID.Bullet;
			projectile.aiStyle = 0;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.light = 0.1f;
			projectile.timeLeft = 90;
			projectile.magic = true;
			projectile.extraUpdates = 1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.ai[0] > 0)
				return false;
			return base.CanHitNPC(target);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{

			if (projectile.velocity.X != oldVelocity.X)
			{
				projectile.velocity.X = -oldVelocity.X * 0.5f;
			}
			if (projectile.velocity.Y != oldVelocity.Y)
			{
				projectile.velocity.Y = -oldVelocity.Y / 3f;
			}

			if (projectile.ai[0] == 0)
			{
				projectile.ai[0] = 1;
				projectile.timeLeft += 300;
				projectile.netUpdate = true;
			}

			return false;
		}

		public override void AI()
		{
			projectile.localAI[0] += 1;


			if (projectile.ai[0] > 0)
			{
				projectile.velocity.Y += 0.1f;
				projectile.velocity.X /= 1.15f;
			}

			if ((int)projectile.localAI[0] % 10 == 0)
			{
				foreach (NPC npc in Main.npc)
				{
					if (!npc.dontTakeDamage && !npc.friendly && !npc.townNPC)
					{
						Rectangle rec1 = new Rectangle((int)projectile.position.X-24, (int)projectile.Center.Y-48, projectile.width+48, (int)projectile.height + 64);
						Rectangle rec2 = new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height);
						if (rec1.Intersects(rec2))
						{
							npc.AddBuff(BuffID.ShadowFlame, 60 * 2);
						}

					}
				}

			}
			Dust num126;
			if (projectile.ai[0] < 1)
			{
				num126 = Dust.NewDustPerfect(projectile.Center, 62, projectile.velocity + new Vector2(Main.rand.NextFloat(-1, 1), Main.rand.NextFloat(-8, 0)), 0, Color.White, 3f);
				num126.noGravity = true;
				num126.velocity *= 0.25f;
			}

			num126 = Dust.NewDustPerfect(new Vector2(projectile.Center.X, projectile.Center.Y) + new Vector2(10 - Main.rand.Next(0, 20), 10 - Main.rand.Next(0, 20)), 173, projectile.velocity, 0, Color.Blue, 1f);
			num126.noGravity = false;
			num126.velocity += Vector2.Normalize(num126.position-projectile.Center)+new Vector2(Main.rand.NextFloat(-6f, 6f)* projectile.ai[0], Main.rand.NextFloat(-projectile.ai[0]*6,0));

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override string Texture => "SGAmod/Invisible";

	}
		public class CorruptedTome : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Corrupted Tome");
			Tooltip.SetDefault("Spews a stream of Cursed Flames\nInflicts Cursed Inferno for far longer than usual and inflicts Everlasting Suffering\nEverlasting Suffering increases damage over time by 250% and makes other debuffs last until it ends");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.width = 40;
			item.height = 20;
			item.useTime = 5;
			item.mana = 8;
			item.useAnimation = 15;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true; //so the item's animation doesn't do damage
			item.knockBack = 0.20f;
			item.value = 100000;
			item.rare = ItemRarityID.Lime;
			item.autoReuse = true;
			item.UseSound = SoundID.Item34;
			item.shootSpeed = 8f;
			item.shoot = ProjectileID.EyeFire;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.CursedFlames, 1);
			recipe.AddIngredient(ItemID.Flamethrower, 1);
			recipe.AddIngredient(ItemID.SpectreBar, 8);
			recipe.AddIngredient(ItemID.CursedFlame, 10);
			recipe.AddIngredient(ItemID.SpellTome, 1);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int probg = Projectile.NewProjectile(position.X + (int)(speedX * 0f), position.Y + (int)(speedY * 0f), speedX, speedY, type, damage, knockBack, player.whoAmI);
			Main.projectile[probg].ranged = false;
			Main.projectile[probg].magic = true;
			Main.projectile[probg].melee = false;
			Main.projectile[probg].friendly = true;
			Main.projectile[probg].hostile = false;
			Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(7));
			Main.projectile[probg].velocity.X = perturbedSpeed.X;
			Main.projectile[probg].velocity.Y = perturbedSpeed.Y;
			IdgProjectile.AddOnHitBuff(probg, BuffID.CursedInferno, 60 * 20);
			IdgProjectile.AddOnHitBuff(probg, mod.BuffType("EverlastingSuffering"), 60 * 7);
			IdgProjectile.Sync(probg);
			return false;
		}
	}

}
