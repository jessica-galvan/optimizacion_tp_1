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
    private const string MAIN_MENU_SCENE = "Menu";

    [Header("Pause")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject buttonsContainer;
    [SerializeField] private MenuButton resumeButton;
    [SerializeField] private MenuButton restartButton;
    [SerializeField] private MenuButton menuButton;
    [SerializeField] private MenuButton quitButton;
    public KeyCode goBackKey = KeyCode.Escape;

    [Header("HUD")]
    [SerializeField] private GameObject hud;
    [SerializeField] private TMP_Text timer;
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private TMP_Text txtEnemyCount;
    [SerializeField] private GameObject deadCounter;
    [SerializeField] private TMP_Text txtDeadCount;

    [Header("Prompt Popup")]
    [SerializeField] private GameObject exitPopup;
    [SerializeField] private MenuButton exitPopupButton;
    [SerializeField] private MenuButton cancelExitPopupButton;

    [Header("Win Popup")]
    [SerializeField] private GameObject popupWin;
    [SerializeField] private MenuButton popupConfirmWinButton;

    private GameManager gameManager;
    private MenuButton selectedButton;
    private Action confirmAction = delegate { };
    private bool promptPopupActive;
    private bool deadCountActive = false;

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

        gameManager.enemyManager.OnEnemyKilled += UpdateEnemyCounter;

        UpdateEnemyCounter(gameManager.enemyManager.totalKilled);

        //Pause
        pauseMenu.SetActive(false);
        resumeButton.button.onClick.AddListener(OnClickResumeHandler);
        restartButton.button.onClick.AddListener(OnClickRestartHandler);
        menuButton.button.onClick.AddListener(OnClickMenuHandler);
        quitButton.button.onClick.AddListener(OnClickQuitHandler);

        exitPopup.SetActive(false);
        promptPopupActive = false;
        popupConfirmWinButton.button.onClick.AddListener(MainMenu);
        exitPopupButton.button.onClick.AddListener(ConfirmAction);
        cancelExitPopupButton.button.onClick.AddListener(()=>SetExitPopupActive(false));

        //HUD
        hud.SetActive(true);
        deadCounter.SetActive(false);

        //POP
        gameManager.OnWin += OnWin;
        SetWinPopUpActive(false);
    }

    public void DoUpdate()
    {
        UpdateTimer(gameManager.CurrentTime);
    }

    private void OnPause(bool isPause)
    {
        hud.SetActive(!isPause);
        pauseMenu.SetActive(isPause);

        if (isPause)
        {
            selectedButton = resumeButton;
            selectedButton.button.Select();
        }
        else
        {
            SetExitPopupActive(false);
        }

    }

    #region HUD
    private void UpdateEnemyCounter(int currentKilledQuantity)
    {
        txtEnemyCount.text = $"{currentKilledQuantity}/{gameManager.globalConfig.totalEnemiesLevel}"; 
    }

    //private void UpdateBullets(int bulletQuantity)
    //{
    //    bool bulletVisilble = true;
    //    bulletQuantity--; //because the bulletsUI starts in 0
    //    for (int i = 0; i < bulletsUI.Count; i++)
    //    {
    //        if (bulletVisilble)
    //        {
    //            bulletVisilble = bulletQuantity >= i;
    //        }

    //        bulletsUI[i].SetActive(bulletVisilble);
    //    }
    //}

    public void UpdateTimer(float timeInSeconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(timeInSeconds);
        timer.text = time.ToString("mm':'ss");
    }
    #endregion

    #region PauseMenu
    private void OnClickResumeHandler()
    {
        selectedButton = resumeButton;
        gameManager.SetPause(false);
    }

    private void OnClickRestartHandler()
    {
        selectedButton = restartButton;
        confirmAction = ReloadScene;
        SetExitPopupActive(true);
    }

    private void OnClickMenuHandler()
    {
        selectedButton = menuButton;
        confirmAction = MainMenu;
        SetExitPopupActive(true);
    }

    private void OnClickQuitHandler()
    {
        selectedButton = quitButton;
        confirmAction = QuitApp;
        SetExitPopupActive(true);
    }

    private void ConfirmAction()
    {
        confirmAction.Invoke();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void MainMenu()
    {
        SceneManager.LoadScene(MAIN_MENU_SCENE);
    }

    private void QuitApp()
    {
        Application.Quit();
    }

    #endregion

    private void SetExitPopupActive(bool value)
    {
        if (promptPopupActive == value) return;
        promptPopupActive = value;

        exitPopup.SetActive(value);
        buttonsContainer.SetActive(!value);

        if (!value)
        {
            confirmAction = null;
            if (gameManager.Pause)
            {
                exitPopupButton.Deselect();
                cancelExitPopupButton.Deselect();
                selectedButton.button.Select();
            }
        }
        else
        {
            exitPopupButton.button.Select();
        }
    }

    private void OnWin()
    {
        SetWinPopUpActive(true);
    }

    private void SetWinPopUpActive(bool value)
    {
        popupWin.SetActive(value);

        if (value)
        {
            popupConfirmWinButton.button.Select();
        }
    }

    private void OnPlayerDead()
    {
        txtDeadCount.text = gameManager.PlayerDeadCounter.ToString();

        if (!deadCountActive)
        {
            deadCountActive = true;
            deadCounter.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        resumeButton.button.onClick.RemoveAllListeners();
        restartButton.button.onClick.RemoveAllListeners();
        menuButton.button.onClick.RemoveAllListeners();
        quitButton.button.onClick.RemoveAllListeners();
        popupConfirmWinButton.button.onClick.RemoveAllListeners();
        exitPopupButton.button.onClick.RemoveAllListeners();
        cancelExitPopupButton.button.onClick.RemoveAllListeners();


        if (GameManager.HasInstance)
        {
            gameManager.OnPause -= OnPause;
            gameManager.OnPlayerDie -= OnPlayerDead;
            gameManager.OnWin -= OnWin;
            gameManager.updateManager.uiCustomUpdate.Remove(this);
            gameManager.enemyManager.OnEnemyKilled -= UpdateEnemyCounter;
        }
    }
}
