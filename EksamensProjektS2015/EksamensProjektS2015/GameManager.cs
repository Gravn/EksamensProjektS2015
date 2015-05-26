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
        public static SpriteFont Arial12;
        public static SpriteFont CopperPlateGothicLight48;
        public static SpriteFont CopperPlateGothicLight36;

        public Texture2D red1;
        public Texture2D arrow;
        public Texture2D Start_Normal;
        public Texture2D Main_Medium_Normal;
        public Texture2D bg_Noise;

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
            buttons[0] = new Button(new Vector2(460, 100), "Start", CopperPlateGothicLight48, Color.Black, Start_Normal, new Vector2(320, 110),false);
            buttons[1] = new Button(new Vector2(470, 270), "Om spillet", CopperPlateGothicLight36, Color.Black, Main_Medium_Normal, new Vector2(300, 75), false);
            buttons[2] = new Button(new Vector2(470, 370), "Highscore", CopperPlateGothicLight36, Color.Black, Main_Medium_Normal, new Vector2(300, 75), false);
            buttons[3] = new Button(new Vector2(470, 530), "Afslut", CopperPlateGothicLight36, Color.Black, red1, new Vector2(300, 75), true);
            
            menus[0] = new GameObject[4];
            menus[0][0] = buttons[0];
            menus[0][1] = buttons[1];
            menus[0][2] = buttons[2];
            menus[0][3] = buttons[3];
            MenuToggle();

            //Name input
            texts[0] = new TextBox(new Vector2(100, 100), "Navn:", ArialNarrow48, Color.White, red1, new Vector2(150, 100),true);
            texts[1] = new TextBox(new Vector2(250, 100), name, ArialNarrow48, Color.White, red1, new Vector2(220, 100),true);
            buttons[4] = new Button(new Vector2(100, 240), "Videre", ArialNarrow48, Color.White, red1, new Vector2(220, 100),true);
            
            menus[1] = new GameObject[3];
            menus[1][0] = texts[0];
            menus[1][1] = texts[1];
            menus[1][2] = buttons[4];

            //Choice            
            texts[2] = new TextBox(new Vector2(100, 30), "Du er lige startet med at arbejde på virksomheden ARBEJDSPLADS. Du har fået dit eget kontor at arbejde på.\nPå dit kontor står firmaets printer, da kontoret blev brugt som printerrum før.\nDu har efter noget tid i firmaet, fundet det generende for dit arbejde at den står og larmer. \nDu er stadig ny på arbejdspladsen, så du skal tage stilling til, om det er værd at tage op med chefen, eller bare at leve med det.\n\nDu har nu 2 valgmuligheder." + name,Arial12, Color.Black, red1, new Vector2(1080,180),true);
            buttons[5] = new Button(new Vector2(100 + 120, 240), "Pas dit arbejde.", Arial12, Color.Black, Main_Medium_Normal, new Vector2(180, 80),false);
            buttons[6] = new Button(new Vector2(100 + 120, 340), "Konfronter chefen", Arial12, Color.Black,Main_Medium_Normal, new Vector2(180, 80),false);
            texts[3] = new TextBox(new Vector2(200, 440), "Vidste du at printere og kopimaskiner skal ud af arbejdslokalet, hvis de bliver brugt jævnligt i løbet af en dag." + name,Arial12, Color.White, red1, new Vector2(980, 240),true);
            
            menus[2] = new GameObject[4];
            menus[2][0] = texts[2];
            menus[2][1] = buttons[5];
            menus[2][2] = buttons[6];
            menus[2][3] = texts[3];

            //Consequence
            menus[3] = new GameObject[1];
            menus[3][0] = new Button(new Vector2(640, 360), "Konsekvens.", ArialNarrow48, Color.White, red1, new Vector2(80, 80), true);
           

            //HighScore
            menus[4] = new GameObject[1];
            menus[4][0] = new Button(new Vector2(640,360), "Nothing to see here, move along(back)", ArialNarrow48, Color.White, red1, new Vector2(80, 80), true);

            //About
            menus[5] = new GameObject[1];
            menus[5][0] = new Button(new Vector2(640, 360), "Really Nothing to see here, move along(back)", ArialNarrow48, Color.White, red1, new Vector2(80, 80), true);

        }

        public void MenuToggle()
        {
            for (int i = 0; i < menus[(int)menuState].Length; i++)
            {
                if (gameObjects.Contains(menus[(int)menuState][i]))
                {
                    if (menus[(int)menuState][i] is Button)
                    {
                        (menus[(int)menuState][i] as Button).clicked = false;
                    }
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

            CopperPlateGothicLight48 = Content.Load<SpriteFont>("CopperPlate Gothic Light 48");
            CopperPlateGothicLight36 = Content.Load<SpriteFont>("CopperPlate Gothic Light 36");
            ArialNarrow48 = Content.Load<SpriteFont>("ArialNarrow48");
            Arial12 = Content.Load<SpriteFont>("Arial12");
            
            red1 = Content.Load<Texture2D>("Red1");

            arrow = Content.Load<Texture2D>("Arrow");

            bg_Noise = Content.Load<Texture2D>("bg_LightGreyNoise");
            Start_Normal = Content.Load<Texture2D>("Btn_Normal_Start");
            Main_Medium_Normal = Content.Load<Texture2D>("Btn_Normal_Main_Medium");

            
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
                //Start
                if(buttons[0].clicked)
                {

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

            if (menuState.Equals(Menu.Highscore))
            {
                if ((menus[4][0] as Button).clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.About))
            {
                if ((menus[5][0] as Button).clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
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
                    //texts[2].content = "Velkommen, "+name;
                    menuState = Menu.Choice;
                    MenuToggle();
                }
            }

            if(menuState.Equals(Menu.Choice))
            {
                //JA
                if ((menus[2][1] as Button).clicked)
                {
                    MenuToggle();
                    menuState = Menu.Consequence;
                    MenuToggle();
                }
                //Nej
                if ((menus[2][2] as Button).clicked)
                {
                    MenuToggle();
                    menuState = Menu.Consequence;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.Consequence))
            {
                if ((menus[3][0] as Button).clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    
                    //temp code.
                    dayCounter+=10;
                    
                    MenuToggle();
                }
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

            spriteBatch.Draw(bg_Noise, new Rectangle(0, 0, bg_Noise.Width, bg_Noise.Height), Color.White);
            spriteBatch.Draw(arrow, new Rectangle(10, dayCounter, 16, 16), Color.White);

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
