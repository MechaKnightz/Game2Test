using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2Test.Sprites.Helpers
{
    public class Upgrade
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Cost { get; set; }

        public float HealthBoost { get; set; }
        public float EnergyBoost { get; set; }
        public float EnergyRegenBoost { get; set; }

        public Upgrade(Upgrade upgrade)
        {
            HealthBoost = upgrade.HealthBoost;
            EnergyBoost = upgrade.EnergyBoost;
            EnergyRegenBoost = upgrade.EnergyRegenBoost;
            Name = upgrade.Name;
            Description = upgrade.Description;
            Cost = upgrade.Cost;
        }

        public Upgrade(float healthBoost, float energyBoost, float energyRegenBoost, float cost)
        {
            HealthBoost = healthBoost;
            EnergyBoost = energyBoost;
            EnergyRegenBoost = energyRegenBoost;
            Cost = cost;

            Description = "Healthboos: " + HealthBoost + "\nEnergyboost:" + EnergyBoost + "\nEnergyRegboost: " +
                          EnergyRegenBoost;

            Name = "Healthbooster";
            if (EnergyBoost > HealthBoost) Name = "Energybooster";
            if (EnergyRegenBoost > HealthBoost) Name = "EnergyRegbooster";
        }
    }
}
