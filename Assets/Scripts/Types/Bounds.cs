using System;
using UnityEngine;

[Serializable]
public class Bounds
{
    public Vector3 Size;
    public Vector3 Offset;

    public Vector3 GetRandomPos()
    {
        return new Vector3(
            UnityEngine.Random.Range(-Size.x / 2f, Size.x / 2f),
            UnityEngine.Random.Range(-Size.y / 2f, Size.y / 2f),
            UnityEngine.Random.Range(-Size.z / 2f, Size.z / 2f)
        ) + Offset;
    }
}
