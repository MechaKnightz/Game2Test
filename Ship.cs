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
        public Dictionary<string, List<Turret>> turrets = new Dictionary<string, List<Turret>>();
        public Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        public string textureIndexCounter = "default";
        public int shipCurrentIndex = 0;
        public int shipPreviousIndex = 0;

        public string description;
        public string name;
        public float cost;

        public float health;
        public float healthMax;

        public float energy;
        public float energyMax;
        public float energyRegen;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureDictionary">all ship textures</param>
        /// <param name="position">position of the ship</param>
        /// <param name="turrets">all the turrets on the ship</param>
        /// <param name="healthMax">ship max health</param>
        /// <param name="energyMax">ship max energy</param>
        /// /// <param name="energyRegen">energy regeneration per frame</param>
        public Ship(Dictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, List<Turret>> turrets, float healthMax, float energyMax, float energyRegen) : base(textureDictionary["default"], position)
        {
            foreach (var t in turrets)
            {
                this.turrets.Add(t.Key, t.Value);
            }
            foreach (KeyValuePair<string, Texture2D> entry in textureDictionary)
            {
                this.textureDictionary.Add(entry.Key, entry.Value);
            }
            this.healthMax = healthMax;
            this.health = healthMax;
            this.energyMax = energyMax;
            this.energy = energyMax;
            this.energyRegen = energyRegen;
        }

        public new void Update()
        {
            rectangle.X = (int)Position.X;
            rectangle.Y = (int)Position.Y;
            foreach (var t in turrets)
            {
                //sets the turrets at the position of the ship
                foreach(var tur in t.Value)
                {
                    var temp = tur.Position;
                    temp.X = Position.X;
                    temp.Y = Position.Y;

                    //x turret offset
                    temp.X -= (float)(tur.offset.X * Math.Cos(rotation - Math.PI));
                    temp.Y -= (float)(tur.offset.X * Math.Sin(rotation - Math.PI));

                    //y turret offset
                    temp.X -= (float)(tur.offset.Y * Math.Cos(rotation - (Math.PI / 2)));
                    temp.Y -= (float)(tur.offset.Y * Math.Sin(rotation - (Math.PI / 2)));
                    tur.Position = temp;
                }
            }
        }

        public void UpdateEnergy()
        {
            if (energy + energyRegen <= energyMax) energy += energyRegen;
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureDictionary[textureIndexCounter], Position, origin: origin, rotation: rotation);
            textureIndexCounter = "default";
            DrawTurrets(spriteBatch);
        }

        public void UpdateTurrets()
        {
            foreach (var t in turrets)
            {
                foreach(var tur in t.Value)
                {
                    tur.Update();
                }
            }
        }

        public void DrawTurrets(SpriteBatch spriteBatch)
        {
            foreach (var t in turrets)
            {
                foreach (var tur in t.Value)
                {
                    tur.Draw(spriteBatch);
                }
            }
        }
        /// <summary>
        /// returns vector at the back of the ship, based off of the back of the texture
        /// </summary>
        /// <returns></returns>
        public Vector2 GetBackOfShip()
        {
            Vector2 position;
            position.X = this.Position.X;
            position.Y = this.Position.Y;

            //x turret offset
            position.X += (float)((texture.Width / 2) * Math.Cos(rotation - Math.PI));
            position.Y += (float)((texture.Width / 2) * Math.Sin(rotation - Math.PI));

            //y turret offset
            //position.X -= (float)(turrets[i].offset.Y * Math.Cos(rotation - (Math.PI / 2)));
            //position.Y -= (float)(turrets[i].offset.Y * Math.Sin(rotation - (Math.PI / 2)));

            return position;
        }
        /// <summary>
        /// same as above but with vector
        /// </summary>
        /// <param name="vector">amount to move ship with</param>
        /// <param name="rotation">adjust turret position for rotation</param>
        public new void SetPos(Vector2 vector)
        {
            Position = vector;
            Update();
        }
        public bool TurretCollision(Rectangle rectangle, out Turret turret, out Shot tempShot)
        {
            foreach (var t in turrets)
            {
                foreach(var tur in t.Value)
                {
                    if (tur.ShotCollision(rectangle, out tempShot))
                    {
                        turret = tur;
                        return true;
                    }
                }
            }
            turret = null;
            tempShot = null;
            return false;
        }
        public List<Turret> ShuffleTurrets(List<Turret> turrets)
        {
            var tempTurret = turrets[0];
            for (int i = 0; i < turrets.Count; i++)
            {
                if (i == 0) continue;
                turrets[i - 1] = turrets[i];
            }
            turrets[turrets.Count - 1] = tempTurret;
            return turrets;
        } //TODO: make dictionary with list of turrets, each list for each group so that you can shuffle them easier

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameOfGunGroup">the turrets' name et.c primary0, primary1 where name is primary</param>
        /// <param name="nameOfShot">name of the projectile you're firing</param>
        public void Fire(string nameOfGunGroup, string nameOfShot)
        {
            foreach (var t in turrets)
            {
                if(t.Key == nameOfGunGroup)
                {
                    foreach(var tur in t.Value)
                    {
                        //TODO fix energy cost
                        if(energy - tur.energyCost > 0)
                        {
                            tur.Fire();
                            energy -= tur.energyCost;
                        }
                    }
                }
            }
        }
    }
}