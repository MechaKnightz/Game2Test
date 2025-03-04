﻿using System;
using Game2Test.Sprites.Entities;
using Microsoft.Xna.Framework;
using static ClassLibary.Angle;

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
                    if (tur.Range < shortestRange)
                        shortestRange = tur.Range;
                }
            }
            var distanceToGoal = Vector2.Distance(ship.Position, goal.Position);
            if (distanceToGoal > shortestRange) //change 500 to range of the shortest weapons
            {
                var diff = Math.Abs(MathHelper.WrapAngle(ship.Rotation - angleToGoal));
                if (diff < 0.2) ship.Move(MoveDirection.Forward);
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

        public static void ShootIfInAim(Ship ship, Vector2 target)
        {
            foreach (var turGroup in ship.Turrets)
            {
                foreach (var tur in turGroup.Value)
                {
                    var angleToTargetShip = AngleToOther(tur.Position, target);
                    float diff = Math.Abs(MathHelper.WrapAngle(tur.Rotation - angleToTargetShip));
                    if (diff < 0.05f) ship.Fire(turGroup.Key); //TODO fix default
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
                //TODO
            }
        }
        public static void ShootAtAsteroid(Station station, Asteroid asteroid)
        {
            //TODO
        }
    }
}
