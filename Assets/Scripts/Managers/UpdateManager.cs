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
        //there are two gameplayList because the second one can constantly change as the bullets and enemies come and go.
        //Meanwhile the fixed one are the ones that don't have a set frame, they update all the frames as we want don't want to limit the frame check as they depend on the craprichious input system
        //GameManager, InputManager, PlayerController.

        fixCustomUpdater = gameObject.AddComponent<CustomUpdate>();
        fixCustomUpdater.Initialize(0, "Managers");

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
