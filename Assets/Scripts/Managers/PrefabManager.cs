using UnityEngine;

/// <summary>
/// Centralized helper for instantiating and managing prefab spawns.
/// Avoids duplicated Instantiate code and keeps BattleManager clean.
/// </summary>
public class PrefabManager
{
    private Transform defaultParent;

    public PrefabManager(Transform defaultParent = null)
    {
        this.defaultParent = defaultParent;
    }

    /// <summary>
    /// Spawns a prefab at a world position with optional parent override.
    /// </summary>
    public GameObject Spawn(GameObject prefab, Vector3 position, Transform parentOverride = null)
    {
        if (prefab == null)
        {
            Debug.LogError("Tried to spawn a null prefab.");
            return null;
        }

        Transform parent = parentOverride ?? defaultParent;
        GameObject obj = Object.Instantiate(prefab, position, Quaternion.identity, parent);

        //Scale to size
        obj.transform.localScale = prefab.transform.localScale;
        return obj;

    }

    /// <summary>
    /// Spawns a prefab with localPosition instead of world space.
    /// Useful for UI or anchored transforms.
    /// </summary>
    public GameObject SpawnLocal(GameObject prefab, Vector3 localPosition, Transform parentOverride = null)
    {
        GameObject obj = Spawn(prefab, Vector3.zero, parentOverride);
        if (obj != null)
        {
            obj.transform.localPosition = localPosition;
        }
        return obj;
    }

    /// <summary>
    /// Quickly attach prefab to a parent without caring about position.
    /// </summary>
    public GameObject Attach(GameObject prefab, Transform parentOverride = null)
    {
        return Spawn(prefab, Vector3.zero, parentOverride);
    }
}
