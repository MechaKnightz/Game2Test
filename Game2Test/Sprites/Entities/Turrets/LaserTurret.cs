using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    class LaserTurret : BaseTurret, ITurret
    {
        private float Length { get; set; }

        public LaserTurret() { }
        public LaserTurret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Shot shot, float energyCost, float turnRate, TurretType type, float cooldown, float length) : base(texture, position, offset, rotation, shot, energyCost, turnRate, type, cooldown)
        {
        }
        public LaserTurret(LaserTurret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            Cooldown = turret.Cooldown;
            Length = turret.Length;
            //Sprite
            Rotation = turret.Rotation;
            Position = turret.Position;
            Rectangle = turret.Rectangle;
            Origin = turret.Origin;
            Texture = turret.Texture;
            //End
            Shot = new Shot(turret.Shot);
        }
        public ITurret CloneTurret(ITurret turret)
        {
            var tempTurret = new LaserTurret();
            tempTurret = (LaserTurret)turret;
            return tempTurret;
        }
    }
}
