using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Game2Test.Ships;

namespace Game2Test
{
    public class Ship : Sprite
    {
        public List<Turret> turrets = new List<Turret>();
        public List<Texture2D> textures = new List<Texture2D>();
        public List<string> textureIndex = new List<string>();
        public int textureIndexCounter = 0;
        public int currentShipIndex = 0;
        public string description;

        public Ship(List<Texture2D> textures, Vector2 position, List<Turret> turrets, List<string> textureIndex) : base(textures[0], position)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                this.turrets.Add(turrets[i]);
            }
            for (int i = 0; i < textures.Count; i++)
            {
                this.textures.Add(textures[i]);
            }
            for (int i = 0; i < textureIndex.Count; i++)
            {
                this.textureIndex.Add(textureIndex[i]);
            }
        }
        public Ship(List<Texture2D> textures, Vector2 position, List<Turret> turrets) : base(textures[0], position)
        {
            for (int i = 0; i < turrets.Count; i++)
            {
                this.turrets.Add(turrets[i]);
            }
            for (int i = 0; i < textures.Count; i++)
            {
                this.textures.Add(textures[i]);
            }
        }
        new public void Update()
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            for (int i = 0; i < turrets.Count; i++)
            {
                //sets the turrets at the position of the ship
                turrets[i].position.X = position.X;
                turrets[i].position.Y = position.Y;

                //x turret offset
                turrets[i].position.X -= (float)(turrets[i].offset.X * Math.Cos(rotation - Math.PI));
                turrets[i].position.Y -= (float)(turrets[i].offset.X * Math.Sin(rotation - Math.PI));

                //y turret offset
                turrets[i].position.X -= (float)(turrets[i].offset.Y * Math.Cos(rotation - (Math.PI / 2)));
                turrets[i].position.Y -= (float)(turrets[i].offset.Y * Math.Sin(rotation - (Math.PI / 2)));
            }
        }
        /// <summary>
        /// returns vector at the back of the ship, based off of the back of the texture
        /// </summary>
        /// <returns></returns>
        public Vector2 GetBackOfShip()
        {
            Vector2 position;
            position.X = this.position.X;
            position.Y = this.position.Y;

            //x turret offset
            position.X += (float)((texture.Width / 2) * Math.Cos(rotation - Math.PI));
            position.Y += (float)((texture.Width / 2) * Math.Sin(rotation - Math.PI));

            //y turret offset
            //position.X -= (float)(turrets[i].offset.Y * Math.Cos(rotation - (Math.PI / 2)));
            //position.Y -= (float)(turrets[i].offset.Y * Math.Sin(rotation - (Math.PI / 2)));

            return position;
        }
        /// <summary>
        /// moves the ship and then updates
        /// </summary>
        /// <param name="x"> move along x-axis by this amount</param>
        /// <param name="y">move along y-axis by this amount</param>
        new public void SetPos(float x, float y)
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
        public new void SetPos(Vector2 vector)
        {
            position = vector;
            Update();
        }
        new public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[textureIndexCounter], position, origin: origin, rotation: rotation);
            textureIndexCounter = 0;
        }
    }
}