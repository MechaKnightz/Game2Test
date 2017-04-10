using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2Test.Sprites.Entities
{
    public class Shot : Sprite
    {
        public string Name { get; set; } = "default";
        public int Duration { get; set; }
        public float Speed { get; }
        public float Damage;

        public Shot() { }
        public Shot(Texture2D texture, Vector2 position, float rotation, int duration) : base(texture, position, rotation)
        {
            Duration = duration;
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

        public void Update()
        {
            
        }
    }
}