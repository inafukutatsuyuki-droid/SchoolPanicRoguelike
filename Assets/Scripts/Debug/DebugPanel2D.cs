using UnityEngine;
using UnityEngine.UI;
using SchoolPanicRoguelike.Enemies;
using SchoolPanicRoguelike.Items;
using SchoolPanicRoguelike.Game;

namespace SchoolPanicRoguelike.Debugging
{
    public class DebugPanel2D : MonoBehaviour
    {
        [SerializeField]
        private GameObject panelRoot;

        [SerializeField]
        private ZombieSpawner2D zombieSpawner;

        [SerializeField]
        private ItemSpawner2D itemSpawner;

        [SerializeField]
        private Slider zombieLimitSlider;

        [SerializeField]
        private Slider itemCountSlider;

        [SerializeField]
        private Slider difficultySlider;

        private bool _isOpen;

        private void Start()
        {
            ApplyInitialUIValues();
            SetPanelActive(false);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F1))
            {
                TogglePanel();
            }
        }

        private void ApplyInitialUIValues()
        {
            if (zombieLimitSlider != null)
            {
                zombieLimitSlider.value = zombieLimitSlider.maxValue;
            }

            if (itemCountSlider != null)
            {
                itemCountSlider.value = itemCountSlider.maxValue;
            }

            if (difficultySlider != null && GameManager.Instance != null)
            {
                difficultySlider.value = GameManager.Instance.DifficultyLevel;
            }
        }

        private void TogglePanel()
        {
            _isOpen = !_isOpen;
            SetPanelActive(_isOpen);
        }

        private void SetPanelActive(bool active)
        {
            if (panelRoot != null)
            {
                panelRoot.SetActive(active);
            }
        }

        public void OnZombieLimitChanged(float value)
        {
            if (zombieSpawner != null)
            {
                typeof(ZombieSpawner2D).GetField("maxZombies", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(zombieSpawner, Mathf.RoundToInt(value));
            }
        }

        public void OnItemCountChanged(float value)
        {
            if (itemSpawner != null)
            {
                typeof(ItemSpawner2D).GetField("baseItemCount", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(itemSpawner, Mathf.RoundToInt(value));
            }
        }

        public void OnDifficultyChanged(float value)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetDifficulty(Mathf.RoundToInt(value));
            }
        }
    }
}
