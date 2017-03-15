using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using MonoGame.Extended;
using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;

namespace Game2Test
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouseState, oldMouseState;
        KeyboardState keyState, oldState;

        //lists
        List<Sprite> rocks = new List<Sprite>();
        List<Texture2D> asteroidTextures = new List<Texture2D>();
        List<Texture2D> backgrounds = new List<Texture2D>();
        List<int> highscores = new List<int>();

        Dictionary<string, Turret> turrets0 = new Dictionary<string, Turret>();
        Dictionary<string, Turret> turrets1 = new Dictionary<string, Turret>();
        Dictionary<string, Turret> turrets2 = new Dictionary<string, Turret>();

        Dictionary<string, Texture2D> ship0Dictionary = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> ship1Dictionary = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> ship2Dictionary = new Dictionary<string, Texture2D>();

        List<Ship> ships = new List<Ship>();

        Vector2 defaultShipPos, tempPos, tempPos2, tempPos3, tempPos4, halfScreenPos, halfScreen;
        Rectangle speedbarRectangle, speedbarRectangle2;
        Texture2D shot0Texture, aimTexture, turret0Texture, turret1Texture;

        Shot shot0;

        int sida = 21;
        SpriteFont font;
        int maxShotCount = 2;
        System.Random rnd;
        int score, lives, speed;
        Color[] menuColor = new Color[3];
        int selected = 0;
        int defaultScore = 0;
        int defaultLives = 5;
        int defaultSpeed = 10;
        int menuLength = 3;
        int settingsMenuLength = 2;
        ParticleEngine particleEngine;
        bool moving = false;
        bool drawParticles = false;
        int movingDelayCounter = 0;
        GameState gameState = (GameState)2;
        Ship selectedShip;
        Sprite aimSprite;
        Camera2D camera;
        string xPosString, yPosString;
        Vector2 viewXPos, viewYPos, viewLivesPos, viewScorePos;
        int mapSize = 3; //times 1920x1920
        Vector2 backgroundSize = new Vector2(1920, 1920);
        int rocksPerBackground = 20;
        Color clearColor;
        const float speedBoostConst = 1.5f;
        int asteroidTextureAmount = 8;
        Matrix viewMatrix;

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

            //graphics.PreferredBackBufferWidth = 1366;  // window width 801
            //graphics.PreferredBackBufferHeight = 768;   // window height 701
            //graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = 801;  // window width 
            graphics.PreferredBackBufferHeight = 701;   // window height 

            graphics.ApplyChanges();

            halfScreen = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            defaultShipPos = new Vector2(0, 0);

            turret0Texture = Content.Load<Texture2D>("turret0");
            turret1Texture = Content.Load<Texture2D>("turret1");

            shot0Texture = Content.Load<Texture2D>("shot0");
            shot0 = new Shot(shot0Texture, 60, "default", 15);
            var shot0Dictionary = new Dictionary<string, Shot>();
            shot0Dictionary.Add(shot0.name, shot0);

            turrets0.Add("primary0", new Turret(turret1Texture, new Vector2(-7, -10), new Vector2(-7, -10), 0, shot0Dictionary));
            turrets0.Add("primary1", new Turret(turret1Texture, new Vector2(-7, 10), new Vector2(-7, 10), 0, shot0Dictionary));

            turrets1.Add("primary0", new Turret(turret1Texture, new Vector2(-10, -10), new Vector2(-10, -10), 0, shot0Dictionary));
            turrets1.Add("primary1", new Turret(turret1Texture, new Vector2(-10, 10), new Vector2(-10, 10), 0, shot0Dictionary));

            turrets2.Add("primary0", new Turret(turret0Texture, new Vector2(5, 0), new Vector2(5, 0), 0, shot0Dictionary));

            //ship0
            ship0Dictionary.Add("default", Content.Load<Texture2D>("ship0Texture0"));
            ship0Dictionary.Add("left", Content.Load<Texture2D>("ship0Texture1"));
            ship0Dictionary.Add("right", Content.Load<Texture2D>("ship0Texture2"));

            ships.Add(new Ship(ship0Dictionary, defaultShipPos, turrets0));
            //ship0 end

            //ship1
            ship1Dictionary.Add("default", Content.Load<Texture2D>("ship1Texture0"));
            ship1Dictionary.Add("left", Content.Load<Texture2D>("ship1Texture1"));
            ship1Dictionary.Add("right", Content.Load<Texture2D>("ship1Texture2"));

            ships.Add(new Ship(ship1Dictionary, defaultShipPos, turrets1));
            //ship1 end

            //ship2
            ship2Dictionary.Add("default", Content.Load<Texture2D>("ship2Texture0"));
            ship2Dictionary.Add("left", Content.Load<Texture2D>("ship2Texture1"));
            ship2Dictionary.Add("right", Content.Load<Texture2D>("ship2Texture2"));

            ships.Add(new Ship(ship2Dictionary, defaultShipPos, turrets2));
            //ship2 end

            selectedShip = ships[0];

            aimTexture = Content.Load<Texture2D>("aimWhite");
            aimSprite = new Sprite(aimTexture, new Vector2(halfScreen.X, halfScreen.Y), new Rectangle(0, 0, aimTexture.Width, aimTexture.Height));
            font = Content.Load<SpriteFont>("font");
            rnd = new System.Random();
            speedbarRectangle = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301, 20);
            speedbarRectangle2 = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301, 20);
            speed = defaultSpeed;
            halfScreenPos = new Vector2(halfScreen.X, halfScreen.Y);

            viewScorePos = new Vector2(10, 10);
            viewLivesPos = new Vector2(10, 40);
            viewXPos = new Vector2(0, 10);
            viewYPos = new Vector2(0, 40);

            clearColor = Color.FromNonPremultiplied(22, 68, 39, 252);
            viewMatrix = camera.GetViewMatrix();

            //rock textures
            for (int i = 1; i < asteroidTextureAmount + 1; i++)
            {
                asteroidTextures.Add(Content.Load<Texture2D>("asteroid" + i));
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
            particleEngine = new ParticleEngine(textures, new Vector2(0, 0));
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
            for(int i = 0; i < rocks.Count; i++)
            {
                if (IsInView(rocks[i]))
                {
                    if (selectedShip.TurretCollision(rocks[i].rectangle))
                    {
                        rocks.RemoveAt(i);
                    }
                }
            }
        }
        void CollisionTest2() //kollar om rocks intersects med ship.rectangle
        {
            for (int i = 0; i < rocks.Count; i++)
            {
                if (IsInView(rocks[i]))
                {
                    if (rocks[i].rectangle.Intersects(selectedShip.rectangle))
                    {
                        lives--;
                        rocks.RemoveAt(i);
                        if (lives <= 0)
                        {
                            ChangeState(1);
                            highscores.Add(score);// TODO: add fixerino LEjalkafsdbd
                        }
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
                tempPos = selectedShip.position;
                tempPos.X += (float)(System.Math.Cos(selectedShip.rotation)) * (speed * speedBoostConst);
                tempPos.Y += (float)(System.Math.Sin(selectedShip.rotation)) * (speed * speedBoostConst);
                selectedShip.SetPos(tempPos);
                moving = true;
            }
            else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                tempPos = selectedShip.position;
                tempPos.X += (float)(System.Math.Cos(selectedShip.rotation)) * speed;
                tempPos.Y += (float)(System.Math.Sin(selectedShip.rotation)) * speed;
                selectedShip.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                tempPos = selectedShip.position;
                tempPos.X -= (float)(System.Math.Cos(selectedShip.rotation)) * (speed / 5);
                tempPos.Y -= (float)(System.Math.Sin(selectedShip.rotation)) * (speed / 5);
                selectedShip.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                selectedShip.rotation -= 0.05f;
                if(selectedShip.textureDictionary.ContainsKey("left"))
                {
                    selectedShip.textureIndexCounter = "left";
                }
                selectedShip.Update();

            }
            else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                selectedShip.rotation += 0.05f;
                if (selectedShip.textureDictionary.ContainsKey("right"))
                {
                    selectedShip.textureIndexCounter = "right";
                }
                selectedShip.Update();
            }

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R))
            {
                selectedShip.SetPos(defaultShipPos);
            }
            if (keyState.IsKeyDown(Keys.G) && !oldState.IsKeyDown(Keys.G))
            {
                NextShip();
            }

            tempPos4 = camera.Position;
            tempPos4 = selectedShip.position - halfScreen;
            camera.Position = tempPos4;

            aimSprite.SetPos(camera.ScreenToWorld(mouseState.Position.ToVector2()));

            foreach (var t in selectedShip.turrets)
            {
                t.Value.rotation = RotationToMouse(t.Value.position);
            }
            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed)
            {
                selectedShip.FirePrimary();
            }
            foreach (var t in rocks)
            {
                if (Vector2.Distance(t.position, selectedShip.position) < 1000)
                {
                    tempPos3 = t.position;
                    if (tempPos3.X < selectedShip.position.X) tempPos3.X += 3;
                    else if (tempPos3.X > selectedShip.position.X) tempPos3.X -= 3;

                    if (tempPos3.Y < selectedShip.position.Y) tempPos3.Y += 3;
                    else if (tempPos3.Y > selectedShip.position.Y) tempPos3.Y -= 3;
                    t.SetPos(tempPos3);
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

            //particleEngine.EmitterLocation = camera.WorldToScreen(selectedShip.position);

            if (selectedShip.rotation > (float)Math.PI * 2f || selectedShip.rotation < (float)-Math.PI * 2f) selectedShip.rotation -= (float)Math.PI * 2f;
            selectedShip.UpdateTurrets();

            particleEngine.EmitterLocation = selectedShip.GetBackOfShip();
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
            if (selected < 0) selected = menuLength - 1;
            if (selected > menuLength - 1) selected = 0;
            if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter))
            {
                switch (selected)
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
            if (selected < 0) selected = settingsMenuLength - 1;
            if (selected > settingsMenuLength - 1) selected = 0;

            if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter)) if (selected == 0) ChangeState(2);

            if (keyState.IsKeyDown(Keys.Left) && selected == 1 && speed <= 101 && speed > 1) speed--;
            if (keyState.IsKeyDown(Keys.Right) && selected == 1 && speed < 101 && speed >= 1) speed++;

            speedbarRectangle.Width = speed;

            oldState = Keyboard.GetState();
        }
        void DrawEndScreen()
        {
            spriteBatch.DrawString(font, "Final Score: " + score.ToString(), new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2) - (font.MeasureString("Final Score: " + score.ToString()) / 2), Color.Black);
            spriteBatch.DrawString(font, "Press R to reset.", new Vector2(10, 10), Color.Black);
        }
        void DrawGame()
        {
            spriteBatch.End();

            //backgrounds

            DrawBackground();

            //backgrounds end

            if (drawParticles) particleEngine.Draw(spriteBatch, camera.GetViewMatrix());

            BeginSpriteBatchCamera(spriteBatch);

            //ship.rotationRender = (float)(Math.Round(ship.rotation / (Math.PI / 4)) * (Math.PI / 4));
            //spriteBatch.Draw(ship.texture, new Vector2(ship.rectangle.X, ship.rectangle.Y), rotation: ship.rotation, origin: ship.Origin);

            selectedShip.Draw(spriteBatch);
            selectedShip.DrawTurrets(spriteBatch);

            foreach (var t in selectedShip.turrets)
            {
                t.Value.Draw(spriteBatch);
            }
            foreach (var t in rocks)
            {
                if (IsInView(t)) t.Draw(spriteBatch);
            }

            spriteBatch.DrawString(font, "Score: " + score.ToString(), camera.ScreenToWorld(viewScorePos), Color.White);
            spriteBatch.DrawString(font, "Lives: " + lives, camera.ScreenToWorld(viewLivesPos), Color.White);

            xPosString = "Xpos: " + selectedShip.position.X.ToString("F0");
            yPosString = "Ypos: " + selectedShip.position.Y.ToString("F0");
            viewXPos.X = (halfScreen.X * 2) - font.MeasureString(xPosString).X;
            viewYPos.X = (halfScreen.X * 2) - font.MeasureString(yPosString).X;
            spriteBatch.DrawString(font, xPosString, camera.ScreenToWorld(viewXPos), Color.White);
            spriteBatch.DrawString(font, yPosString, camera.ScreenToWorld(viewYPos), Color.White); 

            aimSprite.Draw(spriteBatch);

        }
        void DrawMenu()
        {
            spriteBatch.DrawString(font, "Begin", new Vector2(graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 5) * 2) - (font.MeasureString("Begin") / 2), menuColor[0]);
            spriteBatch.DrawString(font, "Settings", new Vector2(graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 5) * 3) - (font.MeasureString("Settings") / 2), menuColor[1]);
            spriteBatch.DrawString(font, "Quit", new Vector2(graphics.PreferredBackBufferWidth / 2, (graphics.PreferredBackBufferHeight / 5) * 4) - (font.MeasureString("Quit") / 2), menuColor[2]);
        }
        void DrawSettings()
        {
            spriteBatch.DrawString(font, "Back", new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.MeasureString("Back").X / 2), (graphics.PreferredBackBufferHeight / 2) - font.MeasureString("Speed").Y), menuColor[0]);
            spriteBatch.DrawString(font, "Speed", new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.MeasureString("Speed").X / 2), (graphics.PreferredBackBufferHeight / 2)), menuColor[1]);
            spriteBatch.Draw(selectedShip.texture, speedbarRectangle2, Color.White);
            spriteBatch.Draw(asteroidTextures[4], speedbarRectangle, Color.White);
            spriteBatch.DrawString(font, speed.ToString(), new Vector2((graphics.PreferredBackBufferWidth / 2) - (font.MeasureString(speed.ToString()).X / 2), (graphics.PreferredBackBufferHeight / 2) + 20 + (font.MeasureString("Speed").Y)), Color.Black);
        }
        private void ChangeColor()
        {
            for (int i = 0; i < menuColor.Length; i++)
            {
                if (i == selected) menuColor[i] = Color.White;
                else menuColor[i] = Color.Black;
            }
        }
        void ResetGame()
        {
            selectedShip = ships[0];
            lives = defaultLives;
            score = defaultScore;
            selectedShip.position = defaultShipPos;
            selectedShip.rotation = 0;
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
            viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);
        }
        /// <summary>
        /// Generate all the astroids
        /// </summary>
        void GenerateRocks()
        {
            for (int repeatIndex2 = -mapSize; repeatIndex2 <= mapSize; repeatIndex2++)
            {
                for (int repeatIndex = -mapSize; repeatIndex <= mapSize; repeatIndex++)
                {
                    for (int i = 0; i < rocksPerBackground; i++)
                    {
                        if (repeatIndex2 == 0 || repeatIndex2 == -1 || repeatIndex == 0 || repeatIndex == -1) break;
                        Vector2 position = new Vector2((repeatIndex * backgroundSize.X) + rnd.Next(0, (int)backgroundSize.X + 1), (repeatIndex2 * backgroundSize.Y) + rnd.Next(0, (int)backgroundSize.Y + 1));
                        rocks.Add(new Sprite(asteroidTextures[rnd.Next(asteroidTextures.Count)], position));
                    }
                }
            }
        }
        //void GenerateShotBoost()
        //{
        //    for (int repeatIndex2 = -mapSize; repeatIndex2 <= mapSize; repeatIndex2++)
        //    {
        //        for (int repeatIndex = -mapSize; repeatIndex <= mapSize; repeatIndex++)
        //        {
        //            for (int i = 0; i < rocksPerBackground; i++)
        //            {
        //                if (repeatIndex2 == 0 || repeatIndex2 == -1 || repeatIndex == 0 || repeatIndex == -1) break;
        //                Vector2 position = new Vector2((repeatIndex * backgroundSize.X) + rnd.Next(0, (int)backgroundSize.X + 1), (repeatIndex2 * backgroundSize.Y) + rnd.Next(0, (int)backgroundSize.Y + 1));
        //                rocks.Add(new Sprite(asteroidTextures[rnd.Next(asteroidTextures.Count)], position));
        //            }
        //        }
        //    }
        //}
        /// <summary>
        /// call when spritebatch has ended
        /// </summary>
        void DrawBackground()
        {
            for (var layerIndex = 0; layerIndex < backgrounds.Count; layerIndex++)
            {
                var parallaxFactor = Vector2.One * (0.25f * layerIndex);
                viewMatrix = camera.GetViewMatrix(parallaxFactor);
                spriteBatch.Begin(transformMatrix: viewMatrix);

                for (var repeatIndex2 = -mapSize * 3; repeatIndex2 <= mapSize * 3; repeatIndex2++)
                {
                    for (var repeatIndex = -mapSize * 3; repeatIndex <= mapSize * 3; repeatIndex++)
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
            if (sprite.position.X - sprite.texture.Width > camera.Position.X + camera.GetBoundingRectangle().Width || sprite.position.X + sprite.texture.Width < camera.Position.X)
                return false;

            // if the object is not within the vertical bounds of the screen
            if (sprite.position.Y - sprite.texture.Height > camera.Position.Y + camera.GetBoundingRectangle().Height || sprite.position.Y + sprite.texture.Height < camera.Position.Y)
                return false;

            // if in view
            return true;
        }
        public float RotationToMouse(Vector2 position)
        {
            return (float)Math.Atan2(aimSprite.position.Y - position.Y, aimSprite.position.X - position.X);
        }
        //public string FindIndex(string queryString, Dictionary<string, Texture2D> list)
        //{
        //    return list.FindIndex(x => x == queryString);
        //}
        public void NextShip()
        {
            if (selectedShip.currentShipIndex == ships.Count-1)
            {
                selectedShip.previousShipIndex = selectedShip.currentShipIndex;
                selectedShip.currentShipIndex = 0;
                ChangeShip();
            }
            else
            {
                selectedShip.previousShipIndex = selectedShip.currentShipIndex;
                selectedShip.currentShipIndex++;
                ChangeShip();
            }
        }
        public void ChangeShip()
        {
            var tempRot = selectedShip.rotation;
            var tempPos = selectedShip.position;
            var tempIndex = selectedShip.currentShipIndex;
            selectedShip = ships[selectedShip.currentShipIndex];
            selectedShip.currentShipIndex = tempIndex;
            selectedShip.position = tempPos;
            selectedShip.rotation = tempRot;
            selectedShip.Update();
        }

        //public int GetIndex(string queryString, OrderedDictionary dictionary)
        //{
        //    int i = 0;

        //    foreach (var key in dictionary.Keys)
        //    {
        //        i++;
        //        if (key.Equals(queryString))
        //            return i;
        //    }
        //    return -1;
        //}
    }
}