using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using SGAmod.Projectiles;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class GlassSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glass Sword");
			Tooltip.SetDefault("Shatters on the first hit, throwing out several glass shards\nThis weapon ignores enemy defense");
			Item.staff[item.type] = true; 
		}

		public override bool ConsumeItem(Player player)
		{
			return player.itemAnimation>0;
		}

		public override bool CanUseItem(Player player)
		{
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/GlassSword");
			}
			item.noMelee = false;
			return true;
		}

        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
			damage += target.defense/2;
		}

        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			item.noMelee = true;
			player.ConsumeItem(item.type);
			Main.PlaySound(SoundID.Item, (int)player.Center.X, (int)player.Center.Y, 27, 0.75f, 0f);

			for (float i = 24; i < 80; i += 20)
			{
				Vector2 position = player.Center;

				Vector2 eree = player.itemRotation.ToRotationVector2().RotatedBy(MathHelper.ToRadians(-45f* player.direction));
				eree *= player.direction;

				position += eree * i;

				int thisoned = Projectile.NewProjectile(position.X, position.Y, eree.X*Main.rand.NextFloat(2.4f,5f), eree.X * Main.rand.NextFloat(0.5f, 2f), mod.ProjectileType("BrokenGlass"), damage, 0f, Main.myPlayer);

			}

				if (!Main.dedServ)
				{
					item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/GlassSwordBreak");
				}
		}

		public override void SetDefaults()
		{
			item.damage = 5;
			item.maxStack = 99;
			item.crit = 0;
			item.melee = true;
			item.width = 54;
			item.height = 54;
			item.useTime = 2;
			item.useAnimation = 22;
			item.reuseDelay = 30;
			item.consumable = true;
			item.useStyle = 1;
			item.knockBack = 1;
			item.noUseGraphic = true;
			item.value = Item.sellPrice(0, 0, 0, 5);
			item.rare = 0;
	        item.UseSound = SoundID.Item1;
			item.useTurn = false;
	     	item.autoReuse = true;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Glass, 4);
			recipe.AddTile(TileID.Furnaces);
			recipe.SetResult(this, 15);
			recipe.AddRecipe();
		}
	
	}

	public class BrokenGlass : ModProjectile
	{

		bool hittile = false;
		public virtual bool hitwhilefalling => false;
		public virtual float trans => 1f;
		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Broken Glass");
		}


		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 12;
			projectile.height = 12;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.melee = true;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + ProjectileID.RocketII; }
		}

		public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
		{
			//HairShaderData shader = GameShaders.Hair.GetShaderFromItemId(ItemID.LeinforsAccessory);
			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
			Texture2D texture = Main.itemTexture[ItemID.GlassBowl];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor * trans, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
			return false;
		}

		public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.Center.X, (int)projectile.Center.Y, 50, 0.50f, 0f);
			projectile.type = ProjectileID.Fireball;
			Main.PlaySound(2, (int)projectile.position.X, (int)projectile.position.Y, 10);
			for (int num315 = 0; num315 < 40; num315 = num315 + 1)
			{
				Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
				randomcircle *= Main.rand.NextFloat(0f, 3f);
				int num316 = Dust.NewDust(new Vector2(projectile.position.X - 1, projectile.position.Y), projectile.width, projectile.height, DustID.Ice, 0, 0, 50, Color.Gray, projectile.scale * 0.5f);
				Main.dust[num316].noGravity = false;
				Main.dust[num316].velocity = new Vector2(randomcircle.X, randomcircle.Y)+projectile.velocity;
			}

			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			hittile = true;
			return true;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (projectile.velocity.Y < 0 && hitwhilefalling)
				return false;
			if (projectile.ai[1] < 5)
				return false;
			return base.CanHitNPC(target);
		}

		public override void AI()
		{

			Tile tile = Main.tile[(int)projectile.Center.X / 16, (int)projectile.Center.Y / 16];
			if (tile != null)
				if (tile.liquid > 64)
					projectile.Kill();

			//projectile.scale = ((float)projectile.width / 14f);

			projectile.velocity.Y += 0.1f;
			projectile.rotation += projectile.velocity.X * 0.1f;

			projectile.ai[1] += 1;
		}


	}



}