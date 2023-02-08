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

namespace Final_Prototype
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Coconut Coconut1;
        PlayerLives PlayerOneLife;
        PlayerLives PlayerTwoLife;
        DonkeyKong PlayerOne;
        DiddyKong PlayerTwo;
        Necky NeckyL;              //Left Side Necky (Boss)
        Necky NeckyR;              //Right Side Necky (Boss)
        Random random;             //Random Generation Seed
        Vector2 SpriteScale;       //Resolution Scaling Vector
        Vector2 StartPt_NeckyL;    //Start Coordinates of NeckyL
        Vector2 StartPt_NeckyR;    //Start Coordinates of NeckyR
        KeyboardState newKeyState; //Current Keyboard State
        KeyboardState oldKeyState; //Old Keyboard State
        Controller PlayerTwoControl;
        MouseState newMouseState;  //Current Mouse State
        MouseState oldMouseState;  //Old Mouse State
        Song MenuTheme;            //Menu Music
        Song MenuLoop;
        Song GameTheme;            //Game Music
        Song GameOver;
        Song Winner;
        Tramp trampoline;
        GameStats saveData;
        BossHealth neckyHP;
        Boundaries GameBounds;
        Menu GameMenu;
        Backgrounds Background;
        Display DisplaySettings;

        bool spawnRight;      //Determines Which Necky Spawns
        bool reachedSpawn;    //Determines When Necky Reaches Start Coordinates
        bool movingBack;      //Determines if Necky is Despawning
        bool isPlaying;
        bool hasBeenHit;
        bool Multiplayer;
        float restartTime;
        float restartTimeEnd;
        float NeckyIdleAngle; //Angle For Necky's Idle 'Animation'

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Set_Display();
            Set_Scale();
            Set_Background();
            Set_Menu();
            restartTimeEnd = 7000f;
            saveData = new GameStats(Content, @"C:\Users\Public\PlayerScore.txt", SpriteScale);
            base.Initialize();
        }

        public void Set_Boundaries()
        {
            trampoline = new Tramp(Content, new Vector2(DisplaySettings.currentWidth, DisplaySettings.currentHeight), SpriteScale);     
            PlayerOneLife = new PlayerLives(Content, SpriteScale, 1);

            if (Multiplayer)
            {
                PlayerTwoLife = new PlayerLives(Content, SpriteScale, 2);
            }

            GameBounds = new Boundaries(new Vector2(DisplaySettings.currentWidth, DisplaySettings.currentHeight), SpriteScale);      
        }

        public void Set_DK()
        {
            PlayerOne = new DonkeyKong(Content, SpriteScale, @"Images\DK_Walk");
            PlayerOne.controlOne = true;

            if (Multiplayer)
            {
                PlayerTwo = new DiddyKong(Content, SpriteScale, @"Images\Diddy_Walk");
                PlayerTwo.controlOne = false;
            }
            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MenuTheme = Content.Load<Song>(@"Music\MenuStart");
            MenuLoop = Content.Load<Song>(@"Music\MenuLoop");
            GameTheme = Content.Load<Song>(@"Music\BossLoop");
            GameOver = Content.Load<Song>(@"Music\GameOver");
            Winner = Content.Load<Song>(@"Music\YouWin");   
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            newKeyState = Keyboard.GetState();

            if (Multiplayer)
            {
                PlayerTwoControl.NewState();
            }

            newMouseState = Mouse.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || newKeyState.IsKeyDown(Keys.Escape))
                this.Exit();
                
            if (GameMenu.inMenu == true)
            {
                if (isPlaying == false)
                {
                    MediaPlayer.Play(MenuTheme);
                    MediaPlayer.Volume = 0.25f;
                    MediaPlayer.IsRepeating = true;
                    isPlaying = true;
                }                     

                Menu();
            }
            else
            {
                if((!NeckyL.isDead && !NeckyR.isDead) && (PlayerOneLife.lifeCounter != 0 || PlayerTwoLife.lifeCounter != 0))
                {
                    saveData.GameTimer(gameTime);

                    if (isPlaying == false)
                    {
                        MediaPlayer.Play(GameTheme);
                        isPlaying = true;
                    }

                    if (PlayerOneLife.lifeCounter != 0)
                    {
                        PlayerOne.UpdatePosition();
                        PlayerOne.UpdateCollisionBoxes(SpriteScale);
                        PlayerOne.PlayerControls(newKeyState, oldKeyState, SpriteScale);
                        PlayerOne.Update(gameTime);
                        GameBounds.CheckWallCollision(PlayerOne);
                    }

                    if (Multiplayer && PlayerTwoLife.lifeCounter != 0)
                    {
                        PlayerTwo.UpdatePosition();
                        PlayerTwo.UpdateCollisionBoxes(SpriteScale);
                        PlayerTwo.PlayerControllerControls(PlayerTwoControl.newControlState, PlayerTwoControl.oldControlState, SpriteScale);
                        PlayerTwo.Update(gameTime);
                        GameBounds.CheckWallCollision(PlayerTwo);
                    }

                    trampoline.Update(gameTime);                      

                    if (!spawnRight)
                    {
                        NeckyL.NeckyCollision(SpriteScale, spawnRight);
                    }
                    else
                    {
                        NeckyR.NeckyCollision(SpriteScale, spawnRight);
                    }

                    if (PlayerOne.isCollidingWith(Coconut1.collision) && !Coconut1.hitPlayer)
                    {
                        if (PlayerOne.oldPosition.Y < (Coconut1.position.Y - Coconut1.origin.Y))
                        {
                            PlayerOne.DoJump(SpriteScale);
                        }
                        else
                        {
                            PlayerOneLife.LoseLife();
                            PlayerOne.HurtSound();
                        }

                        Coconut1.hitPlayer = true;
                    }
                    else if (PlayerOne.isCollidingWith(GameBounds.bottomBoundary))
                    {
                        PlayerOne.FloorCollide(newKeyState, GameBounds.wallList, SpriteScale, true);
                    }

                    if (PlayerOne.isCollidingWith(trampoline.Collision()))
                    {
                        PlayerOne.PlayerActor.currentIndex = 0;

                        if (trampoline.trampoline.animationPlayed)
                        {
                            trampoline.canBounceS = false;
                            trampoline.canBounceB = false;
                            trampoline.trampoline.currentIndex = 0;
                            trampoline.canPlaySound = true;
                        }

                        TireBounce(true);
                    }
                    else if(trampoline.trampoline.animationPlayed)
                    {
                        trampoline.canBounceS = false;
                        trampoline.canBounceB = false;
                        trampoline.trampoline.currentIndex = 0;
                        trampoline.canPlaySound = true;
                    }

                    if (Multiplayer && PlayerTwoLife.lifeCounter != 0)
                    {
                        if (PlayerTwo.isCollidingWith(Coconut1.collision) && !Coconut1.hitPlayer)
                        {
                            if (PlayerTwo.oldPosition.Y < (Coconut1.position.Y - Coconut1.origin.Y))
                            {
                                PlayerTwo.DoJump(SpriteScale);
                            }
                            else
                            {
                                PlayerTwoLife.LoseLife();
                                PlayerTwo.HurtSound();
                            }

                            Coconut1.hitPlayer = true;
                        }
                        else if (PlayerTwo.isCollidingWith(GameBounds.bottomBoundary))
                        {
                            PlayerTwo.ControllerFloorCollide(PlayerTwoControl.newControlState, GameBounds.wallList, SpriteScale, true);
                        }

                        if (PlayerTwo.isCollidingWith(trampoline.Collision()))
                        {
                            PlayerTwo.PlayerActor.currentIndex = 0;
           
                            if (trampoline.animationFinished())
                            {
                                trampoline.canBounceS = false;
                                trampoline.canBounceB = false;
                                trampoline.trampoline.currentIndex = 0;
                                trampoline.canPlaySound = true;
                            }

                            TireBounce(false);
                        }
                        else if (trampoline.animationFinished())
                        {
                            trampoline.canBounceS = false;
                            trampoline.canBounceB = false;
                            trampoline.trampoline.currentIndex = 0;
                            trampoline.canPlaySound = true;
                        }
                    }

                    if (reachedSpawn == false && movingBack == false)
                    {
                        Necky_MoveToSpawn();
                    }

                    if ((movingBack == true || Coconut1.neckyCanDespawn))
                    {
                        if (!spawnRight)
                        {
                            if (!NeckyL.gotHurt)
                            {
                                Necky_MoveOffScreen();
                            }
                        }
                        else
                        {
                            if (!NeckyR.gotHurt)
                            {
                                Necky_MoveOffScreen();
                            }
                        }
                    
                    }

                    if ((NeckyL.gotHurt == false || NeckyR.gotHurt == false) && hasBeenHit == false && Coconut1.canHitBox && (PlayerOne.isCollidingWith(NeckyL.HitBox) || PlayerOne.isCollidingWith(NeckyR.HitBox)))
                    {
                        if (!spawnRight && (PlayerOne.oldPosition.Y + PlayerOne.PlayerActor.origin.Y) < NeckyL.HitBox.Y)
                        {
                            NeckyL.HurtTrue();
                            PlayerOne.DoJump(SpriteScale);
                            neckyHP.hurtBoss();
                            NeckyL.HurtSound();
                        }
                        else if (spawnRight && (PlayerOne.oldPosition.Y + PlayerOne.PlayerActor.origin.Y) < NeckyR.HitBox.Y)
                        {
                            NeckyR.HurtTrue();
                            PlayerOne.DoJump(SpriteScale);
                            neckyHP.hurtBoss();
                            NeckyR.HurtSound();
                        }
                        else
                        {
                            PlayerOneLife.LoseLife();
                            PlayerOne.HurtSound();
                        }
               
                        NeckyL.gotHurt = true;
                        NeckyR.gotHurt = true;
                        hasBeenHit = true;
                    }

                    if (Multiplayer && PlayerTwoLife.lifeCounter != 0)
                    {
                        if ((NeckyL.gotHurt == false || NeckyR.gotHurt == false) && hasBeenHit == false && Coconut1.canHitBox && (PlayerTwo.isCollidingWith(NeckyL.HitBox) || PlayerTwo.isCollidingWith(NeckyR.HitBox)))
                        {
                            if (!spawnRight && (PlayerTwo.oldPosition.Y + PlayerTwo.PlayerActor.origin.Y) < NeckyL.HitBox.Y)
                            {
                                NeckyL.HurtTrue();
                                PlayerTwo.DoJump(SpriteScale);
                                neckyHP.hurtBoss();
                                NeckyL.HurtSound();
                            }
                            else if (spawnRight && (PlayerTwo.oldPosition.Y + PlayerTwo.PlayerActor.origin.Y) < NeckyR.HitBox.Y)
                            {
                                NeckyR.HurtTrue();
                                PlayerTwo.DoJump(SpriteScale);
                                neckyHP.hurtBoss();
                                NeckyR.HurtSound();
                            }
                            else
                            {
                                PlayerTwoLife.LoseLife();
                                PlayerTwo.HurtSound();
                            }

                            NeckyL.gotHurt = true;
                            NeckyR.gotHurt = true;
                            hasBeenHit = true;
                        }
                    }

                    if (!spawnRight && NeckyL.gotHurt && NeckyL.NeckyActor.animationPlayed)
                    {
                        NeckyL.gotHurt = false;
                        NeckyL.NeckyActor.currentIndex = 0;
                    }
                    else if (spawnRight && NeckyR.gotHurt && NeckyR.NeckyActor.animationPlayed)
                    {
                        NeckyR.gotHurt = false;
                        NeckyR.NeckyActor.currentIndex = 0;
                    }

                    if (NeckyR.canBeDefeated == false || NeckyL.canBeDefeated == false)
                    {
                        Necky_Idle();
                    }

                    if (Coconut1.canSet == true)
                    {
                        if (!spawnRight)
                        {
                            Coconut1.Set_Coconut(new Vector2(PlayerOne.PlayerActor.position.X, PlayerOne.PlayerActor.position.Y), StartPt_NeckyL, new Vector2(DisplaySettings.defaultWidth, DisplaySettings.defaultHeight), new Vector2(DisplaySettings.currentWidth, DisplaySettings.currentHeight), NeckyL.msPerFrame, spawnRight);
                        }
                        else
                        {
                            Coconut1.Set_Coconut(new Vector2(PlayerOne.PlayerActor.position.X, PlayerOne.PlayerActor.position.Y), StartPt_NeckyR, new Vector2(DisplaySettings.defaultWidth, DisplaySettings.defaultHeight), new Vector2(DisplaySettings.currentWidth, DisplaySettings.currentHeight), NeckyR.msPerFrame, spawnRight);
                        }

                        NeckyL.canShootSound = true;
                    }

                    if (Coconut1.canFire == true)
                    {
                        Coconut1.Shoot_Coconut(gameTime, NeckyL);
                    }

                    if (Coconut1.canBounce == true)
                    {
                        Coconut1.Bounce_Coconut();
                    }

                    if (Coconut1.bouncing == true)
                    {
                        Coconut1.Bouncing_Coconut(gameTime);
                    }

                    PlayerOneLife.Update(gameTime, spriteBatch, SpriteScale);

                    if (Multiplayer)
                    {
                        PlayerTwoLife.Update(gameTime, spriteBatch, SpriteScale);
                    }

                    neckyHP.Update(gameTime, spriteBatch);
                    neckyHP.BossDeath(NeckyL, NeckyR, PlayerOneLife.lifeCounter, isPlaying);

                    if (PlayerOneLife.lifeCounter == 0 || neckyHP.bossHP == 0)
                    {
                        isPlaying = false;
                    }
                }  
                else
                {
                    if (restartTime >= restartTimeEnd)
                    {
                        MediaPlayer.Stop();
                        isPlaying = false;
                        restartTime = 0;
                        NeckyL.isDead = false;
                        NeckyL.canBeDefeated = false;
                        NeckyL.NeckyActor.currentIndex = 0;
                        NeckyL.canIdle = true;
                        NeckyL.deathRepeat = 0;
                        NeckyL.fallCount = 0;
                        NeckyL.NeckyActor.msPerFrame = 75;
                        NeckyR.isDead = false;
                        NeckyR.NeckyActor.currentIndex = 0;
                        NeckyR.canIdle = true;
                        NeckyR.canBeDefeated = false;
                        NeckyR.deathRepeat = 0;
                        NeckyR.fallCount = 0;
                        NeckyR.NeckyActor.msPerFrame = 75;
                        Initialize();
                        return;
                    }

                    restartTime += gameTime.ElapsedGameTime.Milliseconds;

                    Coconut1.position.X = -1000;
                    Coconut1.position.Y = -1000;

                    PlayerOne.SetAllOff();

                    if (Multiplayer)
                    {
                        PlayerTwo.SetAllOff();
                    }

                    if (!saveData.SaveGameTime && (PlayerOneLife.lifeCounter != 0 || PlayerTwoLife.lifeCounter != 0))
                    {
                        saveData.SaveTime();
                    }

                    if (isPlaying == false && (PlayerOneLife.lifeCounter != 0 || PlayerTwoLife.lifeCounter != 0))
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(Winner);
                        MediaPlayer.IsRepeating = false;
                        isPlaying = true;
                    }
                    else if(isPlaying == false)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Play(GameOver);
                        MediaPlayer.IsRepeating = false;
                        isPlaying = true;
                    }

                    if(!spawnRight && PlayerOneLife.lifeCounter != 0)
                    {
                        NeckyL.DeathFall(gameTime, StartPt_NeckyL);
                    }
                    else if(spawnRight && (PlayerOneLife.lifeCounter != 0 || PlayerTwoLife.lifeCounter != 0))
                    {
                        NeckyR.DeathFall(gameTime, StartPt_NeckyR);
                    }

                    PlayerOne.UpdatePosition();  
                    PlayerOne.Update(gameTime);
                    PlayerOne.UpdateCollisionBoxes(SpriteScale);
                    GameBounds.CheckWallCollision(PlayerOne);

                    if (Multiplayer)
                    {
                        PlayerTwo.UpdatePosition();
                        PlayerTwo.Update(gameTime);
                        PlayerTwo.UpdateCollisionBoxes(SpriteScale);
                        GameBounds.CheckWallCollision(PlayerTwo);
                    }

                    if (PlayerOne.isCollidingWith(GameBounds.bottomBoundary))
                    {
                        PlayerOne.FloorCollide(newKeyState, GameBounds.wallList, SpriteScale, false);
                    }

                    if (Multiplayer)
                    {
                        PlayerTwo.canWin = true;
                        PlayerTwo.PlayerActor.scale.X = SpriteScale.X * 0.75f;
                        PlayerTwo.PlayerActor.scale.Y = SpriteScale.Y * 0.75f;

                        if (PlayerTwo.isCollidingWith(GameBounds.bottomBoundary))
                        {
                            
                            PlayerTwo.ControllerFloorCollide(PlayerTwoControl.newControlState, GameBounds.wallList, SpriteScale, false);
                        }
                    }
                }
                
                NeckyL.Update(gameTime);
                NeckyR.Update(gameTime);
            }

            oldKeyState = newKeyState;

            if (Multiplayer)
            {
                PlayerTwoControl.OldState();
            }

            oldMouseState = newMouseState;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();           
            if (GameMenu.inMenu == true)
            {
                Background.DrawMenuBackground(gameTime, spriteBatch);
                saveData.getBestTime(gameTime, spriteBatch);
                GameMenu.Draw(gameTime, spriteBatch);
            }
            else
            {
                Background.DrawBackground(gameTime, spriteBatch);
                saveData.DrawTime(gameTime, spriteBatch);
                neckyHP.Draw(gameTime, spriteBatch);
                Coconut1.Draw(gameTime, spriteBatch);
                NeckyL.NeckyActor.Draw(gameTime, spriteBatch);
                NeckyR.NeckyActor.Draw(gameTime, spriteBatch);
                trampoline.Draw(gameTime, spriteBatch);

                if (Multiplayer && PlayerTwoLife.lifeCounter != 0)
                {
                    PlayerTwo.Draw(gameTime, spriteBatch);
                }

                if (PlayerOneLife.lifeCounter != 0)
                {
                    PlayerOne.Draw(gameTime, spriteBatch);
                }

                Background.DrawForeground(gameTime, spriteBatch);
                PlayerOneLife.Draw(gameTime, spriteBatch, SpriteScale);

                if (Multiplayer)
                {
                    PlayerTwoLife.Draw(gameTime, spriteBatch, SpriteScale);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void TireBounce(bool isPlayerOne)
        {
            if (isPlayerOne)
            {
                if (newKeyState.IsKeyDown(Keys.Space))
                {
                    trampoline.canBounceB = true;
                    PlayerOne.vy = -30 * SpriteScale.Y;
                }
                else
                {
                    trampoline.canBounceS = true;
                    PlayerOne.vy = -15 * SpriteScale.Y;
                }
            }
            else
            {
                if (PlayerTwoControl.newControlState.Buttons.A == ButtonState.Pressed)
                {
                    trampoline.canBounceB = true;
                    PlayerTwo.vy = -30 * SpriteScale.Y;           
                }
                else
                {
                    trampoline.canBounceS = true;
                    PlayerTwo.vy = -15 * SpriteScale.Y;        
                }
            }
        }

        //Initializes Display Settings
        private void Set_Display()
        {
            DisplaySettings = new Display(graphics);
            IsMouseVisible = true;
            IsFixedTimeStep = true;
            Window.AllowUserResizing = false;
        }//end Set_Display Function

        //Initializes Sprite Scale Based on Display Settings
        private void Set_Scale()
        {
            Console.WriteLine("inside Set_Scale Function");

            //Sprite Scaling Vector
            SpriteScale = new Vector2(((float)DisplaySettings.currentWidth / (float)DisplaySettings.defaultWidth), ((float)DisplaySettings.currentHeight / (float)DisplaySettings.defaultHeight));

            Console.WriteLine(" -Scale Factor X: " + SpriteScale.X);
            Console.WriteLine(" -Scale Factor Y: " + SpriteScale.Y);

        }//end Set_Scale Function

        //Initializes Background Image Settings
        public void Set_Background()
        {
            Background = new Backgrounds(Content, new Vector2(DisplaySettings.currentWidth, DisplaySettings.currentHeight), SpriteScale);
        }//end Set_Background

        //Sets All Menu Buttons
        public void Set_Menu()
        {
            GameMenu = new Menu(Content, new Vector2(DisplaySettings.defaultWidth, DisplaySettings.defaultHeight), SpriteScale);
        }

        //Updates All Menu Buttons
        public void Menu()
        {
            GameMenu.Update(new Point(newMouseState.X, newMouseState.Y));

            if (GameMenu.menuButtons[0].Selected)
            {
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState != newMouseState)
                {
                    Multiplayer = false;
                    Set_DK();
                    Set_Boundaries();
                    Spawn_Necky();
                    neckyHP = new BossHealth(Content, SpriteScale);
                    Coconut1 = new Coconut(Content, @"Images\Coconut", SpriteScale);
                    Coconut1.canSet = false;
                    IsMouseVisible = false;
                    MediaPlayer.Stop();
                    isPlaying = false;
                    GameMenu.inMenu = false;
                }
            }
            else if (GameMenu.menuButtons[1].Selected)
            {
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState != newMouseState)
                {
                    Multiplayer = true;
                    PlayerTwoControl = new Controller(1);
                    Set_DK();
                    Set_Boundaries();
                    Spawn_Necky();
                    neckyHP = new BossHealth(Content, SpriteScale);
                    Coconut1 = new Coconut(Content, @"Images\Coconut", SpriteScale);
                    Coconut1.canSet = false;
                    IsMouseVisible = false;
                    MediaPlayer.Stop();
                    isPlaying = false;
                    GameMenu.inMenu = false;
                }
            }
            else if (GameMenu.menuButtons[3].Selected)
            {
                if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState != newMouseState)
                {
                    this.Exit();
                }
            }
        }//end Menu

        //Decides Whether to Spawn Necky on Left or Right Side
        private void RandomGeneration()
        {
            random = new Random();               //Random Seeder
            int randomSpawn = random.Next(1, 3); //Random Number Between 1 and 2
            reachedSpawn = false;                //Indicates Necky Has Not Moved to Start Point Yet
            movingBack = false;

            //If Random Number is 1 Spawn Necky on Right Side
            if (randomSpawn == 1)
            {
                spawnRight = true;
                return;
            }

            //Otherwise Spawn Necky on Left Side
            spawnRight = false;
        }//end RandomGeneration Function

        //Creates Left Side Necky
        private void Set_NeckyL()
        {
            //If NeckyL is Uninitialized Creates NeckyL
            if(NeckyL == null)
            {
                NeckyL = new Necky(Content, false);               //Calls BaseSprite Constructor
                Vector2 OriginPt_NeckyL = new Vector2(0, 50);     //Creates Origin Point for NeckyL
                NeckyL.NeckyActor.origin = OriginPt_NeckyL;       //Assigns Origin Point to NeckyL
                NeckyL.NeckyActor.scale.X = SpriteScale.X * 2.0f; //Scale X by Scale Factor and Extra Scale Increase
                NeckyL.NeckyActor.scale.Y = SpriteScale.Y * 2.0f; //Scale Y by Scale Factor and Extra Scale Increase
            }

            if (Coconut1 != null)
            {
                Coconut1.canHitBox = false;
            }

            hasBeenHit = false;
            NeckyL.gotHurt = false;
            float randomY = random.Next(DisplaySettings.currentHeight / 4, (DisplaySettings.currentHeight / 4) * 3);                    //Random y-position Between 1/4 and 3/4 of Screen
            float spawnOffset = 140f * SpriteScale.X;                                                    //How Far 'Back' NeckyL Spawns From Desired Start Point
            StartPt_NeckyL = new Vector2(0 - ((100 * SpriteScale.X) / 2), randomY);                     //Creates Position Point for NeckyL
            NeckyL.NeckyActor.position = new Vector2(StartPt_NeckyL.X - spawnOffset, StartPt_NeckyL.Y); //Assigns Position Point to NeckyL
            NeckyL.a = (DisplaySettings.defaultHeight / 2) * SpriteScale.Y;
            NeckyL.t = (float)Math.Sqrt(((DisplaySettings.currentHeight - 50 * SpriteScale.X) - StartPt_NeckyL.Y) / (0.5f * NeckyL.a)) * 1000f;
            NeckyL.startY = StartPt_NeckyL.Y;
        }//end Set_NeckyL Function

        //Creates Right Side Necky
        private void Set_NeckyR()
        {
            //If NeckyR is Uninitialized Creates NeckyR
            if(NeckyR == null)
            {
                NeckyR = new Necky(Content, true);                //Calls BaseSprite Constructor
                Vector2 OriginPt_NeckyR = new Vector2(100, 50);   //Creates Origin Point for NeckyR
                NeckyR.NeckyActor.origin = OriginPt_NeckyR;       //Assigns Origin Point to NeckyR
                NeckyR.NeckyActor.scale.X = SpriteScale.X * 2.0f; //Scale X by Scale Factor and Extra Scale Increase
                NeckyR.NeckyActor.scale.Y = SpriteScale.Y * 2.0f; //Scale Y by Scale Factor and Extra Scale Increase
            }

            if (Coconut1 != null)
            {
                Coconut1.canHitBox = false;
            }
			
            hasBeenHit = false;
            NeckyR.gotHurt = false;
            float randomY = random.Next(DisplaySettings.currentHeight / 4, (DisplaySettings.currentHeight / 4) * 3);                    //Random y-position Between 1/4 and 3/4 of Screen
            float spawnOffset = 140f * SpriteScale.X;                                                    //How Far 'Forward' NeckyR Spawns From Desired Start Point
            StartPt_NeckyR = new Vector2(DisplaySettings.currentWidth + ((100 * SpriteScale.X) / 2), randomY);          //Creates Position Point for NeckyR
            NeckyR.NeckyActor.position = new Vector2(StartPt_NeckyR.X + spawnOffset, StartPt_NeckyR.Y); //Assigns Position Point to NeckyR
            NeckyR.a = (DisplaySettings.defaultHeight / 2) * SpriteScale.Y;
            NeckyR.t = (float)Math.Sqrt(((DisplaySettings.currentHeight - 50 * SpriteScale.X) - StartPt_NeckyR.Y) / (0.5f * NeckyR.a)) * 1000f;
            NeckyR.startY = StartPt_NeckyR.Y;
        }//end Set_NeckyR Function

        //Removes Left Side Necky from Screen
        private void Remove_NeckyL()
        {
            //If NeckyL is Uninitialized Creates NeckyL
            if(NeckyL == null)
            {
                Console.WriteLine(" -NeckyL Uninitialized");
                Set_NeckyL();
            }

            NeckyL.NeckyActor.position = new Vector2(-100, -100); //Move NeckyL Off the Screen
        }//end Remove_NeckyL Function

        //Removes Right Side Necky from Screen
        private void Remove_NeckyR()
        {
            //If NeckyR is Uninitialized Creates NeckyR
            if(NeckyR == null)
            {
                Set_NeckyR();
            }

            NeckyR.NeckyActor.position = new Vector2(-100, -100); //Move NeckyR Off the Screen
        }//end Remove_NeckyR Function

        //Spawns Necky on Left or Right Side Based on Random Generation
        private void Spawn_Necky()
        {
            NeckyIdleAngle = 0; //Resets Idle 'Animation' Angle
            RandomGeneration(); //Calls RandomGeneration Function

            //If Left Side Chosen Spawn Necky on Left Side
            //Else Spawn Necky on Right Side
            if(!spawnRight)
            {
                Set_NeckyL();    //Spawn Left Side Necky
                Remove_NeckyR(); //Remove Right Side Necky
            }
            else
            {
                Set_NeckyR();    //Spawn Right Side Necky
                Remove_NeckyL(); //Remove Left Side Necky
            }
        }//end Spawn_Necky Function

        //Moves Necky in a Circle to Produce a Living Feeling
        private void Necky_Idle()
        {
            float xRadius = 0.12f * SpriteScale.X; //X Radius of Circle
            float yRadius = 0.70f * SpriteScale.Y; //Y Radius of Circle
            float speed = 0.05f;                   //Speed of 'Animation'
            
            //If Left Side Necky is Active
            //Else If Right Side Necky is Active
            if(!spawnRight)
            {
                NeckyL.NeckyActor.position.X -= xRadius * (float)Math.Cos(NeckyIdleAngle);
                NeckyL.NeckyActor.position.Y += yRadius * (float)Math.Sin(NeckyIdleAngle);
                NeckyIdleAngle -= speed;
            }
            else
            {
                NeckyR.NeckyActor.position.X += xRadius * (float)Math.Cos(NeckyIdleAngle);
                NeckyR.NeckyActor.position.Y += yRadius * (float)Math.Sin(NeckyIdleAngle);
                NeckyIdleAngle += speed;
            }
        }//end Necky_Idle Function

        //Moves Necky to Start Point Coordinates When Spawned
        private void Necky_MoveToSpawn()
        {
            float speed = (0.75f * SpriteScale.X); //Speed of Necky Moving to Start Coordinates
            
            //If NeckyL is Active
            //Else if NeckyR is Active
            if(!spawnRight)
            {   
                //Sets reachedSpawn to True if NeckyL Reaches Start Coordinates
                if(NeckyL.NeckyActor.position.X >= StartPt_NeckyL.X)
                {
                    reachedSpawn = true;
                    Fire_Coconut();
                    return;
                }

                //Otherwise Move NeckyL at Desired Speed
                NeckyL.NeckyActor.position.X += speed;
            }
            else
            {
                //Sets reachedSpawn to True if NeckyR Reaches Start Coordinates
                if (NeckyR.NeckyActor.position.X <= StartPt_NeckyR.X)
                {
                    reachedSpawn = true;
                    Fire_Coconut();
                    return;
                }

                //Otherwise Move NeckyR at Desired Speed
                NeckyR.NeckyActor.position.X -= speed;
            }
        }//end Necky_MoveToSpawn Function

        //Despawns Necky
        private void Necky_MoveOffScreen()
        {
            float speed = (0.75f * SpriteScale.X); //Speed of Necky Moving to Despawn Coordinates
            float offScreen;
            movingBack = true;

            //If NeckyL is Active
            //Else if NeckyR is Active
            if (!spawnRight)
            {
                offScreen = -(200f * SpriteScale.X);

                //Sets movingBack to False if NeckyL Reaches Despawn Coordinates
                if (NeckyL.NeckyActor.position.X <= offScreen)
                {
                    movingBack = false;
                    Coconut1.neckyCanDespawn = false;
                    Spawn_Necky();
                    return;
                }

                //Otherwise Move NeckyL at Desired Speed
                NeckyL.NeckyActor.position.X -= speed;
            }
            else
            {
                offScreen = DisplaySettings.currentWidth + (200f * SpriteScale.X);

                //Sets movingBack to False if NeckyR Reaches Despawn Coordinates
                if (NeckyR.NeckyActor.position.X >= offScreen)
                {
                    movingBack = false;
                    Coconut1.neckyCanDespawn = false;
                    Spawn_Necky();
                    return;
                }

                //Otherwise Move NeckyR at Desired Speed
                NeckyR.NeckyActor.position.X += speed;
            }
        }//end Necky_MoveToSpawn Function

        //Function That Fires Coconut
        public void Fire_Coconut()
        {
            //If Left Side Active Shoot From Left Side
            //Else Shoot From Right Side
            if(!spawnRight)
            {
                NeckyL.NeckyActor.animationPlayed = false;
                NeckyL.NeckyActor.currentIndex = 0;
                NeckyL.canIdle = false;
                NeckyL.canBeHurt = false;
                NeckyL.canBeDefeated = false;
                NeckyL.canAttack = true;
            }
            else
            {
                NeckyR.NeckyActor.animationPlayed = false;
                NeckyR.NeckyActor.currentIndex = 0;
                NeckyR.canIdle = false;
                NeckyR.canBeHurt = false;
                NeckyR.canBeDefeated = false;
                NeckyR.canAttack = true;
            }

            Coconut1.canSet = true;
        }//end Fire_Coconut

    }//end Game1 Class
}//end Final_Prototype Namespace
