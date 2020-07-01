using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class ThermalBlade : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Blade");
		}
		
		public override void SetDefaults()
		{
			item.damage = 56;
			item.melee = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 28;
			item.useAnimation = 20;
			item.useStyle = 1;
			item.knockBack = 8;
			item.value = 100000;
			item.rare = 6;
	        item.UseSound = SoundID.Item1;		
			item.autoReuse = true;
			if (!Main.dedServ)
			{
				item.GetGlobalItem<ItemUseGlow>().glowTexture = mod.GetTexture("Items/GlowMasks/ThermalBlade_Glow");
			}

		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "FieryShard", 12);
            recipe.AddIngredient(mod.ItemType("UnmanedBar"), 10);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
            recipe.AddRecipe();
        }

		public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(8) == 0)
			{
				int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("HotDust"));
		Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = player.velocity*(float)(Main.rand.Next(20,100)*0.002f);
			}
			Lighting.AddLight(player.position, 0.6f, 0.5f, 0f);
		}
		
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if(!(Main.rand.Next(2) == 0))
			{
				target.AddBuff(mod.BuffType("ThermalBlaze"), 300, false);
			}
		}
    }
}