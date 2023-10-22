using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class BlendshapeInterface : MonoBehaviour
{
    private SkinnedMeshRenderer mr;
    private float mValue;
    public float Value
    {
        get => mValue; set
        {
            mValue = value;
            mr.SetBlendShapeWeight(0, value);
        }
    }

    private int targetBlendShapeIndex;    

}
