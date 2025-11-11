using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<EnemyTemplate> enemyType;
    [SerializeField] GameObject enemyPrefab;
    GameObject player;
    List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        //Finds player target
        player = GameObject.Find("Player");

        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(4, 0, 0), Quaternion.identity);
        enemies.Add(newEnemy);
        player.SendMessage("AddEnemy", newEnemy);
        EnemyTemplate newEnemyType = enemyType[Random.Range(0, enemyType.Count)];
        newEnemy.SendMessage("RandomizeEnemy", newEnemyType);
        Debug.Log($"Spawned {newEnemyType.name}");
    }

    void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("Removed Enemy");
    }

    void CheckEnemies()
    {
        if (enemies.Count == 0)
        {
            SpawnEnemy();
        }
    }
}
