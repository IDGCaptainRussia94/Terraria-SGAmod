using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using Idglibrary;

namespace SGAmod.Items.Weapons.Trap
{
    public class NadeFlail : SpikeballFlail
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Flail-O-Nades");
            Tooltip.SetDefault("Applies Sticky Grenades with a delayed blast to enemies it hits\nUnleashes long lasting Proxy Mines pretty much whenever it hits a wall\nFaster speeds release more\nHigh velocity impacts spawn a tier 3 Proxy Mine\nCounts as trap damage, doesn't crit\n'This is a REALLY great idea!'");
        }
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 10;
            item.damage = 50;
            item.rare = 6;
            item.noMelee = true;
            item.useStyle = 5;
            item.useAnimation = 10;
            item.useTime = 24;
            item.knockBack = 5f;
            item.scale = 2f;
            item.noUseGraphic = true;
            item.shoot = ModContent.ProjectileType<NadeFlailBall>();
            item.shootSpeed = 25.1f;
            item.UseSound = SoundID.Item1;
            item.melee = true;
            item.channel = true;
            item.value = Item.sellPrice(0, 3, 0, 0);
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("SpikeballFlail"), 1);
            recipe.AddIngredient(ItemID.ProximityMineLauncher, 1);
            recipe.AddIngredient(ItemID.StickyGrenade, 100);
            recipe.AddIngredient(ItemID.LandMine, 10);
            recipe.AddIngredient(ItemID.HallowedBar, 8);
            recipe.AddIngredient(ItemID.IllegalGunParts, 1);
            recipe.AddTile(mod.GetTile("ReverseEngineeringStation"));
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
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
            projectile.trap = true;
            projectile.aiStyle = 15;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 25;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            
            if (oldVelocity.Length() > 3)
            {
                for (int num315 = 1; num315 < 0.5f + (oldVelocity.Length() / 6); num315 = num315 + 1)
                {
                    if (Main.player[projectile.owner].ownedProjectileCounts[ProjectileID.ProximityMineI] < 30)
                    {
                        Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                        float velincrease = ((float)(num315 + 8) / 2f);
                        int thisone = Projectile.NewProjectile(projectile.Center.X - projectile.velocity.X, projectile.Center.Y - projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ProjectileID.ProximityMineI, (int)(projectile.damage * 2.50), 0f, projectile.owner, 0.0f, 0f);
                        Main.projectile[thisone].melee = true;
                        Main.projectile[thisone].thrown = false;
                        Main.projectile[thisone].trap = true;
                        Main.projectile[thisone].ranged = false;
                        Main.projectile[thisone].localNPCHitCooldown = -1;
                        Main.projectile[thisone].usesLocalNPCImmunity = true;
                        Main.projectile[thisone].timeLeft = 60 * 20;
                        Main.projectile[thisone].netUpdate = true;
                        IdgProjectile.Sync(thisone);
                    }
                }

            }
            
            if (oldVelocity.Length() >  16)
            {
                for (int num315 = 15; num315 < 16; num315 = num315 + 1)
                {
                    if (Main.player[projectile.owner].ownedProjectileCounts[ProjectileID.ProximityMineIII] < 3)
                    {
                        Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                        float velincrease = ((float)(num315 + 8) / 3f)*2f;
                        int thisone = Projectile.NewProjectile(projectile.Center.X - projectile.velocity.X, projectile.Center.Y - projectile.velocity.Y, randomcircle.X * velincrease, randomcircle.Y * velincrease, ProjectileID.ProximityMineIII, (int)(projectile.damage * 5.00), 0f, projectile.owner, 0.0f, 0f);
                        Main.projectile[thisone].melee = true;
                        Main.projectile[thisone].thrown = false;
                        Main.projectile[thisone].trap = true;
                        Main.projectile[thisone].ranged = false;
                        Main.projectile[thisone].localNPCHitCooldown = -1;
                        Main.projectile[thisone].usesLocalNPCImmunity = true;
                        Main.projectile[thisone].netUpdate = true;
                        IdgProjectile.Sync(thisone);
                    }
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

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            int projer = Projectile.NewProjectile(projectile.Center, Main.rand.NextVector2Circular(3f, 3f), ModContent.ProjectileType<NadeFlailStickyNade>(), projectile.damage, projectile.knockBack+5f,projectile.owner);

            if (projer >= 0)
            {
                Projectile proj = Main.projectile[projer];

                proj.melee = true;
                proj.thrown = false;
                proj.trap = true;
                proj.ranged = false;
                proj.netUpdate = true;

                NadeFlailStickyNade flaier = proj.modProjectile as NadeFlailStickyNade;
                flaier.StickTo(target);
            }

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

	public class NadeFlailStickyNade : HavocGear.Projectiles.HotRound
    {

		public Player P;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nade Flail Sticky Nide");
		}

        public override string Texture => "Terraria/Projectile_"+ProjectileID.StickyGrenade;

        public override void SetDefaults()
		{
			//projectile.CloneDefaults(ProjectileID.CursedFlameHostile);
			projectile.width = 16;
			projectile.height = 16;
			projectile.ignoreWater = true;          //Does the projectile's speed be influenced by water?
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.tileCollide = true;
			projectile.penetrate = 200;
			projectile.magic = true;
			projectile.timeLeft = 3 * 60;
			projectile.scale = 0.75f;
			aiType = 0;
		}

        public override bool CanDamage()
        {
            return false;
        }

        public override bool PreKill(int timeLeft)
		{
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 10);

            if (timeLeft < 2 && stickin >= 0)
            {
                var snd = Main.PlaySound(SoundID.Item14, projectile.Center);
                if (snd != null)
                {
                    snd.Pitch = 0.75f;
                    if (SGAmod.ScreenShake < 16)
                        SGAmod.AddScreenShake(10, 1200, projectile.Center);
                }

            }

				int proj = Projectile.NewProjectile(projectile.Center, Vector2.Normalize(projectile.velocity) * 2f, ProjectileID.StickyGrenade, projectile.damage*2, projectile.knockBack * 3f, projectile.owner);
            if (proj >= 0)
            {
                Main.projectile[proj].melee = true;
                Main.projectile[proj].thrown = false;
                Main.projectile[proj].ranged = false;
                Main.projectile[proj].trap = true;
                Main.projectile[proj].timeLeft = 1;
                Main.projectile[proj].netUpdate = true;
            }

            return true;
		}

        public void StickTo(NPC npc)
        {
            projectile.penetrate = 50;
            offset = (npc.Center - projectile.Center);
            stickin = npc.whoAmI;
            projectile.netUpdate = true;
        }

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			//nope
		}
	}
}


