using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary;
using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Items.Tools
{
	public class Geyodo : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geyodo");
			Tooltip.SetDefault("Control a yoyo bound with stronger pickaxes\nCarves tunnels out of earth AND your enemies, even more so!");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			item.damage = 20;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.noMelee = true;
			item.knockBack = 7f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Orange;
			item.pick = 180;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item19;
			item.shoot = mod.ProjectileType("GeyodoProj");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Excavator"), 1);
			recipe.AddRecipeGroup("SGAmod:Tier5Pickaxe", 1);
			recipe.AddRecipeGroup("SGAmod:Tier6Pickaxe", 1);
			recipe.AddRecipeGroup("SGAmod:Tier7Pickaxe", 1);
			recipe.AddIngredient(ItemID.MoltenPickaxe, 1);
			recipe.AddIngredient(mod.ItemType("VirulentBar"), 5);
			recipe.AddIngredient(mod.ItemType("CryostalBar"), 5);
			recipe.AddIngredient(mod.ItemType("WraithFragment4"), 15);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
		public class GeyodoProj : ExcavatorProj
	{
		public override int[] Pickaxes => new int[] { ItemID.CobaltPickaxe, ItemID.MythrilPickaxe, ItemID.AdamantitePickaxe, ItemID.MoltenPickaxe };
		public override int RealPickPower => 45;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("GeyodoProj");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 400f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 8f;
		}
	}

	public class Excavator : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("The Excavator");
			Tooltip.SetDefault("Control a yoyo bound with pickaxes\nCarves tunnels out of earth AND your enemies!");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			item.damage = 20;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.noMelee = true;
			item.knockBack = 7f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Orange;
			item.pick = 70;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item19;
			item.shoot = mod.ProjectileType("ExcavatorProj");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddRecipeGroup("SGAmod:Tier1Pickaxe", 1);
			recipe.AddRecipeGroup("SGAmod:Tier2Pickaxe", 1);
			recipe.AddRecipeGroup("SGAmod:Tier3Pickaxe", 1);
			recipe.AddRecipeGroup("SGAmod:Tier4Pickaxe", 1);
			recipe.AddIngredient(ItemID.WoodYoyo, 1);
			recipe.AddIngredient(mod.ItemType("EvilBossMaterials"), 15);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int proj = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, (int)((double)damage), knockBack, player.whoAmI, 0.0f, 0.0f);
			//(Main.projectile[proj].modProjectile as ExcavatorProj).PickPower = item.pick;
			Main.projectile[proj].netUpdate = true;

			return false;
		}
	}

		public class ExcavatorProj : ModProjectile
	{
		public int PickPower = 0;
		public int PowerPick = 0;
		public virtual int RealPickPower => 10;
		public virtual int[] Pickaxes => new int[] { ItemID.CopperPickaxe, ItemID.IronPickaxe, ItemID.SilverPickaxe, ItemID.GoldPickaxe };
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("ExcavatorProj");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 6f;
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			PickPower = reader.ReadInt32();
		}
		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(PickPower);
		}

		public override void SetDefaults()
		{
			Projectile refProjectile = new Projectile();
			refProjectile.SetDefaults(ProjectileID.TheEyeOfCthulhu);
			projectile.extraUpdates = 0;
			projectile.width = 16;
			projectile.height = 16;
			projectile.aiStyle = 99;
			projectile.friendly = true;
			projectile.penetrate = -1;
			projectile.melee = true;
			projectile.scale = 1f;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			if (owner != null && !owner.dead)
			{
				PickPower += 1;

				if (PickPower > 60)
				{
					int pickPower = RealPickPower;
					owner.SGAPly().forcedMiningSpeed = 10;
					//owner.SGAPly().UseTimeMulPickaxe
					if ((PickPower % (int)(20 * (owner.meleeSpeed/(owner.SGAPly().UseTimeMulPickaxe/owner.pickSpeed)))) == 0)
					{
						PowerPick += 1;
						Point16 hereIAm = new Point16((int)projectile.Center.X >> 4, (int)projectile.Center.Y >> 4);
						for (int x = -3; x <= 3; x += 1)
						{
							for (int y = -3; y <= 3; y += 1)
							{
								if (new Vector2(x, y).LengthSquared() < 3*3 && !Main.tileAxe[Main.tile[hereIAm.X + x, hereIAm.Y + y].type])
									owner.PickTile(hereIAm.X + x, hereIAm.Y + y, pickPower);
							}
						}
					}
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{

			float percenthit = MathHelper.Clamp(PickPower/60f, 0f,1f);
			for (int i = 0; i < Pickaxes.Length; i += 1)
			{
				Texture2D tex = Main.itemTexture[Pickaxes[i]];
				Vector2 offset = new Vector2(0, tex.Height*percenthit);
				float angle = projectile.rotation + MathHelper.TwoPi * (i / (float)Pickaxes.Length);
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle(0, (int)(tex.Height * (1f-percenthit)),(int)(tex.Width* percenthit), (int)(tex.Height* (percenthit))), lightColor, angle, offset, projectile.scale, default, 0);
			}
			return true;
		}

	}

}