using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public float zSpawn = 0;
    public float tileLength = 25f;
    public int numberOfTiles = 3;
    public Transform playerTransform;

    private List<GameObject> activeTiles = new List<GameObject>();
    private List<GameObject> inactiveTiles = new List<GameObject>();
    private const int totalTiles = 9;

    void Awake()
    {
        if (tilePrefabs.Length > totalTiles)
        {
            Debug.LogWarning($"Chỉ sử dụng {totalTiles} prefab đầu tiên từ mảng tilePrefabs!");
        }

        for (int i = 0; i < totalTiles; i++)
        {
            int prefabIndex = i % tilePrefabs.Length;
            GameObject tile = Instantiate(tilePrefabs[prefabIndex]);
            tile.SetActive(false);
            inactiveTiles.Add(tile);
        }
    }

    void Start()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            SpawnRandomTile();
        }
    }

    void Update()
    {
        if (playerTransform.position.z - 25 > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnRandomTile();
            RecycleTile();
        }
    }

    public void SpawnRandomTile()
    {
        if (inactiveTiles.Count == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, inactiveTiles.Count);
        GameObject tile = inactiveTiles[randomIndex];
        inactiveTiles.RemoveAt(randomIndex);
        tile.SetActive(true);

        // Reset coin
        ResetCoinsInTile(tile);

        tile.transform.position = transform.forward * zSpawn;
        tile.transform.rotation = transform.rotation;

        activeTiles.Add(tile);
        zSpawn += tileLength;
    }

    private void ResetCoinsInTile(GameObject tile)
    {
        Coin[] coins = tile.GetComponentsInChildren<Coin>(true);
        foreach (Coin coin in coins)
        {
            // 90% chance to spawn a coin
            bool shouldSpawn = Random.value < 0.9f;
            coin.gameObject.SetActive(shouldSpawn);
        }
        FlyItem[] flyitems = tile.GetComponentsInChildren<FlyItem>(true);
        foreach (FlyItem flyitem in flyitems)
        {
            // 30% chance to spawn a fly item
            bool shouldSpawn = Random.value < 0.3f;
            flyitem.gameObject.SetActive(shouldSpawn);
        }
        MagnetItem[] magnetItems = tile.GetComponentsInChildren<MagnetItem>(true);
        foreach (MagnetItem maganetitem in magnetItems)
        {
             // 30% chance to spawn a magnet item
            bool shouldSpawn = Random.value < 0.3f;
            maganetitem.gameObject.SetActive(shouldSpawn);
        }
        InvincibilityItem[] invincibilityItems = tile.GetComponentsInChildren<InvincibilityItem>(true);
        foreach (InvincibilityItem invincibilityItem in invincibilityItems)
        {
             // 30% chance to spawn an invincibility item
            bool shouldSpawn = Random.value < 0.3f;
            invincibilityItem.gameObject.SetActive(shouldSpawn);
        }
    }

    private void RecycleTile()
    {
        if (activeTiles.Count == 0)
        {
            return;
        }

        GameObject tile = activeTiles[0];
        activeTiles.RemoveAt(0);

        tile.SetActive(false);
        inactiveTiles.Add(tile);
    }
}