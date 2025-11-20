using System.Collections.Generic;
using UnityEngine;
using SchoolPanicRoguelike.Game;
using SchoolPanicRoguelike.Level;

namespace SchoolPanicRoguelike.Enemies
{
    public class GuardSpawner2D : MonoBehaviour
    {
        [SerializeField]
        private GameObject guardPrefab;

        [SerializeField]
        private List<GuardPatrolPoint2D> spawnPoints = new List<GuardPatrolPoint2D>();

        [SerializeField]
        private int baseGuardCount = 3;

        public void Start()
        {
            SpawnGuards();
        }

        public void SetGuardCount(int count)
        {
            baseGuardCount = Mathf.Max(0, count);
        }

        private void SpawnGuards()
        {
            if (GameManager.Instance == null || GameManager.Instance.CurrentScenario != ScenarioType.TrainingGoneWrong)
            {
                return;
            }

            if (guardPrefab == null || spawnPoints.Count == 0)
            {
                return;
            }

            int difficulty = Mathf.Max(1, GameManager.Instance.DifficultyLevel);
            int spawnCount = Mathf.Min(spawnPoints.Count, baseGuardCount * difficulty);

            for (int i = 0; i < spawnCount; i++)
            {
                GuardPatrolPoint2D point = spawnPoints[Random.Range(0, spawnPoints.Count)];
                GameObject guard = Instantiate(guardPrefab, point.transform.position, Quaternion.identity);
                GuardController2D controller = guard.GetComponent<GuardController2D>();
                if (controller != null)
                {
                    controller.SetPatrolRoute(point.GetPatrolPoints());
                }
            }
        }
    }
}
