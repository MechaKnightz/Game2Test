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
        public static void MoveTowardsGoal(Ship ship, Vector2 goal)
        {
            var angleToGoal = AngleToOther(ship.Position, goal);
            if (Vector2.Distance(ship.Position, goal) > 500) //change 500 to range of weapons
            {

                if (ship.rotation > angleToGoal) ship.Turn(Direction.Left);
                if (ship.rotation < angleToGoal) ship.Turn(Direction.Right);


                float diff = Math.Abs(MathHelper.WrapAngle(ship.rotation - angleToGoal));
                if (diff < 0.2) ship.Move(MoveDirection.Forward, false);
            }
        }
        public static float AngleToOther(Vector2 main, Vector2 other)
        {
            return (float)Math.Atan2(other.Y - main.Y, other.X - main.X);
        }

        public static bool AimAtShip(Ship ship, Ship targetShip)
        {
            var angleToTargetShip = AngleToOther(ship.Position, targetShip.Position);
            if (!ship.Moving)
            {
                if (ship.rotation > angleToTargetShip) ship.Turn(Direction.Left);
                if (ship.rotation < angleToTargetShip) ship.Turn(Direction.Right);
            }
        }

        public static void ShootAtShip(Ship ship,Ship targetShip)
        {
            bool shoot = AimAtShip();
        }
    }
}
