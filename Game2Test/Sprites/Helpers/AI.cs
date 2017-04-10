using System;
using Game2Test.Sprites.Entities;
using Microsoft.Xna.Framework;

namespace Game2Test.Sprites.Helpers
{
    public static class AI
    {
        public static void MoveTowardsGoal(Ship ship, Ship goal)
        {
            var angleToGoal = AngleToOther(ship.Position, goal.Position);

            float shortestRange = 9999999;
            foreach (var turGroup in ship.Turrets)
            {
                foreach (var tur in turGroup.Value)
                {
                    if (tur.Shots["default"].Speed * tur.Shots["default"].Duration < shortestRange)
                        shortestRange = tur.Shots["default"].Speed * tur.Shots["default"].Duration;
                }
            }
            var distanceToGoal = Vector2.Distance(ship.Position, goal.Position);
            if (distanceToGoal > shortestRange) //change 500 to range of the shortest weapons
            {
                float diff = Math.Abs(MathHelper.WrapAngle(ship.Rotation - angleToGoal));
                if (diff < 0.2) ship.Move(MoveDirection.Forward, false);
                else
                {
                    if (ship.Rotation > angleToGoal)
                    {
                        if (ship.Rotation > angleToGoal) ship.Turn(Direction.Left);
                    }
                    else
                    {
                        if (ship.Rotation < angleToGoal) ship.Turn(Direction.Right);
                    }
                }
            }
            if(distanceToGoal < shortestRange+10) ShootAtShip(ship, goal);
        }
        public static float AngleToOther(Vector2 main, Vector2 other)
        {
            return (float)Math.Atan2(other.Y - main.Y, other.X - main.X);
        }

        public static void ShootIfInAim(Ship ship, Vector2 target)
        {
            foreach (var turGroup in ship.Turrets)
            {
                foreach (var tur in turGroup.Value)
                {
                    var angleToTargetShip = Game1.AngleToOther(tur.Position, target);
                    float diff = Math.Abs(MathHelper.WrapAngle(tur.Rotation - angleToTargetShip));
                    if (diff < 0.05f) ship.Fire(turGroup.Key, "default"); //TODO fix default
                }
            }
        }

        public static void ShootAtShip(Ship ship, Ship targetShip)
        {
            if (!targetShip.Moving)
            {
                ship.AimTurrets(targetShip.Position);
                ShootIfInAim(ship, targetShip.Position);
            }
            else if (targetShip.Moving)
            {
                
            }
        }
        public static void ShootAtAsteroid(Station station, Asteroid asteroid)
        {

        }
    }
}
