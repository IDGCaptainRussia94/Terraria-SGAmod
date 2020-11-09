using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Items.Weapons.SeriousSam;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
	public class WaveBeam : SeriousSamWeapon
	{
		private bool altfired=false;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam");
			Tooltip.SetDefault("Fires 2 helix shots that pass through tiles\nhold fire to charge a shot that does 15X damage with increased crit chance, homes in slighty, and stuns enemies\nworm enemies are unaffected by stun and bosses are stunned for far shorter times\nAlso try Metroid mod by PhilBill44! (Godspeed Phil, godspeed!)");
		}

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
			base.ModifyTooltips(tooltips);

			Color c = Main.hslToRgb((float)(Main.GlobalTime/4)%1f, 0.4f, 0.45f);
            //string potion="[i:" + ItemID.RedPotion + "]";
            tooltips.Add(new TooltipLine(mod,"IDG Debug Item", Idglib.ColorText(c,"PhilBill44's dev weapon")));
        }

		public override void SetDefaults()
		{
			item.damage = 250;
			item.crit = 5;
			item.magic = true;
			item.width = 32;
			item.height = 62;
			item.useTime = 15;
			item.useAnimation = 15;
			item.useStyle = 5;
			item.noMelee = true;
			item.knockBack = 2;
			item.value = Item.buyPrice(0,50,0,0);
			item.rare = 11;
			//item.UseSound = SoundID.Item99;
			item.autoReuse = true;
			item.shoot = 10;
			item.shootSpeed = 50f;
			item.noUseGraphic = true;
			item.channel=true;
			item.reuseDelay = 5;
			item.expert=true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("NoviteBlaster"), 1);
			recipe.AddIngredient(ItemID.LunarBar, 8);
			recipe.AddIngredient(ItemID.FragmentVortex, 6);
			recipe.AddIngredient(ItemID.FragmentNebula, 5);
			recipe.AddIngredient(mod.ItemType("AdvancedPlating"), 5);
			recipe.AddIngredient(mod.ItemType("PlasmaCell"), 2);
			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 5);
			recipe.AddIngredient(mod.ItemType("ManaBattery"), 2);
			recipe.AddIngredient(mod.ItemType("CosmicFragment"), 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{

			float speed=1.5f;
			float numberProjectiles = 1;
			float rotation = MathHelper.ToRadians(0);
			position += Vector2.Normalize(new Vector2(speedX, speedY)) * 8f;
			if (player.ownedProjectileCounts[mod.ProjectileType("WaveBeamCharging")]<1){
			//if (altfired){
			int proj=Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("WaveBeamCharging"), damage, knockBack, player.whoAmI);

			/*}else{
			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = (new Vector2(speedX, speedY)*speed).RotatedBy(MathHelper.Lerp(-rotation, rotation, (float)Main.rand.Next(0,100)/100f)) * .2f; // Watch out for dividing by 0 if there is only 1 projectile.
				int proj=Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("WaveBeam"), damage, knockBack, player.whoAmI);
				Main.projectile[proj].timeLeft=60;
				Main.projectile[proj].penetrate=2;
				Main.projectile[proj].knockBack=item.knockBack;
				IdgProjectile.AddOnHitBuff(proj,BuffID.OnFire,60*10);
			*/
		}
			return false;
		}

	}

	public class WaveBeamCharging : ModProjectile
	{

		int chargeuptime=90;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Wave Beam Charging");
		}

	public override bool? CanHitNPC(NPC target){return false;}

			public override string Texture
		{
			get { return("SGAmod/Projectiles/WaveProjectile");}
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile=false;
			projectile.friendly=true;
			projectile.tileCollide = false;
			projectile.magic = true;
			aiType = 0;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			return false;
		}

		public override void AI()
		{
		Player player=Main.player[projectile.owner];

		if (player==null)
		projectile.Kill();
		if (player.dead)
		projectile.Kill();
		projectile.timeLeft=2;
		player.itemTime=6;
		player.itemAnimation=6;
		Vector2 direction=(Main.MouseWorld-player.Center);
		Vector2 directionmeasure=direction;
		direction.Normalize();
		projectile.ai[0]+=1;
		bool channeling = ((player.channel || projectile.ai[0]<5) && !player.noItems && !player.CCed);
		projectile.Center=player.Center+direction*16;

		int num315;
		if (projectile.ai[0]>10){

		player.bodyFrame.Y=player.bodyFrame.Height*3;
		if (directionmeasure.Y-Math.Abs(directionmeasure.X)>25)
		player.bodyFrame.Y=player.bodyFrame.Height*4;
		if (directionmeasure.Y+Math.Abs(directionmeasure.X)<-25)
		player.bodyFrame.Y=player.bodyFrame.Height*2;
		if (directionmeasure.Y+Math.Abs(directionmeasure.X)<-160)
		player.bodyFrame.Y=player.bodyFrame.Height*5;
		player.direction=(directionmeasure.X>0).ToDirectionInt();

		if (projectile.ai[0]<chargeuptime){
		for (num315 = 0; num315 < 2; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X-1,projectile.Center.Y)+randomcircle*20, 0, 0, 185, 0f, 0f, 100, default(Color), 0.75f);

					Main.dust[num622].scale = 1f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = -randomcircle.X;
					Main.dust[num622].velocity.Y = -randomcircle.Y;
					Main.dust[num622].alpha = 150;
			}
			}else{
		for (num315 = 0; num315 < 2; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X-1,projectile.Center.Y), 0, 0, 185, 0f, 0f, 100, default(Color), 0.75f);

					Main.dust[num622].scale = 1.5f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X*2;
					Main.dust[num622].velocity.Y = randomcircle.Y*2;
					Main.dust[num622].alpha = 100;
			}
			}
		}

		if (projectile.ai[0]==chargeuptime){
		for (num315 = 0; num315 < 60; num315 = num315 + 1)
			{
					Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
					int num622 = Dust.NewDust(new Vector2(projectile.Center.X-1,projectile.Center.Y), 0, 0, 185, 0f, 0f, 100, default(Color), 0.75f);

					Main.dust[num622].scale = 2.8f;
					Main.dust[num622].noGravity=true;
					//Main.dust[num622].fadeIn = 1f + (float)Main.rand.Next(10) * 0.1f;
					Main.dust[num622].velocity.X = randomcircle.X*5f;
					Main.dust[num622].velocity.Y = randomcircle.Y*5f;
					Main.dust[num622].alpha = 200;
			}
		}

		if (!channeling){
				float speed=15f;
				Vector2 perturbedSpeed = (new Vector2(direction.X, direction.Y)*speed); // Watch out for dividing by 0 if there is only 1 projectile.

				if (projectile.ai[0]>chargeuptime-1){
				perturbedSpeed*=1.25f;
				int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("ChargedWave"), projectile.damage*15, projectile.knockBack, player.whoAmI);
				Main.projectile[proj].timeLeft=120;
				Main.projectile[proj].penetrate=1;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wave_Beam_Charge_Shot").WithVolume(.7f).WithPitchVariance(.25f),projectile.Center);
				}else{
				int proj=Projectile.NewProjectile(projectile.Center.X, projectile.Center.Y, perturbedSpeed.X, perturbedSpeed.Y, mod.ProjectileType("WaveBeam"), projectile.damage, projectile.knockBack, player.whoAmI);
				Main.projectile[proj].timeLeft=120;
				Main.projectile[proj].penetrate=1;
				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/Wide_Beam_Shot").WithVolume(.7f).WithPitchVariance(.25f),projectile.Center);
				}

		projectile.Kill();
		}

	}

}

}
