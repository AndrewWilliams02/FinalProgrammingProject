using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameObject player, enemyController, battle, rest, gameOver, reward, turnManager;
    int battleStage = 0; //Vraiable tracking the current battle number
    public float difficultyMod = 0.05f; //Variable that increases the difficulty

    private void Start()
    {
        StartBattle(); //Starts initiale battle on game start
    }

    //Function that enables resting game state (UI)
    public void StartResting()
    {
        SetState(false, true, false, false);
        player.SendMessage("UpdateStatText");
    }

    //Function that enables battle game state (UI)
    public void StartBattle()
    {
        SetState(true, false, false, false);

        //Checks if this is NOT the first battle of the game
        if (battleStage > 0)
        {
            //If so increases difficulty, generates enemies, and re-enables the players battle UI
            enemyController.SendMessage("IncreaseDifficulty", 1 + difficultyMod);
            enemyController.SendMessage("GenerateEnemies");
            turnManager.SendMessage("EnablePlayerTurn");
        }
        battleStage++;
    }

    //Function that enables gameover state (UI)
    public void GameOver()
    {
        SetState(false, false, true, false);
        battleStage = 0;
    }

    //Function that enables post-fight reward state (UI)
    public void PostFight()
    {
        SetState(false, false, false, true);
    }

    //Function that sets the state depending on battle count
    public void RewardSelected()
    {
        if (battleStage % 3 == 0)
        {
            //Every three battle start the rest scene
            StartResting();
            player.SendMessage("UpdateStatsInfo");
        }
        else
        {
            //If it hasn't been three battles start the next battle
            StartBattle();
        }
    }

    //Function that restarts variables and game state
    public void RestartGame()
    {
        StartBattle();
        player.SendMessage("ResetPlayer");
        enemyController.SendMessage("ResetDifficulty");
        enemyController.SendMessage("GenerateEnemies");
        turnManager.SendMessage("EnablePlayerTurn");
    }

    //Function that sets UI state (active/disactive)
    void SetState(bool state1, bool state2, bool state3, bool state4)
    {
        battle.SetActive(state1);
        rest.SetActive(state2);
        gameOver.SetActive(state3);
        reward.SetActive(state4);
    }
}
