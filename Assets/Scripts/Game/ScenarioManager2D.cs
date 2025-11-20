using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SchoolPanicRoguelike.Enemies;

namespace SchoolPanicRoguelike.Game
{
    public class ScenarioManager2D : MonoBehaviour
    {
        private readonly List<ZombieSpawner2D> _zombieSpawners = new List<ZombieSpawner2D>();
        private readonly List<GuardSpawner2D> _guardSpawners = new List<GuardSpawner2D>();

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CacheSpawners();
            ApplyScenarioSettings();
        }

        private void CacheSpawners()
        {
            _zombieSpawners.Clear();
            _guardSpawners.Clear();
            _zombieSpawners.AddRange(FindObjectsOfType<ZombieSpawner2D>(true));
            _guardSpawners.AddRange(FindObjectsOfType<GuardSpawner2D>(true));
        }

        private void ApplyScenarioSettings()
        {
            ScenarioType scenario = GameManager.Instance != null ? GameManager.Instance.CurrentScenario : ScenarioType.Zombie;
            bool trainingScenario = scenario == ScenarioType.TrainingGoneWrong;

            foreach (ZombieSpawner2D spawner in _zombieSpawners)
            {
                if (spawner != null)
                {
                    spawner.gameObject.SetActive(!trainingScenario);
                    spawner.enabled = !trainingScenario;
                }
            }

            foreach (GuardSpawner2D spawner in _guardSpawners)
            {
                if (spawner != null)
                {
                    spawner.gameObject.SetActive(trainingScenario);
                    spawner.enabled = trainingScenario;
                }
            }
        }
    }
}
