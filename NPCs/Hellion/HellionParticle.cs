using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SGAmod.Effects;
using Terraria;
using Terraria.ModLoader;

namespace SGAmod.NPCs.Hellion
{
    public class ShadowParticle
    {
        public static RenderTarget2D shadowSurface;
        public static RenderTarget2D shadowSurfaceShaderApplied;
        public static RenderTarget2D shadowHellion;
        public static int shadowParticlesDrawTime = 0;
        protected static float starAlpha = 1f;
        public static float StarAlpha
        {
            get
            {
                return starAlpha;
            }
            set
            {
                starAlpharesetTime = 30;
                starAlpha = value;
            }

        }
        public static int starAlpharesetTime = 0;


        public static Effect ShadowEffect;
        public static List<ShadowParticle> particles;
        public Texture2D CloudTexture => SGAmod.Instance.GetTexture("NPCs/Hellion/Clouds" + (1 + cloudIndex));

        public static void Load()
        {
            ShadowEffect = SGAmod.Instance.GetEffect("Effects/Shadow");
            particles = new List<ShadowParticle>();
            SGAmod.RenderTargetsEvent += SGAmod_RenderTargetsEvent;
            SGAmod.RenderTargetsCheckEvent += SGAmod_RenderTargetsCheckEvent;
        }

        private static void SGAmod_RenderTargetsCheckEvent(ref bool yay)
        {
            yay &= !((shadowHellion == null || shadowHellion.IsDisposed) || (shadowSurfaceShaderApplied == null || shadowSurfaceShaderApplied.IsDisposed) || (shadowSurface == null || shadowSurface.IsDisposed));
        }

        private static void SGAmod_RenderTargetsEvent()
        {
                shadowSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
                shadowSurfaceShaderApplied = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
                shadowHellion = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
        }

        public static void Unload()
        {
            if (!ShadowEffect.IsDisposed)
                ShadowEffect.Dispose();
            if (!shadowSurface.IsDisposed)
                shadowSurface.Dispose();
            if (!shadowHellion.IsDisposed)
                shadowHellion.Dispose();
        }

        public ShadowParticle(Vector2 position,Vector2 velocity, Vector2 scale, int timeLeft, Vector2 scaleToAdd = default, Vector2 friction = default, float rotation = 0, float fadePercent = 1f, float rotationAdd = 0f)
        {
            this.position = position;
            this.velocity = velocity;
            this.scale = scale;
            this.timeLeft = timeLeft;
            this.rotation = rotation;
            this.rotationAdd = rotationAdd;
            this.scaleToAdd = scaleToAdd;
            this.fadePercent = fadePercent;
            this.friction = friction;
            this.maxTimeLeft = timeLeft;

            if (scaleToAdd == default)
            {
                this.scaleToAdd = new Vector2(1f, 1f) * 0.01f;
            }
            if (rotation == 0)
            {
                this.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
            }
            if (rotationAdd == 0)
            {
                this.rotationAdd = Main.rand.NextFloat(-MathHelper.Pi, MathHelper.Pi) * 0.01f;
            }
            cloudIndex = Main.rand.Next(6);
            if (fadePercent != 1f)
            {
                this.fadePercent = Main.rand.NextFloat(1f,2f);
            }
            if (friction == default)
            {
                this.friction = Vector2.One;
            }
            alphaBoost = Main.rand.NextFloat(0.05f, 0.40f);
            alpha = 1f;
        }

        public bool active = true;
        public int timeLeft = 120;
        public int maxTimeLeft = 120;
        public float fadePercent = 1f;
        public float rotation = 0f;
        public float rotationAdd = 0f;
        public int cloudIndex = 0;
        public float alpha = 0f;
        public float alphaBoost = 0f;
        public Player ownedPlayer = default;
        public float followPlayerPercent = 0f;


        public Vector2 position;
        public Vector2 friction;
        public Vector2 velocity;
        public Vector2 scale;
        public Vector2 scaleToAdd;

        public void Update()
        {
            timeLeft -= 1;
            if (timeLeft < 1 || !active)
            {
                active = false;
                return;
            }
            if (ownedPlayer != default)
            {
                position += ownedPlayer.velocity * followPlayerPercent;
            }
            position += velocity;
            velocity *= friction;
            rotation += rotationAdd;
            //alpha += alphaBoost;
        }

        public static void AddParticle(ShadowParticle particle)
        {
            if (!SGAConfigClient.Instance.HellionFog)
                return;
            particles.Insert(0,particle);
        }

        public static void UpdateAll()
        {

            particles = new List<ShadowParticle>(particles).Where(testby => testby.active).ToList();

            shadowParticlesDrawTime -= 1;
            starAlpharesetTime -= 1;
            if (starAlpharesetTime < 1 && starAlpha != 1f)
                starAlpha += (1f- starAlpha) / 60f;

            foreach (ShadowParticle particle in particles)
            {
                shadowParticlesDrawTime = 10;
                particle.Update();
            }

            DrawToRenderTarget();
        }

