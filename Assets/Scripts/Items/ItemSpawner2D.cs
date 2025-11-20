using System.Collections.Generic;
using UnityEngine;
using SchoolPanicRoguelike.Game;
using SchoolPanicRoguelike.Level;

namespace SchoolPanicRoguelike.Items
{
    public class ItemSpawner2D : MonoBehaviour
    {
        [SerializeField]
        private List<SpawnPoint2D> spawnPoints = new List<SpawnPoint2D>();

        [SerializeField]
        private GameObject weaponItemPrefab;

        [SerializeField]
        private GameObject healingItemPrefab;

        [SerializeField]
        private int baseItemCount = 3;

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted += SpawnItems;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted -= SpawnItems;
            }
        }

        private void SpawnItems()
        {
            int difficulty = GameManager.Instance != null ? GameManager.Instance.DifficultyLevel : 1;
            int spawnCount = baseItemCount + difficulty;
            List<SpawnPoint2D> itemPoints = spawnPoints.FindAll(p => p.SpawnType == SpawnCategory.Item);

            if (itemPoints.Count == 0)
            {
                return;
            }

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnPoint2D point = itemPoints[Random.Range(0, itemPoints.Count)];
                SpawnRandomItem(point.transform.position);
            }
        }

        private void SpawnRandomItem(Vector3 position)
        {
            GameObject prefabToSpawn = Random.value > 0.5f ? weaponItemPrefab : healingItemPrefab;
            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, position, Quaternion.identity);
            }
        }
    }
}
