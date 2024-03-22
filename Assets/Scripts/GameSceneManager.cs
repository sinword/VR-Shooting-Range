using System;
using System.Collections;
using System.Collections.Generic;
using Assets.OVR.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gameMenuUI;
    [SerializeField]
    private GameObject _gameInfoUI;
    [SerializeField]
    private GameObject _ControllerRayInteractableLeft;
    [SerializeField]
    private GameObject _ControllerRayInteractableRight;
    [SerializeField]
    private GameObject _gun;
    [SerializeField]
    private RecordManager _recordManager;
    [SerializeField]
    private AudioSource _gameStartAudio;
    [SerializeField]
    private AudioSource _gameOverAudio;

    string playerName;
    // record the gun original position and rotation
    private Vector3 _gunOriginalPosition;
    private Quaternion _gunOriginalRotation;
    
    public void Start() {
        _gameInfoUI.GetComponentInChildren<Canvas>().enabled = false;
        Transform _gunModelTransform = _gun.transform.Find("M1911 Handgun_Silver (Shooting)");
        // Set the gun to the orignal position
        _gunOriginalPosition = _gunModelTransform.position;
        _gunOriginalRotation = _gunModelTransform.rotation;
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(false);
        playerName = PlayerPrefs.GetString("PlayerName");
        Debug.LogWarning("Player name: " + playerName);
        ShowRecord();
    }
    
    public void PlayButton() {  
        Debug.LogWarning("Game Start");
        _gameStartAudio.Play();
        _gameInfoUI.GetComponent<GameInfoManager>().Reset();
        _gameInfoUI.GetComponent<GameInfoManager>().OnGameOver += GameOver;
         // Close gameMenuUI and open gameInfoUI
        _gameMenuUI.SetActive(false);
        _gameInfoUI.GetComponentInChildren<Canvas>().enabled = true;
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(true);
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").position = _gunOriginalPosition;
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").rotation= _gunOriginalRotation;
        SwtichRayInteractable(false);
    }

    public void BackButton() {
        SceneManager.LoadScene("StartScene");
    }

    private void GameOver() {
        Debug.LogWarning("Game over.");
        _gameOverAudio.Play();
        _gameInfoUI.GetComponent<GameInfoManager>().OnGameOver -= GameOver;
        // Close gameInfoUI and open gameMenuUI
        _gameMenuUI.SetActive(true);
        _gameInfoUI.GetComponentInChildren<Canvas>().enabled = false;
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(false);
        SwtichRayInteractable(true);
        UpdateRecord();
        ShowRecord();
    }

    private void SwtichRayInteractable(bool status) {
        _ControllerRayInteractableLeft.SetActive(status);
        _ControllerRayInteractableRight.SetActive(status);
    }

    private void ShowRecord() {
        List<RecordInfo> recordInfos = _recordManager.GetRecords();
        string recordText = "";
        for (int i = 0; i < 10; i++) {
            if (i < recordInfos.Count) {
                recordText += (i + 1) + ".\t" + recordInfos[i].playerName + "\t" + recordInfos[i].score + "\n";
            }
            else {
                recordText += (i + 1) + ".\t" + "Empty\n";
            }
        }
        
        Debug.LogWarning("Record text: " + recordText);
        GameObject.Find("RecordList").GetComponent<TMPro.TextMeshProUGUI>().text = recordText;
    }

    private void UpdateRecord() {
        RecordInfo recordInfo = new RecordInfo();
        recordInfo.playerName = playerName;
        recordInfo.score = _gameInfoUI.GetComponent<GameInfoManager>().GetScore();
        _recordManager.SaveRedcord(recordInfo);
    }
}
