using Microsoft.Xna.Framework;
using Terraria;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;

namespace SGAmod.Items.Tools
{
	public class Braxsaw : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Braxsaw");
			Tooltip.SetDefault("Mine through the oldest fabric of the universe!");
		}

		public override void SetDefaults()
		{
			item.damage = 150;
			item.melee = true;
			item.width = 56;
			item.height = 22;
			item.useTime = 1;
			item.useAnimation = 18;
			item.channel = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.pick = 250;
			item.axe = 150;
			item.tileBoost += 5;
			item.useStyle = 5;
			item.knockBack = 5;
			item.value = 3000000;
			item.rare = 11;
			item.UseSound = SoundID.Item23;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("BraxsawProj");
			/*
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/Braxsaw_Glow");
				item.GetGlobalItem<ItemUseGlow>().GlowColor = delegate (Item item, Player player)
				{
					return Color.White * (0.75f+MathHelper.Clamp((float)(Math.Sin(Main.GlobalTime)*0.35f)+0.75f,0f,1f)*0.25f);
				};
			}
			*/
			item.shootSpeed = 40f;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<StarMetalBar>(), 32);
			recipe.AddIngredient(ModContent.ItemType<LunarRoyalGel>(), 10);
			recipe.AddIngredient(ItemID.Drax, 1);
			recipe.AddIngredient(ModContent.ItemType <BoreicDrill>(), 1);
			recipe.AddIngredient(ModContent.ItemType <HavocGear.Items.Tools.VirulentDrill>(), 1);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

        public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
        {
        float vel = item.velocity.X / 6f;
			//new Vector2(0, Main.itemTexture[item.type].Height - 4)
			Vector2 drawPos = (item.Center) - Main.screenPosition;

			Color glowColor = Color.White * (0.60f + MathHelper.Clamp((float)(Math.Sin(Main.GlobalTime) * 0.65f) + 0.50f, 0f, 1f) * 0.40f);

			Texture2D glow = mod.GetTexture("Items/GlowMasks/Braxsaw/Braxsaw_Glow");

			spriteBatch.Draw(glow, drawPos, null, glowColor, vel, Main.itemTexture[item.type].Size() / 2f,scale, SpriteEffects.None, 0f);
		}
	}

	public class BraxsawProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Boreic Drill");
		}

		public override string Texture
		{
			get { return ("SGAmod/Items/Tools/BraxsawProj"); }
		}

		public override void SetDefaults()
		{
			int[] itta = { ProjectileID.SolarFlareDrill, ProjectileID.NebulaDrill , ProjectileID.StardustDrill , ProjectileID.VortexDrill };
			projectile.CloneDefaults(itta[Main.rand.Next(0, itta.Length)]);
			projectile.glowMask = 0;
		}
        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
			Vector2 drawPos = (projectile.Center) - Main.screenPosition;

			Texture2D glow = mod.GetTexture("Items/GlowMasks/Braxsaw/BraxsawProj_Glow");
			Texture2D itemtex = Main.projectileTexture[projectile.type];

			Color glowColor = Color.White * (0.60f + MathHelper.Clamp((float)(Math.Sin(Main.GlobalTime) * 0.65f) + 0.50f, 0f, 1f) * 0.40f);

			SpriteEffects effect = projectile.rotation.ToRotationVector2().Y < 0 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			Vector2 offset = new Vector2(glow.Width / 2, (int)(glow.Height * 0.35f));

			spriteBatch.Draw(itemtex, drawPos, null, lightColor, projectile.rotation, offset, projectile.scale, effect, 0f);



			List<(Texture2D, float)> glowTexts = new List<(Texture2D, float)>();
			for (int i = 0; i < 4; i += 1)
			{
				float percent = (i / 4f) * MathHelper.TwoPi;
				float fadeInAndOut = Math.Max(0, 0.35f + ((float)(Math.Sin((-Main.GlobalTime*1.5f) + percent) * 0.50f)));
				glowTexts.Add((mod.GetTexture("Items/GlowMasks/Braxsaw/BraxsawProj_Glow" + (i + 1)), fadeInAndOut));
			}
			int index = 0;
			foreach ((Texture2D, float) tex in glowTexts.OrderBy(testby => 10f-testby.Item2))
			{
				Color glowColor2 = Color.White * tex.Item2;
				spriteBatch.Draw(tex.Item1, drawPos, null, glowColor2, projectile.rotation, offset, projectile.scale, effect, 0f);
				index += 1;
			}


			spriteBatch.Draw(glow, drawPos, null, glowColor, projectile.rotation, offset, projectile.scale, effect, 0f);
			return false;
		}

    }

}