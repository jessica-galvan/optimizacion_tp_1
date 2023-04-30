using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MiscUtils 
{
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static int RandomInt(int min, int max)
    {
        return Random.Range(min, max);
    }

    public static float RandomFloat(float min, float max)
    {
        return Random.Range(min, max);
    }

}
