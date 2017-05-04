using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace Game2Test.Sprites.Entities
{
    public class Shot : Sprite
    {
        public string Name { get; set; } = "default";
        public int Duration { get; set; }
        public float Speed { get; set; }
        public float Damage { get; set; }

        public Shot() { }

        public Shot(Shot shot)
        {
            Name = shot.Name;
            Duration = shot.Duration;
            Speed = shot.Speed;
            Damage = shot.Damage;
            //Sprite
            Rotation = shot.Rotation;
            Position = shot.Position;
            Rectangle = shot.Rectangle;
            Origin = shot.Origin;
            Texture = shot.Texture;
            //End
        }
        public Shot(Texture2D texture, Vector2 position, float rotation, int duration, float speed, float damage) : base(texture, position, rotation)
        {
            Duration = duration;
            Speed = speed;
            Damage = damage;
        }
        public Shot(Texture2D texture, int duration, string name, int speed, float damage) : base(texture)
        {
            Duration = duration;
            Speed = speed;
            Damage = damage;
            Name = name;
        }

        public Shot(Texture2D texture, Vector2 position, float rotation, int duration, float damage, Rectangle rectangle, Vector2 origin) : base(texture, position, rotation)
        {
            Duration = duration;
            Damage = damage;
            Rectangle = rectangle;
            Origin = origin;
        }
    }
}