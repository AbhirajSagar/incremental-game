using System;
using System.Linq;
using UnityEngine;

public class FishSpawnManager : Singleton<FishSpawnManager>
{
    [Header("BOUNDS")]
    public Bounds SpawnBounds;
    public Bounds[] SpawnBoundsArray;

    [Header("SPAWN SETTINGS")]
    public float SpawnInterval = 1f;
    
    // UPDATED: Now uses the new struct for better performance!
    public MinMaxInt InitialSpawnCount = new MinMaxInt { Min = 8, Max = 24 };

    [Header("REFERENCES")]
    public Fish FishPrefab;

    protected override void Awake()
    {
        base.Awake();
        InitialFishSpawning();
        InvokeRepeating(nameof(SpawnFish), SpawnInterval, SpawnInterval);
    }

    private void InitialFishSpawning()
    {
        for (int i = 0; i < InitialSpawnCount.GetRandomValue(); i++)
        {
            Fish FishInstance = SpawnFishInBounds(SpawnBounds);
            Bounds FarthestBound = SpawnBoundsArray.OrderByDescending(bounds => bounds.DistanceFrom(FishInstance.transform.position)).First();
            SetTargetPosFromBounds(FishInstance, FarthestBound);
        }
    }

    private void SpawnFish()
    {
        if (SpawnBoundsArray == null || SpawnBoundsArray.Length == 0) return;

        int index = UnityEngine.Random.Range(0, SpawnBoundsArray.Length);

        Fish FishInstance = SpawnFishInBounds(SpawnBoundsArray[index]);
        index = index == 0 ? 1 : 0;

        SetTargetPosFromBounds(FishInstance, SpawnBoundsArray[index]);
    }

    private Fish SpawnFishInBounds(Bounds bounds)
    {
        Vector3 RandomSpawnPos = bounds.GetRandomPos();
        Fish FishInstance = Instantiate(FishPrefab, transform.position + RandomSpawnPos, Quaternion.identity);

        return FishInstance;
    }

    private void SetTargetPosFromBounds(Fish fish, Bounds bounds)
    {
        Vector3 RandomTargetPos = bounds.GetRandomPos();
        fish.SetTargetPos(transform.position + RandomTargetPos);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + SpawnBounds.Offset, SpawnBounds.Size);

        if (SpawnBoundsArray != null && SpawnBoundsArray.Length >= 2)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + SpawnBoundsArray[0].Offset, SpawnBoundsArray[0].Size);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position + SpawnBoundsArray[1].Offset, SpawnBoundsArray[1].Size);
        }
    }
}