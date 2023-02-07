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
using Engine;

namespace Engine
{
    class Tramp
    {
        public List<Rectangle> bounceListS;
        public List<Rectangle> bounceListB;
        public List<Rectangle> idleList;
        public Rectangle idleTramp;
        public SpriteSheet trampoline;
        public bool canBounceS = false;
        public bool canBounceB = false;
        public int MSPF;
        public bool isBouncingS;
        public bool isBouncingb;
        SoundEffect BounceSmall;
        SoundEffect BounceBig;
        public bool canPlaySound;

        public Tramp(ContentManager CM, Vector2 CurrentResolution, Vector2 SpriteScale)
        {
            bounceListB = new List<Rectangle>();
            bounceListS = new List<Rectangle>();
            idleList = new List<Rectangle>();

            bounceListB.Add(new Rectangle(0, 0, 38, 46));
            bounceListB.Add(new Rectangle(38, 0, 38, 46));
            bounceListB.Add(new Rectangle(76, 0, 38, 46));
            bounceListB.Add(new Rectangle(114, 0, 38, 46));
            bounceListB.Add(new Rectangle(152, 0, 38, 46));
            bounceListB.Add(new Rectangle(190, 0, 38, 46));
            bounceListB.Add(new Rectangle(228, 0, 38, 46));
            bounceListB.Add(new Rectangle(190, 0, 38, 46));
            bounceListB.Add(new Rectangle(152, 0, 38, 46));
            bounceListB.Add(new Rectangle(114, 0, 38, 46));
            bounceListB.Add(new Rectangle(76, 0, 38, 46));
            bounceListB.Add(new Rectangle(38, 0, 38, 46));

            bounceListS.Add(new Rectangle(0, 0, 38, 46));
            bounceListS.Add(new Rectangle(38, 0, 38, 46));
            bounceListS.Add(new Rectangle(76, 0, 38, 46));
            bounceListS.Add(new Rectangle(114, 0, 38, 46));
            bounceListS.Add(new Rectangle(76, 0, 38, 46));
            bounceListS.Add(new Rectangle(38, 0, 38, 46));

            idleList.Add(new Rectangle(0, 0, 38, 46));

            isBouncingS = false;
            isBouncingb = false;
            idleTramp = new Rectangle(0, 0, 38, 46);
            MSPF = 20;
            trampoline = new SpriteSheet(CM, @"Images\DK_trampoline_SS", MSPF);
            trampoline.sourceRectangle = new Rectangle(0, 0, 38, 46);
            trampoline.origin.X = trampoline.sourceRectangle.Value.Width / 2f;
            trampoline.origin.Y = trampoline.sourceRectangle.Value.Height;
            trampoline.position.X = CurrentResolution.X / 2.0f;
            trampoline.position.Y = CurrentResolution.Y;
            trampoline.scale.X = SpriteScale.X * 2.0f;
            trampoline.scale.Y = SpriteScale.Y * 2.0f;
            canPlaySound = true;
            trampoline.collision = new Rectangle((int)(390 * SpriteScale.X), (int)(545 * SpriteScale.Y), (int)(20 * SpriteScale.X), (int)(5 * SpriteScale.Y));
            BounceBig = CM.Load<SoundEffect>(@"Sound\TireBig");
            BounceSmall = CM.Load<SoundEffect>(@"Sound\TireSmall");
        }

        public bool animationFinished()
        {
            return trampoline.animationPlayed;
        }

        public void BigSound()
        {
            BounceBig.Play();
        }

        public void SmallSound()
        {
            BounceSmall.Play();
        }

        public void Update(GameTime gameTime)
        {
            trampoline.origin.X = trampoline.sourceRectangle.Value.Width / 2f;
            trampoline.origin.Y = trampoline.sourceRectangle.Value.Height;

            if (canBounceS)
            {
                canBounceB = false;

                if (canPlaySound)
                {
                    SmallSound();
                    canPlaySound = false;
                }

                trampoline.Update(gameTime, 0, bounceListS);
            }
            else if (canBounceB)
            {
                canBounceS = false;

                if (canPlaySound)
                {
                    BigSound();
                    canPlaySound = false;
                }

                trampoline.Update(gameTime, 0, bounceListB);
            }
            else
            {
                canPlaySound = true;
                trampoline.currentIndex = 0;
                trampoline.UpdateIdle(gameTime, idleList[0]);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch SB)
        {
            trampoline.Draw(gameTime, SB);
        }

        public Rectangle Collision()
        {
            return trampoline.collision;
        }
    }
}