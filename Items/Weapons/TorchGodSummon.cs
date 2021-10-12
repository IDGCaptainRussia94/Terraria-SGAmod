using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System.Linq;
using Microsoft.Xna.Framework.Audio;

namespace SGAmod.Items.Weapons
{
	public class TorchGodSummon : ModItem
	{
        public override bool Autoload(ref string name)
        {
			SGAPlayer.PostUpdateEquipsEvent += AddFunction;
			return true;
        }
        public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torch God's Summon");
			Tooltip.SetDefault("Summons 2 torches per free minion slot; attacking with torches burns them out for 3 seconds\nGain +10 damage per max minions, and +1 pierce per max Sentries, Biome torches inflict debuffs\nTorches provide a small ammount of light in the fog");
			Item.staff[item.type] = true;
		}

		public override void SetDefaults()
		{
			item.damage = 10;
			item.crit = 15;
			item.summon = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 10;
			item.noMelee = true;
			item.value = Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.LightRed;
			//item.UseSound = SoundID.Item71;
			item.shoot = 10;
			item.shootSpeed = 30f;
			item.autoReuse = true;
			item.useTurn = false;
			item.mana = 4;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/TorchGodSummon_Glow");
			}
		}

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
			flat += player.maxMinions * 10;
        }

		public void AddFunction(SGAPlayer sgaply)
        {
			Player player = sgaply.player;
			if (!player.dead && player.HeldItem.type == ModContent.ItemType<TorchGodSummon>())
			{
				for (int i = 0; i < ((player.maxMinions - (player.SGAPly().GetMinionSlots)) * 2)-player.ownedProjectileCounts[ModContent.ProjectileType<TorchGodSummonMinion>()]; i += 1)
				{
					Projectile proj = Projectile.NewProjectileDirect(player.Center, Vector2.Zero, ModContent.ProjectileType<TorchGodSummonMinion>(), item.damage, 10f, player.whoAmI, player.ownedProjectileCounts[ModContent.ProjectileType<TorchGodSummonMinion>()]+i, -i);
					if (proj != null)
					{

					}
				}
			}
        }

		public static TorchGodSummonMinion GetProjectileToShootFrom(Player player)
        {
			TorchGodSummonMinion sum = null;

			foreach (Projectile proj in Main.projectile.Where(testby => testby.active && testby.owner == player.whoAmI && testby.modProjectile != null && testby.ai[1]<0 && testby.type == ModContent.ProjectileType<TorchGodSummonMinion>()).OrderBy(testby => testby.ai[1]))
			{
				//Main.NewText("test");
				sum = (proj.modProjectile) as TorchGodSummonMinion;
				break;
			}
			return sum;
		}

        public override bool CanUseItem(Player player)
        {
			return GetProjectileToShootFrom(player) != null;
		}

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 speedz = new Vector2(speedX, speedY);
			speedz.Normalize(); speedz *= (30f+(player.maxMinions*2f)); speedX = speedz.X; speedY = speedz.Y;

			GetProjectileToShootFrom(player)?.ShootFlame(damage,knockBack, 12f + (player.maxMinions * 1f));

				//int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("TorchGodSummonProjectile"), damage, knockBack, player.whoAmI);
				//Main.projectile[proj].netUpdate = true;
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new StarMetalRecipes(mod);
			recipe.AddIngredient(ItemID.Torch, 200);
			recipe.AddRecipeGroup("SGAmod:Gems",10);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}


	}

	public class TorchGodSummonProjectile : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torched");
		}

		public override string Texture => "SGAmod/Invisible";

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.Boulder);
			aiType = ProjectileID.Boulder;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.light = 0f;
			projectile.width = 8;
			projectile.height = 8;
			projectile.knockBack = 0.5f;
			projectile.magic = false;
			projectile.minion = true;
			projectile.tileCollide = true;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = -1;
		}

		public override bool PreKill(int timeLeft)
		{
			Microsoft.Xna.Framework.Audio.SoundEffectInstance snd = Main.PlaySound(SoundID.Item110, projectile.Center);
			if (snd != null)
			{
				snd.Pitch = 0.50f;
			}

			for(int i = 0; i < 16; i++)
            {
				Vector2 velocity = Main.rand.NextVector2Circular(8f,8f) * Main.rand.NextFloat(0.2f, 1f);

				int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight);
				Main.dust[dust].scale = 1.00f;
				Main.dust[dust].fadeIn = 1.5f;
				Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)projectile.ai[0] % 16];
				Main.dust[dust].alpha = 250;
				Main.dust[dust].velocity = velocity;
				Main.dust[dust].noGravity = true;

				velocity = Main.rand.NextVector2Circular(16f, 16f);

				dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight);
				Main.dust[dust].scale = 1.50f;
				Main.dust[dust].fadeIn = 0.25f;
				Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)projectile.ai[0] % 16];
				Main.dust[dust].alpha = 250;
				Main.dust[dust].velocity = velocity;
				Main.dust[dust].noGravity = true;

			}


			return true;
		}

		public override void AI()
		{

			if (projectile.ai[1] < 10)
            {
				projectile.ai[1] = 10;
				projectile.penetrate = Main.player[projectile.owner].maxTurrets;
			}
			Lighting.AddLight(projectile.Center, TorchGodSummonMinion.TorchColors[(int)projectile.ai[0] % 16].ToVector3());

			int dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight);
			Main.dust[dust].scale = 1.00f;
			Main.dust[dust].fadeIn = 1.5f;
			Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)projectile.ai[0] % 16];
			Main.dust[dust].alpha = 250;
			Main.dust[dust].noGravity = true;

			dust = Dust.NewDust(projectile.position, projectile.width, projectile.height, DustID.AncientLight);
			Main.dust[dust].scale = 1.50f;
			Main.dust[dust].fadeIn = 0.25f;
			Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)projectile.ai[0] % 16];
			Main.dust[dust].alpha = 250;
			Main.dust[dust].noGravity = true;

		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			int index = TorchGodSummonMinion.GetBuffIndex((int)projectile.ai[0] % 16);
			if (index > 0)
				target.AddBuff(index, index == ModContent.BuffType< Buffs.MoonLightCurse >() ? 60 * 2 : 60*8);

		}
	}

	public class TorchGodSummonMinion : ModProjectile
	{

		public static Color[] TorchColors => new Color[] {Color.DarkOrange
		,Color.Blue
		,Color.Red
		,Color.Lime
		,Color.DarkViolet
		,Color.White
		,Color.Yellow
		,Color.DarkMagenta
		,Color.Green
		,Color.SkyBlue
		,Color.Gold
		,Color.DarkGoldenrod
		,Color.DarkTurquoise
		,Color.Gray
		,Color.Magenta
		,Color.Pink
		};

		public static int GetBuffIndex(int theIndex)
		{
			int index = -1;
			switch (theIndex)
			{
				case 7:
					index = BuffID.ShadowFlame;
					break;
				case 8:
					index = BuffID.CursedInferno;
					break;
				case 9:
					index = BuffID.Frostburn;
					break;
				case 10:
					index = BuffID.OnFire;
					break;
				case 11:
					index = BuffID.Ichor;
					break;
				case 12:
					index = ModContent.BuffType<Buffs.MoonLightCurse>();
					break;
			}

			return index;
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Torch God Summon Minion");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Weapons/TorchGodSummon"); }
		}

		public override bool CanDamage()
		{
			return false;
		}

		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = false;
			projectile.penetrate = -1;
			projectile.minion = true;
			aiType = 0;
		}

		public void ShootFlame(int damage, float knockBack, float speed2)
		{
			Vector2 mousePos = Main.MouseWorld;
			projectile.ai[1] = 180;
			projectile.localAI[0] = 180;
			projectile.netUpdate = true;

			Vector2 speed = Vector2.Normalize(mousePos - projectile.Center) * speed2;

			Main.PlaySound(SoundID.Item45, projectile.Center);

			Projectile.NewProjectile(projectile.Center, speed, ModContent.ProjectileType<TorchGodSummonProjectile>(), damage, knockBack, projectile.owner, projectile.ai[0]);

		}

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (projectile.ai[1] < 1)
				Lighting.AddLight(projectile.Center, TorchColors[(int)projectile.ai[0] % 16].ToVector3());

			projectile.ai[1] -= 1;

			projectile.localAI[0] -= 1;

			if (projectile.localAI[0] < 0 && projectile.localAI[0] > -10)
			{
				int dust = Dust.NewDust(projectile.position + new Vector2(Main.rand.NextFloat(-6, 6), Main.rand.NextFloat(-6, 6) + projectile.height / 4), projectile.width, projectile.height, DustID.AncientLight);
				Main.dust[dust].scale = 1.50f;
				Main.dust[dust].fadeIn = 1.2f;
				Main.dust[dust].color = TorchGodSummonMinion.TorchColors[(int)projectile.ai[0] % 16];
				Main.dust[dust].velocity = -Vector2.UnitY * 3f;
				Main.dust[dust].alpha = 200;
				Main.dust[dust].noGravity = true;

				if (projectile.localAI[0] == -1)
				{
					SoundEffectInstance snd = Main.PlaySound(SoundID.Item45, projectile.Center);
					if (snd != null)
					{
						snd.Pitch = 0.6f;
					}
				}
			}

			float us = 0f;
			float maxus = 0f;

			for (int i = 0; i < Main.maxProjectiles; i++) // Loop all projectiles
			{
				Projectile currentProjectile = Main.projectile[i];
				if (currentProjectile.active // Make sure the projectile is active
				&& currentProjectile.owner == Main.myPlayer // Make sure the projectile's owner is the client's player
				&& currentProjectile.type == projectile.type)
				{

					if (i == projectile.whoAmI)
						us = maxus;
					maxus += 1f;

				}
			}

			if (!player.active || player.dead || ((us >= ((player.maxMinions - (player.SGAPly().GetMinionSlots)) * 2) || player.HeldItem.type != ModContent.ItemType<TorchGodSummon>()) && projectile.ai[1] < 1))
			{
				projectile.Kill();
			}

			float angle = (us / maxus) * MathHelper.TwoPi;
			float dist = 64f;
			Vector2 wegohere = player.MountedCenter + Vector2.UnitX.RotatedBy(angle) * dist;

			projectile.Center = wegohere;

			if (projectile.localAI[0]>-10)
			SGAmod.PostDraw.Add(new PostDrawCollection(new Vector3(wegohere.X - Main.screenPosition.X, wegohere.Y - Main.screenPosition.Y, 128)));

		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D textureTorch = Main.tileTexture[TileID.Torches];

			if (textureTorch != null)
			{

				Texture2D textureFlame = Main.FlameTexture[0];

				Vector2 offset = new Vector2(11, 11);
				Rectangle rect = new Rectangle(projectile.ai[1] > -9999990 ? 66 : 0, (int)(projectile.ai[0] % 16) * 22, 22, 22);
				Rectangle rect2 = new Rectangle(0, (int)(projectile.ai[0] % 16) * 22, 22, 22);

				Vector2 flameoffset = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));


				spriteBatch.Draw(textureTorch, projectile.Center - Main.screenPosition, rect, Color.White, projectile.rotation, offset, new Vector2(1f, 1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

				spriteBatch.Draw(textureFlame, projectile.Center + (flameoffset / 6f) - Main.screenPosition, rect2, (Color.White * 0.75f) * MathHelper.Clamp((1f - projectile.localAI[0] - 5f) / 7f, 0f, 1f), projectile.rotation, offset, new Vector2(1f, 1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);

				flameoffset = new Vector2(Main.rand.NextFloat(-3, 3), Main.rand.NextFloat(-3, 3));

				spriteBatch.Draw(textureFlame, projectile.Center + flameoffset - Main.screenPosition, rect2, Color.White * MathHelper.Clamp((1f - projectile.localAI[0] - 5f) / 10f, 0f, 1f), projectile.rotation, offset, new Vector2(1f, 1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
			}

			return false;
		}

	}


}