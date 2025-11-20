using UnityEngine;
using SchoolPanicRoguelike.Game;

namespace SchoolPanicRoguelike.Enemies
{
    public class ZombieStats : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth = 30f;

        [SerializeField]
        private float attackPower = 10f;

        public float AttackPower => attackPower;
        public float CurrentHealth { get; private set; }

        private void Awake()
        {
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            if (CurrentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            GameManager.Instance?.RegisterZombieKill();
            Destroy(gameObject);
        }
    }
}
