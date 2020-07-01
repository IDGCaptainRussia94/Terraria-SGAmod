using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Idglibrary.Bases;

namespace SGAmod.HavocGear.Projectiles
{
	public class ThermalPikeProj : ProjectileSpearBase
	{

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thermal Pike");
		}

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            bool facingleft = projectile.Center.X < Main.player[projectile.owner].Center.X;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            Texture2D texture = Main.projectileTexture[projectile.type];
            Texture2D glow = mod.GetTexture("Items/GlowMasks/ThermalPike_Glow");
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
            Main.spriteBatch.Draw(glow, projectile.Center - Main.screenPosition, new Rectangle?(), Color.White, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
            return false;
        }

        public override void SetDefaults()
        {
            projectile.width = 35;
            projectile.height = 35;
            projectile.friendly = true;
            projectile.hostile = false;
            projectile.tileCollide = false;
            projectile.ignoreWater = true;
            projectile.penetrate = -1;
            projectile.ownerHitCheck = true;
            projectile.aiStyle = 19;
            projectile.melee = true;
            projectile.timeLeft = 90;
            projectile.hide = true;

            movein=1.2f;
            moveout=0.60f;
            thrustspeed=2f;
        }
        public override void AI()
        {
		base.AI();
        if (Main.rand.Next(0,30)<25){
        Vector2 adder=new Vector2(Main.rand.Next(-100,100),Main.rand.Next(-100,100))*0.15f;
        Vector2 center=new Vector2(projectile.position.X+(float)(projectile.width / 2),projectile.position.Y+(float)(projectile.width / 2));
        Vector2 launchvector=new Vector2((float)(Math.Cos(truedirection)),(float)(Math.Sin(truedirection)));
        int dust = Dust.NewDust((center+(launchvector*((Main.rand.Next(-50,80))*0.25f)))+adder, 0, 0, 6);
        Main.dust[dust].scale = 0.8f;
        Main.dust[dust].noGravity = false;
        Main.dust[dust].velocity = projectile.velocity*(float)(Main.rand.Next(20,100)*0.002f);
        }
		Lighting.AddLight(projectile.position, 0.6f, 0.5f, 0f);
	}

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if(!(Main.rand.Next(8) == 0))
            {
            Player player = Main.player[projectile.owner];
            target.immune[projectile.owner] = 2;
            target.AddBuff(mod.BuffType("ThermalBlaze"), 120);
            }
        }
    }
}