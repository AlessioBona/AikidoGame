using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int maxEnemies;
    public int count = 0;
    public float repeatTime = 5f;
    public float startTime = 2f;
    public GameObject prefab;

    private void Start()
    {
        InvokeRepeating("Spawn", startTime, repeatTime);
    }

    void Spawn()
    {
        if (count >= maxEnemies) return;
        count++;
        Instantiate(GetPrefab(), transform.position, Quaternion.identity);
    }

    private GameObject GetPrefab()
    {
        return prefab;
    }
}
