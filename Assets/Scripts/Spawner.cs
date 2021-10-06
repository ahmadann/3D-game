using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject tree;
    public float xPos;
    public float zPos;
    public float yPos;
    public int treesCount;
    public float height;
    private int treesSpawned;
    [SerializeField]
    float rayLength;
    [SerializeField]
    int iceLayer;
    [SerializeField]
    int terrainLayer;
    [SerializeField]
    private float minTreeSpawnPosition;
    [SerializeField]
    private float maxTreeSpawnPosition;
    [SerializeField]
    private int maxTries;
    private int attempts;

    public enum SpawnLayer
    {
        Trees,
    }

    private void Start()
    {
        FindObjectOfType<TerrainGenerator>().onDone += Spawn;
        maxTries = treesCount; // Add different objects in here
    }

    bool IsDone()
    {
        return treesCount <= treesSpawned; // Add && objectCount <= objectsSpawned
    }

    void Spawn()
    {
        while (!IsDone())
        {
            xPos = Random.Range(1f, 256f);
            zPos = Random.Range(1f, 256f);

            Vector3 origin = new Vector3(xPos, height, zPos);

            int rayMask = (1 << iceLayer | 1 << terrainLayer);

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, rayMask))
            {
                bool[] spawnableObjects = new bool[3];
                spawnableObjects[(int)SpawnLayer.Trees] = CanSpawnTree(hit.point.y);
                List<SpawnLayer> spawnableLayers = new List<SpawnLayer>();

                for (int i = 0; i < spawnableObjects.Length; i++)
                    if (spawnableObjects[i])
                        spawnableLayers.Add((SpawnLayer)i);

                int index = Random.Range(0, spawnableLayers.Count);

                if (spawnableLayers.Count == 0)
                {
                    continue;
                }

                switch ((SpawnLayer)index)
                {
                    case SpawnLayer.Trees:
                        SpawnTree(hit.point);
                        break;
                }

                attempts++;

                if (attempts > maxTries)
                {
                    Debug.LogError("fail to create map");
                    break;
                }
            }
        }
    }

    // Create another function like this as a check (make sure they're SerializeField)
    bool CanSpawnTree(float position)
    {
        Debug.Log($"{position}, {position > minTreeSpawnPosition}, {position < maxTreeSpawnPosition}");
        return position > minTreeSpawnPosition && position < maxTreeSpawnPosition;
    }

    // Create another for the object you want to spawn (this is the tree example)
    void SpawnTree(Vector3 position)
    {
        SpawnObject(tree, position);
        treesSpawned += 1;
    }

    /// <summary>
    /// Spawn any object with random rotation
    /// </summary>
    /// <param name="prefab">The prefab you want to spawn</param>
    /// <param name="position">The position it spawns at</param>
    private void SpawnObject(GameObject prefab, Vector3 position)
    {
        Vector3 randomEuler = new Vector3();

        for (int i = 0; i < 3; i++)
        {
            if (i == 1)
                randomEuler[i] = Random.Range(-360, 360);
            else
                randomEuler[i] = Random.Range(-10, 10);
        }

        Quaternion rotation = Quaternion.Euler(randomEuler);
        Instantiate(prefab, position, rotation);
    }
}


