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
        #region variables
        public Rectangle bounds;

        private bool clicked = false;

        public bool Clicked
        {
            get { return clicked; }
            set { clicked = value; }
        }
        private bool pressed = false;

        public bool Pressed
        {
            get { return pressed; }
            set { pressed = value; }
        }
        #endregion

        //Constructor for the button
        public Button(Vector2 position,string content,SpriteFont font,Color fontColor,Texture2D texture,Vector2 size,bool fill):base(position,content,font,fontColor,texture,size,fill)
        {
            
        }

        public Button()
        {
        
        }

        public override void Update(float deltaTime)
        {
            if (Mouse.GetState().X > position.X && Mouse.GetState().X < size.X + position.X && Mouse.GetState().Y > position.Y && Mouse.GetState().Y < size.Y + position.Y)
            {
                Hover();
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    MouseDown();
                    pressed = true;
                }

                if (Mouse.GetState().LeftButton == ButtonState.Released && pressed)
                {
                    pressed = false;
                    clicked = true;
                }
            }
            else
            {
                Normal();
                pressed = false;
                clicked = false;
            }

            base.Update(deltaTime);
        }

        //Adds an effect when you hover over a button
        public void Hover()
        {
            backGroundColor = Color.LightGray;
            //content = "Hover: P:" + pressed + " c:" + clicked;
        }

        //Normal effect for the button
        public void Normal()
        {
            backGroundColor = Color.White;
            //content = "Normal: P:"+pressed+" c:"+clicked;
        }

        //adds an effect when you press a button
        public void MouseDown()
        {
            backGroundColor = Color.Gray;
            //content = "Pressed: P:"+pressed+" c:"+clicked;
        }

    }
}
