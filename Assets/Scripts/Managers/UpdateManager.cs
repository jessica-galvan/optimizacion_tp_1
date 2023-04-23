using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public int targetFPS = 60;

    private List<IUpdate> gameplay = new List<IUpdate>();
    private List<IUpdate> ui = new List<IUpdate>();

    void Awake()
    {
        //seteamos que el framerate del juego sea al target, en este caso 60ps
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFPS;
    }

    void Update()
    {
        //Gameplay Updatea todos los frames, el framerate esta en 60
        for (int i = gameplay.Count - 1; i >= 0; i--)
        {
            gameplay[i].DoUpdate();
        }

        //UI debe correr a 30fps
        var currentTimeFrame = Time.frameCount % 2;//UI que es la mitad de frames de Gameplay, los cuales seteamos en el Awake, entonces dividimos el frame count por 2
        if (currentTimeFrame == 1) //y hacemos que cada dos frames updatee UI, quedando asi 30
        {
            for (int i = ui.Count - 1; i >= 0; i--)
            {
                ui[i].DoUpdate();
            }
        }
    }

    public void AddToGameplayUpdate(IUpdate item)
    {
        if (!gameplay.Contains(item))
            gameplay.Add(item);
    }

    public void RemoveToGameplayUpdate(IUpdate item)
    {
        if(gameplay.Contains(item))
            gameplay.Remove(item);
    }

    public void AddToUIUpdate(IUpdate item)
    {
        if (!ui.Contains(item))
            ui.Add(item);
    }

    public void RemoveToUIUpdate(IUpdate item)
    {
        if (ui.Contains(item))
            ui.Remove(item);
    }
}
