using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurnManager : MonoBehaviour
{
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

        EnablePlayerTurn();
    }

    //Function called that ends the players turn
    public void EndPlayersTurn()
    {
        if (enemies.Count > 0)
        {
            StartCoroutine(EnemyTurn()); //Starts enemy turns
        }
        else
        {
            EnablePlayerTurn();
        }
    }

    //Updates the turn managers enemy list
    public void UpdateEnemies(List<GameObject> aliveEnemies)
    {
        enemies = aliveEnemies;
    }

    public void EnablePlayerTurn()
    {
        //Enables the players turn
        ui.SendMessage("EnablePlayerTurnUI");
    }
}
