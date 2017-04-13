using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Game2Test.Sprites.Entities;

namespace Game2Test
{
    public class Station : Ship
    {
        public Station() { }

        public Station(Station station) :base(station)
        {
            
        }
        public Station(Dictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, List<Turret>> turrets, float healthMax, float energyMax, float energyRegen, float turnRate, float speed, TractorBeam tractorBeam, int upgradeCount) : base(textureDictionary, position, turrets, healthMax, energyMax, energyRegen, turnRate, speed, tractorBeam, upgradeCount)
        {
        }
    }
}
