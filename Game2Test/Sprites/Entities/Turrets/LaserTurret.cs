using System;
using System.Collections.Generic;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Shapes;

namespace Game2Test.Sprites.Entities.Turrets
{
    class LaserTurret : BaseTurret, ITurret
    {
        private float Length { get; set; }
        private float BaseTurnrate { get; set; }
        private float Duration { get; set; }
        private float _durationCounter;
        private float Damage { get; set; }

        private List<Sprite> SpriteList { get; set; } = new List<Sprite>();
        private Sprite Start { get; set; }
        private Sprite Middle { get; set; }
        private Sprite End { get; set; }

        public LaserTurret() { }
        public LaserTurret(Texture2D texture, Sprite start, Sprite middle, Sprite end, Vector2 position, Vector2 offset, float rotation, float energyCost, float turnRate, TurretType type, float cooldown, float length, float duration, float damage) : base(texture, position, offset, rotation, energyCost, turnRate, type, cooldown)
        {
            Length = length;
            Cooldown = cooldown;
            CooldownCounter = cooldown;
            Start = start;
            Middle = middle;
            End = end;
            Start.Origin = new Vector2(0, start.Texture.Height / 2f);
            Middle.Origin = new Vector2(0, middle.Texture.Height / 2f);
            End.Origin = new Vector2(0, end.Texture.Height / 2f);
            BaseTurnrate = turnRate;
            Speed = float.MaxValue;
            Range = length;
            Duration = duration;
            Damage = damage;
        }
        public LaserTurret(LaserTurret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            Cooldown = turret.Cooldown + turret.Duration;
            Length = turret.Length;
            BaseTurnrate = turret.BaseTurnrate;
            Speed = turret.Speed;
            Range = turret.Range;
            Duration = turret.Duration;
            Start = turret.Start;
            Middle = turret.Middle;
            End = turret.End;
            Damage = turret.Damage;
            //Sprite
            Rotation = turret.Rotation;
            Position = turret.Position;
            Rectangle = turret.Rectangle;
            Origin = turret.Origin;
            Texture = turret.Texture;
            //End
            Start = new Sprite(turret.Start);
            Middle = new Sprite(turret.Middle);
            End = new Sprite(turret.End);
        }

        public ITurret CloneTurret(ITurret turret)
        {
            var tempTurret = new LaserTurret();
            tempTurret = (LaserTurret)turret;
            return tempTurret;
        }

        public float Update(float energy)
        {
            if (energy - EnergyCost <= 0)
            {
                SpriteList.Clear();
                IsFiring = false;
            }

            if (IsFiring)
            {
                UpdateShots();
                TurnRate = BaseTurnrate * 0.1f;
                return EnergyCost;
            }
            TurnRate = BaseTurnrate;
            if (CooldownCounter != Cooldown) CooldownCounter++;
            return 0f;
        }

        public void UpdateShots()
        {
            _durationCounter--;

            Start.Position = Position;
            Start.Rotation = Rotation;

            var tempPos = Middle.Position;
            tempPos.X = (float)Math.Cos(Rotation) * Start.Rectangle.Width + Position.X;
            tempPos.Y = (float)Math.Sin(Rotation) * Start.Rectangle.Width + Position.Y;
            Middle.Position = tempPos;
            Middle.Rotation = Rotation;

            var tempPos2 = End.Position;
            tempPos2.X = (float)Math.Cos(Rotation) * Middle.Rectangle.Width + Middle.Position.X;
            tempPos2.Y = (float)Math.Sin(Rotation) * Middle.Rectangle.Width + Middle.Position.Y;
            End.Position = tempPos2;
            End.Rotation = Rotation;

            if (_durationCounter > 00)
            {
                IsFiring = true;
                return;
            }
            SpriteList.Clear();
            IsFiring = false;
        }

        public float Fire()
        {
            if (CooldownCounter != Cooldown) return 0f;

            var middleLength = Length - Start.Texture.Width - End.Texture.Width;

            var tempRect = Start.Rectangle;
            tempRect.Width = Start.Texture.Width;
            tempRect.Height = Start.Texture.Height;
            Start.Rectangle = tempRect;

            var tempRect2 = Middle.Rectangle;
            tempRect2.Width = Convert.ToInt16(middleLength);
            Middle.Rectangle = tempRect2;

            var tempRect3 = End.Rectangle;
            tempRect3.Width = End.Texture.Width;
            tempRect3.Height = End.Texture.Height;
            End.Rectangle = tempRect3;

            Start.Position = Position;
            Start.Rotation = Rotation;

            var tempPos = Middle.Position;
            tempPos.X = (int) Math.Cos(Rotation) * Start.Texture.Width + Position.X;
            tempPos.Y = (int) Math.Sin(Rotation) * Start.Texture.Width + Position.Y;
            Middle.Position = tempPos;
            
            Middle.Rotation = Rotation;

            var tempPos2 = End.Position;
            tempPos.X = (int) Math.Cos(Rotation) * Middle.Rectangle.Width + Middle.Position.X;
            tempPos.Y = (int) Math.Sin(Rotation) * Middle.Rectangle.Width + Middle.Position.Y;
            End.Position = tempPos2;
            End.Rotation = Rotation;

            SpriteList.Add(Start);
            SpriteList.Add(Middle);
            SpriteList.Add(End);

            CooldownCounter = 0f;
            _durationCounter = Duration;
            IsFiring = true;
            TurnRate = BaseTurnrate * 0.1f;

            return EnergyCost;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);

            foreach (var sprite in SpriteList)
            {
                spriteBatch.Draw(sprite.Texture, destinationRectangle: sprite.Rectangle, rotation: sprite.Rotation, origin: sprite.Origin);
            }
        }

        public bool Collision(Rectangle rectangle, out float damage)
        {
            foreach (var laserSegment in SpriteList)
            {
                if (!laserSegment.RotatedRectangle.Intersects(rectangle)) continue;
                var tempRect = Middle.Rectangle;
                tempRect.Width = Convert.ToInt16(Vector2.Distance(Position, rectangle.Location.ToVector2()) - (rectangle.Width + rectangle.Height)/2f);
                Middle.Rectangle = tempRect;
                damage = Damage;
                return true;
            }
            var tempRect2 = Middle.Rectangle;
            tempRect2.Width = Convert.ToInt16(Length);
            Middle.Rectangle = tempRect2;
            damage = 0;
            return false;
        }
    }
}
