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
    class Necky
    {
        public String NeckyAsset;
        public SpriteSheet NeckyActor;
        public List<Rectangle> IdleList;
        public List<Rectangle> AttackList;
        public List<Rectangle> HurtList;
        public List<Rectangle> DefeatedList;
        public Rectangle Collision;
        public Rectangle HitBox;
        public int NeckyIndex;
        public int msPerFrame;
        public bool canIdle;
        public bool canAttack;
        public bool canBeHurt;
        public bool canBeDefeated;
        public bool gotHurt;
        public bool isDead;
        public bool canHitBox;
        public float a;
        public float t;
        public float fallCount;
        public float startY;
        public float bounceY;
        public int deathRepeat;
        SoundEffect Hurt;
        SoundEffect Shoot;
        public bool canShootSound;
        
        public Necky(ContentManager Content, bool right)
        {
            IdleList = new List<Rectangle>();
            AttackList = new List<Rectangle>();
            HurtList = new List<Rectangle>();
            DefeatedList = new List<Rectangle>();

            if(!right)
            {
                NeckyAsset = @"Images\NeckyL_Sheet";
            }
            else
            {
                NeckyAsset = @"Images\NeckyR_Sheet";
            }   
    
            IdleList.Add(new Rectangle(0, 0, 100, 100));

            AttackList.Add(new Rectangle(0, 0, 100, 100));
            AttackList.Add(new Rectangle(100, 0, 100, 100));
            AttackList.Add(new Rectangle(200, 0, 100, 100));
            AttackList.Add(new Rectangle(300, 0, 100, 100));
            AttackList.Add(new Rectangle(400, 0, 100, 100));
            AttackList.Add(new Rectangle(500, 0, 100, 100));
            AttackList.Add(new Rectangle(600, 0, 100, 100));
            AttackList.Add(new Rectangle(700, 0, 100, 100));
            AttackList.Add(new Rectangle(800, 0, 100, 100));
            AttackList.Add(new Rectangle(900, 0, 100, 100));
            AttackList.Add(new Rectangle(900, 0, 100, 100));

            HurtList.Add(new Rectangle(0, 0, 100, 100));
            HurtList.Add(new Rectangle(100, 200, 100, 100));
            HurtList.Add(new Rectangle(200, 200, 100, 100));
            HurtList.Add(new Rectangle(300, 200, 100, 100));
            HurtList.Add(new Rectangle(400, 200, 100, 100));
            HurtList.Add(new Rectangle(500, 200, 100, 100));
            HurtList.Add(new Rectangle(400, 200, 100, 100));      
            HurtList.Add(new Rectangle(300, 200, 100, 100));
            HurtList.Add(new Rectangle(200, 200, 100, 100));
            HurtList.Add(new Rectangle(100, 200, 100, 100));
            
            DefeatedList.Add(new Rectangle(0, 100, 100, 100));
            DefeatedList.Add(new Rectangle(100, 100, 100, 100));
            DefeatedList.Add(new Rectangle(200, 100, 100, 100));
            DefeatedList.Add(new Rectangle(300, 100, 100, 100));
            DefeatedList.Add(new Rectangle(400, 100, 100, 100));
            DefeatedList.Add(new Rectangle(500, 100, 100, 100));
            DefeatedList.Add(new Rectangle(600, 100, 100, 100));
            DefeatedList.Add(new Rectangle(700, 100, 100, 100));
            DefeatedList.Add(new Rectangle(800, 100, 100, 100));
            DefeatedList.Add(new Rectangle(900, 100, 100, 100));
            DefeatedList.Add(new Rectangle(0, 200, 100, 100));
            DefeatedList.Add(new Rectangle(900, 100, 100, 100));
            DefeatedList.Add(new Rectangle(800, 100, 100, 100));
            DefeatedList.Add(new Rectangle(700, 100, 100, 100));
            a = 0;
            t = 0;
            fallCount = 0;
            NeckyIndex = 0;
            startY = 0;
            msPerFrame = 75;
            canIdle = true;
            canAttack = false;
            canBeHurt = false;
            canBeDefeated = false;
            gotHurt = false;
            canHitBox = false;
            isDead = false;
            canShootSound = false;
            deathRepeat = 0;
            Hurt = Content.Load<SoundEffect>(@"Sound\NeckyHurt");
            Shoot = Content.Load<SoundEffect>(@"Sound\NeckyShoot");
            NeckyActor = new SpriteSheet(Content, NeckyAsset, msPerFrame);
            NeckyActor.sourceRectangle = IdleList[0];
        }

        public void HurtSound()
        {
            Hurt.Play();
        }

        public void ShootSound()
        {
            Shoot.Play();
        }

        public void HurtTrue()
        {
            NeckyActor.animationPlayed = false;
            NeckyActor.currentIndex = 0;
            canIdle = false;
            canBeDefeated = false;
            canAttack = false;
            canBeHurt = true;
        }

        public void DeathFall(GameTime gameTime, Vector2 StartPt)
        {
            canBeHurt = false;
            canIdle = false;
            canAttack = false;

            if (NeckyActor.animationPlayed && deathRepeat != 3)
            {
                deathRepeat++;
                NeckyActor.currentIndex = 0;
                NeckyActor.animationPlayed = false;

                if (deathRepeat < 3)
                {
                    HurtSound();
                }
            }

            if (deathRepeat <= 2)
            {
                NeckyActor.Update(gameTime, 0, HurtList);
            }

            if (deathRepeat == 3)
            {
                canBeDefeated = true;
                NeckyActor.msPerFrame = 100;

                if (fallCount <= t)
                {
                    fallCount += gameTime.ElapsedGameTime.Milliseconds;
                    NeckyActor.position.Y = startY + (0.5f * a * (fallCount / 1000f) * (fallCount / 1000f));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if(canIdle == true)
            {
                NeckyActor.Update(gameTime, 0, IdleList);
            }
            else if (canAttack == true)
            {
                NeckyActor.Update(gameTime, 0, AttackList);
            }
            else if (canBeHurt == true)
            {
                NeckyActor.Update(gameTime, 0, HurtList);
            }
            else if (canBeDefeated == true)
            {
                NeckyActor.Update(gameTime, 7, DefeatedList);
            }
        }

        public void NeckyCollision(Vector2 Scaler, bool right)
        {
            Collision = new Rectangle((int)(NeckyActor.position.X - (NeckyActor.origin.X * Scaler.X)), (int)(NeckyActor.position.Y - (NeckyActor.origin.Y * Scaler.Y)), (int)(NeckyActor.sourceRectangle.Value.Width * Scaler.X), (int)(NeckyActor.sourceRectangle.Value.Width * Scaler.Y));

            if (!right)
            {
                HitBox = new Rectangle((int)(NeckyActor.position.X - ((NeckyActor.origin.X - 100f) * Scaler.X)), (int)(NeckyActor.position.Y - ((NeckyActor.origin.Y - 16f) * Scaler.Y)), (int)(63f * Scaler.X), (int)(25f * Scaler.Y));
            }
            else
            {
                HitBox = new Rectangle((int)(NeckyActor.position.X - ((NeckyActor.origin.X + 63f) * Scaler.X)), (int)(NeckyActor.position.Y - ((NeckyActor.origin.Y - 16f) * Scaler.Y)), (int)(63f * Scaler.X), (int)(25f * Scaler.Y));
            }
        }

    }
}
