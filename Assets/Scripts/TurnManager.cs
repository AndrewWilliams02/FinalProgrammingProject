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

    private IEnumerator EnemyTurn()
    {
        yield return new WaitForSeconds(0.75f);

        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SendMessage("AttackPlayer");
            yield return new WaitForSeconds(0.75f);
        }

        isPlayersTurn = true;
        ui.SendMessage("EnablePlayerTurnUI");
    }

    public void EndPlayersTurn()
    {
        if (isPlayersTurn)
        {
            isPlayersTurn = false;
            StartCoroutine(EnemyTurn());
        }
    }
    void UpdateEnemies(List<GameObject> aliveEnemies)
    {
        enemies = aliveEnemies;
    }
}
