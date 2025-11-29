using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyController : MonoBehaviour
{
    [SerializeField] List<EnemyTemplate> enemyType;
    [SerializeField] GameObject enemyPrefab;
    GameObject player, turnManager;
    List<GameObject> enemies = new List<GameObject>(); //Handles all enemies currently alive

    [SerializeField] GameObject[] spawners;

    void Start()
    {
        player = GameObject.Find("Player");
        turnManager = GameObject.Find("TurnManager");

        int enemyNum = Random.Range(1, 4);
        SpawnEnemy(enemyNum);
    }

    //Function that spawns new enemies
    void SpawnEnemy(int numOfEnemies)
    {
        switch (numOfEnemies)
        {
            case 1:
                CreateEnemy(0);
                return;
            case 2:
                CreateEnemy(0);
                CreateEnemy(1);
                return;
            case 3:
                CreateEnemy(0);
                CreateEnemy(1);
                CreateEnemy(2);
                return;
        }
    }

    //Function that removes enemy from the list of alive enemies
    void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        UpdateLists();
        Debug.Log("Removed Enemy");
    }

    //Updates the alive enemy lists for player and turn manager
    void UpdateLists()
    {
        player.SendMessage("UpdateEnemies", enemies);
        turnManager.SendMessage("UpdateEnemies", enemies);
    }

    void CreateEnemy(int index)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawners[index].transform.position, Quaternion.identity);
        enemies.Add(newEnemy);
        UpdateLists();
        EnemyTemplate newEnemyType = enemyType[Random.Range(0, enemyType.Count)];
        newEnemy.SendMessage("RandomizeEnemy", newEnemyType);
        Debug.Log($"Spawned {newEnemyType.name}");
    }
}
