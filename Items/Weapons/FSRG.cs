using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using SGAmod.Items.Weapons.SeriousSam;

namespace SGAmod.Items.Weapons
{
	public class FSRG : Vibranium.VibraniumText
    {
		int shootCount = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("F.S.R.G");
			Tooltip.SetDefault("'Furious Sting-Ray Gun'\nRapidly fires multi-hitting flaming stingers that cause no immunity frames\nThese inflict Gourged and leave behind poison clouds\nStingers are most effective against larger enemies\n75% to not consume ammo");
		}

		public override void SetDefaults()
		{
			item.damage = 75;
			item.ranged = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 10;
			item.useAnimation = 10;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = 750000;
			item.rare = ItemRarityID.Purple;
			item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 20f;
			item.useAmmo = AmmoID.Dart;
		}

        public override bool ConsumeAmmo(Player player)
        {
			return Main.rand.Next(100) < 25;
        }

        public override Vector2? HoldoutOffset()
		{
			return new Vector2(-24, 0);
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			shootCount += 1;
			float speed=1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(4);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 45f;

			for (int i = 0; i < numberProjectiles; i++)
			{
				int typeOfShot = mod.ProjectileType("FlamingStinger");
				if (false)
					typeOfShot = type;

				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0,100)/100f)) * .3f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj=Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y,typeOfShot , damage, knockBack, player.whoAmI);
				Main.projectile[proj].friendly=true;
				Main.projectile[proj].hostile=false;
				Main.projectile[proj].knockBack=item.knockBack;
				Main.projectile[proj].ai[0] = (int)Main.rand.Next(0, 80);
				Main.projectile[proj].netUpdate = true;
				Main.projectile[proj].localNPCHitCooldown = 3;
				Main.projectile[proj].usesLocalNPCImmunity = true;

				IdgProjectile.AddOnHitBuff(proj,BuffID.OnFire,60*6);
				IdgProjectile.AddOnHitBuff(proj, mod.BuffType("Gourged"), 60 * 6);
				IdgProjectile.Sync(proj);
			}
			return false;
		}

		public override void AddRecipes()
		{
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType <Gatlipiller>(), 1);
			recipe.AddIngredient(ItemID.Stinger, 12);
			recipe.AddIngredient(ModContent.ItemType <HavocGear.Items.Weapons.SharkTooth>(), 50);
			recipe.AddIngredient(ModContent.ItemType<HavocGear.Items.VirulentBar>(), 5);
			recipe.AddIngredient(ModContent.ItemType<IlluminantEssence>(), 20);
			recipe.AddIngredient(ModContent.ItemType<VibraniumBar>(), 8);
			recipe.AddIngredient(ItemID.LunarBar, 5);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			//recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
            recipe.AddRecipe();
		}

	}

	public class FlamingStinger : ModProjectile
	{

		int fakeid=ProjectileID.Stinger;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Flaming Stinger");
		}

		public override void SetDefaults()
		{
			base.SetDefaults();
			projectile.width = 8;
			projectile.height = 8;
			projectile.ranged = true;
			projectile.extraUpdates = 3;
			projectile.penetrate = 5;
			projectile.timeLeft = 300;
			projectile.localNPCHitCooldown = 3;
			projectile.usesLocalNPCImmunity = true;
		}

		public override string Texture
		{
			get { return "Terraria/Projectile_" + fakeid; }
		}

		public override bool PreKill(int timeLeft)
		{
			projectile.type=fakeid;
			return true;
		}

		public override void AI()
		{
			projectile.ai[0] += 1;
			if (projectile.ai[0] % 40 == 0)
			{
				Vector2 avel = projectile.velocity.RotatedBy(MathHelper.ToRadians(projectile.ai[0] % 80==0 ? 90 : -90))/5f;
				int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, avel.X, avel.Y, ProjectileID.SporeGas3, projectile.damage*2, projectile.knockBack, projectile.owner);
				Main.projectile[proj].usesLocalNPCImmunity = true;
				Main.projectile[proj].localNPCHitCooldown = -1;
				Main.projectile[proj].scale = 0.5f;
				Main.projectile[proj].extraUpdates = 1;
				Main.projectile[proj].ranged = true;
				Main.projectile[proj].netUpdate = true;
				IdgProjectile.AddOnHitBuff(proj, mod.BuffType("AcidBurn"), 60 * 2);
				IdgProjectile.Sync(proj);
			}

			int dust = Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 6);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = projectile.velocity*(float)(Main.rand.Next(20,100)*0.005f);
        projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;
		}

	}


}
