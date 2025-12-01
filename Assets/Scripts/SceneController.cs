using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    //Function that loads a new scene
    public void StartScene(string gameSceneName)
    {
        SceneManager.LoadScene(gameSceneName);
    }

    //Function that closes the game or stops playtest if in editor
    public void ExitGame()
    {
        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
