using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

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
        public Texture2D Texture { get; set; }
        public UpgradeType Type { get; set; }

        public Upgrade(Upgrade upgrade)
        {
            HealthBoost = upgrade.HealthBoost;
            EnergyBoost = upgrade.EnergyBoost;
            EnergyRegenBoost = upgrade.EnergyRegenBoost;
            Name = upgrade.Name;
            Description = upgrade.Description;
            Cost = upgrade.Cost;
            Type = upgrade.Type;
            Texture = upgrade.Texture;
        }

        public Upgrade(float healthBoost, float energyBoost, float energyRegenBoost, float cost, UpgradeType type, Texture2D texture)
        {
            HealthBoost = healthBoost;
            EnergyBoost = energyBoost;
            EnergyRegenBoost = energyRegenBoost;
            Cost = cost;
            Type = type;
            Texture = texture;

            Description = "Healthboos: " + HealthBoost + "\nEnergyboost:" + EnergyBoost + "\nEnergyRegboost: " +
                          EnergyRegenBoost;

            Name = "Healthbooster";
            if (EnergyBoost > HealthBoost) Name = "Energybooster";
            if (EnergyRegenBoost > HealthBoost) Name = "EnergyRegbooster";
        }
    }
}
