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
    class MenuButton : BaseSprite
    {
        public Texture2D highlightTexture;
        public Texture2D originalTexture;
        public SoundEffect Highlighted;
        public bool highlightPlayed;
        public bool Selected;

        public MenuButton(ContentManager Content, String ImageName, String HoverName)
            : base(Content, ImageName)
        {
            highlightTexture = Content.Load<Texture2D>(HoverName);
            originalTexture = Content.Load<Texture2D>(ImageName);
            Highlighted = Content.Load<SoundEffect>(@"Sound\Hover");
            Selected = false;
            highlightPlayed = false;
        }

        public void CheckCollisionWithMouse(Point MouseLocation)
        {          
            if (collision.Contains(MouseLocation))
            {
                this.texture = highlightTexture;
                Selected = true;

                if(highlightPlayed == false)
                {
                    Highlighted.Play();
                    highlightPlayed = true;
                }
            }
            else
            {
                if (texture == highlightTexture)
                {
                    this.texture = originalTexture;
                }

                Selected = false;
                highlightPlayed = false;
            }
        }

    }//end Class
}//end Namespace
