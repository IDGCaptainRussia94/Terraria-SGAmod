using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SGAmod.Buffs;
using Idglibrary;

namespace SGAmod.Items.Consumables
{
    class Jarate : ModItem
    {

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
			DisplayName.SetDefault("Jarate");
			Tooltip.SetDefault("Throws a jar of 'nature's rain', which inflicts Ichor on everyone in a large area for an extended time\nIf it directly hits an enemy, they will get Sodden instead even if immune\nThis increases any further damage they take by 33%\n'Heads up!'\n"+Idglib.ColorText(Color.Orange,"Requires 1 Cooldown stack, adds 30 seconds"));

		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Jarate"); }
		}

		public override void SetDefaults()
		{
			item.useStyle = 1;
			item.shootSpeed = 12f;
			item.shoot = mod.ProjectileType("JarateProj");
			item.width = 16;
			item.height = 16;
			item.maxStack = 10;
			item.consumable = true;
			item.UseSound = SoundID.Item1;
			item.useAnimation = 40;
			item.useTime = 40;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.value = Item.buyPrice(0, 0, 20, 0);
			item.rare = ItemRarityID.Yellow;
		}

		public override bool CanUseItem(Player player)
		{
			return player.SGAPly().CooldownStacks.Count < player.SGAPly().MaxCooldownStacks;
		}

		public override bool ConsumeItem(Player player)
		{
			return true;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			player.SGAPly().AddCooldownStack(30 * 60);
			return base.Shoot(player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);

		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("FieryShard"), 2);
			recipe.AddIngredient(mod.ItemType("MurkyGel"), 5);
			recipe.AddIngredient(ItemID.Bottle, 3);
			recipe.AddIngredient(ItemID.Ichor, 2);
			recipe.AddTile(TileID.WorkBenches);
			recipe.SetResult(this,3);
			recipe.AddRecipe();
		}

	}

	public class JarateProj : GasPasserProj
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Jar of Piss");
		}
		public override float beginhoming => 99990f;

		public override string Texture
		{
			get { return ("SGAmod/Items/Consumables/Jarate"); }
		}

		public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.ranged = true;
			projectile.timeLeft = 240;
			projectile.arrow = true;
			projectile.damage = 0;
			aiType = ProjectileID.WoodenArrowFriendly;
		}

		public override void AI()
		{
			effects(0);

			projectile.ai[0] = projectile.ai[0] + 1;
			projectile.velocity.Y += 0.15f;
			projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;


			NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), true, true, true, projectile)];
			if (target != null)
			{
				if (new Rectangle((int)projectile.position.X, (int)projectile.position.Y, projectile.width, projectile.height).Intersects
					(new Rectangle((int)target.position.X, (int)target.position.Y, target.width, target.height)))
				{
					IdgNPC.AddBuffBypass(target.whoAmI, mod.BuffType("Sodden"), 60*45);
					if (Main.player[projectile.owner].GetModPlayer<SGAPlayer>().MVMBoost)
						IdgNPC.AddBuffBypass(target.whoAmI,mod.BuffType("SoddenSlow"), 60 * 45);
					projectile.Kill();
				}
			}


		}

		public override void effects(int type)
		{
			if (type == 0)
			base.effects(type);
			if (type == 1)
			{

				Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/Custom/jar_explode").WithVolume(.7f).WithPitchVariance(.25f),projectile.Center);
				projectile.type = ProjectileID.IchorSplash;
				//int proj = Projectile.NewProjectile(new Vector2(projectile.Center.X, projectile.Center.Y), new Vector2(0, 0), mod.ProjectileType("GasCloud"), 1, projectile.knockBack, Main.player[projectile.owner].whoAmI);
				for (int q = 0; q < 100; q++)
				{
					float randomfloat = Main.rand.NextFloat(5f, 15f);
					Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
					Vector2 speedz = new Vector2(randomcircle.X, randomcircle.Y) * randomfloat;


					int dust = Dust.NewDust(projectile.Center-new Vector2(24,24), 48, 48, 75, speedz.X, speedz.Y, 80, Color.LightGoldenrodYellow * 0.85f, 8f);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].velocity = speedz;
					Main.dust[dust].fadeIn = 2f;
					Main.dust[dust].noLight = true;
				}

				for (int num172 = 0; num172 < Main.maxNPCs; num172 += 1)
				{
					NPC target = Main.npc[num172];
					float damagefalloff = 1F - ((target.Center - projectile.Center).Length() / 180f);
					// && target.modNPC.CanBeHitByProjectile(projectile) == true)
					if ((target.Center - projectile.Center).Length() < 180f && !target.friendly && !target.dontTakeDamage && (target.modNPC == null || (target.modNPC != null)))
					{
						target.AddBuff(BuffID.Ichor,(60*5)+(int)(damagefalloff*60f*10));
					}
				}

			}

		}


	}

}



