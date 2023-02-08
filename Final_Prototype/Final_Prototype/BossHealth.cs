/*
 * Author: Connor Pandolph
 * Game: Necky's Revenge
 * Framework: Microsoft XNA
 * Date: 2013
 */
 
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
    class BossHealth
    {
        public List<HealthBar> bossBars;
        public int bossHP;

        public BossHealth(ContentManager Content, Vector2 SpriteScale)
        {
            bossHP = 5;
            bossBars = new List<HealthBar>();

            for (int i = 0; i < bossHP; i++)
            {
                bossBars.Add(new HealthBar(Content, @"Images\Health_green"));
                bossBars[i].Set_Scale(0.5f, SpriteScale);
                bossBars[i].Set_Position((int)((500 * bossBars[i].scale.X) + (i * (120 * bossBars[i].scale.X))), (int)(20 * bossBars[i].scale.Y), SpriteScale);
            }      
        }

        public void hurtBoss()
        {
            bossHP--;
            bossBars.Remove(bossBars[bossBars.Count() - 1]);
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (HealthBar bar in bossBars)
            {
                bar.Update(gameTime, spriteBatch, bossHP);
            }
        }

        public void BossDeath(Necky NeckyL, Necky NeckyR, int playerLives, bool isPlaying)
        {
            if (bossBars.Count == 0 || playerLives == 0)
            {
                NeckyL.isDead = true;
                NeckyR.isDead = true;
                isPlaying = false;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (HealthBar bar in bossBars)
            {
                bar.Draw(gameTime, spriteBatch);
            }
        }

    }//end class
}//end namespace
