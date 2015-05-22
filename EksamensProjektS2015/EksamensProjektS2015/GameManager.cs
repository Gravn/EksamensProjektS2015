using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using Input;

namespace EksamensProjektS2015
{
    public class GameManager : Game
    {
        public enum Menu
        {
            Main = 0,
            Name = 1,
            Choice = 2,
            Consequence = 3,
            Highscore = 4,
            About = 5
        };

        public Menu menuState = Menu.Main;
        public GameObject[][] menus = new GameObject[10][];

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardEvents textInput = new KeyboardEvents();

        public static SpriteFont ArialNarrow48;

        public Texture2D red1;
        public Texture2D arrow;

        public TextBox[] texts = new TextBox[10];
        public Button[] buttons = new Button[10];

        //private TimeLine TL;
        private int dayCounter = 40;

        private static List<GameObject> gameObjects = new List<GameObject>();

        public string name = "";

        public static List<GameObject> GameObjects
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

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            KeyboardEvents.KeyTyped += KeyTyped;
            IsMouseVisible = true;

            //Main Menu
            buttons[0] = new Button(new Vector2(500,100),"Start",ArialNarrow48,Color.White,red1,new Vector2(280,80));
            buttons[1] = new Button(new Vector2(500, 220), "Om spillet", ArialNarrow48, Color.White, red1, new Vector2(280, 80));
            buttons[2] = new Button(new Vector2(500, 340), "Highscore", ArialNarrow48, Color.White, red1, new Vector2(280, 80));
            buttons[3] = new Button(new Vector2(500, 460), "Afslut", ArialNarrow48, Color.White, red1, new Vector2(280, 80));
            
            menus[0] = new GameObject[4];
            menus[0][0] = buttons[0];
            menus[0][1] = buttons[1];
            menus[0][2] = buttons[2];
            menus[0][3] = buttons[3];
            MenuToggle();

            //Name input
            texts[0] = new TextBox(new Vector2(100, 100), "Navn:", ArialNarrow48, Color.White, red1, new Vector2(150, 100));
            texts[1] = new TextBox(new Vector2(250, 100), name, ArialNarrow48, Color.White, red1, new Vector2(220, 100));
            buttons[4] = new Button(new Vector2(100, 240), "Videre", ArialNarrow48, Color.White, red1, new Vector2(220, 100));
            
            menus[1] = new GameObject[3];
            menus[1][0] = texts[0];
            menus[1][1] = texts[1];
            menus[1][2] = buttons[4];

            //Choice            
            texts[2] = new TextBox(new Vector2(100, 60), "Velkommen, " + name, ArialNarrow48, Color.White, red1, new Vector2(1080, 240));
            buttons[5] = new Button(new Vector2(100 + 120, 60 + 240 + 30), "JA", ArialNarrow48, Color.White, red1, new Vector2(80, 80));
            buttons[6] = new Button(new Vector2(100 + 1080 - 120 - 40, 60 + 240 + 30), "Nej", ArialNarrow48, Color.White, red1, new Vector2(80, 80));
            texts[3] = new TextBox(new Vector2(150, 440), "Vidste du, at ... " + name, ArialNarrow48, Color.White, red1, new Vector2(980, 240));
            
            menus[2] = new GameObject[4];
            menus[2][0] = texts[2];
            menus[2][1] = buttons[5];
            menus[2][2] = buttons[6];
            menus[2][3] = texts[3];

            //Consequence

            //About

            //HighScore
            
        }

        public void MenuToggle()
        {
            for (int i = 0; i < menus[(int)menuState].Length; i++)
            {
                if (gameObjects.Contains(menus[(int)menuState][i]))
                {
                    gameObjects.Remove(menus[(int)menuState][i]);
                }
                else
                {
                    gameObjects.Add(menus[(int)menuState][i]);
                }
            }
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used tos draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            ArialNarrow48 = Content.Load<SpriteFont>("ArialNarrow48");
            red1 = Content.Load<Texture2D>("Red1");

            arrow = Content.Load<Texture2D>("Arrow");
            //TL = new TimeLine(new Vector2(10, 10), dayCounter);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (menuState == Menu.Main)
            {
                if(buttons[0].clicked)
                {
                    //Start
                    dayCounter += 100;
                    MenuToggle();
                    menuState = Menu.Name;
                    MenuToggle();
                }
                    //About
                if(buttons[1].clicked)
                {
                    MenuToggle();
                    menuState = Menu.About;
                    MenuToggle();

                }
                //Highscore
                if (buttons[2].clicked)
                {
                    MenuToggle();
                    menuState = Menu.Highscore;
                    MenuToggle();
                }
                //Exit
                if (buttons[3].clicked)
                {
                    Exit();
                }
            }

            if (menuState.Equals(Menu.About))
            {

            }

            if (menuState.Equals(Menu.Highscore))
            { 
                    
            }

            if (menuState.Equals(Menu.Name))
            {
                textInput.HandleKeyUpdate(gameTime);
                texts[1].content = name;

                if (buttons[4].clicked)
                {
                    //if name conditions not met: show error msg
                    //else
                    MenuToggle();
                    texts[2].content = "Velkommen, "+name;
                    menuState = Menu.Choice;
                    MenuToggle();
                }
            }

            if(menuState.Equals(Menu.Choice))
            {
                //JA

                //Nej
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update(deltaTime);
            }
                // TODO: Add your update logic here
            //TL.Update(deltaTime);
                base.Update(gameTime);
        }

        void KeyTyped(object sender, KeyboardEventArgs e)
        {
            if (e.character.HasValue)
            {
                name += e.character.Value;
            }
            
            if (e.key == Keys.Back && name.Length > 0)
            {
                name = name.Substring(0, name.Length - 1);
            }
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

            spriteBatch.Draw(arrow, new Rectangle(10, dayCounter, 16, 16), Color.White);

            spriteBatch.End();
            
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
