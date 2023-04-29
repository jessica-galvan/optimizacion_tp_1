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

    [Header("WinPopup")]
    [SerializeField] private string mainMenuScene = "Menu";
    [SerializeField] private GameObject popupWin;
    [SerializeField] private Button popupMenuButton;

    private PlayerModel player;
    private List<GameObject> bulletsUI = new List<GameObject>();

    private void Awake()
    {
        GameManager.Instance.OnPause += OnPause;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        GameManager.Instance.updateManager.uiCustomUpdate.Add(this);
        player = GameManager.Instance.Player;

        //Pause
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(OnClickResumeHandler);
        restartButton.onClick.AddListener(OnClickRestartHandler);
        menuButton.onClick.AddListener(OnClickMenuHandler);
        quitButton.onClick.AddListener(OnClickQuitHandler);
        popupMenuButton.onClick.AddListener(OnClickMenuHandler);

        //HUD
        hud.SetActive(true);
        bulletsUI.Add(bulletUI);
        int maxNeedBullets = player.maxBullets - 1;
        for (int i = 0; i < maxNeedBullets; i++)
        {
            var bulletAux = Instantiate(bulletUI, bulletUI.transform.parent);
            bulletsUI.Add(bulletAux);
        }

        //POP
        GameManager.Instance.OnWin += OnWin;
        SetWinPopUpActive(false);
    }

    public void DoUpdate()
    {
        UpdateBullets(player.CurrentBullets); //could be an event
        UpdateTimer(GameManager.Instance.CurrentTime);
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
        GameManager.Instance.SetPause(false);
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

    private void OnDestroy()
    {
        resumeButton.onClick.RemoveAllListeners();
        restartButton.onClick.RemoveAllListeners();
        menuButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
        popupMenuButton.onClick.RemoveAllListeners();

        if (GameManager.HasInstance)
        {
            GameManager.Instance.OnPause -= OnPause;
            GameManager.Instance.OnWin -= OnWin;
            GameManager.Instance.updateManager.uiCustomUpdate.Remove(this);
        }
    }
}
