using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace Game2Test.Ships
{
    public class Ship1 : Ship
    {
        public List<Turret> turrets = new List<Turret>();
        public Ship1(Texture2D texture, Vector2 position, Turret[] turrets) : base(texture, position)
        {
            for(int i = 0; i < turrets.Length; i++)
            {
                this.turrets.Add(turrets[i]);
            }
        }
        public void Update()
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            for(int i = 0; i < turrets.Count; i++)
            {
                turrets[i].position.X = position.X + (float)(turrets[i].offset.X * Math.Cos(rotation - Math.PI));
                turrets[i].position.Y = position.Y + (float)(turrets[i].offset.Y * Math.Sin(rotation - Math.PI));
            }
        }
        /// <summary>
        /// moves the ship and then updates
        /// </summary>
        /// <param name="x"> move along x-axis by this amount</param>
        /// <param name="y">move along y-axis by this amount</param>
        public void SetPos(float x, float y)
        {
            position.X = x;
            position.X = x;
            Update();
        }
        /// <summary>
        /// same as above but with vector
        /// </summary>
        /// <param name="vector">amount to move ship with</param>
        /// <param name="rotation">adjust turret position for rotation</param>
        public void SetPos(Vector2 vector)
        {
            position = vector;
            Update();
        }
    }
}
