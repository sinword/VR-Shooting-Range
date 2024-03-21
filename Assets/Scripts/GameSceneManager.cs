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

    string playerName;
    // record the gun original position and rotation
    private Vector3 _gunOriginalPosition;
    private Quaternion _gunOriginalRotation;
    
    public void Start() {
        _gameInfoUI.GetComponentInChildren<Canvas>().enabled = false;
        Transform childTransform = _gun.transform.Find("M1911 Handgun_Silver (Shooting)");
        _gunOriginalPosition = childTransform.position;
        _gunOriginalRotation = childTransform.rotation;
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(false);
        playerName = PlayerPrefs.GetString("PlayerName");
        Debug.LogWarning("Player name: " + playerName);
        ShowRecord();
        // GameObject.Find("RankPlayerName").GetComponent<TMPro.TextMeshProUGUI>().text = playerName;
    }
    
    public void PlayButton() {  
        Debug.LogWarning("Game Start");
        _gameInfoUI.GetComponent<GameInfoManager>().Reset();
        _gameInfoUI.GetComponent<GameInfoManager>().OnGameOver += GameOver;
         // Close gameMenuUI and open gameInfoUI
        _gameMenuUI.GetComponentInChildren<Canvas>().enabled = false;
        _gameInfoUI.GetComponentInChildren<Canvas>().enabled = true;
        SwtichRayInteractable(false);
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(true);
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").position = _gunOriginalPosition;
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").rotation= _gunOriginalRotation;
        // _gun.SetActive(true);
    }

    public void BackButton() {
        SceneManager.LoadScene("StartScene");
    }

    private void GameOver() {
        // Close gameInfoUI and open gameMenuUI
        Debug.LogWarning("Game over.");
        _gameInfoUI.GetComponent<GameInfoManager>().OnGameOver -= GameOver;
        _gameMenuUI.GetComponentInChildren<Canvas>().enabled = true;
        _gameInfoUI.GetComponentInChildren<Canvas>().enabled = false;
        SwtichRayInteractable(true);
        // Set gun to original position
        _gun.transform.Find("M1911 Handgun_Silver (Shooting)").gameObject.SetActive(false);
        UpdateRecord();
        ShowRecord();
        // GameObject.Find("RankPlayerName").GetComponent<TMPro.TextMeshProUGUI>().text = playerName + ", Score: " + _gameInfoUI.GetComponent<GameInfoManager>().GetScore();
    }

    private void SwtichRayInteractable(bool status) {
        _ControllerRayInteractableLeft.SetActive(status);
        _ControllerRayInteractableRight.SetActive(status);
    }

    private void ShowRecord() {
        List<RecordInfo> recordInfos = _recordManager.GetRecords();
        string recordText = "";
        if (recordInfos.Count <= 10) {
            for (int i = 0; i < recordInfos.Count; i++) {
                recordText += (i + 1) + ".\t" + recordInfos[i].playerName + "\t" + recordInfos[i].score + "\n";
            }
            for (int i = recordInfos.Count; i < 10; i++) {
                recordText += (i + 1) + ".\t" + "Empty\n";
            }
        }
        else {
            for (int i = 0; i < 10; i++) {
                recordText += (i + 1) + ".\t" + recordInfos[i].playerName + "\t" + recordInfos[i].score + "\n";
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
