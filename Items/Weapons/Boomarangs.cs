using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons
{

	public class Fridgeflamarang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflamarang");
			Tooltip.SetDefault("Throws an Icey and Flaming Boomerang pair guided by a light that split apart on hit\nUsable only when less than 4 Boomerangs are active");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.damage = 50;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.noUseGraphic = true;
			item.useTime = 10;
			item.knockBack = 4f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 1;
			item.value = Item.buyPrice(gold: 15);
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<FridgeflamarangProj>();
			item.shootSpeed = 10f;
		}
		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1 && (player.ownedProjectileCounts[ProjectileID.Flamarang] + player.ownedProjectileCounts[ProjectileID.IceBoomerang] < 4);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Fridgeflame>(), 10);
			recipe.AddIngredient(ItemID.SoulofLight, 5);
			recipe.AddIngredient(ItemID.Flamarang, 1);
			recipe.AddIngredient(ItemID.IceBoomerang, 1);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		/*public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
		{
			mult *= Main.dayTime ? 0.50f : 1f;
		}*/

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed.RotatedByRandom(MathHelper.Pi / 8f);

			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, (int)(damage * 0.10f), knockBack, Main.myPlayer);

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.IceBoomerang];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.Center + new Vector2(-6, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.Flamarang], item.Center + new Vector2(6, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.FlipHorizontally, 0f);
			}
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[ItemID.IceBoomerang];
				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos + new Vector2(-6, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.Flamarang], drawPos + new Vector2(6, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.FlipHorizontally, 0f);
			}
		}

	}

	public class FridgeflamarangProj : CoralrangProj
	{
		protected override int maxOrbiters => 2;
		protected override float damageMul => 10f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Lunarang");
		}
		public FridgeflamarangProj()
		{
			orbitors = new Projectile[maxOrbiters];
			spinners = new float[maxOrbiters];
			projID = new int[] {ProjectileID.IceBoomerang, ProjectileID.Flamarang };
			spinDist = 10f;
			spinDiv = 16f;// (MathHelper.TwoPi*100f)+MathHelper.Pi;
			spinVelocity = 12f;
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			spriteBatch.Draw(tex, projectile.Center - projectile.velocity - Main.screenPosition, null, Color.White, projectile.velocity.X / 20f, tex.Size() / 2f,0.5f, default, 0);
			return false;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.IceBoomerang);
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.melee = true;
			projectile.scale = 1f;
			projectile.extraUpdates = 1;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback /= 5f;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return true;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			projectile.Kill();
		}

	}

	public class Coralrang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coralrang");
			Tooltip.SetDefault("Throws a returning clump of razer sharp coral that splits apart on hit\nThe clump and splitting projectiles cannot hit the same target");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.damage = 28;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 35;
			item.useStyle = 5;
			item.noUseGraphic = true;
			item.useTime = 35;
			item.knockBack = 4f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 1;
			item.value = Item.buyPrice(gold: 2);
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<CoralrangProj>();
			item.shootSpeed = 7f;
		}
		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Coral, 10);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.BiomassBar>(), 8);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed.RotatedByRandom(MathHelper.Pi / 8f);
			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer);

			return false;
		}
	}

	public class CoralrangProj : ModProjectile
	{
		protected virtual int maxOrbiters => 5;
		protected int[] projID;
		protected float spinDist = 4f;
		protected float spinRand = 0f;
		protected float spinDiv = 8f;
		protected float spinVelocity = 0f;
		protected float angleOffset = 0f;//MathHelper.Pi;
		protected Projectile[] orbitors;
		protected float[] spinners;
		protected virtual float damageMul => 0.75f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coralrang");
		}

		public CoralrangProj()
		{
			orbitors = new Projectile[maxOrbiters];
			spinners = new float[maxOrbiters];
			projID = new int[] {ModContent.ProjectileType<CoralrangProj2>()};
			spinRand = MathHelper.Pi / 2f;
		}

		public override string Texture
		{
			get { return ("Terraria/Tiles_" + TileID.Coral); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.IceBoomerang);
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = 1;
			projectile.melee = true;
			projectile.scale = 1f;
		}

		public override void SendExtraAI(System.IO.BinaryWriter writer)
		{
			for (int i = 0; i < orbitors.Length; i += 1)
			{
				if (orbitors[i]==null)
					writer.Write(-1);
				else
					writer.Write((ushort)orbitors[i].whoAmI);
			}
		}

		public override void ReceiveExtraAI(System.IO.BinaryReader reader)
		{
			for (int i = 0; i < orbitors.Length; i += 1)
			{
				int theyare = (int)reader.ReadUInt16();
				if (theyare > -1)
				{
					Projectile proj = Main.projectile[theyare];

					if (proj != null && proj.active && projID.FirstOrDefault(type => type == orbitors[i].type) != default)
						orbitors[i] = Main.projectile[theyare];
				}
			}
		}

		public override void AI()
		{
			for (int k = 0; k < orbitors.Length; k += 1)
			{
				if (spinners[k] == default)
				{
					spinners[k] = Main.rand.NextFloat(spinRand);
					Vector2 movespeed = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-4f, 2f));
					int newb = Projectile.NewProjectile(projectile.Center, movespeed, projID[k % projID.Length], (int)(projectile.damage * damageMul), projectile.knockBack, Main.myPlayer, 0, Main.rand.Next(4));
					Main.projectile[newb].tileCollide = false;
					Main.projectile[newb].rotation += MathHelper.TwoPi*(k/ orbitors.Length);
					Main.projectile[newb].netUpdate = true;
					orbitors[k] = Main.projectile[newb];
					projectile.netUpdate = true;
				}
				else
				{
					if (orbitors[k] != null)
					{
						if (projID.FirstOrDefault(type => type == orbitors[k].type) != default)
						{
							float rotate = MathHelper.TwoPi * (projectile.rotation / spinDiv);

							if (spinDiv > 100f)
								rotate = projectile.rotation + spinDiv;

							float anglex = (k / (float)orbitors.Length) * MathHelper.TwoPi;
							float anglez = (anglex + spinners[k]) + (rotate);

							Vector2 loc = new Vector2((float)Math.Cos(anglez), (float)Math.Sin(anglez));
							Vector2 gohere = projectile.Center + loc;

							if (spinDiv <= 100f)
								orbitors[k].rotation = (loc).ToRotation() + angleOffset;

							orbitors[k].Center = gohere + loc * spinDist;
							if (spinVelocity > 0f)
							{
								orbitors[k].velocity = loc * spinVelocity;
							}

							if (orbitors[k].type == ModContent.ProjectileType<CoralrangProj2>())
							{
								orbitors[k].timeLeft = 300;
								orbitors[k].localAI[1] = 100;
							}
						}
					}
				}
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			for (int k = 0; k < orbitors.Length; k += 1)
			{
				if (orbitors[k] != null)
				{
					if (projID.FirstOrDefault(type => type == orbitors[k].type) != default)
					{
						orbitors[k].ai[0] = target.whoAmI;
						orbitors[k].localAI[1] = 3;
						orbitors[k].timeLeft = 300;
						if (orbitors[k].type != ModContent.ProjectileType<CoralrangProj2>())
							orbitors[k].tileCollide = true;
						orbitors[k].netUpdate = true;
					}
				}
			}

			projectile.Kill();
		}
	}

	public class CoralrangProj2 : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Coralrang");
		}

		public override void SetDefaults()
		{
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.timeLeft = 2;
			projectile.melee = true;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override bool? CanHitNPC(NPC target)
		{
			return target.whoAmI != (int)projectile.ai[0] && projectile.localAI[1]<1;
		}

		public override string Texture
		{
			get { return ("Terraria/Tiles_" + TileID.Coral); }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frames = tex.Width / 6;
			Vector2 offset = new Vector2(frames,tex.Height) / 2f;

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle((int)projectile.ai[1] * frames, 0, frames, tex.Height), lightColor,projectile.rotation,offset,projectile.scale,SpriteEffects.None,0);
			return false;
		}

		public override void AI()
		{
			projectile.localAI[1] -= 1;

			if (projectile.localAI[1] < 97 && projectile.localAI[1] > 80)
			{
				projectile.localAI[1] = 9001;
				projectile.Kill();
			}

			if ((int)projectile.localAI[0] == 0)
			{
				projectile.localAI[0] = Main.rand.NextFloat(3f, 30f) * (Main.rand.NextBool() ? 1f : -1f);
			}

			if (projectile.localAI[1] < 1)
			{
				projectile.tileCollide = true;
				projectile.rotation += (projectile.localAI[0]-Math.Sign(projectile.localAI[0]*2.5f))*0.02f;
				projectile.velocity.Y += 0.125f;
			}
			else
			{
				projectile.position -= projectile.velocity;
			}

			Vector2 velo = new Vector2(Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-1f, 1f));
			Dust dust = Dust.NewDustPerfect(projectile.position + new Vector2(Main.rand.NextFloat(projectile.width), Main.rand.NextFloat(projectile.height)), 33, velo,150,Color.White,0.75f);
			dust.noGravity = false;

		}

		public override bool PreKill(int timeLeft)
		{
			if (projectile.localAI[1] < 9001)
			{
				for (int i = 0; i < 20; i += 1)
				{
					Vector2 velo = new Vector2(Main.rand.NextFloat(-4f, 4f), Main.rand.NextFloat(-1f, 1f));
					Dust dust = Dust.NewDustPerfect(projectile.position + new Vector2(Main.rand.NextFloat(projectile.width), Main.rand.NextFloat(projectile.height)), 33, velo, 150, Color.White, 1f);
					dust.noGravity = false;
				}
			}
			return true;
		}

	}
}