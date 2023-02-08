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
    class Menu
    {
        public bool inMenu;
        public List<MenuButton> menuButtons;

        public Menu(ContentManager Content, Vector2 DefaultResolution, Vector2 SpriteScale)
        {
            inMenu = true;
            menuButtons = new List<MenuButton>();
            menuButtons.Add(SetSinglePlayer(Content, DefaultResolution, SpriteScale, new MenuButton(Content, @"Images\Singleplayer", @"Images\Singleplayer_Highlighted")));
            menuButtons.Add(SetMultiPlayer(Content, DefaultResolution, SpriteScale, new MenuButton(Content, @"Images\Multiplayer", @"Images\Multiplayer_Highlighted")));
            menuButtons.Add(SetOptions(Content, DefaultResolution, SpriteScale, new MenuButton(Content, @"Images\Options", @"Images\Options_Highlighted")));
            menuButtons.Add(SetExit(Content, DefaultResolution, SpriteScale, new MenuButton(Content, @"Images\Exit", @"Images\Exit_Highlighted")));
        }

        public MenuButton SetSinglePlayer(ContentManager Content, Vector2 DefaultResolution, Vector2 SpriteScale, MenuButton SinglePlayer)
        {
            SinglePlayer.origin = new Vector2(SinglePlayer.texture.Width / 2.0f, SinglePlayer.texture.Height / 2.0f);
            SinglePlayer.scale.X = SpriteScale.X * 0.15f;
            SinglePlayer.scale.Y = SpriteScale.Y * 0.15f;
            SinglePlayer.position = new Vector2((DefaultResolution.X / 8.0f) * 1.65f * SpriteScale.X, (DefaultResolution.Y / 6.0f) * 1.75f * SpriteScale.Y);
            return SinglePlayer;
        }

        public MenuButton SetMultiPlayer(ContentManager Content, Vector2 DefaultResolution, Vector2 SpriteScale, MenuButton MultiPlayer)
        {
            MultiPlayer.origin = new Vector2(MultiPlayer.texture.Width / 2.0f, MultiPlayer.texture.Height / 2.0f);
            MultiPlayer.scale.X = SpriteScale.X * 0.15f;
            MultiPlayer.scale.Y = SpriteScale.Y * 0.15f;
            MultiPlayer.position = new Vector2((DefaultResolution.X / 8.0f) * 1.65f * SpriteScale.X, (DefaultResolution.Y / 6.0f) * 2.25f * SpriteScale.Y);
            return MultiPlayer;
        }

        public MenuButton SetOptions(ContentManager Content, Vector2 DefaultResolution, Vector2 SpriteScale, MenuButton Options)
        {
            Options.origin = new Vector2(Options.texture.Width / 2.0f, Options.texture.Height / 2.0f);
            Options.scale.X = SpriteScale.X * 0.15f;
            Options.scale.Y = SpriteScale.Y * 0.15f;
            Options.position = new Vector2((DefaultResolution.X / 8.0f) * 1.65f * SpriteScale.X, (DefaultResolution.Y / 6.0f) * 2.75f * SpriteScale.Y);
            return Options;
        }

        public MenuButton SetExit(ContentManager Content, Vector2 DefaultResolution, Vector2 SpriteScale, MenuButton Exit)
        {
            Exit.origin = new Vector2(Exit.texture.Width / 2.0f, Exit.texture.Height / 2.0f);
            Exit.scale.X = SpriteScale.X * 0.15f;
            Exit.scale.Y = SpriteScale.Y * 0.15f;
            Exit.position = new Vector2((DefaultResolution.X / 8.0f) * 1.65f * SpriteScale.X, (DefaultResolution.Y / 6.0f) * 3.25f * SpriteScale.Y);
            return Exit;
        }

        public void Update(Point MouseLoc)
        {
            foreach (MenuButton Button in menuButtons)
            {
                Button.UpdateCollision();
                Button.CheckCollisionWithMouse(MouseLoc);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (MenuButton Button in menuButtons)
            {
                Button.Draw(gameTime, spriteBatch);
            }
        }
    }
}
