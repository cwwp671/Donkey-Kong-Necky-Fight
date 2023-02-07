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
    class Backgrounds
    {
        public BaseSprite MenuBackground;
        public BaseSprite Background;
        public BaseSprite Foreground;

        public Backgrounds(ContentManager Content, Vector2 CurrentResolution, Vector2 SpriteScale)
        {
            MenuBackground = new BaseSprite(Content, @"Images\MenuBackground");
            MenuBackground.origin = new Vector2(MenuBackground.texture.Width / 2.0f, MenuBackground.texture.Height / 2.0f);
            MenuBackground.scale.X = SpriteScale.X;
            MenuBackground.scale.Y = SpriteScale.Y;
            MenuBackground.position.X = CurrentResolution.X / 2.0f;
            MenuBackground.position.Y = CurrentResolution.Y / 2.0f;

            Background = new BaseSprite(Content, @"Images\BossLevelDefault");
            Background.origin = new Vector2(Background.texture.Width / 2.0f, Background.texture.Height / 2.0f);
            Background.scale.X = SpriteScale.X;
            Background.scale.Y = SpriteScale.Y;
            Background.position.X = CurrentResolution.X / 2.0f;
            Background.position.Y = CurrentResolution.Y / 2.0f;

            Foreground = new BaseSprite(Content, @"Images\ForegroundDefault");
            Foreground.origin = new Vector2(Foreground.texture.Width / 2.0f, Foreground.texture.Height / 2.0f);
            Foreground.scale.X = SpriteScale.X;
            Foreground.scale.Y = SpriteScale.Y;
            Foreground.position.X = CurrentResolution.X / 2.0f;
            Foreground.position.Y = CurrentResolution.Y / 2.0f;
        }

        public void DrawMenuBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            MenuBackground.Draw(gameTime, spriteBatch);
        }

        public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Background.Draw(gameTime, spriteBatch);
        }

        public void DrawForeground(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Foreground.Draw(gameTime, spriteBatch);
        }

    }//end class
}//end namespace
