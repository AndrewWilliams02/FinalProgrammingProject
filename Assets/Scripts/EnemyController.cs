using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<EnemyTemplate> enemyType;
    [SerializeField] GameObject enemyPrefab;
    List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(4, 0, 0), Quaternion.identity);
        enemies.Add(newEnemy);
        newEnemy.SendMessage("RandomizeEnemy", enemyType[Random.Range(0, enemyType.Count)]);
    }
}
