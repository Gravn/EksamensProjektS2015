using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Diagnostics;
using Input;
using Database;
using System;

namespace EksamensProjektS2015
{
    public class GameManager : Game
    {
        public enum Menu
        {
            Main = 0,
            Name = 1,
            Choice = 2,
            Highscore = 3,
            About = 4,
            ContinuePromt = 5
        };

        #region variables
        public Menu menuState = Menu.Main;
        public GameObject[][] menus = new GameObject[10][];

        private SQLiteCommand dbComm;
        private SQLiteConnection dbConn;

        private SQLiteCommand dbCommHs;
        private SQLiteConnection dbConnHs;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardEvents textInput = new KeyboardEvents();

        public static SpriteFont ArialNarrow48;
        public static SpriteFont ErasMediumITC14;
        public static SpriteFont CopperPlateGothicLight48;
        public static SpriteFont CopperPlateGothicLight36;

        private double KollegaLøn = 25000;

        public Texture2D arrow;
        public Texture2D Start_Normal;
        public Texture2D Main_Medium_Normal;
        public Texture2D InGameScreenshot640x353;
        public Texture2D bg_Noise;
        public Texture2D valg_button, valg_textbox, valg_divider;
        public Texture2D SidePanel_left, SidePanel_Right;
        public Texture2D Title;
        public Texture2D Rival_Silhouette;
        public Texture2D TLtest;


        private string highscore;
        private bool loaded = false;
        private int row = 0;
        private string[] svarValg = new string[2];
        private string[] konTekst = new string[2];

        // Delegates to simplify menu code later in the script
        delegate void GetFunctions();
        private GetFunctions[] buttonFuctions;

        private string text_situation = "", text_fakta = "", text_A = "", text_B = "", text_konFaktaTekst = "", text_konTekst;
        public int currentValg = 1;

        private TimeLine TL;
        public static int dayCounter = 40;

        private static List<GameObject> gameObjects = new List<GameObject>();

        public string name = "";

        public static List<GameObject> GameObjects
        {
            get { return gameObjects; }
            set { gameObjects = value; }
        }
        #endregion

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

            dbConn = new SQLiteConnection("Data Source=Content/TextContent.db;Version=3");
            dbComm = new SQLiteCommand();
            dbConn.Open();

