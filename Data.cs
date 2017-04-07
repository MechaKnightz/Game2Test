using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;

namespace Game2Test
{
    [Serializable]
    public struct Data
    {
        public List<Ship> OwnedShips { get; set; }
        public List<Sector> DiscoveredSectors { get; set; }
        public Sector CurrentSector { get; set; }
        public Ship CurrentShip { get; set; }
        public float Score { get; set; }
    }
}