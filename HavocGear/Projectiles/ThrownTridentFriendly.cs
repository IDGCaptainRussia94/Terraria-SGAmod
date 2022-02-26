using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;


namespace SGAmod.HavocGear.Projectiles
{
    public class ThrownTridentFriendly : ThrownTrident
    {
        public override void SetStaticDefaults()
	    {
		    DisplayName.SetDefault("Trident");
	    }
		
		public override void SetDefaults()
        { 
            projectile.width = 12;      
            projectile.height = 12; 
            projectile.friendly = true;     
            projectile.melee = false;
            projectile.Throwing().thrown = true;
            projectile.tileCollide = true;   
            projectile.penetrate = 5;     
            projectile.timeLeft = 500;  
            projectile.light = 0.25f;   
            projectile.extraUpdates = 1;
   		    projectile.ignoreWater = true;   
        }

        public override string Texture
        {
            get { return("SGAmod/HavocGear/Projectiles/ThrownTrident"); }
        }

        public override void AI()           //this make that the projectile will face the corect way
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + 1.57f;  
		projectile.velocity=projectile.velocity+new Vector2(0,0.2f);
       }

        public override bool PreDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch, Color drawColor)
        {
            bool facingleft = projectile.velocity.X>0;
            Microsoft.Xna.Framework.Graphics.SpriteEffects effect = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
            Texture2D texture = Main.projectileTexture[projectile.type];
            Vector2 origin = new Vector2(texture.Width * 0.5f, texture.Height * 0.5f);
            Main.spriteBatch.Draw(texture, projectile.Center - Main.screenPosition, new Rectangle?(), drawColor, projectile.rotation + (facingleft ? (float)(1f * Math.PI) : 0f), origin, projectile.scale, facingleft ? effect : SpriteEffects.None, 0);
            return false;
        }
    }
}