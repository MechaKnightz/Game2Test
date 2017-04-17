using System.Collections.Generic;
using Game2Test.Sprites.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities.Turrets
{
    public interface ITurret
    {
        List<Shot> ShotList { get; set; }

        //Offset == turret position compared to ship position
        Vector2 Offset { get; }
        float EnergyCost { get; }
        float TurnRate { get; set; }
        TurretType Type { get; set; }
        float Cooldown { get; set; }
        float CooldownCounter { get; set; }
        bool IsFiring { get; set; }
        float Speed { get; set; }
        float Range { get; set; }

        //sprite
        Vector2 Origin { get; set; }
        Rectangle Rectangle { get; set; }
        RotatedRectangle RotatedRectangle { get; set; }

        float Rotation { get; set; }
        Texture2D Texture { get; set; }

        Vector2 Position { get; set; }
        //sprite end

        void SetRotation(float rotation);

        float Update(float energy);

        void UpdateShots();

        void Draw(SpriteBatch spriteBatch);

        bool ShotCollision(Rectangle rectangle, out Shot tempShot);

        float Fire();
        void Turn(Direction direction);

        ITurret CloneTurret(ITurret turret);
    }
}
