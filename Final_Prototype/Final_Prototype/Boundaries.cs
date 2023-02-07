using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Engine;

namespace Engine
{
    class Boundaries
    {
        public List<Rectangle> wallList;
        public Rectangle bottomBoundary;
        
        public Boundaries(Vector2 CurrentResolution, Vector2 SpriteScale)
        {
            Rectangle leftBoundary = new Rectangle((int)(-70 * SpriteScale.X), (int)(-300 * SpriteScale.Y), (int)(20 * SpriteScale.X), (int)(CurrentResolution.Y + (300 * SpriteScale.Y))); ;
            Rectangle rightBoundary = new Rectangle((int)(CurrentResolution.X + (50 * SpriteScale.X)), (int)(-300 * SpriteScale.Y), (int)(20 * SpriteScale.X), (int)(CurrentResolution.Y + (300 * SpriteScale.Y)));;
            bottomBoundary = new Rectangle(0, (int)CurrentResolution.Y, (int)CurrentResolution.X, (int)(100 * SpriteScale.Y));
            wallList = new List<Rectangle>();
            wallList.Add(leftBoundary);
            wallList.Add(rightBoundary);
        }

        public void CheckWallCollision(Player player)
        {
            foreach (Rectangle wall in wallList)
            {
                if (player.isCollidingWith(wall))
                {
                    player.WallCollide();
                }
            }
        }

        public void Debug_Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(Rectangle wall in wallList)
            {
                LineDraw.LineDrawer.DrawSolidRectangle(spriteBatch, Color.Red, wall);
            }

            LineDraw.LineDrawer.DrawSolidRectangle(spriteBatch, Color.Blue, bottomBoundary);
        }
    }
}
