/*
 * Author: Connor Pandolph
 * Game: Necky's Revenge
 * Framework: Microsoft XNA
 * Date: 2013
 */
 
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace LineDraw
{
    public static class LineDrawer
    {
  
        static Texture2D blank;

        private static void InitLineDrawer(GraphicsDevice GraphicsDevice)
        {
            blank = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            blank.SetData(new[] { Color.White });
        }

        public static void DrawLine(SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            if (blank == null)
            {
                InitLineDrawer(spriteBatch.GraphicsDevice);
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            Vector2 offset = new Vector2(0.0f,0.5f);

            spriteBatch.Draw(blank, point1, null, color,
                       angle, offset, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        public static void DrawLine2(SpriteBatch spriteBatch, float width, Color color, Vector2 point1, Vector2 point2)
        {
            if (blank == null)
            {
                InitLineDrawer(spriteBatch.GraphicsDevice);
            }

            float angle = (float)Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);
            float length = Vector2.Distance(point1, point2);

            spriteBatch.Draw(blank, point1, null, color,
                       angle, Vector2.Zero, new Vector2(length, width),
                       SpriteEffects.None, 0);
        }

        public static void DrawRectangle(SpriteBatch spriteBatch, float width, Color color, Rectangle Rec)
        {
            DrawLine(spriteBatch, width, color,     new Vector2( (Rec.X), (Rec.Y) ) ,                               new Vector2( (Rec.X + Rec.Width), (Rec.Y) ) );
            DrawLine(spriteBatch, width, color,     new Vector2( (Rec.X + Rec.Width), Rec.Y ),                      new Vector2( (Rec.X + Rec.Width), (Rec.Y + Rec.Height) ) );
            DrawLine(spriteBatch, width, color,     new Vector2( (Rec.X + Rec.Width), (Rec.Y + Rec.Height) ) ,      new Vector2( (Rec.X ), (Rec.Y + Rec.Height) ) );
            DrawLine(spriteBatch, width, color,     new Vector2( (Rec.X ), (Rec.Y + Rec.Height) ) ,                 new Vector2( (Rec.X ), (Rec.Y) ) );
        }

        public static void DrawSolidRectangle(SpriteBatch spriteBatch, Color color, Rectangle Rec)
        {
            float width = Rec.Width; 
            DrawLine(spriteBatch, width, color, new Vector2(Rec.Center.X, Rec.Bottom), new Vector2(Rec.Center.X, Rec.Top));
           
        }

        public static void DrawCircle(SpriteBatch spriteBatch, float width, Color color, Vector2 Position, float Radius, int Sides)
        {
            Vector2 Starting = new Vector2(Radius, 0);
            Vector2 Next;
            Vector2 Previous = Starting;
            float degrees = 360 / Sides;

            if (Sides < 3)
            {
                Sides = 36;
            }

            for (int i = 1; i < Sides;  i++)
            {
                /// This is drawing the circle Counter Clock Wise) 
                Next = AngleToV2((i * degrees), Radius);
                LineDrawer.DrawLine2(spriteBatch, width, color, Position + Previous, Position + Next);
                Previous = Next;

            }

            LineDrawer.DrawLine2(spriteBatch, width, color, Position + Previous, Position + Starting);

        }

        public static Vector2 AngleToV2(float angle, float length)
        {
            Vector2 direction = Vector2.Zero;
            direction.X = (float)Math.Cos(MathHelper.ToRadians(angle)) * length;
            direction.Y = (float)Math.Sin(MathHelper.ToRadians(angle)) * length;
            return direction;
        }

    }
  
}
