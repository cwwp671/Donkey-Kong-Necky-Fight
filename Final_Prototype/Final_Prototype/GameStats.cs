using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    class GameStats
    {
        public bool SaveGameTime;
        public bool LoadGameTime;
        public List<double> times;
        public List<double> loadedSec;
        public List<double> loadedMin;
        public double totalMs;
        public double ms;
        public double sec;
        public double min;
        public string FileName;
        SpriteFont TextStyle;
        Vector2 Scaler;

        public GameStats(ContentManager Content, string FileID, Vector2 SpriteScale)
        {
            SaveGameTime = false;
            LoadGameTime = false;
            times = new List<double>();
            loadedSec = new List<double>();
            loadedMin = new List<double>();
            totalMs = 0;
            ms = 0;
            sec = 0;
            min = 0;
            FileName = FileID;
            TextStyle = Content.Load<SpriteFont>(@"Fonts\TimeText");
            Scaler = SpriteScale;  
        }

        public void DrawTime(GameTime gameTime, SpriteBatch spriteBatch)
        {
            String time = String.Format("{0}:{1}:{2}", min, sec, ms);
            Vector2 timeOrigin = TextStyle.MeasureString(time) * 0.5f;
            spriteBatch.DrawString(TextStyle, time, new Vector2(40 * Scaler.X, 0), Color.White, 0f, Vector2.Zero, Scaler.X, SpriteEffects.None, 0);
        }

        public void getBestTime(GameTime gameTime, SpriteBatch spriteBatch)
        {
            LoadData();
            int index = -1;
            double temp = 0;

            foreach (double time in times)
            {
                if (temp > time || temp == 0)
                {
                    temp = time;
                    index++;
                }
            }

            if (temp == 0)
            {
                return;
            }

            ConvertBest(gameTime, spriteBatch, temp);
        }

        public void SaveTime()
        {
            StreamWriter TextFile = File.AppendText(FileName);
            {
                TextFile.WriteLine(totalMs.ToString());
            }
            TextFile.Close();

            SaveGameTime = true;
        }

        public void GameTimer(GameTime gameTime)
        {
            ms += gameTime.ElapsedGameTime.Milliseconds;
            totalMs += gameTime.ElapsedGameTime.Milliseconds;

            if (ms >= 1000)
            {
                sec++;

                if (sec >= 60)
                {
                    min++;
                    sec = 0;
                }

                ms = 0;
            }
        }

        public void ConvertBest(GameTime gameTime, SpriteBatch spriteBatch, double time)
        {
            double tempMs = time;
            double tempSec = time / 1000f;
            double tempMin = Math.Floor(tempSec / 60f);
            tempSec -= Math.Floor(tempMin * 60f);
            DrawBestTime(gameTime, spriteBatch, tempMin, tempSec);        
        }

        public void DrawBestTime(GameTime gameTime, SpriteBatch spriteBatch, double Min, double Sec)
        {
            String bestTime = String.Format("Best Time: {0}:{1}", Min, Sec);
            spriteBatch.DrawString(TextStyle, bestTime, new Vector2(40 * Scaler.X, 0), Color.White, 0f, Vector2.Zero, Scaler.X, SpriteEffects.None, 0);
        }

        public void convertLoad()
        {
            foreach (double time in times)
            {
                double tempMs = time;
                double tempSec = time / 1000f;
                double tempMin = Math.Floor(tempSec / 60f);
                tempSec -= Math.Floor(tempMin * 60f);
                tempMs -= tempMin * 600000f;
                tempMs -= tempSec * 1000f;
                tempMs *= 0.0001f;
                loadedSec.Add(tempSec + tempMs);
                loadedMin.Add(tempMin);
            }
        }

        public void LoadData()
        {
            if (File.Exists(FileName))
            {
                StreamReader TextFile = File.OpenText(FileName);
                {
                    while (!TextFile.EndOfStream)
                    {
                        double data = double.Parse(TextFile.ReadLine());
                        times.Add(data);
                    }
                }
                TextFile.Close();
            }
        }

    }//end Class
}//end NameSpace
