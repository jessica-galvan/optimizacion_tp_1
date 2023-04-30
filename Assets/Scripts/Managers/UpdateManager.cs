using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    [ReadOnly] public CustomUpdate fixCustomUpdater;
    [ReadOnly] public CustomUpdate gameplayCustomUpdate;
    public int targetFrameRateGameplay = 60;

    [ReadOnly] public CustomUpdate uiCustomUpdate;
    public int targetFrameRateUI = 30;

    public bool setApplicationTargetFramte = false;
    public int maxTargetFrame = 75;

    public void Initialize()
    {
        //there are two gaemplayList because the second one can constantly change as the bullets and enemies come and go.
        //Meanwhile the fixed one does not change from start to finish and has the main things: 
        //GameManager, InputManager, EnemyManger, PlayerController,
        //concidentally, those are the ones we want to check before the other ones

        fixCustomUpdater = gameObject.AddComponent<CustomUpdate>();
        fixCustomUpdater.Initialize(targetFrameRateGameplay, "Managers");

        gameplayCustomUpdate = gameObject.AddComponent<CustomUpdate>();
        gameplayCustomUpdate.Initialize(targetFrameRateGameplay, "Gameplay");

        uiCustomUpdate = gameObject.AddComponent<CustomUpdate>();
        uiCustomUpdate.Initialize(targetFrameRateUI, "UI");


        if (setApplicationTargetFramte)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = maxTargetFrame;
        }
    }

    void Update()
    {
        fixCustomUpdater.UpdateList();
        gameplayCustomUpdate.UpdateList();
        uiCustomUpdate.UpdateList();
    }
}
