﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

namespace EksamensProjektS2015
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GameManager : Game
    {
        public int menuState = 0;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static SpriteFont ArialNarrow48;
        public Texture2D red1;
        public TextBox[] texts = new TextBox[10];
        public Button[] buttons = new Button[10];
        public static List<GameObject> gameObjects = new List<GameObject>();

        public string name = "";

        private static List<GameObject> GameObjects
        {
            get { return gameObjects; }
            set { gameObjects = value; }
        }

        public GameManager()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            Content.RootDirectory = "Content";

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            IsMouseVisible = true;
            texts[0] = new TextBox(new Vector2(100, 100), "Navn:", ArialNarrow48, Color.White,red1,new Vector2(150,100));
            texts[1] = new TextBox(new Vector2(250, 100), name, ArialNarrow48, Color.White, red1, new Vector2(220, 100));

            buttons[0] = new Button(new Vector2(520,100),"Start",ArialNarrow48,Color.White,red1,new Vector2(240,80));
            buttons[1] = new Button(new Vector2(520, 200), "Om spillet", ArialNarrow48, Color.White, red1, new Vector2(240, 80));
            buttons[2] = new Button(new Vector2(520, 300), "Highscore", ArialNarrow48, Color.White, red1, new Vector2(240, 80));
            buttons[3] = new Button(new Vector2(520, 400), "Afslut", ArialNarrow48, Color.White, red1, new Vector2(240, 80));
            gameObjects.Add(texts[0]);
            gameObjects.Add(texts[1]);
            gameObjects.Add(buttons[0]);
            gameObjects.Add(buttons[1]);
            gameObjects.Add(buttons[2]);
            gameObjects.Add(buttons[3]);

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used tos draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ArialNarrow48 = Content.Load<SpriteFont>("ArialNarrow48");
            red1 = Content.Load<Texture2D>("Red1");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        float deltaTime;
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (menuState == 1)
            { 
                
            }

            texts[1].content = name;
            HandleKeys();

            if (buttons[0].clicked)
            {
                buttons[0].content = name;
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(deltaTime);
            }
                // TODO: Add your update logic here
                base.Update(gameTime);
        }

        Keys[] lastPressedKeys = new Keys[10];
        
        public void HandleKeys()
        {
            KeyboardState kbState = Keyboard.GetState();
            Keys[] pressedKeys = kbState.GetPressedKeys();

            //check if any of the previous update's keys are no longer pressed
            foreach (Keys key in lastPressedKeys)
            {
                if (!pressedKeys.Contains(key))
                {
                    OnKeyUp(key);
                }
            }

            //check if the currently pressed keys were already pressed
            foreach (Keys key in pressedKeys)
            {
                if (!lastPressedKeys.Contains(key))
                {
                    OnKeyDown(key);
                }
            }

            //save the currently pressed keys so we can compare on the next update
            lastPressedKeys = pressedKeys;
        }

        public void OnKeyDown(Keys key)
        {
            if (key == Keys.Back && name.Length>0)
            {
                char[] tempname = new char[name.Length-1];
                for (int i = 0; i < name.Length - 1; i++)
                {
                    tempname[i] = name[i];
                }

                name = "";

                for (int i = 0; i < tempname.Length; i++)
                {
                    name += tempname[i];
                }
            }
            else
            {
                name += key.ToString();
            }
        }

        public void OnKeyUp(Keys key)
        { 
            
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }
            spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
