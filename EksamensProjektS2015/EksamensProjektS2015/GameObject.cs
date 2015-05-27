using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace EksamensProjektS2015
{
    public abstract class GameObject
    {
        protected Vector2 position;

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public GameObject(Vector2 position)
        {
            this.position = position;   
        }

        public virtual void Update(float deltaTime)
        { 
               
        }

        public virtual void Draw(SpriteBatch sb)
        { 
            
        }

        public void Destroy(GameObject obj)
        {
            GameManager.GameObjects.Remove(obj);   
        }

        public GameObject Clone()
        {
            GameObject clone = (GameObject)this.MemberwiseClone();
            return clone; 
        }
    }
}
