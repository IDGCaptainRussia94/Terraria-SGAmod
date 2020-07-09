using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Weapons
{
    public class NadeFlail : ModItem
    {
        public override void SetDefaults()
        {

            item.width = 30;
            item.height = 10;
            item.value = Item.sellPrice(0, 3, 0, 0);
            item.rare = 6;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 10;
            item.useTime = 24;
            item.knockBack = 5f;
            item.damage = 50;
            item.scale = 2f;
            item.noUseGraphic = true;
            item.shoot = mod.ProjectileType("NadeFlailBall");
            item.shootSpeed = 25.1f;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.channel = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SpikeballFlail"), 1);
            recipe.AddIngredient(ItemID.ProximityMineLauncher, 1);
            recipe.AddIngredient(ItemID.StickyGrenade, 100);
            recipe.AddIngredient(ItemID.HallowedBar, 10);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flail-O-Nades");
            Tooltip.SetDefault("Unleashes long lasting Sticky Grenades pretty much whenever it hits a wall\nFaster speeds release more grenades\nHigh velocity impacts spawn a Proxy Mine\n'This is a REALLY great idea!'");
        }

    }

    public class NadeFlailBall : ModProjectile
    {
        float[] angles = new float[20];
        float[] dist = new float[20];
        bool doinit = false;
        public override void SetDefaults()
        {
            projectile.width = 40;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.penetrate = -1;
            projectile.melee = true;
            projectile.aiStyle = 15;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {

            if (oldVelocity.Length() > 3)
            {
                for (int num315 = 1; num315 < 0.5f+(oldVelocity.Length()/6); num315 = num315 + 1)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    float velincrease = ((float)(num315 + 8) / 2f);
                    int thisone = Projectile.NewProjectile(projectile.Center.X - projectile.velocity.X, projectile.Center.Y - projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ProjectileID.StickyGrenade, (int)(projectile.damage * 2.50), 0f, projectile.owner, 0.0f, 0f);
                    Main.projectile[thisone].melee = true;
                    Main.projectile[thisone].thrown = false;
                    Main.projectile[thisone].ranged = false;
                    Main.projectile[thisone].timeLeft = 60 * 20;
                    Main.projectile[thisone].netUpdate = true;
                    IdgProjectile.Sync(thisone);
                }

            }
            if (oldVelocity.Length() >  16)
            {
                for (int num315 = 15; num315 < 16; num315 = num315 + 1)
                {
                    Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                    float velincrease = ((float)(num315 + 8) / 3f);
                    int thisone = Projectile.NewProjectile(projectile.Center.X - projectile.velocity.X, projectile.Center.Y - projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ProjectileID.ProximityMineIII, (int)(projectile.damage * 5.00), 0f, projectile.owner, 0.0f, 0f);
                    Main.projectile[thisone].melee = true;
                    Main.projectile[thisone].thrown = false;
                    Main.projectile[thisone].ranged = false;
                    Main.projectile[thisone].netUpdate = true;
                    IdgProjectile.Sync(thisone);
                }

            }
            return true;
        }

        public override string Texture
        {
            get { return ("Terraria/Projectile_" + 14); }
        }


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Nade Ball");
        }

        public override void AI()
        {
            if (doinit == false)
            {
                doinit = true;
                for (int num315 = 0; num315 < angles.Length; num315 = num315 + 1)
                {
                    angles[num315] = Main.rand.NextFloat((float)-Math.PI, (float)Math.PI);
                    dist[num315] = Main.rand.NextFloat(0f, 24f);
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {


                Texture2D texture = Main.chain2Texture;

            Vector2 position = projectile.Center;
            Vector2 mountedCenter = Main.player[projectile.owner].MountedCenter;
            Microsoft.Xna.Framework.Rectangle? sourceRectangle = new Microsoft.Xna.Framework.Rectangle?();
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);
            float num1 = (float)texture.Height;
            Vector2 vector2_4 = mountedCenter - position;
            float rotation = (float)Math.Atan2((double)vector2_4.Y, (double)vector2_4.X) - 1.57f;
            bool flag = true;
            if (float.IsNaN(position.X) && float.IsNaN(position.Y))
                flag = false;
            if (float.IsNaN(vector2_4.X) && float.IsNaN(vector2_4.Y))
                flag = false;
            while (flag)
            {
                if ((double)vector2_4.Length() < (double)num1 + 1.0)
                {
                    flag = false;
                }
                else
                {
                    Vector2 vector2_1 = vector2_4;
                    vector2_1.Normalize();
                    position += vector2_1 * num1;
                    vector2_4 = mountedCenter - position;
                    Microsoft.Xna.Framework.Color color2 = Lighting.GetColor((int)position.X / 16, (int)((double)position.Y / 16.0));
                    color2 = projectile.GetAlpha(color2);
                    Main.spriteBatch.Draw(texture, position - Main.screenPosition, sourceRectangle, color2, rotation, origin, 1.35f, SpriteEffects.None, 0.0f);
                }
            }

            Matrix dothematrx = Matrix.CreateRotationZ(projectile.rotation - MathHelper.ToRadians(90)) *
                Matrix.CreateScale(1f, 1f, 1f) *
            Matrix.CreateTranslation((new Vector3(projectile.Center.X, projectile.Center.Y, 0) - new Vector3(Main.screenPosition.X, Main.screenPosition.Y, 0)))
            * Main.GameViewMatrix.ZoomMatrix;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, dothematrx);
            for (int num315 = 0; num315 < angles.Length; num315 = num315 + 1)
            {
                Vector2 there = new Vector2((float)Math.Cos(angles[num315]), (float)Math.Sin(angles[num315])) * dist[num315];
                Texture2D tex2 = Main.itemTexture[ItemID.StickyGrenade];
                Main.spriteBatch.Draw(tex2, there, sourceRectangle, lightColor, 0, new Vector2(tex2.Width, tex2.Height)/2, 1f, SpriteEffects.None, 0.0f);
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.ZoomMatrix);


            return false;
        }
    }

}


