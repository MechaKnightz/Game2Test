using System;
using System.Collections.Generic;
using System.Linq;
using Game2Test.Sprites.Entities.Turrets;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Game2Test.Sprites.Entities
{
    public class Ship : Sprite, ITargetable
    {
        public Dictionary<string, List<ITurret>> Turrets { get; set; } = new Dictionary<string, List<ITurret>>();

        [JsonIgnore]
        public Dictionary<string, Texture2D> TextureDictionary { get; set; } = new Dictionary<string, Texture2D>();

        public TractorBeam TractorBeam { get; set; }
        public Upgrade[] Upgrades { get; set; }

        public bool Moving { get; private set; }
        public string TextureIndexCounter { get; set; } = "Default";

        public string Description { get; set; }
        public string Name { get; set; }

        public int ShipCurrentIndex { get; set; }
        public int ShipPreviousIndex { get; set; }
        public float Cost { get; set; }
        public float Money { get; set; }

        public float TurnRate { get; set; }

        public float Speed { get; set; }
        public float BackwardsSpeed { get; set; }
        public float StrafeSpeed { get; set; }

        public bool BoostBool { get; set; }
        public float Boost { get; set; }

        public float BaseHealth { get; set; }
        public float Health { get; set; }
        public float HealthMax { get; set; }

        public float BaseEnergy { get; set; }
        public float Energy { get; set; }
        public float EnergyMax { get; set; }

        public float BaseEnergyRegen { get; set; }
        public float EnergyRegen { get; set; }

        private int _beamDelayCounter;

        public Ship() { }

        public Ship(Ship ship)
        {
            TextureDictionary = ship.TextureDictionary;
            //Sprite
            Rotation = ship.Rotation;
            Position = ship.Position;
            Rectangle = ship.Rectangle;
            Origin = ship.Origin;
            Texture = ship.Texture;
            //End
            TextureIndexCounter = ship.TextureIndexCounter;
            ShipCurrentIndex = ship.ShipCurrentIndex;
            ShipPreviousIndex = ship.ShipPreviousIndex;
            Description = ship.Description;
            Name = ship.Name;
            Cost = ship.Cost;
            TurnRate = ship.TurnRate;
            Speed = ship.Speed;
            BackwardsSpeed = ship.BackwardsSpeed;
            StrafeSpeed = ship.StrafeSpeed;
            Boost = ship.Boost;
            Moving = ship.Moving;
            BaseHealth = ship.BaseHealth;
            Health = ship.Health;
            HealthMax = ship.HealthMax;
            BaseEnergy = ship.BaseEnergy;
            Energy = ship.Energy;
            EnergyMax = ship.EnergyMax;
            BaseEnergyRegen = ship.BaseEnergyRegen;
            EnergyRegen = ship.EnergyRegen;

            Upgrades = new Upgrade[ship.Upgrades.Length];
            for (int i = 0; i < ship.Upgrades.Length; i++)
            {
                if(ship.Upgrades[i] == null) continue;
                Upgrades[i] = new Upgrade(ship.Upgrades[i]);
            }

            if(ship.TractorBeam != null) TractorBeam = new TractorBeam(ship.TractorBeam);

            foreach (var turretGroup in ship.Turrets)
            {
                Turrets.Add(turretGroup.Key, new List<ITurret>());
            }
            foreach (var turretList in Turrets.Values)
            {
                foreach (var turretGroup in ship.Turrets)
                {
                    foreach (var turret in turretGroup.Value)
                    {
                        turretList.Add(turret.CloneTurret(turret)); //TODO see if this can be don easier instead of implementing CloneTurret in every turret
                    }
                }
            }
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
        public Ship(IReadOnlyDictionary<string, Texture2D> textureDictionary, Vector2 position, Dictionary<string, List<ITurret>> turrets, float healthMax, float energyMax, float energyRegen, float turnRate, float speed, TractorBeam tractorBeam, int upgradeCount) : base(textureDictionary["Default"], position)
        {
            foreach (var t in turrets)
            {
                Turrets.Add(t.Key, t.Value);
            }
            foreach (var entry in textureDictionary)
            {
                TextureDictionary.Add(entry.Key, entry.Value);
            }
            Upgrades = new Upgrade[upgradeCount];
            TractorBeam = tractorBeam;
            HealthMax = healthMax;
            BaseHealth = healthMax;
            Health = healthMax;
            BaseEnergy = energyMax;
            EnergyMax = energyMax;
            Energy = energyMax;
            BaseEnergyRegen = energyRegen;
            EnergyRegen = energyRegen;
            TurnRate = turnRate;
            Speed = speed;
            BackwardsSpeed = 1 / 5f * speed;
            StrafeSpeed = 3 / 5f * speed;
        }

        public void Update()
        {
            UpdateEnergy();
            UpdateTurrets();
            if (!Moving && _beamDelayCounter != TractorBeam.BeamDelay) _beamDelayCounter++;
            Moving = false;
            BoostBool = false;
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

        public void UpdateTractorBeam(Sector sector, Ship ship)
        {
            if (!Moving && _beamDelayCounter == TractorBeam.BeamDelay)
            {
                TractorBeam.Update(sector);
            }
        }

        public void PickUpCrystals(Sector sector, Ship ship)
        {
            for (int i = 0; i < sector.Asteroids.Count; i++)
            {
                if (sector.Asteroids[i].Destroyed)
                {
                    for (int j = 0; j < sector.Asteroids[i].Crystals.Count; j++)
                    {
                        if (ship.Rectangle.Intersects(sector.Asteroids[i].Crystals[j].Rectangle))
                        {
                            Money += sector.Asteroids[i].Crystals[j].Size;
                            sector.Asteroids[i].Crystals.RemoveAt(j);
                            _beamDelayCounter = 0;
                            j--;
                        }
                    }
                }
            }
        }

        public void DrawTractorBeam(SpriteBatch spriteBatch)
        {
            if (TractorBeam.DrawBeam) TractorBeam.Draw(spriteBatch);
        }

        public void AimTurrets(Vector2 target)
        {
            foreach (var turGroup in Turrets)
            {
                foreach (var tur in turGroup.Value)
                {
                    switch (tur.Type)
                    {
                        case TurretType.Rotating:
                            var angleToOther = Game1.AngleToOther(tur.Position, target);

                            if (angleToOther < tur.Rotation + tur.TurnRate && angleToOther > tur.Rotation - tur.TurnRate)
                            {
                                tur.Rotation = angleToOther;
                                continue;
                            }

                            var direction = WayToTurn(tur.Rotation, angleToOther); //TODO fix turning

                            tur.Turn(direction);
                            break;
                        case TurretType.Static:
                            tur.Rotation = Rotation;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public Upgrade ReplaceUpgrade(Upgrade upgrade, int index)
        {
            var tempUpgrade = Upgrades[index];
            Upgrades[index] = upgrade;
            return tempUpgrade;
        }

        public void AddUpgrade(Upgrade upgrade)
        {
            for (int i = 0; i < Upgrades.Length; i++)
            {
                if (Upgrades[i] == null)
                {
                    Upgrades[i] = upgrade;
                    ApplyUpgrades();
                    return;
                }
            }
            ApplyUpgrades();
        }

        public void ApplyUpgrades()
        {
            var percentageHealth = Health / HealthMax;
            var percentageEnergy = Energy / EnergyMax;

            HealthMax = BaseHealth;
            EnergyMax = BaseEnergy;
            EnergyRegen = BaseEnergyRegen;

            float healthMaxSum = 0;
            float energyMaxSum = 0;
            float energyRegenSum = 0;
            foreach (var upgrade in Upgrades)
            {
                if (upgrade == null) continue;
                healthMaxSum += upgrade.HealthBoost;
                energyMaxSum += upgrade.EnergyBoost;
                energyRegenSum += upgrade.EnergyRegenBoost;
            }
            HealthMax += healthMaxSum;
            EnergyMax += energyMaxSum;
            EnergyRegen += energyRegenSum;

            Health = percentageHealth * HealthMax;
            Energy = percentageEnergy * EnergyMax;
        }

        public Direction WayToTurn(float angle, float angleToOther)
        {
            angleToOther -= angle;

            while (true)
            {

                if (angleToOther > Game1.DoublePI) angleToOther -= Game1.DoublePI;
                else if (angleToOther < 0f) angleToOther += Game1.DoublePI;

                if (angleToOther > 0 && angleToOther < Game1.DoublePI) break;
            }

            if (angleToOther > Math.PI) return Direction.Left;
            if (angleToOther < Math.PI) return Direction.Right;

            return Direction.Left;
        }

        public void UpdateTurrets()
        {
            foreach (var t in Turrets)
            {
                foreach(var tur in t.Value)
                {
                    Energy -= tur.Update(Energy);
                }
            }
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;

            if (Rotation > Game1.DoublePI) Rotation -= Game1.DoublePI;
            else if (Rotation < 0) Rotation += Game1.DoublePI;

            TractorBeam.Position = Position;

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

        public void BuyTurrets()
        {
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
            TractorBeam.Position = Position;
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
        public bool TurretCollision(Rectangle rectangle, out Shot tempShot)
        {
            foreach (var t in Turrets)
            {
                foreach(var tur in t.Value)
                {
                    if (!tur.Collision(rectangle, out tempShot)) continue;
                    return true;
                }
            }
            tempShot = null;
            return false;
        }
        public List<BaseTurret> ShuffleTurrets(List<BaseTurret> turrets)
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

        public void Fire(string nameOfTurretGroup)
        {
            foreach (var t in Turrets)
            {
                if(t.Key == nameOfTurretGroup)
                {
                    foreach(var tur in t.Value)
                    {
                        //TODO fix energy cost
                        if(Energy - tur.EnergyCost > 0)
                        {
                            Energy -= tur.Fire();
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

        public void Move(MoveDirection moveDirection)
        {
            switch (moveDirection)
            {
                case MoveDirection.Forward:
                    if (BoostBool)
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
                    tempPos2.X += (float)Math.Cos(Rotation + Math.PI) * BackwardsSpeed;
                    tempPos2.Y += (float)Math.Sin(Rotation + Math.PI) * BackwardsSpeed;
                    SetPosition(tempPos2);
                    break;
                case MoveDirection.Left:
                    var tempPos3 = Position;
                    tempPos3.X += (float)Math.Cos(Rotation - Math.PI / 2) * BackwardsSpeed;
                    tempPos3.Y += (float)Math.Sin(Rotation - Math.PI / 2) * BackwardsSpeed;
                    SetPosition(tempPos3);
                    break;
                case MoveDirection.Right:
                    var tempPos4 = Position;
                    tempPos4.X += (float)Math.Cos(Rotation + Math.PI / 2) * BackwardsSpeed;
                    tempPos4.Y += (float)Math.Sin(Rotation + Math.PI / 2) * BackwardsSpeed;
                    SetPosition(tempPos4);
                    break;
            }
            Moving = true;
        }

        public void HitByShot(Shot shot)
        {
            Health -= shot.Damage;
        }
        public void HitByAsteroid(Asteroid asteroid)
        {
            Health -= asteroid.Size;
        }
    }
}