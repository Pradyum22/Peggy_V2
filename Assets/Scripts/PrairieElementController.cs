using System.Collections.Generic;
using UnityEngine;

public class PrairieElementController : MonoBehaviour
{
    [Header("Grass Prefab")]
    public GameObject grassPrefab;

    [Header("Spawn Points (Drag from Scene)")]
    public List<Transform> spawnPoints;

    private List<GameObject> spawnedGrass = new List<GameObject>();

    public void SetGrassAmount(int totalGrass)
    {
        Debug.Log($"[PrairieController] SetGrassAmount called with value: {totalGrass}");

        foreach (var grass in spawnedGrass)
            Destroy(grass);

        spawnedGrass.Clear();

        if (grassPrefab == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("[PrairieController] Missing prefab or spawn points.");
            return;
        }

        for (int i = 0; i < totalGrass; i++)
        {
            Transform spawn = spawnPoints[i % spawnPoints.Count];
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f));
            GameObject grass = Instantiate(grassPrefab, spawn.position + offset, Quaternion.identity, transform);
            spawnedGrass.Add(grass);
        }
    }
}
