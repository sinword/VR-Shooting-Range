using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using Oculus.Interaction;
public class GunManager : MonoBehaviour
{
    string pattern = @"\d+";
    private int _score = 0;
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
    // [SerializeField]
    // private TrailRenderer _bulletTrail;
    [SerializeField]
    private float _shootDelay = 0.1f;
    private float _lastShootTime;

    void Start()
    {
        _grabbable = GetComponent<Grabbable>();
        _fireSound = GetComponent<AudioSource>();
        if (_bulletSpawnPoint != null)
        {
            Vector3 forwardDirection = _bulletSpawnPoint.forward;
            Debug.LogWarning("Bullet spawn point forward direction: " + forwardDirection);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(shootButton, shootingController) && _grabbable != null && _grabbable.IsGrabbed()) {
            if (_lastShootTime + _shootDelay < Time.time) {
                simpleShoot.StartShoot();
                _fireSound.Play();
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
                        _score += targetNumber;
                        Debug.LogWarning("Score: " + _score);
                    }
                }
                else {
                    Debug.LogWarning("No hit");
                }
            }
            
        }
    }

    // private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit) {
    //     float time = 0;
    //     Vector3 startPos = trail.transform.position;

    //     while (time < 1) {
    //         trail.transform.position = Vector3.Lerp(startPos, hit.point, time);
    //         time += Time.deltaTime / trail.time;
    //         yield return null;
    //     }

    //     trail.transform.position = hit.point;

    //     Destroy(trail.gameObject, trail.time);
    // }
}
