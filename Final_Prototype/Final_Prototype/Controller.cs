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
    class Controller
    {
        public GamePadState newControlState;
        public GamePadState oldControlState;
        public PlayerIndex playerAssignment;

        public Controller(int playerID)
        {
            newControlState = new GamePadState();
            oldControlState = new GamePadState();

            if (playerID == 1)
            {
                playerAssignment = PlayerIndex.One;
            }
            else if (playerID == 2)
            {
                playerAssignment = PlayerIndex.Two;
            }
        }

        public void NewState()
        {
            newControlState = GamePad.GetState(playerAssignment);
        }

        public void OldState()
        {
            oldControlState = newControlState;
        }
    }
}
