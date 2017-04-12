using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Game2Test.Sprites.Entities;

namespace Game2Test.Sprites.Helpers
{
    public interface ITargetable
    {
        void HitByShot(Shot shot);
        void HitByAsteroid(Asteroid asteroid);
    }
}
