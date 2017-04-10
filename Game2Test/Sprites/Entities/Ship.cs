using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections.Specialized;
using System.Runtime.InteropServices.ComTypes;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Game2Test.Sprites;

namespace Game2Test
{
    public class Ship : Sprite
    {
        public Dictionary<string, List<Turret>> turrets = new Dictionary<string, List<Turret>>();
        [JsonIgnore]
        public Dictionary<string, Texture2D> textureDictionary = new Dictionary<string, Texture2D>();
        public string textureIndexCounter = "Default";
        public int shipCurrentIndex = 0;
        public int shipPreviousIndex = 0;

        public string description;
        public string Name { get; set; }
        public float cost;

        public float TurnRate { get; set; }
        public float Speed { get; set; }
        public float Boost { get; set; }
        public bool Moving { get; set; }

        public float health;
        public float healthMax;

        public float energy;
        public float energyMax;
        public float energyRegen;

        public Ship() { }

        public Ship(Ship ship)
        {
            turrets = ship.turrets;
            textureDictionary = ship.textureDictionary;
            Rotation = ship.Rotation;
            Position = ship.Position;
            Rectangle = ship.Rectangle;
            Origin = ship.Origin;
            TextureName = ship.TextureName;
            Texture = ship.Texture;
            textureIndexCounter = ship.textureIndexCounter;
            shipCurrentIndex = ship.shipCurrentIndex;
            shipPreviousIndex = ship.shipPreviousIndex;
            description = ship.description;
            Name = ship.Name;
            cost = ship.cost;
            TurnRate = ship.TurnRate;
            Speed = ship.Speed;
            Boost = ship.Boost;
            Moving = ship.Moving;
            health = ship.health;
            healthMax = ship.healthMax;
            energy = ship.energy;
            energyMax = ship.energyMax;
            energyRegen = ship.energyRegen;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param Name="textureDictionary">all ship textures</param>
        /// <param Name="position">position of the ship</param>
        /// <param Name="turrets">all the turrets on the ship</param>
        /// <param Name="healthMax">ship max health</param>
        /// <param Name="energyMax">ship max energy</param>
        /// /// <param Name="energyRegen">energy regeneration per frame</param>
        public Ship(Dictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, List<Turret>> turrets, float healthMax, float energyMax, float energyRegen, float turnRate) : base(textureDictionary["Default"], position)
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
            TurnRate = turnRate;
        }

        public void UpdateEnergy()
        {
            if (energy + energyRegen <= energyMax) energy += energyRegen;
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureDictionary[textureIndexCounter], Position, origin: Origin, rotation: Rotation);
            textureIndexCounter = "Default";
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

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
            foreach (var t in turrets)
            {
                //sets the turrets at the position of the ship
                foreach (var tur in t.Value)
                {
                    var temp = tur.Position;
                    temp.X = Position.X;
                    temp.Y = Position.Y;

                    //x turret offset
                    temp.X -= (float)(tur.offset.X * Math.Cos(Rotation - Math.PI));
                    temp.Y -= (float)(tur.offset.X * Math.Sin(Rotation - Math.PI));

                    //y turret offset
                    temp.X -= (float)(tur.offset.Y * Math.Cos(Rotation - (Math.PI / 2)));
                    temp.Y -= (float)(tur.offset.Y * Math.Sin(Rotation - (Math.PI / 2)));

                    tur.Position = temp;
                }
            }
        }
        public void SetPosition(Vector2 position)
        {
            Position = position;
            foreach (var t in turrets)
            {
                //sets the turrets at the position of the ship
                foreach (var tur in t.Value)
                {
                    var temp = tur.Position;
                    temp.X = Position.X;
                    temp.Y = Position.Y;

                    //x turret offset
                    temp.X -= (float)(tur.offset.X * Math.Cos(Rotation - Math.PI));
                    temp.Y -= (float)(tur.offset.X * Math.Sin(Rotation - Math.PI));

                    //y turret offset
                    temp.X -= (float)(tur.offset.Y * Math.Cos(Rotation - (Math.PI / 2)));
                    temp.Y -= (float)(tur.offset.Y * Math.Sin(Rotation - (Math.PI / 2)));

                    tur.Position = temp;
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
            position.X += (float)((Texture.Width / 2) * Math.Cos(Rotation - Math.PI));
            position.Y += (float)((Texture.Width / 2) * Math.Sin(Rotation - Math.PI));

            //y turret offset
            //position.X -= (float)(turrets[i].offset.Y * Math.Cos(Rotation - (Math.PI / 2)));
            //position.Y -= (float)(turrets[i].offset.Y * Math.Sin(Rotation - (Math.PI / 2)));

            return position;
        }
        /// <summary>
        /// same as above but with vector
        /// </summary>
        /// <param Name="vector">amount to moveDirection ship with</param>
        /// <param Name="Rotation">adjust turret position for Rotation</param>
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
        /// <param Name="nameOfGunGroup">the turrets' Name et.c primary0, primary1 where Name is primary</param>
        /// <param Name="nameOfShot">Name of the projectile you're firing</param>
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

        public void Turn(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    SetRotation(Rotation - TurnRate);
                    if (textureDictionary.ContainsKey(direction.ToString()))
                    {
                        textureIndexCounter = direction.ToString();
                    }
                    break;
                case Direction.Right:
                    SetRotation(Rotation + TurnRate);
                    if (textureDictionary.ContainsKey(direction.ToString()))
                    {
                        textureIndexCounter = direction.ToString();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }

        public void Move(MoveDirection moveDirection, bool speedBoost)
        {
            switch (moveDirection)
            {
                case MoveDirection.Forward:
                    if (!speedBoost)
                    {
                        var tempPos = Position;
                        tempPos.X += (float)Math.Cos(Rotation) * Speed;
                        tempPos.Y += (float)Math.Sin(Rotation) * Speed;
                        SetPosition(tempPos);
                    }
                    else if (speedBoost)
                    {
                        var tempPos = Position;
                        tempPos.X += (float)Math.Cos(Rotation) * Speed * Boost;
                        tempPos.Y += (float)Math.Sin(Rotation) * Speed * Boost;
                        SetPosition(tempPos);
                    }
                    break;
                case MoveDirection.Backward:
                    var tempPos2 = Position;
                    tempPos2.X -= (float)Math.Cos(Rotation) * (Speed / 5);
                    tempPos2.Y -= (float)Math.Sin(Rotation) * (Speed / 5);
                    SetPosition(tempPos2);
                    break;
            }
            Moving = true;
        }
    }
}