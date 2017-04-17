using System;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    class LaserTurret : BaseTurret, ITurret
    {
        private float Length { get; set; }
        private float BaseTurnrate { get; set; }

        public LaserTurret() { }
        public LaserTurret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Shot shot, float energyCost, float turnRate, TurretType type, float cooldown, float length) : base(texture, position, offset, rotation, shot, energyCost, turnRate, type, cooldown)
        {
            Length = length;
            Cooldown = cooldown;
            CooldownCounter = cooldown;
            Shot.Origin = new Vector2(0, shot.Texture.Height / 2f);
            BaseTurnrate = turnRate;
            Speed = float.MaxValue;
            Range = length;
        }
        public LaserTurret(LaserTurret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            Cooldown = turret.Cooldown + turret.Shot.Duration;
            Length = turret.Length;
            BaseTurnrate = turret.BaseTurnrate;
            Speed = turret.Speed;
            Range = turret.Range;
            //Sprite
            Rotation = turret.Rotation;
            Position = turret.Position;
            Rectangle = turret.Rectangle;
            Origin = turret.Origin;
            Texture = turret.Texture;
            //End
            Shot = new Shot(turret.Shot);
            var tempRect = Shot.Rectangle;
            tempRect.Y = turret.Shot.Texture.Height;
            Shot.Rectangle = tempRect;
            Shot.Origin = new Vector2(0, Shot.Texture.Height / 2f);
        }

        public ITurret CloneTurret(ITurret turret)
        {
            var tempTurret = new LaserTurret();
            tempTurret = (LaserTurret)turret;
            return tempTurret;
        }

        public float Update(float energy)
        {
            UpdateShots();
            TurnRate = BaseTurnrate;

            if (energy - EnergyCost <= 0)
            {
                for (int i = 0; i < ShotList.Count; i++)
                {
                    ShotList.RemoveAt(i);
                    IsFiring = false;
                }
            }

            if (IsFiring)
            {
                TurnRate = BaseTurnrate * 0.1f;
                return EnergyCost;
            }
            if (CooldownCounter != Cooldown) CooldownCounter++;
            return 0f;
        }

        public new void UpdateShots()
        {
            for (int i = 0; i < ShotList.Count; i++)
            {
                ShotList[i].Duration--;
                ShotList[i].Position = Position;
                ShotList[i].Rotation = Rotation;

                if (ShotList[i].Duration > 00)
                {
                    IsFiring = true;
                    continue;
                }
                ShotList.RemoveAt(i);
                IsFiring = false;
            }
        }

        public new float Fire()
        {
            if (CooldownCounter != Cooldown) return 0f;

            var tempRect = Shot.Rectangle;
            tempRect.Width = Convert.ToInt16(Length);
            Shot.Rectangle = tempRect;

            ShotList.Add(new Shot(Shot.Texture, Position, Rotation, Shot.Duration, Shot.Damage, Shot.Rectangle, Shot.Origin));
            CooldownCounter = 0f;
            IsFiring = true;
            TurnRate = BaseTurnrate * 0.1f;

            return EnergyCost;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);
            foreach (var shot in ShotList)
            {
                spriteBatch.Draw(shot.Texture, destinationRectangle: shot.Rectangle, rotation: Rotation, origin: shot.Origin);
            }
        }
    }
}
