using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjektS2015
{
    class TextBox : GameObject
    {
        public string content = "Default";

        public SpriteFont font;

        public Color fontColor = Color.White;
        public Color backGroundColor;

        public Vector2 size = Vector2.Zero;
        public Texture2D texture;

        public TextBox(Vector2 position,string content,SpriteFont font,Color fontColor,Texture2D texture,Vector2 size):base(position)
        {
            this.content = content;
            this.font = font;
            this.fontColor = fontColor;
            this.backGroundColor = backGroundColor;
            this.size = size;
            this.texture = texture;

        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            position += new Vector2(1, 0);
        }

        public void FadeOut()
        { 
            
        }

        public void FadeIn()
        { 
            
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < size.X; i++)
            {
                for (int j = 0; j < size.Y; j++)
                {
                    sb.Draw(texture, new Rectangle((int)position.X+i,(int)position.Y+ j, 1, 1), Color.White);
                }
            }

            sb.DrawString(font, content, position, fontColor);
            //base.Draw(sb);
        }
    }
}
