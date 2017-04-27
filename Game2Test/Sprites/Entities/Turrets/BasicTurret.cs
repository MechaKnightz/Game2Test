using System;
using System.Collections.Generic;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    public class BasicTurret : BaseTurret, ITurret
    {
        public List<Shot> ShotList { get; set; } = new List<Shot>();
        public Shot Shot { get; set; } = new Shot();

        public BasicTurret() { }
        public BasicTurret(Texture2D texture, Vector2 position, Vector2 offset, float rotation, Shot shot, float energyCost, float turnRate, TurretType type, float cooldown) : base(texture, position, offset, rotation, energyCost, turnRate, type, cooldown)
        {
            Range = shot.Duration * shot.Speed;
            Speed = shot.Speed;
            Shot = shot;
        }
        public BasicTurret(BasicTurret turret)
        {
            Offset = turret.Offset;
            EnergyCost = turret.EnergyCost;
            TurnRate = turret.TurnRate;
            Type = turret.Type;
            Cooldown = turret.Cooldown;
            Speed = turret.Speed;
            Range = turret.Range;
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

        public float Fire()
        {
            if (CooldownCounter != Cooldown) return 0f;

            ShotList.Add(new Shot(Shot.Texture, Position, Rotation, Shot.Duration, Shot.Speed, Shot.Damage));
            CooldownCounter = 0f;

            return EnergyCost;
        }
        public void UpdateShots()
        {
            for (int i = 0; i < ShotList.Count; i++)
            {
                ShotList[i].Duration--;

                var temp = ShotList[i].Position;
                temp.X += (float)Math.Cos(ShotList[i].Rotation) * ShotList[i].Speed;
                temp.Y += (float)Math.Sin(ShotList[i].Rotation) * ShotList[i].Speed;
                ShotList[i].Position = temp;

                if (ShotList[i].Duration > 00) continue;
                ShotList.RemoveAt(i);
                i--;
            }
        }
        public bool Collision(Sprite sprite, out float damage)
        {
            for (int i = 0; i < ShotList.Count; i++)
            {
                if (!ShotList[i].Intersects(sprite)) continue;
                damage = ShotList[i].Damage;
                ShotList.RemoveAt(i);
                return true;
            }
            damage = 0;
            return false;
        }
        public float Update(float energy)
        {
            if (CooldownCounter != Cooldown) CooldownCounter++;
            UpdateShots();
            return 0f;
        }

        public new void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, origin: Origin, rotation: Rotation);
            foreach (var shot in ShotList)
            {
                shot.Draw(spriteBatch);
            }
        }
    }
}
