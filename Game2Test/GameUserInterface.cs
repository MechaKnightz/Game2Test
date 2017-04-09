using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;

namespace Game2Test
{
    public class GameUserInterface
    {
        private Game1 _game1;
        private Button mainMenuPlayButton;
        private Button mainMenuSettingsButton;
        private Button mainMenuExitButton;
        private Button warpButton;
        private PanelTabs tabs;
        private PanelTabs.TabData tab0;
        private PanelTabs.TabData tab1;
        private PanelTabs.TabData tab2;
        private Data data;

        public GameUserInterface(Game1 game1)
        {
            _game1 = game1;
        }

        private int CurrentScreenHeight
        {
            get { return _game1.graphics.PreferredBackBufferHeight; }
            set { _game1.graphics.PreferredBackBufferHeight = value; }
        }

        private int CurrentScreenWidth
        {
            get { return _game1.graphics.PreferredBackBufferWidth; }
            set { _game1.graphics.PreferredBackBufferWidth = value; }
        }


        internal void GenerateUserInterface(GameState tempGameState)
        {
            switch (tempGameState)
            {
                case GameState.MainGame:
                    UserInterface.SetCursor(_game1.transparent);
                    _game1.shopHUDButton = new Button("Shop (G)", anchor: Anchor.BottomRight, size: new Vector2(150, 50), skin: ButtonSkin.Alternative);
                    _game1.shopHUDButton.ButtonParagraph.Scale = 1.0f;
                    _game1.shopHUDButton.ButtonParagraph.WrapWords = false;
                    _game1.shopHUDButton.OnClick = (Entity btn) =>
                    {
                        if (_game1.gameState == GameState.MainGame) _game1.ChangeState(GameState.ShopMenu);
                        else if (_game1.gameState == GameState.ShopMenu) _game1.ChangeState(GameState.MainGame);
                    };
                    UserInterface.AddEntity(_game1.shopHUDButton);
                    break;
                case GameState.EndScreen:
                    UserInterface.SetCursor(CursorType.Default);
                    break;
                case GameState.MainMenu:
                    _game1.mainMenuPanel = new Panel(new Vector2(300, 500));
                    UserInterface.AddEntity(_game1.mainMenuPanel);
                    mainMenuPlayButton = new Button("Play");
                    mainMenuPlayButton.ButtonParagraph.Scale = 0.5f;
                    var mainMenuControlsButton = new Button("Controls");
                    mainMenuControlsButton.ButtonParagraph.Scale = 0.5f;
                    mainMenuSettingsButton = new Button("Settings");
                    mainMenuSettingsButton.ButtonParagraph.Scale = 0.5f;
                    mainMenuExitButton = new Button("Quit");
                    mainMenuExitButton.ButtonParagraph.Scale = 0.5f;

                    _game1.mainMenuPanel.AddChild(mainMenuPlayButton);
                    _game1.mainMenuPanel.AddChild(mainMenuControlsButton);
                    _game1.mainMenuPanel.AddChild(mainMenuSettingsButton);
                    _game1.mainMenuPanel.AddChild(mainMenuExitButton);

                    mainMenuControlsButton.OnClick = (Entity btn) =>
                    {
                        _game1.ChangeState(GameState.ControlsMenu);
                    };
                    mainMenuPlayButton.OnClick = (Entity btn) =>
                    {
                        _game1.ResetGame();
                        _game1.ChangeState(GameState.MainGame);
                    };
                    mainMenuSettingsButton.OnClick = (Entity btn) =>
                    {
                        _game1.ChangeState(GameState.SettingsMenu);
                    };
                    mainMenuExitButton.OnClick = (Entity btn) =>
                    {
                        _game1.Exit();
                    };
                    break;
                case GameState.ControlsMenu:
                    _game1.controlsMenuPanel = new Panel(new Vector2(700, 500));
                    UserInterface.AddEntity(_game1.controlsMenuPanel);

                    var backButton2 = new Button("Back");
                    backButton2.ButtonParagraph.Scale = 0.5f;
                    backButton2.OnClick = (Entity btn) =>
                    {
                        _game1.ChangeState(GameState.MainMenu);
                    };
                    _game1.controlsMenuPanel.AddChild(backButton2);

                    var forwardKeyButton = new Button(_game1.forwardKey.ToString(), size: new Vector2(75, 75), anchor: Anchor.TopLeft, offset: new Vector2(0, 75 + 30));
                    forwardKeyButton.ButtonParagraph.Scale = 1.0f;
                    forwardKeyButton.ButtonParagraph.SetAnchor(Anchor.BottomCenter);
                    forwardKeyButton.ButtonParagraph.SetOffset(new Vector2(0, -27));

                    var forwardKeyButton2 = new Button(_game1.forwardKey2.ToString(), size: new Vector2(75, 75), anchor: Anchor.TopLeft, offset: new Vector2(75 + 30, 75 + 30));
                    forwardKeyButton2.ButtonParagraph.Scale = 1.0f;
                    forwardKeyButton2.ButtonParagraph.SetAnchor(Anchor.BottomCenter);
                    forwardKeyButton2.ButtonParagraph.SetOffset(new Vector2(0, -27));

                    var forwardKeyParagraph = new Paragraph(" - Forward", anchor: Anchor.TopLeft, offset: new Vector2(150 + 30, 75 + 30 + 5));
                    forwardKeyParagraph.Scale = 1.0f;
                    _game1.controlsMenuPanel.AddChild(forwardKeyParagraph);
                    _game1.controlsMenuPanel.AddChild(forwardKeyButton);
                    _game1.controlsMenuPanel.AddChild(forwardKeyButton2);

                    break;
                case GameState.SettingsMenu:
                    UserInterface.SetCursor(CursorType.Default);

                    _game1.settingsMenuPanel = new Panel(new Vector2(700, 500));
                    UserInterface.AddEntity(_game1.settingsMenuPanel);

                    var backButton = new Button("Back");
                    backButton.ButtonParagraph.Scale = 0.5f;
                    backButton.OnClick = btn =>
                    {
                        _game1.ChangeState(GameState.MainMenu);
                    };
                    _game1.settingsMenuPanel.AddChild(backButton);

                    var fullscreenCheckbox = new CheckBox("Fullscreen");
                    fullscreenCheckbox.TextParagraph.Scale = 0.5f;
                    fullscreenCheckbox.OnValueChange = box =>
                    {
                        _game1.graphics.IsFullScreen = fullscreenCheckbox.Checked;
                        _game1.graphics.ApplyChanges();
                    };
                    _game1.settingsMenuPanel.AddChild(fullscreenCheckbox);

                    var dropDown2 = new DropDown(new Vector2(0, 0));
                    dropDown2.SelectedTextPanelParagraph.Text = CurrentScreenWidth + "x" + CurrentScreenHeight;
                    dropDown2.AddItem("2560x1080");
                    dropDown2.AddItem("1920x1080");
                    dropDown2.AddItem("1680x1050");
                    dropDown2.AddItem("1440x900");
                    dropDown2.AddItem("1336x768");
                    dropDown2.AddItem("1280x800");
                    dropDown2.AddItem("1240x720");

                    dropDown2.OnValueChange = (Entity drop) =>
                    {
                        string selected = dropDown2.SelectedValue;
                        var index = selected.IndexOf('x');

                        var widthRes = selected.Substring(0, index);
                        var heightRes = selected.Substring(index + 1);

                        CurrentScreenWidth = Convert.ToInt16(widthRes);
                        CurrentScreenHeight = Convert.ToInt16(heightRes);
                        _game1.graphics.ApplyChanges();
                    };

                    _game1.settingsMenuPanel.AddChild(dropDown2);

                    break;
                case GameState.ShopMenu:
                    UserInterface.SetCursor(CursorType.Default);
                    _game1.shopPanel = new Panel(new Vector2(500, 300), anchor: Anchor.TopLeft, offset: new Vector2(0, 70));
                    UserInterface.AddEntity(_game1.shopPanel);
                    tabs = new PanelTabs();
                    tab0 = tabs.AddTab("Warp");
                    tab0.button.ButtonParagraph.Scale = 0.5f;
                    tab1 = tabs.AddTab("Shop");
                    tab1.button.ButtonParagraph.Scale = 0.5f;
                    tab2 = tabs.AddTab("Upgrades");
                    tab2.button.ButtonParagraph.Scale = 0.5f;
                    tab2.button.ButtonParagraph.WrapWords = false;

                    //first tab
                    warpButton = new Button("Warp to new sector");
                    warpButton.ButtonParagraph.Scale = 0.5f;
                    warpButton.OnClick = (Entity btn) =>
                    {
                        _game1.currentSector = _game1.GenerateSector();
                        _game1.ChangeState(GameState.MainGame);
                    };

                    tab0.panel.AddChild(warpButton);
                    tab0.panel.AddChild(new HorizontalLine());

                    var dropDown = new DropDown(new Vector2(0, 0));
                    dropDown.SelectedTextPanelParagraph.Scale = 0.5f;
                    dropDown.SelectList.ItemsScale = 0.5f;
                    dropDown.SelectedTextPanelParagraph.Text = "Warp to an old sector";
                    for (int i = 0; i < _game1.sectors.Count; i++)
                    {
                        dropDown.AddItem(_game1.sectors[i].Name);
                    }
                    dropDown.AttachedData = dropDown.SelectedValue;
                    dropDown.OnValueChange = (Entity drop) =>
                    {
                        _game1.currentSector = _game1.sectors.FirstOrDefault(x => x.Name == dropDown.SelectedValue);
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
                    for (int i = 0; i < _game1.availableShips.Count; i++)
                    {
                        var offset = new Vector2(0, (height + 20) * i);
                        var img = new Image(_game1.availableShips[i].texture, new Vector2(150, 100), anchor: Anchor.TopLeft);
                        img.SetOffset(offset);

                        _game1.shopDescriptions.Add(new Paragraph(_game1.availableShips[i].description));
                        img.Identifier = i.ToString();
                        img.OnMouseEnter = (Entity entity) =>
                        {
                            UserInterface.AddEntity(_game1.shopDescriptions[Convert.ToUInt16(entity.Identifier)]);
                        };
                        img.OnMouseLeave = (Entity entity) =>
                        {
                            UserInterface.RemoveEntity(_game1.shopDescriptions[Convert.ToUInt16(entity.Identifier)]);
                        };
                        tab1.panel.AddChild(img);

                        var name = new Paragraph(_game1.availableShips[i].Name, anchor: Anchor.TopRight);
                        name.Scale = 0.5f;
                        name.SetOffset(offset);
                        tab1.panel.AddChild(name);

                        var cost = new Paragraph("Cost: " + _game1.availableShips[i].cost, anchor: Anchor.CenterRight);
                        cost.Scale = 0.5f;
                        cost.SetOffset(offset - new Vector2(-25, 50));
                        tab1.panel.AddChild(cost);

                        var buttonlul = new Button("Buy", size: new Vector2(100, 50), anchor: Anchor.CenterRight);
                        buttonlul.ButtonParagraph.Scale = 0.5f;
                        _game1.shopButtons.Add(buttonlul);

                        _game1.shopButtons[i].SetOffset(offset + new Vector2(0, 0));
                        _game1.shopButtons[i].ButtonParagraph.SetOffset(new Vector2(0, -22));
                        tab1.panel.AddChild(_game1.shopButtons[i]);
                        tab1.panel.AddChild(new HorizontalLine());

                        _game1.shopButtons[i].Identifier = i.ToString();
                        _game1.shopButtons[i].OnClick = (Entity btn) =>
                        {
                            BuyShip(_game1.availableShips[Convert.ToUInt16(btn.Identifier)]);
                        };
                    }

                    tab1.panel.PanelOverflowBehavior = PanelOverflowBehavior.VerticalScroll;
                    tab1.panel.Scrollbar.Max = Convert.ToUInt16((int)((_game1.availableShips.Count * (height + 20)) - _game1.shopPanel.Size.Y));
                    tab1.panel.Scrollbar.StepsCount = (uint)_game1.availableShips.Count * 5;

                    //second tab end

                    _game1.shopPanel.AddChild(tabs);
                    break;
                case GameState.PauseMenu:
                    UserInterface.SetCursor(CursorType.Default);
                    //panel
                    _game1.pauseMenuPanel = new Panel(new Vector2(300, 500));
                    UserInterface.AddEntity(_game1.pauseMenuPanel);
                    //resumebutton
                    var resumeButton = new Button("Resume");
                    resumeButton.ButtonParagraph.Scale = 0.5f;
                    resumeButton.OnClick = (Entity btn) =>
                    {
                        _game1.ChangeState(GameState.MainGame);
                    };
                    _game1.pauseMenuPanel.AddChild(resumeButton);
                    //save button
                    var saveButton = new Button("Save game");
                    saveButton.ButtonParagraph.Scale = 0.5f;
                    saveButton.OnClick = (Entity btn) =>
                    {
                        SaveGame();
                    };
                    _game1.pauseMenuPanel.AddChild(saveButton);
                    //load button
                    var loadButton = new Button("Load game");
                    loadButton.ButtonParagraph.Scale = 0.5f;
                    loadButton.OnClick = (Entity btn) =>
                    {
                        LoadGame();
                    };
                    _game1.pauseMenuPanel.AddChild(loadButton);
                    //main menu button
                    var mainMenuButton = new Button("Main Menu");
                    mainMenuButton.ButtonParagraph.Scale = 0.5f;
                    mainMenuButton.OnClick = (Entity btn) =>
                    {
                        _game1.ChangeState(GameState.MainMenu);
                    };
                    _game1.pauseMenuPanel.AddChild(mainMenuButton);
                    //exit button
                    var exitButton = new Button("Exit");
                    exitButton.ButtonParagraph.Scale = 0.5f;
                    exitButton.OnClick = (Entity btn) =>
                    {
                        _game1.Exit();
                    };
                    _game1.pauseMenuPanel.AddChild(exitButton);

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(tempGameState), tempGameState, null);
            }
        }

