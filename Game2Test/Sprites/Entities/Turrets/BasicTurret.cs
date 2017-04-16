using System;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    public class BasicTurret : BaseTurret, ITurret
    {
        public BasicTurret() { }
        public BasicTurret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Shot shot, float energyCost, float turnRate, TurretType type, float cooldown) : base(texture, position, offset, rotation, shot, energyCost, turnRate, type, cooldown)
        {
        }
        public BasicTurret(BaseTurret turret)
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
            Shot = new Shot(turret.Shot);
        }
        public ITurret CloneTurret(ITurret turret)
        {
            var tempTurret = new BasicTurret((BasicTurret)turret);
            return tempTurret;
        }
    }
}
