using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace SGAmod.HavocGear.Items.Weapons
{
	public class ThrownTrident : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elemental's Trident");
			//Tooltip.SetDefault("Shoots a spread of bullets");
		}

		public override void SetDefaults()
		{
			item.CloneDefaults(ItemID.DayBreak);
			item.damage = 15;
			item.ranged = true;
			item.width = 56;
			item.height = 28;
			item.useTime = 20;
			item.useAnimation = 20;
			item.noMelee = true;
			item.melee = false;
			item.ranged = false;
			item.magic = false;
			item.thrown = false;
			item.Throwing().thrown = true;
			item.knockBack = 5;
			item.value = 10000;
			item.rare = ItemRarityID.Green;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("ThrownTridentFriendly");
			item.shootSpeed = 12f;

		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
				speedY-=1f;
				Vector2 perturbedSpeed = new Vector2(speedX, speedY).RotatedByRandom(MathHelper.ToRadians(10));
				//float scale = (1f - (Main.rand.NextFloat() * .01f))*(player.thrownVelocity);
				//perturbedSpeed = perturbedSpeed * scale; 
				speedX=perturbedSpeed.X; speedY=perturbedSpeed.Y;		
			return true;
		}
	}
}
