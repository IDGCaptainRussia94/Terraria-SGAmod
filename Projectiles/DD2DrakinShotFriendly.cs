using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Projectiles
{
	//Not a perfect copy, but very close. The vanilla projectile has a shader applied to it which is not implemented here.
	public class DD2DrakinShotFriendly : ModProjectile
	{
		public override string Texture => "Terraria/Projectile_" + ProjectileID.DD2DrakinShot;
        public override void SetStaticDefaults()
        {
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;    //The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;        //The recording mode
		}
        public override void SetDefaults()
		{
			projectile.CloneDefaults(ProjectileID.DD2DrakinShot);
			projectile.magic = true;
			projectile.friendly = true;
			projectile.hostile = false;
			aiType = ProjectileID.DD2DrakinShot;
		}
		public override void PostDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
		}
		public override void Kill(int timeLeft)
		{
			for (int i = 0; i < 30; i++)
			{
				int selectRand = Utils.SelectRandom(Main.rand, 27, 27, 62); //27 = Shadowflame, 62 = Purple Torch
				Dust killDust = Main.dust[Dust.NewDust(projectile.position, projectile.width, projectile.height, selectRand)];
				killDust.noGravity = true;
				killDust.scale = 1.25f + Main.rand.NextFloat();
				killDust.fadeIn = 0.25f;
				Dust killDust2 = killDust;
				Dust killDust3 = killDust2;
				killDust3.velocity *= 2f;
				killDust.noLight = true;
			}
		}
		public override Color? GetAlpha(Color newColor)
		{
			return new Color(255, 255, 255, 255) * projectile.Opacity;
		}
	}
}