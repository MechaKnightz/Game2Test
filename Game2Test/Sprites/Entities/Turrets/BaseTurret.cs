using System;
using System.Collections.Generic;
using System.Linq;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    public class BaseTurret : Sprite
    {
        //Offset == turret position compared to ship position
        public Vector2 Offset { get; set; }
        public float EnergyCost { get; set; }
        public float TurnRate { get; set; }
        public TurretType Type { get; set; }
        public float Cooldown { get; set; }
        public float CooldownCounter { get; set; }
        public bool IsFiring { get; set; }
        public float Speed { get; set; }
        public float Range { get; set; }

        public BaseTurret() { }

        public BaseTurret(BaseTurret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            Cooldown = turret.Cooldown;
            //Sprite
            Rotation = turret.Rotation;
            Position = turret.Position;
            Rectangle = turret.Rectangle;
            Origin = turret.Origin;
            Texture = turret.Texture;
            //End
        }
        public BaseTurret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, float energyCost, float turnRate, TurretType type, float cooldown) : base(texture, position, rotation)
        {
            Offset = offset;
            EnergyCost = energyCost;
            TurnRate = turnRate;
            Type = type;
            Cooldown = cooldown;
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
            if (Rotation > Game1.DoublePI) Rotation -= Game1.DoublePI;
            else if (Rotation < 0) Rotation += Game1.DoublePI;
        }

        public void Turn(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    Rotation -= TurnRate;
                    break;
                case Direction.Right:
                    Rotation += TurnRate;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
}
