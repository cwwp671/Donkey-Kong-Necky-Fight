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

namespace Engine
{
    class BaseSprite
    {
        public Texture2D texture;          //Texture of Image File
        public Vector2 position;           //Position of Sprite
        public Rectangle? sourceRectangle; //Local Size of Sprite
        public Color tint;                 //Color Variable Required to Draw
        public float rotation;             //Rotation of Sprite
        public Vector2 origin;             //Origin Point of Sprite
        public Vector2 scale;              //Scale of Sprite
        public SpriteEffects effects;      //SpriteEffects Variable Required to Draw
        public float layer;                //Layer Order of Sprite
        public Rectangle collision;

        //Constructor
        public BaseSprite(ContentManager Content, String ImageName)
            : base()
        {
            texture = Content.Load<Texture2D>(ImageName); //Sets Texture of Sprite to Texture of Image File
            position = Vector2.Zero;                      //Sprite Position Defaults to (0, 0)
            sourceRectangle = new Rectangle?();           //Size of Sprite Defaults to Null (Entire Image)
            tint = Color.White;                           //Color Defaults to White
            rotation = 0.0f;                              //Rotation Defaults to 0
            origin = Vector2.Zero;                        //Origin Defaults to (0, 0)
            scale = new Vector2(1.0f, 1.0f);                    //Scale Defaults to 100%
            effects = SpriteEffects.None;                 //Effects Defaults to None
            layer = 1.0f;                                 //Layer Order Defaults to 1
            collision = new Rectangle(0, 0, 0, 0);
        }//end Constructor

        //Draw Function
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, tint, rotation, origin, scale, effects, layer);
        }//end Draw Function 

        public void UpdateCollision()
        {
            collision = new Rectangle((int)(position.X - (origin.X * scale.X)), (int)(position.Y - (origin.Y * scale.Y)), (int)(texture.Width * scale.X), (int)(texture.Height * scale.Y));
        }
      
    }//end BaseSprite Class
}//end Engine Namespace
