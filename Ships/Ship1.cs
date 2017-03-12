using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Game2Test.Ships
{
    class Ship1 : Ship
    {
        Turret[] turrets = new Turret[1];
        Ship1(Texture2D texture, Vector2 position, Turret turret) : base(texture, position)
        {

        }
    }
}
