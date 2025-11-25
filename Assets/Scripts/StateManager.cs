using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameObject player, battle, rest, gameOver;

    private void Start()
    {
        StartBattle();
    }

    public void StartResting()
    {
        SetState(false, true, false);
    }

    public void StartBattle()
    {
        SetState(true, false, false);
    }

    public void GameOver()
    {
        SetState(false, false, true);
    }

    void SetState(bool state1, bool state2, bool state3)
    {
        battle.SetActive(state1);
        rest.SetActive(state2);
        gameOver.SetActive(state3);
    }
}
