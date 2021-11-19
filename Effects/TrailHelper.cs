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
using System.Linq;

namespace SGAmod.Effects
{
    //No steal, thanks please, and thank Boffin for the shader! (The rest of the code is mine)
    public class TrailHelper
    {
        Effect Effect => SGAmod.TrailEffect;
        public Vector2 projsize;
        public float trailThickness = 1f;
        public float trailThicknessIncrease = 1f;
        public string pass;
        public float strength;
        public float yFade = 1f; 
        public bool doFade = true;
        public bool connectEnds = false;
        public Vector2 coordOffset;
        public Vector2 coordMultiplier;
        public Vector2 rainbowCoordOffset;
        public Vector2 rainbowCoordMultiplier;
        public Vector3 rainbowColor;
        public bool perspective = false;
        public float strengthPow = 0f;
        public Vector2 capsize;
        public float ZDistScaling = 0.01f;
        public float rainbowScale = 1f;
        public Texture2D rainbowTexture;
        public Func<float, Color> color;
        public Func<float, Color> colorPerSegment;
        public Func<float,float> trailThicknessFunction;

        public Texture tex;
        public TrailHelper(string pass,Texture2D tex2,Color color2 = default)
        {
            tex = tex2;
            projsize = Vector2.Zero;
            this.pass = pass;
            coordOffset = Vector2.Zero;
            coordMultiplier = Vector2.One;
            rainbowCoordOffset = Vector2.Zero;
            rainbowCoordMultiplier = Vector2.One;
            rainbowColor = Vector3.One;
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
            DrawTrail(drawPoses.Select(testby => testby.ToVector3()).ToList(), defaultloc);
        }

            public void DrawTrail(List<Vector3> drawPoses, Vector2 defaultloc = default)
        {

            if (!SGAConfigClient.Instance.PrimTrails)
                goto theend;

            VertexBuffer vertexBuffer;

            Effect.Parameters["WorldViewProjection"].SetValue((perspective ? WVP.perspectiveView(Main.GameViewMatrix.Zoom) : WVP.View(Main.GameViewMatrix.Zoom)) * (perspective ? WVP.PerspectiveProjection() : WVP.Projection()));
            Effect.Parameters["imageTexture"].SetValue(tex);
            Effect.Parameters["coordOffset"].SetValue(coordOffset);
            Effect.Parameters["coordMultiplier"].SetValue(coordMultiplier);
            Effect.Parameters["strength"].SetValue(strength);
            Effect.Parameters["yFade"].SetValue(yFade);
            Effect.Parameters["strengthPow"].SetValue(strengthPow);

            Effect.Parameters["rainbowCoordOffset"].SetValue(rainbowCoordOffset);
            Effect.Parameters["rainbowCoordMultiplier"].SetValue(rainbowCoordMultiplier);
            Effect.Parameters["rainbowColor"].SetValue(rainbowColor);
            Effect.Parameters["rainbowScale"].SetValue(rainbowScale);
            Effect.Parameters["rainbowTexture"].SetValue(rainbowTexture);

            int totalcount = drawPoses.Count;
            int caps = (int)capsize.X;

            Vector3[] prevcoords = { Vector3.One, Vector3.One };

            int k = 1;
            Vector3[] firstloc = { Vector3.Zero, Vector3.Zero };
            int fractiontotal = totalcount + (connectEnds ? 1 : 0);

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[((fractiontotal + 1) * 6)+(caps * 3)];

            float totalcount2 = (float)(totalcount - 1 + (connectEnds ? 1 : 0));

            for (k = 1; k < totalcount; k += 1)
            {

                float fraction = (k-1) / totalcount2;
                float fractionPlus = k / totalcount2;
                float invertFrac = 1f - (k / totalcount2);

                Vector2 trailloc = new Vector2(drawPoses[k].X, drawPoses[k].Y) + projsize;
                Vector2 prev2 = new Vector2(drawPoses[k - 1].X, drawPoses[k - 1].Y) + projsize;
                if (prev2 == default)
                    prev2 = trailloc;

                //You want prims, you get prims!

                float sizeboost = perspective ? 1f : 1f+(ZDistScaling * drawPoses[k].Z);

                float thickness = 0;
                if (trailThicknessFunction == default)
                    thickness = Math.Max(0, (trailThickness + invertFrac * trailThicknessIncrease) * (sizeboost));
                else
                    thickness = trailThicknessFunction(invertFrac) * (sizeboost);

                Vector2 normal = Vector2.Normalize(trailloc - prev2);
                Vector3 left = (normal.RotatedBy(MathHelper.Pi / 2f) * (thickness)).ToVector3();
                Vector3 right = -left;// (normal.RotatedBy(-MathHelper.Pi / 2f) * (thickness)).ToVector3();

                Vector3 updown = -Vector3.UnitZ * ((perspective ? drawPoses[k].Z : 0));

                Vector3 drawtop = (trailloc - Main.screenPosition).ToVector3()+ updown;
                Vector3 drawbottom = (prev2 - Main.screenPosition).ToVector3()+ updown;

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

                Color valuecol1 = colorPerSegment != default ? colorPerSegment(k-1) : color(fraction);
                Color valuecol2 = colorPerSegment != default ? colorPerSegment(k) : color(fractionPlus);

                float fadeTo = doFade ? 0f : 1f;
                Color colortemp = Color.Lerp(valuecol1, valuecol1 * fadeTo, fraction);
                Color colortemp2 = Color.Lerp(valuecol2, valuecol2 * fadeTo, fractionPlus);

                vertices[0 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], colortemp, new Vector2(0, fraction));
                vertices[1 + (k * 6)] = new VertexPositionColorTexture(drawtopright, colortemp2, new Vector2(1, fractionPlus));
                vertices[2 + (k * 6)] = new VertexPositionColorTexture(drawtopleft, colortemp2, new Vector2(0, fractionPlus));

                vertices[3 + (k * 6)] = new VertexPositionColorTexture(prevcoords[0], colortemp, new Vector2(0, fraction));
                vertices[4 + (k * 6)] = new VertexPositionColorTexture(prevcoords[1], colortemp, new Vector2(1, fraction));
                vertices[5 + (k * 6)] = new VertexPositionColorTexture(drawtopright, colortemp2, new Vector2(1, fractionPlus));

                prevcoords = new Vector3[2] { drawtop + left, drawtop + right };

                if (connectEnds && repeat)
                {
                    repeat = false;
                    drawtopleft = firstloc[0];
                    drawtopright = firstloc[1];
                    prevcoords = new Vector3[2] { drawtop + left, drawtop + right };
                    k += 1;
                    fraction = (k - 1) / totalcount2;
                    fractionPlus = k / totalcount2;
                    goto repeater;
                }

                //Idglib.DrawTether(SGAmod.ExtraTextures[21], prev2, goto2, 1f, 0.25f, 1f, Color.Magenta);

            }

