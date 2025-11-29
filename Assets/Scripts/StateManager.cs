using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameObject player, enemyController, battle, rest, gameOver, reward, turnManager;
    int battleStage = 0;

    private void Start()
    {
        StartBattle();
    }

    public void StartResting()
    {
        SetState(false, true, false, false);
    }

    public void StartBattle()
    {
        SetState(true, false, false, false);
        if (battleStage > 0)
        {
            enemyController.SendMessage("GenerateEnemies");
            turnManager.SendMessage("EnablePlayerTurn");
        }
        battleStage++;
    }

    public void GameOver()
    {
        SetState(false, false, true, false);
    }

    public void PostFight()
    {
        SetState(false, false, false, true);
    }

    public void RewardSelected()
    {
        if (battleStage % 3 == 0)
        {
            StartResting();
        }
        else
        {
            StartBattle();
        }
    }

    void SetState(bool state1, bool state2, bool state3, bool state4)
    {
        battle.SetActive(state1);
        rest.SetActive(state2);
        gameOver.SetActive(state3);
        reward.SetActive(state4);
    }
}
