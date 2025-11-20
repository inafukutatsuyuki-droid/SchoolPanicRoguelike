using System.Collections.Generic;
using UnityEngine;
using SchoolPanicRoguelike.Game;

namespace SchoolPanicRoguelike.Level
{
    public class PlayerSpawnManager2D : MonoBehaviour
    {
        [SerializeField]
        private GameObject playerPrefab;

        [SerializeField]
        private List<SpawnPoint2D> spawnPoints = new List<SpawnPoint2D>();

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted += SpawnPlayer;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnRunStarted -= SpawnPlayer;
            }
        }

        private void SpawnPlayer()
        {
            if (playerPrefab == null)
            {
                return;
            }

            List<SpawnPoint2D> playerPoints = spawnPoints.FindAll(p => p.SpawnType == SpawnCategory.Player);
            if (playerPoints.Count == 0)
            {
                return;
            }

            SpawnPoint2D point = playerPoints[Random.Range(0, playerPoints.Count)];
            Instantiate(playerPrefab, point.transform.position, Quaternion.identity);
        }
    }
}
