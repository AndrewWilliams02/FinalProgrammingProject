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
        player.SendMessage("UpdateStatText");
    }

    public void StartBattle()
    {
        SetState(true, false, false, false);
        if (battleStage > 0)
        {
            enemyController.SendMessage("IncreaseDifficulty", 1.1f);
            enemyController.SendMessage("GenerateEnemies");
            turnManager.SendMessage("EnablePlayerTurn");
        }
        battleStage++;
    }

    public void GameOver()
    {
        SetState(false, false, true, false);
        battleStage = 0;
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

    public void RestartGame()
    {
        StartBattle();
        player.SendMessage("ResetPlayer");
        enemyController.SendMessage("ResetDifficulty");
        enemyController.SendMessage("GenerateEnemies");
        turnManager.SendMessage("EnablePlayerTurn");
    }

    void SetState(bool state1, bool state2, bool state3, bool state4)
    {
        battle.SetActive(state1);
        rest.SetActive(state2);
        gameOver.SetActive(state3);
        reward.SetActive(state4);
    }
}
