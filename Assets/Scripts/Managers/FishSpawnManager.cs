using System;
using UnityEngine;

public class FishSpawnManager : Singleton<FishSpawnManager>
{
    [Header("BOUNDS")]
    public Vector3 SpawnBounds;
    public Bounds[] SpawnBoundsArray;

    [Header("SPAWN SETTINGS")]
    public float SpawnInterval = 1f;

    [Header("REFERENCES")]
    public Fish FishPrefab;

    protected override void Awake()
    {
        base.Awake();
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
