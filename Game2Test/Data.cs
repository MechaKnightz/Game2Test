using System.Collections.Generic;
using Game2Test.Sprites.Entities;

namespace Game2Test
{
    public struct Data
    {
        public int Score { get; set; }
        public float Health { get; set; }

        public List<Sector> DiscoveredSectors { get; set; }
        public List<Ship> OwnedShips { get; set; }

        public string CurrentShipName { get; set; }
        public string CurrentSectorName { get; set; }
    }
}