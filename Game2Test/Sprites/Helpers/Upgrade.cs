using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2Test.Sprites.Helpers
{
    public class Upgrade
    {
        public float HealthBoost { get; set; }
        public float EnergyBoost { get; set; }
        public float EnergyRegenBoost { get; set; }

        public Upgrade(Upgrade upgrade)
        {
            HealthBoost = upgrade.HealthBoost;
            EnergyBoost = upgrade.EnergyBoost;
            EnergyRegenBoost = upgrade.EnergyRegenBoost;
        }

        public Upgrade(float healthBoost, float energyBoost, float energyRegenBoost)
        {
            HealthBoost = healthBoost;
            EnergyBoost = energyBoost;
            EnergyRegenBoost = energyRegenBoost;
        }
    }
}
