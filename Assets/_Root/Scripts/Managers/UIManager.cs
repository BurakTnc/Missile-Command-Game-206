using System;
using _Root.Scripts.Signals;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Root.Scripts.Managers
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        
        [SerializeField] private GameObject mainPanel, gamePanel, winPanel, losePanel, storePanel;
        [SerializeField] private TextMeshProUGUI[] moneyText;


        private void Awake()
        {
            #region Singleton

            if (Instance != this && Instance != null)
            {

                Destroy(this);
                return;
            }
            
            Instance = this;

            #endregion

        }

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            UnSubscribe();
        }

        private void Start()
        {
            SetMoneyTexts();
        }

        #region Subscribtions
        private void Subscribe()
                {
                    CoreGameSignals.Instance.OnLevelWin += LevelWin;
                    CoreGameSignals.Instance.OnLevelFail += LevelLose;
                    CoreGameSignals.Instance.OnGameStart += OnGameStart;
                }
        
                private void UnSubscribe()
                {
                    CoreGameSignals.Instance.OnLevelWin -= LevelWin;
                    CoreGameSignals.Instance.OnLevelFail -= LevelLose;
                    CoreGameSignals.Instance.OnGameStart -= OnGameStart;
                }

        #endregion

        private void Update()
        {
            SetMoneyTexts();
        }

        private void OnGameStart()
        {
            mainPanel.SetActive(false);
            gamePanel.SetActive(true);
        }
        private void SetMoneyTexts()
        {
            if (moneyText.Length <= 0) return;

            foreach (var t in moneyText)
            {
                if (t)
                {
                    t.text = "$" + GameManager.Instance.GetMoney();
                }
            }
        }
        private void LevelWin()
        {
            gamePanel.SetActive(false);
            winPanel.SetActive(true);
        }

        private void LevelLose()
        {
            gamePanel.SetActive(false);
            losePanel.SetActive(true);
        }

        public void PlayButton()
        {
            CoreGameSignals.Instance.OnGameStart?.Invoke();
            //OnGameStart();
        }

        public void MenuButton()
        {
            mainPanel.SetActive(true);
            winPanel.SetActive(false);
            losePanel.SetActive(false);
        }

        public void NextButton()
        {
            CoreGameSignals.Instance.OnSave?.Invoke();
            SceneManager.LoadScene(0);
            //CoreGameSignals.Instance.OnLevelLoad?.Invoke();
        }

        public void RetryButton()
        {
            CoreGameSignals.Instance.OnSave?.Invoke();
            SceneManager.LoadScene(0);
            //CoreGameSignals.Instance.OnLevelLoad?.Invoke();
        }

        public void CloseButton(GameObject panel)
        {
            panel.SetActive(false);
        }

        public void OpenButton(GameObject panel)
        {
            panel.SetActive(true);
        }
    }
}
