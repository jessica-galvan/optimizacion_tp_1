using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour, IUpdate
{
    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button menuButton;
    [SerializeField] private Button quitButton;

    [Header("HUD")]
    [SerializeField] private GameObject hud;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private TMP_Text enemyCount;
    [SerializeField] private TMP_Text deadCount;

    [Header("WinPopup")]
    [SerializeField] private string mainMenuScene = "Menu";
    [SerializeField] private GameObject popupWin;
    [SerializeField] private Button popupMenuButton;

    private GameManager gameManager;
    private List<GameObject> bulletsUI = new List<GameObject>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        gameManager = GameManager.Instance;
        gameManager.OnPause += OnPause;
        gameManager.OnPlayerDie += OnPlayerDead;
        gameManager.updateManager.uiCustomUpdate.Add(this);

        //Pause
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(OnClickResumeHandler);
        restartButton.onClick.AddListener(OnClickRestartHandler);
        menuButton.onClick.AddListener(OnClickMenuHandler);
        quitButton.onClick.AddListener(OnClickQuitHandler);
        popupMenuButton.onClick.AddListener(OnClickMenuHandler);

        //HUD
        hud.SetActive(true);



        //bulletsUI.Add(bulletUI);
        //int maxNeedBullets = player.maxBullets - 1;
        //for (int i = 0; i < maxNeedBullets; i++)
        //{
        //    var bulletAux = Instantiate(bulletUI, bulletUI.transform.parent);
        //    bulletsUI.Add(bulletAux);
        //}

        //POP
        gameManager.OnWin += OnWin;
        SetWinPopUpActive(false);
    }

    public void DoUpdate()
    {
        //UpdateBullets(player.CurrentBullets); //could be an event
        UpdateTimer(gameManager.CurrentTime);
    }

    private void OnPause(bool isPause)
    {
        hud.SetActive(!isPause);
        pauseMenu.SetActive(isPause);
    }

    #region HUD
    private void UpdateBullets(int bulletQuantity)
    {
        bool bulletVisilble = true;
        bulletQuantity--; //because the bulletsUI starts in 0
        for (int i = 0; i < bulletsUI.Count; i++)
        {
            if (bulletVisilble)
            {
                bulletVisilble = bulletQuantity >= i;
            }

            bulletsUI[i].SetActive(bulletVisilble);
        }
    }

    public void UpdateTimer(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        timer.text = time.ToString("mm':'ss");
    }
    #endregion

    #region PauseMenu
    private void OnClickResumeHandler()
    {
        gameManager.SetPause(false);
    }

    private void OnClickRestartHandler()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnClickMenuHandler()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    private void OnClickQuitHandler()
    {
        print("Quit Game");
        Application.Quit();
    }

    #endregion

    public void OnWin()
    {
        SetWinPopUpActive(true);
    }

    public void SetWinPopUpActive(bool value)
    {
        popupWin.SetActive(value);
    }

    public void OnPlayerDead()
    {
        deadCount.text = gameManager.PlayerDeadCounter.ToString();
    }

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        popupMenuButton.onClick.RemoveAllListeners();

        if (GameManager.HasInstance)
        {
            gameManager.OnPause -= OnPause;
            gameManager.OnPlayerDie -= OnPlayerDead;
            gameManager.OnWin -= OnWin;
            gameManager.updateManager.uiCustomUpdate.Remove(this);
        }
    }
}