        public static void DrawToRenderTarget()
        {
            if (Main.dedServ || shadowParticlesDrawTime<1)
                return;

            RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();


            //Draw additive white particles

            Main.graphics.GraphicsDevice.SetRenderTarget(shadowSurface);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            //Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            //Gains a different look when the Hellion Core spawns it
            int fogtype = NPC.CountNPCS(ModContent.NPCType<HellionCore>())>0 ? 0 : 1;

            Color fogTo = Color.DarkMagenta;
            Color fogFrom = Color.Black;
            float edging = 0.10f;
            int smokecount = fogtype == 0 ? 16 : 32;


            foreach (ShadowParticle particle in particles)
            {
                Vector2 size = particle.CloudTexture.Size();
                float alpha = (float)Math.Sin((particle.timeLeft / (float)particle.maxTimeLeft) * MathHelper.Pi);
                Main.spriteBatch.Draw(particle.CloudTexture, (particle.position - Main.screenPosition) / 2f, null, Color.White * MathHelper.Clamp(alpha*particle.alpha * particle.fadePercent, 0f, 1f), particle.rotation, size / 2f, particle.scale / 2f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();

            //Now, draw the interior effect using the above as texture mask

            Main.graphics.GraphicsDevice.SetRenderTarget(shadowSurfaceShaderApplied);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            //Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            ShadowEffect.Parameters["overlayTexture"].SetValue(shadowSurface);
            ShadowEffect.Parameters["colorAmmount"].SetValue(smokecount);
            ShadowEffect.Parameters["screenSize"].SetValue(shadowSurface.Size());
            //Color.DarkMagenta
            //Color.Black
            ShadowEffect.Parameters["colorFrom"].SetValue((fogTo).ToVector4()*(fogtype == 0 ? 1f : 0.01f));
            ShadowEffect.Parameters["colorTo"].SetValue((fogFrom).ToVector4()*1f);
            ShadowEffect.Parameters["colorOutline"].SetValue((Color.Transparent).ToVector4()*1f);

            ShadowEffect.Parameters["edgeSmooth"].SetValue(edging);
            ShadowEffect.Parameters["noiseTexture"].SetValue(shadowHellion);
            ShadowEffect.Parameters["invertLuma"].SetValue(false);
            ShadowEffect.Parameters["alpha"].SetValue(1f);


            //Show through
            float percent = 1f;

            ShadowEffect.Parameters["noisePercent"].SetValue(percent);
            ShadowEffect.Parameters["noiseScalar"].SetValue(new Vector4(0,0,1f,1f));

            ShadowEffect.CurrentTechnique.Passes["ColorFilter"].Apply();

            Main.graphics.GraphicsDevice.SamplerStates[1] = SamplerState.PointClamp;
            Main.spriteBatch.Draw(shadowSurface, Vector2.Zero, new Rectangle(0, 0, Main.screenWidth/2, Main.screenHeight/2), Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0f);

            Main.spriteBatch.End();

            if (percent > 0)
            {
                Main.graphics.GraphicsDevice.SetRenderTarget(shadowHellion);
                Main.graphics.GraphicsDevice.Clear(Color.Transparent);

                //Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);

                Vector2 parallex = new Vector2(0, 0);

                Effect effect = SGAmod.TrailEffect;

                Terraria.Utilities.UnifiedRandom rando = new Terraria.Utilities.UnifiedRandom(1);

                float timer = rando.NextFloat(MathHelper.TwoPi);

                for (int i = 0; i < 12; i += 1)
                {

                    //Used to pulse in and out, didn't think it was needed in the end thou
                    float alpha = 1f*starAlpha;
                    #region oldcode
                    // MathHelper.Clamp(0.40f + (float)Math.Sin(((SGAWorld.modtimer / 35f) + rando.NextFloat(MathHelper.TwoPi)) * rando.NextFloat(0.25f, 0.75f))*0.80f, 0f, 1f);
                    //if (alpha <= 0 && i > 0)
                    //    continue;
                    #endregion

                    Matrix rotation = Matrix.CreateRotationZ(timer);
                    Matrix rotation2 = Matrix.CreateRotationZ((-timer));
                    Matrix rotation3 = Matrix.CreateRotationZ((timer - MathHelper.Pi));

                    Matrix rotationOffset = Matrix.CreateTranslation(-shadowSurface.Width / 2, -shadowSurface.Height / 2, 0)*Matrix.CreateScale(3,3f,1f)*rotation*Matrix.CreateTranslation(shadowSurface.Width / 2, shadowSurface.Height / 2, 0);

                    if (i > 0)
                    {
                        Vector2 pos2 = ((Main.screenPosition / ((8000f + (i * 4000f))) * 1f) + new Vector2(rando.NextFloat(1f), rando.NextFloat(1f)));
                        Vector2 pos = -Vector2.One+new Vector2(pos2.X%1f, pos2.Y%1f) *2f;
                        Matrix rotationOffset2 = Matrix.CreateTranslation(pos.X, pos.Y, 0)* rotation2;
                        parallex = Vector2.Transform(Vector2.One/2f, rotationOffset2);

                    }
                    if (fogtype == 1)
                    {
                        effect.Parameters["rainbowCoordOffset"].SetValue(new Vector2(rando.NextFloat(1f), rando.NextFloat(1f)));
                        effect.Parameters["rainbowCoordMultiplier"].SetValue(new Vector2(1f, 1f));
                        effect.Parameters["rainbowColor"].SetValue(new Vector3(SGAWorld.modtimer / 300f, 1f, 0.75f));
                        effect.Parameters["rainbowScale"].SetValue(1f);
                        effect.Parameters["rainbowTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));
                    }

                    effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Vector2.One) * WVP.Projection());
                    effect.Parameters["imageTexture"].SetValue(i<1 ? SGAmod.Instance.GetTexture("TiledPerlin") : SGAmod.Instance.GetTexture("Space"));
                    effect.Parameters["coordOffset"].SetValue(parallex);
                    effect.Parameters["coordMultiplier"].SetValue(i < 1 ? new Vector2(2f, 1.5f)*3f : (new Vector2(3f, 2f)+ new Vector2(rando.NextFloat(-0.5f,0.5f), rando.NextFloat(-0.5f, 0.5f)))*3f);
                    effect.Parameters["strength"].SetValue(i == 0 ? 0.50f : (1.75f* alpha)/(1f+(i/4f)));

                    VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];
                    VertexBuffer vertexBuffer;

                    Vector3 screenPos = new Vector3(-16, 0, 0);

                    Color colorsa = fogtype == 0 ? Color.Lerp(fogFrom, fogTo, 0.50f) : Color.White;

                    if (i > 0)
                    {
                        colorsa = fogtype == 0 ? Color.Lerp(fogTo, fogFrom, (float)i / 16f) : Color.White * (1f-((float)i / 48f));
                    }

                    vertices[0] = new VertexPositionColorTexture(Vector3.Transform(screenPos + new Vector3(-16, -16, 0), rotationOffset), colorsa, new Vector2(0, 0));
                    vertices[1] = new VertexPositionColorTexture(Vector3.Transform(screenPos + new Vector3(-16, Main.screenHeight+16, 0), rotationOffset), colorsa, new Vector2(0, 1));
                    vertices[2] = new VertexPositionColorTexture(Vector3.Transform(screenPos + new Vector3(Main.screenWidth + 16, -16, 0), rotationOffset), colorsa, new Vector2(1, 0));

                    vertices[3] = new VertexPositionColorTexture(Vector3.Transform(screenPos + new Vector3(Main.screenWidth + 16, Main.screenHeight+16, 0), rotationOffset), colorsa, new Vector2(1, 1));
                    vertices[4] = new VertexPositionColorTexture(Vector3.Transform(screenPos + new Vector3(-16, Main.screenHeight+16, 0), rotationOffset), colorsa, new Vector2(0, 1));
                    vertices[5] = new VertexPositionColorTexture(Vector3.Transform(screenPos + new Vector3(Main.screenWidth + 16, -16, 0), rotationOffset), colorsa, new Vector2(1, 0));

                    vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                    vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

                    Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                    RasterizerState rasterizerState = new RasterizerState();
                    rasterizerState.CullMode = CullMode.None;
                    Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                    effect.CurrentTechnique.Passes[i < 1 ? (fogtype == 1 ? "RainbowEffectPass" : "BasicEffectPass") : (fogtype == 1 ? "RainbowEffectAlphaPass" : "BasicEffectAlphaPass")].Apply();
                    Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                }

                
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                //originally, the worms were drawn INSIDE the rendertarget, but this has since been changed
                #region oldcode
                /*foreach(NPC hellionarm in Main.npc.Where(testby => testby.active && (testby.type == ModContent.NPCType<HellionCore>() || testby.type == ModContent.NPCType<HellionMonolog>())))
                {
                    Hellion.HellionTeleport(Main.spriteBatch, (Main.screenPosition+hellionarm.Center)/2f, 0.5f, 48);
                    //((HellionWorm)(hellionarm.modNPC as HellionWorm)).DrawMe(Main.spriteBatch,Color.White,0.5f);
                }*/

                //Main.spriteBatch.Draw(hellionTex, -reallyHellion.velocity + new Vector2(0, 10) + (reallyHellion.Center - Main.screenPosition) / 2f, null, Color.White * (settings != null ? settings.HelliontransparencyRate : 0.15f), 0, hellionTex.Size() / 2f, 0.50f, SpriteEffects.None, 0f);
                #endregion

                Main.spriteBatch.End();
                
            }

            Main.graphics.GraphicsDevice.SetRenderTargets(binds);

        }

        public static void Draw()
        {
            if (ShadowParticle.shadowParticlesDrawTime < 1)
                return;

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

                Main.spriteBatch.Draw(shadowSurfaceShaderApplied, Vector2.Zero, new Rectangle(0,0,Main.screenWidth,Main.screenHeight), Color.White, 0, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
