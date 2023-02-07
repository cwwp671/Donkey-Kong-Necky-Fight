using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Engine
{
    class SpriteSheet : BaseSprite
    {
        public int msPerFrame;
        public int currentIndex;
        private int timeSinceLastFrame;
        public bool animationPlayed;

        public SpriteSheet(ContentManager Content, string ImageName, int MSPS)
            : base(Content, ImageName)
        {
            msPerFrame = MSPS;
            animationPlayed = false;
        }

        public void Update(GameTime gameTime, int loopFrame, List<Rectangle> AnimationSet)
        {
            if (animationPlayed == true && loopFrame == 0)
            {
                
            }
            else
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

                if (timeSinceLastFrame > msPerFrame)
                {
                    
                    timeSinceLastFrame -= msPerFrame;
                    currentIndex++;
                    if (currentIndex >= AnimationSet.Count)
                    {
                        animationPlayed = true;
                        currentIndex = loopFrame;
                    }
                }
            }

              sourceRectangle = new Rectangle?(AnimationSet[currentIndex]);  
        }

        public void UpdateIdle(GameTime gameTime, Rectangle idleRect)
        {
            animationPlayed = false;
            sourceRectangle = new Rectangle?(idleRect);
        }

    }//end Class
}//end Namespace