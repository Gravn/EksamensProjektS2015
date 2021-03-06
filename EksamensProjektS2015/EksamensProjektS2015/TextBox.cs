﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjektS2015
{
    public class TextBox : GameObject
    {

        #region variables
        private string content = "Default";

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public bool visible = true;
        public int allignment = 1;
        public SpriteFont font;

        public Color fontColor = Color.White;
        public Color backGroundColor = Color.White;

        public bool fill = true;
        public Vector2 size = Vector2.Zero;
        public Texture2D texture;
        #endregion

        //Constructor for the textbox
        public TextBox(Vector2 position,string content, SpriteFont font, Color fontColor, Texture2D texture, Vector2 size, bool fill) : base(position)
        {
            this.content = content;
            this.font = font;
            this.fontColor = fontColor;
            this.size = size;
            this.texture = texture;
            this.fill = fill;
        }

        //Constructor for the textbox, with text alignment
        public TextBox(Vector2 position, string content, SpriteFont font, Color fontColor, int allignment, Texture2D texture, Vector2 size, bool fill) : base(position)
        {
            this.content = content;
            this.font = font;
            this.fontColor = fontColor;
            this.allignment = allignment;
            this.size = size;
            this.texture = texture;
            this.fill = fill;
        }
        /// <summary>
        /// Empty constructor for testing
        /// </summary>
        public TextBox()
        {
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
            if (visible && content != null)
            {
                if (fill)
                {
                    for (int i = 0; i < size.X; i++)
                    {
                        for (int j = 0; j < size.Y; j++)
                        {
                            sb.Draw(texture, new Rectangle((int)position.X + i, (int)position.Y + j, 1, 1), backGroundColor);
                        }
                    }
                }
                else
                {
                    if (texture != null)
                    {
                        sb.Draw(texture, new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height), backGroundColor);
                    }
                }

                if (allignment == 0)//Left
                { 
                    sb.DrawString(font, content, position + new Vector2(0,(int)size.Y / 2 - font.MeasureString(content).Y / 2 + 2), fontColor, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0); 
                }

                if (allignment == 1)//Center
                {
                    sb.DrawString(font, content, position + new Vector2((int)size.X / 2 - font.MeasureString(content).X / 2 + 1, (int)size.Y / 2 - font.MeasureString(content).Y / 2 + 2), fontColor, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                }

                if (allignment == 2)//Right
                { 
                    sb.DrawString(font, content, position + new Vector2((int)size.X - font.MeasureString(content).X, (int)size.Y / 2 - font.MeasureString(content).Y / 2 + 2), fontColor, 0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0);
                }
            }
            //base.Draw(sb);
        }
    }
}
