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

        public void StartScenario()
        {
            int seed = (int)System.DateTime.Now.Ticks;
            GameManager.Instance?.StartRun(seed, defaultDifficulty, gameSceneName);
        }
    }
}
