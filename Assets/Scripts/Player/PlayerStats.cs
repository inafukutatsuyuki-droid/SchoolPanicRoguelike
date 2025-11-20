using UnityEngine;
using SchoolPanicRoguelike.Game;

namespace SchoolPanicRoguelike.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField]
        private float maxHealth = 100f;

        [SerializeField]
        private float maxStamina = 100f;

        [SerializeField]
        private float staminaRegenPerSecond = 15f;

        public float CurrentHealth { get; private set; }
        public float CurrentStamina { get; private set; }

        private void Awake()
        {
            CurrentHealth = maxHealth;
            CurrentStamina = maxStamina;
        }

        private void Update()
        {
            RegenerateStamina();
        }

        public void TakeDamage(float amount)
        {
            CurrentHealth -= amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);

            if (CurrentHealth <= 0)
            {
                GameManager.Instance?.HandleGameOver();
            }
        }

        public bool TryConsumeStamina(float amount)
        {
            if (CurrentStamina < amount)
            {
                return false;
            }

            CurrentStamina -= amount;
            return true;
        }

        public void RestoreStamina(float amount)
        {
            CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, maxStamina);
        }

        public void Heal(float amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, maxHealth);
        }

        private void RegenerateStamina()
        {
            if (CurrentStamina < maxStamina)
            {
                CurrentStamina += staminaRegenPerSecond * Time.deltaTime;
                CurrentStamina = Mathf.Clamp(CurrentStamina, 0, maxStamina);
            }
        }
    }
}
