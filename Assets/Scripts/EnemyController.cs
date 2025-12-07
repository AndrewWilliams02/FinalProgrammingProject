using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyController : MonoBehaviour
{
    //Variable that modifies enemy stats
    float difficultyModifier = 1f;

    [SerializeField] DataList dataList;

    [SerializeField] List<EnemyTemplate> enemyType;
    [SerializeField] GameObject enemyPrefab;
    GameObject player, turnManager, stateManager;
    List<GameObject> enemies = new List<GameObject>(); //Handles all enemies currently alive

    [SerializeField] GameObject[] spawners;

    [SerializeField] RuntimeAnimatorController[] animators;
    [SerializeField] Sprite[] sprites;
    

    private void Awake()
    {
        //Set the list of possible enemy types from the data list of all enemies
        enemyType = dataList.allEnemies;
    }

    void Start()
    {
        player = GameObject.Find("Player");
        turnManager = GameObject.Find("TurnManager");
        stateManager = GameObject.Find("State Manager");
        GenerateEnemies(); //Spawns first set of enemies
    }

    //Script that generates 1-3 enemies to spawn
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

        //Checks if all enemies are dead and starts the post-fight rewards state if so
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

    //Function that creates new enemies with a random type
    void CreateEnemy(int index)
    {
        int randomInt = Random.Range(0, enemyType.Count);

        GameObject newEnemy = Instantiate(enemyPrefab, spawners[index].transform.position, Quaternion.identity);
        enemies.Add(newEnemy);
        EnemyTemplate newEnemyType = enemyType[randomInt];
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();
        enemyScript.RandomizeEnemy(newEnemyType, difficultyModifier, sprites[randomInt], animators[randomInt]);
        //Debug.Log($"Spawned {newEnemyType.name}");
        UpdateLists();
    }

    //Function that increases the modifier for enemy stats
    public void IncreaseDifficulty(float modifier)
    {
        difficultyModifier *= modifier;
    }

    //Function to reset the modifier for enemy stats
    public void ResetDifficulty()
    {
        difficultyModifier = 1f;
    }
}
