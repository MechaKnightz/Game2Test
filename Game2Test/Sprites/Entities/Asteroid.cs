﻿using System;
using System.Collections.Generic;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace Game2Test.Sprites.Entities
{
    public class Asteroid : Sprite, ITargetable
    {
        public float Speed { get; set; }
        public float Acceleration { get; set; } = 1.1f;
        public float Health { get; set; }
        public float HealthMax { get; set; }

        public int Size { get; set; }
        public Bar Bar1 { get; set; }

        public bool Destroyed { get; set; } = false;

        public List<Crystal> Crystals { get; set; } = new List<Crystal>();

        public Asteroid(Texture2D texture, Vector2 position, float speed, float healthMax, Bar bar, int size, List<Crystal> crystals) :base(texture, position)
        {
            Speed = speed;
            HealthMax = healthMax;
            Health = healthMax;
            Bar1 = bar;
            Size = size;
            Crystals = crystals;
        }

        public Asteroid() { }

        public void Update(Vector2 position)
        {
            if (Destroyed)
            {
                for (var i = 0; i < Crystals.Count; i++)
                {
                    if (!Crystals[i].BeingBeamed)
                    {
                        Crystals[i].Duration--;
                        var temp = Crystals[i].Position;
                        temp.X += (float)Math.Cos(Crystals[i].Rotation) * Crystals[i].Speed;
                        temp.Y += (float)Math.Sin(Crystals[i].Rotation) * Crystals[i].Speed;
                        Crystals[i].Position = temp;

                        if (Crystals[i].Duration > 00) continue;
                        Crystals.RemoveAt(i);
                        i--;
                    }
                    else Crystals[i].BeingBeamed = false;
                }
            }
            else MoveTowardsPosition(position);
        }
        public void MoveTowardsPosition(Vector2 towardsPosition)
        {
            var angle = (float)Math.Atan2(towardsPosition.Y - Position.Y, towardsPosition.X - Position.X);

            var temp = Position;
            temp.X += (float)Math.Cos(angle) * Speed * Acceleration;
            temp.Y += (float)Math.Sin(angle) * Speed * Acceleration;
            Position = temp;

            Acceleration += 0.005f;

            var tempRect = Rectangle;
            tempRect.X = (int)Position.X;
            tempRect.Y = (int)Position.Y;
            Rectangle = tempRect;
        }

        public void Destroy()
        {
            Destroyed = true;
            foreach (var crystal in Crystals)
            {
                crystal.Position = Position;
                crystal.Rotation = (float)GetRandomNumber(0, Math.PI * 2);
            }

        }
        public new void Draw(SpriteBatch spriteBatch)
        {
            if (Destroyed)
            {
                foreach (var crystal in Crystals)
                {
                    crystal.Draw(spriteBatch);
                }
            }
            else
            {
                if (Health < HealthMax) Bar1.Draw(spriteBatch, Position, Health, HealthMax);
                spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);
            }
        }
        public double GetRandomNumber(double minimum, double maximum)
        {
            Random random = new Random();
            return random.NextDouble() * (maximum - minimum) + minimum;
        }

        public void HitByShot(Shot shot)
        {
            Health -= shot.Damage;
        }
        public void HitByAsteroid(Asteroid asteroid)
        {
            Health -= asteroid.Size;
        }
    }
}
