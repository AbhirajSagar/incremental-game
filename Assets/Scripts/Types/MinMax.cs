using System;
using UnityEngine;

[Serializable]
public struct MinMaxInt
{
    public int Min;
    public int Max;
    public int GetRandomValue() => UnityEngine.Random.Range(Min, Max + 1);
}

[Serializable]
public struct MinMaxFloat
{
    public float Min;
    public float Max;
    public float GetRandomValue() => UnityEngine.Random.Range(Min, Max);
}

[Serializable]
public struct MinMaxVector3
{
    public Vector3 Min;
    public Vector3 Max;
    public Vector3 GetRandomValue()
    {
        return new Vector3(
            UnityEngine.Random.Range(Min.x, Max.x),
            UnityEngine.Random.Range(Min.y, Max.y),
            UnityEngine.Random.Range(Min.z, Max.z)
        );
    }
}