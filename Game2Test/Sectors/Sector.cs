using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game2Test.Sprites.Entities;

namespace Game2Test
{
    public class Sector
    {
        public string Name;
        public Station SectorStation;

        public List<Ship> NPCShips = new List<Ship>();
        public Ship CurrentShip = new Ship();
        public Station CurrentStation = new Station();

        [JsonIgnore]
        public List<Texture2D> Backgrounds = new List<Texture2D>();
        public List<Asteroid> Asteroids = new List<Asteroid>();
        public void Update()
        {
            foreach (var ship in NPCShips)
            {
                ship.UpdateTurrets();
                ship.UpdateEnergy();
                ship.Moving = false;
            }
            CurrentShip.UpdateTurrets();
            CurrentShip.UpdateEnergy();
            CurrentShip.Moving = false;
        }
    }
}
