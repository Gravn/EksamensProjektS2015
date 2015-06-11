using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace EksamensProjektS2015
{
    class TimeLine : GameObject
    {
        #region variables
        public string[] dates = new string[35];
        public float posY;
        
        public float speed = 0;

        public DateTime startDate = DateTime.Parse("25/05/2015");
        public DateTime currentDate;
        public DateTime targetDate;
        public DateTime[] events = new DateTime[8]
        {
          DateTime.Parse("25/05/2015"),//KopiMaskinen
          DateTime.Parse("14/07/2015"),//Stolen
          DateTime.Parse("05/08/2015"),//Museskader
          DateTime.Parse("28/08/2015"),//LønForhandling
          DateTime.Parse("12/09/2015"),//Temperatur
          DateTime.Parse("17/10/2015"),//Skadedyr
          DateTime.Parse("20/11/2015"),//Skadedyr
          DateTime.Parse("18/12/2015") //LønForhandling
        };

        public int currentDay = 0;
        public int targetDay;
        public float difference;
        public float distance;

        public bool active = true;
        public float waitTimer = 0;
        #endregion

        //Timeline contructor
        public TimeLine(Vector2 position)
            : base(position)
        {
            currentDate = startDate;
            //currentDay = startDate.DayOfYear;
            targetDate = events[0];
        }

        //Jumps to a new event on the timeline
        public void NewEvent(int i)
        {
            if (i < events.Length)
            {
                targetDate = events[i - 1];
            }
        }

        public override void Update(float deltaTime)
        {
            if (currentDate >= targetDate)
            {
                currentDate = targetDate;
            }

            if (targetDate.Year.Equals(currentDate.Year))
            {
                difference = (int.Parse(targetDate.DayOfYear.ToString()) - int.Parse(currentDate.DayOfYear.ToString()));
            }
            else
            {
                int left = int.Parse(DateTime.Parse(string.Format("31/12/{0:yyyy}", currentDate)).DayOfYear.ToString()) - int.Parse(currentDate.DayOfYear.ToString()) + 1;
                difference = left+int.Parse(targetDate.DayOfYear.ToString()) - int.Parse(DateTime.Parse(string.Format("01/01/{0:yyyy}",targetDate)).DayOfYear.ToString());
            }

            distance = difference*22;

            speed = distance * 0.95f + 80;

            if (difference<= 0)
            {
                speed = 0;
            }

            posY -= speed * deltaTime;

            if (posY <= -25)
            {
                currentDay++;
                currentDate = startDate.AddDays(currentDay);
                posY += 22;
            }
            for (int i = 0; i < 35; i++)
            {
                dates[i] = string.Format("{0:dd}/{0:MM}/{0:yyyy}",  currentDate.AddDays(i-7)); //DateTime.Today.AddDays(i).ToString();

                for (int j = 0; j < events.Length ; j++)
                {
                    if (events[j].ToString().Equals(dates[i]))
                    {
                        dates[i] = "(+) " + dates[i];
                    }
                }
            }
                base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < 35; i++)
            {
                sb.DrawString(GameManager.ErasMediumITC14, ""+dates[i], new Vector2(1140,3+ 23* i+posY)+position, Color.White);
            }

            //Current Day
            //sb.DrawString(GameManager.ErasMediumITC14, "CurrentDay: " + currentDay + "|" + targetDay, new Vector2(0, 23 * 1), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, string.Format("Target: {0:dd}/{0:MM}/{0:yyyy}", targetDate), new Vector2(0, 23 * 2), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, string.Format("CurrentDate: {0:dd}/{0:MM}/{0:yyyy}", currentDate), new Vector2(0, 23 * 3), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, "Difference: " + difference, new Vector2(0, 23 * 4), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, "Distance: " + distance, new Vector2(0, 23 * 5), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, "Speed: " + speed, new Vector2(0, 23 * 6), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, "Year?: " + (targetDate.Year.Equals(currentDate.Year)), new Vector2(0, 23 * 7), Color.White);
            //sb.DrawString(GameManager.ErasMediumITC14, "Left: " + (int.Parse(DateTime.Parse(string.Format("31/12/{0:yyyy}", currentDate)).DayOfYear.ToString()) - int.Parse(currentDate.DayOfYear.ToString())), new Vector2(0, 23 * 8), Color.White);   
            base.Draw(sb);
        }
    }
}