            dbConnHs = new SQLiteConnection("Data Source=Content/Players.db;Version=3");
            dbCommHs = new SQLiteCommand();
            dbConnHs.Open();

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
            menus[0] = new GameObject[5];
            menus[0][0] = new Button(new Vector2(180, 200), "Start", CopperPlateGothicLight48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[0][1] = new Button(new Vector2(180, 320), "Om spillet", CopperPlateGothicLight36, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[0][2] = new Button(new Vector2(180, 440), "Highscore", CopperPlateGothicLight36, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[0][3] = new Button(new Vector2(180, 560), "Afslut", CopperPlateGothicLight36, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[0][4] = new TextBox(new Vector2(0, 20), "", ArialNarrow48, Color.White, Title, new Vector2(150, 100), false);

            MenuToggle();

            //Name input
            menus[1] = new GameObject[4];
            menus[1][0] = new TextBox(new Vector2(100, 100), "Navn:", ArialNarrow48, Color.White, Main_Medium_Normal, new Vector2(150, 100), false);
            menus[1][1] = new TextBox(new Vector2(250, 100), name, ArialNarrow48, Color.White,0, Main_Medium_Normal, new Vector2(220, 100), false);
            menus[1][2] = new Button(new Vector2(100, 240), "Videre", ArialNarrow48, Color.Black, Main_Medium_Normal, new Vector2(220, 100), false);
            menus[1][3] = new Button(new Vector2(100, 540), "Tilbage", ArialNarrow48, Color.Black, Main_Medium_Normal, new Vector2(220, 100), false);

            //Choice
            menus[2] = new GameObject[19];
            menus[2][0] = new TextBox(new Vector2(180, 40), "" + text_situation, ErasMediumITC14, Color.White, valg_textbox, new Vector2(920, 220), false);
            menus[2][1] = new Button(new Vector2(180, 40 + 220), "" + text_A, ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[2][2] = new Button(new Vector2(180, 40 + 220 + 100), "" + text_B, ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[2][3] = new TextBox(new Vector2(180, 40 + 220 + 100 + 100), "" + text_fakta, ErasMediumITC14, Color.White, valg_textbox, new Vector2(920, 220), false);

            //Consequence
            menus[2][4] = new TextBox(new Vector2(180, 720 + 220 + 40), "", ErasMediumITC14, Color.White, valg_divider, Vector2.Zero, false);
            menus[2][5] = new TextBox(new Vector2(180, 720 + 220 + 160), "", ErasMediumITC14, Color.White, valg_divider, Vector2.Zero, false);

            menus[2][6] = new TextBox(new Vector2(180, 40 + 720), "" + text_konTekst, ErasMediumITC14, Color.White, valg_textbox, new Vector2(920, 220), false);
            menus[2][7] = new Button(new Vector2(180, 40 + 720 + 220 + 50), "Videre", ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[2][8] = new TextBox(new Vector2(180, 40 + 720 + 220 + 50 + 100 + 50), "" + text_konFaktaTekst, ErasMediumITC14, Color.White, valg_textbox, new Vector2(920, 220), false);

            menus[2][9] = new TextBox(new Vector2(180, -40), "", ErasMediumITC14, Color.White, valg_divider, Vector2.Zero, false);
            menus[2][10] = new TextBox(new Vector2(180, 720 - 40), "", ErasMediumITC14, Color.White, valg_divider, Vector2.Zero, false);

            menus[2][11] = new TextBox(new Vector2(0, 0), "", ErasMediumITC14, Color.White, SidePanel_left, Vector2.Zero, false);
            menus[2][12] = new TextBox(new Vector2(1100, 0), "", ErasMediumITC14, Color.White, SidePanel_Right, Vector2.Zero, false);
            menus[2][13] = new TextBox(new Vector2(5, 150), "", ErasMediumITC14, Color.White, Rival_Silhouette, Vector2.Zero, false);
            menus[2][14] = new TextBox(new Vector2(5, 300), "Karl Åge\nLøn: 35.000kr\nErfaring: 2 år", ErasMediumITC14, Color.White, null, new Vector2(170, 70), false);
            menus[2][15] = new TextBox(new Vector2(1100, 150), "01/06/2015\n\n02/05/2015\n\n03/06/2015\n\n04/06/2015\n\n05/06/2015\n\n06/06/2015\n\n07/06/2015\n\n08/06/2015\n\n09/06/2015\n", ErasMediumITC14, Color.White, null, new Vector2(170, 300), false);
            menus[2][16] = new TextBox(new Vector2(0, 500), "" + name.ToString() + "Din Løn:", ErasMediumITC14, Color.White, null, new Vector2(170, 0), false);
            menus[2][17] = new Button(new Vector2(1100, 650), "Menu" + text_B, ArialNarrow48, Color.Black, Main_Medium_Normal, new Vector2(180, 180), false);
            menus[2][18] = new TextBox(new Vector2(1100, 100), "", ErasMediumITC14, Color.White, TLtest, new Vector2(180, 25), false);

            //HighScore
            menus[3] = new GameObject[2];
            menus[3][0] = new Button(new Vector2(180, 640), "Tilbage", ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);
            menus[3][1] = new TextBox(new Vector2(480, 50),"", ErasMediumITC14, Color.White, 0, null, new Vector2(920, 600), false);
            
            //About
            menus[4] = new GameObject[3];
            menus[4][0] = new Button(new Vector2(1050, 650), "Back", ArialNarrow48, Color.White, Main_Medium_Normal, new Vector2(180, 100),false);
            menus[4][1] = new TextBox(new Vector2(600, 200), "Om Spillet.\n\n Du er blevet ansat sammen med Karl Åge, i en lille IT virksomhed som arbejder med support og IT løsninger til andre IT firmaer. \n Virksomheden har eksisteret i 2 år, og salget går fremad.\n\n Du står nu med et arbejde men uden en fagforening og en a kasse, og bliver nu udsat for den hårdeste arbejdsmåned i dit liv.\n Med de mest mærkværdige udfordringer en person kunne tænkes at blive udsat for, i løbet af arbejdslivet. \n\n Det er nu din opgave at klare dig gennem arbejdet, UDEN hjælp fra en fagforening, \n for at se hvordan arbejdet kunne se ud, hvis du stod uden en. \n\nDu vil på samme tid skulle kæmpe mod din kollega, og se hvem der kan få sin løn højest, sammen med de mærkværdige udfordringer.", ErasMediumITC14, Color.White, null, new Vector2(170, 0), false);
            menus[4][2] = new TextBox(new Vector2(350, 360), " ", ErasMediumITC14, Color.White, InGameScreenshot640x353, new Vector2(640, 353), false);

            
            //ContinouePromt
            menus[5] = new GameObject[2];
            menus[5][0] = new Button(new Vector2(180, 160), "Fortsæt", ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);

            menus[5][1] = new Button(new Vector2(180, 260), "Nyt Spil", ArialNarrow48, Color.Black, valg_button, new Vector2(920, 100), false);


            ReadValgContent();
        }
        private void MoveElements(float deltaTime)
        {
            
                
            /*TextBox[] topElements = (TextBox[])menus[2];
            Vector2[] positions = new Vector2[topElements.Length];
            for (int i = 0; i < menus[2].Length; i++)
            {
                positions[i] = topElements[i].Position;
                positions[i] -= new Vector2(1, 0);
                if((positions[i].Y) > 0)
                {

                }
            }*/
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used tos draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            CopperPlateGothicLight48 = Content.Load<SpriteFont>("CopperPlate Gothic Light 48");
            CopperPlateGothicLight36 = Content.Load<SpriteFont>("CopperPlate Gothic Light 36");
            ArialNarrow48 = Content.Load<SpriteFont>("ArialNarrow48");
            ErasMediumITC14 = Content.Load<SpriteFont>("Eras Medium ITC");

            Title = Content.Load<Texture2D>("Title");

            SidePanel_left = Content.Load<Texture2D>("SidePanel_Left");
            SidePanel_Right = Content.Load<Texture2D>("SidePanel_Right");
            arrow = Content.Load<Texture2D>("Arrow");

            bg_Noise = Content.Load<Texture2D>("bg_LightGreyNoise");
            Start_Normal = Content.Load<Texture2D>("Btn_Normal_Start");
            Main_Medium_Normal = Content.Load<Texture2D>("Btn_Normal_Main_Medium");
            InGameScreenshot640x353 = Content.Load<Texture2D>("InGameScreenshot640x353");
            TLtest = Content.Load<Texture2D>("TimeLineTest");

            valg_textbox = Content.Load<Texture2D>("Panel_Textbox");
            valg_button = Content.Load<Texture2D>("Panel_Button_normal");
            valg_divider = Content.Load<Texture2D>("Divider");
            Rival_Silhouette = Content.Load<Texture2D>("Silhouette");

            TL = new TimeLine(new Vector2(1100, 50));
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
            if (currentValg != 1)
            {
                (menus[0][0] as Button).Content = "Fortsæt/Nyt Spil";
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            //dayCounter++;
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
                        menus[2][i].Position -= new Vector2(0, 600 * deltaTime);
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
            /*if (move)
            {
                float[] distances = new float[menus[2].Length - 6];
                for (int i = 0; i < menus[2].Length - 6; i++)
                {
                    float yPos = (menus[2][i] as TextBox).Position.Y;
                    if(distances[i] == 0)
                    {
                        distances[i] = yPos - 720 + (720 - yPos);
                    }
                    menus[2][i].Position = new Vector2(menus[2][i].Position.X, Lerp(yPos, distances[i], deltaTime));
                }
            }*/

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
                if ((menus[0][0] as Button).Clicked)
                {

                    MenuToggle();
                    if (currentValg == 1)
                    {
                        menuState = Menu.Name;
                    }
                    else
                    {
                        menuState = Menu.ContinuePromt;
                    }
                    MenuToggle();
                }

                //About
                if ((menus[0][1] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.About;
                    MenuToggle();
                }

                //Highscore
                if ((menus[0][2] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Highscore;
                    MenuToggle();
                }

                //Exit
                if ((menus[0][3] as Button).Clicked)
                {
                    Exit();
                }
            }

            if (menuState.Equals(Menu.Highscore))
            {
                if (!loaded)
                {
                    SQLiteDataReader reader = Database.Functions.TableSelectAllDescending(dbConnHs, dbCommHs, "spiller","point");

                    while (reader.Read())
                    {
                        //ID
                        for (int j = 0; j < 2 - (row+1).ToString().Length; j++)
                        {
                            highscore += "  ";
                        }

                        highscore += (row+1).ToString();

                        for (int j = 0; j < 10; j++)
                        {
                            highscore += "  ";
                        }
                            
                        //Name
                        highscore += reader[1].ToString();

                        for (int j =0 ; j <(20-reader[1].ToString().Length); j++)
                        {
                            highscore += "  ";
                        }

                        //Point
                        highscore += (reader[1].ToString().Length);
                        //highscore += reader[2].ToString();

                        highscore += "\n";
                        row++;
                    }
                    row = 0;

                    (menus[3][1] as TextBox).Content = ""+highscore;

                    loaded = true;
                }


                if ((menus[3][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.About))
            {
                if ((menus[4][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.Name))
            {
                currentValg = 1;
                textInput.HandleKeyUpdate(gameTime);
                (menus[1][1] as TextBox).Content = name;

                if ((menus[1][2] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    MenuToggle();
                }

                if ((menus[1][3] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.Choice))
            {
                //JA
                if ((menus[2][1] as Button).Clicked)
                {
                    ReadSvarContent(0);

                    move = true;

                    //MenuToggle();
                    //menuState = Menu.Consequence;
                    //MenuToggle();
                }

                //Nej
                if ((menus[2][2] as Button).Clicked)
                {
                    ReadSvarContent(1);
                    move = true;

                }

                //videre
                if ((menus[2][7] as Button).Clicked)
                {
                    menus[2][15].Position -= new Vector2(0, 45);
                    currentValg++;

                    switch (currentValg)
                    {
                        case 5:
                            double rnd = GetRandomNumber(0, 0.05);
                            KollegaLøn = (KollegaLøn * rnd) + KollegaLøn;
                            KollegaLøn = (double)Math.Round((decimal)KollegaLøn, 0);
                            (menus[2][14] as TextBox).Content = "Karl Åge\nLøn: " + KollegaLøn.ToString() + " \nErfaring: 2 år";
                            break;
                        case 12:
                            double rnd2 = GetRandomNumber(0, 0.05);
                            KollegaLøn = (KollegaLøn * rnd2) + KollegaLøn;
                            KollegaLøn = (double)Math.Round((decimal)KollegaLøn, 0);
                            (menus[2][14] as TextBox).Content = "Karl Åge\nLøn: " + KollegaLøn.ToString() + " \nErfaring: 2 år";

                            break;
                        case 18:
                            double rnd3 = GetRandomNumber(0, 0.05);
                            KollegaLøn = (KollegaLøn * rnd3) + KollegaLøn;
                            KollegaLøn = (double)Math.Round((decimal)KollegaLøn, 0);
                            (menus[2][14] as TextBox).Content = "Karl Åge\nLøn: " + KollegaLøn.ToString() + " \nErfaring: 2 år";

                            break;
                        case 26:
                            double rnd4 = GetRandomNumber(0, 0.05);
                            KollegaLøn = (KollegaLøn * rnd4) + KollegaLøn;
                            KollegaLøn = (double)Math.Round((decimal)KollegaLøn, 0);
                            (menus[2][14] as TextBox).Content = "Karl Åge\nLøn: " + KollegaLøn.ToString() + " \nErfaring: 2 år";
                            break;

                        default:
                            break;
                    }

                    ReadValgContent();
                    //ReadValgContent(currentValg);
                    (menus[2][7] as Button).Clicked = false;
                    move = true;
                }

                if ((menus[2][17] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }

            if (menuState.Equals(Menu.ContinuePromt))
            {
                if ((menus[5][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    MenuToggle();
                }

                if ((menus[5][1] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Name;
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

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            spriteBatch.Draw(bg_Noise, new Rectangle(0, 0, bg_Noise.Width, bg_Noise.Height), Color.White);

            TL.Draw(spriteBatch);

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }
           // spriteBatch.Draw(arrow, new Rectangle(1110, 124, 16, 16), Color.White);

#if DEBUG
            spriteBatch.DrawString(ErasMediumITC14, "" + currentValg, Vector2.Zero, Color.White);
#endif
            spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
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

        //loads the next choice content.
        public void ReadValgContent()
        {
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "valg", "ID", currentValg);

            while (reader.Read())
            {
                //read  and assign dilemma text
                text_situation = (string)("" + reader["SpgTekst"]);
                (menus[2][0] as TextBox).Content = text_situation;

                //read dilemma and assign fact text
                text_fakta = "" + reader["FaktaTekst"];
                (menus[2][3] as TextBox).Content = text_fakta;

                //read Kontekst, assign later when textbox is out of sight.
                text_konFaktaTekst = "" + reader["KonFaktaTekst"];
            }

            //button content.
            reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", currentValg);

            //read and put into array.
            while (reader.Read())
            {
                svarValg[row] = reader["svarValg"].ToString();
                row++;
            }
            row = 0;

            //rassign from array when done reading.
            (menus[2][1] as Button).Content = svarValg[0];
            (menus[2][2] as Button).Content = svarValg[1];
        }

        public void ReadSvarContent(int index)
        {
            //This reads the consequences
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", currentValg);
            while (reader.Read())
            {
                konTekst[row] = reader["konTekst"].ToString();
                row++;
            }
            row = 0;

            //assign consequence fact text, now that we´ve scrolled past.
            //Read at ReadValgContent()
            (menus[2][8] as TextBox).Content = text_konFaktaTekst;

            //Assign the correct consequence text.
            (menus[2][6] as TextBox).Content = konTekst[index];

        }

        /// <summary>
        /// Navigates from questions to consequences, based on the user's answers
        /// </summary>
        /// <param name="btn"></param>
        public void MakeChoice(Button btn, int situationID)
        {
            for (int i = 0; i < 2; i++)
            {
                if (btn.Clicked)
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
            /*if(from < to + 0.2f || from > to - 0.2f)
            {
                return to;
            }*/
            return (to - from) * time;
        }

        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }
    }
}
