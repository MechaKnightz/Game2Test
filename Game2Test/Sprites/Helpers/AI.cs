using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game2Test.Sprites.Helpers
{
    public static class AI
    {
        public static void MoveTowardsGoal(Ship ship, Ship goal)
        {
            var angleToGoal = AngleToOther(ship.Position, goal.Position);

            float shortestRange = 9999999;
            foreach (var turGroup in ship.turrets)
            {
                foreach (var tur in turGroup.Value)
                {
                    if (tur.shots["default"].speed * tur.shots["default"].duration < shortestRange)
                        shortestRange = tur.shots["default"].speed * tur.shots["default"].duration;
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

        public static void ShootAtShip(Ship ship, Ship targetShip)
        {
            if (!ship.Moving)
            {
                foreach (var turGroup in ship.turrets)
                {
                    foreach (var tur in turGroup.Value)
                    {
                        var angleToTargetShip = AngleToOther(ship.Position, targetShip.Position);

                        if (tur.Rotation > angleToTargetShip) tur.Turn(Direction.Left);
                        if (tur.Rotation < angleToTargetShip) tur.Turn(Direction.Right);

                        float diff = Math.Abs(MathHelper.WrapAngle(tur.Rotation - angleToTargetShip));
                        if (diff < 0.05f) ship.Fire(turGroup.Key, "default"); //TODO fix default
                    }
                }
            }
        }
    }
}
