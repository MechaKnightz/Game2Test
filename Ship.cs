using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using Game2Test.Ships;
using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;

namespace Game2Test
{
    public class Ship : Sprite
    {
        public List<Turret> turrets = new List<Turret>();
        public Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        public string textureIndexCounter = "default";
        public int currentShipIndex = 0;
        public int previousShipIndex = 0;
        public string description;

        public Ship(List<Texture2D> textures, Vector2 position, List<Turret> turrets, List<string> textureIndex) : base(textures[0], position)
        {
            foreach (var t in turrets)
            {
                this.turrets.Add(t);
            }
            for (var i = 0; i < textures.Count; i++)
            {
                textureDictionary.Add(textureIndex[i], textures[i]);
            }
        }
        public Ship(Dictionary<string, Texture2D> textureDictionary, Vector2 position, List<Turret> turrets) : base(textureDictionary["default"], position)
        {
            foreach (var t in turrets)
            {
                this.turrets.Add(t);
            }
            foreach (KeyValuePair<string, Texture2D> entry in textureDictionary)
            {
                this.textureDictionary.Add(entry.Key, entry.Value);
            }
        }
        public new void Update()
        {
            rectangle.X = (int)position.X;
            rectangle.Y = (int)position.Y;
            foreach (var t in turrets)
            {
//sets the turrets at the position of the ship
                t.position.X = position.X;
                t.position.Y = position.Y;

                //x turret offset
                t.position.X -= (float)(t.offset.X * Math.Cos(rotation - Math.PI));
                t.position.Y -= (float)(t.offset.X * Math.Sin(rotation - Math.PI));

                //y turret offset
                t.position.X -= (float)(t.offset.Y * Math.Cos(rotation - (Math.PI / 2)));
                t.position.Y -= (float)(t.offset.Y * Math.Sin(rotation - (Math.PI / 2)));
            }
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureDictionary[textureIndexCounter], position, origin: origin, rotation: rotation);
            textureIndexCounter = "default";
        }

        public void UpdateTurrets()
        {
            foreach (var t in turrets)
            {
                t.Update();
            }
        }

        public void DrawTurrets(SpriteBatch spriteBatch)
        {
            foreach (var t in turrets)
            {
                t.Draw(spriteBatch);
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
        public new void SetPos(float x, float y)
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
        public bool TurretCollision(Rectangle rectangle)
        {
            foreach (var t in turrets)
            {
                if (t.ShotCollision(rectangle)) return true;
            }
            return false;
        }
        public void FireAll()
        {
            foreach (var t in turrets)
            {
                t.Fire();
            }
        }
    }
}