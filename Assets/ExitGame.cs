using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGame : MonoBehaviour
{
    private GameManager GameManager;

    private void Start()
    {
        GameManager = GetComponent<GameManager>();
    }

    public void QuitGame()
    {
        GameManager.SavePlayerData();
        Application.Quit(); // Closes the game

        // In the Editor, this won't work, so add:
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
