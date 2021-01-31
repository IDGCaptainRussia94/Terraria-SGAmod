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

namespace SGAmod.Effects
{
    //No steal, thanks please, and thank Boffin for the shader! (The rest of the code is mine)
    public class TrailHelper
    {
        Effect effect => SGAmod.TrailEffect;
        public Vector2 projsize;
        public float trailThickness = 1f;
        public float trailThicknessIncrease = 1f;
        public string pass;
        public float strength;
        public bool doFade = true;
        public bool connectEnds = false;
        public Vector2 coordOffset;
        public Vector2 coordMultiplier;
        public Vector2 capsize;
        public Func<float, Color> color;

        public Texture tex;
        public TrailHelper(string pass,Texture2D tex2,Color color2 = default)
        {
            tex = tex2;
            projsize = Vector2.Zero;
            this.pass = pass;
            coordOffset = Vector2.Zero;
            coordMultiplier = Vector2.One;
            strength = 1f;
            capsize = new Vector2(0,64f);
            if (color2 == default)
            {
                color = delegate (float percent)
                {
                    return Color.White;
                };
            }

        }

        public void DrawTrail(List<Vector2> drawPoses, Vector2 defaultloc = default)
        {

            VertexBuffer vertexBuffer;

            /*basicEffect.World = WVP.World();
            basicEffect.View = WVP.View(Main.GameViewMatrix.Zoom);
            basicEffect.Projection = WVP.Projection();
            basicEffect.VertexColorEnabled = true;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = SGAmod.ExtraTextures[21];*/
            effect.Parameters["WorldViewProjection"].SetValue(WVP.View(Main.GameViewMatrix.Zoom) * WVP.Projection());
            effect.Parameters["imageTexture"].SetValue(tex);
            effect.Parameters["coordOffset"].SetValue(coordOffset);
            effect.Parameters["coordMultiplier"].SetValue(coordMultiplier);
            effect.Parameters["strength"].SetValue(strength);

            int totalcount = drawPoses.Count;
            int caps = (int)capsize.X;

            Vector3[] prevcoords = { Vector3.One, Vector3.One };

            int k = 1;
            Vector3[] firstloc = { Vector3.Zero, Vector3.Zero };
            int fractiontotal = totalcount + (connectEnds ? 1 : 0);

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[((fractiontotal + 1) * 6)+(caps * 3)];

            for (k = 1; k < totalcount; k += 1)
            {
                float fraction = (float)k / (float)fractiontotal;
                float fractionPlus = (float)(k + 1) / (float)fractiontotal;

                Vector2 trailloc = drawPoses[k] + projsize;
                Vector2 prev2 = drawPoses[k - 1] + projsize;
                if (prev2 == default)
                    prev2 = trailloc;

                //You want prims, you get prims!

                float thickness = trailThickness + (1f - (k / (float)drawPoses.Count)) * trailThicknessIncrease;

                Vector2 normal = Vector2.Normalize(trailloc - prev2);
                Vector3 left = (normal.RotatedBy(MathHelper.Pi / 2f) * (thickness)).ToVector3();
                Vector3 right = (normal.RotatedBy(-MathHelper.Pi / 2f) * (thickness)).ToVector3();

                Vector3 drawtop = (trailloc - Main.screenPosition).ToVector3();
                Vector3 drawbottom = (prev2 - Main.screenPosition).ToVector3();

                if (k == 1)
                {
                    //firstloc[0] = drawbottom+left;
                    //firstloc[1] = drawbottom + right;
                }

                if (prevcoords[0] == Vector3.One)
                {
                    prevcoords = new Vector3[2] { drawbottom + left, drawbottom + right };
                    firstloc[0] = drawbottom + left;
                    firstloc[1] = drawbottom + right;
                }

                bool repeat = (k == totalcount - 1);

                Vector3 drawtopleft = drawtop + left;
                Vector3 drawtopright = drawtop + right;

            repeater:

                Color valuecol1 = color(fraction);
                Color valuecol2 = color(fractionPlus);

                float fadeTo = doFade ? 0f : 1f;
                Color colortemp = Color.Lerp(valuecol1, valuecol1 * fadeTo, fraction);
                Color colortemp2 = Color.Lerp(valuecol2, valuecol2 * fadeTo, fractionPlus);

                vertices[0 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], colortemp, new Vector2(0, fractionPlus));
                vertices[1 + (k * 6)] = new VertexPositionColorTexture(drawtopright, colortemp2, new Vector2(1, fraction));
                vertices[2 + (k * 6)] = new VertexPositionColorTexture(drawtopleft, colortemp2, new Vector2(0, fraction));

                vertices[3 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], colortemp, new Vector2(0, fractionPlus));
                vertices[4 + (k * 6)] = new VertexPositionColorTexture(prevcoords[1], colortemp, new Vector2(1, fractionPlus));
                vertices[5 + (k * 6)] = new VertexPositionColorTexture(drawtopright, colortemp2, new Vector2(1, fraction));