        public void BuyShip(Ship ship)
        {
            if (_game1.ownedShips.All(x => x.Name != ship.Name)) _game1.ownedShips.Add(ship);

            var tempRot = _game1.currentShip.rotation;
            var tempPos = _game1.currentStationShip.Position;
            var tempIndex = _game1.currentShip.shipCurrentIndex;
            var tempHealth = _game1.currentShip.health;
            var tempEnergy = _game1.currentShip.energy;
            _game1.currentShip = ship;

            _game1.currentShip.shipCurrentIndex = tempIndex;

            _game1.currentShip.Position = tempPos;
            _game1.currentShip.rotation = tempRot;
            _game1.currentShip.health = tempHealth;
            _game1.currentShip.energy = tempEnergy;

            _game1.currentShip.Update();
        }

        private void SaveGame()
        {
            data.Score = _game1.score;
            data.Health = _game1.currentShip.health;

            data.DiscoveredSectors = _game1.sectors;
            data.OwnedShips = _game1.ownedShips;

            data.CurrentSectorName = _game1.currentSector.Name;
            data.CurrentShipName = _game1.currentShip.Name;

            List<Data> _data = new List<Data>();
            _data.Add(data);
            string json = JsonConvert.SerializeObject(_data.ToArray(), Formatting.Indented);

            File.WriteAllText("save.json", json);
        }

        public void LoadGame()
        {
            var text = File.ReadAllText("save.json");

            List<Data> _data = new List<Data>();
            _data = (JsonConvert.DeserializeObject<List<Data>>(text));

            data = _data[0];

            _game1.score = data.Score;
            _game1.currentShip.health = data.Health;
        }
    }
}