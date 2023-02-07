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
    class Coconut : BaseSprite
    {
        public bool canSet;
        public bool canFire;
        public bool canBounce;
        public bool bouncing;
        public bool rightSide;
        public bool hitPlayer;
        public bool neckyCanDespawn;
        public bool canHitBox;
        public int endX;
        public int MSPF;
        public float shootTime;
        public float shootTimer;
        public float coconutCounter;
        public float h;
        public float a;
        public float t;
        public float vx;
        public Vector2 Scaler;
        public Vector2 DKPos;
        public Vector2 NeckyStartPos;
        public Vector2 DefaultRes;
        public Vector2 CurrentRes;
        public Vector2 BouncePt_Coconut;

        public Coconut(ContentManager Content, String ImageName, Vector2 SpriteScale)
            : base(Content, ImageName)
        {
            Scaler = SpriteScale;
            bouncing = false;
            canSet = false;
            canFire = false;
            canBounce = false;
            neckyCanDespawn = false;
            hitPlayer = false;
            Vector2 OriginPt_Coconut = new Vector2(texture.Width / 2f, texture.Height / 2f); //Creates Origin Point for Coconut
            origin = OriginPt_Coconut;                                                               //Assigns Origin Point to Coconut
            scale.X = Scaler.X * 2.0f;                                                          //Scale X by Scale Factor and Extra Scale Increase
            scale.Y = Scaler.Y * 2.0f;                                                          //Scale Y by Scale Factor and Extra Scale Increase
            canFire = false;
            position.X = -1000;
            position.Y = -1000;
            sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);

            Console.WriteLine(" -Coconut x-origin: " + OriginPt_Coconut.X);
            Console.WriteLine(" -Coconut y-origin: " + OriginPt_Coconut.Y);
            Console.WriteLine(" -Coconut Scale X: " + scale.X);
            Console.WriteLine(" -Coconut Scale Y: " + scale.Y);
        }

        public void Set_Coconut(Vector2 DKPosition, Vector2 NeckyInitialPosition, Vector2 DefaultResolution, Vector2 CurrentResolution, int msPerFrame, bool spawnRight)
        {
            Console.WriteLine("inside Set_Coconut Function");

            MSPF = msPerFrame;
            DKPos = DKPosition;
            NeckyStartPos = NeckyInitialPosition;
            DefaultRes = DefaultResolution;
            CurrentRes = CurrentResolution;
            rightSide = spawnRight;

            hitPlayer = false;
            canSet = false;
            canFire = true;
            endX = (int)DKPos.X;
            float ySpawnOffset = 10 * Scaler.Y;
            shootTime = MSPF * 9;

            if(rightSide)
            {
                endX -= (int)CurrentRes.X;
            }

            position.X = -1000;
            position.Y = -1000;
            h = NeckyStartPos.Y;
            h -= ySpawnOffset;
            a = (DefaultRes.Y / 2) * Scaler.Y;
            t = (float)Math.Sqrt((CurrentRes.Y - h) / (0.5f * a));
            float d = (float)endX;
            vx = d / t;
            shootTimer = 0;
            coconutCounter = 0;
            canBounce = false;

            UpdateCollision();
            Console.WriteLine(" -Coconut Height: " + h + " Pixels");
            Console.WriteLine(" -Coconut Acceleration: " + a + " Pixels Per Second²");
            Console.WriteLine(" -Coconut Fall Time: " + t + " Seconds");
            Console.WriteLine(" -Coconut Distance: " + d + " Pixels");
            Console.WriteLine(" -Coconut X-velocity: " + vx + " Pixels Per Second");
        }//end Set_Coconut Function

        public void Shoot_Coconut(GameTime gameTime, Necky NeckyL)
        {
            shootTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            
            if (shootTimer >= shootTime)
            {
                canHitBox = true;
                coconutCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;
                Move_Coconut();

                if (NeckyL.canShootSound)
                {
                    NeckyL.ShootSound();
                    NeckyL.canShootSound = false;
                }
                

                if (coconutCounter >= t)
                {  
                    neckyCanDespawn = true;
                    canBounce = true;
                    canFire = false;
                }
            }

        }//end Shoot_Coconut Function

        public void Move_Coconut()
        {
            float xSpawnOffset;

            if (!rightSide)
            {
                xSpawnOffset = NeckyStartPos.X + (135 * Scaler.X);
                position.X = xSpawnOffset + vx * coconutCounter;
                rotation = (0.00285f * Scaler.X) * position.X;
            }
            else
            {
                xSpawnOffset = NeckyStartPos.X - (135 * Scaler.X);
                position.X = xSpawnOffset + vx * coconutCounter;
                rotation = -((0.00285f * Scaler.X) * position.X);
            }

            position.Y = h + (0.5f * a * coconutCounter * coconutCounter);
            UpdateCollision();

        }//end Move_Coconut Function

        public void Bounce_Coconut()
        {

            float bounceEndX;

            if (!rightSide)
            {
                bounceEndX = position.X + vx;
            }
            else
            {
                bounceEndX = position.X - vx;
            }

            h = position.Y;
            a = (DefaultRes.Y / 2) * Scaler.Y;
            t = (float)Math.Sqrt((h) / (0.5f * a));
            float d = vx;
            vx = d / t;

            BouncePt_Coconut = new Vector2(position.X, position.Y);
            canBounce = false;
            bouncing = true;
            coconutCounter = 0;
        }

         public void Bouncing_Coconut(GameTime gameTime)
        {
            coconutCounter += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (coconutCounter <= (t * 2))
            {
                if (!rightSide)
                {
                    position.X = BouncePt_Coconut.X + (vx * coconutCounter);
                    position.Y = (h + ((-(h - NeckyStartPos.Y) * coconutCounter) + (0.5f * a * coconutCounter * coconutCounter)));
                    rotation = ((0.00285f * Scaler.X) * position.X);
                }
                else
                {
                    position.X = BouncePt_Coconut.X + (vx * coconutCounter);
                    position.Y = (h + ((-(h - NeckyStartPos.Y) * coconutCounter) + (0.5f * a * coconutCounter * coconutCounter)));
                    rotation = -((0.00285f * Scaler.X) * position.X);
                }
            }
            else
            {
                bouncing = false;
            }

            UpdateCollision();
        }

         //public void UpdateCollision()
         //{
         //    Collision = new Rectangle((int)(position.X - origin.X), (int)(position.Y - origin.Y), (int)(sourceRectangle.Value.Width * Scaler.X), (int)(sourceRectangle.Value.Height * Scaler.Y));
         //}

    }//end Class
}//end Namespace
