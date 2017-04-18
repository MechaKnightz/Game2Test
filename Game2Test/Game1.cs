using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using MonoGame.Extended;
using GeonBit.UI;
using GeonBit.UI.Entities;
using System.IO;
using System.Text;
using Game2Test.Input;
using Game2Test.Sprites;
using Game2Test.Sprites.Entities;
using Game2Test.Sprites.Entities.Turrets;
using Game2Test.Sprites.Helpers;
using static ClassLibary.Angle;

namespace Game2Test
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouseState, oldMouseState;
        KeyboardState keyState, oldState;

        //lists
        List<Texture2D> asteroidTextures = new List<Texture2D>();
        List<Texture2D> crystalTextures = new List<Texture2D>();
        
        List<int> highscores = new List<int>();
        private List<Name> names = new List<Name>();

        private List<string> nameArray;

        List<ITurret> turrets0 = new List<ITurret>();
        List<ITurret> turrets1 = new List<ITurret>();
        List<ITurret> turrets2 = new List<ITurret>();
        List<ITurret> turrets3 = new List<ITurret>();
        List<ITurret> turrets3Secondary = new List<ITurret>();
        List<ITurret> turretsStation = new List<ITurret>();
        private TractorBeam testTractorBeam;

        public List<Sector> sectors = new List<Sector>();

        Dictionary<string, Texture2D> stationDictionary = new Dictionary<string, Texture2D>();

        Dictionary<string, Shot> shot0List = new Dictionary<string, Shot>();
        Dictionary<string, Shot> shot1List = new Dictionary<string, Shot>();

        public List<Ship> ships = new List<Ship>();
        public Ship  testShip;
        public Station currentStation;
        public List<Ship> availableShips = new List<Ship>();
        public List<Upgrade> availableUpgrades = new List<Upgrade>();
        public List<Ship> ownedShips = new List<Ship>();

        Vector2 defaultShipPos, tempPos, halfScreenPos;
        public Vector2 halfScreen;

        Texture2D shot0Texture, shot1Texture, aimTexture, turret0Texture, turret1Texture, energyIconTexture,
            redPixel, greenPixel, turretStationTexture, whitePixel;

        public Texture2D transparent;

        Shot shot0, shot1;
        Sprite greenHealth;
        Sprite redHealth;

        SpriteFont font;
        private static Random _rnd;
        int speed;
        Color[] menuColor = new Color[3];
        int defaultScore = 0;
        int defaultSpeed = 10;
        ParticleEngine particleEngine;
        bool drawParticles;
        int movingDelayCounter;
        public GameState gameState = (GameState)2;
        public GameState oldGameState;
        Sprite aimSprite, energyBarSprite;
        Camera2D camera;
        string xPosString, yPosString;
        Vector2 viewXPos, viewYPos, viewLivesPos, viewScorePos;
        int mapSize = 3; //times 1920x1920
        Vector2 backgroundSize = new Vector2(1920, 1920);
        int rocksPerBackground = 20;
        Color clearColor;
        private int asteroidTextureAmount = 8;
        private int crystalTextureAmount = 3;
        Matrix viewMatrix;
        public Panel mainMenuPanel;
        public Panel shopPanel;
        public Panel pauseMenuPanel;
        public Panel settingsMenuPanel;
        public Panel controlsMenuPanel;
        public Button shopHUDButton;
        public List<Button> shopButtons = new List<Button>();
        public List<Paragraph> shopDescriptions = new List<Paragraph>();
        public List<Paragraph> shopDescriptions2 = new List<Paragraph>();
        private float distanceToStation;
        private float shopRadius = 200;
        public Sector currentSector;
        private List<Texture2D> layer2 = new List<Texture2D>();
        public const float DoublePI = (float)Math.PI * 2;
        public Keys forwardKey;
        public Keys forwardKey2;


        private readonly GameUserInterface _gameUserInterface;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "..\\Content";
            _gameUserInterface = new GameUserInterface(this);
        }

        protected override void Initialize()
        {

            UserInterface.Initialize(Content, "custom");
            UserInterface.UseRenderTarget = true;
            Paragraph.BaseSize = 1.0f;
            UserInterface.GlobalScale = 1.0f;
            UserInterface.CursorScale = 0.7f;
            var form = (System.Windows.Forms.Form)System.Windows.Forms.Control.FromHandle(this.Window.Handle);
            form.Location = new System.Drawing.Point(0, 0);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            _rnd = new Random();
            camera = new Camera2D(GraphicsDevice);

            graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;  // window width 801
            graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;   // window height 701
            graphics.IsFullScreen = false;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

            //graphics.PreferredBackBufferWidth = 801;  // window width 801
            //graphics.PreferredBackBufferHeight = 701;   // window height 701
            //graphics.IsFullScreen = false;

            graphics.ApplyChanges();

            halfScreen = new Vector2(graphics.PreferredBackBufferWidth / 2f, graphics.PreferredBackBufferHeight / 2f);

            defaultShipPos = Vector2.Zero;

            var tractorBeamTexture = Content.Load<Texture2D>("tractorBeam");

            turret0Texture = Content.Load<Texture2D>("turret0");
            turret1Texture = Content.Load<Texture2D>("turret1");
            turretStationTexture = Content.Load<Texture2D>("turretStationTexture");

            shot0Texture = Content.Load<Texture2D>("shot0");
            shot0 = new Shot(shot0Texture, 60, "default", 15, 2);

            shot1Texture = Content.Load<Texture2D>("shot1");
            shot1 = new Shot(shot1Texture, 60, "default", 15, 1);

            var laserStart = new Sprite(Content.Load<Texture2D>("laserStart"));
            var laserMiddle = new Sprite(Content.Load<Texture2D>("laserMiddle"));
            var laserEnd = new Sprite(Content.Load<Texture2D>("laserEnd"));

            turrets0.Add(new BasicTurret(turret1Texture, new Vector2(-7, -10), new Vector2(-7, -10), 0, shot1, 100, 0.05f, TurretType.Rotating, 30f));
            turrets0.Add(new BasicTurret(turret1Texture, new Vector2(-7, 10), new Vector2(-7, 10), 0, shot1, 100, 0.05f, TurretType.Rotating, 30f));

            turrets1.Add(new BasicTurret(turret1Texture, new Vector2(-10, -10), new Vector2(-10, -10), 0, shot0, 150, 0.05f, TurretType.Static, 30f));
            turrets1.Add(new BasicTurret(turret1Texture, new Vector2(-10, 10), new Vector2(-10, 10), 0, shot0, 150, 0.05f, TurretType.Static, 30f));

            turrets2.Add(new BasicTurret(turret0Texture, new Vector2(5, 0), new Vector2(5, 0), 0, shot0, 150, 0.05f, TurretType.Rotating, 30f));

            turrets3.Add(new BasicTurret(turret1Texture, new Vector2(-5.5f, -28.5f), new Vector2(-5.5f, -28.5f), 0, shot0, 150, 0.05f, TurretType.Rotating, 30f));
            turrets3.Add(new BasicTurret(turret1Texture, new Vector2(-5.5f, 27.5f), new Vector2(-5.5f, 27.5f), 0, shot0, 150, 0.05f, TurretType.Rotating, 30f));

            turrets3Secondary.Add(new LaserTurret(turret1Texture, laserStart, laserMiddle, laserEnd, new Vector2(-4.5f, 0f), new Vector2(-4.5f, 0), 0, 20, 0.05f, TurretType.Rotating, 180f, 800f, 120f, 1f));

            turretsStation.Add(new BasicTurret(turretStationTexture, new Vector2(5, 27), new Vector2(0, 27), 0, shot0, 150, 0.05f, TurretType.Rotating, 30f));
            turretsStation.Add(new BasicTurret(turretStationTexture, new Vector2(5, -27), new Vector2(5, -27), 0, shot0, 150, 0.05f, TurretType.Rotating, 30f));


            testTractorBeam = new TractorBeam(tractorBeamTexture, 300, 15, 20); // speed is speed/asteroid size
            LoadShips(); //LOADS SHIPS

            availableUpgrades.Add(new Upgrade(0, 0, 15, 10));
            availableUpgrades.Add(new Upgrade(0, 400, 0, 15));
            availableUpgrades.Add(new Upgrade(10, 0, 10, 25));

            var turretStationCollection = new Dictionary<string, List<ITurret>>();
            turretStationCollection.Add("primary", turretsStation);

            //stationShip

            stationDictionary.Add("Default", Content.Load<Texture2D>("stationTexture"));

            currentStation = new Station(stationDictionary, defaultShipPos, turretStationCollection, 100, 500, 15, 0.05f, 0, testTractorBeam, 5);

            //stationShip end

            //asteroid textures
            for (int i = 1; i < asteroidTextureAmount + 1; i++)
            {
                asteroidTextures.Add(Content.Load<Texture2D>("asteroid" + i));
            }
            for (int i = 1; i < crystalTextureAmount + 1; i++)
            {
                crystalTextures.Add(Content.Load<Texture2D>("crystal" + i));
            }

            redPixel = Content.Load<Texture2D>("redPixel");
            whitePixel = Content.Load<Texture2D>("whitePixel");
            greenPixel = Content.Load<Texture2D>("greenPixel");
            redHealth = new Sprite(redPixel);
            greenHealth = new Sprite(greenPixel);
            //first sector

            currentSector = GenerateSector();
            currentSector.CurrentShip = new Ship(ships[0]);
            currentSector.CurrentStation = currentStation;

            //first sector end

            energyIconTexture = Content.Load<Texture2D>("battery2");
            aimTexture = Content.Load<Texture2D>("aimWhite");
            aimSprite = new Sprite(aimTexture, new Vector2(halfScreen.X, halfScreen.Y), new Rectangle(0, 0, aimTexture.Width, aimTexture.Height));
            energyBarSprite = new Sprite(whitePixel, new Vector2(0,0), new Rectangle(0, 0, 18, 292));
            font = Content.Load<SpriteFont>("font");
            speed = defaultSpeed;
            halfScreenPos = new Vector2(halfScreen.X, halfScreen.Y);
            transparent = Content.Load<Texture2D>("transparent");

            viewScorePos = new Vector2(10, 10);
            viewLivesPos = new Vector2(10, 40);
            viewXPos = new Vector2(0, 10);
            viewYPos = new Vector2(0, 40);

            clearColor = Color.FromNonPremultiplied(37, 40, 41, 255);
            viewMatrix = camera.GetViewMatrix();

            //map generation

            ResetGame();

            //backgrounds larger = front


            // TODO: https://msdn.microsoft.com/en-us/library/bb531208.aspx

            //particles
            var textures = new List<Texture2D>
            {
                Content.Load<Texture2D>("red"),
                Content.Load<Texture2D>("orange"),
                Content.Load<Texture2D>("yellow")
            };
            particleEngine = new ParticleEngine(textures, new Vector2(0, 0));


            //main menu

            _gameUserInterface.GenerateUserInterface(GameState.MainMenu);

            //test ships

            //testShip = new Ship(ships[2]);
            //testShip.SetPosition(new Vector2(-500, -500));

            //bind default keys

            forwardKey = Keys.W;
            forwardKey2 = Keys.Up;

        }

        private void LoadShips()
        {
            var turret0Collection = InitialiseTurretCollection(turrets0);
            var turret1Collection = InitialiseTurretCollection(turrets1);
            var turret2Collection = InitialiseTurretCollection(turrets2);
            var turret3Collection = InitialiseTurretCollection(turrets3);
            turret3Collection.Add("secondary", turrets3Secondary);

            var ship0 = InitialiseShip("ship0", "Human ship 1 description", 0f, 10f, 1.5f, turret0Collection, 0.05f, 1000, 5, 5);
            var ship1 = InitialiseShip("ship1", "Human ship 2 description", 0f, 10f, 1.5f, turret1Collection, 0.05f, 1000, 5, 5);
            var ship2 = InitialiseShip("ship2", "Alien ship 1 description", 0f, 10f, 1.5f, turret2Collection, 0.05f, 1000, 5, 5);
            var ship3 = InitialiseShip("ship3", "Himan ship 3 description", 0f, 5f, 1.5f, turret3Collection, 0.02f, 2000, 10, 10);

            var initiatedShips = new List<Ship> { ship0, ship1, ship2, ship3 };

            ships.AddRange(initiatedShips);
            ownedShips.Add(ship0);
            availableShips.AddRange(initiatedShips);
        }

        private Dictionary<string, List<ITurret>> InitialiseTurretCollection(List<ITurret> turretList)
        {
            return new Dictionary<string, List<ITurret>>
            {
                {"primary", turretList }
            };
        }

        private Ship InitialiseShip(
            string shipName,
            string shipDescription,
            float shipCost,
            float shipSpeed,
            float shipBoost,
            Dictionary<string, List<ITurret>> turretCollection,
            float turnRate,
            float energyMax,
            float energyRegen,
            int maxUpgrades)
        {
            var shipDictionary = new Dictionary<string, Texture2D>
            {
                { "Default", Content.Load<Texture2D>($"{shipName}Texture0")},
                { "Left", Content.Load<Texture2D>($"{shipName}Texture1")},
                { "Right", Content.Load<Texture2D>($"{shipName}Texture2")}
            };

            var ship = new Ship(shipDictionary, defaultShipPos, turretCollection, 10, energyMax, energyRegen, turnRate, shipSpeed, testTractorBeam, maxUpgrades)
            {
                Cost = shipCost,
                Description = shipDescription,
                Name = shipName,
                Boost = shipBoost
            };

            return ship;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

            KeyInput.Update(gameTime);
            UserInterface.Update(gameTime);

            switch (gameState)
            {
                case GameState.MainGame: //main game
                    GameLogic(gameTime);
                    break;
                case GameState.EndScreen: //gameover
                    EndScreenLogic();
                    break;
                case GameState.MainMenu: //main menu
                    MenuLogic(gameTime);
                    break;
                case GameState.SettingsMenu: //settings
                    SettingsLogic();
                    break;
                case GameState.ShopMenu:
                    GameLogic(gameTime);
                    break;
                case GameState.PauseMenu:
                    PauseMenuLogic(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            UserInterface.Draw(spriteBatch);

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
                    break;
                case GameState.ShopMenu:
                    DrawGame();
                    break;
                case GameState.PauseMenu:
                    DrawGame();
                    break;
            }

            spriteBatch.End();

            UserInterface.DrawMainRenderTarget(spriteBatch);

            oldState = Keyboard.GetState();
            base.Draw(gameTime);
        }

        void CollisionTest() //kollar om shots intersects med asteroids
        {
            for (int i = 0; i < currentSector.Asteroids.Count; i++)
            {
                if (IsInView(currentSector.Asteroids[i]) && !currentSector.Asteroids[i].Destroyed)
                {
                    float damage;
                    if (currentSector.CurrentShip.TurretCollision(currentSector.Asteroids[i].Rectangle, out damage))
                    {
                        currentSector.Asteroids[i].Health -= damage;
                        if (currentSector.Asteroids[i].Health <= 0)
                        {
                            currentSector.Asteroids[i].Destroy();
                            i--;
                        }
                    }
                    float damage2;
                    if (currentSector.CurrentStation.TurretCollision(currentSector.Asteroids[i].Rectangle, out damage2))
                    {
                        currentSector.Asteroids[i].Health -= damage2;
                        if (currentSector.Asteroids[i].Health <= 0)
                        {
                            currentSector.Asteroids[i].Destroy();
                            i--;
                        }
                    }
                }
            }
        }
        void CollisionTest2() //kollar om asteroids intersects med ship.rectangle
        {
            for (int i = 0; i < currentSector.Asteroids.Count; i++)
            {
                if (IsInView(currentSector.Asteroids[i]))
                {
                    if (currentSector.Asteroids[i].RotatedRectangle.Intersects(currentSector.CurrentShip.RotatedRectangle) && !currentSector.Asteroids[i].Destroyed)
                    {
                        currentSector.CurrentShip.Health--;
                        currentSector.Asteroids.RemoveAt(i);
                        if (currentSector.CurrentShip.Health <= 0)
                        {
                            ChangeState(GameState.EndScreen);
                            highscores.Add((int)currentSector.CurrentShip.Money);// TODO: add fixerino LEjalkafsdbd
                        }
                    }
                }
            }
        }
        void GameLogic(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            distanceToStation = Vector2.Distance(currentSector.CurrentShip.Position, currentSector.CurrentStation.Position);


            if (KeyInput.IsKeyClicked(Keys.Escape) && gameState == GameState.MainGame)
            {
                ChangeState(GameState.PauseMenu);
            }
            //moving forward
            if (keyState.IsKeyDown(Keys.LeftShift))
            {
                currentSector.CurrentShip.BoostBool = true;
            }
            if (KeyInput.EitherKeyDown(forwardKey, forwardKey2))
            {
                currentSector.CurrentShip.Move(MoveDirection.Forward);
            }
            //moving 
            if (KeyInput.EitherKeyDown(Keys.Down, Keys.S))
            {
                currentSector.CurrentShip.Move(MoveDirection.Backward);
            }
            if (KeyInput.EitherKeyDown(Keys.Left, Keys.A))
            {
                currentSector.CurrentShip.Turn(Direction.Left);
            }
            else if (KeyInput.EitherKeyDown(Keys.Right, Keys.D))
            {
                currentSector.CurrentShip.Turn(Direction.Right);
            }

            if (keyState.IsKeyDown(Keys.E))
            {
                currentSector.CurrentShip.Move(MoveDirection.Right);
            }
            else if (keyState.IsKeyDown(Keys.Q))
            {
                currentSector.CurrentShip.Move(MoveDirection.Left);
            }

            if (gameState == GameState.ShopMenu && keyState.IsKeyDown(Keys.Escape)) ChangeState(GameState.MainGame);
            if (KeyInput.IsKeyClicked(Keys.G) && distanceToStation < shopRadius)
            {
                switch (gameState)
                {
                    case GameState.MainGame:
                        ChangeState(GameState.ShopMenu);
                        break;
                    case GameState.ShopMenu:
                        ChangeState(GameState.MainGame);
                        break;
                }
            }

            tempPos = camera.Position;
            tempPos = currentSector.CurrentShip.Position - halfScreen;
            camera.Position = tempPos;

            aimSprite.Position = camera.ScreenToWorld(mouseState.Position.ToVector2());

            foreach (var t in currentSector.CurrentStation.Turrets)
            {
                foreach (var tur in t.Value)
                {
                    var asteroid = new Asteroid();
                    float tempDistance = 99999999;
                    for (int i = 0; i < currentSector.Asteroids.Count; i++) //TODO fix targetting
                    {
                        if (Vector2.Distance(currentSector.Asteroids[i].Position, tur.Position) < tempDistance && !currentSector.Asteroids[i].Destroyed)
                        {
                            tempDistance = Vector2.Distance(currentSector.Asteroids[i].Position, tur.Position);
                            asteroid.Position = currentSector.Asteroids[i].Position;
                            asteroid.Rotation = currentSector.Asteroids[i].Rotation;
                            asteroid.Speed = currentSector.Asteroids[i].Speed;
                            asteroid.Acceleration = currentSector.Asteroids[i].Acceleration;
                            var tempTime = tempDistance / tur.Speed;
                            for (var j = 0; j < tempTime; j++)
                            {
                                asteroid.MoveTowardsPosition(currentSector.CurrentShip.Position);
                            }
                        }
                    }
                    if (tempDistance < 800)
                    {
                        var rot = AngleToOther(tur.Position, asteroid.Position);
                        const float rotConst = 0.03f; //TODO fix the if statements
                        if (tur.Rotation > rot) tur.Rotation -= rotConst;
                        if (tur.Rotation < rot) tur.Rotation += rotConst;
                        //t.Value.Rotation = AngleToOther(t.Value.position, asteroid.position));
                        float diff = Math.Abs(MathHelper.WrapAngle(rot - tur.Rotation));
                        if (diff < 0.05) currentSector.CurrentStation.Fire("primary");
                    }
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed && gameState == GameState.MainGame)
            {
                currentSector.CurrentShip.Fire("primary");
            }
            if (mouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton != ButtonState.Pressed && gameState == GameState.MainGame)
            {
                currentSector.CurrentShip.Fire("secondary");
            }

            if (distanceToStation < shopRadius)
            {
                shopHUDButton.Disabled = false;
            }
            else
            {
                shopHUDButton.Disabled = true;
            }
            if (distanceToStation > shopRadius && gameState == GameState.ShopMenu)
            {
                ChangeState(GameState.MainGame);
            }

            //VVV - UPDATE BELOW -  VVV

            currentSector.CurrentShip.AimTurrets(aimSprite.Position);

            //testing

            //AI.MoveTowardsGoal(testShip, currentSector.CurrentShip);
            //testShip.Update();

            //determines if should draw particles, must be after move/turn ship but before update
            if (currentSector.CurrentShip.Moving) movingDelayCounter = 5;
            else movingDelayCounter--;
            if (movingDelayCounter <= 0) drawParticles = false;
            else drawParticles = true;

            //end

            currentSector.Update(currentSector);

            if (currentSector.CurrentShip.Rotation > DoublePI) currentSector.CurrentShip.Rotation -= DoublePI;
            else if (currentSector.CurrentShip.Rotation < -DoublePI) currentSector.CurrentShip.Rotation += DoublePI;

            particleEngine.EmitterLocation = currentSector.CurrentShip.GetBackOfShip();
            particleEngine.Update();

            CollisionTest();
            CollisionTest2();

            oldState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
        }
        void MenuLogic(GameTime gameTime)
        {

        }

        public void ChangeState(GameState tempGameState)
        {
            oldGameState = gameState;
            _gameUserInterface.GenerateUserInterface(tempGameState);
            _gameUserInterface.RemoveInterface(gameState, this);
            gameState = tempGameState;
        }

        private void PauseMenuLogic(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (KeyInput.IsKeyClicked(Keys.Escape))
            {
                if (gameState == GameState.PauseMenu) ChangeState(GameState.MainGame);
            }

            oldState = Keyboard.GetState();
        }

        private void EndScreenLogic()
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R)) ChangeState(GameState.MainMenu);

            oldState = Keyboard.GetState();
        }
        private void SettingsLogic()
        {
            keyState = Keyboard.GetState();

            if (KeyInput.IsKeyClicked(Keys.Escape))
            {
                ChangeState(oldGameState);
            }

            oldState = Keyboard.GetState();
        }
        private void DrawEndScreen()
        {
            spriteBatch.DrawString(font, "Final Money: " + currentSector.CurrentShip.Money, new Vector2(graphics.PreferredBackBufferWidth / 2f, graphics.PreferredBackBufferHeight / 2f) - (font.MeasureString("Final Score: " + currentSector.CurrentShip.Money) / 2), Color.Black);
            spriteBatch.DrawString(font, "Press R to reset.", new Vector2(10, 10), Color.Black);
        }
        void DrawGame()
        {

            spriteBatch.End();

            DrawBackground();

            if (drawParticles) particleEngine.Draw(spriteBatch, camera.GetViewMatrix());

            BeginSpriteBatchCamera(spriteBatch);

            //ship.rotationRender = (float)(Math.Round(ship.Rotation / (Math.PI / 4)) * (Math.PI / 4));
            //spriteBatch.Draw(ship.texture, new Vector2(ship.rectangle.X, ship.rectangle.Y), Rotation: ship.Rotation, origin: ship.Origin);

            currentSector.CurrentStation.Draw(spriteBatch);
            currentSector.CurrentShip.DrawTractorBeam(spriteBatch);
            currentSector.CurrentShip.Draw(spriteBatch);
            //testShip.Draw(spriteBatch);

            currentSector.CurrentShip.DrawTurrets(spriteBatch);

            for (int i = 0; i < currentSector.Asteroids.Count; i++)
            {
                if (IsInView(currentSector.Asteroids[i]))
                {
                    currentSector.Asteroids[i].Draw(spriteBatch);
                }
            }

            spriteBatch.DrawString(font, "Money: " + currentSector.CurrentShip.Money + "k $", camera.ScreenToWorld(viewScorePos), Color.White);
            spriteBatch.DrawString(font, "Lives: " + currentSector.CurrentShip.Health, camera.ScreenToWorld(viewLivesPos), Color.White);

            xPosString = "Xpos: " + currentSector.CurrentShip.Position.X.ToString("F0");
            yPosString = "Ypos: " + currentSector.CurrentShip.Position.Y.ToString("F0");
            viewXPos.X = (halfScreen.X * 2) - font.MeasureString(xPosString).X;
            viewYPos.X = (halfScreen.X * 2) - font.MeasureString(yPosString).X;
            spriteBatch.DrawString(font, xPosString, camera.ScreenToWorld(viewXPos), Color.White);
            spriteBatch.DrawString(font, yPosString, camera.ScreenToWorld(viewYPos), Color.White);

            var energyUIPos = camera.ScreenToWorld(new Vector2(0, halfScreen.Y * 2 - energyIconTexture.Height));
            var energyBarPos = energyUIPos + new Vector2(5, 297);

            var tempRect = energyBarSprite.Rectangle;
            tempRect.Height = Convert.ToInt16((currentSector.CurrentShip.Energy / currentSector.CurrentShip.EnergyMax) * 292);
            var rest = new Vector2(0, -energyBarSprite.Rectangle.Height);

            tempRect.X = Convert.ToInt16((energyBarPos + rest).X);
            tempRect.Y = Convert.ToInt16((energyBarPos + rest).Y);

            energyBarSprite.Rectangle = tempRect;

            spriteBatch.Draw(energyBarSprite.Texture, destinationRectangle: energyBarSprite.Rectangle);

            spriteBatch.Draw(energyIconTexture, energyUIPos);

            spriteBatch.DrawString(font, currentSector.CurrentShip.Energy.ToString(), camera.ScreenToWorld(new Vector2(energyIconTexture.Width, halfScreen.Y * 2 - energyIconTexture.Height) + new Vector2(0, 307)), Color.White);

            if (gameState == GameState.MainGame) aimSprite.Draw(spriteBatch);

        }
        void DrawMenu()
        {

        }

        public void ResetGame()
        {
            currentSector.CurrentShip = ships[0];
            currentSector.CurrentShip.Health = currentSector.CurrentShip.HealthMax;
            currentSector.CurrentShip.Money = defaultScore;
            currentSector.CurrentShip.Position = defaultShipPos;
            currentSector.CurrentShip.Rotation = 0;
            GenerateAsteroids();
        }
        void BeginSpriteBatchCamera(SpriteBatch spriteBatch)
        {
            viewMatrix = camera.GetViewMatrix();
            spriteBatch.Begin(transformMatrix: viewMatrix);
        }
        /// <summary>
        /// Generate all the astroids
        /// </summary>
        private List<Asteroid> GenerateAsteroids()
        {
            var asteroids = new List<Asteroid>();
            for (int repeatIndex2 = -mapSize; repeatIndex2 <= mapSize; repeatIndex2++)
            {
                for (int repeatIndex = -mapSize; repeatIndex <= mapSize; repeatIndex++)
                {
                    for (int i = 0; i < rocksPerBackground; i++)
                    {
                        Vector2 position = new Vector2(repeatIndex * backgroundSize.X + _rnd.Next(0, (int)backgroundSize.X + 1), repeatIndex2 * backgroundSize.Y + _rnd.Next(0, (int)backgroundSize.Y + 1));
                        var rndInt = _rnd.Next(asteroidTextures.Count);
                        var maxHealth = rndInt + 1;
                        var crystalList = new List<Crystal>();
                        float crystalCounter = 0;
                        while (true)
                        {
                            var crystal = GenerateCrystal();

                            if ((int)crystalCounter == rndInt+1) break;
                            if (crystal.Size + crystalCounter > rndInt+1) continue;

                            crystalList.Add(crystal);
                            crystalCounter += crystal.Size;
                        }
                        asteroids.Add(new Asteroid(asteroidTextures[rndInt], position, 1.5f / rndInt, maxHealth, new Bar(redHealth, greenHealth, position, 50, 20, new Vector2(0, -30), maxHealth), rndInt, crystalList));
                    }
                }
            }
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (Vector2.Distance(Vector2.Zero, asteroids[i].Position) < 1100)
                {
                    asteroids.RemoveAt(i);
                    i--;
                }
            }
            return asteroids;
        }

        public Crystal GenerateCrystal()
        {
            var rndInt = _rnd.Next(1, crystalTextureAmount+1);
            var crystal = new Crystal(crystalTextures[rndInt-1], rndInt);
            return crystal;
        }
        /// <summary>
        /// call when spritebatch has ended
        /// </summary>
        void DrawBackground()
        {
            for (var layerIndex = 0; layerIndex < currentSector.Backgrounds.Count; layerIndex++)
            {
                var parallaxFactor = Vector2.One * (0.25f * layerIndex);
                viewMatrix = camera.GetViewMatrix(parallaxFactor);
                spriteBatch.Begin(transformMatrix: viewMatrix);

                for (var repeatIndex2 = -mapSize * 3; repeatIndex2 <= mapSize * 3; repeatIndex2++)
                {
                    for (var repeatIndex = -mapSize * 3; repeatIndex <= mapSize * 3; repeatIndex++)
                    {
                        var texture = currentSector.Backgrounds[layerIndex];
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
        /// <param Name="sprite"> the sprite you want to check</param>
        /// <returns></returns>
        public bool IsInView(Sprite sprite)
        {
            // if not within the horizontal bounds of the screen
            if (sprite.Position.X - sprite.Texture.Width > camera.Position.X + camera.GetBoundingRectangle().Width || sprite.Position.X + sprite.Texture.Width < camera.Position.X)
                return false;

            // if not within the vertical bounds of the screen
            if (sprite.Position.Y - sprite.Texture.Height > camera.Position.Y + camera.GetBoundingRectangle().Height || sprite.Position.Y + sprite.Texture.Height < camera.Position.Y)
                return false;

            // if in view
            return true;
        }
        public float AngleToMouse(Vector2 position)
        {
            return AngleToOther(position, aimSprite.Position);
        }

        //public string FindIndex(string queryString, Dictionary<string, Texture2D> list)
        //{
        //    return list.FindIndex(x => x == queryString);
        //}
        public void NextShip()
        {
            if (currentSector.CurrentShip.ShipCurrentIndex == ships.Count - 1)
            {
                currentSector.CurrentShip.ShipPreviousIndex = currentSector.CurrentShip.ShipCurrentIndex;
                currentSector.CurrentShip.ShipCurrentIndex = 0;
                ChangeShip();
            }
            else
            {
                currentSector.CurrentShip.ShipPreviousIndex = currentSector.CurrentShip.ShipCurrentIndex;
                currentSector.CurrentShip.ShipCurrentIndex++;
                ChangeShip();
            }
        }

        public void ChangeShip()
        {
            var tempRot = currentSector.CurrentShip.Rotation;
            var tempPos = currentSector.CurrentShip.Position;
            var tempIndex = currentSector.CurrentShip.ShipCurrentIndex;
            var tempHealth = currentSector.CurrentShip.Health;
            var tempEnergy = currentSector.CurrentShip.Energy;

            currentSector.CurrentShip = ships[currentSector.CurrentShip.ShipCurrentIndex];

            currentSector.CurrentShip.ShipCurrentIndex = tempIndex;
            currentSector.CurrentShip.Position = tempPos;
            currentSector.CurrentShip.Rotation = tempRot;
            currentSector.CurrentShip.Health = tempHealth;
            currentSector.CurrentShip.Energy = tempEnergy;
        }
        private List<Texture2D> RandomizeBackground()
        {
            layer2.Add(Content.Load<Texture2D>("background2Green"));
            layer2.Add(Content.Load<Texture2D>("background2Blue"));

            var randomBackgroundList = new List<Texture2D>();

            randomBackgroundList.Add(Content.Load<Texture2D>("background1"));

            randomBackgroundList.Add(layer2[_rnd.Next(0, layer2.Count)]);

            randomBackgroundList.Add(Content.Load<Texture2D>("background3"));
            return randomBackgroundList;
        }

        public Sector GenerateSector()
        {
            var sector = new Sector();
            //background
            sector.Backgrounds = RandomizeBackground();

            while (true)
            {
                char[] array = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
                string tempString = array[_rnd.Next(0, array.Length)].ToString() +
                                    array[_rnd.Next(0, array.Length)] +
                                    array[_rnd.Next(0, array.Length)];
                //Name
                //AAA - 000
                string name = tempString + " - " + _rnd.Next(0, 1000);
                if (sectors.Any(x => x.Name == name)) continue;
                sector.Name = name;
                break;
            }

            //asteroids
            sector.Asteroids = GenerateAsteroids();
            sectors.Add(sector);
            return sector;
        }

        public void LoadNames()
        {
            nameArray = new List<string>();
            const Int32 bufferSize = 128;
            using (var fileStream = File.OpenRead("Names.txt"))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8, true, bufferSize))
            {
                String line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    nameArray.Add(line);
                }
            }
        }

        private Name GenerateFullName()
        {
            if (names == null) LoadNames();
            Name name = new Name();
            while (true)
            {
                name.FirstName = nameArray[_rnd.Next(0, names.Count)];
                name.LastName = nameArray[_rnd.Next(0, names.Count)];
                name.FullName = name.FirstName + " " + name.LastName;

                if (names.All(x => x.FullName != name.FullName)) break;
            }
            names.Add(name);

            return name;
        }
        public static double GetRandomNumber(double minimum, double maximum)
        {
            return _rnd.NextDouble() * (maximum - minimum) + minimum;
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