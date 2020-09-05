using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Characters
{
    public class DamageCalculator : MonoBehaviour, IDamageCalculator
    {
        [Range(0, 100)]
        public float DamageReducer;

        public float Calculate(float amount, ref float armor)
        {
            if (armor <= 0)
                return amount;

            armor -= amount;

            if (armor < 0)
                amount += Math.Abs(armor);

            amount = amount / 100 * (100 - DamageReducer);

            return amount;
        }
    }
}
