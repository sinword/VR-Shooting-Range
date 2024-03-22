using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{
    private TouchScreenKeyboard _keyboard;
    private string _inputText;

    public void StartButton()
    {
        string playerName = "Default";
        // if (_keyboard != null && _keyboard.text != "")
        // {
        //     playerName = _keyboard.text;
        // }
        _inputText = _keyboard.text;
        if (!string.IsNullOrEmpty(_inputText)) {
            playerName = _inputText;
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
