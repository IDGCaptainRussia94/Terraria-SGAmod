using System.Collections.Generic;
using Idglibrary;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class CelestialFlare : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Celestial Flare");
			Tooltip.SetDefault("Engulfs enemies in a devastating inferno\nSends an Explosion through the enemy on hit\nCounts as a True Melee sword");
		}
		
		public override void SetDefaults()
		{
			item.damage = 240;
			item.melee = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 30;
			item.useAnimation = 12;
			item.useStyle = 1;
			item.knockBack = 10;
			item.value = 500000;
			item.rare = 10;
	        item.UseSound = SoundID.Item1;
			item.autoReuse = true;
			item.useTurn = false;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/CelestialFlare_Glow");
			}
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType("Mudmore"), 1);
			recipe.AddIngredient(mod.ItemType("ThermalBlade"), 1);
			recipe.AddIngredient(ItemID.FragmentSolar, 8);
 			recipe.AddIngredient(mod.ItemType("StarMetalBar"), 12);
			recipe.AddIngredient(mod.ItemType("IlluminantEssence"), 20);
			recipe.AddIngredient(ItemID.SoulofLight, 6);
			recipe.AddTile(TileID.LunarCraftingStation);
			recipe.SetResult(this);
            recipe.AddRecipe();

		}

	public override void MeleeEffects(Player player, Rectangle hitbox)
	{

		for (int num475 = 0; num475 < 3; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("HotDust"));
		Main.dust[dust].scale=0.5f+(((float)num475)/3.5f);
		Main.dust[dust].velocity=new Vector2(0f,-5f);
		Main.dust[dust].velocity.Normalize();
		Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
		Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
		}
		Lighting.AddLight(player.position, 0.9f, 0.9f, 0f);
	}
	
	public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
	{

		if (!(Main.rand.Next(5) == 0))
		{
			target.AddBuff(mod.BuffType("ThermalBlaze"), 600, false);
			target.AddBuff(BuffID.Daybreak, 600, true);
			target.AddBuff(BuffID.OnFire, 600, true);
		}
			Projectile.NewProjectile(target.Center, Vector2.Normalize(target.Center - player.Center) * 12f, ProjectileID.SolarWhipSwordExplosion, (int)(damage*0.4), knockback/5f,player.whoAmI,0f, 0.85f + Main.rand.NextFloat() * 1.15f);
	}
    }
}