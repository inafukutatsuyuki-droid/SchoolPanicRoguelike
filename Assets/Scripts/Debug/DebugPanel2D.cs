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

        [SerializeField]
        private Dropdown scenarioDropdown;

        [SerializeField]
        private Slider guardCountSlider;

        [SerializeField]
        private Slider guardViewDistanceSlider;

        [SerializeField]
        private Slider guardHearingSlider;

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

            if (scenarioDropdown != null && GameManager.Instance != null)
            {
                scenarioDropdown.value = GameManager.Instance.CurrentScenario == ScenarioType.TrainingGoneWrong ? 1 : 0;
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

        public void OnScenarioChanged(int index)
        {
            if (GameManager.Instance == null)
            {
                return;
            }

            ScenarioType scenario = index == 1 ? ScenarioType.TrainingGoneWrong : ScenarioType.Zombie;
            GameManager.Instance.SetScenario(scenario);
        }

        public void OnGuardCountChanged(float value)
        {
            foreach (GuardSpawner2D spawner in FindObjectsOfType<GuardSpawner2D>())
            {
                spawner.SetGuardCount(Mathf.RoundToInt(value));
            }
        }

        public void OnGuardViewDistanceChanged(float value)
        {
            foreach (GuardVision2D vision in FindObjectsOfType<GuardVision2D>())
            {
                vision.SetViewDistance(value);
            }
        }

        public void OnGuardHearingRangeChanged(float value)
        {
            foreach (GuardHearing2D hearing in FindObjectsOfType<GuardHearing2D>())
            {
                hearing.SetHearingRange(value);
            }
        }
    }
}
