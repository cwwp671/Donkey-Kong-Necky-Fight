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
    class Display
    {
        public int defaultWidth;
        public int defaultHeight;
        public int currentWidth;
        public int currentHeight;

        public Display(GraphicsDeviceManager graphics)
        {
            defaultWidth = 800;
            defaultHeight = 600;
            currentWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            currentHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //currentWidth = 800;                                                     //TEST RESOLUTION X SETTING
            //currentHeight = 600;                                                    //TEST RESOLUTION Y SETTING
            graphics.PreferredBackBufferWidth = currentWidth;
            graphics.PreferredBackBufferHeight = currentHeight;
            graphics.IsFullScreen = true;
            graphics.SynchronizeWithVerticalRetrace = true;
            graphics.PreferMultiSampling = true;
            //graphics.IsFullScreen = false;                                          //TEST RESOLUTION FULLSCREEN OFF SETTING
            graphics.ApplyChanges();
        }

        public void SetDisplay(GraphicsDeviceManager graphics, Vector2 InputResolution, bool fullscreen)
        {
            currentWidth = (int)InputResolution.X;
            currentHeight = (int)InputResolution.Y;
            graphics.PreferredBackBufferWidth = currentWidth;
            graphics.PreferredBackBufferHeight = currentHeight;

            if (fullscreen)
            {
                graphics.IsFullScreen = true;
            }
            else
            {
                graphics.IsFullScreen = false;
            }

            graphics.ApplyChanges();
        }
        
    }//end class
}//end namespace
