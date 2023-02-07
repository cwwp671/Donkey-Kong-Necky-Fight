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
    class DonkeyKong : Player
    {
        SoundEffect Hurt;

        public DonkeyKong(ContentManager Content, Vector2 SpriteScale, string ImageFile)
            : base(Content, ImageFile, SpriteScale, new Rectangle(0, 0, 46, 41), 50)
        {
            walkImage = @"Images\DK_Walk";
            walkTexture = Content.Load<Texture2D>(@"Images\DK_Walk");
            sprintTexture = Content.Load<Texture2D>(@"Images\DK_Sprint");
            jumpTexture = Content.Load<Texture2D>(@"Images\DK_Jump");
            controlOne = true;
            SetAnimationLists();
            PlayerActor.scale.X = SpriteScale.X * 2f;
            PlayerActor.scale.Y = SpriteScale.Y * 2f;
            PlayerActor.sourceRectangle = new Rectangle(0, 0, 46, 41);
            Hurt = Content.Load<SoundEffect>(@"Sound\DKHurt");
        }

        public void HurtSound()
        {
            Hurt.Play();
        }

        public void SetAnimationLists()
        {           
            walkListL.Add(new Rectangle(0, 0, 46, 41));    //00
            walkListL.Add(new Rectangle(46, 0, 46, 41));   //01
            walkListL.Add(new Rectangle(92, 0, 46, 41));   //02
            walkListL.Add(new Rectangle(138, 0, 46, 41));  //03
            walkListL.Add(new Rectangle(184, 0, 46, 41));  //04
            walkListL.Add(new Rectangle(230, 0, 46, 41));  //05
            walkListL.Add(new Rectangle(276, 0, 46, 41));  //06
            walkListL.Add(new Rectangle(322, 0, 46, 41));  //07
            walkListL.Add(new Rectangle(368, 0, 46, 41));  //08
            walkListL.Add(new Rectangle(414, 0, 46, 41));  //09
            walkListL.Add(new Rectangle(0, 41, 46, 41));    //10
            walkListL.Add(new Rectangle(46, 41, 46, 41));   //11
            walkListL.Add(new Rectangle(92, 41, 46, 41));   //12
            walkListL.Add(new Rectangle(138, 41, 46, 41));  //13
            walkListL.Add(new Rectangle(184, 41, 46, 41));  //14
            walkListL.Add(new Rectangle(230, 41, 46, 41));  //15
            walkListL.Add(new Rectangle(276, 41, 46, 41));  //16
            walkListL.Add(new Rectangle(322, 41, 46, 41));  //17

            walkListR.Add(new Rectangle(368, 41, 46, 41));  //18
            walkListR.Add(new Rectangle(414, 41, 46, 41));  //19
            walkListR.Add(new Rectangle(0, 82, 46, 41));   //20
            walkListR.Add(new Rectangle(46, 82, 46, 41));  //21
            walkListR.Add(new Rectangle(92, 82, 46, 41));  //22
            walkListR.Add(new Rectangle(138, 82, 46, 41)); //23
            walkListR.Add(new Rectangle(184, 82, 46, 41)); //24
            walkListR.Add(new Rectangle(230, 82, 46, 41)); //25
            walkListR.Add(new Rectangle(276, 82, 46, 41)); //26
            walkListR.Add(new Rectangle(322, 82, 46, 41)); //27
            walkListR.Add(new Rectangle(368, 82, 46, 41)); //28
            walkListR.Add(new Rectangle(414, 82, 46, 41)); //29
            walkListR.Add(new Rectangle(0, 123, 46, 41));   //30
            walkListR.Add(new Rectangle(46, 123, 46, 41));  //31
            walkListR.Add(new Rectangle(92, 123, 46, 41));  //32
            walkListR.Add(new Rectangle(138, 123, 46, 41)); //33
            walkListR.Add(new Rectangle(184, 123, 46, 41)); //34
            walkListR.Add(new Rectangle(230, 123, 46, 41)); //35

            sprintListL.Add(new Rectangle(0, 0, 72, 48));    //00
            sprintListL.Add(new Rectangle(72, 0, 72, 48));   //01
            sprintListL.Add(new Rectangle(144, 0, 72, 48));  //02
            sprintListL.Add(new Rectangle(216, 0, 72, 48));  //03
            sprintListL.Add(new Rectangle(288, 0, 72, 48));  //04
            sprintListL.Add(new Rectangle(360, 0, 72, 48));  //05
            sprintListL.Add(new Rectangle(432, 0, 72, 48));  //06
            sprintListL.Add(new Rectangle(504, 0, 72, 48));  //07
            sprintListL.Add(new Rectangle(576, 0, 72, 48));  //08
            sprintListL.Add(new Rectangle(648, 0, 72, 48));  //09
            sprintListL.Add(new Rectangle(0, 48, 72, 48));    //10
            sprintListL.Add(new Rectangle(72, 48, 72, 48));   //11
            sprintListL.Add(new Rectangle(144, 48, 72, 48));  //12
            sprintListL.Add(new Rectangle(216, 48, 72, 48));  //13

            sprintListR.Add(new Rectangle(288, 48, 72, 48));  //14
            sprintListR.Add(new Rectangle(360, 48, 72, 48));  //15
            sprintListR.Add(new Rectangle(432, 48, 72, 48));  //16
            sprintListR.Add(new Rectangle(504, 48, 72, 48));  //17
            sprintListR.Add(new Rectangle(576, 48, 72, 48));  //18
            sprintListR.Add(new Rectangle(648, 48, 72, 48));  //19
            sprintListR.Add(new Rectangle(0, 96, 72, 48));   //20
            sprintListR.Add(new Rectangle(72, 96, 72, 48));  //21
            sprintListR.Add(new Rectangle(144, 96, 72, 48)); //22
            sprintListR.Add(new Rectangle(216, 96, 72, 48)); //23
            sprintListR.Add(new Rectangle(288, 96, 72, 48)); //24
            sprintListR.Add(new Rectangle(360, 96, 72, 48)); //25
            sprintListR.Add(new Rectangle(432, 96, 72, 48)); //26
            sprintListR.Add(new Rectangle(504, 96, 72, 48)); //27

            jumpListL.Add(new Rectangle(0, 0, 45, 73));   //00
            jumpListL.Add(new Rectangle(45, 0, 45, 73));  //01
            jumpListL.Add(new Rectangle(90, 0, 45, 73));  //02
            jumpListL.Add(new Rectangle(135, 0, 45, 73)); //03
            jumpListL.Add(new Rectangle(180, 0, 45, 73)); //04
            jumpListL.Add(new Rectangle(225, 0, 45, 73)); //05
            jumpListL.Add(new Rectangle(270, 0, 45, 73)); //06
            jumpListL.Add(new Rectangle(315, 0, 45, 73)); //07
            jumpListL.Add(new Rectangle(360, 0, 45, 73)); //08
            jumpListL.Add(new Rectangle(405, 0, 45, 73)); //09
            jumpListL.Add(new Rectangle(0, 73, 45, 73));   //10
            jumpListL.Add(new Rectangle(45, 73, 45, 73));  //11
            jumpListL.Add(new Rectangle(90, 73, 45, 73));  //12
            jumpListL.Add(new Rectangle(135, 73, 45, 73)); //13
            jumpListL.Add(new Rectangle(180, 73, 45, 73)); //14
            jumpListL.Add(new Rectangle(225, 73, 45, 73)); //15
            jumpListL.Add(new Rectangle(270, 73, 45, 73)); //16
            jumpListL.Add(new Rectangle(315, 73, 45, 73)); //17
            jumpListL.Add(new Rectangle(360, 73, 45, 73)); //18

            jumpListR.Add(new Rectangle(405, 73, 45, 73)); //19
            jumpListR.Add(new Rectangle(0, 146, 45, 73));   //20
            jumpListR.Add(new Rectangle(45, 146, 45, 73));  //21
            jumpListR.Add(new Rectangle(90, 146, 45, 73));  //22
            jumpListR.Add(new Rectangle(135, 146, 45, 73)); //23
            jumpListR.Add(new Rectangle(180, 146, 45, 73)); //24
            jumpListR.Add(new Rectangle(225, 146, 45, 73)); //25
            jumpListR.Add(new Rectangle(270, 146, 45, 73)); //26
            jumpListR.Add(new Rectangle(315, 146, 45, 73)); //27
            jumpListR.Add(new Rectangle(360, 146, 45, 73)); //28
            jumpListR.Add(new Rectangle(405, 146, 45, 73)); //29
            jumpListR.Add(new Rectangle(0, 219, 45, 73));    //30
            jumpListR.Add(new Rectangle(45, 219, 45, 73));   //31
            jumpListR.Add(new Rectangle(90, 219, 45, 73));   //32
            jumpListR.Add(new Rectangle(135, 219, 45, 73));  //33
            jumpListR.Add(new Rectangle(180, 219, 45, 73));  //34
            jumpListR.Add(new Rectangle(225, 219, 45, 73));  //35
            jumpListR.Add(new Rectangle(270, 219, 45, 73));  //36
            jumpListR.Add(new Rectangle(315, 219, 45, 73));  //37
        }
    }
}
