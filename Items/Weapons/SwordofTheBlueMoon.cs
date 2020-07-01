using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons
{
	public class SwordofTheBlueMoon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sword of the Blue Moon");
			Tooltip.SetDefault("Born from a great white being?\nI'm sure Moonlord fits that description");
		}
		
		public override void SetDefaults()
		{
			item.damage = 160;
			item.melee = true;
			item.width = 44;
			item.height = 52;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = 1;
			item.knockBack = 10;
			item.value = 250000;
			item.rare = 9;
	        item.UseSound = SoundID.Item71;
	     	item.autoReuse = false;
			item.useTurn = false;
		    
		}

		public override void MeleeEffects(Player player, Rectangle hitbox)
	{

		for (int num475 = 0; num475 < 3; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 20);
		Main.dust[dust].scale=0.5f+(((float)num475)/3.5f);
		Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
		Main.dust[dust].velocity=(randomcircle/2f)+(player.itemRotation.ToRotationVector2());
		Main.dust[dust].noGravity=true;
		//Main.dust[dust].velocity.Normalize();
		//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
		//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
		}

		for (int num475 = 3; num475 < 5; num475++)
		{
		int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 27);
		Main.dust[dust].scale=0.5f+(((float)num475)/15f);
		Vector2 randomcircle=new Vector2(Main.rand.Next(-8000,8000),Main.rand.Next(-8000,8000)); randomcircle.Normalize();
		Main.dust[dust].velocity=(randomcircle/3f)+((player.direction)*player.itemRotation.ToRotationVector2()*(float)num475);
		Main.dust[dust].noGravity=true;
		//Main.dust[dust].velocity.Normalize();
		//Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
		//Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
		}

		Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
	}

}
	
}