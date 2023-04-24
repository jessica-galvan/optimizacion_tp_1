using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUpdate : MonoBehaviour
{
    public int targetFrameRate = 30; //podria ser un scriptable object donde tengo los framerates de todos los customUpdates y los instancio en el awake del update manager pero quizas es too much.

    private List<IUpdate> updatingList = new List<IUpdate>();
    private float targetTime;
    private float currentTime;

    public void Initialize()
    {
        //calculamos el tiempo de cada framerate
        targetTime = (float) 1 / targetFrameRate;
    }

    public void UpdateList()
    {
        //en cada frame, nos fijamos si es el momento de updatear esta lista, si devuelve falso, no updatea y ya. 
        if (!CanUpdate()) return;
        //print($"Updating {targetFrameRate} fps / frameCount {Time.frameCount} ");

        for (int i = 0; i < updatingList.Count; i++)
        {
            updatingList[i].DoUpdate();
        }
    }

    private bool CanUpdate()
    {
        var answer = false;
        currentTime += Time.deltaTime;
        if (currentTime >= targetTime)
        {
            currentTime = 0;
            answer = true;
        }
        return answer;
    }

    public void Add(IUpdate item)
    {
        if (!updatingList.Contains(item))
        {
            updatingList.Add(item);
        }
    }

    public void Remove(IUpdate item)
    {
        if (updatingList.Contains(item))
        {
            updatingList.Remove(item);
        }
    }
}
