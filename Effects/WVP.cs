using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Terraria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SGAmod.Effects
{
    class WVP
    {
        public static GraphicsDevice graphics => Main.graphics.GraphicsDevice;
        public static Matrix World()
        {
            return Matrix.CreateTranslation(0, 0, 0);
        }
        public static Matrix View(Vector2 zoom)
        {
            int width = graphics.Viewport.Width;
            int height = graphics.Viewport.Height;
            return Matrix.CreateLookAt(Vector3.Zero, Vector3.UnitZ, Vector3.Up) * Matrix.CreateTranslation(width / 2, height / -2, 0) * Matrix.CreateRotationZ(MathHelper.Pi) * Matrix.CreateScale(zoom.X, zoom.Y, 1f);
        }
        public static Matrix Projection()
        {
            int width = graphics.Viewport.Width;
            int height = graphics.Viewport.Height;
            return Matrix.CreateOrthographic(width, height, 0, 1000);
        }

    }
}
