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
        //Enum to control the menu system
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
        // SQLite command og connection til databasen
        private SQLiteCommand dbComm;
        private SQLiteConnection dbConn;
        // Separat SQLite command og connection til highscore databasen
        private SQLiteCommand dbCommHs;
        private SQLiteConnection dbConnHs;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardEvents textInput = new KeyboardEvents();

        //Spritefonts for the text in the game
        public static SpriteFont ArialNarrow48;
        public static SpriteFont ErasMediumITC14;
        public static SpriteFont CopperPlateGothicLight48;
        public static SpriteFont CopperPlateGothicLight36;

        //textures used in the game
        public Texture2D arrow;
        public Texture2D inputBox;
        public Texture2D Main_Medium_Normal;
        public Texture2D InGameScreenshot640x353;
        public Texture2D bg_Noise;
        public Texture2D choice_button, choice_textbox, choice_divider;
        public Texture2D SidePanel_left, SidePanel_Right;
        public Texture2D Title;
        public Texture2D Rival_Silhouette;
        public Texture2D TLtest;
        public Texture2D Tutorial;
        public Texture2D GotIt;
        public Texture2D ColleaguePic;
        public Texture2D PlayerPic;
        public Texture2D SliderBlock;
        public Texture2D SliderBar;

        // Tutorial
        private Vector2[] tutorialPos = new Vector2[10];
        private bool[] tutActive = new bool[5] { true, true, true, true, true };
        private int currentTutorial = 0;
        private int slidingElements = 14;
        private Vector2[] positions;

        private float salaryChance = 0;
        public float SliderPercent = 0;
        private string highscore;
        private bool loaded = false;
        private int row = 0;
        private string[] answerchoice = new string[2];
        private string[] conText = new string[2];

        private Vector2 mouseDelta = Vector2.Zero;
        private Vector2 mouseLastPos = Vector2.Zero;

        private string text_situation = "", text_fakta = "", text_A = "", text_B = "", text_konFaktaTekst = "", text_konTekst;
        public int currentChoice = 1;
        private float colleagueSalary = 25000;
        private float playersalary = 25000;

        private int playerExperience = 0;
        private int colleagueExperience = 2;

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


            //Opens connection to the databases
            dbConn = new SQLiteConnection("Data Source=Content/TextContent.db;Version=3");
            dbComm = new SQLiteCommand();
            dbConn.Open();

            dbConnHs = new SQLiteConnection("Data Source=Content/HighScore.db;Version=3");
            dbCommHs = new SQLiteCommand();
            dbConnHs.Open();

            //Database.Functions.CreateDatabase("HighScore");
            //Database.Functions.ManualFunction(dbConn, dbComm, "CREATE TABLE IF NOT EXISTS 'Player' ('ID' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 'Name' TEXT,'Point' INTEGER'");
            //Database.Functions.ManualFunction(dbConn, dbComm, "CREATE TABLE IF NOT EXISTS 'Player' (ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, Name TEXT,Point INTEGER)");
            //Database.Functions.ManualFunction(dbConn, dbComm, "INSERT INTO 'Player' ('Name', 'Point'') VALUES ('Prosa vil hjælpe dig, hvis du udsættes for sexchikane på arbejdspladsen.', 'Din chef tager på dig. \nHvad vil du gøre?', 0, 1)");

            KeyboardEvents.KeyTyped += KeyTyped;
            IsMouseVisible = true;

            //Main Menu
            
            menus[0] = new GameObject[5];
            menus[0][0] = new Button(new Vector2(180, 200), "Start", CopperPlateGothicLight48, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[0][1] = new Button(new Vector2(180, 320), "Om spillet", CopperPlateGothicLight36, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[0][2] = new Button(new Vector2(180, 440), "Highscore", CopperPlateGothicLight36, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[0][3] = new Button(new Vector2(180, 560), "Afslut", CopperPlateGothicLight36, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[0][4] = new TextBox(new Vector2(0, 20), "", ArialNarrow48, Color.White, Title, new Vector2(150, 100), false);

            //Name input
            menus[1] = new GameObject[8];
            menus[1][0] = new TextBox(new Vector2(365, 242), " Navn:\n Erfaring: 0 år\n Løn: 25.000Kr.\n Fagforening: Nej", ErasMediumITC14, Color.White, 0, inputBox, new Vector2(180, 100), false);
            menus[1][1] = new TextBox(new Vector2(735, 242), " Navn: Karl Åge\n Erfaring: 2 år\n Løn: 25.000Kr.\n Fagforening: Ja ", ErasMediumITC14, Color.White, 0, inputBox, new Vector2(180, 100), false);
            menus[1][2] = new Button(new Vector2(640, 562), "Videre", ArialNarrow48, Color.Black, Main_Medium_Normal, new Vector2(480, 100), false);
            menus[1][3] = new Button(new Vector2(180, 562), "Tilbage", ArialNarrow48, Color.Black, Main_Medium_Normal, new Vector2(480, 100), false);
            menus[1][4] = new TextBox(new Vector2(370, 100), "", ArialNarrow48, Color.White, 0, PlayerPic, new Vector2(220, 100), false);
            menus[1][5] = new TextBox(new Vector2(740, 100), "", ArialNarrow48, Color.White, 0, ColleaguePic, new Vector2(220, 100), false);
            menus[1][6] = new TextBox(new Vector2(180, 342), "Karl Åge er din kollega, og har forud for ansættelsen på denne arbejdsplads, \n" +
                "haft noget erhvervserfaring inden for jobbet.\n" +
                "Karl Åge er derfor med i en fagforening, da han ved dette kan blive nødvendigt.\n" +
                "\nDu er dog ikke med i en, da du mener det er spild af penge, og sagtens kan klare det selv.", ErasMediumITC14, Color.White, 1, choice_textbox, new Vector2(920, 220), false);
            menus[1][7] = new TextBox(new Vector2(418, 250), name, ErasMediumITC14, Color.White, 0, choice_divider, new Vector2(125, 20), true);

            //Choice

            SetMenuPos();
            

            //HighScore
            menus[3] = new GameObject[2];
            menus[3][0] = new Button(new Vector2(180, 640), "Tilbage", ArialNarrow48, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[3][1] = new TextBox(new Vector2(480, 50), "", ErasMediumITC14, Color.White, 0, null, new Vector2(920, 600), false);

            //About
            menus[4] = new GameObject[3];
            menus[4][0] = new Button(new Vector2(1050, 650), "Back", ArialNarrow48, Color.White, Main_Medium_Normal, new Vector2(180, 100), false);
            menus[4][1] = new TextBox(new Vector2(600, 200), "Om Spillet.\n\n Du er blevet ansat i EDB i Skyen sammen med Karl Åge.\n EDB i Skyen er en lille IT-virksomhed som arbejder med support og IT-løsninger til andre IT-firmaer.\n Virksomheden har eksisteret i 2 år, og salget går fremad.\n\n Du står nu med et arbejde men uden en fagforening eller A-kasse, og bliver nu udsat for den hårdeste arbejdsmåned i dit liv.\n Med de mest mærkværdige udfordringer en person kunne tænkes at blive udsat for, i løbet af arbejdslivet. \n\n Det er nu din opgave at klare dig gennem arbejdet, UDEN hjælp fra en fagforening, \n for at se hvordan arbejdet kunne se ud, hvis du stod uden en. \n\nDu vil på samme tid skulle kæmpe mod din kollega, og se hvem der kan få sin løn højest, sammen med de mærkværdige udfordringer.", ErasMediumITC14, Color.White, null, new Vector2(170, 0), false);
            menus[4][2] = new TextBox(new Vector2(350, 360), " ", ErasMediumITC14, Color.White, InGameScreenshot640x353, new Vector2(640, 353), false);

            //ContinouePromt
            menus[5] = new GameObject[2];
            menus[5][0] = new Button(new Vector2(180, 160), "Fortsæt", ArialNarrow48, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[5][1] = new Button(new Vector2(180, 260), "Nyt Spil", ArialNarrow48, Color.Black, choice_button, new Vector2(920, 100), false);

            menuState = Menu.Main;
            //MenuToggle();



            if (currentChoice == 1)
            {
                MenuToggle();
            }

            ReadchoiceContent();
            //changeTutorial(0, new Vector2(500, 200), "Læs situationen igennem.\nTryk derefter på en af\nvalgmulighederne nedenfor");
        }
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used tos draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Henter alle teksturer og fonte til spillet
            CopperPlateGothicLight48 = Content.Load<SpriteFont>("CopperPlate Gothic Light 48");
            CopperPlateGothicLight36 = Content.Load<SpriteFont>("CopperPlate Gothic Light 36");
            ArialNarrow48 = Content.Load<SpriteFont>("ArialNarrow48");
            ErasMediumITC14 = Content.Load<SpriteFont>("Eras Medium ITC");

            Title = Content.Load<Texture2D>("Title");

            SidePanel_left = Content.Load<Texture2D>("SidePanel_Left");
            SidePanel_Right = Content.Load<Texture2D>("SidePanel_Right");
            arrow = Content.Load<Texture2D>("Arrow");

            bg_Noise = Content.Load<Texture2D>("bg_LightGreyNoise");
            inputBox = Content.Load<Texture2D>("input_box");
            Main_Medium_Normal = Content.Load<Texture2D>("Btn_Normal_Main_Medium");
            InGameScreenshot640x353 = Content.Load<Texture2D>("InGameScreenshot640x353");
            TLtest = Content.Load<Texture2D>("TimeLineTest");
            ColleaguePic = Content.Load<Texture2D>("Silhouette");
            PlayerPic = Content.Load<Texture2D>("PlayerPicture");

            choice_textbox = Content.Load<Texture2D>("Panel_Textbox");
            choice_button = Content.Load<Texture2D>("Panel_Button_normal");
            choice_divider = Content.Load<Texture2D>("Divider");
            Rival_Silhouette = Content.Load<Texture2D>("Silhouette");
            SliderBlock = Content.Load<Texture2D>("SliderBlock");
            SliderBar = Content.Load<Texture2D>("SliderBar");

            Tutorial = Content.Load<Texture2D>("bck_Tutorial");
            GotIt = Content.Load<Texture2D>("bck_GotIt");

            //timeLine = new TimeLine(new Vector2(1100, 50));
            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        public bool move = false;

        protected override void Update(GameTime gameTime)
        {
            //Checks if it is the first time playing
            if (currentChoice != 1)
            {
                (menus[0][0] as Button).Content = "Fortsæt/Nyt Spil";
            }

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            // Scrolling
            if (move)
            {
                // Get all moveable items
                for (int i = 0; i < 14; i++)
                {
                    // Move the items upwards
                    menus[2][i].Position += new Vector2(0, Lerp(menus[2][i].Position.Y, positions[i].Y, deltaTime * 2) - 5);
                    // Check if the last item is being checked
                    if (i == 13)
                    {
                        // Check if the last item has reached its destination
                        if (menus[2][i].Position.Y <= positions[i].Y)
                        {
                            // Get all the items again
                            for (int j = 0; j < 14; j++)
                            {
                                // All items that has past the top boundry, get returned to bottom
                                if (menus[2][j].Position.Y + (menus[2][j] as TextBox).size.Y <= 0)
                                {
                                    menus[2][j].Position = positions[j];   // Snap to position
                                    menus[2][j].Position += new Vector2(0, 1440);
                                }
                            }
                            move = false;   // Disable move
                        }
                    }
                }
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
            // If the tutoral button is pressed
            if ((menus[2][22] as Button).Clicked && (menus[2][22] as Button).visible)
            {
                if (tutActive[currentTutorial])
                {
                    tutActive[currentTutorial] = false;
                    currentTutorial++;
                    (menus[2][21] as TextBox).visible = false;
                    (menus[2][22] as Button).visible = false;
                    Illumination();
                }
                (menus[2][22] as Button).Clicked = false;
            }
            #region Main:
            if (menuState == Menu.Main)
            {
                //Start
                if ((menus[0][0] as Button).Clicked)
                {
                    MenuToggle();
                    if (currentChoice == 1)
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
            #endregion
            #region Highscore:
            if (menuState.Equals(Menu.Highscore))
            {
                if (!loaded)
                {
                    SQLiteDataReader reader = Database.Functions.TableSelectAllDescending(dbConnHs, dbCommHs, "Player", "Point");

                    while (reader.Read())
                    {
                        //ID
                        for (int j = 0; j < 2 - (row + 1).ToString().Length; j++)
                        {
                            highscore += "  ";
                        }

                        highscore += (row + 1).ToString();

                        for (int j = 0; j < 10; j++)
                        {
                            highscore += "  ";
                        }

                        //Name
                        highscore += reader[1].ToString();

                        for (int j = 0; j < (20 - reader[1].ToString().Length); j++)
                        {
                            highscore += "  ";
                        }

                        //Point
                        highscore += (reader[2].ToString());
                        //highscore += reader[2].ToString();

                        highscore += "\n";
                        row++;
                    }
                    row = 0;

                    (menus[3][1] as TextBox).Content = "" + highscore;

                    loaded = true;
                }

                if ((menus[3][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }
            #endregion
            #region About:
            if (menuState.Equals(Menu.About))
            {
                if ((menus[4][0] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }
            #endregion
            #region Name
            if (menuState.Equals(Menu.Name))
            {
                currentChoice = 1;
                textInput.HandleKeyUpdate(gameTime);
                (menus[1][7] as TextBox).Content = name;

                if ((menus[1][2] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Choice;
                    slidingElements = 14;
                    playersalary = 25000;
                    colleagueSalary = 25000;
                    (menus[2][16] as TextBox).Content = "Navn: Karl Åge\nErfaring: 2 år\nLøn: " + colleagueSalary + "kr.\nFagforening: Ja";
                    (menus[2][18] as TextBox).Content = "Navn:" + name + "\nErfaring: 0 år\nLøn: " + playersalary + "Kr.\nFagforening: Nej";
                    MenuToggle();
                }

                if ((menus[1][3] as Button).Clicked)
                {
                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                }
            }
            #endregion
            #region Choice:
            if (menuState.Equals(Menu.Choice))
            {
                /*changeTutorial(0, new Vector2(200, 200), "Læs situationen igennem.\nTryk derefter på en af\nvalgmulighederne nedenfor");
                if ((menus[2][1] as Button).Pressed)
                {
                    (menus[2][0] as TextBox).backGroundColor = Color.LightGray;
                }
                else
                {
                    (menus[2][0] as TextBox).backGroundColor = Color.White;
                }*/

                changeTutorial(0, new Vector2(640 - 175, 330), "Læs situationen igennem.\nTryk derefter på en af\nvalgmulighederne nedenfor");
                //JA
                if ((menus[2][1] as Button).Clicked && !(menus[2][21] as TextBox).visible && !move)
                {
                        ReadAnswerContent(0);
                    GetPlayerSalary(currentChoice);
                    move = true;
                    MoveElements();
                    (menus[2][1] as Button).Clicked = false;

                }
                //Nej
                if ((menus[2][2] as Button).Clicked && !(menus[2][21] as TextBox).visible && !move)
                {
                    ReadAnswerContent(1);
                    move = true;
                    MoveElements();
                    (menus[2][2] as Button).Clicked = false;
                }

                //videre
                if ((menus[2][7] as Button).Clicked && !move)
                {
                    currentChoice++;
                    (menus[2][20] as TimeLine).NewEvent(currentChoice);
                    (menus[2][7] as Button).Clicked = false;
                    switch (currentChoice)
                    {
                        case 5:
                            colleagueSalary = (float)SalaryCalc(0.3, colleagueSalary);
                            break;
                        case 12:
                            colleagueSalary = (float)SalaryCalc(0.3, colleagueSalary);
                            break;
                        case 18:
                            colleagueSalary = (float)SalaryCalc(0.3, colleagueSalary);
                            break;
                        case 26:
                            colleagueSalary = (float)SalaryCalc(0.3, colleagueSalary);
                            break;

                        default:
                            break;
                    }
                    (menus[2][16] as TextBox).Content = "Navn: Karl Åge\nErfaring: 2 år\nLøn: " + colleagueSalary.ToString() + " Kr. \nFagforening: Ja";
                    ReadchoiceContent();
                    //ReadValgContent(currentChoice);



                    if (currentChoice != 9)
                    {
                        MoveElements();
                        move = true;
                    }

                    if (salaryChance >= SliderPercent && currentChoice == 5 || currentChoice == 9)
                    {
                        playersalary += playersalary * (SliderPercent / 100);
                        (menus[2][18] as TextBox).Content = "Navn:" + name + "\nErfaring: 0 år\nLøn: " + playersalary + "Kr.\nFagforening: Nej";
                        salaryChance = 0;
                    }
                }

                if(currentChoice >= 9)
                {
                    
                    if (menus[2][0].Position.Y > -1000)
                    {
                        for (int i = 0; i < menus[2].Length-1; i++)
                        {
                            menus[2][i].Position += new Vector2(0, Lerp(menus[2][i].Position.Y, -1440, deltaTime * 1) - 5);
                        }
                    }
                    
                    
                }

                if ((menus[2][24] as Button).Clicked)
                {
                    if (currentChoice >= 9)
                    {
                        Database.Functions.ManualFunction(dbConnHs, dbCommHs, "Insert into Player (Name,Point) values ('" + name + "','" + playersalary + "')");
                        currentChoice = 1;
                    }

                    MenuToggle();
                    menuState = Menu.Main;
                    MenuToggle();
                    SetMenuPos();
                    ReadchoiceContent();

                }

                if (currentChoice == 4 || currentChoice == 8)
                {
                    if (!move)
                    {
                        changeTutorial(1, new Vector2(640 - 175, 330), "Du kan nu justere din\nønskede lønforhøjelse.\nChefen afgøre om det er i orden.");
                    }
                    (menus[2][11] as TextBox).visible = true;
                    (menus[2][12] as Button).visible = true;
                    (menus[2][13] as TextBox).visible = true;
                    if ((menus[2][12] as Button).Pressed && !(menus[2][21] as TextBox).visible && !move)
                    {

                        mouseDelta = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y) - new Vector2(mouseLastPos.X, mouseLastPos.Y);

                        if (menus[2][12].Position.X + (menus[2][12] as Button).size.X / 2 > 240 - mouseDelta.X && menus[2][12].Position.X + (menus[2][12] as Button).size.X / 2 < 1040 - mouseDelta.X)
                        {
                            menus[2][12].Position += new Vector2(mouseDelta.X, 0);
                        }

                        //(menus[2][12] as Button).Content = "" + Math.Round((menus[2][12].Position.X+(menus[2][12] as Button).size.X/2 - 240) / (795)*5,1);

                    }
                    SliderPercent = (float)Math.Round((menus[2][12].Position.X + (menus[2][12] as Button).size.X / 2 - 240) / (795) * 5, 1);
                    (menus[2][13] as TextBox).Content = SliderPercent + "%";
                    mouseLastPos = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
                }
                else
                {
                    (menus[2][13] as TextBox).visible = false;
                    (menus[2][11] as TextBox).visible = false;
                    (menus[2][12] as Button).visible = false;
                }
            }
            #endregion
            #region ContinuePromt:
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
                    playersalary = 25000;
                    colleagueSalary = 25000;
                    ResetTutorials();
                    currentChoice = 1;
                    ReadchoiceContent();
                    menuState = Menu.Name;
                    MenuToggle();
                }
            }
            #endregion
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

            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }
            // spriteBatch.Draw(arrow, new Rectangle(1110, 124, 16, 16), Color.White);
            if (menuState.Equals(Menu.Choice) && currentChoice < 9)
            {
                spriteBatch.DrawString(ErasMediumITC14, "Din Kollega", new Vector2(40, 430), Color.DarkRed);
                spriteBatch.DrawString(ErasMediumITC14, "Dig", new Vector2(75, 140), Color.DarkRed);

            }

            if (menuState.Equals(Menu.Choice) && currentChoice >= 9)
            {
                spriteBatch.DrawString(ErasMediumITC14,"Dette var fagforeningsspillet. Vi håber oplevelsen var lærerig, og du måske vil prøve det igen?\n\n" +
                                       "Din slutløn endte på :" + playersalary.ToString() +
                                       "\n\nKarl Åge endte med :" + colleagueSalary.ToString() +
                                       "\n\nLønnen betyder dog ikke alt, og i burde have fået samme løn gennem en overenskomst. " +
                                       "\nSå i burde være gode kollegaer, i stedet for rivaler.",new Vector2(200,200),Color.White);
            }
#if DEBUG
            spriteBatch.DrawString(ErasMediumITC14, "Valg: " + currentChoice, Vector2.Zero, Color.White);
#endif
            spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        //Checks what key is typed, to fill in the name string
        void KeyTyped(object sender, KeyboardEventArgs e)
        {
            if (e.character.HasValue && ErasMediumITC14.MeasureString("Navn:" + name).X < 165)
            {
                name += e.character.Value;
            }

            if (e.key == Keys.Back && name.Length > 0)
            {
                name = name.Substring(0, name.Length - 1);
            }
        }

        //loads the next choice content.
        public void ReadchoiceContent()
        {
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "valg", "ID", currentChoice);

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
            reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", currentChoice);

            //read and put into array.
            while (reader.Read())
            {
                answerchoice[row] = reader["svarValg"].ToString();
                row++;
            }
            row = 0;

            //rassign from array when done reading.
            (menus[2][1] as Button).Content = "" + answerchoice[0];
            (menus[2][2] as Button).Content = "" + answerchoice[1];
        }

        public void ReadAnswerContent(int index)
        {
            //This reads the consequences
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", currentChoice);
            while (reader.Read())
            {
                conText[row] = reader["konTekst"].ToString();
                row++;
            }
            row = 0;

            //assign consequence fact text, now that we´ve scrolled past.
            //Read at ReadValgContent()
            (menus[2][8] as TextBox).Content = text_konFaktaTekst;

            //Assign the correct consequence text.
            (menus[2][6] as TextBox).Content = conText[index];

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
                    // Remove the object from the gameobjects list
                    gameObjects.Remove(menus[(int)menuState][i]);
                }
                else
                {
                    // Otherwise add the button
                    gameObjects.Add(menus[(int)menuState][i]);
                }
            }
        }

        public void GetPlayerSalary(int index)
        {
            SQLiteDataReader reader = Database.Functions.TableSelectRow(dbConn, dbComm, "konsekvens", "valgID", index);
            while (reader.Read())
            {
                salaryChance += int.Parse(reader["salary"].ToString());
            }
        }

        //Makes the tutorial box
        private void changeTutorial(int tutNumber, Vector2 position, string text)
        {
            TextBox tut = (TextBox)menus[2][21];
            Button but = (Button)menus[2][22];
            if (tutActive[tutNumber])
            {
                if (!tut.visible)
                {
                    tut.visible = true;
                    but.visible = true;
                }
                Darkness(new GameObject[2] { menus[2][21], menus[2][22] });
                tut.Position = position;
                tut.Content = text;
                but.Position = position + new Vector2(105, 150);
            }
        }
        public float Lerp(float from, float to, float time)
        {
            return (to - from) * time;
        }

        //Method to get a random number, using a minimum and a maximum value
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        //Calculates the salary
        public double SalaryCalc(double koefficient, double salary)
        {
            double rnd = GetRandomNumber(0.03, koefficient);
            return (double)Math.Round((salary * rnd) + salary);
            //ColleagueSalary = (ColleagueSalary * rnd) + ColleagueSalary;
            //ColleagueSalary = (double)Math.Round((decimal)ColleagueSalary, 0);
        }

        //Makes everything behind the tutorial prompt dimm 
        private void Darkness(GameObject[] objects)
        {
            foreach (GameObject obj in menus[2])
            {
                if (!objects.Contains<GameObject>(obj))
                {
                    if (obj is TextBox)
                    {
                        (obj as TextBox).backGroundColor = Color.DarkGray;
                        (obj as TextBox).fontColor = Color.DarkGray;
                    }
                }
            }
        }

        //Adds more effect to the darkness method
        private void Illumination()
        {
            foreach (GameObject obj in menus[2])
            {
                if (obj is TextBox)
                {
                    (obj as TextBox).backGroundColor = Color.White;
                    if (obj is Button)
                    {
                        (obj as TextBox).fontColor = Color.Black;
                    }
                    else
                    {
                        (obj as TextBox).fontColor = Color.White;
                    }
                }
            }
        }

        //Resets the tutorials
        private void ResetTutorials()
        {
            currentTutorial = 0;
            for (int i = 0; i < tutActive.Length; i++)
            {
                tutActive[i] = true;
            }
        }

        //used to move elements on the screen
        private void MoveElements()
        {
            //positions = new Vector2[slidingElements+3];
            positions = new Vector2[menus[2].Length - 8];
            for (int i = 0; i < positions.Length - 1; i++)
            {
                positions[i] = menus[2][i].Position - new Vector2(0, 720);
            }
        }

        private void SetMenuPos()
        {
            menus[2] = new GameObject[25];

            menus[2][0] = new TextBox(new Vector2(180, 40), "" + text_situation, ErasMediumITC14, Color.White, choice_textbox, new Vector2(920, 220), false);
            menus[2][1] = new Button(new Vector2(180, 40 + 220), "" + text_A, ArialNarrow48, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[2][2] = new Button(new Vector2(180, 40 + 220 + 100), "" + text_B, ArialNarrow48, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[2][3] = new TextBox(new Vector2(180, 40 + 220 + 100 + 100), "" + text_fakta, ErasMediumITC14, Color.White, choice_textbox, new Vector2(920, 220), false);

            //Consequence
            menus[2][4] = new TextBox(new Vector2(180, 720 + 220 + 40), "", ErasMediumITC14, Color.White, choice_divider, new Vector2(920, 80), false);
            menus[2][5] = new TextBox(new Vector2(180, 720 + 220 + 160), "", ErasMediumITC14, Color.White, choice_divider, new Vector2(920, 80), false);

            menus[2][6] = new TextBox(new Vector2(180, 40 + 720), "" + text_konTekst, ErasMediumITC14, Color.White, choice_textbox, new Vector2(920, 220), false);
            menus[2][7] = new Button(new Vector2(180, 40 + 720 + 220 + 50), "Videre", ArialNarrow48, Color.Black, choice_button, new Vector2(920, 100), false);
            menus[2][8] = new TextBox(new Vector2(180, 40 + 720 + 220 + 50 + 100 + 50), "" + text_konFaktaTekst, ErasMediumITC14, Color.White, choice_textbox, new Vector2(920, 220), false);

            menus[2][9] = new TextBox(new Vector2(180, -40), "", ErasMediumITC14, Color.White, choice_divider, new Vector2(920, 80), false);
            menus[2][10] = new TextBox(new Vector2(180, 720 - 40), "", ErasMediumITC14, Color.White, choice_divider, new Vector2(920, 80), false);
            menus[2][11] = new TextBox(new Vector2(240, 500), "", ErasMediumITC14, Color.White, SliderBar, new Vector2(50, 80), false);
            menus[2][12] = new Button(new Vector2(400, 480), "", ErasMediumITC14, Color.White, SliderBlock, new Vector2(50, 80), false);

            menus[2][13] = new TextBox(new Vector2(640, 630), SliderPercent + "%", ArialNarrow48, Color.White, null, new Vector2(0, 0), false);
            menus[2][14] = new TextBox(new Vector2(0, 0), "", ErasMediumITC14, Color.White, SidePanel_left, Vector2.Zero, false);
            menus[2][17] = new TextBox(new Vector2(1100, 0), "", ErasMediumITC14, Color.White, SidePanel_Right, Vector2.Zero, false);

            //Karl picture and information
            menus[2][15] = new TextBox(new Vector2(5, 450), "", ErasMediumITC14, Color.White, Rival_Silhouette, Vector2.Zero, false);
            menus[2][16] = new TextBox(new Vector2(5, 600), "Navn: Karl Åge\nErfaring: 2 år\nLøn: 25000 kr.\nFagforening: Ja", ErasMediumITC14, Color.White, 0, null, new Vector2(170, 70), false);

            //Player picture and information
            menus[2][23] = new TextBox(new Vector2(5, 160), "", ErasMediumITC14, Color.White, PlayerPic, new Vector2(170, 0), false);
            menus[2][18] = new TextBox(new Vector2(5, 345), "Navn:" + name + "\nErfaring: 0 år\nLøn: " + playersalary + "Kr.\nFagforening: Nej", ErasMediumITC14, Color.White, 0, null, new Vector2(170, 0), false);

            menus[2][24] = new Button(new Vector2(-920 + 175, 0), "Menu" + text_B, ArialNarrow48, Color.Black, choice_button, new Vector2(1650, 100), false);
            menus[2][19] = new TextBox(new Vector2(1100, 100), "", ErasMediumITC14, Color.White, TLtest, new Vector2(180, 25), false);

            // Tutorial & Info
            menus[2][21] = new TextBox(new Vector2(0, 0), "<Tutorial>", ErasMediumITC14, Color.White, 1, Tutorial, new Vector2(350, 100), false);
            menus[2][22] = new Button(new Vector2(0, 0), "OK", ArialNarrow48, Color.Black, GotIt, new Vector2(141, 53), false);

            menus[2][20] = new TimeLine(Vector2.Zero);

            (menus[2][21] as TextBox).visible = false;
            (menus[2][22] as Button).visible = false;
            (menus[0][0] as Button).Content = "Nyt Spil";
        }
    }
}
