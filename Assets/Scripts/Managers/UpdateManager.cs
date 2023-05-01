using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    [ReadOnly] public CustomUpdate fixCustomUpdater;
    [ReadOnly] public CustomUpdate gameplayCustomUpdate;
    [ReadOnly] public CustomUpdate uiCustomUpdate;

    public void Initialize()
    {
        //there are two gaemplayList because the second one can constantly change as the bullets and enemies come and go.
        //Meanwhile the fixed one does not change from start to finish and has the main things: 
        //GameManager, InputManager, EnemyManger, PlayerController,
        //concidentally, those are the ones we want to check before the other ones

        fixCustomUpdater = gameObject.AddComponent<CustomUpdate>();
        fixCustomUpdater.Initialize(GameManager.Instance.globalConfig.gameplayFPSTarget, "Managers");

        gameplayCustomUpdate = gameObject.AddComponent<CustomUpdate>();
        gameplayCustomUpdate.Initialize(GameManager.Instance.globalConfig.gameplayFPSTarget, "Gameplay");

        uiCustomUpdate = gameObject.AddComponent<CustomUpdate>();
        uiCustomUpdate.Initialize(GameManager.Instance.globalConfig.uiFPSTarget, "UI");


        if (GameManager.Instance.globalConfig.activeMaxAppTarget)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = GameManager.Instance.globalConfig.maxFPSTarget;
        }
    }

    void Update()
    {
        fixCustomUpdater.UpdateList();
        gameplayCustomUpdate.UpdateList();
        uiCustomUpdate.UpdateList();
    }
}
