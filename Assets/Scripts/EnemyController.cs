using UnityEngine;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<EnemyTemplate> enemyType;
    [SerializeField] GameObject enemyPrefab;
    GameObject player;
    List<GameObject> enemies = new List<GameObject>(); //Handles all enemies currently alive

    void Start()
    {
        player = GameObject.Find("Player");
        SpawnEnemy();
    }

    //Function that spawns new enemies
    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemyPrefab, new Vector3(4, 0, 0), Quaternion.identity); //Spawns new enemy
        enemies.Add(newEnemy); //Adds the enemy to a list of all alive enemies
        player.SendMessage("AddEnemy", newEnemy); //Adds the enemy to the players targets list

        //Randomizes enemies type and sets its stats
        EnemyTemplate newEnemyType = enemyType[Random.Range(0, enemyType.Count)];
        newEnemy.SendMessage("RandomizeEnemy", newEnemyType);
        Debug.Log($"Spawned {newEnemyType.name}");
    }

    //Function that removes enemy from the list of alive enemies
    void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("Removed Enemy");
    }

    //Function that checks if there are any enemies left and spawns new ones if not
    void CheckEnemies()
    {
        if (enemies.Count == 0)
        {
            SpawnEnemy();
        }
    }
}
