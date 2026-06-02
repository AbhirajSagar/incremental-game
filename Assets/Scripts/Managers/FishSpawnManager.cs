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

public class FishSpawnManager : MonoBehaviour
{
    [Header("BOUNDS")]
    public Vector3 SpawnBounds;
    public Bounds[] SpawnBoundsArray;

    [Header("SPAWN SETTINGS")]
    public float SpawnInterval = 1f;

    [Header("REFERENCES")]
    public Fish FishPrefab;

    private void Awake()
    {
        InvokeRepeating(nameof(SpawnFish), SpawnInterval, SpawnInterval);
    }

    private void SpawnFish()
    {
        int index = UnityEngine.Random.Range(0, SpawnBoundsArray.Length);
        
        Vector3 RandomSpawnPos = SpawnBoundsArray[index].GetRandomPos();
        Fish FishInstance = Instantiate(FishPrefab, transform.position + RandomSpawnPos, Quaternion.identity);
        
        index = index == 0 ? 1 : 0;

        Vector3 RandomTargetPos = SpawnBoundsArray[index].GetRandomPos();
        FishInstance.SetTargetPos(transform.position + RandomTargetPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, SpawnBounds);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + SpawnBoundsArray[0].Offset, SpawnBoundsArray[0].Size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position + SpawnBoundsArray[1].Offset, SpawnBoundsArray[1].Size);
    }
}
