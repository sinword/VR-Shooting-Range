using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Oculus.Interaction;
public class GunManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _gun;
    [SerializeField]
    private GameSceneManager _gameSceneManager;
    public bool canShoot = true;
    string pattern;
    private int _hitScore;
    public SimpleShoot simpleShoot;
    public OVRInput.Button shootButton;
    public OVRInput.Controller shootingController;
    private Grabbable _grabbable;
    private AudioSource _fireSound;

    // Bullet
    [SerializeField]
    private Transform _bulletSpawnPoint;
    [SerializeField]
    private GameObject _hitEffect;
    [SerializeField]
    private float _shootDelay = 0.1f;
    private float _lastShootTime;
    public delegate void FireEvent();
    public event FireEvent OnShoot;

    private List<GameObject> _hitInstances;

    void Start()
    {
        _grabbable = _gun.GetComponent<Grabbable>();
        _fireSound = _gun.GetComponent<AudioSource>();
        _hitInstances = new List<GameObject>();
        pattern = "TargetCollider(\\d+)";
        if (_bulletSpawnPoint != null) {
            Vector3 forwardDirection = _bulletSpawnPoint.forward;
        }
        if (_gameSceneManager != null) {
            _gameSceneManager.OnGameStart += DestroyHitInstances;
        }
    }

    void Update()
    {
        if (canShoot && OVRInput.GetDown(shootButton, shootingController) && _grabbable != null && _grabbable.IsGrabbed()) {
            if (_lastShootTime + _shootDelay < Time.time) {
                simpleShoot.StartShoot();
                _fireSound.Play();
                _lastShootTime = Time.time;
                _hitScore = FireCheck();
                OnShoot?.Invoke();
            }       
        }
    }

    private int FireCheck() {
        Vector3 direction = _bulletSpawnPoint.forward;
        RaycastHit hitInfo;
        if (Physics.Raycast(_bulletSpawnPoint.position, direction, out hitInfo, 100f)) {
            // Debug.LogWarning("Hit: " + hitInfo.collider.gameObject.name);
            GameObject hitEffectInstance = Instantiate(_hitEffect, hitInfo.point, _hitEffect.transform.rotation);
            _hitInstances.Add(hitEffectInstance);
            Match match = Regex.Match(hitInfo.collider.gameObject.name, pattern);
            if (match.Success) {
                int targetNumber = int.Parse(match.Groups[1].Value);
                Debug.LogWarning("Hit target: " + targetNumber);
                return targetNumber;
            }
        }
        return 0;
    }

    public int GetScore() {
        return _hitScore;
    }

    public void DestroyHitInstances() {
        foreach (GameObject hitInstance in _hitInstances) {
            Destroy(hitInstance);
        }
        _hitInstances.Clear();
    }
}
