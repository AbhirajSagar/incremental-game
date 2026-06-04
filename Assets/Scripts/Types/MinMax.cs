using System;
using UnityEngine;

[Serializable]
public class MinMax<T> where T : struct
{
    public T Min;
    public T Max;

    public MinMax(T min, T max)
    {
        Min = min;
        Max = max;
    }

    public T GetRandomValue()
    {
        if (typeof(T) == typeof(int))
        {
            int minValue = Convert.ToInt32(Min);
            int maxValue = Convert.ToInt32(Max);
            return (T)(object)UnityEngine.Random.Range(minValue, maxValue + 1);
        }
        else if (typeof(T) == typeof(float))
        {
            float minValue = Convert.ToSingle(Min);
            float maxValue = Convert.ToSingle(Max);
            return (T)(object)UnityEngine.Random.Range(minValue, maxValue);
        }
        else if(typeof(T) == typeof(Vector3))
        {
            Vector3 minValue = (Vector3)(object)Min;
            Vector3 maxValue = (Vector3)(object)Max;
            Vector3 randomValue = new Vector3
            (
                UnityEngine.Random.Range(minValue.x, maxValue.x),
                UnityEngine.Random.Range(minValue.y, maxValue.y),
                UnityEngine.Random.Range(minValue.z, maxValue.z)
            );
            
            return (T)(object)randomValue;
        }
        else
        {
            throw new InvalidOperationException("Unsupported type for MinMax. Only int, float and Vector3 are supported.");
        }
    }
}