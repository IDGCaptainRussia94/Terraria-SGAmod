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


        public static Effect ShadowEffect;
        public static List<ShadowParticle> particles;
        public Texture2D CloudTexture => SGAmod.Instance.GetTexture("NPCs/Hellion/Clouds" + (1 + cloudIndex));

        public static void Load()
        {
            ShadowEffect = SGAmod.Instance.GetEffect("Effects/Shadow");
            particles = new List<ShadowParticle>();
            shadowSurface = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
            shadowSurfaceShaderApplied = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
            shadowHellion = new RenderTarget2D(Main.graphics.GraphicsDevice, Main.screenWidth / 2, Main.screenHeight / 2, false, Main.graphics.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24, 1, RenderTargetUsage.DiscardContents);
            
            SGAmod.PostUpdateEverythingEvent += UpdateAll;
        }

        public static void Unload()
        {
            if (!ShadowEffect.IsDisposed)
                ShadowEffect.Dispose();
            if (!shadowSurface.IsDisposed)
                shadowSurface.Dispose();
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
            particles.Insert(0,particle);
        }

        public static void UpdateAll()
        {

            particles = new List<ShadowParticle>(particles).Where(testby => testby.active).ToList();

            shadowParticlesDrawTime -= 1;

            foreach (ShadowParticle particle in particles)
            {
                shadowParticlesDrawTime = 10;
                particle.Update();
            }

            if (shadowParticlesDrawTime>0)
            DrawToRenderTarget();
        }

        public static void DrawToRenderTarget()
        {
            if (Main.dedServ || shadowParticlesDrawTime<1)
                return;

            RenderTargetBinding[] binds = Main.graphics.GraphicsDevice.GetRenderTargets();

            Main.graphics.GraphicsDevice.SetRenderTarget(shadowSurface);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            //Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Texture, BlendState.Additive, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

            Color fogTo = Color.DarkMagenta;
            Color fogFrom = Color.Black;
            float edging = 0.10f;
            int smokecount = 5;

            foreach (ShadowParticle particle in particles)
            {
                Vector2 size = particle.CloudTexture.Size();
                float alpha = (float)Math.Sin((particle.timeLeft / (float)particle.maxTimeLeft) * MathHelper.Pi);
                Main.spriteBatch.Draw(particle.CloudTexture, (particle.position - Main.screenPosition) / 2f, null, Color.White * MathHelper.Clamp(alpha*particle.alpha * particle.fadePercent, 0f, 1f), particle.rotation, size / 2f, particle.scale / 2f, SpriteEffects.None, 0f);
            }

            Main.spriteBatch.End();

            Main.graphics.GraphicsDevice.SetRenderTarget(shadowSurfaceShaderApplied);
            Main.graphics.GraphicsDevice.Clear(Color.Transparent);

            //Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity);

            ShadowEffect.Parameters["overlayTexture"].SetValue(shadowSurface);
            ShadowEffect.Parameters["colorAmmount"].SetValue(smokecount);
            ShadowEffect.Parameters["screenSize"].SetValue(shadowSurface.Size());
            //Color.DarkMagenta
            //Color.Black
            ShadowEffect.Parameters["colorFrom"].SetValue((fogTo).ToVector4());
            ShadowEffect.Parameters["colorTo"].SetValue((fogFrom).ToVector4()*1f);
            ShadowEffect.Parameters["colorOutline"].SetValue((Color.Transparent).ToVector4()*1f);

            ShadowEffect.Parameters["edgeSmooth"].SetValue(edging);
            ShadowEffect.Parameters["noiseTexture"].SetValue(shadowHellion);

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
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);// Main.GameViewMatrix.TransformationMatrix);

                Vector2 parallex = new Vector2(0, 0);

                Effect effect = SGAmod.TrailEffect;

                effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Vector2.One) * WVP.Projection());
                effect.Parameters["imageTexture"].SetValue(SGAmod.Instance.GetTexture("TiledPerlin"));
                effect.Parameters["coordOffset"].SetValue(parallex);
                effect.Parameters["coordMultiplier"].SetValue(new Vector2(1f,1f));
                effect.Parameters["strength"].SetValue(1f);

                VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6];
                VertexBuffer vertexBuffer;

                Vector3 screenPos = new Vector3(-16, 0, 0);
                float skymove = ((Math.Max(Main.screenPosition.Y - 8000, 0)) / (Main.maxTilesY * 16f));

                Color colorsa = Color.Lerp(fogFrom,fogTo,0.50f);

                vertices[0] = new VertexPositionColorTexture(screenPos + new Vector3(-16, 0, 0), colorsa, new Vector2(0, 0));
                vertices[1] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), colorsa, new Vector2(0, 1));
                vertices[2] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), colorsa, new Vector2(1, 0));

                vertices[3] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, Main.screenHeight, 0), colorsa, new Vector2(1, 1));
                vertices[4] = new VertexPositionColorTexture(screenPos + new Vector3(-16, Main.screenHeight, 0), colorsa, new Vector2(0, 1));
                vertices[5] = new VertexPositionColorTexture(screenPos + new Vector3(Main.screenWidth + 16, 0, 0), colorsa, new Vector2(1, 0));

                vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
                vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

                Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

                RasterizerState rasterizerState = new RasterizerState();
                rasterizerState.CullMode = CullMode.None;
                Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;

                effect.CurrentTechnique.Passes["BasicEffectPass"].Apply();
                Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

                /*foreach(NPC hellionarm in Main.npc.Where(testby => testby.active && (testby.type == ModContent.NPCType<HellionCore>() || testby.type == ModContent.NPCType<HellionMonolog>())))
                {
                    Hellion.HellionTeleport(Main.spriteBatch, (Main.screenPosition+hellionarm.Center)/2f, 0.5f, 48);
                    //((HellionWorm)(hellionarm.modNPC as HellionWorm)).DrawMe(Main.spriteBatch,Color.White,0.5f);
                }*/

                //Main.spriteBatch.Draw(hellionTex, -reallyHellion.velocity + new Vector2(0, 10) + (reallyHellion.Center - Main.screenPosition) / 2f, null, Color.White * (settings != null ? settings.HelliontransparencyRate : 0.15f), 0, hellionTex.Size() / 2f, 0.50f, SpriteEffects.None, 0f);
                Main.spriteBatch.End();
                
            }


            //Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.GameViewMatrix.TransformationMatrix);

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
