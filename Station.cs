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
    public class Station : Ship
    {
        public Station(Dictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, List<Turret>> turrets, float healthMax, float energyMax, float energyRegen) : base(textureDictionary, position, turrets, healthMax, energyMax, energyRegen)
        {
        }
    }
}
