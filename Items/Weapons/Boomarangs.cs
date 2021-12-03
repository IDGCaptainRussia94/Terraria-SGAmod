using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Utilities;
using Terraria.DataStructures;
using System.IO;

namespace SGAmod.Items.Weapons
{

	public class ExplosionBoomerang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BOOMerang");
			Tooltip.SetDefault("Explodes when it hits an enemy, does not explode on tiles\n'This is what happens when you let Demolitionist and Tinkerer have too much fun...'");
		}

		public override void SetDefaults()
		{
			item.width = 16;
			item.height = 16;
			item.damage = 14;
			item.crit = 5;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 30;
			item.useStyle = 5;
			item.noUseGraphic = true;
			item.useTime = 30;
			item.knockBack = 1f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 30;
			item.consumable = true;
			item.value = Item.buyPrice(silver: 15);
			item.rare = ItemRarityID.Blue;
			item.shoot = ModContent.ProjectileType<ExplosionBoomerangProj>();
			item.shootSpeed = 15f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);

			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer);

			return false;
		}
	}

	public class ExplosionBoomerangProj : ModProjectile
	{
		bool HitEnemy = false;


		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("BOOMerang");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/ExplosionBoomerang"); }
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.IceBoomerang);
			projectile.width = 18;
			projectile.height = 18;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
			projectile.tileCollide = true;
		}

		public override void AI()
		{

			int dustIndexsmoke = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 31, 0f, 0f, 100, default(Color), 1f);
			Main.dust[dustIndexsmoke].scale = 0.1f + (float)Main.rand.Next(5) * 0.1f;
			Main.dust[dustIndexsmoke].fadeIn = 1.5f + (float)Main.rand.Next(5) * 0.1f;
			Main.dust[dustIndexsmoke].noGravity = true;
			Main.dust[dustIndexsmoke].position = projectile.Center + new Vector2(0f, (float)(-(float)projectile.height / 2)).RotatedBy((double)projectile.rotation, default(Vector2)) * 1.1f;
			dustIndexsmoke = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6, 0f, 0f, 100, default(Color), 1f);
			Main.dust[dustIndexsmoke].scale = 1f + (float)Main.rand.Next(5) * 0.1f;
			Main.dust[dustIndexsmoke].noGravity = true;
			Main.dust[dustIndexsmoke].position = projectile.Center + new Vector2(0f, (float)(-(float)projectile.height / 2 - 6)).RotatedBy((double)projectile.rotation, default(Vector2)) * 1.1f;

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			HitEnemy = true;
			projectile.Kill();

		}

		public override bool PreKill(int timeLeft)
		{
			if (HitEnemy == true)
			{
				int proj = Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, Vector2.Zero.X, Vector2.Zero.Y, ModContent.ProjectileType<CreepersThrowBoom>(), projectile.damage, projectile.knockBack, projectile.owner, 0.0f, 0f);
				Main.projectile[proj].timeLeft = 2;
				Main.projectile[proj].netUpdate = true;
			}
			else
			{
				Player player = Main.player[projectile.owner];
				if ((projectile.Center - player.MountedCenter).Length() < 80)
					player.QuickSpawnItem(ModContent.ItemType<ExplosionBoomerang>(), 1);


			}

			return true;
		}

	}

	public class Specterang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Specterang");
			Tooltip.SetDefault("Throws a ghostly Boomerang that passes through enemies and walls\nLeaves behind ghostly images of itself that damages enemies");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.damage = 30;
			item.crit = 10;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.noUseGraphic = true;
			item.useTime = 10;
			item.knockBack = 1f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 1;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<SpecterangProj>();
			item.shootSpeed = 20f;
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 1;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.EnchantedBoomerang, 1);
			recipe.AddIngredient(ItemID.SoulofLight, 6);
			recipe.AddIngredient(ItemID.SpectreBar, 8);
			recipe.AddIngredient(ItemID.Ectoplasm, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);

			Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, player.whoAmI);

			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			/*if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[item.type];
				Texture2D texture2 = ModContent.GetTexture("SGAmod/Items/GlowMasks/Specterang_Glow");
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.position + new Vector2(0, 0) - Main.screenPosition, null, lightColor*0.5f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, item.position + new Vector2(0, 0) - Main.screenPosition, null, lightColor*0.50f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture2, item.position + new Vector2(0, 0) - Main.screenPosition, null, lightColor* 0.75f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
			}*/
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Texture2D texture = Main.itemTexture[item.type];
				Texture2D texture2 = ModContent.GetTexture("SGAmod/Items/GlowMasks/Specterang_Glow");

				Vector2 slotSize = new Vector2(52f, 52f);
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.Center + new Vector2(0, 0) - Main.screenPosition, null, drawColor * 0.5f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture, item.Center + new Vector2(0, 0) - Main.screenPosition, null, drawColor * 0.50f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(texture2, item.Center + new Vector2(0, 0) - Main.screenPosition, null, drawColor * 0.75f, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
			}
		}
	}

	public class SpecterangProj : ModProjectile, IDrawAdditive
	{
		protected virtual int ReturnTime => 20;
		protected virtual int ReturnTimeNoSlow => 70;
		protected virtual float SolidAmmount => 6f;
		protected float startSpeed = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Specterang");
		}

		public override string Texture
		{
			get { return ("Terraria/Projectile_658"); }
		}

		public static void DrawSpecterang(Projectile projectile, SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.itemTexture[ModContent.ItemType<Specterang>()];
			Texture2D tex2 = ModContent.GetTexture("SGAmod/Items/GlowMasks/Specterang_Glow");

			float alpha = projectile.localAI[0];

			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, lightColor * 0.50f * alpha, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.50f * alpha, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
			spriteBatch.Draw(tex2, projectile.Center - Main.screenPosition, null, Color.White * 0.80f * alpha, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
		}

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			if (GetType() == typeof(SpecterangProj))
				DrawSpecterang(projectile, spriteBatch, Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16), Color.White));

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			UnifiedRandom rand = new UnifiedRandom(projectile.whoAmI);
			for (float a = 6f; a > 0; a -= 0.25f)
			{
				float offset = rand.NextFloat(-MathHelper.Pi / 4f, MathHelper.Pi / 4f);
				float scale = (1f - (a / 6f));
				spriteBatch.Draw(tex, projectile.Center - (projectile.velocity * a) - Main.screenPosition, null, Color.White * 0.05f * scale, projectile.rotation + offset, tex.Size() / 2f, new Vector2(2f, 2f) * scale, default, 0);
			}

			return false;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.IceBoomerang);
			projectile.width = 24;
			projectile.height = 24;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.aiStyle = -1;
			projectile.scale = 1f;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 15;
			projectile.extraUpdates = 2;
			projectile.tileCollide = false;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{

			bool solid = projectile.ai[0] >= ReturnTime && projectile.ai[0] < ReturnTimeNoSlow;

			if (startSpeed == default)
			{
				startSpeed = projectile.velocity.Length();
				projectile.aiStyle = (projectile.velocity.X > 0 ? 1 : -1) - 20;
			}

			projectile.localAI[0] = 1f;// MathHelper.Clamp(projectile.localAI[0] += (solid ? 0.08f : -0.04f), 0.5f, 1f);

			projectile.ai[0] += 1;

			projectile.rotation += 0.4f * (float)(projectile.aiStyle + 20f);

			if (GetType() == typeof(SpecterangProj))
			{

				if (projectile.soundDelay == 0)
				{
					projectile.soundDelay = 10;
					Main.PlaySound(SoundID.Item7, projectile.position);
				}
				if (projectile.ai[0] % 2 == 0)
				{
					int proj = Projectile.NewProjectile(projectile.Center, -projectile.velocity * 0.25f, ModContent.ProjectileType<SpecterangProj2>(), projectile.damage, 0f, projectile.owner);
					Main.projectile[proj].rotation = projectile.rotation;
				}
			}

			if (projectile.ai[0] >= ReturnTime)
			{
				Player owner = Main.player[projectile.owner];

				Vector2 distmeasure = owner.MountedCenter - projectile.Center;

				projectile.velocity += Vector2.Normalize(distmeasure) * (0.70f);

				if (projectile.ai[0] >= ReturnTimeNoSlow)
				{
					//projectile.velocity *= 0.75f;
					float dist = Math.Min(((projectile.ai[0] - ReturnTimeNoSlow) / 4f), distmeasure.Length());
					projectile.Center += Vector2.Normalize(distmeasure) * (0.50f) * dist;
				}

				projectile.velocity *= 0.99f;
				if (projectile.velocity.Length() > startSpeed)
					projectile.velocity = Vector2.Normalize(projectile.velocity) * startSpeed;

				if (Main.myPlayer == projectile.owner)
				{
					Rectangle rectangle = new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height);
					Rectangle value2 = new Rectangle((int)Main.player[projectile.owner].position.X, (int)Main.player[projectile.owner].position.Y, Main.player[projectile.owner].width, Main.player[projectile.owner].height);
					if (rectangle.Intersects(value2))
					{
						projectile.Kill();
					}
				}

			}

		}

	}

	public class SpecterangProj2 : ModProjectile, IDrawAdditive
	{

		protected float startSpeed = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Specterang Shadow");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Specterang"); }
		}
		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			SpecterangProj.DrawSpecterang(projectile, spriteBatch, Lighting.GetColor((int)(projectile.Center.X / 16), (int)(projectile.Center.Y / 16), Color.White));
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj2>()];
			float scale = (float)projectile.timeLeft / 20f;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, Color.White * 0.5f* scale, projectile.rotation, tex.Size() / 2f, new Vector2(2f, 2f) * scale, default, 0);

			return false;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.IceBoomerang);
			projectile.width = 32;
			projectile.height = 32;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.timeLeft = 10;
			projectile.aiStyle = -1;
			projectile.scale = 1f;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
			projectile.extraUpdates = 0;
			projectile.tileCollide = false;
		}

		public override void AI()
		{
			projectile.localAI[0] = (float)projectile.timeLeft / 15f;
		}

	}

	public class Wirang : ModItem
	{
		static int wireType=0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wirang");
			Tooltip.SetDefault("Throws multi-color Boomerang-Shaped wire bundles\nThese activate their respective wires when they pass over them\nThrows up to 4 at once");
		}

		public override void SetDefaults()
		{
			item.width = 12;
			item.height = 12;
			item.damage = 32;
			item.crit = 5;
			item.melee = true;
			item.noMelee = true;
			item.useTurn = true;
			item.noUseGraphic = true;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.noUseGraphic = true;
			item.useTime = 10;
			item.knockBack = 0f;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.maxStack = 1;
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<WirangProj>();
			item.shootSpeed = 10f;
			item.mech = true;
		}
		public override string Texture
		{
			get { return ("Terraria/UI/Wires_2"); }
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[item.shoot] < 4;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speed = new Vector2(speedX, speedY);
			speed.RotatedByRandom(MathHelper.Pi / 8f);

			int prog = Projectile.NewProjectile(position.X, position.Y, speed.X, speed.Y, type, damage, knockBack, Main.myPlayer);
			Main.projectile[prog].localAI[1] = wireType;
			Main.projectile[prog].netUpdate = true;
			wireType = (wireType + 1) % 4;

			return false;
		}

		public override bool PreDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{

			Texture2D inner = Main.wireUITexture[2 + ((int)(Main.GlobalTime)%4)];

			Vector2 slotSize = new Vector2(52f, 52f);
			position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
			Vector2 drawPos = position + slotSize * Main.inventoryScale / 2f;
			Vector2 textureOrigin = new Vector2(inner.Width / 2, inner.Height / 2);

			spriteBatch.Draw(inner, drawPos, null, Color.White, 0, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class WirangProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wirang");
		}

		public override string Texture
		{
			get { return ("Terraria/UI/Wires_2"); }
		}

        public override void SendExtraAI(BinaryWriter writer)
        {
			writer.Write((int)projectile.localAI[1]);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
			projectile.localAI[1] = reader.ReadInt16();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.wireUITexture[2+(int)projectile.localAI[1]];
			spriteBatch.Draw(tex, projectile.Center - projectile.velocity - Main.screenPosition, null, lightColor, projectile.rotation, tex.Size() / 2f, projectile.scale, default, 0);
			return false;
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.WoodenBoomerang);
			projectile.width = 16;
			projectile.height = 16;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
			projectile.extraUpdates = 0;
		}

        public override void AI()
        {
			Point16 here = new Point16((int)projectile.Center.X / 16, (int)projectile.Center.Y / 16);
			if (WorldGen.InWorld(here.X, here.Y))
			{
				Tile tile = Main.tile[here.X, here.Y];
				if ((tile.wire() && projectile.localAI[1] == 0) ||
				(tile.wire3() && projectile.localAI[1] == 1) ||
				(tile.wire2() && projectile.localAI[1] == 2) ||
				(tile.wire4() && projectile.localAI[1] == 3))
				{
					Wiring.TripWire(here.X, here.Y, 1, 1);
					if (projectile.extraUpdates<1)
					projectile.extraUpdates = 1;
				}
			}
        }

        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			knockback /= 2f;
		}

	}

	public class Fridgeflamarang : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Fridgeflamarang");
			Tooltip.SetDefault("Throws an Icy and Flaming Boomerang pair guided by a light that split apart on hit\nUsable only when less than 4 Boomerangs are active");
		}

		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 10;
			item.damage = 50;
			item.crit = 5;
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
			item.value = Item.buyPrice(gold: 5);
			item.rare = ItemRarityID.LightPurple;
			item.shoot = ModContent.ProjectileType<FridgeflamarangProj>();
			item.shootSpeed = 25f;
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
				Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, item.Center - Main.screenPosition, null, Color.White, 0f, textureOrigin, 0.50f, SpriteEffects.FlipHorizontally, 0f);

				/*texture = Main.itemTexture[ItemID.IceBoomerang];
				textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);

				spriteBatch.Draw(texture, item.Center + new Vector2(-8, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.Flamarang], item.Center + new Vector2(8, 0) - Main.screenPosition, null, lightColor, 0f, textureOrigin, 1f, SpriteEffects.FlipHorizontally, 0f);*/
			}
		}

		public override void PostDrawInInventory(SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			if (!Main.gameMenu)
			{
				Vector2 slotSize = new Vector2(52f, 52f);

				Texture2D texture = Main.projectileTexture[ModContent.ProjectileType<SpecterangProj>()];
				Vector2 textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				Vector2 drawPos = (position) + (slotSize * Main.inventoryScale) *0.31f;
				spriteBatch.Draw(texture, drawPos, null, drawColor, 0f, textureOrigin, Main.inventoryScale/2.5f, SpriteEffects.None, 0f);

				/*texture = Main.itemTexture[ItemID.IceBoomerang];
				position -= slotSize * Main.inventoryScale / 2f - frame.Size() * scale / 2f;
				drawPos = position + slotSize * Main.inventoryScale / 2f;
				textureOrigin = new Vector2(texture.Width / 2, texture.Height / 2);
				spriteBatch.Draw(texture, drawPos + new Vector2(-8, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.None, 0f);
				spriteBatch.Draw(Main.itemTexture[ItemID.Flamarang], drawPos + new Vector2(8, 0), null, drawColor, 0f, textureOrigin, Main.inventoryScale, SpriteEffects.FlipHorizontally, 0f);*/
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
			item.damage = 42;
			item.crit = 5;
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
			item.value = Item.buyPrice(gold: 1);
			item.rare = ItemRarityID.Green;
			item.shoot = ModContent.ProjectileType<CoralrangProj>();
			item.shootSpeed = 32f;
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
			projectile.extraUpdates = 0;
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

			if (projectile.ai[1]>0)
			projectile.ai[1] += 2.5f;

			if (projectile.ai[0] > 0)
            {
				Player P = Main.player[projectile.owner];
				Vector2 dist = P.Center - projectile.Center;
				projectile.velocity += Vector2.Normalize(dist) *MathHelper.Clamp(dist.Length()/180f,0f,1f);
            }

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
