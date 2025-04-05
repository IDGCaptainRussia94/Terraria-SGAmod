using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using AAAAUThrowing;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SGAmod.Items.Weapons.ThrowingKnives
{
	public class ThrowingKnifeBaseItem : ModItem
	{
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(ThrowingKnifeBaseItem);
		}
		public override string Texture => "Terraria/Item_" + ItemID.ThrowingKnife;
		public override void SetDefaults()
		{
			item.shootSpeed = 10f;
			item.damage = 12;
			item.crit = 0;
			item.knockBack = 2f;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useAnimation = 15;
			item.useTime = 15;
			item.width = 10;
			item.height = 24;
			item.maxStack = 999;
			item.rare = ItemRarityID.White;

			item.consumable = true;
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = false;
			item.Throwing().thrown = true;

			item.UseSound = SoundID.Item1;
			item.value = Item.sellPrice(copper: 3);
			item.shoot = ProjectileID.ThrowingKnife;
		}
	}
	public class ThrowingKnifeBaseProjectle : ModProjectile
	{
		public override bool Autoload(ref string name)
		{
			return GetType() != typeof(ThrowingKnifeBaseProjectle);
		}
		public override string Texture => "Terraria/Projectile_" + ProjectileID.ThrowingKnife;
		public override void SetDefaults()
		{
			projectile.width = 12;               //The width of projectile hitbox
			projectile.height = 12;              //The height of projectile hitbox
			projectile.aiStyle = 2;             //The ai style of the projectile, please reference the source code of Terraria
			projectile.friendly = true;         //Can the projectile deal damage to enemies?
			projectile.hostile = false;         //Can the projectile deal damage to the player?
			projectile.Throwing().thrown = true;
			projectile.penetrate = 2;           //How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
			projectile.ignoreWater = false;          //Does the projectile's speed be influenced by water?
			projectile.tileCollide = true;          //Can the projectile collide with tiles?
			aiType = ProjectileID.ThrowingKnife;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			//Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
			for (int k = 0; k < projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
				Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
				spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
			}
			return true;
		}

		public override void Kill(int timeLeft)
		{
			// This code and the similar code above in OnTileCollide spawn dust from the tiles collided with. SoundID.Dig is the bounce sound you hear.
			Collision.HitTiles(projectile.position + projectile.velocity, projectile.velocity, projectile.width, projectile.height);
			Main.PlaySound(SoundID.Dig, projectile.position);
		}
	}
}