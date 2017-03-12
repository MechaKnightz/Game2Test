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
        Texture2D rockTexture, shotTexture, aimTexture;
        KeyboardState keyState, oldState;
        List<float> shotRotation = new List<float>();
        List<Sprite> shots = new List<Sprite>();
        int sida = 21;
        SpriteFont font;
        int maxShotCount = 5;
        List<Sprite> rocks = new List<Sprite>();
        System.Random rnd;
        int score, lives;
        Color[] menuColor = new Color[3];
        int selected = 0;
        int defaultScore = 0;
        int defaultLives = 9999;
        Vector2 defaultShipPos, tempPos, tempPos2, tempPos3, tempPos4;
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
        Vector2 shotOrigin, halfScreenPos;
        int halfScreenX, halfScreenY;
        Ship ship;
        Sprite aimSprite;
        Camera2D camera;
        MouseState mouseState;
        List<int> shotTimer = new List<int>();



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

            IsMouseVisible = false;
            camera = new Camera2D(GraphicsDevice.Viewport);

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

            ship = new Ship(Content.Load<Texture2D>("ship2"), new Vector2(0, 0), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2, shipSize, shipSize));

            rockTexture = Content.Load<Texture2D>("rocks");
            shotTexture = Content.Load<Texture2D>("shot");
            aimTexture = Content.Load<Texture2D>("aim");
            aimSprite = new Sprite(aimTexture, new Vector2(halfScreenX, halfScreenY), new Rectangle(0, 0, aimTexture.Width, aimTexture.Height));
            font = Content.Load<SpriteFont>("font");
            rnd = new System.Random();
            speedbarRectangle = new Rectangle((graphics.PreferredBackBufferWidth/2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301,20);
            speedbarRectangle2 = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301,20);
            ResetGame();
            speed = defaultSpeed;
            defaultShipPos = new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2);
            shotOrigin = new Vector2(sida / 2, sida / 2);
            halfScreenPos = new Vector2(halfScreenX, halfScreenY);

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

            switch (gameState)
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

            switch (gameState)
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
                    if (shots[i].rectangle.Intersects(rocks[j].rectangle))
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
        void CollisionTest2() //kollar om rocks intersects med ship.rectangle
        {
            for(int i = 0; i < rocks.Count; i++)
            {
                if(rocks[i].rectangle.Intersects(ship.rectangle))
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
            mouseState = Mouse.GetState();

            aimSprite.SetPos(mouseState.Position.ToVector2());

            tempPos4 = camera.position;
            tempPos4.X = ship.position.X - halfScreenX;
            tempPos4.Y = ship.position.Y - halfScreenY;
            camera.position = tempPos4;

            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                tempPos = ship.position;
                tempPos.X += (float)(System.Math.Cos(ship.rotation)) * 10;
                tempPos.Y += (float)(System.Math.Sin(ship.rotation)) * 10;
                ship.SetPos(tempPos);
                moving = true;
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                tempPos = ship.position;
                tempPos.X -= (float)(System.Math.Cos(ship.rotation))*2;
                tempPos.Y -= (float)(System.Math.Sin(ship.rotation))*2;
                ship.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A)) ship.rotation -= 0.1f;
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D)) ship.rotation += 0.1f;

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R))
            {
                ship.SetPos(halfScreenPos);
            }


            if (keyState.IsKeyDown(Keys.Space) && !oldState.IsKeyDown(Keys.Space) && shots.Count < maxShotCount)
            {
                shots.Add(new Sprite(shotTexture, new Vector2(ship.position.X,ship.position.Y), new Rectangle(0,0,sida, sida)));
                shotRotation.Add(ship.rotation);
                shotTimer.Add(0);

            }
            for (int i = 0; i < shots.Count; i++)
            {
                tempPos2 = shots[i].position;
                tempPos2.X += (float)(System.Math.Cos(shotRotation[i])) * 10;
                tempPos2.Y += (float)(System.Math.Sin(shotRotation[i])) * 10;
                shots[i].SetPos(tempPos2);
                shotTimer[i]++;
                if (shotTimer[i] >= 180)
                {
                    shots.RemoveAt(i);
                    shotRotation.RemoveAt(i);
                    shotTimer.RemoveAt(i);
                }
            }
            //for (int i = 0; i < shots.Count; i++)
            //{
            //    if ((shots[i].position.X <= 0) || (shots[i].position.X >= graphics.PreferredBackBufferWidth) ||
            //        (shots[i].position.Y <= 0) || (shots[i].position.Y >= graphics.PreferredBackBufferHeight))
            //    {
            //        shots.RemoveAt(i);
            //        shotRotation.RemoveAt(i);
            //    }
            //}

            if (rnd.Next(0, 1001) > 990) //rock genererare
            {
                Rectangle temp = GenerateRock();
                rocks.Add(new Sprite(rockTexture,new Vector2(temp.X, temp.Y), GenerateRock()));
            }
            for (int i = 0; i < rocks.Count; i++) //rock flyttare
            {
                tempPos3 = rocks[i].position;

                if (tempPos3.X < ship.position.X) tempPos3.X += speed;
                else if (tempPos3.X > ship.position.X) tempPos3.X -= speed;

                if (tempPos3.Y < ship.position.Y) tempPos3.Y += speed;
                else if (tempPos3.Y > ship.position.Y) tempPos3.Y -= speed;

                //int skillnadX = ship.rectangle.X - temp.X;
                //int skillnadY = ship.rectangle.Y - temp.Y;
                //temp.X += System.Convert.ToInt16(0.05f * skillnadX);
                //temp.Y += System.Convert.ToInt16(0.05f * skillnadY);

                rocks[i].SetPos(tempPos3);
            }

            if (ship.rotation < 2 * System.Math.PI) ship.rotation -= 2 * (float)System.Math.PI;

            //particleEngine.EmitterLocation = ship.position;
            particleEngine.EmitterLocation = ship.position-camera.position;
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

            BeginSpriteBatchCamera(spriteBatch);

            //ship.rotationRender = (float)(Math.Round(ship.rotation / (Math.PI / 4)) * (Math.PI / 4));
            //spriteBatch.Draw(ship.texture, new Vector2(ship.rectangle.X, ship.rectangle.Y), rotation: ship.rotation, origin: ship.Origin);

            ship.Draw(spriteBatch);

            for (int i = 0; i < shots.Count; i++)
            {
                shots[i].Draw(spriteBatch);
            }
            for (int i = 0; i < rocks.Count; i++)
            {
                rocks[i].Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, "Score: " + score.ToString(), camera.position + new Vector2(10, 10), Color.Black);
            spriteBatch.DrawString(font, "Lives: " + lives, camera.position + new Vector2(10, 40), Color.Black);


            aimSprite.Draw(spriteBatch, camera);
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
            spriteBatch.Draw(ship.texture, speedbarRectangle2, Color.White);
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
            ship.position.X = (int)defaultShipPos.X;
            ship.position.Y = (int)defaultShipPos.Y;
            ship.rotation = 0;

        }
        void ChangeState(int state)
        {
            gameState = (GameState)state;
            selected = 0;
            
        }
        void BeginSpriteBatchCamera(SpriteBatch spriteBatch)
        {
            var viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);
        }
    }
}
