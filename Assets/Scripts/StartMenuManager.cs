using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private TouchScreenKeyboard _keyboard;

    public void StartButton()
    {
        string playerName = "Default";
        if (_keyboard != null && _keyboard.text != "")
        {
            playerName = _keyboard.text;
        }
        PlayerPrefs.SetString("PlayerName", playerName);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void ShowKeyboard() {
        Debug.LogWarning("Show keyboard is called.");
        _keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default);
    }
}
