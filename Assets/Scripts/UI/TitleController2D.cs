using UnityEngine;
using SchoolPanicRoguelike.Game;

namespace SchoolPanicRoguelike.UI
{
    public class TitleController2D : MonoBehaviour
    {
        [SerializeField]
        private string gameSceneName = "Game";

        [SerializeField]
        private int defaultDifficulty = 1;

        public void StartZombieScenario()
        {
            StartScenario(ScenarioType.Zombie);
        }

        public void StartTrainingScenario()
        {
            StartScenario(ScenarioType.TrainingGoneWrong);
        }

        private void StartScenario(ScenarioType scenario)
        {
            int seed = (int)System.DateTime.Now.Ticks;
            if (GameManager.Instance != null)
            {
                GameManager.Instance.SetScenario(scenario);
                GameManager.Instance.StartRun(seed, defaultDifficulty, gameSceneName);
            }
        }
    }
}
