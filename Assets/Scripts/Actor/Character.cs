using System;
using Assets.Scripts.Characters;
using UnityEngine;

//This is the base class of all characters, including the NPC's
namespace Assets.Scripts.Actor
{
    public abstract class Character : MonoBehaviour
    {
        [SerializeField] private bool _isDead;
        [SerializeField] private float _health = 100f;
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _maxHealthModifier = 130f;
        [SerializeField] private float _armor = 50f;
        [SerializeField] private float _maxArmor = 50f;
        [SerializeField] private float _maxArmorModifier = 100f;
        public IDamageCalculator DamageCalculator;

        private float _defaultHealth;
        private float _defaultArmor;

        private void Start()
        {
            _defaultHealth = _maxHealth;
            _defaultArmor = _maxArmorModifier;

            DamageCalculator = GetComponent<IDamageCalculator>();

            if (DamageCalculator == null)
                throw new NullReferenceException("A component with the IDamageCalculator is required for a Character.");
        }

        /// <summary>
        /// Brings damage to the character if the health is above zero
        /// </summary>
        /// <param name="amount">The amount of damage</param>
        /// <param name="sender">Who caused the damage</param>
        /// <param name="ignoreArmor">Whether or the armor need to be ignored</param>
        public void TakeDamage(float amount, bool ignoreArmor = false)
        {
            if (_isDead)
                return;

            if (!ignoreArmor)
                amount = DamageCalculator.Calculate(amount, ref _armor);

            _health -= amount;

            if (_health < 0)
            {
                Die();
                return;
            }

            if (_maxHealth > _defaultHealth)
                _maxHealth = _health <= _maxHealth ? _defaultHealth : _maxHealth;

            if (_maxArmor > _defaultArmor)
                _maxArmor = _armor <= _maxArmor ? _defaultArmor : _maxArmor;
        }

        /// <summary>
        /// Adds a health modifier to the maximum health
        /// </summary>
        /// <param name="amount">The amount of health to add</param>
        public void AddHealthModifier(float amount) => 
            _maxHealth = _maxHealth + amount > _maxHealthModifier ? _maxHealthModifier : _maxHealth;

        /// <summary>
        /// Adds an armor modifier to the maximum armor
        /// </summary>
        /// <param name="amount">The amount of armor to add</param>
        public void AddArmorModifier(float amount) => 
            _maxArmor = _maxArmor + amount > _maxArmorModifier ? _maxArmorModifier : _maxArmor;


        protected void Die()
        {
            Debug.Log($"Character was killed", this);
            _isDead = true;
            Destroy(gameObject);
        }
    }
}
