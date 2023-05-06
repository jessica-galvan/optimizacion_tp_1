using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class EditorOnly : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ProfileStart(string profileScopeName)
    {
#if UNITY_EDITOR
        Profiler.BeginSample(profileScopeName);
#endif
    }


    public static void ProfileEnd()
    {
#if UNITY_EDITOR
        Profiler.EndSample();
#endif
    }
}
