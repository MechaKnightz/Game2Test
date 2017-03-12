using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Game2Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Rectangle pilRectangle;
        Texture2D pilTexture, rockTexture, shotTexture;
        Vector2 pilOrigin, pilPos;
        float pilRotation, pilRotationRender;
        KeyboardState keyState, oldState;
        List<float> shotRotation = new List<float>();
        List<Rectangle> shots = new List<Rectangle>();
        int sida = 21;
        SpriteFont font;
        int maxShotCount = 5;
        List<Rectangle> rocks = new List<Rectangle>();
        System.Random rnd;
        int score, lives;
        Color[] menuColor = new Color[3];
        int selected = 0;
        int defaultScore = 0;
        int defaultLives = 999;
        Vector2 defaultShipPos;
        Rectangle speedbarRectangle, speedbarRectangle2;
        int speed;
        int defaultSpeed = 5;
        int menuLength = 3;
        int settingsMenuLength = 2;
        ParticleEngine particleEngine;
        bool moving = false;
        bool drawParticles = false;
        int movingDelayCounter = 0;
        List<int> highscores = new List<int>();
        GameState gameState = (GameState)2;
        int shipSize = 41;
        Vector2 shotOrigin;
        int halfScreenX, halfScreenY;



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
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

            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            graphics.PreferredBackBufferWidth = 801;  // set this value to the desired width of your window
            graphics.PreferredBackBufferHeight = 701;   // set this value to the desired height of your window
            graphics.ApplyChanges();

            halfScreenX = graphics.PreferredBackBufferWidth / 2;
            halfScreenY = graphics.PreferredBackBufferHeight / 2;

            Sprite ship = new Sprite(pilTexture, new Vector2(halfScreenX, halfScreenY), pilRectangle, pilRotation, pilOrigin);

            pilTexture = Content.Load<Texture2D>("ship2");
            rockTexture = Content.Load<Texture2D>("rocks");
            shotTexture = Content.Load<Texture2D>("shot");
            pilRectangle = new Rectangle(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2, shipSize, shipSize);
            pilPos = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);
            pilOrigin = new Vector2(shipSize/2,shipSize/2);
            IsMouseVisible = true;
            font = Content.Load<SpriteFont>("font");
            rnd = new System.Random();
            speedbarRectangle = new Rectangle((graphics.PreferredBackBufferWidth/2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301,20);
            speedbarRectangle2 = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301,20);
            ResetGame();
            speed = defaultSpeed;
            defaultShipPos = new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2);
            shotOrigin = new Vector2(sida / 2, sida / 2);

            //particles
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("red"));
            textures.Add(Content.Load<Texture2D>("orange"));
            textures.Add(Content.Load<Texture2D>("yellow"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));



            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch(gameState)
            {
                case GameState.MainGame: //main game
                    GameLogic();
                    break;
                case GameState.EndScreen: //gameover
                    EndScreenLogic();
                    break;
                case GameState.MainMenu: //main menu
                    ChangeColor();
                    MenuLogic();
                    break;
                case GameState.SettingsMenu: //settings
                    ChangeColor();
                    SettingsLogic();
                    break;
            }

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

            switch(gameState)
            {
                case GameState.MainGame:
                    DrawGame();
                    break;
                case GameState.EndScreen:
                    DrawEndScreen();
                    break;
                case GameState.MainMenu:
                    DrawMenu();
                    break;
                case GameState.SettingsMenu:
                    DrawSettings();
                    break;
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            if (moving) movingDelayCounter = 5;
            else movingDelayCounter--;
            if (movingDelayCounter <= 0) drawParticles = false;
            else drawParticles = true;
            moving = false;
            
            oldState = Keyboard.GetState();
            base.Draw(gameTime);
        }
        Rectangle GenerateRock()
        {
            System.Random rnd = new System.Random();

            int sidaRnd = rnd.Next(0, 5);
            int rndX = rnd.Next(0, graphics.PreferredBackBufferWidth);
            int rndY = rnd.Next(0, graphics.PreferredBackBufferHeight);
            Rectangle rock = new Rectangle(0,0,sida,sida);

            switch(sidaRnd)
            {
                case 0:
                    rock = new Rectangle(rndX, 0, sida, sida);
                    break;
                case 1:
                    rock = new Rectangle(graphics.PreferredBackBufferWidth, rndY, sida, sida);
                    break;
                case 2:
                    rock = new Rectangle(rndX, graphics.PreferredBackBufferHeight, sida, sida);
                    break;
                case 3:
                    rock = new Rectangle(0, rndY, sida, sida);
                    break;
            }
            return rock;
        }
        void CollisionTest() //kollar om shots intersects med rocks
        {
            for (int i = 0; i < shots.Count; i++)
            {
                for (int j = 0; j < rocks.Count; j++)
                {
                    if (shots[i].Intersects(rocks[j]))
                    {
                        shots.RemoveAt(i);
                        rocks.RemoveAt(j);
                        shotRotation.RemoveAt(i);
                        score++;
                        break;
                    }
                }
            }
        }
        void CollisionTest2() //kollar om rocks intersects med pilRectangle
        {
            for(int i = 0; i < rocks.Count; i++)
            {
                if(rocks[i].Intersects(pilRectangle))
                {
                    lives--;
                    rocks.RemoveAt(i);
                    if (lives <= 0)
                    {
                        ChangeState(1);
                        highscores.Add(score);//add fixerino LEjalkafsdbd
                    }
                }
            }
        }
        void GameLogic()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up))
            {
                //pilRectangle.X += System.Convert.ToInt16(System.Math.Cos(pilRotation)) * 10;
                //pilRectangle.Y += System.Convert.ToInt16(System.Math.Sin(pilRotation)) * 10;
                pilPos.X += (float)(System.Math.Cos(pilRotation)) * 10;
                pilPos.Y += (float)(System.Math.Sin(pilRotation)) * 10;
                pilRectangle.X = (int)pilPos.X - pilRectangle.Width/2;
                pilRectangle.Y = (int)pilPos.Y - pilRectangle.Height/2;
                moving = true;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                pilRectangle.X -= System.Convert.ToInt16(System.Math.Cos(pilRotation));
                pilRectangle.Y -= System.Convert.ToInt16(System.Math.Sin(pilRotation));
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Left)) pilRotation -= 0.1f;
            if (keyState.IsKeyDown(Keys.Right)) pilRotation += 0.1f;

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R))
            {
                pilPos.X = 401;
                pilPos.Y = 241;
            }


            if (keyState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space) && shots.Count < maxShotCount)
            {
                shots.Add(new Rectangle((int)pilPos.X, (int)pilPos.Y, sida, sida));
                shotRotation.Add(pilRotation);

            }
            for (int i = 0; i < shots.Count; i++)
            {
                Rectangle temp = shots[i];
                temp.X += System.Convert.ToInt16(System.Math.Cos(shotRotation[i])) * 10;
                temp.Y += System.Convert.ToInt16(System.Math.Sin(shotRotation[i])) * 10;
                shots[i] = temp;
            }
            for (int i = 0; i < shots.Count; i++)
            {
                if ((shots[i].X <= 0) || (shots[i].X >= graphics.PreferredBackBufferWidth) ||
                    (shots[i].Y <= 0) || (shots[i].Y >= graphics.PreferredBackBufferHeight))
                {
                    shots.RemoveAt(i);
                    shotRotation.RemoveAt(i);

                    //rectangle är structs, kolla upp Lists
                }
            }

            if (rnd.Next(0, 1001) > 990) //rock genererare
            {
                rocks.Add(GenerateRock());
            }
            for (int i = 0; i < rocks.Count; i++) //rock flyttare
            {
                Rectangle temp = rocks[i];

                if (temp.X < pilPos.X) temp.X += speed;
                else if (temp.X > pilPos.X) temp.X -= speed;

                if (temp.Y < pilPos.Y) temp.Y += speed;
                else if (temp.Y > pilPos.Y) temp.Y -= speed;

                //int skillnadX = pilRectangle.X - temp.X;
                //int skillnadY = pilRectangle.Y - temp.Y;
                //temp.X += System.Convert.ToInt16(0.05f * skillnadX);
                //temp.Y += System.Convert.ToInt16(0.05f * skillnadY);

                rocks[i] = temp;
            }

            if (pilRotation < 2 * System.Math.PI) pilRotation -= 2 * (float)System.Math.PI;

            particleEngine.EmitterLocation = pilPos;
            particleEngine.Update();

            CollisionTest();
            CollisionTest2();
        }
        void MenuLogic()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up)) selected--;
            if (keyState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down)) selected++;
            if (selected < 0) selected = menuLength-1 ;
            if (selected > menuLength-1) selected = 0;
            if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                switch(selected)
                {
                    case 0:
                        ResetGame();
                        ChangeState(0);
                        break;
                    case 1:
                        ChangeState(3);
                        break;
                    case 2:
                        Exit();
                        break;
                }
            }

            oldState = Keyboard.GetState();
        }
        void EndScreenLogic()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R)) ChangeState(2);

            oldState = Keyboard.GetState();
        }

        private void SettingsLogic()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up)) selected--;
            if (keyState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down)) selected++;
            if (selected < 0) selected = settingsMenuLength-1;
            if (selected > settingsMenuLength - 1) selected = 0;

            if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter)) if (selected == 0) ChangeState(2);

            if (keyState.IsKeyDown(Keys.Left) && selected == 1 && speed <= 101 && speed > 1) speed--;
            if (keyState.IsKeyDown(Keys.Right) && selected == 1 && speed < 101 && speed >= 1) speed++;

            speedbarRectangle.Width = speed;

            oldState = Keyboard.GetState();
        }
        void DrawEndScreen()
        {
            spriteBatch.DrawString(font, "Final Score: " + score.ToString(), new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2) - (font.MeasureString("Final Score: " + score.ToString()) /2), Color.Black);
            spriteBatch.DrawString(font, "Press R to reset.", new Vector2(10, 10), Color.Black);
        }
        void DrawGame()
        {
            spriteBatch.End();

            if(drawParticles) particleEngine.Draw(spriteBatch);

            spriteBatch.Begin();

            //pilRotationRender = (float)(Math.Round(pilRotation / (Math.PI / 4)) * (Math.PI / 4));
            //spriteBatch.Draw(pilTexture, new Vector2(pilRectangle.X, pilRectangle.Y), rotation: pilRotation, origin: pilOrigin);
            spriteBatch.Draw(pilTexture, pilPos, rotation: pilRotation, origin: pilOrigin);

            for (int i = 0; i < shots.Count; i++)
            {
                spriteBatch.Draw(shotTexture, new Vector2(shots[i].X, shots[i].Y), rotation: shotRotation[i], origin: shotOrigin);
            }
            for (int i = 0; i < rocks.Count; i++)
            {
                spriteBatch.Draw(rockTexture, rocks[i], Color.White);
            }

            spriteBatch.DrawString(font, "Score: " + score.ToString(), new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(font, "Lives: " + lives, new Vector2(10, 40), Color.Black);
        }
        void DrawMenu()
        {
            spriteBatch.DrawString(font, "Begin", new Vector2(graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 5)*2) - (font.MeasureString("Begin") / 2), menuColor[0]);
            spriteBatch.DrawString(font, "Settings", new Vector2(graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 5) * 3) - (font.MeasureString("Settings") / 2), menuColor[1]);
            spriteBatch.DrawString(font, "Quit", new Vector2(graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 5) * 4) - (font.MeasureString("Quit") / 2), menuColor[2]);
        }
        void DrawSettings()
        {
            spriteBatch.DrawString(font, "Back", new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.MeasureString("Back").X / 2), (graphics.PreferredBackBufferHeight / 2) - font.MeasureString("Speed").Y), menuColor[0]);
            spriteBatch.DrawString(font, "Speed", new Vector2(graphics.PreferredBackBufferWidth / 2 -(font.MeasureString("Speed").X / 2), (graphics.PreferredBackBufferHeight  /2)), menuColor[1]);
            spriteBatch.Draw(pilTexture, speedbarRectangle2, Color.White);
            spriteBatch.Draw(rockTexture, speedbarRectangle, Color.White);
            spriteBatch.DrawString(font, speed.ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - (font.MeasureString(speed.ToString()).X / 2), (graphics.PreferredBackBufferHeight / 2) + 20 +(font.MeasureString("Speed").Y)), Color.Black);
        }

        private void ChangeColor()
        {
            for(int i = 0; i < menuColor.Length; i++)
            {
                if (i == selected) menuColor[i] = Color.White;
                else menuColor[i] = Color.Black;
            }
        }
        void ResetGame()
        {
            shotRotation.Clear();
            shots.Clear();
            rocks.Clear();
            lives = defaultLives;
            score = defaultScore;
            pilPos.X = (int)defaultShipPos.X;
            pilPos.Y = (int)defaultShipPos.Y;
            pilRotation = 0;

        }
        void ChangeState(int state)
        {
            gameState = (GameState)state;
            selected = 0;   
        }
        void SortHighscores(List<int> highscoresLocal)
        {

        }
    }
}
