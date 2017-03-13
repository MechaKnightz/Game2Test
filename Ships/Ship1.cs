using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Game2Test.Ships
{
    public class Ship1 : Ship
    {
        public Ship1(Texture2D texture, Vector2 position, List<Turret> turrets, string description) : base(texture, position, turrets, description)
        {
        }
    }
}
