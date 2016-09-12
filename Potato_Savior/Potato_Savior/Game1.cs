using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Potato_Savior
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Button start;
        private Button exit;
        private Button about;
        private Button back;

        private Texture2D backTexture;
        private Rectangle backRectangle;

        private Song music;
        private SoundEffect sound;
        private SoundEffect click1;
        private SoundEffect click2;
        private SoundEffect gameover;
        private SoundEffect putIn;
        private SoundEffect plop;

        private SpriteFont MenuFont; //переменная со шрифтом главного меню. Поддержка русского включена в файле со шрифтом
        private SpriteFont endGameFont;

        private KeyboardState presentKey;//переменные состояния памяти, нужны для реализации одиночного нажатия по принципу нажал-отпустил
        private KeyboardState pastKey;

        int timeBugInterval = 0; //переменные которые будут использоваться при генерации элементов, по сути их обнуление ничего не даёт и устанавливать их здесь не имеет смысла
        int timePotatoInterval = 0;
        int timeAngelInterval = 0;

        int Points;
        int Kills;
        int Bulbas;

        bool pause;
        private MouseState mousePresent;//переменные состояния мыши, для одиночного нажатия
        private MouseState mousePast;

        private Plant Kust;
        private Bag Meshok;

        private BugTest spriteBug;

        private List<Bug> Bugs = new List<Bug>(); 

        private List<Potato> Potatos = new List<Potato>();

        private List<Angel> Angels = new List<Angel>();

        public enum GameScreens //игровые экраны, перечисление
        {
            MainMenu,  //главное меню
            GamePlay,    //игровой процесс
            GameOver,   //Экран завершения игры
            About       //Окно о программе
        }
        GameScreens CurrentScreen = GameScreens.MainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferWidth = 1366;
            graphics.PreferredBackBufferHeight = 768;
            IsMouseVisible = true;
            

        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            start = new Button(Content.Load<Texture2D>("кнопкастартанимация2"), new Vector2(1150, 475));
            about = new Button(Content.Load<Texture2D>("кнопкаанимацияэбаут2"), new Vector2(1150, 590));
            exit = new Button(Content.Load<Texture2D>("кнопкавыходанимация2"), new Vector2(1150, 705));
            back = new Button(Content.Load<Texture2D>("кнопканазаданимация"), new Vector2(150, 600));
            

            base.Initialize();
        }


        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            MenuFont = Content.Load<SpriteFont>("MenuFont"); //загрузка шрифта для меню
            endGameFont = Content.Load<SpriteFont>("endGameFont");

            music = Content.Load<Song>("тема 8бит");
            sound = Content.Load<SoundEffect>("удар");
            click1 = Content.Load<SoundEffect>("клик");
            click2  = Content.Load<SoundEffect>("клик2");
            putIn = Content.Load<SoundEffect>("нямзвук");
            plop = Content.Load<SoundEffect>("клик");
            //gameover = Content.Load<SoundEffect>("конец");

            MediaPlayer.Volume = 0.2f;
            MediaPlayer.Play(music);
            MediaPlayer.IsRepeating = true;

        }


        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            mousePresent = Mouse.GetState();
            presentKey = Keyboard.GetState();

            switch (CurrentScreen)//тела экранов
            {
                case GameScreens.MainMenu:

                    for (int i = 0; i < Bugs.Count; i++)
                    {
                            Bugs.RemoveAt(i);
                            i--;
                    }
                    Kust = new Plant(Content.Load<Texture2D>("кусткартошки"), new Rectangle(603, 320, 160, 128));
                    Kust.PlantPosition = new Vector2(603, 320);
                    Meshok = new Bag(Content.Load < Texture2D > ("мешоканимация2"));

                    Points = 0;
                    Kills = 0;
                    Bulbas = 0;

                    if (presentKey.IsKeyDown(Keys.Enter) && pastKey.IsKeyUp(Keys.Enter))
                        CurrentScreen = GameScreens.GamePlay;
                    if (presentKey.IsKeyDown(Keys.Escape) && pastKey.IsKeyUp(Keys.Escape))
                        Exit();
                    pause = false;

                    if(gameTime.TotalGameTime.TotalMilliseconds - timeAngelInterval >=300)
                    {
                        GenerateAngel();
                        timeAngelInterval = (int)gameTime.TotalGameTime.TotalMilliseconds;
                    }
                    for (int i = 0; i < Angels.Count; i++ )
                    {
                        Angels[i].Update(gameTime);
                        if(!Angels[i].isVisible)
                        {
                            Angels.RemoveAt(i);
                            i--;
                        }

                    }

                    start.Update(gameTime, mousePresent, mousePast, click1, click2);
                    if(start.IsClicked)
                    {
                        CurrentScreen = GameScreens.GamePlay;
                    }
                    about.Update(gameTime, mousePresent, mousePast, click1, click2);
                    if(about.IsClicked)
                    {
                        CurrentScreen = GameScreens.About;
                    }
                    exit.Update(gameTime, mousePresent, mousePast, click1, click2);
                    if(exit.IsClicked)
                    {
                        Exit();
                    }
                   

                        break;


                case GameScreens.GamePlay:

                    if (presentKey.IsKeyDown(Keys.Space) && pastKey.IsKeyUp(Keys.Space))
                        pause = !pause;
                    if (presentKey.IsKeyDown(Keys.Escape) && pastKey.IsKeyUp(Keys.Escape))
                        CurrentScreen = GameScreens.MainMenu;

                    
                   // if (presentKey.IsKeyDown(Keys.Space) && pastKey.IsKeyUp(Keys.Space))
                    if (!pause)
                    {
                        if (gameTime.TotalGameTime.TotalMilliseconds - timeBugInterval >= (1000-Kills*2)) //генерация жуков по таймеру
                        {
                            GenerateBug();
                            timeBugInterval = (int)gameTime.TotalGameTime.TotalMilliseconds;
                        }

                        if (gameTime.TotalGameTime.TotalMilliseconds - timePotatoInterval >= 3500) //генерация картошки по таймеру
                        {
                            GeneratePotato();
                            timePotatoInterval = (int)gameTime.TotalGameTime.TotalMilliseconds;
                        }

                        // GenerateBug();   очень много жуков:)
                        for (int i = 0; i < Bugs.Count; i++)
                        {

                            Bugs[i].Update(gameTime, Kust, mousePresent, mousePast, sound);
                            if (!Bugs[i].isVisible)
                            {

                                Bugs.RemoveAt(i);
                                Kills++;
                                i--;
                            }
                        }
                        Meshok.Update(gameTime, Potatos, mousePresent, mousePast, putIn);
                        for (int i = 0; i < Potatos.Count; i++)
                        {
                            Potatos[i].Update(mousePresent, mousePast);

                            if (!Potatos[i].isVisible)
                            {
                                Bulbas++;
                                Potatos.RemoveAt(i);
                                i--;
                            }
                        }
                        if (Kust.health <= 0)
                            CurrentScreen = GameScreens.GameOver;
                    } 
                   
                    break;



                case GameScreens.GameOver:
                    if (presentKey.IsKeyDown(Keys.Enter) && pastKey.IsKeyUp(Keys.Enter))
                        CurrentScreen = GameScreens.MainMenu;
                    Points = Kills + Bulbas * 5;
                    for (int i = 0; i < Bugs.Count; i++)
                    {
                        Bugs.RemoveAt(i);
                        i--;
                    }

                    back.Update(gameTime, mousePresent, mousePast, click1, click2);
                    if(back.IsClicked)
                    {
                        CurrentScreen = GameScreens.MainMenu;
                    }

                    break;

                case GameScreens.About:

                    back.Update(gameTime, mousePresent, mousePast, click1, click2);
                    if(back.IsClicked)
                    {
                        CurrentScreen = GameScreens.MainMenu;
                    }

                    break;                    

            }

            
            pastKey = presentKey;
            mousePast = mousePresent;

            base.Update(gameTime);
        }


        public void GenerateBug()//создание жуков       
        {
            Bug newBug = new Bug(Content.Load<Texture2D>("jukspriteV2"), 45, 49, Kust);

            Bugs.Add(newBug);
        }

        public void GeneratePotato()
        {
            Potato newPotato = new Potato(Content.Load<Texture2D>("картошка"), Kust);
            Potatos.Add(newPotato);
        }

        public void GenerateAngel()
        {
            Angel newAngel = new Angel(Content.Load<Texture2D>("ангелкартошка2"));
            Angels.Add(newAngel);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            switch (CurrentScreen) //отрисовка разных игровых экранов
            {
                case GameScreens.MainMenu:  //отрисовка главного меню
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Draw(Content.Load<Texture2D>("менюфон"), backRectangle = new Rectangle(0, 0, 1370, 772), Color.White);
                
                    foreach(Angel angel in Angels)
                    {
                        angel.Draw(spriteBatch);
                    }

                    start.Draw(spriteBatch);
                    about.Draw(spriteBatch);
                    exit.Draw(spriteBatch);
                   // spriteBatch.Draw(Content.Load<Texture2D>("кнопкастартанимация"), backRectangle = new Rectangle(400, 300, 500, 200), Color.White);
                    break;


                case GameScreens.GamePlay: //отрисовка основного игрового окна
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Draw(Content.Load<Texture2D>("почва"), backRectangle = new Rectangle(0, 0, 1370, 772), Color.White);
                    foreach(Bug colorad in Bugs)
                    {
                        
                        colorad.Draw(spriteBatch);           
                        
                    }

                    foreach(Potato potato in Potatos)
                    {
                        potato.Draw(spriteBatch);
                    }
                    Meshok.Draw(spriteBatch);
                    spriteBatch.DrawString(MenuFont, "Plant health " + Kust.health , new Vector2(20, 20), Color.White);
                    spriteBatch.DrawString(MenuFont, "Очки " + (Kills + Bulbas*5), new Vector2(1100, 20), Color.White);
                    spriteBatch.Draw(Kust.plantTexture, Kust.PlantPosition, null, Color.White);
                   
                    
                    break;

                case GameScreens.GameOver:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Draw(Content.Load<Texture2D>("почва"), backRectangle = new Rectangle(0, 0, 1370, 772), Color.White);
                    spriteBatch.DrawString(endGameFont, "Очков набрано " + Points, new Vector2(600, 370), Color.White);

                    back.Draw(spriteBatch);

                   
                    break;

                case GameScreens.About:
                    GraphicsDevice.Clear(Color.CornflowerBlue);
                    spriteBatch.Draw(Content.Load<Texture2D>("эбаутфон"), backRectangle = new Rectangle(0, 0, 1370, 772), Color.White);
                    back.Draw(spriteBatch);
                    
                    break;

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
