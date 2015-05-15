using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjektS2015
{
    public class TextBox : GameObject
    {
        public string content = "Default";

        public SpriteFont font;

        public Color fontColor = Color.White;
        public Color backGroundColor = Color.White;

        public Vector2 size = Vector2.Zero;
        public Texture2D texture;



        public TextBox(Vector2 position,string content,SpriteFont font,Color fontColor,Texture2D texture,Vector2 size):base(position)
        {
            this.content = content;
            this.font = font;
            this.fontColor = fontColor;
            this.size = size;
            this.texture = texture;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
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
                    sb.Draw(texture, new Rectangle((int)position.X+i,(int)position.Y+ j, 1, 1),backGroundColor);
                }
            }

            sb.DrawString(font, content, position+new Vector2((int)size.X/2-font.MeasureString(content).X/2,0), fontColor);
            //base.Draw(sb);
        }
    }
}
