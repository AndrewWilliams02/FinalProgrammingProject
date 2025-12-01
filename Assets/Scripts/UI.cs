using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject playerTurnUI;

    //Function that enables a given UI
    public void EnableUI(GameObject UI) { UI.SetActive(true); }

    //Function that disables a given UI
    public void DisableUI(GameObject UI) { UI.SetActive(false); }

    //Function that enables the players battle UI
    public void EnablePlayerTurnUI()
    {
        playerTurnUI.SetActive(true);
    }
}
