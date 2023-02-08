/*
 * Author: Connor Pandolph
 * Game: Necky's Revenge
 * Framework: Microsoft XNA
 * Date: 2013
 */
 
using System;

namespace Final_Prototype
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
#endif
}

