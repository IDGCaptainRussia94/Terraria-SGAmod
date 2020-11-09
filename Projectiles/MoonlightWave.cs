using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.Enums;
using Idglibrary;
using SGAmod.Effects;

namespace SGAmod.Projectiles
{
    public class MoonlightWaveLv1 : ModProjectile
    {
        virtual protected int extraparticles => 0;
        public BasicEffect basicEffect = new BasicEffect(Main.graphics.GraphicsDevice);
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonlight Wave");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 20;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 32;
            projectile.height = 32;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = 8;
            projectile.alpha = 40;
            projectile.timeLeft = 500;
            projectile.light = 0.75f;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {

                VertexBuffer vertexBuffer;

                basicEffect.World = WVP.World();
                basicEffect.View = WVP.View(Main.GameViewMatrix.Zoom);
                basicEffect.Projection = WVP.Projection();
                basicEffect.VertexColorEnabled = true;
                basicEffect.TextureEnabled = true;
                basicEffect.Texture = SGAmod.ExtraTextures[21];


                int totalcount = projectile.oldPos.Length;

            for(int i=0;i< projectile.oldPos.Length; i += 1)//dumb hack to get the trails to not appear at 0,0
            {
                if (projectile.oldPos[i]==default)
                projectile.oldPos[i] = projectile.position;
            }

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[((totalcount + 1) * 6)];

                Vector3[] prevcoords = { Vector3.One, Vector3.One };


            float coordprogress = 0;
            for (int k = 1; k < totalcount; k += 1)
                {
                    float fraction = (float)k / (float)totalcount;
                    float fractionPlus = (float)(k + 1) / (float)totalcount;

                    Vector2 size = new Vector2(projectile.width, projectile.height) / 2f;
                    Vector2 trailloc = projectile.oldPos[k] + size;
                    Vector2 prev2 = projectile.oldPos[k - 1] + size;
                if (prev2 == default)
                    prev2 = trailloc;

                //You want prims, you get prims!

                float thickness = 13+(1f-(k/(float)projectile.oldPos.Length))*16;

                    Vector2 normal = Vector2.Normalize(trailloc - prev2);
                    Vector3 left = (normal.RotatedBy(MathHelper.Pi / 2f) * (thickness)).ToVector3();
                    Vector3 right = (normal.RotatedBy(-MathHelper.Pi / 2f) * (thickness)).ToVector3();

                    Vector3 drawtop = (trailloc - Main.screenPosition).ToVector3();
                    Vector3 drawbottom = (prev2 - Main.screenPosition).ToVector3();

                    if (prevcoords[0] == Vector3.One)
                    {
                        prevcoords = new Vector3[2] { drawbottom + left, drawbottom + right };
                    }

                    Color color = Color.Lerp(Color.White, Color.White * 0f, fraction);
                    Color color2 = Color.Lerp(Color.White, Color.White * 0f, fractionPlus);

                    vertices[0 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], color, new Vector2(0, fractionPlus));
                    vertices[1 + (k * 6)] = new VertexPositionColorTexture(drawtop + right, color2, new Vector2(1, fraction));
                    vertices[2 + (k * 6)] = new VertexPositionColorTexture(drawtop + left, color2, new Vector2(0, fraction));

                    vertices[3 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], color, new Vector2(0, fractionPlus));
                    vertices[4 + (k * 6)] = new VertexPositionColorTexture(prevcoords[1], color, new Vector2(1, fractionPlus));
                    vertices[5 + (k * 6)] = new VertexPositionColorTexture(drawtop + right, color2, new Vector2(1, fraction));

                    prevcoords = new Vector3[2] { drawtop + left, drawtop + right };

                    //Idglib.DrawTether(SGAmod.ExtraTextures[21], prev2, goto2, 1f, 0.25f, 1f, Color.Magenta);

                }

                vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

                Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, ((totalcount + 1) * 2));
                }

            Texture2D texture = Main.projectileTexture[mod.ProjectileType(this.GetType().Name)];
            Vector2 origin = new Vector2((float)texture.Width * 0.5f, (float)texture.Height * 0.5f);

            spriteBatch.Draw(texture, projectile.Center + new Vector2(1, 0) - Main.screenPosition, null, Color.White, projectile.rotation, origin, new Vector2(1f, 1f), projectile.spriteDirection > 0 ? SpriteEffects.None : SpriteEffects.FlipVertically, 0f);
            return false;
        }

        public override bool PreKill(int timeLeft)
        {
            Main.PlaySound(SoundID.Item94, projectile.Center);
            Main.PlaySound(SoundID.Item89, projectile.Center);
            for (float num475 = -6 - extraparticles; num475 < 6 + extraparticles; num475 += 0.3f)
            {
                float anglehalf = (float)(((double)projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc2 = projectile.velocity;
                startloc2.Normalize();
                Vector2 startloc = (projectile.Center + (startloc2 * 12f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
                Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

                Main.dust[dust].scale = 2f - Math.Abs(num475) / 4f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 3f);
                Main.dust[dust].velocity += (projectile.velocity * num475);
                Main.dust[dust].noGravity = true;
            }

            return true;
        }

        public override void AI()
        {
            projectile.rotation = (float)Math.Atan2((double)projectile.velocity.Y * projectile.spriteDirection, (double)projectile.velocity.X * projectile.spriteDirection) + 1.57f;

            for (float num475 = -2; num475 < 3; num475 += 2)
            {
                float anglehalf = (float)(((double)projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc2 = projectile.velocity;
                startloc2.Normalize();
                Vector2 startloc = (projectile.Center + (startloc2 * 8f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);

                float anglehalf2 = anglehalf + ((float)Math.PI / 2f);
                Main.dust[dust].position += anglehalf2.ToRotationVector2() * (float)((Main.rand.Next(-200, 200) / 10f));

                Main.dust[dust].scale = 1.2f - Math.Abs(num475) / 5f;
                Vector2 randomcircle = new Vector2(Main.rand.Next(-8000, 8000), Main.rand.Next(-8000, 8000)); randomcircle.Normalize();
                Main.dust[dust].velocity = (randomcircle / 3f);
                Main.dust[dust].velocity += projectile.velocity / 2f;
                Main.dust[dust].noGravity = true;
            }


            for (float num475 = -2f; num475 < 3f; num475 += 4f)
            {

                float anglehalf = (float)(((double)projectile.velocity.ToRotation()) + 2.0 * Math.PI);
                Vector2 startloc = (projectile.Center + (projectile.velocity * 1f));
                int dust = Dust.NewDust(new Vector2(startloc.X, startloc.Y), 0, 0, 185);
                Main.dust[dust].scale = 1f;
                Main.dust[dust].velocity = projectile.velocity;
                Main.dust[dust].noGravity = true;
                if (Math.Abs(num475) > 0)
                {
                    anglehalf += ((float)Math.PI / 2f);
                    Main.dust[dust].velocity += anglehalf.ToRotationVector2() * ((num475 * 4f) / 1.25f);
                }
            }

        }


        public override void OnHitPlayer(Player target, int damage, bool crit)
        {
            target.AddBuff(mod.BuffType("MoonLightCurse"), 60 * 8);
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (this.GetType() == typeof(MoonlightWaveLv2))
                target.AddBuff(mod.BuffType("MoonLightCurse"), 60 * 5);
        }

    }

    public class MoonlightWaveLv2 : MoonlightWaveLv1
    {
        virtual protected int extraparticles => 6;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonlight Wave");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 30;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 42;
            projectile.height = 42;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = 12;
            projectile.alpha = 40;
            projectile.timeLeft = 500;
            projectile.light = 1.15f;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 6;
        }

        public override void AI()
        {
            base.AI();
            if (projectile.ai[0] < 1)
            {
                projectile.ai[0] = 1;
                Main.PlaySound(SoundID.Item30, projectile.Center);
            }
        }

    }

    public class MoonlightWaveLv3 : MoonlightWaveLv1
    {
        virtual protected int extraparticles => 16;
        double keepspeed = 0.0;
        float homing = 0.07f;
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Moonlight Wave");
            ProjectileID.Sets.TrailCacheLength[projectile.type] = 40;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 52;
            projectile.height = 52;
            projectile.friendly = true;
            projectile.melee = true;
            projectile.tileCollide = false;
            projectile.penetrate = -1;
            projectile.alpha = 40;
            projectile.timeLeft = 500;
            projectile.light = 1.5f;
            projectile.extraUpdates = 1;
            projectile.ignoreWater = true;
            projectile.usesLocalNPCImmunity = true;
            projectile.localNPCHitCooldown = 30;
            Main.PlaySound(SoundID.Item119, projectile.Center);
        }

        public override bool PreKill(int timeLeft)
        {
            base.PreKill(timeLeft);
            Main.PlaySound(SoundID.Item124, projectile.Center);
            Main.PlaySound(SoundID.NPCDeath60, projectile.Center);
            return true;
        }

        public override void AI()
        {
            base.AI();
            if (projectile.ai[0] < 1)
            {
                projectile.ai[0] = 1;
                keepspeed = (projectile.velocity).Length();
                Main.PlaySound(SoundID.Item119, projectile.Center);
            }

            projectile.spriteDirection = (projectile.velocity.X > 0).ToDirectionInt();

            NPC target = Main.npc[Idglib.FindClosestTarget(0, projectile.Center, new Vector2(0f, 0f), false, true, true, projectile)];
            if (target != null)
            {
                if ((target.Center - projectile.Center).Length() < 1000f)
                {
                    projectile.velocity = projectile.velocity + (projectile.DirectionTo(target.Center) * ((float)keepspeed * homing));
                    projectile.velocity.Normalize();
                    projectile.velocity = projectile.velocity * (float)keepspeed;
                }
            }


        }

    }


}