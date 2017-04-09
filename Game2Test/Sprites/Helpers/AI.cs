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

        public static void ShootAtShip(Ship ship, Ship targetShip)
        {
            if (!ship.Moving)
            {
                foreach (var turGroup in ship.turrets)
                {
                    foreach (var tur in turGroup.Value)
                    {
                        var angleToTargetShip = AngleToOther(ship.Position, targetShip.Position);

                        if (tur.rotation > angleToTargetShip) tur.Turn(Direction.Left);
                        if (tur.rotation < angleToTargetShip) tur.Turn(Direction.Right);

                        float diff = Math.Abs(MathHelper.WrapAngle(tur.rotation - angleToTargetShip));
                        if (diff < 0.05f) ship.Fire(turGroup.Key, "default"); //TODO fix default
                    }
                }
            }
        }
    }
}
