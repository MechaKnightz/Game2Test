﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2Test
{
    public class Sector
    {
        public string Name;
        public Station SectorStation;

        private List<Ship> NPCShips = new List<Ship>();
        private Ship MainShip = new Ship();

        [JsonIgnore]
        public List<Texture2D> Backgrounds = new List<Texture2D>();
        public List<Asteroid> Asteroids = new List<Asteroid>();
        public void Update()
        {
            foreach (var ship in NPCShips)
            {
                ship.Moving = false;
            }
            MainShip.Moving = false;
        }
    }
}
