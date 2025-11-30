using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyController : MonoBehaviour
{
    float difficultyModifier = 1f;

    [SerializeField] DataList dataList;

    [SerializeField] List<EnemyTemplate> enemyType;
    [SerializeField] GameObject enemyPrefab;
    GameObject player, turnManager, stateManager;
    List<GameObject> enemies = new List<GameObject>(); //Handles all enemies currently alive

    [SerializeField] GameObject[] spawners;

    private void Awake()
    {
        enemyType = dataList.allEnemies;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        turnManager = GameObject.Find("TurnManager");
        stateManager = GameObject.Find("State Manager");
        GenerateEnemies();
    }

    void GenerateEnemies()
    {
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
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        UpdateLists();
        //Debug.Log("Removed Enemy");

        if (enemies.Count <= 0)
        {
            StateManager stateManagerScript = stateManager.GetComponent<StateManager>();
            stateManagerScript.PostFight();
        }
    }

    //Updates the alive enemy lists for player and turn manager
    void UpdateLists()
    {
        Player playerScript = player.GetComponent<Player>();
        TurnManager turnManagerScript = turnManager.GetComponent<TurnManager>();
        playerScript.UpdateEnemies(enemies);
        turnManagerScript.UpdateEnemies(enemies);
    }

    void CreateEnemy(int index)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, spawners[index].transform.position, Quaternion.identity);
        enemies.Add(newEnemy);
        EnemyTemplate newEnemyType = enemyType[Random.Range(0, enemyType.Count)];
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.RandomizeEnemy(newEnemyType, difficultyModifier);
        //Debug.Log($"Spawned {newEnemyType.name}");
        UpdateLists();
    }

    public void IncreaseDifficulty(float modifier)
    {
        difficultyModifier *= modifier;
    }
}
