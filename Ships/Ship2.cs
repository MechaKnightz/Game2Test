using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Game2Test.Ships
{
    public class Ship2 : Ship
    {
        public Ship2(Texture2D texture, Vector2 position, List<Turret> turrets) : base(texture, position, turrets)
        {
        }
    }
}
