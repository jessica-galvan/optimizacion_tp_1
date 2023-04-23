using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class HUDManager : MonoBehaviour, IUpdate
{
    [Header("Pause")]
    public GameObject pause;

    [Header("HUD")]
    public GameObject hud;
    public TMP_Text timer;
    public GameObject bulletUI;
    

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
        GameManager.Instance.updateManager.AddToUIUpdate(this);
        player = GameManager.Instance.Player;

        //pauseMenu.SetActive(false);

        //HUD
        hud.SetActive(true);
        bulletsUI.Add(bulletUI);
        int maxNeedBullets = player.maxBullets - 1;
        for (int i = 0; i < maxNeedBullets; i++)
        {
            var bulletAux = Instantiate(bulletUI, bulletUI.transform.parent);
            bulletsUI.Add(bulletAux);
        }
    }

    public void DoUpdate()
    {
        print("ui update");
        UpdateBullets(player.CurrentBullets); //could be an event
        UpdateTimer(GameManager.Instance.CurrentTime);
    }

    private void OnPause(bool isPause)
    {
        hud.SetActive(!isPause);
        //pauseMenu.SetActive(isPause);
    }

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

    private void OnDestroy()
    {
        GameManager.Instance.OnPause -= OnPause;
        GameManager.Instance.updateManager.RemoveToUIUpdate(this);
    }
}
