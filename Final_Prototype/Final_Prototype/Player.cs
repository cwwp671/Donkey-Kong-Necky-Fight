/*
 * Author: Connor Pandolph
 * Game: Necky's Revenge
 * Framework: Microsoft XNA
 * Date: 2013
 */
 
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
    class Player
    {
        public Rectangle boundsRect;
        public Rectangle collision;
        public List<Rectangle> walkListL;
        public List<Rectangle> walkListR;
        public List<Rectangle> idleList;
        public List<Rectangle> sprintListL;
        public List<Rectangle> sprintListR;
        public List<Rectangle> jumpListL;
        public List<Rectangle> jumpListR;
        public List<Rectangle> winList;
        public SpriteSheet PlayerActor;
        public String walkImage;
        public Texture2D walkTexture;
        public Texture2D sprintTexture;
        public Texture2D jumpTexture;
        public Texture2D winTexture;
        public Vector2 oldPosition;
        public int msPerFrame;
        public float vy;
        public float vx;
        public float gravity;
        public bool controlOne;
        public bool isDead;
        public bool canWin;
        public bool canWalkL;
        public bool canWalkR;
        public bool canSprintL;
        public bool canSprintR;
        public bool canJump;
        public bool canJumpL;
        public bool canJumpR;
        public bool idleLeft = false;
        public bool idleRight = false;
        public bool isWalkingL = false;
        public bool isWalkingR = false;
        public bool isSprintingL = false;
        public bool isSprintingR = false;
        public bool isJumpingL = false;
        public bool isJumpingR = false;
        public bool isInAir = false;

        public Player(ContentManager Content, string assetName, Vector2 SpriteScale, Rectangle StartRect, int MSPS)
        {
            walkListL = new List<Rectangle>();
            walkListR = new List<Rectangle>();
            sprintListL = new List<Rectangle>();
            sprintListR = new List<Rectangle>();
            jumpListL = new List<Rectangle>();
            jumpListR = new List<Rectangle>();
            winList = new List<Rectangle>();
            
            isDead = false;
            idleList = new List<Rectangle>();
            idleList.Add(StartRect);    //00
            oldPosition = Vector2.Zero;
            msPerFrame = MSPS;
            vy = 0;
            vx = 0;
            gravity = SpriteScale.Y;
            PlayerActor = new SpriteSheet(Content, assetName, msPerFrame);
            PlayerActor.position = new Vector2(300 * SpriteScale.X, 400 * SpriteScale.Y);
            PlayerActor.sourceRectangle = StartRect;
            boundsRect = new Rectangle(0, 0, PlayerActor.sourceRectangle.Value.Width, PlayerActor.sourceRectangle.Value.Height);
            PlayerActor.origin = new Vector2(boundsRect.Width / 2f, boundsRect.Height / 2f);
        }

        public void UpdatePosition()
        {
            oldPosition.X = PlayerActor.position.X;
            oldPosition.Y = PlayerActor.position.Y;
        }

        public void WinCollide()
        {
            canWalkL = false;
            canWalkR = false;
            canSprintL = false;
            canSprintR = false;
            canJumpL = false;
            canJumpR = false;
            PlayerActor.position.X = oldPosition.X;
            PlayerActor.position.Y = oldPosition.Y;
            vy = 0;
        }

        public void PlayerControllerControls(GamePadState newControlState, GamePadState oldControlState, Vector2 SpriteScale)
        {
            if (newControlState.ThumbSticks.Left.X < 0)
            {
                ControllerLeft(newControlState, oldControlState, SpriteScale);
            }
            else
            {
                StopLeft();
            }

            if (newControlState.ThumbSticks.Left.X > 0)
            {
                ControllerRight(newControlState, oldControlState, SpriteScale);
            }
            else
            {
                StopRight();
            }

            if (newControlState.Buttons.A == ButtonState.Pressed)
            {
                Jump(SpriteScale);
            }
        }

        public void PlayerControls(KeyboardState newKeyState, KeyboardState oldKeyState, Vector2 SpriteScale)
        {
             
            if (newKeyState.IsKeyDown(Keys.A))
            {
                MoveLeft(newKeyState, oldKeyState, SpriteScale);
            }
            else if (newKeyState.IsKeyUp(Keys.A))
            {
                StopLeft();
            }

            if (newKeyState.IsKeyDown(Keys.D))
            {
                MoveRight(newKeyState, oldKeyState, SpriteScale);
            }
            else if (newKeyState.IsKeyUp(Keys.D))
            {
                StopRight();
            }

            if (newKeyState.IsKeyDown(Keys.Space))
            {
                Jump(SpriteScale);
            }
            
            
        }

        public void ControllerLeft(GamePadState newControlState, GamePadState oldControlState, Vector2 SpriteScale)
        {
            if (canJumpR)
            {
                canJumpR = false;
                canJumpL = true;
            }

            if (isWalkingL == false)
            {
                PlayerActor.currentIndex = 0;
                isWalkingL = true;
            }

            if (newControlState.Buttons.LeftStick == ButtonState.Released)
            {
                isSprintingL = false;
            }

            if (newControlState.Buttons.LeftStick == ButtonState.Pressed)
            {
                if (isSprintingL == false)
                {
                    PlayerActor.currentIndex = 0;
                    isSprintingL = true;
                }

                canWalkL = false;
                canSprintL = true;
            }

            if (newControlState.Buttons.A == ButtonState.Pressed)
            {
                if (isJumpingL == false)
                {
                    PlayerActor.currentIndex = 0;
                    isJumpingL = true;
                }

                canWalkL = false;
                canSprintL = false;
                canJumpL = true;
            }
            

            if (!canSprintL && !canJumpL)
            {
                canWalkR = false;
                canJumpL = false;
                canWalkL = true;

                if (PlayerActor.animationPlayed)
                {
                    PlayerActor.currentIndex = 0;
                    PlayerActor.animationPlayed = false;
                }
            }

            if (isSprintingL)
            {
                PlayerActor.position.X -= 7 * SpriteScale.X;
            }
            else
            {
                PlayerActor.position.X -= 5 * SpriteScale.X;
            }
        }

        public void MoveLeft(KeyboardState newKeyState, KeyboardState oldKeyState, Vector2 SpriteScale)
        {
            if (canJumpR)
            {
                canJumpR = false;
                canJumpL = true;
            }

            if (isWalkingL == false)
            {
                PlayerActor.currentIndex = 0;
                isWalkingL = true;
            }

            if (newKeyState.IsKeyUp(Keys.LeftShift))
            {
                isSprintingL = false;
            }

            if (newKeyState.IsKeyDown(Keys.LeftShift))
            {
                if (isSprintingL == false)
                {
                    PlayerActor.currentIndex = 0;
                    isSprintingL = true;
                }

                canWalkL = false;
                canSprintL = true;
            }

            if (newKeyState.IsKeyDown(Keys.Space))
            {
                if (isJumpingL == false || oldKeyState != newKeyState)
                {
                    PlayerActor.currentIndex = 0;
                    isJumpingL = true;
                }

                canWalkL = false;
                canSprintL = false;
                canJumpL = true;
            }          

            if (!canSprintL && !canJumpL)
            {
                canWalkR = false;
                canJumpL = false;
                canWalkL = true;

                if (PlayerActor.animationPlayed)
                {
                    PlayerActor.currentIndex = 0;
                    PlayerActor.animationPlayed = false;
                }
            }

            if (isSprintingL)
            {
                PlayerActor.position.X -= 7 * SpriteScale.X;
            }
            else
            {
                PlayerActor.position.X -= 5 * SpriteScale.X;
            }
        }//end MoveLeft

        public void StopLeft()
        {
            isWalkingL = false;
            canWalkL = false;
            canSprintL = false;
        }//end StopLeft

        public void ControllerRight(GamePadState newControlState, GamePadState oldControlState, Vector2 SpriteScale)
        {
            if (canJumpL)
            {
                canJumpL = false;
                canJumpR = true;
            }

            if (isWalkingR == false)
            {
                PlayerActor.currentIndex = 0;
                isWalkingR = true;
            }

            if (newControlState.Buttons.LeftStick == ButtonState.Released)
            {
                isSprintingR = false;
            }

            if (newControlState.Buttons.LeftStick == ButtonState.Pressed)
            {
                if (isSprintingR == false)
                {
                    PlayerActor.currentIndex = 0;
                    isSprintingR = true;
                }

                canWalkR = false;
                canSprintR = true;
            }

            if (newControlState.Buttons.A == ButtonState.Pressed)
            {
                if (isJumpingR == false)
                {
                    PlayerActor.currentIndex = 0;
                    canSprintR = false;
                    isJumpingR = true;
                }

                canWalkR = false;
                canJumpR = true;
            }

            if (!canSprintR && !canJumpR)
            {
                canWalkL = false;
                canJumpR = false;
                canWalkR = true;

                if (PlayerActor.animationPlayed)
                {
                    PlayerActor.currentIndex = 0;
                    PlayerActor.animationPlayed = false;
                }
            }

            if (isSprintingR)
            {
                PlayerActor.position.X += 7 * SpriteScale.X;
            }
            else
            {
                PlayerActor.position.X += 5 * SpriteScale.X;
            }
        }

        public void MoveRight(KeyboardState newKeyState, KeyboardState oldKeyState, Vector2 SpriteScale)
        {
            if (canJumpL)
            {
                canJumpL = false;
                canJumpR = true;
            }

            if (isWalkingR == false)
            {
                PlayerActor.currentIndex = 0;
                isWalkingR = true;
            }

            if (newKeyState.IsKeyUp(Keys.LeftShift))
            {
                isSprintingR = false;
            }

            if (newKeyState.IsKeyDown(Keys.LeftShift))
            {
                if (isSprintingR == false)
                {
                    PlayerActor.currentIndex = 0;
                    isSprintingR = true;
                }

                canWalkR = false;
                canSprintR = true;
            }

            if (newKeyState.IsKeyDown(Keys.Space))
            {
                if (isJumpingR == false || oldKeyState != newKeyState)
                {
                    PlayerActor.currentIndex = 0;
                    canSprintR = false;
                    isJumpingR = true;
                }

                canWalkR = false;
                canJumpR = true;
            }    

            if (!canSprintR && !canJumpR)
            {
                canWalkL = false;
                canJumpR = false;
                canWalkR = true;

                if (PlayerActor.animationPlayed)
                {
                    PlayerActor.currentIndex = 0;
                    PlayerActor.animationPlayed = false;
                }
            }

            if (isSprintingR)
            {
                PlayerActor.position.X += 7 * SpriteScale.X;
            }
            else
            {
                PlayerActor.position.X += 5 * SpriteScale.X;
            }
        }//end MoveRight

        public void StopRight()
        {
            isWalkingR = false;
            canWalkR = false;
            canSprintR = false;
        }//end StopRight

        public void ControllerFloorCollide(GamePadState newControlState, List<Rectangle> walls, Vector2 SpriteScale, bool canControl)
        {
            PlayerActor.position.X = oldPosition.X;
            PlayerActor.position.Y = oldPosition.Y;
            isInAir = false;
            canJump = true;
            vy = 0;

            if (canJumpR == true)
            {
                canJumpR = false;
            }
            else if (canJumpL == true)
            {
                canJumpL = false;
            }

            if (newControlState.ThumbSticks.Left.X < 0 && canControl)
            {
                PlayerActor.position.X += -5 * SpriteScale.X;

                foreach (Rectangle wall in walls)
                {
                    if (isCollidingWith(wall))
                    {
                        PlayerActor.position.X = oldPosition.X;
                    }
                }
            }

            if (newControlState.ThumbSticks.Left.X > 0 && canControl)
            {
                PlayerActor.position.X += 5 * SpriteScale.X;

                foreach (Rectangle wall in walls)
                {
                    if (isCollidingWith(wall))
                    {
                        PlayerActor.position.X = oldPosition.X;
                    }
                }
            }
        }

        public void FloorCollide(KeyboardState newKeyState, List<Rectangle> walls, Vector2 SpriteScale, bool canControl)
        {
            PlayerActor.position.X = oldPosition.X;
            PlayerActor.position.Y = oldPosition.Y;
            isInAir = false;
            canJump = true;
            vy = 0;

            if (canJumpR == true)
            {
                canJumpR = false;
            }
            else if (canJumpL == true)
            {
                canJumpL = false;
            }

            if (newKeyState.IsKeyDown(Keys.A) && canControl)
            {
                PlayerActor.position.X += -5 * SpriteScale.X;

                foreach (Rectangle wall in walls)
                {
                    if (isCollidingWith(wall))
                    {
                        PlayerActor.position.X = oldPosition.X;
                    }
                }
            }

            if (newKeyState.IsKeyDown(Keys.D) && canControl)
            {
                PlayerActor.position.X += 5 * SpriteScale.X;

                foreach (Rectangle wall in walls)
                {
                    if (isCollidingWith(wall))
                    {
                        PlayerActor.position.X = oldPosition.X;
                    }
                }
            }    
        }

        public void Jump(Vector2 SpriteScale)
        {
            isInAir = true;

            if (canJump == true)
            {
                DoJump(SpriteScale);

                if (idleRight || isWalkingR)
                {
                    if (canJumpR == false)
                    {
                        PlayerActor.currentIndex = 0;
                    }

                    canJumpL = false;
                    canJumpR = true;
                }
                else
                {
                    if (canJumpL == false)
                    {
                        PlayerActor.currentIndex = 0;
                    }

                    canJumpR = false;
                    canJumpL = true;
                }
            }

            canJump = false;
            isInAir = true;
        }

        public void UpdateCollisionBoxes(Vector2 SpriteScale)
        {
            boundsRect = new Rectangle(0, 0, (int)(PlayerActor.sourceRectangle.Value.Width), (int)(PlayerActor.sourceRectangle.Value.Height));
            PlayerActor.origin = new Vector2(boundsRect.Width / 2f, boundsRect.Height / 2f);
        }

        public void DoJump(Vector2 SpriteScale)
        {
            vy = -22 * SpriteScale.Y;
        }

        public void Update(GameTime gameTime)
        {
            vy += gravity;
            PlayerActor.position.Y += vy;

            if (canWalkL)
            {
                this.PlayerActor.texture = walkTexture;
                PlayerActor.Update(gameTime, 1, walkListL);
                idleRight = false;
            }
            else if (canWalkR)
            {
                this.PlayerActor.texture = walkTexture;
                PlayerActor.Update(gameTime, 1, walkListR);
                idleRight = true;
            }
            else if (canSprintL)
            {
                this.PlayerActor.texture = sprintTexture;
                PlayerActor.Update(gameTime, 1, sprintListL);
            }
            else if (canSprintR)
            {
                this.PlayerActor.texture = sprintTexture;
                PlayerActor.Update(gameTime, 1, sprintListR);
            }
            else if (canJumpL)
            {
                this.PlayerActor.texture = jumpTexture;
                PlayerActor.Update(gameTime, 1, jumpListL);
            }
            else if (canJumpR)
            {
                this.PlayerActor.texture = jumpTexture;
                PlayerActor.Update(gameTime, 1, jumpListR);
            }
            else if (canWin)
            {
                this.PlayerActor.texture = winTexture;
                PlayerActor.Update(gameTime, 1, winList);
            }
            else
            {
                PlayerActor.currentIndex = 0;
                this.PlayerActor.texture = walkTexture;
                if (idleRight == true)
                {
                    PlayerActor.sourceRectangle = walkListR[0];
                }
                else
                {
                    PlayerActor.sourceRectangle = walkListL[0];
                }
            }
        }

        public void SetAllOff()
        {
            canJumpL = false;
            canJumpR = false;
            canWalkL = false;
            canWalkR = false;
            canSprintL = false;
            canSprintR = false;
        }

        public void WallCollide()
        {
            PlayerActor.position.X = oldPosition.X;
        }

        public void Draw(GameTime gameTime, SpriteBatch SB)
        {
            PlayerActor.Draw(gameTime, SB);          
        }

        public bool isCollidingWith(Rectangle other)
        {
            collision.X = (int)(PlayerActor.position.X - PlayerActor.origin.X * PlayerActor.scale.X);
            collision.Y = (int)(PlayerActor.position.Y - PlayerActor.origin.Y * PlayerActor.scale.Y);

            if (collision.IsEmpty == true)
            {
                if (collision == null)
                {
                    collision.Width = PlayerActor.texture.Width;
                    collision.Height = PlayerActor.texture.Height;
                }
            }
            else
            {
                collision.Width = (int)(boundsRect.Width * PlayerActor.scale.X);
                collision.Height = (int)(boundsRect.Height * PlayerActor.scale.Y);
            }
            
            return this.collision.Intersects(other);
        }

    }
}
