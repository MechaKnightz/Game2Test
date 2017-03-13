using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Game2Test.Ships
{
    public class Ship1 : Ship
    {
        public Ship1(List<Texture2D> textures, Vector2 position, List<Turret> turrets) : base(textures, position, turrets)
        {
        }

    }
}
