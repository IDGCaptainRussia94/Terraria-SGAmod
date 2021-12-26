using System;
using System.Linq;
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
using AAAAUThrowing;
using SGAmod.Buffs;
using SGAmod.Effects;
using Microsoft.Xna.Framework.Audio;
using SGAmod.Items.Weapons.Technical;
using Terraria.ModLoader.IO;

namespace SGAmod.Items.Weapons.Technical
{

	public class LaserMarker : Shields.CorrodedShield, IHitScanItem
	{
		public override bool CanBlock => false;

		public int gemType = ItemID.Amethyst;

		public override bool CloneNewInstances => true;

		public override void NetSend(BinaryWriter writer)
		{
			writer.Write((short)gemType);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			gemType = reader.ReadInt16();
		}
		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound
			{
				["gemType"] = gemType
			};
			return tag;
		}
		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("gemType"))
			{
				gemType = tag.GetInt("gemType");
			}
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Laser Marker");
			Tooltip.SetDefault("Laser Pointer, aim at your enemies to mark them, increasing damage they take by 10%\nCan be held out like a torch and used normally by holding shift\nCan also be thrown like a " + Terraria.Localization.Language.GetTextValue("ItemName.Glowstick"));
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.width = 32;
			item.height = 12;
			item.useTime = 20;
			item.useAnimation = 21;
			item.reuseDelay = 100;
			item.useStyle = 1;
			item.knockBack = 5;
			item.noUseGraphic = true;
			item.value = Item.buyPrice(0, 0, 0, 80);
			item.rare = 2;
			item.UseSound = SoundID.Item7;
			item.consumable = true;
			item.maxStack = 30;
			item.shootSpeed = 10f;
			item.useTurn = false;
			item.autoReuse = false;
			item.noMelee = true;

			item.shoot = ModContent.ProjectileType<LaserMarkerThrownProj>();
		}

		public override bool ConsumeItem(Player player)
		{
			return player.reuseDelay > 3;
		}

		public override bool CanUseItem(Player player)
		{
			return true;// player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			SGAmod.GemColors.TryGetValue(gemType, out Color color);

			tooltips.Add(new TooltipLine(mod, "LaserColor", Idglib.ColorText(color, "Gem Colored Lens")));
		}

		public override void AddRecipes()
		{
			ModRecipe recipe;
			foreach (int itemtype in SGAmod.GemColors.Keys)
			{
				recipe = new ModRecipe(mod);
				recipe.AddIngredient(itemtype, 1);
				recipe.AddRecipeGroup("SGAmod:Tier1Bars", 1);
				recipe.AddRecipeGroup("SGAmod:BasicWraithShards", 1);
				recipe.AddIngredient(ItemID.Glass, 5);
				recipe.AddIngredient(ItemID.Lens, 1);
				recipe.AddIngredient(ItemID.Glowstick, 3);
				recipe.AddTile(TileID.WorkBenches);
				recipe.SetResult(this, 5);
				recipe.AddRecipe();
			}
		}

		public override bool UseItem(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			Vector2 speedto = Main.MouseWorld - position;
			Vector2 speed = Vector2.Normalize(speedto) * (Math.Min(400f * player.Throwing().thrownVelocity, speedto.Length()));

			speed /= 30f;

			speedX = speed.X;
			speedY = speed.Y;

			//position = Main.MouseWorld;
			Projectile proj = Projectile.NewProjectileDirect(position, new Vector2(speedX, speedY), type, damage, knockBack, player.whoAmI, 0f, 0f);
			if (proj != null)
			{
				SGAmod.GemColors.TryGetValue(gemType, out Color color);
				((LaserMarkerProj)proj.modProjectile).gemColor = color;
			}

			return false;
		}


	}

	public class LaserMarkerProj : Shields.CorrodedShieldProj, IDrawAdditive
	{
		protected int MyLaser = default;
		public Vector2 EndPoint = default;
		public Color gemColor = Color.Black;
		protected override bool CanBlock => false;
		public override bool CloneNewInstances => true;
		public override ModProjectile Clone()
		{
			LaserMarkerProj clone = (LaserMarkerProj)MemberwiseClone();
			clone.gemColor = this.gemColor;
			return clone;
		}

		public override bool Blocking => false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LaserMarkerProj");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 10;
			projectile.hostile = false;
			projectile.penetrate = 10;
			projectile.light = 0.35f;
			projectile.width = 24;
			projectile.height = 24;
			projectile.tileCollide = false;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Technical/LaserMarker"); }
		}

		public override bool PreAI()
		{
			return true;
		}

		public override void AI()
		{

			Player player = Main.player[projectile.owner];
			bool heldone = player.HeldItem.type != mod.ItemType(ItemName);
			if (projectile.ai[0] > 0 || (player.HeldItem == null || heldone) || player.itemAnimation > 0 || player.dead)
			{
				projectile.Kill();
				return;
			}
			else
			{
				if (projectile.timeLeft < 3)
					projectile.timeLeft = 3;
				Vector2 mousePos = Main.MouseWorld;

				if (projectile.owner == Main.myPlayer)
				{
					Vector2 diff = mousePos - player.Center;
					projectile.velocity = diff;
					projectile.direction = Main.MouseWorld.X > player.position.X ? 1 : -1;
					projectile.netUpdate = true;
					projectile.Center = mousePos;
				}
				int dir = projectile.direction;
				player.ChangeDir(dir);

				Vector2 direction = (projectile.velocity);
				Vector2 directionmeasure = direction;

				player.heldProj = projectile.whoAmI;

				projectile.velocity.Normalize();
				projectile.rotation = projectile.velocity.ToRotation();

				player.bodyFrame.Y = player.bodyFrame.Height * 3;
				if (directionmeasure.Y - Math.Abs(directionmeasure.X) > 25)
					player.bodyFrame.Y = player.bodyFrame.Height * 4;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -25)
					player.bodyFrame.Y = player.bodyFrame.Height * 2;
				if (directionmeasure.Y + Math.Abs(directionmeasure.X) < -160)
					player.bodyFrame.Y = player.bodyFrame.Height * 5;
				player.direction = (directionmeasure.X > 0).ToDirectionInt();

				projectile.Center = player.Center + (projectile.velocity * 10f);
				projectile.velocity *= 8f;

				MyLaser = Projectile.NewProjectile(projectile.Center + projectile.rotation.ToRotationVector2() * 12f, projectile.rotation.ToRotationVector2() * 2f, ModContent.ProjectileType<LaserMarkerLaserProj>(), 0, 0, player.whoAmI, ai1: (int)projectile.whoAmI);
				Main.projectile[MyLaser].localAI[0] = Main.rgbToHsl(gemColor).X;

			}
		}

		protected override void DrawAdd()
		{
			base.DrawAdd();
			if (EndPoint != default)
				DoDraw(Main.spriteBatch, Color.White);
		}
		public void DoDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			Vector2 origin = new Vector2(16, 1);
			Vector2 start = projectile.Center + projectile.rotation.ToRotationVector2() * 12f;

			Vector2 diff = (EndPoint - start);
			float length = diff.Length();

			Texture2D endpointtex = ModContent.GetTexture("SGAmod/Extra_49c");

			Main.spriteBatch.Draw(ModContent.GetTexture("SGAmod/LaserBeam"), start - Main.screenPosition, new Rectangle(0, 0, 32, 1), gemColor * 0.50f, diff.ToRotation() + MathHelper.PiOver2, origin, new Vector2(0.25f, length), SpriteEffects.None, 0);
			//Main.spriteBatch.Draw(endpointtex, EndPoint - Main.screenPosition, null, gemColor * 0.50f, 0, endpointtex.Size()/2f, 0.5f, SpriteEffects.None, 0);

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{

			bool facingleft = projectile.velocity.X > 0;
			Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.None;
			Texture2D texture = Main.projectileTexture[projectile.type];
			Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
			Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor * projectile.Opacity, projectile.rotation + (facingleft ? 0 : MathHelper.Pi), origin, projectile.scale, facingleft ? effect : SpriteEffects.FlipHorizontally, 0);

			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

			Texture2D endpointtex = ModContent.GetTexture("SGAmod/Glow");
			Main.spriteBatch.Draw(endpointtex, EndPoint - Main.screenPosition, null, gemColor * 1f, 0, endpointtex.Size() / 2f, 1f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(endpointtex, EndPoint - Main.screenPosition, null, Color.White * 1f, 0, endpointtex.Size() / 2f, 0.75f, SpriteEffects.None, 0);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
		}

	}

	class LaserMarkerThrownProj : LaserMarkerProj, IDrawAdditive
	{

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Laser");
		}

		public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.Glowstick);
			projectile.Throwing().thrown = true;
			projectile.timeLeft = 1800;
			projectile.width = 8;
			projectile.height = 8;
			projectile.light = 0.35f;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Technical/LaserMarker"); }
		}

		public override bool PreKill(int timeLeft)
		{
			for (int i = 0; i < 20; i += 1)
			{
				int dust = Dust.NewDust(new Vector2(projectile.Center.X - 4, projectile.Center.Y - 4), 8, 8, 269);
				Main.dust[dust].scale = 0.50f;
				Main.dust[dust].noGravity = false;
				Main.dust[dust].velocity = Main.rand.NextVector2Circular(6f, 6f);
			}
			SoundEffectInstance sound = Main.PlaySound(SoundID.NPCHit, (int)projectile.Center.X, (int)projectile.Center.Y, 53);
			if (sound != null)
				sound.Pitch -= 0.5f;

			return true;
		}

		public override void AI()
		{
			//vanilla
			Player player = Main.player[projectile.owner];
			MyLaser = Projectile.NewProjectile(projectile.Center + projectile.rotation.ToRotationVector2() * 12f, projectile.rotation.ToRotationVector2() * 2f, ModContent.ProjectileType<LaserMarkerLaserProj>(), 0, 0, player.whoAmI, ai1: (int)projectile.whoAmI);
			Main.projectile[MyLaser].localAI[0] = Main.rgbToHsl(gemColor).X;

		}

	}

	public class LaserMarkerLaserProj : ModProjectile
	{
		public Vector2 EndPoint = default;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("LaserMarkerLaserProj");
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.timeLeft = 1;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.light = 0.35f;
			projectile.width = 4;
			projectile.height = 4;
			projectile.tileCollide = false;
			projectile.extraUpdates = 0;
			drawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/Technical/LaserMarker"); }
		}

		//The distance charge particle from the player center
		private const float MOVE_DISTANCE = 0f;

		// The actual distance is stored in the ai0 field
		// By making a property to handle this it makes our life easier, and the accessibility more readable
		public float Distance
		{
			get => projectile.ai[0];
			set => projectile.ai[0] = value;
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void AI()
		{

			//projectile.Center = Main.projectile[(int)projectile.knockBack].Center;

			Player player = Main.player[projectile.owner];

			NPC hitnpc = default;

			Vector3 colorz = Main.hslToRgb(projectile.localAI[0], 1f, 0.75f).ToVector3() * 0.50f;

			SetLaserPosition(player, ref hitnpc);

			if (hitnpc != default)
			{
				hitnpc.AddBuff(ModContent.BuffType<Marked>(), 2);
			}

			Lighting.AddLight(EndPoint, colorz * 1.25f);

			Projectile owner = Main.projectile[(int)projectile.ai[1]];

			if (owner != null && owner.active && owner.modProjectile != null && owner.modProjectile is LaserMarkerProj laser)
				laser.EndPoint = EndPoint;

			//Utils.PlotTileLine(projectile.Center, projectile.Center + projectile.velocity * (Distance - MOVE_DISTANCE), 26, new Utils.PerLinePoint(DelegateMethods.CastLight));


		}

		private void SetLaserPosition(Player player, ref NPC hitnpc)
		{
			Color colorz2 = Main.hslToRgb(projectile.localAI[0], 1f, 0.75f);

			float distanceboost = 4f;

			for (Distance = MOVE_DISTANCE; Distance <= 2200f; Distance += distanceboost)
			{
				var start = projectile.Center + projectile.velocity * Distance;
				EndPoint = start;

				Vector3 colorz = colorz2.ToVector3() * Main.rand.NextFloat(0.10f, 0.30f);

				//if (Main.rand.Next(0, 10) == 0)

				//projectile.Center + projectile.velocity * (Distance+distanceboost)
				if (!Collision.CanHit(start, 0, 0, projectile.Center + projectile.velocity * (Distance + distanceboost), 0, 0))
				{
					//Point checkthere = ((projectile.Center + projectile.velocity * (Distance + distanceboost))/16).ToPoint();

					//Tile tilecol = Framing.GetTileSafely(checkthere.X, checkthere.Y);
					//if (tilecol.type == TileID.emer)

					Distance -= distanceboost;
					distanceboost -= 1f;
					if (distanceboost < 1f)
						return;
				}

				Lighting.AddLight(start, colorz);

				foreach (NPC npc in Main.npc)
				{
					if (npc.active && !npc.dontTakeDamage && new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height).Contains(start.ToPoint()))
					{
						hitnpc = npc;
						Distance -= 5f;
						return;
					}

				}
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			return false;
		}

	}
}
