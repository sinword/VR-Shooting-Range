using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour
{
    string playerName;
    [SerializeField]
    private GameObject _gameMenu;
    [SerializeField]
    private GameObject _ControllerRayInteractableLeft;
    [SerializeField]
    private GameObject _ControllerRayInteractableRight;
    [SerializeField]
    private GameObject _gun;

    public void Start() {
        playerName = PlayerPrefs.GetString("PlayerName");
        Debug.LogWarning("Player name: " + playerName);
        // Change text "PlayerName" to the actual player name
        GameObject.Find("PlayerName").GetComponent<TMPro.TextMeshProUGUI>().text = playerName;
    }
    
    public void PlayButton() {
        // Close menu and start game
        Debug.LogWarning("Play button is clicked.");
        _gameMenu.SetActive(false);
        _ControllerRayInteractableLeft.SetActive(false);
        _ControllerRayInteractableRight.SetActive(false);
        _gun.SetActive(true);
    }

    public void BackButton() {
        SceneManager.LoadScene("StartScene");
    }
}
