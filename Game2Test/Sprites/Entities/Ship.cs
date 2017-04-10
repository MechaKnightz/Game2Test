﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Game2Test.Sprites.Entities
{
    public class Ship : Sprite
    {
        public Dictionary<string, List<Turret>> Turrets { get; set; } = new Dictionary<string, List<Turret>>();

        [JsonIgnore]
        public Dictionary<string, Texture2D> TextureDictionary { get; set; } = new Dictionary<string, Texture2D>();

        public bool Moving { get; set; }
        public string TextureIndexCounter { get; set; } = "Default";

        public string Description { get; set; }
        public string Name { get; set; }

        public int ShipCurrentIndex { get; set; }
        public int ShipPreviousIndex { get; set; }
        public float Cost { get; set; }

        public float TurnRate { get; set; }
        public float Speed { get; set; }
        public float Boost { get; set; }

        public float Health { get; set; }
        public float HealthMax { get; }

        public float Energy { get; set; }
        public float EnergyMax { get; }
        public float EnergyRegen { get; }

        public Ship() { }

        public Ship(Ship ship)
        {
            Turrets = ship.Turrets;
            TextureDictionary = ship.TextureDictionary;
            Rotation = ship.Rotation;
            Position = ship.Position;
            Rectangle = ship.Rectangle;
            Origin = ship.Origin;
            TextureName = ship.TextureName;
            Texture = ship.Texture;
            TextureIndexCounter = ship.TextureIndexCounter;
            ShipCurrentIndex = ship.ShipCurrentIndex;
            ShipPreviousIndex = ship.ShipPreviousIndex;
            Description = ship.Description;
            Name = ship.Name;
            Cost = ship.Cost;
            TurnRate = ship.TurnRate;
            Speed = ship.Speed;
            Boost = ship.Boost;
            Moving = ship.Moving;
            Health = ship.Health;
            HealthMax = ship.HealthMax;
            Energy = ship.Energy;
            EnergyMax = ship.EnergyMax;
            EnergyRegen = ship.EnergyRegen;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="textureDictionary">all the different textures the ship uses, most used are Default, Left and Right</param>
        /// <param name="position">position of the ship</param>
        /// <param name="turrets">turrets on the ship</param>
        /// <param name="healthMax">max health</param>
        /// <param name="energyMax">max energy</param>
        /// <param name="energyRegen">energy regen per frame</param>
        /// <param name="turnRate">how fast the ship turns</param>
        public Ship(IReadOnlyDictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, List<Turret>> turrets, float healthMax, float energyMax, float energyRegen, float turnRate) : base(textureDictionary["Default"], position)
        {
            foreach (var t in turrets)
            {
                Turrets.Add(t.Key, t.Value);
            }
            foreach (var entry in textureDictionary)
            {
                TextureDictionary.Add(entry.Key, entry.Value);
            }
            HealthMax = healthMax;
            Health = healthMax;
            EnergyMax = energyMax;
            Energy = energyMax;
            EnergyRegen = energyRegen;
            TurnRate = turnRate;
        }

        public void Update()
        {
            UpdateEnergy();
            UpdateTurrets();
            Moving = false;
        }

        public void UpdateEnergy()
        {
            if (Energy + EnergyRegen <= EnergyMax) Energy += EnergyRegen;
        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureDictionary[TextureIndexCounter], Position, origin: Origin, rotation: Rotation);
            TextureIndexCounter = "Default";
            DrawTurrets(spriteBatch);
        }

        public void UpdateTurrets()
        {
            foreach (var t in Turrets)
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
            foreach (var t in Turrets)
            {
                //sets the turrets at the position of the ship
                foreach (var tur in t.Value)
                {
                    var temp = tur.Position;
                    temp.X = Position.X;
                    temp.Y = Position.Y;

                    //x turret offset
                    temp.X -= (float)(tur.Offset.X * Math.Cos(Rotation - Math.PI));
                    temp.Y -= (float)(tur.Offset.X * Math.Sin(Rotation - Math.PI));

                    //y turret offset
                    temp.X -= (float)(tur.Offset.Y * Math.Cos(Rotation - (Math.PI / 2)));
                    temp.Y -= (float)(tur.Offset.Y * Math.Sin(Rotation - (Math.PI / 2)));

                    tur.Position = temp;
                }
            }
        }
        public void SetPosition(Vector2 position)
        {
            Position = position;
            foreach (var t in Turrets)
            {
                //sets the turrets at the position of the ship
                foreach (var tur in t.Value)
                {
                    var temp = tur.Position;
                    temp.X = Position.X;
                    temp.Y = Position.Y;

                    //x turret offset
                    temp.X -= (float)(tur.Offset.X * Math.Cos(Rotation - Math.PI));
                    temp.Y -= (float)(tur.Offset.X * Math.Sin(Rotation - Math.PI));

                    //y turret offset
                    temp.X -= (float)(tur.Offset.Y * Math.Cos(Rotation - (Math.PI / 2)));
                    temp.Y -= (float)(tur.Offset.Y * Math.Sin(Rotation - (Math.PI / 2)));

                    tur.Position = temp;
                }
            }
        }

        public void DrawTurrets(SpriteBatch spriteBatch)
        {
            foreach (var t in Turrets)
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
            position.X = Position.X;
            position.Y = Position.Y;

            //x turret offset
            position.X += (float)(Texture.Width / 2f * Math.Cos(Rotation - Math.PI));
            position.Y += (float)(Texture.Width / 2f * Math.Sin(Rotation - Math.PI));

            //y turret offset
            //position.X -= (float)(turrets[i].offset.Y * Math.Cos(Rotation - (Math.PI / 2)));
            //position.Y -= (float)(turrets[i].offset.Y * Math.Sin(Rotation - (Math.PI / 2)));

            return position;
        }
        public bool TurretCollision(Rectangle rectangle, out Turret turret, out Shot tempShot)
        {
            foreach (var t in Turrets)
            {
                foreach(var tur in t.Value)
                {
                    if (!tur.ShotCollision(rectangle, out tempShot)) continue;
                    turret = tur;
                    return true;
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

        public void Fire(string nameOfGunGroup, string nameOfShot)
        {
            foreach (var t in Turrets)
            {
                if(t.Key == nameOfGunGroup)
                {
                    foreach(var tur in t.Value)
                    {
                        //TODO fix energy cost
                        if(Energy - tur.EnergyCost > 0)
                        {
                            tur.Fire();
                            Energy -= tur.EnergyCost;
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
                    if (TextureDictionary.ContainsKey(direction.ToString()))
                    {
                        TextureIndexCounter = direction.ToString();
                    }
                    break;
                case Direction.Right:
                    SetRotation(Rotation + TurnRate);
                    if (TextureDictionary.ContainsKey(direction.ToString()))
                    {
                        TextureIndexCounter = direction.ToString();
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
                    if (speedBoost)
                    {
                        var tempPos = Position;
                        tempPos.X += (float)Math.Cos(Rotation) * Speed * Boost;
                        tempPos.Y += (float)Math.Sin(Rotation) * Speed * Boost;
                        SetPosition(tempPos);
                    }
                    else 
                    {
                        var tempPos = Position;
                        tempPos.X += (float)Math.Cos(Rotation) * Speed;
                        tempPos.Y += (float)Math.Sin(Rotation) * Speed;
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