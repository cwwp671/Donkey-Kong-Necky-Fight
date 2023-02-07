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
    class HealthBar : BaseSprite
    {
        public Texture2D GreenTexture;
        public Texture2D OrangeTexture;
        public Texture2D RedTexture;
        public bool removed;

        public HealthBar(ContentManager Content, String ImageName)
            : base(Content, ImageName)
        {
            texture = Content.Load<Texture2D>(ImageName);
            GreenTexture = Content.Load<Texture2D>(@"Images\Health_green");
            OrangeTexture = Content.Load<Texture2D>(@"Images\Health_orange");
            RedTexture = Content.Load<Texture2D>(@"Images\Health_red");
            removed = false;
        }

        public void Set_Healthbar(Vector2 SpriteScale, Vector2 Position)
        {
            position.X = Position.X * SpriteScale.X;
            position.Y = Position.Y * SpriteScale.Y;
            scale.X = SpriteScale.X;
            scale.Y = SpriteScale.Y;
        }

        public void Update(GameTime gameTime, SpriteBatch spriteBatch, int BossHP)
        {
            if (BossHP < 4 && BossHP > 2)
            {
                texture = OrangeTexture;
            }
            else if(BossHP <= 1)
            {
                texture = RedTexture;
            }
        }

        public void Set_Position(int X, int Y, Vector2 SpriteScale)
        {
            position.X = X;
            position.Y = Y;
        }

        public void Set_Scale(float multiplier, Vector2 SpriteScale)
        {
            scale.X *= multiplier * SpriteScale.X;
            scale.Y *= multiplier * SpriteScale.Y;
        }
    }
}
