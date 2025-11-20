using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SchoolPanicRoguelike.Game;

namespace SchoolPanicRoguelike.UI
{
    public class ResultController2D : MonoBehaviour
    {
        [SerializeField]
        private GameObject resultPanel;

        [SerializeField]
        private Text resultText;

        [SerializeField]
        private Text timeText;

        [SerializeField]
        private Text killText;

        [SerializeField]
        private string titleSceneName = "Title";

        private void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver += ShowGameOver;
                GameManager.Instance.OnGameClear += ShowGameClear;
            }
        }

        private void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnGameOver -= ShowGameOver;
                GameManager.Instance.OnGameClear -= ShowGameClear;
            }
        }

        private void ShowGameOver()
        {
            ShowResult("GAME OVER");
        }

        private void ShowGameClear()
        {
            ShowResult("CLEAR!");
        }

        private void ShowResult(string title)
        {
            if (resultPanel != null)
            {
                resultPanel.SetActive(true);
            }

            if (resultText != null)
            {
                resultText.text = title;
            }

            if (timeText != null && GameManager.Instance != null)
            {
                float time = GameManager.Instance.RunTime;
                int minutes = Mathf.FloorToInt(time / 60f);
                int seconds = Mathf.FloorToInt(time % 60f);
                timeText.text = $"Time: {minutes:00}:{seconds:00}";
            }

            if (killText != null && GameManager.Instance != null)
            {
                killText.text = $"Kills: {GameManager.Instance.ZombieKillCount}";
            }
        }

        public void ReturnToTitle()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadTitleScene(titleSceneName);
            }
            else
            {
                SceneManager.LoadScene(titleSceneName);
            }
        }
    }
}
