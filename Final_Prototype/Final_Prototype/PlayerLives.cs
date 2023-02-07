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
    class PlayerLives
    {
        public List<Rectangle> loopList;
        public SpriteSheet playerLife;
        public Vector2 HidePos;
        public Vector2 ShowPos;
        public SpriteFont TextStyle;
        public int MSPF;
        public bool canShow;
        public float hideTimer;
        public float hideTime;
        public int lifeCounter;
        public bool canShowString;

        public PlayerLives(ContentManager Content, Vector2 SpriteScale, int playerID)
        {
            MSPF = 100;

            if (playerID == 1)
            {
                SetPlayerOne(Content, SpriteScale);
            }
            else if (playerID == 2)
            {
                SetPlayerTwo(Content, SpriteScale);
            }

            
            playerLife.scale.X = SpriteScale.X;
            playerLife.scale.Y = SpriteScale.Y;
            canShow = false;
            canShowString = false;
            hideTime = 5000f;
            hideTimer = 0f;
            lifeCounter = 3;
            TextStyle = Content.Load<SpriteFont>(@"Fonts\LiveText");
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 SpriteScale)
        {
            Vector2 StringPos = new Vector2(ShowPos.X + (40 * SpriteScale.X), playerLife.position.Y);
            playerLife.Draw(gameTime, spriteBatch);
            spriteBatch.DrawString(TextStyle, lifeCounter.ToString(), StringPos, Color.White, 0f, Vector2.Zero, SpriteScale.X, SpriteEffects.None, 0);  
        }

        public void SetPlayerOne(ContentManager Content, Vector2 SpriteScale)
        {
            loopList = new List<Rectangle>();
            loopList.Add(new Rectangle(0, 0, 23, 58));
            loopList.Add(new Rectangle(0, 0, 23, 58));
            loopList.Add(new Rectangle(46, 0, 23, 58));
            loopList.Add(new Rectangle(69, 0, 23, 58));
            loopList.Add(new Rectangle(92, 0, 23, 58));
            loopList.Add(new Rectangle(115, 0, 23, 58));
            loopList.Add(new Rectangle(138, 0, 23, 58));
            playerLife = new SpriteSheet(Content, @"Images\P1Life", MSPF);
            playerLife.sourceRectangle = loopList[0];
            playerLife.scale.X = SpriteScale.X;
            playerLife.scale.Y = SpriteScale.Y;
            playerLife.position.X = 650 * SpriteScale.X;
            playerLife.position.Y = -60 * SpriteScale.Y;
            HidePos = new Vector2(640 * SpriteScale.X, -60 * SpriteScale.Y);
            ShowPos = new Vector2(640 * SpriteScale.X, 5 * SpriteScale.Y);
            Console.WriteLine(playerLife.msPerFrame);
        }

        public void SetPlayerTwo(ContentManager Content, Vector2 SpriteScale)
        {
            loopList = new List<Rectangle>();
            loopList.Add(new Rectangle(0, 0, 32, 52));
            loopList.Add(new Rectangle(0, 0, 32, 52));
            loopList.Add(new Rectangle(64, 0, 32, 52));
            loopList.Add(new Rectangle(96, 0, 32, 52));
            loopList.Add(new Rectangle(128, 0, 32, 52));
            loopList.Add(new Rectangle(160, 0, 32, 52));
            loopList.Add(new Rectangle(192, 0, 32, 52));
            playerLife = new SpriteSheet(Content, @"Images\P2Life", MSPF);
            playerLife.sourceRectangle = loopList[0];
            playerLife.position.X = 700 * SpriteScale.X;
            playerLife.position.Y = -60 * SpriteScale.Y;
            HidePos = new Vector2(690 * SpriteScale.X, -60 * SpriteScale.Y);
            ShowPos = new Vector2(690 * SpriteScale.X, 5 * SpriteScale.Y);
            Console.WriteLine(playerLife.msPerFrame);
        }

        public void LoseLife()
        {
            lifeCounter--;
            canShow = true;
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch, Vector2 SpriteScale)
        {
            playerLife.Update(gameTime, 1, loopList);

            if (canShow)
            {
                hideTimer += gameTime.ElapsedGameTime.Milliseconds;
                moveToScreen(spriteBatch, SpriteScale);

                if (hideTimer >= hideTime)
                {
                    canShow = false;
                    canShowString = false;
                    hideTimer = 0;
                }
            }
            else
            {
                moveOffScreen(spriteBatch, SpriteScale);
            }
        }

        public void moveToScreen(SpriteBatch spriteBatch, Vector2 SpriteScale)
        {
            if(playerLife.position.Y < ShowPos.Y)
            {
                playerLife.position.Y += 3f * SpriteScale.Y;
            }
        }

        public void moveOffScreen(SpriteBatch spriteBatch, Vector2 SpriteScale)
        {
            if(playerLife.position.Y > HidePos.Y)
            {
                playerLife.position.Y -= 3f * SpriteScale.Y;
            }
        }

    }
}
