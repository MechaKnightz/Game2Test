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
    public struct SaveGameData
    {
        List<Ship> OwnedShips { get; set; }
        List<Sector> DiscoveredSectors { get; set; }
        Ship CurrentShip { get; set; }
        float Score { get; set; }
        float Lives { get; set; }


    }
}