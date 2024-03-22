using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Oculus.Interaction;
public class GunManager : MonoBehaviour
{
    public bool canShoot = true;
    string pattern;
    private int _hitScore;
    public SimpleShoot simpleShoot;
    public OVRInput.Button shootButton;
    public OVRInput.Controller shootingController;
    [SerializeField]
    private GameObject _gun;
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
    void Start()
    {
        _grabbable = _gun.GetComponent<Grabbable>();
        _fireSound = _gun.GetComponent<AudioSource>();
        pattern = @"\d+";
        if (_bulletSpawnPoint != null)
        {
            Vector3 forwardDirection = _bulletSpawnPoint.forward;
        }
    }

    // Update is called once per frame
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
        // Debug.LogWarning("Bullet shoot direction: " + direction);
        if (Physics.Raycast(_bulletSpawnPoint.position, direction, out hitInfo, 100f)) {
            // Debug.LogWarning("Hit: " + hitInfo.collider.gameObject.name);
            GameObject hitEffectInstance = Instantiate(_hitEffect, hitInfo.point, Quaternion.identity);
            Destroy(hitEffectInstance, 5f);
            Match match = Regex.Match(hitInfo.collider.gameObject.name, pattern);
            if (match.Success) {
                int targetNumber = int.Parse(match.Value);
                Debug.LogWarning("Hit target: " + targetNumber);
                return targetNumber;
            }
        }
        return 0;
    }

    public int GetScore() {
        return _hitScore;
    }
}
