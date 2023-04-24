using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public CustomUpdate uiCustomUpdate;
    public CustomUpdate gameplayCustomUpdate;

    void Awake()
    {
        gameplayCustomUpdate.Initialize();
        uiCustomUpdate.Initialize();

        //idea original deprecada: seteamos que el framerate del juego sea al target, en este caso 60ps
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
    }


    void Update()
    {
        gameplayCustomUpdate.UpdateList();
        uiCustomUpdate.UpdateList();

        //Jess: idea original deprecada. Hice dos customUpdates y adentro chequeo si puede updatear cada uno. Pero mantengo el UpdateManager porque quiero controlar en que orden updatean los managers, primero gameplay y luego ui. 
        //Podria hacer que sortee por targetFrameRate para que siempre updatee primero el que mas frames tiene... pero ya es too much
        
        ////Gameplay Updatea todos los frames, el framerate esta en 60
        //gameplayCustomUpdate.UpdateList();

        ////UI debe correr a 30fps
        //var currentTimeFrame = Time.frameCount % 2;//UI que es la mitad de frames de Gameplay, los cuales seteamos en el Awake, entonces dividimos el frame count por 2
        //if (currentTimeFrame == 1) //y hacemos que cada dos frames updatee UI, quedando asi 30
        //{
        //    uiCustomUpdate.UpdateList();
        //}
    }
}
