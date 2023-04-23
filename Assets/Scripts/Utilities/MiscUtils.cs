using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscUtils 
{
    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}
