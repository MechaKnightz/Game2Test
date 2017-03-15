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
    public class Ship : Sprite
    {
        public Dictionary<string, Turret> turrets = new Dictionary<string, Turret>();
        public Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        public string textureIndexCounter = "default";
        public int shipCurrentIndex = 0;
        public int shipPreviousIndex = 0;
        public string description;

        public string health;
        public string healthMax;

        public string energy;
        public string energyMax;

        public Ship(List<Texture2D> textures, Vector2 position, Dictionary<string, Turret> turrets, List<string> textureIndex) : base(textures[0], position)
        {
            foreach (KeyValuePair<string, Turret> t in turrets)
            {
                this.turrets.Add(t.Key, t.Value);
            }
            for (var i = 0; i < textures.Count; i++)
            {
                textureDictionary.Add(textureIndex[i], textures[i]);
            }
        }
        public Ship(Dictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, Turret> turrets) : base(textureDictionary["default"], position)
        {
            foreach (var t in turrets)
            {
                this.turrets.Add(t.Key, t.Value);
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
                t.Value.position.X = position.X;
                t.Value.position.Y = position.Y;

                //x turret offset
                t.Value.position.X -= (float)(t.Value.offset.X * Math.Cos(rotation - Math.PI));
                t.Value.position.Y -= (float)(t.Value.offset.X * Math.Sin(rotation - Math.PI));

                //y turret offset
                t.Value.position.X -= (float)(t.Value.offset.Y * Math.Cos(rotation - (Math.PI / 2)));
                t.Value.position.Y -= (float)(t.Value.offset.Y * Math.Sin(rotation - (Math.PI / 2)));
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
                t.Value.Update();
            }
        }

        public void DrawTurrets(SpriteBatch spriteBatch)
        {
            foreach (var t in turrets)
            {
                t.Value.Draw(spriteBatch);
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
                if (t.Value.ShotCollision(rectangle)) return true;
            }
            return false;
        }
        public void FirePrimary()
        {
            foreach (var t in turrets)
            {
                for(int i = 0; i < turrets.Count; i++)
                {
                    if (t.Key == "primary" + i) t.Value.Fire();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOfGunGroup">the turrets' name et.c primary0, primary1 where name is primary</param>
        /// <param name="nameOfShot">name of the projectile you're firing</param>
        public void Fire(string nameOfGunGroup, string nameOfShot)
        {
            foreach (var t in turrets)
            {
                for (int i = 0; i < turrets.Count; i++)
                {
                    if (t.Key == nameOfGunGroup + i) t.Value.Fire(nameOfShot);
                }
            }
        }
    }
}