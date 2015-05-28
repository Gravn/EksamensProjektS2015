using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using Input;
using Database;

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

        private SQLiteCommand dbComm;
        private SQLiteConnection dbConn;

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
        public Texture2D content_textBox;
        public Texture2D[] valg_button = new Texture2D[5];
        public Texture2D SidePanel_left,SidePanel_Right;

        public TextBox[] texts = new TextBox[10];
        public Button[] buttons = new Button[10];

        delegate void GetFunctions();
        private GetFunctions[] buttonFuctions;

        private string text_situation = "", text_fakta = "",text_A= "",text_B = "";

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

            dbConn = new SQLiteConnection("Data Source=dbProsa_27-5-15.db;Version=3");
            dbComm = new SQLiteCommand();
            dbConn.Open();

            //Database.Functions.CreateDatabase("dbProsa");

            //Database.Functions.ManualFunction(dbConn, dbComm, "CREATE TABLE IF NOT EXISTS 'valg' ('ID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 'Fakta' TEXT,'Spoergsmaal' TEXT, 'Konsekvens_A' INTEGER,'Konsekvens_B' INTEGER)");

            //Database.Functions.ManualFunction(dbConn, dbComm, "INSERT INTO 'valg' ('Fakta', 'Spoergsmaal', 'Konsekvens_A', 'Konsekvens_B') VALUES ('Prosa vil hjælpe dig, hvis du udsættes for sexchikane på arbejdspladsen.', 'Din chef tager på dig. \nHvad vil du gøre?', 0, 1)");

            KeyboardEvents.KeyTyped += KeyTyped;
            IsMouseVisible = true;

            buttonFuctions = new GetFunctions[]
            {
                MenuToggle
            };

            //Main Menu
            buttons[0] = new Button(new Vector2(460, 100), "Start", CopperPlateGothicLight48, Color.Black, Start_Normal, new Vector2(320, 110),false);
            buttons[1] = new Button(new Vector2(470, 270), "Om spillet", CopperPlateGothicLight36, Color.Black, Main_Medium_Normal, new Vector2(300, 75), false);
            buttons[2] = new Button(new Vector2(470, 370), "Highscore", CopperPlateGothicLight36, Color.Black, Main_Medium_Normal, new Vector2(300, 75), false);
            buttons[3] = new Button(new Vector2(470, 530), "Afslut", CopperPlateGothicLight36, Color.Black, Main_Medium_Normal, new Vector2(300, 75), false);
            
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


            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm,"valg","ID","1");
            while (reader.Read())
            {
                text_situation = (string)("" + reader["SpgTekst"]).Replace("\\n", "\n");
                text_fakta = (string)("" + reader["FaktaTekst"]).Replace("\\n", "\n");
                

            }

            //Choice
            menus[2] = new GameObject[7];
            menus[2][0] = texts[2] = new TextBox(new Vector2(180, 40), "" + text_situation, Arial12, Color.White, content_textBox, new Vector2(920, 220), false);
            menus[2][1] = buttons[5] = new Button(new Vector2(180,40+220), "Ja" + text_A, ArialNarrow48, Color.White, valg_button[0], new Vector2(920, 100), false);
            menus[2][2] = buttons[6] = new Button(new Vector2(180,40+220+100), "Nej" + text_B, ArialNarrow48, Color.White, valg_button[0], new Vector2(920,100), false);
            menus[2][3] = texts[3] = new TextBox(new Vector2(180, 40+220+100+100), "" + text_fakta, Arial12, Color.White, content_textBox, new Vector2(920, 220), false);

            //Consequence
            //load konsekvens fra db.

            menus[2][4] = new TextBox(new Vector2(180, 40 + 220 + 100 + 100+220), "Det var smart.", Arial12, Color.White, content_textBox, new Vector2(920, 220), false);
            menus[2][5] = new Button(new Vector2(180,40+220+100+100+220+220), "Videre", Arial12, Color.White, valg_button[0], new Vector2(920, 100), false);
            menus[2][6] = new TextBox(new Vector2(180, 40 + 220 + 100 + 100 + 220+220+100), "Vidste du, at", Arial12, Color.White, content_textBox, new Vector2(920, 220), false);

            //HighScore
            menus[4] = new GameObject[1];
            menus[4][0] = new Button(new Vector2(640,360), "Nothing to see here, move along(back)", ArialNarrow48, Color.White, red1, new Vector2(80, 80), true);

            //About
            menus[5] = new GameObject[1];
            menus[5][0] = new Button(new Vector2(640, 360), "Really Nothing to see here, move along(back)", ArialNarrow48, Color.White, red1, new Vector2(80, 80), true);

        }
        /// <summary>
        /// Navigates from questions to consequences, based on the user's answers
        /// </summary>
        /// <param name="btn"></param>
        public void MakeChoice(Button btn, int situationID)
        {
            for (int i = 0; i < 2; i++ )
            {
                if(btn.Clicked)
                {

                }
            }
        }
        public void MenuToggle()
        {
            for (int i = 0; i < menus[(int)menuState].Length; i++)
            {
                if (gameObjects.Contains(menus[(int)menuState][i]))
                {
                    if (menus[(int)menuState][i] is Button)
                    {
                        (menus[(int)menuState][i] as Button).Clicked = false;
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

            SidePanel_left = Content.Load<Texture2D>("SidePanel_Left");
            SidePanel_Right = Content.Load<Texture2D>("SidePanel_Right");
            arrow = Content.Load<Texture2D>("Arrow");

            bg_Noise = Content.Load<Texture2D>("bg_LightGreyNoise");
            Start_Normal = Content.Load<Texture2D>("Btn_Normal_Start");
            Main_Medium_Normal = Content.Load<Texture2D>("Btn_Normal_Main_Medium");

            content_textBox = Content.Load<Texture2D>("Textbox1");
            valg_button[0] = Content.Load<Texture2D>("Button");
            
            //TL = new TimeLine(new Vector2(10, 10), dayCounter);
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public float vScroll = 0;
        public bool move = false;
        protected override void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            dayCounter++;
            if (dayCounter >= 680)
            {
                dayCounter = 100;
            }

            if (vScroll > -680 && move == true)
            {
                vScroll -= 400*deltaTime;

                for (int i = 0; i < menus[2].Length; i++)
                {
                    menus[2][i].Position -= new Vector2(0, 400*deltaTime);
                }
            }
            else
            {
                move = false;
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (menuState == Menu.Main)
            {
                //Start
                if(buttons[0].Clicked)
                {
                    dayCounter += 100;
                    MenuToggle();
                    menuState = Menu.Name;
                    MenuToggle();
                }

                //About
                if(buttons[1].Clicked)
                {
                    MenuToggle();
                    menuState = Menu.About;
                    MenuToggle();
                }

                //Highscore
                if (buttons[2].Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Highscore;
                    MenuToggle();
                }

                //Exit
                if (buttons[3].Clicked)
                {
                    Exit();
                }
            }

            if (menuState.Equals(Menu.Highscore))
            {
                if ((menus[4][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.About))
            {
                if ((menus[5][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.Name))
            {
                //textInput.HandleKeyUpdate(gameTime);
                //texts[1].content = name;

                if (buttons[4].Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    MenuToggle();
                }
            }

            if(menuState.Equals(Menu.Choice))
            {
                /*for (int i = 0; i < menus.Length; i++ )
                {
                    for (int j = 0; j < menus[i].Length; j++)
                    {
                        if((menus[i][j] as Button).Clicked)
                        {
                            buttonFuctions[j]();
                        }
                    }
                }*/
                //JA
                if ((menus[2][1] as Button).Clicked)
                {
                    move = true;
                    //MenuToggle();
                    //menuState = Menu.Consequence;
                    //MenuToggle();
                }

                //Nej
                if ((menus[2][2] as Button).Clicked)
                {
                    move = true;

                }
                
                if ((menus[2][5] as Button).Clicked)
                {
                    move = true;
                }
            }

            if (menuState.Equals(Menu.Consequence))
            {
                if ((menus[3][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    //temp code.
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
