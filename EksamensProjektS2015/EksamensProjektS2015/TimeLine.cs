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

        public TimeLine(Vector2 position)
            : base(position)
        {

        }

        public override void Update(float deltaTime)
        {
            switch (GameManager.dayCounter)
            {
                case 0:
                    this.position.Y = 40;
                    break;
                case 1:
                    this.position.Y = 80;
                    break;
                case 2:
                    this.position.Y = 120;
                    break;
                case 3:
                    this.position.Y = 160;
                    break;
                case 4:
                    this.position.Y = 200;
                    break;
                case 5:
                    this.position.Y = 240;
                    break;
                case 6:
                    this.position.Y = 280;
                    break;
                default:
                    break;
            }

            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {

            base.Draw(sb);
        }

    }
}