            int vertoffset = ((k + 1) * 6);

            if (caps > 0)
            {

                Vector2 loc = new Vector2(drawPoses[0].X, drawPoses[0].Y) + projsize;
                Vector2 normal = Vector2.Normalize(loc - (new Vector2(drawPoses[1].X, drawPoses[1].Y) + projsize));

                for (int capnum = 1; capnum < caps; capnum += 1)
                {
                    float percent = ((capnum - 1f) / (caps - 1f));
                    float percentNext = ((capnum) / (caps - 1f));

                    float angle = percent * MathHelper.Pi;
                    float angleNext = percentNext * MathHelper.Pi;

                    float thickness = trailThickness+trailThicknessIncrease;
                    if (trailThicknessFunction != default)
                        thickness = trailThicknessFunction(1f);

                        float rotAngle = (-MathHelper.Pi / 2f) + (angle);
                    float rotAngleNext = (-MathHelper.Pi / 2f) + (angleNext); //Idglib.DrawTether(SGAmod.ExtraTextures[21], loc, loc+ (normal.RotatedBy(rotAngle) * thickness), 1f, 0.25f, 1f, Color.White);

                    float tricknessAdd = 0f;// (float)Math.Max(-2f + Math.Sin(percent * (MathHelper.Pi)) * 2.5f, 0) * capsize.Y;
                    float tricknessAddNext = 0f;//(float)Math.Max(-2f + Math.Sin(percentNext * (MathHelper.Pi)) * 2.5f,0) * capsize.Y;

                    Vector3 left = (normal.RotatedBy(rotAngle) * (thickness + tricknessAdd)).ToVector3();
                    Vector3 leftNextStep = (normal.RotatedBy(rotAngleNext) * (thickness + tricknessAddNext)).ToVector3();
                    Vector3 locv3 = (loc-Main.screenPosition).ToVector3();

                    Color color2 = colorPerSegment != default ? colorPerSegment(0) : color(0f);
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



            Effect.CurrentTechnique.Passes[pass].Apply();
            Main.graphics.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, ((totalcount + 1) * 2)+(caps));

            theend:

            Effect.Parameters["coordOffset"].SetValue(Vector2.Zero);
            Effect.Parameters["coordMultiplier"].SetValue(Vector2.One);
            Effect.Parameters["strength"].SetValue(1f);
            Effect.Parameters["yFade"].SetValue(1f);
            Effect.Parameters["strengthPow"].SetValue(0f);

        }
    }



}