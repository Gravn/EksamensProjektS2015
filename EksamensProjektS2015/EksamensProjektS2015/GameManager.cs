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

        public Texture2D arrow;
        public Texture2D Start_Normal;
        public Texture2D Main_Medium_Normal;
        public Texture2D bg_Noise;
        public Texture2D valg_button, valg_textbox, valg_divider;
        public Texture2D SidePanel_left,SidePanel_Right;
        public Texture2D Title;

        public TextBox[] texts = new TextBox[10];
        public Button[] buttons = new Button[10];

        // Delegates to simplify menu code later in the script
        delegate void GetFunctions();
        private GetFunctions[] buttonFuctions;

        private string text_situation = "", text_fakta = "",text_A= "",text_B = "",text_konFaktaTekst = "",text_konTekst;
        public int currentValg = 1;

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

        public void ReadValgContent(int i)
        {
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "valg", "ID", i);

            while (reader.Read())
            {
                text_situation = (string)("" + reader["SpgTekst"]);
                (menus[2][0] as TextBox).Content = text_situation; 

                text_fakta = "" + reader["FaktaTekst"];
                (menus[2][3] as TextBox).Content = text_fakta;

                text_konFaktaTekst = "" + reader["KonFaktaTekst"];
                (menus[2][8] as TextBox).Content = text_konFaktaTekst;

            }

            reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", i);
            while (reader.Read())
            {
                text_A = "" + reader["svarValg"];
                (menus[2][1] as Button).Content = text_A;
            }

            reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", i+1);
            while (reader.Read())
            {
                text_B = ""+reader["svarValg"];
                (menus[2][2] as Button).Content = text_B;
            }
        }

        public void ReadSvarContent(int i)
        {
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", i);
            while (reader.Read())
            {
                text_konTekst = "" + reader["konTekst"];
                (menus[2][6] as TextBox).Content = text_konTekst;
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            dbConn = new SQLiteConnection("Data Source=Content/dbProsa_27-5-15.db;Version=3");
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
            buttons[0] = new Button(new Vector2(180, 200), "Start", CopperPlateGothicLight48, Color.Black,valg_button, new Vector2(920,100),false);
            buttons[1] = new Button(new Vector2(180, 320), "Om spillet", CopperPlateGothicLight36, Color.Black, valg_button, new Vector2(920, 100), false);
            buttons[2] = new Button(new Vector2(180, 440), "Highscore", CopperPlateGothicLight36, Color.Black, valg_button, new Vector2(920, 100), false);
            buttons[3] = new Button(new Vector2(180, 560), "Afslut", CopperPlateGothicLight36, Color.Black, valg_button, new Vector2(920, 100), false);
            
            menus[0] = new GameObject[5];
            menus[0][0] = buttons[0];
            menus[0][1] = buttons[1];
            menus[0][2] = buttons[2];
            menus[0][3] = buttons[3];
            menus[0][4] = new TextBox(new Vector2(0,20), "", ArialNarrow48, Color.White, Title, new Vector2(150, 100), false);

            MenuToggle();

            //Name input
            texts[0] = new TextBox(new Vector2(100, 100), "Navn:", ArialNarrow48, Color.White,Main_Medium_Normal, new Vector2(150, 100),true);
            texts[1] = new TextBox(new Vector2(250, 100), name, ArialNarrow48, Color.White, Main_Medium_Normal, new Vector2(220, 100),true);
            buttons[4] = new Button(new Vector2(100, 240), "Videre", ArialNarrow48, Color.White, Main_Medium_Normal, new Vector2(220, 100),true);
            
            menus[1] = new GameObject[3];
            menus[1][0] = texts[0];
            menus[1][1] = texts[1];
            menus[1][2] = buttons[4];




            //Choice
            menus[2] = new GameObject[13];
            menus[2][0] = texts[2] = new TextBox(new Vector2(180, 40), "" + text_situation, Arial12, Color.White,valg_textbox, new Vector2(920, 220), false);
            menus[2][1] = buttons[5] = new Button(new Vector2(180,40+220), "" + text_A, ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[2][2] = buttons[6] = new Button(new Vector2(180,40+220+100), "" + text_B, ArialNarrow48, Color.Black, valg_button, new Vector2(920,100), false);
            menus[2][3] = texts[3] = new TextBox(new Vector2(180, 40+220+100+100), "" + text_fakta, Arial12, Color.White, valg_textbox, new Vector2(920, 220), false);

            //Consequence
            //load konsekvens from db.
            menus[2][4] = new TextBox(new Vector2(180, 720 + 220+40), "", Arial12, Color.White, valg_divider, Vector2.Zero, false);
            menus[2][5] = new TextBox(new Vector2(180, 720 + 220 +160), "", Arial12, Color.White, valg_divider, Vector2.Zero, false);

            menus[2][6] = new TextBox(new Vector2(180,40+720), "" + text_konTekst , Arial12, Color.White, valg_textbox, new Vector2(920, 220), false);
            menus[2][7] = new Button(new Vector2(180,40+720+220+50), "Videre", ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[2][8] = new TextBox(new Vector2(180, 40+720+220+50+100+50), ""+text_konFaktaTekst, Arial12, Color.White, valg_textbox, new Vector2(920, 220), false);
            
            menus[2][9] = new TextBox(new Vector2(180, -40), "", Arial12, Color.White, valg_divider, Vector2.Zero, false);
            menus[2][10] = new TextBox(new Vector2(180, 720 - 40), "", Arial12, Color.White, valg_divider, Vector2.Zero, false);
            
            menus[2][11] = new TextBox(new Vector2(0, 0), "" , Arial12, Color.White, SidePanel_left, Vector2.Zero, false);
            menus[2][12] = new TextBox(new Vector2(1100, 0), "", Arial12, Color.White, SidePanel_Right, Vector2.Zero, false);

            //HighScore
            menus[4] = new GameObject[1];
            menus[4][0] = new Button(new Vector2(640,360), "Nothing to see here, move along(back)", ArialNarrow48, Color.White,Main_Medium_Normal, new Vector2(80, 80), true);

            //About
            menus[5] = new GameObject[1];
            menus[5][0] = new Button(new Vector2(640, 360), "Really Nothing to see here, move along(back)", ArialNarrow48, Color.White,Main_Medium_Normal, new Vector2(80, 80), true);

            ReadValgContent(1);
            ReadSvarContent(1);   
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
            // Get all objects in menus
            for (int i = 0; i < menus[(int)menuState].Length; i++)
            {
                // Checks if the object exists in the gameobjects list
                if (gameObjects.Contains(menus[(int)menuState][i]))
                {
                    // Insure the object is a button
                    if (menus[(int)menuState][i] is Button)
                    {
                        // Set the 'clicked' bool for the button to false
                        (menus[(int)menuState][i] as Button).Clicked = false;
                    }
                    // Remove the button from the gameobjects list
                    gameObjects.Remove(menus[(int)menuState][i]);
                }
                else
                {
                    // Otherwise add the button
                    gameObjects.Add(menus[(int)menuState][i]);
                }
            }
        }
        public void MenuNavigation(Menu menu)
        {
            MenuToggle();
            menuState = menu;
            MenuToggle();
        }
        public float Lerp(float from, float to, float time)
        {
            return (to - from) * time;
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used tos draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CopperPlateGothicLight48 = Content.Load<SpriteFont>("CopperPlate Gothic Light 48");
            CopperPlateGothicLight36 = Content.Load<SpriteFont>("CopperPlate Gothic Light 36");
            ArialNarrow48 = Content.Load<SpriteFont>("ArialNarrow48");
            Arial12 = Content.Load<SpriteFont>("Arial12");

            Title = Content.Load<Texture2D>("Title");

            SidePanel_left = Content.Load<Texture2D>("SidePanel_Left");
            SidePanel_Right = Content.Load<Texture2D>("SidePanel_Right");
            arrow = Content.Load<Texture2D>("Arrow");

            bg_Noise = Content.Load<Texture2D>("bg_LightGreyNoise");
            Start_Normal = Content.Load<Texture2D>("Btn_Normal_Start");
            Main_Medium_Normal = Content.Load<Texture2D>("Btn_Normal_Main_Medium");

            valg_textbox = Content.Load<Texture2D>("Panel_Textbox");
            valg_button = Content.Load<Texture2D>("Panel_Button_normal");
            valg_divider = Content.Load<Texture2D>("Divider");
            
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

            if (vScroll < 720)
            {
                if (move == true)
                {
                    //menus[2][i].Position -= new Vector2(0, 400 * deltaTime);
                    vScroll += 600 * deltaTime;

                    for (int i = 0; i < 11; i++)
                    {
                        menus[2][i].Position -= new Vector2(0, 600 * deltaTime);//Lerp(menus[2][i].Position.Y, 600, deltaTime);
                        if (menus[2][i].Position.Y < -220)
                        {
                            menus[2][i].Position += new Vector2(0, 1440);
                        }
                    }
                }
            }
            else
            {
                move = false;
                vScroll = 0;
            }
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            // Check all buttons' states in the menu
            /*for (int i = 0; i < menus.Length; i++)
            {
                if (menuState == (Menu)(i))
                {
                    for (int j = 0; j < menus[i].Length; j++)
                    {
                        if (menus[i][j] is Button)
                        {
                            if((menus[i][j] as Button).Clicked)
                            {
                                
                            }
                        }
                    }
                }
            }*/
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
                textInput.HandleKeyUpdate(gameTime);
                texts[1].Content = name;

                if (buttons[4].Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    MenuToggle();
                }
            }

            if(menuState.Equals(Menu.Choice))
            {
                //JA
                if ((menus[2][1] as Button).Clicked)
                {
                    ReadSvarContent(currentValg);

                    move = true;

                    //MenuToggle();
                    //menuState = Menu.Consequence;
                    //MenuToggle();
                }

                //Nej
                if ((menus[2][2] as Button).Clicked)
                {
                    ReadSvarContent(currentValg+1);
                    move = true;

                }
                

                //videre
                if ((menus[2][7] as Button).Clicked)
                {
                    
                    ReadValgContent(currentValg);
                    currentValg++;
                    //ReadValgContent(currentValg);
                    
                    move = true;
                }
            }

            if (menuState.Equals(Menu.Consequence))
            {
                if ((menus[3][0] as Button).Clicked)
                {
                    move = true;
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
