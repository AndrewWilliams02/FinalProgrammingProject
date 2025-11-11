using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    [SerializeField] GameObject playerTurnUI;

    public void EnableUI(GameObject UI) { UI.SetActive(true); }
    public void DisableUI(GameObject UI) { UI.SetActive(false); }

    public void EnablePlayerTurnUI()
    {
        playerTurnUI.SetActive(true);
    }
}
