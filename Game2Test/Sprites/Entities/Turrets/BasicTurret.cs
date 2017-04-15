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

        public ITurret CloneTurret(ITurret turret)
        {
            var tempTurret = new BasicTurret();

            tempTurret.Offset = turret.Offset;
            tempTurret.EnergyCost = turret.EnergyCost;
            tempTurret.TurnRate = turret.TurnRate;
            tempTurret.Type = turret.Type;
            tempTurret.Cooldown = turret.Cooldown;
            //Sprite
            tempTurret.Rotation = turret.Rotation;
            tempTurret.Position = turret.Position;
            tempTurret.Rectangle = turret.Rectangle;
            tempTurret.Origin = turret.Origin;
            tempTurret.Texture = turret.Texture;
            //End
            tempTurret.Shot = new Shot(turret.Shot);

            return tempTurret;
        }
    }
}
