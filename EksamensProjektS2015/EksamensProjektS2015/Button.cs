using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace EksamensProjektS2015
{
    public class Button : TextBox
    {
        public Rectangle bounds;

        public Button(Vector2 position,string content,SpriteFont font,Color fontColor,Texture2D texture,Vector2 size):base(position,content,font,fontColor,texture,size)
        {
            
        }

        public bool clicked = false;

        public override void Update(float deltaTime)
        {
            if (Mouse.GetState().X > position.X && Mouse.GetState().X < size.X + position.X && Mouse.GetState().Y > position.Y && Mouse.GetState().Y < size.Y + position.Y)
            {
                Hover();
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    MouseDown();
                    clicked = true;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Released && clicked)
                {
                    clicked = true;
                }
            }
            else
            {
                Normal();
                clicked = false;
            }

            base.Update(deltaTime);
        }

        public void Hover()
        {
            backGroundColor = Color.LightGray;
        }

        public void Normal()
        {
            backGroundColor = Color.White;
        }

        public void MouseDown()
        {
            backGroundColor = Color.Gray;
        }

        public bool Click()
        {
            return true;
        }

    }
}
