using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class UI : MonoBehaviour
{
    public void EnableUI(GameObject UI) { UI.SetActive(true); }
    public void DisableUI(GameObject UI) { UI.SetActive(false); }
}
