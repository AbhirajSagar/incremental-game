using UnityEngine;
using UnityEngine.Pool;

public class DamageNumbersGenerator : Singleton<DamageNumbersGenerator>
{
    [Header("References")]
    public DamageNumber Prefab; // Assign via Inspector (No more Resources.Load)

    private ObjectPool<DamageNumber> pool;

    protected override void Awake()
    {
        base.Awake();
        pool = new ObjectPool<DamageNumber>(
            createFunc: () => Instantiate(Prefab, transform),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            defaultCapacity: 20,
            maxSize: 100
        );
    }

    public void Spawn(Vector3 position, float damage)
    {
        DamageNumber clone = pool.Get();
        clone.transform.position = position;
        clone.Initialize(damage, ReturnToPool);
    }

    private void ReturnToPool(DamageNumber num)
    {
        pool.Release(num);
    }
}