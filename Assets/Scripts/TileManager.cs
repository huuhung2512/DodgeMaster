using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefab;
    public float zSpawn = 0;
    public float tileLenght = 10f;
    public int numberOfTile = 5;
    public Transform playerTransform;
    private List<GameObject> activeTile = new List<GameObject>();
    void Start()
    {
        for (int i = 0; i < numberOfTile; i++)
        {
            if (i == 0)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(0, tilePrefab.Length));

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerTransform.position.z -15 > zSpawn - (numberOfTile * tileLenght))
        {
            SpawnTile(Random.Range(0, tilePrefab.Length));
            DeleteTile();
        }
    }
    public void SpawnTile(int indexTile)
    {
        GameObject go = Instantiate(tilePrefab[indexTile], transform.forward * zSpawn, transform.rotation);
        activeTile.Add(go);
        zSpawn += tileLenght;
    }
    private void DeleteTile(){
        Destroy(activeTile[0]);
        activeTile.RemoveAt(0);
    }
}