                prevcoords = new Vector3[2] { drawtop + left, drawtop + right };

                if (connectEnds && repeat)
                {
                    repeat = false;
                    drawtopleft = firstloc[0];
                    drawtopright = firstloc[1];
                    prevcoords = new Vector3[2] { drawtop + left, drawtop + right };
                    k += 1;
                    goto repeater;
                }

                //Idglib.DrawTether(SGAmod.ExtraTextures[21], prev2, goto2, 1f, 0.25f, 1f, Color.Magenta);

            }

            int vertoffset = ((k + 1) * 6);

            if (caps > 0)
            {
                for (int capnum = 1; capnum < caps; capnum += 1)
                {
                    float percent = ((capnum - 1f) / (caps - 1f));
                    float percentNext = ((capnum) / (caps - 1f));

                    float angle = percent * MathHelper.Pi;
                    float angleNext = percentNext * MathHelper.Pi;

                    //(1f - (k / (float)drawPoses.Count))
                    float thickness = trailThickness+trailThicknessIncrease;
                    //thickness *= 2; 

                    Vector2 loc = drawPoses[0] + projsize;
                    Vector2 normal = Vector2.Normalize(loc - (drawPoses[1] + projsize));

                    float rotAngle = (-MathHelper.Pi / 2f) + (angle);
                    float rotAngleNext = (-MathHelper.Pi / 2f) + (angleNext);

                    //Idglib.DrawTether(SGAmod.ExtraTextures[21], loc, loc+ (normal.RotatedBy(rotAngle) * thickness), 1f, 0.25f, 1f, Color.White);

                    float tricknessAdd = 0f;// (float)Math.Max(-2f + Math.Sin(percent * (MathHelper.Pi)) * 2.5f, 0) * capsize.Y;
                    float tricknessAddNext = 0f;//(float)Math.Max(-2f + Math.Sin(percentNext * (MathHelper.Pi)) * 2.5f,0) * capsize.Y;

                    Vector3 left = (normal.RotatedBy(rotAngle) * (thickness + tricknessAdd)).ToVector3();
                    Vector3 leftNextStep = (normal.RotatedBy(rotAngleNext) * (thickness + tricknessAddNext)).ToVector3();
                    Vector3 locv3 = (loc-Main.screenPosition).ToVector3();

                    Color color2 = color(0f);
                    Vector2 texCoord = new Vector2((float)Math.Sin(percent * MathHelper.Pi)/2f, 0f);
                    Vector2 texCoordNext = new Vector2((float)Math.Sin(percentNext*MathHelper.Pi)/2f,0f);

                    vertices[((capnum - 1) * 3) + 2 + vertoffset] = new VertexPositionColorTexture(locv3, color2, new Vector2(0.50f, 1f));
                    vertices[((capnum - 1) * 3) + 0 + vertoffset] = new VertexPositionColorTexture(locv3 + left, color2, texCoord);
                    vertices[((capnum - 1) * 3) + 1 + vertoffset] = new VertexPositionColorTexture(locv3 + leftNextStep, color2, texCoordNext);

                }
            }

            vertexBuffer = new VertexBuffer(Main.graphics.GraphicsDevice, typeof(VertexPositionColorTexture), vertices.Length, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

            Main.graphics.GraphicsDevice.SetVertexBuffer(vertexBuffer);

            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            Main.graphics.GraphicsDevice.RasterizerState = rasterizerState;



            effect.CurrentTechnique.Passes[pass].Apply();
            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, ((totalcount + 1) * 2)+(caps));

        }
    }



}