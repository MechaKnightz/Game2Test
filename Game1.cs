using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using MonoGame.Extended;
using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;
using GeonBit.UI;
using GeonBit.UI.Entities;
using System.IO.IsolatedStorage;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Game2Test
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        MouseState mouseState, oldMouseState;
        KeyboardState keyState, oldState;

        //lists
        List<Asteroid> currentAsteroids = new List<Asteroid>();
        List<Texture2D> asteroidTextures = new List<Texture2D>();

        List<Texture2D> backgrounds = new List<Texture2D>();
        List<int> highscores = new List<int>();

        List<Turret> turrets0 = new  List<Turret>();
        List<Turret> turrets1 = new List<Turret>();
        List<Turret> turrets2 = new List<Turret>();
        List<Turret> turretsStation = new List<Turret>();

        List<Sector> sectors = new List<Sector>();

        Dictionary<string, Texture2D> ship0Dictionary = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> ship1Dictionary = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> ship2Dictionary = new Dictionary<string, Texture2D>();
        Dictionary<string, Texture2D> stationDictionary = new Dictionary<string, Texture2D>();

        Dictionary<string, Shot> shot0Dictionary = new Dictionary<string, Shot>();
        Dictionary<string, Shot> shot1Dictionary = new Dictionary<string, Shot>();
        Ship currentStationShip;

        Texture2D stationTexture;

        List<Ship> ships = new List<Ship>();
        List<Ship> availableShips = new List<Ship>();
        List<Ship> ownedShips = new List<Ship>();
            
        Vector2 defaultShipPos, tempPos, tempPos4, halfScreenPos, halfScreen;
        Rectangle speedbarRectangle, speedbarRectangle2;
        Texture2D shot0Texture, shot1Texture, aimTexture, turret0Texture, turret1Texture, shieldIconTexture,
            redPixel, greenPixel, turretStationTexture, transparent;

        Shot shot0, shot1;
        Sprite greenHealth;
        Sprite redHealth;

        SpriteFont font;
        System.Random rnd;
        int score, speed;
        Color[] menuColor = new Color[3];
        int selected = 0;
        int defaultScore = 0;
        int defaultSpeed = 10;
        int menuLength = 3;
        int settingsMenuLength = 2;
        ParticleEngine particleEngine;
        bool moving = false;
        bool drawParticles = false;
        int movingDelayCounter = 0;
        GameState gameState = (GameState)2;
        Ship currentShip;
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
        private Panel mainMenuPanel, shopPanel, pauseMenuPanel;
        Button mainMenuPlayButton, mainMenuSettingsButton, mainMenuExitButton, warpButton, shopHUDButton;
        List<Button> shopButtons = new List<Button>();
        List<Paragraph> shopDescriptions = new List<Paragraph>();
        private PanelTabs tabs;
        private PanelTabs.TabData tab0, tab1, tab2;
        private float distanceToStation;
        private float shopRadius = 200;
        Sector currentSector;
        List<Texture2D> layer2 = new List<Texture2D>();
        Data data;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            UserInterface.Initialize(Content, BuiltinThemes.hd);
            UserInterface.UseRenderTarget = true;
            Paragraph.BaseSize = 1f;
            UserInterface.GlobalScale = 1f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            camera = new Camera2D(GraphicsDevice);

            //graphics.PreferredBackBufferWidth = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width; ;  // window width 801
            //graphics.PreferredBackBufferHeight = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height; ;   // window height 701
            graphics.IsFullScreen = false;

            //graphics.PreferredBackBufferWidth = GraphicsDevice.Viewport.Width;
            //graphics.PreferredBackBufferHeight = GraphicsDevice.Viewport.Height;
            //graphics.IsFullScreen = true;

            graphics.PreferredBackBufferWidth = 801;  // window width 801
            graphics.PreferredBackBufferHeight = 701;   // window height 701
            //graphics.IsFullScreen = false;

            graphics.ApplyChanges();

            halfScreen = new Vector2(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2);

            defaultShipPos = Vector2.Zero;

            turret0Texture = Content.Load<Texture2D>("turret0");
            turret1Texture = Content.Load<Texture2D>("turret1");
            turretStationTexture = Content.Load<Texture2D>("turretStationTexture");

            shot0Texture = Content.Load<Texture2D>("shot0");
            shot0 = new Shot(shot0Texture, 60, "default", 15, 2);
            shot0Dictionary = new Dictionary<string, Shot>();
            shot0Dictionary.Add(shot0.name, shot0);

            shot1Texture = Content.Load<Texture2D>("shot1");
            shot1 = new Shot(shot1Texture, 60, "default", 15, 1);
            shot1Dictionary = new Dictionary<string, Shot>();
            shot1Dictionary.Add(shot1.name, shot1);

            turrets0.Add(new Turret(turret1Texture, new Vector2(-7, -10), new Vector2(-7, -10), 0, shot1Dictionary, 150));
            turrets0.Add(new Turret(turret1Texture, new Vector2(-7, 10), new Vector2(-7, 10), 0, shot1Dictionary, 150));

            turrets1.Add(new Turret(turret1Texture, new Vector2(-10, -10), new Vector2(-10, -10), 0, shot0Dictionary, 150));
            turrets1.Add(new Turret(turret1Texture, new Vector2(-10, 10), new Vector2(-10, 10), 0, shot0Dictionary, 150));

            turrets2.Add(new Turret(turret0Texture, new Vector2(5, 0), new Vector2(5, 0), 0, shot0Dictionary, 150));

            turretsStation.Add(new Turret(turretStationTexture, new Vector2(5, 27), new Vector2(0, 27), 0, shot0Dictionary, 150));
            turretsStation.Add(new Turret(turretStationTexture, new Vector2(5, -27), new Vector2(5, -27), 0, shot0Dictionary, 150));

            //ship0
            ship0Dictionary.Add("default", Content.Load<Texture2D>("ship0Texture0"));
            ship0Dictionary.Add("left", Content.Load<Texture2D>("ship0Texture1"));
            ship0Dictionary.Add("right", Content.Load<Texture2D>("ship0Texture2"));

            var turret0Collection = new Dictionary<string, List<Turret>>();
            turret0Collection.Add("primary",turrets0);

            var turret1Collection = new Dictionary<string, List<Turret>>();
            turret1Collection.Add("primary", turrets1);

            var turret2Collection = new Dictionary<string, List<Turret>>();
            turret2Collection.Add("primary", turrets2);

            var turretStationCollection = new Dictionary<string, List<Turret>>();
            turretStationCollection.Add("primary", turretsStation);

            var ship0 = new Ship(ship0Dictionary, defaultShipPos, turret0Collection, 10, 1000, 5);
            ship0.cost = 0f;
            ship0.description = "Human ship 1 description";
            ship0.Name = "Human ship 1";
            ships.Add(ship0);
            ownedShips.Add(ship0);
            availableShips.Add(ship0); //TODO REMOVE
            //ship0 end

            //ship1
            ship1Dictionary.Add("default", Content.Load<Texture2D>("ship1Texture0"));
            ship1Dictionary.Add("left", Content.Load<Texture2D>("ship1Texture1"));
            ship1Dictionary.Add("right", Content.Load<Texture2D>("ship1Texture2"));

            var ship1 = new Ship(ship1Dictionary, defaultShipPos, turret1Collection, 10, 1000, 5);
            ship1.cost = 15f;
            ship1.description = "Human ship 2 description";
            ship1.Name = "Human ship 2";
            ships.Add(ship1);
            availableShips.Add(ship1);
            //ship1 end

            //ship2
            ship2Dictionary.Add("default", Content.Load<Texture2D>("ship2Texture0"));
            ship2Dictionary.Add("left", Content.Load<Texture2D>("ship2Texture1"));
            ship2Dictionary.Add("right", Content.Load<Texture2D>("ship2Texture2"));

            var ship2 = new Ship(ship2Dictionary, defaultShipPos, turret2Collection, 10, 1000, 5);
            ship2.cost = 10f;
            ship2.description = "Alien ship 1 description";
            ship2.Name = "Alien ship 1";
            ships.Add(ship2);
            availableShips.Add(ship2);
            //ship2 end

            //stationShip

            stationDictionary.Add("default", Content.Load<Texture2D>("stationTexture"));

            currentStationShip = new Ship(stationDictionary, defaultShipPos, turretStationCollection, 100, 500, 15);
            
            //stationShip end

            currentShip = ships[0];

            redPixel = Content.Load<Texture2D>("redPixel");
            greenPixel = Content.Load<Texture2D>("greenPixel");
            redHealth = new Sprite(redPixel);
            greenHealth = new Sprite(greenPixel);
            shieldIconTexture = Content.Load<Texture2D>("shieldIcon");
            aimTexture = Content.Load<Texture2D>("aimWhite");
            aimSprite = new Sprite(aimTexture, new Vector2(halfScreen.X, halfScreen.Y), new Rectangle(0, 0, aimTexture.Width, aimTexture.Height));
            font = Content.Load<SpriteFont>("font");
            rnd = new System.Random();
            speedbarRectangle = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301, 20);
            speedbarRectangle2 = new Rectangle((graphics.PreferredBackBufferWidth / 2) - 151, (graphics.PreferredBackBufferHeight / 2) + (int)font.MeasureString("Speed").Y, 301, 20);
            speed = defaultSpeed;
            halfScreenPos = new Vector2(halfScreen.X, halfScreen.Y);
            transparent = Content.Load<Texture2D>("transparent");

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


            // TODO: https://msdn.microsoft.com/en-us/library/bb531208.aspx

            currentSector = GenerateSector();

            //particles
            List<Texture2D> textures = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("red"));
            textures.Add(Content.Load<Texture2D>("orange"));
            textures.Add(Content.Load<Texture2D>("yellow"));
            particleEngine = new ParticleEngine(textures, new Vector2(0, 0));

            
            //main menu

            GenerateInterface(GameState.MainMenu);

            //end menu
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (!IsActive)
                return;

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
                    ChangeColor();
                    MenuLogic(gameTime);
                    break;
                case GameState.SettingsMenu: //settings
                    ChangeColor();
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
                    DrawSettings();
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

            if (moving) movingDelayCounter = 5;
            else movingDelayCounter--;
            if (movingDelayCounter <= 0) drawParticles = false;
            else drawParticles = true;
            moving = false;

            oldState = Keyboard.GetState();
            base.Draw(gameTime);
        }

        void CollisionTest() //kollar om shots intersects med asteroids
        {
            for(int i = 0; i < currentSector.Asteroids.Count; i++)
            {
                if (IsInView(currentSector.Asteroids[i]))
                {
                    var tempTurret = new Turret();
                    var tempShot = new Shot();
                    if (currentShip.TurretCollision(currentSector.Asteroids[i].rectangle, out tempTurret, out tempShot))
                    {
                        currentSector.Asteroids[i].health -= tempShot.Damage;
                        if (currentSector.Asteroids[i].health <= 0)
                        {
                            currentSector.Asteroids.RemoveAt(i);
                            score++;
                        }
                    }
                    var tempTurret2 = new Turret();
                    var tempShot2 = new Shot();
                    if (currentStationShip.TurretCollision(currentSector.Asteroids[i].rectangle, out tempTurret2, out tempShot2))
                    {
                        currentSector.Asteroids[i].health -= tempShot2.Damage;
                        if (currentSector.Asteroids[i].health <= 0)
                        {
                            currentSector.Asteroids.RemoveAt(i);
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
                    if (currentSector.Asteroids[i].rectangle.Intersects(currentShip.rectangle))
                    {
                        currentShip.health--;
                        currentSector.Asteroids.RemoveAt(i);
                        if (currentShip.health <= 0)
                        {
                            ChangeState(GameState.EndScreen);
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
                tempPos = currentShip.Position;
                tempPos.X += (float)System.Math.Cos(currentShip.rotation) * (speed * speedBoostConst);
                tempPos.Y += (float)System.Math.Sin(currentShip.rotation) * (speed * speedBoostConst);
                currentShip.SetPos(tempPos);
                moving = true;
            }
            else if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                tempPos = currentShip.Position;
                tempPos.X += (float)(System.Math.Cos(currentShip.rotation)) * speed;
                tempPos.Y += (float)(System.Math.Sin(currentShip.rotation)) * speed;
                currentShip.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                tempPos = currentShip.Position;
                tempPos.X -= (float)(System.Math.Cos(currentShip.rotation)) * (speed / 5);
                tempPos.Y -= (float)(System.Math.Sin(currentShip.rotation)) * (speed / 5);
                currentShip.SetPos(tempPos);
                moving = true;
            }

            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                currentShip.rotation -= 0.05f;
                if(currentShip.textureDictionary.ContainsKey("left"))
                {
                    currentShip.textureIndexCounter = "left";
                }
                currentShip.Update();

            }
            else if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                currentShip.rotation += 0.05f;
                if (currentShip.textureDictionary.ContainsKey("right"))
                {
                    currentShip.textureIndexCounter = "right";
                }
                currentShip.Update();
            }

            distanceToStation = Vector2.Distance(currentShip.Position, currentStationShip.Position);

            if (keyState.IsKeyDown(Keys.R) && !oldState.IsKeyDown(Keys.R))
            {
                currentShip.SetPos(defaultShipPos);
            }
            if (keyState.IsKeyDown(Keys.G) && !oldState.IsKeyDown(Keys.G) && distanceToStation < shopRadius)
            {
                if(gameState == GameState.MainGame) ChangeState(GameState.ShopMenu);
                else if(gameState == GameState.ShopMenu) ChangeState(GameState.MainGame);
            }

            tempPos4 = camera.Position;
            tempPos4 = currentShip.Position - halfScreen;
            camera.Position = tempPos4;

            aimSprite.SetPos(camera.ScreenToWorld(mouseState.Position.ToVector2()));

            foreach (var t in currentStationShip.turrets)
            {
                foreach(var tur in t.Value)
                {
                    Asteroid asteroid = new Asteroid();
                    float tempDistance = 99999999;
                    for (int i = 0; i < currentSector.Asteroids.Count; i++) //TODO fix targetting
                    {
                        if (Vector2.Distance(currentSector.Asteroids[i].Position, tur.Position) < tempDistance)
                        {
                            tempDistance = Vector2.Distance(currentSector.Asteroids[i].Position, tur.Position);
                            asteroid.Position = currentSector.Asteroids[i].Position;
                            asteroid.rotation = currentSector.Asteroids[i].rotation;
                            asteroid.speed = currentSector.Asteroids[i].speed;
                            asteroid.acceleration = currentSector.Asteroids[i].acceleration;
                            var tempTime = tempDistance / tur.shots["default"].speed;
                            for (int j = 0; j < tempTime; j++)
                            {
                                asteroid.MoveTowardsPosition(currentShip.Position);
                            }
                        }
                    }
                    if (tempDistance < 800)
                    {
                        var rot = AngleToOther(tur.Position, asteroid.Position);
                        const float rotConst = 0.03f; //TODO fix the if statements
                        if (tur.rotation > rot) tur.rotation -= rotConst;
                        else if (tur.rotation < rot) tur.rotation += rotConst;
                        //t.Value.rotation = AngleToOther(t.Value.position, asteroid.position));
                        float diff = Math.Abs(MathHelper.WrapAngle(rot - tur.rotation));
                        if (diff < 0.2) currentStationShip.Fire("primary", "default");
                    }
                }
            }
            //stationShip.turrets["primary"] = stationShip.ShuffleTurrets(stationShip.turrets["primary"]);

            foreach (var t in currentShip.turrets)
            {
                foreach(var tur in t.Value)
                {
                    tur.rotation = AngleToMouse(tur.Position);
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton != ButtonState.Pressed && gameState == GameState.MainGame)
            {
                currentShip.Fire("primary", "default");
            }

            if (distanceToStation < shopRadius)
            {
                shopHUDButton.Disabled = false;
            }
            else
            {
                shopHUDButton.Disabled = true;
            }
            if (distanceToStation > shopRadius && gameState != GameState.MainGame)
            {
                ChangeState(GameState.MainGame);
            }

            foreach (var t in currentSector.Asteroids)
            {
                if (Vector2.Distance(t.Position, currentShip.Position) < 1000)
                {
                    t.MoveTowardsPosition(currentShip.Position);
                }
            }
            currentShip.UpdateEnergy();
            currentStationShip.UpdateEnergy();

            if (currentShip.rotation > (float)Math.PI * 2f) currentShip.rotation -= (float)Math.PI * 2f;
            else if(currentShip.rotation < (float)-Math.PI * 2f) currentShip.rotation += (float)Math.PI * 2f;
            currentShip.UpdateTurrets();
            currentStationShip.UpdateTurrets();

            particleEngine.EmitterLocation = currentShip.GetBackOfShip();
            particleEngine.Update();

            CollisionTest();
            CollisionTest2();

            if (keyState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape) && gameState == GameState.MainGame)
            {
                ChangeState(GameState.PauseMenu);
            }

            oldState = Keyboard.GetState();
            oldMouseState = Mouse.GetState();
        }
        void MenuLogic(GameTime gameTime)
        {
            
        }

        private void ChangeState(GameState tempGameState)
        {
            GenerateInterface(tempGameState);
            RemoveInterface(gameState);
            gameState = tempGameState;
        }

        private void RemoveInterface(GameState gameState)
        {
            switch(gameState)
            {
                case GameState.MainMenu:
                    mainMenuPanel.Visible = false;
                    break;
                case GameState.ShopMenu:
                    shopPanel.Visible = false;
                    shopButtons.Clear();
                    shopDescriptions.Clear();
                    break;
                case GameState.MainGame:
                    shopHUDButton.Visible = false;
                    break;
                case GameState.PauseMenu:
                    pauseMenuPanel.Visible = false;
                    break;
            }
        }

        private void GenerateInterface(GameState tempGameState)
        {
            switch (tempGameState)
            {
                case GameState.MainGame:
                    UserInterface.SetCursor(transparent);
                    shopHUDButton = new Button("Shop (G)", anchor: Anchor.BottomRight, size: new Vector2(150, 50), skin: ButtonSkin.Alternative);
                    shopHUDButton.ButtonParagraph.WrapWords = false;
                    shopHUDButton.OnClick = (Entity btn) =>
                    {
                        if(gameState == GameState.MainGame) ChangeState(GameState.ShopMenu);
                        else if(gameState == GameState.ShopMenu) ChangeState(GameState.MainGame);
                    };
                    UserInterface.AddEntity(shopHUDButton);
                    break;
                case GameState.EndScreen:
                    UserInterface.SetCursor(CursorType.Default);
                    break;
                case GameState.MainMenu:
                    mainMenuPanel = new Panel(new Vector2(300, 500));
                    UserInterface.AddEntity(mainMenuPanel);
                    mainMenuPlayButton = new Button("Play");
                    mainMenuSettingsButton = new Button("Settings");
                    mainMenuExitButton = new Button("Quit");

                    mainMenuPanel.AddChild(mainMenuPlayButton);
                    mainMenuPanel.AddChild(mainMenuSettingsButton);
                    mainMenuPanel.AddChild(mainMenuExitButton);

                    mainMenuPlayButton.OnClick = (Entity btn) => {
                        ResetGame();
                        ChangeState(GameState.MainGame);
                    };
                    mainMenuSettingsButton.OnClick = (Entity btn) => {
                        ChangeState(GameState.SettingsMenu);
                    };
                    mainMenuExitButton.OnClick = (Entity btn) => {
                        Exit();
                    };
                    break;
                case GameState.SettingsMenu:
                    UserInterface.SetCursor(CursorType.Default);
                    break;
                case GameState.ShopMenu:
                    UserInterface.SetCursor(CursorType.Default);
                    shopPanel = new Panel(new Vector2(500, 300), anchor: Anchor.TopLeft, offset: new Vector2(0, 70));
                    UserInterface.AddEntity(shopPanel);
                    tabs = new PanelTabs();
                    tab0 = tabs.AddTab("Warp");
                    tab1 = tabs.AddTab("Shop");
                    tab2 = tabs.AddTab("Upgrades");
                    tab2.button.ButtonParagraph.WrapWords = false;

                    //first tab
                    warpButton = new Button("Warp to new sector");
                    warpButton.OnClick = (Entity btn) => {
                        currentSector = GenerateSector();
                        ChangeState(GameState.MainGame);
                    };

                    tab0.panel.AddChild(warpButton);
                    tab0.panel.AddChild(new HorizontalLine());
                    
                    var dropDown = new DropDown(new Vector2(0, 0));
                    dropDown.SelectedTextPanelParagraph.Text = "Warp to an old sector";
                    for (int i = 0; i < sectors.Count; i++)
                    {
                        dropDown.AddItem(sectors[i].Name);
                    }
                    dropDown.AttachedData = dropDown.SelectedValue;
                    dropDown.OnValueChange = (Entity drop) =>
                    {
                        currentSector = sectors.FirstOrDefault(x => x.Name == dropDown.SelectedValue);
                    };
                    tab0.panel.AddChild(dropDown);

                    //for (int i = 0; i < sectors.Count; i++)
                    //{
                    //    var button = new Button("Sector: " + sectors[i].Name);
                    //    button.Identifier = i.ToString();
                    //    button.OnClick = (Entity btn) => {
                    //        currentSector = sectors[Convert.ToInt16(btn.Identifier)];
                    //        ChangeState(GameState.MainGame);
                    //    };
                    //    tab0.panel.AddChild(button);
                    //    tab0.panel.AddChild(new HorizontalLine());
                    //}

                    //second tab

                    int height = 200;
                    //TODO: offset = text width, measurestring
                    for (int i = 0; i < availableShips.Count; i++)
                    {
                        var offset = new Vector2(0, (height + 20) * i);
                        var img = new Image(availableShips[i].texture, new Vector2(150, 100), anchor: Anchor.TopLeft);
                        img.SetOffset(offset);

                        shopDescriptions.Add(new Paragraph(availableShips[i].description));
                        img.Identifier = i.ToString();
                        img.OnMouseEnter = (Entity entity) =>
                        {
                            UserInterface.AddEntity(shopDescriptions[Convert.ToUInt16(entity.Identifier)]);
                        };
                        img.OnMouseLeave = (Entity entity) =>
                        {
                            UserInterface.RemoveEntity(shopDescriptions[Convert.ToUInt16(entity.Identifier)]);
                        };
                        tab1.panel.AddChild(img);

                        var name = new Paragraph(availableShips[i].Name, anchor: Anchor.TopRight);
                        name.SetOffset(offset);
                        tab1.panel.AddChild(name);

                        var cost = new Paragraph("Cost: " + availableShips[i].cost.ToString(), anchor: Anchor.CenterRight);
                        cost.SetOffset(offset - new Vector2(-25, 50));
                        tab1.panel.AddChild(cost);

                        shopButtons.Add(new Button("Buy", size: new Vector2(100,50), anchor: Anchor.CenterRight));
                        shopButtons[i].SetOffset(offset + new Vector2(0, 0));
                        shopButtons[i].ButtonParagraph.SetOffset(new Vector2(0, -22));
                        tab1.panel.AddChild(shopButtons[i]);
                        tab1.panel.AddChild(new HorizontalLine());

                        shopButtons[i].Identifier = i.ToString();
                        shopButtons[i].OnClick = (Entity btn) => {
                            BuyShip(availableShips[Convert.ToUInt16(btn.Identifier)]);
                        };
                    }

                    tab1.panel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
                    tab1.panel.Scrollbar.Max = Convert.ToUInt16((availableShips.Count * (height+20)) - shopPanel.Size.Y);
                    tab1.panel.Scrollbar.StepsCount = (uint)availableShips.Count*5;

                    //second tab end

                    shopPanel.AddChild(tabs);
                    break;
                case GameState.PauseMenu:
                    UserInterface.SetCursor(CursorType.Default);
                    //panel
                    pauseMenuPanel = new Panel(new Vector2(300, 500));
                    UserInterface.AddEntity(pauseMenuPanel);
                    //resumebutton
                    var resumeButton = new Button("Resume");
                    resumeButton.OnClick = (Entity btn) =>
                    {
                        ChangeState(GameState.MainGame);
                    };
                    pauseMenuPanel.AddChild(resumeButton);
                    //save button
                    var saveButton = new Button("Save game");
                    saveButton.OnClick = (Entity btn) =>
                    {
                        SaveGame();
                    };
                    pauseMenuPanel.AddChild(saveButton);
                    //load button
                    var loadButton = new Button("Load game");
                    loadButton.OnClick = (Entity btn) =>
                    {
                        //todo
                    };
                    pauseMenuPanel.AddChild(loadButton);
                    //main menu button
                    var mainMenuButton = new Button("Main Menu");
                    mainMenuButton.OnClick = (Entity btn) =>
                    {
                        ChangeState(GameState.MainMenu);
                    };
                    pauseMenuPanel.AddChild(mainMenuButton);
                    //exit button
                    var exitButton = new Button("Exit");
                    exitButton.OnClick = (Entity btn) =>
                    {
                        Exit();
                    };
                    pauseMenuPanel.AddChild(exitButton);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tempGameState), tempGameState, null);
            }
        }
        private void PauseMenuLogic(GameTime gameTime)
        {
            keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape) && !oldState.IsKeyDown(Keys.Escape))
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

            if (keyState.IsKeyDown(Keys.Up) && !oldState.IsKeyDown(Keys.Up)) selected--;
            if (keyState.IsKeyDown(Keys.Down) && !oldState.IsKeyDown(Keys.Down)) selected++;
            if (selected < 0) selected = settingsMenuLength - 1;
            if (selected > settingsMenuLength - 1) selected = 0;

            if (keyState.IsKeyDown(Keys.Enter) && !oldState.IsKeyDown(Keys.Enter)) if (selected == 0) ChangeState(GameState.MainMenu);

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

            DrawBackground();

            if (drawParticles) particleEngine.Draw(spriteBatch, camera.GetViewMatrix());

            BeginSpriteBatchCamera(spriteBatch);

            //ship.rotationRender = (float)(Math.Round(ship.rotation / (Math.PI / 4)) * (Math.PI / 4));
            //spriteBatch.Draw(ship.texture, new Vector2(ship.rectangle.X, ship.rectangle.Y), rotation: ship.rotation, origin: ship.Origin);

            currentStationShip.Draw(spriteBatch);
            currentShip.Draw(spriteBatch);

            currentShip.DrawTurrets(spriteBatch);

            for(int i = 0; i < currentSector.Asteroids.Count; i++)
            {
                if (IsInView(currentSector.Asteroids[i]))
                {
                    currentSector.Asteroids[i].Draw(spriteBatch);
                }
            }

            spriteBatch.DrawString(font, "Score: " + score.ToString(), camera.ScreenToWorld(viewScorePos), Color.White);
            spriteBatch.DrawString(font, "Lives: " + currentShip.health, camera.ScreenToWorld(viewLivesPos), Color.White);

            xPosString = "Xpos: " + currentShip.Position.X.ToString("F0");
            yPosString = "Ypos: " + currentShip.Position.Y.ToString("F0");
            viewXPos.X = (halfScreen.X * 2) - font.MeasureString(xPosString).X;
            viewYPos.X = (halfScreen.X * 2) - font.MeasureString(yPosString).X;
            spriteBatch.DrawString(font, xPosString, camera.ScreenToWorld(viewXPos), Color.White);
            spriteBatch.DrawString(font, yPosString, camera.ScreenToWorld(viewYPos), Color.White);

            spriteBatch.Draw(shieldIconTexture, camera.ScreenToWorld(new Vector2(0, halfScreen.Y*2 - shieldIconTexture.Height)));
            spriteBatch.DrawString(font, currentShip.energy.ToString(), camera.ScreenToWorld(new Vector2(shieldIconTexture.Width, halfScreen.Y * 2 - shieldIconTexture.Height)), Color.White);

            if(gameState == GameState.MainGame) aimSprite.Draw(spriteBatch);

        }
        void DrawMenu()
        {

        }
        void DrawSettings()
        {
            spriteBatch.DrawString(font, "Back", new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.MeasureString("Back").X / 2), (graphics.PreferredBackBufferHeight / 2) - font.MeasureString("Speed").Y), menuColor[0]);
            spriteBatch.DrawString(font, "Speed", new Vector2(graphics.PreferredBackBufferWidth / 2 - (font.MeasureString("Speed").X / 2), (graphics.PreferredBackBufferHeight / 2)), menuColor[1]);
            spriteBatch.Draw(currentShip.texture, speedbarRectangle2, Color.White);
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
            currentShip = ships[0];
            currentShip.health = currentShip.healthMax;
            score = defaultScore;
            currentShip.Position = defaultShipPos;
            currentShip.rotation = 0;
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
        List<Asteroid> GenerateAsteroids()
        {
            var asteroids = new List<Asteroid>();
            for (int repeatIndex2 = -mapSize; repeatIndex2 <= mapSize; repeatIndex2++)
            {
                for (int repeatIndex = -mapSize; repeatIndex <= mapSize; repeatIndex++)
                {
                    for (int i = 0; i < rocksPerBackground; i++)
                    {
                        Vector2 position = new Vector2((repeatIndex * backgroundSize.X) + rnd.Next(0, (int)backgroundSize.X + 1), (repeatIndex2 * backgroundSize.Y) + rnd.Next(0, (int)backgroundSize.Y + 1));
                        var rndInt = rnd.Next(asteroidTextures.Count);
                        var maxHealth = rndInt + 1;
                        asteroids.Add(new Asteroid(asteroidTextures[rndInt], position, 1.5f/rndInt, maxHealth, new Bar(redHealth, greenHealth, position,50, 20, new Vector2(0,-30), maxHealth)));
                    }
                }
            }
            for (int i = 0; i < asteroids.Count; i++)
            {
                if (Vector2.Distance(currentStationShip.Position, asteroids[i].Position) < 1100)
                {
                    asteroids.RemoveAt(i);
                    i--;
                }
            }
            return asteroids;
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
            if (sprite.Position.X - sprite.texture.Width > camera.Position.X + camera.GetBoundingRectangle().Width || sprite.Position.X + sprite.texture.Width < camera.Position.X)
                return false;

            // if not within the vertical bounds of the screen
            if (sprite.Position.Y - sprite.texture.Height > camera.Position.Y + camera.GetBoundingRectangle().Height || sprite.Position.Y + sprite.texture.Height < camera.Position.Y)
                return false;

            // if in view
            return true;
        }
        public float AngleToMouse(Vector2 position)
        {
            return (float)AngleToOther(position, aimSprite.Position);
        }
        public float AngleToOther(Vector2 main, Vector2 other)
        {
            return (float)Math.Atan2(other.Y - main.Y, other.X - main.X);
        }
        //public string FindIndex(string queryString, Dictionary<string, Texture2D> list)
        //{
        //    return list.FindIndex(x => x == queryString);
        //}
        public void NextShip()
        {
            if (currentShip.shipCurrentIndex == ships.Count-1)
            {
                currentShip.shipPreviousIndex = currentShip.shipCurrentIndex;
                currentShip.shipCurrentIndex = 0;
                ChangeShip();
            }
            else
            {
                currentShip.shipPreviousIndex = currentShip.shipCurrentIndex;
                currentShip.shipCurrentIndex++;
                ChangeShip();
            }
        }

        public void BuyShip(Ship ship)
        {
            if(ownedShips.All(x => x.Name != ship.Name)) ownedShips.Add(ship);
            var tempRot = currentShip.rotation;
            var tempPos = currentStationShip.Position;
            var tempIndex = currentShip.shipCurrentIndex;
            var tempHealth = currentShip.health;
            var tempEnergy = currentShip.energy;
            currentShip = ship;
            currentShip.shipCurrentIndex = tempIndex;
            currentShip.Position = tempPos;
            currentShip.rotation = tempRot;
            currentShip.health = tempHealth;
            currentShip.energy = tempEnergy;

            currentShip.Update();
        }
        public void ChangeShip()
        {
            var tempRot = currentShip.rotation;
            var tempPos = currentShip.Position;
            var tempIndex = currentShip.shipCurrentIndex;
            var tempHealth = currentShip.health;
            var tempEnergy = currentShip.energy;
            currentShip = ships[currentShip.shipCurrentIndex];
            currentShip.shipCurrentIndex = tempIndex;
            currentShip.Position = tempPos;
            currentShip.rotation = tempRot;
            currentShip.health = tempHealth;
            currentShip.energy = tempEnergy;

            currentShip.Update();
        }
        private List<Texture2D> RandomizeBackground()
        {
            layer2.Add(Content.Load<Texture2D>("background2Green"));
            layer2.Add(Content.Load<Texture2D>("background2Blue"));
            var randomBackgroundList = new List<Texture2D>();
            randomBackgroundList.Add(Content.Load<Texture2D>("background1"));

            randomBackgroundList.Add(layer2[rnd.Next(0, layer2.Count)]);

            randomBackgroundList.Add(Content.Load<Texture2D>("background3"));
            return randomBackgroundList;
        }

        private Sector GenerateSector()
        {
            var sector = new Sector();
            //background
            sector.Backgrounds = RandomizeBackground();

            char[] array = new[] {'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};
            string tempString = array[rnd.Next(0, array.Length)].ToString() +
                                array[rnd.Next(0, array.Length)] +
                                array[rnd.Next(0, array.Length)];
            //Name
            //AAA - 000
            string name = tempString +" - " + rnd.Next(0, 1000).ToString();
            sector.Name = name;

            //asteroids
            sector.Asteroids = GenerateAsteroids();
            //TODO make station dependant on sector
            sectors.Add(sector);
            return sector;
        }
        private void SaveGame()
        {
            data.score = score;

            data.ownedShips = ownedShips;

            List<Data> _data = new List<Data>();
            _data.Add(data);
            string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented);

            File.WriteAllText("save.json", json);
        }
        public void LoadGame()
        {
            
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