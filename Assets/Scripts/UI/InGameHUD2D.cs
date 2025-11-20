using UnityEngine;
using UnityEngine.UI;
using SchoolPanicRoguelike.Game;
using SchoolPanicRoguelike.Player;

namespace SchoolPanicRoguelike.UI
{
    public class InGameHUD2D : MonoBehaviour
    {
        [SerializeField]
        private Slider healthSlider;

        [SerializeField]
        private Slider staminaSlider;

        [SerializeField]
        private Text weaponNameText;

        [SerializeField]
        private Text timerText;

        [SerializeField]
        private GameObject detectionMeterRoot;

        private PlayerStats _playerStats;

        private void OnEnable()
        {
            UpdateDetectionMeterVisibility();
        }

        public void BindPlayer(PlayerStats stats)
        {
            _playerStats = stats;
            if (healthSlider != null)
            {
                healthSlider.maxValue = _playerStats.CurrentHealth;
                healthSlider.value = _playerStats.CurrentHealth;
            }

            if (staminaSlider != null)
            {
                staminaSlider.maxValue = _playerStats.CurrentStamina;
                staminaSlider.value = _playerStats.CurrentStamina;
            }
        }

        public void SetWeaponName(string name)
        {
            if (weaponNameText != null)
            {
                weaponNameText.text = name;
            }
        }

        private void Update()
        {
            if (_playerStats != null)
            {
                if (healthSlider != null)
                {
                    healthSlider.value = _playerStats.CurrentHealth;
                }

                if (staminaSlider != null)
                {
                    staminaSlider.value = _playerStats.CurrentStamina;
                }
            }

            if (timerText != null && GameManager.Instance != null)
            {
                float time = GameManager.Instance.RunTime;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        private void UpdateDetectionMeterVisibility()
        {
            if (detectionMeterRoot == null)
            {
                return;
            }

            bool showMeter = GameManager.Instance != null && GameManager.Instance.CurrentScenario == ScenarioType.TrainingGoneWrong;
            detectionMeterRoot.SetActive(showMeter);
        }
    }
}
