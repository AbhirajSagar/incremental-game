using UnityEngine;

public static class DamageNumbersGenerator
{
    public static Transform CreateDamageNumber(Vector3 position, float damage)
    {
        DamageNumber Prefab = Resources.Load<DamageNumber>(nameof(DamageNumber));
        DamageNumber Clone = Object.Instantiate(Prefab, position, Quaternion.identity);
        Clone.Initialize(damage);
        
        return Clone.transform;
    }
}