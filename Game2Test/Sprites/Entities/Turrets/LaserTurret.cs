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
        public ITurret CloneTurret(ITurret turret)
        {
            var tempTurret = (LaserTurret)turret;
            return tempTurret;
        }
    }
}
