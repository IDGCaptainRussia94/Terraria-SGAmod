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

namespace SGAmod.Items.Weapons.Technical
{
	public class NoviteKnife : SeriousSamWeapon
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Novite Knife");
			Tooltip.SetDefault("Instantly hits against targets where you swing\nHitting some types of targets in the rear will backstab, automatically becoming a crit");
		}
		public override void SetDefaults()
		{
			base.SetDefaults();

			item.damage = 20;
			item.width = 48;
			item.height = 48;
			item.melee = true;
			item.useTurn = true;
			item.rare = 1;
			item.value = 2000;
			item.useStyle = 1;
			item.useAnimation = 50;
			item.useTime = 50;
			item.knockBack = 8;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.consumable = false;
			item.noMelee = true;
			item.shootSpeed = 2f;
			item.shoot = ModContent.ProjectileType<NoviteStab>();
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/Weapons/Technical/NoviteKnife");
				item.GetGlobalItem<ItemUseGlow>().angleAdd = MathHelper.ToRadians(-20);
			}
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBar"), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Knife").WithVolume(.7f).WithPitchVariance(.15f), player.Center);
			return true;
		}

	}

	public class NoviteStab : ModProjectile
	{
		public override void SetDefaults()
		{
			projectile.width = 6;
			projectile.height = 6;
			projectile.aiStyle = -1;
			projectile.friendly = true;
			projectile.hostile = false;
			projectile.penetrate = 1;
			projectile.melee = true;
			projectile.timeLeft = 30;
			projectile.extraUpdates = 30;
			aiType = -1;
			Main.projFrames[projectile.type] = 1;
		}

		public override string Texture
		{
			get { return "SGAmod/HavocGear/Projectiles/BoulderBlast"; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Stab");
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.Kill();
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			Player player = Main.player[projectile.owner];
			if (target.spriteDirection== player.direction)
			{
				if ((target.aiStyle > 1 && target.aiStyle < 10) || target.aiStyle == 14 || target.aiStyle == 16 || target.aiStyle == 26 || target.aiStyle == 39 || target.aiStyle == 41 || target.aiStyle == 44)
					crit = true;

			}
		}

		public override void AI()
		{

		}
	}

}
