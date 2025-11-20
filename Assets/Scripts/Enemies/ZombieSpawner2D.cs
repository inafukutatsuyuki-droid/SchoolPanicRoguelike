using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SchoolPanicRoguelike.Game;
using SchoolPanicRoguelike.Level;

namespace SchoolPanicRoguelike.Enemies
{
    public class ZombieSpawner2D : MonoBehaviour
    {
        [SerializeField]
        private GameObject zombiePrefab;

        [SerializeField]
        private List<SpawnPoint2D> spawnPoints = new List<SpawnPoint2D>();

        [SerializeField]
        private int baseSpawnCount = 5;

        [SerializeField]
        private int maxZombies = 25;

        [SerializeField]
        private float spawnInterval = 10f;

        private readonly List<GameObject> _spawnedZombies = new List<GameObject>();
        private Coroutine _spawnRoutine;

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted += HandleRunStarted;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted -= HandleRunStarted;
            }
        }

        private void HandleRunStarted()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
            }

            _spawnRoutine = StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            SpawnInitialWave();

            while (GameManager.Instance != null && GameManager.Instance.CurrentState == GameState.Playing)
            {
                yield return new WaitForSeconds(spawnInterval);
                SpawnZombie();
                CleanupDestroyedZombies();
            }
        }

        private void SpawnInitialWave()
        {
            CleanupDestroyedZombies();
            int difficulty = GameManager.Instance != null ? GameManager.Instance.DifficultyLevel : 1;
            int spawnCount = baseSpawnCount * difficulty;

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnZombie();
            }
        }

        private void SpawnZombie()
        {
            CleanupDestroyedZombies();

            if (_spawnedZombies.Count >= maxZombies)
            {
                return;
            }

            SpawnPoint2D point = GetRandomSpawnPoint();
            if (point == null || zombiePrefab == null)
            {
                return;
            }

            GameObject zombie = Instantiate(zombiePrefab, point.transform.position, Quaternion.identity);
            _spawnedZombies.Add(zombie);
        }

        private SpawnPoint2D GetRandomSpawnPoint()
        {
            List<SpawnPoint2D> zombiePoints = spawnPoints.FindAll(p => p.SpawnType == SpawnCategory.Zombie);
            if (zombiePoints.Count == 0)
            {
                return null;
            }

            int index = Random.Range(0, zombiePoints.Count);
            return zombiePoints[index];
        }

        private void CleanupDestroyedZombies()
        {
            _spawnedZombies.RemoveAll(z => z == null);
        }
    }
}
