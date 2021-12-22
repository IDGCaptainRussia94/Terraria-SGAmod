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
using System.Linq;
using SGAmod.Dimensions;

namespace SGAmod.Items.Tools
{
	public class TerraExcavator : Geyodo
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Excavator");
			Tooltip.SetDefault("Control a yoyo bound with even stronger pickaxes\nCarves tunnels out of earth AND your enemies, terra tier!");
		}

		public override void SetDefaults()
		{
			Item refItem = new Item();
			refItem.SetDefaults(ItemID.TheEyeOfCthulhu);
			item.damage = 46;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.noMelee = true;
			item.knockBack = 7f;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.Lime;
			item.pick = 200;
			item.noUseGraphic = true;
			item.autoReuse = true;
			item.UseSound = SoundID.Item19;
			item.shoot = mod.ProjectileType("TerraExcavatorProj");
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Geyodo"), 1);
			recipe.AddIngredient(ItemID.SpectrePickaxe, 1);
			recipe.AddIngredient(ItemID.ShroomiteDiggingClaw, 1);
			recipe.AddIngredient(ItemID.ChlorophytePickaxe, 1);
			recipe.AddIngredient(ItemID.PickaxeAxe, 1);
			recipe.AddIngredient(ItemID.BrokenHeroSword, 1);
			recipe.AddTile(mod.TileType("ReverseEngineeringStation"));
			recipe.SetResult(this);
			recipe.AddRecipe();

		}
	}
	public class TerraExcavatorProj : ExcavatorProj
	{
		public override int[] Pickaxes => new int[] { ItemID.SpectrePickaxe, ItemID.ShroomiteDiggingClaw, ItemID.ChlorophytePickaxe, ItemID.PickaxeAxe };
		public override int RealPickPower => 100;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Terra Excavator");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 8f;
		}
	}
	public class Geyodo : Excavator
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
			item.damage = 36;
			item.useTime = 60;
			item.useAnimation = 60;
			item.useStyle = 5;
			item.channel = true;
			item.melee = true;
			item.noMelee = true;
			item.knockBack = 7f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.rare = ItemRarityID.Pink;
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
		public override int RealPickPower => 70;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Geyodo Proj");
			//ProjectileID.Sets.YoyosLifeTimeMultiplier[projectile.type] = 5000f;
			ProjectileID.Sets.YoyosMaximumRange[projectile.type] = 300f;
			ProjectileID.Sets.YoyosTopSpeed[projectile.type] = 7f;
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
		public virtual int RealPickPower => 20;
		public virtual int[] Pickaxes => new int[] { ItemID.CopperPickaxe, ItemID.IronPickaxe, ItemID.SilverPickaxe, ItemID.GoldPickaxe };
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Excavator Proj");
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

						int dist = 64 * 64;
						foreach(Projectile asteriodproj in Main.projectile.Where(testby => testby.active && testby.modProjectile != null && testby.modProjectile is IMineableAsteriod && (testby.Center - projectile.Center).LengthSquared() < dist))
                        {
							IMineableAsteriod asteriod = asteriodproj.modProjectile as IMineableAsteriod;
							asteriod.MineAsteriod(owner.HeldItem,false);
						}

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

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			Effect fadeIn = SGAmod.FadeInEffect;

			float percenthit = MathHelper.Clamp(PickPower / 60f, 0f, 1f);
			float percenthit2 = MathHelper.Clamp((PickPower - 45) / 60f, 0f, 1f);

			fadeIn.Parameters["alpha"].SetValue(1);
			fadeIn.Parameters["strength"].SetValue(1f-percenthit2);
			fadeIn.Parameters["fadeColor"].SetValue(Color.Goldenrod.ToVector3());
			fadeIn.Parameters["blendColor"].SetValue(lightColor.ToVector3());

			fadeIn.CurrentTechnique.Passes["FadeIn"].Apply();

			for (int i = 0; i < Pickaxes.Length; i += 1)
			{
				Texture2D tex = Main.itemTexture[Pickaxes[i]];
				Vector2 offset = new Vector2(0, tex.Height*percenthit);
				float angle = projectile.rotation + MathHelper.TwoPi * (i / (float)Pickaxes.Length);
				//spriteBatch.Draw(tex, projectile.Center - Main.screenPosition,null, lightColor, angle, offset, projectile.scale, default, 0);

				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, new Rectangle(0, (int)(tex.Height * (1f-percenthit)),(int)(tex.Width* percenthit), (int)(tex.Height* (percenthit))), Color.White, angle, offset, projectile.scale, default, 0);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			/*Effect RadialEffect = SGAmod.RadialEffect;

			Texture2D mainTex = mod.GetTexture("GreyHeart");//Main.projectileTexture[projectile.type];

			RadialEffect.Parameters["overlayTexture"].SetValue(mod.GetTexture("Space"));
			RadialEffect.Parameters["alpha"].SetValue(1f);
			RadialEffect.Parameters["texOffset"].SetValue(new Vector2(-Main.GlobalTime*0.125f, Main.GlobalTime * 0.275f));
			RadialEffect.Parameters["texMultiplier"].SetValue(new Vector2(2f,1f));
			RadialEffect.Parameters["ringScale"].SetValue(0.30f);
			RadialEffect.Parameters["ringOffset"].SetValue(0.50f);
			RadialEffect.Parameters["tunnel"].SetValue(false);

			RadialEffect.CurrentTechnique.Passes["Radial"].Apply();

			spriteBatch.Draw(mainTex, projectile.Center - Main.screenPosition, null, Color.White, 0, mainTex.Size()/2f, 32f, default, 0);*/

			return true;
		}

	}

}