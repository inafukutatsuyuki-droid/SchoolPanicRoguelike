using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SchoolPanicRoguelike.Game
{
    public enum GameState
    {
        Title,
        Playing,
        GameOver,
        Clear
    }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        public event Action OnRunStarted;
        public event Action OnGameOver;
        public event Action OnGameClear;

        [field: SerializeField]
        public int CurrentSeed { get; private set; } = 0;

        [field: SerializeField]
        public int DifficultyLevel { get; private set; } = 1;

        [field: SerializeField]
        public GameState CurrentState { get; private set; } = GameState.Title;

        public int ZombieKillCount { get; private set; }

        public float RunTime { get; private set; }

        private float _runStartTime;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (CurrentState == GameState.Playing)
            {
                RunTime = Time.time - _runStartTime;
            }
        }

        public void StartRun(int seed, int difficultyLevel, string sceneName)
        {
            CurrentSeed = seed;
            DifficultyLevel = Mathf.Max(1, difficultyLevel);
            ZombieKillCount = 0;
            RunTime = 0f;
            _runStartTime = Time.time;

            CurrentState = GameState.Playing;

            UnityEngine.Random.InitState(CurrentSeed);

            SceneManager.LoadScene(sceneName);
            OnRunStarted?.Invoke();
        }

        public void HandleGameOver()
        {
            if (CurrentState != GameState.Playing)
            {
                return;
            }

            CurrentState = GameState.GameOver;
            OnGameOver?.Invoke();
        }

        public void HandleGameClear()
        {
            if (CurrentState != GameState.Playing)
            {
                return;
            }

            CurrentState = GameState.Clear;
            OnGameClear?.Invoke();
        }

        public void RegisterZombieKill()
        {
            ZombieKillCount++;
        }

        public void LoadTitleScene(string sceneName)
        {
            CurrentState = GameState.Title;
            SceneManager.LoadScene(sceneName);
        }

        public void SetDifficulty(int difficultyLevel)
        {
            DifficultyLevel = Mathf.Max(1, difficultyLevel);
        }
    }
}
