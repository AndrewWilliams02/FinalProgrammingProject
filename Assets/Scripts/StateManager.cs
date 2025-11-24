using UnityEngine;

public class StateManager : MonoBehaviour
{
    public GameObject player, battle, rest;

    public void StartResting()
    {
        player.SendMessage("Resting", true);
        SetState(false, true);
    }

    public void StartBattle()
    {
        player.SendMessage("Resting", false);
        SetState(true, false);
    }

    void SetState(bool state1, bool state2)
    {
        battle.SetActive(state1);
        rest.SetActive(state2);
    }
}
