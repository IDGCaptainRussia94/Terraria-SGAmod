using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.HavocGear.Items.Weapons
{
	public class Mudmore : ModItem
	{
        public override void SetDefaults()
		{
			base.SetDefaults();

            item.damage = 50;
            item.width = 50;
			item.height = 58;
            item.melee = true;
            item.useTurn = true;
            item.rare = 4;
            item.useStyle = 1;
            item.useAnimation = 26;
           	item.knockBack = 1;
            item.useTime = 36;
            item.consumable = false;
            item.UseSound = SoundID.Item1;
        }

    		public override void MeleeEffects(Player player, Rectangle hitbox)
        {

            for (int num475 = 0; num475 < 3; num475++)
            {
                int dust = Dust.NewDust(new Vector2(hitbox.X, hitbox.Y), hitbox.Width, hitbox.Height, 184);
                Main.dust[dust].scale = 0.5f + (((float)num475) / 3.5f);
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 2f) + (player.itemRotation.ToRotationVector2());
                Main.dust[dust].noGravity = true;
                //Main.dust[dust].velocity.Normalize();
                //Main.dust[dust].velocity+=new Vector2(player.velocity.X/4,0f);
                //Main.dust[dust].velocity*=(((float)Main.rand.Next(0,100))/30f);
            }

            Lighting.AddLight(player.position, 0.1f, 0.1f, 0.9f);
        }

        public override void SetStaticDefaults()
    {
      DisplayName.SetDefault("Mudmore");
      Tooltip.SetDefault("Releases mud upon striking enemies\nMud splashes the Oiled debuff onto nearby enemies");
    }


        public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
        {
            Projectile.NewProjectile(target.Center.X, target.Center.Y, Main.rand.Next(-3, 4), Main.rand.Next(-3, 0), mod.ProjectileType("MudBlob"), item.damage - 30, 0f, Main.myPlayer, 0f, 0f);
        }

    }
}   
