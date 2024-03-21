using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfoManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerName;
    [SerializeField]
    private GameObject _score;
    [SerializeField]
    private GameObject _ammo;
    [SerializeField]
    private GunManager _gunManager;
    string playerName;
    
    private int _totalScore = 0;
    private int _leftAmmo = 10;

    public delegate void GameOver();
    public event GameOver OnGameOver;

    void Start()
    {
        playerName = PlayerPrefs.GetString("PlayerName");
        _playerName.GetComponent<TMPro.TextMeshProUGUI>().text = "Player: " + playerName;
        _score.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + _totalScore;
        _ammo.GetComponent<TMPro.TextMeshProUGUI>().text = "Ammo: " + _leftAmmo;
        _gunManager = FindObjectOfType<GunManager>();
        _gunManager.OnShoot += UpdateGameInfo;
    }


    private void UpdateGameInfo() {
        if (_gunManager != null) {
            _totalScore += _gunManager.GetScore();
            --_leftAmmo;
            _score.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + _totalScore;
            _ammo.GetComponent<TMPro.TextMeshProUGUI>().text = "Ammo: " + _leftAmmo;
            if (_leftAmmo <= 0) {
                StartCoroutine(GameOverWithDelay(0.5f));
            }
        }
    }

    public int GetScore() {
        return _totalScore;
    }

    public void Reset() {
        _totalScore = 0;
        _leftAmmo = 10;
        _gunManager.canShoot = true;
        _score.GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + _totalScore;
        _ammo.GetComponent<TMPro.TextMeshProUGUI>().text = "Ammo: " + _leftAmmo;
    }

    IEnumerator GameOverWithDelay(float delay) {
        _gunManager.canShoot = false;
        yield return new WaitForSeconds(delay);
        OnGameOver?.Invoke();
    }
}
