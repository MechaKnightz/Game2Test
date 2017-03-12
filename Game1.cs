using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using MonoGame.Extended;

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
        List<Shot> shots = new List<Shot>();
        int sida = 21;
        SpriteFont font;
        int maxShotCount = 1;
        List<Sprite> rocks = new List<Sprite>();
        System.Random rnd;
        int score, lives;
        Color[] menuColor = new Color[3];
        int selected = 0;
        int defaultScore = 0;
        int defaultLives = 5;
        Vector2 defaultShipPos, tempPos, tempPos2, tempPos3, tempPos4;
        Rectangle speedbarRectangle, speedbarRectangle2;
        int speed;
        int defaultSpeed = 10;
        int menuLength = 3;
        int settingsMenuLength = 2;
        ParticleEngine particleEngine;
        bool moving = false;
        bool drawParticles = false;
        int movingDelayCounter = 0;
        List<int> highscores = new List<int>();
        List<Texture2D> backgrounds = new List<Texture2D>();
        GameState gameState = (GameState)2;
        int shipSize = 41;
        Vector2 shotOrigin, halfScreenPos, halfScreen;
        Ship ship;
        Sprite aimSprite;
        Camera2D camera;
        MouseState mouseState, oldMouseState;
        string xPosString, yPosString;
        Vector2 viewXPos, viewYPos, viewLivesPos, viewScorePos;
        int mapSize = 3; //1920x1920
        Vector2 backgroundSize = new Vector2(1920, 1920);
        int rocksPerBackground = 20;
        Color clearColor;
        const float speedBoostConst = 1.5f;
        List<Texture2D> rockTextures = new List<Texture2D>();
        int asteroidTextureAmount = 8;

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

            IsMouseVisible = false;
            camera = new Camera2D(GraphicsDevice);

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
            
            graphics.PreferredBackBufferWidth = 801;  // window width
            graphics.PreferredBackBufferHeight = 701;   // window height
            graphics.ApplyChanges();

            halfScreen = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            defaultShipPos = new Vector2(0, 0);

            ship = new Ship(Content.Load<Texture2D>("ship2"), defaultShipPos, new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2, shipSize, shipSize));

            shotTexture = Content.Load<Texture2D>("shot");
            aimTexture = Content.Load<Texture2D>("aimWhite");
            rockTexture = Content.Load<Texture2D>("rock");
            aimSprite = new Sprite(aimTexture, new Vector2(halfScreen.X, halfScreen.Y), new Rectangle(0, 0, aimTexture.Width, aimTexture.Height));
            font = Content.Load<SpriteFont>("font");
            rnd = new System.Random();
            speedbarRectangle = new Rectangle((graphics.PreferredBackBufferWidth/2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301,20);
            speedbarRectangle2 = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301,20);
            speed = defaultSpeed;
            shotOrigin = new Vector2(sida / 2, sida / 2);
            halfScreenPos = new Vector2(halfScreen.X, halfScreen.Y);

            viewScorePos = new Vector2(10, 10);
            viewLivesPos = new Vector2(10, 40);
            viewXPos = new Vector2(0, 10);
            viewYPos = new Vector2(0, 40);

            clearColor = Color.FromNonPremultiplied(22, 68, 39, 252);

            //rock textures
            for (int i = 1; i < asteroidTextureAmount+1; i++)
            {
                rockTextures.Add(Content.Load<Texture2D>("asteroid" + i));
            }

            //map generation

            ResetGame();

            //backgrounds larger = front
            for (int i = 1; i < 4; i++)
            {
                backgrounds.Add(Content.Load<Texture2D>("background" + i));
            }

            //particles
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("red"));
            textures.Add(Content.Load<Texture2D>("orange"));
            textures.Add(Content.Load<Texture2D>("yellow"));
            particleEngine = new ParticleEngine(textures, new Vector2(400, 240));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
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
                    GameLogic(gameTime);
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
            GraphicsDevice.Clear(clearColor);

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

            if (moving) movingDelayCounter = 5;
            else movingDelayCounter--;
            if (movingDelayCounter <= 0) drawParticles = false;
            else drawParticles = true;
            moving = false;
            
            oldState = Keyboard.GetState();
            base.Draw(gameTime);
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
        void GameLogic(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            if ((keyState.IsKeyDown(Keys.Up) && (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))) || (keyState.IsKeyDown(Keys.W) && (keyState.IsKeyDown(Keys.LeftShift) || keyState.IsKeyDown(Keys.RightShift))))
            {
                {
                    tempPos = ship.position;
                    tempPos.X += (float)(System.Math.Cos(ship.rotation)) * (speed * speedBoostConst);
                    tempPos.Y += (float)(System.Math.Sin(ship.rotation)) * (speed * speedBoostConst);
                    ship.SetPos(tempPos);
                    moving = true;
                }
            }
            else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                tempPos = ship.position;
                tempPos.X += (float)(System.Math.Cos(ship.rotation)) * speed;
                tempPos.Y += (float)(System.Math.Sin(ship.rotation)) * speed;
                ship.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                tempPos = ship.position;
                tempPos.X -= (float)(System.Math.Cos(ship.rotation))*(speed/5);
                tempPos.Y -= (float)(System.Math.Sin(ship.rotation))*(speed/5);
                ship.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A)) ship.rotation -= 0.05f;
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D)) ship.rotation += 0.05f;

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R))
            {
                ship.SetPos(defaultShipPos);
            }

            tempPos4 = camera.Position;
            tempPos4.X = ship.position.X - halfScreen.X;
            tempPos4.Y = ship.position.Y - halfScreen.Y;
            camera.Position = tempPos4;

            aimSprite.SetPos(mouseState.Position.ToVector2() + camera.Position);

            if (mouseState.LeftButton == ButtonState.Pressed && !(oldMouseState.LeftButton == ButtonState.Pressed) && shots.Count < maxShotCount)
            {
                shots.Add(new Shot(shotTexture, new Vector2(ship.position.X,ship.position.Y), (float)Math.Atan2(aimSprite.position.Y - ship.position.Y, aimSprite.position.X - ship.position.X), 0));

            }
            for (int i = 0; i < shots.Count; i++)
            {
                tempPos2 = shots[i].position;
                tempPos2.X += (float)(System.Math.Cos(shots[i].rotation)) * 15;
                tempPos2.Y += (float)(System.Math.Sin(shots[i].rotation)) * 15;
                shots[i].SetPos(tempPos2);

                shots[i].duration++;
                if(shots[i].duration > 60)
                {
                    shots.RemoveAt(i);
                }
            }
            for (int i = 0; i < rocks.Count; i++)
            {
                if (Vector2.Distance(rocks[i].position, ship.position) < 1000)
                {
                    tempPos3 = rocks[i].position;
                    if (tempPos3.X < ship.position.X) tempPos3.X += 3;
                    else if (tempPos3.X > ship.position.X) tempPos3.X -= 3;

                    if (tempPos3.Y < ship.position.Y) tempPos3.Y += 3;
                    else if (tempPos3.Y > ship.position.Y) tempPos3.Y -= 3;
                    rocks[i].SetPos(tempPos3);
                }
            }
            //for (int i = 0; i < rocks.Count; i++) //rock flyttare
            //{
            //    tempPos3 = rocks[i].position;

            //    if (tempPos3.X < ship.position.X) tempPos3.X += speed;
            //    else if (tempPos3.X > ship.position.X) tempPos3.X -= speed;

            //    if (tempPos3.Y < ship.position.Y) tempPos3.Y += speed;
            //    else if (tempPos3.Y > ship.position.Y) tempPos3.Y -= speed;

            //    //int skillnadX = ship.rectangle.X - temp.X;
            //    //int skillnadY = ship.rectangle.Y - temp.Y;
            //    //temp.X += System.Convert.ToInt16(0.05f * skillnadX);
            //    //temp.Y += System.Convert.ToInt16(0.05f * skillnadY);

            //    rocks[i].SetPos(tempPos3);
            //}

            if (ship.rotation < 2 * System.Math.PI) ship.rotation -= 2 * (float)System.Math.PI;

            //particleEngine.EmitterLocation = ship.position;

            particleEngine.EmitterLocation = camera.WorldToScreen(ship.position);
            particleEngine.Update();

            CollisionTest();
            CollisionTest2();

            oldState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
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

            //backgrounds

            DrawBackground();

            //backgrounds end

            if (drawParticles) particleEngine.Draw(spriteBatch);

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
                if(IsInView(rocks[i]))rocks[i].Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, "Score: " + score.ToString(), camera.ScreenToWorld(viewScorePos), Color.Black);
            spriteBatch.DrawString(font, "Lives: " + lives, camera.ScreenToWorld(viewLivesPos), Color.Black);

            xPosString = "Xpos: " + ship.position.X.ToString("F0");
            yPosString = "Ypos: " + ship.position.Y.ToString("F0");
            viewXPos.X = (halfScreen.X * 2) - font.MeasureString(xPosString).X;
            viewYPos.X = (halfScreen.X * 2) - font.MeasureString(yPosString).X;
            spriteBatch.DrawString(font, "Xpos: " + ship.position.X.ToString("F0"), camera.ScreenToWorld(viewXPos), Color.Black);
            spriteBatch.DrawString(font, "Ypos: " + ship.position.Y.ToString("F0"), camera.ScreenToWorld(viewYPos), Color.Black);

            aimSprite.Draw(spriteBatch);

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
            shots.Clear();
            lives = defaultLives;
            score = defaultScore;
            ship.position = defaultShipPos;
            ship.rotation = 0;
            rocks.Clear();
            GenerateRocks();

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
        void GenerateRocks()
        {
            for (int repeatIndex2 = -mapSize; repeatIndex2 <= mapSize; repeatIndex2++)
            {
                for (int repeatIndex = -mapSize; repeatIndex <= mapSize; repeatIndex++)
                {
                    for (int i = 0; i < rocksPerBackground; i++)
                    {
                        if (repeatIndex2 == 0|| repeatIndex2 == -1 || repeatIndex == 0 || repeatIndex == -1) break;
                        Vector2 position = new Vector2((repeatIndex * backgroundSize.X) + rnd.Next(0, (int)backgroundSize.X + 1), (repeatIndex2 * backgroundSize.Y) + rnd.Next(0, (int)backgroundSize.Y + 1));
                        rocks.Add(new Sprite(rockTextures[rnd.Next(rockTextures.Count)], position));
                    }
                }
            }
        }
        /// <summary>
        /// call when spritebatch has ended
        /// </summary>
        void DrawBackground()
        {
            for (var layerIndex = 0; layerIndex < backgrounds.Count; layerIndex++)
            {
                var parallaxFactor = Vector2.One * (0.25f * layerIndex);
                var viewMatrix = camera.GetViewMatrix(parallaxFactor);
                spriteBatch.Begin(transformMatrix: viewMatrix);

                for (var repeatIndex2 = -mapSize*3; repeatIndex2 <= mapSize*3; repeatIndex2++)
                {
                    for (var repeatIndex = -mapSize*3; repeatIndex <= mapSize*3; repeatIndex++)
                    {
                        var texture = backgrounds[layerIndex];
                        var position = new Vector2(repeatIndex * texture.Width, repeatIndex2 * texture.Height);
                        spriteBatch.Draw(texture, position, Color.White);
                    }
                }
                spriteBatch.End();
            }
        }
        /// <summary>
        /// is the parameterrectangle in the screen, if so return true, otherwise false
        /// </summary>
        /// <param name="sprite"> the sprite you want to check</param>
        /// <returns></returns>
        public bool IsInView(Sprite sprite)
        {
            // if the object is not within the horizontal bounds of the screen

            if (sprite.position.X - sprite.texture.Width > camera.Position.X + camera.GetBoundingRectangle().Width || sprite.position.X + sprite.texture.Width  < camera.Position.X)
                return false;

            // if the object is not within the vertical bounds of the screen
            if (sprite.position.Y - sprite.texture.Height > camera.Position.Y + camera.GetBoundingRectangle().Height || sprite.position.Y + sprite.texture.Height < camera.Position.Y)
                return false;

            // if in view
            return true;
        }
    }
}