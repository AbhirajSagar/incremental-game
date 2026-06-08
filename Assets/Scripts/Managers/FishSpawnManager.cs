using System.Linq;
using UnityEngine;

public class FishSpawnManager : Singleton<FishSpawnManager>
{
    [Header("BOUNDS")]
    public Bounds SpawnBounds;
    public Bounds[] SpawnBoundsArray;

    [Header("SPAWN SETTINGS")]
    public float SpawnInterval = 1f;
    public MinMaxInt InitialSpawnCount = new MinMaxInt { Min = 8, Max = 24 };

    [Header("REFERENCES")]
    public Fish FishPrefab;

    protected override void Awake()
    {
        base.Awake();
        InitialFishSpawning();
        InvokeRepeating(nameof(SpawnFish), SpawnInterval, SpawnInterval);
    }

    public void StopSpawning() => CancelInvoke(nameof(SpawnFish));

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
        
        // [FIXED] Previously hardcoded logic that broke if array was > 2 elements
        int targetIndex = index;
        if (SpawnBoundsArray.Length > 1)
        {
            while (targetIndex == index)
            {
                targetIndex = UnityEngine.Random.Range(0, SpawnBoundsArray.Length);
            }
        }

        SetTargetPosFromBounds(FishInstance, SpawnBoundsArray[targetIndex]);
    }

    private Fish SpawnFishInBounds(Bounds bounds)
    {
        Vector3 RandomSpawnPos = bounds.GetRandomPos();
        return Instantiate(FishPrefab, transform.position + RandomSpawnPos, Quaternion.identity);
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
            for (int i = 0; i < SpawnBoundsArray.Length; i++)
            {
                Gizmos.DrawWireCube(transform.position + SpawnBoundsArray[i].Offset, SpawnBoundsArray[i].Size);
            }
        }
    }
}