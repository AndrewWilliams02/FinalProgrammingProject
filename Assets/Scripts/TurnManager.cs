using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurnManager : MonoBehaviour
{
    bool isPlayersTurn = true;

    List<GameObject> enemies;
    [SerializeField] GameObject ui;

    //Coroutine that cycles through alive enemies turns with pauses inbetween
    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.75f); //Waits 0.75 seconds before enemy turns

        //Has enemies deal damage to player in turns with 0.75 second pauses between
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SendMessage("AttackPlayer");
            yield return new WaitForSeconds(0.75f);
        }

        //Enables the players turn
        isPlayersTurn = true;
        ui.SendMessage("EnablePlayerTurnUI");
    }

    //Function called that ends the players turn
    public void EndPlayersTurn()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            StartCoroutine(EnemyTurn()); //Starts enemy turns
        }
    }

    //Updates the turn managers enemy list
    void UpdateEnemies(List<GameObject> aliveEnemies)
    {
        enemies = aliveEnemies;
    }
}
