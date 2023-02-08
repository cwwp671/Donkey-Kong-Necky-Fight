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
    class DiddyKong : Player
    {
        SoundEffect Hurt;

        public DiddyKong(ContentManager Content, Vector2 SpriteScale, string ImageFile)
            : base(Content, ImageFile, SpriteScale, new Rectangle(0, 0, 82, 80), 75)
        {
            walkImage = @"Images\Diddy_Walk";
            walkTexture = Content.Load<Texture2D>(@"Images\Diddy_Walk");
            sprintTexture = Content.Load<Texture2D>(@"Images\Diddy_Sprint");
            jumpTexture = Content.Load<Texture2D>(@"Images\Diddy_Jump");
            winTexture = Content.Load<Texture2D>(@"Images\Diddy_Win");
            controlOne = false;
            SetAnimationLists();
            PlayerActor.sourceRectangle = new Rectangle(0, 0, 82, 80);
            PlayerActor.scale.X = SpriteScale.X;
            PlayerActor.scale.Y = SpriteScale.Y;
            Hurt = Content.Load<SoundEffect>(@"Sound\DiddyHurt");
        }

        public void HurtSound()
        {
            Hurt.Play();
        }

        public void SetAnimationLists()
        {
            int width = 82;
            int height = 80;
            walkListR.Add(new Rectangle(0, 0, width, height));    //00
            walkListR.Add(new Rectangle(82, 0, width, height));   //01
            walkListR.Add(new Rectangle(164, 0, width, height));  //02
            walkListR.Add(new Rectangle(246, 0, width, height));  //03
            walkListR.Add(new Rectangle(328, 0, width, height));  //04
            walkListR.Add(new Rectangle(410, 0, width, height));  //05
            walkListR.Add(new Rectangle(492, 0, width, height));  //06
            walkListR.Add(new Rectangle(574, 0, width, height));  //07
            walkListR.Add(new Rectangle(656, 0, width, height));  //08

            walkListL.Add(new Rectangle(0, height, width, height));   //10
            walkListL.Add(new Rectangle(82, height, width, height));  //11
            walkListL.Add(new Rectangle(164, height, width, height)); //12
            walkListL.Add(new Rectangle(246, height, width, height)); //13
            walkListL.Add(new Rectangle(328, height, width, height)); //14
            walkListL.Add(new Rectangle(410, height, width, height)); //15
            walkListL.Add(new Rectangle(492, height, width, height)); //16
            walkListL.Add(new Rectangle(574, height, width, height)); //17
            walkListL.Add(new Rectangle(656, height, width, height)); //18

            int sprintWidth = 92;
            int sprintHeight = 90;
            int sprintRowHeight = sprintHeight * 2;
            sprintListR.Add(new Rectangle(0, 0, sprintWidth, sprintHeight));    //00
            sprintListR.Add(new Rectangle(92, 0, sprintWidth, sprintHeight));   //01
            sprintListR.Add(new Rectangle(184, 0, sprintWidth, sprintHeight));  //02
            sprintListR.Add(new Rectangle(276, 0, sprintWidth, sprintHeight));  //03
            sprintListR.Add(new Rectangle(368, 0, sprintWidth, sprintHeight));  //04
            sprintListR.Add(new Rectangle(460, 0, sprintWidth, sprintHeight));  //05
            sprintListR.Add(new Rectangle(552, 0, sprintWidth, sprintHeight));  //06
            sprintListR.Add(new Rectangle(644, 0, sprintWidth, sprintHeight));  //07
            sprintListR.Add(new Rectangle(736, 0, sprintWidth, sprintHeight));  //08
            sprintListR.Add(new Rectangle(0, sprintHeight, sprintWidth, sprintHeight));  //09
            sprintListR.Add(new Rectangle(92, sprintHeight, sprintWidth, sprintHeight));    //10
            sprintListR.Add(new Rectangle(184, sprintHeight, sprintWidth, sprintHeight));   //11

            sprintListL.Add(new Rectangle(276, sprintHeight, sprintWidth, sprintHeight));  //14
            sprintListL.Add(new Rectangle(368, sprintHeight, sprintWidth, sprintHeight));  //15
            sprintListL.Add(new Rectangle(460, sprintHeight, sprintWidth, sprintHeight));  //16
            sprintListL.Add(new Rectangle(552, sprintHeight, sprintWidth, sprintHeight));  //17
            sprintListL.Add(new Rectangle(644, sprintHeight, sprintWidth, sprintHeight));  //18
            sprintListL.Add(new Rectangle(736, sprintHeight, sprintWidth, sprintHeight));  //19
            sprintListL.Add(new Rectangle(0, sprintRowHeight, sprintWidth, sprintHeight));   //20
            sprintListL.Add(new Rectangle(92, sprintRowHeight, sprintWidth, sprintHeight));  //21
            sprintListL.Add(new Rectangle(184, sprintRowHeight, sprintWidth, sprintHeight)); //22
            sprintListL.Add(new Rectangle(276, sprintRowHeight, sprintWidth, sprintHeight)); //23
            sprintListL.Add(new Rectangle(368, sprintRowHeight, sprintWidth, sprintHeight)); //24
            sprintListL.Add(new Rectangle(460, sprintRowHeight, sprintWidth, sprintHeight)); //25

            int jumpWidth = 92;
            int jumpHeight = 118;
            int jumpRowHeight = jumpHeight * 2;
            jumpListR.Add(new Rectangle(0, 0, jumpWidth, jumpHeight));   //00
            jumpListR.Add(new Rectangle(92, 0, jumpWidth, jumpHeight));  //01
            jumpListR.Add(new Rectangle(184, 0, jumpWidth, jumpHeight)); //02
            jumpListR.Add(new Rectangle(276, 0, jumpWidth, jumpHeight)); //03
            jumpListR.Add(new Rectangle(368, 0, jumpWidth, jumpHeight)); //04
            jumpListR.Add(new Rectangle(460, 0, jumpWidth, jumpHeight)); //05
            jumpListR.Add(new Rectangle(552, 0, jumpWidth, jumpHeight)); //06
            jumpListR.Add(new Rectangle(644, 0, jumpWidth, jumpHeight)); //07
            jumpListR.Add(new Rectangle(736, 0, jumpWidth, jumpHeight)); //08
            jumpListR.Add(new Rectangle(0, jumpHeight, jumpWidth, jumpHeight)); //10

            jumpListL.Add(new Rectangle(92, jumpHeight, jumpWidth, jumpHeight)); //11
            jumpListL.Add(new Rectangle(184, jumpHeight, jumpWidth, jumpHeight));   //12
            jumpListL.Add(new Rectangle(276, jumpHeight, jumpWidth, jumpHeight));  //13
            jumpListL.Add(new Rectangle(368, jumpHeight, jumpWidth, jumpHeight));  //14
            jumpListL.Add(new Rectangle(460, jumpHeight, jumpWidth, jumpHeight)); //15
            jumpListL.Add(new Rectangle(552, jumpHeight, jumpWidth, jumpHeight)); //16
            jumpListL.Add(new Rectangle(644, jumpHeight, jumpWidth, jumpHeight)); //17
            jumpListL.Add(new Rectangle(736, jumpHeight, jumpWidth, jumpHeight)); //18
            jumpListL.Add(new Rectangle(0, jumpRowHeight, jumpWidth, jumpHeight)); //20
            jumpListL.Add(new Rectangle(92, jumpRowHeight, jumpWidth, jumpHeight)); //21

            int winWidth = 128;
            int winHeight = 344;
            //first hat throw
            winList.Add(new Rectangle(0, 0, winWidth, winHeight)); //00
            winList.Add(new Rectangle(64 * 2, 0, winWidth, winHeight)); //01
            winList.Add(new Rectangle(128 * 2, 0, winWidth, winHeight)); //02
            winList.Add(new Rectangle(192 * 2, 0, winWidth, winHeight)); //03
            winList.Add(new Rectangle(256 * 2, 0, winWidth, winHeight)); //04
            winList.Add(new Rectangle(320 * 2, 0, winWidth, winHeight)); //05
            winList.Add(new Rectangle(384 * 2, 0, winWidth, winHeight)); //06
            winList.Add(new Rectangle(448 * 2, 0, winWidth, winHeight)); //07
            winList.Add(new Rectangle(512 * 2, 0, winWidth, winHeight)); //08
            winList.Add(new Rectangle(0, 172 * 2, winWidth, winHeight)); //10
            winList.Add(new Rectangle(64 * 2, 172 * 2, winWidth, winHeight)); //11
            winList.Add(new Rectangle(128 * 2, 172 * 2, winWidth, winHeight)); //12
            winList.Add(new Rectangle(192 * 2, 172 * 2, winWidth, winHeight)); //13
            winList.Add(new Rectangle(256 * 2, 172 * 2, winWidth, winHeight)); //14
            winList.Add(new Rectangle(320 * 2, 172 * 2, winWidth, winHeight)); //15
            winList.Add(new Rectangle(384 * 2, 172 * 2, winWidth, winHeight)); //16
            winList.Add(new Rectangle(448 * 2, 172 * 2, winWidth, winHeight)); //17
            winList.Add(new Rectangle(512 * 2, 172 * 2, winWidth, winHeight)); //18
            winList.Add(new Rectangle(0, 344 * 2, winWidth, winHeight)); //20
            winList.Add(new Rectangle(64 * 2, 344 * 2, winWidth, winHeight)); //21
            winList.Add(new Rectangle(128 * 2, 344 * 2, winWidth, winHeight)); //22
            winList.Add(new Rectangle(192 * 2, 344 * 2, winWidth, winHeight)); //23
            winList.Add(new Rectangle(256 * 2, 344 * 2, winWidth, winHeight)); //24
            winList.Add(new Rectangle(320 * 2, 344 * 2, winWidth, winHeight)); //25
            winList.Add(new Rectangle(384 * 2, 344 * 2, winWidth, winHeight)); //26
            winList.Add(new Rectangle(448 * 2, 344 * 2, winWidth, winHeight)); //27
            winList.Add(new Rectangle(512 * 2, 344 * 2, winWidth, winHeight)); //28
            winList.Add(new Rectangle(0, 516 * 2, winWidth, winHeight)); //20
            winList.Add(new Rectangle(64 * 2, 516 * 2, winWidth, winHeight)); //21
            winList.Add(new Rectangle(128 * 2, 516 * 2, winWidth, winHeight)); //22
            winList.Add(new Rectangle(192 * 2, 516 * 2, winWidth, winHeight)); //23
            winList.Add(new Rectangle(256 * 2, 516 * 2, winWidth, winHeight)); //24
            winList.Add(new Rectangle(320 * 2, 516 * 2, winWidth, winHeight)); //25
            //second hat throw
            winList.Add(new Rectangle(320 * 2, 0, winWidth, winHeight)); //05
            winList.Add(new Rectangle(384 * 2, 0, winWidth, winHeight)); //06
            winList.Add(new Rectangle(448 * 2, 0, winWidth, winHeight)); //07
            winList.Add(new Rectangle(512 * 2, 0, winWidth, winHeight)); //08
            winList.Add(new Rectangle(0, 172 * 2, winWidth, winHeight)); //10
            winList.Add(new Rectangle(64 * 2, 172 * 2, winWidth, winHeight)); //11
            winList.Add(new Rectangle(128 * 2, 172 * 2, winWidth, winHeight)); //12
            winList.Add(new Rectangle(192 * 2, 172 * 2, winWidth, winHeight)); //13
            winList.Add(new Rectangle(256 * 2, 172 * 2, winWidth, winHeight)); //14
            winList.Add(new Rectangle(320 * 2, 172 * 2, winWidth, winHeight)); //15
            winList.Add(new Rectangle(384 * 2, 172 * 2, winWidth, winHeight)); //16
            winList.Add(new Rectangle(448 * 2, 172 * 2, winWidth, winHeight)); //17
            winList.Add(new Rectangle(512 * 2, 172 * 2, winWidth, winHeight)); //18
            winList.Add(new Rectangle(0, 344 * 2, winWidth, winHeight)); //20
            winList.Add(new Rectangle(64 * 2, 344 * 2, winWidth, winHeight)); //21
            winList.Add(new Rectangle(128 * 2, 344 * 2, winWidth, winHeight)); //22
            winList.Add(new Rectangle(192 * 2, 344 * 2, winWidth, winHeight)); //23
            winList.Add(new Rectangle(256 * 2, 344 * 2, winWidth, winHeight)); //24
            winList.Add(new Rectangle(320 * 2, 344 * 2, winWidth, winHeight)); //25
            winList.Add(new Rectangle(384 * 2, 344 * 2, winWidth, winHeight)); //26
            winList.Add(new Rectangle(448 * 2, 344 * 2, winWidth, winHeight)); //27
            winList.Add(new Rectangle(512 * 2, 344 * 2, winWidth, winHeight)); //28
            winList.Add(new Rectangle(0, 516 * 2, winWidth, winHeight)); //20
            winList.Add(new Rectangle(64 * 2, 516 * 2, winWidth, winHeight)); //21
            winList.Add(new Rectangle(128 * 2, 516 * 2, winWidth, winHeight)); //22
            winList.Add(new Rectangle(192 * 2, 516 * 2, winWidth, winHeight)); //23
            winList.Add(new Rectangle(256 * 2, 516 * 2, winWidth, winHeight)); //24
            winList.Add(new Rectangle(320 * 2, 516 * 2, winWidth, winHeight)); //25
            //third hat throw
            winList.Add(new Rectangle(320 * 2, 0, winWidth, winHeight)); //05
            winList.Add(new Rectangle(384 * 2, 0, winWidth, winHeight)); //06
            winList.Add(new Rectangle(448 * 2, 0, winWidth, winHeight)); //07
            winList.Add(new Rectangle(512 * 2, 0, winWidth, winHeight)); //08
            winList.Add(new Rectangle(0, 172 * 2, winWidth, winHeight)); //10
            winList.Add(new Rectangle(64 * 2, 172 * 2, winWidth, winHeight)); //11
            winList.Add(new Rectangle(128 * 2, 172 * 2, winWidth, winHeight)); //12
            winList.Add(new Rectangle(192 * 2, 172 * 2, winWidth, winHeight)); //13
            winList.Add(new Rectangle(256 * 2, 172 * 2, winWidth, winHeight)); //14
            winList.Add(new Rectangle(320 * 2, 172 * 2, winWidth, winHeight)); //15
            winList.Add(new Rectangle(384 * 2, 172 * 2, winWidth, winHeight)); //16
            winList.Add(new Rectangle(448 * 2, 172 * 2, winWidth, winHeight)); //17
            winList.Add(new Rectangle(512 * 2, 172 * 2, winWidth, winHeight)); //18
            winList.Add(new Rectangle(0, 344 * 2, winWidth, winHeight)); //20
            winList.Add(new Rectangle(64 * 2, 344 * 2, winWidth, winHeight)); //21
            winList.Add(new Rectangle(128 * 2, 344 * 2, winWidth, winHeight)); //22
            winList.Add(new Rectangle(192 * 2, 344 * 2, winWidth, winHeight)); //23
            winList.Add(new Rectangle(256 * 2, 344 * 2, winWidth, winHeight)); //24
            winList.Add(new Rectangle(320 * 2, 344 * 2, winWidth, winHeight)); //25
            winList.Add(new Rectangle(384 * 2, 344 * 2, winWidth, winHeight)); //26
            winList.Add(new Rectangle(448 * 2, 344 * 2, winWidth, winHeight)); //27
            winList.Add(new Rectangle(512 * 2, 344 * 2, winWidth, winHeight)); //28
            winList.Add(new Rectangle(0, 516 * 2, winWidth, winHeight)); //20
            winList.Add(new Rectangle(64 * 2, 516 * 2, winWidth, winHeight)); //21
            winList.Add(new Rectangle(128 * 2, 516 * 2, winWidth, winHeight)); //22
            winList.Add(new Rectangle(192 * 2, 516 * 2, winWidth, winHeight)); //23
            winList.Add(new Rectangle(256 * 2, 516 * 2, winWidth, winHeight)); //24
            winList.Add(new Rectangle(320 * 2, 516 * 2, winWidth, winHeight)); //25
            //putting hat back on
            winList.Add(new Rectangle(256 * 2, 0, winWidth, winHeight)); //04
            winList.Add(new Rectangle(192 * 2, 0, winWidth, winHeight)); //03
            winList.Add(new Rectangle(128 * 2, 0, winWidth, winHeight)); //02
            winList.Add(new Rectangle(64 * 2, 0, winWidth, winHeight)); //01
            winList.Add(new Rectangle(0, 0, winWidth, winHeight)); //00
        }
    }
}
